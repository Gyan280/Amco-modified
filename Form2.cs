using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
namespace amco
{
    public partial class Form2 : Form
    {
        
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
       (
           int nLeftRect,     // x-coordinate of upper-left corner
           int nTopRect,      // y-coordinate of upper-left corner
           int nRightRect,    // x-coordinate of lower-right corner
           int nBottomRect,   // y-coordinate of lower-right corner
           int nWidthEllipse, // width of ellipse
           int nHeightEllipse // height of ellipse
       );
        public Form2()
        {
            InitializeComponent();

            tableLayoutPanel1.AutoSize = true;
        }
        

        //total of items
        double totalOfAddMore = 0.00;
        // randomly generated invoice numbers
        string randInvoiceNo = "1234567890";
        // Price of single item
        double priceOfSingle = 1.00;
        //total of individual item
        double totalOfIndItem = 0.00;
        //Subtotal of items
        double subTotalOfIndItem = 0.00;
        //Amount paid of after total
        double amountPaidAfterTotal = 0.00;
        //countnumber of form render
        int count = 0;
        // items to save 9in database

        string itemnumbertosave, itemNametosave, quantitytosave, lengthtosave, pricetosave, totalOfProducttosave;

       
        protected string GenerateID()
        {

            string characters = randInvoiceNo;
            int length = 10;
            string id = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (id.IndexOf(character) != -1);
                id += character;
            }
            randInvoiceNo = "AMC" + id;
            return randInvoiceNo;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(2, 3, this.Width, this.Height, 15, 15)); //play with these values till you are happy
        }




        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {





            this.tableLayoutPanel1.RowCount++;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 28.57143F));


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

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            this.tableLayoutPanel1.RowCount--;
        }

        public void perfromPriceCalc()
        {

            try
            {
                int quantity = Convert.ToInt32(tonyTextbox6.Texts); ;
                int length = Convert.ToInt32(tonyTextbox15.Texts);
                if (gunaTextBox1.Text == null || gunaTextBox1.Text == "")
                {
                    gunaTextBox1.Text = "0.00";
                    if (tonyTextbox10.Texts == "0" || gunaTextBox2.Text == "0")
                    {
                        MessageBox.Show("Insert Nhis and Vat and press enter");
                    }
                    tonyTextbox10.Texts = "0.025";
                    gunaTextBox2.Text = "0.12";
                }
                double nhisText = Convert.ToDouble(tonyTextbox10.Texts);
                double vatText = Convert.ToDouble(gunaTextBox2.Text);
                double nhis = nhisText / 100;
                double vat = vatText / 100;
                priceOfSingle = Convert.ToDouble(gunaTextBox1.Text);
                totalOfIndItem = quantity * length * priceOfSingle;
                count += 1;
                double itemdiv = count * priceOfSingle;
                subTotalOfIndItem += totalOfIndItem;

                tonyTextbox17.Texts = totalOfIndItem.ToString();
                tonyTextbox9.Texts = subTotalOfIndItem.ToString();
                double totalAfterNhis = (subTotalOfIndItem * nhis) + subTotalOfIndItem;
                double totalAfterVat = (totalAfterNhis * vat) + totalAfterNhis;
                tonyTextbox11.Texts = totalAfterVat.ToString();
                tonyTextbox13.Texts = totalAfterVat.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Insert Nhis and Vat and press enter");
            }
            // int total1 = quantity * length * price;


        }


        public void saveToDatabase()
        {

            try
            {

                string receiverName = tonyTextbox18.Texts;
                string receiverAddress = tonyTextbox19.Texts;
                string subTotal = tonyTextbox9.Texts;
                string levy = tonyTextbox10.Texts;
                string vat = gunaTextBox2.Text;
                string amountPaid = textBox1.Text;
                int totalint = Int32.Parse(totalOfProducttosave);
                double vatint = 0.125;
                double levyint = 0.25;
                double totalAfterlevy = totalint * (levyint / 100) + totalint;
                double totalAfterVat = Convert.ToDouble(tonyTextbox11.Texts);
                DateTime dateTime = DateTime.Today;
                SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\amcoDB.mdf;Integrated Security=True;Connect Timeout=30");
                SqlCommand cmd = new SqlCommand("insert into invoice_order_item (order_item_code,order_item_name,order_item_quantity,order_item_length,order_item_price,order_item_final_amount,order_date,order_receiver_name,order_receiver_address,order_total_before_levy,order_total_levy,order_levy_per,order_total_after_levy,order_vat,order_total_after_VAT,order_amount_paid,order_total_amount_due)values('" + itemnumbertosave + "','" + itemNametosave + "'," +
                    "'" + quantitytosave + "','" + lengthtosave + "','" + pricetosave + "','" + totalOfProducttosave + "','" + dateTime + "'," +
                    "'" + receiverName + "','" + receiverAddress + "','" + subTotal + "','" + levy + "','" + levy + "','" + totalAfterlevy + "','" + vat + "','" + totalAfterVat + "','" + amountPaid + "','" + totalAfterVat + "')", conn);
                // SqlCommand cmd2 = new SqlCommand("insert into invoice_order(order_date,order_receiver_name,order_receiver_address,order_total_before_levy,order_total_levy,order_levy_per,order_total_after_levy,order_vat,order_total_after_VAT,order_amount_paid,order_total_amount_due)values('" + dateTime + "'," +
                //    "'" + receiverName + "','" + receiverAddress + "','" + subTotal + "','" + levy + "','" + levy + "','" + totalAfterlevy + "','" + vat + "','" + totalAfterVat + "','" + amountPaid + "','" + totalAfterVat + "')",conn);
                conn.Open();
                // cmd2.ExecuteNonQuery();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("please Fill everything");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string itemnumber = tonyTextbox3.Texts;
                string itemName = tonyTextbox5.Texts;
                string quantity = tonyTextbox6.Texts;
                string length = tonyTextbox15.Texts;
                string price = gunaTextBox1.Text;
                string totalOfProduct = tonyTextbox17.Texts;
                string receiverName = tonyTextbox18.Texts;
                string receiverAddress = tonyTextbox19.Texts;
                string subTotal = tonyTextbox9.Texts;
                string levy = tonyTextbox10.Texts;
                string vat = gunaTextBox2.Text;
                string amountPaid = textBox1.Text;
                int totalint = Int32.Parse(totalOfProduct);
                double vatint = 0.125;
                double levyint = 0.25;
                double totalAfterlevy = totalint * (levyint / 100) + totalint;
                double totalAfterVat = totalAfterlevy + vatint;
                DateTime dateTime = DateTime.Today;
                SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\DELL\source\repos\amco\amcoDB.mdf;Integrated Security=True");
                SqlCommand cmd = new SqlCommand("insert into invoice_order_item (order_item_code,order_item_name,order_item_quantity,order_item_length,order_item_price,order_item_final_amount,order_date,order_receiver_name,order_receiver_address,order_total_before_levy,order_total_levy,order_levy_per,order_total_after_levy,order_vat,order_total_after_VAT,order_amount_paid,order_total_amount_due)values('" + itemnumber + "','" + itemName + "'," +
                    "'" + quantity + "','" + length + "','" + price + "','" + totalOfProduct + "','" + dateTime + "'," +
                    "'" + receiverName + "','" + receiverAddress + "','" + subTotal + "','" + levy + "','" + levy + "','" + totalAfterlevy + "','" + vat + "','" + totalAfterVat + "','" + amountPaid + "','" + totalAfterVat + "')", conn);
                // SqlCommand cmd2 = new SqlCommand("insert into invoice_order(order_date,order_receiver_name,order_receiver_address,order_total_before_levy,order_total_levy,order_levy_per,order_total_after_levy,order_vat,order_total_after_VAT,order_amount_paid,order_total_amount_due)values('" + dateTime + "'," +
                //    "'" + receiverName + "','" + receiverAddress + "','" + subTotal + "','" + levy + "','" + levy + "','" + totalAfterlevy + "','" + vat + "','" + totalAfterVat + "','" + amountPaid + "','" + totalAfterVat + "')",conn);
                conn.Open();
                // cmd2.ExecuteNonQuery();
                cmd.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Fill All items");
            }
        }

        private void tonyTextbox16__TextChanged(object sender, EventArgs e)
        {
            // perfromPriceCalc();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Form4().Show();
            this.Hide();
        }



        private void tonyTextbox14_KeyUp(object sender, KeyEventArgs e)
        {
            string itemnumber = tonyTextbox3.Texts;
            string itemName = tonyTextbox5.Texts;
            string quantity = tonyTextbox6.Texts;
            string length = tonyTextbox15.Texts;
            string price = gunaTextBox1.Text;
            string subTotal = tonyTextbox9.Texts;
            string afterTotal = tonyTextbox11.Texts;
            string totalOfProduct = tonyTextbox17.Texts;
            string receiverName = tonyTextbox18.Texts;
            string receiverAddress = tonyTextbox19.Texts;
            string subTotal1 = tonyTextbox9.Texts;
            string levy = tonyTextbox10.Texts;
            string vat = gunaTextBox2.Text;
            string amountPaid = textBox1.Text;
            double totalint = Convert.ToDouble(totalOfProduct);
            double vatint = Convert.ToDouble(vat);
            double levyint = Convert.ToDouble(levy);
            double amountpaidint = Convert.ToDouble(amountPaid);
            //int totalAfterlevy = totalint * (levyint / 100) + totalint;
            //int totalAfterVat = totalAfterlevy + vatint;




            //tonyTextbox17.Texts = tonyTextbox16.Texts;
            int price1 = Int32.Parse(price);
            int quantity1 = Int32.Parse(quantity);
            int length1 = Int32.Parse(length);
            int total = quantity1 * length1 * price1;
            double totalAfterlevy = levyint * 0.01;
            double totalAfterVat = totalAfterlevy + vatint + total;
            double amountDue = amountpaidint - totalAfterVat;
            tonyTextbox17.Texts = total.ToString();
            tonyTextbox9.Texts = total.ToString();
            tonyTextbox11.Texts = totalAfterVat.ToString();
            tonyTextbox13.Texts = amountDue.ToString();



            //int total1 = quantity * length * price;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string itemnumber = tonyTextbox3.Texts;
            string itemName = tonyTextbox5.Texts;
            string quantity = tonyTextbox6.Texts;
            string length = tonyTextbox15.Texts;
            string price = gunaTextBox1.Text;
            string subTotal = tonyTextbox9.Texts;
            string afterTotal = tonyTextbox11.Texts;
            string totalOfProduct = tonyTextbox17.Texts;
            string receiverName = tonyTextbox18.Texts;
            string receiverAddress = tonyTextbox19.Texts;
            string subTotal1 = tonyTextbox9.Texts;
            string levy = tonyTextbox10.Texts;
            string vat = gunaTextBox2.Text;
            string amountPaid = textBox1.Text;
            double totalint = Convert.ToDouble(totalOfProduct);
            double vatint = Convert.ToDouble(vat);
            double levyint = Convert.ToDouble(levy);
            double amountpaidint = Convert.ToDouble(amountPaid);
            //int totalAfterlevy = totalint * (levyint / 100) + totalint;
            //int totalAfterVat = totalAfterlevy + vatint;




            //tonyTextbox17.Texts = tonyTextbox16.Texts;
            int price1 = Int32.Parse(price);
            int quantity1 = Int32.Parse(quantity);
            int length1 = Int32.Parse(length);
            int total = quantity1 * length1 * price1;
            double totalAfterlevy = levyint * 0.01;
            double totalAfterVat = totalAfterlevy + vatint + total;
            double amountDue = amountpaidint - totalAfterVat;
            tonyTextbox17.Texts = total.ToString();
            tonyTextbox9.Texts = total.ToString();
            tonyTextbox11.Texts = totalAfterVat.ToString();
            tonyTextbox13.Texts = amountDue.ToString();



            //int total1 = quantity * length * price;
        }

        private void textBox1_OnValueChanged(object sender, EventArgs e)
        {
            try
            {
                double totalToPay = Convert.ToDouble(tonyTextbox11.Texts);
                if (textBox1.Text == null || textBox1.Text == "")
                {
                    textBox1.Text = "0.00";
                }
                amountPaidAfterTotal = Convert.ToDouble(textBox1.Text);
                double amountPaidCalc = amountPaidAfterTotal - totalToPay;
                tonyTextbox13.Texts = amountPaidCalc.ToString("0.00");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gunaGradientButton1_Click(object sender, EventArgs e)
        {
            try
            {
                saveToDatabase();
                //double vatint = 0.125;
                //double levyint = 0.25;
                //string totalOfProduct = tonyTextbox17.Texts;
                //int totalint = Int32.Parse(totalOfProduct);
                //double totalAfterlevy = totalint * (levyint / 100) + totalint;
                //double totalAfterVat = totalAfterlevy + vatint;
                GenerateID();

                using (PrintReceipt frm = new PrintReceipt(receiptBindingSource.DataSource as List<Receipt>, string.Format("¢{0}", tonyTextbox11.Texts), string.Format("¢{0}", textBox1.Text), string.Format("¢{0}", tonyTextbox13.Texts), DateTime.Now.ToString(), string.Format("{0}", tonyTextbox18.Texts), string.Format("¢{0}", tonyTextbox9.Texts), string.Format("{0}", randInvoiceNo), string.Format("{0}%", gunaTextBox2.Text), string.Format("{0}%", tonyTextbox10.Texts)))
                {
                    frm.ShowDialog();
                };

               
            }
            catch
            {
                MessageBox.Show("Add item to receipt");
            }

        }

        private void gunaGradientButton2_Click(object sender, EventArgs e)
        {
            saveToDatabase();
        }



        private void gunaGradientButton5_Click(object sender, EventArgs e)
        {
            try
            {
                new Form4().Show();
                this.Hide();
            }
            catch
            {
                MessageBox.Show("Loading.....");
            }
        }

        private void gunaGradientButton3_Click(object sender, EventArgs e)
        {

            double vatint = 0.125;
            double levyint = 0.25;
            string totalOfProduct = tonyTextbox17.Texts;
            int totalint = Int32.Parse(totalOfProduct);
            double totalAfterlevy = totalint * (levyint / 100) + totalint;
            double totalAfterVat = totalAfterlevy + vatint;
            Receipt obj = receiptBindingSource.Current as Receipt;
            if (obj != null)
            {
                tonyTextbox17.Texts = Convert.ToString(obj.Price * obj.Quantity);
                tonyTextbox11.Texts = string.Format("{0}", totalAfterVat);
            }
            receiptBindingSource.RemoveCurrent();
            //this.tableLayoutPanel1.RowCount--;
        }

        private void gunaGradientButton4_Click(object sender, EventArgs e)
        {
            try
            {
                //id,itemname,quantity,length,price,total
                itemnumbertosave = tonyTextbox3.Texts;
                itemNametosave = tonyTextbox5.Texts;
                lengthtosave = tonyTextbox15.Texts;
                quantitytosave = tonyTextbox6.Texts;
                pricetosave = gunaTextBox1.Text;
                totalOfProducttosave = tonyTextbox17.Texts;
                double vatint = 0.125;
                double levyint = 0.25;
                string totalOfProduct = tonyTextbox17.Texts;
                string subTotal = tonyTextbox11.Texts;
                double SubTotal = Convert.ToDouble(subTotal);
                int totalint = Int32.Parse(totalOfProduct);
                double moreTotal = SubTotal;
                moreTotal += totalint;
                double totalAfterlevy = totalint * (levyint / 100) + totalint;
                double totalAfterVat = totalAfterlevy + vatint;
                if (!string.IsNullOrEmpty(tonyTextbox5.Texts) && !string.IsNullOrEmpty(gunaTextBox1.Text))
                {
                    Receipt obj = new Receipt()
                    {
                        Id = Convert.ToInt32(tonyTextbox3.Texts),
                        ProductName = tonyTextbox5.Texts,
                        CompanyName = tonyTextbox18.Texts,
                        Price = Convert.ToDouble(gunaTextBox1.Text),
                        Length = Convert.ToInt32(tonyTextbox15.Texts),
                        Quantity = Convert.ToInt32(tonyTextbox6.Texts)
                    };
                    totalOfAddMore += moreTotal;
                    receiptBindingSource.Add(obj);
                    receiptBindingSource.MoveLast();
                    tonyTextbox3.Texts = string.Empty;
                    tonyTextbox5.Texts = string.Empty;
                    tonyTextbox6.Texts = string.Empty;
                    tonyTextbox15.Texts = string.Empty;
                    gunaTextBox1.Text = string.Empty;


                }
            }
            catch
            {
                MessageBox.Show("Input the right names and figures");
            }
        }

        private void gunaGradientButton6_Click(object sender, EventArgs e)
        {
            Receipt obj = receiptBindingSource.Current as Receipt;
            receiptBindingSource.Remove(obj);
            totalOfAddMore = 0.00;
            totalOfIndItem = 0.00;
            subTotalOfIndItem = 0.00;
            count = 0;
            tonyTextbox3.Texts = "";
            tonyTextbox5.Texts = "";
            tonyTextbox6.Texts = "";
            tonyTextbox15.Texts = "";
            gunaTextBox1.Text = "";
            tonyTextbox15.Texts = "";
            tonyTextbox13.Texts = "";
            tonyTextbox9.Texts = "";
            textBox1.Text = "";
            tonyTextbox13.Texts = "";
            tonyTextbox17.Texts = "";
            tonyTextbox18.Texts = "";
            tonyTextbox19.Texts = "";
            tonyTextbox11.Texts = "";
        }

        private void gunaTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void gunaTextBox1_KeyDown_1(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    perfromPriceCalc();
                }
            }
            catch
            {
                MessageBox.Show("Check all inputs Make sure They are Correct");
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            receiptBindingSource.DataSource = new List<Receipt>();// Init Empty
        }

        private void gunaDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

        }

        private void gunaTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    perfromPriceCalc();
                }
            }
            catch
            {
                MessageBox.Show("Check all inputs Make sure They are Correct");
            }
        }

    }
    
    

}