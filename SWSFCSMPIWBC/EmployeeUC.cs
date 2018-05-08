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
    public partial class EmployeeUC : UserControl
    {
        static string connectionString =
       System.Configuration.ConfigurationManager.
       ConnectionStrings["SWSFCSMPIWBC.Properties.Settings.slimmersdbConnectionString"].ConnectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);
        public EmployeeUC()
        {
            InitializeComponent();
            schedAdd.BringToFront();
            schedAdd.Visible = false;
            button30.IdleFillColor = Color.FromArgb(4, 91, 188);
            button30.IdleForecolor = Color.White;

            button12.IdleFillColor = Color.White;
            button12.IdleLineColor = Color.FromArgb(4, 91, 188);
            button12.IdleForecolor = Color.FromArgb(4, 91, 188);
            button11.IdleFillColor = Color.White;
            button11.IdleLineColor = Color.FromArgb(4, 91, 188);
            button11.IdleForecolor = Color.FromArgb(4, 91, 188);

            ClearError();
            positionPanel.Show();
            employeePanel.Hide();
            schedulePanel.Hide();
            //button14.BackColor = Color.Silver;
            //button15.BackColor = Color.Transparent;
            //button17.BackColor = Color.Silver;
            GetAllEmpPosition();
            comboBox1.ItemHeight = 10;
            comboBox10.ItemHeight = 10;
            comboBox11.ItemHeight = 10;
            comboBox2.ItemHeight = 10;
            comboBox3.ItemHeight = 10;
            comboBox4.ItemHeight = 10;
            comboBox5.ItemHeight = 10;
            comboBox6.ItemHeight = 10;
            comboBox7.ItemHeight = 10;
            comboBox9.ItemHeight = 10;
            int posno = 0;
            string posname, posdesc;
            try
            {
                connection.Open();
                string query = "Select * from employee_positiontbl order by Employee_Position_No LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    posno = dataReader.GetInt32("Employee_Position_No");
                    posname = dataReader.GetString("Position_Name");
                    posdesc = dataReader.GetString("Position_Description");

                    textBox14.Text = posname;
                    textBox15.Text = posno.ToString();
                    richTextBox2.Text = posdesc;
                }
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();
            //initTime();
            try
            {
                comboBox6.SelectedIndex = 0;
            }
            catch (Exception) { }
        }
        public void ClearError()
        {
            label66.Text = "";
            textBox14.BackColor = Color.White;
            label67.Text = "";
            richTextBox2.BackColor = Color.White;
            label68.Text = "";
            textBox16.BackColor = Color.White;
            label69.Text = "";
            richTextBox1.BackColor = Color.White;
            label48.Text = "";
            textBox12.BackColor = Color.White;
            label49.Text = "";
            textBox11.BackColor = Color.White;
            textBox3.BackColor = Color.White;
            label51.Text = "";
            label52.Text = "";
            textBox2.BackColor = Color.White;
            label53.Text = "";
            comboBox1.BackColor = Color.White;
            label54.Text = "";
            textBox6.BackColor = Color.White;
            label55.Text = "";
            textBox5.BackColor = Color.White;
            label57.Text = "";
            textBox7.BackColor = Color.White;
            label58.Text = "";
            comboBox4.BackColor = Color.White;
            label59.Text = "";
            textBox8.BackColor = Color.White;
            label62.Text = "";
            label65.Text = "";
        }
        public void GetEmployeeNo()
        {
            int empno = 0;
            try
            {
                connection.Open();

                string query1 = "Select Employee_No from employeetbl order by Employee_No";
                MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                while (dataReader1.Read())
                {
                    empno = dataReader1.GetInt32("Employee_No");
                }
                empno = empno + 1;
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            textBox9.Text = empno.ToString();
            connection.Close();
        }
        public int GetScheduleNo()
        {
            int schedno = 0;
            try
            {
                connection.Open();
                string query2 = "select * from employee_schedtbl order by Schedule_No";
                MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                MySqlDataReader dataReader2 = cmd2.ExecuteReader();
                while (dataReader2.Read())
                {
                    schedno = dataReader2.GetInt32("Schedule_No");

                }
                schedno = schedno + 1;
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            return schedno;
        }
        public void GetSelectedEmployee()
        {
            string empname = "";
            dataGridView4.Rows.Clear();
            try
            {
                connection.Open();
                string query = "SELECT *,CONCAT(e.Employee_LName,', ',e.Employee_FName,' ',e.Employee_MidInit) from employee_schedtbl es, employeetbl e where es.Employee_No = (SELECT e.Employee_No from employeetbl e, employee_schedtbl es where e.Employee_No = es.Employee_No order by Employee_No LIMIT 1) and e.Employee_No = es.Employee_No and e.Employee_Status = 'Active'";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    empname = dataReader.GetString("CONCAT(e.Employee_LName,', ',e.Employee_FName,' ',e.Employee_MidInit)");
                    dataGridView4.Rows.Add(dataReader.GetString("Schedule_Day"), dataReader.GetString("Schedule_TimeIn"), dataReader.GetString("Schedule_TimeOut"));
                }
                textBox19.Text = empname;
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
        }
        public void GetAllEmployeeSched()
        {
            dataGridView3.Rows.Clear();
            try
            {
                connection.Open();
                string query = "Select *,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from employeetbl where Employee_No IN (SELECT Employee_No from employee_schedtbl)";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView3.Rows.Add(dataReader.GetString("Employee_No"), dataReader.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)"));
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
        }
        public void GetAllEmployeeWithSched()
        {
            bool check = true;
            comboBox11.Items.Clear();
            try
            {
                connection.Open();
                string query1 = "SELECT * from employee_schedtbl";
                MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                while (dataReader1.Read())
                {
                    check = false;
                }
                connection.Close();
                if (check == false)
                {
                    try
                    {
                        connection.Open();
                        string query = "Select *,CONCAT(e.Employee_LName,', ',e.Employee_FName,' ',e.Employee_MidInit) from employeetbl e LEFT JOIN employee_schedtbl es on e.Employee_No = es.Employee_No where es.Employee_No is null and e.Employee_Status = 'Active'";
                        MySqlCommand cmd = new MySqlCommand(query, connection);
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        while (dataReader.Read())
                        {
                            comboBox11.Items.Add(dataReader.GetString("CONCAT(e.Employee_LName,', ',e.Employee_FName,' ',e.Employee_MidInit)"));
                        }
                        connection.Close();
                    }
                    catch (MySqlException me)
                    {
                        MessageBox.Show(me.Message);
                    }
                }
                else
                {
                    try
                    {
                        connection.Open();
                        string query = "Select *,CONCAT(e.Employee_LName,', ',e.Employee_FName,' ',e.Employee_MidInit) from employeetbl e where  Employee_Status = 'Active' order by e.Employee_No";
                        MySqlCommand cmd = new MySqlCommand(query, connection);
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        while (dataReader.Read())
                        {
                            comboBox11.Items.Add(dataReader.GetString("CONCAT(e.Employee_LName,', ',e.Employee_FName,' ',e.Employee_MidInit)"));
                        }
                        connection.Close();
                    }
                    catch (MySqlException me)
                    {
                        MessageBox.Show(me.Message);
                    }
                }
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }

        }
        public void GetAllEmployee()
        {
            dataGridView1.Rows.Clear();
            try
            {
                connection.Open();
                string query2 = "Select Employee_No,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from employeetbl where Employee_Status = 'Active' order by Employee_No ";
                MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                MySqlDataReader dataReader2 = cmd2.ExecuteReader();
                while (dataReader2.Read())
                {
                    dataGridView1.Rows.Add(dataReader2.GetInt32("Employee_No"), dataReader2.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)"));
                }
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView1.Enabled = false;
            button1.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            button30.Enabled = false;
            button11.Enabled = false;
            button12.Enabled = false;
            cancelBtn.Enabled = true;
            button10.Enabled = true;
            panel9.Enabled = true;
            panel9.BringToFront();
            addTransition.ShowSync(panel9);
            


            panel9.Show();
            GetEmployeeNo();
        }
        private void textBox6_Leave(object sender, EventArgs e)
        {
            string lname = textBox6.Text.Trim();
            bool containsNum = Regex.IsMatch(lname, @"[0-9~!@#$%^&*()_+=-]");
            if (lname.Equals("") || lname == "")
            {
                errorProvider.SetError(textBox6, "You must enter last name");

            }
            else
            {
                if (containsNum)
                {
                    errorProvider.SetError(textBox6, "Invalid format");

                }
                else
                {
                    errorProvider.SetError(textBox6, string.Empty);
                }

            }
        }
        public bool IsValid(string emailaddress)
        {
            try
            {
                //MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        ErrorProvider errorProvider = new ErrorProvider();
        private void textBox7_Leave(object sender, EventArgs e)
        {
            string email = textBox7.Text.Trim();
            if (!IsValid(email))
            {
                errorProvider.SetError(textBox7, "Invalid email");
            }
            else
            {
                errorProvider.SetError(textBox7, string.Empty);
            }
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            string fname = textBox5.Text.Trim();
            bool containsNum = Regex.IsMatch(fname, @"[0-9~!@#$%^&*()_+=-]");
            if (fname.Equals("") || fname == "")
            {
                errorProvider.SetError(textBox5, "You must enter first name");

            }
            else
            {
                if (containsNum)
                {
                    errorProvider.SetError(textBox5, "Invalid format");

                }
                else
                {
                    errorProvider.SetError(textBox5, string.Empty);
                }

            }
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            string mi = textBox4.Text.Trim();
            bool containsNum = Regex.IsMatch(mi, @"[0-9~!@#$%^&*()_+=-]");
            if (containsNum)
            {
                errorProvider.SetError(textBox4, "Invalid format");

            }
            else
            {
                errorProvider.SetError(textBox4, string.Empty);
            }
        }

        private void textBox8_Leave(object sender, EventArgs e)
        {
            long cno = 0;
            try
            {
                cno = Convert.ToInt64(textBox8.Text);
                if (cno.ToString().Length < 11)
                {
                    errorProvider.SetError(textBox8, "Invalid Contact Number");
                }
                else
                {
                    errorProvider.SetError(textBox8, string.Empty);
                }
            }
            catch (FormatException)
            {
                errorProvider.SetError(textBox8, "Invalid Contact Number Format");

            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            dataGridView1.Enabled = true;
            button1.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = true;
            button30.Enabled = true;
            button11.Enabled = true;
            button12.Enabled = true;
            button8.Enabled = true;
            panel9.Hide();



            bool checks = false, exists = false;
            string lname = textBox6.Text.Trim();
            string fname = textBox5.Text.Trim();
            string mi = textBox4.Text.Trim();
            string position = "";
            int empno = Convert.ToInt32(textBox9.Text);
            int posno = 0;
            string cno = "";
            string containsNum = @"[0-9~!@#$%^&*()_+=-]";
            string email = textBox7.Text.Trim();
            string fullname = "", checkfullname = "", checkemail = "", checkcno = "";
            fullname = lname + ", " + fname + " " + mi;
            label27.Text = "";

            position = comboBox4.Text.Trim();
            if (position.Length == 0)
            {
                label58.Text = "Please select employee's position";
                comboBox4.BackColor = Color.Red;

                checks = true;
            }
            else
            {
                label58.Text = "";
                comboBox4.BackColor = Color.White;
            }
            try
            {
                connection.Open();
                string query3 = "Select * from employee_positiontbl where Position_Name = '" + position + "'";
                MySqlCommand cmd3 = new MySqlCommand(query3, connection);
                MySqlDataReader dataReader3 = cmd3.ExecuteReader();
                while (dataReader3.Read())
                {
                    posno = dataReader3.GetInt32("Employee_Position_No");
                }
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();
            if (!IsValid(email))
            {
                label57.Text = "Invalid email";
                textBox7.BackColor = Color.FromArgb(252, 224, 224);
                checks = true;
            }
            else
            {
                label57.Text = "";
                textBox7.BackColor = Color.White;
            }
            try
            {
                cno = textBox8.Text.Trim();
                if (cno.Length == 0)
                {
                    label59.Text = "Required Contact Number";
                    textBox8.BackColor = Color.FromArgb(252, 224, 224);
                    checks = true;
                }
                else if (cno.Length < 11)
                {
                    label59.Text = "Incomplete Contact Number";
                    textBox8.BackColor = Color.FromArgb(252, 224, 224);
                    checks = true;
                }
                else
                {
                    if (Regex.IsMatch(cno, @"[A-Za-z~!@#$%^&*()_+=-]"))
                    {
                        label59.Text = "Invalid Contact Number";
                        textBox8.BackColor = Color.FromArgb(252, 224, 224);
                        checks = true;
                    }
                    else
                    {
                        label59.Text = "";
                        textBox8.BackColor = Color.White;
                    }
                }
            }
            catch (FormatException)
            {
                label59.Text = "Invalid Contact Number Format";
                textBox8.BackColor = Color.FromArgb(252, 224, 224);
                checks = true;
            }
            if (lname.Equals("") || lname == "")
            {
                label54.Text = "You must enter last name";
                textBox6.BackColor = Color.FromArgb(252, 224, 224);
                checks = true;
            }
            else
            {
                if (Regex.IsMatch(lname, containsNum))
                {
                    label54.Text = "Invalid format";
                    textBox6.BackColor = Color.FromArgb(252, 224, 224);
                    checks = true;
                }
                else
                {
                    label54.Text = "";
                    textBox6.BackColor = Color.White;
                }

            }
            if (fname.Equals("") || fname == "")
            {
                label55.Text = "You must enter first name";
                checks = true;
            }
            else
            {
                if (Regex.IsMatch(fname, containsNum))
                {
                    label55.Text = "Invalid format";
                    textBox5.BackColor = Color.FromArgb(252, 224, 224);
                    checks = true;
                }
                else
                {
                    label55.Text = "";
                    textBox5.BackColor = Color.White;
                }

            }
            if (Regex.IsMatch(mi, containsNum))
            {
                label56.Text = "Invalid format";
                textBox4.BackColor = Color.FromArgb(252, 224, 224);
                checks = true;
            }
            else
            {
                label56.Text = "";
                textBox4.BackColor = Color.White;
            }
            try
            {
                connection.Open();
                string query4 = "SELECT *,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from employeetbl";
                MySqlCommand cmd4 = new MySqlCommand(query4, connection);
                MySqlDataReader dataReader4 = cmd4.ExecuteReader();
                while (dataReader4.Read())
                {
                    checkfullname = dataReader4.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)");
                    checkemail = dataReader4.GetString("Employee_Email");
                    checkcno = dataReader4.GetString("Employee_ContactNo");
                    if (checkfullname.Equals(fullname) && checkemail.Equals(email) && checkcno.Equals(cno))
                    {
                        label27.Text = "This employee already exists";
                        checks = true;
                        break;
                    }
                    else if (checkemail.Equals(email))
                    {
                        label27.Text = "Email already exists";
                        textBox7.BackColor = Color.FromArgb(252, 224, 224);
                        checks = true;
                        break;
                    }
                    else if (checkcno.Equals(cno))
                    {
                        label27.Text = "Contact Number already exists";
                        textBox8.BackColor = Color.FromArgb(252, 224, 224);
                        checks = true;
                        break;
                    }
                    else
                    {
                        label27.Text = "";
                        textBox7.BackColor = Color.White;
                        textBox8.BackColor = Color.White;
                    }

                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            if (checks == false)
            {
                try
                {
                    connection.Open();
                    string query = "Insert into employeetbl values ('" + empno + "','" + lname + "','" + fname + "','" + mi + "','" + cno + "','" + email + "','" + posno + "','Active','Available')";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Employee added successfully!");
                    connection.Close();
                    GetEmployeeNo();
                    panel9.Hide();
                    employeePanel.Enabled = true;
                    dataGridView1.Enabled = true;
                    button1.Enabled = true;
                    button6.Enabled = true;
                    button7.Enabled = true;
                    button8.Enabled = true;
                    button30.Enabled = true;
                    button11.Enabled = true;
                    button12.Enabled = true;
                    panel9.Enabled = true;
                    panel7.Show();
                    GetAllEmployee();
                    GetFirstEmployee();
                    textBox4.Text = "";
                    textBox5.Text = "";
                    textBox6.Text = "";
                    textBox7.Text = "";
                    textBox8.Text = "";
                    comboBox4.SelectedIndex = 0;

                }
                catch (MySqlException me)
                {
                    MessageBox.Show(me.Message);
                }

            }
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            ClearError();
            panel9.SendToBack();
            panel7.BringToFront();
            label28.Text = "";
            button8.Enabled = false;
            textBox12.ReadOnly = true;
            textBox11.ReadOnly = true;
            textBox10.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox2.ReadOnly = true;
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            textBox12.BorderStyle = BorderStyle.None;
            textBox11.BorderStyle = BorderStyle.None;
            textBox10.BorderStyle = BorderStyle.None;
            textBox3.BorderStyle = BorderStyle.None;
            textBox2.BorderStyle = BorderStyle.None;
            int rows = 0, empno = 0;

            rows = dataGridView1.CurrentCell.RowIndex;
            empno = Convert.ToInt32(dataGridView1.Rows[rows].Cells[0].Value);

            try
            {
                connection.Open();
                string query1 = "Select * from employeetbl e, employee_positiontbl ep where e.Employee_No = '" + empno + "' and e.Employee_Position_No = ep.Employee_Position_No order by Employee_No";
                MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                while (dataReader1.Read())
                {
                    textBox1.Text = dataReader1.GetInt32("Employee_No").ToString();
                    textBox12.Text = dataReader1.GetString("Employee_LName");
                    textBox11.Text = dataReader1.GetString("Employee_FName");
                    textBox10.Text = dataReader1.GetString("Employee_MidInit");
                    textBox3.Text = dataReader1.GetString("Employee_Email");
                    textBox2.Text = dataReader1.GetString("Employee_ContactNo");
                    comboBox1.Text = dataReader1.GetString("Position_Name");
                    comboBox2.Text = dataReader1.GetString("Employee_Status");
                }
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox12.ReadOnly = false;
            textBox11.ReadOnly = false;
            textBox10.ReadOnly = false;
            textBox3.ReadOnly = false;
            textBox2.ReadOnly = false;
            textBox12.BorderStyle = BorderStyle.FixedSingle;
            textBox11.BorderStyle = BorderStyle.FixedSingle;
            textBox10.BorderStyle = BorderStyle.FixedSingle;
            textBox3.BorderStyle = BorderStyle.FixedSingle;
            textBox2.BorderStyle = BorderStyle.FixedSingle;
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            button8.Enabled = true;
            button8.Visible = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {


            textBox12.ReadOnly = true;
            textBox11.ReadOnly = true;
            textBox10.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox12.BorderStyle = BorderStyle.None;
            textBox11.BorderStyle = BorderStyle.None;
            textBox10.BorderStyle = BorderStyle.None;
            textBox3.BorderStyle = BorderStyle.None;
            textBox2.BorderStyle = BorderStyle.None;
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            button8.Enabled = true;
            button8.Visible = false;

            bool checks = false, exists = false;
            string lname = textBox12.Text.Trim();
            string fname = textBox11.Text.Trim();
            string mi = textBox10.Text.Trim();
            string position = "", checkposition = "", checkstatus = "";
            int empno = Convert.ToInt32(textBox1.Text);
            int posno = 0;
            int checkempno = 0;
            string cno = "", checkfullname = "", checkemail = "", checkcno = "";
            string containsNum = @"[0-9~!@#$%^&*()_+=-]";
            string email = textBox3.Text.Trim();
            string empstat = comboBox2.Text;
            string fullname = lname + ", " + fname + " " + mi;
            label28.Text = "";

            try
            {
                position = comboBox1.Text;
                label53.Text = "";
                comboBox1.BackColor = Color.White;
            }
            catch (Exception)
            {
                label53.Text = "Please select employee's position";
                comboBox1.BackColor = Color.FromArgb(252, 224, 224);
                checks = true;
            }
            try
            {
                connection.Open();
                string query3 = "Select * from employee_positiontbl where Position_Name = '" + position + "'";
                MySqlCommand cmd3 = new MySqlCommand(query3, connection);
                MySqlDataReader dataReader3 = cmd3.ExecuteReader();
                while (dataReader3.Read())
                {
                    posno = dataReader3.GetInt32("Employee_Position_No");
                }
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();
            if (!IsValid(email))
            {
                label51.Text = "Invalid email";
                textBox3.BackColor = Color.FromArgb(252, 224, 224);
                checks = true;
            }
            else
            {
                label51.Text = "";
                textBox3.BackColor = Color.White;
            }

            cno = textBox2.Text.Trim();
            if (cno.Length == 0)
            {
                label52.Text = "Contact Number is required";
                textBox2.BackColor = Color.FromArgb(252, 224, 224);
                checks = true;
            }
            else
            {
                if (cno.Length < 11)
                {
                    label52.Text = "Invalid Contact Number";
                    textBox2.BackColor = Color.FromArgb(252, 224, 224);
                    checks = true;
                }
                else
                {
                    if (Regex.IsMatch(cno, @"[A-Za-z~!@#$%^&*()_.,/\|?<>+=-]"))
                    {
                        label52.Text = "Contact Number should be numbers only";
                        textBox2.BackColor = Color.FromArgb(252, 224, 224);
                        checks = true;
                    }
                    else
                    {
                        label52.Text = "";
                        textBox2.BackColor = Color.White;
                    }
                }
            }


            if (lname.Equals("") || lname == "")
            {
                label48.Text = "You must enter last name";
                textBox12.BackColor = Color.FromArgb(252, 224, 224);
                checks = true;
            }
            else
            {
                if (Regex.IsMatch(lname, containsNum))
                {
                    label48.Text = "Invalid format";
                    textBox12.BackColor = Color.FromArgb(252, 224, 224);
                    checks = true;
                }
                else
                {
                    label48.Text = "";
                    textBox12.BackColor = Color.White;
                }

            }
            if (fname.Equals("") || fname == "")
            {
                label49.Text = "You must enter first name";
                textBox11.BackColor = Color.FromArgb(252, 224, 224);
                checks = true;
            }
            else
            {
                if (Regex.IsMatch(fname, containsNum))
                {
                    label49.Text = "Invalid format";
                    textBox11.BackColor = Color.FromArgb(252, 224, 224);
                    checks = true;
                }
                else
                {
                    label49.Text = "";
                    textBox11.BackColor = Color.White;
                }

            }
            if (Regex.IsMatch(mi, containsNum))
            {
                label50.Text = "Invalid format";
                textBox10.BackColor = Color.FromArgb(252, 224, 224);
                checks = true;
            }
            else
            {
                label50.Text = "";
                textBox10.BackColor = Color.White;
            }
            try
            {
                connection.Open();
                MySqlCommand cmd4 = new MySqlCommand("SELECT *,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from employeetbl e, employee_positiontbl ep where Employee_Status = 'Active' and e.Employee_Position_No = ep.Employee_Position_No", connection);
                MySqlDataReader dataReader4 = cmd4.ExecuteReader();
                while (dataReader4.Read())
                {
                    checkfullname = dataReader4.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)");
                    checkemail = dataReader4.GetString("Employee_Email");
                    checkcno = dataReader4.GetString("Employee_ContactNo");
                    checkempno = dataReader4.GetInt32("Employee_No");
                    checkposition = dataReader4.GetString("Position_Name");
                    checkstatus = dataReader4.GetString("Employee_Status");
                    if (checkfullname.Equals(fullname) && checkemail.Equals(email) && checkcno.Equals(cno) && position.Equals(checkposition) && empstat.Equals(checkstatus))
                    {
                        label28.Text = "Employee already exists";
                        checks = true;
                        break;
                    }
                    if (checkempno != empno)
                    {
                        if (checkemail.Equals(email))
                        {
                            label28.Text = "Email already exists";
                            textBox3.BackColor = Color.FromArgb(252, 224, 224);
                            checks = true;
                            break;
                        }
                        else if (checkcno.Equals(cno))
                        {
                            label28.Text = "Contact Number already exists";
                            textBox2.BackColor = Color.FromArgb(252, 224, 224);
                            checks = true;
                            break;
                        }
                        else
                        {
                            label28.Text = "";
                            textBox3.BackColor = Color.White;
                            textBox2.BackColor = Color.White;
                        }
                    }

                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            if (checks == false)
            {
                try
                {
                    connection.Open();
                    string query = "Update employeetbl set Employee_LName = '" + lname + "',Employee_FName = '" + fname + "',Employee_MidInit = '" + mi + "',Employee_ContactNo = '" + cno + "',Employee_Email = '" + email + "',Employee_Position_No = '" + posno + "',Employee_Status = '" + empstat + "' where Employee_No = '" + empno + "'";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Employee updated successfully!");
                    connection.Close();
                    GetAllEmployee();
                    GetFirstEmployee();
                    textBox12.ReadOnly = true;
                    textBox11.ReadOnly = true;
                    textBox10.ReadOnly = true;
                    textBox3.ReadOnly = true;
                    textBox2.ReadOnly = true;
                    comboBox1.Enabled = false;
                    comboBox2.Enabled = false;
                    button8.Enabled = false;
                    textBox12.BorderStyle = BorderStyle.None;
                    textBox11.BorderStyle = BorderStyle.None;
                    textBox10.BorderStyle = BorderStyle.None;
                    textBox3.BorderStyle = BorderStyle.None;
                    textBox2.BorderStyle = BorderStyle.None;
                }
                catch (MySqlException me)
                {
                    MessageBox.Show(me.Message);
                }
            }
        }
        private void button1_Click_2(object sender, EventArgs e)
        {
            int emp_no = 0;
            int rows = 0;
            rows = dataGridView1.CurrentCell.RowIndex;
            emp_no = Convert.ToInt32(dataGridView1.Rows[rows].Cells[0].Value);
            DialogResult dr = MessageBox.Show("Do you really want to delete?", "Delete", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE employeetbl set Employee_Status = 'Deleted' where Employee_No = '" + emp_no + "'";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record deleted!");
                    connection.Close();
                    GetAllEmployee();
                }
                catch (MySqlException me)
                {
                    MessageBox.Show(me.Message);
                }
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            schedAdd.BringToFront();
            schedAdd.Enabled = true;
            
            button30.Enabled = false;
            button11.Enabled = false;
            button12.Enabled = false;
            button24.Enabled = false;
            button23.Enabled = false;
            button22.Enabled = false;
            dataGridView3.Enabled = false;

            addTransition.ShowSync(schedAdd);

            schedAdd.Show();
            GetAllEmployeeWithSched();
            try
            {
                comboBox11.SelectedIndex = 0;
            }
            catch (Exception)
            {
                MessageBox.Show("All employee have their schedule");
                schedAdd.SendToBack();
                schedEdit.BringToFront();
            }
            try
            {
                comboBox10.SelectedIndex = 0;
                comboBox9.SelectedIndex = 0;
                comboBox3.SelectedIndex = 0;
                comboBox6.SelectedIndex = 0;
                comboBox5.SelectedIndex = 0;
            }
            catch (Exception) { }
        }

        private void button28_Click(object sender, EventArgs e)
        {
            string empname = comboBox11.Text;
            string day = "";
            string timein = "";
            string timeout = "";
            int timeinhour = 0, timeouthour = 0, timeinmin = 0, timeoutmin = 0, totalhour = 0;
            bool exists = false, check = false;
            try
            {
                day = comboBox10.Text;
                label63.Text = "";
            }
            catch (Exception)
            {
                label63.Text = "Please select day";
                check = true;
            }
            try
            {
                timein = comboBox9.Text;
                timeout = comboBox3.Text;
                label64.Text = "";
            }
            catch (Exception)
            {
                label64.Text = "Please select time";
                check = true;
            }
            timeinhour = Convert.ToInt32(timein.Substring(0, 2));
            timeinmin = Convert.ToInt32(timein.Substring(3, 2));
            timeouthour = Convert.ToInt32(timeout.Substring(0, 2)) + 12;
            timeoutmin = Convert.ToInt32(timeout.Substring(3, 2));
            totalhour = timeouthour - timeinhour;
            
            for (int i = 0; i < dataGridView5.Rows.Count; i++)
            {
                if (day == dataGridView5.Rows[i].Cells[0].Value.ToString())
                {
                    label63.Text = "Day exists in the table";
                    exists = true;
                    break;
                }
                else
                {
                    label63.Text = "";
                }

            }
            if (exists == false && check == false)
            {
                dataGridView5.Rows.Add(day, timein, timeout);
            }
        }

        private void button27_Click(object sender, EventArgs e)
        {
            errorProvider.SetError(comboBox3, string.Empty);
            try
            {
                dataGridView5.Rows.RemoveAt(dataGridView5.CurrentRow.Index);
            }
            catch (NullReferenceException ne)
            {
                MessageBox.Show("No selected row");
            }
        }

        private void button29_Click(object sender, EventArgs e)
        {
            string emp = comboBox11.Text;
            string day, timein, timeout;
            int empno = 0;
            int schedno = 0;
            bool check = false;
            try
            {
                connection.Open();
                string query = "Select Employee_No,CONCAT(Employee_LName, ', ',Employee_FName,' ',Employee_MidInit) from employeetbl where CONCAT(Employee_LName, ', ',Employee_FName,' ',Employee_MidInit) =  '" + emp + "'";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    empno = dataReader.GetInt32("Employee_No");
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            if (dataGridView5.Rows.Count == 0)
            {
                errorProvider.SetError(comboBox3, "Please select schedule for the employee first");
                check = true;
            }
            if (check == false)
            {
                errorProvider.SetError(comboBox3, string.Empty);
                for (int i = 0; i < dataGridView5.Rows.Count; i++)
                {

                    schedno = GetScheduleNo();
                    day = dataGridView5.Rows[i].Cells[0].Value.ToString();
                    timein = dataGridView5.Rows[i].Cells[1].Value.ToString();
                    timeout = dataGridView5.Rows[i].Cells[2].Value.ToString();

                    try
                    {
                        connection.Open();
                        string query1 = "Insert into employee_schedtbl values ('" + schedno + "','" + day + "','" + timein + "','" + timeout + "','" + empno + "')";
                        MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                        cmd1.ExecuteNonQuery();
                        connection.Close();
                    }
                    catch (MySqlException me)
                    {
                        MessageBox.Show(me.Message);
                    }

                }
                MessageBox.Show("Employee schedule added successfully");
                comboBox11.Items.Clear();
                GetAllEmployeeWithSched();
                try
                {
                    comboBox11.SelectedIndex = 0;
                }
                catch (Exception)
                {
                }
                comboBox10.SelectedIndex = 0;
                comboBox9.SelectedIndex = 0;
                comboBox3.SelectedIndex = 0;
                dataGridView3.Rows.Clear();
                GetAllEmployeeSched();
                GetSelectedEmployee();
               
                schedEdit.BringToFront();

                schedAdd.SendToBack();
                button30.Enabled = true;
                button11.Enabled = true;
                button12.Enabled = true;
                button24.Enabled = true;
                button23.Enabled = true;
                button22.Enabled = true;
                dataGridView3.Enabled = true;

                schedAdd.Hide();
            }

        }

        private void button17_Click(object sender, EventArgs e)
        {
            button11.IdleFillColor = Color.FromArgb(4, 91, 188);
            button11.IdleForecolor = Color.White;

            button30.IdleFillColor = Color.White;
            button30.IdleLineColor = Color.FromArgb(4, 91, 188);
            button30.IdleForecolor = Color.FromArgb(4, 91, 188);
            button12.IdleFillColor = Color.White;
            button12.IdleLineColor = Color.FromArgb(4, 91, 188);
            button12.IdleForecolor = Color.FromArgb(4, 91, 188);

            textBox18.Visible = true;
            textBox18.BringToFront();
            textBox24.SendToBack();
            textBox13.SendToBack();

            ClearError();
            //button14.BackColor = Color.Silver;
            //button15.BackColor = Color.Silver;
            //button17.BackColor = Color.Transparent;
            comboBox5.Enabled = false;
            comboBox6.Enabled = false;
            comboBox7.Enabled = false;
            button25.Visible = false;
            button26.Visible = false;
            button23.Enabled = false;
            employeePanel.Hide();
            positionPanel.Hide();
            schedulePanel.Show();
            schedEdit.Show();
            schedAdd.Hide();
            GetAllEmployeeSched();
            GetSelectedEmployee();
            try
            {
                comboBox5.SelectedIndex = 0;
                comboBox6.SelectedIndex = 0;
                comboBox7.SelectedIndex = 0;
            }
            catch (Exception) { }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            button23.Visible = true;


            comboBox5.Enabled = true;
            comboBox6.Enabled = true;
            comboBox7.Enabled = true;
            button25.Visible = true;
            button26.Visible = true;
            button23.Enabled = true;
        }

        private void button26_Click(object sender, EventArgs e)
        {
            string day = "";
            string timein = "";
            string timeout = "";
            int timeinhour = 0, timeouthour = 0, timeinmin = 0, timeoutmin = 0, totalhour = 0;
            bool exists = false, check = false;
            try
            {
                day = comboBox7.Text;
                label60.Text = "";
                comboBox7.BackColor = Color.White;
            }
            catch (Exception)
            {
                label60.Text = "Please select day";
                comboBox7.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }
            try
            {
                timein = comboBox6.Text;
                timeout = comboBox5.Text;
                label61.Text = "";
                comboBox6.BackColor = Color.White;
            }
            catch (Exception)
            {
                label61.Text = "Please select time";
                comboBox6.BackColor = Color.FromArgb(252, 224, 224);
            }
            timeinhour = Convert.ToInt32(timein.Substring(0, 2));
            timeinmin = Convert.ToInt32(timein.Substring(3, 2));
            timeouthour = Convert.ToInt32(timeout.Substring(0, 2)) + 12;
            timeoutmin = Convert.ToInt32(timeout.Substring(3, 2));
            totalhour = timeouthour - timeinhour;
            if (totalhour < 8)
            {
                label61.Text = "Working Hours should be minimum of 8 hours";
                check = true;
            }
            else
            {
                if (totalhour == 8 && timeinmin != timeoutmin)
                {
                    label61.Text = "Working Hours should be minimum of 8 hours";
                    check = true;
                }
                else
                {
                    label61.Text = "";
                }
            }
            for (int i = 0; i < dataGridView4.Rows.Count; i++)
            {
                if (day == dataGridView4.Rows[i].Cells[0].Value.ToString())
                {
                    label60.Text = "Day exists in the table";
                    comboBox7.BackColor = Color.FromArgb(252, 224, 224);
                    exists = true;
                    break;
                }
                else
                {
                    label60.Text = "";
                    comboBox7.BackColor = Color.White;
                }

            }
            if (exists == false && check == false)
            {
                dataGridView4.Rows.Add(day, timein, timeout);
            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            errorProvider.SetError(comboBox7, string.Empty);
            try
            {
                dataGridView4.Rows.RemoveAt(dataGridView4.CurrentRow.Index);
            }
            catch (NullReferenceException ne)
            {
                MessageBox.Show("No selected row");
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {
            button30.IdleFillColor = Color.FromArgb(4, 91, 188);
            button30.IdleForecolor = Color.White;

            button12.IdleFillColor = Color.White;
            button12.IdleLineColor = Color.FromArgb(4, 91, 188);
            button12.IdleForecolor = Color.FromArgb(4, 91, 188);
            button11.IdleFillColor = Color.White;
            button11.IdleLineColor = Color.FromArgb(4, 91, 188);
            button11.IdleForecolor = Color.FromArgb(4, 91, 188);

            textBox13.Visible = true;
            textBox13.BringToFront();
            textBox18.SendToBack();
            textBox24.SendToBack();

            ClearError();
            positionPanel.Show();
            employeePanel.Hide();
            schedulePanel.Hide();
            positionEdit.Show();
            positionAdd.Hide();
            textBox14.ReadOnly = true;
            richTextBox2.ReadOnly = true;
            textBox14.BorderStyle = BorderStyle.None;
            richTextBox2.BorderStyle = BorderStyle.None;
            button20.Enabled = false;
            //button14.BackColor = Color.Silver;
            //button15.BackColor = Color.Transparent;
            //button17.BackColor = Color.Silver;
            dataGridView2.Rows.Clear();
            GetAllEmpPosition();
            GetFirstPosition();
        }
        public void GetFirstPosition()
        {
            int posno = 0;
            string posname, posdesc;
            try
            {
                connection.Open();
                string query = "Select * from employee_positiontbl order by Employee_Position_No LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    posno = dataReader.GetInt32("Employee_Position_No");
                    posname = dataReader.GetString("Position_Name");
                    posdesc = dataReader.GetString("Position_Description");

                    textBox14.Text = posname;
                    textBox15.Text = posno.ToString();
                    richTextBox2.Text = posdesc;
                }
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();
        }
        public void GetFirstEmployee()
        {
            try
            {
                connection.Open();
                string query1 = "Select * from employeetbl e, employee_positiontbl ep where e.Employee_Position_No = ep.Employee_Position_No and e.Employee_Status = 'Active' order by Employee_No LIMIT 1";
                MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                while (dataReader1.Read())
                {
                    textBox1.Text = dataReader1.GetInt32("Employee_No").ToString();
                    textBox12.Text = dataReader1.GetString("Employee_LName");
                    textBox11.Text = dataReader1.GetString("Employee_FName");
                    textBox10.Text = dataReader1.GetString("Employee_MidInit");
                    textBox3.Text = dataReader1.GetString("Employee_Email");
                    textBox2.Text = dataReader1.GetString("Employee_ContactNo");
                    comboBox1.Text = dataReader1.GetString("Position_Name");
                    comboBox2.Text = dataReader1.GetString("Employee_Status");
                }
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();
        }
        private void button14_Click(object sender, EventArgs e)
        {
            button12.IdleFillColor = Color.FromArgb(4, 91, 188);
            button12.IdleForecolor = Color.White;

            button30.IdleFillColor = Color.White;
            button30.IdleLineColor = Color.FromArgb(4, 91, 188);
            button30.IdleForecolor = Color.FromArgb(4, 91, 188);
            button11.IdleFillColor = Color.White;
            button11.IdleLineColor = Color.FromArgb(4, 91, 188);
            button11.IdleForecolor = Color.FromArgb(4, 91, 188);
            textBox24.Visible = true;
            textBox24.BringToFront();
            textBox18.SendToBack();
            textBox13.SendToBack();

            ClearError();
            positionPanel.Hide();
            employeePanel.Show();
            schedulePanel.Hide();
            panel7.Show();
            panel9.Hide();
            //button14.BackColor = Color.Transparent;
            //button15.BackColor = Color.Silver;
            //button17.BackColor = Color.Silver;
            textBox12.ReadOnly = true;
            textBox11.ReadOnly = true;
            textBox10.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox12.BorderStyle = BorderStyle.None;
            textBox11.BorderStyle = BorderStyle.None;
            textBox10.BorderStyle = BorderStyle.None;
            textBox3.BorderStyle = BorderStyle.None;
            textBox2.BorderStyle = BorderStyle.None;
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            button8.Enabled = false;
            GetAllEmployee();
            comboBox4.Items.Clear();
            comboBox1.Items.Clear();
            try
            {
                connection.Open();
                string query = "Select Position_Name from employee_positiontbl order by Employee_Position_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    comboBox4.Items.Add(dataReader.GetString("Position_Name"));
                    comboBox1.Items.Add(dataReader.GetString("Position_Name"));
                }
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();
            comboBox4.SelectedIndex = 0;
            comboBox1.SelectedIndex = 0;
            GetFirstEmployee();
        }

        private void dataGridView3_Click(object sender, EventArgs e)
        {
            ClearError();
            schedAdd.SendToBack();
            schedEdit.BringToFront();
            dataGridView4.Rows.Clear();
            button23.Enabled = false;
            int rows = dataGridView3.CurrentCell.RowIndex;
            string empname = dataGridView3.Rows[rows].Cells[1].Value.ToString();
            try
            {
                connection.Open();
                string query = "SELECT *,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from employee_schedtbl es, employeetbl e where CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) = '" + empname + "' and e.Employee_No = es.Employee_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    textBox19.Text = dataReader.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)");
                    dataGridView4.Rows.Add(dataReader.GetString("Schedule_Day"), dataReader.GetString("Schedule_TimeIn"), dataReader.GetString("Schedule_TimeOut"));
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {
            string search = textBox18.Text.Trim();

            dataGridView3.Rows.Clear();
            try
            {
                connection.Open();
                string query = "Select *,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from employeetbl where Employee_No IN (SELECT Employee_No from employee_schedtbl) and (Employee_LName LIKE '%" + search + "%' OR Employee_FName LIKE '%" + search + "%')";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView3.Rows.Add(dataReader.GetInt32("Employee_No"), dataReader.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)"));
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            dataGridView4.Rows.Clear();
            int empno = 0;

            try
            {
                empno = Convert.ToInt32(dataGridView3.Rows[dataGridView3.CurrentCell.RowIndex].Cells[0].Value.ToString());
            }
            catch (Exception)
            {
                MessageBox.Show("No Result!");
                textBox18.Text = "";
            }


            try
            {
                connection.Open();
                string query1 = "SELECT *,CONCAT(e.Employee_LName,', ',e.Employee_FName,' ',e.Employee_MidInit) from employee_schedtbl es, employeetbl e where es.Employee_No = '" + empno + "' and e.Employee_No = es.Employee_No";
                MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                while (dataReader1.Read())
                {
                    textBox19.Text = dataReader1.GetString("CONCAT(e.Employee_LName,', ',e.Employee_FName,' ',e.Employee_MidInit)");
                    dataGridView4.Rows.Add(dataReader1.GetString("Schedule_Day"), dataReader1.GetString("Schedule_TimeIn"), dataReader1.GetString("Schedule_TimeOut"));
                }

                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
        }
        public void GetPositionNo()
        {
            int posno = 0;

            try
            {
                connection.Open();
                string query = "SELECT Employee_Position_No from employee_positiontbl order by Employee_Position_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    posno = dataReader.GetInt32("Employee_Position_No");
                }
                posno = posno + 1;
                textBox17.Text = posno.ToString();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();

        }
        public void GetAllEmpPosition()
        {
            int posno = 0;
            string posname;
            try
            {
                connection.Open();
                string query = "Select * from employee_positiontbl order by Employee_Position_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    posno = dataReader.GetInt32("Employee_Position_No");
                    posname = dataReader.GetString("Position_Name");
                    dataGridView2.Rows.Add(posno, posname);
                }
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();
        }
        private void button18_Click(object sender, EventArgs e)
        {
            positionAdd.Visible = false;
            positionAdd.BringToFront();
            dataGridView2.Enabled = false;
            button30.Enabled = false;
            button11.Enabled = false;
            button12.Enabled = false;
            button19.Enabled = false;
            button20.Enabled = false;
            button18.Enabled = false;

            addTransition.ShowSync(positionAdd);
            positionAdd.Show();
            GetPositionNo();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            
            positionAdd.Visible = false;
            dataGridView2.Enabled = true;
            button30.Enabled = true;
            button11.Enabled = true;
            button12.Enabled = true;
            button19.Enabled = true;
            button18.Enabled = true;

            positionAdd.Hide();
            bool check = false;
            bool exists = false;

            string posname = textBox16.Text.Trim(), checkpos;
            string posdesc = richTextBox1.Text.Trim();
            string containNumber = @"[0-9~!@#$%^&*()_+=-]";
            int posno = Convert.ToInt32(textBox17.Text.Trim());
            if (posname.Length == 0)
            {
                label68.Text = "Position name is requried.";
                check = true;
            }
            else
            {
                if (Regex.IsMatch(posname, containNumber))
                {
                    label68.Text = "Position name should not contain numbers";
                    check = true;
                }
                else
                {
                    try
                    {
                        connection.Open();
                        string query1 = "Select Position_Name from employee_positiontbl where Employee_Position_No != '" + posno + "'";
                        MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                        MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                        while (dataReader1.Read())
                        {
                            checkpos = dataReader1.GetString("Position_Name");
                            if (checkpos.Equals(posname))
                            {
                                exists = true;
                                break;
                            }

                        }
                    }
                    catch (MySqlException me)
                    {
                        MessageBox.Show(me.Message);
                    }
                    connection.Close();
                }
            }
            if (exists)
            {
                label68.Text = "Position name already exists";
                check = true;
            }
            else
            {
                label68.Text = "";
            }

            if (posdesc.Length == 0)
            {
                label69.Text = "Position description is required";
                check = true;
            }
            else
            {
                label69.Text = "";
            }
            if (check == false)
            {
                try
                {
                    connection.Open();
                    string query = "INSERT into employee_positiontbl values ('" + posno + "','" + posname + "','" + posdesc + "')";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Successfully added Employee position!");
                    connection.Close();
                    textBox16.Text = "";
                    richTextBox1.Text = "";
                    dataGridView2.Rows.Clear();
                    positionAdd.Hide();
                    positionEdit.Show();
                    GetAllEmpPosition();
                    GetFirstPosition();
                }
                catch (MySqlException me)
                {
                    MessageBox.Show(me.Message);
                }
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            button20.Visible = true;
            textBox14.ReadOnly = false;
            richTextBox2.ReadOnly = false;
            textBox14.BorderStyle = BorderStyle.FixedSingle;
            richTextBox2.BorderStyle = BorderStyle.FixedSingle;
            button20.Enabled = true;
        }

        private void button20_Click(object sender, EventArgs e)
        {

            button20.Visible = false;
            textBox14.ReadOnly = true;
            richTextBox2.ReadOnly = true;
            textBox14.BorderStyle = BorderStyle.FixedSingle;
            richTextBox2.BorderStyle = BorderStyle.FixedSingle;
            button20.Enabled = true;

            bool check = false, exists = false;
            string posname, posdesc, checkpos;
            posname = textBox14.Text.Trim();
            posdesc = richTextBox2.Text.Trim();

            string containNumber = @"[0-9~!@#$%^&*()_+=-]";
            int posno = Convert.ToInt32(textBox15.Text.Trim());
            if (posname.Length == 0)
            {
                label66.Text = "Position name is requried.";
                check = true;
            }
            else
            {
                if (Regex.IsMatch(posname, containNumber))
                {
                    label66.Text = "Position name should not contain numbers";
                    check = true;
                }
                else
                {
                    try
                    {
                        connection.Open();
                        string query1 = "Select Position_Name from employee_positiontbl where Employee_Position_No != '" + posno + "'";
                        MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                        MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                        while (dataReader1.Read())
                        {
                            checkpos = dataReader1.GetString("Position_Name");
                            if (checkpos.Equals(posname))
                            {
                                exists = true;
                                break;
                            }

                        }
                    }
                    catch (MySqlException me)
                    {
                        MessageBox.Show(me.Message);
                    }
                    connection.Close();
                }
            }
            if (exists)
            {
                label66.Text = "Position name already exists";
                check = true;
            }
            else
            {
                label66.Text = "";
            }

            if (posdesc.Length == 0)
            {
                label67.Text = "Position description is required";
                check = true;
            }
            else
            {
                label67.Text = "";
            }
            if (check == false)
            {
                try
                {
                    connection.Open();
                    string query = "Update employee_positiontbl set Position_Name = '" + posname + "',Position_Description = '" + posdesc + "' where Employee_Position_No = '" + posno + "'";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Successfully updated Employee position!");
                    connection.Close();
                    dataGridView2.Rows.Clear();
                    GetAllEmpPosition();
                    GetFirstPosition();
                    textBox14.ReadOnly = true;
                    richTextBox2.ReadOnly = true;
                    button20.Enabled = false;
                    textBox14.BorderStyle = BorderStyle.None;
                    richTextBox2.BorderStyle = BorderStyle.None;
                }
                catch (MySqlException me)
                {
                    MessageBox.Show(me.Message);
                }
            }
        }

        private void dataGridView2_Click(object sender, EventArgs e)
        {
            ClearError();
            positionAdd.SendToBack();
            positionEdit.BringToFront();
            button20.Enabled = false;
            textBox14.ReadOnly = true;
            richTextBox2.ReadOnly = true;
            textBox14.BorderStyle = BorderStyle.None;
            richTextBox2.BorderStyle = BorderStyle.None;
            int rows = 0, posno = 0;

            rows = dataGridView2.CurrentCell.RowIndex;
            posno = Convert.ToInt32(dataGridView2.Rows[rows].Cells[0].Value);

            try
            {
                connection.Open();
                string query4 = "select * from employee_positiontbl where Employee_Position_No = '" + posno + "' order by Employee_Position_No LIMIT 1";
                MySqlCommand cmd4 = new MySqlCommand(query4, connection);
                MySqlDataReader dataReader4 = cmd4.ExecuteReader();
                while (dataReader4.Read())
                {
                    textBox14.Text = dataReader4.GetString("Position_Name");
                    textBox15.Text = dataReader4.GetInt32("Employee_Position_No").ToString();
                    richTextBox2.Text = dataReader4.GetString("Position_Description");
                }
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            positionEdit.BringToFront();
            positionAdd.SendToBack();
            string pos = textBox13.Text.Trim();
            dataGridView2.Rows.Clear();
            try
            {
                connection.Open();
                string query = "Select * from employee_positiontbl where Position_Name LIKE '%" + pos + "%'";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView2.Rows.Add(dataReader.GetInt32("Employee_Position_No"), dataReader.GetString("Position_Name"));
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            int posnum = 0;
            try
            {
                posnum = Convert.ToInt32(dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex].Cells[0].Value.ToString());
            }
            catch (Exception)
            {
                MessageBox.Show("No Result!");
                textBox13.Text = "";
            }
            try
            {
                connection.Open();
                string query = "Select * from employee_positiontbl where Employee_Position_No = '" + posnum + "' order by Employee_Position_No LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    textBox15.Text = dataReader.GetInt32("Employee_Position_No").ToString();
                    textBox14.Text = dataReader.GetString("Position_Name");
                    richTextBox2.Text = dataReader.GetString("Position_Description");
                }
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();
        }
        private void textBox14_KeyUp(object sender, KeyEventArgs e)
        {
            string containsNum = @"[0-9~!@#$%^&*(),/\|?<>_+=-]";
            string empname = textBox14.Text.Trim();
            if (Regex.IsMatch(textBox14.Text, containsNum))
            {
                label66.Text = "No numeric character";
                textBox14.BackColor = Color.FromArgb(252, 224, 224);
            }
            else
            {
                label66.Text = "";
                textBox14.BackColor = Color.White;
            }
        }

        private void textBox16_KeyUp(object sender, KeyEventArgs e)
        {
            string containsNum = @"[0-9~!@#$%^&*(),/\|?<>_+=-]";
            string empname = textBox16.Text.Trim();
            if (Regex.IsMatch(textBox16.Text, containsNum))
            {
                label68.Text = "No numeric character";
                textBox16.BackColor = Color.FromArgb(252, 224, 224);
            }
            else
            {
                label68.Text = "";
                textBox16.BackColor = Color.White;
            }
        }

        private void textBox12_KeyUp(object sender, KeyEventArgs e)
        {
            string containsNum = @"[0-9~!@#$%^&*()_+=-]";
            string empname = textBox12.Text.Trim();
            if (Regex.IsMatch(textBox12.Text, containsNum))
            {
                label48.Text = "No numeric character";
                textBox12.BackColor = Color.FromArgb(252, 224, 224);
                empname.Remove(empname.Length - 1);
                textBox12.Text = empname;
            }
            else
            {
                label48.Text = "";
                textBox12.BackColor = Color.White;
            }
        }

        private void textBox11_KeyUp(object sender, KeyEventArgs e)
        {
            string containsNum = @"[0-9~!@#$%^&*()_+=-]";
            string empname = textBox11.Text.Trim();
            if (Regex.IsMatch(textBox11.Text, containsNum))
            {
                label49.Text = "No numeric character";
                textBox11.BackColor = Color.FromArgb(252, 224, 224);
                empname.Remove(empname.Length - 1);
                textBox11.Text = empname;
            }
            else
            {
                label49.Text = "";
                textBox11.BackColor = Color.White;
            }
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            string containsLetter = @"[A-Za-z~!@#$%^&*()_+=-]";
            string cno = textBox2.Text.Trim();
            if (Regex.IsMatch(cno, containsLetter))
            {
                textBox2.BackColor = Color.FromArgb(252, 224, 224);
                label52.Text = "Numeric only";
            }
            else
            {
                label52.Text = "";
                textBox2.BackColor = Color.White;
            }
        }

        private void textBox6_KeyUp(object sender, KeyEventArgs e)
        {
            string containsNum = @"[0-9~!@#$%^&*()_+=-]";
            string empname = textBox6.Text.Trim();
            if (Regex.IsMatch(textBox6.Text, containsNum))
            {
                label54.Text = "No numeric character";
                textBox6.BackColor = Color.FromArgb(252, 224, 224);
                empname.Remove(empname.Length - 1);
                textBox6.Text = empname;
            }
            else
            {
                label54.Text = "";
                textBox6.BackColor = Color.White;
            }
        }

        private void textBox5_KeyUp(object sender, KeyEventArgs e)
        {
            string containsNum = @"[0-9~!@#$%^&*()_+=-]";
            string empname = textBox5.Text.Trim();
            if (Regex.IsMatch(textBox5.Text.Trim(), containsNum))
            {
                label55.Text = "No numeric character";
                textBox5.BackColor = Color.FromArgb(252, 224, 224);
                empname.Remove(empname.Length - 1);
                textBox5.Text = empname;
            }
            else
            {
                label55.Text = "";
                textBox5.BackColor = Color.White;
            }
        }

        private void textBox8_KeyUp(object sender, KeyEventArgs e)
        {
            string containsLetter = @"[A-Za-z~!@#$%^&*()_+=-]";
            string cno = textBox8.Text.Trim();
            if (Regex.IsMatch(cno, containsLetter))
            {
                textBox8.BackColor = Color.FromArgb(252, 224, 224);
                label59.Text = "Numeric only";
            }
            else
            {
                label59.Text = "";
                textBox8.BackColor = Color.White;
            }
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox5.Items.Clear();
            string endtime = "", endampm = "am";
            string time = comboBox6.Text.Trim(), hour = time.Substring(0, 2), min = time.Substring(3, 2), ampm = time.Substring(6, 2);
            int totalhour = 0, minhour = 2, maxhour = 13, endhour = 0, endmin = 0;

            totalhour = Convert.ToInt32(hour) + minhour;

            if (totalhour > 12)
            {
                endampm = "pm";
                totalhour = totalhour - 12;
            }
            else
            {
                maxhour = maxhour + 12;
            }
            for (int j = totalhour; j < maxhour - minhour; j++)
            {
                endhour = j;
                if (endhour > 12)
                {
                    endhour = j - 12;
                    endampm = "pm";
                }
                if (endhour == 12)
                {
                    endampm = "pm";
                }
                for (int o = 0; o <= 30; o = o + 30)
                {

                    endtime = endhour.ToString("D2") + ":" + o.ToString("D2") + " " + endampm;
                    comboBox5.Items.Add(endtime);

                }
            }
            try
            {
                comboBox5.SelectedIndex = 0;
            }
            catch (Exception) { }
        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            string endtime = "", endampm = "am";
            string time = comboBox9.Text.Trim(), hour = time.Substring(0, 2), min = time.Substring(3, 2), ampm = time.Substring(6, 2);
            int totalhour = 0, minhour = 2, maxhour = 13, endhour = 0, endmin = 0;

            totalhour = Convert.ToInt32(hour) + minhour;

            if (totalhour > 12)
            {
                endampm = "pm";
                totalhour = totalhour - 12;
            }
            else
            {
                maxhour = maxhour + 12;
            }
            for (int j = totalhour; j < maxhour - minhour; j++)
            {
                endhour = j;
                if (endhour > 12)
                {
                    endhour = j - 12;
                    endampm = "pm";
                }
                if (endhour == 12)
                {
                    endampm = "pm";
                }
                for (int o = 0; o <= 30; o = o + 30)
                {
                    endtime = endhour.ToString("D2") + ":" + o.ToString("D2") + " " + endampm;
                    comboBox3.Items.Add(endtime);
                }
            }
            try
            {
                comboBox3.SelectedIndex = 0;
            }
            catch (Exception) { }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            dataGridView1.Enabled = true;
            button1.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = true;
            button30.Enabled = true;
            button11.Enabled = true;
            button12.Enabled = true;
            button8.Enabled = true;
            panel9.Hide();




        }

        private void panel28_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button22_Click_1(object sender, EventArgs e)
        {

        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            schedAdd.Visible = false;
            button30.Enabled = true;
            button11.Enabled = true;
            button12.Enabled = true;
            button24.Enabled = true;
            button23.Enabled = true;
            button22.Enabled = true;
            dataGridView3.Enabled = true;

            schedAdd.Hide();
        }

        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            positionAdd.Visible = false;
            dataGridView2.Enabled = true;
            button30.Enabled = true;
            button11.Enabled = true;
            button12.Enabled = true;
            button19.Enabled = true;
            button18.Enabled = true;

            positionAdd.Hide();
        }

        private void textBox24_TextChanged(object sender, EventArgs e)
        {

            string search = textBox24.Text.Trim();

            dataGridView1.Rows.Clear();
            try
            {
                connection.Open();
                string query = "Select *,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from employeetbl where Employee_No IN (SELECT Employee_No from employee_schedtbl) and (Employee_LName LIKE '%" + search + "%' OR Employee_FName LIKE '%" + search + "%')";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView1.Rows.Add(dataReader.GetInt32("Employee_No"), dataReader.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)"));
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            dataGridView4.Rows.Clear();
            int empno = 0;

            try
            {
                empno = Convert.ToInt32(dataGridView3.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString());
            }
            catch (Exception)
            {
                MessageBox.Show("No Result!");
                textBox18.Text = "";
            }


            try
            {
                connection.Open();
                string query1 = "SELECT *,CONCAT(e.Employee_LName,', ',e.Employee_FName,' ',e.Employee_MidInit) from employee_schedtbl es, employeetbl e where es.Employee_No = '" + empno + "' and e.Employee_No = es.Employee_No";
                MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                while (dataReader1.Read())
                {
                    textBox19.Text = dataReader1.GetString("CONCAT(e.Employee_LName,', ',e.Employee_FName,' ',e.Employee_MidInit)");
                    dataGridView4.Rows.Add(dataReader1.GetString("Schedule_Day"), dataReader1.GetString("Schedule_TimeIn"), dataReader1.GetString("Schedule_TimeOut"));
                }

                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
        }

        private void positionPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void EmployeeUC_Load(object sender, EventArgs e)
        {

        }
    }
}
