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
using System.IO;
using System.Diagnostics;
using iTextSharp.text.pdf;
using System.Net.Mail;
using iTextSharp.text;

namespace SWSFCSMPIWBC
{
    public partial class regUC : UserControl
    {
        static string connectionString =
        System.Configuration.ConfigurationManager.
        ConnectionStrings["SWSFCSMPIWBC.Properties.Settings.slimmersdbConnectionString"].ConnectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);
        public Point current = new Point();
        public Point old = new Point();
        public Graphics g;
        Bitmap DrawArea;
        public Pen p = new Pen(Color.Black, 1);
        public regUC()
        {

            InitializeComponent();
            GetPatientNo();
            GetConsultant();
            DrawArea = new Bitmap(pictureBox1.Image, pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = DrawArea;
            p.SetLineCap(System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.DashCap.Round);
            button18.Textcolor = Color.FromArgb(4, 180, 253);
        }
        public regUC(string user)
        {
            this.userLabel = user;
            InitializeComponent();
            GetPatientNo();
            GetConsultant();
            DrawArea = new Bitmap(pictureBox1.Image, pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = DrawArea;
            p.SetLineCap(System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.DashCap.Round);
            button18.Textcolor = Color.FromArgb(4, 180, 253);
            
        }
       
        public string userLabel
        {
            get;
            set;
        }
        public event EventHandler CloseButtonClicked;
        protected virtual void OnCloseButtonClicked(EventArgs e)
        {
            var handler = CloseButtonClicked;
            if (handler != null)
            {
                handler(this, e);
            }

        }
        public HomePage ParentForm { get; set; }
        public void GetConsultant()
        {
            comboBox10.Items.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("Select CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from employeetbl e, employee_positiontbl ep where ep.Position_Name = 'Consultant' and e.Employee_Status = 'Active' and e.Employee_Position_No = ep.Employee_Position_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    comboBox10.Items.Add(dataReader.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)"));
                }
                connection.Close();

                comboBox10.SelectedIndex = 0;
            }
            catch (Exception me)
            {
                comboBox10.Text = "No Consultant";
                MessageBox.Show(me.Message);
            }
        }
        public void GetPatientNo()
        {
            int patientno = 0;
            try
            {
                connection.Open();
                string query = "SELECT Patient_No from patienttbl order by Patient_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    patientno = dataReader.GetInt32("Patient_No");
                }
                patientno = patientno + 1;
                textBox14.Text = patientno.ToString();
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
        }
        public int GetDemNo()
        {
            int demno = 0;
            connection.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * from patient_demtbl order by Dem_No", connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                demno = dataReader.GetInt32("Dem_No");
            }
            demno = demno + 1;
            connection.Close();

            return demno;
        }
        public bool IsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void GetServices()
        {
            comboBox1.Items.Clear();
            try
            {
                connection.Open();
                string query = "SELECT * from servicetbl where Service_Status = 'Active' order by Service_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    comboBox1.Items.Add(dataReader.GetString("Service_Name"));
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            try
            {
                comboBox1.SelectedIndex = 0;
            }
            catch (Exception)
            {
                comboBox1.Text = "No available";
            }
            string first = comboBox1.Text;
            try
            {
                connection.Open();
                string query1 = "Select * from servicetbl where Service_Name = '" + first + "' and Service_Status = 'Active' order by Service_No";
                MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                while (dataReader1.Read())
                {
                    textBox39.Text = dataReader1.GetDecimal("Service_Fee").ToString();
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
                textBox39.Text = "0.00";
            }
        }
        public void GetEmployee()
        {
            comboBox2.Items.Clear();

            try
            {


                connection.Open();
                string query1 = "SELECT CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from employeetbl e, employee_positiontbl ep where ep.Position_Name = 'Therapist' and ep.Employee_Position_No = e.Employee_Position_No and e.Employee_Status = 'Active' order by Employee_No";
                MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                while (dataReader1.Read())
                {
                    comboBox2.Items.Add(dataReader1.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)"));
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            try
            {
                comboBox2.SelectedIndex = 0;
            }
            catch (Exception)
            {
                comboBox2.Text = "No Available";
            }


        }
        public void GetEndTime(string start)
        {
            string service = comboBox1.Text;
            string startampm = "", endampm = "am", endtime = "";
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

            if (Convert.ToInt32(endmin1) >= 60 && Convert.ToInt32(endhour) >= 12 && startampm == "pm")
            {
                endhour = Convert.ToInt32(Convert.ToInt32(endhour) + (Convert.ToInt32(endmin1) / 60)).ToString("D2");
                endmin1 = Convert.ToInt32(Convert.ToInt32(endmin1) % 60).ToString("D2");
                endampm = "pm";
            }
            else if (Convert.ToInt32(endmin1) >= 60 && Convert.ToInt32(endhour) < 12 && startampm == "am")
            {
                endhour = Convert.ToInt32(Convert.ToInt32(endhour) + (Convert.ToInt32(endmin1) / 60)).ToString("D2");
                endmin1 = Convert.ToInt32(Convert.ToInt32(endmin1) % 60).ToString("D2");
                endampm = "am";
            }
            else if (Convert.ToInt32(endmin1) >= 60 && Convert.ToInt32(endhour) >= 12 && startampm == "am")
            {
                endhour = Convert.ToInt32(Convert.ToInt32(endhour) + (Convert.ToInt32(endmin1) / 60)).ToString("D2");
                endmin1 = Convert.ToInt32(Convert.ToInt32(endmin1) % 60).ToString("D2");
                endampm = "am";
            }
            if (Convert.ToInt32(endhour) > 12)
            {
                endhour = Convert.ToInt32(Convert.ToInt32(endhour) - 12).ToString("D2");
                endampm = "pm";
            }
            if (Convert.ToInt32(endhour) == 12)
            {
                endhour = Convert.ToInt32(endhour).ToString("D2");
                endampm = "pm";
            }
            endtime = endhour + ":" + endmin1 + " " + endampm;
            textBox40.Text = endtime;
        }
        public void GetMachines(string service)
        {
            comboBox4.Items.Clear();
            try
            {
                connection.Open();
                string query = "SELECT Machine_Name from machinetbl m, machine_typetbl mt, servicetbl s where s.Service_Name = '" + service + "' and s.Service_No = mt.Service_No and mt.Machine_Type_No = m.Machine_Type_No and Machine_Status = 'Available' order by Machine_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    comboBox4.Items.Add(dataReader.GetString("Machine_Name"));
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            try
            {
                comboBox4.SelectedIndex = 0;
                comboBox4.Enabled = true;
            }
            catch (Exception)
            {
                comboBox4.Items.Add("No available");
                comboBox4.SelectedIndex = 0;
                comboBox4.Enabled = false;
            }
        }
        public void GetAppointmentNo()
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
                textBox38.Text = appointno.ToString();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
        }

        ErrorProvider errorProvider = new ErrorProvider();
        private Control textBox21;
        private void button11_Click(object sender, EventArgs e)
        {
            string datetoday = DateTime.Now.ToString("yyyy-MM-dd");
            List<string> risks = new List<string>();
            string others = "", othermedhist = "";
            bool check = false;
            string lname, fname, mi, gender, lno, st, brgy, city, address, bdate, cstatus, occupation;
            long cno = 0;
            int age = 0, pno = 0;
            string containNumber = @"[0-9~!@#$%^&*()_+=-]";
            int patientno = Convert.ToInt32(textBox14.Text);
            lname = textBox1.Text.Trim();
            fname = textBox2.Text.Trim();
            mi = textBox3.Text.Trim();
            lno = textBox6.Text.Trim();
            st = textBox7.Text.Trim();
            brgy = textBox8.Text.Trim();
            city = textBox9.Text.Trim();
            address = lno + " " + st + " " + brgy + ", " + city;
            bdate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string email = textBox11.Text.Trim();
            occupation = textBox10.Text.Trim();
            string consultant = comboBox10.Text;
            int consultantno = 0;
            int emp_patientno = GetEmployeePatientNo();
            int employee_patientno = 0;
            if (consultant.Equals("No Consultant") || string.IsNullOrEmpty(consultant))
            {
                label24.Text = "There are no available consultant";
                check = true;
            }
            else
            {
                label24.Text = "";
            }
            if (!IsValid(email))
            {
                label6.Text = "Invalid email";
                textBox11.BackColor = Color.FromArgb(252, 224, 224);
                textBox11.BorderColorIdle = Color.Maroon;
                check = true;
                label23.Text = "**There's a problem with your personal information";
            }
            else
            {
                textBox11.BackColor = Color.White;
                textBox11.BorderColorIdle = Color.Black;
                label6.Text = "";
                label23.Text = "";
            }
            try
            {
                connection.Open();
                string query3 = "SELECT Patient_No from patienttbl";
                MySqlCommand cmd3 = new MySqlCommand(query3, connection);
                MySqlDataReader dataReader3 = cmd3.ExecuteReader();
                while (dataReader3.Read())
                {
                    pno = dataReader3.GetInt32("Patient_No");
                }
                pno = pno + 1;
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();
            if (lname.Length == 0)
            {
                check = true;
                label7.Text = "You must enter last name";
                textBox1.BorderColorIdle = Color.Maroon;
                textBox1.BackColor = Color.FromArgb(252, 224, 224);
                label23.Text = "**There's a problem with your personal information";
            }
            else
            {

                if (Regex.IsMatch(lname, containNumber))
                {
                    check = true;
                    label7.Text = "Last name format invalid";
                    textBox1.BorderColorIdle = Color.Maroon;
                    textBox1.BackColor = Color.FromArgb(252, 224, 224);
                    label23.Text = "**There's a problem with your personal information";
                }
                else
                {
                    label7.Text = "";
                    textBox1.BorderColorIdle = Color.Black;
                    textBox1.BackColor = Color.White;
                    label23.Text = "";
                }
            }

            if (fname.Length == 0)
            {
                check = true;
                label8.Text = "You must enter first name";
                textBox2.BorderColorIdle = Color.Maroon;
                textBox2.BackColor = Color.FromArgb(252, 224, 224);
                label23.Text = "**There's a problem with your personal information";
            }
            else
            {

                if (Regex.IsMatch(fname, containNumber))
                {
                    check = true;
                    label8.Text = "First name format invalid";
                    textBox2.BorderColorIdle = Color.Maroon;
                    textBox2.BackColor = Color.FromArgb(252, 224, 224);
                    label23.Text = "**There's a problem with your personal information";
                }
                else
                {
                    label8.Text = "";
                    textBox2.BorderColorIdle = Color.Black;
                    textBox2.BackColor = Color.White;
                    label23.Text = "";
                }
            }

            if (Regex.IsMatch(mi, containNumber))
            {
                check = true;
                label26.Text = "Middle initial format invalid";
                textBox3.BorderColorIdle = Color.Maroon;
                textBox3.BackColor = Color.FromArgb(252, 224, 224);
                label23.Text = "**There's a problem with your personal information";
            }
            else
            {
                label26.Text = "";
                textBox3.BorderColorIdle = Color.Black;
                textBox3.BackColor = Color.White;
                label23.Text = "";
            }

            if (st.Length == 0)
            {
                check = true;
                label10.Text = "You must enter street/subdivision name";
                textBox7.BorderColorIdle = Color.Maroon;
                textBox7.BackColor = Color.FromArgb(252, 224, 224);
                label23.Text = "**There's a problem with your personal information";
            }
            else
            {
                label10.Text = "";
                textBox7.BorderColorIdle = Color.Black;
                textBox7.BackColor = Color.White;
                label23.Text = "";
            }

            if (brgy.Length == 0)
            {
                check = true;
                label11.Text = "You must enter barangay name";
                textBox8.BorderColorIdle = Color.Maroon;
                textBox8.BackColor = Color.FromArgb(252, 224, 224);
                label23.Text = "**There's a problem with your personal information";
            }
            else
            {
                label11.Text = "";
                textBox8.BorderColorIdle = Color.Black;
                textBox8.BackColor = Color.White;
                label23.Text = "";
            }

            if (city.Length == 0)
            {
                check = true;
                label12.Text = "You must enter city name";
                textBox9.BorderColorIdle = Color.Maroon;
                textBox9.BackColor = Color.FromArgb(252, 224, 224);
                label23.Text = "**There's a problem with your personal information";
            }
            else
            {

                if (Regex.IsMatch(city, containNumber))
                {
                    check = true;
                    label12.Text = "City Name format invalid";
                    textBox9.BorderColorIdle = Color.Maroon;
                    textBox9.BackColor = Color.FromArgb(252, 224, 224);
                    label23.Text = "**There's a problem with your personal information";
                }
                else
                {
                    label12.Text = "";
                    textBox9.BorderColorIdle = Color.Black;
                    textBox9.BackColor = Color.White;
                    label23.Text = "";
                }
            }


            try
            {
                age = Convert.ToInt32(textBox5.Text.Trim());
                label13.Text = "";
                label23.Text = "";
            }
            catch (FormatException)
            {
                check = true;
                label13.Text = "Age is required. Please check your birthdate";
                label23.Text = "**There's a problem with your personal information";
            }
            if (age < 16)
            {
                check = true;
                label13.Text = "Age is not valid it should be 16+";
                textBox5.BackColor = Color.FromArgb(252, 224, 224);
                label23.Text = "**There's a problem with your personal information";
            }
            else if(age > 90)
            {
                check = true;
                label13.Text = "Age too old for a patient";
                textBox5.BackColor = Color.FromArgb(252, 224, 224);
                label23.Text = "**There's a problem with your personal information";
            }
            else
            {
                label13.Text = "";
                textBox5.BackColor = Color.White;
                label23.Text = "";
            }
            try
            {
                cno = Convert.ToInt64(textBox4.Text.Trim());
                label5.Text = "";
                textBox4.BackColor = Color.White;
                label23.Text = "";
            }
            catch (FormatException)
            {
                check = true;
                label5.Text = "Contact number format invalid";
                textBox4.BackColor = Color.FromArgb(252, 224, 224);
                label23.Text = "**There's a problem with your personal information";
            }
            if (radioButton1.Checked)
            {
                gender = "Male";
            }
            else
            {
                gender = "Female";
            }
            if (radioButton3.Checked)
            {
                cstatus = "Single";
            }
            else if (radioButton4.Checked)
            {
                cstatus = "Married";
            }
            else if (radioButton5.Checked)
            {
                cstatus = "Widowed";
            }
            else
            {
                cstatus = "Others";
            }
            string weight = "", height = "";
            string bodyframe = "", bp = "", prate = "";
            char smoke, alcoholic;
            int medno = 0;
            int bodyfat = 0;
            string bp1 = "", bp2 = "", emp_status = "";
            string containLetter = @"[A-Za-z~!@#$%^&*()_+=-]";
            try
            {
                bodyfat = Convert.ToInt32(textBox27.Text.Trim());
            }
            catch (Exception)
            {
                bodyfat = 0;
            }
            height = textBox12.Text.Trim();
            weight = textBox19.Text.Trim();
            bp1 = textBox13.Text.Trim();
            bp2 = textBox18.Text.Trim();
            prate = textBox24.Text.Trim();

            try
            {
                connection.Open();
                string query = "SELECT * from patient_medicaltbl order by Medical_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    medno = dataReader.GetInt32("Medical_No");
                }
                medno = medno + 1;
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            if (string.IsNullOrEmpty(height))
            {
                label14.Text = "Required height";
                textBox12.BackColor = Color.FromArgb(252, 224, 224);
                textBox12.BorderColorIdle = Color.Maroon;
                check = true;
            }
            else
            {
                if (Regex.IsMatch(height, containLetter))
                {
                    label14.Text = "Invalid height";
                    textBox12.BackColor = Color.FromArgb(252, 224, 224);
                    textBox12.BorderColorIdle = Color.Maroon;
                    check = true;
                }
                else
                {
                    label14.Text = "";
                    textBox12.BackColor = Color.White;
                    textBox12.BorderColorIdle = Color.Black;
                }
            }
            if (string.IsNullOrEmpty(weight))
            {
                label15.Text = "Required weight";
                textBox19.BackColor = Color.FromArgb(252, 224, 224);
                textBox19.BorderColorIdle = Color.Maroon;
                check = true;
            }
            else
            {
                if (Regex.IsMatch(weight, containLetter))
                {
                    label15.Text = "Invalid weight";
                    textBox19.BackColor = Color.FromArgb(252, 224, 224);
                    textBox19.BorderColorIdle = Color.Maroon;
                    check = true;

                }
                else
                {
                    label15.Text = "";
                    textBox19.BackColor = Color.White;
                    textBox19.BorderColorIdle = Color.Black;
                }
            }
            bodyframe = comboBox5.Text.Trim();

            if (string.IsNullOrEmpty(bp1))
            {
                label16.Text = "Required blood pressure";
                textBox13.BackColor = Color.FromArgb(252, 224, 224);
                textBox13.BorderColorIdle = Color.Maroon;
                check = true;
            }
            else
            {
                if (Regex.IsMatch(bp1, containLetter))
                {
                    label16.Text = "Invalid blood pressure";
                    textBox13.BackColor = Color.FromArgb(252, 224, 224);
                    textBox13.BorderColorIdle = Color.Maroon;
                    check = true;
                }
                else
                {
                    label16.Text = "";
                    textBox13.BackColor = Color.White;
                    textBox13.BorderColorIdle = Color.Black;
                }
            }
            if (string.IsNullOrEmpty(bp2))
            {
                label16.Text = "Required blood pressure";
                textBox18.BackColor = Color.FromArgb(252, 224, 224);
                textBox18.BorderColorIdle = Color.Maroon;
                check = true;
            }
            else
            {
                if (Regex.IsMatch(bp2, containLetter))
                {
                    label16.Text = "Invalid blood pressure";
                    textBox18.BackColor = Color.FromArgb(252, 224, 224);
                    textBox18.BorderColorIdle = Color.Maroon;
                    check = true;
                }
                else
                {
                    label16.Text = "";
                    textBox18.BackColor = Color.White;
                    textBox18.BorderColorIdle = Color.Black;
                }
            }
            bp = bp1.ToString() + '/' + bp2.ToString();
            if (string.IsNullOrEmpty(prate))
            {
                label17.Text = "Required pulse rate";
                textBox24.BackColor = Color.FromArgb(252, 224, 224);
                textBox24.BorderColorIdle = Color.Maroon;
                check = true;
            }
            else
            {
                if (Regex.IsMatch(prate, containLetter))
                {
                    label17.Text = "Invalid pulse rate";
                    textBox24.BackColor = Color.FromArgb(252, 224, 224);
                    textBox24.BorderColorIdle = Color.Maroon;
                    check = true;
                }
                else
                {
                    label17.Text = "";
                    textBox24.BackColor = Color.White;
                    textBox24.BorderColorIdle = Color.Black;
                }
            }
            if (radioButton11.Checked)
            {
                smoke = 'T';
            }
            else
            {
                smoke = 'F';
            }
            if (radioButton31.Checked)
            {
                alcoholic = 'T';
            }
            else
            {
                alcoholic = 'F';
            }
            if (checkBox37.Checked)
            {
                risks.Add("Diabetes");
            }
            if (checkBox36.Checked)
            {
                risks.Add("Allergy");
            }
            if (checkBox35.Checked)
            {
                risks.Add("Heart Disease");
            }
            if (checkBox34.Checked)
            {
                risks.Add("Pace Maker");
            }
            if (checkBox33.Checked)
            {
                risks.Add("Seizures");
            }
            if (checkBox32.Checked)
            {
                risks.Add("Headaches");
            }
            if (checkBox31.Checked)
            {
                risks.Add("Chest Pains");
            }
            if (checkBox30.Checked)
            {
                others = textBox30.Text;
            }
            if (!string.IsNullOrEmpty(textBox31.Text.Trim()))
            {
                othermedhist = textBox31.Text;
            }
            if (radioButton13.Checked)
            {
                risks.Add("Dizziness");
            }
            if (radioButton15.Checked)
            {
                risks.Add("Asthma");
            }
            if (radioButton17.Checked)
            {
                risks.Add("Nausea");
            }
            if (radioButton19.Checked)
            {
                risks.Add("Arthritis");
            }
            if (radioButton21.Checked)
            {
                risks.Add("Bladder Problems");
            }
            if (radioButton23.Checked)
            {
                risks.Add("Cancer");
            }
            if (radioButton25.Checked)
            {
                risks.Add("Ringing Ears");
            }
            if (radioButton27.Checked)
            {
                risks.Add("Thyroid Conditions");
            }
            string allrisk = "";
            foreach (var risk in risks)
            {
                allrisk += risk + ", ";
            }
            try
            {
                allrisk = allrisk.Substring(0, allrisk.Length - 2);
            }catch(Exception me)
            {
                allrisk = "";
            }
            bool proceed = false;
            if (check == false)
            {

                try
                {
                    connection.Open();
                    string query2 = "INSERT INTO patienttbl values('" + patientno + "','" + lname + "','" + fname + "','" + mi + "','" + gender + "','" + bdate + "','" + age + "','" + address + "','" + cno + "','" + email + "','" + cstatus + "','" + occupation + "','Active','New','" + datetoday + "')";
                    MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                    cmd2.ExecuteNonQuery();
                    connection.Close();

                    connection.Open();
                    string query1 = "INSERT INTO patient_medicaltbl values ('" + medno + "','" + height + "','" + weight + "','" + bodyfat + "','" + bodyframe + "','" + bp + "','" + prate + "','"+smoke+"','"+alcoholic+"','" + allrisk + "','" + others + "','" + othermedhist + "','" + patientno + "')";
                    MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                    cmd1.ExecuteNonQuery();
                    connection.Close();

                    connection.Open();
                    MySqlCommand cmd3 = new MySqlCommand("SELECT Employee_No,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from employeetbl where CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) = '" + consultant + "'", connection);
                    MySqlDataReader dataReader3 = cmd3.ExecuteReader();
                    while (dataReader3.Read())
                    {
                        consultantno = dataReader3.GetInt32("Employee_No");
                    }
                    connection.Close();

                    connection.Open();
                    MySqlCommand cmd4 = new MySqlCommand("INSERT into employee_patienttbl values (@employee_patient,@patient,@employee)", connection);
                    cmd4.Parameters.AddWithValue("@employee_patient", emp_patientno);
                    cmd4.Parameters.AddWithValue("@patient", patientno);
                    cmd4.Parameters.AddWithValue("@employee", consultantno);
                    cmd4.ExecuteNonQuery();
                    connection.Close();

                    connection.Open();
                    MySqlCommand cmd5 = new MySqlCommand("SELECT Employee_Availability from employeetbl where Employee_No = '" + consultantno + "'", connection);
                    MySqlDataReader dataReader5 = cmd5.ExecuteReader();
                    while (dataReader5.Read())
                    {
                        emp_status = dataReader5.GetString("Employee_Availability");
                    }
                    connection.Close();
                    if (emp_status == "Available")
                    {
                        connection.Open();
                        MySqlCommand cmd8 = new MySqlCommand("INSERT into patient_waitlisttbl(Employee_Patient_No,Waiting_For,Waiting_Status) values ('" + emp_patientno + "','Consultation','On Going')", connection);
                        cmd8.ExecuteNonQuery();
                        connection.Close();

                        connection.Open();
                        MySqlCommand cmd6 = new MySqlCommand("UPDATE employeetbl e, employee_patienttbl ep, patienttbl p set e.Employee_Availability = 'Not Available' where e.Employee_No = ep.Employee_No and p.Patient_No = '" + patientno + "' and p.Patient_No = ep.Patient_No", connection);
                        cmd6.ExecuteNonQuery();
                        connection.Close();
                    }
                    else
                    {
                        connection.Open();
                        MySqlCommand cmd7 = new MySqlCommand("INSERT into patient_waitlisttbl(Employee_Patient_No,Waiting_For,Waiting_Status) values ('" + emp_patientno + "','Consultation','Not Started')", connection);
                        cmd7.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                catch (MySqlException me)
                {
                    connection.Close();
                    MessageBox.Show(me.Message);
                }
                MessageBox.Show("Patient record added!");
                dashboardUC dash = new dashboardUC(ParentForm.Username);
                dash.BringToFront();
                dash.Show();
                this.Hide();
                ClearError();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            Process.Start(Application.StartupPath + @"\Forms\TestWaiver.pdf");

            //string name = textBox1.Text + ", " + textBox2.Text + " " + textBox3.Text;

            //string pathin = Environment.CurrentDirectory + @"\Forms\TestWaiver.pdf";
            //string pathout = Environment.CurrentDirectory + @"\Forms\PatientWaiver.pdf";

            //PdfReader reader = new PdfReader(pathin);
            //int n = reader.NumberOfPages;
            //PdfStamper stamper = new PdfStamper(reader, new FileStream(pathout,FileMode.Create,FileAccess.Write));
            //PdfImportedPage page;
            //for (int i = 0; i < n;)
            //{
            //    page = stamper.GetImportedPage(reader, ++i);
            //}
            //int lastPage = n + 1;
            //iTextSharp.text.Rectangle rect = PageSize.LETTER;
            //stamper.InsertPage(lastPage, rect);
            //PdfContentByte cb = stamper.GetOverContent(lastPage);
            //ColumnText.ShowTextAligned(
            //  cb, Element.ALIGN_LEFT, new Phrase("YOUR PARAGRAPH"),
            //  rect.Left + 36, rect.Top - 72, 0
            //);
            //stamper.Close();

            //PdfReader reader = new PdfReader(pathin);
            //iTextSharp.text.Rectangle size = reader.GetPageSizeWithRotation(1);
            //Document document = new Document(size);

            //// open the writer
            //FileStream fs = new FileStream(pathin, FileMode.Create, FileAccess.Write);
            //PdfWriter writer = PdfWriter.GetInstance(document, fs);
            //document.Open();

            //// the pdf content
            //PdfContentByte cb = writer.DirectContent;

            //// select the font properties
            //BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            //cb.SetColorFill(BaseColor.DARK_GRAY);
            //cb.SetFontAndSize(bf, 8);

            //// write the text in the pdf content
            //cb.BeginText();
            //string text = "Some random blablablabla...";
            //// put the alignment and coordinates here
            //cb.ShowTextAligned(1, text, 520, 640, 0);
            //cb.EndText();
            //cb.BeginText();
            //text = "Other random blabla...";
            //// put the alignment and coordinates here
            //cb.ShowTextAligned(2, text, 100, 200, 0);
            //cb.EndText();

            //// create the new page and add it to the pdf
            //PdfImportedPage page = writer.GetImportedPage(reader, 1);
            //cb.AddTemplate(page, 0, 0);

            //// close the streams and voilá the file should be changed :)
            //document.Close();
            //fs.Close();
            //writer.Close();
            //reader.Close();



            //using (var reader = new PdfReader(pathin))
            //{
            //    using (var filestream = new FileStream(pathout, FileMode.Create, FileAccess.Write))
            //    {
            //        var document = new Document(reader.GetPageSizeWithRotation(1));
            //        var writer = PdfWriter.GetInstance(document, filestream);

            //        document.Open();

            //        document.NewPage();
            //        var basefont = BaseFont.CreateFont(BaseFont.HELVETICA,BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            //        var importedpage = writer.GetImportedPage(reader,1);
            //        var contentByte = writer.DirectContent;
            //        contentByte.BeginText();
            //        contentByte.SetFontAndSize(basefont, 12);
            //        contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER,name, 200, 200, 0);

            //        contentByte.EndText();
            //        contentByte.AddTemplate(importedpage, 0, 0);

            //        document.Close();
            //        writer.Close();

            //    }
            //}


        }
        private void CreateWaiver()
        {
            
        }
        static void ManipulatePdf(String src, String dest)
        {

           
            // CLose the stamper

        }
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(Application.StartupPath + @"\Forms\TestConsent.pdf");


        }
        private void button3_Click(object sender, EventArgs e)
        {
            string image = @"\pics\Mukha.png";
            pictureBox1.Image = System.Drawing.Image.FromFile(Application.StartupPath + image);
            DrawArea = new Bitmap(pictureBox1.Image, pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = DrawArea;
            p.SetLineCap(System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.DashCap.Round);
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            old = e.Location;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.ParentForm.Controls.Remove(this);
            dashboardUC dash = new dashboardUC(ParentForm.Username);
            dash.BringToFront();
            dash.Show();
            
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            textBox4.Text = "";
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
                MessageBox.Show(me.Message);
            }
            return emp_patientno;
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            string consultdate = DateTime.Today.ToString("yyyy-MM-dd");
            string date = dateTimePicker2.Value.ToString("yyyy-MM-dd");
            string starttime = "";
            string endtime = textBox40.Text;
            decimal servicefee = Convert.ToDecimal(textBox39.Text);
            string checkdate = "", checkstart = "", checkend = "", startampm = "", endampm = "", checkstartampm = "", checkendampm = "", checkmachine = "", checkemp = "";
            int starthour = 0, startmin = 0, endhour = 0, endmin = 0, checkstarthour = 0, checkstartmin = 0, checkendhour = 0, checkendmin = 0;
            bool check = false, checker = false;
            string machinename = "", servicename = "", consultant = "", therapist = "";
            int machineno = 0, serviceno = 0, consultantno = 0, therapistno = 0;
            int appointno = Convert.ToInt32(textBox38.Text);
            int patientno = 0;
            string patient = comboBox3.Text.Trim();
            bool haspatient = false;
            patientno = Convert.ToInt32(label4.Text);
            //connection.Open();
            //MySqlCommand cmd8 = new MySqlCommand("SELECT * from patienttbl where RTRIM(CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)) = '" + patient + "'", connection);
            //MySqlDataReader dataReader8 = cmd8.ExecuteReader();
            //while (dataReader8.Read())
            //{
            //    patientno = dataReader8.GetInt32("Patient_No");
            //    haspatient = true;
            //}
            //connection.Close();
            //if (haspatient == false)
            //{
            //    check = true;
            //    checker = true;
            //}
            machinename = comboBox4.Text;
            servicename = comboBox1.Text;
            string skintypes = "", acnes = "", hyperpigments = "", warts = "", recommendations = "";
            int demno = GetDemNo();
            List<string> skintypearray = new List<string>();
            List<string> acnearray = new List<string>();
            List<string> hyperpigmentarray = new List<string>();
            List<string> wartsarray = new List<string>();
            string allergies = "", frownlines = "", finelines = "", wrinkles = "", sagging = "";
            allergies = textBox47.Text.Trim();
            string demimagepath = "",savedemimagepath = "";
            Bitmap bmp = new Bitmap(pictureBox1.ClientSize.Width, pictureBox1.ClientSize.Height);
            pictureBox1.DrawToBitmap(bmp, pictureBox1.ClientRectangle);
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString();
            directory = directory.Replace("\\", "/");
            try
            {
                Directory.CreateDirectory(directory + "/SMPIWBC/DemPic");
            }
            catch (Exception me) { MessageBox.Show(me.Message); }
            demimagepath = directory + "/SMPIWBC/DemPic/Dem" + demno + ".jpg";
            if (File.Exists(demimagepath))
            {
                File.Delete(demimagepath);
            }
            bmp.Save(demimagepath);
            
            savedemimagepath = demimagepath.Replace("/", ",");
            skintypearray.Add(textBox48.Text.Trim());
            if (checkBox1.Checked)
            {
                frownlines = "Have";
            }
            else
            {
                frownlines = "None";
            }
            if (checkBox2.Checked)
            {
                finelines = "Have";
            }
            else
            {
                finelines = "None";
            }
            if (checkBox23.Checked)
            {
                wrinkles = "Have";
            }
            else
            {
                wrinkles = "None";
            }
            if (checkBox24.Checked)
            {
                sagging = "Have";
            }
            else
            {
                sagging = "None";
            }
            if (checkBox3.Checked)
            {
                skintypearray.Add("Oily");
            }
            if (checkBox4.Checked)
            {
                skintypearray.Add("Sensitive");
            }
            if (checkBox5.Checked)
            {
                skintypearray.Add("Dry");
            }
            if (checkBox6.Checked)
            {
                skintypearray.Add("Pigmented");
            }
            if (checkBox7.Checked)
            {
                skintypearray.Add("Combination");
            }
            if (checkBox8.Checked)
            {
                acnearray.Add("Blackheads");
            }
            if (checkBox9.Checked)
            {
                acnearray.Add("Whiteheads");
            }
            if (checkBox12.Checked)
            {
                wartsarray.Add("Neck");
            }
            if (checkBox13.Checked)
            {
                wartsarray.Add("Face");
            }
            if (checkBox14.Checked)
            {
                wartsarray.Add("Upper Body");
            }
            if (checkBox15.Checked)
            {
                wartsarray.Add("Lower Body");
            }
            if (checkBox16.Checked)
            {
                wartsarray.Add("Upper Back");
            }
            if (checkBox17.Checked)
            {
                wartsarray.Add("Lower Back");
            }
            if (checkBox18.Checked)
            {
                hyperpigmentarray.Add(checkBox18.Text);
            }
            if (checkBox19.Checked)
            {
                hyperpigmentarray.Add(checkBox19.Text);
            }
            if (checkBox20.Checked)
            {
                hyperpigmentarray.Add(checkBox20.Text);
            }
            if (checkBox21.Checked)
            {
                hyperpigmentarray.Add(checkBox21.Text);
            }
            if (checkBox22.Checked)
            {
                hyperpigmentarray.Add(checkBox22.Text);
            }
            foreach (var stype in skintypearray)
            {
                skintypes += stype + ", ";
            }
            try
            {
                skintypes = skintypes.Substring(0, skintypes.Length - 2);
            }
            catch (Exception me)
            {
                skintypes = "";
            }
            foreach (var acne in acnearray)
            {
                acnes += acne + ", ";
            }
            try
            {
                acnes = acnes.Substring(0, acnes.Length - 2);
            }
            catch (Exception me)
            {
                acnes = "";
            }
            foreach (var hp1 in hyperpigmentarray)
            {
                hyperpigments += hp1 + ", ";
            }
            try
            {
                hyperpigments = hyperpigments.Substring(0, hyperpigments.Length - 2);
            }
            catch (Exception me)
            {
                hyperpigments = "";
            }
            foreach (var wart in wartsarray)
            {
                warts += wart + ", ";
            }
            try
            {
                warts = warts.Substring(0, warts.Length - 2);
            }
            catch (Exception)
            {
                warts = "";
            }
            recommendations = richTextBox1.Text.Trim();
            try
            {
                connection.Open();
                MySqlCommand cmd6 = new MySqlCommand("SELECT Employee_No from accounttbl where Username = '" + this.userLabel + "'", connection);
                MySqlDataReader dataReader6 = cmd6.ExecuteReader();
                while (dataReader6.Read())
                {
                    consultantno = dataReader6.GetInt32("Employee_No");
                }
                connection.Close();
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
            }
            therapist = comboBox2.Text;
            try
            {
                errorProvider.SetError(comboBox7, string.Empty);
                starttime = comboBox7.Text;
                starthour = Convert.ToInt32(starttime.Substring(0, 2));
                startmin = Convert.ToInt32(starttime.Substring(3, 2));
                startampm = starttime.Substring(6, 2);
                endhour = Convert.ToInt32(endtime.Substring(0, 2));
                endmin = Convert.ToInt32(endtime.Substring(3, 2));
                endampm = endtime.Substring(6, 2);
            }
            catch (Exception me)
            {
                errorProvider.SetError(comboBox7, "Please select time first");
                checker = true;
                MessageBox.Show(me.Message);
            }
            if (string.IsNullOrEmpty(machinename))
            {
                errorProvider.SetError(comboBox4, "Please select machine first");
                checker = true;
            }
            else
            {
                errorProvider.SetError(comboBox4, string.Empty);
            }
            if (string.IsNullOrEmpty(servicename))
            {
                errorProvider.SetError(comboBox1, "Please select service first");
                checker = true;
            }
            else
            {
                errorProvider.SetError(comboBox1, string.Empty);
            }

            if (string.IsNullOrEmpty(therapist))
            {
                errorProvider.SetError(comboBox2, "Please select your therapist first");
                checker = true;
            }
            else
            {
                errorProvider.SetError(comboBox2, string.Empty);
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
                string query4 = "SELECT Employee_No,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from employeetbl where CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)='" + therapist + "'";
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

                connection.Close();
                if (check == false && checker == false)
                {
                    try
                    {
                        connection.Open();
                        string query6 = "INSERT into patient_demtbl values ('" + demno + "','" + consultdate + "','" + allergies + "','" + frownlines + "','" + finelines + "','" + wrinkles + "','" + sagging + "','" + skintypes + "','" + acnes + "','" + warts + "','" + hyperpigments + "','" + recommendations + "','" + savedemimagepath + "','" + patientno + "')";
                        MySqlCommand cmd6 = new MySqlCommand(query6, connection);
                        cmd6.ExecuteNonQuery();
                        connection.Close();
                        int emp_patientno = GetEmployeePatientNo();

                        connection.Open();
                        string query3 = "INSERT into appointmenttbl values ('" + appointno + "','" + date + "','" + starttime + "','" + endtime + "','Not Started','" + serviceno + "','" + machineno + "')";
                        MySqlCommand cmd3 = new MySqlCommand(query3, connection);
                        cmd3.ExecuteNonQuery();
                        connection.Close();

                        connection.Open();
                        string query5 = "Insert into employee_patienttbl values('" + emp_patientno + "','" + patientno + "','" + therapistno + "')";
                        MySqlCommand cmd5 = new MySqlCommand(query5, connection);
                        cmd5.ExecuteNonQuery();
                        connection.Close();

                        connection.Open();
                        MySqlCommand cmd7 = new MySqlCommand("INSERT into employee_appointmenttbl(Appointment_No,Employee_Patient_No) values ('" + appointno + "','" + emp_patientno + "')", connection);
                        cmd7.ExecuteNonQuery();
                        connection.Close();
                        
                        connection.Open();
                        MySqlCommand cmd10 = new MySqlCommand("UPDATE patient_waitlisttbl pw, employeetbl e, patienttbl p, employee_patienttbl ep set Waiting_Status = 'Done' where e.Employee_No = '" + consultantno + "' and p.Patient_No = '" + patientno + "' and e.Employee_No = ep.Employee_No and p.Patient_No = ep.Patient_No and ep.Employee_Patient_No = pw.Employee_Patient_No", connection);
                        cmd10.ExecuteNonQuery();
                        connection.Close();

                        connection.Open();
                        MySqlCommand cmd11 = new MySqlCommand("UPDATE patient_waitlisttbl set Waiting_Status = 'On Going' where Employee_Patient_No IN (SELECT ep.Employee_Patient_No from  employeetbl e, employee_patienttbl ep where e.Employee_No = '" + consultantno + "' and e.Employee_No = ep.Employee_No) and Waiting_Status = 'Not Started' order by Waiting_No LIMIT 1", connection);
                        cmd11.ExecuteNonQuery();
                        connection.Close();

                        connection.Open();
                        MySqlCommand cmd12 = new MySqlCommand("Update patienttbl set Patient_Status2 = 'Old' where Patient_No = '" + patientno + "' and Patient_Status = 'New'", connection);
                        cmd12.ExecuteNonQuery();
                        connection.Close();

                        haspatient = CheckWaitList(consultantno);
                        if (!haspatient)
                        {
                            connection.Open();
                            MySqlCommand cmd9 = new MySqlCommand("UPDATE employeetbl set Employee_Availability = 'Available' where Employee_No = '" + consultantno + "'", connection);
                            cmd9.ExecuteNonQuery();
                            connection.Close();
                        }
                        
                    }
                    catch (MySqlException me)
                    {
                        MessageBox.Show(me.Message);
                    }
                    MessageBox.Show("Success!");
                    ParentForm.Patient = comboBox3.Text;
                    ParentForm.Service = servicename;
                    ParentForm.Fee = servicefee;
                    ParentForm.AppointNo = appointno;
                    OnCloseButtonClicked(e);
                }
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
        }

        protected bool CheckWaitList(int consultno)
        {
            bool check = false;

            connection.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * from patient_waitlisttbl pw, employee_patienttbl ep, employeetbl e where pw.Employee_Patient_No = ep.Employee_Patient_No and ep.Employee_No = e.Employee_No and e.Employee_No = '" + consultno + "' and pw.Waiting_Status = 'On Going'",connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                check = true;
            }
            connection.Close();

            return check;
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            panel7.Hide();
            button2.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel11.Show();
            button9.Enabled = false;
            button4.Enabled = false;
            button18.Enabled = false;
            button17.Enabled = false;
            button8.Enabled = false;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox58.Checked == true)
            {
                bool check = false;
                int age = 0;
                try
                {
                    age = Convert.ToInt32(textBox5.Text);
                }
                catch (Exception)
                {
                    check = true;
                }
                if (check == false)
                {
                    linkLabel1.Visible = true;
                    if (age < 18)
                    {
                        linkLabel2.Visible = true;
                    }
                    CheckToNext();
                }
            }
            else
            {
                linkLabel1.Visible = false;
                linkLabel2.Visible = false;
                button19.Enabled = false;
            }
        }
        private void button4_Click_1(object sender, EventArgs e)
        {
            AddAppointmentTransition.ShowSync(panel11);
            panel11.BringToFront();
            button4.Enabled = false;
            panel26.Enabled = false;
            button8.Enabled = false;
            button17.Enabled = false;
            panel11.Show();
            GetAppointmentNo();
            GetServices();
            GetEmployee();
            dateTimePicker2.MinDate = DateTime.Now;
            try
            {
                comboBox7.SelectedIndex = 0;
            }
            catch (Exception) { }
            string start = comboBox7.Text;
            GetEndTime(start);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            panel11.Hide();
            panel7.Show();
        }
        private void button12_Click(object sender, EventArgs e)
        {
            panel31.BringToFront();
            panel34.SendToBack();
            panel25.SendToBack();
        }
        public void GetConsultPatient()
        {
            List<string> patientArray = new List<string>();
            comboBox3.Items.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT *,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from patienttbl where Patient_Status2 = 'New'", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    comboBox3.Items.Add(dataReader.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)"));
                    patientArray.Add(dataReader.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)"));
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            AutoCompleteStringCollection col = new AutoCompleteStringCollection();
            foreach (var patients in patientArray)
            {
                col.Add(patients);
            }
            comboBox3.AutoCompleteCustomSource = col;
            try
            {
                comboBox3.SelectedIndex = 0;
            }
            catch (Exception)
            {
                comboBox3.Text = "No Patient";
            }
        }
        private void button17_Click(object sender, EventArgs e)
        {
            panel25.BringToFront();
            panel34.SendToBack();
            panel31.SendToBack();

            slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Top;
            slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Height;
            button17.Textcolor = Color.FromArgb(4, 180, 253);
            button18.Textcolor = Color.White;

            GetConsultPatient();

        }
        public void ClearError()
        {
            textBox1.BackColor = Color.White;
            textBox2.BackColor = Color.White;
            textBox4.BackColor = Color.White;
            textBox5.BackColor = Color.White;
            textBox6.BackColor = Color.White;
            textBox11.BackColor = Color.White;
            textBox7.BackColor = Color.White;
            textBox8.BackColor = Color.White;
            textBox9.BackColor = Color.White;
            textBox12.BackColor = Color.White;
            textBox19.BackColor = Color.White;
            textBox13.BackColor = Color.White;
            textBox18.BackColor = Color.White;
            textBox24.BackColor = Color.White;
            label7.Text = "";
            label5.Text = "";
            label6.Text = "";
            label8.Text = "";
            label9.Text = "";
            label10.Text = "";
            label11.Text = "";
            label12.Text = "";
            label13.Text = "";
            label14.Text = "";
            label15.Text = "";
            label16.Text = "";
            label17.Text = "";
        }
        private void button18_Click(object sender, EventArgs e)
        {
            ClearError();
            panel31.BringToFront();
            panel34.SendToBack();
            panel25.SendToBack();

            button18.Textcolor = Color.FromArgb(4, 180, 253);
            button17.Textcolor = Color.White;

            slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Top;
            slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Height;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            ClearError();
            label23.Text = "";
            panel34.BringToFront();
            panel31.SendToBack();
            panel25.SendToBack();
            comboBox5.SelectedIndex = 0;
        }
        private void button15_Click(object sender, EventArgs e)
        {
            label1.Text = "";
            label19.Text = "";
            label22.Text = "";
            label20.Text = "";
            label21.Text = "";
            string date = dateTimePicker2.Value.ToString("yyyy-MM-dd");
            string starttime = "";
            string endtime = textBox40.Text;
            decimal servicefee = Convert.ToDecimal(textBox39.Text);
            string checkdate = "", checkstart = "", checkend = "", startampm = "", endampm = "", checkstartampm = "", checkendampm = "", checkmachine = "", checkemp = "";
            int starthour = 0, startmin = 0, endhour = 0, endmin = 0, checkstarthour = 0, checkstartmin = 0, checkendhour = 0, checkendmin = 0;
            bool check = false, checker = false,checking = false;
            string machinename = "", servicename = "", consultant = "", therapist = "";
            int empstarthour = 0, empstartmin = 0, empendhour = 0, empendmin = 0;
            string empstartampm = "", empendampm = "";
            int machineno = 0, serviceno = 0, consultantno = 0, therapistno = 0;
            int appointno = Convert.ToInt32(textBox38.Text);
            therapist = comboBox2.Text;
            machinename = comboBox4.Text;
            try
            {
                connection.Open();
                MySqlCommand cmd4 = new MySqlCommand("SELECT Employee_No,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from employeetbl where CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)= '" + therapist + "'", connection);
                MySqlDataReader dataReader4 = cmd4.ExecuteReader();
                while (dataReader4.Read())
                {
                    therapistno = dataReader4.GetInt32("Employee_No");
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            if (string.IsNullOrEmpty(machinename.Trim()) || machinename == "No available")
            {
                label19.Text = "No available Machine";
                checker = true;
            }
            else
            {
                label19.Text = "";
            }
            if (string.IsNullOrEmpty(therapist) || therapist == "No available")
            {
                label20.Text = "No available therapist";
                checker = true;
            }
            else
            {
                label19.Text = "";
            }

            try
            {
                label21.Text = "";
                starttime = comboBox7.Text;
                starthour = Convert.ToInt32(starttime.Substring(0, 2));
                startmin = Convert.ToInt32(starttime.Substring(3, 2));
                startampm = starttime.Substring(6, 2);
                endhour = Convert.ToInt32(endtime.Substring(0, 2));
                endmin = Convert.ToInt32(endtime.Substring(3, 2));
                endampm = endtime.Substring(6, 2);
            }
            catch (Exception me)
            {
                label21.Text = "Please select time first";
                checker = true;
            }
            if (string.IsNullOrEmpty(comboBox4.Text.Trim()))
            {
                label19.Text = "Please select machine first";
                checker = true;
            }
            else
            {
                label19.Text = "";
            }
            if (string.IsNullOrEmpty(comboBox1.Text.Trim()) || comboBox1.Text.Trim() == "No Available")
            {
                label18.Text = "No Available Service";
                checker = true;
            }
            else
            {
                label18.Text = "";
            }
            if (string.IsNullOrEmpty(textBox40.Text))
            {
                label21.Text = "Please select time first";
                checker = true;
            }
            else
            {
                label21.Text = "";
            }
            //try
            //{
            //    connection.Open();
            //    string day = dateTimePicker2.Value.ToString("dddd");
            //    MySqlCommand cmd1 = new MySqlCommand("SELECT *,DAYNAME('2017-10-20') from employee_schedtbl es, employeetbl e where es.Employee_No = '"+therapistno+"' and es.Employee_No = e.Employee_No and Schedule_Day = '"+day+"'", connection);
            //    MySqlDataReader dataReader1 = cmd1.ExecuteReader();
            //    while (dataReader1.Read())
            //    {
            //        empstarthour = Convert.ToInt32(dataReader1.GetString("Schedule_TimeIn").Substring(0,2));
            //        empstartmin = Convert.ToInt32(dataReader1.GetString("Schedule_TimeIn").Substring(3, 2));
            //        empstartampm = dataReader1.GetString("Schedule_TimeIn").Substring(6, 2);
            //        empendhour = Convert.ToInt32(dataReader1.GetString("Schedule_TimeOut").Substring(0, 2));
            //        empendmin = Convert.ToInt32(dataReader1.GetString("Schedule_TimeOut").Substring(3, 2));
            //        empendampm = dataReader1.GetString("Schedule_TimeOut").Substring(6, 2);
            //        if(empstarthour < 12 && empstartampm == "pm"){
            //            empstarthour = empstarthour + 12;
            //        }
            //        if(empendhour < 12 && empendampm == "pm"){
            //            empendhour = empendhour + 12;
            //        }
            //        if(starthour < 12 && startampm == "pm"){
            //            starthour = starthour + 12;
            //        }
            //        if(endhour < 12 && endampm == "pm"){
            //            endhour = endhour + 12;
            //        }
            //        if ((starthour >= empstarthour) && ((endhour <= empendhour && string.Equals(endampm,empendampm,StringComparison.OrdinalIgnoreCase)) || endampm != empendampm))
            //        {
            //            checking = true;
            //            break;
            //        }
            //    }
            //    connection.Close();
            //    if (!checking)
            //    {
            //        label1.Text = "Therapist is not available at this time";
            //        check = true;
            //        checker = true;
            //    }
            //    else
            //    {
            //        label1.Text = "";
                    
            //    }
            
            //}
            //catch (Exception me)
            //{
            //    connection.Close();
            //    MessageBox.Show(me.Message);
            //}

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
                                checking = false;
                                break;
                            }
                        }
                    }
                }
                connection.Close();
                if (check == true && checker == true)
                {
                    label22.Text = "There's already an appointment";
                }
                else
                {
                    label22.Text = "";
                }
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }

            if (checker == false && check == false)
            {
                panel11.Hide();
                button4.Enabled = true;
                panel26.Enabled = true;
                button2.Visible = true;
                button8.Enabled = true;
                button17.Enabled = true;
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            int age = DateTime.Now.Year - dateTimePicker1.Value.Year;
            if (dateTimePicker1.Value.AddYears(age) > DateTime.Now)
            {
                age--;
            }
            textBox5.Text = age.ToString();
            if (age < 13)
            {

                label13.Text = "Age 13+ only";
                textBox5.BackColor = Color.FromArgb(252, 224, 224);
            }
            else if (age > 90)
            {

                label13.Text = "Age too old for a patient";
                textBox5.BackColor = Color.FromArgb(252, 224, 224);
            }
            else
            {
                label13.Text = "";
                textBox5.BackColor = Color.White;
            }
            CheckToNext();
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            string containNumber = @"[0-9~!@#$%^&*()_+=-]";
            string lname = "";
            try
            {
                lname = textBox1.Text;
                if (Regex.IsMatch(lname, containNumber))
                {
                    textBox1.Text = "";
                }
            }
            catch (Exception)
            {
            }
            CheckToNext();
        }

        private void textBox11_Leave(object sender, EventArgs e)
        {
            string email = textBox11.Text.Trim();
            if (string.IsNullOrEmpty(email))
            {

                textBox11.BorderColorIdle = Color.Maroon;
                label6.Text = "Email required";
                textBox11.BackColor = Color.FromArgb(252, 224, 224);
            }
            else
            {
                if (!IsValid(email))
                {
                    textBox11.BorderColorIdle = Color.Maroon;
                    label6.Text = "Invalid email";
                    textBox11.BackColor = Color.FromArgb(252, 224, 224);
                }
                else
                {
                    textBox11.BorderColorIdle = Color.Black;
                    label6.Text = "";
                    textBox11.BackColor = Color.White;
                }
            }
            CheckToNext();
        }
        public void CheckToNext()
        {
            if (string.IsNullOrEmpty(textBox1.Text.Trim()) || string.IsNullOrEmpty(textBox2.Text.Trim()) || string.IsNullOrEmpty(textBox4.Text.Trim())
                    || string.IsNullOrEmpty(textBox11.Text.Trim()) || string.IsNullOrEmpty(textBox5.Text.Trim()) || string.IsNullOrEmpty(textBox6.Text.Trim())
                    || string.IsNullOrEmpty(textBox7.Text.Trim()) || string.IsNullOrEmpty(textBox8.Text.Trim()) || string.IsNullOrEmpty(textBox9.Text.Trim())
                        || !IsValid(textBox11.Text.Trim()) || Convert.ToInt32(textBox5.Text) < 13 || Convert.ToInt32(textBox5.Text) > 90 || checkBox58.Checked == false)
            {
                button19.Enabled = false;
            }
            else
            {
                button19.Enabled = true;
            }
        }
        public void CheckToSubmit()
        {
            if (string.IsNullOrEmpty(textBox1.Text.Trim()) || string.IsNullOrEmpty(textBox2.Text.Trim()) || string.IsNullOrEmpty(textBox4.Text.Trim())
                    || string.IsNullOrEmpty(textBox11.Text.Trim()) || string.IsNullOrEmpty(textBox5.Text.Trim()) || string.IsNullOrEmpty(textBox6.Text.Trim())
                    || string.IsNullOrEmpty(textBox7.Text.Trim()) || string.IsNullOrEmpty(textBox8.Text.Trim()) || string.IsNullOrEmpty(textBox9.Text.Trim())
                        || !IsValid(textBox11.Text.Trim()) || Convert.ToInt32(textBox5.Text) < 13 || string.IsNullOrEmpty(textBox12.Text.Trim()) || string.IsNullOrEmpty(textBox19.Text.Trim())
                || string.IsNullOrEmpty(textBox13.Text.Trim()) || string.IsNullOrEmpty(textBox18.Text.Trim()) || string.IsNullOrEmpty(textBox24.Text.Trim()))
            {
                button21.Enabled = false;
            }
            else
            {
                button21.Enabled = true;
            }
        }
        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text.Trim()))
            {
                textBox1.BorderColorIdle = Color.Maroon;
                label7.Text = "Last Name required";
                textBox1.BackColor = Color.FromArgb(252, 224, 224);
            }
            else
            {
                textBox1.BorderColorIdle = Color.Black;
                label7.Text = "";
                textBox1.BackColor = Color.White;
            }
            CheckToNext();
            CheckToSubmit();
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text.Trim()))
            {
                textBox2.BorderColorIdle = Color.Maroon;
                label8.Text = "First Name required";
                textBox2.BackColor = Color.FromArgb(252, 224, 224);
            }
            else
            {
                textBox2.BorderColorIdle = Color.Black;
                label8.Text = "";
                textBox2.BackColor = Color.White;
            }
            CheckToNext();
            CheckToSubmit();
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text.Trim()))
            {

                label5.Text = "Contact No required";
                textBox4.BackColor = Color.FromArgb(252, 224, 224);
            }
            else
            {
                label5.Text = "";
                textBox4.BackColor = Color.White;
            }
            CheckToNext();
            CheckToSubmit();
        }

        private void dateTimePicker1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox5.Text.Trim()))
            {
                label13.Text = "Pick bday first";
                textBox5.BackColor = Color.FromArgb(252, 224, 224);
            }
            else
            {
                label13.Text = "";
                textBox4.BackColor = Color.White;
            }
            CheckToNext();
            CheckToSubmit();
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox6.Text.Trim()))
            {
                textBox6.BorderColorIdle = Color.Maroon;
                label9.Text = "Lot No required";
                textBox6.BackColor = Color.FromArgb(252, 224, 224);
            }
            else
            {
                textBox6.BorderColorIdle = Color.Black;
                label9.Text = "";
                textBox6.BackColor = Color.White;
            }
            CheckToNext();
            CheckToSubmit();
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox7.Text.Trim()))
            {
                textBox7.BorderColorIdle = Color.Maroon;
                label10.Text = "Street required";
                textBox7.BackColor = Color.FromArgb(252, 224, 224);
            }
            else
            {
                textBox7.BorderColorIdle = Color.Black;
                label10.Text = "";
                textBox7.BackColor = Color.White;
            }
            CheckToNext();
            CheckToSubmit();
        }

        private void textBox8_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox8.Text.Trim()))
            {
                textBox8.BorderColorIdle = Color.Maroon;
                label11.Text = "Barangay required";
                textBox8.BackColor = Color.FromArgb(252, 224, 224);
            }
            else
            {
                textBox8.BorderColorIdle = Color.Black;
                label11.Text = "";
                textBox8.BackColor = Color.White;
            }
            CheckToNext();
            CheckToSubmit();
        }

        private void textBox9_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox9.Text.Trim()))
            {
                textBox9.BorderColorIdle = Color.Maroon;
                label12.Text = "City required";
                textBox9.BackColor = Color.FromArgb(252, 224, 224);
            }
            else
            {
                textBox9.BorderColorIdle = Color.Black;
                label12.Text = "";
                textBox9.BackColor = Color.White;
            }
            CheckToNext();
            CheckToSubmit();
        }
        private void textBox27_TextChanged(object sender, EventArgs e)
        {
            string containLetter = @"[A-Za-z~!@#$%^&*()_+=-]";
            string cno = "";
            try
            {
                cno = textBox27.Text;
                if (Regex.IsMatch(cno, containLetter))
                {
                    textBox27.Text = "";
                }
            }
            catch (Exception)
            {
            }
            CheckToSubmit();
        }
        private void textBox12_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox12.Text.Trim()))
            {
                textBox12.BorderColorIdle = Color.Maroon;
                label14.Text = "Height required";
                textBox12.BackColor = Color.FromArgb(252, 224, 224);
            }
            else
            {
                textBox12.BorderColorIdle = Color.Black;
                label14.Text = "";
                textBox12.BackColor = Color.White;
            }
            CheckToSubmit();
        }
        private void textBox19_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox19.Text.Trim()))
            {
                textBox19.BorderColorIdle = Color.Maroon;
                label15.Text = "Weight required";
                textBox19.BackColor = Color.FromArgb(252, 224, 224);
            }
            else
            {
                textBox19.BorderColorIdle = Color.Black;
                label15.Text = "";
                textBox19.BackColor = Color.White;

            }
            CheckToSubmit();
        }
        private void textBox13_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox13.Text.Trim()))
            {
                textBox13.BorderColorIdle = Color.Maroon;
                label16.Text = "BP required";
                textBox13.BackColor = Color.FromArgb(252, 224, 224);
            }
            else if (string.IsNullOrEmpty(textBox18.Text.Trim()))
            {
                textBox18.BorderColorIdle = Color.Maroon;
                label16.Text = "BP required";
                textBox18.BackColor = Color.FromArgb(252, 224, 224);
            }
            else if (!string.IsNullOrEmpty(textBox13.Text.Trim()))
            {
                textBox13.BorderColorIdle = Color.Black;
                label16.Text = "";
                textBox13.BackColor = Color.White;
            }
            else if (!string.IsNullOrEmpty(textBox18.Text.Trim()))
            {
                textBox18.BorderColorIdle = Color.Black;
                label16.Text = "";
                textBox18.BackColor = Color.White;
            }
            CheckToSubmit();
        }

        private void textBox18_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox13.Text.Trim()))
            {
                textBox13.BorderColorIdle = Color.Maroon;
                label16.Text = "BP required";
                textBox13.BackColor = Color.FromArgb(252, 224, 224);
            }
            else if (string.IsNullOrEmpty(textBox18.Text.Trim()))
            {
                textBox18.BorderColorIdle = Color.Maroon;
                label16.Text = "BP required";
                textBox18.BackColor = Color.FromArgb(252, 224, 224);
            }
            else if (!string.IsNullOrEmpty(textBox13.Text.Trim()))
            {
                textBox13.BorderColorIdle = Color.Black;
                label16.Text = "";
                textBox13.BackColor = Color.White;
            }
            else if (!string.IsNullOrEmpty(textBox18.Text.Trim()))
            {
                textBox18.BorderColorIdle = Color.Black;
                label16.Text = "";
                textBox18.BackColor = Color.White;
            }
            CheckToSubmit();
        }

        private void textBox24_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox24.Text.Trim()))
            {
                textBox24.BorderColorIdle = Color.Maroon;
                label17.Text = "Pulse Rate required";
                textBox24.BackColor = Color.FromArgb(252, 224, 224);
            }
            else
            {
                textBox24.BorderColorIdle = Color.Black;
                label17.Text = "";
                textBox24.BackColor = Color.White;
            }
            CheckToSubmit();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

            string timetoday = DateTime.Now.ToString("hh:mm tt");
            string checktime = "", ampm = timetoday.Substring(6, 2).ToLower(), ampm1 = "am";
            int hourtoday = Convert.ToInt32(timetoday.Substring(0, 2));
            int mintoday = Convert.ToInt32(timetoday.Substring(3, 2));
            int hour = 0, min = 0, maxtime = 12, maxtime1 = 0;
            if (dateTimePicker2.Value.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
            {
                comboBox7.Items.Clear();
                if (ampm == "pm" && hourtoday < 12)
                {
                    maxtime1 = 21 - (hourtoday + 12);
                }
                else if (ampm == "pm" && hourtoday == 12)
                {
                    maxtime1 = 21 - hourtoday;
                }
                else if (ampm == "am")
                {
                    maxtime1 = 21 - hourtoday;
                }
                for (int j = hourtoday; j < maxtime1 + hourtoday; j++)
                {
                    hour = j;
                    if (hour > 12)
                    {
                        hour = hour - 12;
                        ampm = "pm";
                    }
                    if (hour == 12)
                    {
                        hour = j;
                        ampm = "pm";
                    }
                    for (int o = 0; o <= 45; o = o + 15)
                    {
                        if (o > mintoday && j == hourtoday)
                        {
                            min = o;
                            checktime = hour.ToString("D2") + ":" + min.ToString("D2") + " " + ampm;
                            comboBox7.Items.Add(checktime);
                        }
                        else if (j > hourtoday)
                        {
                            checktime = hour.ToString("D2") + ":" + o.ToString("D2") + " " + ampm;
                            comboBox7.Items.Add(checktime);
                        }

                    }
                }
            }
            else
            {
                comboBox7.Items.Clear();
                for (int r = 9; r < maxtime + 9; r++)
                {
                    hour = r;
                    if (r > 12)
                    {
                        hour = hour - 12;
                        ampm1 = "pm";
                    }
                    for (int m = 0; m <= 45; m = m + 15)
                    {
                        checktime = hour.ToString("D2") + ":" + m.ToString("D2") + " " + ampm1;
                        comboBox7.Items.Add(checktime);
                    }
                }
            }
            try
            {
                comboBox7.SelectedIndex = 0;
            }
            catch (Exception)
            {
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                g = Graphics.FromImage(DrawArea);
                current = e.Location;
                g.DrawLine(p, old, current);
                old = current;
                pictureBox1.Invalidate();
            }
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            string starttime = comboBox7.Text;
            GetEndTime(starttime);
        }

        private void textBox40_TextChanged(object sender, EventArgs e)
        {
            string endtime = textBox40.Text;
            int endhour = 0, endmin = 0;
            string endampm = "";
            try
            {
                endhour = Convert.ToInt32(endtime.Substring(0, 2));
                endmin = Convert.ToInt32(endtime.Substring(3, 2));
                endampm = endtime.Substring(6, 2);

            }
            catch (Exception)
            {
                textBox40.Text = "";
            }
            if (endhour >= 10 && endmin > 0 && endampm == "pm" && endhour != 12)
            {
                textBox40.Text = "";
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            string containsNum = @"[0-9~!@#$%^&*()_+=-]";
            string empname = textBox1.Text.Trim();
            if (Regex.IsMatch(textBox1.Text.Trim(), containsNum))
            {
                textBox1.BorderColorIdle = Color.Maroon;
                label7.Text = "No numeric character";
                textBox1.BackColor = Color.FromArgb(252, 224, 224);
            }
            else
            {
                textBox1.BorderColorIdle = Color.Black;
                label7.Text = "";
                textBox1.BackColor = Color.White;
                CheckToNext();
            }
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            string containsNum = @"[0-9~!@#$%^&*()_+=-]";
            string empname = textBox2.Text.Trim();
            if (Regex.IsMatch(textBox2.Text.Trim(), containsNum))
            {
                textBox2.BorderColorIdle = Color.Maroon;
                label8.Text = "No numeric character";
                textBox2.BackColor = Color.FromArgb(252, 224, 224);
            }
            else
            {
                textBox2.BorderColorIdle = Color.Black;
                label8.Text = "";
                textBox2.BackColor = Color.White;
                CheckToNext();
            }
        }

        private void textBox4_KeyUp(object sender, KeyEventArgs e)
        {
            string containsLetter = @"[A-Za-z~!@#$%^&*()_+=-]";
            string cno = textBox4.Text.Trim();
            if (Regex.IsMatch(cno, containsLetter))
            {

                textBox4.BackColor = Color.FromArgb(252, 224, 224);
                label5.Text = "Numeric only";
            }
            else
            {
                label5.Text = "";
                textBox4.BackColor = Color.White;
                CheckToNext();
            }
        }

        private void textBox12_KeyUp(object sender, KeyEventArgs e)
        {
            string containsLetter = @"[A-Za-z~!@#$%^&*()'_+=-]";
            string cno = textBox12.Text.Trim();
            if (Regex.IsMatch(cno, containsLetter))
            {
                textBox12.BorderColorIdle = Color.Maroon;
                textBox12.BackColor = Color.FromArgb(252, 224, 224);
                label14.Text = "Numeric only";
            }
            else
            {
                textBox12.BorderColorIdle = Color.Black;
                label14.Text = "";
                textBox12.BackColor = Color.White;
                CheckToSubmit();
            }
        }

        private void textBox19_KeyUp(object sender, KeyEventArgs e)
        {
            string containsLetter = @"[A-Za-z~!@#$%^&*()_+=-]";
            string cno = textBox19.Text.Trim();
            if (Regex.IsMatch(cno, containsLetter))
            {
                textBox19.BorderColorIdle = Color.Maroon;
                textBox19.BackColor = Color.FromArgb(252, 224, 224);
                label15.Text = "Numeric only";
            }
            else
            {
                textBox19.BorderColorIdle = Color.Black;
                label15.Text = "";
                textBox19.BackColor = Color.White;
                CheckToSubmit();
            }
        }

        private void textBox13_KeyUp(object sender, KeyEventArgs e)
        {
            string containsLetter = @"[A-Za-z~!@#$%^&*()_.,<>/\|+=-]";
            string cno = textBox13.Text.Trim();
            if (Regex.IsMatch(cno, containsLetter))
            {
                textBox13.BorderColorIdle = Color.Maroon;
                textBox13.BackColor = Color.FromArgb(252, 224, 224);
                label16.Text = "Numeric only";
            }

            else
            {
                textBox13.BorderColorIdle = Color.Black;
                label16.Text = "";
                textBox13.BackColor = Color.White;
                CheckToSubmit();
            }
        }

        private void textBox18_KeyUp(object sender, KeyEventArgs e)
        {
            string containsLetter = @"[A-Za-z~!@#$%^&*(),.<>/\?'_+=-]";
            string cno = textBox18.Text.Trim();
            if (Regex.IsMatch(cno, containsLetter))
            {
                textBox18.BorderColorIdle = Color.Maroon;
                textBox18.BackColor = Color.FromArgb(252, 224, 224);
                label16.Text = "Numeric only";
            }
            else
            {
                textBox18.BorderColorIdle = Color.Black;
                label16.Text = "";
                textBox18.BackColor = Color.White;
                CheckToSubmit();
            }
        }

        private void textBox24_KeyUp(object sender, KeyEventArgs e)
        {
            string containsLetter = @"[A-Za-z~!@#$%^&*().,<>/\|_+=-]";
            string cno = textBox24.Text.Trim();
            if (Regex.IsMatch(cno, containsLetter))
            {
                textBox24.BorderColorIdle = Color.Maroon;
                textBox24.BackColor = Color.FromArgb(252, 224, 224);
                label17.Text = "Numeric only";
            }
            else
            {
                textBox24.BorderColorIdle = Color.Black;
                label17.Text = "";
                textBox24.BackColor = Color.White;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string service = "";
            service = comboBox1.Text;
            try
            {
                connection.Open();
                string query = "SELECT * from servicetbl where Service_Status = 'Active' and Service_Name = '" + service + "' order by Service_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    textBox39.Text = dataReader.GetDecimal("Service_Fee").ToString();
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            try
            {
                comboBox7.SelectedIndex = 0;
            }
            catch (Exception) { }
            string start = comboBox7.Text;
            GetEndTime(start);
            GetMachines(service);
        }
        private void cancelBtn_Click(object sender, EventArgs e)
        {
            label1.Text = "";
            label19.Text = "";
            label22.Text = "";
            label20.Text = "";
            label21.Text = "";
            button4.Enabled = true;
            panel25.Enabled = true;
            button2.Enabled = true;
            button8.Enabled = true;
            button17.Enabled = true;
            button9.Enabled = true;
            button18.Enabled = false;
            panel11.Hide();
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        private void textBox11_Leave(object sender, KeyEventArgs e)
        {

        }

        private void checkBox30_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string patient = comboBox3.Text.Trim();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT *,RTRIM(CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)) from patienttbl where RTRIM(CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)) = '" + patient + "'", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    label4.Text = dataReader.GetInt32("Patient_No").ToString();
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }

        private void textBox1_OnValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_OnValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_OnValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox67_KeyUp(object sender, KeyEventArgs e)
        {
           
        }

        private void textBox27_KeyUp(object sender, KeyEventArgs e)
        {
            string containsLetter = @"[A-Za-z~!@#$%^&*()_+=-]";
            string cno = textBox27.Text.Trim();
            if (Regex.IsMatch(cno, containsLetter))
            {
                textBox27.BorderColorIdle = Color.Maroon;
                textBox27.BackColor = Color.FromArgb(252, 224, 224);
                label27.Text = "Numeric only";
            }
            else
            {
                textBox27.BorderColorIdle = Color.Black;
                label27.Text = "";
                textBox27.BackColor = Color.White;
                CheckToSubmit();
            }
        }
    }
}
