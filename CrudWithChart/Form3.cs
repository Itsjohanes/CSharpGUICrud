using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrudWithChart
{
    public partial class Form3 : Form
    {
        public string namaCashier = "cashier";
        public Form3()
        {
            InitializeComponent();
        }
        public Form3(string name)
        {
            InitializeComponent();
            namaCashier = name;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            label2.Text = "Welcome,[" + namaCashier + "]";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form7 form7 = new Form7();
            form7.Show();
            this.Hide();

        }
    }
}
