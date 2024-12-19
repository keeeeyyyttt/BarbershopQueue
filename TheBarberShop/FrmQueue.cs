using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TheBarberShop
{
    public partial class FrmQueue : Form
    {
        //connection string to connect to the database
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=E:\\Downloads\\System_TP\\TheBarberShop\\TheBarberShop\\BarberDB.mdf;Integrated Security=True";

        //property to store the queue name
        public string QueueName { get; set; }

        public FrmQueue()
        {
            InitializeComponent(); //initialize the form components
            SetupListViews(); //set up the ListView controls
            LoadData(); //load the data from the database
        }

        private void SetupListViews()
        {
            //set up the lvQueue ListView
            lvQueue.View = View.Details;
            lvQueue.Columns.Add("Name", 100); //add Name column
            lvQueue.Columns.Add("Service", 100); //add Service column
            lvQueue.Columns.Add("Time", 100); //add Time column

            //set up the lvPending ListView
            lvPending.View = View.Details;
            lvPending.Columns.Add("Name", 100); //add Name column
            lvPending.Columns.Add("Service", 100); //add Service column
            lvPending.Columns.Add("Time", 100); //add Time column
        }

        private void LoadData()
        {
            //clear the existing items to avoid duplicates
            lvQueue.Items.Clear();
            lvPending.Items.Clear();

            //open a connection to the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open(); //open the connection
                string query = "SELECT FirstName, LastName, Service, Time, Status FROM booking_History"; //query to fetch booking data
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader(); //execute the query and get a data reader

                //loop through the data and add it to the ListViews
                while (reader.Read())
                {
                    //get data from each column
                    string fname = reader["FirstName"].ToString();
                    string lname = reader["LastName"].ToString();
                    string service = reader["Service"].ToString();
                    string time = reader["Time"].ToString();
                    string status = reader["Status"].ToString();

                    //combine first and last name into a full name
                    string fullName = $"{fname} {lname}";

                    //create a ListView item and add the data
                    ListViewItem item = new ListViewItem(fullName);
                    item.SubItems.Add(service);
                    item.SubItems.Add(time);

                    //add the item to the appropriate ListView based on the status
                    if (status == "Active")
                    {
                        lvQueue.Items.Add(item); //add to active queue
                    }
                    else if (status == "Pending")
                    {
                        lvPending.Items.Add(item); //add to pending queue
                    }
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //refresh the data in the ListViews
            LoadData();
        }
    }
}
