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
    public partial class PullOutProductsUC : UserControl
    {
        static string connectionString =
       System.Configuration.ConfigurationManager.
       ConnectionStrings["SWSFCSMPIWBC.Properties.Settings.slimmersdbConnectionString"].ConnectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);
        public PullOutProductsUC()
        {
            InitializeComponent();
            GetInventoryItems();
            ClearItem();
            CheckPullOut();
        }
        public void CheckPullOut()
        {
            if (dataGridView1.Rows.Count > 0)
            {
                button8.Visible = false;
            }
            else
            {
                button8.Visible = true;
            }
        }
        public void ClearItem()
        {
            label5.Text = "";
            label6.Text = "";
            label10.Text = "";
            numericUpDown1.Value = 0;
            richTextBox1.Text = "";
            label2.Text = "";
        }
        public void GetInventoryItems()
        {
            dataGridView1.Rows.Clear();
            int quantity = 0;
            string deducted = null;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT SUM(Quantity_Deducted),ia.Inventory_Added_No,Date_Added,Expiration_Date,Product_Name,Product_Type,Quantity_Added from inventory_addedtbl ia LEFT JOIN product_inventorytbl pi ON pi.Inventory_No = ia.Inventory_No LEFT JOIN product_prodtypetbl ppt ON pi.Product_ProdType_No = ppt.Product_ProdType_No LEFT JOIN product_typetbl pt ON ppt.Product_Type_No = pt.Product_Type_No LEFT JOIN producttbl p ON ppt.Product_No = p.Product_No LEFT JOIN inventory_subtracttbl ins ON ia.Inventory_Added_No = ins.Inventory_Added_No group by Expiration_Date HAVING SUM(Quantity_Deducted) < Quantity_Added ", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    try
                    {
                        deducted = dataReader.GetString("SUM(Quantity_Deducted)");
                    }
                    catch (Exception)
                    {
                        deducted = dataReader.GetInt32("Quantity_Added").ToString();
                    }
                    quantity = dataReader.GetInt32("Quantity_Added") - Convert.ToInt32(deducted);
                    dataGridView1.Rows.Add(dataReader.GetInt32("Inventory_Added_No"),dataReader.GetDateTime("Date_Added").ToString("yyyy-MM-dd"), dataReader.GetDateTime("Expiration_Date").ToString("yyyy-MM-dd"), dataReader.GetString("Product_Name"), dataReader.GetString("Product_Type"), quantity.ToString());
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
            }
        }
        public int GetSubtractNo()
        {
            int inventsubtractno = 0;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from inventory_subtracttbl order by Inventory_Subtract_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    inventsubtractno = dataReader.GetInt32(0);
                }
                connection.Close();
                inventsubtractno = inventsubtractno + 1;
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            return inventsubtractno;
        }
        private void button8_Click(object sender, EventArgs e)
        {
            int row = dataGridView1.CurrentCell.RowIndex;
            label2.Text = dataGridView1.Rows[row].Cells[0].Value.ToString();
            label5.Text = dataGridView1.Rows[row].Cells[3].Value.ToString();
            label6.Text = dataGridView1.Rows[row].Cells[4].Value.ToString();
            label10.Text = dataGridView1.Rows[row].Cells[2].Value.ToString();
            numericUpDown1.Maximum = Convert.ToInt32(dataGridView1.Rows[row].Cells[5].Value);
            button10.Enabled = true;
            metroButton1.Enabled = false;
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            int row = dataGridView1.CurrentCell.RowIndex;

            numericUpDown1.Value = Convert.ToInt32(dataGridView1.Rows[row].Cells[5].Value);
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            label5.Text = "";
            label6.Text = "";
            label10.Text = "";
            label2.Text = "";
            numericUpDown1.Value = 0;
            richTextBox1.Text = "";
            button10.Enabled = false;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string product = label5.Text, ptype = label6.Text, expdate = label10.Text, reason = richTextBox1.Text;
            int added_no = Convert.ToInt32(label2.Text);
            int removed = Convert.ToInt32(numericUpDown1.Value), newtotal = 0, current = 0, inventoryno = 0, inventsubtractno = GetSubtractNo();
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            bool check = false;
            if (string.IsNullOrEmpty(richTextBox1.Text))
            {
                label17.Text = "You must need a valid reason!";
                check = true;
            }
            else
            {
                label17.Text = "";
            }

            if (check == false)
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd1 = new MySqlCommand("SELECT * from product_inventorytbl pi, inventory_addedtbl ia, producttbl p, product_typetbl pt,product_prodtypetbl ppt where p.Product_Name = '" + product + "' and pt.Product_Type = '" + ptype + "' and p.Product_No = ppt.Product_No and pt.Product_Type_No = ppt.Product_Type_No and ppt.Product_ProdType_No = pi.Product_ProdType_No and ia.Expiration_Date = '" + expdate + "' and ia.Inventory_No = pi.Inventory_No group by ia.Expiration_Date", connection);
                    MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                    while (dataReader1.Read())
                    {
                        current = dataReader1.GetInt32("Total_Quantity");
                        inventoryno = dataReader1.GetInt32("Inventory_No");
                    }
                    connection.Close();
                    newtotal = current - removed;
                    if (newtotal <= 0)
                    {
                        newtotal = 0;
                    }
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand("UPDATE product_inventorytbl set Total_Quantity = '" + newtotal + "' where Inventory_No = '" + inventoryno + "'", connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();

                    connection.Open();
                    MySqlCommand cmd2 = new MySqlCommand("INSERT into inventory_subtracttbl values('" + inventsubtractno + "','"+date+"','" + inventoryno + "','"+current+"','" + removed + "','" + reason + "','"+added_no+"')", connection);
                    cmd2.ExecuteNonQuery();
                    connection.Close();

                    MessageBox.Show("Products removed");
                    button10.Enabled = false;
                    metroButton1.Enabled = false;
                    GetInventoryItems();
                }
                catch (Exception me)
                {
                    connection.Close();
                    MessageBox.Show(me.Message);
                }
            }
            ClearItem();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            label5.Text = "";
            label6.Text = "";
            label10.Text = "";
            numericUpDown1.Value = 0;
            richTextBox1.Text = "";
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void metroButton1_Click_1(object sender, EventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

    }
}
