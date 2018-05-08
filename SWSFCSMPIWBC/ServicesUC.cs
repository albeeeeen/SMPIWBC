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
    public partial class ServicesUC : UserControl
    {
        static string connectionString =
       System.Configuration.ConfigurationManager.
       ConnectionStrings["SWSFCSMPIWBC.Properties.Settings.slimmersdbConnectionString"].ConnectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);

        public ServicesUC()
        {
            InitializeComponent();
            ClearError();
            GetAllService();
            GetProducts();
            GetFirstService();
            /* try
             {
                 connection.Open();
                 string query3 = "Select * from servicetbl s, requisite_servicetbl rs where s.ServiceNo = (Select Service_No from servicetbl order by Service_No LIMIT 1)";
                 MySqlCommand cmd3 = new MySqlCommand(query3, connection);
                 MySqlDataReader dataReader3 = cmd3.ExecuteReader();
                 while (dataReader3.Read())
                 {
                     reqservice = dataReader3.GetString("Service_Name");
                 }
                 connection.Close();
             }
             catch (MySqlException me)
             {
                 MessageBox.Show(me.Message);
             }   GETTING THE FIRST REQUISITE SERVICE IN EDIT*/

            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox7.ReadOnly = true;
            numericUpDown3.Enabled = false;
            textBox2.BorderStyle = BorderStyle.None;
            textBox3.BorderStyle = BorderStyle.None;
            textBox7.BorderStyle = BorderStyle.None;
            numericUpDown3.BorderStyle = BorderStyle.None;
            comboBox4.Hide();
            button14.Hide();
            button13.Hide();
            button8.Visible = false;
        }
        public void GetProducts()
        {
            comboBox2.Items.Clear();
            comboBox1.Items.Clear();
            try
            {
                connection.Open();
                string query = "Select * from product_typetbl order by Product_Type_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    comboBox1.Items.Add(dataReader.GetString("Product_Type"));
                    comboBox2.Items.Add(dataReader.GetString("Product_Type"));
                }
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();
            try
            {
                comboBox1.SelectedIndex = 0;
            }
            catch (Exception)
            {
                comboBox1.Items.Add("No available");
                comboBox1.SelectedIndex = 0;
            }
            try
            {
                comboBox2.SelectedIndex = 0;
            }
            catch (Exception)
            {
                comboBox2.Items.Add("No available");
                comboBox2.SelectedIndex = 0;
            }
        }
        public void ClearError()
        {
            label1.Text = "";
            textBox2.BackColor = Color.White;
            label28.Text = "";
            textBox3.BackColor = Color.White;
            label29.Text = "";
            textBox7.BackColor = Color.White;
            label30.Text = "";
            label26.Text = "";
            numericUpDown3.BackColor = Color.White;
            label31.Text = "";
            label32.Text = "";
            textBox5.BackColor = Color.White;
            label33.Text = "";
            textBox4.BackColor = Color.White;
            label34.Text = "";
            numericUpDown5.BackColor = Color.White;
            label37.Text = "";
            numericUpDown2.BackColor = Color.White;
            label36.Text = "";
            label35.Text = "";
        }
        public void GetFirstService()
        {
            string servicename = "", machinename = "";
            int serviceno = 0, visit = 0, hour = 0, min = 0;
            decimal servicefee = 0;
            dataGridView3.Rows.Clear();
            try
            {
                connection.Open();
                string query2 = "Select * from servicetbl s, service_producttbl sp, producttbl p, product_typetbl pt, product_prodtypetbl ppt where s.Service_No = (SELECT Service_No from servicetbl order by Service_No LIMIT 1) and s.Service_No = sp.Service_No and sp.Product_ProdType_No = ppt.Product_ProdType_No and ppt.Product_No = p.Product_No and ppt.Product_Type_No = pt.Product_Type_No order by s.Service_No,p.Product_No";
                MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                MySqlDataReader dataReader2 = cmd2.ExecuteReader();
                while (dataReader2.Read())
                {

                    dataGridView3.Rows.Add(dataReader2.GetString("Product_Name"), dataReader2.GetString("Product_Type"));
                    serviceno = dataReader2.GetInt32("Service_No");
                    servicename = dataReader2.GetString("Service_Name");
                    servicefee = dataReader2.GetDecimal("Service_Fee");
                    visit = dataReader2.GetInt32("No_of_Visit");
                    hour = dataReader2.GetInt32("Hour_Consumed");
                    hour = hour * 60;
                    min = dataReader2.GetInt32("Minute_Consumed");
                    //numericUpDown3.Value = min + hour;
                }
                textBox1.Text = serviceno.ToString();
                textBox2.Text = servicename;
                textBox3.Text = servicefee.ToString();
                textBox7.Text = visit.ToString();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();
        }
        public void GetServiceNo()
        {
            int serviceno = 0;
            try
            {
                connection.Open();
                string query = "Select Service_No from servicetbl order by Service_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    serviceno = dataReader.GetInt32("Service_No");
                }
                serviceno = serviceno + 1;
                textBox6.Text = serviceno.ToString();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();
        }
        public void GetAllService()
        {
            dataGridView1.Rows.Clear();
            try
            {
                connection.Open();
                string query = "Select * from servicetbl where Service_Status = 'Active' order by Service_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView1.Rows.Add(dataReader.GetInt32("Service_No"), dataReader.GetString("Service_Name"));
                }
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();
        }
        public int GetServiceProduct()
        {
            int serviceprodno = 0;
            try
            {
                connection.Open();
                string query = "SELECT Service_ProductNo from service_producttbl order by Service_ProductNo";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    serviceprodno = dataReader.GetInt32("Service_ProductNo");
                }
                serviceprodno = serviceprodno + 1;
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            return serviceprodno;
        }
        private void button6_Click(object sender, EventArgs e)
        {
         
            dataGridView1.Enabled = false;
            button17.Enabled = false;
            button6.Enabled = false;
            button8.Enabled = false;
            button7.Enabled = false;
            addPanel.Visible = true;
            GetProducts();

            editPanel.SendToBack();
            addPanel.BringToFront();

            GetServiceNo();
            comboBox3.Items.Clear();
            try
            {
                connection.Open();
                string query = "SELECT * from producttbl where Product_Status = 'Available' order by Product_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    comboBox3.Items.Add(dataReader.GetString("Product_Name"));
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            comboBox3.SelectedIndex = 0;
            addTransition.ShowSync(addPanel);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string servicename, checkservice, products, machinename, producttype;
            bool exists = false, check = false;
            servicename = textBox2.Text.Trim();
            decimal fee = 0;
            int serviceno = 0, prodno = 0;
            serviceno = Convert.ToInt32(textBox1.Text);
            int visit = 0, reqno = 0, hour = 0, min = 0, minutes = 0;

            if (string.IsNullOrEmpty(numericUpDown3.Text))
            {
                numericUpDown3.Text = "0";
            }
            min = Convert.ToInt32(numericUpDown3.Text);
            hour = min / 60;
            minutes = min % 60;
            bool containsNum = Regex.IsMatch(servicename, @"[0-9~!@#$%^&*()_+=-]");
            if (dataGridView3.Rows.Count == 0)
            {
                label31.Text = "Please select product first";
                check = true;
            }
            else
            {
                label31.Text = "";
            }

            if (servicename.Length == 0)
            {
                label1.Text = "Service Name required";
                textBox2.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }
            else
            {
                try
                {
                    connection.Open();
                    string query = "Select Service_Name from servicetbl where Service_Name != '" + servicename + "'";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        checkservice = dataReader.GetString("Service_Name");
                        if (checkservice.Equals(servicename))
                        {
                            exists = true;
                            break;
                        }
                    }
                    connection.Close();
                }
                catch (MySqlException me)
                {
                    MessageBox.Show(me.Message);
                }

                if (exists)
                {
                    label1.Text = "Service name already exists";
                    textBox2.BackColor = Color.FromArgb(252, 224, 224);
                    check = true;
                }
                else
                {
                    if (containsNum)
                    {
                        label1.Text = "Service Name should not provide numbers and special characters";
                        textBox2.BackColor = Color.FromArgb(252, 224, 224);
                        check = true;
                    }
                    else
                    {
                        label1.Text = "";
                        textBox2.BackColor = Color.White;
                    }

                }
            }
            try
            {
                visit = Convert.ToInt32(textBox7.Text);
                label29.Text = "";
                textBox7.BackColor = Color.White;
            }
            catch (FormatException)
            {
                label29.Text = "Invalid no. of visits";
                textBox7.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }
            try
            {
                fee = Convert.ToDecimal(textBox3.Text);
                if (fee.ToString().Length == 0)
                {
                    label28.Text = "Required Service fee";
                    textBox3.BackColor = Color.FromArgb(252, 224, 224);
                    check = true;
                }
                else
                {
                    if (fee == 0 || fee < 500)
                    {
                        label28.Text = "Service fee should not be less than 500 or equal to 0";
                        textBox3.BackColor = Color.FromArgb(252, 224, 224);
                        check = true;
                    }
                    else
                    {
                        label28.Text = "";
                        textBox3.BackColor = Color.White;
                    }
                }
            }
            catch (FormatException)
            {
                label28.Text = "Invalid Service fee";
                textBox3.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }
            if (min == 0)
            {
                label26.Text = "Time consumed is required";
                numericUpDown3.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }
            else if (hour == 0 && min < 30)
            {
                label26.Text = "Time consumed should be 30 min";
                numericUpDown3.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }
            else
            {
                label26.Text = "";
                numericUpDown3.BackColor = Color.White;
            }

            if (check == false)
            {


                try
                {
                    connection.Open();
                    string query2 = "Update servicetbl set Service_Name = '" + servicename + "',Service_Fee = '" + fee + "',No_Of_Visit = '" + visit + "',Hour_Consumed = '" + hour + "', Minute_Consumed = '" + min + "' where Service_No = '" + serviceno + "'";
                    MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                    cmd2.ExecuteNonQuery();
                    connection.Close();
                }
                catch (MySqlException me)
                {
                    MessageBox.Show(me.Message);
                }

                try
                {
                    connection.Open();
                    string query4 = "delete from service_producttbl where Service_No = '" + serviceno + "'";
                    MySqlCommand cmd4 = new MySqlCommand(query4, connection);
                    cmd4.ExecuteNonQuery();
                    connection.Close();
                }
                catch (MySqlException me)
                {
                    MessageBox.Show(me.Message);
                }
                foreach (DataGridViewRow dgvr in dataGridView3.Rows)
                {
                    int serviceprodno = GetServiceProduct();
                    products = dgvr.Cells[0].Value.ToString();
                    producttype = dgvr.Cells[1].Value.ToString();
                    try
                    {
                        connection.Open();
                        string query = "Select Product_ProdType_No from product_prodtypetbl ppt, producttbl p,product_typetbl pt where p.Product_Name = '" + products + "' and pt.Product_Type = '" + producttype + "' and p.Product_No = ppt.Product_No and pt.Product_Type_No = ppt.Product_Type_No";
                        MySqlCommand cmd = new MySqlCommand(query, connection);
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        while (dataReader.Read())
                        {
                            prodno = dataReader.GetInt32("Product_ProdType_No");
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
                        string query1 = "Insert into service_producttbl values ('" + serviceprodno + "','" + serviceno + "','" + prodno + "')";
                        MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                        cmd1.ExecuteNonQuery();
                        connection.Close();
                    }
                    catch (MySqlException me)
                    {
                        MessageBox.Show(me.Message);
                    }
                }
                MessageBox.Show("Service updated successfully");
                GetAllService();
                GetFirstService();
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;
                textBox7.ReadOnly = true;
                numericUpDown3.Enabled = false;
                textBox2.BorderStyle = BorderStyle.None;
                textBox3.BorderStyle = BorderStyle.None;
                textBox7.BorderStyle = BorderStyle.None;
                numericUpDown3.BorderStyle = BorderStyle.None;
                comboBox4.Hide();
                button14.Hide();
                button13.Hide();
                button8.Visible = false;
            }
        }
        ErrorProvider errorProvider = new ErrorProvider();
        private void button11_Click(object sender, EventArgs e)
        {
            bool exists = false, check = false;
            string prod = "", prodtype = comboBox2.Text;

            prod = comboBox3.Text;
            if (prodtype == "No available")
            {
                label35.Text = "No available product type";
                check = true;
            }
            else
            {
                if (prod == "No available")
                {
                    label35.Text = "No available product";
                    check = true;
                }
                else
                {
                    label35.Text = "";
                }
            }

            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if (prod == dataGridView2.Rows[i].Cells[0].Value.ToString() && prodtype == dataGridView2.Rows[i].Cells[1].Value.ToString())
                {
                    label35.Text = "Product already exists in the datagridview!";
                    comboBox3.BackColor = Color.FromArgb(252, 224, 224);
                    exists = true;
                    break;
                }
                else
                {
                    label35.Text = "";
                    comboBox3.BackColor = Color.White;
                }
            }
            if (exists == false && check == false)
            {
                dataGridView2.Rows.Add(prod, prodtype);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            errorProvider.SetError(comboBox3, string.Empty);
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
            
            string servicename, checkservice, products, machinename, producttype;
            bool exists = false, check = false;
            servicename = textBox5.Text.Trim();
            decimal fee = 0;
            int serviceno = 0, prodno = 0;
            serviceno = Convert.ToInt32(textBox6.Text);
            int visit = 0;
            int hour = 0, min = 0, minutes = 0;

            if (string.IsNullOrEmpty(numericUpDown2.Text))
            {
                numericUpDown2.Text = "0";
            }

            min = Convert.ToInt32(numericUpDown2.Text);
            hour = min / 60;
            minutes = min % 60;
            if (dataGridView2.Rows.Count == 0)
            {
                label36.Text = "Please select product first";
                check = true;
            }
            else
            {
                label36.Text = "";
            }
            if (min == 0)
            {
                label37.Text = "Time consumed is required";
                numericUpDown2.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }
            else if (hour == 0 && min < 30)
            {
                label37.Text = "Time consumed should not be less than 30 mins";
                numericUpDown2.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }
            else
            {
                label37.Text = "";
                numericUpDown2.BackColor = Color.White;
            }
            //bool containsNum = Regex.IsMatch(servicename, @"[0-9~!@#$%^&*()_+=-]");
            try
            {
                visit = Convert.ToInt32(numericUpDown5.Text);
                label34.Text = "";
                numericUpDown5.BackColor = Color.White;
            }
            catch (FormatException)
            {
                label34.Text = "Invalid no. of visits";
                numericUpDown5.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }
            try
            {
                fee = Convert.ToDecimal(textBox4.Text);
                if (fee.ToString().Length == 0)
                {
                    label33.Text = "Required Service fee";
                    textBox4.BackColor = Color.FromArgb(252, 224, 224);
                    check = true;
                }
                else
                {
                    if (fee == 0 || fee < 500)
                    {
                        label33.Text = "Service fee should not be less than 500 and equal to 0";
                        textBox4.BackColor = Color.FromArgb(252, 224, 224);
                        check = true;
                    }
                    else
                    {
                        label33.Text = "";
                        textBox4.BackColor = Color.White;
                    }
                }
            }
            catch (FormatException)
            {
                label33.Text = "Invalid Service fee";
                textBox4.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }

            if (servicename.Length == 0)
            {
                label32.Text = "Service Name required";
                textBox5.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }
            else
            {
                try
                {
                    connection.Open();
                    string query = "Select Service_Name from servicetbl";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        checkservice = dataReader.GetString("Service_Name");
                        if (checkservice.Equals(servicename))
                        {
                            exists = true;
                            break;
                        }
                    }

                    connection.Close();
                }
                catch (MySqlException me)
                {
                    connection.Close();
                }

                if (exists)
                {
                    label32.Text = "Service name already exists";
                    textBox5.BackColor = Color.FromArgb(252, 224, 224);
                    check = true;
                }
                else
                {
                    //if (containsNum)
                    //{
                    //    label32.Text = "Service Name should not containt numbers and special characters";
                    //    textBox5.BackColor = Color.FromArgb(252, 224, 224);
                    //    check = true;
                    //}
                    //else
                    //{
                    //    label32.Text = "";
                    //    textBox5.BackColor = Color.White;
                    //}
                }
            }


            if (check == false)
            {
                try
                {
                    connection.Open();
                    string query2 = "Insert into servicetbl values ('" + serviceno + "','" + servicename + "','" + fee + "','" + visit + "','" + hour + "','" + min + "','Active')";
                    MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                    cmd2.ExecuteNonQuery();
                    connection.Close();
                }
                catch (MySqlException me)
                {
                    connection.Close();
                }



                foreach (DataGridViewRow dgvr in dataGridView2.Rows)
                {
                    int serviceprodno = GetServiceProduct();
                    products = dgvr.Cells[0].Value.ToString();
                    producttype = dgvr.Cells[1].Value.ToString();
                    try
                    {
                        connection.Open();
                        string query = "Select Product_ProdType_No from product_prodtypetbl ppt, producttbl p,product_typetbl pt where p.Product_Name = '" + products + "' and pt.Product_Type = '" + producttype + "' and p.Product_No = ppt.Product_No and pt.Product_Type_No = ppt.Product_Type_No";
                        MySqlCommand cmd = new MySqlCommand(query, connection);
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        while (dataReader.Read())
                        {
                            prodno = dataReader.GetInt32("Product_ProdType_No");
                        }
                        connection.Close();
                    }
                    catch (MySqlException me)
                    {
                        connection.Close();
                    }
                    try
                    {
                        connection.Open();
                        string query1 = "Insert into service_producttbl values ('" + serviceprodno + "','" + serviceno + "','" + prodno + "')";
                        MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                        cmd1.ExecuteNonQuery();
                        connection.Close();
                    }
                    catch (MySqlException me)
                    {
                        connection.Close();
                    }
                }
                MessageBox.Show("Service added successfully");
                dataGridView2.Rows.Clear();
                textBox4.Text = "";
                textBox5.Text = "";
                numericUpDown5.Value = 1;
                comboBox3.SelectedIndex = 0;
                GetServiceNo();
                GetAllService();

                dataGridView1.Enabled = true;
                button17.Enabled = true;
                button6.Enabled = true;
                button8.Enabled = true;
                button7.Enabled = true;
                addPanel.Hide();

            }

        }
        private void textBox5_Leave(object sender, EventArgs e)
        {
            string servicename, checkservice;
            bool exists = false;
            servicename = textBox5.Text.Trim();

            if (servicename.Length == 0)
            {
                label32.Text = "Service Name required";
                textBox5.BackColor = Color.FromArgb(252, 224, 224);
            }
            else
            {
                try
                {
                    connection.Open();
                    string query = "Select Service_Name from servicetbl where Service_Status ='Active'";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        checkservice = dataReader.GetString("Service_Name");
                        if (checkservice.Equals(servicename))
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

                if (exists)
                {
                    label32.Text = "Service name already exists";
                    textBox5.BackColor = Color.FromArgb(252, 224, 224);
                }
                else
                {
                    label32.Text = "";
                    textBox5.BackColor = Color.White;
                }
            }
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            decimal fee = 0;
            try
            {
                fee = Convert.ToDecimal(textBox4.Text);
                if (fee.ToString().Length == 0)
                {
                    label33.Text = "Required Service fee";
                    textBox4.BackColor = Color.FromArgb(252, 224, 224);

                }
                else
                {
                    label33.Text = "";
                    textBox4.BackColor = Color.White;
                }
            }
            catch (FormatException)
            {
                textBox4.Text = "";
            }
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            ClearError();
            editPanel.BringToFront();
            addPanel.SendToBack();
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox7.ReadOnly = true;
            numericUpDown3.Enabled = false;
            comboBox1.Visible = false;
            comboBox4.Visible = false;
            button14.Visible = false;
            button13.Visible = false;
            textBox2.BorderStyle = BorderStyle.None;
            textBox3.BorderStyle = BorderStyle.None;
            textBox7.BorderStyle = BorderStyle.None;
            numericUpDown3.BorderStyle = BorderStyle.None;

            string servicename = "", machinename = "", req = "";
            int serviceno = 0, rows = 0, visits = 0;
            int hour = 0, min = 0,totalmin = 0;
            decimal servicefee = 0;
            rows = dataGridView1.CurrentCell.RowIndex;
            serviceno = Convert.ToInt32(dataGridView1.Rows[rows].Cells[0].Value);
            dataGridView3.Rows.Clear();
            try
            {
                connection.Open();
                string query2 = "Select * from servicetbl s, service_producttbl sp, producttbl p, product_typetbl pt, product_prodtypetbl ppt where s.Service_No = '" + serviceno + "' and s.Service_No = sp.Service_No and sp.Product_ProdType_No = ppt.Product_ProdType_No and ppt.Product_No = p.Product_No and ppt.Product_Type_No = pt.Product_Type_No order by s.Service_No,p.Product_No";
                MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                MySqlDataReader dataReader2 = cmd2.ExecuteReader();
                while (dataReader2.Read())
                {
                    dataGridView3.Rows.Add(dataReader2.GetString("Product_Name"), dataReader2.GetString("Product_Type"));
                    serviceno = dataReader2.GetInt32("Service_No");
                    servicename = dataReader2.GetString("Service_Name");
                    servicefee = dataReader2.GetDecimal("Service_Fee");
                    visits = dataReader2.GetInt32("No_Of_Visit");
                    hour = dataReader2.GetInt32("Hour_Consumed");
                    hour = hour * 60;
                    min = dataReader2.GetInt32("Minute_Consumed");
                    totalmin = hour + min;
                    numericUpDown3.Value = Convert.ToDecimal(totalmin);
                }

                textBox1.Text = serviceno.ToString();
                textBox2.Text = servicename;
                textBox3.Text = servicefee.ToString();
                textBox7.Text = visits.ToString();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();

        }

        private void button14_Click(object sender, EventArgs e)
        {
            bool exists = false, check = false;
            string prod = "", prodtype = comboBox1.Text;

            prod = comboBox4.Text;
            if (prodtype == "No available")
            {
                check = true;
                label30.Text = "No available product type";
            }
            else
            {
                if (prod == "No available")
                {
                    check = true;
                    label30.Text = "No available product";
                }
                else
                {
                    label30.Text = "";
                }
            }
            for (int i = 0; i < dataGridView3.Rows.Count; i++)
            {
                if (prod == dataGridView3.Rows[i].Cells[0].Value.ToString() && prodtype == dataGridView3.Rows[i].Cells[1].Value.ToString())
                {
                    label30.Text = "Product already exists in the datagridview!";
                    comboBox4.BackColor = Color.FromArgb(252, 224, 224);
                    exists = true;
                    break;
                }
                else
                {
                    label30.Text = "";

                }
            }


            if (exists == false && check == false)
            {
                dataGridView3.Rows.Add(prod, prodtype);
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            errorProvider.SetError(comboBox4, string.Empty);
            try
            {
                dataGridView3.Rows.RemoveAt(dataGridView3.CurrentRow.Index);
            }
            catch (NullReferenceException ne)
            {
                MessageBox.Show("No selected row");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox2.ReadOnly = false;
            textBox3.ReadOnly = false;
            textBox7.ReadOnly = false;
            numericUpDown3.Enabled = true;
            textBox2.BorderStyle = BorderStyle.Fixed3D;
            textBox3.BorderStyle = BorderStyle.Fixed3D;
            textBox7.BorderStyle = BorderStyle.Fixed3D;
            numericUpDown3.BorderStyle = BorderStyle.Fixed3D;
            comboBox4.Show();
            comboBox1.Show();
            button14.Show();
            button13.Show();
            button8.Visible = true;
        }



        private void textBox24_TextChanged(object sender, EventArgs e)
        {
            string search = textBox24.Text.Trim();
            dataGridView1.Rows.Clear();
            addPanel.SendToBack();
            editPanel.BringToFront();
            try
            {
                connection.Open();
                string query = "Select * from servicetbl where Service_Name LIKE '%" + search + "%' and Service_Status = 'Active' order by Service_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView1.Rows.Add(dataReader.GetInt32("Service_No"), dataReader.GetString("Service_Name"));
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            int serveno = 0;
            int serviceno = 0, servicefee = 0;
            string servicename = "";
            try
            {
                serveno = Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString());
            }
            catch (Exception)
            {
                MessageBox.Show("No Result");
                textBox24.Text = "";
            }
            dataGridView3.Rows.Clear();
            try
            {
                connection.Open();
                string query2 = "Select * from servicetbl s, service_producttbl sp, producttbl p, product_typetbl pt, product_prodtypetbl ppt where s.Service_No = '" + serveno + "' and s.Service_No = sp.Service_No and sp.Product_ProdType_No = ppt.Product_ProdType_No and ppt.Product_No = p.Product_No and ppt.Product_Type_No = pt.Product_Type_No order by s.Service_No,p.Product_No";
                MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                MySqlDataReader dataReader2 = cmd2.ExecuteReader();
                while (dataReader2.Read())
                {
                    dataGridView3.Rows.Add(dataReader2.GetString("Product_Name"), dataReader2.GetString("Product_Type"));
                    serviceno = dataReader2.GetInt32("Service_No");
                    servicename = dataReader2.GetString("Service_Name");
                    servicefee = dataReader2.GetInt32("Service_Fee");
                }
                textBox1.Text = serviceno.ToString();
                textBox2.Text = servicename;
                textBox3.Text = servicefee.ToString();

                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
        }
        private void button17_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Do you really want to delete?", "Delete", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                int rows = dataGridView1.CurrentCell.RowIndex;
                int service_no = Convert.ToInt32(dataGridView1.Rows[rows].Cells[0].Value);

                try
                {
                    connection.Open();
                    string query = "UPDATE servicetbl set Service_Status = 'Deleted' where Service_No = '" + service_no + "'";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record Deleted");
                    connection.Close();
                    GetAllService();
                }
                catch (MySqlException me)
                {
                    MessageBox.Show(me.Message);
                }
            }

        }


        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown2_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(numericUpDown2.Text))
            {
                numericUpDown2.Text = "0";
            }
        }


        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_KeyUp(object sender, KeyEventArgs e)
        {
            string containsLetter = @"[A-Za-z~!@#$%^&*()_+=-]";
            string cno = textBox3.Text.Trim();
            if (Regex.IsMatch(cno, containsLetter))
            {
                textBox3.BackColor = Color.FromArgb(252, 224, 224);
                label28.Text = "Numeric only";
            }
            else
            {
                label28.Text = "";
                textBox3.BackColor = Color.White;
            }
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            string containsNum = @"[0-9~!@#$%^&*()_+=-]";
            string empname = textBox2.Text.Trim();
            if (Regex.IsMatch(textBox2.Text.Trim(), containsNum))
            {
                label1.Text = "No numeric character";
                textBox2.BackColor = Color.FromArgb(252, 224, 224);
            }
            else
            {
                label1.Text = "";
                textBox2.BackColor = Color.White;
            }
        }

        private void textBox7_KeyUp(object sender, KeyEventArgs e)
        {
            string containsLetter = @"[A-Za-z~!@#$%^&*()_+=-]";
            string cno = textBox7.Text.Trim();
            if (Regex.IsMatch(cno, containsLetter))
            {
                textBox7.BackColor = Color.FromArgb(252, 224, 224);
                label29.Text = "Numeric only";
            }
            else
            {
                label29.Text = "";
                textBox7.BackColor = Color.White;
            }
        }

        private void textBox5_KeyUp(object sender, KeyEventArgs e)
        {
            string containsNum = @"[0-9~!@#$%^&*()_+=-]";
            string empname = textBox5.Text.Trim();
            if (Regex.IsMatch(textBox5.Text.Trim(), containsNum))
            {
                label32.Text = "No numeric character";
                textBox5.BackColor = Color.FromArgb(252, 224, 224);
            }
            else
            {
                label32.Text = "";
                textBox5.BackColor = Color.White;
            }
        }

        private void textBox4_KeyUp(object sender, KeyEventArgs e)
        {
            string containsLetter = @"[A-Za-z~!@#$%^&*()_+=-]";
            string cno = textBox4.Text.Trim();
            if (Regex.IsMatch(cno, containsLetter))
            {
                textBox4.BackColor = Color.FromArgb(252, 224, 224);
                label33.Text = "Numeric only";
            }
            else
            {
                label33.Text = "";
                textBox4.BackColor = Color.White;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            string prodtype = comboBox2.Text;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from product_typetbl pt, producttbl p, product_prodtypetbl ppt where pt.Product_Type = '" + prodtype + "' and pt.Product_Type_No = ppt.Product_Type_No and ppt.Product_No = p.Product_No order by ppt.Product_Type_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    comboBox3.Items.Add(dataReader.GetString("Product_Name"));
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
                comboBox3.SelectedIndex = 0;
            }
            catch (Exception)
            {
                comboBox3.Items.Add("No available");
                comboBox3.SelectedIndex = 0;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox4.Items.Clear();
            string prodtype = comboBox1.Text;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from product_typetbl pt, producttbl p, product_prodtypetbl ppt where pt.Product_Type = '" + prodtype + "' and pt.Product_Type_No = ppt.Product_Type_No and ppt.Product_No = p.Product_No order by ppt.Product_Type_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    comboBox4.Items.Add(dataReader.GetString("Product_Name"));
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
                comboBox4.SelectedIndex = 0;
            }
            catch (Exception)
            {
                comboBox4.Items.Add("No available");
                comboBox4.SelectedIndex = 0;
            }
        }
        private void comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Enabled = true;
            addPanel.SendToBack();
            editPanel.BringToFront();
        }


        private void cancelBtn_Click(object sender, EventArgs e)
        {
            dataGridView1.Enabled = true;
            button17.Enabled = true;
            button6.Enabled = true;
            button8.Enabled = true;
            button7.Enabled = true;
            addPanel.Hide();

        }

        private void addPanel_Paint(object sender, PaintEventArgs e)
        {
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        
   
      
    }
}
