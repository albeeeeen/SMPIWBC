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
    public partial class Order : Form
    {
        static string connectionString = "datasource=localhost" + ";" + "DATABASE=slimmersdb" + ";" + "UID=root"
        + ";" + "PASSWORD=root" + ";";
        MySqlConnection connection = new MySqlConnection(connectionString);
        public Order()
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
            label23.Text = DateTime.Now.ToLongDateString();
            label14.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Inventory invent = new Inventory();
            invent.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string user = label15.Text;
            AddInventory add = new AddInventory();
            add.label1.Text = user;
            add.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string user = label15.Text;
            Order order = new Order();
            order.label15.Text = user;
            order.Show();
            this.Hide();

        }

        private void button9_Click(object sender, EventArgs e)
        {
            string user = label15.Text;
            HomePage hp = new HomePage();
            hp.label15.Text = user;
            hp.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string user = label15.Text;
            PullOutProduct pop = new PullOutProduct();
            pop.label15.Text = user;
            pop.Show();
            this.Hide();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            string user = label15.Text;
            Inventory i = new Inventory();
            i.label15.Text = user;
            i.Show();
            this.Hide();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void metroPanel16_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {

        }

        
        int ctr = 0;
        private void button14_Click(object sender, EventArgs e)
        {
            ctr++;
            if (ctr % 2 == 0)
            {
                panel2.Visible = false;
            }
            else
            {
                panel2.Visible = true;
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }
    }
}
