using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace SWSFCSMPIWBC
{
    public partial class smUC : UserControl
    {
        static string connectionString =
        System.Configuration.ConfigurationManager.
        ConnectionStrings["SWSFCSMPIWBC.Properties.Settings.slimmersdbConnectionString"].ConnectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);
        public smUC()
        {
            InitializeComponent();
            servicemonitoringPanel.Show();
            schedulePanel.Hide();
            consultantschedPanel.Hide();
            empButtons.Hide();
            emSchedPanel.Hide();
            string patientname = "", consultant = "";
            GetPatients(patientname);
            GetMachines();
            GetEmployee();
            dateTimePicker1.MinDate = DateTime.Now;
            string patient = "";
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT *,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from patienttbl where Patient_Status = 'Active' order by Patient_No LIMIT 1", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    patient = dataReader.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)");
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            GetSelectedPatient(patient);
            string service = "";
            for (int j = 0; j < dataGridView2.Rows.Count; j++)
            {
                service = dataGridView2.Rows[j].Cells[0].Value.ToString();
                int no = Convert.ToInt32(dataGridView2.Rows[j].Cells[1].Value);
                int reqno = Convert.ToInt32(dataGridView2.Rows[j].Cells[2].Value);
                if (no != reqno)
                {
                    break;
                }
            }
            textBox3.Text = service;
           
            servicemonitoringPanel.Show();
            schedulePanel.Hide();
            consultantschedPanel.Hide();
            empButtons.Hide();
            button4.BackColor = Color.FromArgb(4, 180, 253);
            button1.BackColor = Color.Transparent;
            button8.BackColor = Color.Transparent;
            ConsultantAvail();

            button4.Textcolor = Color.FromArgb(4, 180, 253);
            WaitingPatients();
        }
        public HomePage ParentForm { get; set; }
        public void GetEmployeeAvail(string employee)
        {
            dataGridView9.Rows.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT *,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from employeetbl where CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) LIKE '%" + employee + "%' and Employee_Status = 'Active' order by Employee_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView9.Rows.Add(dataReader.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)"), dataReader.GetString("Employee_Availability"));
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }

        public void GetMachineAvail(string machine)
        {
            dataGridView10.Rows.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from machinetbl where Machine_Name LIKE '%" + machine + "%' order by Machine_Type_No, Machine_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView10.Rows.Add(dataReader.GetInt32("Machine_No"), dataReader.GetString("Machine_Name"), dataReader.GetString("Machine_Status"));
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }

        public int GetSubtractNo()
        {
            int subtractno = 0;

            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * From inventory_subtracttbl order by Inventory_Subtract_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    subtractno = dataReader.GetInt32("Inventory_Subtract_No");
                }
                subtractno = subtractno + 1;
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }

            return subtractno;
        }
        public void GetPatients(string patientname)
        {
            dataGridView1.Rows.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT *,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from patienttbl where CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) LIKE '%" + patientname + "%' and Patient_Status = @status order by Patient_No", connection);
                cmd.Parameters.AddWithValue("@status", "Active");
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView1.Rows.Add(dataReader.GetInt32("Patient_No"), dataReader.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)"));
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }
        public void GetSelectedPatient(string patient)
        {
            dataGridView2.Rows.Clear();
            textBox2.Text = "";
            try
            {
                connection.Open();
                MySqlCommand cmd1 = new MySqlCommand("SELECT *,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit),COUNT(a.Service_No) as VisitCount from patienttbl p, appointmenttbl a,servicetbl s,employee_appointmenttbl ea, employee_patienttbl ep where CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) = @patient and Patient_Status = @status and ea.Appointment_no = a.Appointment_No and p.Patient_No = ep.Patient_No and ep.Employee_Patient_No = ea.Employee_Patient_No and a.Service_No = s.Service_No and a.Appointment_Status = 'Done' group by ep.Patient_No order by p.Patient_No", connection);
                cmd1.Parameters.AddWithValue("@status", "Active");
                cmd1.Parameters.AddWithValue("@patient", patient);
                MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                while (dataReader1.Read())
                {
                    dataGridView2.Rows.Add(dataReader1.GetString("Service_Name"), dataReader1.GetInt32("VisitCount"), dataReader1.GetInt32("No_Of_Visit"));
                }

                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            if (dataGridView2.Rows.Count == 0)
            {
                label63.Text = "**No history of treatments";
                bunifuThinButton21.Enabled = false;
            }
            else
            {
                label63.Text = "";
                bunifuThinButton21.Enabled = true;
            }
        }
        public void GetSchedule(string empname)
        {
            string datenow = DateTime.Now.ToString("yyyy-MM-dd");
            dataGridView4.Rows.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd1 = new MySqlCommand("Select *,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit),CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from appointmenttbl a, employee_appointmenttbl ea, employeetbl e, employee_patienttbl ep,patienttbl p,servicetbl s where Appointment_Date = '" + datenow + "' and CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) = '" + empname + "' and ea.Appointment_No = a.Appointment_No and ea.Employee_Patient_No = ep.Employee_Patient_No and ep.Employee_No = e.Employee_No and ep.Patient_No = p.Patient_No and a.Service_No = s.Service_No and a.Appointment_Status <> 'Cancelled'", connection);
                MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                while (dataReader1.Read())
                {

                    dataGridView4.Rows.Add(dataReader1.GetInt32("Appointment_No"), dataReader1.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)"), dataReader1.GetString("Service_Name"), dataReader1.GetString("Appointment_StartTime"), dataReader1.GetString("Appointment_EndTime"), dataReader1.GetString("Appointment_Status"));

                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            for (int row = 0; row < dataGridView4.Rows.Count; row++)
            {
                if (dataGridView4.Rows[row].Cells[5].Value.ToString() == "Not Started")
                {
                    dataGridView4.Rows[row].Cells[5].Style.BackColor = Color.Bisque;
                    DataGridViewTextBoxCell txtCell = new DataGridViewTextBoxCell();
                    dataGridView4.Rows[row].Cells[8] = txtCell;
                    dataGridView4.Rows[row].Cells[8].ReadOnly = true;
                }
                else if (dataGridView4.Rows[row].Cells[5].Value.ToString() == "On Going")
                {
                    dataGridView4.Rows[row].Cells[5].Style.BackColor = Color.DeepSkyBlue;
                    DataGridViewTextBoxCell txtCell = new DataGridViewTextBoxCell();
                    dataGridView4.Rows[row].Cells[6] = txtCell;
                    dataGridView4.Rows[row].Cells[6].ReadOnly = true;
                }
                else if (dataGridView4.Rows[row].Cells[5].Value.ToString() == "Cancelled")
                {
                    dataGridView4.Rows[row].Cells[5].Style.BackColor = Color.LightCoral;
                }
                else if (dataGridView4.Rows[row].Cells[5].Value.ToString() == "Done")
                {
                    dataGridView4.Rows[row].Cells[5].Style.BackColor = Color.MediumSeaGreen;
                    DataGridViewTextBoxCell txtCell = new DataGridViewTextBoxCell();
                    DataGridViewTextBoxCell txtCell1 = new DataGridViewTextBoxCell();
                    DataGridViewTextBoxCell txtCell2 = new DataGridViewTextBoxCell();
                    dataGridView4.Rows[row].Cells[6] = txtCell;
                    dataGridView4.Rows[row].Cells[6].ReadOnly = true;
                    dataGridView4.Rows[row].Cells[7] = txtCell1;
                    dataGridView4.Rows[row].Cells[7].ReadOnly = true;
                    dataGridView4.Rows[row].Cells[8] = txtCell2;
                    dataGridView4.Rows[row].Cells[8].ReadOnly = true;
                }

            }

        }

        public void GetEmployeeWithSched(string employee)
        {
            flowLayoutPanel1.Controls.Clear();
            string datetoday = DateTime.Now.ToString("yyyy-MM-dd");
            int ctr = 0;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("Select *,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit),CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from appointmenttbl a, employee_appointmenttbl ea, employeetbl e, employee_patienttbl ep,patienttbl p,servicetbl s where CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) LIKE '%" + employee + "%' and a.Appointment_Date = '" + datetoday + "' and ea.Appointment_No = a.Appointment_No and ea.Employee_Patient_No = ep.Employee_Patient_No and ep.Employee_No = e.Employee_No and ep.Patient_No = p.Patient_No and a.Service_No = s.Service_No and a.Appointment_Status <> 'Cancelled' group by e.Employee_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    var btnEmp = new Button();

                    btnEmp.Text = dataReader.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)");
                    btnEmp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    btnEmp.Size = new Size(171, 60);
                    btnEmp.Font = new Font("Arial", 10, FontStyle.Regular);
                    btnEmp.Margin = new Padding(25, 15, 0, 0);
                    btnEmp.FlatStyle = FlatStyle.Flat;
                    btnEmp.FlatAppearance.BorderSize = 0;
                    btnEmp.Cursor = Cursors.Hand;
                    btnEmp.BackColor = Color.LightCyan;

                    btnEmp.Click += delegate
                    {
                        string empname = btnEmp.Text;
                        GetSchedule(empname);
                        dataGridView4.ClearSelection();
                        label53.Text = empname;
                        servicemonitoringPanel.Hide();
                        consultantschedPanel.Hide();
                        empButtons.Hide();
                        schedulePanel.Show();
                    };
                    flowLayoutPanel1.Controls.Add(btnEmp);
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string emp = "";
            GetEmployeeWithSched(emp);

            servicemonitoringPanel.Hide();
            consultantschedPanel.Hide();
            empButtons.Show();
            emSchedPanel.Hide();
            schedulePanel.Hide();
            slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Top;
            slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Height;

            button1.Textcolor = Color.FromArgb(4, 180, 253);
            button4.Textcolor = Color.White;
            button14.Textcolor = Color.White;
            button8.Textcolor = Color.White;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            servicemonitoringPanel.Show();
            schedulePanel.Hide();
            consultantschedPanel.Hide();
            empButtons.Hide();
            emSchedPanel.Hide();
            slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Top;
            slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Height;

            button4.Textcolor = Color.FromArgb(4, 180, 253);
            button1.Textcolor = Color.White;
            button14.Textcolor = Color.White;
            button8.Textcolor = Color.White;

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
        public int GetPatientNo(string patient)
        {
            int patientno = 0;
            try
            {
                connection.Open();
                string query = "SELECT *,CONCAT(Patient_LName,', ', Patient_FName,' ',Patient_MidInit) from patienttbl where CONCAT(Patient_LName,', ', Patient_FName,' ',Patient_MidInit) = '" + patient + "'";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    patientno = dataReader.GetInt32("Patient_No");
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            return patientno;
        }
        public int GetAppointmentNo()
        {
            int appointno = 0;
            try
            {
                connection.Open();
                string query = "SELECT Appointment_No from appointmenttbl order by Appointment_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    appointno = dataReader.GetInt32("Appointment_No");
                }
                connection.Close();
                appointno = appointno + 1;
            }
            catch (MySqlException me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            return appointno;
        }
        public void GetEndTime(string start)
        {
            string service = textBox3.Text;
            string startampm = "", endampm = "", endtime = "";
            int hour = 0, min = 0, starthour = 0, startmin = 0, endmin = 0;
            int endmin1 = 0;
            int endhour = 0;
            try
            {
                connection.Open();
                string query = "SELECT * from servicetbl where Service_Name = '" + service + "'";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    hour = dataReader.GetInt32("Hour_Consumed");
                    min = dataReader.GetInt32("Minute_Consumed");
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            starthour = Convert.ToInt32(start.Substring(0, 2));
            startmin = Convert.ToInt32(start.Substring(3, 2));
            startampm = start.Substring(6, 2);
            if (starthour < 9 && startampm == "pm")
            {
                starthour = starthour + 12;
            }
            endhour = Convert.ToInt32(starthour + hour);
            endmin1 = Convert.ToInt32(startmin + min);
            if (Convert.ToInt32(endhour) > 12)
            {
                endhour = Convert.ToInt32(Convert.ToInt32(endhour) - 12);
                
            }
            if (Convert.ToInt32(endmin1) >= 60)
            {
                endhour = endhour + (Convert.ToInt32(endmin1) / 60);
                endmin1 = Convert.ToInt32(Convert.ToInt32(endmin1) % 60);
            }
            if (Convert.ToInt32(endhour) >= 12 || startampm == "pm")
            {
                endampm = "pm";
            }
            else
            {
                endampm = "am";
            }

            endtime = endhour.ToString("D2") + ":" + endmin1.ToString("D2") + " " + endampm;
            textBox1.Text = endtime;
        }
        public void GetMachines()
        {
            comboBox2.Items.Clear();
            //comboBox6.Items.Clear();
            try
            {
                connection.Open();
                string query = "SELECT Machine_Name from machinetbl where Machine_Status = 'Available' order by Machine_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    comboBox2.Items.Add(dataReader.GetString("Machine_Name"));
                   // comboBox6.Items.Add(dataReader.GetString("Machine_Name"));
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            try
            {
                comboBox2.SelectedIndex = 0;
                comboBox2.Enabled = true;
            }
            catch (Exception)
            {
                comboBox2.Items.Add("No available");
                comboBox2.SelectedIndex = 0;
                comboBox2.Enabled = false;
            }
            try
            {
               // comboBox6.SelectedIndex = 0;
            }
            catch (Exception)
            {
                //comboBox6.Items.Add("No available");
                //comboBox6.SelectedIndex = 0;
            }
        }
        public void GetEmployee()
        {
            comboBox3.Items.Clear();
           // comboBox4.Items.Clear();
            try
            {
                connection.Open();
                string query = "SELECT CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from employeetbl e, employee_positiontbl ep where ep.Position_Name = 'Therapist' and ep.Employee_Position_No = e.Employee_Position_No and e.Employee_Status = 'Active' order by Employee_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    comboBox3.Items.Add(dataReader.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)"));
                    comboBox4.Items.Add(dataReader.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)"));
                }
                connection.Close();

            }
            catch (MySqlException me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            try
            {
                comboBox3.SelectedIndex = 0;
                comboBox4.SelectedIndex = 0;
            }
            catch (Exception)
            {
                comboBox3.Items.Add("No available");
                comboBox3.SelectedIndex = 0;
            }
            try
            {
                comboBox4.SelectedIndex = 0;
            }
            catch (Exception)
            {
                comboBox4.Items.Add("No available");
                comboBox4.SelectedIndex = 0;
            }
        }
        ErrorProvider errorProvider = new ErrorProvider();
        private void button11_Click(object sender, EventArgs e)
        {
            string date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string starttime = "";
            string endtime = textBox1.Text;
            string patient = textBox2.Text;
            int patientno = GetPatientNo(patient);
            string checkdate = "", checkstart = "", checkend = "", startampm = "", endampm = "", checkstartampm = "", checkendampm = "", checkmachine = "", checkemp = "";
            int starthour = 0, startmin = 0, endhour = 0, endmin = 0, checkstarthour = 0, checkstartmin = 0, checkendhour = 0, checkendmin = 0;
            bool check = false, checker = false,checking = false;
            int empstarthour = 0, empstartmin = 0, empendhour = 0, empendmin = 0;
            string empstartampm = "", empendampm = "";
            string machinename = "", servicename = "", consultant = "", therapist = "";
            int machineno = 0, serviceno = 0, consultantno = 0, therapistno = 0;
            int appointno = GetAppointmentNo();
            int emp_patientno = GetEmployeePatientNo();
            try
            {
                errorProvider.SetError(comboBox5, string.Empty);
                starttime = comboBox5.Text;

                starthour = Convert.ToInt32(starttime.Substring(0, 2));
                startmin = Convert.ToInt32(starttime.Substring(3, 2));
                startampm = starttime.Substring(6, 2);
                endhour = Convert.ToInt32(endtime.Substring(0, 2));
                endmin = Convert.ToInt32(endtime.Substring(3, 2));
                endampm = endtime.Substring(6, 2);
            }
            catch (Exception me)
            {
                errorProvider.SetError(comboBox5, "Please select time first");
                checker = true;
            }
            servicename = textBox3.Text.Trim();
            machinename = comboBox2.Text;
            therapist = comboBox3.Text;
            if (string.IsNullOrEmpty(machinename))
            {
                errorProvider.SetError(comboBox2, "Please select machine first");
                checker = true;
            }
            else
            {
                errorProvider.SetError(comboBox2, string.Empty);
            }
            if (string.IsNullOrEmpty(servicename))
            {
                errorProvider.SetError(textBox3, "Please select service first");
                checker = true;
            }
            else
            {
                errorProvider.SetError(textBox3, string.Empty);
            }
            if (string.IsNullOrEmpty(therapist))
            {
                errorProvider.SetError(comboBox3, "Please select your therapist first");
                checker = true;
            }
            else
            {
                errorProvider.SetError(comboBox3, string.Empty);
            }
            if (comboBox5.Text == "No available")
            {
                errorProvider.SetError(textBox1, "No available time");
                checker = true;
            }
            else
            {
                errorProvider.SetError(textBox1, string.Empty);
            }
            try
            {
                connection.Open();
                string query1 = "SELECT Machine_No from machinetbl where Machine_Name = '" + machinename + "'";
                MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                while (dataReader1.Read())
                {
                    machineno = dataReader1.GetInt32("Machine_No");
                }
                connection.Close();

                connection.Open();
                string query2 = "SELECT Service_No from servicetbl where Service_Name = '" + servicename + "'";
                MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                MySqlDataReader dataReader2 = cmd2.ExecuteReader();
                while (dataReader2.Read())
                {
                    serviceno = dataReader2.GetInt32("Service_No");
                }
                connection.Close();

                connection.Open();
                string query4 = "SELECT Employee_No,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from employeetbl where CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)= '" + therapist + "'";
                MySqlCommand cmd4 = new MySqlCommand(query4, connection);
                MySqlDataReader dataReader4 = cmd4.ExecuteReader();
                while (dataReader4.Read())
                {
                    therapistno = dataReader4.GetInt32("Employee_No");
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }


            try
            {
                connection.Open();
                string query = "SELECT *,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from appointmenttbl a,employee_appointmenttbl ea,employee_patienttbl ept, employeetbl e, employee_positiontbl ep,machinetbl m where Appointment_Status <> 'Done' and Appointment_Status <> 'Cancelled' and a.Appointment_No = ea.Appointment_No and a.Machine_No = m.Machine_No and ea.Employee_Patient_No = ept.Employee_Patient_No and ept.Employee_No = e.Employee_No and ep.Position_Name = 'Therapist' and e.Employee_Position_No = ep.Employee_Position_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    checkdate = dataReader.GetDateTime("Appointment_Date").ToString("yyyy-MM-dd");
                    checkstart = dataReader.GetString("Appointment_StartTime");
                    checkend = dataReader.GetString("Appointment_EndTime");
                    checkmachine = dataReader.GetString("Machine_Name");
                    checkemp = dataReader.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)");
                    checkstarthour = Convert.ToInt32(checkstart.Substring(0, 2));
                    checkstartmin = Convert.ToInt32(checkstart.Substring(3, 2));
                    checkstartampm = checkstart.Substring(6, 2);
                    checkendhour = Convert.ToInt32(checkend.Substring(0, 2));
                    checkendmin = Convert.ToInt32(checkend.Substring(3, 2));
                    checkendampm = checkend.Substring(6, 2);

                    if (checkstarthour < 12 && checkstartampm == "pm")
                    {
                        checkstarthour += 12;
                    }
                    if (checkendhour < 12 && checkendampm == "pm")
                    {
                        checkendhour += 12;
                    }
                    if (starthour < 12 && startampm == "pm")
                    {
                        starthour += 12;
                    }
                    if (endhour < 12 && endampm == "pm")
                    {
                        endhour += 12;
                    }
                    if (date == checkdate)
                    {
                        if ((((starthour > checkstarthour && (starthour < checkendhour)) || ((starthour == checkendhour && startmin <= checkendmin) || (starthour == checkstarthour && startmin >= checkstartmin))) || (((endhour > checkstarthour) && (endhour < checkendhour)) || ((endhour == checkstarthour && endmin >= checkstartmin) || (endhour == checkendhour && endmin < checkendmin))) || ((starthour < checkstarthour && starthour < checkendhour) && (endhour > checkstarthour && endhour < checkendhour))))
                        {
                            if (machinename == checkmachine || therapist == checkemp)
                            {
                                check = true;
                                checker = true;
                                MessageBox.Show("There's an appointment");
                                break;
                            }
                        }
                    }
                }
                if (check == true)
                {
                    errorProvider.SetError(textBox1, "There's already an appointment within the time");
                }
                else
                {
                    errorProvider.SetError(textBox1, string.Empty);
                }

                

                }
                catch (Exception me)
                {
                    connection.Close();
                    MessageBox.Show(me.Message);
                }

                connection.Close();
                try
                {
                    connection.Open();
                    string day = dateTimePicker1.Value.ToString("dddd");
                    MySqlCommand cmd1 = new MySqlCommand("SELECT * from employee_schedtbl es, employeetbl e where es.Employee_No = '"+therapistno+"' and es.Employee_No = e.Employee_No and Schedule_Day = '" + day + "'", connection);
                    MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                    while (dataReader1.Read())
                    {
                        empstarthour = Convert.ToInt32(dataReader1.GetString("Schedule_TimeIn").Substring(0, 2));
                        empstartmin = Convert.ToInt32(dataReader1.GetString("Schedule_TimeIn").Substring(3, 2));
                        empstartampm = dataReader1.GetString("Schedule_TimeIn").Substring(6, 2);
                        empendhour = Convert.ToInt32(dataReader1.GetString("Schedule_TimeOut").Substring(0, 2));
                        empendmin = Convert.ToInt32(dataReader1.GetString("Schedule_TimeOut").Substring(3, 2));
                        empendampm = dataReader1.GetString("Schedule_TimeOut").Substring(6, 2);
                        if (empstarthour < 12 && empstartampm == "pm")
                        {
                            empstarthour = empstarthour + 12;
                        }
                        if (empendhour < 12 && empendampm == "pm")
                        {
                            empendhour = empendhour + 12;
                        }
                        if (starthour < 12 && startampm == "pm")
                        {
                            starthour = starthour + 12;
                        }
                        if (endhour < 12 && endampm == "pm")
                        {
                            endhour = endhour + 12;
                        }
                        if ((starthour >= empstarthour) && ((endhour <= empendhour && string.Equals(endampm, empendampm, StringComparison.OrdinalIgnoreCase)) || endampm != empendampm))
                        {
                            checking = true;
                            break;
                        }
                    }
                    connection.Close();
                    if (!checking)
                    {
                        label26.Text = "Therapist is not available at this time";
                    }
                    else
                    {
                        label26.Text = "";
                    }
                if (check == false && checker == false && checking == true)
                {
                    try
                    {
                        connection.Open();
                        string query3 = "INSERT into appointmenttbl values ('" + appointno + "','" + date + "','" + starttime + "','" + endtime + "','Not Started','" + serviceno + "','" + machineno + "')";
                        MySqlCommand cmd3 = new MySqlCommand(query3, connection);
                        cmd3.ExecuteNonQuery();
                        connection.Close();

                        connection.Open();
                        MySqlCommand cmd4 = new MySqlCommand("INSERT into employee_patienttbl values ('" + emp_patientno + "','" + patientno + "','" + therapistno + "')", connection);
                        cmd4.ExecuteNonQuery();
                        connection.Close();

                        connection.Open();
                        string query5 = "Insert into employee_appointmenttbl(Appointment_No,Employee_Patient_No) values('" + appointno + "','" + emp_patientno + "')";
                        MySqlCommand cmd5 = new MySqlCommand(query5, connection);
                        cmd5.ExecuteNonQuery();
                        connection.Close();
                    }
                    catch (MySqlException me)
                    {
                        connection.Close();
                        MessageBox.Show(me.Message);
                    }
                    MessageBox.Show("Successfully added appointment");
                }
            }
            catch (MySqlException me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            string starttime = comboBox5.Text;
            GetEndTime(starttime);
        }
        public void WaitingPatients()
        {
            dataGridView7.Rows.Clear();
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            int waitingno = 0;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT p.Patient_No,RTRIM(CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)) from patienttbl p, appointmenttbl a, employee_patienttbl ep, employee_appointmenttbl ea where a.Appointment_Status = 'Cancelled' and a.Appointment_Date = '" + date + "' and ea.Appointment_No = a.Appointment_No and ea.Employee_Patient_No = ep. Employee_Patient_No and ep.Patient_No = p.Patient_No order by ep.Patient_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    waitingno++;
                    dataGridView7.Rows.Add(waitingno, dataReader.GetString("RTRIM(CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit))"), "Treatment");
                }
                connection.Close();

                connection.Open();
                MySqlCommand cmd1 = new MySqlCommand("SELECT *,RTRIM(CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)) from patient_waitlisttbl pw, patienttbl p,employee_patienttbl ep where pw.Employee_Patient_No = ep.Employee_Patient_No and ep.Patient_No = p.Patient_No and Waiting_Status = 'Not Started' order by Waiting_No", connection);
                MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                while (dataReader1.Read())
                {
                    waitingno++;
                    dataGridView7.Rows.Add(waitingno, dataReader1.GetString("RTRIM(CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit))"), dataReader1.GetString("Waiting_For"));
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }
        public void ConsultantAvail()
        {
            dataGridView6.Rows.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT *,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from employeetbl e, employee_positiontbl ep where e.Employee_Status = 'Active' and ep.Position_Name = 'Consultant' and e.Employee_Position_No = ep.Employee_Position_No order by e.Employee_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView6.Rows.Add(dataReader.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)"), dataReader.GetString("Employee_Availability"));
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            servicemonitoringPanel.Hide();
            consultantschedPanel.Show();
            schedulePanel.Hide();
            empButtons.Hide();
            emSchedPanel.Hide();
            slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Top;
            slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Height;

            Timer timer = new Timer();
            timer.Interval = (10 * 1000); // 5 secs
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

            button8.Textcolor = Color.FromArgb(4, 180, 253);
            button1.Textcolor = Color.White;
            button14.Textcolor = Color.White;
            button4.Textcolor = Color.White;
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            WaitingPatients();
            ConsultantAvail();
        }
        private void dataGridView1_Click(object sender, EventArgs e)
        {
            string patient = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1].Value.ToString();
            GetSelectedPatient(patient);
            textBox2.Text = patient;
            string service = "";
            for (int j = 0; j < dataGridView2.Rows.Count; j++)
            {
                service = dataGridView2.Rows[j].Cells[0].Value.ToString();
                int no = Convert.ToInt32(dataGridView2.Rows[j].Cells[1].Value);
                int reqno = Convert.ToInt32(dataGridView2.Rows[j].Cells[2].Value);
                if (no != reqno)
                {
                    break;
                }
            }
            textBox3.Text = service;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string patient = dataGridView7.Rows[dataGridView7.CurrentCell.RowIndex].Cells[1].Value.ToString().Trim();
            string reason = dataGridView7.Rows[dataGridView7.CurrentCell.RowIndex].Cells[2].Value.ToString();
            int patientno = 0;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT Patient_No,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from patienttbl where CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) = '" + patient + "'", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    patientno = dataReader.GetInt32("Patient_No");
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            if (reason == "Consultation")
            {
                connection.Open();
                MySqlCommand cmd1 = new MySqlCommand("UPDATE employeetbl e, employee_patienttbl ep, patienttbl p set e.Employee_Availability = 'Not Available' where e.Employee_No = ep.Employee_No and p.Patient_No = '" + patientno + "' and p.Patient_No = ep.Patient_No", connection);
                cmd1.ExecuteNonQuery();
                connection.Close();
            }

        }
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            string pname = textBox7.Text.Trim();
            if (pname.Length == 0)
            {
                pname = "null";
            }
            bool meron = false;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT *,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from patienttbl where CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) LIKE '%" + pname + "%' and Patient_Status = 'Active'", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    listBox1.Items.Add(dataReader.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)"));
                    meron = true;
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            if (meron)
            {
                listBox1.Visible = true;
            }
            else
            {
                listBox1.Visible = false;
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            int index = 0;
            try
            {
                if (keyData == Keys.Down)
                {
                    //Perform validations and so on then
                    try
                    {
                        listBox1.SetSelected(listBox1.SelectedIndex + 1, true);
                        return true;
                    }
                    catch (Exception)
                    { }
                }
                else if (keyData == Keys.Up)
                {
                    //Perform validations and so on then
                    try
                    {
                        listBox1.SetSelected(listBox1.SelectedIndex - 1, true);
                        return true;
                    }
                    catch (Exception)
                    {
                    }
                }
                if (keyData == Keys.Enter)
                {
                    textBox7.Text = listBox1.SelectedItem.ToString();
                    listBox1.Visible = false;
                }
            }
            catch (Exception)
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        void listBox1_LostFocus(object sender, System.EventArgs e)
        {
            listBox1.Visible = false;
        }
        private void listBox1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                textBox7.Text = listBox1.SelectedItem.ToString();
                listBox1.Visible = false;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            waitlistTransition.ShowSync(panel2);
            GetAvailableConsultant();
            button10.Enabled = false;
            dataGridView6.Enabled = false;
            dataGridView7.Enabled = false;
            button13.Enabled = false;
            panel2.Show();
            panel2.BringToFront();
        }

        private void listBox1_MouseLeave(object sender, EventArgs e)
        {
            listBox1.Visible = false;
        }
        public int GetEmployeePatientNo()
        {
            int emp_patientno = 0;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT Employee_Patient_No from employee_patienttbl order by Employee_Patient_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    emp_patientno = dataReader.GetInt32("Employee_Patient_No");
                }
                emp_patientno = emp_patientno + 1;
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            return emp_patientno;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            string emp = textBox4.Text.Trim();
            GetEmployeeWithSched(emp);
        }

        private void button12_Click_1(object sender, EventArgs e)
        {
            string emp = "";
            GetEmployeeWithSched(emp);
            label53.Text = "";
            servicemonitoringPanel.Hide();
            consultantschedPanel.Hide();
            empButtons.Show();
            schedulePanel.Hide();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            string machine = "", employee = "";
            servicemonitoringPanel.Hide();
            consultantschedPanel.Hide();
            schedulePanel.Hide();
            empButtons.Hide();
            emSchedPanel.Show();
            GetMachineAvail(machine);
            GetEmployeeAvail(employee);
            slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Top;
            slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Height;

            button14.Textcolor = Color.FromArgb(4, 180, 253);
            button1.Textcolor = Color.White;
            button4.Textcolor = Color.White;
            button8.Textcolor = Color.White;
        }
        public void UpdateSchedStatus(int schedno, string status, int machine, int employee, string status2)
        {
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE appointmenttbl set Appointment_Status = '" + status + "' where Appointment_No = '" + schedno + "'", connection);
                cmd.ExecuteNonQuery();
                connection.Close();

                connection.Open();
                MySqlCommand cmd1 = new MySqlCommand("UPDATE employeetbl set Employee_Availability = '" + status2 + "' where Employee_No = '" + employee + "'", connection);
                cmd1.ExecuteNonQuery();
                connection.Close();

                connection.Open();
                MySqlCommand cmd2 = new MySqlCommand("Update machinetbl set Machine_Status = '" + status2 + "' where Machine_No = '" + machine + "'", connection);
                cmd2.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }
        public void GetProductsPerService(string service)
        {
            dataGridView5.Rows.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from servicetbl s, producttbl p, product_typetbl pt, product_prodtypetbl ppt, service_producttbl sp where s.Service_Name = '" + service + "' and s.Service_No = sp.Service_No and ppt.Product_ProdType_No = sp.Product_ProdType_No and ppt.Product_No = p.Product_No and ppt.Product_Type_No = pt.Product_Type_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView5.Rows.Add(dataReader.GetString("Product_Type"), dataReader.GetString("Product_Name"));
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }
        public void GetProducts()
        {
            comboBox10.Items.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from producttbl p, product_inventorytbl pi, product_prodtypetbl ppt, product_typetbl pt where p.Product_Status = 'Available' and pi.Total_Quantity > 0 and p.Product_No = ppt.Product_No and ppt.Product_ProdType_No = pi.Product_ProdType_No and  ppt.Product_Type_No = pt.Product_Type_No group by pt.Product_Type_No order by p.Product_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    comboBox10.Items.Add(dataReader.GetString("Product_Type"));
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            try
            {
                comboBox10.SelectedIndex = 0;
            }
            catch (Exception)
            {
                comboBox10.Items.Add("No available");
                comboBox10.SelectedIndex = 0;
            }
        }
        public void GetProduct(string prodtype)
        {
            comboBox11.Items.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from product_typetbl pt, producttbl p, product_prodtypetbl ppt, product_inventorytbl pi where pi.Total_Quantity > 0 and pt.Product_Type = '" + prodtype + "' and pt.Product_Type_No = ppt.Product_Type_No and ppt.Product_No = p.Product_No and ppt.Product_ProdType_No = pi.Product_ProdType_No order by ppt.Product_Type_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    comboBox11.Items.Add(dataReader.GetString("Product_Name"));
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            try
            {
                comboBox11.SelectedIndex = 0;
            }
            catch (Exception)
            {
                comboBox11.Items.Add("No available");
                comboBox11.SelectedIndex = 0;
            }
        }

        public void ServiceProductDeduct(int schedno, int quantity, int prodtypeno)
        {
            int prevqty = 0, newqty = 0;
            int inventoryno = 0;
            int subtractno = GetSubtractNo();
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string added_no = null;
            try
            {
                connection.Open();
                MySqlCommand cmd1 = new MySqlCommand("SELECT * from product_inventorytbl pi, actual_servicetbl acs where acs.Appointment_No = '" + schedno + "' and pi.Product_ProdType_No = '" + prodtypeno + "' and acs.Product_ProdType_No = pi.Product_ProdType_No", connection);
                MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                while (dataReader1.Read())
                {
                    prevqty = dataReader1.GetInt32("Total_Quantity");
                    inventoryno = dataReader1.GetInt32("Inventory_No");
                }
                connection.Close();
                newqty = prevqty - quantity;
                if (newqty <= 0)
                {
                    newqty = 0;
                }
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE product_inventorytbl pi, actual_servicetbl acs set pi.Total_Quantity = '" + newqty + "' where acs.Appointment_No = '" + schedno + "' and pi.Product_ProdType_No = '" + prodtypeno + "' and acs.Product_ProdType_No = pi.Product_ProdType_No", connection);
                cmd.ExecuteNonQuery();
                connection.Close();

                connection.Open();
                MySqlCommand cmd2 = new MySqlCommand("Insert into inventory_subtracttbl values ('" + subtractno + "','"+date+"','" + inventoryno + "','"+prevqty+"','" + quantity + "','For Service','"+Convert.ToInt32(added_no)+"')", connection);
                cmd2.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }

        public void UpdateReason(int typeno)
        {
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("Update inventory_subtracttbl ins, product_inventorytbl pi set ins.Reason_Usage = 'Cancelled Service' where pi.Product_ProdType_No ='" + typeno + "' and pi.Inventory_No = ins.Inventory_No", connection);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
                MessageBox.Show(e.Message);
            }
        }
        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int schedno = 0;
            string status = "";
            string empname = label53.Text;
            string paymentstatus = "";
            string status2 = "";
            int machineno = 0, employeeno = 0;
            int quantitydeduct = 0;
            schedno = Convert.ToInt32(dataGridView4.Rows[e.RowIndex].Cells[0].Value);
            if (e.ColumnIndex == dataGridView4.Columns[6].Index)
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * from appointment_payment where Appointment_No = '" + schedno + "'", connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        paymentstatus = dataReader.GetString("Payment_Status");
                    }
                    connection.Close();
                }
                catch (Exception me)
                {
                    connection.Close();
                    MessageBox.Show(me.Message);
                }
                string service = dataGridView4.Rows[e.RowIndex].Cells[2].Value.ToString();
                if (dataGridView4.Rows[e.RowIndex].Cells[5].Value.ToString() == "Done")
                {
                    MessageBox.Show("Session already done!");
                }
                else if (dataGridView4.Rows[e.RowIndex].Cells[5].Value.ToString() == "On Going")
                {
                    MessageBox.Show("Session is already on going!");
                }
                else if (dataGridView4.Rows[e.RowIndex].Cells[5].Value.ToString() == "Cancelled")
                {
                    MessageBox.Show("Session is already cancelled!");
                }
                else
                {
                    if (paymentstatus == "Paid")
                    {
                        label60.Text = schedno.ToString();
                        GetProductsPerService(service);
                        GetProducts();
                        button2.Enabled = false;
                        button4.Enabled = false;
                        button1.Enabled = false;
                        button14.Enabled = false;
                        button8.Enabled = false;
                        button12.Enabled = false;
                        dataGridView4.Enabled = false;
                        panel22.Visible = true;
                        panel22.BringToFront();
                        panel5.SendToBack();
                        dataGridView4.SendToBack();
                    }
                    else
                    {
                        MessageBox.Show("Please settle first the balance");
                    }
                }

            }
            if (e.ColumnIndex == dataGridView4.Columns[7].Index)
            {
                string pname = dataGridView4.Rows[e.RowIndex].Cells[1].Value.ToString();
                string service = dataGridView4.Rows[e.RowIndex].Cells[2].Value.ToString();

                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * from appointmenttbl a, employee_appointmenttbl ea, employee_patienttbl ep where a.Appointment_No = '" + schedno + "' and a.Appointment_No = ea.Appointment_No and ea.Employee_Patient_No = ep.Employee_Patient_No", connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        machineno = dataReader.GetInt32("Machine_No");
                        employeeno = dataReader.GetInt32("Employee_No");
                    }
                    connection.Close();
                }
                catch (Exception me)
                {
                    connection.Close();
                    MessageBox.Show(me.Message);
                }
                if (dataGridView4.Rows[e.RowIndex].Cells[5].Value.ToString() == "Done")
                {
                    MessageBox.Show("Session already done!");
                }
                else if (dataGridView4.Rows[e.RowIndex].Cells[5].Value.ToString() == "Cancelled")
                {
                    MessageBox.Show("Session is already cancelled!");
                }
                else
                {
                    List<int> prodtypenums = new List<int>();
                    int prodtypeno = 0;
                    connection.Open();
                    MySqlCommand cmd1 = new MySqlCommand("SELECT * from appointmenttbl a, actual_servicetbl acs where a.Appointment_No = '" + schedno + "' and a.Appointment_No = acs.Appointment_No", connection);
                    MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                    while (dataReader1.Read())
                    {
                        prodtypenums.Add(dataReader1.GetInt32("Product_ProdType_No"));
                    }
                    connection.Close();
                    for (int x = 0; x < prodtypenums.Count; x++)
                    {
                        prodtypeno = prodtypenums[x];
                        UpdateReason(prodtypeno);
                    }
                    prodtypenums.Clear();
                    DialogResult dr = MessageBox.Show("Do you want it to be re-scheduled?", "Wait", MessageBoxButtons.YesNo);
                    if (dr == DialogResult.Yes)
                    {
                        label38.Text = schedno.ToString();
                        label39.Text = pname;
                        label40.Text = service;
                        GetMachines();
                        GetEmployee();
                        dateTimePicker2.MinDate = DateTime.Now;
                        button2.Enabled = false;
                        button4.Enabled = false;
                        button1.Enabled = false;
                        button14.Enabled = false;
                        button8.Enabled = false;
                        button12.Enabled = false;
                        dataGridView4.Enabled = false;
                        panel5.Visible = true;
                        panel5.BringToFront();
                        panel22.SendToBack();
                        dataGridView4.SendToBack();
                    }
                    else
                    {
                        status = "Cancelled";
                        status2 = "Available";
                        UpdateSchedStatus(schedno, status, machineno, employeeno, status2);
                        GetSchedule(empname);
                        dataGridView4.ClearSelection();
                        MessageBox.Show("Appointment Cancelled!");
                        dataGridView8.Rows.Clear();
                    }
                }
            }

            if (e.ColumnIndex == dataGridView4.Columns[8].Index)
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * from appointmenttbl a, employee_appointmenttbl ea, employee_patienttbl ep where a.Appointment_No = '" + schedno + "' and a.Appointment_No = ea.Appointment_No and ea.Employee_Patient_No = ep.Employee_Patient_No", connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        machineno = dataReader.GetInt32("Machine_No");
                        employeeno = dataReader.GetInt32("Employee_No");
                    }
                    connection.Close();
                }
                catch (Exception me)
                {
                    connection.Close();
                    MessageBox.Show(me.Message);
                }

                if (dataGridView4.Rows[e.RowIndex].Cells[5].Value.ToString() == "Done")
                {
                    MessageBox.Show("Session already done!");
                }
                else if (dataGridView4.Rows[e.RowIndex].Cells[5].Value.ToString() == "Cancelled")
                {
                    MessageBox.Show("Session is already cancelled!");
                }
                else
                {
                    status = "Done";
                    status2 = "Available";
                    UpdateSchedStatus(schedno, status, machineno, employeeno, status2);
                    GetSchedule(empname);
                    dataGridView4.ClearSelection();
                    MessageBox.Show("Appointment Done!");
                }
            }
        }

        private void dataGridView3_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 6 || e.ColumnIndex == 7)
            {
                dataGridView4.Cursor = Cursors.Hand;
            }
            else
            {
                dataGridView4.Cursor = Cursors.Default;
            }
        }

        private void label31_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            button4.Enabled = true;
            button1.Enabled = true;
            button14.Enabled = true;
            button8.Enabled = true;
            button12.Enabled = true;
            dataGridView4.Enabled = true;
            panel10.Visible = false;
            label38.Text = "";
            label39.Text = "";
            label40.Text = "";
        }
        public void GetAvailableTime()
        {
            int appointmentno = Convert.ToInt32(label44.Text);
            string date = dateTimePicker3.Value.ToString("yyyy-MM-dd");
            string service = label40.Text;
            string machinename = comboBox7.Text, therapist = comboBox8.Text;
            string starttime = "";
            string endtime = "";
            int min = 0, hour = 0, servicehour = 0, servicemin = 0;
            string zero = "";
            string checkdate = "", checkstart = "", checkend = "", startampm = "am", endampm = "am", checkstartampm = "", checkendampm = "", checkmachine = "", checkemp = "";
            int starthour = 0, startmin = 0, endhour = 0, endmin = 0, checkstarthour = 0, checkstartmin = 0, checkendhour = 0, checkendmin = 0;
            bool check = false;
            List<string> starttimelist = new List<string>();
            comboBox9.Items.Clear();
            try
            {
                connection.Open();
                string query = "SELECT * from servicetbl where Service_Name = '" + service + "'";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    servicehour = dataReader.GetInt32("Hour_Consumed");
                    servicemin = dataReader.GetInt32("Minute_Consumed");
                }
                connection.Close();

                for (int j = 9; j < 21; j++)
                {
                    hour = j;
                    if (hour >= 12)
                    {
                        startampm = "pm";
                    }
                    if (hour > 12)
                    {
                        hour = j - 12;
                    }
                    for (int o = 0; o <= 45; o = o + 15)
                    {
                        starttime = hour.ToString("D2") + ":" + o.ToString("D2") + " " + startampm;
                        starthour = Convert.ToInt32(starttime.Substring(0, 2));
                        startmin = Convert.ToInt32(starttime.Substring(3, 2));
                        startampm = starttime.Substring(6, 2);
                        endhour = starthour + servicehour;
                        endmin = startmin + servicemin;
                        if (endmin >= 60)
                        {
                            endhour = endhour + (endmin / 60);
                            endmin = endmin % 60;
                        }
                        if (endhour > 12)
                        {
                            endhour = endhour - 12;
                        }
                        if (endhour >= 12 || startampm == "pm")
                        {
                            endampm = "pm";
                        }
                        endtime = endhour.ToString("D2") + ":" + endmin.ToString("D2") + " " + endampm;
                        starttimelist.Add(starttime);

                        connection.Open();
                        string query1 = "SELECT *,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from appointmenttbl a,employee_appointmenttbl ea,employee_patienttbl ept, employeetbl e, employee_positiontbl ep,machinetbl m where a.Appointment_No <> '" + appointmentno + "' and a.Appointment_Status <> 'Done' and Appointment_Status <> 'Cancelled' and a.Appointment_No = ea.Appointment_No and a.Machine_No = m.Machine_No and ea.Employee_Patient_No = ept.Employee_Patient_No and ept.Employee_No = e.Employee_No and ep.Position_Name = 'Therapist' and e.Employee_Position_No = ep.Employee_Position_No";
                        MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                        MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                        while (dataReader1.Read())
                        {
                            checkdate = dataReader1.GetDateTime("Appointment_Date").ToString("yyyy-MM-dd");
                            checkstart = dataReader1.GetString("Appointment_StartTime");
                            checkend = dataReader1.GetString("Appointment_EndTime");
                            checkmachine = dataReader1.GetString("Machine_Name");
                            checkemp = dataReader1.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)");
                            checkstarthour = Convert.ToInt32(checkstart.Substring(0, 2));
                            checkstartmin = Convert.ToInt32(checkstart.Substring(3, 2));
                            checkstartampm = checkstart.Substring(6, 2);
                            checkendhour = Convert.ToInt32(checkend.Substring(0, 2));
                            checkendmin = Convert.ToInt32(checkend.Substring(3, 2));
                            checkendampm = checkend.Substring(6, 2);
                            if (date == checkdate)
                            {
                                if ((((starthour >= checkstarthour) && (starthour <= checkendhour && startmin <= checkendmin)) && (checkendampm == startampm || startampm == checkstartampm) || ((endhour >= checkstarthour) && (endhour <= checkendhour) && (checkstartampm == endampm || endampm == checkendampm))))
                                {
                                    if (machinename == checkmachine || therapist == checkemp)
                                    {
                                        starttimelist.Remove(starttime);
                                        break;
                                    }
                                }
                            }

                        }
                        connection.Close();
                    }
                }
            }
            catch (MySqlException me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            foreach (var time in starttimelist)
            {
                comboBox9.Items.Add(time);
            }
            try
            {
                comboBox9.SelectedIndex = 0;
            }
            catch (Exception)
            {
                comboBox9.Items.Add("No available");
                comboBox9.SelectedIndex = 0;
            }
        }
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            GetAvailableTime();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string service = label40.Text, start = comboBox1.Text;
            string startampm = "", endampm = "", endtime = "";
            int hour = 0, min = 0, starthour = 0, startmin = 0, endmin = 0;
            string endmin1 = "";
            string endhour = "";
            try
            {
                connection.Open();
                string query = "SELECT * from servicetbl where Service_Name = '" + service + "'";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    hour = dataReader.GetInt32("Hour_Consumed");
                    min = dataReader.GetInt32("Minute_Consumed");
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            starthour = Convert.ToInt32(start.Substring(0, 2));
            startmin = Convert.ToInt32(start.Substring(3, 2));
            startampm = start.Substring(6, 2);
            if (starthour < 9 && startampm == "pm")
            {
                starthour = starthour + 12;
            }
            endhour = Convert.ToInt32(starthour + hour).ToString();
            endmin1 = Convert.ToInt32(startmin + min).ToString();
            if (Convert.ToInt32(endhour) > 12)
            {
                endhour = Convert.ToInt32(Convert.ToInt32(endhour) - 12).ToString();
                endhour = "0" + endhour;
            }
            if (Convert.ToInt32(endmin1) >= 60)
            {
                endhour = endhour + (Convert.ToInt32(endmin1) / 60);
                endmin1 = Convert.ToInt32(Convert.ToInt32(endmin1) % 60).ToString();
            }
            if (endmin1.Equals("0") || Convert.ToInt32(endmin1) < 10)
            {
                endmin1 = "0" + endmin1;

            }
            if (Convert.ToInt32(endhour) >= 12 || startampm == "pm")
            {
                endampm = "pm";
            }
            else
            {
                endampm = "am";
            }

            endtime = endhour + ":" + endmin1 + " " + endampm;
            textBox8.Text = endtime;
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetAvailableTime();
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetAvailableTime();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            string empname = label53.Text;
            string therapist = comboBox4.Text, machine = comboBox6.Text, date = dateTimePicker2.Value.ToString("yyyy-MM-dd"), starttime = comboBox1.Text, endtime = textBox8.Text;
            int appointmentno = Convert.ToInt32(label38.Text), machineno = 0, therapistno = 0;
            try
            {
                connection.Open();
                MySqlCommand cmd1 = new MySqlCommand("SELECT Machine_No from machinetbl where Machine_Name = '" + machine + "'", connection);
                MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                while (dataReader1.Read())
                {
                    machineno = dataReader1.GetInt32("Machine_No");
                }
                connection.Close();

                connection.Open();
                MySqlCommand cmd3 = new MySqlCommand("SELECT Employee_No,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from employeetbl where CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) = '" + therapist + "'", connection);
                MySqlDataReader dataReader3 = cmd3.ExecuteReader();
                while (dataReader3.Read())
                {
                    therapistno = dataReader3.GetInt32("Employee_No");
                }
                connection.Close();

                connection.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE appointmenttbl set Appointment_Date = '" + date + "',Appointment_StartTime = '" + starttime + "', Appointment_EndTime='" + endtime + "', Appointment_Status = 'Not Started', Machine_No = '" + machineno + "' where Appointment_No = '" + appointmentno + "'", connection);
                cmd.ExecuteNonQuery();

                MySqlCommand cmd2 = new MySqlCommand("UPDATE employee_patienttbl ep, employee_appointmenttbl ea, appointmenttbl a set Employee_No = '" + therapistno + "' where a.Appointment_No = '" + appointmentno + "' and ea.Appointment_No = a.Appointment_No and ep.Employee_Patient_No = ea.Employee_Patient_No", connection);
                cmd2.ExecuteNonQuery();
                connection.Close();

                MessageBox.Show("Re-scheduling successful!");
                GetSchedule(empname);
                dataGridView4.ClearSelection();
                button2.Enabled = true;
                button4.Enabled = true;
                button1.Enabled = true;
                button14.Enabled = true;
                button8.Enabled = true;
                button12.Enabled = true;
                dataGridView4.Enabled = true;
                panel10.Visible = false;
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            bool check = false;
            string empname = label53.Text;
            string therapist = comboBox8.Text, machine = comboBox7.Text, date = dateTimePicker3.Value.ToString("yyyy-MM-dd"), starttime = comboBox9.Text, endtime = textBox9.Text;
            int appointmentno = Convert.ToInt32(label44.Text), machineno = 0, therapistno = 0;
            try
            {
                connection.Open();
                MySqlCommand cmd1 = new MySqlCommand("SELECT Machine_No from machinetbl where Machine_Name = '" + machine + "'", connection);
                MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                while (dataReader1.Read())
                {
                    machineno = dataReader1.GetInt32("Machine_No");
                }
                connection.Close();

                connection.Open();
                MySqlCommand cmd3 = new MySqlCommand("SELECT Employee_No,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from employeetbl where CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) = '" + therapist + "'", connection);
                MySqlDataReader dataReader3 = cmd3.ExecuteReader();
                while (dataReader3.Read())
                {
                    therapistno = dataReader3.GetInt32("Employee_No");
                }
                connection.Close();

                if (string.IsNullOrEmpty(comboBox9.Text.Trim()))
                {
                    check = true;
                    errorProvider.SetError(textBox9, "Select time first");
                }
                else
                {
                    if (comboBox9.Text == "No available")
                    {
                        check = true;
                        errorProvider.SetError(textBox9, "No available time");
                    }
                    else
                    {
                        errorProvider.SetError(textBox9, string.Empty);
                    }
                }

                if (check == false)
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand("UPDATE appointmenttbl set Appointment_Date = '" + date + "',Appointment_StartTime = '" + starttime + "', Appointment_EndTime='" + endtime + "', Appointment_Status = 'Not Started', Machine_No = '" + machineno + "' where Appointment_No = '" + appointmentno + "'", connection);
                    cmd.ExecuteNonQuery();

                    MySqlCommand cmd2 = new MySqlCommand("UPDATE employee_patienttbl ep, employee_appointmenttbl ea, appointmenttbl a set Employee_No = '" + therapistno + "' where a.Appointment_No = '" + appointmentno + "' and ea.Appointment_No = a.Appointment_No and ep.Employee_Patient_No = ea.Employee_Patient_No", connection);
                    cmd2.ExecuteNonQuery();
                    connection.Close();

                    MessageBox.Show("Re-scheduling successful!");
                    GetSchedule(empname);
                    dataGridView4.ClearSelection();
                    button2.Enabled = true;
                    button4.Enabled = true;
                    button1.Enabled = true;
                    button14.Enabled = true;
                    button8.Enabled = true;
                    button12.Enabled = true;
                    dataGridView4.Enabled = true;
                    panel5.Visible = false;
                }
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            GetAvailableTime();
        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            string service = label27.Text, start = comboBox9.Text;
            string startampm = "", endampm = "", endtime = "";
            int hour = 0, min = 0, starthour = 0, startmin = 0, endmin = 0;
            string endmin1 = "";
            string endhour = "";
            try
            {
                connection.Open();
                string query = "SELECT * from servicetbl where Service_Name = '" + service + "'";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    hour = dataReader.GetInt32("Hour_Consumed");
                    min = dataReader.GetInt32("Minute_Consumed");
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            starthour = Convert.ToInt32(start.Substring(0, 2));
            startmin = Convert.ToInt32(start.Substring(3, 2));
            startampm = start.Substring(6, 2);
            if (starthour < 9 && startampm == "pm")
            {
                starthour = starthour + 12;
            }
            endhour = Convert.ToInt32(starthour + hour).ToString();
            endmin1 = Convert.ToInt32(startmin + min).ToString();
            if (Convert.ToInt32(endhour) > 12)
            {
                endhour = Convert.ToInt32(Convert.ToInt32(endhour) - 12).ToString();
                endhour = "0" + endhour;
            }
            if (Convert.ToInt32(endmin1) >= 60)
            {
                endhour = endhour + (Convert.ToInt32(endmin1) / 60);
                endmin1 = Convert.ToInt32(Convert.ToInt32(endmin1) % 60).ToString();
            }
            if (endmin1.Equals("0") || Convert.ToInt32(endmin1) < 10)
            {
                endmin1 = "0" + endmin1;

            }
            if (Convert.ToInt32(endhour) >= 12 || startampm == "pm")
            {
                endampm = "pm";
            }
            else
            {
                endampm = "am";
            }

            endtime = endhour + ":" + endmin1 + " " + endampm;
            textBox9.Text = endtime;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            string date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string service = textBox3.Text;
            string machinename = comboBox2.Text, therapist = comboBox3.Text;
            string starttime = "";
            string endtime = "";
            int min = 0, hour = 0, servicehour = 0, servicemin = 0;
            string zero = "";
            string checkdate = "", checkstart = "", checkend = "", startampm = "am", endampm = "am", checkstartampm = "", checkendampm = "", checkmachine = "", checkemp = "";
            int starthour = 0, startmin = 0, endhour = 0, endmin = 0, checkstarthour = 0, checkstartmin = 0, checkendhour = 0, checkendmin = 0;
            bool check = false;
            List<string> starttimelist = new List<string>();
            comboBox5.Items.Clear();
            try
            {
                connection.Open();
                string query = "SELECT * from servicetbl where Service_Name = '" + service + "'";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    servicehour = dataReader.GetInt32("Hour_Consumed");
                    servicemin = dataReader.GetInt32("Minute_Consumed");
                }
                connection.Close();

                for (int j = 9; j < 21; j++)
                {
                    hour = j;
                    if (hour >= 12)
                    {
                        startampm = "pm";
                    }
                    if (hour > 12)
                    {
                        hour = j - 12;
                    }
                    for (int o = 0; o <= 45; o = o + 15)
                    {
                        starttime = hour.ToString("D2") + ":" + o.ToString("D2") + " " + startampm;
                        starthour = Convert.ToInt32(starttime.Substring(0, 2));
                        startmin = Convert.ToInt32(starttime.Substring(3, 2));
                        startampm = starttime.Substring(6, 2);
                        endhour = starthour + servicehour;
                        endmin = startmin + servicemin;
                        if (endmin >= 60)
                        {
                            endhour = endhour + (endmin / 60);
                            endmin = endmin % 60;
                        }
                        if (endhour > 12)
                        {
                            endhour = endhour - 12;
                        }
                        if (endhour >= 12 || startampm == "pm")
                        {
                            endampm = "pm";
                        }
                        endtime = endhour.ToString("D2") + ":" + endmin.ToString("D2") + " " + endampm;
                        starttimelist.Add(starttime);

                        connection.Open();
                        string query1 = "SELECT *,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from appointmenttbl a,employee_appointmenttbl ea,employee_patienttbl ept, employeetbl e, employee_positiontbl ep,machinetbl m where a.Appointment_Status <> 'Done' and Appointment_Status <> 'Cancelled' and a.Appointment_No = ea.Appointment_No and a.Machine_No = m.Machine_No and ea.Employee_Patient_No = ept.Employee_Patient_No and ept.Employee_No = e.Employee_No and ep.Position_Name = 'Therapist' and e.Employee_Position_No = ep.Employee_Position_No";
                        MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                        MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                        while (dataReader1.Read())
                        {
                            checkdate = dataReader1.GetDateTime("Appointment_Date").ToString("yyyy-MM-dd");
                            checkstart = dataReader1.GetString("Appointment_StartTime");
                            checkend = dataReader1.GetString("Appointment_EndTime");
                            checkmachine = dataReader1.GetString("Machine_Name");
                            checkemp = dataReader1.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)");
                            checkstarthour = Convert.ToInt32(checkstart.Substring(0, 2));
                            checkstartmin = Convert.ToInt32(checkstart.Substring(3, 2));
                            checkstartampm = checkstart.Substring(6, 2);
                            checkendhour = Convert.ToInt32(checkend.Substring(0, 2));
                            checkendmin = Convert.ToInt32(checkend.Substring(3, 2));
                            checkendampm = checkend.Substring(6, 2);
                            if (date == checkdate)
                            {
                                if ((((starthour >= checkstarthour) && (starthour <= checkendhour && startmin <= checkendmin)) && (checkendampm == startampm || startampm == checkstartampm) || ((endhour >= checkstarthour) && (endhour <= checkendhour) && (checkstartampm == endampm || endampm == checkendampm))))
                                {
                                    if (machinename == checkmachine || therapist == checkemp)
                                    {
                                        starttimelist.Remove(starttime);
                                        break;
                                    }
                                }
                            }

                        }
                        connection.Close();
                    }
                }
            }
            catch (MySqlException me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            foreach (var time in starttimelist)
            {
                comboBox5.Items.Add(time);
            }
            try
            {
                comboBox5.SelectedIndex = 0;
            }
            catch (Exception)
            {
                comboBox5.Items.Add("No available");
                comboBox5.SelectedIndex = 0;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            comboBox10.Enabled = true;
            comboBox11.Enabled = true;
            button9.Enabled = true;
            button16.Enabled = true;
        }
        public int GetActualServiceNo()
        {
            int actualserviceno = 0;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from actual_servicetbl order by Actual_ServiceNo", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    actualserviceno = dataReader.GetInt32("Actual_ServiceNo");
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            actualserviceno = actualserviceno + 1;
            return actualserviceno;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            string containLetter = @"[A-Za-z~!@#$%^&*()_+=-]";
            int schedno = 0;
            string status = "";
            string empname = label53.Text;
            string qty = "";
            bool check = false;
            int machineno = 0, employeeno = 0;
            schedno = Convert.ToInt32(label60.Text);
            string status2 = "";
            int prodtypeno = 0;
            bool checker = false;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from appointmenttbl a, employee_appointmenttbl ea, employee_patienttbl ep where a.Appointment_No = '" + schedno + "' and a.Appointment_No = ea.Appointment_No and ea.Employee_Patient_No = ep.Employee_Patient_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    machineno = dataReader.GetInt32("Machine_No");
                    employeeno = dataReader.GetInt32("Employee_No");
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            for (int j = 0; j < dataGridView5.Rows.Count; j++)
            {
                string prod = dataGridView5.Rows[j].Cells[1].Value.ToString(), prodtype = dataGridView5.Rows[j].Cells[0].Value.ToString();
                try
                {
                    qty = dataGridView5.Rows[j].Cells[2].Value.ToString();
                }
                catch (Exception)
                {
                    check = true;
                    label57.Text = "Please input quantity";
                    break;
                }
                if (string.IsNullOrEmpty(qty))
                {
                    label57.Text = "Please input quantity";
                    check = true;
                    break;
                }
                else
                {
                    if (Regex.IsMatch(qty, containLetter))
                    {
                        label57.Text = "Quantity should not contain alpha characters";
                        check = true;
                        break;
                    }
                    else
                    {
                        try
                        {
                            connection.Open();
                            MySqlCommand cmd = new MySqlCommand("SELECT * from product_inventorytbl pi, producttbl p, product_typetbl pt, product_prodtypetbl ppt where p.Product_Name = '" + prod + "' and pt.Product_Type = '" + prodtype + "' and p.Product_No = ppt.Product_No and pt.Product_Type_No = ppt.Product_Type_No and ppt.Product_ProdType_No = pi.Product_ProdType_No and pi.Total_Quantity > '" + Convert.ToInt32(qty) + "'", connection);
                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            while (dataReader.Read())
                            {
                                checker = true;
                            }
                            connection.Close();
                        }
                        catch (Exception me)
                        {
                            connection.Close();
                            MessageBox.Show(me.Message);
                        }
                        if (!checker)
                        {
                            dataGridView5.Rows[j].Cells[0].Style.BackColor = Color.Salmon;
                            dataGridView5.Rows[j].Cells[1].Style.BackColor = Color.Salmon;
                            dataGridView5.Rows[j].Cells[2].Style.BackColor = Color.Salmon;
                            label57.Text = "Quantity is exceeding the maximum quantity in the inventory";
                            check = true;
                            break;
                        }
                        else
                        {
                            dataGridView5.Rows[j].Cells[0].Style.BackColor = Color.White;
                            dataGridView5.Rows[j].Cells[1].Style.BackColor = Color.White;
                            dataGridView5.Rows[j].Cells[2].Style.BackColor = Color.White;
                            label57.Text = "";
                        }
                    }
                }
            }
            if (!check)
            {
                try
                {
                    for (int j = 0; j < dataGridView5.Rows.Count; j++)
                    {
                        int actualserviceno = GetActualServiceNo();
                        int actualqty = Convert.ToInt32(dataGridView5.Rows[j].Cells[2].Value.ToString());
                        string prodname = "", prodtype = "";
                        try
                        {
                            prodname = dataGridView5.Rows[j].Cells[1].Value.ToString();
                            prodtype = dataGridView5.Rows[j].Cells[0].Value.ToString();
                        }
                        catch (Exception)
                        {
                        }
                        connection.Open();
                        MySqlCommand cmd1 = new MySqlCommand("SELECT * from product_prodtypetbl ppt, product_typetbl pt, producttbl p where p.Product_Name = '" + prodname + "' and pt.Product_Type = '" + prodtype + "' and p.Product_No = ppt.Product_No and pt.Product_Type_No = ppt.Product_Type_No", connection);
                        MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                        while (dataReader1.Read())
                        {
                            prodtypeno = dataReader1.GetInt32("Product_ProdType_No");
                        }
                        connection.Close();

                        connection.Open();
                        MySqlCommand cmd = new MySqlCommand("INSERT into actual_servicetbl values ('" + actualserviceno + "','" + schedno + "','" + prodtypeno + "','" + actualqty + "')", connection);
                        cmd.ExecuteNonQuery();
                        connection.Close();
                        ServiceProductDeduct(schedno, actualqty, prodtypeno);
                    }
                }
                catch (Exception me)
                {
                    connection.Close();
                    MessageBox.Show(me.Message);
                }
                status = "On Going";
                status2 = "Not Available";
                UpdateSchedStatus(schedno, status, machineno, employeeno, status2);
                GetSchedule(empname);
                dataGridView4.ClearSelection();
                button2.Enabled = true;
                button4.Enabled = true;
                button1.Enabled = true;
                button14.Enabled = true;
                button8.Enabled = true;
                button12.Enabled = true;
                dataGridView4.Enabled = true;
                panel22.Visible = false;
                MessageBox.Show("Appointment is ready to go!");
            }
        }

        private void label67_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            button4.Enabled = true;
            button1.Enabled = true;
            button14.Enabled = true;
            button8.Enabled = true;
            button12.Enabled = true;
            dataGridView4.Enabled = true;
            panel22.Visible = false;
        }

        private void label51_Click(object sender, EventArgs e)
        {
            panel5.Visible = false;
            button2.Enabled = true;
            button4.Enabled = true;
            button1.Enabled = true;
            button14.Enabled = true;
            button8.Enabled = true;
            button12.Enabled = true;
            dataGridView4.Enabled = true;
            label44.Text = "";
            label21.Text = "";
            label27.Text = "";
        }

        private void comboBox11_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void dataGridView5_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dataGridView5.CommitEdit(DataGridViewDataErrorContexts.Commit);
                string prod = dataGridView5.Rows[e.RowIndex].Cells[1].Value.ToString(), prodtype = dataGridView5.Rows[e.RowIndex].Cells[0].Value.ToString();
                string containLetter = @"[A-Za-z~!@#$%^&*()_+=-]";
                string qty = dataGridView5.Rows[e.RowIndex].Cells[2].Value.ToString();
                bool check = false;
                if (Regex.IsMatch(qty, containLetter))
                {
                    dataGridView5.Rows[e.RowIndex].Cells[2].Value = 0;
                }
                else
                {
                    try
                    {
                        connection.Open();
                        MySqlCommand cmd = new MySqlCommand("SELECT * from product_inventorytbl pi, producttbl p, product_typetbl pt, product_prodtypetbl ppt where p.Product_Name = '" + prod + "' and pt.Product_Type = '" + prodtype + "' and p.Product_No = ppt.Product_No and pt.Product_Type_No = ppt.Product_Type_No and ppt.Product_ProdType_No = pi.Product_ProdType_No and pi.Total_Quantity > '" + Convert.ToInt32(qty) + "'", connection);
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        while (dataReader.Read())
                        {
                            check = true;
                        }
                        connection.Close();
                    }
                    catch (Exception me)
                    {
                        connection.Close();
                        MessageBox.Show(me.Message);
                    }
                    if (!check)
                    {
                        dataGridView5.Rows[e.RowIndex].Cells[0].Style.BackColor = Color.Salmon;
                        dataGridView5.Rows[e.RowIndex].Cells[1].Style.BackColor = Color.Salmon;
                        dataGridView5.Rows[e.RowIndex].Cells[2].Style.BackColor = Color.Salmon;
                        label57.Text = "Quantity is exceeding the maximum quantity in the inventory";
                        dataGridView5.Rows[e.RowIndex].Cells[2].Value = "";
                    }
                    else
                    {
                        dataGridView5.Rows[e.RowIndex].Cells[0].Style.BackColor = Color.White;
                        dataGridView5.Rows[e.RowIndex].Cells[1].Style.BackColor = Color.White;
                        dataGridView5.Rows[e.RowIndex].Cells[2].Style.BackColor = Color.White;
                        label57.Text = "";
                    }
                }
            }
            catch (Exception) { }

        }
        private void button9_Click(object sender, EventArgs e)
        {
            string prodtype = comboBox10.Text, prodname = comboBox11.Text;
            string checkprodtype = "", checkprodname = "";
            bool check = false;
            for (int j = 0; j < dataGridView5.Rows.Count; j++)
            {
                checkprodtype = dataGridView5.Rows[j].Cells[0].Value.ToString();
                checkprodname = dataGridView5.Rows[j].Cells[1].Value.ToString();
                if (prodtype.Equals(checkprodtype) && prodname.Equals(checkprodname))
                {
                    check = true;
                    break;
                }
            }
            if (check)
            {
                errorProvider.SetError(comboBox11, "Product already in the table");
            }
            else
            {
                errorProvider.SetError(comboBox11, string.Empty);
                dataGridView5.Rows.Add(prodtype, prodname, "");
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {

            if (dataGridView5.Rows.Count <= 0)
            {
                errorProvider.SetError(button16, "No item to delete");
            }
            else
            {
                dataGridView5.Rows.RemoveAt(dataGridView5.CurrentCell.RowIndex);
                errorProvider.SetError(button16, string.Empty);
            }
        }

        private void dataGridView4_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView1.CurrentRow.Selected = false;
            dataGridView1.CurrentCell.Selected = false;
        }

        private void comboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prodtype = comboBox10.Text;
            GetProduct(prodtype);
        }

        private void dataGridView5_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView5.CurrentRow.Selected = false;
            dataGridView5.CurrentCell.Selected = false;
        }
        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
            dataGridView1.Enabled = false;
            dataGridView2.Enabled = false;
            bunifuThinButton21.Enabled = false;
            //appTransition.ShowSync(panel3);
            panel3.Visible = true;
            comboBox5.SelectedIndex = 0;
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            panel3.Visible = true;
            dataGridView1.Enabled = true;
            dataGridView2.Enabled = false;
            bunifuThinButton21.Enabled = true;
            panel3.Visible = false;
        }
        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            button4.Enabled = true;
            button1.Enabled = true;
            button14.Enabled = true;
            button8.Enabled = true;
            button12.Enabled = true;
            dataGridView4.Enabled = true;
            panel22.SendToBack();
            panel22.Hide();
        }
        public void GetConsultant(string consultant)
        {
            dataGridView6.Rows.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT *,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from employeetbl e, employee_positiontbl ep where CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) LIKE '%" + consultant + "%' and ep.Position_Name = 'Consultant' and e.Employee_Position_No = ep.Employee_Position_No and e.Employee_Status = 'Active'", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView6.Rows.Add(dataReader.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)"), dataReader.GetString("Employee_Availability"));
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }
        private void bunifuMaterialTextbox1_KeyUp(object sender, KeyEventArgs e)
        {
            dataGridView6.Rows.Clear();
            string consultant = bunifuMaterialTextbox1.Text.Trim();
            GetConsultant(consultant);

        }

        private void bunifuMaterialTextbox2_KeyUp(object sender, KeyEventArgs e)
        {
            string employee = bunifuMaterialTextbox2.Text.Trim();
            GetEmployeeAvail(employee);
        }

        private void bunifuMaterialTextbox3_KeyUp(object sender, KeyEventArgs e)
        {
            string machine = bunifuMaterialTextbox3.Text.Trim();
            GetMachineAvail(machine);
        }

        private void textBox5_KeyUp(object sender, KeyEventArgs e)
        {
            string patient = textBox5.Text.Trim();
            GetPatients(patient);
        }
        public void GetPatientByConsultant(string consultant)
        {
            dataGridView1.Rows.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT *,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit),CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from patienttbl p,employee_patienttbl ep, employee_positiontbl epos, employeetbl e where CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) LIKE '%" + consultant + "%' and epos.Position_Name = 'Consultant' and epos.Employee_Position_No = e.Employee_Position_No and e.Employee_No = ep.Employee_No and p.Patient_No = ep.Patient_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView1.Rows.Add(dataReader.GetInt32("Patient_No"), dataReader.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)"));
                }

                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }
        private void textBox6_KeyUp(object sender, KeyEventArgs e)
        {
            string consultant = textBox6.Text.Trim();
            GetPatientByConsultant(consultant);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.ParentForm.Controls.Remove(this);
            dashboardUC dash = new dashboardUC(ParentForm.Username);
            dash.BringToFront();
            dash.Show();
            this.Hide();
        }

        private void bunifuMaterialTextbox2_KeyUp(object sender, EventArgs e)
        {
            string employee = bunifuMaterialTextbox2.Text.Trim();
            GetEmployeeAvail(employee);
        }

        private void bunifuMaterialTextbox3_OnValueChanged(object sender, EventArgs e)
        {

        }

        public void GetAvailableConsultant()
        {
            comboBox12.Items.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("Select *,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from employeetbl e, employee_positiontbl ep where Position_Name = 'Consultant' and e.Employee_Position_No = ep.Employee_Position_No and e.Employee_Status = 'Active'",connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    comboBox12.Items.Add(dataReader.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)"));
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            try
            {
                comboBox12.SelectedIndex = 0;
            }
            catch (Exception)
            {
                comboBox12.Items.Add("No available consultant");
                comboBox12.SelectedIndex = 0;
            }
        }

        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            errorProvider.SetError(textBox7, string.Empty);
            string patientname = textBox7.Text.Trim();
            string checkname = "", employee = "",emp_status="",waitlist_stat = "";
            int patientno = 0, employeeno = 0, emp_patientno = 0;
            bool check = false,checker = false;
            int no = dataGridView7.Rows.Count;
            employee = comboBox12.Text;
            if (employee == "No available consultant")
            {
                check = true;
                label14.Text = "There's no available consultant";
            }
            else
            {
                label14.Text = "";
            }
            try
            {
                connection.Open();
                MySqlCommand cmd2 = new MySqlCommand("SELECT *, CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from employeetbl where CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) = '"+employee+"'", connection);
                MySqlDataReader dataReader2 = cmd2.ExecuteReader();
                while (dataReader2.Read())
                {
                    employeeno = dataReader2.GetInt32("Employee_No");
                    emp_status = dataReader2.GetString("Employee_Availability");
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            if (patientname.Length == 0)
            {
                errorProvider.SetError(textBox7, "Please insert name");
                check = true;
            }
            else
            {
                errorProvider.SetError(textBox7, string.Empty);
            }
            for (int j = 0; j < dataGridView7.Rows.Count; j++)
            {
                checkname = dataGridView7.Rows[j].Cells[1].Value.ToString();
                if (checkname.EndsWith(" "))
                {

                    if (patientname.ToLower().Equals(checkname.Trim().ToLower()))
                    {
                        errorProvider.SetError(textBox7, "Name already exists on the table");
                        check = true;
                        break;
                    }
                }
                else
                {
                    if (patientname.ToLower().Equals(checkname.ToLower()))
                    {
                        errorProvider.SetError(textBox7, "Name already exists on the table");
                        check = true;
                        break;
                    }
                }
            }
            if (check == false)
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd1 = new MySqlCommand("SELECT Patient_No,RTRIM(CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)) from patienttbl where RTRIM(CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)) = '" + patientname + "'", connection);
                    MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                    while (dataReader1.Read())
                    {
                        patientno = dataReader1.GetInt32("Patient_No");
                    }
                    connection.Close();

                    connection.Open();
                    MySqlCommand cmd3 = new MySqlCommand("SELECT Employee_Patient_No from employee_patienttbl where Patient_No = '" + patientno + "' and Employee_No = '" + employeeno + "'", connection);
                    MySqlDataReader dataReader3 = cmd3.ExecuteReader();
                    while (dataReader3.Read())
                    {
                        emp_patientno = dataReader3.GetInt32("Employee_Patient_No");
                        checker = true;
                    }
                    connection.Close();

                    if (checker == false)
                    {
                        emp_patientno = GetEmployeePatientNo();
                        connection.Open();
                        MySqlCommand cmd4 = new MySqlCommand("INSERT into employee_patienttbl values ('" + emp_patientno + "','" + patientno + "','" + employeeno + "')", connection);
                        cmd4.ExecuteNonQuery();
                        connection.Close();
                    }
                    if (emp_status == "Available")
                    {
                        waitlist_stat = "On Going";
                        connection.Open();
                        MySqlCommand cmd5 = new MySqlCommand("UPDATE employeetbl set Employee_Availability = 'Not Available' where Employee_No = '"+employee+"'", connection);
                        cmd5.ExecuteNonQuery();
                        connection.Close();
                    }
                    else
                    {
                        waitlist_stat = "Not Started";
                    }
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand("INSERT into patient_waitlisttbl(Employee_Patient_No,Waiting_For,Waiting_Status) values ('" + emp_patientno + "','Consultation','"+waitlist_stat+"')", connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception me)
                {
                    connection.Close();
                    MessageBox.Show(me.Message);
                }
                no = no + 1;
                dataGridView7.Rows.Add(no, patientname, "Consultation");
                button10.Enabled = true;
                dataGridView6.Enabled = true;
                dataGridView7.Enabled = true;
                button13.Enabled = true;
                textBox7.Text = "";
                label14.Text = "";
                errorProvider.SetError(textBox7, string.Empty);
                panel2.Hide();
            }
        }
        private void bunifuThinButton23_Click(object sender, EventArgs e)
        {
            button10.Enabled = true;
            dataGridView6.Enabled = true;
            dataGridView7.Enabled = true;
            button13.Enabled = true;
            label14.Text = "";
            errorProvider.SetError(textBox7, string.Empty);
            textBox7.Text = "";
            panel2.Hide();
        }
        public void GetConsultation(int patientno)
        {
            bunifuCustomDataGrid1.Rows.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from patient_demtbl where Patient_No = '" + patientno + "'", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    
                    bunifuCustomDataGrid1.Rows.Add(dataReader.GetString("Consult_Date"),dataReader.GetString("Dem_Allergies"),dataReader.GetString("Dem_Frownlines"),dataReader.GetString("Dem_Finelines"),dataReader.GetString("Dem_Wrinkles"),dataReader.GetString("Dem_Sagging"),dataReader.GetString("Dem_Reco"));
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }
        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            dataGridView2.Enabled = false;
            dataGridView1.Enabled = false;
            bunifuThinButton21.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            int row = dataGridView1.CurrentCell.RowIndex;
            int patientno = Convert.ToInt32(dataGridView1.Rows[row].Cells[0].Value);
            consultantschedPanel.Visible = false;
            consultationTransition.ShowSync(consultationPanel);
            
            GetConsultation(patientno);
            consultationPanel.Show();
            consultationPanel.BringToFront();
        }

        private void label20_Click(object sender, EventArgs e)
        {
            dataGridView2.Enabled = true;
            dataGridView1.Enabled = true;
            bunifuThinButton21.Enabled = true;
            textBox5.Enabled = true;
            textBox6.Enabled = true;
            consultationPanel.Hide();
        }

        private void dataGridView5_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView5.IsCurrentCellDirty)
            {
                // This fires the cell value changed handler below
                dataGridView5.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dateTimePicker4_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            GetSelectedAppoint();
        }
        public void GetSelectedAppoint()
        {
            dataGridView8.Rows.Clear();
            string date = dateTimePicker4.Value.ToString("yyyy-MM-dd");
            dataGridView8.ClearSelection();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT *,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from appointmenttbl a, employee_appointmenttbl ea, employee_patienttbl ep,servicetbl s,patienttbl p where Appointment_Date = '" + date + "' and ea.Appointment_No = a.Appointment_No and ep.Employee_Patient_No = ea.Employee_Patient_No and a.Service_No = s.Service_No and ep.Patient_No = p.Patient_No and a.Appointment_Status = 'Not Started' group by a.Appointment_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView8.ClearSelection();
                    dataGridView8.Rows.Add(dataReader.GetInt32("Appointment_No"), dataReader.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)"), dataReader.GetString("Service_Name"), dataReader.GetString("Appointment_StartTime"), dataReader.GetString("Appointment_EndTime"), dataReader.GetString("Appointment_Status"));
                }
                connection.Close();
                dataGridView8.ClearSelection();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }
        public void GetSearchedAppoint(string name)
        {
            dataGridView8.Rows.Clear();
        
            dataGridView8.ClearSelection();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT *,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from appointmenttbl a, employee_appointmenttbl ea, employee_patienttbl ep,servicetbl s,patienttbl p where CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) LIKE '%"+name+"%' and ea.Appointment_No = a.Appointment_No and ep.Employee_Patient_No = ea.Employee_Patient_No and a.Service_No = s.Service_No and ep.Patient_No = p.Patient_No and a.Appointment_Status = 'Not Started' group by a.Appointment_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView8.ClearSelection();
                    dataGridView8.Rows.Add(dataReader.GetInt32("Appointment_No"), dataReader.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)"), dataReader.GetString("Service_Name"), dataReader.GetString("Appointment_StartTime"), dataReader.GetString("Appointment_EndTime"), dataReader.GetString("Appointment_Status"));
                }
                connection.Close();
                dataGridView8.ClearSelection();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }
        private void bunifuThinButton24_Click(object sender, EventArgs e)
        {
            dataGridView8.Rows.Clear();
            panel8.Visible = false;
            waitlistTransition.ShowSync(panel8);
            panel8.BringToFront();
            panel8.Show();
        }

        private void label29_Click(object sender, EventArgs e)
        {
            panel8.Hide();
        }

        private void dataGridView8_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int appointno = Convert.ToInt32(dataGridView8.Rows[e.RowIndex].Cells[0].Value);
            if (e.ColumnIndex == dataGridView8.Columns[6].Index)
            {
                DialogResult dr = MessageBox.Show("Do you really want to cancel?", "Wait", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        connection.Open();
                        MySqlCommand cmd = new MySqlCommand("update appointmenttbl set Appointment_Status = 'Cancelled' where Appointment_No = '"+appointno+"'", connection);
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                    catch (Exception me)
                    {
                        connection.Close();
                        MessageBox.Show(me.Message);
                    }
                    MessageBox.Show("Appointment Cancelled");
                }
            }
        }

        private void bunifuMetroTextbox1_OnValueChanged(object sender, EventArgs e)
        {
            string name = bunifuMetroTextbox1.Text.Trim();
            GetSearchedAppoint(name);
        }
    }
}
