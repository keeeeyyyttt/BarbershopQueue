using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TheBarberShop
{
    public partial class FrmRegistration : Form
    {
        //connects to the database BarberDB
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=E:\\Downloads\\System_TP\\TheBarberShop\\TheBarberShop\\BarberDB.mdf;Integrated Security=True";

        public FrmRegistration()
        {
            InitializeComponent();
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            //back button
            this.Close();
            FrmHome back = new FrmHome();
            back.Show();
        }

        private void btnRegister_Click(object sender, EventArgs e) //click event for the register button
        {
            string firstName = txtFirstname.Text;
            string lastName = txtLastname.Text;
            string phoneNumber = txtPhonenum.Text;
            string customerType = cbType.SelectedItem?.ToString();

            //the following if statements are for the validation. If a textbox/combo box is empty, it will prompt the user to fill it.
            if (string.IsNullOrWhiteSpace(firstName))
            {
                MessageBox.Show("First Name cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                MessageBox.Show("Last Name cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                MessageBox.Show("Phone Number cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!long.TryParse(phoneNumber, out _) || phoneNumber.Length != 11 || !phoneNumber.StartsWith("09"))
            {
                MessageBox.Show("Phone Number must be 11 digits long and start with 09.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (customerType == null)
            {
                MessageBox.Show("Please select a Customer Type.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection dataconn = new SqlConnection(connectionString)) //this will fill the table customer_Info
            {
                try
                {
                    dataconn.Open();
                    string query = "INSERT INTO customer_Info (FirstName, LastName, PhoneNumber, CustomerType) VALUES (@FirstName, @LastName, @PhoneNumber, @CustomerType)";
                    SqlCommand cmd = new SqlCommand(query, dataconn);
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);
                    cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                    cmd.Parameters.AddWithValue("@CustomerType", customerType);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer registered successfully!");
                    FrmHome frmHome = new FrmHome();
                    frmHome.Show();
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); //catch statement just in case an error shows
                }
            }
        }
    }
}
