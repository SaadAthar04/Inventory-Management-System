using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
namespace Inventory_Management
{
    public partial class SellingForm : Form
    {
        OracleConnection con;
        public SellingForm()
        {
            InitializeComponent();
        }

        private void SellingForm_Load(object sender, EventArgs e)
        {
            string conStr = @"DATA SOURCE = localhost:1521/xe; USER ID=MAAN;PASSWORD=12345";
            con = new OracleConnection(conStr);
            ShowGrid();
            ShowBillGrid();
            fillcatselect();
            SellerNamelbl.Text = LoginForm.sellername;
        }

        private void ShowGrid()
        {
            con.Open();
            string query = "SELECT PRODUCT_NAME,PRODUCT_QUANTITY,PRODUCT_PRICE FROM PRODUCTTBL";
            OracleDataAdapter oda = new OracleDataAdapter(query, con);
            OracleCommandBuilder builder = new OracleCommandBuilder(oda);
            var ds = new DataSet();
            oda.Fill(ds);
            SellProGrid.DataSource = ds.Tables[0];
            con.Close();

        }

        private void ShowBillGrid()
        {
            con.Open();
            string query = "SELECT * FROM BILLTBL";
            OracleDataAdapter oda = new OracleDataAdapter(query, con);
            OracleCommandBuilder builder = new OracleCommandBuilder(oda);
            var ds = new DataSet();
            oda.Fill(ds);
            BillGrid.DataSource = ds.Tables[0];
            con.Close();

        }
        private void SellProGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ProName.Text = SellProGrid.SelectedRows[0].Cells[0].Value.ToString();
            ProPrice.Text = SellProGrid.SelectedRows[0].Cells[1].Value.ToString();
        }

        private void EXIT_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Datelbl_Click(object sender, EventArgs e)
        {
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Datelbl.Text = DateTime.Today.Day.ToString() + "/" + DateTime.Today.Month.ToString() + "/" + DateTime.Today.Year.ToString();
        }
        int grand_total = 0, n = 0;

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (BillId.Text == "")
            {
                MessageBox.Show("Missing Bill ID");
            }
            else
            {
                try
                {
                    con.Open();
                    string query = "insert into billtbl values(" + BillId.Text + ",'" + SellerNamelbl.Text + "','" + Datelbl.Text + "'," + AmountLbl.Text + ")";
                    OracleCommand cmd = new OracleCommand(query, con);
                    int rows = cmd.ExecuteNonQuery();
                    MessageBox.Show("Order Added Successfully");
                    con.Close();
                    ShowBillGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void BillGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString("INVENTORY MANAGEMENT SYSTEM",new Font("Century Gothic",25,FontStyle.Bold),Brushes.Red,new Point(140));
            e.Graphics.DrawString("Bill ID: "+BillGrid.SelectedRows[0].Cells[0].Value.ToString(), new Font("Century Gothic", 20, FontStyle.Bold), Brushes.Blue, new Point(100,70));
            e.Graphics.DrawString("Seller Name: " + BillGrid.SelectedRows[0].Cells[1].Value.ToString(), new Font("Century Gothic", 20, FontStyle.Bold), Brushes.Blue, new Point(100, 100));
            e.Graphics.DrawString("Date: " + BillGrid.SelectedRows[0].Cells[2].Value.ToString(), new Font("Century Gothic", 20, FontStyle.Bold), Brushes.Blue, new Point(100, 130));
            e.Graphics.DrawString("Total Amount: " + BillGrid.SelectedRows[0].Cells[3].Value.ToString(), new Font("Century Gothic", 20, FontStyle.Bold), Brushes.Blue, new Point(100, 160));
            e.Graphics.DrawString("THANK YOU!!", new Font("Century Gothic", 20, FontStyle.Italic), Brushes.Red, new Point(270,230));

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ShowGrid();
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            con.Open();
            string query = "SELECT PRODUCT_NAME,PRODUCT_QUANTITY,PRODUCT_PRICE FROM PRODUCTTBL where Product_category='" + comboBox1.SelectedValue.ToString() + "'";
            OracleDataAdapter sda=new OracleDataAdapter(query, con);
            OracleCommandBuilder builder=new OracleCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            SellProGrid.DataSource = ds.Tables[0];
            con.Close();
        }

        private void fillcatselect()
        {
            con.Open();
            OracleCommand cmd = new OracleCommand("SELECT CATEGORY_NAME FROM CATEGORYTBL", con);
            OracleDataReader rdr;
            rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("category_name", typeof(string));
            dt.Load(rdr);
            comboBox1.ValueMember = "category_name";
            comboBox1.DataSource = dt;
            con.Close();
        }

        private void Logoutlbl_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm login = new LoginForm();
            login.Show();
        }

        private void btnAddPro_Click(object sender, EventArgs e)
        {
            
            if (ProName.Text == "" || ProQuantity.Text == "")
            {
                MessageBox.Show("Missing Data");
            }
            else
            {
                int total = Convert.ToInt32(ProPrice.Text) * Convert.ToInt32(ProQuantity.Text);
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(OrderGrid);
                row.Cells[0].Value = n + 1;
                row.Cells[1].Value = ProName.Text;
                row.Cells[2].Value = ProPrice.Text;
                row.Cells[3].Value = ProQuantity.Text;
                row.Cells[4].Value = Convert.ToInt32(ProPrice.Text) * Convert.ToInt32(ProQuantity.Text);
                OrderGrid.Rows.Add(row);
                n++;
                grand_total = grand_total + total;
                AmountLbl.Text = "" + grand_total;
            }
        }
    }
}