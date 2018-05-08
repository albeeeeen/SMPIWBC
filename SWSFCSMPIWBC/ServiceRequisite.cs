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
    public partial class ServiceRequisite : Form
    {
        static string connectionString = "datasource=localhost" + ";" + "DATABASE=slimmersdb" + ";" + "UID=root"
        + ";" + "PASSWORD=root" + ";";
        MySqlConnection connection = new MySqlConnection(connectionString);
        public ServiceRequisite()
        {
            InitializeComponent();
            GetServiceRequisite();
            GetFirstRequisite();
            GetRequisiteNo();
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox2.Items.Add("None");
            GetServices();
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }
        public int GetRequisiteNo()
        {
            int reqno = 0;
            try
            {
                connection.Open();
                string query = "Select * from requisite_servicetbl order by Requisite_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    reqno = dataReader.GetInt32("Requisite_No");
                }
                reqno = reqno + 1;
                textBox6.Text = reqno.ToString();
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            return reqno;
        }
        public void GetServiceRequisite()
        {
            dataGridView1.Rows.Clear();
            try
            {
                connection.Open();
                string query = "Select * from requisite_servicetbl rs, servicetbl s where rs.Service_No = s.Service_No and rs.Requisite_Status = 'Active'";
                MySqlCommand cmd = new MySqlCommand(query,connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while(dataReader.Read()){
                    dataGridView1.Rows.Add(dataReader.GetInt32("Requisite_No"), dataReader.GetString("Service_Name"), dataReader.GetString("Requisite_Service"));
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
        }
        public void GetFirstRequisite()
        {
            try
            {
                connection.Open();
                string query = "Select * from requisite_servicetbl rs, servicetbl s where rs.Service_No = s.Service_No and rs.Requisite_Status = 'Active' order by rs.Requisite_No LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    textBox1.Text = dataReader.GetInt32("Requisite_No").ToString();
                    comboBox4.Text = dataReader.GetString("Service_Name");
                    comboBox3.Text = dataReader.GetString("Requisite_Service");
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
        }
        public void GetServices()
        {
            try
            {
                connection.Open();
                string query = "Select * from servicetbl where Service_Status = 'Active' order by Service_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    comboBox1.Items.Add(dataReader.GetString("Service_Name"));
                    comboBox2.Items.Add(dataReader.GetString("Service_Name"));
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            GetRequisiteNo();
            addPanel.BringToFront();
            editPanel.SendToBack();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ServiceRequisite sr = new ServiceRequisite();
            sr.Show();
            this.Hide();
        }
        ErrorProvider errorProvider = new ErrorProvider();
        private void button10_Click(object sender, EventArgs e)
        {
            bool check = false,checker = false;
            int reqno = 0,serveno = 0,serve_reqno = 0,checkserveno = 0;
            string service = "", service_req = "",checkserve_req="";
            reqno = Convert.ToInt32(textBox6.Text);
            service = comboBox1.SelectedItem.ToString();
            service_req = comboBox2.SelectedItem.ToString();
            
            
            try
            {
                connection.Open();
                string query = "Select Service_No from servicetbl where Service_Name = '" + service + "'";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    serveno = dataReader.GetInt32("Service_No");
                }
                connection.Close();
                connection.Open();
                string query3 = "Select * from requisite_servicetbl";
                MySqlCommand cmd3 = new MySqlCommand(query3, connection);
                MySqlDataReader dataReader3 = cmd3.ExecuteReader();
                while (dataReader3.Read())
                {
                    checkserveno = dataReader3.GetInt32("Service_No");
                    checkserve_req = dataReader3.GetString("Requisite_Service");
                    if (checkserveno == serveno)
                    {
                        check = true;
                        checker = true;
                        errorProvider.SetError(comboBox1, "Service already had a requisite");
                        break;
                    }
                    else
                    {
                        errorProvider.SetError(comboBox1, string.Empty);
                    }
                    if (checkserveno == serveno && checkserve_req == service_req)
                    {
                        errorProvider.SetError(comboBox2, "Record already existed");
                        check = true;
                        checker = true;
                        break;
                    }
                    else
                    {
                        errorProvider.SetError(comboBox2, string.Empty);
                    }
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            if (check == false)
            {
                if (service == service_req)
                {
                    errorProvider.SetError(comboBox2, "Invalid service requirement");
                    checker = true;
                }
                else
                {
                    errorProvider.SetError(comboBox2, string.Empty);
                }
                
            }
            if (checker == false)
            {
                try
                {
                    connection.Open();
                    string query = "INSERT into requisite_servicetbl values ('" + reqno + "','" + serveno + "','" + service_req + "','Active')";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Service Requisite successfully added!");
                    comboBox1.SelectedIndex = 0;
                    comboBox2.SelectedIndex = 0;

                    connection.Close();
                    GetServiceRequisite();
                }
                catch (MySqlException me)
                {
                    MessageBox.Show(me.Message);
                }
            }
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            editPanel.BringToFront();
            addPanel.SendToBack();
            int reqno = 0;
            string servicename = "", reqservicename = "";
            int rows = dataGridView1.CurrentCell.RowIndex;
            reqno = Convert.ToInt32(dataGridView1.Rows[rows].Cells[0].Value);
            try
            {
                connection.Open();
                string query = "SELECT * from requisite_servicetbl rs, servicetbl s where rs.Requisite_No = '" + reqno + "' and rs.Service_No = s.Service_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    textBox1.Text = dataReader.GetInt32("Requisite_No").ToString();
                    comboBox4.Text = dataReader.GetString("Service_Name");
                    comboBox3.Text = dataReader.GetString("Requisite_Service");
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            comboBox3.Enabled = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            bool check = false;
            int reqno = 0, serveno = 0, serve_reqno = 0, checkserveno = 0;
            string service = "", service_req = "", checkserve_req = "";
            reqno = Convert.ToInt32(textBox1.Text);
            service = comboBox4.SelectedItem.ToString();
            service_req = comboBox3.SelectedItem.ToString();

            try
            {
                connection.Open();

                string query3 = "Select * from requisite_servicetbl";
                MySqlCommand cmd3 = new MySqlCommand(query3, connection);
                MySqlDataReader dataReader3 = cmd3.ExecuteReader();
                while (dataReader3.Read())
                {
                    checkserveno = dataReader3.GetInt32("Service_No");
                    checkserve_req = dataReader3.GetString("Requisite_Service");
                    if (checkserveno == serveno)
                    {
                        check = true;
                        errorProvider.SetError(comboBox4, "Service already had a requisite");
                        break;
                    }
                    else
                    {
                        errorProvider.SetError(comboBox4, string.Empty);
                    }
                    if (checkserveno == serveno && checkserve_req == service_req)
                    {
                        errorProvider.SetError(comboBox3, "Record already existed");
                        check = true;
                        break;
                    }
                    else
                    {
                        errorProvider.SetError(comboBox3, string.Empty);
                    }
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            if (check == false)
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE requisite_servicetbl set Requisite_Service = '"+service_req+"' where Requisite_No = '"+reqno+"'";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Service Requisite successfully updated!");
                    GetFirstRequisite();
                }
                catch (MySqlException me)
                {
                    MessageBox.Show(me.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Services service = new Services();
            service.Show();
            this.Hide();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            HomePage hp = new HomePage();
            hp.Show();
            this.Hide();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            int service_reqno = 0;
            int rows = 0;
            rows = dataGridView1.CurrentCell.RowIndex;
            service_reqno = Convert.ToInt32(dataGridView1.Rows[rows].Cells[0].Value);
             DialogResult dr = MessageBox.Show("Do you really want to delete?", "Delete", MessageBoxButtons.YesNo);
             if (dr == DialogResult.Yes)
             {
                 try
                 {
                     connection.Open();
                     string query = "UPDATE requisite_servicetbl set Requisite_Status = 'Deleted' where Requisite_No = '" + service_reqno + "'";
                     MySqlCommand cmd = new MySqlCommand(query, connection);
                     cmd.ExecuteNonQuery();
                     MessageBox.Show("Record deleted!");
                     connection.Close();
                     GetServiceRequisite();
                 }
                 catch (MySqlException me)
                 {
                     MessageBox.Show(me.Message);
                 }
             }
        }
    }
}
