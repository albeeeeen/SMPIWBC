using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SWSFCSMPIWBC
{
    public partial class Employee : Form
    {
       
        public Employee()
        {

            InitializeComponent();
            
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
            label1.Text = DateTime.Now.ToLongDateString();
            label5.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }
        
        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void Employee_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
        //    string user = label26.Text;
        //    Employee emp = new Employee();
        //    emp.label26.Text = user;
        //    emp.employeePanel.SendToBack();
        //    emp.positionPanel.BringToFront();
        //    emp.schedulePanel.SendToBack();
        //    GetFirstPosition();
        //    emp.Show();
        //    this.Hide();
            
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string user = label26.Text;
            Login login = new Login();
            login.CheckUser(user);
            this.Hide();
        }
        private void button9_Click1(object sender, EventArgs e)
        {
            Promo dp = new Promo();
            dp.Show();
            this.Hide();
        }


       
        private void button1_Click(object sender, EventArgs e)
        {
            Maintenance mainte = new Maintenance();
            mainte.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string user = label26.Text;
            Services service = new Services();
            service.label5.Text = user;
            service.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string user = label26.Text;
            Machine mach = new Machine();
            mach.label24.Text = user;
            mach.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string user = label26.Text;
            Product prod = new Product();
            prod.label37.Text = user;
            prod.Show();
            this.Hide();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            
            EmployeePosition epos = new EmployeePosition();
            epos.Show();
            this.Hide();
        }
        
        private void button11_Click(object sender, EventArgs e)
        {
            EmployeeSched empsched = new EmployeeSched();
            empsched.Show();
            this.Hide();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Maintenance mainte = new Maintenance();
            mainte.Show();
            this.Hide();
        }


        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {
            string user = label26.Text;
            Discounts discounts = new Discounts();
            discounts.label5.Text = user;
            discounts.Show();
            this.Hide();
        }

        

        private void dataGridView1_ChangeUICues(object sender, UICuesEventArgs e)
        {

        }

        private void label60_Click(object sender, EventArgs e)
        {

        }

        private void positionPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox5_TextChanged_1(object sender, EventArgs e)
        {
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void textBox11_TextChanged_1(object sender, EventArgs e)
        {
            
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void schedEdit_Paint(object sender, PaintEventArgs e)
        {

        }

        private void schedAdd_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel25_Paint(object sender, PaintEventArgs e)
        {

        }

        
        int ctr = 0;
        private void button16_Click(object sender, EventArgs e)
        {
            ctr++;
            if (ctr % 2 == 0)
            {
                panel4.Visible = false;
            }
            else
            {
                panel4.Visible = true;
            }
        }

        private void button14_Click_1(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }


    }
}
