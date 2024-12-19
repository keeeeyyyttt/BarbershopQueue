using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TheBarberShop
{
    public partial class FrmHistory : Form
    {
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=E:\\Downloads\\System_TP\\TheBarberShop\\TheBarberShop\\BarberDB.mdf;Integrated Security=True";

        public FrmHistory()
        {
            InitializeComponent(); //initialize the form components
            LoadHistory(); //load the booking history
        }

        private void LoadHistory()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open(); //open the database connection
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM booking_History", conn); //retrieve data from booking_History
                DataTable dt = new DataTable();
                da.Fill(dt); //fill the data table with the query result
                dgHistory.DataSource = dt; //bind the data table to the DataGridView
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            FrmHome frmHome = new FrmHome(); //create new home form
            frmHome.Show(); //show the home form
            this.Hide(); //hide the current form
        }

        private void btnAddQueue_Click(object sender, EventArgs e)
        {
            if (dgHistory.SelectedRows.Count > 0) //check if a row is selected
            {
                DataGridViewRow selectedRow = dgHistory.SelectedRows[0]; //get the selected row
                string status = selectedRow.Cells["Status"].Value.ToString(); //get the status of the selected row

                if (status == "Cancelled") //check if the booking is cancelled
                {
                    MessageBox.Show("This booking has been cancelled and cannot be added to the queue. Please book again."); //show a message
                    return; //exit the method
                }

                string firstName = selectedRow.Cells["FirstName"].Value.ToString(); //get first name
                string lastName = selectedRow.Cells["LastName"].Value.ToString(); //get last name
                string service = selectedRow.Cells["Service"].Value.ToString(); //get service

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); //open the database connection
                    string query = "UPDATE booking_History SET Status = @Status WHERE FirstName = @FirstName AND LastName = @LastName AND Service = @Service"; //update the status of the booking
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Status", "Active"); //set status to active
                        cmd.Parameters.AddWithValue("@FirstName", firstName); //add first name parameter
                        cmd.Parameters.AddWithValue("@LastName", lastName); //add last name parameter
                        cmd.Parameters.AddWithValue("@Service", service); //add service parameter
                        cmd.ExecuteNonQuery(); //execute the update query
                    }
                }

                AddToQueue(firstName, lastName, service); //add to queue
                LoadHistory(); //reload history
            }
            else
            {
                MessageBox.Show("Please select a row to add to the queue."); //show a message if no row is selected
            }
        }

        private bool AddToQueue(string firstName, string lastName, string service)
        {
            foreach (DataGridViewRow row in dgHistory.Rows) //iterate through each row in history
            {
                if (row.Cells["FirstName"].Value.ToString() == firstName &&
                    row.Cells["LastName"].Value.ToString() == lastName &&
                    row.Cells["Service"].Value.ToString() == service) //check if the row already exists in the queue
                {
                    return false; //if exists, return false
                }
            }
            dgHistory.Rows.Add(firstName, lastName, service, "Active"); //add a new row to the queue
            return true; //return true after successfully adding
        }

        private void btnRemoveQueue_Click(object sender, EventArgs e)
        {
            if (dgHistory.SelectedRows.Count > 0) //check if a row is selected
            {
                DataGridViewRow selectedRow = dgHistory.SelectedRows[0]; //get the selected row
                string firstName = selectedRow.Cells["FirstName"].Value.ToString(); //get first name
                string lastName = selectedRow.Cells["LastName"].Value.ToString(); //get last name
                string service = selectedRow.Cells["Service"].Value.ToString(); //get service

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); //open the database connection
                    string query = "UPDATE booking_History SET Status = @Status WHERE FirstName = @FirstName AND LastName = @LastName AND Service = @Service"; //update the status to done
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Status", "Done"); //set status to done
                        cmd.Parameters.AddWithValue("@FirstName", firstName); //add first name parameter
                        cmd.Parameters.AddWithValue("@LastName", lastName); //add last name parameter
                        cmd.Parameters.AddWithValue("@Service", service); //add service parameter
                        cmd.ExecuteNonQuery(); //execute the update query
                    }
                }

                RemoveToQueue(firstName, lastName, service); //remove from queue
                LoadHistory(); //reload history
            }
            else
            {
                MessageBox.Show("Please select a row to remove from the queue."); //show a message if no row is selected
            }
        }

        private bool RemoveToQueue(string firstName, string lastName, string service)
        {
            foreach (DataGridViewRow row in dgHistory.Rows) //iterate through each row in history
            {
                if (row.Cells["FirstName"].Value.ToString() == firstName &&
                    row.Cells["LastName"].Value.ToString() == lastName &&
                    row.Cells["Service"].Value.ToString() == service) //check if the row matches
                {
                    row.Cells["Status"].Value = "Done"; //set the status to done
                    return true; //return true after removing from queue
                }
            }
            return false; //return false if not found
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (dgHistory.SelectedRows.Count > 0) //check if a row is selected
            {
                DataGridViewRow selectedRow = dgHistory.SelectedRows[0]; //get the selected row
                string status = selectedRow.Cells["Status"].Value.ToString(); //get the status

                if (status == "Done") //check if the booking is already done
                {
                    MessageBox.Show("This booking has already been completed and cannot be canceled."); //show a message
                    return; //exit the method
                }

                string firstName = selectedRow.Cells["FirstName"].Value.ToString(); //get first name
                string lastName = selectedRow.Cells["LastName"].Value.ToString(); //get last name
                string service = selectedRow.Cells["Service"].Value.ToString(); //get service

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); //open the database connection
                    string query = "UPDATE booking_History SET Status = @Status WHERE FirstName = @FirstName AND LastName = @LastName AND Service = @Service"; //update the status to cancelled
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Status", "Cancelled"); //set status to cancelled
                        cmd.Parameters.AddWithValue("@FirstName", firstName); //add first name parameter
                        cmd.Parameters.AddWithValue("@LastName", lastName); //add last name parameter
                        cmd.Parameters.AddWithValue("@Service", service); //add service parameter
                        cmd.ExecuteNonQuery(); //execute the update query
                    }
                }

                CancelQueue(firstName, lastName, service); //cancel the queue
                LoadHistory(); //reload history
            }
            else
            {
                MessageBox.Show("Please select a row to cancel."); //show a message if no row is selected
            }
        }

        private bool CancelQueue(string firstName, string lastName, string service)
        {
            foreach (DataGridViewRow row in dgHistory.Rows) //iterate through each row in history
            {
                if (row.Cells["FirstName"].Value.ToString() == firstName &&
                    row.Cells["LastName"].Value.ToString() == lastName &&
                    row.Cells["Service"].Value.ToString() == service) //check if the row matches
                {
                    row.Cells["Status"].Value = "Cancelled"; //set the status to cancelled
                    return true; //return true after cancelling the queue
                }
            }
            return false; //return false if not found
        }
    }
}
