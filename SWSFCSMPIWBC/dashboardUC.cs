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
    public partial class dashboardUC : UserControl
    {
        static string connectionString =
       System.Configuration.ConfigurationManager.
       ConnectionStrings["SWSFCSMPIWBC.Properties.Settings.slimmersdbConnectionString"].ConnectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);
        public dashboardUC()
        {
            InitializeComponent();
            GraphLoad();
            GetPatients();
            GetArrivalRate();
            GetPromo();
            GetTop();
        }
        public dashboardUC(string label)
        {
            InitializeComponent();
            GraphLoad();
            GetPatients();
            GetArrivalRate();
            GetTop();
            GetPromo();
            this.userLabel = label;
            GetMyPatients(label);
            timer1.Start();
        }
        public string userLabel
        {
            get;
            set;
        }
        public HomePage ParentForm { get; set; }
        public void GetPatients()
        {
            int newpatients = 0, existing = 0, all = 0;
            string month = DateTime.Now.ToString("MM");
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*),MONTH(Date_Registered) from patienttbl where MONTH(Date_Registered) = '" + month + "'", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    newpatients = dataReader.GetInt32("COUNT(*)");
                    bunifuCustomLabel10.Text = newpatients.ToString();
                }
                connection.Close();

                connection.Open();
                MySqlCommand cmd1 = new MySqlCommand("SELECT COUNT(*) from patienttbl", connection);
                MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                while (dataReader1.Read())
                {
                    all = dataReader1.GetInt32("COUNT(*)");
                    bunifuCustomLabel9.Text = all.ToString();
                }
                connection.Close();

                existing = all - newpatients;
                bunifuCustomLabel11.Text = existing.ToString();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }
        public void GetArrivalRate()
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            int max = 0, arrived = 0, rate = 0, ans = 0;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("Select COUNT(*) from appointmenttbl where Appointment_Date = '" + date + "'", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    max = dataReader.GetInt32("COUNT(*)");
                }
                connection.Close();

                connection.Open();
                MySqlCommand cmd1 = new MySqlCommand("SELECT COUNT(*) from appointmenttbl where Appointment_Date = '" + date + "' and (Appointment_Status = 'On Going' or Appointment_Status = 'Done')", connection);
                MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                while (dataReader1.Read())
                {
                    arrived = dataReader1.GetInt32("Count(*)");
                }
                connection.Close();
                try
                {
                    ans = arrived / max;
                }
                catch (Exception)
                {
                    ans = 0;
                }
                rate = ans * 100;
                bunifuCircleProgressbar1.Value = rate;
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }
        public void GetPromo()
        {
            int month = Convert.ToInt32(DateTime.Now.ToString("MM"));
            int day = Convert.ToInt32(DateTime.Now.ToString("dd"));
            int year = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
            int startmonth = 0, startday = 0, startyear = 0, endmonth = 0, endday = 0, endyear = 0;
            string promo = "No available promo";
            string status = "", currentstatus = "";
            List<int> promoList = new List<int>();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT *,YEAR(Promo_Start),MONTH(Promo_Start),DAY(Promo_Start),YEAR(Promo_End),MONTH(Promo_End),DAY(Promo_End) from service_promotbl sp, discount_servicestbl ds,servicetbl s where sp.Promo_No = ds.Promo_No and ds.Service_No = s.Service_No and sp.Promo_Status <> 'Done'", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    startmonth = dataReader.GetInt32("MONTH(Promo_Start)");
                    startday = dataReader.GetInt32("DAY(Promo_Start)");
                    startyear = dataReader.GetInt32("YEAR(Promo_Start)");
                    endmonth = dataReader.GetInt32("MONTH(Promo_End)");
                    endday = dataReader.GetInt32("DAY(Promo_End)");
                    endyear = dataReader.GetInt32("YEAR(Promo_End)");
                    currentstatus = dataReader.GetString("Promo_Status");
                    if (((year == startyear && month == startmonth && day >= startday) && ((year == endyear && month == endmonth && day <= endday) || (year == endyear && endmonth > month)) || (year > startyear && year < endyear)) && currentstatus == "Pending")
                    {
                        status = "On Going";
                        promoList.Add(dataReader.GetInt32("Promo_No"));
                    }
                    else if (((year == startyear && month == startmonth && day < startday) || (year < startyear) || (year == startyear && month < startmonth)) && currentstatus == "On Going")
                    {
                        status = "Pending";
                        promoList.Add(dataReader.GetInt32("Promo_No"));
                    }
                    else if(((year == endyear && month == endmonth && day > endday) || (year == endyear && month > endmonth) || (year > endyear)) && currentstatus != "Done")
                    {
                        status = "Done";
                        promoList.Add(dataReader.GetInt32("Promo_No"));
                    }
                }
                connection.Close();
                for (int x = 0; x < promoList.Count; x++)
                {
                    MessageBox.Show(promoList[x].ToString());
                    connection.Open();
                    MySqlCommand cmd1 = new MySqlCommand("Update service_promotbl set Promo_Status = '" + status + "' where Promo_No = '" + promoList[x] + "'", connection);
                    cmd1.ExecuteNonQuery();
                    connection.Close();
                }

                connection.Open();
                MySqlCommand cmd2 = new MySqlCommand("SELECT *,YEAR(Promo_Start),MONTH(Promo_Start),DAY(Promo_Start),YEAR(Promo_End),MONTH(Promo_End),DAY(Promo_End) from service_promotbl sp, discount_servicestbl ds,servicetbl s where sp.Promo_No = ds.Promo_No and ds.Service_No = s.Service_No and sp.Promo_Status = 'On Going'", connection);
                MySqlDataReader dataReader2 = cmd2.ExecuteReader();
                while (dataReader2.Read())
                {
                    promo = "";
                    promo += dataReader2.GetString("Promo_Description") + "\n";
                }
                bunifuCustomLabel13.Text = promo;
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }

       

        public void GetTop()
        {
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT s.Service_Name,COUNT(*) from appointmenttbl a, servicetbl s where (Appointment_Status = 'Done' or Appointment_Status = 'On Going') and a.Service_No = s.Service_No group by a.Service_No order by COUNT(*) desc LIMIT 1", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    label1.Text = dataReader.GetString("Service_Name");
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }
        public void GetMyPatients(string user)
        {
            label22.Text = "";
            dataGridView1.Rows.Clear();
            int no = 0;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT *,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from patienttbl p, accounttbl a, employeetbl e, employee_patienttbl ep, patient_waitlisttbl pw where a.Username='" + user + "' and a.Employee_No = e.Employee_No and e.Employee_No = ep.Employee_No and p.Patient_No = ep.Patient_No and ep.Employee_Patient_No = pw.Employee_Patient_No and pw.Waiting_Status <> 'Done' order by Waiting_No,Waiting_Status", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    no++;
                    dataGridView1.Rows.Add(no, dataReader.GetInt32("Patient_No"), dataReader.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)"), dataReader.GetString("Waiting_Status"));
                }
                connection.Close();
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
            }
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            int patientnum = Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1].Value);
            string status = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[3].Value.ToString();
            string patientname = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[2].Value.ToString();
            if (status.Equals("Not Started"))
            {
                MessageBox.Show("Patient is not yet queued");
            }
            else
            {
                ParentForm.Patient = patientname;
                ParentForm.PatientNo = patientnum;
                OnCloseButtonClicked(e);
            }

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
        public void GraphLoad()
        {
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT s.Service_Name,COUNT(*) from appointmenttbl a, servicetbl s where (Appointment_Status = 'Done' or Appointment_Status = 'On Going') and a.Service_No = s.Service_No group by a.Service_No order by COUNT(*)", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    chart1.Series.Add(dataReader.GetString("Service_Name"));
                    chart1.Series[dataReader.GetString("Service_Name")].Points.AddY(dataReader.GetInt32("COUNT(*)"));
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            
            //chart1.Series["Laser"].Points.AddXY("4", 200);
            //chart1.Series["Warts Removal"].Points.AddXY("7", 400);
            //chart1.Series["Diamond Peel"].Points.AddXY("9", 80);
            //chart1.Series["Tightening"].Points.AddXY("12", 260);
            //chart1.Series["Acne Solution"].Points.AddXY("20", 170);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }

        private void dashboardUC_VisibleChanged(object sender, EventArgs e)
        {
            
        }
    }
}
