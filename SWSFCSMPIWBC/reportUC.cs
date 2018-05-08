using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace SWSFCSMPIWBC
{
    public partial class reportUC : UserControl
    {
        static string connectionString =
       System.Configuration.ConfigurationManager.
       ConnectionStrings["SWSFCSMPIWBC.Properties.Settings.slimmersdbConnectionString"].ConnectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);
        public reportUC()
        {
            InitializeComponent();
            salesPanel.BringToFront();
            salesPanel.Show();
            productFilter.Show();
            productFilter.BringToFront();
            productViewer.Show();
            productViewer.BringToFront();
            string file = Application.StartupPath + @"\Reports\DailySalesReportProduct.rpt";
            GetDaily(file);
        }
        public HomePage ParentForm { get; set; }
        public void GetYear()
        {
            cboYear.Items.Clear();
            comboBox2.Items.Clear();
            comboBox7.Items.Clear();
            string date = DateTime.Today.ToString("yyyy-MM-dd");
            int year = Convert.ToInt32(date.Substring(0, 4));
            for (int i = year; i >= 1960; i--)
            {
                cboYear.Items.Add(i.ToString());
                comboBox2.Items.Add(i.ToString());
                comboBox7.Items.Add(i.ToString());
            }
            try
            {
                cboYear.SelectedIndex = 0;
                cboMonth.SelectedIndex = 0;
                comboBox2.SelectedIndex = 0;
                comboBox3.SelectedIndex = 0;
                comboBox7.SelectedIndex = 0;
                comboBox8.SelectedIndex = 0;
            }
            catch (Exception)
            {
            }
        }
        public void GetDaily(string file)
        {
            try
            {
                ReportDocument cryRpt = new ReportDocument();
                cryRpt.Load(file);

                ConnectionInfo crConnectionInfo = new ConnectionInfo();

                crConnectionInfo.ServerName = "localhost";
                crConnectionInfo.DatabaseName = "slimmersdb";
                crConnectionInfo.UserID = "root";
                crConnectionInfo.Password = "root";

                productViewer.ReportSource = cryRpt;
                productViewer.RefreshReport();
            }
            catch (Exception me)
            {
            }
        }
        public void GetToday(string date,string file)
        {
            
            ReportDocument cryRpt = new ReportDocument();
            cryRpt.Load(file);

            ConnectionInfo crConnectionInfo = new ConnectionInfo();

            ParameterFieldDefinitions crParameterFieldDefinitions;
            ParameterFieldDefinition crParameterFieldDefinition;
            ParameterValues crParameterValues = new ParameterValues();
            ParameterDiscreteValue crParameterDiscreteValue = new ParameterDiscreteValue();

            crParameterDiscreteValue.Value = date;
            crParameterFieldDefinitions = cryRpt.DataDefinition.ParameterFields;
            crParameterFieldDefinition = crParameterFieldDefinitions["dateparam"];
            crParameterValues = crParameterFieldDefinition.CurrentValues;
            crParameterValues.Clear();
            crParameterValues.Add(crParameterDiscreteValue);
            crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

            crConnectionInfo.ServerName = "localhost";
            crConnectionInfo.DatabaseName = "slimmersdb";
            crConnectionInfo.UserID = "root";
            crConnectionInfo.Password = "root";

            productViewer.ReportSource = cryRpt;
            productViewer.RefreshReport();
        }
        public void GetWeekly(string file)
        {
            int year = Convert.ToInt32(cboYear.Text.Trim());
            string month = cboMonth.Text.Trim();
            ReportDocument cryRpt = new ReportDocument();
            cryRpt.Load(file);

            ConnectionInfo crConnectionInfo = new ConnectionInfo();

            ParameterFieldDefinitions crParameterFieldDefinitions;
            ParameterFieldDefinition crParameterFieldDefinition;
            ParameterValues crParameterValues = new ParameterValues();
            ParameterDiscreteValue crParameterDiscreteValue = new ParameterDiscreteValue();

            crParameterDiscreteValue.Value = year;
            crParameterFieldDefinitions = cryRpt.DataDefinition.ParameterFields;
            crParameterFieldDefinition = crParameterFieldDefinitions["yearparam"];
            crParameterValues = crParameterFieldDefinition.CurrentValues;
            crParameterValues.Clear();
            crParameterValues.Add(crParameterDiscreteValue);
            crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

            crParameterDiscreteValue.Value = month;
            crParameterFieldDefinitions = cryRpt.DataDefinition.ParameterFields;
            crParameterFieldDefinition = crParameterFieldDefinitions["monthparam"];
            crParameterValues = crParameterFieldDefinition.CurrentValues;
            crParameterValues.Clear();
            crParameterValues.Add(crParameterDiscreteValue);
            crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

            crConnectionInfo.ServerName = "localhost";
            crConnectionInfo.DatabaseName = "slimmersdb";
            crConnectionInfo.UserID = "root";
            crConnectionInfo.Password = "root";

            productViewer.ReportSource = cryRpt;
            productViewer.RefreshReport();
            filter = "";
            filter2 = "";
            panel2.Hide();
            productFilter.Enabled = true;
            serviceFilter.Enabled = true;
            bunifuThinButton21.Enabled = true;
            bunifuThinButton22.Enabled = true;
        }

        public void GetMonthly(string file)
        {
            int year = Convert.ToInt32(cboYear.Text.Trim());
            string month = cboMonth.Text.Trim();
            ReportDocument cryRpt = new ReportDocument();
            cryRpt.Load(file);

            ConnectionInfo crConnectionInfo = new ConnectionInfo();

            ParameterFieldDefinitions crParameterFieldDefinitions;
            ParameterFieldDefinition crParameterFieldDefinition;
            ParameterValues crParameterValues = new ParameterValues();
            ParameterDiscreteValue crParameterDiscreteValue = new ParameterDiscreteValue();

            crParameterDiscreteValue.Value = year;
            crParameterFieldDefinitions = cryRpt.DataDefinition.ParameterFields;
            crParameterFieldDefinition = crParameterFieldDefinitions["yearparam"];
            crParameterValues = crParameterFieldDefinition.CurrentValues;
            crParameterValues.Clear();
            crParameterValues.Add(crParameterDiscreteValue);
            crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

            crParameterDiscreteValue.Value = month;
            crParameterFieldDefinitions = cryRpt.DataDefinition.ParameterFields;
            crParameterFieldDefinition = crParameterFieldDefinitions["monthparam"];
            crParameterValues = crParameterFieldDefinition.CurrentValues;
            crParameterValues.Clear();
            crParameterValues.Add(crParameterDiscreteValue);
            crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

            crConnectionInfo.ServerName = "localhost";
            crConnectionInfo.DatabaseName = "slimmersdb";
            crConnectionInfo.UserID = "root";
            crConnectionInfo.Password = "root";

            productViewer.ReportSource = cryRpt;
            productViewer.RefreshReport();
            filter = "";
            filter2 = "";
            panel2.Hide();
            productFilter.Enabled = true;
            serviceFilter.Enabled = true;
            bunifuThinButton21.Enabled = true;
            bunifuThinButton22.Enabled = true;
        }

        public void GetAnnual(string file)
        {
            int year = Convert.ToInt32(cboYear.Text.Trim());
            ReportDocument cryRpt = new ReportDocument();
            cryRpt.Load(file);

            ConnectionInfo crConnectionInfo = new ConnectionInfo();

            ParameterFieldDefinitions crParameterFieldDefinitions;
            ParameterFieldDefinition crParameterFieldDefinition;
            ParameterValues crParameterValues = new ParameterValues();
            ParameterDiscreteValue crParameterDiscreteValue = new ParameterDiscreteValue();

            crParameterDiscreteValue.Value = year;
            crParameterFieldDefinitions = cryRpt.DataDefinition.ParameterFields;
            crParameterFieldDefinition = crParameterFieldDefinitions["yearparam"];
            crParameterValues = crParameterFieldDefinition.CurrentValues;
            crParameterValues.Clear();
            crParameterValues.Add(crParameterDiscreteValue);
            crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

            crConnectionInfo.ServerName = "localhost";
            crConnectionInfo.DatabaseName = "slimmersdb";
            crConnectionInfo.UserID = "root";
            crConnectionInfo.Password = "root";

            productViewer.ReportSource = cryRpt;
            productViewer.RefreshReport();
            filter = "";
            filter2 = "";
            panel2.Hide();
            productFilter.Enabled = true;
            serviceFilter.Enabled = true;
            bunifuThinButton21.Enabled = true;
            bunifuThinButton22.Enabled = true;
        }
        public void GetInventory(string file)
        {
            
            ReportDocument cryRpt = new ReportDocument();
            cryRpt.Load(file);

            ConnectionInfo crConnectionInfo = new ConnectionInfo();

            crConnectionInfo.ServerName = "localhost";
            crConnectionInfo.DatabaseName = "slimmersdb";
            crConnectionInfo.UserID = "root";
            crConnectionInfo.Password = "root";

            inventoryView.ReportSource = cryRpt;
            inventoryView.RefreshReport();
        }

        public void GetInventoryPerName()
        {
            string name = comboBox5.Text.Trim();
            ReportDocument cryRpt = new ReportDocument();
            cryRpt.Load(Application.StartupPath + @"\Reports\InventoryListReportPerProduct.rpt");

            ConnectionInfo crConnectionInfo = new ConnectionInfo();

            ParameterFieldDefinitions crParameterFieldDefinitions;
            ParameterFieldDefinition crParameterFieldDefinition;
            ParameterValues crParameterValues = new ParameterValues();
            ParameterDiscreteValue crParameterDiscreteValue = new ParameterDiscreteValue();

            crParameterDiscreteValue.Value = name;
            crParameterFieldDefinitions = cryRpt.DataDefinition.ParameterFields;
            crParameterFieldDefinition = crParameterFieldDefinitions["productparam"];
            crParameterValues = crParameterFieldDefinition.CurrentValues;
            crParameterValues.Clear();
            crParameterValues.Add(crParameterDiscreteValue);
            crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

            crConnectionInfo.ServerName = "localhost";
            crConnectionInfo.DatabaseName = "slimmersdb";
            crConnectionInfo.UserID = "root";
            crConnectionInfo.Password = "root";

            inventoryView.ReportSource = cryRpt;
            inventoryView.RefreshReport();
            inventfilter = "";
            panel5.Hide();
            inventoryFilter.Enabled = true;
            btnInventoryList.Enabled = true;
            btnPullOut.Enabled = true;
            btnRestock.Enabled = true;
        }

        public void GetInventoryPerType()
        {
            string name = comboBox5.Text.Trim();
            ReportDocument cryRpt = new ReportDocument();
            cryRpt.Load(Application.StartupPath + @"\Reports\InventoryListReportPerType.rpt");

            ConnectionInfo crConnectionInfo = new ConnectionInfo();

            ParameterFieldDefinitions crParameterFieldDefinitions;
            ParameterFieldDefinition crParameterFieldDefinition;
            ParameterValues crParameterValues = new ParameterValues();
            ParameterDiscreteValue crParameterDiscreteValue = new ParameterDiscreteValue();

            crParameterDiscreteValue.Value = name;
            crParameterFieldDefinitions = cryRpt.DataDefinition.ParameterFields;
            crParameterFieldDefinition = crParameterFieldDefinitions["typeparam"];
            crParameterValues = crParameterFieldDefinition.CurrentValues;
            crParameterValues.Clear();
            crParameterValues.Add(crParameterDiscreteValue);
            crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

            crConnectionInfo.ServerName = "localhost";
            crConnectionInfo.DatabaseName = "slimmersdb";
            crConnectionInfo.UserID = "root";
            crConnectionInfo.Password = "root";

            inventoryView.ReportSource = cryRpt;
            inventoryView.RefreshReport();
            inventfilter = "";
            panel5.Hide();
            inventoryFilter.Enabled = true;
            btnInventoryList.Enabled = true;
            btnPullOut.Enabled = true;
            btnRestock.Enabled = true;
        }
        public void GetInventoryMonthly(string file)
        {
            int year = Convert.ToInt32(comboBox2.Text.Trim());
            string month = comboBox3.Text.Trim();
            ReportDocument cryRpt = new ReportDocument();
            cryRpt.Load(file);

            ConnectionInfo crConnectionInfo = new ConnectionInfo();

            ParameterFieldDefinitions crParameterFieldDefinitions;
            ParameterFieldDefinition crParameterFieldDefinition;
            ParameterValues crParameterValues = new ParameterValues();
            ParameterDiscreteValue crParameterDiscreteValue = new ParameterDiscreteValue();

            crParameterDiscreteValue.Value = year;
            crParameterFieldDefinitions = cryRpt.DataDefinition.ParameterFields;
            crParameterFieldDefinition = crParameterFieldDefinitions["yearparam"];
            crParameterValues = crParameterFieldDefinition.CurrentValues;
            crParameterValues.Clear();
            crParameterValues.Add(crParameterDiscreteValue);
            crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

            crParameterDiscreteValue.Value = month;
            crParameterFieldDefinitions = cryRpt.DataDefinition.ParameterFields;
            crParameterFieldDefinition = crParameterFieldDefinitions["monthparam"];
            crParameterValues = crParameterFieldDefinition.CurrentValues;
            crParameterValues.Clear();
            crParameterValues.Add(crParameterDiscreteValue);
            crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

            crConnectionInfo.ServerName = "localhost";
            crConnectionInfo.DatabaseName = "slimmersdb";
            crConnectionInfo.UserID = "root";
            crConnectionInfo.Password = "root";

            inventoryView.ReportSource = cryRpt;
            inventoryView.RefreshReport();
            inventfilter = "";
            inventfilter2 = "";
            panel7.Hide();
            btnSales.Enabled = true;
            btnInventory.Enabled = true;
            btnPatients.Enabled = true;
            btnReceipts.Enabled = true;
            btnAppointments.Enabled = true;
            btnInventoryList.Enabled = true;
            btnPullOut.Enabled = true;
            btnRestock.Enabled = true;
            Pulloutfilter.Enabled = true;
            Restockfilter.Enabled = true;
            inventoryFilter.Enabled = true;
            bunifuFlatButton19.Enabled = true;
            bunifuFlatButton18.Enabled = true;
        }

        public void GetInventoryAnnual(string file)
        {
            int year = Convert.ToInt32(comboBox2.Text.Trim());
            ReportDocument cryRpt = new ReportDocument();
            cryRpt.Load(file);

            ConnectionInfo crConnectionInfo = new ConnectionInfo();

            ParameterFieldDefinitions crParameterFieldDefinitions;
            ParameterFieldDefinition crParameterFieldDefinition;
            ParameterValues crParameterValues = new ParameterValues();
            ParameterDiscreteValue crParameterDiscreteValue = new ParameterDiscreteValue();

            crParameterDiscreteValue.Value = year;
            crParameterFieldDefinitions = cryRpt.DataDefinition.ParameterFields;
            crParameterFieldDefinition = crParameterFieldDefinitions["yearparam"];
            crParameterValues = crParameterFieldDefinition.CurrentValues;
            crParameterValues.Clear();
            crParameterValues.Add(crParameterDiscreteValue);
            crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

            crConnectionInfo.ServerName = "localhost";
            crConnectionInfo.DatabaseName = "slimmersdb";
            crConnectionInfo.UserID = "root";
            crConnectionInfo.Password = "root";

            inventoryView.ReportSource = cryRpt;
            inventoryView.RefreshReport();
            inventfilter = "";
            inventfilter2 = "";
            panel7.Hide();
            btnSales.Enabled = true;
            btnInventory.Enabled = true;
            btnPatients.Enabled = true;
            btnReceipts.Enabled = true;
            btnAppointments.Enabled = true;
            btnInventoryList.Enabled = true;
            btnPullOut.Enabled = true;
            btnRestock.Enabled = true;
            Pulloutfilter.Enabled = true;
            Restockfilter.Enabled = true;
            inventoryFilter.Enabled = true;
            bunifuFlatButton19.Enabled = true;
            bunifuFlatButton18.Enabled = true;
        }

        public void GetDailyAppoint(string file)
        {
            try
            {
                ReportDocument cryRpt = new ReportDocument();
                cryRpt.Load(file);

                ConnectionInfo crConnectionInfo = new ConnectionInfo();

                crConnectionInfo.ServerName = "localhost";
                crConnectionInfo.DatabaseName = "slimmersdb";
                crConnectionInfo.UserID = "root";
                crConnectionInfo.Password = "root";

                appointmentViewer.ReportSource = cryRpt;
                appointmentViewer.RefreshReport();
            }
            catch (Exception me)
            {
            }
        }
        public void GetTodayAppoint(string date, string file)
        {

            ReportDocument cryRpt = new ReportDocument();
            cryRpt.Load(file);

            ConnectionInfo crConnectionInfo = new ConnectionInfo();

            ParameterFieldDefinitions crParameterFieldDefinitions;
            ParameterFieldDefinition crParameterFieldDefinition;
            ParameterValues crParameterValues = new ParameterValues();
            ParameterDiscreteValue crParameterDiscreteValue = new ParameterDiscreteValue();

            crParameterDiscreteValue.Value = date;
            crParameterFieldDefinitions = cryRpt.DataDefinition.ParameterFields;
            crParameterFieldDefinition = crParameterFieldDefinitions["dateparam"];
            crParameterValues = crParameterFieldDefinition.CurrentValues;
            crParameterValues.Clear();
            crParameterValues.Add(crParameterDiscreteValue);
            crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

            crConnectionInfo.ServerName = "localhost";
            crConnectionInfo.DatabaseName = "slimmersdb";
            crConnectionInfo.UserID = "root";
            crConnectionInfo.Password = "root";

            appointmentViewer.ReportSource = cryRpt;
            appointmentViewer.RefreshReport();
        }

        public void GetWeeklyAppoint(string file)
        {
            int year = Convert.ToInt32(comboBox7.Text.Trim());
            string month = comboBox8.Text.Trim();
            ReportDocument cryRpt = new ReportDocument();
            cryRpt.Load(file);

            ConnectionInfo crConnectionInfo = new ConnectionInfo();

            ParameterFieldDefinitions crParameterFieldDefinitions;
            ParameterFieldDefinition crParameterFieldDefinition;
            ParameterValues crParameterValues = new ParameterValues();
            ParameterDiscreteValue crParameterDiscreteValue = new ParameterDiscreteValue();

            crParameterDiscreteValue.Value = year;
            crParameterFieldDefinitions = cryRpt.DataDefinition.ParameterFields;
            crParameterFieldDefinition = crParameterFieldDefinitions["yearparam"];
            crParameterValues = crParameterFieldDefinition.CurrentValues;
            crParameterValues.Clear();
            crParameterValues.Add(crParameterDiscreteValue);
            crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

            crParameterDiscreteValue.Value = month;
            crParameterFieldDefinitions = cryRpt.DataDefinition.ParameterFields;
            crParameterFieldDefinition = crParameterFieldDefinitions["monthparam"];
            crParameterValues = crParameterFieldDefinition.CurrentValues;
            crParameterValues.Clear();
            crParameterValues.Add(crParameterDiscreteValue);
            crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

            crConnectionInfo.ServerName = "localhost";
            crConnectionInfo.DatabaseName = "slimmersdb";
            crConnectionInfo.UserID = "root";
            crConnectionInfo.Password = "root";

            appointmentViewer.ReportSource = cryRpt;
            appointmentViewer.RefreshReport();

            appointfilter = "";
            btnAppointments.Enabled = true;
            btnInventory.Enabled = true;
            btnPatients.Enabled = true;
            btnReceipts.Enabled = true;
            btnSales.Enabled = true;
            appointmentFilter.Enabled = true;
            panel14.Visible = false;
            
        }

        public void GetAnnualAppoint(string file)
        {
            int year = Convert.ToInt32(comboBox7.Text.Trim());
            ReportDocument cryRpt = new ReportDocument();
            cryRpt.Load(file);

            ConnectionInfo crConnectionInfo = new ConnectionInfo();

            ParameterFieldDefinitions crParameterFieldDefinitions;
            ParameterFieldDefinition crParameterFieldDefinition;
            ParameterValues crParameterValues = new ParameterValues();
            ParameterDiscreteValue crParameterDiscreteValue = new ParameterDiscreteValue();

            crParameterDiscreteValue.Value = year;
            crParameterFieldDefinitions = cryRpt.DataDefinition.ParameterFields;
            crParameterFieldDefinition = crParameterFieldDefinitions["yearparam"];
            crParameterValues = crParameterFieldDefinition.CurrentValues;
            crParameterValues.Clear();
            crParameterValues.Add(crParameterDiscreteValue);
            crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);

            crConnectionInfo.ServerName = "localhost";
            crConnectionInfo.DatabaseName = "slimmersdb";
            crConnectionInfo.UserID = "root";
            crConnectionInfo.Password = "root";

            appointmentViewer.ReportSource = cryRpt;
            appointmentViewer.RefreshReport();

            appointfilter = "";
            btnAppointments.Enabled = true;
            btnInventory.Enabled = true;
            btnPatients.Enabled = true;
            btnReceipts.Enabled = true;
            btnSales.Enabled = true;
            appointmentFilter.Enabled = true;
            panel14.Visible = false;

        }

        private void bunifuFlatButton6_Click(object sender, EventArgs e)
        {
            
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            string file = Application.StartupPath + @"\Reports\DailySalesReportProduct.rpt";
            GetDaily(file);
            productFilter.Show();
            productFilter.BringToFront();
            productViewer.Show();
            productViewer.BringToFront();
            serviceFilter.SendToBack();
        }

        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            serviceFilter.Show();
            serviceFilter.BringToFront();
            productViewer.SendToBack();
            productFilter.SendToBack();
            string file = Application.StartupPath + @"\Reports\DailySalesReportService.rpt";
            GetDaily(file);
        }

        private void bunifuFlatButton11_Click(object sender, EventArgs e)
        {
            string file = Application.StartupPath + @"\Reports\DailySalesReportService.rpt";
            GetDaily(file);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            string file = Application.StartupPath+@"\Reports\DailySalesReportService.rpt";
            GetDaily(file);
            salesPanel.BringToFront();
            salesPanel.Show();
            productFilter.Show();
            productFilter.BringToFront();
            productViewer.Show();
            productViewer.BringToFront();
            inventoryPanel.SendToBack();
            appointmentPanel.SendToBack();
            patientPanel.SendToBack();
            btnSales.Textcolor = Color.FromArgb(4, 180, 253);
            btnInventory.Textcolor = Color.White;
            btnAppointments.Textcolor = Color.White;
            btnPatients.Textcolor = Color.White;
            btnReceipts.Textcolor = Color.White;

            slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Top;
            slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Height;
            productViewer.RefreshReport();
        }

        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
            string file = Application.StartupPath + @"\Reports\DailySalesReportService.rpt";
            GetDaily(file);
        }

        private void bunifuFlatButton6_Click_1(object sender, EventArgs e)
        {
            string date = DateTime.Today.ToString("MM-dd-yyyy");
            string file = Application.StartupPath + @"\Reports\TodaySalesReportProduct.rpt";
            GetToday(date,file);
        }
        string filter = "";
        string filter2 = "";
        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
                filter = "Product";
                filter2 = "Weekly";
                panel2.Visible = false;
                panel2.BringToFront();
                filterTransition.ShowSync(panel2);
                productFilter.Enabled = false;
                bunifuThinButton21.Enabled = false;
                bunifuThinButton22.Enabled = false;
                label2.Show();
                cboMonth.Show();
                panel2.Show();
                GetYear();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            panel2.Hide();
            productFilter.Enabled = true;
            bunifuThinButton21.Enabled = true;
            bunifuThinButton22.Enabled = true;
        }

        private void bunifuThinButton23_Click(object sender, EventArgs e)
        {
            if (filter == "Product" && filter2 == "Weekly")
            {
                string file = Application.StartupPath + @"\Reports\WeeklySalesReportProductPerYear.rpt";
                GetWeekly(file);
            }
            else if (filter == "Product" && filter2 == "Monthly")
            {
                string file = Application.StartupPath + @"\Reports\MonthlySalesReportProductPerYear.rpt";
                GetMonthly(file);
            }
            else if (filter == Application.StartupPath + @"\Reports\Product" && filter2 == "Annual")
            {
                string file = Application.StartupPath + @"\Reports\AnnualSalesReportProductPerYear.rpt";
                GetAnnual(file);
            }
            else if (filter == "Service" && filter2 == "Weekly")
            {
                string file = Application.StartupPath + @"\Reports\WeeklySalesReportServicePerMonth.rpt";
                GetWeekly(file);
            }
            else if (filter == "Service" && filter2 == "Monthly")
            {
                string file = Application.StartupPath + @"\Reports\MonthlySalesReportServicePerYear.rpt";
                GetMonthly(file);
            }
            else if (filter == "Service" && filter2 == "Annual")
            {
                string file = Application.StartupPath + @"\Reports\AnnualSalesReportPerYear.rpt";
                GetAnnual(file);
            }
        }

        private void bunifuFlatButton4_Click(object sender, EventArgs e)
        {
            filter = "Product";
            filter2 = "Monthly";
            panel2.Visible = false;
            panel2.BringToFront();
            filterTransition.ShowSync(panel2);
            productFilter.Enabled = false;
            bunifuThinButton21.Enabled = false;
            bunifuThinButton22.Enabled = false;
            label2.Show();
            cboMonth.Show();
            panel2.Show();
            GetYear();
        }

        private void bunifuFlatButton5_Click(object sender, EventArgs e)
        {
            filter = "Product";
            filter2 = "Annual";
            panel2.Visible = false;
            panel2.BringToFront();
            filterTransition.ShowSync(panel2);
            productFilter.Enabled = false;
            bunifuThinButton21.Enabled = false;
            bunifuThinButton22.Enabled = false;
            label2.Hide();
            cboMonth.Hide();
            panel2.Show();
            GetYear();
        }

        private void bunifuFlatButton7_Click(object sender, EventArgs e)
        {
            string date = DateTime.Today.ToString("MM-dd-yyyy");
            string file = Application.StartupPath + @"\Reports\TodaySalesReportService.rpt";
            GetToday(date, file);
        }

        private void bunifuFlatButton10_Click(object sender, EventArgs e)
        {
            filter = "Service";
            filter2 = "Weekly";
            panel2.Visible = false;
            panel2.BringToFront();
            filterTransition.ShowSync(panel2);
            productFilter.Enabled = false;
            bunifuThinButton21.Enabled = false;
            bunifuThinButton22.Enabled = false;
            label2.Show();
            cboMonth.Show();
            panel2.Show();
            GetYear();
        }

        private void bunifuFlatButton9_Click(object sender, EventArgs e)
        {
            filter = "Service";
            filter2 = "Monthly";
            panel2.Visible = false;
            panel2.BringToFront();
            filterTransition.ShowSync(panel2);
            productFilter.Enabled = false;
            bunifuThinButton21.Enabled = false;
            bunifuThinButton22.Enabled = false;
            label2.Show();
            cboMonth.Show();
            panel2.Show();
            GetYear();
        }

        private void bunifuFlatButton8_Click(object sender, EventArgs e)
        {
            filter = "Service";
            filter2 = "Annual";
            panel2.Visible = false;
            panel2.BringToFront();
            filterTransition.ShowSync(panel2);
            productFilter.Enabled = false;
            bunifuThinButton21.Enabled = false;
            bunifuThinButton22.Enabled = false;
            label2.Hide();
            cboMonth.Hide();
            panel2.Show();
            GetYear();
        }

        private void bunifuThinButton26_Click(object sender, EventArgs e)
        {
            string file = Application.StartupPath + @"\Reports\InventoryListReport.rpt";
            GetInventory(file);
            inventoryFilter.BringToFront();
            inventoryFilter.Show();
            
        }

        private void bunifuThinButton25_Click(object sender, EventArgs e)
        {
            inventoryFilter.SendToBack();
            Pulloutfilter.BringToFront();
            Pulloutfilter.Show();
            Restockfilter.SendToBack();
            string file = Application.StartupPath + @"\Reports\PullOutProductReport.rpt";
            GetInventory(file);
        }

        private void bunifuThinButton27_Click(object sender, EventArgs e)
        {
            inventoryFilter.SendToBack();
            Pulloutfilter.SendToBack();
            Restockfilter.BringToFront();
            Restockfilter.Show();
            string file = Application.StartupPath + @"\Reports\RestockProductReport.rpt";
            GetInventory(file);
        }
        public void GetProduct()
        {
            comboBox5.Items.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("Select Product_Name from producttbl where Product_Status = 'Available' order by Product_No",connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    comboBox5.Items.Add(dataReader.GetString("Product_Name"));
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
                comboBox5.SelectedIndex = 0;
            }
            catch (Exception)
            {
            }
        }
        public void GetProductType()
        {
            comboBox5.Items.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("Select Product_Type from product_typetbl order by Product_Type_No",connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    comboBox5.Items.Add(dataReader.GetString("Product_Type"));
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
                comboBox5.SelectedIndex = 0;
            }
            catch (Exception)
            {
            }
        }
        string inventfilter = "";
        private void bunifuFlatButton12_Click(object sender, EventArgs e)
        {
            inventfilter = "Name";
            panel5.Visible = false;
            panel5.BringToFront();
            filterTransition.ShowSync(panel5);
            label8.Text = "Product Name";
            inventoryFilter.Enabled = false;
            btnInventoryList.Enabled = false;
            btnPullOut.Enabled = false;
            btnRestock.Enabled = false;
            GetProduct();
            panel5.Show();
        }

        private void bunifuFlatButton16_Click(object sender, EventArgs e)
        {
            inventfilter = "Type";
            panel5.Visible = false;
            panel5.BringToFront();
            filterTransition.ShowSync(panel5);
            label8.Text = "Product Type";
            inventoryFilter.Enabled = false;
            btnInventoryList.Enabled = false;
            btnPullOut.Enabled = false;
            btnRestock.Enabled = false;
            GetProductType();
            panel5.Show();
        }
        string inventfilter2 = "";
        private void bunifuThinButton28_Click(object sender, EventArgs e)
        {
            if (inventfilter == "Name")
            {
                GetInventoryPerName();

            }
            else if (inventfilter == "Type")
            {
                GetInventoryPerType();
            }
            inventfilter = "";
            panel5.Hide();
            inventoryFilter.Enabled = true;
            btnInventoryList.Enabled = true;
            btnPullOut.Enabled = true;
            btnRestock.Enabled = true;
        }

        private void label7_Click(object sender, EventArgs e)
        {
            inventfilter = "";
            panel5.Hide();
            inventoryFilter.Enabled = true;
            btnInventoryList.Enabled = true;
            btnPullOut.Enabled = true;
            btnRestock.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            inventoryPanel.Show();
            inventoryPanel.BringToFront();
            salesPanel.SendToBack();
            appointmentPanel.SendToBack();
            patientPanel.SendToBack();
            inventoryFilter.BringToFront();
            inventoryFilter.Show();
            Pulloutfilter.SendToBack();
            Restockfilter.SendToBack();
            btnSales.Textcolor = Color.White;
            btnInventory.Textcolor = Color.FromArgb(4, 180, 253);
            btnAppointments.Textcolor = Color.White;
            btnPatients.Textcolor = Color.White;
            btnReceipts.Textcolor = Color.White;

            slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Top;
            slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Height;
            string file = Application.StartupPath + @"\Reports\InventoryListReport.rpt";
            GetInventory(file);
            
        }

        private void bunifuFlatButton19_Click(object sender, EventArgs e)
        {
            inventfilter = "PullOut";
            inventfilter2 = "Monthly";
            btnSales.Enabled = false;
            btnInventory.Enabled = false;
            btnPatients.Enabled = false;
            btnReceipts.Enabled = false;
            btnAppointments.Enabled = false;
            btnInventoryList.Enabled = false;
            btnPullOut.Enabled = false;
            btnRestock.Enabled = false;
            Pulloutfilter.Enabled = false;
            bunifuFlatButton19.Enabled = false;
            bunifuFlatButton18.Enabled = false;
            label6.Show();
            comboBox3.Show();
            panel7.Visible = false;
            filterTransition.ShowSync(panel7);
            GetYear();
            panel7.Show();
        }

        private void bunifuThinButton24_Click(object sender, EventArgs e)
        {
            if (inventfilter == "PullOut" && inventfilter2 == "Monthly")
            {
                string file = Application.StartupPath + @"\Reports\MonthlyPullOutProductReport.rpt";
                GetInventoryMonthly(file);
            }
            else if (inventfilter == "PullOut" && inventfilter2 == "Annual")
            {
                string file = Application.StartupPath + @"\Reports\AnnualPullOutProductReport.rpt";
                GetInventoryAnnual(file);
            }
            else if (inventfilter == "Restock" && inventfilter2 == "Monthly")
            {
                string file = Application.StartupPath + @"\Reports\MonthlyRestockProductReport.rpt";
                GetInventoryMonthly(file);
            }
            else if (inventfilter == "Restock" && inventfilter2 == "Annual")
            {
                string file = Application.StartupPath + @"\Reports\AnnualRestockProductReport.rpt";
                GetInventoryAnnual(file);
            }
        }

        private void bunifuFlatButton18_Click(object sender, EventArgs e)
        {
            inventfilter = "PullOut";
            inventfilter2 = "Annual";
            btnSales.Enabled = false;
            btnInventory.Enabled = false;
            btnPatients.Enabled = false;
            btnReceipts.Enabled = false;
            btnAppointments.Enabled = false;
            btnInventoryList.Enabled = false;
            btnPullOut.Enabled = false;
            btnRestock.Enabled = false;
            Pulloutfilter.Enabled = false;
            bunifuFlatButton19.Enabled = false;
            bunifuFlatButton18.Enabled = false;
            panel7.Visible = false;
            label6.Hide();
            comboBox3.Hide();
            filterTransition.ShowSync(panel7);
            GetYear();
            panel7.Show();
        }

        private void bunifuFlatButton13_Click(object sender, EventArgs e)
        {
            inventfilter = "Restock";
            inventfilter2 = "Monthly";
            btnSales.Enabled = false;
            btnInventory.Enabled = false;
            btnPatients.Enabled = false;
            btnReceipts.Enabled = false;
            btnAppointments.Enabled = false;
            btnInventoryList.Enabled = false;
            btnPullOut.Enabled = false;
            btnRestock.Enabled = false;
            Pulloutfilter.Enabled = false;
            bunifuFlatButton19.Enabled = false;
            bunifuFlatButton18.Enabled = false;
            panel7.Visible = false;
            label6.Show();
            comboBox3.Show();
            filterTransition.ShowSync(panel7);
            GetYear();
            panel7.Show();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            inventfilter = "";
            inventfilter2 = "";
            panel7.Hide();
            btnSales.Enabled = true;
            btnInventory.Enabled = true;
            btnPatients.Enabled = true;
            btnReceipts.Enabled = true;
            btnAppointments.Enabled = true;
            btnInventoryList.Enabled = true;
            btnPullOut.Enabled = true;
            btnRestock.Enabled = true;
            Pulloutfilter.Enabled = true;
            Restockfilter.Enabled = true;
            inventoryFilter.Enabled = true;
            bunifuFlatButton19.Enabled = true;
            bunifuFlatButton18.Enabled = true;
        }

        private void btnAppointments_Click(object sender, EventArgs e)
        {
            string file = Application.StartupPath + @"\Reports\DailyServiceReport.rpt";
            GetDailyAppoint(file);
            appointmentPanel.Show();
            appointmentPanel.BringToFront();
            salesPanel.SendToBack();
            inventoryPanel.SendToBack();
            patientPanel.SendToBack();
            btnSales.Textcolor = Color.White;
            btnInventory.Textcolor = Color.White;
            btnAppointments.Textcolor = Color.FromArgb(4, 180, 253);
            btnPatients.Textcolor = Color.White;
            btnReceipts.Textcolor = Color.White;

            slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Top;
            slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Height;
        }

        private void bunifuFlatButton21_Click(object sender, EventArgs e)
        {
            string file = Application.StartupPath + @"\Reports\DailyServiceReport.rpt";
            GetDailyAppoint(file);
        }

        private void bunifuFlatButton14_Click(object sender, EventArgs e)
        {
            string date = DateTime.Today.ToString("MM-dd-yyy");
            string file = Application.StartupPath + @"\Reports\TodayServiceReport.rpt";
            GetTodayAppoint(date, file);
        }
        string appointfilter = "";
        private void bunifuFlatButton20_Click(object sender, EventArgs e)
        {
            appointfilter = "Weekly";
            btnAppointments.Enabled = false;
            btnInventory.Enabled = false;
            btnPatients.Enabled = false;
            btnReceipts.Enabled = false;
            btnSales.Enabled = false;
            appointmentFilter.Enabled = false;
            panel14.Visible = false;
            label11.Show();
            comboBox8.Show();
            filterTransition.ShowSync(panel14);
            GetYear();
            panel14.Show();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            appointfilter = "";
            btnAppointments.Enabled = true;
            btnInventory.Enabled = true;
            btnPatients.Enabled = true;
            btnReceipts.Enabled = true;
            btnSales.Enabled = true;
            appointmentFilter.Enabled = true;
            panel14.Visible = false;
        }

        private void bunifuFlatButton17_Click(object sender, EventArgs e)
        {
            appointfilter = "Monthly";
            btnAppointments.Enabled = false;
            btnInventory.Enabled = false;
            btnPatients.Enabled = false;
            btnReceipts.Enabled = false;
            btnSales.Enabled = false;
            appointmentFilter.Enabled = false;
            panel14.Visible = false;
            label11.Show();
            comboBox8.Show();
            filterTransition.ShowSync(panel14);
            GetYear();
            panel14.Show();
        }

        private void bunifuFlatButton15_Click(object sender, EventArgs e)
        {
            appointfilter = "Annual";
            btnAppointments.Enabled = false;
            btnInventory.Enabled = false;
            btnPatients.Enabled = false;
            btnReceipts.Enabled = false;
            btnSales.Enabled = false;
            appointmentFilter.Enabled = false;
            panel14.Visible = false;
            label11.Hide();
            comboBox8.Hide();
            filterTransition.ShowSync(panel14);
            GetYear();
            panel14.Show();
        }

        private void bunifuThinButton25_Click_1(object sender, EventArgs e)
        {
            if (appointfilter == "Weekly")
            {
                string file = Application.StartupPath + @"\Reports\WeeklyAppointmentReport.rpt";
                GetWeeklyAppoint(file);
            }
            else if (appointfilter == "Monthly")
            {
                string file = Application.StartupPath + @"\Reports\MonthlyAppointmentReport.rpt";
                GetWeeklyAppoint(file);
            }
            else if (appointfilter == "Annual")
            {
                string file = Application.StartupPath + @"\Reports\AnnualAppointmentReport.rpt";
                GetAnnualAppoint(file);
            }
        }

        private void btnPatients_Click(object sender, EventArgs e)
        {
            appointmentPanel.SendToBack();
            salesPanel.SendToBack();
            inventoryPanel.SendToBack();
            patientPanel.BringToFront();
            patientPanel.Show();
            btnSales.Textcolor = Color.White;
            btnInventory.Textcolor = Color.White;
            btnAppointments.Textcolor = Color.FromArgb(4, 180, 253);
            btnPatients.Textcolor = Color.White;
            btnReceipts.Textcolor = Color.White;

            slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Top;
            slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Height;
            string file = Application.StartupPath + @"\Reports\PatientListReport.rpt";
            try
            {
                ReportDocument cryRpt = new ReportDocument();
                cryRpt.Load(file);

                ConnectionInfo crConnectionInfo = new ConnectionInfo();

                crConnectionInfo.ServerName = "localhost";
                crConnectionInfo.DatabaseName = "slimmersdb";
                crConnectionInfo.UserID = "root";
                crConnectionInfo.Password = "root";

                patientViewer.ReportSource = cryRpt;
                patientViewer.RefreshReport();
            }
            catch (Exception me)
            {
            }
        }

        private void btnReceipts_Click(object sender, EventArgs e)
        {
            if (System.IO.Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "/Receipts"))
            {
                Process.Start(AppDomain.CurrentDomain.BaseDirectory + "/Receipts");
            }
            else
            {
                MessageBox.Show("File not found");
            }
            MessageBox.Show(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dashboardUC dash = new dashboardUC(ParentForm.Username);
            dash.BringToFront();
            dash.Show();
            this.Hide();
        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Pulloutfilter_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
