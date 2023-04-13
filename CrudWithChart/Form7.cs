using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrudWithChart
{
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
            //koneksi ke database
            string connection = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=tb_lks;Integrated Security=True";

            //membuat query untuk mengambil data dari tabel OrderDetail dan OrderHeader
            string query = "SELECT OD.OrderId FROM OrderDetail OD INNER JOIN OrderHeader OH ON OD.OrderId = OH.Id WHERE OD.Status = 'Belum'";

            //membuat adapter untuk mengisi data dari query ke dataset
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

            //membuat dataset dan mengisi combobox dengan data dari adapter
            DataSet dataset = new DataSet();
            adapter.Fill(dataset, "OrderDetail");
            List<string> orderIdList = new List<string>();
            foreach (DataRow row in dataset.Tables["OrderDetail"].Rows)
            {
                string orderId = row["OrderId"].ToString();
                if (!orderIdList.Contains(orderId))
                {
                    orderIdList.Add(orderId);
                    comboBox1.Items.Add(orderId);
                }
            }




        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string connection = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=tb_lks;Integrated Security=True";
            string query = "SELECT M.Name, O.qty,  M.price FROM OrderDetail O JOIN MsMenu M ON O.MenuId = M.Id where OrderId = @OrderId";

            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            string orderId = comboBox1.SelectedItem.ToString();
            adapter.SelectCommand.Parameters.AddWithValue("@OrderId", orderId);

            //membuat dataset dan mengisi datagridview dengan data dari adapter
            DataSet dataset = new DataSet();
            adapter.Fill(dataset);
            dataGridView1.DataSource = dataset.Tables[0];

            //menghitung total dengan perkalian qty dan price
            //Kolom ini hanya dipanggil satu kali saja jika belum ada kolom total
            if (dataGridView1.Columns["total"] == null)
            {
                dataGridView1.Columns.Add("total", "Total");

            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                int qty = Convert.ToInt32(row.Cells["qty"].Value);
                decimal price = Convert.ToDecimal(row.Cells["price"].Value);
                decimal total = qty * price;
                row.Cells["total"].Value = total;
            }
            //menghitung total keseluruhan
            int grandTotal = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                int total = Convert.ToInt32(row.Cells["total"].Value);
                grandTotal += total;
            }
            label4.Text = grandTotal.ToString();

        }

        private void Form7_Load(object sender, EventArgs e)
        {
            //membuat query untuk mengambil data dari tabel OrderDetail dan MsMenu
            string connection = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=tb_lks;Integrated Security=True";
            string query = "SELECT M.Name, O.qty,  M.price FROM OrderDetail O JOIN MsMenu M ON O.MenuId = M.Id where OrderId = @OrderId";

            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            string orderId = "";
            adapter.SelectCommand.Parameters.AddWithValue("@OrderId", orderId);

            //membuat dataset dan mengisi datagridview dengan data dari adapter
            DataSet dataset = new DataSet();
            adapter.Fill(dataset);
            dataGridView1.DataSource = dataset.Tables[0];

            //menghitung total dengan perkalian qty dan price
            //Kolom ini hanya dipanggil satu kali saja jika belum ada kolom total
            if (dataGridView1.Columns["total"] == null)
            {
                dataGridView1.Columns.Add("total", "Total");

            }


            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                int qty = Convert.ToInt32(row.Cells["qty"].Value);
                decimal price = Convert.ToDecimal(row.Cells["price"].Value);
                decimal total = qty * price;
                row.Cells["total"].Value = total;
            }
            //memasukan metodeBayar ke combobox
            comboBox2.Items.Add("cash");
            comboBox2.Items.Add("credit");
            //memasukan bank ke combobox
            comboBox3.Items.Add("BCA");
            comboBox3.Items.Add("BNI");
            comboBox3.Items.Add("BRI");
            comboBox3.Items.Add("MANDIRI");

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //jika metode bayar adalah cash maka tampilkan label7 dan textBox2
            if (comboBox2.SelectedItem.ToString() == "cash")
            {
                label7.Visible = true;
                textBox2.Visible = true;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Ubah status semua order menjadi sudah dan ubah payment
            string connection = "Data Source=LAPTOP-CMUG4L07\\SQLEXPRESS;Initial Catalog=tb_lks;Integrated Security=True";
            //jika pembayaran cash
            if (comboBox2.SelectedItem.ToString() == "cash")
            {
                //ubah status menjadi sudah
                string query = "UPDATE OrderDetail SET Status = 'Sudah' WHERE OrderId = @OrderId";
                SqlConnection conn = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@OrderId", comboBox1.SelectedItem.ToString());
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                //tunjukan kembalian
                int kembalian = Convert.ToInt32(textBox2.Text) - Convert.ToInt32(label4.Text);
                MessageBox.Show("Kembalian: " + kembalian.ToString());
            }
            else if ((comboBox2.SelectedItem.ToString() == "credit"))
            {
                //ubah status menjadi sudah dan ubah tabel pada paymentType CardNumber dan Bank orderHeader juga
                //ubah status menjadi sudah
                string query = "UPDATE OrderDetail SET Status = 'Sudah' WHERE OrderId = @OrderId";
                SqlConnection conn = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@OrderId", comboBox1.SelectedItem.ToString());
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                //ubah OrderHeader pada paymentType CardNumber dan Bank orderHeader juga
                string queryy = "UPDATE OrderHeader SET PaymentType = @PaymentType, CardNumber = @CardNumber, Bank = @Bank WHERE Id = @Id";
                conn = new SqlConnection(connection);
                cmd = new SqlCommand(queryy, conn);
                cmd.Parameters.AddWithValue("@PaymentType", "credit");
                cmd.Parameters.AddWithValue("@CardNumber", textBox1.Text.ToString());
                cmd.Parameters.AddWithValue("@Bank", comboBox3.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@Id", comboBox1.SelectedItem.ToString());
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();




            }
            //buat form7 baru
            Form7 form7 = new Form7();
            form7.Show();
            this.Hide();


        }

       
    }
}
