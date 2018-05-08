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
    public partial class Machine : Form
    {
        static string connectionString = "datasource=localhost" + ";" + "DATABASE=slimmersdb" + ";" + "UID=root"
         + ";" + "PASSWORD=root" + ";";
        MySqlConnection connection = new MySqlConnection(connectionString);
        
        public Machine()
        {
            InitializeComponent();
            
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
            label1.Text = DateTime.Now.ToLongDateString();
            label25.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }
      
        private void button1_Click(object sender, EventArgs e)
        {
           
            Maintenance mainte = new Maintenance();
            mainte.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string user = label24.Text;
            Services serve = new Services();
            serve.label5.Text = user;
            serve.Show();
            this.Hide();
        }

        //private void button3_Click(object sender, EventArgs e)
        //{
        //    string user = label24.Text;
        //    Employee emp = new Employee();
        //    emp.label26.Text = user;
        //    emp.Show();
        //    emp.employeePanel.BringToFront();
        //    emp.positionPanel.SendToBack();
        //    emp.schedulePanel.SendToBack();
        //    this.Hide();
        //}

        private void button4_Click(object sender, EventArgs e)
        {
            string user = label24.Text;
            Machine mach = new Machine();
            mach.label24.Text = user;
            mach.Show();
            this.Hide();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string user = label24.Text;
            Login login = new Login();
            login.CheckUser(user);
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string user = label24.Text;
            Product prod = new Product();
            prod.label37.Text = user;
            prod.Show();
            this.Hide();
        }


        private void button10_Click(object sender, EventArgs e)
        {
          
        }
      

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {
            string user = label24.Text;
            Discounts discounts = new Discounts();
            discounts.label5.Text = user;
            discounts.Show();
            this.Hide();
        }


        private void textBox2_Leave(object sender, EventArgs e)
        {

        }

      
        private void dataGridView1_Click_1(object sender, EventArgs e)
        {

        }
       

        private void machinetypePanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label70_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
           
        }

       
        int ctr = 0;
        private void button14_Click(object sender, EventArgs e)
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

        private void button19_Click_1(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }
       
    }
}
