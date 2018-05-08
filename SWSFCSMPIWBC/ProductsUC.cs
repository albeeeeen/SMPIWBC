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
    public partial class ProductsUC : UserControl
    {
        static string connectionString =
       System.Configuration.ConfigurationManager.
       ConnectionStrings["SWSFCSMPIWBC.Properties.Settings.slimmersdbConnectionString"].ConnectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);
        public ProductsUC()
        {
            InitializeComponent();

            ClearError();
            label3.Visible = false;
            dataGridView3.Size = new Size(599, 344);
            dataGridView3.Location = new Point(66, 135);      
            button7.BringToFront();
            button8.SendToBack();
            textBox4.BorderStyle = BorderStyle.None;
            textBox13.BorderStyle = BorderStyle.None;
            textBox2.BorderStyle = BorderStyle.None;
            textBox2.Enabled = false;
            textBox7.Show();
            textBox13.ReadOnly = true;
            textBox24.Hide();
            button11.IdleFillColor = Color.FromArgb(4, 91, 188);
            button11.IdleForecolor = Color.White;

            button14.IdleFillColor = Color.White;
            button14.IdleLineColor = Color.FromArgb(4, 91, 188);
            button14.IdleForecolor = Color.FromArgb(4, 91, 188);

            textBox8.ReadOnly = true;
            richTextBox2.ReadOnly = true;
            textBox8.BorderStyle = BorderStyle.None;
            richTextBox2.BorderStyle = BorderStyle.None;
            button22.Enabled = false;
            typePanel.Show();
            productPanel.Hide();
            dataGridView4.Rows.Clear();
            GetAllProdType();
            FirstProductType();

        }
        public void ClearError()
        {
            label29.Text = "";
            textBox2.BackColor = Color.White;
            label30.Text = "";
            comboBox1.BackColor = Color.White;
            label31.Text = "";
            textBox4.BackColor = Color.White;
            label33.Text = "";
            textBox5.BackColor = Color.White;
            label34.Text = "";
            comboBox2.BackColor = Color.White;
            label35.Text = "";
            textBox3.BackColor = Color.White;
            label25.Text = "";
            textBox8.BackColor = Color.White;
            label26.Text = "";
            richTextBox2.BackColor = Color.White;
            label27.Text = "";
            textBox10.BackColor = Color.White;
            label28.Text = "";
            richTextBox1.BackColor = Color.White;
            label32.Text = "";
            label36.Text = "";
        }
        public void GetProductNo()
        {
            int prodno = 0;
            try
            {
                connection.Open();
                string query1 = "Select Product_No from producttbl order by Product_No";
                MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                while (dataReader1.Read())
                {
                    prodno = dataReader1.GetInt32("Product_No");
                }
                prodno = prodno + 1;
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();

            textBox6.Text = prodno.ToString();
        }
        public void GetAllProduct()
        {
            dataGridView1.Rows.Clear();
            try
            {
                connection.Open();
                string query4 = "Select * from producttbl where Product_Status = 'Available' order by Product_No";
                MySqlCommand cmd4 = new MySqlCommand(query4, connection);
                MySqlDataReader dataReader4 = cmd4.ExecuteReader();
                while (dataReader4.Read())
                {
                    dataGridView1.Rows.Add(dataReader4.GetInt32("Product_No"), dataReader4.GetString("Product_Name"));
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
        }
        public void GetProductType()
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
                    comboBox2.Items.Add(dataReader.GetString("Product_Type"));
                    comboBox1.Items.Add(dataReader.GetString("Product_Type"));
                }
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();
            try
            {
                comboBox2.SelectedIndex = 0;
            }
            catch (Exception)
            {
                comboBox2.Text = "No Available";
            }
            try
            {
                comboBox1.SelectedIndex = 0;
            }
            catch (Exception)
            {
                comboBox1.Text = "No Available";
            }
        }
        public void GetFirstProductType()
        {
            dataGridView3.Rows.Clear();
            try
            {
                connection.Open();
                string query5 = "Select * from producttbl p ,product_typetbl pt, product_prodtypetbl ppt where p.Product_No = (SELECT Product_No from producttbl where Product_Status = 'Available' order by Product_No LIMIT 1) and p.Product_No = ppt.Product_No and pt.Product_Type_No = ppt.Product_Type_No";
                MySqlCommand cmd5 = new MySqlCommand(query5, connection);
                MySqlDataReader dataReader5 = cmd5.ExecuteReader();
                while (dataReader5.Read())
                {
                    textBox2.Text = dataReader5.GetString("Product_Name");
                    textBox1.Text = dataReader5.GetInt32("Product_No").ToString();
                    dataGridView3.Rows.Add(dataReader5.GetString("Product_Type"), dataReader5.GetDecimal("Product_Fee"),dataReader5.GetInt32("Critical_Level"));
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
            addPanel.Visible = false;
            button8.Enabled = false;
            button1.Enabled = false;
            dataGridView1.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            button6.Enabled = false;
            button14.Enabled = false;
            button11.Enabled = false;

            editPanel.SendToBack();
            addPanel.BringToFront();
            GetProductNo();
            GetProductType();
            addPanelTransition.ShowSync(addPanel);
        }
        ErrorProvider errorProvider = new ErrorProvider();
        private void textBox5_Leave(object sender, EventArgs e)
        {
            string prodname, checkprod;
            bool exists = false;
            prodname = textBox5.Text.Trim();

            if (prodname.Length == 0)
            {
                errorProvider.SetError(textBox5, "Product Name required");
            }
            else
            {
                try
                {
                    connection.Open();
                    string query = "Select Product_Name from producttbl";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        checkprod = dataReader.GetString("Product_Name");
                        if (checkprod.Equals(prodname))
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
                    errorProvider.SetError(textBox5, "Product name already exists");
                }
                else
                {
                    errorProvider.SetError(textBox5, string.Empty);
                }
            }
        }
        public int GetInventoryNo()
        {
            int inventno = 0;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from product_inventorytbl order by Inventory_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    inventno = dataReader.GetInt32("Inventory_No");
                }
                inventno = inventno + 1;
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            return inventno;
        }
        private void button10_Click(object sender, EventArgs e)
        {
            string prodname, prodtype, checkprod;
            int prodno = 0, prodtypeno = 0;
            bool check = false, exists = false;
            prodname = textBox5.Text.Trim();
            prodno = Convert.ToInt32(textBox6.Text);
            string containsNum = @"[0-9~!@#$%^&*()_+=-]";

            if (prodname.Length == 0)
            {
                label33.Text = "Product Name required";
                textBox5.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }
            else
            {
                if (Regex.IsMatch(prodname, containsNum))
                {
                    label33.Text = "Product name is invalid";
                    textBox5.BackColor = Color.FromArgb(252, 224, 224);
                    check = true;
                }
                else
                {
                    try
                    {
                        connection.Open();
                        string query = "Select Product_Name from producttbl";
                        MySqlCommand cmd = new MySqlCommand(query, connection);
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        while (dataReader.Read())
                        {
                            checkprod = dataReader.GetString("Product_Name");
                            if (checkprod.Equals(prodname))
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
                        label33.Text = "Product name already exists";
                        textBox5.BackColor = Color.FromArgb(252, 224, 224);
                        check = true;
                    }
                    else
                    {
                        label33.Text = "";
                        textBox5.BackColor = Color.White;
                    }
                }
                if (dataGridView2.Rows.Count == 0)
                {
                    label36.Text = "Please select first product type and fee";
                    check = true;
                }
                if (check == false)
                {
                    
                    label36.Text = "";
                    try
                    {
                        connection.Open();
                        string query2 = "INSERT into producttbl values ('" + prodno + "','" + prodname + "','Available')";
                        MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                        cmd2.ExecuteNonQuery();
                        connection.Close();
                        for (int j = 0; j < dataGridView2.Rows.Count; j++)
                        {
                            int inventno = GetInventoryNo();
                            int critlvl = Convert.ToInt32(dataGridView2.Rows[j].Cells[2].Value);
                            int pptype = 0;
                            decimal fee = Convert.ToDecimal(dataGridView2.Rows[j].Cells[1].Value);
                            connection.Open();
                            string query3 = "SELECT Product_ProdType_No from product_prodtypetbl order by Product_ProdType_No";
                            MySqlCommand cmd3 = new MySqlCommand(query3, connection);
                            MySqlDataReader dataReader3 = cmd3.ExecuteReader();
                            while (dataReader3.Read())
                            {
                                pptype = dataReader3.GetInt32("Product_ProdType_No");
                            }
                            pptype = pptype + 1;
                            connection.Close();
                            int ptypeno = 0;
                            string ptypename = dataGridView2.Rows[j].Cells[0].Value.ToString();
                            connection.Open();
                            MySqlCommand cmd5 = new MySqlCommand("SELECT * from product_typetbl where Product_Type = '" + ptypename + "'", connection);
                            MySqlDataReader dataReader5 = cmd5.ExecuteReader();
                            while (dataReader5.Read())
                            {
                                ptypeno = dataReader5.GetInt32("Product_Type_No");
                            }
                            connection.Close();
                            connection.Open();
                            string query4 = "INSERT into product_prodtypetbl values ('" + pptype + "','" + prodno + "','" + ptypeno + "','" + fee + "','"+critlvl+"')";
                            MySqlCommand cmd4 = new MySqlCommand(query4, connection);
                            cmd4.ExecuteNonQuery();
                            connection.Close();

                            connection.Open();
                            MySqlCommand cmd6 = new MySqlCommand("INSERT INTO product_inventorytbl values ('"+inventno+"','"+pptype+"', '0')", connection);
                            cmd6.ExecuteNonQuery();

                            connection.Close();
                        }
                        MessageBox.Show("Product successfully added!");
                        dataGridView2.Rows.Clear();
                        textBox5.Text = "";
                        GetAllProduct();
                        comboBox2.SelectedIndex = 0;
                        textBox3.Text = "";
                        addPanel.Visible = false;
                        button8.Enabled = true;
                        button1.Enabled = true;
                        dataGridView1.Enabled = true;
                        button7.Enabled = true;
                        button8.Enabled = true;
                        button6.Enabled = true;
                        button14.Enabled = true;
                        button11.Enabled = true;
                        GetProductNo();
                    }
                    catch (MySqlException me)
                    {
                        MessageBox.Show(me.Message);
                    }
                }
            }
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            ClearError();
            dataGridView3.Size = new Size(599, 344);
            dataGridView3.Location = new Point(66, 135);
            button7.BringToFront();
            button8.SendToBack();
            int prodno = 0, rows = 0;
            button17.Visible = false;
            button16.Visible = false;
            textBox2.ReadOnly = true;
            comboBox1.Enabled = false;
            textBox4.Enabled = false;
            button8.Enabled = false;
            textBox2.BorderStyle = BorderStyle.FixedSingle;
            rows = dataGridView1.CurrentCell.RowIndex;
            prodno = Convert.ToInt32(dataGridView1.Rows[rows].Cells[0].Value);
            addPanel.SendToBack();
            editPanel.BringToFront();
            dataGridView3.Rows.Clear();
            try
            {
                connection.Open();
                string query6 = "Select * from producttbl p ,product_typetbl pt, product_prodtypetbl ppt where p.Product_No = '" + prodno + "' and p.Product_No = ppt.Product_No and pt.Product_Type_No = ppt.Product_Type_No and Product_Status = 'Available'";
                MySqlCommand cmd6 = new MySqlCommand(query6, connection);
                MySqlDataReader dataReader6 = cmd6.ExecuteReader();
                while (dataReader6.Read())
                {
                    textBox1.Text = dataReader6.GetInt32("Product_No").ToString();
                    textBox2.Text = dataReader6.GetString("Product_Name");
                    dataGridView3.Rows.Add(dataReader6.GetString("Product_Type"), dataReader6.GetDecimal("Product_Fee"),dataReader6.GetInt32("Critical_Level"));
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
            label3.Visible = true;
            dataGridView3.Location = new Point(66, 286);
            dataGridView3.Size = new Size(599, 193);
            textBox2.ReadOnly = false;
            textBox2.Enabled = true;
            textBox2.BorderStyle = BorderStyle.FixedSingle;
            textBox4.BorderStyle = BorderStyle.FixedSingle;
            textBox13.BorderStyle = BorderStyle.FixedSingle;
            textBox13.ReadOnly = false;
            textBox4.Enabled = true;
            comboBox1.Enabled = true;
            button16.Visible = true;
            button17.Visible = true;
            dataGridView3.Enabled = true;
            GetProductType();
            button8.Visible = true;
            button8.Enabled = true;
            button7.Visible = false;
            button8.BringToFront();
            button7.SendToBack();
            dataGridView3.Columns[2].ReadOnly = false;
            textBox13.Enabled = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            label3.Visible = false;
            dataGridView3.Size = new Size(599, 344);      
            dataGridView3.Location = new Point(66, 135);      
            textBox2.BorderStyle = BorderStyle.None;
            string prodname, prodtype, checkprod;
            int prodno = 0, prodtypeno = 0;
            bool check = false, exists = false;
            prodname = textBox2.Text.Trim();
            prodno = Convert.ToInt32(textBox1.Text);
            prodtype = comboBox1.Text;

            try
            {
                connection.Open();
                string query3 = "Select Product_Type_No from product_typetbl where Product_Type = '" + prodtype + "'";
                MySqlCommand cmd3 = new MySqlCommand(query3, connection);
                MySqlDataReader dataReader3 = cmd3.ExecuteReader();
                while (dataReader3.Read())
                {
                    prodtypeno = dataReader3.GetInt32("Product_Type_No");
                }
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();
            if (prodname.Length == 0)
            {
                label29.Text = "Product Name required";
                textBox2.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }
            else
            {
                try
                {
                    connection.Open();
                    string query = "Select Product_Name from producttbl where Product_No != '" + prodno + "'";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        checkprod = dataReader.GetString("Product_Name");
                        if (checkprod.Equals(prodname))
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
                    label29.Text = "Product name already exists";
                    textBox2.BackColor = Color.FromArgb(252, 224, 224);
                    check = true;
                }
                else
                {
                    label29.Text = "";
                    textBox2.BackColor = Color.White;
                }
            }
            if (dataGridView3.Rows.Count == 0)
            {
                label32.Text = "Please select product type and fee";
                check = true;
            }
            if (check == false)
            {
                label32.Text = "";
                try
                {
                    connection.Open();
                    string query2 = "UPDATE producttbl set Product_Name = '" + prodname + "' where Product_No = '" + prodno + "'";
                    MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                    cmd2.ExecuteNonQuery();
                    connection.Close();

                    connection.Open();
                    string query3 = "DELETE from product_prodtypetbl where Product_No = '" + prodno + "'";
                    MySqlCommand cmd3 = new MySqlCommand(query3, connection);
                    cmd3.ExecuteNonQuery();

                    connection.Close();
                    for (int j = 0; j < dataGridView3.Rows.Count; j++)
                    {
                        int critlvl = Convert.ToInt32(dataGridView3.Rows[j].Cells[2].Value);
                        decimal fee = Convert.ToDecimal(dataGridView3.Rows[j].Cells[1].Value);
                        int pptype = 1;
                        connection.Open();
                        string query4 = "SELECT Product_ProdType_No from product_prodtypetbl order by Product_ProdType_No";
                        MySqlCommand cmd4 = new MySqlCommand(query4, connection);
                        MySqlDataReader dataReader4 = cmd4.ExecuteReader();
                        while (dataReader4.Read())
                        {
                            pptype = dataReader4.GetInt32("Product_ProdType_No");
                        }
                        pptype = pptype + 1;
                        connection.Close();
                        int ptypeno = 0;
                        string ptypename = dataGridView3.Rows[j].Cells[0].Value.ToString();
                        connection.Open();
                        MySqlCommand cmd6 = new MySqlCommand("SELECT * from product_typetbl where Product_Type = '" + ptypename + "'", connection);
                        MySqlDataReader dataReader6 = cmd6.ExecuteReader();
                        while (dataReader6.Read())
                        {
                            ptypeno = dataReader6.GetInt32("Product_Type_No");
                        }
                        connection.Close();
                        connection.Open();
                        string query5 = "INSERT into product_prodtypetbl values ('" + pptype + "','" + prodno + "','" + ptypeno + "','" + fee + "','"+critlvl+"')";
                        MySqlCommand cmd5 = new MySqlCommand(query5, connection);
                        cmd5.ExecuteNonQuery();
                        connection.Close();
                    }
                    MessageBox.Show("Product successfully updated!");
                    GetAllProduct();
                    GetFirstProductType();
                    button17.Visible = false;
                    button16.Visible = false;
                    textBox2.ReadOnly = true;
                    comboBox1.Enabled = false;
                    textBox4.Enabled = false;
                    button8.Visible = false;
                    textBox2.BorderStyle = BorderStyle.None;
                    button7.Visible = true;
                    dataGridView3.Columns[2].ReadOnly = true;
                }
                catch (MySqlException me)
                {
                    MessageBox.Show(me.Message);
                }
            }
        }

        private void textBox24_TextChanged(object sender, EventArgs e)
        {
            string search = textBox24.Text.Trim();
            dataGridView1.Rows.Clear();
            dataGridView3.Rows.Clear();
            try
            {
                connection.Open();
                string query4 = "Select * from producttbl where Product_Name LIKE '%" + search + "%' and Product_Status = 'Available' order by Product_No";
                MySqlCommand cmd4 = new MySqlCommand(query4, connection);
                MySqlDataReader dataReader4 = cmd4.ExecuteReader();
                while (dataReader4.Read())
                {
                    dataGridView1.Rows.Add(dataReader4.GetInt32("Product_No"), dataReader4.GetString("Product_Name"));
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            int prodno = 0;
            try
            {
                prodno = Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString());
            }
            catch (Exception)
            {
                MessageBox.Show("No Result!");
                textBox24.Text = "";
            }
            try
            {
                connection.Open();
                string query5 = "Select * from producttbl p ,product_typetbl pt,product_prodtypetbl ppt where p.Product_No = '" + prodno + "' and p.Product_No = ppt.Product_No and pt.Product_Type_No = ppt.Product_Type_No order by p.Product_No";
                MySqlCommand cmd5 = new MySqlCommand(query5, connection);
                MySqlDataReader dataReader5 = cmd5.ExecuteReader();
                while (dataReader5.Read())
                {
                    textBox2.Text = dataReader5.GetString("Product_Name");
                    textBox1.Text = dataReader5.GetInt32("Product_No").ToString();
                    dataGridView3.Rows.Add(dataReader5.GetString("Product_Type"), dataReader5.GetDecimal("Product_Fee"),dataReader5.GetInt32("Critical_Level"));

                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }

        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            int prod_no = 0;
            int rows = 0;
            rows = dataGridView1.CurrentCell.RowIndex;
            prod_no = Convert.ToInt32(dataGridView1.Rows[rows].Cells[0].Value);
            DialogResult dr = MessageBox.Show("Do you really want to delete?", "Delete", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE producttbl set Product_Status = 'Deleted' where Product_No = '" + prod_no + "'";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record deleted!");
                    connection.Close();
                    GetAllProduct();
                }
                catch (MySqlException me)
                {
                    MessageBox.Show(me.Message);
                }
            }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            decimal fee = 0;
            try
            {
                fee = Convert.ToDecimal(textBox3.Text);
                if (fee.ToString().Length == 0)
                {
                    errorProvider.SetError(textBox3, "Required Product fee");

                }
                else
                {
                    errorProvider.SetError(textBox3, string.Empty);
                }
            }
            catch (FormatException)
            {
                errorProvider.SetError(textBox3, "Invalid Product fee");
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            string ptype = "";
            bool check = false;
            decimal fee = 0;
            int critlvl = 0;
            try
            {
                critlvl = Convert.ToInt32(textBox12.Text.Trim());
                if (critlvl.ToString().Length == 0)
                {
                    label2.Text = "Required Critical level";
                    textBox12.BackColor = Color.FromArgb(252, 224, 224);
                    check = true;
                }
                else
                {
                    if (critlvl == 0)
                    {
                        label2.Text = "Critical level should not be 0";
                        textBox12.BackColor = Color.FromArgb(252, 224, 224);
                        check = true;
                    }
                    else
                    {
                        label2.Text = "";
                        textBox12.BackColor = Color.White;
                    }
                }
            }
            catch (FormatException)
            {
                label2.Text = "Invalid critical level";
                textBox12.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }
            try
            {
                fee = Convert.ToDecimal(textBox3.Text);
                if (fee.ToString().Length == 0)
                {
                    label35.Text = "Required product fee";
                    textBox3.BackColor = Color.FromArgb(252, 224, 224);
                    check = true;
                }
                else
                {
                    if (fee == 0)
                    {
                        label35.Text = "Product fee should not be 0";
                        textBox3.BackColor = Color.FromArgb(252, 224, 224);
                        check = true;
                    }
                    else
                    {
                        label35.Text = "";
                        textBox3.BackColor = Color.White;
                    }
                }
            }
            catch (FormatException)
            {
                label35.Text = "Invalid Service fee";
                textBox3.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }
            try
            {
                ptype = comboBox2.Text;
                label34.Text = "";
                comboBox2.BackColor = Color.White;
            }
            catch (Exception)
            {
                label34.Text = "Please select product type";
                comboBox2.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }
            bool exists = false;
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if (ptype == dataGridView2.Rows[i].Cells[0].Value.ToString())
                {
                    label34.Text = "Product Type already exists in the datagridview!";
                    comboBox2.BackColor = Color.FromArgb(252, 224, 224);
                    exists = true;
                    break;
                }
                else
                {
                    label34.Text = "";
                    comboBox2.BackColor = Color.White;
                }
            }
            if (exists == false && check == false)
            {
                dataGridView2.Rows.Add(ptype, fee,critlvl);
            }
        }

        private void button12_Click(object sender, EventArgs e)
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
        private void button17_Click(object sender, EventArgs e)
        {
            string ptype = "";
            bool exists = false, check = false;
            decimal fee = 0;
            int critlvl = 0;
            try
            {
                critlvl = Convert.ToInt32(textBox13.Text.Trim());
                if (critlvl.ToString().Length == 0)
                {
                    label4.Text = "Required critical level";
                    textBox13.BackColor = Color.FromArgb(252, 224, 224);
                    check = true;
                }
                else
                {
                    if (critlvl <= 0)
                    {
                        label4.Text = "Critical level should not be 0";
                        textBox13.BackColor = Color.FromArgb(252, 224, 224);
                        check = true;
                    }
                    else
                    {
                        label4.Text = "";
                        textBox13.BackColor = Color.White;
                    }
                }
            }
            catch (FormatException)
            {
                label4.Text = "Invalid critical level";
                textBox13.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }
            try
            {
                fee = Convert.ToDecimal(textBox4.Text);
                if (fee.ToString().Length == 0)
                {
                    label31.Text = "Required Product fee";
                    textBox4.BackColor = Color.FromArgb(252, 224, 224);
                    check = true;
                }
                else
                {
                    if (fee <= 0)
                    {
                        label31.Text = "Product fee should not be 0";
                        textBox4.BackColor = Color.FromArgb(252, 224, 224);
                        check = true;
                    }
                    else
                    {
                        label31.Text = "";
                        textBox4.BackColor = Color.White;
                    }
                }
            }
            catch (FormatException)
            {
                label31.Text = "Invalid Service fee";
                textBox4.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }
            try
            {
                ptype = comboBox1.Text;
                label31.Text = "";
                comboBox1.BackColor = Color.White;
            }
            catch (Exception)
            {
                label31.Text = "Please select product type";
                comboBox1.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }
            for (int i = 0; i < dataGridView3.Rows.Count; i++)
            {
                if (ptype == dataGridView3.Rows[i].Cells[0].Value.ToString())
                {
                    label31.Text = "Product Type already exists in the datagridview!";
                    comboBox1.BackColor = Color.FromArgb(252, 224, 224);
                    exists = true;
                    break;
                }
                else
                {
                    label31.Text = "";
                    comboBox1.BackColor = Color.White;
                }
            }
            if (exists == false && check == false)
            {
                dataGridView3.Rows.Add(ptype, fee,critlvl);
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            errorProvider.SetError(comboBox1, string.Empty);
            try
            {
                dataGridView3.Rows.RemoveAt(dataGridView3.CurrentRow.Index);
            }
            catch (NullReferenceException ne)
            {
                MessageBox.Show("No selected row");
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            ClearError();
            textBox7.Show();
            textBox24.Hide();
            button11.IdleFillColor = Color.FromArgb(4, 91, 188); 
            button11.IdleForecolor = Color.White;

            button14.IdleFillColor = Color.White;
            button14.IdleLineColor = Color.FromArgb(4, 91, 188);
            button14.IdleForecolor = Color.FromArgb(4, 91, 188);

            textBox8.ReadOnly = true;
            richTextBox2.ReadOnly = true;
            textBox8.BorderStyle = BorderStyle.None;
            richTextBox2.BorderStyle = BorderStyle.None;
            button22.Enabled = false;
            typePanel.Show();
            productPanel.Hide();
            dataGridView4.Rows.Clear();
            GetAllProdType();
            FirstProductType();
            //button18.BackColor = Color.Transparent;
            //button19.BackColor = Color.Silver;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            textBox7.Hide();
            textBox24.Show();
            button14.IdleFillColor = Color.FromArgb(4, 91, 188);
            button14.IdleForecolor = Color.White;

            button11.IdleFillColor = Color.White;
            button11.IdleLineColor = Color.FromArgb(4, 91, 188);
            button11.IdleForecolor = Color.FromArgb(4, 91, 188);
            ClearError();
            textBox2.ReadOnly = true;
            textBox2.BorderStyle = BorderStyle.None;
            textBox4.Enabled = false;
            comboBox1.Enabled = false;
            button16.Visible = false;
            button17.Visible = false;
            dataGridView3.Enabled = false;
            GetProductType();
            button8.Enabled = false;
            typePanel.Hide();
            productPanel.Show();
            GetAllProduct();
            GetFirstProductType();
            //button18.BackColor = Color.Silver;
            //button19.BackColor = Color.Transparent;
        }
        public void GetProdTypeNo()
        {
            int ptypeno = 0;

            try
            {
                connection.Open();
                string query = "SELECT Product_Type_No from product_typetbl order by Product_Type_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    ptypeno = dataReader.GetInt32("Product_Type_No");
                }
                ptypeno = ptypeno + 1;
                textBox11.Text = ptypeno.ToString();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();
        }
        public void GetAllProdType()
        {
            dataGridView4.Rows.Clear();
            try
            {
                connection.Open();
                string query3 = "SELECT * from product_typetbl order by Product_Type_No";
                MySqlCommand cmd3 = new MySqlCommand(query3, connection);
                MySqlDataReader dataReader3 = cmd3.ExecuteReader();
                while (dataReader3.Read())
                {
                    dataGridView4.Rows.Add(dataReader3.GetInt32("Product_Type_No"), dataReader3.GetString("Product_Type"), dataReader3.GetString("Product_Type_Description"));
                }
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();

        }
        public void FirstProductType()
        {
            try
            {
                connection.Open();
                string query4 = "select * from product_typetbl order by Product_Type_No LIMIT 1";
                MySqlCommand cmd4 = new MySqlCommand(query4, connection);
                MySqlDataReader dataReader4 = cmd4.ExecuteReader();
                while (dataReader4.Read())
                {
                    textBox8.Text = dataReader4.GetString("Product_Type");
                    textBox9.Text = dataReader4.GetInt32("Product_Type_No").ToString();
                    richTextBox2.Text = dataReader4.GetString("Product_Type_Description");
                }
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();
        }
        private void button20_Click(object sender, EventArgs e)
        {
            dataGridView4.Enabled = false;
            button20.Enabled = false;
            button21.Enabled = false;
            button22.Enabled = false;
            button14.Enabled = false;
            button11.Enabled = false;
            typeAdd.Visible = false;


            GetProdTypeNo();
            typeAdd.BringToFront();
            typeEdit.SendToBack();
            addPanelTransition.ShowSync(typeAdd);
        }

        private void dataGridView4_Click(object sender, EventArgs e)
        {
            ClearError();
            typeEdit.BringToFront();
            typeAdd.SendToBack();
            int rows = 0, ptypeno = 0;
            textBox8.BorderStyle = BorderStyle.None;
            richTextBox2.BorderStyle = BorderStyle.None;
            textBox8.ReadOnly = true;
            richTextBox2.ReadOnly = true;
            button22.Enabled = false;
            rows = dataGridView4.CurrentCell.RowIndex;
            ptypeno = Convert.ToInt32(dataGridView4.Rows[rows].Cells[0].Value);

            try
            {
                connection.Open();
                string query4 = "select * from product_typetbl where Product_Type_No = '" + ptypeno + "' order by Product_Type_No LIMIT 1";
                MySqlCommand cmd4 = new MySqlCommand(query4, connection);
                MySqlDataReader dataReader4 = cmd4.ExecuteReader();
                while (dataReader4.Read())
                {
                    textBox8.Text = dataReader4.GetString("Product_Type");
                    textBox9.Text = dataReader4.GetInt32("Product_Type_No").ToString();
                    richTextBox2.Text = dataReader4.GetString("Product_Type_Description");
                }
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            string search = textBox7.Text.Trim();
            dataGridView4.Rows.Clear();
            try
            {
                connection.Open();
                string query3 = "SELECT * from product_typetbl where Product_Type LIKE '%" + search + "%' order by Product_Type_No";
                MySqlCommand cmd3 = new MySqlCommand(query3, connection);
                MySqlDataReader dataReader3 = cmd3.ExecuteReader();
                while (dataReader3.Read())
                {
                    dataGridView4.Rows.Add(dataReader3.GetInt32("Product_Type_No"), dataReader3.GetString("Product_Type"), dataReader3.GetString("Product_Type_Description"));
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            int ptypeno = 0;
            try
            {
                ptypeno = Convert.ToInt32(dataGridView4.Rows[dataGridView4.CurrentCell.RowIndex].Cells[0].Value.ToString());
            }
            catch (Exception)
            {
                MessageBox.Show("No Result");
                textBox7.Text = "";
            }
            try
            {
                connection.Open();
                string query4 = "select * from product_typetbl where Product_Type_No = '" + ptypeno + "' order by Product_Type_No LIMIT 1";
                MySqlCommand cmd4 = new MySqlCommand(query4, connection);
                MySqlDataReader dataReader4 = cmd4.ExecuteReader();
                while (dataReader4.Read())
                {
                    textBox8.Text = dataReader4.GetString("Product_Type");
                    textBox9.Text = dataReader4.GetInt32("Product_Type_No").ToString();
                    richTextBox2.Text = dataReader4.GetString("Product_Type_Description");
                }
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            button22.Visible = true;
            textBox8.ReadOnly = false;
            richTextBox2.ReadOnly = false;
            textBox8.BorderStyle = BorderStyle.FixedSingle;
            richTextBox2.BorderStyle = BorderStyle.FixedSingle;
            button22.Enabled = true;
        }

        private void button22_Click(object sender, EventArgs e)
        {
            button22.Visible = false;
            string checkptype;
            bool exists = false, checker = false;
            string ptype = textBox8.Text.Trim();
            int ptypeno = Convert.ToInt32(textBox9.Text.Trim());
            string pdesc = richTextBox2.Text.Trim();
            if (ptype.Length == 0)
            {
                label25.Text = "Product Type is required";
                textBox8.BackColor = Color.FromArgb(252, 224, 224);
                checker = true;
            }
            else
            {
                try
                {
                    connection.Open();
                    string query1 = "Select Product_Type from product_typetbl where Product_Type_No != '" + ptypeno + "'";
                    MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                    MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                    while (dataReader1.Read())
                    {
                        checkptype = dataReader1.GetString("Product_Type");
                        if (checkptype.Equals(ptype))
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
                    label25.Text = "Product Type already exists";
                    textBox8.BackColor = Color.FromArgb(252, 224, 224);
                    checker = true;
                }
                else
                {
                    label25.Text = "";
                    textBox8.BackColor = Color.White;
                }
                if (pdesc.Length == 0)
                {
                    label26.Text = "Product description is required";
                    richTextBox2.BackColor = Color.FromArgb(252, 224, 224);
                    checker = true;
                }
                else
                {
                    label26.Text = "";
                    richTextBox2.BackColor = Color.White;
                }
                if (checker == false)
                {
                    try
                    {
                        connection.Open();
                        string query2 = "UPDATE product_typetbl set Product_Type = '" + ptype + "', Product_Type_Description = '" + pdesc + "' where Product_Type_No = '" + ptypeno + "'";
                        MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                        cmd2.ExecuteNonQuery();

                        MessageBox.Show("Successfully updated product type");
                        connection.Close();
                        textBox8.ReadOnly = true;
                        richTextBox2.ReadOnly = true;
                        button22.Enabled = false;
                        textBox8.BorderStyle = BorderStyle.None;
                        richTextBox2.BorderStyle = BorderStyle.None;
                        dataGridView4.Rows.Clear();
                        GetAllProdType();
                    }
                    catch (MySqlException me)
                    {
                        MessageBox.Show(me.Message);
                    }

                }
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            typeAdd.Visible = false;
            dataGridView4.Enabled = true;
            typeEdit.Enabled = true;
            button11.Enabled = true;
            button14.Enabled = true;
            button20.Enabled = true;
            button21.Enabled = true;
            
            bool check = false, exists = false;
            string ptype, pdesc, checkptype;
            int ptypeno = 0;
            ptype = textBox10.Text.Trim();
            pdesc = richTextBox1.Text.Trim();
            ptypeno = Convert.ToInt32(textBox11.Text);
            if (ptype.Length == 0)
            {
                label27.Text = "Product type is required.";
                textBox10.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }
            else
            {
                try
                {
                    connection.Open();
                    string query1 = "Select Product_Type from product_typetbl where Product_Type_No != '" + ptypeno + "'";
                    MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                    MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                    while (dataReader1.Read())
                    {
                        checkptype = dataReader1.GetString("Product_Type");
                        if (checkptype.Equals(ptype))
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
            if (exists)
            {
                label27.Text = "Product Type already exists";
                textBox10.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }
            else
            {
                label27.Text = "";
                textBox10.BackColor = Color.White;
            }
            if (pdesc.Length == 0)
            {
                label28.Text = "Product description is required.";
                richTextBox1.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }
            else
            {
                label28.Text = "";
                richTextBox1.BackColor = Color.White;
            }
            if (check == false)
            {
                try
                {
                    connection.Open();
                    string query = "INSERT into product_typetbl values ('" + ptypeno + "','" + ptype + "','" + pdesc + "')";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Successfully added Product type!");
                    connection.Close();
                    textBox10.Text = "";
                    richTextBox1.Text = "";
                    typeEdit.BringToFront();
                    typeAdd.SendToBack();
                    GetAllProdType();
                    FirstProductType();
                }
                catch (MySqlException me)
                {
                    MessageBox.Show(me.Message);
                }
            }
        }
        private void textBox3_KeyUp(object sender, KeyEventArgs e)
        {
            string containsLetter = @"[A-Za-z~!@#$%^&*()_+=-]";
            string cno = textBox3.Text.Trim();
            if (Regex.IsMatch(cno, containsLetter))
            {
                textBox3.BackColor = Color.FromArgb(252, 224, 224);
                label35.Text = "Numeric only";
            }
            else
            {
                label35.Text = "";
                textBox3.BackColor = Color.White;
            }
        }
        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            string containsNum = @"[0-9~!@#$%^&*()_+=-]";
            string empname = textBox2.Text.Trim();
            if (Regex.IsMatch(textBox2.Text, containsNum))
            {
                label29.Text = "No numeric character";
                textBox2.BackColor = Color.FromArgb(252, 224, 224);
            }
            else
            {
                label29.Text = "";
                textBox2.BackColor = Color.White;
            }
        }

        private void textBox4_KeyUp(object sender, KeyEventArgs e)
        {
            string containsLetter = @"[A-Za-z~!@#$%^&*()_+=-]";
            string cno = textBox4.Text.Trim();
            if (Regex.IsMatch(cno, containsLetter))
            {
                textBox4.BackColor = Color.FromArgb(252, 224, 224);
                label31.Text = "Numeric only";
            }
            else
            {
                label31.Text = "";
                textBox4.BackColor = Color.White;
            }
        }

        private void textBox5_KeyUp(object sender, KeyEventArgs e)
        {
            string containsNum = @"[0-9~!@#$%^&*()_+=-]";
            string empname = textBox5.Text.Trim();
            if (Regex.IsMatch(textBox5.Text, containsNum))
            {
                label33.Text = "No numeric character";
                textBox5.BackColor = Color.FromArgb(252, 224, 224);
            }
            else
            {
                label33.Text = "";
                textBox5.BackColor = Color.White;
            }
        }

        private void textBox8_KeyUp(object sender, KeyEventArgs e)
        {
            string containsNum = @"[0-9~!@#$%^&*()_+=-]";
            string empname = textBox8.Text.Trim();
            if (Regex.IsMatch(textBox8.Text, containsNum))
            {
                label25.Text = "No numeric character";
                textBox8.BackColor = Color.FromArgb(252, 224, 224);
            }
            else
            {
                label25.Text = "";
                textBox8.BackColor = Color.White;
            }
        }

        private void textBox10_KeyUp(object sender, KeyEventArgs e)
        {
            string containsNum = @"[0-9~!@#$%^&*()_+=-]";
            string empname = textBox10.Text.Trim();
            if (Regex.IsMatch(textBox10.Text.Trim(), containsNum))
            {
                label27.Text = "No numeric character";
                textBox10.BackColor = Color.FromArgb(252, 224, 224);
            }
            else
            {
                label27.Text = "";
                textBox10.BackColor = Color.White;
            }
        }

        private void typeEdit_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
        }

        private void button14_Click(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_KeyUp(object sender, EventArgs e)
        {

        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {

            dataGridView4.Enabled = true;
            button20.Enabled = true;
            button21.Enabled = true;
            button14.Enabled = true;
            button11.Enabled = true;

            typeAdd.Hide();
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            addPanel.Visible = false;
            button8.Enabled = true;
            button1.Enabled = true;
            dataGridView1.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = true;
            button6.Enabled = true;
            button14.Enabled = true;
            button11.Enabled = true;

        }

        private void textBox12_KeyUp(object sender, KeyEventArgs e)
        {
            string containsLetter = @"[A-Za-z~!@#$%^&*()_+=-]";
            string cno = textBox12.Text.Trim();
            if (Regex.IsMatch(cno, containsLetter))
            {
                textBox12.BackColor = Color.FromArgb(252, 224, 224);
                label2.Text = "Numeric only";
            }
            else
            {
                label35.Text = "";
                textBox2.BackColor = Color.White;
            }
        }

        private void editPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox13_KeyUp(object sender, KeyEventArgs e)
        {
            string containsLetter = @"[A-Za-z~!@#$%^&*()_+=-]";
            string cno = textBox13.Text.Trim();
            if (Regex.IsMatch(cno, containsLetter))
            {
                textBox13.BackColor = Color.FromArgb(252, 224, 224);
                label4.Text = "Numeric only";
            }
            else
            {
                label4.Text = "";
                textBox13.BackColor = Color.White;
            }
        }

        private void dataGridView3_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string critlvl = dataGridView3.Rows[e.RowIndex].Cells[2].Value.ToString();
                string containLetter = @"[A-Za-z~!@#$%^&*()_+=-]";
                bool check = false;
                if (string.IsNullOrEmpty(critlvl))
                {
                    dataGridView3.Rows[e.RowIndex].Cells[2].Value = 0;
                }
                else
                {
                    if (Regex.IsMatch(critlvl, containLetter))
                    {
                        dataGridView3.Rows[e.RowIndex].Cells[2].Value = 0;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void ProductsUC_Load(object sender, EventArgs e)
        {

        }

    }
}
