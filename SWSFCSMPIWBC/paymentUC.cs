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
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Net;
using System.Diagnostics;
using TextmagicRest;
using TextmagicRest.Model;
using System.Net.Mail;
namespace SWSFCSMPIWBC
{
    public partial class paymentUC : UserControl
    {
        static string connectionString =
       System.Configuration.ConfigurationManager.
       ConnectionStrings["SWSFCSMPIWBC.Properties.Settings.slimmersdbConnectionString"].ConnectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);
        object obj;
        public paymentUC()
        {
            InitializeComponent();
            btnFirstPay.Show();
            btnSecondPay.Hide();
            btnFromAppointment.Hide();
            button15.BringToFront(); comboBox6.SelectedIndex = 0;
            label20.Visible = true;
            comboBox2.Visible = true;
            button10.Visible = true;
            button13.Visible = true;
            label29.Visible = true;
            comboBox5.Visible = true;
            label22.Visible = true;
            numericUpDown1.Visible = true;
            
            button16.IdleFillColor = System.Drawing.Color.FromArgb(4, 91, 188);
            button16.IdleForecolor = System.Drawing.Color.White;

            button1.Textcolor = System.Drawing.Color.FromArgb(4, 180, 253);
            bunifuDropdown1.selectedIndex = 0;
            GetPatientPayments();
            GetPatients();
            GetItems();
            GetBalances();
        }

