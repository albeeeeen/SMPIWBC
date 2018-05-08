using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace SWSFCSMPIWBC
{
    public partial class MachineUC : UserControl
    {
        static string connectionString =
       System.Configuration.ConfigurationManager.
       ConnectionStrings["SWSFCSMPIWBC.Properties.Settings.slimmersdbConnectionString"].ConnectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);

        public MachineUC()
        {
            InitializeComponent(); ClearError();
            textBox2.ReadOnly = true;
            textBox2.BorderStyle = BorderStyle.None;
            machinesPanel.Hide();
            machinetypePanel.Show();
            GetAllMachine();
            GetFirstMachType(); 
            button17.IdleFillColor = Color.FromArgb(4, 91, 188);
            button17.IdleForecolor = Color.White;

            button18.IdleFillColor = Color.White;
            button18.IdleLineColor = Color.FromArgb(4, 91, 188);
            button18.IdleForecolor = Color.FromArgb(4, 91, 188);
            button12.Visible = false;

        }
        public void ClearError()
        {
            label21.Text = "";
            textBox3.BackColor = Color.White;
            label22.Text = "";
            label23.Text = "";
            label20.Text = "";
            textBox2.BackColor = Color.White;
        }
        public void GetFirstMachType()
        {
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from machine_typetbl where Machine_Type_Status = 'Active' order by Machine_Type_No LIMIT 1", connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    textBox1.Text = dataReader.GetInt32("Machine_Type_No").ToString();
                    textBox2.Text = dataReader.GetString("Machine_Type_Name");
                }
                connection.Close();
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
            }
        }
        public void GetAllServices()
        {
            comboBox1.Items.Clear();
            try
            {
                connection.Open();
                string query = "Select Service_Name from servicetbl order by Service_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    comboBox1.Items.Add(dataReader.GetString("Service_Name"));
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            try
            {
                comboBox1.SelectedIndex = 0;
            }
            catch (Exception)
            {
                comboBox1.Text = "No Service";
            }
        }
        public void GetFirstMach()
        {
            try
            {
                connection.Open();
                string query = "Select * from machinetbl m, machine_typetbl mt where m.Machine_Type_No = (Select Machine_Type_No from machine_typetbl where Machine_Type_Status = 'Active' order by Machine_Type_No LIMIT 1) and m.Machine_Type_No = mt.Machine_Type_No order by Machine_No LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    textBox6.Text = dataReader.GetInt32("Machine_No").ToString();
                    textBox5.Text = dataReader.GetString("Machine_Name");
                    textBox7.Text = dataReader.GetString("Machine_Type_Name");
                    comboBox2.Text = dataReader.GetString("Machine_Status");
                }
            }
            catch (MySqlException me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            connection.Close();
        }
        public void GetMachineNo()
        {
            int machineno = 0;

            try
            {
                connection.Open();
                string query = "SELECT Machine_Type_No from machine_typetbl order by Machine_Type_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    machineno = dataReader.GetInt32("Machine_Type_No");
                }
                machineno = machineno + 1;
                textBox4.Text = machineno.ToString();
            }
            catch (MySqlException me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            connection.Close();
        }

        public void GetAllMach()
        {
            dataGridView2.Rows.Clear();
            try
            {
                connection.Open();
                string query = "Select * from machinetbl where Machine_Type_No = (SELECT Machine_Type_No from machine_typetbl where Machine_Type_Status = 'Active' order by Machine_Type_No LIMIT 1) order by Machine_Type_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView2.Rows.Add(dataReader.GetInt32("Machine_No"), dataReader.GetString("Machine_Name"), dataReader.GetString("Machine_Status"));
                }
            }
            catch (MySqlException me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            connection.Close();
        }
        public void GetMach(int machno)
        {
            try
            {
                connection.Open();
                string query = "Select * from machinetbl m, machine_typetbl mt where Machine_No = '" + machno + "' and m.Machine_Type_No = mt.Machine_Type_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    textBox6.Text = dataReader.GetInt32("Machine_No").ToString();
                    textBox5.Text = dataReader.GetString("Machine_Name");
                    textBox7.Text = dataReader.GetString("Machine_Type_Name");
                    comboBox2.Text = dataReader.GetString("Machine_Status");
                }
            }
            catch (MySqlException me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            connection.Close();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView1.Enabled = false;
            button1.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button12.Enabled = false;
            button17.Enabled = false;
            button18.Enabled = false;

            addPanel.Visible = false;

            addTransition.ShowSync(addPanel);

            GetMachineNo();
            addPanel.Show();
            GetAllServices();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button12.Visible = true;
            button7.Visible = false;
            textBox2.ReadOnly = false;
            textBox2.BorderStyle = BorderStyle.FixedSingle;
        }
        public void GetAllMachine()
        {
            button12.Visible = true;

            dataGridView1.Rows.Clear();
            dataGridView3.Rows.Clear();
            try
            {
                connection.Open();
                string query3 = "SELECT Machine_Type_No,Machine_Type_Name,Machine_Quantity,Service_Name from machine_typetbl mt, servicetbl s where mt.Service_No = s.Service_No and mt.Machine_Type_Status = 'Active' order by Machine_Type_No";
                MySqlCommand cmd3 = new MySqlCommand(query3, connection);
                MySqlDataReader dataReader3 = cmd3.ExecuteReader();
                while (dataReader3.Read())
                {
                    dataGridView1.Rows.Add(dataReader3.GetInt32("Machine_Type_No"), dataReader3.GetString("Machine_Type_Name"), dataReader3.GetInt32("Machine_Quantity"), dataReader3.GetString("Service_Name"));
                    dataGridView3.Rows.Add(dataReader3.GetInt32("Machine_Type_No"), dataReader3.GetString("Machine_Type_Name"));
                }
            }
            catch (MySqlException me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            connection.Close();
        }
        
        ErrorProvider errorProvider = new ErrorProvider();
        private void textBox3_Leave(object sender, EventArgs e)
        {
            string checkmachine;
            bool exists = false;
            string machinename = textBox3.Text.Trim();
            if (machinename.Length == 0)
            {
                errorProvider.SetError(textBox3, "Machine Name required");
            }
            else
            {
                try
                {
                    connection.Open();
                    string query1 = "Select Machine_Type_Name from machine_typetbl";
                    MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                    MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                    while (dataReader1.Read())
                    {
                        checkmachine = dataReader1.GetString("Machine_Type_Name");
                        if (checkmachine.Equals(machinename))
                        {
                            exists = true;
                            break;
                        }

                    }
                }
                catch (MySqlException me)
                {
                    connection.Close();
                    MessageBox.Show(me.Message);
                }
                connection.Close();
                if (exists)
                {
                    errorProvider.SetError(textBox3, "Machine name already exists");
                }
                else
                {
                    errorProvider.SetError(textBox3, string.Empty);
                }
            }
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
        }
        public void GetFirstMachine()
        {
            int rows = dataGridView3.CurrentCell.RowIndex;
            int machineno = Convert.ToInt32(dataGridView3.Rows[rows].Cells[0].Value);

            try
            {
                connection.Open();
                string query = "SELECT * from machinetbl m, machine_typetbl mt where mt.Machine_Type_No = m.Machine_Type_No and m.Machine_Type_No = '" + machineno + "' order by Machine_No LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    textBox6.Text = dataReader.GetInt32("Machine_No").ToString();
                    textBox5.Text = dataReader.GetString("Machine_Name");
                    comboBox2.Text = dataReader.GetString("Machine_Status");
                    textBox7.Text = dataReader.GetString("Machine_Type_Name");
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }
        private void dataGridView1_Click(object sender, EventArgs e)
        {
            ClearError();
            int rows = dataGridView1.CurrentCell.RowIndex;
            int machineno = Convert.ToInt32(dataGridView1.Rows[rows].Cells[0].Value);
            textBox2.BorderStyle = BorderStyle.None;
            textBox2.ReadOnly = true;
            button12.Enabled = false;
            editPanel.Show();
            addPanel.Hide();
            try
            {
                connection.Open();
                string query = "SELECT * from machine_typetbl where Machine_Type_No = '" + machineno + "'";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    textBox1.Text = dataReader.GetInt32("Machine_Type_No").ToString();
                    textBox2.Text = dataReader.GetString("Machine_Type_Name");
                }
            }
            catch (MySqlException me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            connection.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            comboBox2.Enabled = true;
            button11.Hide();
            button16.Visible = true;
            button8.Visible = false;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            int rows = dataGridView2.CurrentCell.RowIndex;
            int machineno = Convert.ToInt32(dataGridView2.Rows[rows].Cells[0].Value);
            int count = 0, machinetypeno = 0;
            try
            {
                connection.Open();
                string query3 = "Select Machine_Quantity,mt.Machine_Type_No from machinetbl m, machine_typetbl mt where m.Machine_No = '" + machineno + "' and m.Machine_Type_No = mt.Machine_Type_No";
                MySqlCommand cmd3 = new MySqlCommand(query3, connection);
                MySqlDataReader dataReader3 = cmd3.ExecuteReader();
                while (dataReader3.Read())
                {
                    count = dataReader3.GetInt32("Machine_Quantity");
                    machinetypeno = dataReader3.GetInt32("Machine_Type_No");
                }
                count = count - 1;
            }
            catch (MySqlException me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            connection.Close();
            DialogResult dialogResult = MessageBox.Show("Do you want to delete this item?", "Wait!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    connection.Open();
                    string query = "DELETE from machinetbl where Machine_No = '" + machineno + "'";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();

                    string query2 = "Update machine_typetbl set Machine_Quantity = '" + count + "' where Machine_Type_No = '" + machinetypeno + "'";
                    MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                    cmd2.ExecuteNonQuery();

                    MessageBox.Show("Successfully deleted item");
                    connection.Close();
                    GetAllMachine();
                    GetAllMach();
                    GetFirstMach();
                }
                catch (MySqlException me)
                {
                    connection.Close();
                    MessageBox.Show(me.Message);
                }

            }

        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            dataGridView1.Enabled = true;
            button1.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            button12.Enabled = true;
            button17.Enabled = true;
            button18.Enabled = true;
            addPanel.Enabled = true;

            addPanel.Hide();
            string checkmachine;
            bool exists = false, checker = false;
            string machinename = textBox3.Text.Trim();
            int machineno = Convert.ToInt32(textBox4.Text.Trim());
            int machqty = Convert.ToInt32(numericUpDown1.Value.ToString());
            string containsNum = @"[0-9~!@#$%^&*()_+=-]";
            string servicename = comboBox1.Text;
            int serviceno = 0;
            try
            {
                connection.Open();
                string query3 = "Select Service_No from servicetbl where Service_Name = '" + servicename + "'";
                MySqlCommand cmd3 = new MySqlCommand(query3, connection);
                MySqlDataReader dataReader3 = cmd3.ExecuteReader();
                while (dataReader3.Read())
                {
                    serviceno = dataReader3.GetInt32("Service_No");
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            if (machinename.Length == 0)
            {
                label21.Text = "Machine Type Name required";
                checker = true;
            }
            else
            {
                if (Regex.IsMatch(machinename, containsNum))
                {
                    label21.Text = "Machine type name invalid format";
                    checker = true;
                }
                else
                {
                    try
                    {
                        connection.Open();
                        string query1 = "Select Machine_Type_Name from machine_typetbl";
                        MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                        MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                        while (dataReader1.Read())
                        {
                            checkmachine = dataReader1.GetString("Machine_Type_Name");
                            if (checkmachine.Equals(machinename))
                            {
                                exists = true;
                                break;
                            }

                        }
                    }
                    catch (MySqlException me)
                    {
                        connection.Close();
                        MessageBox.Show(me.Message);
                    }
                    connection.Close();
                    if (exists)
                    {
                        label21.Text = "Machine name already exists";
                        checker = true;
                    }
                    else
                    {
                        label21.Text = "";
                    }
                }
            }
            if (machqty == 0)
            {
                label22.Text = "No quantity";
                checker = true;
            }
            else
            {
                label22.Text = "";
            }

            if (checker == false)
            {
                try
                {
                    connection.Open();
                    string query2 = "INSERT into machine_typetbl values ('" + machineno + "','" + machinename + "','" + machqty + "','" + serviceno + "','Active')";
                    MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                    cmd2.ExecuteNonQuery();

                    MessageBox.Show("Successfully added machine type");
                    connection.Close();

                }
                catch (MySqlException me)
                {
                    connection.Close();
                    MessageBox.Show(me.Message);
                }

                for (int x = 1; x <= machqty; x++)
                {
                    int machno = 0;
                    try
                    {
                        connection.Open();
                        string query5 = "SELECT * from machinetbl order by Machine_No";
                        MySqlCommand cmd5 = new MySqlCommand(query5, connection);
                        MySqlDataReader dataReader5 = cmd5.ExecuteReader();
                        while (dataReader5.Read())
                        {
                            machno = dataReader5.GetInt32("Machine_No");
                        }
                        machno = machno + 1;
                    }
                    catch (MySqlException me)
                    {
                        connection.Close();
                        MessageBox.Show(me.Message);
                    }
                    connection.Close();
                    try
                    {
                        connection.Open();
                        string query4 = "INSERT into machinetbl values('" + machno + "',CONCAT('" + machinename + "',' ','" + x + "'),'" + machineno + "','Available')";
                        MySqlCommand cmd4 = new MySqlCommand(query4, connection);
                        cmd4.ExecuteNonQuery();
                    }
                    catch (MySqlException me)
                    {
                        connection.Close();
                        MessageBox.Show(me.Message);
                    }
                    connection.Close();
                }

                GetMachineNo();
                numericUpDown1.ResetText();
                dataGridView1.Show();
                //panel5.Hide();
                GetAllMachine();
                GetAllMach();
                GetFirstMach();
            }

        }

        private void button12_Click(object sender, EventArgs e)
        {
            button7.Visible = true;
            button12.Visible = true;
            textBox2.ReadOnly = true;
            textBox2.BorderStyle = BorderStyle.None;

            string containsNum = @"[0-9~!@#$%^&*()_+=-]";
            string checkmachine, updatedmachine;
            bool exists = false, checker = false;
            string machinename = textBox2.Text.Trim();
            int ctr = 0;
            List<int> machineno = new List<int>();
            int machinetypeno = Convert.ToInt32(textBox1.Text.Trim());
            if (machinename.Length == 0)
            {
                label20.Text = "Machine Name required";
                checker = true;
            }
            else
            {
                if (Regex.IsMatch(machinename, containsNum))
                {
                    label20.Text = "Machine type name invalid format";
                    checker = true;
                }
                else
                {
                    try
                    {
                        connection.Open();
                        string query1 = "Select Machine_Type_Name from machine_typetbl where Machine_Type_No != '" + machinetypeno + "'";
                        MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                        MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                        while (dataReader1.Read())
                        {
                            checkmachine = dataReader1.GetString("Machine_Type_Name");
                            if (string.Equals(machinename, checkmachine, StringComparison.OrdinalIgnoreCase))
                            {
                                exists = true;
                                break;
                            }

                        }
                        connection.Close();
                    }
                    catch (MySqlException me)
                    {
                        connection.Close();
                        MessageBox.Show(me.Message);
                    }

                    if (exists)
                    {
                        label20.Text = "Machine name already exists";
                        checker = true;
                    }
                    else
                    {
                        label20.Text = "";
                    }
                }
            }
            if (checker == false)
            {
                try
                {
                    connection.Open();
                    string query2 = "UPDATE machine_typetbl set Machine_Type_Name = '" + machinename + "' where Machine_Type_No = '" + machinetypeno + "'";
                    MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                    cmd2.ExecuteNonQuery();
                    connection.Close();
                    connection.Open();
                    MySqlCommand cmd3 = new MySqlCommand("SELECT * from machinetbl where Machine_Type_No = '" + machinetypeno + "' order by Machine_No", connection);
                    MySqlDataReader dataReader3 = cmd3.ExecuteReader();
                    while (dataReader3.Read())
                    {
                        ctr++;
                        machineno.Add(dataReader3.GetInt32("Machine_No"));
                    }
                    connection.Close();

                    for (int j = 0; j < ctr; j++)
                    {

                        connection.Open();
                        MySqlCommand cmd4 = new MySqlCommand("UPDATE machinetbl set Machine_Name = REPLACE(Machine_Name,SUBSTRING(Machine_Name,1,CHAR_LENGTH(Machine_Name) - 2), '" + machinename + "') where Machine_No = '" + machineno[j] + "'", connection);
                        cmd4.ExecuteNonQuery();
                        connection.Close();
                    }
                    MessageBox.Show("Successfully updated machine type");

                    textBox2.ReadOnly = true;
                    comboBox2.Enabled = false;
                    textBox2.BorderStyle = BorderStyle.None;
                    dataGridView1.Rows.Clear();
                    GetAllMachine();
                    GetAllMach();
                    button12.Visible = false;
                    button7.Visible = true;
                }
                catch (Exception me)
                {
                    MessageBox.Show(me.Message);
                }

            }

        }

        private void dataGridView2_Click(object sender, EventArgs e)
        {
            int rows = dataGridView2.CurrentCell.RowIndex;
            int machineno = Convert.ToInt32(dataGridView2.Rows[rows].Cells[0].Value);
            GetMach(machineno);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            dataGridView1.Enabled = true;
            button1.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            button12.Enabled = true;
            button17.Enabled = true;
            button18.Enabled = true;
          
            int machineno = 0;
            string machinetype = textBox7.Text;
            int machinetypeno = 0, machinecount = 0, count = 0;

            DialogResult dialogresult = MessageBox.Show("Do you want to add machine?", "Question", MessageBoxButtons.YesNo);
            if (dialogresult == DialogResult.Yes)
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Machine_No from machinetbl order by Machine_No";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        machineno = dataReader.GetInt32("Machine_No");
                    }
                    machineno = machineno + 1;
                }
                catch (MySqlException me)
                {
                    connection.Close();
                    MessageBox.Show(me.Message);
                }
                connection.Close();

                try
                {
                    connection.Open();
                    string query2 = "SELECT mt.Machine_Type_No,SUBSTRING(Machine_Name,CHAR_LENGTH(Machine_Name),1) from machine_typetbl mt, machinetbl m where Machine_Type_Name = '" + machinetype + "' and m.Machine_Type_No = mt.Machine_Type_No";
                    MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                    MySqlDataReader dataReader2 = cmd2.ExecuteReader();
                    while (dataReader2.Read())
                    {
                        machinetypeno = dataReader2.GetInt32("Machine_Type_No");
                        machinecount = Convert.ToInt32(dataReader2.GetString("SUBSTRING(Machine_Name,CHAR_LENGTH(Machine_Name),1)"));
                        count++;
                    }
                    machinecount = machinecount + 1;
                    count = count + 1;
                }
                catch (MySqlException me)
                {
                    connection.Close();
                    MessageBox.Show(me.Message);
                }
                connection.Close();

                try
                {
                    connection.Open();
                    string query1 = "INSERT into machinetbl values ('" + machineno + "',CONCAT('" + machinetype + "',' ','" + machinecount + "'),'" + machinetypeno + "','Available')";
                    MySqlCommand cmd1 = new MySqlCommand(query1, connection);
                    cmd1.ExecuteNonQuery();

                    string query3 = "UPDATE machine_typetbl set Machine_Quantity = '" + count + "' where Machine_Type_No = '" + machinetypeno + "'";
                    MySqlCommand cmd3 = new MySqlCommand(query3, connection);
                    cmd3.ExecuteNonQuery();

                    MessageBox.Show("Machine successfully added!");
                    connection.Close();
                    GetAllMach();
                    GetAllMachine();
                    GetFirstMach();
                }
                catch (MySqlException me)
                {
                    connection.Close();
                    MessageBox.Show(me.Message);
                }

            }
        }
        private void textBox24_TextChanged(object sender, EventArgs e)
        {
            string search = textBox24.Text.Trim();

            dataGridView1.Rows.Clear();
            try
            {
                connection.Open();
                string query = "Select * from machine_typetbl mt, servicetbl s where Machine_Type_Name LIKE '%" + search + "%' and mt.Service_No = s.Service_No and mt.Machine_Type_Status = 'Active'";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView1.Rows.Add(dataReader.GetInt32("Machine_Type_No"), dataReader.GetString("Machine_Type_Name"), dataReader.GetInt32("Machine_Quantity"), dataReader.GetString("Service_Name"));
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            dataGridView2.Rows.Clear();
            int machtypeno = 0, machno = 0;
            try
            {
                machtypeno = Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString());

            }
            catch (Exception)
            {
                MessageBox.Show("No result!");
                textBox24.Text = "";
            }

            try
            {
                connection.Open();
                string query = "Select * from machinetbl  where Machine_Type_No = '" + machtypeno + "' order by Machine_Type_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView2.Rows.Add(dataReader.GetInt32("Machine_No"), dataReader.GetString("Machine_Name"), dataReader.GetString("Machine_Status"));
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            try
            {
                machno = Convert.ToInt32(dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex].Cells[0].Value.ToString());
            }
            catch (Exception)
            {
                MessageBox.Show("No result!");
                textBox24.Text = "";
            }
            try
            {
                connection.Open();
                string query = "Select * from machinetbl m, machine_typetbl mt where m.Machine_No = '" + machno + "' and m.Machine_Type_No = mt.Machine_Type_No order by Machine_No LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    textBox6.Text = dataReader.GetInt32("Machine_No").ToString();
                    textBox5.Text = dataReader.GetString("Machine_Name");
                    textBox7.Text = dataReader.GetString("Machine_Type_Name");
                    comboBox2.Text = dataReader.GetString("Machine_Status");

                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }


        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            int machine_no = 0;
            int rows = 0;
            rows = dataGridView1.CurrentCell.RowIndex;
            machine_no = Convert.ToInt32(dataGridView1.Rows[rows].Cells[0].Value);
            DialogResult dr = MessageBox.Show("Do you really want to delete?", "Delete", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE machine_typetbl set Machine_Type_Status = 'Deleted' where Machine_Type_No = '" + machine_no + "'";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record deleted!");
                    connection.Close();
                    GetAllMachine();
                }
                catch (MySqlException me)
                {
                    connection.Close();
                    MessageBox.Show(me.Message);
                }
            }
        }
        private void button16_Click(object sender, EventArgs e)
        {
            string status = comboBox2.Text;
            int machinetypeno = Convert.ToInt32(dataGridView3.Rows[dataGridView3.CurrentCell.RowIndex].Cells[0].Value);

            foreach (DataGridViewRow row in dataGridView2.SelectedRows)
            {
                int index = row.Index;
                int machineno = Convert.ToInt32(dataGridView2.Rows[index].Cells[0].Value);
                try
                {
                    connection.Open();
                    string query = "Update machinetbl set Machine_Status = '" + status + "' where Machine_No = '" + machineno + "'";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                catch (MySqlException me)
                {
                    connection.Close();
                    MessageBox.Show(me.Message);
                }

            }
            MessageBox.Show("Successfully update machine's status");
            button16.Visible = false;
            button8.Visible = true;
            button11.Show();
            comboBox2.Enabled = false;
            GetSelectedMachine(machinetypeno);
        }

        //private void button17_Click(object sender, EventArgs e)
        //{
        //    panel5.Hide();
        //    dataGridView1.Show();
        //}

        //private void button18_Click(object sender, EventArgs e)
        //{
        //    panel8.Hide();
        //    dataGridView1.Show();
        //}

        private void button19_Click(object sender, EventArgs e)
        {
            textBox24.Show();
            label45.Show();
            button17.IdleFillColor = Color.FromArgb(4, 91, 188);
            button17.IdleForecolor = Color.White;

            button18.IdleFillColor = Color.White;
            button18.IdleLineColor = Color.FromArgb(4, 91, 188);
            button18.IdleForecolor = Color.FromArgb(4, 91, 188);

            ClearError();
            //button19.BackColor = Color.Transparent;
            //button20.BackColor = Color.Silver;
            textBox2.ReadOnly = true;
            textBox2.BorderStyle = BorderStyle.None;
            GetAllMachine();
            GetFirstMachType();
            machinetypePanel.Show();
            machinesPanel.Hide();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            textBox24.Hide();
            label45.Hide();
            button18.IdleFillColor = Color.FromArgb(4, 91, 188);
            button18.IdleForecolor = Color.White;

            button17.IdleFillColor = Color.White;
            button17.IdleLineColor = Color.FromArgb(4, 91, 188);
            button17.IdleForecolor = Color.FromArgb(4, 91, 188);

            ClearError();
            //button20.BackColor = Color.Transparent;
            //button19.BackColor = Color.Silver;
            comboBox2.Enabled = false;
            button11.Show();
            //button16.Hide();
            GetAllMachine();
            GetFirstMach();
            GetAllMach();
            machinesPanel.Show();
            machinetypePanel.Hide();

        }
        public void GetSelectedMachine(int machineno)
        {
            dataGridView2.Rows.Clear();

            try
            {
                connection.Open();
                string query = "SELECT * from machinetbl where Machine_Type_No = '" + machineno + "' order by Machine_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView2.Rows.Add(dataReader.GetInt32("Machine_No"), dataReader.GetString("Machine_Name"), dataReader.GetString("Machine_Status"));
                }
                connection.Close();
            }
            catch (MySqlException me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
        }
        private void dataGridView3_Click(object sender, EventArgs e)
        {
            int rows = dataGridView3.CurrentCell.RowIndex;
            int machineno = Convert.ToInt32(dataGridView3.Rows[rows].Cells[0].Value);
            GetSelectedMachine(machineno);
            GetFirstMachine();
        }
        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            string containsNum = @"[0-9~!@#$%^&*()_+=-]";
            string empname = textBox2.Text.Trim();
            if (Regex.IsMatch(textBox2.Text, containsNum))
            {
                label20.Text = "No numeric character";
                textBox2.BackColor = Color.FromArgb(252, 224, 224);
                empname.Remove(empname.Length - 1);
                textBox2.Text = empname;
            }
            else
            {
                label20.Text = "";
                textBox2.BackColor = Color.White;
            }
        }

        private void textBox3_KeyUp(object sender, KeyEventArgs e)
        {
            string containsNum = @"[0-9~!@#$%^&*()_+=-]";
            string empname = textBox3.Text.Trim();
            if (Regex.IsMatch(textBox3.Text, containsNum))
            {
                label21.Text = "No numeric character";
                textBox3.BackColor = Color.FromArgb(252, 224, 224);
                empname.Remove(empname.Length - 1);
                textBox3.Text = empname;
            }
            else
            {
                label21.Text = "";
                textBox3.BackColor = Color.White;
            }
        }

        private void label45_Click(object sender, EventArgs e)
        {

        }

        private void textBox24_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            dataGridView1.Enabled = true;
            button1.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            button12.Enabled = true;
            button17.Enabled = true;
            button18.Enabled = true;
            addPanel.Enabled = true;

            addPanel.Hide();
        }

    }
}
