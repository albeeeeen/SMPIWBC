using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SWSFCSMPIWBC
{
    public partial class allinventoryUC : UserControl
    {
        public allinventoryUC()
        {
            InitializeComponent();

            GetInventory();
            slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)inventoryBtn).Top;
            slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)inventoryBtn).Height;
        }
        public void GetInventory()
        {
            inventoryUC1.BackColor = System.Drawing.Color.White;
            inventoryUC1.Location = new System.Drawing.Point(152, 41);
            inventoryUC1.ParentForm = this;
            inventoryUC1.Size = new System.Drawing.Size(1190, 644);
            inventoryUC1.TabIndex = 9;
            inventoryUC1.CloseButtonClicked += new System.EventHandler(this.change_OkayButtonClicked);
            this.inventoryUC1.Show();
            this.inventoryUC1.BringToFront();
            this.Controls.Add(inventoryUC1);
            this.Controls.Remove(pullOutRecordUC1);
            this.Controls.Remove(pullOutProductsUC1);
            this.Controls.Remove(orderUC1);
            this.Controls.Remove(adddInventoryUC1);

            inventoryBtn.Textcolor = Color.FromArgb(4, 180, 253);
            requestBtn.Textcolor = Color.White;
            addStocksBtn.Textcolor = Color.White;
            pullOutBtn.Textcolor = Color.White;
            pullOutRecordBtn.Textcolor = Color.White;
        }
        public string Name
        {
            get;
            set;
        }
        public string Type
        {
            get;
            set;
        }
        public HomePage ParentForm { get; set; }
        public PullOutProductsUC pullOutProductsUC1 = new PullOutProductsUC();
        public PullOutRecordUC pullOutRecordUC1 = new PullOutRecordUC();
        public InventoryUC inventoryUC1 = new InventoryUC();
        public OrderUC orderUC1 = new OrderUC();
        public AdddInventoryUC adddInventoryUC1 = new AdddInventoryUC();
        private void button4_Click_2(object sender, EventArgs e)
        {

            GetInventory();
            slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Top;
            slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Height;
        }

        private void requestBtn_Click(object sender, EventArgs e)
        {
            this.orderUC1.BackColor = System.Drawing.Color.White;
            this.orderUC1.Location = new System.Drawing.Point(152, 41);
            this.orderUC1.Size = new System.Drawing.Size(1190, 644);
            this.orderUC1.TabIndex = 8;
            this.orderUC1.Show();
            this.orderUC1.BringToFront();
            this.Controls.Remove(inventoryUC1);
            this.Controls.Remove(pullOutRecordUC1);
            this.Controls.Remove(pullOutProductsUC1);
            this.Controls.Add(orderUC1);
            this.Controls.Remove(adddInventoryUC1);

            orderUC1.comboBox2.Enabled = true;
            orderUC1.comboBox1.Enabled = true;

            requestBtn.Textcolor = Color.FromArgb(4, 180, 253);
            inventoryBtn.Textcolor = Color.White;
            addStocksBtn.Textcolor = Color.White;
            pullOutBtn.Textcolor = Color.White;
            pullOutRecordBtn.Textcolor = Color.White;

            slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Top;
            slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Height;
        }

        private void addStocksBtn_Click(object sender, EventArgs e)
        {
            this.adddInventoryUC1.BackColor = System.Drawing.Color.White;
            this.adddInventoryUC1.Location = new System.Drawing.Point(152, 41);
            this.adddInventoryUC1.Size = new System.Drawing.Size(1190, 644);
            this.adddInventoryUC1.TabIndex = 11;
            this.adddInventoryUC1.dateTimePicker1.MinDate = DateTime.Now;
            adddInventoryUC1.OrderDelivered();
            this.adddInventoryUC1.Show();
            this.adddInventoryUC1.BringToFront();
            this.Controls.Remove(inventoryUC1);
            this.Controls.Remove(pullOutRecordUC1);
            this.Controls.Remove(pullOutProductsUC1);
            this.Controls.Remove(orderUC1);
            this.Controls.Add(adddInventoryUC1);

            addStocksBtn.Textcolor = Color.FromArgb(4, 180, 253);
            requestBtn.Textcolor = Color.White;
            inventoryBtn.Textcolor = Color.White;
            pullOutBtn.Textcolor = Color.White;
            pullOutRecordBtn.Textcolor = Color.White;

            slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Top;
            slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Height;
        }

        private void pullOutBtn_Click(object sender, EventArgs e)
        {
            this.pullOutProductsUC1.BackColor = System.Drawing.Color.White;
            this.pullOutProductsUC1.Location = new System.Drawing.Point(152, 43);
            this.pullOutProductsUC1.Size = new System.Drawing.Size(1190, 644);
            this.pullOutProductsUC1.TabIndex = 10;
            this.pullOutProductsUC1.Show();
            this.pullOutProductsUC1.BringToFront();
            this.Controls.Remove(inventoryUC1);
            this.Controls.Remove(pullOutRecordUC1);
            this.Controls.Add(pullOutProductsUC1);
            this.Controls.Remove(orderUC1);
            this.Controls.Remove(adddInventoryUC1);

            pullOutBtn.Textcolor = Color.FromArgb(4, 180, 253);
            requestBtn.Textcolor = Color.White;
            addStocksBtn.Textcolor = Color.White;
            inventoryBtn.Textcolor = Color.White;
            pullOutRecordBtn.Textcolor = Color.White;

            slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Top;
            slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)sender).Height;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            dashboardUC dash = new dashboardUC(ParentForm.Username);
            dash.BringToFront();
            dash.Show();
            this.Hide();
        }
        
        public event EventHandler CloseButtonClicked;
        void change_OkayButtonClicked(object sender, EventArgs e)
        {
            this.orderUC1.BackColor = System.Drawing.Color.White;
            this.orderUC1.Location = new System.Drawing.Point(152, 41);
            this.orderUC1.Size = new System.Drawing.Size(1190, 644);
            this.orderUC1.TabIndex = 8;
            this.orderUC1.Show();
            this.orderUC1.BringToFront();
            orderUC1.comboBox2.Text = this.Type;
            orderUC1.comboBox1.Text = this.Name;
            orderUC1.comboBox1.Enabled = false;
            orderUC1.comboBox2.Enabled = false;
            this.Controls.Remove(inventoryUC1);
            this.Controls.Remove(pullOutRecordUC1);
            this.Controls.Remove(pullOutProductsUC1);
            this.Controls.Add(orderUC1);
            this.Controls.Remove(adddInventoryUC1);

            requestBtn.Textcolor = Color.FromArgb(4, 180, 253);
            inventoryBtn.Textcolor = Color.White;
            addStocksBtn.Textcolor = Color.White;
            pullOutBtn.Textcolor = Color.White;
            pullOutRecordBtn.Textcolor = Color.White;

            slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)requestBtn).Top;
            slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)requestBtn).Height;
            inventoryUC1.SendToBack();
            pullOutProductsUC1.SendToBack();
            adddInventoryUC1.SendToBack();
            pullOutRecordUC1.SendToBack();
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            this.pullOutRecordUC1.BackColor = System.Drawing.SystemColors.Control;
            this.pullOutRecordUC1.Location = new System.Drawing.Point(152, 41);
            this.pullOutRecordUC1.Size = new System.Drawing.Size(1190, 644);
            this.pullOutRecordUC1.TabIndex = 12;
            this.pullOutRecordUC1.Show();
            this.pullOutRecordUC1.BringToFront();
            this.Controls.Remove(inventoryUC1);
            this.Controls.Add(pullOutRecordUC1);
            this.Controls.Remove(pullOutProductsUC1);
            this.Controls.Remove(orderUC1);
            this.Controls.Remove(adddInventoryUC1);

            pullOutRecordBtn.Textcolor = Color.FromArgb(4, 180, 253);
            inventoryBtn.Textcolor = Color.White;
            requestBtn.Textcolor = Color.White;
            addStocksBtn.Textcolor = Color.White;
            pullOutBtn.Textcolor = Color.White;

            slider.Top = ((Bunifu.Framework.UI.BunifuFlatButton)pullOutRecordBtn).Top;
            slider.Height = ((Bunifu.Framework.UI.BunifuFlatButton)pullOutRecordBtn).Height;
        }

        private void allinventoryUC_Load(object sender, EventArgs e)
        {

        }
    }
}
