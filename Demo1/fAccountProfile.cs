using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Demo.DAO;

namespace Demo
{
    public partial class fAccountProfile : Form
        
    {
        DataProvider provider = new DataProvider();
        private static string chuoiKN = "server=.;database=QL_QUANCAFE;integrated security = SSPI";
        string dn;
        public fAccountProfile()
        {
            InitializeComponent();
        }
        public fAccountProfile(string i)
        {
            InitializeComponent();
            dn = i;
        }
        private void usernameTB_TextChanged(object sender, EventArgs e)
        {
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string chuoiSQL = "Select * from tbl_Account where DisplayName ='" + dn + "'";
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(txbNewPassword.Text);
            byte[] data = new MD5CryptoServiceProvider().ComputeHash(temp);
            string passf = "";
            foreach (byte item in data)
            {
                passf += item;
            }
            string pass = passf.Substring(4, 12);
            DataTable dt = provider.ExecuteQuerry(chuoiSQL);
            if (pass == dt.Rows[0]["Pass"].ToString())
            {
                MessageBox.Show("New assword can not be your current password!!");
            }
            else if (txbNewPassword.Text == txbReEnter.Text)
            {
                string chuoiSQL1 = "Update tbl_Account set Pass = N'" + pass + "' where DisplayName = '" + dn + "'";
                provider.ThucThi(chuoiSQL1);
                MessageBox.Show("Password updated!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Input did not match!!");
            }
        }
    }
}
