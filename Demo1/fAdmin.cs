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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Demo.DAO;

namespace Demo
{
    public partial class fAdmin : Form
    {
        public static string chuoiKN = "server=.;database=QL_QUANCAFE;integrated security = SSPI";
        int I; //INCOME
        string sf = "Select * from tbl_Food";
        string sfc = "Select * from tbl_FCategory";
        string sc = "Select * from tbl_Staff";
        string sa = "Select Username, DisplayName, AccountType from tbl_Account";
        string sm = "Select * from tbl_Members";
        string ic = "exec sp_Income";
        DataProvider provider = new DataProvider();

        public fAdmin()
        {
            InitializeComponent();
        }
        private static string NumberToMoney(string input)
        {
            string getStrNumb = "";
            foreach (char kt in input) if (char.IsDigit(kt)) getStrNumb += kt;
            return ((getStrNumb.Length > 0) ? (string.Format("{0:0,000}", decimal.Parse(getStrNumb))).Replace(',', '.') : "0");
        }
        private void btnSearchBill_Click(object sender, EventArgs e)//INCOME
        {
            DateTime fd = dtpkFromDate.Value;
            DateTime td = dtpkToDate.Value;
            string chuoiSQL = "exec sp_SearchBill '" + fd.ToString() + "','" + td.ToString() + "'";
            dtgvIncome.DataSource = provider.ExecuteQuerry(chuoiSQL);
            DataTable dt = provider.ExecuteQuerry(chuoiSQL);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                I += Convert.ToInt32(dt.Rows[i]["Total"]);
            }
            
            txbIncome.Text = NumberToMoney(I.ToString());
            I = 0;
        }
        private void fAdmin_Load(object sender, EventArgs e)//FORM LOAD
        {
            dtgvFood.DataSource = provider.ExecuteQuerry(sf);
            dtgvCategory.DataSource = provider.ExecuteQuerry(sfc);
            dtgvStaff.DataSource = provider.ExecuteQuerry(sc);
            dtgvAccount.DataSource = provider.ExecuteQuerry(sa);
            dtgvIncome.DataSource = provider.ExecuteQuerry(ic);
            dtgvMembers.DataSource = provider.ExecuteQuerry(sm);
        }
        private void btnSearchFood_Click(object sender, EventArgs e)//SEARCH FOOD BY NAME
        {
            string chuoiSQL = "exec sp_SearchFood N'%"+txbSearchFoodName.Text+"%'";
            dtgvFood.DataSource = provider.ExecuteQuerry(chuoiSQL);
        }


        //------------------------------------ADD BUTTON----------------------------------------
        private void btnAddFood_Click(object sender, EventArgs e)
        {
           try
           {
                string chuoiSQL = "INSERT INTO tbl_Food Values (N'" + txbFoodID.Text + "',N'" + txbFoodName.Text + "',N'" + txbFoodCategory.Text + "'," + txbPrice.Text + ")";
                provider.ThucThi(chuoiSQL);
                dtgvFood.DataSource = provider.ExecuteQuerry(sf);
           }
           catch (Exception ex)
           {
                MessageBox.Show("Add new catergory first!!");
           }
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string chuoiSQL = "INSERT INTO tbl_FCategory Values (N'" + txbCategoryID.Text + "',N'" + txbCategoryName.Text + "')";
            provider.ThucThi(chuoiSQL);
            dtgvCategory.DataSource = provider.ExecuteQuerry(sfc);
        }

        private void btnAddStaff_Click(object sender, EventArgs e)
        {
            string chuoiSQL = "INSERT INTO tbl_Staff Values (N'" + txbStaffID.Text + "',N'" + txbStaffName.Text + "',N'" + txbPNumber.Text + "',N'" + txbPosition.Text+"')";
            provider.ThucThi(chuoiSQL);
            dtgvStaff.DataSource = provider.ExecuteQuerry(sc);
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string pass = "811165451677"; //="cafe"
            string chuoiSQL = "INSERT INTO tbl_Account Values (N'" + txbUsername.Text + "',N'" + txbDisplayName.Text + "',N'" + pass + "',N'" + txbAccountType.Text + "')";
            provider.ThucThi(chuoiSQL);
            dtgvAccount.DataSource = provider.ExecuteQuerry(sa);
        }

