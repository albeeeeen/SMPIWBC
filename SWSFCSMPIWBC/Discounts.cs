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
    public partial class Discounts : Form
    {
        
        public Discounts()
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
            label29.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }
      
        private void button8_Click(object sender, EventArgs e)
        {
            string user = label5.Text;
            Login login = new Login();
            login.CheckUser(user);
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string user = label5.Text;
            Discounts disc = new Discounts();
            disc.label5.Text = user;
            disc.Show();
            this.Hide();
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }
       
        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
           
        }

       
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        

        private void button17_Click(object sender, EventArgs e)
        {
            string user = label5.Text;
            Services service = new Services();
            service.label5.Text = user; 
            service.Show();
            this.Hide();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            //string user = label5.Text;
            //Employee emp = new Employee();
            //emp.label26.Text = user;
            //emp.Show();
            //emp.employeePanel.BringToFront();
            //emp.positionPanel.SendToBack();
            //emp.schedulePanel.SendToBack();
            //this.Hide();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            string user = label5.Text;
            Machine mach = new Machine();
            mach.label24.Text = user;
            mach.Show();
            this.Hide();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            string user = label5.Text;
            Product prod = new Product();
            prod.label37.Text = user;
            prod.Show();
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
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

        private void button6_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }
    }
}
