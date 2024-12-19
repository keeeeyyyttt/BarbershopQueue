using System;
using System.Windows.Forms;

namespace TheBarberShop
{
    public partial class FrmHome : Form
    {
        public FrmHome()
        {
            InitializeComponent(); //initialize the form components
        }

        private void realTime_Tick(object sender, EventArgs e)
        {
            DateTime datetime = DateTime.Now; //get the current date and time
            this.lblRealTime.Text = datetime.ToString(); //update the real-time label
        }

        private void FrmHome_Load(object sender, EventArgs e)
        {
            realTime.Start(); //start the real-time clock
        }

        private void btnNewCust_Click(object sender, EventArgs e)
        {
            FrmRegistration registration = new FrmRegistration(); //create new registration form
            registration.Show(); //show the registration form
            this.Hide(); //hide the current form
        }

        private void lbLogout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult logout = MessageBox.Show("Are you sure you want to Logout?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Information); //ask user for logout confirmation
            if (logout == DialogResult.OK)
            {
                this.Close(); //close the current form
                FrmLogin home = new FrmLogin(); //create a new login form
                home.Show(); //show the login form
            }
            if (logout == DialogResult.Cancel)
            {
                this.Focus(); //keep the current form open if user cancels logout
            }
        }

        private void btnExistCust_Click(object sender, EventArgs e)
        {
            FrmExistingCust existing = new FrmExistingCust(); //create new existing customer form
            existing.Show(); //show the existing customer form
            this.Hide(); //hide the current form
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            FrmHistory frmHistory = new FrmHistory(); //create new history form
            frmHistory.Show(); //show the history form
            this.Hide(); //hide the current form
        }
    }
}
