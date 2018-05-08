using Bunifu.Framework.UI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SWSFCSMPIWBC
{
    public partial class HomePage : Form
    {
        static string connectionString =
       System.Configuration.ConfigurationManager.
       ConnectionStrings["SWSFCSMPIWBC.Properties.Settings.slimmersdbConnectionString"].ConnectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);
        public HomePage()
        {

            InitializeComponent();
            //this.Location = new Point(0, 0);
            //maintenanceUC1.SendToBack();
            //this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            header.BringToFront();
            
            //patientsUC1.Visible = false;
            
            // profileUC1.Visible = false;
            //patientsUC1.Visible = false;
            panel1.Visible = true;
            panel1.BringToFront();
            Timer timer = new Timer();
            timer.Interval = 5000; // 5 secs
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
            initTime();
            initNotif();

            CheckNotification();
            
        }



        public event EventHandler CloseButtonClicked;
        void change_OkayButtonClicked(object sender, EventArgs e)
        {
            string user = label15.Text;
            regUC reg = new regUC(user);
            this.Controls.Add(reg);
            reg.userLabel = user;
            reg.Location = new Point(0, 55);
            reg.Size = new Size(1366, 713);
            reg.button18.Hide();
            reg.button17.Location = new Point(0, 192);
            reg.CloseButtonClicked += new System.EventHandler(change_AppointmentClicked);
            reg.ParentForm = this;
            reg.panel25.Show();
            reg.panel31.Hide();
            reg.panel34.Hide();
            reg.comboBox3.Enabled = false;
            reg.comboBox3.Text = this.Patient;
            reg.label4.Text = this.PatientNo.ToString();
            reg.Show();
            reg.BringToFront();
            
            //patientsUC1.Visible = false;
            
        }

        void change_AppointmentClicked(object sender, EventArgs e)
        {
            paymentUC paymentUC1 = new paymentUC();
            int month = Convert.ToInt32(DateTime.Now.ToString("MM"));
            int day = Convert.ToInt32(DateTime.Now.ToString("dd"));
            int year = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
            int startmonth = 0, startday = 0, startyear = 0, endmonth = 0, endday = 0, endyear = 0;
            int discount = 0;
            decimal subtotal = 0;
            decimal discounted = 0;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT *,YEAR(Promo_Start),MONTH(Promo_Start),DAY(Promo_Start),YEAR(Promo_End),MONTH(Promo_End),DAY(Promo_End) from service_promotbl sp, discount_servicestbl ds,servicetbl s where sp.Promo_No = ds.Promo_No and ds.Service_No = s.Service_No and s.Service_Name = '"+this.Service+"'", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    startmonth = dataReader.GetInt32("MONTH(Promo_Start)");
                    startday = dataReader.GetInt32("DAY(Promo_Start)");
                    startyear = dataReader.GetInt32("YEAR(Promo_Start)");
                    endmonth = dataReader.GetInt32("MONTH(Promo_End)");
                    endday = dataReader.GetInt32("DAY(Promo_End)");
                    endyear = dataReader.GetInt32("YEAR(Promo_End)");
                    if ((year == startyear && month == startmonth && day >= startday) && ((year == endyear && month == endmonth && day <= endday) || (year == endyear && endmonth > month)))
                    {
                        discount = dataReader.GetInt32("Discount_Rate");
                    }
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            decimal total =0;
            discounted = (this.Fee * discount) / 100;
            subtotal = this.Fee - discounted;

            paymentUC1.label6.Text = "Availed Service";
            paymentUC1.comboBox3.Text = this.Patient;
            paymentUC1.panel2.Hide();
            paymentUC1.panel3.Show();
            paymentUC1.panel8.Hide();
            paymentUC1.dataGridView1.Rows.Add(this.Service, "Service", this.Fee, "0",discount,subtotal);
            int rows = 0;
            rows = paymentUC1.dataGridView1.Rows.Count;
            for (int i = 0; i < rows; i++)
            {
                total += Convert.ToDecimal(paymentUC1.dataGridView1.Rows[i].Cells[5].Value);
            }
            paymentUC1.lblTotal.Text = total.ToString();
            paymentUC1.comboBox3.Enabled = false;
            paymentUC1.comboBox2.Enabled = false;
            paymentUC1.comboBox5.Enabled = false;
            paymentUC1.comboBox6.Enabled = false;
            paymentUC1.button10.Visible = false;
            paymentUC1.button13.Visible = false;
            paymentUC1.button14.Visible = false;
            paymentUC1.button15.Enabled = true;
            paymentUC1.label55.Text = this.AppointNo.ToString();
            paymentUC1.button15.BringToFront();
            paymentUC1.btnFromAppointment.Show();
            paymentUC1.btnFirstPay.Hide();
            paymentUC1.btnSecondPay.Hide();
            this.Controls.Add(paymentUC1);
            paymentUC1.ParentForm = this;
            paymentUC1.Size = new Size(1366, 717);
            paymentUC1.Location = new Point(0, 55);
            paymentUC1.BringToFront();
            paymentUC1.Show();
            

            //payment.label15.Text = user;
            //patientsUC1.Visible = false;

        }
        public HomePage(string label)
        {
            this.Username = label;
            InitializeComponent();
            label15.Text = this.Username;
            //this.Location = new Point(0, 0);
            //maintenanceUC1.SendToBack();
            //this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            header.BringToFront();
            dashboardUC dash = new dashboardUC(this.Username);
            this.Controls.Add(dash);
            dash.Show();
            dash.BringToFront();
            dash.ParentForm = this;
            dash.CloseButtonClicked += new System.EventHandler(change_OkayButtonClicked);
            dash.Location = new Point(221, 55);
            dash.Size = new Size(1145, 713);
            
            //patientsUC1.Visible = false;
            
            // profileUC1.Visible = false;
            //patientsUC1.Visible = false;
            panel1.Visible = true;
            panel1.BringToFront();
            Timer timer = new Timer();
            timer.Interval = (1000); // 5 secs
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
            initTime();
            initNotif();

            CheckNotification();
            
        }
        public string Username
        {
            get;
            set;
        }
        public string Patient
        {
            get;
            set;
        }
        public int PatientNo
        {
            get;
            set;
        }
        public string Service
        {
            get;
            set;
        }
        public decimal Fee
        {
            get;
            set;
        }
        public int AppointNo
        {
            get;
            set;
        }
        public void GetEmployeeWithoutUser()
        {

        }
        public void CheckUserAccount()
        {
            bool check = false;
            string user = "", pass = "", cpass = "", curruser = "", currpass = "";
            try
            {
                user = bunifuMetroTextbox6.Text.Trim();
                pass = bunifuMetroTextbox5.Text.Trim();
                cpass = bunifuMetroTextbox4.Text.Trim();
            }
            catch (Exception)
            {
            }
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from accounttbl where Username = '" + user + "'", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    curruser = dataReader.GetString("Username");
                    currpass = dataReader.GetString("Password");
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            if (string.IsNullOrEmpty(user))
            {
                bunifuImageButton3.Enabled = false;
            }
            else
            {
                if (pass == currpass)
                {
                    bunifuImageButton3.Enabled = false;
                    label13.Text = "";
                    bunifuMetroTextbox6.BorderColorFocused = Color.Blue;
                    bunifuMetroTextbox6.BorderColorIdle = Color.Black;
                    bunifuMetroTextbox6.BorderColorMouseHover = Color.Blue;
                }
                else
                {
                    try
                    {
                        connection.Open();
                        MySqlCommand cmd1 = new MySqlCommand("SELECT * from accounttbl where Username <> '" + curruser + "' and Username = '" + user + "'", connection);
                        MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                        while (dataReader1.Read())
                        {
                            check = true;
                            break;
                        }
                        connection.Close();
                    }
                    catch (Exception me)
                    {
                        connection.Close();
                        MessageBox.Show(me.Message);
                    }
                    if (check)
                    {
                        label16.Text = "Username already exists";
                        bunifuMetroTextbox6.BorderColorFocused = Color.Red;
                        bunifuMetroTextbox6.BorderColorIdle = Color.Red;
                        bunifuMetroTextbox6.BorderColorMouseHover = Color.Red;
                        bunifuImageButton3.Enabled = false;
                    }
                    else
                    {
                        label16.Text = "";
                        bunifuMetroTextbox6.BorderColorFocused = Color.Blue;
                        bunifuMetroTextbox6.BorderColorIdle = Color.Black;
                        bunifuMetroTextbox6.BorderColorMouseHover = Color.Blue;
                        if (string.IsNullOrEmpty(pass))
                        {
                            bunifuImageButton3.Enabled = false;
                        }
                        else
                        {
                            if (pass != cpass)
                            {
                                label13.Text = "Password mismatch";
                                bunifuMetroTextbox4.BorderColorFocused = Color.Red;
                                bunifuMetroTextbox4.BorderColorIdle = Color.Red;
                                bunifuMetroTextbox4.BorderColorMouseHover = Color.Red;
                                bunifuMetroTextbox5.BorderColorFocused = Color.Red;
                                bunifuMetroTextbox5.BorderColorIdle = Color.Red;
                                bunifuMetroTextbox5.BorderColorMouseHover = Color.Red;
                            }
                            else
                            {
                                label13.Text = "";
                                bunifuMetroTextbox4.BorderColorFocused = Color.Blue;
                                bunifuMetroTextbox4.BorderColorIdle = Color.Black;
                                bunifuMetroTextbox4.BorderColorMouseHover = Color.Blue;
                                bunifuMetroTextbox5.BorderColorFocused = Color.Blue;
                                bunifuMetroTextbox5.BorderColorIdle = Color.Black;
                                bunifuMetroTextbox5.BorderColorMouseHover = Color.Blue;
                                bunifuImageButton3.Enabled = true;
                            }
                        }
                    }
                }
            }

        }
        public void CheckNotification()
        {
            smUC smUC1 = new smUC();
            paymentUC paymentUC1 = new paymentUC();
            allinventoryUC allinventoryUC1 = new allinventoryUC();
            string user = "";
            int schedctr = 0, critlvlctr = 0, balancectr = 0, donectr = 0, totalctr = 0;
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string time = DateTime.Now.ToString("hh:mm:ss tt");
            int checkhour = Convert.ToInt32(time.Substring(0, 2));
            int checkmin = Convert.ToInt32(time.Substring(3, 2));
            string checkampm = time.Substring(9, 2);
            int hour = 0, min = 0, endhour = 0, endmin = 0, qty = 0, crit = 0, j = 0, ctr = 0, checkappointmentno = 0;
            string ampm = "", endampm = "", empname = "";
            bool check = false, checker = false, checks = false, checking = false;
            int[] appointmentno = new int[500];
            BunifuFlatButton btnDoneAppoint = new BunifuFlatButton();
            BunifuFlatButton btnBalance = new BunifuFlatButton();
            BunifuFlatButton btnCritLvl = new BunifuFlatButton();
            bool upAppoint = checkUpAppoint();
            bool doneAppoint = checkDoneAppoint();
            if (upAppoint)
            {
                Label lbl = new Label();
                lbl.Text = "These employee(s) have an appointment";
                lbl.Font = new Font("Arial", 8);
                lbl.Size = new Size(200, 15);
                lbl.Margin = new Padding(3, 3, 3, 3);
                notificationPanel1.Controls.Add(lbl);
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT *,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from appointmenttbl a, employee_appointmenttbl ea, employee_patienttbl ep, employeetbl e where Appointment_Date = '" + date + "' and a.Appointment_Status = 'Not Started' and a.Appointment_No = ea.Appointment_No and ea.Employee_Patient_No = ep.Employee_Patient_No and ep.Employee_No = e.Employee_No", connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {

                        hour = Convert.ToInt32(dataReader.GetString("Appointment_StartTime").Substring(0, 2));
                        min = Convert.ToInt32(dataReader.GetString("Appointment_StartTime").Substring(3, 2));
                        ampm = dataReader.GetString("Appointment_StartTime").Substring(6, 2);
                        endhour = Convert.ToInt32(dataReader.GetString("Appointment_EndTime").Substring(0, 2));
                        endmin = Convert.ToInt32(dataReader.GetString("Appointment_EndTime").Substring(3, 2));
                        endampm = dataReader.GetString("Appointment_EndTime").Substring(6, 2);
                        empname = dataReader.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)");
                        if (((checkhour == hour && checkmin >= min && string.Equals(ampm, checkampm, StringComparison.OrdinalIgnoreCase)) || (checkhour > hour && string.Equals(ampm, checkampm, StringComparison.OrdinalIgnoreCase))) && ((checkhour < endhour && (!string.Equals(endampm, checkampm, StringComparison.OrdinalIgnoreCase) || string.Equals(endampm, checkampm, StringComparison.OrdinalIgnoreCase))) || (checkhour == endhour && checkmin <= endmin && string.Equals(endampm, checkampm, StringComparison.OrdinalIgnoreCase))))
                        {

                            schedctr++;
                            BunifuFlatButton btnUpAppoint = new BunifuFlatButton();
                            btnUpAppoint.Size = new Size(200, 50);
                            btnUpAppoint.Iconimage = null;
                            btnUpAppoint.Margin = new Padding(0);
                            btnUpAppoint.Text = empname;

                            btnUpAppoint.Click += delegate
                            {

                                string newempname = btnUpAppoint.Text;

                                int x = 0, hour1 = 0, min1 = 0, endhour1 = 0, endmin1 = 0;
                                string ampm1 = "", endampm1 = "", empname1 = "";
                                string datenow = DateTime.Now.ToString("yyyy-MM-dd");
                                smUC1.dataGridView4.Rows.Clear();
                                smUC1.dataGridView4.ClearSelection();
                                try
                                {
                                    connection.Open();
                                    MySqlCommand cmd1 = new MySqlCommand("Select *,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit),CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from appointmenttbl a, employee_appointmenttbl ea, employeetbl e, employee_patienttbl ep,patienttbl p,servicetbl s where Appointment_Date = '" + datenow + "' and CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) = '" + newempname + "' and ea.Appointment_No = a.Appointment_No and ea.Employee_Patient_No = ep.Employee_Patient_No and ep.Employee_No = e.Employee_No and ep.Patient_No = p.Patient_No and a.Service_No = s.Service_No", connection);
                                    MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                                    while (dataReader1.Read())
                                    {
                                        smUC1.dataGridView4.ClearSelection();
                                        hour1 = Convert.ToInt32(dataReader1.GetString("Appointment_StartTime").Substring(0, 2));
                                        min1 = Convert.ToInt32(dataReader1.GetString("Appointment_StartTime").Substring(3, 2));
                                        ampm1 = dataReader1.GetString("Appointment_StartTime").Substring(6, 2);
                                        endhour1 = Convert.ToInt32(dataReader1.GetString("Appointment_EndTime").Substring(0, 2));
                                        endmin1 = Convert.ToInt32(dataReader1.GetString("Appointment_EndTime").Substring(3, 2));
                                        endampm1 = dataReader1.GetString("Appointment_EndTime").Substring(6, 2);
                                        empname1 = dataReader1.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)");

                                        smUC1.dataGridView4.Rows.Add(dataReader1.GetInt32("Appointment_No"), dataReader1.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)"), dataReader1.GetString("Service_Name"), dataReader1.GetString("Appointment_StartTime"), dataReader1.GetString("Appointment_EndTime"), dataReader1.GetString("Appointment_Status"));
                                        if (((checkhour == hour1 && checkmin >= min1 && string.Equals(ampm1, checkampm, StringComparison.OrdinalIgnoreCase)) || (checkhour > hour1 && string.Equals(ampm1, checkampm, StringComparison.OrdinalIgnoreCase))) && ((checkhour < endhour1 && (!string.Equals(endampm1, checkampm, StringComparison.OrdinalIgnoreCase) || string.Equals(endampm1, checkampm, StringComparison.OrdinalIgnoreCase))) || (checkhour == endhour1 && checkmin <= endmin1 && string.Equals(endampm1, checkampm, StringComparison.OrdinalIgnoreCase))))
                                        {
                                            checkappointmentno = dataReader1.GetInt32("Appointment_No");
                                            if (checkappointmentno == Convert.ToInt32(smUC1.dataGridView4.Rows[x].Cells[0].Value))
                                            {
                                                smUC1.dataGridView4.Rows[x].Cells[0].Style.BackColor = Color.Salmon;
                                            }
                                        }
                                        x++;
                                    }
                                    connection.Close();
                                }
                                catch (Exception me)
                                {
                                    connection.Close();
                                    MessageBox.Show(me.Message);
                                }
                                for (int row = 0; row < smUC1.dataGridView4.Rows.Count; row++)
                                {
                                    if (smUC1.dataGridView4.Rows[row].Cells[5].Value.ToString() == "Not Started")
                                    {
                                        smUC1.dataGridView4.Rows[row].Cells[5].Style.BackColor = Color.Bisque;
                                        DataGridViewTextBoxCell txtCell = new DataGridViewTextBoxCell();
                                        smUC1.dataGridView4.Rows[row].Cells[8] = txtCell;
                                        smUC1.dataGridView4.Rows[row].Cells[8].ReadOnly = true;
                                    }
                                    else if (smUC1.dataGridView4.Rows[row].Cells[5].Value.ToString() == "On Going")
                                    {
                                        smUC1.dataGridView4.Rows[row].Cells[5].Style.BackColor = Color.DeepSkyBlue;
                                        DataGridViewTextBoxCell txtCell = new DataGridViewTextBoxCell();
                                        smUC1.dataGridView4.Rows[row].Cells[6] = txtCell;
                                        smUC1.dataGridView4.Rows[row].Cells[6].ReadOnly = true;
                                    }
                                    else if (smUC1.dataGridView4.Rows[row].Cells[5].Value.ToString() == "Cancelled")
                                    {
                                        smUC1.dataGridView4.Rows[row].Cells[5].Style.BackColor = Color.LightCoral;
                                    }
                                    else if (smUC1.dataGridView4.Rows[row].Cells[5].Value.ToString() == "Done")
                                    {
                                        smUC1.dataGridView4.Rows[row].Cells[5].Style.BackColor = Color.MediumSeaGreen;
                                        DataGridViewTextBoxCell txtCell = new DataGridViewTextBoxCell();
                                        DataGridViewTextBoxCell txtCell1 = new DataGridViewTextBoxCell();
                                        DataGridViewTextBoxCell txtCell2 = new DataGridViewTextBoxCell();
                                        smUC1.dataGridView4.Rows[row].Cells[6] = txtCell;
                                        smUC1.dataGridView4.Rows[row].Cells[6].ReadOnly = true;
                                        smUC1.dataGridView4.Rows[row].Cells[7] = txtCell1;
                                        smUC1.dataGridView4.Rows[row].Cells[7].ReadOnly = true;
                                        smUC1.dataGridView4.Rows[row].Cells[8] = txtCell2;
                                        smUC1.dataGridView4.Rows[row].Cells[8].ReadOnly = true;
                                    }

                                }
                                smUC1.label53.Text = newempname;
                                smUC1.servicemonitoringPanel.Hide();
                                smUC1.consultantschedPanel.Hide();
                                smUC1.empButtons.Hide();
                                smUC1.schedulePanel.Show();
                                this.Controls.Add(smUC1);
                                smUC1.ParentForm = this;
                                smUC1.Size = new Size(1366, 717);
                                smUC1.Location = new Point(0, 55);
                                smUC1.Show();
                                smUC1.BringToFront();

                            };

                            notificationPanel1.Controls.Add(btnUpAppoint);

                        }

                    }
                    connection.Close();
                }
                catch (Exception me)
                {
                    connection.Close();
                    MessageBox.Show(me.Message);
                }

            }

            if (doneAppoint)
            {
                Label lbl1 = new Label();
                lbl1.Text = "Employee(s) with done appointment";
                lbl1.Font = new Font("Arial", 8);
                lbl1.Size = new Size(200, 15);
                lbl1.Margin = new Padding(3, 3, 3, 3);
                notificationPanel1.Controls.Add(lbl1);
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT *,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from appointmenttbl a, employee_appointmenttbl ea, employee_patienttbl ep, employeetbl e where Appointment_Date = '" + date + "' and a.Appointment_Status = 'Not Started' and a.Appointment_No = ea.Appointment_No and ea.Employee_Patient_No = ep.Employee_Patient_No and ep.Employee_No = e.Employee_No", connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {

                        hour = Convert.ToInt32(dataReader.GetString("Appointment_StartTime").Substring(0, 2));
                        min = Convert.ToInt32(dataReader.GetString("Appointment_StartTime").Substring(3, 2));
                        ampm = dataReader.GetString("Appointment_StartTime").Substring(6, 2);
                        endhour = Convert.ToInt32(dataReader.GetString("Appointment_EndTime").Substring(0, 2));
                        endmin = Convert.ToInt32(dataReader.GetString("Appointment_EndTime").Substring(3, 2));
                        endampm = dataReader.GetString("Appointment_EndTime").Substring(6, 2);
                        empname = dataReader.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)");

                        if ((checkhour == endhour && checkmin >= endmin && string.Equals(endampm, checkampm, StringComparison.OrdinalIgnoreCase)) || (checkhour > endhour && string.Equals(endampm, checkampm, StringComparison.OrdinalIgnoreCase)))
                        {
                            donectr++;
                            empname = dataReader.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)");
                            btnDoneAppoint = new BunifuFlatButton();
                            btnDoneAppoint.Size = new Size(200, 50);
                            btnDoneAppoint.Iconimage = null;
                            btnDoneAppoint.Margin = new Padding(0);
                            btnDoneAppoint.Text = empname;
                            btnDoneAppoint.Click += delegate
                            {
                                
                                string newempname = btnDoneAppoint.Text;
                                //sm.label15.Text = user;
                                int x = 0, hour1 = 0, min1 = 0, endhour1 = 0, endmin1 = 0;
                                string ampm1 = "", endampm1 = "", empname1 = "";
                                string datenow = DateTime.Now.ToString("yyyy-MM-dd");
                                try
                                {
                                    connection.Open();
                                    MySqlCommand cmd2 = new MySqlCommand("Select *,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit),CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from appointmenttbl a, employee_appointmenttbl ea, employeetbl e, employee_patienttbl ep,patienttbl p,servicetbl s where Appointment_Date = '" + datenow + "' and CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) = '" + newempname + "' and ea.Appointment_No = a.Appointment_No and ea.Employee_Patient_No = ep.Employee_Patient_No and ep.Employee_No = e.Employee_No and ep.Patient_No = p.Patient_No and a.Service_No = s.Service_No", connection);
                                    MySqlDataReader dataReader2 = cmd2.ExecuteReader();
                                    while (dataReader2.Read())
                                    {
                                        smUC1.dataGridView4.ClearSelection();
                                        hour1 = Convert.ToInt32(dataReader2.GetString("Appointment_StartTime").Substring(0, 2));
                                        min1 = Convert.ToInt32(dataReader2.GetString("Appointment_StartTime").Substring(3, 2));
                                        ampm1 = dataReader2.GetString("Appointment_StartTime").Substring(6, 2);
                                        endhour1 = Convert.ToInt32(dataReader2.GetString("Appointment_EndTime").Substring(0, 2));
                                        endmin1 = Convert.ToInt32(dataReader2.GetString("Appointment_EndTime").Substring(3, 2));
                                        endampm1 = dataReader2.GetString("Appointment_EndTime").Substring(6, 2);
                                        empname1 = dataReader2.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)");

                                        smUC1.dataGridView4.Rows.Add(dataReader2.GetInt32("Appointment_No"), dataReader2.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)"), dataReader2.GetString("Service_Name"), dataReader2.GetString("Appointment_StartTime"), dataReader2.GetString("Appointment_EndTime"), dataReader2.GetString("Appointment_Status"));
                                        if ((checkhour == endhour1 && checkmin >= endmin1 && string.Equals(endampm1, checkampm, StringComparison.OrdinalIgnoreCase)) || (checkhour > endhour1 && string.Equals(endampm1, checkampm, StringComparison.OrdinalIgnoreCase)))
                                        {
                                            checkappointmentno = dataReader2.GetInt32("Appointment_No");
                                            if (checkappointmentno == Convert.ToInt32(smUC1.dataGridView4.Rows[x].Cells[0].Value))
                                            {
                                                smUC1.dataGridView4.Rows[x].Cells[0].Style.BackColor = Color.Salmon;
                                            }
                                        }
                                        x++;
                                    }
                                    connection.Close();
                                }
                                catch (Exception me)
                                {
                                    connection.Close();
                                    MessageBox.Show(me.Message);
                                }
                                for (int row = 0; row < smUC1.dataGridView4.Rows.Count; row++)
                                {
                                    if (smUC1.dataGridView4.Rows[row].Cells[5].Value.ToString() == "Not Started")
                                    {
                                        smUC1.dataGridView4.Rows[row].Cells[5].Style.BackColor = Color.Bisque;
                                    }
                                    else if (smUC1.dataGridView4.Rows[row].Cells[5].Value.ToString() == "On Going")
                                    {
                                        smUC1.dataGridView4.Rows[row].Cells[5].Style.BackColor = Color.DeepSkyBlue;
                                    }
                                    else if (smUC1.dataGridView4.Rows[row].Cells[5].Value.ToString() == "Cancelled")
                                    {
                                        smUC1.dataGridView4.Rows[row].Cells[5].Style.BackColor = Color.LightCoral;
                                    }
                                    else if (smUC1.dataGridView4.Rows[row].Cells[5].Value.ToString() == "Done")
                                    {
                                        smUC1.dataGridView4.Rows[row].Cells[5].Style.BackColor = Color.MediumSeaGreen;
                                    }

                                }
                                smUC1.label53.Text = newempname;
                                smUC1.servicemonitoringPanel.Hide();
                                smUC1.consultantschedPanel.Hide();
                                smUC1.empButtons.Hide();
                                smUC1.schedulePanel.Show();
                                this.Controls.Add(smUC1);
                                smUC1.ParentForm = this;
                                smUC1.Size = new Size(1366, 717);
                                smUC1.Location = new Point(0,55);
                                smUC1.Show();
                                smUC1.BringToFront();
                            };
                            notificationPanel1.Controls.Add(btnDoneAppoint);
                        }
                    }
                    connection.Close();
                }
                catch (Exception me)
                {
                    connection.Close();
                    MessageBox.Show(me.Message);
                }
            }
            try
            {
                connection.Open();
                MySqlCommand cmd3 = new MySqlCommand("SELECT Billing_No,Billing_Date,Total_Bill,Balance,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit),b.Patient_No from billingtbl b, patienttbl p where p.Patient_No = b.Patient_No and b.Balance > 0 and Billing_No in (SELECT MAX(Billing_No) from billingtbl group by Patient_No)", connection);
                MySqlDataReader dataReader3 = cmd3.ExecuteReader();
                while (dataReader3.Read())
                {
                    balancectr++;
                    checks = true;
                }
                connection.Close();

                connection.Open();
                string query2 = ("Select Total_Quantity, Critical_Level from product_inventorytbl pi, product_prodtypetbl ppt where ppt.Product_ProdType_No = pi.Product_ProdType_No");
                MySqlCommand cmd4 = new MySqlCommand(query2, connection);
                MySqlDataReader dataReader4 = cmd4.ExecuteReader();
                while (dataReader4.Read())
                {
                    qty = dataReader4.GetInt32("Total_Quantity");
                    crit = dataReader4.GetInt32("Critical_Level");
                    if (qty <= crit)
                    {
                        critlvlctr++;
                        checking = true;
                    }
                    j++;
                }
                connection.Close();


                if (checks)
                {
                    btnBalance = new BunifuFlatButton();
                    btnBalance.Size = new Size(200, 50);
                    btnBalance.Iconimage = null;
                    btnBalance.Margin = new Padding(0);
                    btnBalance.Text = "(" + balancectr + ")" + "  There's a patient with balance";
                    btnBalance.Click += delegate
                    {
                        
                        paymentUC1.panel2.BringToFront();
                        paymentUC1.panel8.SendToBack();
                        paymentUC1.panel3.SendToBack();
                        //pay.label15.Text = user;
                        
                        this.Controls.Add(paymentUC1);
                        paymentUC1.ParentForm = this;
                        paymentUC1.Size = new Size(1366, 717);
                        paymentUC1.Location = new Point(0, 55);
                        paymentUC1.BringToFront();
                        paymentUC1.Show();
                    };
                    notificationPanel1.Controls.Add(btnBalance);
                }

                if (checking)
                {
                    btnCritLvl = new BunifuFlatButton();
                    btnCritLvl.Size = new Size(200, 50);
                    btnCritLvl.Iconimage = null;
                    btnCritLvl.Margin = new Padding(0);
                    btnCritLvl.Text = "(" + critlvlctr + ")" + "  There's a product below critical level!";
                    btnCritLvl.Click += delegate
                    {

                        allinventoryUC1.inventoryUC1.BringToFront();
                        allinventoryUC1.orderUC1.SendToBack();
                        allinventoryUC1.pullOutProductsUC1.SendToBack();
                        allinventoryUC1.adddInventoryUC1.SendToBack();
                        //i.label15.Text = user;
                        
                        this.Controls.Add(allinventoryUC1);
                        allinventoryUC1.ParentForm = this;
                        allinventoryUC1.Size = new Size(1366, 717);
                        allinventoryUC1.Location = new Point(0, 55);
                        allinventoryUC1.Show();
                        allinventoryUC1.BringToFront();

                    };
                    notificationPanel1.Controls.Add(btnCritLvl);
                }
                totalctr = schedctr + critlvlctr + balancectr + donectr;
                if (totalctr != 0)
                {
                    label1.Visible = true;
                }
                else
                {
                    label1.Visible = false;
                }
                label1.Text = totalctr.ToString();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }
        public Boolean checkUpAppoint()
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string time = DateTime.Now.ToString("hh:mm:ss tt");
            int checkhour = Convert.ToInt32(time.Substring(0, 2));
            int checkmin = Convert.ToInt32(time.Substring(3, 2));
            string checkampm = time.Substring(9, 2);
            int hour = 0, min = 0, endhour = 0, endmin = 0;
            string ampm = "", endampm = "", empname = "";
            bool check = false;
            try
            {
                connection.Open();
                MySqlCommand cmd5 = new MySqlCommand("SELECT *,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from appointmenttbl a, employee_appointmenttbl ea, employee_patienttbl ep, employeetbl e where Appointment_Date = '" + date + "' and a.Appointment_Status = 'Not Started' and a.Appointment_No = ea.Appointment_No and ea.Employee_Patient_No = ep.Employee_Patient_No and ep.Employee_No = e.Employee_No", connection);
                MySqlDataReader dataReader5 = cmd5.ExecuteReader();
                while (dataReader5.Read())
                {

                    hour = Convert.ToInt32(dataReader5.GetString("Appointment_StartTime").Substring(0, 2));
                    min = Convert.ToInt32(dataReader5.GetString("Appointment_StartTime").Substring(3, 2));
                    ampm = dataReader5.GetString("Appointment_StartTime").Substring(6, 2);
                    endhour = Convert.ToInt32(dataReader5.GetString("Appointment_EndTime").Substring(0, 2));
                    endmin = Convert.ToInt32(dataReader5.GetString("Appointment_EndTime").Substring(3, 2));
                    endampm = dataReader5.GetString("Appointment_EndTime").Substring(6, 2);
                    empname = dataReader5.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)");
                    if (((checkhour == hour && checkmin >= min && string.Equals(ampm, checkampm, StringComparison.OrdinalIgnoreCase)) || (checkhour > hour && string.Equals(ampm, checkampm, StringComparison.OrdinalIgnoreCase))) && ((checkhour < endhour && (!string.Equals(endampm, checkampm, StringComparison.OrdinalIgnoreCase) || string.Equals(endampm, checkampm, StringComparison.OrdinalIgnoreCase))) || (checkhour == endhour && checkmin <= endmin && string.Equals(endampm, checkampm, StringComparison.OrdinalIgnoreCase))))
                    {
                        check = true;
                    }
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            return check;
        }
        public Boolean checkDoneAppoint()
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string time = DateTime.Now.ToString("hh:mm:ss tt");
            int checkhour = Convert.ToInt32(time.Substring(0, 2));
            int checkmin = Convert.ToInt32(time.Substring(3, 2));
            string checkampm = time.Substring(9, 2);
            int hour = 0, min = 0, endhour = 0, endmin = 0;
            string ampm = "", endampm = "", empname = "";
            bool check = false;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT *,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from appointmenttbl a, employee_appointmenttbl ea, employee_patienttbl ep, employeetbl e where Appointment_Date = '" + date + "' and a.Appointment_Status = 'Not Started' and a.Appointment_No = ea.Appointment_No and ea.Employee_Patient_No = ep.Employee_Patient_No and ep.Employee_No = e.Employee_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {

                    hour = Convert.ToInt32(dataReader.GetString("Appointment_StartTime").Substring(0, 2));
                    min = Convert.ToInt32(dataReader.GetString("Appointment_StartTime").Substring(3, 2));
                    ampm = dataReader.GetString("Appointment_StartTime").Substring(6, 2);
                    endhour = Convert.ToInt32(dataReader.GetString("Appointment_EndTime").Substring(0, 2));
                    endmin = Convert.ToInt32(dataReader.GetString("Appointment_EndTime").Substring(3, 2));
                    endampm = dataReader.GetString("Appointment_EndTime").Substring(6, 2);
                    if ((checkhour == endhour && checkmin >= endmin && string.Equals(endampm, checkampm, StringComparison.OrdinalIgnoreCase)) || (checkhour > endhour && string.Equals(endampm, checkampm, StringComparison.OrdinalIgnoreCase)))
                    {
                        check = true;
                    }
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            return check;
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            
        }
        private void initTime()
        {
            Timer t = new System.Windows.Forms.Timer();
            t.Interval = 1000;
            t.Tick += new EventHandler(t_Tick);
            t.Enabled = true;
        }
        private void initNotif()
        {
            Timer t1 = new System.Windows.Forms.Timer();
            t1.Interval = 15000;
            t1.Tick += new EventHandler(t1_Tick);
            t1.Enabled = true;
        }
        void t1_Tick(object sender, EventArgs e)
        {
            notificationPanel1.Controls.Clear();
            CheckNotification();
        }
        void t_Tick(object sender, EventArgs e)
        {
            label23.Text = DateTime.Now.ToLongDateString();
            label14.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }

        int ctr = 0;
        private void button2_Click_1(object sender, EventArgs e)
        {
            notificationPanel.Visible = false;
            settings.BringToFront();
            settings.BackColor = Color.Transparent;
            panel2.SendToBack();
            ctr++;
            ctr1++;
            settingsTransition.ShowSync(settings);
            if (ctr % 2 == 0)
            {
                settings.Visible = false;
            }
            else
            {
                settings.Visible = true;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
            HomePage hp = new HomePage();
            hp.Hide();
        }



        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            GetEmployeeWithOutAccount();
            panel4.BringToFront();
            addUserTransition.ShowSync(panel4);
            ctr++;
            settings.Visible = false;
            panel4.Visible = true;
            label4.Visible = true;
            bunifuMetroTextbox3.Visible = true;
            label6.Visible = true;
            label8.Visible = true;
            comboBox1.Visible = true;
            label9.Visible = true;

        }

        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {

        }
        int ctr1 = 0;
        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            settings.Visible = false;
            notificationPanel.BringToFront();
            notificationPanel.BackColor = Color.Transparent;
            panel2.SendToBack();
            ctr1++;
            ctr++;
            notificationTransition.ShowSync(notificationPanel);
            if (ctr1 % 2 == 0)
            {
                notificationPanel.Visible = false;
            }
            else
            {
                notificationPanel.Visible = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
            bunifuMetroTextbox1.Text = "";
            bunifuMetroTextbox2.Text = "";
            bunifuMetroTextbox3.Text = "";
            label7.Text = "";
            label8.Text = "";
            label9.Text = "";
            label10.Text = "";
            bunifuMetroTextbox1.BorderColorIdle = Color.Black;
            bunifuMetroTextbox1.BorderColorMouseHover = Color.Blue;
            bunifuMetroTextbox2.BorderColorIdle = Color.Black;
            bunifuMetroTextbox2.BorderColorMouseHover = Color.Blue;
            bunifuMetroTextbox3.BorderColorIdle = Color.Black;
            bunifuMetroTextbox3.BorderColorMouseHover = Color.Blue;
        }
        private void mainteBtn_Click(object sender, EventArgs e)
        {
            maintenanceUC maintenanceUC1 = new maintenanceUC();
            this.Controls.Add(maintenanceUC1);
            maintenanceUC1.ParentForm = this;
            maintenanceUC1.Size = new Size(1366, 717);
            maintenanceUC1.Location = new Point(0, 55);
            maintenanceUC1.Visible = true;
            maintenanceUC1.BringToFront();

            maintenanceUC1.prodBtn.Textcolor = Color.FromArgb(4, 180, 253);
            maintenanceUC1.servicesBtn.Textcolor = Color.White;
            maintenanceUC1.machBtn.Textcolor = Color.White;
            maintenanceUC1.EmpBtn.Textcolor = Color.White;
            maintenanceUC1.button15.Textcolor = Color.White;

            maintenanceUC1.slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)maintenanceUC1.prodBtn).Top;
            maintenanceUC1.slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)maintenanceUC1.prodBtn).Height;
            
        }
        private void button2_Click(object sender, EventArgs e)
        {
            paymentUC paymentUC1 = new paymentUC();
            this.Controls.Add(paymentUC1);
            paymentUC1.ParentForm = this;
            paymentUC1.Size = new Size(1366, 717);
            paymentUC1.Location = new Point(0, 55);
            paymentUC1.Visible = true;
            paymentUC1.BringToFront();
            
            paymentUC1.panel2.Show();
            paymentUC1.panel3.Hide();
            paymentUC1.panel8.Hide();
            paymentUC1.comboBox6.Enabled = false;
            paymentUC1.button16.Hide();
            paymentUC1.button17.Hide();

            paymentUC1.button1.Textcolor = System.Drawing.Color.FromArgb(4, 180, 253);
            paymentUC1.button2.Textcolor = System.Drawing.Color.White;
            paymentUC1.button7.Textcolor = System.Drawing.Color.White;

            paymentUC1.slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)paymentUC1.button1).Top;
            paymentUC1.slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)paymentUC1.button1).Height;

        }



        private void button3_Click(object sender, EventArgs e)
        {
            allinventoryUC allinventoryUC1 = new allinventoryUC();
            this.Controls.Add(allinventoryUC1);
            allinventoryUC1.ParentForm = this;
            allinventoryUC1.Size = new Size(1366, 717);
            allinventoryUC1.Location = new Point(0, 55);
            allinventoryUC1.Visible = true;
            allinventoryUC1.BringToFront();
            allinventoryUC1.inventoryUC1.BringToFront();
            allinventoryUC1.inventoryUC1.Show();
            allinventoryUC1.orderUC1.SendToBack();
            allinventoryUC1.pullOutProductsUC1.SendToBack();
            allinventoryUC1.adddInventoryUC1.SendToBack();
            allinventoryUC1.pullOutRecordUC1.SendToBack();

            allinventoryUC1.inventoryBtn.Textcolor = Color.FromArgb(4, 180, 253);
            allinventoryUC1.requestBtn.Textcolor = Color.White;
            allinventoryUC1.addStocksBtn.Textcolor = Color.White;
            allinventoryUC1.pullOutBtn.Textcolor = Color.White;
            allinventoryUC1.pullOutRecordBtn.Textcolor = Color.White;
            allinventoryUC1.slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)allinventoryUC1.inventoryBtn).Top;
            allinventoryUC1.slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)allinventoryUC1.inventoryBtn).Height;
            allinventoryUC1.BringToFront();
        }

        private void button8_Click(object sender, EventArgs e)
        {

            string user = label15.Text;
            regUC reg = new regUC(user);
            this.Controls.Add(reg);
            reg.Location = new Point(0, 55);
            reg.Size = new Size(1366, 713);
            reg.ParentForm = this;
            reg.CloseButtonClicked += new System.EventHandler(change_AppointmentClicked);
            reg.userLabel = user;
            if (user == "admin")
            {
                reg.button17.Hide();
                reg.button17.Visible = false;
                reg.button17.Location = new Point(0, 192);
                reg.panel31.BringToFront();
                reg.panel34.SendToBack();
                reg.panel25.SendToBack();
            }
            else
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * from accounttbl a, employeetbl e, employee_positiontbl ep where a.Username = '" + user + "' and a.Employee_No = e.Employee_No and e.Employee_Position_No = ep.Employee_Position_No", connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        if (dataReader.GetString("Position_Name") == "Consultant")
                        {
                            reg.button18.Hide();
                            reg.button17.Location = new Point(0, 192);
                            reg.panel25.BringToFront();
                            reg.panel31.SendToBack();
                            reg.panel34.SendToBack();
                            reg.GetConsultPatient();
                        }
                        else if (dataReader.GetString("Position_Name") == "Receptionist")
                        {
                            reg.button17.Hide();
                            reg.button18.Location = new Point(0, 192);
                            reg.panel31.BringToFront();
                            reg.panel34.SendToBack();
                            reg.panel25.SendToBack();
                        }

                    }
                    connection.Close();
                }
                catch (Exception me)
                {
                    MessageBox.Show(me.Message);
                }
            }
            reg.Visible = true;
            reg.BringToFront();

            //this.Hide();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            smUC smUC1 = new smUC();
            this.Controls.Add(smUC1);
            smUC1.ParentForm = this;
            smUC1.Size = new Size(1366, 717);
            smUC1.Location = new Point(0, 55);
            smUC1.Visible = true;
            smUC1.BringToFront();
            smUC1.servicemonitoringPanel.Show();
            smUC1.schedulePanel.Hide();
            smUC1.consultantschedPanel.Hide();
            smUC1.empButtons.Hide();
            smUC1.emSchedPanel.Hide();
            smUC1.slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)smUC1.button4).Top;
            smUC1.slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)smUC1.button4).Height;

            smUC1.button4.Textcolor = Color.FromArgb(4, 180, 253);
            smUC1.button1.Textcolor = Color.White;
            smUC1.button14.Textcolor = Color.White;
            smUC1.button8.Textcolor = Color.White;
            //paymentUC1.Visible = false;
            //maintenanceUC1.Visible = false;
            //dashboardUC1.Visible = true;
            //allinventoryUC1.Visible = false;

        }
        
        private void button13_Click(object sender, EventArgs e)
        {
            //panel2.Show();
            //panel2.BringToFront();
            //GetAllPatients();
            string user = label15.Text;
            AllPatientsUC AllPatientsUC1 = new AllPatientsUC(user);
            this.Controls.Add(AllPatientsUC1);
            AllPatientsUC1.Show();
            AllPatientsUC1.BringToFront();
            AllPatientsUC1.ParentForm = this;
            AllPatientsUC1.Location = new Point(221, 55);
            AllPatientsUC1.Size = new Size(1138, 683);

        }
        public void GetAllPatients()
        {
            dataGridView1.Rows.Clear();
            try
            {
                connection.Open();
                string query = "SELECT *,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from patienttbl order by Patient_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView1.Rows.Add(dataReader.GetInt32("Patient_No"), dataReader.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)"), dataReader.GetString("Patient_Birthdate"), dataReader.GetString("Patient_ContactNo"), dataReader.GetString("Patient_Address"));
                }

                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
        }
        private void menu_Click(object sender, EventArgs e)
        {

            if (panel1.Width == 54)
            {
                //expand
                logo1.Visible = false;
                panel1.Visible = false;
                panel1.Width = 211;
                PanelTransition.ShowSync(panel1);
                LogoTransition.ShowSync(logo);
            }
            else
            {
                //minimize
                logo1.Visible = false;
                logo.Hide();
                panel1.Visible = false;
                panel1.Width = 54;
                Panel2Transition.ShowSync(panel1);
                Logo1Transition.ShowSync(logo1);

            }
        }

        private void menu_Click_1(object sender, EventArgs e)
        {
            if (panel1.Width == 54)
            {
                //expand
                logo1.Visible = false;
                panel1.Visible = false;
                panel1.Width = 211;
                PanelTransition.ShowSync(panel1);
                LogoTransition.ShowSync(logo);
            }
            else
            {
                //minimize
                logo1.Visible = false;
                logo.Hide();
                panel1.Visible = false;
                panel1.Width = 54;
                Panel2Transition.ShowSync(panel1);
                Logo1Transition.ShowSync(logo1);

            }
        }

        private void bunifuMetroTextbox6_Leave(object sender, EventArgs e)
        {
            CheckUserAccount();
        }

        private void bunifuMetroTextbox5_Leave(object sender, EventArgs e)
        {
            CheckUserAccount();
        }

        private void bunifuMetroTextbox4_Leave(object sender, EventArgs e)
        {
            CheckUserAccount();
        }

        private void bunifuMetroTextbox4_KeyUp(object sender, KeyEventArgs e)
        {
            CheckUserAccount();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            label19.Visible = false;
            bunifuMetroTextbox4.Visible = false;
            bunifuMetroTextbox5.Enabled = true;
            bunifuMetroTextbox6.Enabled = true;
            label19.Show();
            label13.Show();
            bunifuMetroTextbox4.Show();
            linkLabel1.Hide();
        }

        private void bunifuImageButton4_Click(object sender, EventArgs e)
        {
            bunifuMetroTextbox6.Text = "";
            bunifuMetroTextbox6.BorderColorFocused = Color.Blue;
            bunifuMetroTextbox6.BorderColorIdle = Color.Black;
            bunifuMetroTextbox6.BorderColorMouseHover = Color.Blue;
            label16.Text = "";
            label11.Text = "";
            bunifuMetroTextbox5.Text = "";
            bunifuMetroTextbox5.BorderColorFocused = Color.Blue;
            bunifuMetroTextbox5.BorderColorIdle = Color.Black;
            bunifuMetroTextbox5.BorderColorMouseHover = Color.Blue;
            label13.Text = "";
            bunifuMetroTextbox4.Text = "";
            bunifuMetroTextbox4.BorderColorFocused = Color.Blue;
            bunifuMetroTextbox4.BorderColorIdle = Color.Black;
            bunifuMetroTextbox4.BorderColorMouseHover = Color.Blue;

            bunifuMetroTextbox5.Enabled = false;
            bunifuMetroTextbox6.Enabled = false;
            bunifuImageButton3.Enabled = false;
            linkLabel1.Show();

            panel10.Hide();
        }

        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            string curruser = "", user = "", pass = "";
            curruser = label15.Text;
            user = bunifuMetroTextbox6.Text.Trim();
            pass = bunifuMetroTextbox5.Text.Trim();

            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("Update accounttbl set Username = '" + user + "', Password = '" + pass + "' where Username = '" + curruser + "'", connection);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            MessageBox.Show("Account information changed");
            Login login = new Login();
            login.Show();
            this.Hide();
        }
        public int AccountNo()
        {
            int accountno = 0;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from accounttbl order by Account_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    accountno = dataReader.GetInt32("Account_No");

                }
                accountno = accountno + 1;
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            return accountno;
        }
        public void GetEmployeeWithOutAccount()
        {
            comboBox1.Items.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT *,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from employeetbl e LEFT JOIN accounttbl a ON e.Employee_No = a.Employee_No LEFT JOIN employee_positiontbl ep ON e.Employee_Position_No = ep.Employee_Position_No  where a.Employee_No IS NULL and (ep.Position_Name = 'Receptionist' or ep.Position_Name = 'Front Desk Officer' or ep.Position_Name = 'Consultant') and e.Employee_Status = 'Active'", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    comboBox1.Items.Add(dataReader.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)"));
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
                comboBox1.SelectedIndex = 0;
            }
            catch (Exception me)
            {
                comboBox1.Text = "No Employee";
            }
        }
        private void bunifuImageButton2_Click_1(object sender, EventArgs e)
        {
            int accountno = AccountNo();
            string user = "", pass = "", employee = "";
            int employeeno = 0;
            user = bunifuMetroTextbox1.Text.Trim();
            pass = bunifuMetroTextbox2.Text.Trim();
            employee = comboBox1.Text;
            bool check = false;
            if (employee == "No employee")
            {
                MessageBox.Show("No employee for an account");
                check = true;
            }
            if (check == false)
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT Employee_No,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from employeetbl where CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) = '" + employee + "'", connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        employeeno = dataReader.GetInt32("Employee_No");
                    }
                    connection.Close();

                    connection.Open();
                    MySqlCommand cmd1 = new MySqlCommand("Insert into accounttbl values ('" + accountno + "','" + user + "','" + pass + "','1','" + employeeno + "')", connection);
                    cmd1.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception me)
                {
                    connection.Close();
                    MessageBox.Show(me.Message);
                }
                MessageBox.Show("Account Successfully created");
                panel4.Hide();
                bunifuMetroTextbox1.Text = "";
                bunifuMetroTextbox2.Text = "";
                bunifuMetroTextbox3.Text = "";
                label7.Text = "";
                label8.Text = "";
                label9.Text = "";
                label10.Text = "";
                bunifuMetroTextbox1.BorderColorIdle = Color.Black;
                bunifuMetroTextbox1.BorderColorMouseHover = Color.Blue;
                bunifuMetroTextbox2.BorderColorIdle = Color.Black;
                bunifuMetroTextbox2.BorderColorMouseHover = Color.Blue;
                bunifuMetroTextbox3.BorderColorIdle = Color.Black;
                bunifuMetroTextbox3.BorderColorMouseHover = Color.Blue;
            }
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            string user = label15.Text;
            panel10.Visible = false;
            panel10.BringToFront();
            passwordTransition.ShowSync(panel10);
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from accounttbl where Username = '" + user + "'", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    bunifuMetroTextbox6.Text = dataReader.GetString("Username");
                    bunifuMetroTextbox5.Text = dataReader.GetString("Password");
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            label19.Hide();
            bunifuMetroTextbox4.Hide();
            label13.Hide();
            panel4.Hide();
            linkLabel1.Show();
            ctr++;
            settings.Hide();
            panel10.Show();
        }

        private void bunifuMetroTextbox5_KeyUp(object sender, KeyEventArgs e)
        {
            CheckUserAccount();
        }

        private void textBox24_OnValueChanged(object sender, EventArgs e)
        {
            string search = textBox24.Text.Trim();
            dataGridView1.Rows.Clear();
            try
            {
                connection.Open();
                string query = "SELECT *,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from patienttbl where Patient_LName LIKE '%" + search + "%' OR Patient_FName LIKE '%" + search + "%' order by Patient_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView1.Rows.Add(dataReader.GetInt32("Patient_No"), dataReader.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)"), dataReader.GetString("Patient_Birthdate"), dataReader.GetString("Patient_ContactNo"), dataReader.GetString("Patient_Address"));
                }

                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            string user = label15.Text;
            string isSmoker="", isAlcoholic= "";
            string risks = "";
            string position = "";
            try
            {
                connection.Open();
                MySqlCommand cmd1 = new MySqlCommand("SELECT * from accounttbl a, employeetbl e, employee_positiontbl ep where a.Username = '" + user + "' and a.Employee_no = e.Employee_No and e.Employee_Position_No = ep.Employee_Position_No", connection);
                MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                while (dataReader1.Read())
                {
                    position = dataReader1.GetString("Position_Name");
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            int row = dataGridView1.CurrentCell.RowIndex;
            int patientno = Convert.ToInt32(dataGridView1.Rows[row].Cells[0].Value);

            Profile profile = new Profile();
            profile.label48.Text = user;
            profile.label2.Text = patientno.ToString();
            string imagepath = "";
            if (position == "Consultant")
            {
                string skintypes = "", acnes = "", warts = "", hyper = "";
                try
                {
                    connection.Open();
                    string query = "SELECT *,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from patienttbl p, patient_demtbl pd where p.Patient_No = '" + patientno + "' and p.Patient_No = pd.Patient_No";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        skintypes = dataReader.GetString("Dem_SkinType");
                        acnes = dataReader.GetString("Dem_Acne");
                        hyper = dataReader.GetString("Dem_Hy");
                        warts = dataReader.GetString("Dem_Warts");
                        profile.label16.Text = dataReader.GetInt32("Patient_No").ToString();
                        profile.label17.Text = dataReader.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)");
                        imagepath = dataReader.GetString("Dem_Picture");
                        imagepath = imagepath.Replace(",", "/");
                        MessageBox.Show(imagepath);
                        profile.pictureBox1.Image = Image.FromFile(imagepath);
                        if (skintypes.Contains("Oily"))
                        {
                            profile.checkBox3.Checked = true;
                        }
                        if (skintypes.Contains("Sensitive"))
                        {
                            profile.checkBox4.Checked = true;
                        }
                        if (skintypes.Contains("Dry"))
                        {
                            profile.checkBox5.Checked = true;
                        }
                        if (skintypes.Contains("Pigmented"))
                        {
                            profile.checkBox6.Checked = true;
                        }
                        if (skintypes.Contains("Combination"))
                        {
                            profile.checkBox7.Checked = true;
                        }
                        if (dataReader.GetString("Dem_Frownlines") == "Have")
                        {
                            profile.checkBox1.Checked = true;
                        }
                        if (dataReader.GetString("Dem_Finelines") == "Have")
                        {
                            profile.checkBox2.Checked = true;
                        }
                        if (dataReader.GetString("Dem_Wrinkles") == "Have")
                        {
                            profile.checkBox23.Checked = true;
                        }
                        if (dataReader.GetString("Dem_Sagging") == "Have")
                        {
                            profile.checkBox24.Checked = true;
                        }
                        profile.textBox47.Text = dataReader.GetString("Dem_Allergies");
                        if (acnes.Contains("Blackheads"))
                        {
                            profile.checkBox8.Checked = true;
                        }
                        if (acnes.Contains("Whiteheads"))
                        {
                            profile.checkBox9.Checked = true;
                        }
                        if (hyper.Contains("Sunspots"))
                        {
                            profile.checkBox18.Checked = true;
                        }
                        if (hyper.Contains("Sunspots 1"))
                        {
                            profile.checkBox19.Checked = true;
                        }
                        if (hyper.Contains("Sunspots 2"))
                        {
                            profile.checkBox20.Checked = true;
                        }
                        if (hyper.Contains("Sunspots 3"))
                        {
                            profile.checkBox21.Checked = true;
                        }
                        if (hyper.Contains("Sunspots 4"))
                        {
                            profile.checkBox22.Checked = true;
                        }
                        if (warts.Contains("Face"))
                        {
                            profile.checkBox12.Checked = true;
                        }
                        if (warts.Contains("Upper Body"))
                        {
                            profile.checkBox13.Checked = true;
                        }
                        if (warts.Contains("Upper Back"))
                        {
                            profile.checkBox14.Checked = true;
                        }
                        if (warts.Contains("Neck"))
                        {
                            profile.checkBox15.Checked = true;
                        }
                        if (warts.Contains("Lower Body"))
                        {
                            profile.checkBox16.Checked = true;
                        }
                        if (warts.Contains("Lower Back"))
                        {
                            profile.checkBox17.Checked = true;
                        }
                        profile.richTextBox1.Text = dataReader.GetString("Dem_Reco");
                    }
                    connection.Close();
                }
                catch (MySqlException me)
                {
                    MessageBox.Show(me.Message);
                }
                profile.panel11.Enabled = true;
                profile.panel11.Show();
                profile.panel5.Hide();
                profile.panel6.Hide();
                profile.panel5.Enabled = false;
                profile.panel6.Enabled = false;
                profile.button18.Hide();
                profile.button17.Show();
                profile.button17.Location = new Point(0, 192);
                profile.Show();
                this.Hide();
            }
            else if (position == "Receptionist")
            {
                
                connection.Open();
                MySqlCommand cmd2 = new MySqlCommand("SELECT * from patienttbl p , patient_medicaltbl pm where p.Patient_No = '" + patientno + "' and p.Patient_No = pm.Patient_No", connection);
                MySqlDataReader dataReader2 = cmd2.ExecuteReader();
                while (dataReader2.Read())
                {
                    risks = dataReader2.GetString("Risk_Factors");
                    isSmoker = dataReader2.GetString("isSmoker");
                    isAlcoholic = dataReader2.GetString("isAlcoholDrinker");
                    profile.textBox14.Text = dataReader2.GetInt32("Patient_No").ToString();
                    profile.textBox1.Text = dataReader2.GetString("Patient_LName");
                    profile.textBox2.Text = dataReader2.GetString("Patient_FName");
                    profile.textBox3.Text = dataReader2.GetString("Patient_MidInit");
                    if (dataReader2.GetString("Patient_Gender") == "Male")
                    {
                        profile.radioButton1.Checked = true;
                    }
                    else
                    {
                        profile.radioButton2.Checked = true;
                    }
                    profile.dateTimePicker1.Value = Convert.ToDateTime(dataReader2.GetString("Patient_Birthdate"));
                    profile.textBox5.Text = dataReader2.GetInt32("Patient_Age").ToString();
                    profile.textBox4.Text = dataReader2.GetInt64("Patient_ContactNo").ToString();
                    profile.textBox11.Text = dataReader2.GetString("Patient_Email");
                    profile.textBox10.Text = dataReader2.GetString("Patient_Occupation");
                    if (dataReader2.GetString("Patient_CStatus") == "Single")
                    {
                        profile.radioButton3.Checked = true;
                    }
                    else if (dataReader2.GetString("Patient_CStatus") == "Married")
                    {
                        profile.radioButton4.Checked = true;
                    }
                    else if (dataReader2.GetString("Patient_CStatus") == "Widowed")
                    {
                        profile.radioButton7.Checked = true;
                    }
                    else if (dataReader2.GetString("Patient_CStatus") == "Others")
                    {
                        profile.radioButton8.Checked = true;
                    }
                    profile.textBox6.Text = dataReader2.GetString("Patient_Address");
                    profile.comboBox2.Text = dataReader2.GetString("Patient_Status");
                    profile.textBox29.Text = dataReader2.GetString("Height");
                    profile.textBox28.Text = dataReader2.GetString("Weight");
                    profile.comboBox1.Text = dataReader2.GetString("Body_Frame");
                    profile.textBox27.Text = dataReader2.GetInt32("Body_Fat").ToString();
                    profile.textBox25.Text = dataReader2.GetString("Blood_Pressure");
                    profile.textBox24.Text = dataReader2.GetInt32("Pulse_Rate").ToString();
                    if (isAlcoholic.Equals('T'))
                    {
                        profile.radioButton12.Checked = true;
                    }
                    else
                    {
                        profile.radioButton11.Checked = true;
                    }
                    if (isSmoker.Equals('T'))
                    {
                        profile.radioButton14.Checked = true;
                    }
                    else
                    {
                        profile.radioButton13.Checked = true;
                    }
                    if (risks.Contains("Diabetes"))
                    {
                        profile.checkBox37.Checked = true;
                    }
                    if (risks.Contains("Allergy"))
                    {
                        profile.checkBox36.Checked = true;
                    }
                    if (risks.Contains("Heart Disease"))
                    {
                        profile.checkBox35.Checked = true;
                    }
                    if (risks.Contains("Pace Maker"))
                    {
                        profile.checkBox34.Checked = true;
                    }
                    if (risks.Contains("Seizures"))
                    {
                        profile.checkBox33.Checked = true;
                    }
                    if (risks.Contains("Headaches"))
                    {
                        profile.checkBox32.Checked = true;
                    }
                    if (risks.Contains("Chest Pains"))
                    {
                        profile.checkBox31.Checked = true;
                    }
                    profile.textBox31.Text = dataReader2.GetString("Other_MedHist");
                    profile.textBox30.Text = dataReader2.GetString("Other_Risks");
                    if (string.IsNullOrEmpty(profile.textBox30.Text))
                    {
                        profile.checkBox30.Checked = false;
                    }
                    else
                    {
                        profile.checkBox30.Checked = true;
                    }
                    if (risks.Contains("Dizziness"))
                    {
                        profile.radioButton5.Checked = true;
                    }
                    if (risks.Contains("Asthma"))
                    {
                        profile.radioButton15.Checked = true;
                    }
                    if (risks.Contains("Nausea"))
                    {
                        profile.radioButton17.Checked = true;
                    }
                    if (risks.Contains("Arthritis"))
                    {
                        profile.radioButton19.Checked = true;
                    }
                    if (risks.Contains("Bladder Problems"))
                    {
                        profile.radioButton21.Checked = true;
                    }
                    if (risks.Contains("Cancer"))
                    {
                        profile.radioButton23.Checked = true;
                    }
                    if (risks.Contains("Ringing Ears"))
                    {
                        profile.radioButton25.Checked = true;
                    }
                    if (risks.Contains("Thyroid Conditions"))
                    {
                        profile.radioButton27.Checked = true;
                    }
                }
                connection.Close();
                profile.panel6.Show();
                profile.panel11.Hide();
                profile.panel5.Hide();
                profile.panel11.Enabled = false;
                profile.panel6.Enabled = true;
                profile.panel5.Enabled = true;
                profile.button17.Hide();
                profile.Show();
                this.Hide();
            }
            else
            {
                
                connection.Open();
                MySqlCommand cmd3 = new MySqlCommand("SELECT * from patienttbl p , patient_medicaltbl pm where p.Patient_No = '" + patientno + "' and p.Patient_No = pm.Patient_No", connection);
                MySqlDataReader dataReader3 = cmd3.ExecuteReader();
                while (dataReader3.Read())
                {
                    isSmoker = dataReader3.GetString("isSmoker");
                    isAlcoholic = dataReader3.GetString("isAlcoholDrinker");
                    risks = dataReader3.GetString("Risk_Factors");
                    profile.textBox14.Text = dataReader3.GetInt32("Patient_No").ToString();
                    profile.textBox1.Text = dataReader3.GetString("Patient_LName");
                    profile.textBox2.Text = dataReader3.GetString("Patient_FName");
                    profile.textBox3.Text = dataReader3.GetString("Patient_MidInit");
                    if (dataReader3.GetString("Patient_Gender") == "Male")
                    {
                        profile.radioButton1.Checked = true;
                    }
                    else
                    {
                        profile.radioButton2.Checked = true;
                    }
                    profile.dateTimePicker1.Value = Convert.ToDateTime(dataReader3.GetString("Patient_Birthdate"));
                    profile.textBox5.Text = dataReader3.GetInt32("Patient_Age").ToString();
                    profile.textBox4.Text = dataReader3.GetInt64("Patient_ContactNo").ToString();
                    profile.textBox11.Text = dataReader3.GetString("Patient_Email");
                    profile.textBox10.Text = dataReader3.GetString("Patient_Occupation");
                    if (dataReader3.GetString("Patient_CStatus") == "Single")
                    {
                        profile.radioButton3.Checked = true;
                    }
                    else if (dataReader3.GetString("Patient_CStatus") == "Married")
                    {
                        profile.radioButton4.Checked = true;
                    }
                    else if (dataReader3.GetString("Patient_CStatus") == "Widowed")
                    {
                        profile.radioButton7.Checked = true;
                    }
                    else if (dataReader3.GetString("Patient_CStatus") == "Others")
                    {
                        profile.radioButton8.Checked = true;
                    }
                    profile.textBox6.Text = dataReader3.GetString("Patient_Address");
                    profile.comboBox2.Text = dataReader3.GetString("Patient_Status");
                    profile.textBox29.Text = dataReader3.GetString("Height");
                    profile.textBox28.Text = dataReader3.GetString("Weight");
                    profile.comboBox1.Text = dataReader3.GetString("Body_Frame");
                    profile.textBox27.Text = dataReader3.GetInt32("Body_Fat").ToString();
                    profile.textBox25.Text = dataReader3.GetString("Blood_Pressure");
                    profile.textBox24.Text = dataReader3.GetInt32("Pulse_Rate").ToString();
                    if (isAlcoholic.Equals('T'))
                    {
                        profile.radioButton12.Checked = true;
                    }
                    else
                    {
                        profile.radioButton11.Checked = true;
                    }
                    if (isSmoker.Equals('T'))
                    {
                        profile.radioButton14.Checked = true;
                    }
                    else
                    {
                        profile.radioButton13.Checked = true;
                    }
                    if (risks.Contains("Diabetes"))
                    {
                        profile.checkBox37.Checked = true;
                    }
                    if (risks.Contains("Allergy"))
                    {
                        profile.checkBox36.Checked = true;
                    }
                    if (risks.Contains("Heart Disease"))
                    {
                        profile.checkBox35.Checked = true;
                    }
                    if (risks.Contains("Pace Maker"))
                    {
                        profile.checkBox34.Checked = true;
                    }
                    if (risks.Contains("Seizures"))
                    {
                        profile.checkBox33.Checked = true;
                    }
                    if (risks.Contains("Headaches"))
                    {
                        profile.checkBox32.Checked = true;
                    }
                    if (risks.Contains("Chest Pains"))
                    {
                        profile.checkBox31.Checked = true;
                    }
                    profile.textBox31.Text = dataReader3.GetString("Other_MedHist");
                    profile.textBox30.Text = dataReader3.GetString("Other_Risks");
                    if (string.IsNullOrEmpty(profile.textBox30.Text))
                    {
                        profile.checkBox30.Checked = false;
                    }
                    else
                    {
                        profile.checkBox30.Checked = true;
                    }
                    if (risks.Contains("Dizziness"))
                    {
                        profile.radioButton5.Checked = true;
                    }
                    if (risks.Contains("Asthma"))
                    {
                        profile.radioButton15.Checked = true;
                    }
                    if (risks.Contains("Nausea"))
                    {
                        profile.radioButton17.Checked = true;
                    }
                    if (risks.Contains("Arthritis"))
                    {
                        profile.radioButton19.Checked = true;
                    }
                    if (risks.Contains("Bladder Problems"))
                    {
                        profile.radioButton21.Checked = true;
                    }
                    if (risks.Contains("Cancer"))
                    {
                        profile.radioButton23.Checked = true;
                    }
                    if (risks.Contains("Ringing Ears"))
                    {
                        profile.radioButton25.Checked = true;
                    }
                    if (risks.Contains("Thyroid Conditions"))
                    {
                        profile.radioButton27.Checked = true;
                    }
                }
                connection.Close();
                profile.panel6.Show();
                profile.panel11.Hide();
                profile.panel5.Hide();
                profile.panel11.Enabled = false;
                profile.panel6.Enabled = true;
                profile.panel5.Enabled = true;
                profile.Show();
                this.Hide();
            }
        }
            //>>>>>>>>>>>>>>>>>>>>Services>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>Services>>>>>>>>>>>>>>>>>>>>>>>>>>>>Services>>>>>>>>>>>>>
            //private void ProductBtn_Click(object sender, EventArgs e)
            //{
            //    productsUC1.BringToFront();
            //    productsUC1.Visible = true ;
            //    servicesUC1.SendToBack();
            //    machineUC1.SendToBack();
            //    employeeUC1.SendToBack();
            //    discountUC1.SendToBack();
            //    dashboardUC1.SendToBack();


            //    prodBtn.Textcolor = Color.FromArgb(4, 180, 253);
            //    servicesBtn.Textcolor = Color.White;
            //    machBtn.Textcolor = Color.White;
            //    EmpBtn.Textcolor = Color.White;
            //    button15.Textcolor = Color.White;

            //    slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Top;
            //    slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Height;
            //}

            //private void servicesBtn_Click(object sender, EventArgs e)
            //{
            //    servicesUC1.BringToFront();
            //    servicesUC1.Visible = true;
            //    productsUC1.SendToBack();
            //    machineUC1.SendToBack();
            //    employeeUC1.SendToBack();
            //    discountUC1.SendToBack();
            //    dashboardUC1.SendToBack();

            //    slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Top;
            //    slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Height;

            //    servicesBtn.Textcolor = Color.FromArgb(4, 180, 253);
            //    prodBtn.Textcolor = Color.White;
            //    machBtn.Textcolor = Color.White;
            //    EmpBtn.Textcolor = Color.White;
            //    button15.Textcolor = Color.White;
            //}

            //private void machBtn_Click(object sender, EventArgs e)
            //{
            //    machineUC1.BringToFront();
            //    machineUC1.Visible = true;
            //    productsUC1.SendToBack();
            //    discountUC1.SendToBack();
            //    employeeUC1.SendToBack();
            //    servicesUC1.SendToBack();
            //    dashboardUC1.SendToBack();

            //    slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Top;
            //    slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Height;

            //    machBtn.Textcolor = Color.FromArgb(4, 180, 253);
            //    servicesBtn.Textcolor = Color.White;
            //    prodBtn.Textcolor = Color.White;
            //    EmpBtn.Textcolor = Color.White;
            //    button15.Textcolor = Color.White;
            //}

            //private void EmpBtn_Click(object sender, EventArgs e)
            //{
            //    employeeUC1.BringToFront();
            //    employeeUC1.Visible = true;
            //    productsUC1.SendToBack();
            //    machineUC1.SendToBack();
            //    discountUC1.SendToBack();
            //    servicesUC1.SendToBack();
            //    dashboardUC1.SendToBack();

            //    slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Top;
            //    slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Height;

            //    EmpBtn.Textcolor = Color.FromArgb(4, 180, 253);
            //    servicesBtn.Textcolor = Color.White;
            //    machBtn.Textcolor = Color.White;
            //    prodBtn.Textcolor = Color.White;
            //    button15.Textcolor = Color.White;
            //}

            //private void button15_Click_1(object sender, EventArgs e)
            //{
            //    discountUC1.BringToFront();
            //    discountUC1.Visible = true;
            //    productsUC1.SendToBack();
            //    machineUC1.SendToBack();
            //    employeeUC1.SendToBack();
            //    servicesUC1.SendToBack();
            //    dashboardUC1.SendToBack();
            //    slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Top;
            //    slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Height;

            //    button15.Textcolor = Color.FromArgb(4, 180, 253);
            //    servicesBtn.Textcolor = Color.White;
            //    machBtn.Textcolor = Color.White;
            //    EmpBtn.Textcolor = Color.White;
            //    prodBtn.Textcolor = Color.White;
            //}



            //private void button9_Click(object sender, EventArgs e)
            //{

            //    dashboardUC1.BringToFront();
            //    productsUC1.Visible = false;
            //    servicesUC1.Visible = false;
            //    employeeUC1.Visible = false;
            //    machineUC1.Visible = false;
            //    discountUC1.Visible = false;
            //    panel3.Hide();
            //    panel1.BringToFront();
            //    panel1.Visible = true;


            //}




        private void bunifuFlatButton4_Click(object sender, EventArgs e)
        {
            dashboardUC dash = new dashboardUC(this.Username);
            this.Controls.Add(dash);
            dash.BringToFront();
            dash.ParentForm = this;
            dash.CloseButtonClicked += new System.EventHandler(change_OkayButtonClicked);
            dash.Location = new Point(221, 55);
            dash.Size = new Size(1138, 683);

        }

        private void button12_Click(object sender, EventArgs e)
        {
            reportUC reportUC1 = new reportUC();
            this.Controls.Add(reportUC1);
            reportUC1.ParentForm = this;
            reportUC1.Size = new Size(1366, 717);
            reportUC1.Location = new Point(0, 55);
            string file = "DailySalesReportService.rpt";
            reportUC1.GetDaily(file);
            reportUC1.salesPanel.BringToFront();
            reportUC1.salesPanel.Show();
            reportUC1.productFilter.Show();
            reportUC1.productFilter.BringToFront();
            reportUC1.productViewer.Show();
            reportUC1.productViewer.BringToFront();
            reportUC1.inventoryPanel.SendToBack();
            reportUC1.appointmentPanel.SendToBack();
            reportUC1.patientPanel.SendToBack();
            reportUC1.btnSales.Textcolor = Color.FromArgb(4, 180, 253);
            reportUC1.btnInventory.Textcolor = Color.White;
            reportUC1.btnAppointments.Textcolor = Color.White;
            reportUC1.btnPatients.Textcolor = Color.White;
            reportUC1.btnReceipts.Textcolor = Color.White;

            reportUC1.slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)reportUC1.btnSales).Top;
            reportUC1.slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)reportUC1.btnSales).Height;
            reportUC1.BringToFront();
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
            notificationTransition.ShowSync(panel3);
            panel3.Show();
            comboBox2.SelectedIndex = 0;
        }

        private void label17_Click(object sender, EventArgs e)
        {
            panel3.Hide();
        }

        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            string dateregistered = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string birthdate = dateTimePicker2.Value.ToString("yyyy-MM-dd");
            string gender = comboBox2.Text.Trim();

            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("Select *,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from patienttbl where (Date_Registered = '"+dateregistered+"' or Date_Registered IS NULL) and (Patient_Gender = '"+gender+"' or Patient_Gender IS NULL) and (Patient_Birthdate = '"+birthdate+"' or Patient_Birthdate IS NULL)",connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView1.Rows.Add(dataReader.GetInt32("Patient_No"), dataReader.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)"), dataReader.GetString("Patient_Birthdate"), dataReader.GetString("Patient_ContactNo"), dataReader.GetString("Patient_Address"));
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            panel3.Hide();
        }

        private void logo1_Click(object sender, EventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void HomePage_Load(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void header_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
