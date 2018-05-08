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
    public partial class InventoryUC : UserControl
    {
        static string connectionString =
       System.Configuration.ConfigurationManager.
       ConnectionStrings["SWSFCSMPIWBC.Properties.Settings.slimmersdbConnectionString"].ConnectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);
        public InventoryUC()
        {
            InitializeComponent();
            GetInventoryItems();
            GetIncompleteOrder();
            GetCriticalLevel();
        }
        public event EventHandler CloseButtonClicked;
        protected virtual void OnCloseButtonClicked(EventArgs e)
        {
            var handler = CloseButtonClicked;
            if (handler != null)
            {
                handler(this, e);
            }

        }
        public allinventoryUC ParentForm { get; set; }
        public void GetCriticalLevel()
        {
            panel2.Visible = false;
            int qty = 0, crit = 0;
            //int qty = 0;
            //for (int j = 0; j < dataGridView1.Rows.Count; j++)
            //{
            //    qty = Convert.ToInt32(dataGridView1.Rows[j].Cells[3].Value);
            //    if (qty <= 10)
            //    {
            //        dataGridView1.Rows[j].Cells[0].Style.BackColor = Color.Salmon;
            //        dataGridView1.Rows[j].Cells[1].Style.BackColor = Color.Salmon;
            //        dataGridView1.Rows[j].Cells[2].Style.BackColor = Color.Salmon;
            //        dataGridView1.Rows[j].Cells[3].Style.BackColor = Color.Salmon;
            //        panel2.Visible = true;
            //    }
            //}
            int j = 0;
            try
            {
                connection.Open();
                string query = ("Select Total_Quantity, Critical_Level from product_inventorytbl pi, product_prodtypetbl ppt where ppt.Product_ProdType_No = pi.Product_ProdType_No");
                MySqlCommand cmd = new MySqlCommand(query,connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while(dataReader.Read())
                {
                    qty = dataReader.GetInt32("Total_Quantity");
                    crit = dataReader.GetInt32("Critical_Level");
                    if (qty <= crit)
                    {
                        dataGridView1.Rows[j].Cells[0].Style.BackColor = Color.Salmon;
                        dataGridView1.Rows[j].Cells[1].Style.BackColor = Color.Salmon;
                        dataGridView1.Rows[j].Cells[2].Style.BackColor = Color.Salmon;
                        dataGridView1.Rows[j].Cells[3].Style.BackColor = Color.Salmon;
                        panel2.Visible = true;
                    }
                    j++;
                }
                connection.Close();
            }
            catch (MySqlException e)
            {
                connection.Close();
                MessageBox.Show(e.Message);
            }
        }
        public void GetInventoryItems()
        {
            dataGridView1.Rows.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from product_inventorytbl pi, product_prodtypetbl ppt, product_typetbl pt, producttbl p where pi.Product_ProdType_No = ppt.Product_ProdType_No and ppt.Product_Type_No = pt.Product_Type_No and ppt.Product_No = p.Product_No order by pi.Inventory_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView1.Rows.Add(dataReader.GetInt32("Inventory_No"), dataReader.GetString("Product_Type"), dataReader.GetString("Product_Name"), dataReader.GetInt32("Total_Quantity"));
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }

        public void GetIncompleteOrder()
        {
            dataGridView2.Rows.Clear();
            try
            {
                // and ia.Order_Product_No = op.Order_Product_No -- NAG EEROR SA CONNECTIOn
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from inventory_addedtbl ia, product_inventorytbl pi, product_prodtypetbl ppt, product_typetbl pt, producttbl p, order_producttbl op where ia.Inadequate_Qty > 0 and ia.Restock_Status = 'Incomplete' and ia.Date_Added in (SELECT MAX(Date_Added) from inventory_addedtbl group by Order_Product_No) and ia.Inventory_Added_No = op.Order_Product_No and ia.Inventory_No = pi.Inventory_No and pi.Product_ProdType_No = ppt.Product_ProdType_No and ppt.Product_Type_No = pt.Product_Type_No and p.Product_No = ppt.Product_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView2.Rows.Add(false,dataReader.GetInt32("Order_Product_No"), dataReader.GetString("Product_Type"), dataReader.GetString("Product_Name"), dataReader.GetInt32("Quantity_Added"), dataReader.GetInt32("Order_Quantity"), dataReader.GetInt32("Inadequate_Qty"), dataReader.GetString("Restock_Status"));
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }
        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                dataGridView1.Cursor = Cursors.Hand;
            }
            else
            {
                dataGridView1.Cursor = Cursors.Default;
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView1.CurrentRow.Selected = false;
            dataGridView1.CurrentCell.Selected = false;
        }
        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string prodname = "", prodtype = "";
            int orderno = 0;
            if (e.ColumnIndex == dataGridView1.Columns[4].Index)
            {
                prodname = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[2].Value.ToString();
                prodtype = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1].Value.ToString();
                ParentForm.Type = prodtype;
                ParentForm.Name = prodname;
                OnCloseButtonClicked(e);
                
            }
            //inventory.slider.Location = new Point(0, 225);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ordersPanel.Visible = false;
            button7.Enabled = false;
            dataGridView2.Enabled = false;
            dataGridView1.Enabled = false;
            dataGridView3.Rows.Clear();
            dataGridView2.CommitEdit(DataGridViewDataErrorContexts.Commit);
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (Convert.ToBoolean(row.Cells[0].Value) == true)
                {
                    dataGridView3.Rows.Add(false,Convert.ToInt32(row.Cells[1].Value), row.Cells[2].Value.ToString(), row.Cells[3].Value.ToString(), Convert.ToInt32(row.Cells[6].Value), row.Cells[7].Value.ToString());
                }
                
            }
            deliverTransition.ShowSync(ordersPanel);
            ordersPanel.Show();
        }

        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            bunifuMetroTextbox1.Text = "";
            label14.Text = "";
            bunifuMetroTextbox1.BorderColorIdle = Color.Black;
            bunifuMetroTextbox1.BorderColorMouseHover = Color.Blue;
            button7.Enabled = true;
            dataGridView2.Enabled = true;
            dataGridView1.Enabled = true;
            dataGridView3.Rows.Clear();
            ordersPanel.Hide();
            GetIncompleteOrder();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView2.CommitEdit(DataGridViewDataErrorContexts.Commit);
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (Convert.ToBoolean(row.Cells[0].Value) == true)
                {
                    button7.Enabled = true;
                    break;
                }
                else
                {
                    button7.Enabled = false;
                }

            }
        }

        private void bunifuMetroTextbox1_KeyUp(object sender, KeyEventArgs e)
        {
            
        }

        private void dataGridView3_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView3.ClearSelection();
            CheckCell();
        }
        public void CheckCell()
        {
            bool check = false;
            string containsLetter = @"[A-Za-z~!@#$%^&*()_+=-]";
            dataGridView3.CommitEdit(DataGridViewDataErrorContexts.Commit);
            try
            {
                foreach (DataGridViewRow row in dataGridView3.Rows)
                {
                    if (string.IsNullOrEmpty(bunifuMetroTextbox1.Text.Trim()))
                    {
                        bunifuThinButton21.Enabled = false;
                        label14.Text = "Required Delivery Receipt No";
                        bunifuMetroTextbox1.BorderColorIdle = Color.Maroon;
                        bunifuMetroTextbox1.BorderColorMouseHover = Color.Maroon;
                    }
                    else
                    {

                        if (Regex.IsMatch(bunifuMetroTextbox1.Text.Trim(), containsLetter))
                        {
                            bunifuThinButton21.Enabled = false;
                            label14.Text = "Invalid Delivery Receipt No";
                            bunifuMetroTextbox1.BorderColorIdle = Color.Maroon;
                            bunifuMetroTextbox1.BorderColorMouseHover = Color.Maroon;
                        }
                        else
                        {
                            dataGridView3.ClearSelection();
                            int drno = 0;
                            try
                            {
                                drno = Convert.ToInt32(bunifuMetroTextbox1.Text.Trim());
                            }
                            catch (Exception)
                            {

                            }
                            try
                            {
                                connection.Open();
                                MySqlCommand cmd1 = new MySqlCommand("SELECT * from deliverytbl", connection);
                                MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                                while (dataReader1.Read())
                                {
                                    if (drno == dataReader1.GetInt64("Delivery_ReceiptNo"))
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
                                label14.Text = "Delivery Receipt No already exists";
                                bunifuMetroTextbox1.BorderColorIdle = Color.Maroon;
                                bunifuMetroTextbox1.BorderColorMouseHover = Color.Maroon;
                                bunifuThinButton21.Enabled = false;
                            }
                            else
                            {
                                label14.Text = "";
                                bunifuMetroTextbox1.BorderColorIdle = Color.Black;
                                bunifuMetroTextbox1.BorderColorMouseHover = Color.Blue;

                                if (string.IsNullOrEmpty(row.Cells[6].Value.ToString()) || string.IsNullOrEmpty(row.Cells[7].Value.ToString()))
                                {
                                    bunifuThinButton21.Enabled = false;
                                    row.Cells[6].Style.BackColor = Color.Salmon;
                                }
                                else
                                {
                                    if (Regex.IsMatch(row.Cells[6].Value.ToString(), containsLetter))
                                    {
                                        bunifuThinButton21.Enabled = false;
                                        row.Cells[6].Style.BackColor = Color.Salmon;
                                    }
                                    else
                                    {
                                        if (Convert.ToInt32(row.Cells[6].Value) > Convert.ToInt32(row.Cells[4].Value))
                                        {
                                            row.Cells[6].Style.BackColor = Color.Salmon;
                                            bunifuThinButton21.Enabled = false;
                                        }
                                        else
                                        {
                                            row.Cells[6].Style.BackColor = Color.White;
                                            bunifuThinButton21.Enabled = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void dataGridView3_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView3.ClearSelection();
            CheckCell();
        }

        private void dataGridView3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                dataGridView3.ClearSelection();
                CheckCell();
            }
            if (e.KeyCode == Keys.Tab)
            {
                dataGridView3.ClearSelection();
                CheckCell();
            }
        }
        public int GetInventoryAddedNo()
        {
            int inventadded = 0;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("Select * from inventory_addedtbl order by Inventory_Added_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    inventadded = dataReader.GetInt32("Inventory_Added_No");
                }
                connection.Close();
                inventadded = inventadded + 1;
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            return inventadded;
        }
        public int GetDeliveryNo()
        {
            int deliveryno = 0;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("Select * from deliverytbl order by Delivery_No",connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    deliveryno = dataReader.GetInt32("Delivery_No");
                   
                }
                deliveryno = deliveryno + 1;
                connection.Close();
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
            int deliveryorderno = 0;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("Select * from delivery_ordertbl order by Delivery_Order_No",connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    deliveryorderno = dataReader.GetInt32("Delivery_Order_No");
                }
                deliveryorderno = deliveryorderno + 1;
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            return deliveryorderno;
        }
        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            int prevqty = 0,insufficientamt = 0,firstprevqty = 0;
            int deliveryreceiptno = Convert.ToInt32(bunifuMetroTextbox1.Text.Trim());
            string dateadded = DateTime.Now.ToString("yyyy-MM-dd");
            string status = "";
            int deliveryno = GetDeliveryNo();
            try
            {
                connection.Open();
                MySqlCommand cmd3 = new MySqlCommand("Insert into deliverytbl values('"+deliveryno+"','"+deliveryreceiptno+"','"+dateadded+"')", connection);
                cmd3.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            for (int i = 0; i < dataGridView3.Rows.Count; i++)
            {
                
                int deliveryorderno = GetDeliveryOrderNo();
                int inventaddedno = GetInventoryAddedNo();
                int order_productno = Convert.ToInt32(dataGridView3.Rows[i].Cells[1].Value);
                int addedqty = Convert.ToInt32(dataGridView3.Rows[i].Cells[6].Value);
                int previnsufficientamt = Convert.ToInt32(dataGridView3.Rows[i].Cells[4].Value);
                int inventno = 0;
                string expdate = dataGridView3.Rows[i].Cells[7].Value.ToString();
                if (addedqty == previnsufficientamt)
                {
                    status = "Complete";
                }
                else
                {
                    status = "Incomplete";
                }
                    try       
                    {
                        connection.Open();
                        MySqlCommand cmd4 = new MySqlCommand("Insert into delivery_ordertbl values('"+deliveryorderno+"','"+deliveryno+"','"+order_productno+"','Received')", connection);
                        cmd4.ExecuteNonQuery();
                        connection.Close();

                        connection.Open();
                        MySqlCommand cmd = new MySqlCommand("SELECT * from inventory_addedtbl ia, product_inventorytbl pi where ia.Order_Product_No = '" + order_productno + "' and ia.Inventory_No = pi.Inventory_No", connection);
                        MySqlDataReader dataReader = cmd.ExecuteReader();
                        while (dataReader.Read())
                        {
                            inventno = dataReader.GetInt32("Inventory_No");
                            prevqty = dataReader.GetInt32("Total_Quantity");
                            firstprevqty = dataReader.GetInt32("Total_Quantity");
                        }
                        connection.Close();
                        prevqty = prevqty + addedqty;
                        insufficientamt = previnsufficientamt - addedqty;
                        connection.Open();
                        MySqlCommand cmd1 = new MySqlCommand("Update product_inventorytbl set Total_Quantity = '"+prevqty+"' where Inventory_No = '"+inventno+"'",connection);
                        cmd1.ExecuteNonQuery();
                        connection.Close();

                        connection.Open();
                        MySqlCommand cmd2 = new MySqlCommand("Insert into inventory_addedtbl values ('"+inventaddedno+"','"+inventno+"','"+order_productno+"','"+firstprevqty+"','"+addedqty+"','"+insufficientamt+"','"+dateadded+"','"+expdate+"','"+status+"')", connection);
                        cmd2.ExecuteNonQuery();
                        connection.Close();
                    }
                    catch (Exception me)
                    {
                        connection.Close();
                        MessageBox.Show(me.Message);
                    }
            }
            MessageBox.Show("Product Added in the inventory");
            ordersPanel.Visible = false;
            button7.Enabled = false;
            dataGridView2.Enabled = true;
            dataGridView1.Enabled = true;
            dataGridView3.Rows.Clear();
            bunifuMetroTextbox1.BorderColorIdle = Color.Black;
            bunifuMetroTextbox1.BorderColorMouseHover = Color.Blue;
            label14.Text = "";
            GetIncompleteOrder();
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView3.CommitEdit(DataGridViewDataErrorContexts.Commit);

            if (e.ColumnIndex == dataGridView3.Columns[0].Index)
            {
                if (Convert.ToBoolean(dataGridView3.Rows[e.RowIndex].Cells[0].Value) == true)
                {
                    dataGridView3.Enabled = false;
                    expirationPanel.Visible = false;
                    expirationPanel.BringToFront();
                    expirationTransition.ShowSync(expirationPanel);
                    dateTimePicker1.MinDate = DateTime.Now.AddDays(30);
                    expirationPanel.Show();
                }
                else
                {
                    expirationPanel.Hide();
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            int row = dataGridView3.CurrentRow.Index;
            expirationPanel.Hide();
            dataGridView3.Enabled = true;
            dataGridView3.Rows[row].Cells[0].Value = false;
            CheckCell();
        }

        private void bunifuThinButton23_Click(object sender, EventArgs e)
        {
            int row = dataGridView3.CurrentRow.Index;
            dataGridView3.Rows[row].Cells[7].Value = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            dataGridView3.Enabled = true;
            expirationPanel.Hide();
            CheckCell();
        }

        private void bunifuMetroTextbox1_OnValueChanged(object sender, EventArgs e)
        {
                CheckCell();
        }

        private void ordersPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
