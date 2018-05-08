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
    public partial class s : Form
    {
        public s()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HomePage hp = new HomePage();
            hp.Show();
            this.Hide();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            HomePage hp = new HomePage();
            hp.Show();
            this.Hide();
        }
        int ctr = 0;
        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            ctr++;
            settingsTransition.ShowSync(panel4);
            if (ctr % 2 == 0)
            {
                panel4.Visible = false;
            }
            else
            {
                panel4.Visible = true;
            }
        }
    }
}
