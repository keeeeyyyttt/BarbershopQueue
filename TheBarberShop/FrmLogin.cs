using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TheBarberShop
{
    public partial class FrmLogin : Form
    {
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=E:\\Downloads\\System_TP\\TheBarberShop\\TheBarberShop\\BarberDB.mdf;Integrated Security=True";

        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtEmployee.Text; //will get the input from txtEmployee
            string password = txtPassword.Text; //will get the input from txtPassword

            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                connect.Open();
                //COLLATE Latin1 will validate the casing of the password
                string query = "SELECT COUNT(*) FROM cashier_Credentials WHERE Username = @Username AND Password = @Password COLLATE Latin1_General_BIN";
                SqlCommand cmd = new SqlCommand(query, connect);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                int result = (int)cmd.ExecuteScalar();

                if (result > 0)
                {
                    MessageBox.Show("Login successful!");
                    FrmHome mainForm = new FrmHome();
                    mainForm.Show();
                    FrmQueue frmQueue = new FrmQueue();
                    frmQueue.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Incorrect username or password. Try again.");
                }
            }
        }

        private void showPass_CheckedChanged(object sender, EventArgs e) //show password checkbox
        {
            if (showPass.Checked)
            {
                txtPassword.UseSystemPasswordChar = false;
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true;
            }
        }
    }
}
