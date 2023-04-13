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
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=tb_lks;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Membuat query SQL untuk mengambil data dari tabel tertentu
                string query = "SELECT * FROM MsMember";

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
            //ambil data
            string name = textBox3.Text;
            string email = textBox4.Text;
            string handphone = textBox5.Text;
            //date hari ini formatnya hari/bulan/tahun
            string joinDate = DateTime.Now.ToString("MM/dd/yyyy");

            //masukan ke database
            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=tb_lks;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // masukan data ke tabel MsMember
                string query = "INSERT INTO MsMember (Name, Email, Handphone, JoinDate) VALUES (@name, @email, @handphone, @joinDate)";
                //koneksikan
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@handphone", handphone);
                    command.Parameters.AddWithValue("@joinDate", joinDate);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                tampilkan();
                clear();

            }


        }
        private void tampilkan()
        {
            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=tb_lks;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Membuat query SQL untuk mengambil data dari tabel tertentu
                string query = "SELECT * FROM MsMember";

                // Membuat adapter data dan dataset
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataSet dataset = new DataSet();

                // Mengisi dataset dengan data dari adapter
                adapter.Fill(dataset, "MsMenu");

                // Menampilkan data pada DataGridView
                dataGridView1.DataSource = dataset.Tables["MsMenu"];
            }
        }
        private void clear()
        {
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //delete
            //delete berdasarkan id
            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=tb_lks;Integrated Security=True";
            string id = textBox2.Text;
            string query = "DELETE FROM MsMember WHERE Id = @Id";
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

            tampilkan();
            clear();

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
                string query = "SELECT * FROM MsMember WHERE Id = @Id";
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
                            textBox2.Text = reader["id"].ToString();
                            textBox3.Text = reader["Name"].ToString();
                            textBox4.Text = reader["Email"].ToString();
                            textBox5.Text = reader["Handphone"].ToString();
                        }
                        connection.Close();
                    }
                }



            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string keyword = textBox1.Text.Trim().ToLower();
            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=tb_lks;Integrated Security=True";

            // Query untuk mencari data berdasarkan keyword
            string query = "SELECT Id, Name, Email, Handphone, JoinDate FROM MsMember WHERE LOWER(Name) LIKE '%" + keyword + "%' OR Email LIKE '%" + keyword + "%' OR Handphone LIKE '%" + keyword + "%'";

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

        private void button2_Click(object sender, EventArgs e)
        {
            //update
            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=tb_lks;Integrated Security=True";

            string id = textBox2.Text;
            string name = textBox3.Text;
            string email = textBox4.Text;
            string handphone = textBox5.Text;
            string query = "UPDATE MsMember SET Name = @Name, Email = @Email, Handphone = @Handphone WHERE Id = @Id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", int.Parse(id));
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Handphone", handphone);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            tampilkan();
            clear();

        }
    }
}
