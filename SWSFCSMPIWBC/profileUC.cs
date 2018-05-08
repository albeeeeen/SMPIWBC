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
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace SWSFCSMPIWBC
{
    public partial class profileUC : UserControl
    {
        static string connectionString =
       System.Configuration.ConfigurationManager.
       ConnectionStrings["SWSFCSMPIWBC.Properties.Settings.slimmersdbConnectionString"].ConnectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);
        public profileUC()
        {
            InitializeComponent();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            checkBox3.Enabled = true;
            checkBox4.Enabled = true;
            checkBox5.Enabled = true;
            checkBox6.Enabled = true;
            checkBox7.Enabled = true;
            textBox47.Enabled = true;
            checkBox1.Enabled = true;
            checkBox2.Enabled = true;
            checkBox23.Enabled = true;
            checkBox24.Enabled = true;
            richTextBox1.Enabled = true;
            checkBox8.Enabled = true;
            checkBox9.Enabled = true;
            checkBox10.Enabled = true;
            checkBox11.Enabled = true;
            checkBox18.Enabled = true;
            checkBox19.Enabled = true;
            checkBox20.Enabled = true;
            checkBox21.Enabled = true;
            checkBox22.Enabled = true;
            checkBox12.Enabled = true;
            checkBox13.Enabled = true;
            checkBox14.Enabled = true;
            checkBox15.Enabled = true;
            checkBox16.Enabled = true;
            checkBox17.Enabled = true;
            pictureBox1.Enabled = true;
            bunifuThinButton22.Enabled = true;
        }
        private void button19_Click(object sender, EventArgs e)
        {
            panel5.Show();
            panel6.Hide();
            panel11.Hide();
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox4.ReadOnly = true;
            textBox10.ReadOnly = true;
            textBox11.ReadOnly = true;
            textBox6.ReadOnly = true;
            textBox1.BorderStyle = BorderStyle.None;
            textBox2.BorderStyle = BorderStyle.None;
            textBox3.BorderStyle = BorderStyle.None;
            textBox4.BorderStyle = BorderStyle.None;
            textBox10.BorderStyle = BorderStyle.None;
            textBox11.BorderStyle = BorderStyle.None;
            textBox6.BorderStyle = BorderStyle.None;
            panel8.Enabled = false;
            dateTimePicker1.Enabled = false;
            panel7.Enabled = false;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            panel6.Show();
            panel5.Hide();
            panel11.Hide();
        }
        private void button18_Click(object sender, EventArgs e)
        {
            panel6.Show();
            panel5.Hide();
            panel11.Hide();

            slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Top;
            slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Height;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            panel11.Show();
            panel6.Hide();
            panel5.Hide();
            int patientno = 0;
            try
            {
                patientno = Convert.ToInt32(label2.Text);
            }
            catch (Exception me)
            {
            }
            MessageBox.Show(patientno.ToString());
            try
            {
                connection.Open();
                string query = "SELECT *,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from patienttbl p, patient_demtbl pd where p.Patient_No = '" + patientno + "' and p.Patient_No = pd.Patient_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    label16.Text = dataReader.GetInt32("Patient_No").ToString();
                    label17.Text = dataReader.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)");
                    pictureBox1.Image = Image.FromFile(dataReader.GetString("Dem_Picture"));
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            panel11.Enabled = true;
            slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Top;
            slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Height;
        }
        private void button1_Click_2(object sender, EventArgs e)
        {
            textBox1.ReadOnly = false;
            textBox2.ReadOnly = false;
            textBox3.ReadOnly = false;
            textBox4.ReadOnly = false;
            textBox10.ReadOnly = false;
            textBox11.ReadOnly = false;
            textBox6.ReadOnly = false;
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox2.BorderStyle = BorderStyle.FixedSingle;
            textBox3.BorderStyle = BorderStyle.FixedSingle;
            textBox4.BorderStyle = BorderStyle.FixedSingle;
            textBox10.BorderStyle = BorderStyle.FixedSingle;
            textBox11.BorderStyle = BorderStyle.FixedSingle;
            textBox6.BorderStyle = BorderStyle.FixedSingle;
            textBox29.ReadOnly = false;
            textBox29.BorderStyle = BorderStyle.FixedSingle;
            textBox28.ReadOnly = false;
            textBox28.BorderStyle = BorderStyle.FixedSingle;
            textBox27.ReadOnly = false;
            textBox27.BorderStyle = BorderStyle.FixedSingle;
            textBox25.ReadOnly = false;
            textBox25.BorderStyle = BorderStyle.FixedSingle;
            textBox24.ReadOnly = false;
            textBox24.BorderStyle = BorderStyle.FixedSingle;
            comboBox1.Enabled = true;
            panel9.Enabled = true;
            panel4.Enabled = true;
            checkBox37.Enabled = true;
            checkBox36.Enabled = true;
            checkBox35.Enabled = true;
            checkBox34.Enabled = true;
            checkBox33.Enabled = true;
            checkBox32.Enabled = true;
            checkBox31.Enabled = true;
            checkBox30.Enabled = true;
            panel17.Enabled = true;
            panel18.Enabled = true;
            panel19.Enabled = true;
            panel20.Enabled = true;
            panel21.Enabled = true;
            panel22.Enabled = true;
            panel23.Enabled = true;
            panel24.Enabled = true;
            panel8.Enabled = true;
            dateTimePicker1.Enabled = true;
            panel7.Enabled = true;
            comboBox2.Enabled = true;
            textBox31.BorderStyle = BorderStyle.FixedSingle;
            textBox31.ReadOnly = false;
            textBox30.BorderStyle = BorderStyle.FixedSingle;
            textBox30.ReadOnly = false;
            button1.Hide();
            button4.Show();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            HomePage hp = new HomePage();
            string user = hp.label15.Text;
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
            address = textBox6.Text.Trim();
            bdate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string email = textBox11.Text.Trim();
            occupation = textBox10.Text.Trim();
            int consultantno = 0;
            int employee_patientno = 0;
            string status = comboBox2.Text;
            if (!IsValid(email))
            {
                label28.Text = "Invalid email";
                textBox11.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
                label38.Text = "**There's a problem with your personal information";
            }
            else
            {
                textBox11.BackColor = Color.White;
                label28.Text = "";
                label38.Text = "";
            }

            if (lname.Length == 0)
            {
                check = true;
                label3.Text = "You must enter last name";
                textBox1.BackColor = Color.FromArgb(252, 224, 224);
                label38.Text = "**There's a problem with your personal information";
            }
            else
            {

                if (Regex.IsMatch(lname, containNumber))
                {
                    check = true;
                    label3.Text = "Last name format invalid";
                    textBox1.BackColor = Color.FromArgb(252, 224, 224);
                    label38.Text = "**There's a problem with your personal information";
                }
                else
                {
                    label3.Text = "";
                    textBox1.BackColor = Color.White;
                    label38.Text = "";
                }
            }

            if (fname.Length == 0)
            {
                check = true;
                label18.Text = "You must enter first name";
                textBox2.BackColor = Color.FromArgb(252, 224, 224);
                label38.Text = "**There's a problem with your personal information";
            }
            else
            {

                if (Regex.IsMatch(fname, containNumber))
                {
                    check = true;
                    label18.Text = "First name format invalid";
                    textBox2.BackColor = Color.FromArgb(252, 224, 224);
                    label38.Text = "**There's a problem with your personal information";
                }
                else
                {
                    label18.Text = "";
                    textBox2.BackColor = Color.White;
                    label38.Text = "";
                }
            }

            if (Regex.IsMatch(mi, containNumber))
            {
                check = true;
                label22.Text = "Invalid Mid Init";
                textBox3.BackColor = Color.FromArgb(252, 224, 224);
                label38.Text = "**There's a problem with your personal information";
            }
            else
            {
                label22.Text = "";
                textBox3.BackColor = Color.White;
                label38.Text = "";
            }
            if (string.IsNullOrEmpty(address))
            {
                check = true;
                label30.Text = "Address required";
                label38.Text = "**There's a problem with your personal information";
                textBox6.BackColor = Color.FromArgb(252, 224, 224);
            }
            else
            {
                label30.Text = "";
                label38.Text = "";
                textBox6.BackColor = Color.White;
            }
            try
            {
                age = Convert.ToInt32(textBox5.Text.Trim());
                label29.Text = "";
                label38.Text = "";
            }
            catch (FormatException)
            {
                check = true;
                label29.Text = "Age is required. Please check your birthdate";
                label38.Text = "**There's a problem with your personal information";
            }
            if (age < 16)
            {
                check = true;
                label29.Text = "Age is not valid it should be 16+";
                textBox5.BackColor = Color.FromArgb(252, 224, 224);
                label38.Text = "**There's a problem with your personal information";
            }
            else
            {
                label29.Text = "";
                textBox5.BackColor = Color.White;
                label38.Text = "";
            }
            try
            {
                cno = Convert.ToInt64(textBox4.Text.Trim());
                label24.Text = "";
                textBox4.BackColor = Color.White;
                label38.Text = "";
            }
            catch (FormatException)
            {
                check = true;
                label24.Text = "Contact number format invalid";
                textBox4.BackColor = Color.FromArgb(252, 224, 224);
                label38.Text = "**There's a problem with your personal information";
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
            string bodyframe = "", bp = "", prate = "", smoke = "";
            int medno = 0;
            string bodyfat = "";
            string bp1 = "", bp2 = "", emp_status = "";
            string containLetter = @"[A-Za-z~!@#$%^&*()_+=-]";
            bodyfat = textBox27.Text.Trim();
            height = textBox29.Text.Trim();
            weight = textBox28.Text.Trim();
            bp = textBox25.Text.Trim();
            prate = textBox24.Text.Trim();

            if (string.IsNullOrEmpty(height))
            {
                label31.Text = "Required height";
                textBox29.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }
            else
            {
                if (Regex.IsMatch(height, containLetter))
                {
                    label31.Text = "Invalid height";
                    textBox29.BackColor = Color.FromArgb(252, 224, 224);
                    check = true;
                }
                else
                {
                    label31.Text = "";
                    textBox29.BackColor = Color.White;
                }
            }
            if (string.IsNullOrEmpty(weight))
            {
                label32.Text = "Required weight";
                textBox28.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }
            else
            {
                if (Regex.IsMatch(weight, containLetter))
                {
                    label32.Text = "Invalid weight";
                    textBox28.BackColor = Color.FromArgb(252, 224, 224);
                    check = true;

                }
                else
                {
                    label32.Text = "";
                    textBox28.BackColor = Color.White;
                }
            }
            bodyframe = comboBox1.Text.Trim();
            if (Regex.IsMatch(bodyfat, containLetter))
            {
                label34.Text = "Invalid bodyfat";
                textBox27.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }
            else
            {
                label34.Text = "";
                textBox27.BackColor = Color.White;
            }

            if (string.IsNullOrEmpty(bp))
            {
                label35.Text = "Required blood pressure";
                textBox25.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }
            else
            {
                if (Regex.IsMatch(bp, containLetter))
                {
                    label35.Text = "Invalid blood pressure";
                    textBox25.BackColor = Color.FromArgb(252, 224, 224);
                    check = true;
                }
                else
                {
                    label35.Text = "";
                    textBox25.BackColor = Color.White;
                }
            }

            if (string.IsNullOrEmpty(prate))
            {
                label36.Text = "Required pulse rate";
                textBox24.BackColor = Color.FromArgb(252, 224, 224);
                check = true;

            }
            else
            {
                if (Regex.IsMatch(prate, containLetter))
                {
                    label36.Text = "Invalid pulse rate";
                    textBox24.BackColor = Color.FromArgb(252, 224, 224);
                    check = true;
                }
                else
                {
                    label36.Text = "";
                    textBox24.BackColor = Color.White;
                }
            }
            if (radioButton14.Checked)
            {
                smoke = "Smoker";
            }
            else
            {
                smoke = "Non Smoker";
            }
            risks.Add(smoke);
            if (radioButton12.Checked)
            {
                risks.Add("Alcohol Drinker");
            }
            else
            {
                risks.Add("Not Alcohol Drinker");
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
            if (radioButton5.Checked)
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
            allrisk = allrisk.Substring(0, allrisk.Length - 2);
            bool proceed = false;
            if (check == false)
            {

                try
                {
                    connection.Open();
                    string query2 = "Update patienttbl set Patient_LName = '" + lname + "', Patient_FName = '" + fname + "', Patient_MidInit = '" + mi + "',Patient_Gender = '" + gender + "',Patient_BirthDate = '" + bdate + "',Patient_Age = '" + age + "',Patient_Address = '" + address + "',Patient_ContactNo = '" + cno + "',Patient_Email = '" + email + "',Patient_CStatus = '" + cstatus + "',Patient_Occupation = '" + occupation + "', Patient_Status = '" + status + "' where Patient_No = '" + patientno + "'";
                    MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                    cmd2.ExecuteNonQuery();
                    connection.Close();

                    connection.Open();
                    string query1 = "Update patient_medicaltbl set Height = '" + height + "',Weight = '" + weight + "',Body_Fat = '" + bodyfat + "',Body_Frame = '" + bodyframe + "',Blood_Pressure = '" + bp + "',Pulse_Rate = '" + prate + "',Risk_Factors = '" + allrisk + "',Other_Risks = '" + others + "',Other_MedHist = '" + othermedhist + "' where Patient_No = '" + patientno + "'";
                    MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                    cmd1.ExecuteNonQuery();
                    connection.Close();


                }
                catch (MySqlException me)
                {
                    connection.Close();
                    MessageBox.Show(me.Message);
                }
                MessageBox.Show("Patient record udpated!");
                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;
                textBox4.ReadOnly = true;
                textBox10.ReadOnly = true;
                textBox11.ReadOnly = true;
                textBox6.ReadOnly = true;
                textBox1.BorderStyle = BorderStyle.None;
                textBox2.BorderStyle = BorderStyle.None;
                textBox3.BorderStyle = BorderStyle.None;
                textBox4.BorderStyle = BorderStyle.None;
                textBox10.BorderStyle = BorderStyle.None;
                textBox11.BorderStyle = BorderStyle.None;
                textBox6.BorderStyle = BorderStyle.None;
                textBox29.ReadOnly = true;
                textBox29.BorderStyle = BorderStyle.None;
                textBox28.ReadOnly = true;
                textBox28.BorderStyle = BorderStyle.None;
                textBox27.ReadOnly = true;
                textBox27.BorderStyle = BorderStyle.None;
                textBox25.ReadOnly = true;
                textBox25.BorderStyle = BorderStyle.None;
                textBox24.ReadOnly = true;
                textBox24.BorderStyle = BorderStyle.None;
                comboBox1.Enabled = false;
                panel9.Enabled = false;
                panel4.Enabled = false;
                checkBox37.Enabled = false;
                checkBox36.Enabled = false;
                checkBox35.Enabled = false;
                checkBox34.Enabled = false;
                checkBox33.Enabled = false;
                checkBox32.Enabled = false;
                checkBox31.Enabled = false;
                checkBox30.Enabled = false;
                panel17.Enabled = false;
                panel18.Enabled = false;
                panel19.Enabled = false;
                panel20.Enabled = false;
                panel21.Enabled = false;
                panel22.Enabled = false;
                panel23.Enabled = false;
                panel24.Enabled = false;
                panel8.Enabled = false;
                dateTimePicker1.Enabled = false;
                panel7.Enabled = false;
                comboBox2.Enabled = false;
                textBox31.BorderStyle = BorderStyle.None;
                textBox31.ReadOnly = true;
                textBox30.BorderStyle = BorderStyle.None;
                textBox30.ReadOnly = true;
                button1.Show();
                button4.Hide();
            }
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
        
        private void checkBox30_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox30.Checked)
            {
                textBox30.Text = "";
                textBox30.BorderStyle = BorderStyle.None;
                textBox30.ReadOnly = true;
            }
            else
            {
                textBox30.BorderStyle = BorderStyle.FixedSingle;
                textBox30.ReadOnly = false;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {

            string checkdate = "", checkstart = "", checkend = "", startampm = "", endampm = "", checkstartampm = "", checkendampm = "", checkmachine = "", checkemp = "";
            int starthour = 0, startmin = 0, endhour = 0, endmin = 0, checkstarthour = 0, checkstartmin = 0, checkendhour = 0, checkendmin = 0;
            bool check = false, checker = false;
            string machinename = "", servicename = "", consultant = "", therapist = "";
            int machineno = 0, serviceno = 0, consultantno = 0, therapistno = 0;
            int patientno = 0;
            int demno = 0;
            bool haspatient = false;
            patientno = Convert.ToInt32(label16.Text);
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from patient_demtbl where Patient_No = '" + patientno + "'", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    demno = dataReader.GetInt32("Dem_No");
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
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
            HomePage hp = new HomePage();
            string user = hp.label15.Text;
            string skintypes = "", acnes = "", hyperpigments = "", warts = "", recommendations = "";
            List<string> skintypearray = new List<string>();
            List<string> acnearray = new List<string>();
            List<string> hyperpigmentarray = new List<string>();
            List<string> wartsarray = new List<string>();
            string allergies = "", frownlines = "", finelines = "", wrinkles = "", sagging = "";
            allergies = textBox47.Text.Trim();
            string filepath = Environment.CurrentDirectory;
            string demimagepath = "";
            Bitmap bmp = new Bitmap(pictureBox1.ClientSize.Width, pictureBox1.ClientSize.Height);
            pictureBox1.DrawToBitmap(bmp, pictureBox1.ClientRectangle);
            if (Directory.Exists(filepath + "/Dempic"))
            {
                demimagepath = filepath + "/DemPic/Dem" + demno + ".jpg";
                bmp.Save(demimagepath);

            }
            else
            {
                Directory.CreateDirectory(filepath + "/DemPic");
                demimagepath = filepath + "/DemPic/Dem" + demno + ".jpg";
                bmp.Save(demimagepath);
            }
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
            catch (Exception)
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
            catch (Exception)
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
            catch (Exception)
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
                MySqlCommand cmd1 = new MySqlCommand("Update patient_demtbl set Dem_Allergies = '" + allergies + "', Dem_Frownlines = '" + frownlines + "', Dem_Wrinkles = '" + wrinkles + "', Dem_Sagging = '" + sagging + "', Dem_SkinType = '" + skintypes + "' ,Dem_Acne = '" + acnes + "' , Dem_Warts = '" + warts + "', Dem_Hy = '" + hyperpigments + "', Dem_Reco = '" + recommendations + "', Dem_Picture = '" + demimagepath + "' where Dem_No = '" + demno + "'", connection);
                cmd1.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }


        }

        private void panel30_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void profileUC_Load(object sender, EventArgs e)
        {

        }
    }
}
