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
    public partial class AddInventory : Form
    {
        static string connectionString = "datasource=localhost" + ";" + "DATABASE=slimmersdb" + ";" + "UID=root"
         + ";" + "PASSWORD=root" + ";";
        MySqlConnection connection = new MySqlConnection(connectionString);
        public AddInventory()
        {
            InitializeComponent();
            initTime();
            
        }
       
        private void button2_Click(object sender, EventArgs e)
        {
            string userlog = label1.Text;
            Login login = new Login();
            login.CheckUser(userlog);
            
            this.Hide();
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
        private void panel2_Paint(object sender, PaintEventArgs e){
        

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Inventory invent = new Inventory();
            invent.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string user = label1.Text;
            AddInventory add = new AddInventory();
            add.label1.Text = user;
            add.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string user = label1.Text;
            Order order = new Order();
            order.label15.Text = user;
            order.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string user = label1.Text;
            PullOutProduct pop = new PullOutProduct();
            pop.label15.Text = user;
            pop.Show();
            this.Hide();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            string user = label1.Text;
            Inventory i = new Inventory();
            i.label15.Text = user;
            i.Show();
            this.Hide();
        }

        private void AddInventory_Load(object sender, EventArgs e)
        {

        }
        
        int ctr = 0;
        private void button14_Click(object sender, EventArgs e)
        {
            ctr++;
            if (ctr % 2 == 0)
            {
                panel3.Visible = false;
            }
            else
            {
                panel3.Visible = true;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }
    }
}
