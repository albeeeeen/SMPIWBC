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
    public partial class Product : Form
    {
        static string connectionString = "datasource=localhost" + ";" + "DATABASE=slimmersdb" + ";" + "UID=root"
          + ";" + "PASSWORD=root" + ";";
        MySqlConnection connection = new MySqlConnection(connectionString);
        public Product()
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
            label38.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }
      

        private void button1_Click(object sender, EventArgs e)
        {
            Maintenance mainte = new Maintenance();
            mainte.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string user = label37.Text;
            Services service = new Services();
            service.label5.Text = user;
            service.Show();
            this.Hide();
        }

       

        private void button4_Click(object sender, EventArgs e)
        {
            string user = label37.Text;
            Machine mach = new Machine();
            mach.label24.Text = user;
            mach.Show();
            this.Hide();
        }

        //private void button5_Click(object sender, EventArgs e)
        //{
        //    string user = label37.Text;
        //    Product prod = new Product();
        //    prod.label37.Text = user;
        //    prod.Show();
        //    prod.typePanel.Show();
        //    prod.productPanel.Hide();
        //   // button18.BackColor = Color.Transparent;
        //   // button19.BackColor = Color.Silver;
        //    this.Hide();
        //}

        private void button9_Click(object sender, EventArgs e)
        {
            string user = label37.Text;
            Login login = new Login();
            login.CheckUser(user);
            this.Hide();
        }
       

        private void button11_Click(object sender, EventArgs e)
        {
            
            ProductType pt = new ProductType();
            pt.Show();
            this.Hide();
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {

        }

      

        private void button13_Click(object sender, EventArgs e)
        {
            string user = label37.Text;
            Discounts discounts = new Discounts();
            discounts.label5.Text = user;
            discounts.Show();
            this.Hide();
        }

        

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        
        private void textBox4_TextChanged_1(object sender, EventArgs e)
        {
            
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
        }

       

        private void addPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
        }

        private void typeAdd_Paint(object sender, PaintEventArgs e)
        {

        }

      
        int ctr = 0;
        private void button24_Click(object sender, EventArgs e)
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

        private void button18_Click_1(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }
    }
}
