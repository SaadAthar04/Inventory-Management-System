using Inventory_Management;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
namespace Inventory_Management
{
    public partial class SellerForm : Form
    {
        OracleConnection con;
        public SellerForm()
        {
            InitializeComponent();
        }

        private void SellerForm_Load(object sender, EventArgs e)
        {
            string conStr = @"DATA SOURCE = localhost:1521/xe; USER ID=MAAN;PASSWORD=12345";
            con = new OracleConnection(conStr);
            ShowGrid();
            fillselect();
        }

        private void SellGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            SellId.Text = SellGrid.SelectedRows[0].Cells[0].Value.ToString();
            SellName.Text = SellGrid.SelectedRows[0].Cells[1].Value.ToString();
            SellAge.Text = SellGrid.SelectedRows[0].Cells[2].Value.ToString();
            SellPhone.Text = SellGrid.SelectedRows[0].Cells[3].Value.ToString();
            SellPass.Text = SellGrid.SelectedRows[0].Cells[2].Value.ToString();
        }

        private void ShowGrid()
        {
            con.Open();
            string query = "SELECT * FROM SELLERTBL ORDER BY SELLER_ID";
            OracleDataAdapter oda = new OracleDataAdapter(query, con);
            OracleCommandBuilder builder = new OracleCommandBuilder(oda);
            var ds = new DataSet();
            oda.Fill(ds);
            SellGrid.DataSource = ds.Tables[0];
            con.Close();

        }

        private void btnpro_sell_Click(object sender, EventArgs e)
        {
            ProductForm pro = new ProductForm();
            pro.Show();
            this.Hide();
        }

        private void btncat_sell_Click(object sender, EventArgs e)
        {
            CategoryForm cat = new CategoryForm();
            cat.Show();
            this.Hide();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                string query = "insert into sellertbl values(" + SellId.Text + ",'" + SellName.Text + "'," + SellAge.Text + ",'" + SellPhone.Text + "','" + SellPass.Text + "')";
                OracleCommand cmd = new OracleCommand(query, con);
                int rows = cmd.ExecuteNonQuery();
                MessageBox.Show("Seller Added Successfully");
                con.Close();
                ShowGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (SellId.Text == "")
                {
                    MessageBox.Show("Select The Seller To Delete");
                }
                else
                {
                    con.Open();
                    string query = " DELETE FROM SELLERTBL WHERE SELLER_Id=" + SellId.Text + "";
                    OracleCommand cmd = new OracleCommand(query, con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Seller Deleted Successfully");
                    con.Close();
                    ShowGrid();
                    SellId.Text = "";
                    SellName.Text = "";
                    SellAge.Text = "";
                    SellPhone.Text = "";
                    SellPass.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (SellId.Text == "" || SellName.Text == "" || SellAge.Text == "" || SellPhone.Text == "" || SellPass.Text == "")
                {
                    MessageBox.Show("Missing Information");
                }
                else
                {
                    con.Open();
                    string query = "UPDATE SELLERTBL SET SELLER_NAME='" + SellName.Text + "',SELLER_AGE=" + SellAge.Text + ",SELLER_PHONE='" + SellPhone.Text + "',SELLER_Pass='" + SellPass.Text + "' WHERE SELLER_ID=" + SellId.Text + "";
                    OracleCommand cmd = new OracleCommand(query, con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Seller Updated Successfully");
                    con.Close();
                    ShowGrid();
                    SellId.Text = "";
                    SellName.Text = "";
                    SellAge.Text = "";
                    SellPhone.Text = "";
                    SellPass.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Logoutlbl_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm login = new LoginForm();
            login.Show();
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            con.Open();
            string query = "select * from SellerTbl where Seller_Name='" + comboBox1.SelectedValue.ToString() + "'";

            OracleDataAdapter sda = new OracleDataAdapter(query, con);
            OracleCommandBuilder builder = new OracleCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            SellGrid.DataSource = ds.Tables[0];
            con.Close();
        }
        private void fillselect()
        {
            con.Open();
            OracleCommand cmd = new OracleCommand("SELECT SELLER_NAME FROM SELLERTBL", con);
            OracleDataReader rdr;
            rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("seller_name", typeof(string));
            dt.Load(rdr);
            //CatSelect.ValueMember = "category_name";
            //CatSelect.DataSource = dt;
            comboBox1.ValueMember = "seller_name";
            comboBox1.DataSource = dt;
            con.Close();
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ShowGrid();
        }
    }
}