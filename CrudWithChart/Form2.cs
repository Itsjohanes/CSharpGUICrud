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
    public partial class Form2 : Form
    {
        string namaAdmin = "admin";
        string IdAdmin = "1";

        public Form2()
        {
            InitializeComponent();
        }
        public Form2(String nama,string id)
        {
            InitializeComponent();
            namaAdmin = nama;
            IdAdmin = id;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            label2.Text = "Welcome [" + namaAdmin + "]";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form4 form4form = new Form4();
            form4form.Show();
            this.Hide();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5();
            form5.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form6 form6 = new Form6(IdAdmin);
            form6.Show();
            this.Hide();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form8 form8 = new Form8();
            form8.Show();
            this.Hide();


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        
    }
}
