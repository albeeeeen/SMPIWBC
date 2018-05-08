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
    public partial class PullOutRecordUC : UserControl
    {
        static string connectionString =
       System.Configuration.ConfigurationManager.
       ConnectionStrings["SWSFCSMPIWBC.Properties.Settings.slimmersdbConnectionString"].ConnectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);
        public PullOutRecordUC()
        {
            InitializeComponent();
        }
        public void GetPurchasedRecord()
        {
            dataGridView1.Rows.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from inventory_subtracttbl ins, product_inventorytbl pi, product_prodtypetbl ppt, product_typetbl pt, producttbl p where ins.Inventory_No = pi.Inventory_No and pi.Product_ProdType_No = ppt.Product_ProdType_No and ppt.Product_Type_No = pt.Product_Type_No and ppt.Product_No = p.Product_No and ins.Reason_Usage = 'Purchased'", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows.Add(dataReader.GetDateTime("Date_Deducted").ToString("MM-dd-yyyy"), dataReader.GetString("Product_Type"), dataReader.GetString("Product_Name"), dataReader.GetInt32("Previous_Quantity"), dataReader.GetInt32("Quantity_Deducted"), dataReader.GetString("Reason_Usage"));
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }
        public void GetExpiredRecord()
        {
            dataGridView1.Rows.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from inventory_subtracttbl ins, product_inventorytbl pi, product_prodtypetbl ppt, product_typetbl pt, producttbl p where ins.Inventory_No = pi.Inventory_No and pi.Product_ProdType_No = ppt.Product_ProdType_No and ppt.Product_Type_No = pt.Product_Type_No and ppt.Product_No = p.Product_No and ins.Inventory_Added_No <> 0", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows.Add(dataReader.GetDateTime("Date_Deducted").ToString("MM-dd-yyyy"), dataReader.GetString("Product_Type"), dataReader.GetString("Product_Name"), dataReader.GetInt32("Previous_Quantity"), dataReader.GetInt32("Quantity_Deducted"), dataReader.GetString("Reason_Usage"));
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }
        public void GetUsedRecord()
        {
            dataGridView1.Rows.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from inventory_subtracttbl ins, product_inventorytbl pi, product_prodtypetbl ppt, product_typetbl pt, producttbl p where ins.Inventory_No = pi.Inventory_No and pi.Product_ProdType_No = ppt.Product_ProdType_No and ppt.Product_Type_No = pt.Product_Type_No and ppt.Product_No = p.Product_No and (ins.Reason_Usage = 'Cancelled Service' or ins.Reason_Usage = 'For Service')", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows.Add(dataReader.GetDateTime("Date_Deducted").ToString("MM-dd-yyyy"), dataReader.GetString("Product_Type"), dataReader.GetString("Product_Name"), dataReader.GetInt32("Previous_Quantity"), dataReader.GetInt32("Quantity_Deducted"), dataReader.GetString("Reason_Usage"));
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }

        public void GetOthersRecord()
        {
            dataGridView1.Rows.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from inventory_subtracttbl ins, product_inventorytbl pi, product_prodtypetbl ppt, product_typetbl pt, producttbl p where ins.Inventory_No = pi.Inventory_No and pi.Product_ProdType_No = ppt.Product_ProdType_No and ppt.Product_Type_No = pt.Product_Type_No and ppt.Product_No = p.Product_No and (ins.Reason_Usage <> 'Cancelled Service' and ins.Reason_Usage <> 'For Service' and ins.Reason_Usage <> 'Expired Item' and ins.Reason_Usage <> 'Expired' and ins.Reason_Usage <> 'Purchased')", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows.Add(dataReader.GetDateTime("Date_Deducted").ToString("MM-dd-yyyy"), dataReader.GetString("Product_Type"), dataReader.GetString("Product_Name"), dataReader.GetInt32("Previous_Quantity"), dataReader.GetInt32("Quantity_Deducted"), dataReader.GetString("Reason_Usage"));
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            GetPurchasedRecord();
        }

        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            GetExpiredRecord();
        }

        private void bunifuThinButton23_Click(object sender, EventArgs e)
        {
            GetUsedRecord();
        }

        private void bunifuThinButton24_Click(object sender, EventArgs e)
        {
            GetOthersRecord();
        }
    }
}
