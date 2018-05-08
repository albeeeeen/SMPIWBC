using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Windows;

namespace SWSFCSMPIWBC
{
    [RunInstaller(true)]
    public partial class Installer1 : System.Configuration.Install.Installer
    {
        public Installer1()
        {
            InitializeComponent();
        }
        private string GetSql(string Name)
        {
            StreamReader reader = null;
            try
            {
                // Gets the current assembly.
                Assembly Asm = Assembly.GetExecutingAssembly();

                // Resources are named using a fully qualified name.
                Stream strm = Asm.GetManifestResourceStream(Asm.GetName().Name + "." + Name);

                // Reads the contents of the embedded file.
                reader = new StreamReader(strm);
            }catch(Exception me)
            {
                MessageBox.Show(me.Message);
            }
            return reader.ReadToEnd();
        }
        private void ExecuteSQL(string SQL)
        {
            try
            {
                string connectionString = "DataSource=localhost;Initial Catalog = test; UID = root; password = root; Integrated Security = true";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();

                }
            }catch(Exception me)
            {
                MessageBox.Show(me.Message);
            }
        }
        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
            string script = GetSql("slimmersdb.txt");
            ExecuteSQL(script);
        }
    }
}
