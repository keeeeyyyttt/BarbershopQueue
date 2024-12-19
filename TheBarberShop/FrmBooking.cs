using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TheBarberShop
{
    public partial class FrmBooking : Form
    {
        private decimal haircutPrice = 200; //price for haircut service
        private decimal beardTrimPrice = 150; //price for beard trimming service
        private decimal hairStylingPrice = 250; //price for hair styling service
        private decimal hairColoringPrice = 300; //price for hair coloring service
        private decimal shavesPrice = 100; //price for shaves service
        private decimal facialPrice = 180; //price for facial service

        private decimal studentDiscount = 0.10m; //discount for students
        private decimal seniorDiscount = 0.20m; //discount for senior citizens
        private decimal regularDiscount = 0.0m; //no discount for regular customers

        private decimal currentPrice = 0.0m; //current price for selected service
        private decimal discount = 0.0m; //applied discount based on customer type

        public FrmBooking(string firstName, string lastName, string contact, string customerType)
        {
            InitializeComponent();
            lblName.Text = $"{firstName} {lastName}"; //set customer full name
            lblContact.Text = contact; //set customer contact number

            //apply discount based on customer type
            if (customerType == "Student")
                discount = studentDiscount;
            else if (customerType == "Senior Citizen")
                discount = seniorDiscount;
            else
                discount = regularDiscount;
        }

        private void FrmBooking_Load(object sender, EventArgs e)
        {
            cbServices.SelectedIndex = -1; //clear the service dropdown selection on load
        }

        private void btnBookNow_Click(object sender, EventArgs e)
        {
            string selectedService = cbServices.SelectedItem?.ToString(); //get selected service
            if (string.IsNullOrEmpty(selectedService))
            {
                MessageBox.Show("Please select a service before booking.");
                return;
            }
            string selectedTime = cbTime.SelectedItem?.ToString(); //get selected time
            if (string.IsNullOrEmpty(selectedTime) || selectedTime.Contains("Unavailable"))
            {
                MessageBox.Show("Please select a valid time slot before booking.");
                return;
            }

            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=E:\\Downloads\\System_TP\\TheBarberShop\\TheBarberShop\\BarberDB.mdf;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string service = cbServices.SelectedItem?.ToString();
                string time = cbTime.SelectedItem?.ToString();
                conn.Open();
                string query = "INSERT INTO booking_History (FirstName, LastName, PhoneNumber, Service, Time, BookTime, Status, Amount)" +
                               "VALUES (@FirstName, @LastName, @PhoneNumber, @Service, @Time, @BookTime, @Status, @Amount)"; //insert booking details into database

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    string fullName = lblName.Text;
                    int spaceIndex = fullName.IndexOf(' '); //get the space index between first and last name

                    string firstName;
                    string lastName;

                    //split the full name into first and last names
                    if (spaceIndex > 0)
                    {
                        firstName = fullName.Substring(0, spaceIndex);
                        lastName = fullName.Substring(spaceIndex + 1);
                    }
                    else
                    {
                        firstName = fullName;
                        lastName = string.Empty;
                    }

                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);
                    cmd.Parameters.AddWithValue("@PhoneNumber", lblContact.Text);
                    cmd.Parameters.AddWithValue("@Service", service);
                    cmd.Parameters.AddWithValue("@Time", time);
                    cmd.Parameters.AddWithValue("@BookTime", DateTime.Now); //set the current time as the booking time
                    cmd.Parameters.AddWithValue("@Status", "Pending"); //set the initial status as pending
                    cmd.Parameters.AddWithValue("@Amount", lblDiscAmount.Text); //set the amount after discount

                    try
                    {
                        cmd.ExecuteNonQuery(); //execute the insert query
                        MessageBox.Show("Booking confirmed!"); //notify user of successful booking

                        FrmHome frmHome = new FrmHome();
                        frmHome.Show(); //redirect to home form
                        this.Hide();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred while booking: " + ex.Message); //show error message if booking fails
                    }
                }
            }
        }

        private void cbTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check if selected time is unavailable
            if (cbTime.SelectedItem != null && cbTime.SelectedItem.ToString().Contains("Unavailable"))
            {
                MessageBox.Show("You cannot select an unavailable time slot.");
                cbTime.SelectedIndex = -1; //reset time selection
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            FrmHome back = new FrmHome();
            back.Show(); //return to home form
            this.Hide();
        }

        private void cbServices_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
            {
                if (cbServices.SelectedItem == null)
                {
                    lblOrigAmount.Text = "₱0.00"; //set original amount to 0 if no service is selected
                    lblDiscAmount.Text = "₱0.00"; //set discounted amount to 0
                    return;
                }

                string selectedService = cbServices.SelectedItem.ToString();
                switch (selectedService)
                {
                    case "Haircut":
                        currentPrice = haircutPrice; //set price for haircut
                        break;
                    case "Beard Trimming":
                        currentPrice = beardTrimPrice; //set price for beard trimming
                        break;
                    case "Hair Styling":
                        currentPrice = hairStylingPrice; //set price for hair styling
                        break;
                    case "Hair Coloring":
                        currentPrice = hairColoringPrice; //set price for hair coloring
                        break;
                    case "Shaves":
                        currentPrice = shavesPrice; //set price for shaves
                        break;
                    case "Facial":
                        currentPrice = facialPrice; //set price for facial
                        break;
                    default:
                        MessageBox.Show("Invalid service selected.");
                        return;
                }

                lblOrigAmount.Text = $"₱{currentPrice:F2}"; //display original price
                lblDiscAmount.Text = $"₱{currentPrice * (1 - discount):F2}"; //display price after discount
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message); //handle errors
            }
        }
    }
}
