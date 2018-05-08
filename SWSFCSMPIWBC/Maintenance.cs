using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SWSFCSMPIWBC
{
    public partial class Maintenance : Form
    {
        static string connectionString = "datasource=localhost" + ";" + "DATABASE=slimmersdb" + ";" + "UID=root"
         + ";" + "PASSWORD=''" + ";";
        MySqlConnection connection = new MySqlConnection(connectionString);
        public Maintenance()
        {
            InitializeComponent();
           
        }
        public void GetAllPatient()
        {
            try
            {
                connection.Open();
                string query5 = "SELECT Patient_No,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from patienttbl order by Patient_No";
                MySqlCommand cmd5 = new MySqlCommand(query5, connection);
                MySqlDataReader dataReader5 = cmd5.ExecuteReader();
                while (dataReader5.Read())
                {
                    dataGridView1.Rows.Add(dataReader5.GetInt32("Patient_No"), dataReader5.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)"));
                }
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();
        }
        private void Maintenance_Load(object sender, EventArgs e)
        {
            panel10.BringToFront();
            panel9.SendToBack();
            try
            {
                connection.Open();
                string query3 = "SELECT Patient_No,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from patienttbl";
                MySqlCommand cmd3 = new MySqlCommand(query3, connection);
                MySqlDataReader dataReader3 = cmd3.ExecuteReader();
                while (dataReader3.Read())
                {
                    dataGridView1.Rows.Add(dataReader3.GetInt32("Patient_No"), dataReader3.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)"));
                }
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();

            try
            {
                connection.Open();
                string query4 = "select * from patienttbl order by Patient_No LIMIT 1";
                MySqlCommand cmd4 = new MySqlCommand(query4, connection);
                MySqlDataReader dataReader4 = cmd4.ExecuteReader();
                while (dataReader4.Read())
                {
                    textBox18.Text = dataReader4.GetString("Patient_LName");
                    textBox17.Text = dataReader4.GetString("Patient_FName");
                    textBox16.Text = dataReader4.GetString("Patient_MidInit");
                    dateTimePicker2.Value = DateTime.Parse(dataReader4.GetString("Patient_Birthdate"));
                    textBox15.Text = dataReader4.GetInt32("Patient_Age").ToString();
                    textBox14.Text = dataReader4.GetInt64("Patient_ContactNo").ToString();
                    textBox13.Text = dataReader4.GetString("Patient_Address");
                    if (dataReader4.GetString("Patient_Gender").Equals("Male"))
                    {
                        radioButton4.Checked = true;
                    }
                    else
                    {
                        radioButton3.Checked = true;
                    }
                    if (dataReader4.GetString("Patient_CStatus").Equals("Single"))
                    {
                        radioButton7.Checked = true;
                    }
                    else if (dataReader4.GetString("Patient_CStatus").Equals("Married"))
                    {
                        radioButton8.Checked = true;
                    }
                    else
                    {
                        radioButton9.Checked = true;
                    }
                    textBox25.Text = dataReader4.GetString("Patient_Occupation");
                }
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            Machine mach = new Machine();
            mach.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
            Product prod = new Product();
            prod.Show();
            this.Hide();
        }
        private void button9_Click(object sender, EventArgs e)
        {

            Promo dp = new Promo();
            dp.Show();
            this.Hide();
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

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
        private void button8_Click(object sender, EventArgs e)
        {
            HomePage hp = new HomePage();
            hp.Show();
            this.Hide();
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            Services services = new Services();
            services.Show();
            this.Hide();
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }


        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {
            
        }


        private void button3_Click(object sender, EventArgs e)
        {
            
            Employee emp = new Employee();
            emp.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Maintenance mainte = new Maintenance();
            mainte.Show();
            this.Hide();
        }
        Byte[] ImageByte;
        private void button6_Click(object sender, EventArgs e)
        {
            panel9.BringToFront();
            connection.Open();
            
            string query1 = "SELECT Dem_Picture from patient_demtbl";
            MySqlCommand cmd1 = new MySqlCommand(query1, connection);
            MySqlDataReader dataReader1 = cmd1.ExecuteReader();
            while (dataReader1.Read())
            {
                ImageByte = (Byte[])(dataReader1["Dem_Picture"]);

            }
            if (ImageByte != null)
            {
                pictureBox1.Image = ByteToImage(ImageByte);
                pictureBox1.Refresh();
            }
            connection.Close();
           
        }
        public static Bitmap ByteToImage(byte[] byteArray)
        {
            MemoryStream mStream = new MemoryStream();
            byte[] pData = byteArray;
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();
            return bm;

        }
        bool witherror = false;
        ErrorProvider errorProvider = new ErrorProvider();
        private void textBox1_Leave(object sender, EventArgs e)
        {
            
            string lname = textBox1.Text.Trim();
            bool containsNum = Regex.IsMatch(lname, @"[0-9~!@#$%^&*()_+=-]");
            if (lname.Equals("") || lname == "")
            {
                errorProvider.SetError(textBox1, "You must enter last name");
               
            }
            else
            {
                if (containsNum)
                {
                    errorProvider.SetError(textBox1, "Invalid format");
                    
                }
                else
                {
                    errorProvider.SetError(textBox1, string.Empty);
                }
                
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            string fname = textBox2.Text.Trim();
            bool containsNum = Regex.IsMatch(fname, @"[0-9~!@#$%^&*()_+=-]");
            if (fname.Equals("") || fname == "")
            {
                errorProvider.SetError(textBox2, "You must enter first name");
               
            }
            else
            {
                if (containsNum)
                {
                    errorProvider.SetError(textBox2, "Invalid format");
                   
                }
                else
                {
                    errorProvider.SetError(textBox2, string.Empty);
                }

            }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            string mi = textBox3.Text.Trim();
            bool containsNum = Regex.IsMatch(mi, @"[0-9~!@#$%^&*()_+=-]");
            if (containsNum)
                {
                    errorProvider.SetError(textBox3, "Invalid format");
                    
                }
                else
                {
                    errorProvider.SetError(textBox3, string.Empty);
                }

            
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            long cno = 0;
            try
            {
                cno = Convert.ToInt64(textBox4.Text);
                if (cno.ToString().Length < 10)
                {
                    errorProvider.SetError(textBox4, "Invalid Contact Number");
                    
                }
                else
                {
                    errorProvider.SetError(textBox4, string.Empty);
                }
            }
            catch (FormatException)
            {
                errorProvider.SetError(textBox4, "Invalid Contact Number Format");
               
            }
            
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            textBox4.Text = "";
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            string st = textBox7.Text.Trim();
            if (st.Equals("") || st == "" || st.Length == 0)
            {
                errorProvider.SetError(textBox7, "Street/Subdivision name is requried");
               
            }
            else
            {
                errorProvider.SetError(textBox7, string.Empty);
            }
        }

        private void textBox8_Leave(object sender, EventArgs e)
        {
            string brgy = textBox8.Text.Trim();
            if (brgy.Equals("") || brgy == "" || brgy.Length == 0)
            {
                errorProvider.SetError(textBox8, "Barangay name is requried");
                
            }
            else
            {
                errorProvider.SetError(textBox8, string.Empty);
            }
        }

        private void textBox9_Leave(object sender, EventArgs e)
        {
            string city = textBox9.Text.Trim();
            bool containsNum = Regex.IsMatch(city, @"[0-9~!@#$%^&*()_+=-]");
            if (city.Equals("") || city == "" || city.Length == 0)
            {
                errorProvider.SetError(textBox9, "City name is requried");
            }
            else
            {
                if (containsNum)
                {
                    errorProvider.SetError(textBox9, "Invalid City Name");
                  
                }
                else
                {
                    errorProvider.SetError(textBox9, string.Empty);
                }
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
            if (textBox5.Text.Trim().Length != 0)
            {
                errorProvider.SetError(textBox5, string.Empty);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox18.ReadOnly = false;
            textBox17.ReadOnly = false;
            textBox16.ReadOnly = false;
            dateTimePicker2.Enabled = true;
            textBox14.ReadOnly = false;
            textBox13.ReadOnly = false;
            textBox10.ReadOnly = false;
            button10.Enabled = true;
            textBox25.ReadOnly = false;
            radioButton4.Enabled = true;
            radioButton3.Enabled = true;
            radioButton7.Enabled = true;
            radioButton8.Enabled = true;
            radioButton9.Enabled = true;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            panel10.BringToFront();
            panel9.SendToBack();
            int pno = 0;
            int rows = 0;
            rows = dataGridView1.CurrentCell.RowIndex;
            pno = Convert.ToInt32(dataGridView1.Rows[rows].Cells[0].Value);
            try
            {
                connection.Open();
                string query4 = "select * from patienttbl where Patient_No = '" + pno + "' order by Patient_No";
                MySqlCommand cmd4 = new MySqlCommand(query4, connection);
                MySqlDataReader dataReader4 = cmd4.ExecuteReader();
                while (dataReader4.Read())
                {
                    textBox18.Text = dataReader4.GetString("Patient_LName");
                    textBox17.Text = dataReader4.GetString("Patient_FName");
                    textBox16.Text = dataReader4.GetString("Patient_MidInit");
                    dateTimePicker2.Value = DateTime.Parse(dataReader4.GetString("Patient_Birthdate"));
                    textBox15.Text = dataReader4.GetInt32("Patient_Age").ToString();
                    textBox14.Text = dataReader4.GetInt64("Patient_ContactNo").ToString();
                    textBox13.Text = dataReader4.GetString("Patient_Address");
                    if (dataReader4.GetString("Patient_Gender").Equals("Male"))
                    {
                        radioButton4.Checked = true;
                    }
                    else
                    {
                        radioButton3.Checked = true;
                    }
                    if (dataReader4.GetString("Patient_CStatus").Equals("Single"))
                    {
                        radioButton7.Checked = true;
                    }
                    else if (dataReader4.GetString("Patient_CStatus").Equals("Married"))
                    {
                        radioButton8.Checked = true;
                    }
                    else
                    {
                        radioButton9.Checked = true;
                    }
                    textBox25.Text = dataReader4.GetString("Patient_Occupation");
                }
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            int age = DateTime.Now.Year - dateTimePicker2.Value.Year;
            if (dateTimePicker2.Value.AddYears(age) > DateTime.Now)
            {
                age--;
            }
            textBox15.Text = age.ToString();
            if (textBox15.Text.Trim().Length != 0)
            {
                errorProvider.SetError(textBox15, string.Empty);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {

            bool check = false;
            string lname, fname, mi, gender, lno, st, brgy, city, address, bdate;
            long cno = 0;
            int age = 0, pno = 0,rows=0;
            string containNumber = @"[0-9~!@#$%^&*()_+=-]";
            string cstatus, occupation;
            rows = dataGridView1.CurrentCell.RowIndex;
            pno = Convert.ToInt32(dataGridView1.Rows[rows].Cells[0].Value);
            lname = textBox18.Text.Trim();
            fname = textBox17.Text.Trim();
            mi = textBox16.Text.Trim();

            address = textBox13.Text.Trim();
            bdate = dateTimePicker2.Value.ToString("yyyy-MM-dd");
            string email = textBox10.Text.Trim();
            if (!IsValid(email))
            {
                errorProvider.SetError(textBox10, "Invalid email");
            }
            else
            {
                errorProvider.SetError(textBox10, string.Empty);
            }
            if (lname.Length == 0)
            {
                check = true;
                errorProvider.SetError(textBox18, "You must enter last name");
            }
            else
            {

                if (Regex.IsMatch(lname, containNumber))
                {
                    check = true;
                    errorProvider.SetError(textBox18, "Last name format invalid");
                }
                else
                {
                    errorProvider.SetError(textBox18, string.Empty);
                }
            }

            if (fname.Length == 0)
            {
                check = true;
                errorProvider.SetError(textBox17, "You must enter first name");
            }
            else
            {

                if (Regex.IsMatch(fname, containNumber))
                {
                    check = true;
                    errorProvider.SetError(textBox17, "First name format invalid");
                }
                else
                {
                    errorProvider.SetError(textBox17, string.Empty);
                }
            }

            if (Regex.IsMatch(mi, containNumber))
            {
                check = true;
                errorProvider.SetError(textBox16, "Middle initial format invalid");
            }
            else
            {
                errorProvider.SetError(textBox16, string.Empty);
            }

            if (address.Length == 0)
            {
                check = true;
                errorProvider.SetError(textBox13, "You must enter your address");
            }
            else
            {
                errorProvider.SetError(textBox13, string.Empty);

            }


            try
            {
                age = Convert.ToInt32(textBox15.Text.Trim());
                errorProvider.SetError(textBox15, string.Empty);
            }
            catch (FormatException)
            {
                check = true;
                errorProvider.SetError(textBox15, "Please check your birthdate.");
            }
            try
            {
                cno = Convert.ToInt64(textBox14.Text.Trim());
                errorProvider.SetError(textBox14, string.Empty);
            }
            catch (FormatException)
            {
                check = true;
                errorProvider.SetError(textBox14, "Contact number format invalid");
            }
            if (radioButton4.Checked)
            {
                gender = "Male";
            }
            else
            {
                gender = "Female";
            }
            if (radioButton7.Checked)
            {
                cstatus = "Single";
            }
            else if (radioButton8.Checked)
            {
                cstatus = "Married";
            }
            else
            {
                cstatus = "Widowed";
            }
            occupation = textBox25.Text;
            if (check == false)
            {

                try
                {
                    connection.Open();
                    string query1 = "UPDATE patienttbl set Patient_LName = '" + lname + "', Patient_FName = '" + fname + "', Patient_MidInit = '" + mi + "', Patient_Gender = '" + gender + "',Patient_Birthdate = '" + bdate + "', Patient_Age = '" + age + "', Patient_Address = '" + address + "', Patient_ContactNo = '" + cno + "',Patient_Email = '"+email+"',Patient_CStatus = '"+cstatus+"',Patient_Occupation='"+occupation+"' where Patient_No = '"+pno+"' ";
                    MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                    cmd1.ExecuteNonQuery();

                    MessageBox.Show("Patient's Information updated!");
                    textBox18.ReadOnly = true;
                    textBox17.ReadOnly = true;
                    textBox16.ReadOnly = true;
                    textBox14.ReadOnly = true;
                    textBox13.ReadOnly = true;
                    textBox10.ReadOnly = true;
                    textBox25.ReadOnly = true;
                    radioButton4.Enabled = false;
                    radioButton3.Enabled = false;
                    radioButton7.Enabled = false;
                    radioButton8.Enabled = false;
                    radioButton9.Enabled = false;
                    dateTimePicker2.Enabled = false;
                    dataGridView1.Rows.Clear();
                    try
                    {
                        string query5 = "SELECT Patient_No,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from patienttbl";
                        MySqlCommand cmd5 = new MySqlCommand(query5, connection);
                        MySqlDataReader dataReader5 = cmd5.ExecuteReader();
                        while (dataReader5.Read())
                        {
                            dataGridView1.Rows.Add(dataReader5.GetInt32("Patient_No"), dataReader5.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)"));
                        }
                    }
                    catch (MySqlException me)
                    {
                        MessageBox.Show(me.Message);
                    }
                }
                catch (MySqlException me)
                {
                    MessageBox.Show(me.Message);
                }
                connection.Close();
            }
        }

        private void textBox11_Leave(object sender, EventArgs e)
        {
            string email = textBox11.Text.Trim();
            if (!IsValid(email))
            {
                errorProvider.SetError(textBox11, "Invalid email");
            }
            else
            {
                errorProvider.SetError(textBox11, string.Empty);
            }
        }

        private void textBox10_Leave(object sender, EventArgs e)
        {
            string email = textBox10.Text.Trim();
            if (!IsValid(email))
            {
                errorProvider.SetError(textBox10, "Invalid email");
            }
            else
            {
                errorProvider.SetError(textBox10, string.Empty);
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            bool check = false;
            string lname, fname, mi, gender, lno, st, brgy, city, address, bdate;
            long cno = 0;
            int age = 0, pno = 0;
            string containNumber = @"[0-9~!@#$%^&*()_+=-]";

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
            if (!IsValid(email))
            {
                errorProvider.SetError(textBox11, "Invalid email");
                check = true;
            }
            else
            {
                errorProvider.SetError(textBox11, string.Empty);
            }
            try
            {
                connection.Open();
                string query = "SELECT Patient_No from patienttbl";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    pno = dataReader.GetInt32("Patient_No");
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
                errorProvider.SetError(textBox1, "You must enter last name");
            }
            else
            {

                if (Regex.IsMatch(lname, containNumber))
                {
                    check = true;
                    errorProvider.SetError(textBox1, "Last name format invalid");
                }
                else
                {
                    errorProvider.SetError(textBox1, string.Empty);
                }
            }

            if (fname.Length == 0)
            {
                check = true;
                errorProvider.SetError(textBox2, "You must enter first name");
            }
            else
            {

                if (Regex.IsMatch(fname, containNumber))
                {
                    check = true;
                    errorProvider.SetError(textBox2, "First name format invalid");
                }
                else
                {
                    errorProvider.SetError(textBox2, string.Empty);
                }
            }

            if (Regex.IsMatch(mi, containNumber))
            {
                check = true;
                errorProvider.SetError(textBox3, "Middle initial format invalid");
            }
            else
            {
                errorProvider.SetError(textBox3, string.Empty);
            }

            if (st.Length == 0)
            {
                check = true;
                errorProvider.SetError(textBox7, "You must enter street/subdivision name");
            }
            else
            {
                errorProvider.SetError(textBox7, string.Empty);

            }

            if (brgy.Length == 0)
            {
                check = true;
                errorProvider.SetError(textBox8, "You must enter barangay name");
            }
            else
            {
                errorProvider.SetError(textBox8, string.Empty);

            }

            if (city.Length == 0)
            {
                check = true;
                errorProvider.SetError(textBox9, "You must enter city name");
            }
            else
            {

                if (Regex.IsMatch(city, containNumber))
                {
                    check = true;
                    errorProvider.SetError(textBox9, "City Name format invalid");
                }
                else
                {
                    errorProvider.SetError(textBox9, string.Empty);
                }
            }


            try
            {
                age = Convert.ToInt32(textBox5.Text.Trim());
                errorProvider.SetError(textBox5, string.Empty);
            }
            catch (FormatException)
            {
                check = true;
                errorProvider.SetError(textBox5, "Age is required. Please check your birthdate");
            }
            try
            {
                cno = Convert.ToInt64(textBox4.Text.Trim());
                errorProvider.SetError(textBox4, string.Empty);
            }
            catch (FormatException)
            {
                check = true;
                errorProvider.SetError(textBox4, "Contact number format invalid");
            }
            if (radioButton1.Checked)
            {
                gender = "Male";
            }
            else
            {
                gender = "Female";
            }

            if (check == false)
            {
                linkLabel1.Show();
                if (age < 18)
                {
                    linkLabel2.Show();
                }
               
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (linkLabel2.Visible == false)
            {
                tabPage5.Show();
                tabControl1.SelectTab("tabPage5");
                panel7.Enabled = true;
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tabPage5.Show();
            tabControl1.SelectTab("tabPage5");
            panel7.Enabled = true;
        }

        private void textBox24_TextChanged(object sender, EventArgs e)
        {
            string search = textBox24.Text.Trim();
            dataGridView1.Rows.Clear();
            try
            {
                connection.Open();
                string query = "Select *,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from patienttbl where Patient_LName LIKE '%" + search + "%' OR Patient_FName LIKE '%" + search + "%'";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView1.Rows.Add(dataReader.GetInt32("Patient_No"), dataReader.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)"));
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            int patientno = 0;
            try
            {
                patientno = Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString());
            }
            catch (Exception)
            {
                MessageBox.Show("No Result!");
                textBox24.Text = "";
            }
            try
            {
                connection.Open();
                string query4 = "select * from patienttbl where Patient_No = '"+patientno+"' order by Patient_No LIMIT 1";
                MySqlCommand cmd4 = new MySqlCommand(query4, connection);
                MySqlDataReader dataReader4 = cmd4.ExecuteReader();
                while (dataReader4.Read())
                {
                    textBox18.Text = dataReader4.GetString("Patient_LName");
                    textBox17.Text = dataReader4.GetString("Patient_FName");
                    textBox16.Text = dataReader4.GetString("Patient_MidInit");
                    dateTimePicker2.Value = DateTime.Parse(dataReader4.GetString("Patient_Birthdate"));
                    textBox15.Text = dataReader4.GetInt32("Patient_Age").ToString();
                    textBox14.Text = dataReader4.GetInt64("Patient_ContactNo").ToString();
                    textBox13.Text = dataReader4.GetString("Patient_Address");
                    if (dataReader4.GetString("Patient_Gender").Equals("Male"))
                    {
                        radioButton4.Checked = true;
                    }
                    else
                    {
                        radioButton3.Checked = true;
                    }
                    if (dataReader4.GetString("Patient_CStatus").Equals("Single"))
                    {
                        radioButton7.Checked = true;
                    }
                    else if (dataReader4.GetString("Patient_CStatus").Equals("Married"))
                    {
                        radioButton8.Checked = true;
                    }
                    else
                    {
                        radioButton9.Checked = true;
                    }
                    textBox25.Text = dataReader4.GetString("Patient_Occupation");
                }
                connection.Close();
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
            }
        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel12_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            Discounts discounts = new Discounts();
            discounts.Show();
            this.Hide();
        }

       

    }
}
