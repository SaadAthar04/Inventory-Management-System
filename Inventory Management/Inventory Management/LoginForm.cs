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
using System.Collections;
using System.Security.Cryptography;

namespace Inventory_Management
{
    public partial class LoginForm : Form
    {
        OracleConnection con;
        public LoginForm()
        {
            InitializeComponent();
        }

        public static string sellername = "";
        private void LoginForm_Load(object sender, EventArgs e)
        {
            string conStr = @"DATA SOURCE = localhost:1521/xe; USER ID=MAAN;PASSWORD=12345";
            con = new OracleConnection(conStr);
        }

        private void rolebox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
             Uname.Text = "";
             Pass.Text = "";
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            if (Uname.Text == "" || Pass.Text == "")
            {
                MessageBox.Show("Enter Username or Password");
            }
            else
            {
                if (rolebox.SelectedIndex > -1)
                {

                    if (rolebox.SelectedItem.ToString() == "ADMIN")
                    {
                        con.Open();
                        OracleCommand cmd = new OracleCommand("Select password from logintbl where role='admin'", con);
                        string pass=cmd.ExecuteScalar().ToString();
                        con.Close();
                        if (Uname.Text == "Admin" && Hash.Hash_SHA1(Pass.Text) ==pass)
                        {
                            ProductForm pro = new ProductForm();
                            pro.Show();
                            this.Hide();

                        }
                        else
                        {
                            MessageBox.Show("Enter Correct Id And Password");
                        }
                    }
                    else
                    {
                        OracleDataAdapter sda = new OracleDataAdapter("Select count(8) from SellerTbl where Seller_Name='"+Uname.Text+"'and Seller_Pass='"+Pass.Text+"'",con);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows[0][0].ToString() == "1")
                        {
                            sellername=Uname.Text;
                            SellingForm sell = new SellingForm();
                            sell.Show();
                            this.Hide();
                            con.Close();
                        }
                        else
                        {
                            MessageBox.Show("Wrong UserName or Password");
                        }
                        con.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Select a Role");
                }
            }
        }

    }
}