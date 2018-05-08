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
    public partial class PaymentRecord : Form
    {
        static string connectionString = "datasource=localhost" + ";" + "DATABASE=slimmersdb" + ";" + "UID=root"
        + ";" + "PASSWORD=root" + ";";
        MySqlConnection connection = new MySqlConnection(connectionString);
        public PaymentRecord()
        {
            InitializeComponent();
            GetPatientPayments();
            initTime();
        }
        private void initTime()
        {
            Timer t = new System.Windows.Forms.Timer();
            t.Interval = 1000;
            t.Tick += new EventHandler(t_Tick);
            t.Enabled = true;
        }

        void t_Tick(object sender, EventArgs e)
        {
            label14.Text = DateTime.Now.ToLongDateString();
            label28.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }
        public void GetPatientPayments()
        {
            dataGridView2.Rows.Clear();
            string patient = "";
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT *,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from patienttbl p, billingtbl b, paymenttbl py where b.Patient_No = p.Patient_No and b.Billing_No = py.Billing_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    patient = dataReader.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)");
                   
                    dataGridView2.Rows.Add(dataReader.GetInt32("Billing_No"),patient, dataReader.GetString("Billing_Date"), dataReader.GetDecimal("Total_Bill"), dataReader.GetDecimal("Amount_Paid"), dataReader.GetDecimal("Change"));
                }
                connection.Close();
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
            }
        }
        public void GetWalkInPayments()
        {
            dataGridView2.Rows.Clear();
            string patient = "";
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from billingtbl b, paymenttbl py where b.Patient_No IS NULL and b.Billing_No = py.Billing_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView2.Rows.Add(dataReader.GetInt32("Billing_No"), "Walk-In", dataReader.GetString("Billing_Date"), dataReader.GetDecimal("Total_Bill"), dataReader.GetDecimal("Amount_Paid"), dataReader.GetDecimal("Change"));
                }
                connection.Close();
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
            }
            
        }
        private void PaymentRecord_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string user = label15.Text;
            Payment pay = new Payment();
            pay.label15.Text = user;
            pay.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string user = label15.Text;
            PaymentRecord pr = new PaymentRecord();
            pr.label15.Text = user;
            pr.Show();
            this.Hide();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string userlog = label15.Text;
            Login login = new Login();
            login.CheckUser(userlog);
            this.Hide();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            string patient = textBox1.Text.Trim(), pname = "";
            if (e.KeyCode == Keys.Enter)
            {
                dataGridView2.Rows.Clear();
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT *,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from patienttbl p, billingtbl b, paymenttbl py where CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) LIKE '%" + patient + "%' and b.Patient_No = p.Patient_No and b.Billing_No = py.Billing_No", connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        pname = dataReader.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)");
                        dataGridView2.Rows.Add(dataReader.GetInt32("Billing_No"), pname , dataReader.GetString("Billing_Date"), dataReader.GetDecimal("Total_Bill"), dataReader.GetDecimal("Amount_Paid"), dataReader.GetDecimal("Change"));
                    }
                    connection.Close();
                }
                catch (Exception me)
                {
                    MessageBox.Show(me.Message);
                }
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            textBox1.Hide();
            label10.Hide();
            GetWalkInPayments();
            button14.BackColor = Color.Transparent;
            button15.BackColor = Color.Silver;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            textBox1.Show();
            label10.Show();
            GetPatientPayments();
            button15.BackColor = Color.Transparent;
            button14.BackColor = Color.Silver;
        }

        private void dataGridView2_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void label7_Click(object sender, EventArgs e)
        {
            panel5.Hide();
            panel10.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel5.Hide();
            panel10.Enabled = true;
        }

        private void dataGridView2_DoubleClick(object sender, EventArgs e)
        {
            int row = dataGridView2.CurrentCell.RowIndex;
            int billingno = Convert.ToInt32(dataGridView2.Rows[row].Cells[0].Value);
            int y = 8;
            bool check = false;
            label32.Text = "";
            label33.Text = "";
            label34.Text = "";
            flowLayoutPanel1.Controls.Clear();
            label25.Text = "0.00";
            label27.Text = "0.00";
            label26.Text = "0.00";
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT Product_Name,Product_Type,Product_Fee,Quantity from billingtbl b, billing_itemstbl bi,product_prodtypetbl ppt, producttbl p, product_typetbl pt where b.Billing_No = '" + billingno + "' and b.Billing_No = bi.Billing_No and bi.Product_ProdType_No = ppt.Product_ProdType_No and ppt.Product_No = p.Product_No and ppt.Product_Type_No = pt.Product_Type_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    check = true;
                    Label lblitemname = new Label();
                    Label lblitemtype = new Label();
                    Label lblitemfee = new Label();
                    Label lblitemqty = new Label();

                    lblitemname.Font = new Font("Century Gothic", 9, FontStyle.Regular);
                    lblitemtype.Font = new Font("Century Gothic", 9, FontStyle.Regular);
                    lblitemfee.Font = new Font("Century Gothic", 9, FontStyle.Regular);
                    lblitemqty.Font = new Font("Century Gothic", 9, FontStyle.Regular);

                    lblitemname.Width = 194;
                    lblitemtype.Width = 120;
                    lblitemfee.Width = 92;
                    lblitemqty.Width = 60;

                    lblitemqty.TextAlign = ContentAlignment.MiddleCenter;
                    lblitemfee.TextAlign = ContentAlignment.MiddleCenter;

                    lblitemname.Margin = new Padding(7, y, 0, 0);
                    lblitemtype.Margin = new Padding(0, y, 0, 0);
                    lblitemfee.Margin = new Padding(0, y, 0, 0);
                    lblitemqty.Margin = new Padding(0, y, 0, 0);

                    lblitemname.Text = dataReader.GetString("Product_Name");
                    lblitemtype.Text = dataReader.GetString("Product_Type");
                    lblitemfee.Text = dataReader.GetDecimal("Product_Fee").ToString();
                    lblitemqty.Text = dataReader.GetInt32("Quantity").ToString();

                    flowLayoutPanel1.Controls.Add(lblitemname);
                    flowLayoutPanel1.Controls.Add(lblitemtype);
                    flowLayoutPanel1.Controls.Add(lblitemfee);
                    flowLayoutPanel1.Controls.Add(lblitemqty);
                }
                connection.Close();

                connection.Open();
                MySqlCommand cmd1 = new MySqlCommand("SELECT Service_Name,Service_Fee,Quantity from billingtbl b, billing_itemstbl bi,servicetbl s where b.Billing_No = '" + billingno + "' and b.Billing_No = bi.Billing_No and bi.Product_ProdType_No IS NOT NULL and bi.Service_No = s.Service_No", connection);
                MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                while (dataReader1.Read())
                {
                    Label lblitemname1 = new Label();
                    Label lblitemtype1 = new Label();
                    Label lblitemfee1 = new Label();
                    Label lblitemqty1 = new Label();

                    lblitemname1.Font = new Font("Century Gothic", 9, FontStyle.Regular);
                    lblitemtype1.Font = new Font("Century Gothic", 9, FontStyle.Regular);
                    lblitemfee1.Font = new Font("Century Gothic", 9, FontStyle.Regular);
                    lblitemqty1.Font = new Font("Century Gothic", 9, FontStyle.Regular);

                    lblitemname1.Width = 194;
                    lblitemtype1.Width = 120;
                    lblitemfee1.Width = 92;
                    lblitemqty1.Width = 60;

                    lblitemname1.Margin = new Padding(7, y, 0, 0);
                    lblitemtype1.Margin = new Padding(0, y, 0, 0);
                    lblitemfee1.Margin = new Padding(0, y, 0, 0);
                    lblitemqty1.Margin = new Padding(0, y, 0, 0);

                    lblitemqty1.TextAlign = ContentAlignment.MiddleCenter;
                    lblitemfee1.TextAlign = ContentAlignment.MiddleCenter;

                    lblitemname1.Text = dataReader1.GetString("Service_Name");
                    lblitemtype1.Text = "Service";
                    lblitemfee1.Text = dataReader1.GetDecimal("Service_Fee").ToString();
                    lblitemqty1.Text = dataReader1.GetInt32("Quantity").ToString();

                    flowLayoutPanel1.Controls.Add(lblitemname1);
                    flowLayoutPanel1.Controls.Add(lblitemtype1);
                    flowLayoutPanel1.Controls.Add(lblitemfee1);
                    flowLayoutPanel1.Controls.Add(lblitemqty1);
                }
                connection.Close();

                if (check == false)
                {
                    Label lblitemname1 = new Label();
                    Label lblitemtype1 = new Label();
                    Label lblitemfee1 = new Label();
                    Label lblitemqty1 = new Label();

                    lblitemname1.Font = new Font("Century Gothic", 9, FontStyle.Regular);
                    lblitemtype1.Font = new Font("Century Gothic", 9, FontStyle.Regular);
                    lblitemfee1.Font = new Font("Century Gothic", 9, FontStyle.Regular);
                    lblitemqty1.Font = new Font("Century Gothic", 9, FontStyle.Regular);

                    lblitemname1.Width = 194;
                    lblitemtype1.Width = 120;
                    lblitemfee1.Width = 92;
                    lblitemqty1.Width = 60;

                    lblitemqty1.TextAlign = ContentAlignment.MiddleCenter;
                    lblitemfee1.TextAlign = ContentAlignment.MiddleCenter;

                    lblitemname1.Margin = new Padding(7, y, 0, 0);
                    lblitemtype1.Margin = new Padding(0, y, 0, 0);
                    lblitemfee1.Margin = new Padding(0, y, 0, 0);
                    lblitemqty1.Margin = new Padding(0, y, 0, 0);

                    lblitemname1.Text = "Balance";
                    lblitemtype1.Text = "balance";
                    lblitemfee1.Text = dataGridView2.Rows[row].Cells[3].Value.ToString();
                    lblitemqty1.Text = "0";

                    flowLayoutPanel1.Controls.Add(lblitemname1);
                    flowLayoutPanel1.Controls.Add(lblitemtype1);
                    flowLayoutPanel1.Controls.Add(lblitemfee1);
                    flowLayoutPanel1.Controls.Add(lblitemqty1);
                }
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
            }
            label32.Text = dataGridView2.Rows[row].Cells[0].Value.ToString();
            label33.Text = dataGridView2.Rows[row].Cells[1].Value.ToString();
            label34.Text = dataGridView2.Rows[row].Cells[2].Value.ToString();
            label25.Text = dataGridView2.Rows[row].Cells[3].Value.ToString();
            label27.Text = dataGridView2.Rows[row].Cells[4].Value.ToString();
            label26.Text = dataGridView2.Rows[row].Cells[5].Value.ToString();
            panel5.Show();
            panel10.Enabled = false;
        }

        private void metroPanel16_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
