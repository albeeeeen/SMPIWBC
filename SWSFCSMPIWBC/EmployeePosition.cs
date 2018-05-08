using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SWSFCSMPIWBC
{
    public partial class EmployeePosition : Form
    {
        static string connectionString = "datasource=localhost" + ";" + "DATABASE=slimmersdb" + ";" + "UID=root"
          + ";" + "PASSWORD=root" + ";";
        MySqlConnection connection = new MySqlConnection(connectionString);
        public EmployeePosition()
        {
            InitializeComponent();
        }

        private void EmployeePosition_Load(object sender, EventArgs e)
        {
            GetAllEmpPosition();

            int posno = 0;
            string posname,posdesc;
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

                    textBox1.Text = posname;
                    textBox2.Text = posno.ToString();
                    richTextBox2.Text = posdesc;
                }
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();
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
                textBox6.Text = posno.ToString();
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
                    dataGridView1.Rows.Add(posno, posname);
                }
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();
        }
        private void headerPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            editPanel.SendToBack();
            addPanel.BringToFront();
            GetPositionNo();
        }

        private void editPanel_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void button10_Click(object sender, EventArgs e)
        {
            bool check = false;
            bool exists = false;

            string posname = textBox5.Text.Trim(),checkpos;
            string posdesc = richTextBox1.Text.Trim();
            string containNumber = @"[0-9~!@#$%^&*()_+=-]";
            int posno = Convert.ToInt32(textBox6.Text.Trim());
            if (posname.Length == 0)
            {
                errorProvider.SetError(textBox5, "Position name is requried.");
                check = true;
            }
            else
            {
                if (Regex.IsMatch(posname, containNumber))
                {
                    errorProvider.SetError(textBox5, "Position name should not contain numbers");
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
                errorProvider.SetError(textBox5, "Position name already exists");
                check = true;
            }
            else
            {
                errorProvider.SetError(textBox5, string.Empty);
            }

            if (posdesc.Length == 0)
            {
                errorProvider.SetError(richTextBox1, "Position description is required");
                check = true;
            }
            else
            {
                errorProvider.SetError(richTextBox1, string.Empty);
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
                    textBox5.Text = "";
                    richTextBox1.Text = "";
                    dataGridView1.Rows.Clear();
                    addPanel.SendToBack();
                    editPanel.BringToFront();
                    GetAllEmpPosition();
                }
                catch (MySqlException me)
                {
                    MessageBox.Show(me.Message);
                }
            }
        }
        ErrorProvider errorProvider = new ErrorProvider();
        private void textBox5_Leave(object sender, EventArgs e)
        {
            string posname = textBox5.Text.Trim();
            string containNumber = @"[0-9~!@#$%^&*()_+=-]";

            if (posname.Length == 0)
            {
                errorProvider.SetError(textBox5, "Position name is requried.");
            }
            else
            {
                if (Regex.IsMatch(posname, containNumber))
                {
                    errorProvider.SetError(textBox5, "Position name should not contain numbers");
                }
                else
                {
                    errorProvider.SetError(textBox5, string.Empty);
                }
            }
        }

        private void richTextBox1_Leave(object sender, EventArgs e)
        {
            string posdesc;
            posdesc = richTextBox1.Text.Trim();

            if (posdesc.Length == 0)
            {
                errorProvider.SetError(richTextBox1, "Position Description is required");

            }
            else
            {
                errorProvider.SetError(richTextBox1, string.Empty);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            
            EmployeePosition epos = new EmployeePosition();
            epos.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            Maintenance mainte = new Maintenance();
            mainte.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            Services service = new Services();
            service.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            Employee emp = new Employee();
            emp.Show();
            this.Hide();
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

        private void button11_Click(object sender, EventArgs e)
        {
            EmployeeSched empsched = new EmployeeSched();
            empsched.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            bool check = false, exists = false;
            string posname, posdesc, checkpos;
            posname = textBox1.Text.Trim();
            posdesc = richTextBox2.Text.Trim();

            string containNumber = @"[0-9~!@#$%^&*()_+=-]";
            int posno = Convert.ToInt32(textBox2.Text.Trim());
            if (posname.Length == 0)
            {
                errorProvider.SetError(textBox5, "Position name is requried.");
                check = true;
            }
            else
            {
                if (Regex.IsMatch(posname, containNumber))
                {
                    errorProvider.SetError(textBox5, "Position name should not contain numbers");
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
                errorProvider.SetError(textBox5, "Position name already exists");
                check = true;
            }
            else
            {
                errorProvider.SetError(textBox5, string.Empty);
            }

            if (posdesc.Length == 0)
            {
                errorProvider.SetError(richTextBox1, "Position description is required");
                check = true;
            }
            else
            {
                errorProvider.SetError(richTextBox1, string.Empty);
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
                    dataGridView1.Rows.Clear();
                    GetAllEmpPosition();
                    textBox1.ReadOnly = true;
                    richTextBox2.ReadOnly = true;
                    button8.Enabled = false;
                }
                catch (MySqlException me)
                {
                    MessageBox.Show(me.Message);
                }
            }
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            addPanel.SendToBack();
            editPanel.BringToFront();
            textBox1.ReadOnly = true;
            richTextBox2.ReadOnly = true;
            int rows = 0, posno = 0;

            rows = dataGridView1.CurrentCell.RowIndex;
            posno = Convert.ToInt32(dataGridView1.Rows[rows].Cells[0].Value);

            try
            {
                connection.Open();
                string query4 = "select * from employee_positiontbl where Employee_Position_No = '" + posno + "' order by Employee_Position_No LIMIT 1";
                MySqlCommand cmd4 = new MySqlCommand(query4, connection);
                MySqlDataReader dataReader4 = cmd4.ExecuteReader();
                while (dataReader4.Read())
                {
                    textBox1.Text = dataReader4.GetString("Position_Name");
                    textBox2.Text = dataReader4.GetInt32("Employee_Position_No").ToString();
                    richTextBox2.Text = dataReader4.GetString("Position_Description");
                }
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.ReadOnly = false;
            richTextBox2.ReadOnly = false;
            button8.Enabled = true;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            HomePage hp = new HomePage();
            hp.Show();
            this.Hide();
        }

        private void textBox24_TextChanged(object sender, EventArgs e)
        {
            editPanel.BringToFront();
            addPanel.SendToBack();
            string pos = textBox24.Text.Trim();
            dataGridView1.Rows.Clear();
            try
            {
                connection.Open();
                string query = "Select * from employee_positiontbl where Position_Name LIKE '%" + pos + "%'";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView1.Rows.Add(dataReader.GetInt32("Employee_Position_No"),dataReader.GetString("Position_Name"));
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
                posnum = Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString());
            }
            catch (Exception)
            {
                MessageBox.Show("No Result!");
                textBox24.Text = "";
            }
            try
            {
                connection.Open();
                string query = "Select * from employee_positiontbl where Employee_Position_No = '"+posnum+"' order by Employee_Position_No LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    textBox2.Text = dataReader.GetInt32("Employee_Position_No").ToString();
                    textBox1.Text = dataReader.GetString("Position_Name");
                    richTextBox2.Text = dataReader.GetString("Position_Description");
                }
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {
            this.Hide();
            Discounts discounts = new Discounts();
            discounts.Show();
        }

        private void button14_Click(object sender, EventArgs e)
        {
           
        }
    }
}
