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

namespace amco
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        
        private void GetDataFromDatabase()
        {
            SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\amcoDB.mdf;Integrated Security=True;Connect Timeout=30");
            conn.Open();
            try
            {
                SqlDataAdapter sqlData = new SqlDataAdapter("SELECT * FROM  invoice_order_item", conn);
                SqlDataAdapter sqlData1 = new SqlDataAdapter("SELECT order_price FROM  invoice_order_item where order_id = 1", conn);
                DataTable dataTable = new DataTable();
                sqlData.Fill(dataTable);
                dataGridView1.AutoGenerateColumns = false;
                // dataGridView1.DataSource = dataTable;
                // dataGridView1.AutoGenerateColumns = false;
                gunaDataGridView1.AutoGenerateColumns = false;
                gunaDataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void pictureBox1_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //conn.Open();
           // SqlDataAdapter sqlData = new SqlDataAdapter("SELECT * FROM  invoice_order_item", conn);
           // DataTable dataTable = new DataTable();
           // sqlData.Fill(dataTable);
           // dataGridView1.AutoGenerateColumns = false;
           // dataGridView1.DataSource = dataTable;
           // conn.Close();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            GetDataFromDatabase();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

            if (this.WindowState != FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void gunaDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\amcoDB.mdf;Integrated Security=True;Connect Timeout=30");
            if (gunaDataGridView1.Columns[e.ColumnIndex].HeaderText == "Delete")
            {
                int id;
                id = Convert.ToInt32(gunaDataGridView1.Rows[e.RowIndex].Cells["invoiceNoColumn"].Value);
                conn.Open();
                try
                {
                    string query = "DELETE FROM invoice_order_item WHERE order_item_id=@code";
                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@code", id);
                    int result = command.ExecuteNonQuery();
                    if(result > 0)
                    {
                        MessageBox.Show("Data Deleted Succesful");
                    }
                    else
                    {
                        MessageBox.Show("Data not deleted");
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void Form4_Activated(object sender, EventArgs e)
        {
            GetDataFromDatabase();
        }

        private void gunaGradientButton1_Click(object sender, EventArgs e)
        {
            new Form2().Show();
            this.Hide();
        }
    }
}
