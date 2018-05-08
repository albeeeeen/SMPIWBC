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
    public partial class OrderUC : UserControl
    {
        static string connectionString =
       System.Configuration.ConfigurationManager.
       ConnectionStrings["SWSFCSMPIWBC.Properties.Settings.slimmersdbConnectionString"].ConnectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);

        public OrderUC()
        {
            InitializeComponent();
            GetProductType();
            int orderno = GetOrderNo();
            textBox1.Text = orderno.ToString("D4");
            GetDateNow();
            GetAllOrder();
        }
        
        public void GetAllOrder()
        {
            dataGridView1.Rows.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from ordertbl order by Order_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView1.Rows.Add(dataReader.GetInt32("Order_No").ToString("D4"), dataReader.GetString("Order_Status"));
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }
        public void GetDateNow()
        {
            textBox4.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }
        public int GetOrderNo()
        {
            int orderno = 0;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("Select * from ordertbl order by Order_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    orderno = dataReader.GetInt32("Order_No");
                }
                orderno = orderno + 1;
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
            }
            return orderno;
        }
        public void GetProductType()
        {
            comboBox2.Items.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("Select * from product_typetbl order by Product_Type_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    comboBox2.Items.Add(dataReader.GetString("Product_Type"));
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
                comboBox2.Items.Add("No available");
                comboBox2.SelectedIndex = 0;
            }
        }
        private void button16_Click(object sender, EventArgs e)
        {
            string containsLetter = @"[A-Za-z~!@#$%^&*()_+=-]";
            string product = comboBox1.Text, ptype = comboBox2.Text;
            string quantity = textBox2.Text.Trim();
            bool check = false;
            if (string.IsNullOrEmpty(quantity))
            {
                check = true;
                label2.Text = "Input quantity";
                textBox2.BackColor = Color.FromArgb(252, 224, 224);
            }
            else
            {
                if (Regex.IsMatch(quantity, containsLetter))
                {
                    check = true;
                    textBox2.BackColor = Color.FromArgb(252, 224, 224);
                    label2.Text = "Numeric only";
                }
                else
                {
                    textBox2.BackColor = Color.White;
                    label2.Text = "";
                }
            }

            for (int j = 0; j < dataGridView2.Rows.Count; j++)
            {
                if (product == dataGridView2.Rows[j].Cells[0].Value.ToString() && ptype == dataGridView2.Rows[j].Cells[1].Value.ToString())
                {
                    label7.Text = "Product already exists in the table";
                    check = true;
                }
                else
                {
                    label7.Text = "";
                }
            }
            if (!check)
            {
                dataGridView2.Rows.Add(product, ptype, quantity);
            }
            CheckToOrderRequest();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        public int GetOrderProductNo()
        {
            int order_productno = 0;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from order_producttbl order by Order_Product_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    order_productno = dataReader.GetInt32("Order_Product_No");
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            order_productno = order_productno + 1;
            return order_productno;
        }
        public void CheckToOrderRequest()
        {

            if (dataGridView2.RowCount == 0)
            {      
                label3.Text = "Please add the product.";   
                button7.Enabled = false;
            }
            else if (dataGridView2.RowCount > 0)
            {
                button7.Enabled = true;
                label3.Text = "";
            }
            else
            {
                button7.Enabled = true;
                label3.Text = "";
            }
        }
       

       
        private void button7_Click(object sender, EventArgs e)
        {
                      
            int orderno = 0;
            string orderdate = textBox4.Text;
            string product = "", ptype = "";
            int qty = 0,prod_typeno=0;
            orderno = Convert.ToInt32(textBox1.Text);

            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT into ordertbl values('" + orderno + "','" + orderdate + "','Pending')", connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                for (int j = 0; j < dataGridView2.Rows.Count; j++)
                {
                    int order_productno = GetOrderProductNo();
                    product = dataGridView2.Rows[j].Cells[0].Value.ToString();
                    ptype = dataGridView2.Rows[j].Cells[1].Value.ToString();
                    qty = Convert.ToInt32(dataGridView2.Rows[j].Cells[2].Value);

                    connection.Open();
                    MySqlCommand cmd1 = new MySqlCommand("SELECT * from product_prodtypetbl ppt, producttbl p, product_typetbl pt where Product_Name = '" + product + "' and Product_Type = '" + ptype + "' and p.Product_No = ppt.Product_No and pt.Product_Type_No = ppt.Product_Type_No", connection);
                    MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                    while (dataReader1.Read())
                    {
                        prod_typeno = dataReader1.GetInt32("Product_ProdType_No");
                    }
                    connection.Close();

                    connection.Open();
                    MySqlCommand cmd2 = new MySqlCommand("INSERT into order_producttbl values('" + order_productno + "','" + qty + "','" + prod_typeno + "','" + orderno + "','Not Added')", connection);
                    cmd2.ExecuteNonQuery();
                    connection.Close();
                }
                MessageBox.Show("Request order sent");
                textBox1.Text = GetOrderNo().ToString("D4");
                GetDateNow();
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                dataGridView2.Rows.Clear();
                GetProductType();
                GetAllOrder();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            
        }
        public void UpdateOrderStatus(string status)
        {
            int orderno = Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value);

            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE ordertbl set Order_Status = '" + status + "' where Order_No = '" + orderno + "'", connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                
                GetAllOrder();
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
                connection.Close();
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            bunifuThinButton23.Hide();
            bunifuThinButton21.Visible = true;
            bunifuThinButton22.Visible = true;

            ordersPanel.Visible = false;
            bool check = false;
            if (dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1].Value.ToString() == "Cancelled")
            {
                label6.Text = "Order is already cancelled";
                check = true;
            }
            else if (dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1].Value.ToString() == "Complete Order")
            {
                label6.Text = "Order is already delivered";
                check = true;
            }
            else
            {
                label6.Text = "";
            }
            if (!check)
            {
                bunifuThinButton23.Hide();
                label4.Text = "Select All";
                dataGridView3.Show();
                dataGridView4.Hide();
                bunifuCheckbox1.BringToFront();
                bunifuCheckbox2.SendToBack();
                label18.Hide();
                comboBox3.Hide();
                label13.Show();
                bunifuMetroTextbox1.Show();
                button7.Enabled = false;
                button8.Enabled = false;
                button6.Enabled = false;
                button16.Enabled = false;
                button15.Enabled = false;
                

                GetOrdered();
                orderTransition.ShowSync(ordersPanel);
                ordersPanel.Show();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {

            string status = "Cancelled";
            bool check = false;
            if (dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1].Value.ToString() == "Cancelled")
            {
                label6.Text = "Order is already cancelled";
                check = true;
            }
            else if (dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1].Value.ToString() == "Delivered")
            {
                label6.Text = "Order is already delivered";
                check = true;
            }
            else
            {
                label6.Text = "";
            }
            if (!check)
            {
                UpdateOrderStatus(status);
                MessageBox.Show("Order Cancelled");
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                label3.Text = "";
                dataGridView2.Rows.RemoveAt(dataGridView2.CurrentCell.RowIndex);
            }
            catch (Exception)
            {
                label3.Text = "No product to remove";
            }
        }
        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            string containsLetter = @"[A-Za-z~!@#$%^&*()_+=-]";
            string cno = textBox2.Text.Trim();
            if (Regex.IsMatch(cno, containsLetter))
            {
                textBox2.BackColor = Color.FromArgb(252, 224, 224);
                label2.Text = "Numeric only";
            }
            else
            {
                label2.Text = "";
                textBox2.BackColor = Color.White;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            string prodtype = comboBox2.Text;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from product_typetbl pt, producttbl p, product_prodtypetbl ppt where pt.Product_Type = '" + prodtype + "' and pt.Product_Type_No = ppt.Product_Type_No and ppt.Product_No = p.Product_No order by ppt.Product_Type_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    comboBox1.Items.Add(dataReader.GetString("Product_Name"));
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
                comboBox1.SelectedIndex = 0;
            }
            catch (Exception)
            {
                comboBox1.Items.Add("No available");
                comboBox1.SelectedIndex = 0;
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text.Trim()))
            {
                label2.Text = "Please Enter Quantity";
                textBox2.BackColor = Color.FromArgb(252, 224, 224);
            }
            else
            {
                label2.Text = "";
                textBox2.BackColor = Color.White;
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            bunifuThinButton23.Show();
            bunifuThinButton21.Visible = false;
            bunifuThinButton22.Visible = false;
            ordersPanel.Visible = false;

            int orderno = Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value);
            bunifuThinButton21.Hide();
            bunifuThinButton22.Hide();
            bunifuThinButton23.Show();
            comboBox3.Show();
            label18.Show();
            dataGridView3.Hide();
            dataGridView4.Show();
            label13.Hide();
            bunifuMetroTextbox1.Hide();
            bunifuCheckbox2.BringToFront();
            bunifuCheckbox1.SendToBack();
            label4.Text = "All";
            button7.Enabled = false;
            button8.Enabled = false;
            button6.Enabled = false;
            button16.Enabled = false;
            button15.Enabled = false;
            GetOrderInfo();
            GetDeliveryReceipt(orderno);
            orderTransition.ShowSync(ordersPanel);
            ordersPanel.Show();
        }
        public void GetOrderInfo()
        {
            int row = dataGridView1.CurrentCell.RowIndex;
            int orderno = Convert.ToInt32(dataGridView1.Rows[row].Cells[0].Value);
            string orderdate = "", orderstatus = "";
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from ordertbl where Order_No = '"+orderno+"'", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    orderdate = dataReader.GetDateTime("Order_Date").ToString("yyyy-MM-dd");
                    orderstatus = dataReader.GetString("Order_Status");
                    
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            lblNo.Text = orderno.ToString();
            lblDate.Text = orderdate;
            lblStatus.Text = orderstatus;
        }
        public void GetDeliveryReceipt(int orderno)
        {
            comboBox3.Items.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT distinct(Delivery_ReceiptNo) from ordertbl o, order_producttbl op, deliverytbl d, delivery_ordertbl deo where o.Order_No = '"+orderno+"' and o.Order_No = op.Order_No and op.Order_Product_No = deo.Order_Product_No and deo.Delivery_No = d.Delivery_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    comboBox3.Items.Add(dataReader.GetString("Delivery_ReceiptNo"));
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
                comboBox3.Text = "No Delivery Receipt";
            }
        }
        public void GetDelivered(string dr,string orderno)
        {
            dataGridView4.Rows.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from ordertbl o LEFT JOIN order_producttbl op ON o.Order_No = op.Order_No LEFT OUTER JOIN delivery_ordertbl deo ON op.Order_Product_No = deo.Order_Product_No LEFT JOIN deliverytbl d ON deo.Delivery_No = d.Delivery_No LEFT JOIN product_prodtypetbl ppt ON op.Product_ProdType_No = ppt.Product_ProdType_No LEFT JOIN product_typetbl pt ON ppt.Product_Type_No = pt.Product_Type_No LEFT JOIN producttbl p ON ppt.Product_No = p.Product_No where o.Order_No = '" + orderno + "' and d.Delivery_ReceiptNo = '"+dr+"'", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView4.Rows.Add(dataReader.GetInt32("Order_Product_No"),dataReader.GetString("Delivery_ReceiptNo"), dataReader.GetString("Product_Type"), dataReader.GetString("Product_Name"), dataReader.GetString("Order_Quantity"), "Received");
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }
        public void GetOrdered()
        {
            dataGridView3.Rows.Clear();
            
            int row = dataGridView1.CurrentCell.RowIndex;
            int orderno = Convert.ToInt32(dataGridView1.Rows[row].Cells[0].Value);
            string orderdate = "", orderstatus = "";
            int y = 8;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from ordertbl o LEFT JOIN order_producttbl op ON o.Order_No = op.Order_No LEFT OUTER JOIN delivery_ordertbl deo ON op.Order_Product_No = deo.Order_Product_No and (deo.Delivery_Status = 'Received' or deo.Delivery_Status = 'Cancelled') LEFT JOIN product_prodtypetbl ppt ON op.Product_ProdType_No = ppt.Product_ProdType_No LEFT JOIN product_typetbl pt ON ppt.Product_Type_No = pt.Product_Type_No LEFT JOIN producttbl p ON ppt.Product_No = p.Product_No where o.Order_No = '"+orderno+"' and deo.Order_Product_No IS NULL", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    orderdate = dataReader.GetDateTime("Order_Date").ToString("yyyy-MM-dd");
                    orderstatus = dataReader.GetString("Order_Status");
                    dataGridView3.Rows.Add(false,dataReader.GetInt32("Order_Product_No"), dataReader.GetString("Product_Type"), dataReader.GetString("Product_Name"), dataReader.GetString("Order_Quantity"),"Pending");

                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            dataGridView3.ClearSelection();
            lblNo.Text = orderno.ToString();
            lblDate.Text = orderdate;
            lblStatus.Text = orderstatus;
            
        }
        public int GetDeliveryNo()
        {
            int deliveryno = 0;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from deliverytbl order by Delivery_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    deliveryno = dataReader.GetInt32("Delivery_No");
                }
                connection.Close();
                deliveryno = deliveryno + 1;
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            return deliveryno;
        }
        public int GetDeliveryOrderNo()
        {
            int dono = 0;

            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from delivery_ordertbl order by Delivery_Order_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dono = dataReader.GetInt32("Delivery_Order_No");
                }
                connection.Close();
                dono = dono + 1;
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }

            return dono;
        }
        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            string status = "",devstatus = "";
            string containsLetter = @"[A-Za-z~!@#$%^&*()_+=-]";
            int deliveryno = GetDeliveryNo();
            int orderproductno = 0;
            string date = DateTime.Today.ToString("yyyy-MM-dd");
            string dr = null;
            try
            {
                dr = bunifuMetroTextbox1.Text.Trim();
            }
            catch (Exception)
            {
                dr = null;
            }
            bool check = false;
            if (Regex.IsMatch(dr.ToString(), containsLetter))
            {
                check = true;
                label14.Text = "Invalid delivery receipt number";
                bunifuMetroTextbox1.BorderColorIdle = Color.Maroon;
            }
            else
            {
                bunifuMetroTextbox1.BorderColorIdle = Color.Black;
                label14.Text = "";
            }
            
            if (!check)
            {
            dataGridView3.CommitEdit(DataGridViewDataErrorContexts.Commit);
            for (int i = 0; i < dataGridView3.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dataGridView3.Rows[i].Cells[0].Value) == true)
                {
                    status = "Complete Order";
                }
                else if (Convert.ToBoolean(dataGridView3.Rows[i].Cells[0].Value) == false && dataGridView3.Rows[i].Cells[5].Value.ToString() == "Cancelled")
                {
                    status = "Cancelled Order";
                }
                else
                {
                    status = "Incomplete Order";
                    break;
                }
            }
                
                try{
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("Insert into deliverytbl values ('"+deliveryno+"', '"+dr+"', '"+date+"')",connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                dataGridView3.CommitEdit(DataGridViewDataErrorContexts.Commit);
                for (int i = 0; i < dataGridView3.Rows.Count; i++)
                {
                    string orderstatus = dataGridView3.Rows[i].Cells[5].Value.ToString();
                    if (Convert.ToBoolean(dataGridView3.Rows[i].Cells[0].Value) == true || dataGridView3.Rows[i].Cells[5].Value.ToString() == "Cancelled")
                    {
                        int delorderno = GetDeliveryOrderNo();
                        orderproductno = Convert.ToInt32(dataGridView3.Rows[i].Cells[1].Value);
                        devstatus = dataGridView3.Rows[i].Cells[1].Value.ToString();
                        connection.Open();
                        MySqlCommand cmd1 = new MySqlCommand("Insert into delivery_ordertbl values ('" + delorderno + "','" + deliveryno + "','" + orderproductno + "','"+orderstatus+"')", connection);
                        cmd1.ExecuteNonQuery();
                        connection.Close();
                    }  
                }
                }catch(Exception me){
                    connection.Close();
                    MessageBox.Show(me.Message);
                }
                UpdateOrderStatus(status);
                MessageBox.Show("Order Received Successfully");
                bunifuMetroTextbox1.Text = "";
                label14.Text = "";
                bunifuMetroTextbox1.BorderColorIdle = Color.Black;
                bunifuMetroTextbox1.BorderColorFocused = Color.Blue;
                button7.Enabled = true;
                button8.Enabled = true;
                button6.Enabled = true;
                button16.Enabled = true;
                button15.Enabled = true;
                ordersPanel.Hide();
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView3.CommitEdit(DataGridViewDataErrorContexts.Commit);

            if (e.ColumnIndex == dataGridView3.Columns[0].Index)
            {
                try
                {
                    if (Convert.ToBoolean(dataGridView3.Rows[e.RowIndex].Cells[0].Value) == true)
                    {
                        dataGridView3.Rows[e.RowIndex].Cells[5].Value = "Received";
                    }
                    else
                    {
                        dataGridView3.Rows[e.RowIndex].Cells[5].Value = "Pending";
                    }
                }
                catch (Exception)
                {
                }
                CheckboxCheck();
            }
        }

        private void bunifuCheckbox1_OnChange(object sender, EventArgs e)
        {
            if (bunifuCheckbox1.Checked)
            {
                for (int i = 0; i < dataGridView3.Rows.Count; i++)
                {
                    dataGridView3.Rows[i].Cells[0].Value = true;
                    dataGridView3.Rows[i].Cells[5].Value = "Received";
                }
            }
            else
            {
                for (int i = 0; i < dataGridView3.Rows.Count; i++)
                {
                    dataGridView3.Rows[i].Cells[0].Value = false;
                    dataGridView3.Rows[i].Cells[5].Value = "Pending";
                    bunifuThinButton21.Enabled = false;
                }
            }
            CheckboxCheck();
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            
        }

        private void dataGridView3_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == dataGridView3.Columns[5].Index)
                {
                    string status = dataGridView3.Rows[e.RowIndex].Cells[5].Value.ToString();
                    if (status == "Cancelled")
                    {
                        dataGridView3.Rows[e.RowIndex].Cells[0].ReadOnly = true;
                        dataGridView3.Rows[e.RowIndex].Cells[0].Value = false;
                    }
                    else if (status == "Pending")
                    {
                        dataGridView3.Rows[e.RowIndex].Cells[0].Value = false;
                        dataGridView3.Rows[e.RowIndex].Cells[0].ReadOnly = false;
                    }
                    else if(status == "Received")
                    {
                        dataGridView3.Rows[e.RowIndex].Cells[0].Value = true;
                        dataGridView3.Rows[e.RowIndex].Cells[0].ReadOnly = false;
                    }
                    CheckboxCheck();
                }
                if (e.ColumnIndex == dataGridView3.Columns[0].Index)
                {
                        CheckboxCheck();
                }
               
            }
            catch (Exception me)
            {
            }
        }

        private void dataGridView3_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            
        }
        public void CheckboxCheck()
        {
            int orderno = Convert.ToInt32(lblNo.Text);
            bool check = false;
            string containsLetter = @"[A-Za-z~!@#$%^&*()_+=-]";
            int dr = 0;
            try
            {
                dr = Convert.ToInt32(bunifuMetroTextbox1.Text.Trim());
            }
            catch (Exception)
            {
                bunifuThinButton21.Enabled = false;
                label14.Text = "";
                bunifuMetroTextbox1.BorderColorIdle = Color.Black;
                bunifuMetroTextbox1.BorderColorFocused = Color.Blue;
            }
            foreach (DataGridViewRow rows in dataGridView3.Rows)
            {
                if (Convert.ToBoolean(rows.Cells[0].Value) == true)
                {
                    bunifuMetroTextbox1.Enabled = true;
                    if (Regex.IsMatch(bunifuMetroTextbox1.Text.Trim(), containsLetter) || string.IsNullOrEmpty(bunifuMetroTextbox1.Text.Trim()))
                    {
                        bunifuThinButton21.Enabled = false;
                        label14.Text = "Invalid delivery receipt number";
                        bunifuMetroTextbox1.BorderColorIdle = Color.Maroon;
                        bunifuMetroTextbox1.BorderColorFocused = Color.Maroon;
                    }
                    else
                    {

                        try
                        {
                            connection.Open();
                            MySqlCommand cmd = new MySqlCommand("SELECT distinct(Delivery_ReceiptNo) from ordertbl o, order_producttbl op, deliverytbl d, delivery_ordertbl deo where o.Order_No = '" + orderno + "' and o.Order_No = op.Order_No and op.Order_Product_No = deo.Order_Product_No and deo.Delivery_No = d.Delivery_No", connection);
                            MySqlDataReader dataReader = cmd.ExecuteReader();
                            while (dataReader.Read())
                            {
                                if (dr == dataReader.GetInt32("Delivery_ReceiptNo"))
                                {
                                    check = true;
                                    break;
                                }
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
                            label14.Text = "Delivery Receipt Number already exists";
                            bunifuMetroTextbox1.BorderColorIdle = Color.Maroon;
                            bunifuMetroTextbox1.BorderColorFocused = Color.Maroon;
                            bunifuThinButton21.Enabled = false;
                        }
                        else
                        {
                            label14.Text = "";
                            bunifuMetroTextbox1.BorderColorIdle = Color.Black;
                            bunifuMetroTextbox1.BorderColorFocused = Color.Blue;
                            dataGridView3.CommitEdit(DataGridViewDataErrorContexts.Commit);
                            for (int i = 0; i < dataGridView3.Rows.Count; i++)
                            {
                                if (Convert.ToBoolean(dataGridView3.Rows[i].Cells[0].Value) == true || dataGridView3.Rows[i].Cells[5].Value.ToString() == "Cancelled")
                                {
                                    bunifuThinButton21.Enabled = true;
                                }
                            }
                        }
                    }
                    break;
                }
                else if(rows.Cells[5].Value.ToString() == "Cancelled")
                {
                    foreach (DataGridViewRow rowrow in dataGridView3.Rows)
                    {
                        if (rowrow.Cells[5].Value.ToString() == "Cancelled")
                        {
                            label14.Text = "";
                            bunifuMetroTextbox1.BorderColorIdle = Color.Black;
                            bunifuMetroTextbox1.BorderColorFocused = Color.Blue;
                            bunifuMetroTextbox1.Enabled = false;
                            bunifuThinButton21.Enabled = true;
                            bunifuMetroTextbox1.Text = "";
                        }
                    }
                }
            }
        }
        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            ordersPanel.Visible = false;
            button7.Enabled = true;
            button8.Enabled = true;
            button6.Enabled = true;
            button16.Enabled = true;
            button15.Enabled = true;
            lblNo.Text = "";
            lblDate.Text = "";
            lblStatus.Text = "";
            bunifuMetroTextbox1.Text = "";
            bunifuMetroTextbox1.BorderColorIdle = Color.Black;
            bunifuMetroTextbox1.BorderColorFocused = Color.Blue;
            label14.Text = "";
        }

        private void bunifuMetroTextbox1_KeyUp(object sender, KeyEventArgs e)
        {
            CheckboxCheck();
        }

        private void ordersPanel_Paint(object sender, PaintEventArgs e)
        {
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string dr = comboBox3.Text;
            string orderno = lblNo.Text;
            GetDelivered(dr, orderno);
            
        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void bunifuCheckbox2_OnChange(object sender, EventArgs e)
        {
            string dr = comboBox3.Text;
            string orderno = lblNo.Text;
            if (bunifuCheckbox2.Checked)
            {
                dataGridView4.Rows.Clear();
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * from ordertbl o LEFT JOIN order_producttbl op ON o.Order_No = op.Order_No LEFT OUTER JOIN delivery_ordertbl deo ON op.Order_Product_No = deo.Order_Product_No LEFT JOIN deliverytbl d ON deo.Delivery_No = d.Delivery_No LEFT JOIN product_prodtypetbl ppt ON op.Product_ProdType_No = ppt.Product_ProdType_No LEFT JOIN product_typetbl pt ON ppt.Product_Type_No = pt.Product_Type_No LEFT JOIN producttbl p ON ppt.Product_No = p.Product_No where o.Order_No = '" + orderno + "'", connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        dataGridView4.Rows.Add(dataReader.GetInt32("Order_Product_No"), dataReader.GetString("Delivery_ReceiptNo"), dataReader.GetString("Product_Type"), dataReader.GetString("Product_Name"), dataReader.GetString("Order_Quantity"), "Received");
                    }
                    connection.Close();
                }
                catch (Exception me)
                {
                    connection.Close();
                    MessageBox.Show(me.Message);
                }
            }
            else
            {
                GetDelivered(dr, orderno);
            }
        }

        private void bunifuThinButton23_Click(object sender, EventArgs e)
        {
            button7.Enabled = true;
            button8.Enabled = true;
            button6.Enabled = true;
            button16.Enabled = true;
            button15.Enabled = true;
            ordersPanel.Hide();
        }

        private void dataGridView3_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView3.IsCurrentCellDirty)
            {
                // This fires the cell value changed handler below
                dataGridView3.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void textBox3_OnValueChanged(object sender, EventArgs e)
        {
            string po = "";
            try
            {
                po = textBox3.Text.Trim();
            }
            catch (Exception)
            {
                po = "";
            }
            dataGridView1.Rows.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from ordertbl where Order_No LIKE '%"+po+"%' order by Order_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView1.Rows.Add(dataReader.GetInt32("Order_No").ToString("D4"), dataReader.GetString("Order_Status"));
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }
    }
}
