using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SWSFCSMPIWBC
{
    public partial class PullOutProduct : Form
    {
        static string connectionString = "datasource=localhost" + ";" + "DATABASE=slimmersdb" + ";" + "UID=root"
         + ";" + "PASSWORD=root" + ";";
        MySqlConnection connection = new MySqlConnection(connectionString);
        public PullOutProduct()
        {
            InitializeComponent();
           
        }

        
        private void button2_Click(object sender, EventArgs e)
        {
            string user = label15.Text;
            PullOutProduct pop = new PullOutProduct();
            pop.label15.Text = user;
            pop.Show();
            this.Hide();
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

        private void button7_Click(object sender, EventArgs e)
        {
            string user = label15.Text;
            HomePage hp = new HomePage();
            hp.label15.Text = user;
            hp.Show();
            this.Hide();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Filter_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void headerPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            string user = label15.Text;
            Inventory inventory = new Inventory();
            inventory.label15.Text = user;
            inventory.Show();
            this.Hide();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        
        private void metroPanel17_Paint(object sender, PaintEventArgs e)
        {

        }
        int ctr = 0;
        private void button6_Click(object sender, EventArgs e)
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

        private void button14_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }
    }
}