        //------------------------------------DELETE BUTTON-------------------------------------
        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            string chuoiSQL = "delete tbl_Food where FoodID =N'" + txbFoodID.Text + "'";
            provider.ThucThi(chuoiSQL);
            dtgvFood.DataSource = provider.ExecuteQuerry(sf);
            txbFoodID.Text = "";
        }

        private void bbtnDeleteCategory_Click(object sender, EventArgs e)
        {
            string chuoiSQL = "delete tbl_FCategory where CategoryID =N'" + txbCategoryID.Text + "'";
            provider.ThucThi(chuoiSQL);
            dtgvCategory.DataSource = provider.ExecuteQuerry(sfc);
            txbCategoryID.Text = "";
        }

        private void btnDeleteStaff_Click(object sender, EventArgs e)
        {
            string chuoiSQL = "delete tbl_Staff where StaffID =N'" + txbStaffID.Text + "'";
            provider.ThucThi(chuoiSQL);
            dtgvStaff.DataSource = provider.ExecuteQuerry(sfc);
            txbStaffID.Text = "";
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string chuoiSQL = "delete tbl_Account where Username ='" + txbUsername.Text + "'";
            provider.ThucThi(chuoiSQL);
            dtgvAccount.DataSource = provider.ExecuteQuerry(sa);
            txbUsername.Text = "";
        }

        //------------------------------------ADJUST BUTTON-------------------------------------
        // FOOD
        private void btnAdjustFood_Click(object sender, EventArgs e)
        {
            string chuoiSQL = "Update tbl_Food set FoodName = N'" + txbFoodName.Text + "',Price = N'" + txbPrice.Text + "',FCategoryID = N'" + txbFoodCategory.Text + "' where FoodID = '" + txbFoodID.Text + "'";
            provider.ThucThi(chuoiSQL);
            dtgvFood.DataSource = provider.ExecuteQuerry(sf);
            txbFoodID.Text = txbFoodName.Text = txbPrice.Text = txbFoodCategory.Text = "";
        }
        private void dtgvFood_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = dtgvFood.CurrentRow.Index;
            txbFoodID.Text = dtgvFood.Rows[i].Cells[0].Value.ToString();
            txbFoodName.Text = dtgvFood.Rows[i].Cells[1].Value.ToString();
            txbFoodCategory.Text = dtgvFood.Rows[i].Cells[2].Value.ToString();
            txbPrice.Text = dtgvFood.Rows[i].Cells[3].Value.ToString();

        }
        //CATEGORY
        private void btnAdjustCategory_Click(object sender, EventArgs e)
        {
            string chuoiSQL = "Update tbl_FCategory set FCategoryName = N'" + txbCategoryName.Text + "' where FCategoryID = '" + txbCategoryID.Text + "'";
            provider.ThucThi(chuoiSQL);
            dtgvCategory.DataSource = provider.ExecuteQuerry(sfc);
            txbCategoryID.Text = txbCategoryName.Text = "";
        }
        private void dtgvCategory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = dtgvCategory.CurrentRow.Index;
            txbCategoryID.Text = dtgvCategory.Rows[i].Cells[0].Value.ToString();
            txbCategoryName.Text = dtgvCategory.Rows[i].Cells[1].Value.ToString();
        }
        //STAFF
        private void btnAdjustStaff_Click(object sender, EventArgs e)
        {
            string chuoiSQL = "Update tbl_Staff set StaffName = N'" + txbStaffName.Text + "',PNumber = N'" + txbPNumber.Text + "',Position = N'" + txbPosition.Text + "' where StaffID = '" + txbStaffID.Text + "'";
            provider.ThucThi(chuoiSQL);
            dtgvStaff.DataSource = provider.ExecuteQuerry(sc);
            txbStaffID.Text = txbStaffName.Text = txbPNumber.Text = txbPosition.Text = "";
        }
        private void dtgvStaff_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = dtgvStaff.CurrentRow.Index;
            txbStaffID.Text = dtgvStaff.Rows[i].Cells[0].Value.ToString();
            txbStaffName.Text = dtgvStaff.Rows[i].Cells[1].Value.ToString();
            txbPNumber.Text = dtgvStaff.Rows[i].Cells[2].Value.ToString();
            txbPosition.Text = dtgvStaff.Rows[i].Cells[3].Value.ToString();
        }
        //ACCOUNT
        private void btnAdjustAccount_Click(object sender, EventArgs e)
        {
            string chuoiSQL = "Update tbl_Account set DisplayName = '" + txbDisplayName.Text + "' where Username = 'N" + txbUsername.Text + "'";
            provider.ThucThi(chuoiSQL);
            dtgvFood.DataSource = provider.ExecuteQuerry(sf);
            txbUsername.Text = txbDisplayName.Text = txbAccountType.Text = "";
        }
        private void dtgvAccount_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = dtgvAccount.CurrentRow.Index;
            txbUsername.Text = dtgvAccount.Rows[i].Cells[0].Value.ToString();
            txbDisplayName.Text = dtgvAccount.Rows[i].Cells[1].Value.ToString();
            txbAccountType.Text = dtgvAccount.Rows[i].Cells[2].Value.ToString();
        }

        //------------------------------------MEMBERS TAB-------------------------------------
        private void dtgvMembers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = dtgvMembers.CurrentRow.Index;
            string memname = dtgvMembers.Rows[i].Cells[1].Value.ToString();
            DataTable dt = provider.ExecuteQuerry("exec sp_MemberTotalSpend '"+memname+"'");
            txbMemberTotal.Text = dt.Rows[0]["Total"].ToString();
            dtgvMemberBill.DataSource = provider.ExecuteQuerry("exec sp_MemberBill '"+memname+"'");
        }
        private void btnSearchMember_Click(object sender, EventArgs e)
        {   
            if(txbMemberNameSearch.Text=="")
            {
                dtgvMembers.DataSource = provider.ExecuteQuerry("Select * from tbl_Members");
            }
            else
            {
                string chuoiSQL = "Select * from tbl_Members where MemberName like '%" + txbMemberNameSearch.Text + "%'";
                dtgvMembers.DataSource = provider.ExecuteQuerry(chuoiSQL);
            }
        }



        //KHONG DUNG DEN!!!!!!!!!!!!!
        private void tpCategory_Click(object sender, EventArgs e)
        {

        }
        private void dtgvCategory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void panel4_Paint(object sender, PaintEventArgs e)
        {
        }
        private void category_Click(object sender, EventArgs e)
        {
        }
        private void dtpkToDate_ValueChanged(object sender, EventArgs e)
        {
        }       
        private void tpFood_Click(object sender, EventArgs e)
        {
        }

    }
}
