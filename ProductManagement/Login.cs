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

namespace ProductManagement
{
    public partial class Login : Form
    {
        SqlConnection connection;
        public Login()
        {
            InitializeComponent();
            connection = new SqlConnection("Server=CNTT1\\SQLEXPRESS;Database=product_management;Integrated Security = true;");
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string query = "select * from account where username=@username and u_password=@password";
            connection.Open();
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@username", SqlDbType.VarChar);
            cmd.Parameters["@username"].Value = username;
            cmd.Parameters.AddWithValue("@password", SqlDbType.VarChar);
            cmd.Parameters["@password"].Value = password;
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string role = reader["u_role"].ToString();
                if (role.Equals("admin"))
                {
                    MessageBox.Show(this, "Login successful!", "Result", MessageBoxButtons.OK, MessageBoxIcon.None);
                    this.Hide();
                    Form1 form = new Form1(username);
                    form.ShowDialog();
                    this.Dispose();
                }
                else if (role.Equals("user"))
                {
                    MessageBox.Show(this, "Login successful!", "Result", MessageBoxButtons.OK, MessageBoxIcon.None);
                    this.Hide();
                    Form1 form = new Form1(username);
                    form.ShowDialog();
                    this.Dispose();
                }
                else
                    lblError.Text = "You are not allowed to access";
            }
            else
            {
                lblError.Text = "Wrong username or password";
            }
            connection.Close();
        }

        private void btnExist_Click(object sender, EventArgs e)
        {
            if((MessageBox.Show(this, "Do you want to exit?", "Question", MessageBoxButtons.OK, MessageBoxIcon.Question) == DialogResult.Yes))
            {
                Application.Exit();
            }
        }
    }
}
