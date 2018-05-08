using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SWSFCSMPIWBC
{
    public partial class maintenanceUC : UserControl
    {
        public maintenanceUC()
        {
            InitializeComponent();
            ShowProduct();
        }
        public ProductsUC productsUC1 = new ProductsUC();
        public ServicesUC servicesUC1 = new ServicesUC();
        public MachineUC machineUC1 = new MachineUC();
        public EmployeeUC employeeUC1 = new EmployeeUC();
        public DiscountUC discountUC1 = new DiscountUC();
        private void ProductBtn_Click(object sender, EventArgs e)
        {

            //dashboardUC1.SendToBack();
            ShowProduct();
            slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Top;
            slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Height;

        }
        public void ShowProduct()
        {
            productsUC1.BackColor = System.Drawing.Color.White;
            productsUC1.Location = new System.Drawing.Point(158, 22);
            productsUC1.Size = new System.Drawing.Size(1190, 644);
            productsUC1.TabIndex = 10;
            productsUC1.Show();
            productsUC1.BringToFront();
            this.Controls.Add(productsUC1);
            this.Controls.Remove(servicesUC1);
            this.Controls.Remove(machineUC1);
            this.Controls.Remove(employeeUC1);
            this.Controls.Remove(discountUC1);

            prodBtn.Textcolor = Color.FromArgb(4, 180, 253);
            servicesBtn.Textcolor = Color.White;
            machBtn.Textcolor = Color.White;
            EmpBtn.Textcolor = Color.White;
            button15.Textcolor = Color.White;

           
        }
        public HomePage ParentForm { get; set; }
        private void servicesBtn_Click(object sender, EventArgs e)
        {
            
            servicesUC1.Location = new System.Drawing.Point(158, 22);
            servicesUC1.Size = new System.Drawing.Size(1190, 644);
            servicesUC1.TabIndex = 9;
            servicesUC1.Show();
            servicesUC1.BringToFront();
            this.Controls.Remove(productsUC1);
            this.Controls.Add(servicesUC1);
            this.Controls.Remove(machineUC1);
            this.Controls.Remove(employeeUC1);
            this.Controls.Remove(discountUC1);
            //dashboardUC1.SendToBack();

            slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Top;
            slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Height;

            servicesBtn.Textcolor = Color.FromArgb(4, 180, 253);
            prodBtn.Textcolor = Color.White;
            machBtn.Textcolor = Color.White;
            EmpBtn.Textcolor = Color.White;
            button15.Textcolor = Color.White;
        }

        private void machBtn_Click(object sender, EventArgs e)
        {
           
            machineUC1.BackColor = System.Drawing.Color.White;
            machineUC1.Location = new System.Drawing.Point(158, 22);
            machineUC1.Size = new System.Drawing.Size(1190, 644);
            machineUC1.TabIndex = 8;
            machineUC1.Show();
            machineUC1.BringToFront();
            this.Controls.Remove(productsUC1);
            this.Controls.Remove(servicesUC1);
            this.Controls.Add(machineUC1);
            this.Controls.Remove(employeeUC1);
            this.Controls.Remove(discountUC1);
            //dashboardUC1.SendToBack();

            slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Top;
            slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Height;

            machBtn.Textcolor = Color.FromArgb(4, 180, 253);
            servicesBtn.Textcolor = Color.White;
            prodBtn.Textcolor = Color.White;
            EmpBtn.Textcolor = Color.White;
            button15.Textcolor = Color.White;
        }

        private void EmpBtn_Click(object sender, EventArgs e)
        {
            
            employeeUC1.BackColor = System.Drawing.Color.White;
            employeeUC1.Location = new System.Drawing.Point(158, 22);
            employeeUC1.Size = new System.Drawing.Size(1190, 644);
            employeeUC1.TabIndex = 7;
            employeeUC1.Show();
            employeeUC1.BringToFront();
            this.Controls.Remove(productsUC1);
            this.Controls.Remove(servicesUC1);
            this.Controls.Remove(machineUC1);
            this.Controls.Add(employeeUC1);
            this.Controls.Remove(discountUC1);
            // dashboardUC1.SendToBack();

            slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Top;
            slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Height;

            EmpBtn.Textcolor = Color.FromArgb(4, 180, 253);
            servicesBtn.Textcolor = Color.White;
            machBtn.Textcolor = Color.White;
            prodBtn.Textcolor = Color.White;
            button15.Textcolor = Color.White;
        }

        private void button15_Click_1(object sender, EventArgs e)
        {
            
            discountUC1.BackColor = System.Drawing.Color.White;
            discountUC1.Location = new System.Drawing.Point(158, 22);
            discountUC1.Size = new System.Drawing.Size(1190, 644);
            discountUC1.TabIndex = 0;
            discountUC1.Show();
            discountUC1.BringToFront();
            this.Controls.Remove(productsUC1);
            this.Controls.Remove(servicesUC1);
            this.Controls.Remove(machineUC1);
            this.Controls.Remove(employeeUC1);
            this.Controls.Add(discountUC1);
            // dashboardUC1.SendToBack();
            slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Top;
            slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Height;

            button15.Textcolor = Color.FromArgb(4, 180, 253);
            servicesBtn.Textcolor = Color.White;
            machBtn.Textcolor = Color.White;
            EmpBtn.Textcolor = Color.White;
            prodBtn.Textcolor = Color.White;
        }



        private void button9_Click(object sender, EventArgs e)
        {
            this.Controls.Remove(productsUC1);
            this.Controls.Remove(servicesUC1);
            this.Controls.Remove(machineUC1);
            this.Controls.Remove(employeeUC1);
            this.Controls.Remove(discountUC1);
            dashboardUC dash = new dashboardUC(ParentForm.Username);
            dash.BringToFront();
            dash.Show();
            this.Hide();
        }
    }
}
