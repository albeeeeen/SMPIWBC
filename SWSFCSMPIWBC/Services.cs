using Bunifu.Framework.UI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SWSFCSMPIWBC
{
    public partial class Services : Form
    {
        static string connectionString = "datasource=localhost" + ";" + "DATABASE=slimmersdb" + ";" + "UID=root"
         + ";" + "PASSWORD=root" + ";";
        MySqlConnection connection = new MySqlConnection(connectionString);
        public Services()
        {
            InitializeComponent();
            productsUC1.BringToFront();
            //servicesUC1.SendToBack();
            //machineUC1.SendToBack();s
            //employeeUC1.SendToBack();
            //discountUC1.SendToBack();
            initTime();
            initNotif();
            //prodBtn.Textcolor = Color.FromArgb(4, 180, 253);
            CheckNotification();
        }
        private void initNotif()
        {
            Timer t1 = new System.Windows.Forms.Timer();
            t1.Interval = 15000;
            t1.Tick += new EventHandler(t1_Tick);
            t1.Enabled = true;
        }
        void t1_Tick(object sender, EventArgs e)
        {
            notificationPanel1.Controls.Clear();
            CheckNotification();
        }
        public void CheckNotification()
        {
            string user = "";
            int schedctr = 0, critlvlctr = 0, balancectr = 0, donectr = 0, totalctr = 0;
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string time = DateTime.Now.ToString("hh:mm:ss tt");
            int checkhour = Convert.ToInt32(time.Substring(0, 2));
            int checkmin = Convert.ToInt32(time.Substring(3, 2));
            string checkampm = time.Substring(9, 2);
            int hour = 0, min = 0, endhour = 0, endmin = 0, qty = 0, crit = 0, j = 0, ctr = 0, checkappointmentno = 0;
            string ampm = "", endampm = "", empname = "";
            bool check = false, checker = false, checks = false, checking = false;
            int[] appointmentno = new int[500];
            BunifuFlatButton btnDoneAppoint = new BunifuFlatButton();
            BunifuFlatButton btnBalance = new BunifuFlatButton();
            BunifuFlatButton btnCritLvl = new BunifuFlatButton();
            bool upAppoint = checkUpAppoint();
            bool doneAppoint = checkDoneAppoint();
            if (upAppoint)
            {
                Label lbl = new Label();
                lbl.Text = "These employee(s) have an appointment";
                lbl.Font = new Font("Arial", 8);
                lbl.Size = new Size(200, 15);
                lbl.Margin = new Padding(3, 3, 3, 3);
                notificationPanel1.Controls.Add(lbl);
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT *,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from appointmenttbl a, employee_appointmenttbl ea, employee_patienttbl ep, employeetbl e where Appointment_Date = '" + date + "' and a.Appointment_Status = 'Not Started' and a.Appointment_No = ea.Appointment_No and ea.Employee_Patient_No = ep.Employee_Patient_No and ep.Employee_No = e.Employee_No group by e.Employee_No", connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {

                        hour = Convert.ToInt32(dataReader.GetString("Appointment_StartTime").Substring(0, 2));
                        min = Convert.ToInt32(dataReader.GetString("Appointment_StartTime").Substring(3, 2));
                        ampm = dataReader.GetString("Appointment_StartTime").Substring(6, 2);
                        endhour = Convert.ToInt32(dataReader.GetString("Appointment_EndTime").Substring(0, 2));
                        endmin = Convert.ToInt32(dataReader.GetString("Appointment_EndTime").Substring(3, 2));
                        endampm = dataReader.GetString("Appointment_EndTime").Substring(6, 2);
                        empname = dataReader.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)");
                        if (((checkhour == hour && checkmin >= min && string.Equals(ampm, checkampm, StringComparison.OrdinalIgnoreCase)) || (checkhour > hour && string.Equals(ampm, checkampm, StringComparison.OrdinalIgnoreCase))) && ((checkhour < endhour && (!string.Equals(endampm, checkampm, StringComparison.OrdinalIgnoreCase) || string.Equals(endampm, checkampm, StringComparison.OrdinalIgnoreCase))) || (checkhour == endhour && checkmin <= endmin && string.Equals(endampm, checkampm, StringComparison.OrdinalIgnoreCase))))
                        {

                            schedctr++;
                            BunifuFlatButton btnUpAppoint = new BunifuFlatButton();
                            btnUpAppoint.Size = new Size(200, 50);
                            btnUpAppoint.Iconimage = null;
                            btnUpAppoint.Margin = new Padding(0);
                            btnUpAppoint.Text = empname;

                            btnUpAppoint.Click += delegate
                            {
                                smUC sm = new smUC();
                                user = label5.Text;
                                string newempname = btnUpAppoint.Text;
                                //sm.label15.Text = user;
                                int x = 0, hour1 = 0, min1 = 0, endhour1 = 0, endmin1 = 0;
                                string ampm1 = "", endampm1 = "", empname1 = "";
                                string datenow = DateTime.Now.ToString("yyyy-MM-dd");
                                sm.dataGridView4.Rows.Clear();
                                sm.dataGridView4.ClearSelection();
                                try
                                {
                                    connection.Open();
                                    MySqlCommand cmd1 = new MySqlCommand("Select *,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit),CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from appointmenttbl a, employee_appointmenttbl ea, employeetbl e, employee_patienttbl ep,patienttbl p,servicetbl s where Appointment_Date = '" + datenow + "' and CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) = '" + newempname + "' and ea.Appointment_No = a.Appointment_No and ea.Employee_Patient_No = ep.Employee_Patient_No and ep.Employee_No = e.Employee_No and ep.Patient_No = p.Patient_No and a.Service_No = s.Service_No", connection);
                                    MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                                    while (dataReader1.Read())
                                    {
                                        sm.dataGridView4.ClearSelection();
                                        hour1 = Convert.ToInt32(dataReader1.GetString("Appointment_StartTime").Substring(0, 2));
                                        min1 = Convert.ToInt32(dataReader1.GetString("Appointment_StartTime").Substring(3, 2));
                                        ampm1 = dataReader1.GetString("Appointment_StartTime").Substring(6, 2);
                                        endhour1 = Convert.ToInt32(dataReader1.GetString("Appointment_EndTime").Substring(0, 2));
                                        endmin1 = Convert.ToInt32(dataReader1.GetString("Appointment_EndTime").Substring(3, 2));
                                        endampm1 = dataReader1.GetString("Appointment_EndTime").Substring(6, 2);
                                        empname1 = dataReader1.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)");

                                        sm.dataGridView4.Rows.Add(dataReader1.GetInt32("Appointment_No"), dataReader1.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)"), dataReader1.GetString("Service_Name"), dataReader1.GetString("Appointment_StartTime"), dataReader1.GetString("Appointment_EndTime"), dataReader1.GetString("Appointment_Status"));
                                        if (((checkhour == hour1 && checkmin >= min1 && string.Equals(ampm1, checkampm, StringComparison.OrdinalIgnoreCase)) || (checkhour > hour1 && string.Equals(ampm1, checkampm, StringComparison.OrdinalIgnoreCase))) && ((checkhour < endhour1 && (!string.Equals(endampm1, checkampm, StringComparison.OrdinalIgnoreCase) || string.Equals(endampm1, checkampm, StringComparison.OrdinalIgnoreCase))) || (checkhour == endhour1 && checkmin <= endmin1 && string.Equals(endampm1, checkampm, StringComparison.OrdinalIgnoreCase))))
                                        {
                                            checkappointmentno = dataReader1.GetInt32("Appointment_No");
                                            if (checkappointmentno == Convert.ToInt32(sm.dataGridView4.Rows[x].Cells[0].Value))
                                            {
                                                sm.dataGridView4.Rows[x].Cells[0].Style.BackColor = Color.Salmon;
                                            }
                                        }
                                        x++;
                                    }
                                    connection.Close();
                                }
                                catch (Exception me)
                                {
                                    connection.Close();
                                    MessageBox.Show(me.Message);
                                }
                                for (int row = 0; row < sm.dataGridView4.Rows.Count; row++)
                                {
                                    if (sm.dataGridView4.Rows[row].Cells[5].Value.ToString() == "Not Started")
                                    {
                                        sm.dataGridView4.Rows[row].Cells[5].Style.BackColor = Color.Bisque;
                                        DataGridViewTextBoxCell txtCell = new DataGridViewTextBoxCell();
                                        sm.dataGridView4.Rows[row].Cells[8] = txtCell;
                                        sm.dataGridView4.Rows[row].Cells[8].ReadOnly = true;
                                    }
                                    else if (sm.dataGridView4.Rows[row].Cells[5].Value.ToString() == "On Going")
                                    {
                                        sm.dataGridView4.Rows[row].Cells[5].Style.BackColor = Color.DeepSkyBlue;
                                        DataGridViewTextBoxCell txtCell = new DataGridViewTextBoxCell();
                                        sm.dataGridView4.Rows[row].Cells[6] = txtCell;
                                        sm.dataGridView4.Rows[row].Cells[6].ReadOnly = true;
                                    }
                                    else if (sm.dataGridView4.Rows[row].Cells[5].Value.ToString() == "Cancelled")
                                    {
                                        sm.dataGridView4.Rows[row].Cells[5].Style.BackColor = Color.LightCoral;
                                    }
                                    else if (sm.dataGridView4.Rows[row].Cells[5].Value.ToString() == "Done")
                                    {
                                        sm.dataGridView4.Rows[row].Cells[5].Style.BackColor = Color.MediumSeaGreen;
                                        DataGridViewTextBoxCell txtCell = new DataGridViewTextBoxCell();
                                        DataGridViewTextBoxCell txtCell1 = new DataGridViewTextBoxCell();
                                        DataGridViewTextBoxCell txtCell2 = new DataGridViewTextBoxCell();
                                        sm.dataGridView4.Rows[row].Cells[6] = txtCell;
                                        sm.dataGridView4.Rows[row].Cells[6].ReadOnly = true;
                                        sm.dataGridView4.Rows[row].Cells[7] = txtCell1;
                                        sm.dataGridView4.Rows[row].Cells[7].ReadOnly = true;
                                        sm.dataGridView4.Rows[row].Cells[8] = txtCell2;
                                        sm.dataGridView4.Rows[row].Cells[8].ReadOnly = true;
                                    }

                                }
                                sm.label53.Text = newempname;
                                sm.servicemonitoringPanel.Hide();
                                sm.consultantschedPanel.Hide();
                                sm.empButtons.Hide();
                                sm.schedulePanel.Show();
                                sm.Show();
                                this.Hide();
                            };

                            notificationPanel1.Controls.Add(btnUpAppoint);

                        }

                    }
                    connection.Close();
                }
                catch (Exception me)
                {
                    connection.Close();
                    MessageBox.Show(me.Message);
                }

            }

            if (doneAppoint)
            {
                Label lbl1 = new Label();
                lbl1.Text = "Employee(s) with done appointment";
                lbl1.Font = new Font("Arial", 8);
                lbl1.Size = new Size(200, 15);
                lbl1.Margin = new Padding(3, 3, 3, 3);
                notificationPanel1.Controls.Add(lbl1);
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT *,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from appointmenttbl a, employee_appointmenttbl ea, employee_patienttbl ep, employeetbl e where Appointment_Date = '" + date + "' and a.Appointment_Status = 'Not Started' and a.Appointment_No = ea.Appointment_No and ea.Employee_Patient_No = ep.Employee_Patient_No and ep.Employee_No = e.Employee_No group by e.Employee_No", connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {

                        hour = Convert.ToInt32(dataReader.GetString("Appointment_StartTime").Substring(0, 2));
                        min = Convert.ToInt32(dataReader.GetString("Appointment_StartTime").Substring(3, 2));
                        ampm = dataReader.GetString("Appointment_StartTime").Substring(6, 2);
                        endhour = Convert.ToInt32(dataReader.GetString("Appointment_EndTime").Substring(0, 2));
                        endmin = Convert.ToInt32(dataReader.GetString("Appointment_EndTime").Substring(3, 2));
                        endampm = dataReader.GetString("Appointment_EndTime").Substring(6, 2);
                        empname = dataReader.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)");

                        if ((checkhour == endhour && checkmin >= endmin && string.Equals(endampm, checkampm, StringComparison.OrdinalIgnoreCase)) || (checkhour > endhour && string.Equals(endampm, checkampm, StringComparison.OrdinalIgnoreCase)))
                        {
                            donectr++;
                            empname = dataReader.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)");
                            btnDoneAppoint = new BunifuFlatButton();
                            btnDoneAppoint.Size = new Size(200, 50);
                            btnDoneAppoint.Iconimage = null;
                            btnDoneAppoint.Margin = new Padding(0);
                            btnDoneAppoint.Text = empname;
                            btnDoneAppoint.Click += delegate
                            {
                                smUC sm = new smUC();
                                user = label5.Text;
                                string newempname = btnDoneAppoint.Text;
                                //sm.label15.Text = user;
                                int x = 0, hour1 = 0, min1 = 0, endhour1 = 0, endmin1 = 0;
                                string ampm1 = "", endampm1 = "", empname1 = "";
                                string datenow = DateTime.Now.ToString("yyyy-MM-dd");
                                try
                                {
                                    connection.Open();
                                    MySqlCommand cmd2 = new MySqlCommand("Select *,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit),CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from appointmenttbl a, employee_appointmenttbl ea, employeetbl e, employee_patienttbl ep,patienttbl p,servicetbl s where Appointment_Date = '" + datenow + "' and CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) = '" + newempname + "' and ea.Appointment_No = a.Appointment_No and ea.Employee_Patient_No = ep.Employee_Patient_No and ep.Employee_No = e.Employee_No and ep.Patient_No = p.Patient_No and a.Service_No = s.Service_No", connection);
                                    MySqlDataReader dataReader2 = cmd2.ExecuteReader();
                                    while (dataReader2.Read())
                                    {
                                        sm.dataGridView4.ClearSelection();
                                        hour1 = Convert.ToInt32(dataReader2.GetString("Appointment_StartTime").Substring(0, 2));
                                        min1 = Convert.ToInt32(dataReader2.GetString("Appointment_StartTime").Substring(3, 2));
                                        ampm1 = dataReader2.GetString("Appointment_StartTime").Substring(6, 2);
                                        endhour1 = Convert.ToInt32(dataReader2.GetString("Appointment_EndTime").Substring(0, 2));
                                        endmin1 = Convert.ToInt32(dataReader2.GetString("Appointment_EndTime").Substring(3, 2));
                                        endampm1 = dataReader2.GetString("Appointment_EndTime").Substring(6, 2);
                                        empname1 = dataReader2.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)");

                                        sm.dataGridView4.Rows.Add(dataReader2.GetInt32("Appointment_No"), dataReader2.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)"), dataReader2.GetString("Service_Name"), dataReader2.GetString("Appointment_StartTime"), dataReader2.GetString("Appointment_EndTime"), dataReader2.GetString("Appointment_Status"));
                                        if ((checkhour == endhour1 && checkmin >= endmin1 && string.Equals(endampm1, checkampm, StringComparison.OrdinalIgnoreCase)) || (checkhour > endhour1 && string.Equals(endampm1, checkampm, StringComparison.OrdinalIgnoreCase)))
                                        {
                                            checkappointmentno = dataReader2.GetInt32("Appointment_No");
                                            if (checkappointmentno == Convert.ToInt32(sm.dataGridView4.Rows[x].Cells[0].Value))
                                            {
                                                sm.dataGridView4.Rows[x].Cells[0].Style.BackColor = Color.Salmon;
                                            }
                                        }
                                        x++;
                                    }
                                    connection.Close();
                                }
                                catch (Exception me)
                                {
                                    connection.Close();
                                    MessageBox.Show(me.Message);
                                }
                                for (int row = 0; row < sm.dataGridView4.Rows.Count; row++)
                                {
                                    if (sm.dataGridView4.Rows[row].Cells[5].Value.ToString() == "Not Started")
                                    {
                                        sm.dataGridView4.Rows[row].Cells[5].Style.BackColor = Color.Bisque;
                                    }
                                    else if (sm.dataGridView4.Rows[row].Cells[5].Value.ToString() == "On Going")
                                    {
                                        sm.dataGridView4.Rows[row].Cells[5].Style.BackColor = Color.DeepSkyBlue;
                                    }
                                    else if (sm.dataGridView4.Rows[row].Cells[5].Value.ToString() == "Cancelled")
                                    {
                                        sm.dataGridView4.Rows[row].Cells[5].Style.BackColor = Color.LightCoral;
                                    }
                                    else if (sm.dataGridView4.Rows[row].Cells[5].Value.ToString() == "Done")
                                    {
                                        sm.dataGridView4.Rows[row].Cells[5].Style.BackColor = Color.MediumSeaGreen;
                                    }

                                }
                                sm.label53.Text = newempname;
                                sm.servicemonitoringPanel.Hide();
                                sm.consultantschedPanel.Hide();
                                sm.empButtons.Hide();
                                sm.schedulePanel.Show();
                                sm.Show();
                                this.Hide();
                            };
                            notificationPanel1.Controls.Add(btnDoneAppoint);
                        }
                    }
                    connection.Close();
                }
                catch (Exception me)
                {
                    connection.Close();
                    MessageBox.Show(me.Message);
                }
            }
            try
            {
                connection.Open();
                MySqlCommand cmd3 = new MySqlCommand("SELECT Billing_No,Billing_Date,Total_Bill,Balance,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit),b.Patient_No from billingtbl b, patienttbl p where p.Patient_No = b.Patient_No and b.Balance > 0 and Billing_No in (SELECT MAX(Billing_No) from billingtbl group by Patient_No)", connection);
                MySqlDataReader dataReader3 = cmd3.ExecuteReader();
                while (dataReader3.Read())
                {
                    balancectr++;
                    checks = true;
                }
                connection.Close();

                connection.Open();
                string query2 = ("Select Total_Quantity, Critical_Level from product_inventorytbl pi, product_prodtypetbl ppt where ppt.Product_ProdType_No = pi.Product_ProdType_No");
                MySqlCommand cmd4 = new MySqlCommand(query2, connection);
                MySqlDataReader dataReader4 = cmd4.ExecuteReader();
                while (dataReader4.Read())
                {
                    qty = dataReader4.GetInt32("Total_Quantity");
                    crit = dataReader4.GetInt32("Critical_Level");
                    if (qty <= crit)
                    {
                        critlvlctr++;
                        checking = true;
                    }
                    j++;
                }
                connection.Close();


                if (checks)
                {
                    btnBalance = new BunifuFlatButton();
                    btnBalance.Size = new Size(200, 50);
                    btnBalance.Iconimage = null;
                    btnBalance.Margin = new Padding(0);
                    btnBalance.Text = "(" + balancectr + ")" + "  There's a patient with balance";
                    btnBalance.Click += delegate
                    {
                        user = label5.Text;
                        paymentUC pay = new paymentUC();
                        pay.panel2.BringToFront();
                        pay.panel8.SendToBack();
                        pay.panel3.SendToBack();
                        //pay.label15.Text = user;
                        pay.Show();
                        this.Hide();
                    };
                    notificationPanel1.Controls.Add(btnBalance);
                }

                if (checking)
                {
                    btnCritLvl = new BunifuFlatButton();
                    btnCritLvl.Size = new Size(200, 50);
                    btnCritLvl.Iconimage = null;
                    btnCritLvl.Margin = new Padding(0);
                    btnCritLvl.Text = "(" + critlvlctr + ")" + "  There's a fucking shit in the inventory";
                    btnCritLvl.Click += delegate
                    {
                        user = label5.Text;
                        allinventoryUC i = new allinventoryUC();
                        i.inventoryUC1.BringToFront();
                        i.orderUC1.SendToBack();
                        i.pullOutProductsUC1.SendToBack();
                        i.adddInventoryUC1.SendToBack();
                        //i.label15.Text = user;
                        i.Show();
                        this.Hide();

                    };
                    notificationPanel1.Controls.Add(btnCritLvl);
                }
                totalctr = schedctr + critlvlctr + balancectr + donectr;
                if (totalctr != 0)
                {
                    label1.Visible = true;
                }
                else
                {
                    label1.Visible = false;
                }
                label1.Text = totalctr.ToString();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }
        public Boolean checkUpAppoint()
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string time = DateTime.Now.ToString("hh:mm:ss tt");
            int checkhour = Convert.ToInt32(time.Substring(0, 2));
            int checkmin = Convert.ToInt32(time.Substring(3, 2));
            string checkampm = time.Substring(9, 2);
            int hour = 0, min = 0, endhour = 0, endmin = 0;
            string ampm = "", endampm = "", empname = "";
            bool check = false;
            try
            {
                connection.Open();
                MySqlCommand cmd5 = new MySqlCommand("SELECT *,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from appointmenttbl a, employee_appointmenttbl ea, employee_patienttbl ep, employeetbl e where Appointment_Date = '" + date + "' and a.Appointment_Status = 'Not Started' and a.Appointment_No = ea.Appointment_No and ea.Employee_Patient_No = ep.Employee_Patient_No and ep.Employee_No = e.Employee_No group by e.Employee_No", connection);
                MySqlDataReader dataReader5 = cmd5.ExecuteReader();
                while (dataReader5.Read())
                {

                    hour = Convert.ToInt32(dataReader5.GetString("Appointment_StartTime").Substring(0, 2));
                    min = Convert.ToInt32(dataReader5.GetString("Appointment_StartTime").Substring(3, 2));
                    ampm = dataReader5.GetString("Appointment_StartTime").Substring(6, 2);
                    endhour = Convert.ToInt32(dataReader5.GetString("Appointment_EndTime").Substring(0, 2));
                    endmin = Convert.ToInt32(dataReader5.GetString("Appointment_EndTime").Substring(3, 2));
                    endampm = dataReader5.GetString("Appointment_EndTime").Substring(6, 2);
                    empname = dataReader5.GetString("CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit)");
                    if (((checkhour == hour && checkmin >= min && string.Equals(ampm, checkampm, StringComparison.OrdinalIgnoreCase)) || (checkhour > hour && string.Equals(ampm, checkampm, StringComparison.OrdinalIgnoreCase))) && ((checkhour < endhour && (!string.Equals(endampm, checkampm, StringComparison.OrdinalIgnoreCase) || string.Equals(endampm, checkampm, StringComparison.OrdinalIgnoreCase))) || (checkhour == endhour && checkmin <= endmin && string.Equals(endampm, checkampm, StringComparison.OrdinalIgnoreCase))))
                    {
                        check = true;
                    }
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            return check;
        }
        public Boolean checkDoneAppoint()
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string time = DateTime.Now.ToString("hh:mm:ss tt");
            int checkhour = Convert.ToInt32(time.Substring(0, 2));
            int checkmin = Convert.ToInt32(time.Substring(3, 2));
            string checkampm = time.Substring(9, 2);
            int hour = 0, min = 0, endhour = 0, endmin = 0;
            string ampm = "", endampm = "", empname = "";
            bool check = false;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT *,CONCAT(Employee_LName,', ',Employee_FName,' ',Employee_MidInit) from appointmenttbl a, employee_appointmenttbl ea, employee_patienttbl ep, employeetbl e where Appointment_Date = '" + date + "' and a.Appointment_Status = 'Not Started' and a.Appointment_No = ea.Appointment_No and ea.Employee_Patient_No = ep.Employee_Patient_No and ep.Employee_No = e.Employee_No group by e.Employee_No", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {

                    hour = Convert.ToInt32(dataReader.GetString("Appointment_StartTime").Substring(0, 2));
                    min = Convert.ToInt32(dataReader.GetString("Appointment_StartTime").Substring(3, 2));
                    ampm = dataReader.GetString("Appointment_StartTime").Substring(6, 2);
                    endhour = Convert.ToInt32(dataReader.GetString("Appointment_EndTime").Substring(0, 2));
                    endmin = Convert.ToInt32(dataReader.GetString("Appointment_EndTime").Substring(3, 2));
                    endampm = dataReader.GetString("Appointment_EndTime").Substring(6, 2);
                    if ((checkhour == endhour && checkmin >= endmin && string.Equals(endampm, checkampm, StringComparison.OrdinalIgnoreCase)) || (checkhour > endhour && string.Equals(endampm, checkampm, StringComparison.OrdinalIgnoreCase)))
                    {
                        check = true;
                    }
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            return check;
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
            label23.Text = DateTime.Now.ToLongDateString();
            label9.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }
       
        
       
        private void button9_Click(object sender, EventArgs e)
        {
            string user = label5.Text;
            Login login = new Login();
            login.CheckUser(user);
            this.Hide();
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
            Maintenance mainte = new Maintenance();
            mainte.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string user = label5.Text;
            Employee emp = new Employee();
            emp.label26.Text = user;
            emp.Show();
            //emp.employeePanel.BringToFront();
            //emp.positionPanel.SendToBack();
            //emp.schedulePanel.SendToBack();
            this.Hide();
        }
        
        private void button4_Click(object sender, EventArgs e)
        {
            string user = label5.Text;
            Machine mach = new Machine();
            mach.label24.Text = user;
            mach.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string user = label5.Text;
            Product prod = new Product();
            prod.label37.Text = user;
            prod.Show();
            this.Hide();
        }

        

       

        private void button2_Click(object sender, EventArgs e)
        {
            string user = label5.Text;
            label5.Text = user;
            //service.Show();
            //this.Hide();

            

        }

        private void button15_Click(object sender, EventArgs e)
        {
            string user = label5.Text;
            Discounts discounts = new Discounts();
            discounts.label5.Text = user;
            discounts.Show();
            this.Hide();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            ServiceRequisite sr = new ServiceRequisite();
            sr.Show();
            this.Hide();
        }
        private void button16_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        int ctr = 0;      
        private void button1_Click_2(object sender, EventArgs e)
        {
            notificationPanel.Visible = false;
            ctr++;
            ctr1++;
            settingsTransition.ShowSync(settings);
            settings.BringToFront();
            if (ctr % 2 == 0)
            {
                settings.Visible = false;
            }
            else
            {
                settings.Visible = true;
            }
           
        }
       
        int ctr1 = 0;
        private void btnNotification_Click(object sender, EventArgs e)
        {
            settings.Visible = false;
            ctr1++;
            ctr++;
            notificationTransition.ShowSync(notificationPanel);
            
            if (ctr1 % 2 == 0)
            {
                notificationPanel.Visible = false;
            }
            else
            {
                notificationPanel.Visible = true;
            }
        }


       


    }
}
