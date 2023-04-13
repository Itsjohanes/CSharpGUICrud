using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrudWithChart
{
    public partial class Form8 : Form
    {
        private String connectionString = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=tb_lks;Integrated Security=True";
        public Form8()
        {
            InitializeComponent();
            //memberikan nilai nama-nama bulan ke dalam ComboBox1 dan ComboBox2
            //buat dalam bentuk array
            string[] bulan = new string[] { "Januari", "Februari", "Maret", "April", "Mei", "Juni", "Juli", "Agustus", "September", "Oktober", "November", "Desember" };
            //masukkan nilai array ke dalam ComboBox1 dan ComboBox2
            comboBox1.Items.AddRange(bulan);
            comboBox2.Items.AddRange(bulan);



        }

        private void Form8_Load(object sender, EventArgs e)
        {

            try
            {
                // Buat koneksi ke database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Query untuk mengambil data month dan income
                    string query = "SELECT DATENAME(month, OrderHeader.Date) AS Month, " +
                        "SUM(OrderDetail.Qty * MsMenu.Price) AS Income " +
                        "FROM OrderHeader " +
                        "INNER JOIN OrderDetail ON OrderHeader.Id = OrderDetail.OrderId " +
                        "INNER JOIN MsMenu ON OrderDetail.MenuId = MsMenu.Id " +
                        "GROUP BY DATENAME(month, OrderHeader.Date)";

                    // Buat adapter untuk menjalankan query dan mengambil data dari database
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

                    // Buat datatable untuk menampung hasil query
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Ubah nama bulan menjadi format lengkap (Januari - Desember)
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string monthName = row["Month"].ToString();
                        DateTime date = DateTime.ParseExact(monthName, "MMMM", CultureInfo.CurrentCulture);
                        row["Month"] = date.ToString("MMMM", CultureInfo.GetCultureInfo("id-ID"));
                    }

                    // Tambahkan data ke dalam DataGridView
                    dataGridView1.DataSource = dataTable;

                    // Ubah nama kolom agar lebih jelas
                    dataGridView1.Columns[0].HeaderText = "Bulan";
                    dataGridView1.Columns[1].HeaderText = "Income";

                    // Ubah lebar kolom agar tampilan lebih baik
                    dataGridView1.Columns[0].Width = 150;
                    dataGridView1.Columns[1].Width = 200;

                    // Ubah format tampilan angka pada kolom Income menjadi "C" (currency)
                    dataGridView1.Columns[1].DefaultCellStyle.Format = "C";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }










        }

        private void button1_Click(object sender, EventArgs e)
        {
            //ambil index dari ComboBox1 dan ComboBox2
            int index1 = comboBox1.SelectedIndex + 1; //karena bulan januari bulan ke 1
            int index2 = comboBox2.SelectedIndex + 1; //karena bulan januari bulan ke 1
            //jika index1 lebih besar dari index2 maka tukar
            if (index1 > index2)
            {
                int temp = index1;
                index1 = index2;
                index2 = temp;
            }
            //buat query untuk mengambil data month dan income
            string query = "SELECT DATENAME(month, OrderHeader.Date) AS Month, " +
                "SUM(OrderDetail.Qty * MsMenu.Price) AS Income " +
                "FROM OrderHeader " +
                "INNER JOIN OrderDetail ON OrderHeader.Id = OrderDetail.OrderId " +
                "INNER JOIN MsMenu ON OrderDetail.MenuId = MsMenu.Id " +
                "WHERE MONTH(OrderHeader.Date) BETWEEN " + index1 + " AND " + index2 + " " +
                "GROUP BY DATENAME(month, OrderHeader.Date)";

            //plot chart
            try
            {
                // Buat koneksi ke database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Buat adapter untuk menjalankan query dan mengambil data dari database
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    // Buat datatable untuk menampung hasil query
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    // Ubah nama bulan menjadi format lengkap (Januari - Desember)
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string monthName = row["Month"].ToString();
                        DateTime date = DateTime.ParseExact(monthName, "MMMM", CultureInfo.CurrentCulture);
                        row["Month"] = date.ToString("MMMM", CultureInfo.GetCultureInfo("id-ID"));
                    }
                    // Tambahkan data ke dalam DataGridView
                    dataGridView1.DataSource = dataTable;
                    // Ubah nama kolom agar lebih jelas
                    dataGridView1.Columns[0].HeaderText = "Bulan";
                    dataGridView1.Columns[1].HeaderText = "Income";
                    // Ubah lebar kolom agar tampilan lebih baik
                    dataGridView1.Columns[0].Width = 150;
                    dataGridView1.Columns[1].Width = 200;
                    // Ubah format tampilan angka pada kolom Income menjadi "C" (currency)
                    dataGridView1.Columns[1].DefaultCellStyle.Format = "C";
                    // Buat chart
                    chart1.Series[0].Points.Clear();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string monthName = row["Month"].ToString();
                        decimal income = Convert.ToDecimal(row["Income"]);
                        chart1.Series[0].Points.AddXY(monthName, income);
                    }
                    //set series1 sebagai income
                    chart1.Series[0].LegendText = "Income";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }
    }
}
