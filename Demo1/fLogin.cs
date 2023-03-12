using Demo.DAO;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Demo
{
    public partial class fLogin : Form
    {
        public static string chuoiKN = "server=.;database=QL_QUANCAFE;integrated security = SSPI";
        SqlConnection cn = new SqlConnection(chuoiKN);

        DataProvider provider = new DataProvider();
        public fLogin()
        {
            InitializeComponent();
        }
        private void LoginButton_Click(object sender, EventArgs e)
        {
            if(Login(usernameTB.Text,PasswordTB.Text))
            {
                string chuoiSQL = "Select DisplayName, AccountType from tbl_Account where Username = N'" + usernameTB.Text + "'";
                DataTable a = provider.ExecuteQuerry(chuoiSQL);
                string at = a.Rows[0]["AccountType"].ToString();
                string dn = a.Rows[0]["DisplayName"].ToString();
                fBillManager f = new fBillManager(at,dn);
                this.Hide();
                f.ShowDialog();
                this.Show();  
            }
            else
            {
                MessageBox.Show("Incorrect username or password!");
            }
        }
        bool Login(string username, string password)
        {
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(password);
            byte[] data = new MD5CryptoServiceProvider().ComputeHash(temp);
            string passf = "";
            foreach (byte item in data)
            {
                passf += item;
            }
            string pass = passf.Substring(4, 12);
            string chuoiSQL = "Select * from tbl_Account where Username = N'" + username + "' and Pass = N'" + pass + "'";
            DataTable a = provider.ExecuteQuerry(chuoiSQL);
            return a.Rows.Count > 0;
        }
        private void fLogin_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (MessageBox.Show("Exit application?", "", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void labelExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked==true)
            {
                PasswordTB.UseSystemPasswordChar = false;
            }
            else
            {
                PasswordTB.UseSystemPasswordChar = true;
            }
        }
    }
}