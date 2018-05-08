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

namespace SWSFCSMPIWBC
{
    public partial class AdddInventoryUC : UserControl
    {
        static string connectionString =
       System.Configuration.ConfigurationManager.
       ConnectionStrings["SWSFCSMPIWBC.Properties.Settings.slimmersdbConnectionString"].ConnectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);
        public AdddInventoryUC()
        {
            InitializeComponent();
            dateTimePicker1.MinDate = DateTime.Now;
            OrderDelivered();
        }
        public void OrderDelivered()
        {
            comboBox2.Items.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT distinct(order_No) from order_producttbl op, product_prodtypetbl ppt, product_typetbl pt, producttbl p,delivery_ordertbl deo where op.Order_Product_No = deo.Order_Product_No and op.Order_Product_Status = 'Not Added' and op.Product_ProdType_No = ppt.Product_ProdType_No and ppt.Product_No = p.Product_No and ppt.Product_Type_No = pt.Product_Type_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    comboBox2.Items.Add(dataReader.GetInt32("Order_No").ToString("D4"));
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
                comboBox2.SelectedIndex = 0;
            }
            catch (Exception)
            {
                comboBox2.Text = "No Available";
            }
        }
        public int InventoryNo()
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
                connection.Close();
                inventno = inventno + 1;
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            return inventno;
        }
        public void GetOrders(int orderno)
        {
            int prevqty = 0;
            dataGridView1.Rows.Clear();
            int prod_typeno = 0;

            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from order_producttbl op, product_prodtypetbl ppt, product_typetbl pt, producttbl p,delivery_ordertbl deo where op.Order_No = '" + orderno + "' and op.Order_Product_No = deo.Order_Product_No and op.Order_Product_Status = 'Not Added' and op.Product_ProdType_No = ppt.Product_ProdType_No and ppt.Product_No = p.Product_No and ppt.Product_Type_No = pt.Product_Type_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView1.Rows.Add(dataReader.GetString("Product_Name"), dataReader.GetString("Product_Type"),"", dataReader.GetInt32("Order_Quantity"));
                }
                connection.Close();

                for (int j = 0; j < dataGridView1.Rows.Count; j++)
                {
                    string prod = dataGridView1.Rows[j].Cells[0].Value.ToString(), ptype = dataGridView1.Rows[j].Cells[1].Value.ToString();
                    connection.Open();
                    MySqlCommand cmd1 = new MySqlCommand("SELECT * from order_producttbl op, product_inventorytbl pi,producttbl p,product_typetbl pt, product_prodtypetbl ppt where p.Product_Name = '"+prod+"' and pt.Product_Type = '"+ptype+"' and p.Product_No = ppt.Product_No and pt.Product_Type_No = ppt.Product_Type_No and op.Order_No = '" + orderno + "' and ppt.Product_ProdType_No = op.Product_ProdType_No and op.Product_ProdType_No = pi.Product_ProdType_No", connection);
                    MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                    while (dataReader1.Read())
                    {
                        prevqty = dataReader1.GetInt32("Total_Quantity");
                    }
                    connection.Close();
                    dataGridView1.Rows[j].Cells[2].Value = prevqty;
                    prevqty = 0;
                }
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int orderno = Convert.ToInt32(comboBox2.Text);
            GetOrders(orderno);
            label7.Text = "";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            bool check = false;
            string prod = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString();
            string ptype = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1].Value.ToString();
            int prev = Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[2].Value);
            int ordered = Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[3].Value);
            label7.Text = comboBox2.Text;
            textBox1.Text = prod;
            textBox2.Text = prev.ToString();
            textBox5.Text = ordered.ToString();
            textBox6.Text = ptype;
            textBox3.Enabled = true;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("Select * from product_inventorytbl pi, producttbl p, product_typetbl pt, product_prodtypetbl ppt where p.Product_Name = '" + prod + "' and pt.Product_Type = '" + ptype + "' and p.Product_No = ppt.Product_No and pt.Product_Type_No = ppt.Product_Type_No and ppt.Product_ProdType_No = pi.Product_ProdType_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    check = true;
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            if (check)
            {
                button5.Visible = true;
                button10.Visible = false;
            }
            else
            {
                button5.Visible = false;
                button10.Visible = true;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            string status = "";
            string containLetter = @"[A-Za-z~!@#$%^&*()_+=-]";
            try
            {
                
                int current = Convert.ToInt32(textBox2.Text), order = Convert.ToInt32(textBox5.Text), diff = 0;
                string received = textBox3.Text.Trim();
                if (string.IsNullOrEmpty(received))
                {
                    label15.Text = "Input received quantity";
                    textBox3.BackColor = Color.FromArgb(252, 224, 224);
                    button10.Enabled = false;
                    button5.Enabled = false;
                }
                else
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(received, containLetter))
                    {
                        label15.Text = "Should be numeric";
                        textBox3.BackColor = Color.FromArgb(252, 224, 224);
                        button10.Enabled = false;
                        button5.Enabled = false;
                    }
                    else
                    {

                        if (Convert.ToInt32(received) > order)
                        {
                            label15.Text = "Received item is higher than ordered quantity";
                            textBox3.BackColor = Color.FromArgb(252, 224, 224);
                            button10.Enabled = false;
                            button5.Enabled = false;
                        }
                        else
                        {
                            button10.Enabled = true;
                            button5.Enabled = true;
                            label15.Text = "";
                            textBox3.BackColor = Color.White;
                            diff = order - Convert.ToInt32(received);

                            if (diff > 0)
                            {
                                status = "Incomplete";
                            }
                            else if (diff <= 0)
                            {
                                status = "Complete";
                            }
                            textBox4.Text = status;
                        }
                    }
                }
                
            }
            catch (Exception)
            {
                
            }
        }
        int GetInventAddedNo()
        {
            int added_no = 0;

            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from inventory_addedtbl order by Inventory_Added_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    added_no = dataReader.GetInt32("Inventory_Added_No");
                }
                connection.Close();
                added_no = added_no + 1;
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }

            return added_no;
        }
        private void button10_Click(object sender, EventArgs e)
        {
            string prod = textBox1.Text, prodtype = textBox6.Text, expdate = dateTimePicker1.Value.ToString("yyyy-MM-dd"),status = textBox4.Text,dateadded = DateTime.Now.ToString("yyyy-MM-dd");
            int current = Convert.ToInt32(textBox2.Text), order = Convert.ToInt32(textBox5.Text), receive = Convert.ToInt32(textBox3.Text);
            int inventno = InventoryNo();
            int orderno = Convert.ToInt32(label7.Text);
            int prod_typeno = 0, inventadded_no = GetInventAddedNo();
            int total = current + receive;
            int kulang = order - receive;
            int order_productno = 0;
            if (kulang <= 0)
            {
                kulang = 0;
            }
            try
            {
                connection.Open();
                MySqlCommand cmd1 = new MySqlCommand("SELECT * from product_prodtypetbl ppt, product_typetbl pt, producttbl p where p.Product_Name = '" + prod + "' and pt.Product_Type = '" + prodtype + "' and p.Product_No = ppt.Product_No and pt.Product_Type_No = ppt.Product_Type_No", connection);
                MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                while (dataReader1.Read())
                {
                    prod_typeno = dataReader1.GetInt32("Product_ProdType_No");
                }
                connection.Close();

                connection.Open();
                MySqlCommand cmd4 = new MySqlCommand("SELECT * from order_producttbl where Order_No = '"+orderno+"' and Product_ProdType_No = '"+prod_typeno+"'", connection);
                MySqlDataReader dataReader4 = cmd4.ExecuteReader();
                while (dataReader4.Read())
                {
                    order_productno = dataReader4.GetInt32("Order_Product_No");
                }
                connection.Close();

                connection.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT into product_inventorytbl values('"+inventno+"','"+prod_typeno+"','"+total+"')", connection);
                cmd.ExecuteNonQuery();
                connection.Close();

                connection.Open();
                MySqlCommand cmd2 = new MySqlCommand("INSERT into inventory_addedtbl values ('"+inventadded_no+"','"+inventno+"','"+order_productno+"','"+current+"','"+receive+"','"+kulang+"','"+dateadded+"','"+expdate+"','"+status+"')", connection);
                cmd2.ExecuteNonQuery();
                connection.Close();

                connection.Open();
                MySqlCommand cmd3 = new MySqlCommand("UPDATE order_producttbl set Order_Product_Status = 'Added' where Order_Product_No = '"+order_productno+"'", connection);
                cmd3.ExecuteNonQuery();
                connection.Close();

                MessageBox.Show("Product added in the inventory list!");
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                label7.Text = "";
                textBox3.Enabled = false;
                GetOrders(orderno);
                OrderDelivered();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }


        }

        private void button5_Click(object sender, EventArgs e)
        {
            string prod = textBox1.Text, prodtype = textBox6.Text, expdate = dateTimePicker1.Value.ToString("yyyy-MM-dd"), status = textBox4.Text, dateadded = DateTime.Now.ToString("yyyy-MM-dd");
            int current = Convert.ToInt32(textBox2.Text), order = Convert.ToInt32(textBox5.Text), receive = Convert.ToInt32(textBox3.Text);
            int orderno = Convert.ToInt32(label7.Text);
            int prod_typeno = 0, inventadded_no = GetInventAddedNo();
            int total = current + receive;
            int kulang = order - receive;
            int inventno = 0,order_productno = 0;
            if (kulang <= 0)
            {
                kulang = 0;
            }
            try
            {
                connection.Open();
                MySqlCommand cmd1 = new MySqlCommand("SELECT * from product_prodtypetbl ppt, product_typetbl pt, producttbl p where p.Product_Name = '" + prod + "' and pt.Product_Type = '" + prodtype + "' and p.Product_No = ppt.Product_No and pt.Product_Type_No = ppt.Product_Type_No", connection);
                MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                while (dataReader1.Read())
                {
                    prod_typeno = dataReader1.GetInt32("Product_ProdType_No");
                }
                connection.Close();

                connection.Open();
                MySqlCommand cmd4 = new MySqlCommand("SELECT * from product_inventorytbl where Product_ProdType_No = '" + prod_typeno + "'", connection);
                MySqlDataReader dataReader4 = cmd4.ExecuteReader();
                while (dataReader4.Read())
                {
                    inventno = dataReader4.GetInt32("Inventory_No");
                }
                connection.Close();
                connection.Open();
                MySqlCommand cmd5 = new MySqlCommand("SELECT * from order_producttbl where Order_No = '" + orderno + "' and Product_ProdType_No = '" + prod_typeno + "'", connection);
                MySqlDataReader dataReader5 = cmd5.ExecuteReader();
                while (dataReader5.Read())
                {
                    order_productno = dataReader5.GetInt32("Order_Product_No");
                }
                connection.Close();
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE product_inventorytbl set Total_Quantity = '" + total + "' where Inventory_No = '"+inventno+"'", connection);
                cmd.ExecuteNonQuery();
                connection.Close();

                connection.Open();
                MySqlCommand cmd2 = new MySqlCommand("INSERT into inventory_addedtbl values ('" + inventadded_no + "','" + inventno + "','"+order_productno+"','"+current+"','" + receive + "','" + kulang + "','" + dateadded + "','" + expdate + "','" + status + "')", connection);
                cmd2.ExecuteNonQuery();
                connection.Close();

                connection.Open();
                MySqlCommand cmd3 = new MySqlCommand("UPDATE order_producttbl set Order_Product_Status = 'Added' where Order_No = '" + orderno + "' and Product_ProdType_No = '" + prod_typeno + "'", connection);
                cmd3.ExecuteNonQuery();
                connection.Close();

                MessageBox.Show("Product stock added!");
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                label7.Text = "";
                textBox3.Enabled = false;
                GetOrders(orderno);
                OrderDelivered();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            label7.Text = "";
            textBox3.Enabled = false;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
