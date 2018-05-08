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
    public partial class ProductType : Form
    {
        static string connectionString = "datasource=localhost" + ";" + "DATABASE=slimmersdb" + ";" + "UID=root"
         + ";" + "PASSWORD=root" + ";";
        MySqlConnection connection = new MySqlConnection(connectionString);
        public ProductType()
        {
            InitializeComponent();
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
                textBox6.Text = ptypeno.ToString();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();
        }
        public void GetAllProdType()
        {
            try
            {
                connection.Open();
                string query3 = "SELECT * from product_typetbl order by Product_Type_No";
                MySqlCommand cmd3 = new MySqlCommand(query3, connection);
                MySqlDataReader dataReader3 = cmd3.ExecuteReader();
                while (dataReader3.Read())
                {
                    dataGridView1.Rows.Add(dataReader3.GetInt32("Product_Type_No"), dataReader3.GetString("Product_Type"),dataReader3.GetString("Product_Type_Description"));
                }
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();

        }
        private void ProductType_Load(object sender, EventArgs e)
        {
            
            GetAllProdType();
            GetFirstProductType();
        }
        public void GetFirstProductType()
        {
            try
            {
                connection.Open();
                string query4 = "select * from product_typetbl order by Product_Type_No LIMIT 1";
                MySqlCommand cmd4 = new MySqlCommand(query4, connection);
                MySqlDataReader dataReader4 = cmd4.ExecuteReader();
                while (dataReader4.Read())
                {
                    textBox1.Text = dataReader4.GetString("Product_Type");
                    textBox2.Text = dataReader4.GetInt32("Product_Type_No").ToString();
                    richTextBox2.Text = dataReader4.GetString("Product_Type_Description");
                }
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();
        }
        ErrorProvider errorProvider = new ErrorProvider();
        private void textBox5_Leave(object sender, EventArgs e)
        {
            string ptype;
            ptype = textBox5.Text.Trim();

            if (ptype.Length == 0)
            {
                errorProvider.SetError(textBox5, "Product type is required");
            }
            else
            {
                errorProvider.SetError(textBox5, string.Empty);
            }
        }

        private void richTextBox1_Leave(object sender, EventArgs e)
        {
            string pdesc;
            pdesc = richTextBox1.Text.Trim();

            if (pdesc.Length == 0)
            {
                errorProvider.SetError(richTextBox1, "Product type is required");
            }
            else
            {
                errorProvider.SetError(richTextBox1, string.Empty);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            
            ProductType pt = new ProductType();
            pt.Show();
            this.Hide();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            bool check = false, exists = false;
            string ptype, pdesc,checkptype;
            int ptypeno = 0;
            ptype = textBox5.Text.Trim();
            pdesc = richTextBox1.Text.Trim();
            ptypeno = Convert.ToInt32(textBox6.Text);
            if (ptype.Length == 0)
            {
                errorProvider.SetError(textBox5, "Product type is required.");
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
                errorProvider.SetError(textBox5, "Product Type already exists");
                check = true;
            }
            else
            {
                errorProvider.SetError(textBox5, string.Empty);
            }
            if (pdesc.Length == 0)
            {
                errorProvider.SetError(richTextBox1, "Product description is required.");
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
                    string query = "INSERT into product_typetbl values ('"+ptypeno+"','"+ptype+"','"+pdesc+"')";
                    MySqlCommand cmd = new MySqlCommand(query,connection);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Successfully added Product type!");
                    connection.Close();
                    textBox5.Text = "";
                    richTextBox1.Text = "";
                    editPanel.BringToFront();
                    addPanel.SendToBack();
                    GetAllProdType();
                    GetFirstProductType();
                }
                catch (MySqlException me)
                {
                    MessageBox.Show(me.Message);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            GetProdTypeNo();
            addPanel.BringToFront();
            editPanel.SendToBack();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.ReadOnly = false;
            richTextBox2.ReadOnly = false;
            button8.Enabled = true;
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            int rows = 0, ptypeno = 0;

            rows = dataGridView1.CurrentCell.RowIndex;
            ptypeno = Convert.ToInt32(dataGridView1.Rows[rows].Cells[0].Value);

            try
            {
                connection.Open();
                string query4 = "select * from product_typetbl where Product_Type_No = '" + ptypeno + "' order by Product_Type_No LIMIT 1";
                MySqlCommand cmd4 = new MySqlCommand(query4, connection);
                MySqlDataReader dataReader4 = cmd4.ExecuteReader();
                while (dataReader4.Read())
                {
                    textBox1.Text = dataReader4.GetString("Product_Type");
                    textBox2.Text = dataReader4.GetInt32("Product_Type_No").ToString();
                    richTextBox2.Text = dataReader4.GetString("Product_Type_Description");
                }
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string checkptype;
            bool exists = false, checker = false;
            string ptype = textBox1.Text.Trim();
            int ptypeno = Convert.ToInt32(textBox2.Text.Trim());
            string pdesc = richTextBox2.Text.Trim();
            if (ptype.Length == 0)
            {
                errorProvider.SetError(textBox1, "Product Type is required");
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
                    errorProvider.SetError(textBox1, "Product Type already exists");
                    checker = true;
                }
                else
                {
                    errorProvider.SetError(textBox1, string.Empty);
                }
                if (pdesc.Length == 0)
                {
                    errorProvider.SetError(richTextBox2, "Product description is required");
                    checker = true;
                }
                else
                {
                    errorProvider.SetError(richTextBox2, string.Empty);
                }
                if (checker == false)
                {
                    try
                    {
                        connection.Open();
                        string query2 = "UPDATE product_typetbl set Product_Type = '" + ptype + "', Product_Description = '" + pdesc + "' where Product_Type_No = '" + ptypeno + "'";
                        MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                        cmd2.ExecuteNonQuery();

                        MessageBox.Show("Successfully updated product type");
                        connection.Close();
                        textBox1.ReadOnly = true;
                        richTextBox2.ReadOnly = true;
                        button8.Enabled = false;
                        dataGridView1.Rows.Clear();
                        GetAllProdType();
                    }
                    catch (MySqlException me)
                    {
                        MessageBox.Show(me.Message);
                    }

                }
            }
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

        private void button9_Click(object sender, EventArgs e)
        {
            HomePage hp = new HomePage();
            hp.Show();
            this.Hide();
        }

        private void textBox24_TextChanged(object sender, EventArgs e)
        {
            string search = textBox24.Text.Trim();
            dataGridView1.Rows.Clear();
            try
            {
                connection.Open();
                string query3 = "SELECT * from product_typetbl where Product_Type LIKE '%"+search+"%' order by Product_Type_No";
                MySqlCommand cmd3 = new MySqlCommand(query3, connection);
                MySqlDataReader dataReader3 = cmd3.ExecuteReader();
                while (dataReader3.Read())
                {
                    dataGridView1.Rows.Add(dataReader3.GetInt32("Product_Type_No"), dataReader3.GetString("Product_Type"),dataReader3.GetString("Product_Type_Description"));
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
               ptypeno = Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString());
            }
            catch (Exception)
            {
                MessageBox.Show("No Result");
                textBox24.Text = "";
            }
            try
            {
                connection.Open();
                string query4 = "select * from product_typetbl where Product_Type_No = '"+ptypeno+"' order by Product_Type_No LIMIT 1";
                MySqlCommand cmd4 = new MySqlCommand(query4, connection);
                MySqlDataReader dataReader4 = cmd4.ExecuteReader();
                while (dataReader4.Read())
                {
                    textBox1.Text = dataReader4.GetString("Product_Type");
                    textBox2.Text = dataReader4.GetInt32("Product_Type_No").ToString();
                    richTextBox2.Text = dataReader4.GetString("Product_Type_Description");
                }
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
            }
            connection.Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Discounts discount = new Discounts();
            discount.Show();
            this.Hide();
        }
    }
}
