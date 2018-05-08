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
    public partial class Promo : Form
    {
        static string connectionString = "datasource=localhost" + ";" + "DATABASE=slimmersdb" + ";" + "UID=root"
         + ";" + "PASSWORD=root" + ";";
        MySqlConnection connection = new MySqlConnection(connectionString);
        public Promo()
        {
            InitializeComponent();
        }

        public void GetPromoNo()
        {
            int promono = 0;
            try
            {
                string query = "SELECT Promo_No from service_promotbl order by Promo_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    promono = dataReader.GetInt32("Promo_No");
                }
                promono = promono + 1;
                textBox6.Text = promono.ToString();

            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Hide();
            HomePage hp = new HomePage();
            hp.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Discounts de = new Discounts();
            de.Show();
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label27_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }
        ErrorProvider errorProvider = new ErrorProvider();
        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {
            string desc = richTextBox3.Text.Trim();

            if (desc.Length == 0)
            {
                errorProvider.SetError(richTextBox3, "Promo description is required");
            }
            else
            {
                errorProvider.SetError(richTextBox3, string.Empty);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            GetPromoNo();
            dateTimePicker4.MinDate = DateTime.Today;
            dateTimePicker3.MinDate = DateTime.Now.AddDays(1);
        }
    }
}
