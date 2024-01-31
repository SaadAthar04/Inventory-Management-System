using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
namespace Inventory_Management
{
    public partial class CategoryForm : Form
    {
        OracleConnection con;
        public CategoryForm()
        {
            InitializeComponent();
        }

        private void CatGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CatId.Text = CatGrid.SelectedRows[0].Cells[0].Value.ToString();
            CatName.Text = CatGrid.SelectedRows[0].Cells[1].Value.ToString();
            CatDesc.Text = CatGrid.SelectedRows[0].Cells[2].Value.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                string query = "insert into categorytbl values(" + CatId.Text + ",'" + CatName.Text + "','" + CatDesc.Text + "')";
                OracleCommand cmd = new OracleCommand(query, con);
                int rows = cmd.ExecuteNonQuery();
                MessageBox.Show("Category Added Successfully");
                con.Close();
                ShowGrid();
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

        private void CategoryForm_Load(object sender, EventArgs e)
        {
            string conStr = @"DATA SOURCE = localhost:1521/xe; USER ID=MAAN;PASSWORD=12345";
            con = new OracleConnection(conStr);
            ShowGrid();
        }

        private void ShowGrid()
        {
            con.Open();
            string query = "SELECT * FROM CATEGORYTBL ORDER BY CATEGORY_ID";
            OracleDataAdapter oda = new OracleDataAdapter(query, con);
            OracleCommandBuilder builder = new OracleCommandBuilder(oda);
            var ds = new DataSet();
            oda.Fill(ds);
            CatGrid.DataSource = ds.Tables[0];
            con.Close();

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (CatId.Text == "")
                {
                    MessageBox.Show("Select The Category To Delete");
                }
                else
                {
                    con.Open();
                    string query = " DELETE FROM CATEGORYTBL WHERE Category_Id=" + CatId.Text + "";
                    OracleCommand cmd = new OracleCommand(query, con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Category Deleted Successfully");
                    con.Close();
                    ShowGrid();
                    CatId.Text = "";
                    CatName.Text = "";
                    CatDesc.Text = "";

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
                if (CatId.Text == "" || CatName.Text == "" || CatDesc.Text == "")
                {
                    MessageBox.Show("Missing Information");
                }
                else
                {
                    con.Open();
                    string query = "UPDATE CATEGORYTBL SET CATEGORY_NAME='" + CatName.Text + "',CATEGORY_DESCRIPTION='" + CatDesc.Text + "'WHERE CATEGORY_ID=" + CatId.Text + "";
                    OracleCommand cmd = new OracleCommand(query, con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Category Updated Successfully");
                    con.Close();
                    ShowGrid();
                    CatId.Text = "";
                    CatName.Text = "";
                    CatDesc.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnpro_cat_Click(object sender, EventArgs e)
        {
            ProductForm pro = new ProductForm();
            pro.Show();
            this.Hide();
        }

        private void btnsell_cat_Click(object sender, EventArgs e)
        {
            SellerForm sell = new SellerForm();
            sell.Show();
            this.Hide();
        }

        private void Logoutlbl_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm login = new LoginForm();
            login.Show();
        }
    }
}