        public void GetBalances()
        {
            dataGridView2.Rows.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT Billing_No,Billing_Date,Total_Bill,Balance,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit),b.Patient_No from billingtbl b, patienttbl p where p.Patient_No = b.Patient_No and b.Balance > 0 and Billing_No in (SELECT MAX(Billing_No) from billingtbl group by Patient_No)", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView2.Rows.Add(dataReader.GetInt32("Billing_No"), dataReader.GetDateTime("Billing_Date").ToString("MM-dd-yyyy"), dataReader.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)"), dataReader.GetDecimal("Total_Bill"), dataReader.GetDecimal("Balance"));
                }
                connection.Close();
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
            }
        }
        public void GetItems()
        {
            comboBox4.Items.Clear();
            comboBox5.Items.Clear();
            string prod = "";
            try
            {
                connection.Open();
                string query = "SELECT * from servicetbl where Service_Status = 'Active' order by Service_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    comboBox4.Items.Add(dataReader.GetString("Service_Name"));
                }
                connection.Close();

                connection.Open();
                string query1 = "SELECT * from producttbl p, product_inventorytbl pi, product_prodtypetbl ppt, product_typetbl pt where p.Product_Status = 'Available' and pi.Total_Quantity > 0 and p.Product_No = ppt.Product_No and ppt.Product_ProdType_No = pi.Product_ProdType_No and  ppt.Product_Type_No = pt.Product_Type_No group by pt.Product_Type_No order by p.Product_No";
                MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                while (dataReader1.Read())
                {
                    comboBox5.Items.Add(dataReader1.GetString("Product_Type"));
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
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
            try
            {
                comboBox5.SelectedIndex = 0;
            }
            catch (Exception)
            {
                comboBox5.Items.Add("No available");
                comboBox5.SelectedIndex = 0;
            }

        }
        public void GetPatients()
        {
            comboBox3.Items.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT *,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from patienttbl order by Patient_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    comboBox3.Items.Add(dataReader.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)"));
                }
                connection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void button7_Click_1(object sender, EventArgs e)
        {
            Process.Start(@"C:\Users\Public\Forms\RECEIPT.pdf");

        }
        private void button6_Click(object sender, EventArgs e)
        {
            //string user = label15.Text;
            PaymentRecord pr = new PaymentRecord();
            //pr.label15.Text = user;
            pr.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel2.Show();
            panel3.Hide();
            panel8.Hide();

            button16.Hide();
            button17.Hide();

            button1.Textcolor = System.Drawing.Color.FromArgb(4, 180, 253);
            button2.Textcolor = System.Drawing.Color.White;
            button7.Textcolor = System.Drawing.Color.White;

            slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Top;
            slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Height;
        }
        public int GetBillingNo()
        {
            int billingno = 0;
            try
            {
                connection.Open();
                string query = "SELECT * from billingtbl order by Billing_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    billingno = dataReader.GetInt32("Billing_No");
                }
                billingno = billingno + 1;
                connection.Close();
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
            }
            return billingno;
        }
        public int GetPaymentNo()
        {
            int paymentno = 0;
            try
            {
                connection.Open();
                string query = "SELECT Payment_No from paymenttbl order by Payment_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    paymentno = dataReader.GetInt32("Payment_No");
                }
                paymentno = paymentno + 1;
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            return paymentno;
        }
        public void GetPatientNo(string patient)
        {
            int patientno = 0;
            try
            {
                connection.Open();
                string query = "SELECT *,CONCAT(Patient_LName,', ', Patient_FName,' ',Patient_MidInit) from patienttbl where CONCAT(Patient_LName,', ', Patient_FName,' ',Patient_MidInit) = '" + patient + "'";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    patientno = dataReader.GetInt32("Patient_No");
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
            label13.Text = patientno.ToString();
        }
        public int GetInventorySubtractNo()
        {
            int inventsub = 0;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from inventory_subtracttbl order by Inventory_Subtract_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    inventsub = dataReader.GetInt32("Inventory_Subtract_No");
                }
                inventsub = inventsub + 1;
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            return inventsub;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString();
            path = path.Replace("\\", "/");
            string containsLetter = @"[A-Za-z~!@#$%^&*()_+=-]", mop = "", receiptcode = null;
            string patient = "";
            decimal totalbill = 0, payment = 0, change = 0;
            int paymentno = 0;
            string itemname = "";
            decimal balance = 0, servicefee = 0, itemfee = 0, totalfee = 0;
            string patientno = null, serviceno = null, product_typeno = null, reference = "";
            bool check = false, none = false;
            string date = DateTime.Today.ToString("yyyy-MM-dd");
            int billingno = GetBillingNo();
            int receiptno = GetReceiptNo();
            int qty = 0;
            string added_no = null;
            mop = bunifuDropdown1.selectedValue.ToString();
            try
            {
                patient = comboBox3.Text;
            }
            catch (Exception)
            {
                patient = "";
                patientno = null;
            }
            paymentno = GetPaymentNo();

            try
            {
                totalbill = Convert.ToDecimal(lblTotal.Text);
                change = Convert.ToDecimal(lblChange.Text);
            }
            catch (Exception)
            {

            }

            if (mop == "Cash")
            {
                try
                {
                    payment = Convert.ToDecimal(txtPayment.Text);

                    for (int j = 0; j < dataGridView1.Rows.Count; j++)
                    {
                        if (dataGridView1.Rows[j].Cells[1].Value.ToString().Equals("Service"))
                        {
                            none = true;
                            break;
                        }
                    }
                    if (none)
                    {
                        for (int m = 0; m < dataGridView1.Rows.Count; m++)
                        {
                            if (dataGridView1.Rows[m].Cells[1].Value.ToString().Equals("Service"))
                            {
                                servicefee += Convert.ToDecimal(dataGridView1.Rows[m].Cells[2].Value);
                            }
                            else
                            {
                                itemfee += Convert.ToDecimal(dataGridView1.Rows[m].Cells[2].Value) * Convert.ToInt32(dataGridView1.Rows[m].Cells[3].Value);
                            }
                        }
                        totalfee = (servicefee / 2) + itemfee;
                        if (payment < totalfee)
                        {
                            label54.Text = "Insufficient payment";
                            check = true;
                        }
                        else
                        {
                            label54.Text = "";

                        }
                        if (comboBox3.Text == "")
                        {
                            errorProvider.SetError(comboBox3, "Please select patient");
                            check = true;
                        }
                        else
                        {
                            errorProvider.SetError(comboBox3, string.Empty);
                        }
                    }
                    else
                    {
                        errorProvider.SetError(comboBox3, string.Empty);
                        if (payment < totalbill)
                        {
                            label54.Text = "Insufficient payment";
                            check = true;
                        }
                        else
                        {
                            label54.Text = "";

                        }
                    }

                }
                catch (Exception)
                {
                    payment = 0;
                }
                if (txtPayment.Text.Trim().Length == 0)
                {
                    label54.Text = "Required payment please settle it as soon as possible";
                    check = true;
                }
                else
                {
                    if (payment < balance)
                    {
                        label54.Text = "Insufficient payment, please settle it as soon as possible";
                        check = true;
                    }
                    else
                    {
                        label54.Text = "";
                    }
                }
            }

            else
            {
                try
                {
                    receiptcode = txtReceipt.Text.Trim();
                    if (txtReceipt.Text.Trim().Length == 0)
                    {
                        label54.Text = "Required Receipt Number";
                        check = true;
                    }
                    else
                    {
                        if (Regex.IsMatch(receiptcode, containsLetter))
                        {
                            label54.Text = "Invalid receipt number";
                            check = true;
                        }
                    }
                }
                catch (Exception)
                {
                    label54.Text = "Invalid receipt number";
                    check = true;
                }
                payment = totalbill;
            }
            balance = totalbill - payment;
            if (balance <= 0)
            {
                balance = 0;
            }
            if (dataGridView1.Rows.Count == 0)
            {
                label30.Text = "Please select items you want to avail";
                check = true;
            }
            else
            {
                label30.Text = "";
            }

            patientno =label13.Text;
            if (check == false)
            {
                reference = "Payment for purchased item";
                GenerateReceipt(reference, receiptno);
                try
                {
                    connection.Open();
                    string query3 = "INSERT into billingtbl values (@billingno,@date,@mop,@totalbill,@balance,@patientno)";
                    MySqlCommand cmd3 = new MySqlCommand(query3, connection);
                    cmd3.Parameters.AddWithValue("@billingno", billingno);
                    cmd3.Parameters.AddWithValue("@date", date);
                    cmd3.Parameters.AddWithValue("@mop", mop);
                    cmd3.Parameters.AddWithValue("@totalbill", totalbill);
                    cmd3.Parameters.AddWithValue("@balance", balance);
                    cmd3.Parameters.AddWithValue("@patientno", patientno);
                    cmd3.ExecuteNonQuery();
                    connection.Close();

                    for (int j = 0; j < dataGridView1.Rows.Count; j++)
                    {
                        int billitems = 0,newqty= 0;
                        int inventoryno = 0, prevqty = 0,inventsub = GetInventorySubtractNo();
                        qty = Convert.ToInt32(dataGridView1.Rows[j].Cells[3].Value);
                        itemname = dataGridView1.Rows[j].Cells[0].Value.ToString();
                        string prodtype = dataGridView1.Rows[j].Cells[1].Value.ToString();
                        connection.Open();
                        string query1 = "SELECT * from producttbl p, product_typetbl pt, product_prodtypetbl ppt where p.Product_Name = '" + itemname + "' and pt.Product_Type = '" + prodtype + "' and p.Product_No = ppt.Product_No and pt.Product_Type_No = ppt.Product_Type_No";
                        MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                        MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                        while (dataReader1.Read())
                        {
                            product_typeno = dataReader1.GetInt32("Product_ProdType_No").ToString();
                        }
                        connection.Close();

                        connection.Open();
                        string query2 = "SELECT * from servicetbl where Service_Name = '" + itemname + "'";
                        MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                        MySqlDataReader dataReader2 = cmd2.ExecuteReader();
                        while (dataReader2.Read())
                        {
                            serviceno = dataReader2.GetInt32("Service_No").ToString();
                        }
                        connection.Close();

                        connection.Open();
                        string query4 = "SELECT * from billing_itemstbl order by Billing_ItemNo";
                        MySqlCommand cmd4 = new MySqlCommand(query4, connection);
                        MySqlDataReader dataReader4 = cmd4.ExecuteReader();
                        while (dataReader4.Read())
                        {
                            billitems = dataReader4.GetInt32("Billing_ItemNo");
                        }
                        billitems = billitems + 1;
                        connection.Close();

                        connection.Open();
                        string query5 = "INSERT into billing_itemstbl values (@billitems,@billingno,@product_typeno,@serviceno,@quantity)";
                        MySqlCommand cmd5 = new MySqlCommand(query5, connection);
                        cmd5.Parameters.AddWithValue("@billitems", billitems);
                        cmd5.Parameters.AddWithValue("@billingno", billingno);
                        cmd5.Parameters.AddWithValue("@product_typeno", product_typeno);
                        cmd5.Parameters.AddWithValue("@serviceno", serviceno);
                        cmd5.Parameters.AddWithValue("@quantity", qty);
                        cmd5.ExecuteNonQuery();
                        connection.Close();

                        connection.Open();
                        MySqlCommand cmd8 = new MySqlCommand("SELECT * from product_inventorytbl where Product_ProdType_No = '" + product_typeno + "'", connection);
                        MySqlDataReader dataReader8 = cmd8.ExecuteReader();
                        while (dataReader8.Read())
                        {
                            inventoryno = dataReader8.GetInt32("Inventory_No");
                            prevqty = dataReader8.GetInt32("Total_Quantity");
                        }
                        connection.Close();

                        newqty = prevqty - qty;

                        connection.Open();
                        MySqlCommand cmd10 = new MySqlCommand("UPDATE product_inventorytbl set Total_Quantity = '" + newqty + "' where Inventory_No = '" + inventoryno + "'", connection);
                        cmd10.ExecuteNonQuery();
                        connection.Close();

                        connection.Open();
                        MySqlCommand cmd9 = new MySqlCommand("INSERT into inventory_subtracttbl values ('" + inventsub + "','" + date + "','" + inventoryno + "','" + prevqty + "','" + qty + "','Purchased','"+Convert.ToInt32(added_no)+"')", connection);
                        cmd9.ExecuteNonQuery();
                        connection.Close();

                    }
                    connection.Open();
                    MySqlCommand cmd7 = new MySqlCommand("Insert into receipttbl values ('" + receiptno + "','" + billingno + "','" + receiptcode + "')", connection);
                    cmd7.ExecuteNonQuery();
                    connection.Close();
                    connection.Open();
                    string query6 = "INSERT into paymenttbl values ('" + paymentno + "','" + payment + "','" + change + "','" + date + "','" + billingno + "')";
                    MySqlCommand cmd6 = new MySqlCommand(query6, connection);
                    cmd6.ExecuteNonQuery();
                    connection.Close();

                    MessageBox.Show("Payment Succesful");

                    GetItems();
                    numericUpDown1.Value = 1;
                    dataGridView1.Rows.Clear();
                    lblTotal.Text = "0.00";
                    txtPayment.Text = "";
                    lblChange.Text = "0.00";
                    comboBox3.Text = "";
                    GetBalances();
                    panel12.Hide();
                    //panel10.Enabled = true;
                    panel5.Enabled = true;
                    button15.BringToFront();
                    btnFirstPay.Show();
                    btnSecondPay.Hide();
                    comboBox5.Enabled = true;
                    comboBox2.Enabled = true;
                    comboBox3.Enabled = true;
                    numericUpDown1.Enabled = true;
                    button10.Visible = true;
                    button13.Visible = true;
                    btnFromAppointment.Hide();
                    txtPayment.Enabled = false;
                    panel8.Show();
                    panel3.Hide();
                    panel2.Hide();
                    GetPatientPayments();

                    button16.Visible = true;
                    button17.Visible = true;

                    button7.Textcolor = System.Drawing.Color.FromArgb(4, 180, 253);
                    button2.Textcolor = System.Drawing.Color.White;
                    button1.Textcolor = System.Drawing.Color.White;

                    slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)button7).Top;
                    slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)button7).Height;
                    Process.Start(path + "/SMPIWBC/Receipts/Receipt" + receiptno + ".pdf");
                }
                catch (Exception me)
                {
                    MessageBox.Show(me.Message);
                }
            }
        }
        ErrorProvider errorProvider = new ErrorProvider();
        private void button10_Click(object sender, EventArgs e)
        {
            string prod = comboBox2.Text;
            string prodtype = comboBox5.Text;
            int qty = 0, totalquantity = 0, checkqty = 0, ctr = dataGridView1.Rows.Count;
            decimal fee = 0;
            decimal total1 = 0;
            decimal total = 0;
            decimal subtotal = 0;
            try
            {
                total = Convert.ToDecimal(lblTotal.Text);
            }
            catch (Exception)
            {
                total = 0;
            }
            bool check = false, exists = false;
            connection.Open();
            string query1 = "SELECT * from producttbl p, product_typetbl pt, product_prodtypetbl ppt where p.Product_Name = '" + prod + "' and pt.Product_Type = '" + prodtype + "' and p.Product_No = ppt.Product_No and pt.Product_Type_No = ppt.Product_Type_No";
            MySqlCommand cmd1 = new MySqlCommand(query1, connection);
            MySqlDataReader dataReader1 = cmd1.ExecuteReader();
            while (dataReader1.Read())
            {
                fee = dataReader1.GetDecimal("Product_Fee");
            }
            connection.Close();
            if (comboBox2.Text == "No available")
            {
                check = true;
                errorProvider.SetError(comboBox2, "No available product");
            }
            else
            {
                errorProvider.SetError(comboBox2, string.Empty);
            }
            if (string.IsNullOrEmpty(numericUpDown1.Text))
            {
                errorProvider.SetError(numericUpDown1, "Quantity is required");
                check = true;
            }
            else
            {
                qty = Convert.ToInt32(numericUpDown1.Value);
                if (qty == 0)
                {
                    errorProvider.SetError(numericUpDown1, "No stock available");
                    check = true;
                }
                else
                {
                    errorProvider.SetError(numericUpDown1, string.Empty);
                }
            }
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (prod == dataGridView1.Rows[i].Cells[0].Value.ToString() && prodtype == dataGridView1.Rows[i].Cells[1].Value.ToString())
                {
                    exists = true;
                    checkqty = Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value);
                    totalquantity = checkqty + qty;
                    dataGridView1.Rows[i].Cells[3].Value = totalquantity;
                    subtotal = Convert.ToDecimal(Convert.ToDecimal(dataGridView1.Rows[i].Cells[2].Value) * totalquantity);
                    dataGridView1.Rows[i].Cells[5].Value = subtotal;
                    check = true;
                    break;
                }

            }

            if (exists == false)
            {
                totalquantity = totalquantity + qty;
                subtotal = fee * totalquantity;
                total1 = fee * totalquantity;
                total += total1;
                lblTotal.Text = total.ToString();
            }
            else if (exists && ctr == 1)
            {
                total1 = fee * totalquantity;
                lblTotal.Text = total1.ToString();
            }
            else if (exists == false && ctr > 1)
            {
                total1 = fee * qty;
                total += total1;
                lblTotal.Text = total.ToString();
            }
            else if (exists && ctr > 1)
            {
                total1 = fee * qty;
                total += total1;
                lblTotal.Text = total.ToString();
            }
            if (check == false)
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * from producttbl p, product_typetbl pt, product_prodtypetbl ppt where p.Product_Name = '" + prod + "' and pt.Product_Type = '" + prodtype + "' and p.Product_No = ppt.Product_No and pt.Product_Type_No = ppt.Product_Type_No";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        dataGridView1.Rows.Add(dataReader.GetString("Product_Name"), dataReader.GetString("Product_Type"), dataReader.GetDecimal("Product_Fee"), totalquantity.ToString(),"0",subtotal);
                    }
                    connection.Close();
                }
                catch (MySqlException me)
                {
                    MessageBox.Show(me.Message);
                }
                button15.Enabled = true;
                label54.Text = "";
                label30.Text = "";
            }


        }
        public void GetMaximumPerProduct()
        {
            string product = comboBox2.Text;
            string prodtype = comboBox5.Text;
            int max = 0;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from product_inventorytbl pi, producttbl p, product_typetbl pt, product_prodtypetbl ppt where p.Product_Name = '" + product + "' and pt.Product_Type = '" + prodtype + "' and p.Product_No = ppt.Product_No and pt.Product_Type_No = ppt.Product_Type_No and ppt.Product_ProdType_No = pi.Product_ProdType_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    max = dataReader.GetInt32("Total_Quantity");
                }
                connection.Close();
                numericUpDown1.Maximum = max;
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }
        private void button13_Click(object sender, EventArgs e)
        {
            int qty = Convert.ToInt32(numericUpDown1.Value);

            decimal removed = 0;
            decimal total = 0;
            errorProvider.SetError(comboBox4, string.Empty);
            errorProvider.SetError(comboBox2, string.Empty);
            try
            {
                total = Convert.ToDecimal(lblTotal.Text);
            }
            catch (Exception)
            {
                total = 0;
            }
            try
            {
                int rows = dataGridView1.CurrentRow.Index;
                int totalquantity = Convert.ToInt32(dataGridView1.Rows[rows].Cells[3].Value);

                try
                {

                    if (qty > totalquantity)
                    {
                        qty = totalquantity;
                    }
                    removed = Convert.ToDecimal(dataGridView1.Rows[rows].Cells[2].Value) * qty;
                    total = total - removed;
                    totalquantity -= qty;
                    dataGridView1.Rows[rows].Cells[3].Value = totalquantity;
                    if (totalquantity <= 0)
                    {
                        dataGridView1.Rows.RemoveAt(rows);
                        button15.Enabled = false;
                    }
                }
                catch (Exception)
                {
                }
                lblTotal.Text = total.ToString();
            }
            catch (Exception ne)
            {
                MessageBox.Show("No selected row");
            }

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            string containsLetter = @"[A-Za-z~!@#$%^&*()_+=-]";
            decimal payment = 0, change = 0, total = 0, servicefee = 0, itemfee = 0, totalfee = 0;
            bool check = false;

            for (int j = 0; j < dataGridView1.Rows.Count; j++)
            {
                if (dataGridView1.Rows[j].Cells[1].Value.ToString().Equals("Service"))
                {
                    check = true;
                    break;
                }
            }

            try
            {
                total = Convert.ToDecimal(lblTotal.Text);

            }
            catch (Exception)
            {
                total = 0;

            }
            try
            {
                payment = Convert.ToDecimal(txtPayment.Text);

            }
            catch (Exception)
            {
            }
            if (Regex.IsMatch(txtPayment.Text.ToString(), containsLetter))
            {
                txtPayment.Text = "";
            }
            if (check)
            {
                for (int m = 0; m < dataGridView1.Rows.Count; m++)
                {
                    if (dataGridView1.Rows[m].Cells[1].Value.ToString().Equals("Service"))
                    {
                        servicefee += Convert.ToDecimal(dataGridView1.Rows[m].Cells[2].Value);
                    }
                    else
                    {
                        itemfee += Convert.ToDecimal(dataGridView1.Rows[m].Cells[2].Value) * Convert.ToInt32(dataGridView1.Rows[m].Cells[3].Value);
                    }
                }
                totalfee = (servicefee / 2) + itemfee;
                if (payment < totalfee)
                {
                    errorProvider.SetError(txtPayment, "Insufficient payment");
                }
                else
                {
                    errorProvider.SetError(txtPayment, string.Empty);
                    change = payment - total;
                    if (change < 0)
                    {
                        change = 0;
                    }
                }
            }
            else
            {
                if (payment < total)
                {
                    errorProvider.SetError(txtPayment, "Insufficient payment");
                }
                else
                {
                    errorProvider.SetError(txtPayment, string.Empty);
                    change = payment - total;
                    if (change < 0)
                    {
                        change = 0;
                    }
                }
            }

            lblChange.Text = change.ToString();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        private void button8_Click(object sender, EventArgs e)
        {
            
        }

        private void button11_Click(object sender, EventArgs e)
        {
            panel3.Show();
            panel2.Hide();
            panel8.Hide();
            button15.BringToFront();
            btnSecondPay.Show();
            btnFirstPay.Hide();
            btnFromAppointment.Hide();
            int row = dataGridView2.CurrentCell.RowIndex;
            int billingno = 0;
            billingno = Convert.ToInt32(dataGridView2.Rows[row].Cells[0].Value);
            label12.Text = billingno.ToString();
            decimal balance = Convert.ToDecimal(dataGridView2.Rows[row].Cells[4].Value);
            string patient = dataGridView2.Rows[row].Cells[2].Value.ToString();
            dataGridView1.Rows.Add("Balance", "balance", balance, "","",balance);
            comboBox3.Enabled = false;
            comboBox3.Text = patient;
            lblTotal.Text = balance.ToString();
            button15.Enabled = true;
            label6.Text = "Pay Balance";
            label31.Visible = false;
            comboBox6.Visible = false;
            button10.Visible = false;
            button13.Visible = false;
            button14.Visible = false;
            comboBox2.Enabled = false;
            comboBox5.Enabled = false;
            numericUpDown1.Enabled = false;
            txtPayment.Enabled = true;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString();
            path = path.Replace("\\", "/");
            string containsLetter = @"[A-Za-z~!@#$%^&*()_+=-]";
            int receiptno = GetReceiptNo();
            int billingno = GetBillingNo();
            int paymentno = GetPaymentNo();
            int currbillingno = Convert.ToInt32(label12.Text);
            string receiptcode = null, mop = "", reference = "";
            decimal balance = Convert.ToDecimal(lblTotal.Text);
            decimal bal = 0;
            string patient = comboBox3.Text;
            string patientno = label13.Text;
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            decimal payment = 0, change = 0;

            try
            {
                change = Convert.ToDecimal(lblChange.Text);
            }
            catch (Exception)
            {
                change = 0;
            }
            bool check = false;
            mop = bunifuDropdown1.selectedValue.ToString();
            if (mop == "Cash")
            {
                try
                {
                    payment = Convert.ToDecimal(txtPayment.Text.Trim());
                    if (txtPayment.Text.Trim().Length == 0)
                    {
                        label54.Text = "Required payment please settle it as soon as possible";
                        check = true;
                    }
                    else
                    {
                        if (payment < balance)
                        {
                            label54.Text = "Insufficient payment, please settle it as soon as possible";
                            check = true;
                        }
                        else
                        {
                            label54.Text = "";
                        }
                    }
                }
                catch (Exception)
                {
                    payment = 0;
                }
            }
            else
            {
                try
                {
                    receiptcode = txtReceipt.Text.Trim();
                    if (txtReceipt.Text.Trim().Length == 0)
                    {
                        label54.Text = "Required Receipt Number";
                        check = true;
                    }
                    else
                    {
                        if (Regex.IsMatch(receiptcode, containsLetter))
                        {
                            label54.Text = "Invalid receipt number";
                            check = true;
                        }
                    }
                }
                catch (Exception)
                {
                    label54.Text = "Invalid receipt number";
                    check = true;
                }
            }
            if (check == false)
            {
                reference = "Payment for Balance";
                GenerateReceipt(reference, receiptno);
                bal = balance - payment;
                if (bal <= 0)
                {
                    bal = 0;
                }
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO billingtbl values (@billingno,@billdate,@mop,@totalbill,@balance,@patientno)", connection);
                    cmd.Parameters.AddWithValue("@billingno", billingno);
                    cmd.Parameters.AddWithValue("@billdate", date);
                    cmd.Parameters.AddWithValue("@mop", mop);
                    cmd.Parameters.AddWithValue("@totalbill", balance);
                    cmd.Parameters.AddWithValue("@balance", bal);
                    cmd.Parameters.AddWithValue("@patientno", patientno);
                    cmd.ExecuteNonQuery();
                    connection.Close();

                    connection.Open();
                    MySqlCommand cmd1 = new MySqlCommand("INSERT INTO paymenttbl values(@paymentno,@payment,@change,@date,@billingno)", connection);
                    cmd1.Parameters.AddWithValue("@paymentno", paymentno);
                    cmd1.Parameters.AddWithValue("@payment", payment);
                    cmd1.Parameters.AddWithValue("@change", change);
                    cmd1.Parameters.AddWithValue("@date", date);
                    cmd1.Parameters.AddWithValue("@billingno", billingno);
                    cmd1.ExecuteNonQuery();
                    connection.Close();

                    if (bal <= 0)
                    {
                        connection.Open();
                        MySqlCommand cmd4 = new MySqlCommand("UPDATE appointment_payment set Payment_Status = 'Paid' where Billing_No = '"+currbillingno+"'", connection);
                        cmd4.ExecuteNonQuery();
                        connection.Close();
                    }

                    connection.Open();
                    MySqlCommand cmd2 = new MySqlCommand("Insert into receipttbl values ('" + receiptno + "','" + billingno + "','" + receiptcode + "')", connection);
                    cmd2.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception me)
                {
                    MessageBox.Show(me.Message);
                }
                MessageBox.Show("Payment Successful!");

                lblTotal.Text = "0.00";
                txtPayment.Text = "";
                lblChange.Text = "0.00";
                comboBox3.Text = "";
                GetBalances();
                GetPatients();
                panel12.Hide();
                //panel10.Enabled = true;
                panel5.Enabled = true;
                button15.BringToFront();
                btnFirstPay.Show();
                btnSecondPay.Hide();
                comboBox5.Enabled = true;
                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
                numericUpDown1.Enabled = true;
                button10.Visible = true;
                button13.Visible = true;
                btnFromAppointment.Hide();
                txtPayment.Enabled = false;
                panel8.Show();
                panel3.Hide();
                panel2.Hide();
                GetPatientPayments();

                button16.Visible = true;
                button17.Visible = true;

                button7.Textcolor = System.Drawing.Color.FromArgb(4, 180, 253);
                button2.Textcolor = System.Drawing.Color.White;
                button1.Textcolor = System.Drawing.Color.White;

                slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)button7).Top;
                slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)button7).Height;
                Process.Start(path + "/SMPIWBC/Receipts/Receipt" + receiptno + ".pdf");
            }
        }

        private void comboBox6_SelectedValueChanged(object sender, EventArgs e)
        {
            string item = comboBox6.Text;
            if (item.Equals("Products"))
            {
                label20.Visible = true;
                comboBox2.Visible = true;
                button10.Visible = true;
                button13.Visible = true;
                label29.Visible = true;
                comboBox5.Visible = true;
                label22.Visible = true;
                numericUpDown1.Visible = true;
                comboBox4.Visible = false;
                button14.Visible = false;
                label7.Visible = false;
            }
            else
            {
                label20.Visible = false;
                comboBox2.Visible = false;
                button10.Visible = false;
                button13.Visible = true;
                label29.Visible = false;
                comboBox5.Visible = false;
                label22.Visible = false;
                numericUpDown1.Visible = false;
                comboBox4.Visible = true;
                button14.Visible = true;
                label7.Visible = true;
            }
        }

        private void label28_Click(object sender, EventArgs e)
        {
            panel12.Hide();
            lblTotal2.Text = "0.00";
            txtPayment.Text = "";
            lblChange.Text = "0.00";
            //panel10.Enabled = true;
            panel5.Enabled = true;
            flowLayoutPanel1.Controls.Clear();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            panel12.Visible = false;
            revTransition.ShowSync(panel12);
            panel12.BringToFront();
            decimal total = Convert.ToDecimal(lblTotal.Text);
            lblTotal2.Text = total.ToString();
            int rows = dataGridView1.Rows.Count;
            int y = 8;
            bool check = false;
            string type = comboBox6.Text;
            if (type == "Services")
            {
                if (string.IsNullOrEmpty(comboBox3.Text))
                {
                    errorProvider.SetError(comboBox3, "Please select patient");
                    check = true;
                }
            }
            if (check == false)
            {

                for (int j = 0; j < rows; j++)
                {
                    Label lblitemname = new Label();
                    Label lblitemtype = new Label();
                    Label lblitemfee = new Label();
                    Label lblitemqty = new Label();

                    lblitemname.Font = new System.Drawing.Font("Century Gothic", 9, FontStyle.Regular);
                    lblitemtype.Font = new System.Drawing.Font("Century Gothic", 9, FontStyle.Regular);
                    lblitemfee.Font = new System.Drawing.Font("Century Gothic", 9, FontStyle.Regular);
                    lblitemqty.Font = new System.Drawing.Font("Century Gothic", 9, FontStyle.Regular);

                    lblitemname.Width = 165;
                    lblitemtype.Width = 140;
                    lblitemfee.Width = 110;
                    lblitemqty.Width = 50;

                    lblitemqty.TextAlign = ContentAlignment.MiddleCenter;

                    lblitemname.Margin = new Padding(7, y, 0, 0);
                    lblitemtype.Margin = new Padding(0, y, 0, 0);
                    lblitemfee.Margin = new Padding(0, y, 0, 0);
                    lblitemqty.Margin = new Padding(0, y, 0, 0);

                    lblitemname.Text = dataGridView1.Rows[j].Cells[0].Value.ToString();
                    lblitemtype.Text = dataGridView1.Rows[j].Cells[1].Value.ToString();
                    lblitemfee.Text = dataGridView1.Rows[j].Cells[2].Value.ToString();
                    lblitemqty.Text = dataGridView1.Rows[j].Cells[3].Value.ToString();

                    flowLayoutPanel1.Controls.Add(lblitemname);
                    flowLayoutPanel1.Controls.Add(lblitemtype);
                    flowLayoutPanel1.Controls.Add(lblitemfee);
                    flowLayoutPanel1.Controls.Add(lblitemqty);

                }
            }
               
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            txtPayment.Enabled = false;
            //panel10.Enabled = true;
            dataGridView1.Enabled = true;
            button15.Enabled = true;
            comboBox3.Enabled = true;
            comboBox2.Enabled = true;
            comboBox5.Enabled = true;
            txtPayment.Enabled = true;

            panel12.Hide();
            lblTotal2.Text = "0.00";
            txtPayment.Text = "";
            lblChange.Text = "0.00";
            //panel10.Enabled = true;
            panel5.Enabled = true;
            flowLayoutPanel1.Controls.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel12.Hide();
            lblTotal2.Text = "0.00";
            txtPayment.Text = "";
            lblChange.Text = "0.00";
            //panel10.Enabled = true;
            panel5.Enabled = true;
            flowLayoutPanel1.Controls.Clear();
            txtPayment.Enabled = true;
            button15.SendToBack();
            button15.Visible = false;
            bunifuDropdown1.Enabled = true;
            txtReceipt.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel3.Show();
            panel2.Hide();
            panel8.Hide();
            button16.Hide();
            button17.Hide();
            comboBox6.Enabled = false;
            button2.Textcolor = System.Drawing.Color.FromArgb(4, 180, 253);
            button1.Textcolor = System.Drawing.Color.White;
            button7.Textcolor = System.Drawing.Color.White;
            dataGridView1.BringToFront();
            dataGridView1.Show();
            slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Top;
            slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Height;
        }

        private void button7_Click(object sender, EventArgs e)
        {

            panel8.Show();
            panel3.Hide();
            panel2.Hide();
            GetPatientPayments();

            button16.Visible = true;
            button17.Visible = true;

            button7.Textcolor = System.Drawing.Color.FromArgb(4, 180, 253);
            button2.Textcolor = System.Drawing.Color.White;
            button1.Textcolor = System.Drawing.Color.White;

            slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Top;
            slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Height;
        }

        public void GetPatientPayments()
        {
            dataGridView3.Rows.Clear();
            string patient = "";
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT *,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from patienttbl p, billingtbl b, paymenttbl py where b.Patient_No = p.Patient_No and b.Billing_No = py.Billing_No order by b.Billing_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    patient = dataReader.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)");

                    dataGridView3.Rows.Add(dataReader.GetInt32("Billing_No"), patient, dataReader.GetDateTime("Billing_Date").ToString("MM-dd-yyyy"), dataReader.GetDecimal("Total_Bill"), dataReader.GetDecimal("Amount_Paid"), dataReader.GetDecimal("Change"));
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
            dataGridView3.Rows.Clear();
            string patient = "";
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from billingtbl b, paymenttbl py where b.Patient_No IS NULL and b.Billing_No = py.Billing_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView3.Rows.Add(dataReader.GetInt32("Billing_No"), "-", dataReader.GetDateTime("Billing_Date").ToString("MM-dd-yyyy"), dataReader.GetDecimal("Total_Bill"), dataReader.GetDecimal("Amount_Paid"), dataReader.GetDecimal("Change"));
                }
                connection.Close();
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
            }

        }

        private void button16_Click(object sender, EventArgs e)
        {
            button16.IdleFillColor = System.Drawing.Color.FromArgb(4, 91, 188);
            button16.IdleForecolor = System.Drawing.Color.White;

            button17.IdleFillColor = System.Drawing.Color.White;
            button17.IdleLineColor = System.Drawing.Color.FromArgb(4, 91, 188);
            button17.IdleForecolor = System.Drawing.Color.FromArgb(4, 91, 188);
            textBox1.Show();
            label10.Show();
            GetPatientPayments();
            label52.Show();
            //button15.BackColor = Color.Transparent;
            //button14.BackColor = Color.Silver;

        }

        private void button17_Click(object sender, EventArgs e)
        {
            button17.IdleFillColor = System.Drawing.Color.FromArgb(4, 91, 188);
            button17.IdleForecolor = System.Drawing.Color.White;

            button16.IdleFillColor = System.Drawing.Color.White;
            button16.IdleLineColor = System.Drawing.Color.FromArgb(4, 91, 188);
            button16.IdleForecolor = System.Drawing.Color.FromArgb(4, 91, 188);
            textBox1.Hide();
            label52.Hide();
            GetWalkInPayments();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            panel9.Hide();
            button16.Enabled = true;
            button17.Enabled = true;
            dataGridView3.Enabled = true;
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
        }

        private void dataGridView3_DoubleClick(object sender, EventArgs e)
        {
            panel9.Visible = false;
            histTransition.ShowSync(panel9);
            int row = dataGridView3.CurrentCell.RowIndex;
            int billingno = Convert.ToInt32(dataGridView3.Rows[row].Cells[0].Value);
            int y = 8;
            bool check = false;
            label5.Text = "";
            label2.Text = "";
            label1.Text = "";
            flowLayoutPanel2.Controls.Clear();
            label36.Text = "0.00";
            label16.Text = "0.00";
            label35.Text = "0.00";
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

                    lblitemname.Font = new System.Drawing.Font("Century Gothic", 9, FontStyle.Regular);
                    lblitemtype.Font = new System.Drawing.Font("Century Gothic", 9, FontStyle.Regular);
                    lblitemfee.Font = new System.Drawing.Font("Century Gothic", 9, FontStyle.Regular);
                    lblitemqty.Font = new System.Drawing.Font("Century Gothic", 9, FontStyle.Regular);

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

                    flowLayoutPanel2.Controls.Add(lblitemname);
                    flowLayoutPanel2.Controls.Add(lblitemtype);
                    flowLayoutPanel2.Controls.Add(lblitemfee);
                    flowLayoutPanel2.Controls.Add(lblitemqty);
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

                    lblitemname1.Font = new System.Drawing.Font("Century Gothic", 9, FontStyle.Regular);
                    lblitemtype1.Font = new System.Drawing.Font("Century Gothic", 9, FontStyle.Regular);
                    lblitemfee1.Font = new System.Drawing.Font("Century Gothic", 9, FontStyle.Regular);
                    lblitemqty1.Font = new System.Drawing.Font("Century Gothic", 9, FontStyle.Regular);

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

                    flowLayoutPanel2.Controls.Add(lblitemname1);
                    flowLayoutPanel2.Controls.Add(lblitemtype1);
                    flowLayoutPanel2.Controls.Add(lblitemfee1);
                    flowLayoutPanel2.Controls.Add(lblitemqty1);
                }
                connection.Close();

                if (check == false)
                {
                    Label lblitemname1 = new Label();
                    Label lblitemtype1 = new Label();
                    Label lblitemfee1 = new Label();
                    Label lblitemqty1 = new Label();

                    lblitemname1.Font = new System.Drawing.Font("Century Gothic", 9, FontStyle.Regular);
                    lblitemtype1.Font = new System.Drawing.Font("Century Gothic", 9, FontStyle.Regular);
                    lblitemfee1.Font = new System.Drawing.Font("Century Gothic", 9, FontStyle.Regular);
                    lblitemqty1.Font = new System.Drawing.Font("Century Gothic", 9, FontStyle.Regular);

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
                    lblitemfee1.Text = dataGridView3.Rows[row].Cells[3].Value.ToString();
                    lblitemqty1.Text = "0";

                    flowLayoutPanel2.Controls.Add(lblitemname1);
                    flowLayoutPanel2.Controls.Add(lblitemtype1);
                    flowLayoutPanel2.Controls.Add(lblitemfee1);
                    flowLayoutPanel2.Controls.Add(lblitemqty1);
                }
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
            }
            label5.Text = dataGridView3.Rows[row].Cells[0].Value.ToString();
            label2.Text = dataGridView3.Rows[row].Cells[1].Value.ToString();
            label1.Text = dataGridView3.Rows[row].Cells[2].Value.ToString();
            label36.Text = dataGridView3.Rows[row].Cells[3].Value.ToString();
            label16.Text = dataGridView3.Rows[row].Cells[4].Value.ToString();
            label35.Text = dataGridView3.Rows[row].Cells[5].Value.ToString();
            panel9.Show();
            button16.Enabled = false;
            button17.Enabled = false;
            dataGridView3.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            panel9.Visible = true;
        }
        public int GetAppointPayNo()
        {
            int appointpayno = 0;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from appointment_payment order by Appointment_Payment_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    appointpayno = dataReader.GetInt32("Appointment_Payment_No");
                }
                appointpayno = appointpayno + 1;
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            return appointpayno;
        }
        public int GetReceiptNo()
        {
            int receiptno = 0;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from receipttbl order by Receipt_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    receiptno = dataReader.GetInt32("Receipt_No");
                }
                receiptno = receiptno + 1;
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            return receiptno;
        }
        private void btnFromAppointment_Click(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString();
            path = path.Replace("\\", "/");
            string containsLetter = @"[A-Za-z~!@#$%^&*()_+=-]";
            string patient = "", msgstatus = "", mop = "";
            decimal totalbill = 0, payment = 0, change = 0;
            int receiptno = 0;
            string receiptcode = null;
            int paymentno = 0;
            string itemname = "";
            decimal balance = 0, servicefee = 0, itemfee = 0, totalfee = 0,subtotal = 0;
            string patientno = null, serviceno = null, product_typeno = null, reference = "";
            bool check = false, none = false;
            string date = DateTime.Today.ToString("yyyy-MM-dd");
            int billingno = GetBillingNo();
            int qty = 0;
            string paymentstatus = "";
            int appointno = Convert.ToInt32(label55.Text);
            int appointpay = GetAppointPayNo();
            mop = bunifuDropdown1.selectedValue.ToString();
            receiptno = GetReceiptNo();
            try
            {
                patient = comboBox3.Text;
            }
            catch (Exception)
            {
                patient = "";
                patientno = null;
            }
            paymentno = GetPaymentNo();

            try
            {
                totalbill = Convert.ToDecimal(lblTotal.Text);
                change = Convert.ToDecimal(lblChange.Text);
            }
            catch (Exception)
            {

            }
            if (mop == "Cash")
            {
                try
                {
                    payment = Convert.ToDecimal(txtPayment.Text);

                    for (int j = 0; j < dataGridView1.Rows.Count; j++)
                    {
                        if (dataGridView1.Rows[j].Cells[1].Value.ToString().Equals("Service"))
                        {
                            none = true;
                            break;
                        }
                    }
                    if (none)
                    {
                        for (int m = 0; m < dataGridView1.Rows.Count; m++)
                        {
                            if (dataGridView1.Rows[m].Cells[1].Value.ToString().Equals("Service"))
                            {
                                servicefee += Convert.ToDecimal(dataGridView1.Rows[m].Cells[2].Value);
                                subtotal += Convert.ToDecimal(dataGridView1.Rows[m].Cells[5].Value);
                            }
                            else
                            {
                                itemfee += Convert.ToDecimal(dataGridView1.Rows[m].Cells[2].Value) * Convert.ToInt32(dataGridView1.Rows[m].Cells[3].Value);
                            }
                        }
                        totalfee = (subtotal / 2) + itemfee;
                        if (payment < totalfee)
                        {
                            label54.Text = "Insufficient payment";
                            check = true;
                        }
                        else
                        {
                            label54.Text = "";

                        }
                        if (comboBox3.Text == "")
                        {
                            errorProvider.SetError(comboBox3, "Please select patient");
                            check = true;
                        }
                        else
                        {
                            errorProvider.SetError(comboBox3, string.Empty);
                        }
                    }
                    else
                    {
                        errorProvider.SetError(comboBox3, string.Empty);
                        if (payment < totalbill)
                        {
                            label54.Text = "Insufficient payment";
                            check = true;
                        }
                        else
                        {
                            label54.Text = "";

                        }
                    }

                }
                catch (Exception)
                {
                    payment = 0;
                    label54.Text = "Invalid amount";
                    check = true;
                }
                if (txtPayment.Text.Trim().Length == 0)
                {
                    label54.Text = "Required payment please settle it as soon as possible";
                    check = true;
                }
                else
                {
                    if (payment < balance)
                    {
                        label54.Text = "Insufficient payment, please settle it as soon as possible";
                        check = true;
                    }
                    else
                    {
                        label54.Text = "";
                    }
                }

            }
            else
            {
                try
                {
                    receiptcode = txtReceipt.Text.Trim();
                    if (txtReceipt.Text.Trim().Length == 0)
                    {
                        label54.Text = "Required Receipt Number";
                        check = true;
                    }
                    else
                    {
                        if (Regex.IsMatch(receiptcode, containsLetter))
                        {
                            label54.Text = "Invalid receipt number";
                            check = true;
                        }
                    }
                }
                catch (Exception)
                {
                    receiptcode = null;
                    label54.Text = "Invalid receipt number";
                    check = true;
                }
                payment = totalbill;
            }
            balance = totalbill - payment;
            if (balance <= 0)
            {
                balance = 0;
            }

            if (dataGridView1.Rows.Count == 0)
            {
                label30.Text = "Please select items you want to avail";
                check = true;
            }
            else
            {
                label30.Text = "";
            }

            patientno = label13.Text;
            if (check == false)
            {
                reference = "Payment for Service";
                GenerateReceipt(reference, receiptno);
                try
                {
                    connection.Open();
                    string query3 = "INSERT into billingtbl values (@billingno,@date,@mop,@totalbill,@balance,@patientno)";
                    MySqlCommand cmd3 = new MySqlCommand(query3, connection);
                    cmd3.Parameters.AddWithValue("@billingno", billingno);
                    cmd3.Parameters.AddWithValue("@date", date);
                    cmd3.Parameters.AddWithValue("@mop", mop);
                    cmd3.Parameters.AddWithValue("@totalbill", totalbill);
                    cmd3.Parameters.AddWithValue("@balance", balance);
                    cmd3.Parameters.AddWithValue("@patientno", patientno);
                    cmd3.ExecuteNonQuery();
                    connection.Close();

                    for (int j = 0; j < dataGridView1.Rows.Count; j++)
                    {
                        int billitems = 0;
                        try
                        {
                            qty = Convert.ToInt32(dataGridView1.Rows[j].Cells[3].Value);
                        }
                        catch (Exception)
                        {
                            qty = 1;
                        }
                        itemname = dataGridView1.Rows[j].Cells[0].Value.ToString();
                        string prodtype = dataGridView1.Rows[j].Cells[1].Value.ToString();
                        connection.Open();
                        string query1 = "SELECT * from producttbl p, product_typetbl pt, product_prodtypetbl ppt where p.Product_Name = '" + itemname + "' and pt.Product_Type = '" + prodtype + "' and p.Product_No = ppt.Product_No and pt.Product_Type_No = ppt.Product_Type_No";
                        MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                        MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                        while (dataReader1.Read())
                        {
                            product_typeno = dataReader1.GetInt32("Product_ProdType_No").ToString();
                        }
                        connection.Close();

                        connection.Open();
                        string query2 = "SELECT * from servicetbl where Service_Name = '" + itemname + "'";
                        MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                        MySqlDataReader dataReader2 = cmd2.ExecuteReader();
                        while (dataReader2.Read())
                        {
                            serviceno = dataReader2.GetInt32("Service_No").ToString();
                        }
                        connection.Close();

                        connection.Open();
                        string query4 = "SELECT * from billing_itemstbl order by Billing_ItemNo";
                        MySqlCommand cmd4 = new MySqlCommand(query4, connection);
                        MySqlDataReader dataReader4 = cmd4.ExecuteReader();
                        while (dataReader4.Read())
                        {
                            billitems = dataReader4.GetInt32("Billing_ItemNo");
                        }
                        billitems = billitems + 1;
                        connection.Close();

                        connection.Open();
                        string query5 = "INSERT into billing_itemstbl values (@billitems,@billingno,@product_typeno,@serviceno,@quantity)";
                        MySqlCommand cmd5 = new MySqlCommand(query5, connection);
                        cmd5.Parameters.AddWithValue("@billitems", billitems);
                        cmd5.Parameters.AddWithValue("@billingno", billingno);
                        cmd5.Parameters.AddWithValue("@product_typeno", product_typeno);
                        cmd5.Parameters.AddWithValue("@serviceno", serviceno);
                        cmd5.Parameters.AddWithValue("@quantity", qty);
                        cmd5.ExecuteNonQuery();
                        connection.Close();
                    }

                    connection.Open();
                    string query6 = "INSERT into paymenttbl values ('" + paymentno + "','" + payment + "','" + change + "','" + date + "','" + billingno + "')";
                    MySqlCommand cmd6 = new MySqlCommand(query6, connection);
                    cmd6.ExecuteNonQuery();
                    connection.Close();
                    if (balance > 0)
                    {
                        paymentstatus = "Half Paid";
                        msgstatus = " You have balance to settle!";
                    }
                    else if (balance == 0)
                    {
                        paymentstatus = "Paid";
                    }
                    else
                    {
                        paymentstatus = "Not Paid";
                    }
                    connection.Open();
                    MySqlCommand cmd7 = new MySqlCommand("INSERT into appointment_payment values ('" + appointpay + "','" + appointno + "','" + billingno + "','" + paymentstatus + "')", connection);
                    cmd7.ExecuteNonQuery();
                    connection.Close();

                    connection.Open();
                    MySqlCommand cmd8 = new MySqlCommand("Insert into receipttbl values ('" + receiptno + "','" + billingno + "','" + receiptcode + "')", connection);
                    cmd8.ExecuteNonQuery();
                    connection.Close();

                    MessageBox.Show("Payment Succesful" + msgstatus);

                    GetItems();
                    dataGridView1.Rows.Clear();
                    lblTotal.Text = "0.00";
                    txtPayment.Text = "";
                    lblChange.Text = "0.00";
                    comboBox3.Text = "";
                    GetBalances();
                    panel12.Hide();
                    //panel10.Enabled = true;
                    panel5.Enabled = true;
                    button15.BringToFront();
                    btnFirstPay.Show();
                    btnSecondPay.Hide();
                    comboBox5.Enabled = true;
                    comboBox2.Enabled = true;
                    comboBox3.Enabled = true;
                    numericUpDown1.Enabled = true;
                    button10.Visible = true;
                    button13.Visible = true;
                    btnFromAppointment.Hide();
                    txtPayment.Enabled = false;
                    panel8.Show();
                    panel3.Hide();
                    panel2.Hide();
                    GetPatientPayments();

                    button16.Show();
                    button17.Show();

                    button7.Textcolor = System.Drawing.Color.FromArgb(4, 180, 253);
                    button2.Textcolor = System.Drawing.Color.White;
                    button1.Textcolor = System.Drawing.Color.White;

                    slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)button7).Top;
                    slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)button7).Height;
                    Process.Start(path + "/SMPIWBC/Receipts/Receipt" + receiptno + ".pdf");
                }
                catch (Exception me)
                {
                    MessageBox.Show(me.Message);
                }
            }
        }
        public HomePage ParentForm { get; set; }
        public void GenerateReceipt(string reference, int receiptno)
        {
            string datetoday = DateTime.Today.ToString("yyyy-MM-dd"), mop = bunifuDropdown1.selectedValue.ToString();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString();
            path = path.Replace("\\", "/");
            try
            {
                Directory.CreateDirectory(path + "/SMPIWBC/Receipts");
            }
            catch (Exception me) { MessageBox.Show(me.Message); }
            string imageURL = Application.StartupPath + @"\pics\slimlogo.png";
            decimal payment = 0, totalamt = 0, amt = 0, change = 0;
            string item = "", type = "";
            decimal unitprice = 0;
            int qty = 0;
            string customer = "", employee = "";
            customer = comboBox3.Text.Trim();
            if (string.IsNullOrEmpty(customer))
            {
                customer = " - ";
            }
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT *,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from accounttbl a, employeetbl e where a.Username = '" + ParentForm.Username + "' and a.Employee_No = e.Employee_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    employee = dataReader.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)");
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

                totalamt = Convert.ToDecimal(lblTotal.Text);

            }
            catch (Exception)
            {
                totalamt = 0;
            }
            if (mop == "Cash")
            {
                try
                {
                    payment = Convert.ToDecimal(txtPayment.Text);
                }
                catch (Exception)
                {
                    payment = 0;
                }
            }
            else
            {
                payment = totalamt;
            }
            change = payment - totalamt;
            if (change <= 0)
            {
                change = 0;
            }
            var pagesize = new iTextSharp.text.Rectangle(750, 550);
            PdfWriter writer;
            Document doc = new Document(pagesize);
            doc.SetMargins(30f, 10f, 20f, 10f);
            if(File.Exists(path + "/SMPIWBC/Receipts/Receipt" + receiptno + ".pdf"))
            {
                File.Delete(path + "/SMPIWBC/Receipts/Receipt" + receiptno + ".pdf");
            }
            writer = PdfWriter.GetInstance(doc, new FileStream(path + "/SMPIWBC/Receipts/Receipt" + receiptno + ".pdf", FileMode.Create));

            doc.Open();
            Paragraph title = new Paragraph();
            Paragraph textimage = new Paragraph();
            Paragraph text = new Paragraph();
            iTextSharp.text.Font titleFont = FontFactory.GetFont("Century Gothic", 20, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font regFont = FontFactory.GetFont("Century Gothic", 13);
            iTextSharp.text.Font headerFont = FontFactory.GetFont("Century Gothic", 15, iTextSharp.text.Font.BOLD);

            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
            jpg.ScaleToFit(80f, 80f);

            title.Add(new Phrase("SLIMMERS WORLD SKIN AND FACIAL CLINIC", titleFont));
            textimage.Add(new Chunk(jpg, 0, -65));
            textimage.Add(new Phrase("CCB Unit 49-A Lower Ground Main Building", regFont));
            text.Add(new Phrase("SM City North Edsa", regFont));
            text.Add(new Phrase("\nTel. Nos.: (02) 929-5424", regFont));
            text.Add(new Phrase("\nCell. Nos.: 0906-391-1216\n\n\n\n", regFont));
            textimage.IndentationLeft = 140;
            title.Alignment = Element.ALIGN_CENTER;
            text.Alignment = Element.ALIGN_CENTER;
            doc.Add(title);
            doc.Add(textimage);
            doc.Add(text);

            PdfPTable table = new PdfPTable(5);
            table.HorizontalAlignment = Element.ALIGN_LEFT;
            table.WidthPercentage = 95;
            table.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            table.AddCell("Receipt #");
            table.AddCell(receiptno.ToString());
            table.AddCell("");
            table.AddCell("Total Amount:");
            table.AddCell("Php " + totalamt.ToString("n2"));

            table.AddCell("Customer Name: ");
            table.AddCell(customer);
            table.AddCell("");
            table.AddCell("Paid Amount:");
            table.AddCell("Php " + payment.ToString("n2"));

            table.AddCell("Employee: ");
            table.AddCell(employee);
            table.AddCell("");
            table.AddCell("Change:");
            table.AddCell("Php " + change.ToString("n2"));

            table.AddCell("Payment Date:");
            table.AddCell(datetoday);
            table.AddCell("");
            table.AddCell("Reference:");
            table.AddCell(reference);

            table.AddCell("Payment Mode:");
            table.AddCell(mop);
            table.AddCell("");
            table.AddCell("");
            table.AddCell("");

            PdfPCell cellBlank = new PdfPCell(new Phrase(Chunk.NEWLINE));
            cellBlank.Colspan = 5;
            cellBlank.Border = iTextSharp.text.Rectangle.NO_BORDER;
            table.AddCell(cellBlank);

            PdfPCell detailscell = new PdfPCell(new Phrase("Purchase Details", FontFactory.GetFont("Century Gothic", 13, iTextSharp.text.Font.BOLD)));
            detailscell.Colspan = 5;
            detailscell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            table.AddCell(detailscell);

            PdfPCell cellBlank1 = new PdfPCell(new Phrase(Chunk.NEWLINE));
            cellBlank1.Colspan = 5;
            cellBlank1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            table.AddCell(cellBlank1);

            PdfPCell cell = new PdfPCell(new Phrase("Item Name", headerFont));
            cell.HorizontalAlignment = 1;
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Item Type", headerFont));
            cell.HorizontalAlignment = 1;
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Unit Price", headerFont));
            cell.HorizontalAlignment = 1;
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Quantity", headerFont));
            cell.HorizontalAlignment = 1;
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Amount", headerFont));
            cell.HorizontalAlignment = 1;
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            table.AddCell(cell);


            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    item = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    type = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    amt = Convert.ToDecimal(dataGridView1.Rows[i].Cells[5].Value);
                    unitprice = Convert.ToDecimal(dataGridView1.Rows[i].Cells[2].Value);
                    try
                    {
                        qty = Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value);
                    }
                    catch (Exception)
                    {
                        qty = 1;
                    }

                    PdfPCell itemCell = new PdfPCell(new Phrase(item));
                    itemCell.HorizontalAlignment = 1;
                    itemCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(itemCell);

                    itemCell = new PdfPCell(new Phrase(type));
                    itemCell.HorizontalAlignment = 1;
                    itemCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(itemCell);

                    itemCell = new PdfPCell(new Phrase(unitprice.ToString()));
                    itemCell.HorizontalAlignment = 1;
                    itemCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(itemCell);

                    itemCell = new PdfPCell(new Phrase(qty.ToString()));
                    itemCell.HorizontalAlignment = 1;
                    itemCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(itemCell);

                    itemCell = new PdfPCell(new Phrase("Php " + amt.ToString("n2")));
                    itemCell.HorizontalAlignment = 1;
                    itemCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(itemCell);


                }

                PdfPCell blank = new PdfPCell(new Phrase(Chunk.NEWLINE));
                blank.Colspan = 5;
                blank.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(blank);

                table.AddCell("");
                table.AddCell("");
                table.AddCell("");
                PdfPCell totalcell = new PdfPCell(new Phrase("Total:", FontFactory.GetFont("Century Gothic", 12, iTextSharp.text.Font.BOLD)));
                totalcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(totalcell);
                PdfPCell totalamtcell = new PdfPCell(new Phrase("Php " + totalamt.ToString("n2")));
                totalamtcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                totalamtcell.HorizontalAlignment = 1;
                table.AddCell(totalamtcell);
            }


            doc.Add(table);
            doc.Close();
            
        }
        private void label50_Click(object sender, EventArgs e)
        {
            panel9.Hide();
            button16.Enabled = true;
            button17.Enabled = true;
            dataGridView3.Enabled = true;
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            string prodtype = comboBox5.Text;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from product_typetbl pt, producttbl p, product_prodtypetbl ppt, product_inventorytbl pi where pi.Total_Quantity > 0 and pt.Product_Type = '" + prodtype + "' and pt.Product_Type_No = ppt.Product_Type_No and ppt.Product_No = p.Product_No and ppt.Product_ProdType_No = pi.Product_ProdType_No order by ppt.Product_Type_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    comboBox2.Items.Add(dataReader.GetString("Product_Name"));
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
            GetMaximumPerProduct();
        }

        private void txtPayment_KeyUp(object sender, KeyEventArgs e)
        {
            string containsLetter = @"[A-Za-z~!@#$%^&*()_+=-]";
            string cno = txtPayment.Text.Trim();
            if (Regex.IsMatch(cno, containsLetter))
            {
                txtPayment.BackColor = System.Drawing.Color.FromArgb(252, 224, 224);
                label54.Text = "Numeric only";
            }
            else
            {
                label54.Text = "";
                txtPayment.BackColor = System.Drawing.Color.White;
            }
        }

        private void txtPayment_TextChanged(object sender, EventArgs e)
        {
            string containsLetter = @"[A-Za-z~!@#$%^&*()_+=-]";
            decimal payment = 0, change = 0, total = 0, servicefee = 0, itemfee = 0, totalfee = 0, subtotal = 0;
            bool check = false;

            for (int j = 0; j < dataGridView1.Rows.Count; j++)
            {
                if (dataGridView1.Rows[j].Cells[1].Value.ToString().Equals("Service"))
                {
                    check = true;
                    break;
                }
            }

            try
            {
                total = Convert.ToDecimal(lblTotal.Text);

            }
            catch (Exception)
            {
                total = 0;

            }
            try
            {
                payment = Convert.ToDecimal(txtPayment.Text);

            }
            catch (Exception)
            {
            }
            if (check)
            {
                for (int m = 0; m < dataGridView1.Rows.Count; m++)
                {
                    if (dataGridView1.Rows[m].Cells[1].Value.ToString().Equals("Service"))
                    {
                        servicefee += Convert.ToDecimal(dataGridView1.Rows[m].Cells[2].Value);
                        subtotal += Convert.ToDecimal(dataGridView1.Rows[m].Cells[5].Value);
                    }
                    else
                    {
                        itemfee += Convert.ToDecimal(dataGridView1.Rows[m].Cells[2].Value) * Convert.ToInt32(dataGridView1.Rows[m].Cells[3].Value);
                    }
                }
                totalfee = (subtotal / 2) + itemfee;
                if (payment < totalfee)
                {
                    label54.Text = "Insufficient payment";
                }
                else
                {
                    label54.Text = "";
                    change = payment - total;
                    if (change < 0)
                    {
                        change = 0;
                    }
                }
            }
            else
            {
                if (payment < total)
                {
                    label54.Text = "Insufficient payment";
                }
                else
                {
                    label54.Text = "";
                    change = payment - total;
                    if (change < 0)
                    {
                        change = 0;
                    }
                }
            }

            lblChange.Text = change.ToString();
        }
        

        private void button6_Click_1(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        private void panel12_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            decimal total1 = 0;
            int qty = 1;

            string service = comboBox4.Text;
            bool check = false;
            decimal total = 0;
            if (dataGridView1.Rows.Count > 0)
            {
                button15.Enabled = true;
            }
            else
            {
                button15.Enabled = false;
            }
            try
            {
                total = Convert.ToDecimal(lblTotal.Text);
            }
            catch (Exception)
            {
                total = 0;
            }
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (service == dataGridView1.Rows[i].Cells[0].Value.ToString())
                {
                    errorProvider.SetError(comboBox4, "Product already exists in the datagridview!");
                    check = true;
                    break;
                }
                else
                {
                    errorProvider.SetError(comboBox4, string.Empty);
                }
            }
            if (check == false)
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * from servicetbl where Service_Name = '" + service + "'";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        total1 = dataReader.GetInt32("Service_Fee");
                        dataGridView1.Rows.Add(dataReader.GetString("Service_Name"), "Service", dataReader.GetDecimal("Service_Fee"), qty);
                    }
                    connection.Close();
                }
                catch (MySqlException me)
                {
                    MessageBox.Show(me.Message);
                }
                total += total1;
                lblTotal.Text = total.ToString();
            }
        }
        private void bunifuDropdown1_onItemSelected(object sender, EventArgs e)
        {
            string mop = bunifuDropdown1.selectedValue.ToString();
            if (mop == "Cash")
            {
                label23.Visible = true;
                label25.Visible = true;
                txtPayment.Visible = true;
                label4.Visible = false;
                txtReceipt.Visible = false;
                txtReceipt.Text = "";
            }
            else
            {
                label4.Visible = true;
                txtReceipt.Visible = true;
                label23.Visible = false;
                label25.Visible = false;
                txtPayment.Visible = false;
                txtPayment.Text = "";
            }
        }

        private void txtReceipt_KeyUp(object sender, KeyEventArgs e)
        {
            string containsLetter = @"[A-Za-z~!@#$%^&*()_+=-]";
            string cno = txtReceipt.Text.Trim();
            if (Regex.IsMatch(cno, containsLetter))
            {
                txtReceipt.BackColor = System.Drawing.Color.FromArgb(252, 224, 224);
                label54.Text = "Numeric only";
            }
            else
            {
                label54.Text = "";
                txtReceipt.BackColor = System.Drawing.Color.White;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            string patientname = "";
            try
            {
                patientname = textBox3.Text.Trim();
            }
            catch (Exception)
            {
                patientname = "";
            }
            dataGridView2.Rows.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT Billing_No,Billing_Date,Total_Bill,Balance,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit),b.Patient_No from billingtbl b, patienttbl p where CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) LIKE '%" + patientname + "%' and p.Patient_No = b.Patient_No and b.Balance > 0 and Billing_No in (SELECT MAX(Billing_No) from billingtbl group by Patient_No)", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView2.Rows.Add(dataReader.GetInt32("Billing_No"), dataReader.GetDateTime("Billing_Date").ToString("MM-dd-yyyy"), dataReader.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)"), dataReader.GetDecimal("Total_Bill"), dataReader.GetDecimal("Balance"));
                }
                connection.Close();
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            dashboardUC dash = new dashboardUC(ParentForm.Username);
            dash.BringToFront();
            dash.Show(); 
            this.Hide();
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string patient = comboBox3.Text;
            GetPatientNo(patient);
        }
    }
}
