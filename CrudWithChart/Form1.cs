using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrudWithChart
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text;
            string password = textBox2.Text;
            //koneksi
            SqlConnection conn = new SqlConnection("Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=tb_lks;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM MsEmployee WHERE Email = @email AND password = @password", conn);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@password", password);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                MessageBox.Show("Login Berhasil");
                dr.Read();
                //index kolom ke 5
                string position = dr.GetString(5);
                string nama = dr.GetString(1);
                //ambil int idnya
                int idd = dr.GetInt32(0);
                //convert int ke string
                string id = idd.ToString();
                if (position == "admin")
                {
                    Form2 f2 = new Form2(nama,id);
                    f2.Show();
                    this.Hide();
                }
                else
                {
                    Form3 f3 = new Form3(nama);
                    f3.Show();
                    this.Hide();
                }



                this.Hide();
            }
            else
            {
                MessageBox.Show("Login Gagal");
            }





        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '*';


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

       
    }
}
