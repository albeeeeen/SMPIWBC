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
    public partial class CriticalLvlUC : UserControl
    {
        static string connectionString =
       System.Configuration.ConfigurationManager.
       ConnectionStrings["SWSFCSMPIWBC.Properties.Settings.slimmersdbConnectionString"].ConnectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);
        public CriticalLvlUC()
        {
            InitializeComponent();
            //GetInventoryItems();
            //ClearAll();
        
        }
        public void ClearAll()
        {
            //textBox1.Text = "";
            //textBox2.Text = "";
            //textBox3.Text = "";
            //numericUpDown1.Value = 0;
        }
        public void GetInventoryItems()
        {
            //dataGridView1.Rows.Clear();
            //try
            //{
            //    connection.Open();
            //    MySqlCommand cmd = new MySqlCommand("SELECT * from product_inventorytbl pi, product_prodtypetbl ppt, product_typetbl pt, producttbl p where pi.Product_ProdType_No = ppt.Product_ProdType_No and ppt.Product_Type_No = pt.Product_Type_No and ppt.Product_No = p.Product_No order by pi.Inventory_No", connection);
            //    MySqlDataReader dataReader = cmd.ExecuteReader();
            //    while (dataReader.Read())
            //    {
            //        dataGridView1.Rows.Add(dataReader.GetInt32("Inventory_No"), dataReader.GetString("Product_Type"), dataReader.GetString("Product_Name"), dataReader.GetInt32("Total_Quantity"), dataReader.GetInt32("crit_level"));
            //    }
            //    connection.Close();
            //}
            //catch (Exception me)
            //{
            //    connection.Close();
            //    MessageBox.Show(me.Message);
            //}
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //int prodno = 0;
            //string prodtype = "", prodname = "";
            //int crit = 0;
            //int row = 0;

            //prodno = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            //    try
            //    {
            //        connection.Open();
            //        MySqlCommand cmd = new MySqlCommand("SELECT * from product_inventorytbl pi, product_prodtypetbl ppt, product_typetbl pt, producttbl p where pi.Product_ProdType_No = ppt.Product_ProdType_No and ppt.Product_Type_No = pt.Product_Type_No and ppt.Product_No = p.Product_No order by pi.Inventory_No", connection);
            //        MySqlDataReader dataReader = cmd.ExecuteReader();
            //        while (dataReader.Read())
            //        {
            //            prodno = dataReader.GetInt32("Inventory_No");
            //            prodtype = dataReader.GetString("Product_Type");
            //            prodname = dataReader.GetString("Product_Name");
            //            crit = dataReader.GetInt32("crit_level");
            //        }

            //        textBox1.Text = prodno.ToString();
            //        textBox2.Text = prodtype.ToString();
            //        textBox3.Text = prodname.ToString();
            //        numericUpDown1.Value = crit;
                        

            //        connection.Close();
                    
            //    }
            //    catch (MySqlException me)
            //    {
            //        connection.Close();
            //        MessageBox.Show(me.Message);
            //    }
            
        }
       
        private void button6_Click(object sender, EventArgs e)
        {
            //int crit = Convert.ToInt32(numericUpDown1.Value);
            //int rows = 0, prodno = 0;           
            
            //rows = dataGridView1.CurrentCell.RowIndex;
            //prodno = Convert.ToInt32(dataGridView1.Rows[rows].Cells[0].Value);
            //try
            //{
            //    connection.Open();
            //    string query1 = "Update product_inventorytbl set crit_level = ('" + crit + "') where Inventory_No = ('"+ prodno +"')";
            //    MySqlCommand cmd1 = new MySqlCommand(query1, connection);
            //    cmd1.ExecuteNonQuery();

            //    if (checkBox1.Checked)
            //    {
            //        string query2 = "Update product_inventorytbl set crit_level = ('" + crit + "')";
            //        MySqlCommand cmd2 = new MySqlCommand(query2,connection);
            //        cmd2.ExecuteNonQuery();
            //    }
                 
            //    connection.Close();
            //}
            //catch (MySqlException me)
            //{
            //    connection.Close();
            //    MessageBox.Show(me.Message);
            //}


            //MessageBox.Show("Product Critical Level set.");
            //ClearAll();
            //GetInventoryItems();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            //ClearAll();
            //textBox1.ReadOnly = true;
            //textBox2.ReadOnly = true;
            //textBox3.ReadOnly = true;
            //numericUpDown1.Enabled = true;
            //int prodno = 0;
            //string prodtype = "", prodname = "";
            //int crit = 0;

            //int rows = 0;
            //rows = dataGridView1.CurrentCell.RowIndex;
            //prodno = Convert.ToInt32(dataGridView1.Rows[rows].Cells[0].Value);
            //try
            //{

            //    connection.Open();
            //    string query = ("SELECT * from product_inventorytbl pi, product_prodtypetbl ppt, product_typetbl pt, producttbl p where pi.Inventory_No = ('" + prodno + "') and  pi.Product_ProdType_No = ppt.Product_ProdType_No and ppt.Product_Type_No = pt.Product_Type_No and ppt.Product_No = p.Product_No order by pi.Inventory_No");
            //    MySqlCommand cmd = new MySqlCommand(query, connection);
            //    MySqlDataReader dataReader = cmd.ExecuteReader();
            //    while (dataReader.Read())
            //    {
            //        prodno = dataReader.GetInt32("Inventory_No");
            //        prodtype = dataReader.GetString("Product_Type");
            //        prodname = dataReader.GetString("Product_Name");
            //        crit = dataReader.GetInt32("crit_level");
            //    }

            //    textBox1.Text = prodno.ToString();
            //    textBox2.Text = prodtype.ToString();
            //    textBox3.Text = prodname.ToString();
            //    numericUpDown1.Value = crit;

            //}

            //catch (MySqlException er)
            //{
            //    connection.Close();
            //    MessageBox.Show(er.Message);
            //}

            //connection.Close();
        }

        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            //ClearAll();
        }

    }
}
