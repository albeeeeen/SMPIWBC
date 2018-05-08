using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using Bunifu.Framework.UI;

namespace SWSFCSMPIWBC
{
    public partial class Patients : Form
    {
        static string connectionString = "datasource=localhost" + ";" + "DATABASE=slimmersdb" + ";" + "UID=root"
        + ";" + "PASSWORD=root" + ";";
        MySqlConnection connection = new MySqlConnection(connectionString);
        
        public Patients()
        {
            InitializeComponent();
            GetAllPatients();
            initTime();
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
                                HomePage hp = new HomePage();
                                smUC sm = new smUC();
                                user = label5.Text;
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
                                user = label5.Text;
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
                        user = label5.Text;
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
                        user = label5.Text;
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
        public void GetAllPatients()
        {
            dataGridView1.Rows.Clear();
            try
            {
                connection.Open();
                string query = "SELECT *,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from patienttbl order by Patient_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView1.Rows.Add(dataReader.GetInt32("Patient_No"), dataReader.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)"), dataReader.GetString("Patient_Birthdate"), dataReader.GetString("Patient_ContactNo"), dataReader.GetString("Patient_Address"));
                }

                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
        }
        private void textBox24_TextChanged(object sender, EventArgs e)
        {
            string search = textBox24.Text.Trim();
            dataGridView1.Rows.Clear();
            try
            {
                connection.Open();
                string query = "SELECT *,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from patienttbl where Patient_LName LIKE '%" + search + "%' OR Patient_FName LIKE '%" + search + "%' order by Patient_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView1.Rows.Add(dataReader.GetInt32("Patient_No"), dataReader.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)"), dataReader.GetString("Patient_Birthdate"), dataReader.GetString("Patient_ContactNo"), dataReader.GetString("Patient_Address"));
                }

                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            string user = label5.Text;
            string position = "";
            try
            {
                connection.Open();
                MySqlCommand cmd1 = new MySqlCommand("SELECT * from accounttbl a, employeetbl e, employee_positiontbl ep where a.Username = '" + user + "' and a.Employee_no = e.Employee_No and e.Employee_Position_No = ep.Employee_Position_No", connection);
                MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                while (dataReader1.Read())
                {
                    position = dataReader1.GetString("Position_Name");
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            int row = dataGridView1.CurrentCell.RowIndex;
            int patientno = Convert.ToInt32(dataGridView1.Rows[row].Cells[0].Value);

            Profile profile = new Profile();

            profile.label2.Text = patientno.ToString();
            if (position == "Consultant")
            {
                string skintypes = "",acnes = "",warts = "",hyper = "";
                try
                {
                    connection.Open();
                    string query = "SELECT *,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from patienttbl p, patient_demtbl pd where p.Patient_No = '" + patientno + "' and p.Patient_No = pd.Patient_No";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        skintypes = dataReader.GetString("Dem_SkinType");
                        acnes = dataReader.GetString("Dem_Acne");
                        hyper = dataReader.GetString("Dem_Hy");
                        warts = dataReader.GetString("Dem_Warts");
                        profile.label16.Text = dataReader.GetInt32("Patient_No").ToString();
                        profile.label17.Text = dataReader.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)");
                        profile.pictureBox1.Image = Image.FromFile(dataReader.GetString("Dem_Picture"));
                        if (skintypes.Contains("Oily"))
                        {
                            profile.checkBox3.Checked = true;
                        }
                        if (skintypes.Contains("Sensitive"))
                        {
                            profile.checkBox4.Checked = true;
                        }
                        if (skintypes.Contains("Dry"))
                        {
                            profile.checkBox5.Checked = true;
                        }
                        if (skintypes.Contains("Pigmented"))
                        {
                            profile.checkBox6.Checked = true;
                        }
                        if (skintypes.Contains("Combination"))
                        {
                            profile.checkBox7.Checked = true;
                        }
                        if (dataReader.GetString("Dem_Frownlines") == "Have")
                        {
                            profile.checkBox1.Checked = true;
                        }
                        if (dataReader.GetString("Dem_Finelines") == "Have")
                        {
                            profile.checkBox2.Checked = true;
                        }
                        if (dataReader.GetString("Dem_Wrinkles") == "Have")
                        {
                            profile.checkBox23.Checked = true;
                        }
                        if (dataReader.GetString("Dem_Sagging") == "Have")
                        {
                            profile.checkBox24.Checked = true;
                        }
                        profile.textBox47.Text = dataReader.GetString("Dem_Allergies");
                        if (acnes.Contains("Blackheads"))
                        {
                            profile.checkBox8.Checked = true;
                        }
                        if (acnes.Contains("Whiteheads"))
                        {
                            profile.checkBox9.Checked = true;
                        }
                        if (hyper.Contains("Sunspots"))
                        {
                            profile.checkBox18.Checked = true;
                        }
                        if (hyper.Contains("Sunspots 1"))
                        {
                            profile.checkBox19.Checked = true;
                        }
                        if (hyper.Contains("Sunspots 2"))
                        {
                            profile.checkBox20.Checked = true;
                        }
                        if (hyper.Contains("Sunspots 3"))
                        {
                            profile.checkBox21.Checked = true;
                        }
                        if (hyper.Contains("Sunspots 4"))
                        {
                            profile.checkBox22.Checked = true;
                        }
                        if (warts.Contains("Face"))
                        {
                            profile.checkBox12.Checked = true;
                        }
                        if (warts.Contains("Upper Body"))
                        {
                            profile.checkBox13.Checked = true;
                        }
                        if (warts.Contains("Upper Back"))
                        {
                            profile.checkBox14.Checked = true;
                        }
                        if (warts.Contains("Neck"))
                        {
                            profile.checkBox15.Checked = true;
                        }
                        if (warts.Contains("Lower Body"))
                        {
                            profile.checkBox16.Checked = true;
                        }
                        if (warts.Contains("Lower Back"))
                        {
                            profile.checkBox17.Checked = true;
                        }
                        profile.richTextBox1.Text = dataReader.GetString("Dem_Reco");
                    }
                    connection.Close();
                }
                catch (MySqlException me)
                {
                    MessageBox.Show(me.Message);
                }
                profile.panel11.Enabled = true;
                profile.panel11.Show();
                profile.panel5.Hide();
                profile.panel6.Hide();
                profile.panel5.Enabled = false;
                profile.panel6.Enabled = false;
                profile.label48.Text = user;
                profile.button18.Hide();
                profile.button17.Show();
                profile.button17.Location = new Point(0, 92);
                profile.Show();
                this.Hide();
            }
            else if (position == "Receptionist")
            {
                string risks = "";
                connection.Open();
                MySqlCommand cmd2 = new MySqlCommand("SELECT * from patienttbl p , patient_medicaltbl pm where p.Patient_No = '" + patientno + "' and p.Patient_No = pm.Patient_No", connection);
                MySqlDataReader dataReader2 = cmd2.ExecuteReader();
                while (dataReader2.Read())
                {
                    risks = dataReader2.GetString("Risk_Factors");
                    profile.textBox14.Text = dataReader2.GetInt32("Patient_No").ToString();
                    profile.textBox1.Text = dataReader2.GetString("Patient_LName");
                    profile.textBox2.Text = dataReader2.GetString("Patient_FName");
                    profile.textBox3.Text = dataReader2.GetString("Patient_MidInit");
                    if (dataReader2.GetString("Patient_Gender") == "Male")
                    {
                        profile.radioButton1.Checked = true;
                    }
                    else
                    {
                        profile.radioButton2.Checked = true;
                    }
                    profile.dateTimePicker1.Value = Convert.ToDateTime(dataReader2.GetString("Patient_Birthdate"));
                    profile.textBox5.Text = dataReader2.GetInt32("Patient_Age").ToString();
                    profile.textBox4.Text = dataReader2.GetInt64("Patient_ContactNo").ToString();
                    profile.textBox11.Text = dataReader2.GetString("Patient_Email");
                    profile.textBox10.Text = dataReader2.GetString("Patient_Occupation");
                    if (dataReader2.GetString("Patient_CStatus") == "Single")
                    {
                        profile.radioButton3.Checked = true;
                    }
                    else if (dataReader2.GetString("Patient_CStatus") == "Married")
                    {
                        profile.radioButton4.Checked = true;
                    }
                    else if (dataReader2.GetString("Patient_CStatus") == "Widowed")
                    {
                        profile.radioButton7.Checked = true;
                    }
                    else if (dataReader2.GetString("Patient_CStatus") == "Others")
                    {
                        profile.radioButton8.Checked = true;
                    }
                    profile.textBox6.Text = dataReader2.GetString("Patient_Address");
                    profile.comboBox2.Text = dataReader2.GetString("Patient_Status");
                    profile.textBox29.Text = dataReader2.GetString("Height");
                    profile.textBox28.Text = dataReader2.GetString("Weight");
                    profile.comboBox1.Text = dataReader2.GetString("Body_Frame");
                    profile.textBox27.Text = dataReader2.GetInt32("Body_Fat").ToString();
                    profile.textBox25.Text = dataReader2.GetString("Blood_Pressure");
                    profile.textBox24.Text = dataReader2.GetInt32("Pulse_Rate").ToString();
                    if (risks.Contains("Alcohol Drinker"))
                    {
                        profile.radioButton12.Checked = true;
                    }
                    else if (risks.Contains("Not Alcohol Drinker"))
                    {
                        profile.radioButton11.Checked = false;
                    }
                    if (risks.Contains("Smoker"))
                    {
                        profile.radioButton14.Checked = true;
                    }
                    else if (risks.Contains("Non Smoker"))
                    {
                        profile.radioButton13.Checked = true;
                    }
                    if (risks.Contains("Diabetes"))
                    {
                        profile.checkBox37.Checked = true;
                    }
                    if (risks.Contains("Allergy"))
                    {
                        profile.checkBox36.Checked = true;
                    }
                    if (risks.Contains("Heart Disease"))
                    {
                        profile.checkBox35.Checked = true;
                    }
                    if (risks.Contains("Pace Maker"))
                    {
                        profile.checkBox34.Checked = true;
                    }
                    if (risks.Contains("Seizures"))
                    {
                        profile.checkBox33.Checked = true;
                    }
                    if (risks.Contains("Headaches"))
                    {
                        profile.checkBox32.Checked = true;
                    }
                    if (risks.Contains("Chest Pains"))
                    {
                        profile.checkBox31.Checked = true;
                    }
                    profile.textBox31.Text = dataReader2.GetString("Other_MedHist");
                    profile.textBox30.Text = dataReader2.GetString("Other_Risks");
                    if (string.IsNullOrEmpty(profile.textBox30.Text))
                    {
                        profile.checkBox30.Checked = false;
                    }
                    else
                    {
                        profile.checkBox30.Checked = true;
                    }
                    if (risks.Contains("Dizziness"))
                    {
                        profile.radioButton5.Checked = true;
                    }
                    if (risks.Contains("Asthma"))
                    {
                        profile.radioButton15.Checked = true;
                    }
                    if (risks.Contains("Nausea"))
                    {
                        profile.radioButton17.Checked = true;
                    }
                    if (risks.Contains("Arthritis"))
                    {
                        profile.radioButton19.Checked = true;
                    }
                    if (risks.Contains("Bladder Problems"))
                    {
                        profile.radioButton21.Checked = true;
                    }
                    if (risks.Contains("Cancer"))
                    {
                        profile.radioButton23.Checked = true;
                    }
                    if (risks.Contains("Ringing Ears"))
                    {
                        profile.radioButton25.Checked = true;
                    }
                    if (risks.Contains("Thyroid Conditions"))
                    {
                        profile.radioButton27.Checked = true;
                    }
                }
                connection.Close();
                profile.panel6.Show();
                profile.panel11.Hide();
                profile.panel5.Hide();
                profile.panel11.Enabled = false;
                profile.panel6.Enabled = true;
                profile.panel5.Enabled = true;
                profile.label48.Text = user;
                profile.button17.Hide();
                profile.Show();
                this.Hide();
            }
            else
            {
                string risks = "";
                connection.Open();
                MySqlCommand cmd2 = new MySqlCommand("SELECT * from patienttbl p , patient_medicaltbl pm where p.Patient_No = '" + patientno + "' and p.Patient_No = pm.Patient_No", connection);
                MySqlDataReader dataReader2 = cmd2.ExecuteReader();
                while (dataReader2.Read())
                {
                    risks = dataReader2.GetString("Risk_Factors");
                    profile.textBox14.Text = dataReader2.GetInt32("Patient_No").ToString();
                    profile.textBox1.Text = dataReader2.GetString("Patient_LName");
                    profile.textBox2.Text = dataReader2.GetString("Patient_FName");
                    profile.textBox3.Text = dataReader2.GetString("Patient_MidInit");
                    if (dataReader2.GetString("Patient_Gender") == "Male")
                    {
                        profile.radioButton1.Checked = true;
                    }
                    else
                    {
                        profile.radioButton2.Checked = true;
                    }
                    profile.dateTimePicker1.Value = Convert.ToDateTime(dataReader2.GetString("Patient_Birthdate"));
                    profile.textBox5.Text = dataReader2.GetInt32("Patient_Age").ToString();
                    profile.textBox4.Text = dataReader2.GetInt64("Patient_ContactNo").ToString();
                    profile.textBox11.Text = dataReader2.GetString("Patient_Email");
                    profile.textBox10.Text = dataReader2.GetString("Patient_Occupation");
                    if (dataReader2.GetString("Patient_CStatus") == "Single")
                    {
                        profile.radioButton3.Checked = true;
                    }
                    else if (dataReader2.GetString("Patient_CStatus") == "Married")
                    {
                        profile.radioButton4.Checked = true;
                    }
                    else if (dataReader2.GetString("Patient_CStatus") == "Widowed")
                    {
                        profile.radioButton7.Checked = true;
                    }
                    else if (dataReader2.GetString("Patient_CStatus") == "Others")
                    {
                        profile.radioButton8.Checked = true;
                    }
                    profile.textBox6.Text = dataReader2.GetString("Patient_Address");
                    profile.comboBox2.Text = dataReader2.GetString("Patient_Status");
                    profile.textBox29.Text = dataReader2.GetString("Height");
                    profile.textBox28.Text = dataReader2.GetString("Weight");
                    profile.comboBox1.Text = dataReader2.GetString("Body_Frame");
                    profile.textBox27.Text = dataReader2.GetInt32("Body_Fat").ToString();
                    profile.textBox25.Text = dataReader2.GetString("Blood_Pressure");
                    profile.textBox24.Text = dataReader2.GetInt32("Pulse_Rate").ToString();
                    if (risks.Contains("Alcohol Drinker"))
                    {
                        profile.radioButton12.Checked = true;
                    }
                    else if (risks.Contains("Not Alcohol Drinker"))
                    {
                        profile.radioButton11.Checked = false;
                    }
                    if (risks.Contains("Smoker"))
                    {
                        profile.radioButton14.Checked = true;
                    }
                    else if (risks.Contains("Non Smoker"))
                    {
                        profile.radioButton13.Checked = true;
                    }
                    if (risks.Contains("Diabetes"))
                    {
                        profile.checkBox37.Checked = true;
                    }
                    if (risks.Contains("Allergy"))
                    {
                        profile.checkBox36.Checked = true;
                    }
                    if (risks.Contains("Heart Disease"))
                    {
                        profile.checkBox35.Checked = true;
                    }
                    if (risks.Contains("Pace Maker"))
                    {
                        profile.checkBox34.Checked = true;
                    }
                    if (risks.Contains("Seizures"))
                    {
                        profile.checkBox33.Checked = true;
                    }
                    if (risks.Contains("Headaches"))
                    {
                        profile.checkBox32.Checked = true;
                    }
                    if (risks.Contains("Chest Pains"))
                    {
                        profile.checkBox31.Checked = true;
                    }
                    profile.textBox31.Text = dataReader2.GetString("Other_MedHist");
                    profile.textBox30.Text = dataReader2.GetString("Other_Risks");
                    if (string.IsNullOrEmpty(profile.textBox30.Text))
                    {
                        profile.checkBox30.Checked = false;
                    }
                    else
                    {
                        profile.checkBox30.Checked = true;
                    }
                    if (risks.Contains("Dizziness"))
                    {
                        profile.radioButton5.Checked = true;
                    }
                    if (risks.Contains("Asthma"))
                    {
                        profile.radioButton15.Checked = true;
                    }
                    if (risks.Contains("Nausea"))
                    {
                        profile.radioButton17.Checked = true;
                    }
                    if (risks.Contains("Arthritis"))
                    {
                        profile.radioButton19.Checked = true;
                    }
                    if (risks.Contains("Bladder Problems"))
                    {
                        profile.radioButton21.Checked = true;
                    }
                    if (risks.Contains("Cancer"))
                    {
                        profile.radioButton23.Checked = true;
                    }
                    if (risks.Contains("Ringing Ears"))
                    {
                        profile.radioButton25.Checked = true;
                    }
                    if (risks.Contains("Thyroid Conditions"))
                    {
                        profile.radioButton27.Checked = true;
                    }
                }
                connection.Close();
                profile.panel6.Show();
                profile.panel11.Hide();
                profile.panel5.Hide();
                profile.panel11.Enabled = false;
                profile.panel6.Enabled = true;
                profile.panel5.Enabled = true;
                profile.label48.Text = user;
                profile.Show();
                this.Hide();
            }
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
            label14.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }
        

        private void button8_Click(object sender, EventArgs e)
        {
            string userlog = label5.Text;
            Login login = new Login();
            login.CheckUser(userlog);
            
            this.Hide();
        }

       

        private void button13_Click(object sender, EventArgs e)
        {
            string userlog = label5.Text;
            // HomePage hp = new HomePage();
            //hp.label5.Text = userlog;
            Patients patient = new Patients();
            patient.label5.Text = userlog;
            patient.Show();
            //patientsUC1.Show();
            string pos = "";
            try
            {
                connection.Open();
                MySqlCommand cmd1 = new MySqlCommand("SELECT * from accounttbl a, employeetbl e, employee_positiontbl ep where a.Username = '" + userlog + "' and a.Employee_No = e.Employee_No and e.Employee_Position_No = ep.Employee_Position_No", connection);
                MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                while (dataReader1.Read())
                {
                    pos = dataReader1.GetString("Position_Name");
                }
                connection.Close();
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
            }
            if (pos == "Receptionist")
            {
                patient.mainteBtn.Visible = false;
                patient.button1.Location = new Point(0, 290);
                patient.button10.Location = new Point(0, 343);
                patient.button3.Visible = false;
                patient.button12.Visible = false;
            }
            else if (pos == "Consultant")
            {
                patient.mainteBtn.Visible = false;
                patient.button1.Location = new Point(0, 290);
                patient.button10.Visible = false;
                patient.button3.Visible = false;
                patient.button12.Visible = false;
            }
            this.Hide();
        }

        private void mainteBtn_click(object sender, EventArgs e)
        {
            string user = label5.Text;
            Services s = new Services();
            s.label5.Text = user;
            s.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string user = label5.Text;
            regUC r = new regUC();
            r.label30.Text = user;
            r.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string user = label5.Text;
            ServiceMonitoring sm = new ServiceMonitoring();
            sm.label15.Text = user;
            sm.Show();
            this.Hide();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string user = label5.Text;
            Payment p = new Payment();
            p.label15.Text = user;
            p.Show();
            this.Hide();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string user = label5.Text;
            Inventory i = new Inventory();
            i.label15.Text = user;
            i.Show();
            this.Hide();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            string user = label5.Text;
            s r = new s();
            r.Show();
            this.Hide();
        }

        int ctr = 0;
        private void button16_Click(object sender, EventArgs e)
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

        private void button2_Click_1(object sender, EventArgs e)
        {
            string user = label5.Text;
            regUC r = new regUC();
            r.label30.Text = user;

            r.label30.Text = user;
            string user1 = r.label30.Text;

            if (user == "admin")
            {
                r.button17.Hide();
                r.button17.Location = new Point(0, 192);
                r.panel31.BringToFront();
                r.panel34.SendToBack();
                r.panel25.SendToBack();
            }
            else
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * from accounttbl a, employeetbl e, employee_positiontbl ep where a.Username = '" + user + "' and a.Employee_No = e.Employee_No and e.Employee_Position_No = ep.Employee_Position_No", connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        if (dataReader.GetString("Position_Name") == "Consultant")
                        {
                            r.button18.Hide();
                            r.button17.Location = new Point(0, 192);
                            r.panel25.BringToFront();
                            r.panel31.SendToBack();
                            r.panel34.SendToBack();
                            r.GetConsultPatient();
                        }
                        else if (dataReader.GetString("Position_Name") == "Receptionist")
                        {
                            r.button17.Hide();
                            r.button18.Location = new Point(0, 192);
                            r.panel31.BringToFront();
                            r.panel34.SendToBack();
                            r.panel25.SendToBack();
                        }

                    }
                    connection.Close();
                }
                catch (Exception me)
                {
                    MessageBox.Show(me.Message);
                }
            }
            r.Show();
            this.Hide();
        }

        private void mainteBtn_Click_1(object sender, EventArgs e)
        {
            string user = label5.Text;
            Services s = new Services();
            s.label5.Text = user;
            s.Show();
            this.Hide();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string user = label5.Text;
            ServiceMonitoring sm = new ServiceMonitoring();
            sm.label15.Text = user;
            sm.Show();
            this.Hide();

        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            string user = label5.Text;
            Payment p = new Payment();
            p.label15.Text = user;
            p.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string user = label5.Text;
            Inventory i = new Inventory();
            i.label15.Text = user;
            i.Show();
            this.Hide();
        }

        private void button12_Click_1(object sender, EventArgs e)
        {
            string user = label5.Text;
            s r = new s();
            r.Show();
            this.Hide();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        private void menu_Click(object sender, EventArgs e)
        {
            
            if (panel3.Width == 54)
            {
                //expand
                logo1.Visible = false;
                panel3.Visible = false;
                panel3.Width = 211;
                PanelTransition.ShowSync(panel3);
                LogoTransition.ShowSync(logo);
            }
            else
            {
                //minimize
                logo1.Visible = false;
                logo.Hide();
                panel3.Visible = false;
                panel3.Width = 54;
                Panel2Transition.ShowSync(panel3);
                Logo1Transition.ShowSync(logo1);

            }
        }

        private void Patients_Load(object sender, EventArgs e)
        {
                
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
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
    }
}
