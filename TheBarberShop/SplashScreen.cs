using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TheBarberShop
{
    public partial class FrmLoading : Form
    {
        public FrmLoading()
        {
            InitializeComponent();
        }

        private void loadingTimer_Tick(object sender, EventArgs e)
        {
            plLoading2.Width += 3;

            if (plLoading2.Width >= 360)
            {
                loadingTimer.Stop();
                FrmLogin Login = new FrmLogin();
                Login.Show();
                this.Hide();
            }
        }
    }
}
