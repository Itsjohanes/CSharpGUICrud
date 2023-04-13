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
    public partial class Form6 : Form
    {
        public string id;
        public string idAdmin = "1";

        public Form6()
        {
            InitializeComponent();
            this.id = makeId();

        }
        public Form6(string idAdmin)
        {
            InitializeComponent();
            this.id = makeId();
            this.idAdmin = idAdmin;

        }
        private void Form6_Load(object sender, EventArgs e)
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
            tampilkanTabel();



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
                        command.Parameters.AddWithValue("@Id", id);
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            textBox1.Text = reader["Name"].ToString();
                            label4.Text = reader["Photo"].ToString();
                            label7.Text = reader["Id"].ToString();

                        }
                        connection.Close();
                    }
                }
                //memasukan gambar
                string directoryPath = @"C:\images\";
                string fileName = Path.GetFileName(label4.Text);
                pictureBox1.Image = Image.FromFile(Path.Combine(directoryPath, fileName));


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //memasukan data ke database
            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=tb_lks;Integrated Security=True";


            //jika id MenuId pernah ada di tabel OrderDetail maka akan diupdate dan ditambah Qty
            string query = "IF EXISTS (SELECT * FROM OrderDetail WHERE OrderId = @OrderId AND MenuId = @MenuId) " +
                "UPDATE OrderDetail SET Qty = Qty + @Qty WHERE OrderId = @OrderId AND MenuId = @MenuId " +
                "ELSE " +
                "INSERT INTO OrderDetail(OrderId, MenuId, Qty, Status) VALUES (@OrderId, @MenuId, @Qty, 'Belum')";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderId", this.id);
                    command.Parameters.AddWithValue("@MenuId", label7.Text);
                    command.Parameters.AddWithValue("@Qty", textBox2.Text);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                tampilkanTabel();
                clear();
            }

        }
        public void tampilkanTabel()
        {

            //koneksi ke database
            SqlConnection connection = new SqlConnection("Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=tb_lks;Integrated Security=True");

            //membuat query untuk mengambil data dari tabel OrderDetail dan MsMenu
            string query = "SELECT M.Name, O.qty, M.carbo, M.protein, M.price FROM OrderDetail O JOIN MsMenu M ON O.MenuId = M.Id where OrderId = @OrderId";

            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            adapter.SelectCommand.Parameters.AddWithValue("@OrderId", this.id);

            //membuat dataset dan mengisi datagridview dengan data dari adapter
            DataSet dataset = new DataSet();
            adapter.Fill(dataset);
            dataGridView2.DataSource = dataset.Tables[0];

            //menghitung total dengan perkalian qty dan price
            //Kolom ini hanya dipanggil satu kali saja jika belum ada kolom total
            if (dataGridView2.Columns["total"] == null)
            {
                dataGridView2.Columns.Add("total", "Total");

            }

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                int qty = Convert.ToInt32(row.Cells["qty"].Value);
                decimal price = Convert.ToDecimal(row.Cells["price"].Value);
                decimal total = qty * price;
                row.Cells["total"].Value = total;
            }
            //memasukan jumlah kalori,protein, dan total
            //menghitung total keseluruhan
            int grandTotal = 0;
            int grandCarbo = 0;
            int grandProtein = 0;
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                int total = Convert.ToInt32(row.Cells["total"].Value);
                int qty = Convert.ToInt32(row.Cells["qty"].Value);
                int carbo = Convert.ToInt32(row.Cells["Carbo"].Value);
                int protein = Convert.ToInt32(row.Cells["Protein"].Value);
                grandTotal += total;
                grandCarbo += (carbo * qty);
                grandProtein += (protein * qty);
            }
            label6.Text = "Total: " + grandTotal.ToString();
            label8.Text = "Carbo: " + grandCarbo.ToString();
            label5.Text = "Protein: " + grandProtein.ToString();



        }
        public string makeId()
        {
            DateTime currentDate = DateTime.Now;
            string datePart = currentDate.ToString("yyyyMMdd");
            string numberPart = "{0:D4}";
            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=tb_lks;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand("SELECT MAX(RIGHT(Id, 4)) FROM OrderHeader WHERE LEFT(Id, 8) = @datePart", connection);
                command.Parameters.AddWithValue("@datePart", datePart);
                connection.Open();
                int lastNumber;
                string result = command.ExecuteScalar()?.ToString();
                if (int.TryParse(result, out lastNumber))
                {
                    lastNumber = lastNumber + 1;
                }
                else
                {
                    lastNumber = 1;
                }

                //susunannya harus seperti ini tidak tahu mengapa tidak bisa dd/MM/yyyy
                string joinDate = DateTime.Now.ToString("MM/dd/yyyy");

                string newId = $"{datePart}{string.Format(numberPart, lastNumber)}";

                string query = "Insert into OrderHeader(id, Employeeid, Date,PaymentType) values (@id, @Employeeid, @Date,@PaymentType)";

                connection.Close();

                using (SqlConnection connection1 = new SqlConnection(connectionString))
                {
                    using (SqlCommand command1 = new SqlCommand(query, connection1))
                    {
                        command1.Parameters.AddWithValue("@id", newId);
                        command1.Parameters.AddWithValue("@Employeeid", idAdmin);
                        command1.Parameters.AddWithValue("@Date", joinDate);
                        command1.Parameters.AddWithValue("@PaymentType", "Cash");

                        connection1.Open();

                        command1.ExecuteNonQuery();
                        connection1.Close();
                    }
                }


                return newId;
            }

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                //menggambil id pertama
                DataGridViewRow row = this.dataGridView2.Rows[e.RowIndex];

                string menuName = row.Cells["Name"].Value.ToString();
                string qty = row.Cells["qty"].Value.ToString();

                textBox1.Text = menuName;
                textBox2.Text = qty;




            }
            //memasukan gambar
            string directoryPath = @"C:\images\";
            string fileName = Path.GetFileName(label4.Text);
            pictureBox1.Image = Image.FromFile(Path.Combine(directoryPath, fileName));


        }

        private void button2_Click(object sender, EventArgs e)
        {
            //delete from
            string connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=tb_lks;Integrated Security=True";
            //cari MenuId berdasarkan menuName
            string query = "SELECT Id FROM MsMenu WHERE Name = @Name";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", textBox1.Text);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        label7.Text = reader["Id"].ToString();
                    }
                    connection.Close();
                }
            }
            //delete from
            string query1 = "DELETE FROM OrderDetail WHERE OrderId = @OrderId AND MenuId = @MenuId AND Qty = @Qty";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query1, connection))
                {
                    command.Parameters.AddWithValue("@OrderId", this.id);
                    command.Parameters.AddWithValue("@Qty", textBox2.Text);
                    command.Parameters.AddWithValue("@MenuId", label7.Text);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                tampilkanTabel();
                clear();
            }

        }
        public void clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            label7.Text = "";
        }
    }
}
