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
    public partial class EmployeeSched : Form
    {
        static string connectionString = "datasource=localhost" + ";" + "DATABASE=slimmersdb" + ";" + "UID=root"
        + ";" + "PASSWORD=root" + ";";
        MySqlConnection connection = new MySqlConnection(connectionString);
        public EmployeeSched()
        {
            InitializeComponent();
            GetAllEmployeeSched();
            GetSelectedEmployee();
            comboBox5.SelectedIndex = 0;
            comboBox6.SelectedIndex = 0;
            comboBox7.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Maintenance mainte = new Maintenance();
            mainte.Show();
            this.Hide();
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
            dataGridView3.Rows.Clear();
            try
            {
                connection.Open();
                string query = "SELECT *,CONCAT(e.Employee_LName,', ',e.Employee_FName,' ',e.Employee_MidInit) from employee_schedtbl es, employeetbl e where es.Employee_No = (SELECT e.Employee_No from employeetbl e, employee_schedtbl es where e.Employee_No = es.Employee_No order by Employee_No LIMIT 1) and e.Employee_No = es.Employee_No and e.Employee_Status = 'Active'";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    empname = dataReader.GetString("CONCAT(e.Employee_LName,', ',e.Employee_FName,' ',e.Employee_MidInit)");
                    dataGridView3.Rows.Add(dataReader.GetString("Schedule_Day"), dataReader.GetString("Schedule_TimeIn"), dataReader.GetString("Schedule_TimeOut"));
                }
                comboBox8.Text = empname;
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
        }
        public void GetAllEmployeeSched()
        {
            dataGridView1.Rows.Clear();
            try
            {
                connection.Open();
                string query = "Select *,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from employeetbl where Employee_No IN (SELECT Employee_No from employee_schedtbl)";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView1.Rows.Add(dataReader.GetString("Employee_No"),dataReader.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)"));
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
        }
        public void GetAllEmployee()
        {
            bool check = true;
            comboBox1.Items.Clear();
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
                            comboBox1.Items.Add(dataReader.GetString("CONCAT(e.Employee_LName,', ',e.Employee_FName,' ',e.Employee_MidInit)"));
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
                            comboBox1.Items.Add(dataReader.GetString("CONCAT(e.Employee_LName,', ',e.Employee_FName,' ',e.Employee_MidInit)"));
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

        private void button12_Click(object sender, EventArgs e)
        {
            EmployeePosition emppos = new EmployeePosition();
            emppos.Show();
            this.Hide();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            EmployeeSched empsched = new EmployeeSched();
            empsched.Show();
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

        private void button9_Click(object sender, EventArgs e)
        {
            Promo dp = new Promo();
            dp.Show();
            this.Hide();
        }
        ErrorProvider errorProvider = new ErrorProvider();
        private void button14_Click(object sender, EventArgs e)
        {
          
            string empname = comboBox1.Text;
            string day = "";
            string timein = "";
            string timeout = "";
            bool exists = false, check = false;
            try
            {
                day = comboBox2.Text;
                errorProvider.SetError(comboBox2, string.Empty);
            }
            catch (Exception)
            {
                errorProvider.SetError(comboBox2, "Please select working day");
                check = true;
            }
            try
            {
                timein = comboBox3.Text;
                timeout = comboBox4.Text;
                errorProvider.SetError(comboBox4, string.Empty);
            }catch(Exception)
            {
                errorProvider.SetError(comboBox4, "Please select time");
                check = true;
            }
            
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if (day == dataGridView2.Rows[i].Cells[0].Value.ToString())
                {
                    errorProvider.SetError(comboBox2, "Day already exists in the table");
                    exists = true;
                    break;
                }
              
            }
            if (exists == false && check == false)
            {
                dataGridView2.Rows.Add(day, timein, timeout);
            }
           

        }

        private void button13_Click(object sender, EventArgs e)
        {
            errorProvider.SetError(comboBox2, string.Empty);
            try
            {
                dataGridView2.Rows.RemoveAt(dataGridView2.CurrentRow.Index);
            }
            catch (NullReferenceException ne)
            {
                MessageBox.Show("No selected row");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string emp = comboBox1.Text;
            string day, timein, timeout;
            int empno = 0;
            int schedno = 0;
            
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
          
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {

                schedno = GetScheduleNo();
                day = dataGridView2.Rows[i].Cells[0].Value.ToString();
                timein = dataGridView2.Rows[i].Cells[1].Value.ToString();
                timeout = dataGridView2.Rows[i].Cells[2].Value.ToString();

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
            comboBox1.Items.Clear();
            GetAllEmployee();
            try
            {
                comboBox1.SelectedIndex = 0;
            }
            catch (Exception)
            {
            }
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
            dataGridView2.Rows.Clear();
            GetAllEmployeeSched();
            GetSelectedEmployee();
            addPanel.SendToBack();
            editPanel.BringToFront();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            addPanel.BringToFront();
            editPanel.SendToBack();
            GetAllEmployee();
            try
            {
                comboBox1.SelectedIndex = 0;
            }
            catch (Exception)
            {
                MessageBox.Show("All employee have their schedule");
                addPanel.SendToBack();
                editPanel.BringToFront();
            }
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            comboBox5.Enabled = true;
            comboBox6.Enabled = true;
            comboBox7.Enabled = true;
            button15.Visible = true;
            button16.Visible = true;
            button8.Enabled = true;
        }

        private void button16_Click(object sender, EventArgs e)
        {

            string day = "";
            string timein = "";
            string timeout = "";
            bool exists = false, check = false;
            try
            {
                day = comboBox7.Text;
                errorProvider.SetError(comboBox7, string.Empty);
            }
            catch (Exception)
            {
                errorProvider.SetError(comboBox7, "Please select working hour");
                check = true;
            }
            try
            {
                timein = comboBox6.Text;
                timeout = comboBox5.Text;
                errorProvider.SetError(comboBox5, string.Empty);
            }
            catch (Exception)
            {
                errorProvider.SetError(comboBox5, "Please select time");
            }
            for (int i = 0; i < dataGridView3.Rows.Count; i++)
            {
                if (day == dataGridView3.Rows[i].Cells[0].Value.ToString())
                {
                    errorProvider.SetError(comboBox7, "Day already exists in the table");
                    exists = true;
                    break;
                }

            }
            if (exists == false && check == false)
            {
                dataGridView3.Rows.Add(day, timein, timeout);
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            errorProvider.SetError(comboBox7, string.Empty);
            try
            {
                dataGridView3.Rows.RemoveAt(dataGridView3.CurrentRow.Index);
            }
            catch (NullReferenceException ne)
            {
                MessageBox.Show("No selected row");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string empname = comboBox8.Text;
            string day, timein, timeout;
            errorProvider.SetError(comboBox7, string.Empty);
            int empno = 0;
            int schedno = 0;
            try
            {
                connection.Open();
                string query = "Select Employee_No,CONCAT(Employee_LName, ', ',Employee_FName,' ',Employee_MidInit) from employeetbl where CONCAT(Employee_LName, ', ',Employee_FName,' ',Employee_MidInit) =  '" + empname + "'";
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
            try
            {
                connection.Open();
                string query1 = "Delete from employee_schedtbl where Employee_No = '" + empno + "'";
                MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                cmd1.ExecuteNonQuery();
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            for (int i = 0; i < dataGridView3.Rows.Count; i++)
            {
                schedno = GetScheduleNo();
                day = dataGridView3.Rows[i].Cells[0].Value.ToString();
                timein = dataGridView3.Rows[i].Cells[1].Value.ToString();
                timeout = dataGridView3.Rows[i].Cells[2].Value.ToString();

                try
                {
                    connection.Open();
                    string query2 = "Insert into employee_schedtbl values('"+schedno+"','"+day+"','"+timein+"','"+timeout+"','"+empno+"')";
                    MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                    cmd2.ExecuteNonQuery();
                    connection.Close();
                }
                catch (MySqlException me)
                {
                    MessageBox.Show(me.Message);
                }
            }
            MessageBox.Show("Employee schedule successfully updated!");
            GetAllEmployee();
            GetAllEmployeeSched();
            GetSelectedEmployee();
            editPanel.BringToFront();
            addPanel.SendToBack();
            comboBox5.Enabled = false;
            comboBox6.Enabled = false;
            comboBox7.Enabled = false;
            button15.Visible = false;
            button16.Visible = false;
            button8.Enabled = false;
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            addPanel.SendToBack();
            editPanel.BringToFront();
            dataGridView3.Rows.Clear();
            int rows = dataGridView1.CurrentCell.RowIndex;
            string empname = dataGridView1.Rows[rows].Cells[1].Value.ToString();
            try
            {
                connection.Open();
                string query = "SELECT *,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from employee_schedtbl es, employeetbl e where CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) = '" + empname + "' and e.Employee_No = es.Employee_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    comboBox8.Text = dataReader.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)");
                    dataGridView3.Rows.Add(dataReader.GetString("Schedule_Day"), dataReader.GetString("Schedule_TimeIn"), dataReader.GetString("Schedule_TimeOut"));
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
        }

        private void textBox24_TextChanged(object sender, EventArgs e)
        {
            string search = textBox24.Text.Trim();
 
            dataGridView1.Rows.Clear();
            try
            {
                connection.Open();
                string query = "Select *,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from employeetbl where Employee_LName LIKE '%" + search + "%' OR Employee_FName LIKE '%" + search + "%' and Employee_No IN (SELECT Employee_No from employee_schedtbl)";
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
            dataGridView3.Rows.Clear();
            int empno = 0;
            
            try
            {
                empno = Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString());
            }
            catch (Exception)
            {
                MessageBox.Show("No Result!");
                textBox24.Text = "";
            }
            
            
            try
            {
                connection.Open();
                string query1 = "SELECT *,CONCAT(e.Employee_LName,', ',e.Employee_FName,' ',e.Employee_MidInit) from employee_schedtbl es, employeetbl e where es.Employee_No = '" + empno + "' and e.Employee_No = es.Employee_No";
                MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                while (dataReader1.Read())
                {
                    comboBox8.Text = dataReader1.GetString("CONCAT(e.Employee_LName,', ',e.Employee_FName,' ',e.Employee_MidInit)");
                    dataGridView3.Rows.Add(dataReader1.GetString("Schedule_Day"), dataReader1.GetString("Schedule_TimeIn"), dataReader1.GetString("Schedule_TimeOut"));
                }
                
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox5.Items.Clear();
            string endtime = "", endampm = "am";
            string time = comboBox6.Text.Trim(), hour = time.Substring(0, 2), min = time.Substring(3, 2), ampm = time.Substring(6, 2);
            int totalhour = 0, minhour = 2, maxhour = 13,endhour = 0;

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
                
                for (int o = 0; o <= 30; o = o + 30)
                {
                    endtime = j.ToString("D2") + ":" + o.ToString("D2") + endampm;
                    comboBox5.Items.Add(endtime);
                }
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            this.Hide();
            Discounts discounts = new Discounts();
            discounts.Show();
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox4.Items.Clear();
            string endtime = "", endampm = "am";
            string time = comboBox3.Text.Trim(), hour = time.Substring(0, 2), min = time.Substring(3, 2), ampm = time.Substring(6, 2);
            int totalhour = 0, minhour = 2, maxhour = 13, endhour = 0;

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

                for (int o = 0; o <= 30; o = o + 30)
                {
                    endtime = j.ToString("D2") + ":" + o.ToString("D2") + endampm;
                    comboBox4.Items.Add(endtime);
                }
            }
        }
    }
}
