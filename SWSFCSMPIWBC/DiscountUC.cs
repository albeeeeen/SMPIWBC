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
    public partial class DiscountUC : UserControl
    {
        static string connectionString =
        System.Configuration.ConfigurationManager.
        ConnectionStrings["SWSFCSMPIWBC.Properties.Settings.slimmersdbConnectionString"].ConnectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);
        public DiscountUC()
        {
            InitializeComponent(); ClearError();
            GetServices();
            GetAllPromo();
            GetFirstPromo();
        }
        public void ClearError()
        {
            label19.Text = "";
            richTextBox1.BackColor = Color.White;
            label23.Text = "";
            label24.Text = "";
            textBox1.BackColor = Color.White;
            label16.Text = "";
            richTextBox2.BackColor = Color.White;
            label14.Text = "";
            label15.Text = "";
            textBox2.BackColor = Color.White;
            label28.Text = "";
            label18.Text = "";
        }
        public void GetAllPromo()
        {
            dataGridView1.Rows.Clear();
            try
            {
                connection.Open();
                string query = "SELECT * from service_promotbl where Promo_Status <> 'Deleted' order by Promo_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView1.Rows.Add(dataReader.GetInt32("Promo_No"), dataReader.GetString("Promo_Description"), dataReader.GetDateTime("Promo_Start").ToString("yyyy-MM-dd"), dataReader.GetDateTime("Promo_End").ToString("yyyy-MM-dd"), dataReader.GetString("Promo_Status"));
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
        }
        public void GetFirstPromo()
        {
            try
            {
                connection.Open();
                string query = "SELECT * from service_promotbl sp, discount_servicestbl ds,servicetbl s where sp.Promo_No = (SELECT Promo_No from service_promotbl where Promo_Status <> 'Deleted' order by Promo_No LIMIT 1) and sp.Promo_No = ds.Promo_No and ds.Service_No = s.Service_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    textBox4.Text = dataReader.GetInt32("Promo_No").ToString();
                    richTextBox1.Text = dataReader.GetString("Promo_Description");
                    dateTimePicker2.Value = Convert.ToDateTime(dataReader.GetDateTime("Promo_Start").ToShortDateString());
                    dateTimePicker1.Value = Convert.ToDateTime(dataReader.GetDateTime("Promo_End").ToShortDateString());
                    dataGridView2.Rows.Add(dataReader.GetString("Service_Name"), dataReader.GetInt32("Discount_Rate"));
                    if(dataReader.GetString("Promo_Status") == "Done")
                    {
                        button7.Visible = false;
                    }
                    else
                    {
                        button7.Visible = true;
                    }
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
        }
        public void GetPromoNo()
        {
            int promono = 0;
            try
            {
                connection.Open();
                string query = "SELECT Promo_No from service_promotbl order by Promo_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    promono = dataReader.GetInt32("Promo_No");
                }
                connection.Close();
                promono = promono + 1;
                textBox3.Text = promono.ToString();

            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
        }
        public void GetServices()
        {
            comboBox3.Items.Clear();
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
                    comboBox3.Items.Add(dataReader.GetString("Service_Name"));
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            comboBox3.SelectedIndex = 0;
            comboBox1.SelectedIndex = 0;
        }
        public int GetServiceDiscount()
        {
            int servediscount = 0;
            try
            {
                connection.Open();
                string query = "Select Service_DiscountNo from discount_servicestbl order by Service_DiscountNo";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    servediscount = dataReader.GetInt32("Service_DiscountNo");
                }
                servediscount = servediscount + 1;
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            return servediscount;
        }
        ErrorProvider errorProvider = new ErrorProvider();
        private void button4_Click(object sender, EventArgs e)
        {
            panel11.Visible = false;
            dataGridView1.Enabled = false;
            button13.Enabled = false;
            button7.Enabled = false;
            button12.Enabled = false;
            button4.Enabled = false;

            addTransition.ShowSync(panel11);
            panel7.SendToBack();
            panel11.Visible = true;
            panel11.BringToFront();
            GetServices();
            GetPromoNo();
            dateTimePicker6.MinDate = DateTime.Today;
            dateTimePicker5.MinDate = DateTime.Now.AddDays(1);
        }
        private void richTextBox2_Leave(object sender, EventArgs e)
        {
            string desc = richTextBox2.Text.Trim();
            bool exists = false;
            string checkdesc = "";
            if (desc.Length == 0)
            {
                errorProvider.SetError(richTextBox2, "Promo description is required");
            }
            else
            {
                connection.Open();
                string query = "Select Promo_Description from service_promotbl";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    checkdesc = dataReader.GetString("Promo_Description");
                    if (checkdesc.Equals(desc))
                    {
                        exists = true;
                        break;

                    }
                }
                connection.Close();
                if (exists == true)
                {
                    errorProvider.SetError(richTextBox2, "Promo description already exists");
                }
                else
                {
                    errorProvider.SetError(richTextBox2, string.Empty);
                }
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            errorProvider.SetError(textBox2, string.Empty);
            try
            {
                int rate = Convert.ToInt32(textBox2.Text);
            }
            catch (FormatException)
            {
                errorProvider.SetError(textBox2, "Invalid discount rate");
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            bool check = false;
            string service = comboBox3.Text;
            int rate = 0;
            errorProvider.SetError(textBox2, string.Empty);
            try
            {
                rate = Convert.ToInt32(textBox2.Text);
            }
            catch (FormatException)
            {
                label15.Text = "Invalid discount rate";
                textBox2.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }
            if (rate > 100)
            {
                label15.Text = "Discount Rate is exceeding in 100%";
                textBox2.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }
            else
            {
                label15.Text = "";
                textBox2.BackColor = Color.White;
            }

            for (int i = 0; i < dataGridView4.Rows.Count; i++)
            {
                if (service == dataGridView4.Rows[i].Cells[0].Value.ToString())
                {
                    label14.Text = "Service already exists in the datagridview";
                    check = true;
                    break;
                }
                else
                {
                    label14.Text = "";
                }
            }
            if (check == false)
            {
                dataGridView4.Rows.Add(service, rate);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            errorProvider.SetError(comboBox3, string.Empty);
            try
            {
                dataGridView4.Rows.RemoveAt(dataGridView4.CurrentRow.Index);
            }
            catch (NullReferenceException ne)
            {
                MessageBox.Show("No selected row");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            int promono = Convert.ToInt32(textBox3.Text);
            string desc = richTextBox2.Text.Trim();
            bool check = false;
            bool exists = false;
            int serviceno = 0, rate = 0;
            string checkdesc = "", service = "";
            string datestart = "", dateend = "";
            datestart = dateTimePicker6.Value.ToString("yyyy-MM-dd");
            dateend = dateTimePicker5.Value.ToString("yyyy-MM-dd");
            if (dataGridView4.Rows.Count == 0)
            {
                label18.Text = "Select service first for promo";
                check = true;
            }
            else
            {
                label18.Text = "";
            }
            if (desc.Length == 0)
            {
                label16.Text = "Promo description is required";
                richTextBox2.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }
            else
            {
                connection.Open();
                string query = "Select Promo_Description from service_promotbl";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    checkdesc = dataReader.GetString("Promo_Description");
                    if (checkdesc.Equals(desc))
                    {
                        exists = true;
                        break;

                    }
                }
                connection.Close();
                if (exists == true)
                {
                    label16.Text = "Promo description already exists";
                    richTextBox2.BackColor = Color.FromArgb(252, 224, 224);
                    check = true;
                }
                else
                {
                    label16.Text = "";
                    richTextBox2.BackColor = Color.White;
                }
            }
            if (check == false)
            {
                try
                {
                    connection.Open();
                    string query3 = "Insert into service_promotbl values ('" + promono + "','" + desc + "','" + datestart + "','" + dateend + "','Pending')";
                    MySqlCommand cmd3 = new MySqlCommand(query3, connection);
                    cmd3.ExecuteNonQuery();
                    connection.Close();
                }
                catch (MySqlException me)
                {
                    MessageBox.Show(me.Message);
                }
                for (int i = 0; i < dataGridView4.Rows.Count; i++)
                {
                    int servediscount = GetServiceDiscount();
                    service = dataGridView4.Rows[i].Cells[0].Value.ToString();
                    rate = Convert.ToInt32(dataGridView4.Rows[i].Cells[1].Value);
                    try
                    {
                        connection.Open();
                        string query1 = "Select Service_No from servicetbl where Service_Name = '" + service + "'";
                        MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                        MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                        while (dataReader1.Read())
                        {
                            serviceno = dataReader1.GetInt32("Service_No");
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
                        string query2 = "Insert into discount_servicestbl values ('" + servediscount + "','" + promono + "','" + serviceno + "','" + rate + "')";
                        MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                        cmd2.ExecuteNonQuery();
                        connection.Close();
                    }
                    catch (MySqlException me)
                    {
                        MessageBox.Show(me.Message);
                    }
                }
                MessageBox.Show("Promo added successfully");
                GetAllPromo();
                richTextBox2.Text = "";
                comboBox3.SelectedIndex = 0;
                textBox2.Text = "";
                dataGridView4.Rows.Clear();
                panel7.BringToFront();
                panel11.SendToBack();
                panel11.Visible = false;
                dataGridView1.Enabled = true;
                button13.Enabled = true;
                button7.Enabled = true;
                button12.Enabled = true;
                button4.Enabled = true;
                GetFirstPromo();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            richTextBox1.ReadOnly = false;
            richTextBox1.BorderStyle = BorderStyle.FixedSingle;
            dateTimePicker1.Enabled = true;
            dateTimePicker2.Enabled = true;
            comboBox1.Enabled = true;
            textBox1.ReadOnly = false;
            button9.Visible = true;
            button11.Visible = true;
            button12.Enabled = true;
            button12.Visible = true;
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            ClearError();
            panel7.BringToFront();
            panel11.SendToBack();
            richTextBox1.ReadOnly = true;
            dateTimePicker1.Enabled = false;
            dateTimePicker2.Enabled = false;
            comboBox1.Enabled = false;
            textBox1.ReadOnly = true;
            button9.Visible = false;
            button11.Visible = false;
            richTextBox1.BorderStyle = BorderStyle.None;
            dataGridView2.Rows.Clear();
            int row = dataGridView1.CurrentCell.RowIndex;
            int promono = Convert.ToInt32(dataGridView1.Rows[row].Cells[0].Value);
            try
            {
                connection.Open();
                string query = "SELECT * from service_promotbl sp, discount_servicestbl ds,servicetbl s where sp.Promo_No = '" + promono + "' and sp.Promo_No = ds.Promo_No and ds.Service_No = s.Service_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    textBox4.Text = dataReader.GetInt32("Promo_No").ToString();
                    richTextBox1.Text = dataReader.GetString("Promo_Description");
                    dateTimePicker2.Value = Convert.ToDateTime(dataReader.GetDateTime("Promo_Start").ToShortDateString());
                    dateTimePicker1.Value = Convert.ToDateTime(dataReader.GetDateTime("Promo_End").ToShortDateString());
                    dataGridView2.Rows.Add(dataReader.GetString("Service_Name"), dataReader.GetInt32("Discount_Rate"));
                    if(dataReader.GetString("Promo_Status") == "Done")
                    {
                        button7.Visible = false;
                    }
                    else
                    {
                        button7.Visible = true;
                    }
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            int promono = Convert.ToInt32(textBox4.Text);
            string desc = richTextBox1.Text.Trim();
            bool check = false;
            bool exists = false;
            int serviceno = 0, rate = 0;
            string checkdesc = "", service = "";
            string datestart = "", dateend = "";
            datestart = dateTimePicker2.Value.ToString("yyyy-MM-dd");
            dateend = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            if (dataGridView2.Rows.Count == 0)
            {
                label28.Text = "No service for the promo";
                check = true;
            }
            else
            {
                label28.Text = "";
            }
            if (desc.Length == 0)
            {
                label19.Text = "Promo description is required";
                richTextBox1.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }
            else
            {
                connection.Open();
                string query = "Select Promo_Description from service_promotbl where Promo_Description != '" + desc + "'";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    checkdesc = dataReader.GetString("Promo_Description");
                    if (checkdesc.Equals(desc))
                    {
                        exists = true;
                        break;

                    }
                }
                connection.Close();
                if (exists == true)
                {
                    label19.Text = "Promo description already exists";
                    richTextBox1.BackColor = Color.FromArgb(252, 224, 224);
                    check = true;
                }
                else
                {
                    label19.Text = "";
                    richTextBox1.BackColor = Color.White;
                }
            }
            if (check == false)
            {
                try
                {
                    connection.Open();
                    string query1 = "Update service_promotbl set Promo_Description = '" + desc + "', Promo_Start = '" + datestart + "', Promo_End = '" + dateend + "' where Promo_No = '" + promono + "'";
                    MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                    cmd1.ExecuteNonQuery();
                    connection.Close();
                }
                catch (MySqlException me)
                {
                    MessageBox.Show(me.Message);
                }
                try
                {
                    connection.Open();
                    string query2 = "delete from discount_servicestbl where Promo_No = '" + promono + "'";
                    MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                    cmd2.ExecuteNonQuery();
                    connection.Close();
                }
                catch (MySqlException me)
                {
                    MessageBox.Show(me.Message);
                }
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    int servicedisc = GetServiceDiscount();
                    service = dataGridView2.Rows[i].Cells[0].Value.ToString();
                    rate = Convert.ToInt32(dataGridView2.Rows[i].Cells[1].Value);
                    try
                    {
                        connection.Open();
                        string query3 = "Select Service_No from servicetbl where Service_Name = '" + service + "'";
                        MySqlCommand cmd3 = new MySqlCommand(query3, connection);
                        MySqlDataReader dataReader3 = cmd3.ExecuteReader();
                        while (dataReader3.Read())
                        {
                            serviceno = dataReader3.GetInt32("Service_No");
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
                        string query4 = "Insert into discount_servicestbl values ('" + servicedisc + "','" + promono + "','" + serviceno + "','" + rate + "')";
                        MySqlCommand cmd4 = new MySqlCommand(query4, connection);
                        cmd4.ExecuteNonQuery();
                        connection.Close();
                    }
                    catch (MySqlException me)
                    {
                        MessageBox.Show(me.Message);
                    }
                }
                MessageBox.Show("Promo updated successfully");
                GetAllPromo();
                GetFirstPromo();
                panel7.BringToFront();
                panel11.SendToBack();
                richTextBox1.ReadOnly = true;
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;
                comboBox1.Enabled = false;
                textBox1.ReadOnly = true;
                button9.Visible = false;
                button11.Visible = false;
                button12.Enabled = false;
                richTextBox1.BorderStyle = BorderStyle.None;
            }

        }

        private void button13_Click(object sender, EventArgs e)
        {
            int promo_no = 0;
            int rows = 0;
            rows = dataGridView1.CurrentCell.RowIndex;
            promo_no = Convert.ToInt32(dataGridView1.Rows[rows].Cells[0].Value);
            DialogResult dr = MessageBox.Show("Do you really want to delete?", "Delete", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE service_promotbl set Promo_status = 'Deleted' where Promo_No = '" + promo_no + "'";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record deleted!");
                    connection.Close();
                    GetAllPromo();
                }
                catch (MySqlException me)
                {
                    MessageBox.Show(me.Message);
                }
            }
        }
        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }
        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            string discount = textBox2.Text.Trim();
            string containLetter = @"[A-Za-z~!@#$%^&*()_+=-]";

            if (Regex.IsMatch(discount, containLetter))
            {
                label15.Text = "Numeric only";
                textBox2.BackColor = Color.FromArgb(252, 224, 224);
                discount = discount.Remove(discount.Length - 1);
                textBox2.Text = discount;
            }
            else
            {
                label15.Text = "";
                textBox2.BackColor = Color.White;
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void label27_Click(object sender, EventArgs e)
        {

        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            panel11.Visible = false;
            dataGridView1.Enabled = true;
            button13.Visible = true;
            button7.Visible = true;
            button4.Visible = true;
        }
       
    }
}
