using Inventory_Management;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Inventory_Management
{
    public partial class ProductForm : Form
    {
        OracleConnection con;
        public ProductForm()
        {
            InitializeComponent();
        }

        private void ProductForm_Load(object sender, EventArgs e)
        {
            string conStr = @"DATA SOURCE = localhost:1521/xe; USER ID=MAAN;PASSWORD=12345";
            con = new OracleConnection(conStr);
            fillcatselect();
            ShowGrid();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
            CatSelect.ValueMember = "category_name";
            CatSelect.DataSource = dt;
            SearchBox.ValueMember = "category_name";
            SearchBox.DataSource = dt;
            con.Close();
        }
        private void ShowGrid()
        {
            con.Open();
            string query = "SELECT * FROM PRODUCTTBL ORDER BY PRODUCT_ID";
            OracleDataAdapter oda = new OracleDataAdapter(query, con);
            OracleCommandBuilder builder = new OracleCommandBuilder(oda);
            var ds = new DataSet();
            oda.Fill(ds);
            ProGrid.DataSource = ds.Tables[0];
            con.Close();

        }

        private void btncat_pro_Click(object sender, EventArgs e)
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
                string query = "insert into producttbl values(" + ProId.Text + ",'" + ProName.Text + "'," + ProQuantity.Text + "," + ProPrice.Text + ",'" + CatSelect.SelectedValue.ToString() + "')";
                OracleCommand cmd = new OracleCommand(query, con);
                int rows = cmd.ExecuteNonQuery();
                MessageBox.Show("Product Added Successfully");
                con.Close();
                ShowGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ProGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ProId.Text = ProGrid.SelectedRows[0].Cells[0].Value.ToString();
            ProName.Text = ProGrid.SelectedRows[0].Cells[1].Value.ToString();
            ProQuantity.Text = ProGrid.SelectedRows[0].Cells[2].Value.ToString();
            ProPrice.Text = ProGrid.SelectedRows[0].Cells[3].Value.ToString();
            CatSelect.SelectedValue = ProGrid.SelectedRows[0].Cells[2].Value.ToString();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (ProId.Text == "")
                {
                    MessageBox.Show("Select The Product To Delete");
                }
                else
                {
                    con.Open();
                    string query = " DELETE FROM PRODUCTTBL WHERE PRODUCT_Id=" + ProId.Text + "";
                    OracleCommand cmd = new OracleCommand(query, con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product Deleted Successfully");
                    con.Close();
                    ShowGrid();
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
                if (ProId.Text == "" || ProName.Text == "" || ProQuantity.Text == "" || ProPrice.Text == "" || CatSelect.SelectedValue.ToString() == "")
                {
                    MessageBox.Show("Missing Information");
                }
                else
                {
                    con.Open();
                    string query = "UPDATE PRODUCTTBL SET PRODUCT_NAME='" + ProName.Text + "',PRODUCT_QUANTITY=" + ProQuantity.Text + ",PRODUCT_PRICE=" + ProPrice.Text + ",PRODUCT_CATEGORY='" + CatSelect.SelectedValue.ToString() + "'WHERE PRODUCT_ID=" + ProId.Text + "";

                    OracleCommand cmd = new OracleCommand(query, con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product Updated Successfully");
                    con.Close();
                    ShowGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnsell_pro_Click(object sender, EventArgs e)
        {
            SellerForm sell = new SellerForm();
            sell.Show();
            this.Hide();
        }

        private void CatSelect_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SearchBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            con.Open();
            string query = "select * from ProductTbl where Product_category='" + SearchBox.SelectedValue.ToString() + "'";

            OracleDataAdapter sda = new OracleDataAdapter(query, con);
            OracleCommandBuilder builder = new OracleCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            ProGrid.DataSource = ds.Tables[0];
            con.Close();
        }

        private void Logoutlbl_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm login = new LoginForm();
            login.Show();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ShowGrid();
        }

        private void SearchBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

