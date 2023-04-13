using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrudWithChart
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();


        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "Image Files (*.jpg, *.png, *.bmp)|*.jpg;*.png;*.bmp";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new System.Drawing.Bitmap(openFileDialog1.FileName);
                txtFileName.Text = openFileDialog1.FileName;
                //mengambil namanya saja bukan pathnya
                string[] words = openFileDialog1.FileName.Split('\\');
                textBox5.Text = words[words.Length - 1];
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                // Mendefinisikan direktori penyimpanan
                string directoryPath = @"C:\images\";

                // Memastikan direktori penyimpanan sudah ada
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Mendapatkan nama file dari textbox
                string fileName = Path.GetFileName(txtFileName.Text);

                // Menyimpan gambar ke direktori penyimpanan dengan nama file yang sesuai
                pictureBox1.Image.Save(Path.Combine(directoryPath, fileName), System.Drawing.Imaging.ImageFormat.Jpeg);

                // Menampilkan pesan jika penyimpanan berhasil
                MessageBox.Show("Gambar berhasil disimpan.");
            }
            else
            {
                MessageBox.Show("Silakan unggah gambar terlebih dahulu.");
            }
            //insert kabeh
            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=tb_lks;Integrated Security=True";
            //menuId auto increment
            string Name = textBox3.Text;
            string Price = textBox4.Text;
            string photo = textBox5.Text;
            string Carbo = textBox6.Text;
            string Protein = textBox7.Text;
            string query = "INSERT INTO MsMenu (Name, Price, photo, Carbo, Protein) VALUES (@Name, @Price, @photo, @Carbo, @Protein)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", Name);
                    command.Parameters.AddWithValue("@Price", int.Parse(Price));
                    command.Parameters.AddWithValue("@photo", photo);
                    command.Parameters.AddWithValue("@Carbo", int.Parse(Carbo));
                    command.Parameters.AddWithValue("@Protein", int.Parse(Protein));
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

            }
            tampilkanData();
            //kosongkan data
            clear();



        }
        private void clear()
        {
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            //clear picture
            pictureBox1.Image = null;
        }
        private void Form4_Load(object sender, EventArgs e)
        {
            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=tb_lks;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Membuat query SQL untuk mengambil data dari tabel tertentu
                string query = "SELECT Id, Name, Price, Carbo, Protein FROM MsMenu";

                // Membuat adapter data dan dataset
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataSet dataset = new DataSet();

                // Mengisi dataset dengan data dari adapter
                adapter.Fill(dataset, "MsMenu");

                // Menampilkan data pada DataGridView
                dataGridView1.DataSource = dataset.Tables["MsMenu"];
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                //menggambil id pertama
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                string id = row.Cells["Id"].Value.ToString();

                //mengambil data dari database
                string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=tb_lks;Integrated Security=True";
                string query = "SELECT * FROM MsMenu WHERE Id = @Id";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        textBox2.Text = id;
                        command.Parameters.AddWithValue("@Id", id);
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            textBox3.Text = reader["Name"].ToString();
                            textBox4.Text = reader["Price"].ToString();
                            textBox5.Text = reader["photo"].ToString();
                            textBox6.Text = reader["Carbo"].ToString();
                            textBox7.Text = reader["Protein"].ToString();
                        }
                        connection.Close();
                    }
                }
                //memasukan gambar
                string directoryPath = @"C:\images\";
                string fileName = Path.GetFileName(textBox5.Text);
                pictureBox1.Image = Image.FromFile(Path.Combine(directoryPath, fileName));


            }

        }
        public void tampilkanData()
        {
            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=tb_lks;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Membuat query SQL untuk mengambil data dari tabel tertentu
                string query = "SELECT Id, Name, Price, Carbo, Protein FROM MsMenu";

                // Membuat adapter data dan dataset
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataSet dataset = new DataSet();

                // Mengisi dataset dengan data dari adapter
                adapter.Fill(dataset, "MsMenu");

                // Menampilkan data pada DataGridView
                dataGridView1.DataSource = dataset.Tables["MsMenu"];
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string keyword = textBox1.Text.Trim().ToLower();
            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=tb_lks;Integrated Security=True";

            // Query untuk mencari data berdasarkan keyword
            string query = "SELECT Id, Name, Price, Carbo, Protein FROM MsMenu WHERE LOWER(Name) LIKE '%" + keyword + "%' OR Price LIKE '%" + keyword + "%' OR Carbo LIKE '%" + keyword + "%' OR Protein LIKE '%" + keyword + "%'";

            // Membuat koneksi ke database
            SqlConnection connection = new SqlConnection(connectionString);

            // Membuat adapter dan dataset
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataSet dataSet = new DataSet();

            // Mengisi dataset dengan data dari database
            adapter.Fill(dataSet, "MsMenu");

            // Menampilkan data pada DataGridView
            dataGridView1.DataSource = dataSet.Tables["MsMenu"];


        }

        private void button4_Click(object sender, EventArgs e)
        {
            //delete berdasarkan id
            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=tb_lks;Integrated Security=True";
            string id = textBox2.Text;
            string query = "DELETE FROM MsMenu WHERE Id = @Id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            //delete photo dari direktori

            tampilkanData();
            clear();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                // Mendefinisikan direktori penyimpanan
                string directoryPath = @"C:\images\";

                // Memastikan direktori penyimpanan sudah ada
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Mendapatkan nama file dari textbox
                string fileName = Path.GetFileName(txtFileName.Text);

                // Menyimpan gambar ke direktori penyimpanan dengan nama file yang sesuai
                pictureBox1.Image.Save(Path.Combine(directoryPath, fileName), System.Drawing.Imaging.ImageFormat.Jpeg);

                // Menampilkan pesan jika penyimpanan berhasil
                MessageBox.Show("Gambar berhasil disimpan.");
            }
            else
            {
                MessageBox.Show("Silakan unggah gambar terlebih dahulu.");
            }
            //update
            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=tb_lks;Integrated Security=True";
            string id = textBox2.Text;
            string Name = textBox3.Text;
            string Price = textBox4.Text;
            string photo = textBox5.Text;
            string Carbo = textBox6.Text;
            string Protein = textBox7.Text;
            string query = "UPDATE MsMenu SET Name = @Name, Price = @Price, photo = @photo, Carbo = @Carbo, Protein = @Protein WHERE Id = @Id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Name", Name);
                    command.Parameters.AddWithValue("@Price", int.Parse(Price));
                    command.Parameters.AddWithValue("@photo", photo);
                    command.Parameters.AddWithValue("@Carbo", int.Parse(Carbo));
                    command.Parameters.AddWithValue("@Protein", int.Parse(Protein));
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            tampilkanData();
            clear();
        }

       
    }
}
