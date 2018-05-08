using Bunifu.Framework.UI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SWSFCSMPIWBC
{
    public partial class Profile : Form
    {
        static string connectionString =
       System.Configuration.ConfigurationManager.
       ConnectionStrings["SWSFCSMPIWBC.Properties.Settings.slimmersdbConnectionString"].ConnectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);
        public Profile()
        {
            InitializeComponent();
            initNotif();
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
            bool checks = false, checking = false;
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
                                HomePage hp = new HomePage();
                                smUC sm = new smUC();
                                user = label48.Text;
                                string newempname = btnUpAppoint.Text;
                                hp.label15.Text = user;
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
                                HomePage hp = new HomePage();
                                smUC sm = new smUC();
                                user = label48.Text;
                                string newempname = btnDoneAppoint.Text;
                                hp.label15.Text = user;
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
                        HomePage hp = new HomePage();
                        user = label48.Text;
                        paymentUC pay = new paymentUC();
                        pay.panel2.BringToFront();
                        pay.panel8.SendToBack();
                        pay.panel3.SendToBack();
                        hp.label15.Text = user;
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
                        HomePage hp = new HomePage();
                        user = label48.Text;
                        allinventoryUC i = new allinventoryUC();
                        i.inventoryUC1.BringToFront();
                        i.orderUC1.SendToBack();
                        i.pullOutProductsUC1.SendToBack();
                        i.adddInventoryUC1.SendToBack();
                        hp.label15.Text = user;
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
            string ampm = "", endampm = "";
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
        private void Profile_Load(object sender, EventArgs e)
        {

        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            string user = label48.Text;
            Login login = new Login();
            login.CheckUser(user);
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            checkBox3.Enabled = true;
            checkBox4.Enabled = true;
            checkBox5.Enabled = true;
            checkBox6.Enabled = true;
            checkBox7.Enabled = true;
            textBox47.Enabled = true;
            checkBox1.Enabled = true;
            checkBox2.Enabled = true;
            checkBox23.Enabled = true;
            checkBox24.Enabled = true;
            richTextBox1.Enabled = true;
            checkBox8.Enabled = true;
            checkBox9.Enabled = true;
            checkBox10.Enabled = true;
            checkBox11.Enabled = true;
            checkBox18.Enabled = true;
            checkBox19.Enabled = true;
            checkBox20.Enabled = true;
            checkBox21.Enabled = true;
            checkBox22.Enabled = true;
            checkBox12.Enabled = true;
            checkBox13.Enabled = true;
            checkBox14.Enabled = true;
            checkBox15.Enabled = true;
            checkBox16.Enabled = true;
            checkBox17.Enabled = true;
            pictureBox1.Enabled = true;
            bunifuThinButton22.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label50_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void button19_Click(object sender, EventArgs e)
        {
            panel5.Show();
            panel6.Hide();
            panel11.Hide();
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox4.ReadOnly = true;
            textBox10.ReadOnly = true;
            textBox11.ReadOnly = true;
            textBox6.ReadOnly = true;
            textBox1.BorderStyle = BorderStyle.None;
            textBox2.BorderStyle = BorderStyle.None;
            textBox3.BorderStyle = BorderStyle.None;
            textBox4.BorderStyle = BorderStyle.None;
            textBox10.BorderStyle = BorderStyle.None;
            textBox11.BorderStyle = BorderStyle.None;
            textBox6.BorderStyle = BorderStyle.None;
            panel8.Enabled = false;
            dateTimePicker1.Enabled = false;
            panel7.Enabled = false;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            panel6.Show();
            panel5.Hide();
            panel11.Hide();
        }

        private void label30_Click(object sender, EventArgs e)
        {

        }

        private void checkBox21_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button18_Click(object sender, EventArgs e)
        {
            panel6.Show();
            panel5.Hide();
            panel11.Hide();

            slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Top;
            slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Height;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            string imagepath = "";
            panel11.Show();
            panel6.Hide();
            panel5.Hide();
            int patientno = 0;
            try
            {
                patientno = Convert.ToInt32(label2.Text);
            }
            catch (Exception me)
            {
            }
            try
            {
                connection.Open();
                string query = "SELECT *,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from patienttbl p, patient_demtbl pd where p.Patient_No = '" + patientno + "' and p.Patient_No = pd.Patient_No order by Dem_no desc LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    label16.Text = dataReader.GetInt32("Patient_No").ToString();
                    label17.Text = dataReader.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)");
                    imagepath = dataReader.GetString("Dem_Picture");
                    imagepath = imagepath.Replace(",", "/");
                    pictureBox1.Image = Image.FromFile(imagepath);
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            panel11.Enabled = true;
            slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Top;
            slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Height;
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void panel6_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            textBox1.ReadOnly = false;
            textBox2.ReadOnly = false;
            textBox3.ReadOnly = false;
            textBox4.ReadOnly = false;
            textBox10.ReadOnly = false;
            textBox11.ReadOnly = false;
            textBox6.ReadOnly = false;
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox2.BorderStyle = BorderStyle.FixedSingle;
            textBox3.BorderStyle = BorderStyle.FixedSingle;
            textBox4.BorderStyle = BorderStyle.FixedSingle;
            textBox10.BorderStyle = BorderStyle.FixedSingle;
            textBox11.BorderStyle = BorderStyle.FixedSingle;
            textBox6.BorderStyle = BorderStyle.FixedSingle;
            textBox29.ReadOnly = false;
            textBox29.BorderStyle = BorderStyle.FixedSingle;
            textBox28.ReadOnly = false;
            textBox28.BorderStyle = BorderStyle.FixedSingle;
            textBox27.ReadOnly = false;
            textBox27.BorderStyle = BorderStyle.FixedSingle;
            textBox25.ReadOnly = false;
            textBox25.BorderStyle = BorderStyle.FixedSingle;
            textBox24.ReadOnly = false;
            textBox24.BorderStyle = BorderStyle.FixedSingle;
            comboBox1.Enabled = true;
            panel9.Enabled = true;
            panel4.Enabled = true;
            checkBox37.Enabled = true;
            checkBox36.Enabled = true;
            checkBox35.Enabled = true;
            checkBox34.Enabled = true;
            checkBox33.Enabled = true;
            checkBox32.Enabled = true;
            checkBox31.Enabled = true;
            checkBox30.Enabled = true;
            panel17.Enabled = true;
            panel18.Enabled = true;
            panel19.Enabled = true;
            panel20.Enabled = true;
            panel21.Enabled = true;
            panel22.Enabled = true;
            panel23.Enabled = true;
            panel24.Enabled = true;
            panel8.Enabled = true;
            dateTimePicker1.Enabled = true;
            panel7.Enabled = true;
            comboBox2.Enabled = true;
            textBox31.BorderStyle = BorderStyle.FixedSingle;
            textBox31.ReadOnly = false;
            textBox30.BorderStyle = BorderStyle.FixedSingle;
            textBox30.ReadOnly = false;
            button1.Hide();
            button4.Show();
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }
        public bool IsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            string user = label48.Text;
            List<string> risks = new List<string>();
            string others = "", othermedhist = "";
            bool check = false;
            string lname, fname, mi, gender, address, bdate, cstatus, occupation;
            long cno = 0;
            int age = 0;
            string containNumber = @"[0-9~!@#$%^&*()_+=-]";
            int patientno = Convert.ToInt32(textBox14.Text);
            lname = textBox1.Text.Trim();
            fname = textBox2.Text.Trim();
            mi = textBox3.Text.Trim();
            address = textBox6.Text.Trim();
            bdate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string email = textBox11.Text.Trim();
            occupation = textBox10.Text.Trim();
            string status = comboBox2.Text;
            if (!IsValid(email))
            {
                label28.Text = "Invalid email";
                textBox11.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
                label38.Text = "**There's a problem with your personal information";
            }
            else
            {
                textBox11.BackColor = Color.White;
                label28.Text = "";
                label38.Text = "";
            }
            
            if (lname.Length == 0)
            {
                check = true;
                label3.Text = "You must enter last name";
                textBox1.BackColor = Color.FromArgb(252, 224, 224);
                label38.Text = "**There's a problem with your personal information";
            }
            else
            {

                if (Regex.IsMatch(lname, containNumber))
                {
                    check = true;
                    label3.Text = "Last name format invalid";
                    textBox1.BackColor = Color.FromArgb(252, 224, 224);
                    label38.Text = "**There's a problem with your personal information";
                }
                else
                {
                    label3.Text = "";
                    textBox1.BackColor = Color.White;
                    label38.Text = "";
                }
            }

            if (fname.Length == 0)
            {
                check = true;
                label18.Text = "You must enter first name";
                textBox2.BackColor = Color.FromArgb(252, 224, 224);
                label38.Text = "**There's a problem with your personal information";
            }
            else
            {

                if (Regex.IsMatch(fname, containNumber))
                {
                    check = true;
                    label18.Text = "First name format invalid";
                    textBox2.BackColor = Color.FromArgb(252, 224, 224);
                    label38.Text = "**There's a problem with your personal information";
                }
                else
                {
                    label18.Text = "";
                    textBox2.BackColor = Color.White;
                    label38.Text = "";
                }
            }

            if (Regex.IsMatch(mi, containNumber))
            {
                check = true;
                label22.Text = "Invalid Mid Init";
                textBox3.BackColor = Color.FromArgb(252, 224, 224);
                label38.Text = "**There's a problem with your personal information";
            }
            else
            {
                label22.Text = "";
                textBox3.BackColor = Color.White;
                label38.Text = "";
            }
            if (string.IsNullOrEmpty(address))
            {
                check = true;
                label30.Text = "Address required";
                label38.Text = "**There's a problem with your personal information";
                textBox6.BackColor = Color.FromArgb(252, 224, 224);
            }
            else
            {
                label30.Text = "";
                label38.Text = "";
                textBox6.BackColor = Color.White;
            }
            try
            {
                age = Convert.ToInt32(textBox5.Text.Trim());
                label29.Text = "";
                label38.Text = "";
            }
            catch (FormatException)
            {
                check = true;
                label29.Text = "Age is required. Please check your birthdate";
                label38.Text = "**There's a problem with your personal information";
            }
            if (age < 16)
            {
                check = true;
                label29.Text = "Age is not valid it should be 16+";
                textBox5.BackColor = Color.FromArgb(252, 224, 224);
                label38.Text = "**There's a problem with your personal information";
            }
            else
            {
                label29.Text = "";
                textBox5.BackColor = Color.White;
                label38.Text = "";
            }
            try
            {
                cno = Convert.ToInt64(textBox4.Text.Trim());
                label24.Text = "";
                textBox4.BackColor = Color.White;
                label38.Text = "";
            }
            catch (FormatException)
            {
                check = true;
                label24.Text = "Contact number format invalid";
                textBox4.BackColor = Color.FromArgb(252, 224, 224);
                label38.Text = "**There's a problem with your personal information";
            }
            if (radioButton1.Checked)
            {
                gender = "Male";
            }
            else
            {
                gender = "Female";
            }
            if (radioButton3.Checked)
            {
                cstatus = "Single";
            }
            else if (radioButton4.Checked)
            {
                cstatus = "Married";
            }
            else if (radioButton5.Checked)
            {
                cstatus = "Widowed";
            }
            else
            {
                cstatus = "Others";
            }
            string weight = "", height = "";
            string bodyframe = "", bp = "", prate = "";
            string bodyfat = "";
            char isSmoker, isAlcoholic;
            string containLetter = @"[A-Za-z~!@#$%^&*()_+=-]";
            bodyfat = textBox27.Text.Trim();
            height = textBox29.Text.Trim();
            weight = textBox28.Text.Trim();
            bp = textBox25.Text.Trim();
            prate = textBox24.Text.Trim();
           
            if (string.IsNullOrEmpty(height))
            {
                label31.Text = "Required height";
                textBox29.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }
            else
            {
                if (Regex.IsMatch(height, containLetter))
                {
                    label31.Text = "Invalid height";
                    textBox29.BackColor = Color.FromArgb(252, 224, 224);
                    check = true;
                }
                else
                {
                    label31.Text = "";
                    textBox29.BackColor = Color.White;
                }
            }
            if (string.IsNullOrEmpty(weight))
            {
                label32.Text = "Required weight";
                textBox28.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }
            else
            {
                if (Regex.IsMatch(weight, containLetter))
                {
                    label32.Text = "Invalid weight";
                    textBox28.BackColor = Color.FromArgb(252, 224, 224);
                    check = true;

                }
                else
                {
                    label32.Text = "";
                    textBox28.BackColor = Color.White;
                }
            }
            bodyframe = comboBox1.Text.Trim();
            if (Regex.IsMatch(bodyfat, containLetter))
            {
                label34.Text = "Invalid bodyfat";
                textBox27.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }
            else
            {
                label34.Text = "";
                textBox27.BackColor = Color.White;
            }

            if (string.IsNullOrEmpty(bp))
            {
                label35.Text = "Required blood pressure";
                textBox25.BackColor = Color.FromArgb(252, 224, 224);
                check = true;
            }
            else
            {
                if (Regex.IsMatch(bp, containLetter))
                {
                    label35.Text = "Invalid blood pressure";
                    textBox25.BackColor = Color.FromArgb(252, 224, 224);
                    check = true;
                }
                else
                {
                    label35.Text = "";
                    textBox25.BackColor = Color.White;
                }
            }

            if (string.IsNullOrEmpty(prate))
            {
                label36.Text = "Required pulse rate";
                textBox24.BackColor = Color.FromArgb(252, 224, 224);
                check = true;

            }
            else
            {
                if (Regex.IsMatch(prate, containLetter))
                {
                    label36.Text = "Invalid pulse rate";
                    textBox24.BackColor = Color.FromArgb(252, 224, 224);
                    check = true;
                }
                else
                {
                    label36.Text = "";
                    textBox24.BackColor = Color.White;
                }
            }
            if (radioButton14.Checked)
            {
                isSmoker = 'T';
            }
            else
            {
                isSmoker = 'F';
            }
            if (radioButton12.Checked)
            {
                isAlcoholic = 'T';
            }
            else
            {
                isAlcoholic = 'F';
            }
            if (checkBox37.Checked)
            {
                risks.Add("Diabetes");
            }
            if (checkBox36.Checked)
            {
                risks.Add("Allergy");
            }
            if (checkBox35.Checked)
            {
                risks.Add("Heart Disease");
            }
            if (checkBox34.Checked)
            {
                risks.Add("Pace Maker");
            }
            if (checkBox33.Checked)
            {
                risks.Add("Seizures");
            }
            if (checkBox32.Checked)
            {
                risks.Add("Headaches");
            }
            if (checkBox31.Checked)
            {
                risks.Add("Chest Pains");
            }
            if (checkBox30.Checked)
            {
                others = textBox30.Text;
            }
            if (!string.IsNullOrEmpty(textBox31.Text.Trim()))
            {
                othermedhist = textBox31.Text;
            }
            if (radioButton5.Checked)
            {
                risks.Add("Dizziness");
            }
            if (radioButton15.Checked)
            {
                risks.Add("Asthma");
            }
            if (radioButton17.Checked)
            {
                risks.Add("Nausea");
            }
            if (radioButton19.Checked)
            {
                risks.Add("Arthritis");
            }
            if (radioButton21.Checked)
            {
                risks.Add("Bladder Problems");
            }
            if (radioButton23.Checked)
            {
                risks.Add("Cancer");
            }
            if (radioButton25.Checked)
            {
                risks.Add("Ringing Ears");
            }
            if (radioButton27.Checked)
            {
                risks.Add("Thyroid Conditions");
            }
            string allrisk = "";
            foreach (var risk in risks)
            {
                allrisk += risk + ", ";
            }
            allrisk = allrisk.Substring(0, allrisk.Length - 2);
            if (check == false)
            {

                try
                {
                    connection.Open();
                    string query2 = "Update patienttbl set Patient_LName = '" + lname + "', Patient_FName = '" + fname + "', Patient_MidInit = '" + mi + "',Patient_Gender = '" + gender + "',Patient_BirthDate = '" + bdate + "',Patient_Age = '" + age + "',Patient_Address = '" + address + "',Patient_ContactNo = '" + cno + "',Patient_Email = '" + email + "',Patient_CStatus = '" + cstatus + "',Patient_Occupation = '" + occupation + "', Patient_Status = '"+status+"' where Patient_No = '"+patientno+"'";
                    MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                    cmd2.ExecuteNonQuery();
                    connection.Close();

                    connection.Open();
                    string query1 = "Update patient_medicaltbl set Height = '" + height + "',Weight = '" + weight + "',Body_Fat = '" + bodyfat + "',Body_Frame = '" + bodyframe + "',Blood_Pressure = '" + bp + "',Pulse_Rate = '" + prate + "',isSmoker = '"+isSmoker+"',isAlcoholDrinker = '"+isAlcoholic+"',Risk_Factors = '" + allrisk + "',Other_Risks = '" + others + "',Other_MedHist = '" + othermedhist + "' where Patient_No = '" + patientno + "'";
                    MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                    cmd1.ExecuteNonQuery();
                    connection.Close();

                   
                }
                catch (MySqlException me)
                {
                    connection.Close();
                    MessageBox.Show(me.Message);
                }
                MessageBox.Show("Patient record udpated!");
                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;
                textBox4.ReadOnly = true;
                textBox10.ReadOnly = true;
                textBox11.ReadOnly = true;
                textBox6.ReadOnly = true;
                textBox1.BorderStyle = BorderStyle.None;
                textBox2.BorderStyle = BorderStyle.None;
                textBox3.BorderStyle = BorderStyle.None;
                textBox4.BorderStyle = BorderStyle.None;
                textBox10.BorderStyle = BorderStyle.None;
                textBox11.BorderStyle = BorderStyle.None;
                textBox6.BorderStyle = BorderStyle.None;
                textBox29.ReadOnly = true;
                textBox29.BorderStyle = BorderStyle.None;
                textBox28.ReadOnly = true;
                textBox28.BorderStyle = BorderStyle.None;
                textBox27.ReadOnly = true;
                textBox27.BorderStyle = BorderStyle.None;
                textBox25.ReadOnly = true;
                textBox25.BorderStyle = BorderStyle.None;
                textBox24.ReadOnly = true;
                textBox24.BorderStyle = BorderStyle.None;
                comboBox1.Enabled = false;
                panel9.Enabled = false;
                panel4.Enabled = false;
                checkBox37.Enabled = false;
                checkBox36.Enabled = false;
                checkBox35.Enabled = false;
                checkBox34.Enabled = false;
                checkBox33.Enabled = false;
                checkBox32.Enabled = false;
                checkBox31.Enabled = false;
                checkBox30.Enabled = false;
                panel17.Enabled = false;
                panel18.Enabled = false;
                panel19.Enabled = false;
                panel20.Enabled = false;
                panel21.Enabled = false;
                panel22.Enabled = false;
                panel23.Enabled = false;
                panel24.Enabled = false;
                panel8.Enabled = false;
                dateTimePicker1.Enabled = false;
                panel7.Enabled = false;
                comboBox2.Enabled = false;
                textBox31.BorderStyle = BorderStyle.None;
                textBox31.ReadOnly = true;
                textBox30.BorderStyle = BorderStyle.None;
                textBox30.ReadOnly = true;
                button1.Show();
                button4.Hide();
            }
        }
        int ctr = 0;
        private void button12_Click(object sender, EventArgs e)
        {
            settings.BringToFront();
            notificationPanel.Visible = false;
            ctr++;
            ctr1++;
            settingsTransition.ShowSync(settings);
            if (ctr % 2 == 0)
            {
                settings.Visible = false;
            }
            else
            {
                settings.Visible = true;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        private void panel11_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel30_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label119_Click(object sender, EventArgs e)
        {

        }
        int ctr1 = 0;
        private void btnNotification_Click(object sender, EventArgs e)
        {
            notificationPanel.BringToFront();
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

        private void label31_Click(object sender, EventArgs e)
        {

        }

        private void textBox29_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox30_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox30.Checked)
            {
                textBox30.Text = "";
                textBox30.BorderStyle = BorderStyle.None;
                textBox30.ReadOnly = true;
            }
            else
            {
                textBox30.BorderStyle = BorderStyle.FixedSingle;
                textBox30.ReadOnly = false;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            
            int patientno = 0;
            int demno = 0;
            patientno = Convert.ToInt32(label16.Text);
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from patient_demtbl where Patient_No = '" + patientno + "'", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    demno = dataReader.GetInt32("Dem_No");
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            //connection.Open();
            //MySqlCommand cmd8 = new MySqlCommand("SELECT * from patienttbl where RTRIM(CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)) = '" + patient + "'", connection);
            //MySqlDataReader dataReader8 = cmd8.ExecuteReader();
            //while (dataReader8.Read())
            //{
            //    patientno = dataReader8.GetInt32("Patient_No");
            //    haspatient = true;
            //}
            //connection.Close();
            //if (haspatient == false)
            //{
            //    check = true;
            //    checker = true;
            //}
            string user = label30.Text;
            string skintypes = "", acnes = "", hyperpigments = "", warts = "", recommendations = "";
            List<string> skintypearray = new List<string>();
            List<string> acnearray = new List<string>();
            List<string> hyperpigmentarray = new List<string>();
            List<string> wartsarray = new List<string>();
            string allergies = "", frownlines = "", finelines = "", wrinkles = "", sagging = "";
            allergies = textBox47.Text.Trim();
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString();
            directory = directory.Replace("\\", "/");
            string demimagepath = "",savedemimagepath="";
            Bitmap bmp = new Bitmap(pictureBox1.ClientSize.Width, pictureBox1.ClientSize.Height);
            pictureBox1.DrawToBitmap(bmp, pictureBox1.ClientRectangle);
            try
            {
                Directory.CreateDirectory(directory + "/SMPIWBC/DemPic");
            }
            catch (Exception me) { MessageBox.Show(me.Message); }
            demimagepath = directory + "/SMPIWBC/DemPic/Dem" + demno + ".jpg";
            if (File.Exists(demimagepath))
            {
                File.Delete(demimagepath);
            }
            bmp.Save(demimagepath);
            savedemimagepath = demimagepath.Replace("/", ",");
            if (checkBox1.Checked)
            {
                frownlines = "Have";
            }
            else
            {
                frownlines = "None";
            }
            if (checkBox2.Checked)
            {
                finelines = "Have";
            }
            else
            {
                finelines = "None";
            }
            if (checkBox23.Checked)
            {
                wrinkles = "Have";
            }
            else
            {
                wrinkles = "None";
            }
            if (checkBox24.Checked)
            {
                sagging = "Have";
            }
            else
            {
                sagging = "None";
            }
            if (checkBox3.Checked)
            {
                skintypearray.Add("Oily");
            }
            if (checkBox4.Checked)
            {
                skintypearray.Add("Sensitive");
            }
            if (checkBox5.Checked)
            {
                skintypearray.Add("Dry");
            }
            if (checkBox6.Checked)
            {
                skintypearray.Add("Pigmented");
            }
            if (checkBox7.Checked)
            {
                skintypearray.Add("Combination");
            }
            if (checkBox8.Checked)
            {
                acnearray.Add("Blackheads");
            }
            if (checkBox9.Checked)
            {
                acnearray.Add("Whiteheads");
            }
            if (checkBox12.Checked)
            {
                wartsarray.Add("Neck");
            }
            if (checkBox13.Checked)
            {
                wartsarray.Add("Face");
            }
            if (checkBox14.Checked)
            {
                wartsarray.Add("Upper Body");
            }
            if (checkBox15.Checked)
            {
                wartsarray.Add("Lower Body");
            }
            if (checkBox16.Checked)
            {
                wartsarray.Add("Upper Back");
            }
            if (checkBox17.Checked)
            {
                wartsarray.Add("Lower Back");
            }
            if (checkBox18.Checked)
            {
                hyperpigmentarray.Add(checkBox18.Text);
            }
            if (checkBox19.Checked)
            {
                hyperpigmentarray.Add(checkBox19.Text);
            }
            if (checkBox20.Checked)
            {
                hyperpigmentarray.Add(checkBox20.Text);
            }
            if (checkBox21.Checked)
            {
                hyperpigmentarray.Add(checkBox21.Text);
            }
            if (checkBox22.Checked)
            {
                hyperpigmentarray.Add(checkBox22.Text);
            }
            foreach (var stype in skintypearray)
            {
                skintypes += stype + ", ";
            }
            try
            {
                skintypes = skintypes.Substring(0, skintypes.Length - 2);
            }
            catch (Exception)
            {
                skintypes = "";
            }
            foreach (var acne in acnearray)
            {
                acnes += acne + ", ";
            }
            try
            {
                acnes = acnes.Substring(0, acnes.Length - 2);
            }
            catch (Exception)
            {
                acnes = "";
            }
            foreach (var hp in hyperpigmentarray)
            {
                hyperpigments += hp + ", ";
            }
            try
            {
                hyperpigments = hyperpigments.Substring(0, hyperpigments.Length - 2);
            }
            catch (Exception)
            {
                hyperpigments = "";
            }
            foreach (var wart in wartsarray)
            {
                warts += wart + ", ";
            }
            try
            {
                warts = warts.Substring(0, warts.Length - 2);
            }
            catch (Exception)
            {
                warts = "";
            }
            recommendations = richTextBox1.Text.Trim();

            try
            {
                connection.Open();
                MySqlCommand cmd1 = new MySqlCommand("Update patient_demtbl set Dem_Allergies = '" + allergies + "', Dem_Frownlines = '" + frownlines + "', Dem_Wrinkles = '" + wrinkles + "', Dem_Sagging = '" + sagging + "', Dem_SkinType = '" + skintypes + "' ,Dem_Acne = '" + acnes + "' , Dem_Warts = '" + warts + "', Dem_Hy = '" + hyperpigments + "', Dem_Reco = '" + recommendations + "', Dem_Picture = '" + savedemimagepath + "' where Dem_No = '" + demno + "'", connection);
                cmd1.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }

            
        }

        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {

        }
    }
}
