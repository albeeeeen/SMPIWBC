﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SWSFCSMPIWBC
{
    public partial class PatientsUC : UserControl
    {
        static string connectionString = "datasource=localhost" + ";" + "DATABASE=slimmersdb" + ";" + "UID=root"
           + ";" + "PASSWORD=''" + ";";
        MySqlConnection connection = new MySqlConnection(connectionString);
        public PatientsUC()
        {
            InitializeComponent();
            GetAllPatients();
        }

        private void PatientsUC_Load(object sender, EventArgs e)
        {

        }
        public void GetAllPatients()
        {
            dataGridView1.Rows.Clear();
            try
            {
                connection.Open();
                string query = "SELECT *,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from patienttbl order by Patient_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView1.Rows.Add(dataReader.GetInt32("Patient_No"), dataReader.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)"), dataReader.GetString("Patient_Birthdate"), dataReader.GetString("Patient_ContactNo"), dataReader.GetString("Patient_Address"));
                }

                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
        }
        private void textBox24_TextChanged(object sender, EventArgs e)
        {
            string search = textBox24.Text.Trim();
            dataGridView1.Rows.Clear();
            try
            {
                connection.Open();
                string query = "SELECT *,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from patienttbl where Patient_LName LIKE '%" + search + "%' OR Patient_FName LIKE '%" + search + "%' order by Patient_No";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    dataGridView1.Rows.Add(dataReader.GetInt32("Patient_No"), dataReader.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)"), dataReader.GetString("Patient_Birthdate"), dataReader.GetString("Patient_ContactNo"), dataReader.GetString("Patient_Address"));
                }

                connection.Close();
            }
            catch (MySqlException me)
            {
                MessageBox.Show(me.Message);
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            string user = label5.Text;
            string position = "";
            try
            {
                connection.Open();
                MySqlCommand cmd1 = new MySqlCommand("SELECT * from accounttbl a, employeetbl e, employee_positiontbl ep where a.Username = '" + user + "' and a.Employee_no = e.Employee_No and e.Employee_Position_No = ep.Employee_Position_No", connection);
                MySqlDataReader dataReader1 = cmd1.ExecuteReader();
                while (dataReader1.Read())
                {
                    position = dataReader1.GetString("Position_Name");
                }
                connection.Close();
            }
            catch (Exception me)
            {
                connection.Close();
                MessageBox.Show(me.Message);
            }
            int row = dataGridView1.CurrentCell.RowIndex;
            int patientno = Convert.ToInt32(dataGridView1.Rows[row].Cells[0].Value);

            Profile profile = new Profile();

            profile.label2.Text = patientno.ToString();
            if (position == "Consultant")
            {
                try
                {
                    connection.Open();
                    string query = "SELECT *,CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit) from patienttbl p, patient_demtbl pd where p.Patient_No = '" + patientno + "' and p.Patient_No = pd.Patient_No";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {

                        profile.label16.Text = dataReader.GetInt32("Patient_No").ToString();
                        profile.label17.Text = dataReader.GetString("CONCAT(Patient_LName,', ',Patient_FName,' ',Patient_MidInit)");
                        profile.pictureBox1.Image = Image.FromFile(dataReader.GetString("Dem_Picture"));
                    }
                    connection.Close();
                }
                catch (MySqlException me)
                {
                    MessageBox.Show(me.Message);
                }
                profile.panel11.Enabled = true;
                profile.panel11.Show();
                profile.panel5.Hide();
                profile.panel31.Hide();
                profile.label48.Text = user;
                profile.button18.Hide();
                profile.button17.Show();
                profile.button17.Location = new Point(0, 92);
                profile.Show();
                this.Hide();
            }
            else if (position == "Receptionist")
            {
                string risks = "";
                connection.Open();
                MySqlCommand cmd2 = new MySqlCommand("SELECT * from patienttbl p , patient_medicaltbl pm where p.Patient_No = '" + patientno + "' and p.Patient_No = pm.Patient_No", connection);
                MySqlDataReader dataReader2 = cmd2.ExecuteReader();
                while (dataReader2.Read())
                {
                    risks = dataReader2.GetString("Risk_Factors");
                    profile.textBox14.Text = dataReader2.GetInt32("Patient_No").ToString();
                    profile.textBox1.Text = dataReader2.GetString("Patient_LName");
                    profile.textBox2.Text = dataReader2.GetString("Patient_FName");
                    profile.textBox3.Text = dataReader2.GetString("Patient_MidInit");
                    if (dataReader2.GetString("Patient_Gender") == "Male")
                    {
                        profile.radioButton1.Checked = true;
                    }
                    else
                    {
                        profile.radioButton2.Checked = true;
                    }
                    profile.dateTimePicker1.Value = Convert.ToDateTime(dataReader2.GetString("Patient_Birthdate"));
                    profile.textBox5.Text = dataReader2.GetInt32("Patient_Age").ToString();
                    profile.textBox4.Text = dataReader2.GetInt64("Patient_ContactNo").ToString();
                    profile.textBox11.Text = dataReader2.GetString("Patient_Email");
                    profile.textBox10.Text = dataReader2.GetString("Patient_Occupation");
                    if (dataReader2.GetString("Patient_CStatus") == "Single")
                    {
                        profile.radioButton3.Checked = true;
                    }
                    else if (dataReader2.GetString("Patient_CStatus") == "Married")
                    {
                        profile.radioButton4.Checked = true;
                    }
                    else if (dataReader2.GetString("Patient_CStatus") == "Widowed")
                    {
                        profile.radioButton7.Checked = true;
                    }
                    else if (dataReader2.GetString("Patient_CStatus") == "Others")
                    {
                        profile.radioButton8.Checked = true;
                    }
                    profile.textBox6.Text = dataReader2.GetString("Patient_Address");
                    profile.textBox29.Text = dataReader2.GetString("Height");
                    profile.textBox28.Text = dataReader2.GetString("Weight");
                    profile.textBox26.Text = dataReader2.GetString("Body_Frame");
                    profile.textBox27.Text = dataReader2.GetInt32("Body_Fat").ToString();
                    profile.textBox25.Text = dataReader2.GetString("Blood_Pressure");
                    profile.textBox24.Text = dataReader2.GetInt32("Pulse_Rate").ToString();
                    if (risks.Contains("Alcohol Drinker"))
                    {
                        profile.radioButton12.Checked = true;
                    }
                    else if (risks.Contains("Not Alcohol Drinker"))
                    {
                        profile.radioButton11.Checked = false;
                    }
                    if (risks.Contains("Smoker"))
                    {
                        profile.radioButton14.Checked = true;
                    }
                    else if (risks.Contains("Non Smoker"))
                    {
                        profile.radioButton13.Checked = true;
                    }
                    if (risks.Contains("Diabetes"))
                    {
                        profile.checkBox37.Checked = true;
                    }
                    if (risks.Contains("Allergy"))
                    {
                        profile.checkBox36.Checked = true;
                    }
                    if (risks.Contains("Heart Disease"))
                    {
                        profile.checkBox35.Checked = true;
                    }
                    if (risks.Contains("Pace Maker"))
                    {
                        profile.checkBox34.Checked = true;
                    }
                    if (risks.Contains("Seizures"))
                    {
                        profile.checkBox33.Checked = true;
                    }
                    if (risks.Contains("Headaches"))
                    {
                        profile.checkBox32.Checked = true;
                    }
                    if (risks.Contains("Chest Pains"))
                    {
                        profile.checkBox31.Checked = true;
                    }
                    profile.textBox31.Text = dataReader2.GetString("Other_MedHist");
                    profile.textBox30.Text = dataReader2.GetString("Other_Risks");
                    if (string.IsNullOrEmpty(profile.textBox30.Text))
                    {
                        profile.checkBox30.Checked = false;
                    }
                    else
                    {
                        profile.checkBox30.Checked = true;
                    }
                    if (risks.Contains("Dizziness"))
                    {
                        profile.radioButton5.Checked = true;
                    }
                    if (risks.Contains("Asthma"))
                    {
                        profile.radioButton15.Checked = true;
                    }
                    if (risks.Contains("Nausea"))
                    {
                        profile.radioButton17.Checked = true;
                    }
                    if (risks.Contains("Arthritis"))
                    {
                        profile.radioButton19.Checked = true;
                    }
                    if (risks.Contains("Bladder Problems"))
                    {
                        profile.radioButton21.Checked = true;
                    }
                    if (risks.Contains("Cancer"))
                    {
                        profile.radioButton23.Checked = true;
                    }
                    if (risks.Contains("Ringing Ears"))
                    {
                        profile.radioButton25.Checked = true;
                    }
                    if (risks.Contains("Thyroid Conditions"))
                    {
                        profile.radioButton27.Checked = true;
                    }
                }
                connection.Close();
                profile.panel31.Show();
                profile.panel11.Hide();
                profile.panel5.Hide();
                profile.label48.Text = user;
                profile.button17.Hide();
                profile.Show();
                this.Hide();
            }
            else
            {
                string risks = "";
                connection.Open();
                MySqlCommand cmd2 = new MySqlCommand("SELECT * from patienttbl p , patient_medicaltbl pm where p.Patient_No = '" + patientno + "' and p.Patient_No = pm.Patient_No", connection);
                MySqlDataReader dataReader2 = cmd2.ExecuteReader();
                while (dataReader2.Read())
                {
                    risks = dataReader2.GetString("Risk_Factors");
                    profile.textBox14.Text = dataReader2.GetInt32("Patient_No").ToString();
                    profile.textBox1.Text = dataReader2.GetString("Patient_LName");
                    profile.textBox2.Text = dataReader2.GetString("Patient_FName");
                    profile.textBox3.Text = dataReader2.GetString("Patient_MidInit");
                    if (dataReader2.GetString("Patient_Gender") == "Male")
                    {
                        profile.radioButton1.Checked = true;
                    }
                    else
                    {
                        profile.radioButton2.Checked = true;
                    }
                    profile.dateTimePicker1.Value = Convert.ToDateTime(dataReader2.GetString("Patient_Birthdate"));
                    profile.textBox5.Text = dataReader2.GetInt32("Patient_Age").ToString();
                    profile.textBox4.Text = dataReader2.GetInt64("Patient_ContactNo").ToString();
                    profile.textBox11.Text = dataReader2.GetString("Patient_Email");
                    profile.textBox10.Text = dataReader2.GetString("Patient_Occupation");
                    if (dataReader2.GetString("Patient_CStatus") == "Single")
                    {
                        profile.radioButton3.Checked = true;
                    }
                    else if (dataReader2.GetString("Patient_CStatus") == "Married")
                    {
                        profile.radioButton4.Checked = true;
                    }
                    else if (dataReader2.GetString("Patient_CStatus") == "Widowed")
                    {
                        profile.radioButton7.Checked = true;
                    }
                    else if (dataReader2.GetString("Patient_CStatus") == "Others")
                    {
                        profile.radioButton8.Checked = true;
                    }
                    profile.textBox6.Text = dataReader2.GetString("Patient_Address");
                    profile.textBox29.Text = dataReader2.GetString("Height");
                    profile.textBox28.Text = dataReader2.GetString("Weight");
                    profile.textBox26.Text = dataReader2.GetString("Body_Frame");
                    profile.textBox27.Text = dataReader2.GetInt32("Body_Fat").ToString();
                    profile.textBox25.Text = dataReader2.GetString("Blood_Pressure");
                    profile.textBox24.Text = dataReader2.GetInt32("Pulse_Rate").ToString();
                    if (risks.Contains("Alcohol Drinker"))
                    {
                        profile.radioButton12.Checked = true;
                    }
                    else if (risks.Contains("Not Alcohol Drinker"))
                    {
                        profile.radioButton11.Checked = false;
                    }
                    if (risks.Contains("Smoker"))
                    {
                        profile.radioButton14.Checked = true;
                    }
                    else if (risks.Contains("Non Smoker"))
                    {
                        profile.radioButton13.Checked = true;
                    }
                    if (risks.Contains("Diabetes"))
                    {
                        profile.checkBox37.Checked = true;
                    }
                    if (risks.Contains("Allergy"))
                    {
                        profile.checkBox36.Checked = true;
                    }
                    if (risks.Contains("Heart Disease"))
                    {
                        profile.checkBox35.Checked = true;
                    }
                    if (risks.Contains("Pace Maker"))
                    {
                        profile.checkBox34.Checked = true;
                    }
                    if (risks.Contains("Seizures"))
                    {
                        profile.checkBox33.Checked = true;
                    }
                    if (risks.Contains("Headaches"))
                    {
                        profile.checkBox32.Checked = true;
                    }
                    if (risks.Contains("Chest Pains"))
                    {
                        profile.checkBox31.Checked = true;
                    }
                    profile.textBox31.Text = dataReader2.GetString("Other_MedHist");
                    profile.textBox30.Text = dataReader2.GetString("Other_Risks");
                    if (string.IsNullOrEmpty(profile.textBox30.Text))
                    {
                        profile.checkBox30.Checked = false;
                    }
                    else
                    {
                        profile.checkBox30.Checked = true;
                    }
                    if (risks.Contains("Dizziness"))
                    {
                        profile.radioButton5.Checked = true;
                    }
                    if (risks.Contains("Asthma"))
                    {
                        profile.radioButton15.Checked = true;
                    }
                    if (risks.Contains("Nausea"))
                    {
                        profile.radioButton17.Checked = true;
                    }
                    if (risks.Contains("Arthritis"))
                    {
                        profile.radioButton19.Checked = true;
                    }
                    if (risks.Contains("Bladder Problems"))
                    {
                        profile.radioButton21.Checked = true;
                    }
                    if (risks.Contains("Cancer"))
                    {
                        profile.radioButton23.Checked = true;
                    }
                    if (risks.Contains("Ringing Ears"))
                    {
                        profile.radioButton25.Checked = true;
                    }
                    if (risks.Contains("Thyroid Conditions"))
                    {
                        profile.radioButton27.Checked = true;
                    }
                }
                connection.Close();
                profile.panel31.Show();
                //profile.panel11.Hide();
                profile.panel5.Hide();
                profile.label48.Text = user;
                profile.Show();
                this.Hide();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
