using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TheBarberShop
{
    public partial class FrmExistingCust : Form
    {
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=E:\\Downloads\\System_TP\\TheBarberShop\\TheBarberShop\\BarberDB.mdf;Integrated Security=True";

        public FrmExistingCust()
        {
            InitializeComponent(); //initialize the form components
            LoadCustomers(); //load customer data
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.Close(); //close the current form
            FrmHome back = new FrmHome(); //create a new home form
            back.Show(); //show the home form
        }

        private void LoadCustomers()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open(); //open the database connection
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM customer_Info", conn); //retrieve customer data from customer_Info
                DataTable dt = new DataTable();
                da.Fill(dt); //fill the data table with the customer data
                dataViewCustomers.DataSource = dt; //bind the data table to the DataGridView
            }
        }

        private void btnBookNow_Click(object sender, EventArgs e)
        {
            if (dataViewCustomers.SelectedRows.Count > 0) //check if a row is selected
            {
                DataGridViewRow selectedRow = dataViewCustomers.SelectedRows[0]; //get the selected row

                string firstName = selectedRow.Cells["FirstName"].Value.ToString(); //get first name
                string lastName = selectedRow.Cells["LastName"].Value.ToString(); //get last name
                string phoneNumber = selectedRow.Cells["PhoneNumber"].Value.ToString(); //get phone number
                string customerType = selectedRow.Cells["customerType"].Value.ToString(); //get customer type

                FrmBooking frmBooking = new FrmBooking(firstName, lastName, phoneNumber, customerType); //create a new booking form
                frmBooking.Show(); //show the booking form
                this.Hide(); //hide the current form
            }
            else
            {
                MessageBox.Show("Please select a row first."); //show a message if no row is selected
            }
        }
    }
}
