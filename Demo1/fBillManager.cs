using Demo.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
    public partial class fBillManager : Form
    {
        private static string chuoiKN = "server=.;database=QL_QUANCAFE;integrated security = SSPI";
        SqlConnection cn = new SqlConnection(chuoiKN);
        DataProvider provider = new DataProvider();
        List<string> ls = new List<string>();
        int T; //TongTien
        int D; //Discount
        int Tb; //TongTien cua bill da checkout
        string txb;
        public fBillManager()
        {
            InitializeComponent();  
        }
        public fBillManager(string at,string dn)//Phan quyen
        {
            InitializeComponent();
            if(at=="0")
            {
                adminToolStripMenuItem.Visible = false;
            }
            txbDisplayName.Text = dn;
        }
        private void fBillManager_Load(object sender, EventArgs e)//FORM LOAD
        {
            LC();
            DateTime dt = DateTime.Now;
            string date = dt.ToString();
            dtgvBill.DataSource = provider.ExecuteQuerry("exec sp_TodayBill '"+date+ "'");
        }
        private static string NumberToMoney(string input)//200000 -> 200.000
        {
            string getStrNumb = "";
            foreach (char kt in input) if (char.IsDigit(kt)) getStrNumb += kt;
            return ((getStrNumb.Length > 0) ? (string.Format("{0:0,000}", decimal.Parse(getStrNumb))).Replace(',', '.') : "0");
        }
        void LC()//FUNCTION COMBOBOX CATEGORY
        {
            cn.Open();
            string chuoiSQL = "select * from tbl_FCategory";
            DataTable dt = provider.ExecuteQuerry(chuoiSQL);
            cbCategory.Items.Clear();
            cbCategory.Text ="Category";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cbCategory.Items.Add(dt.Rows[i]["FCategoryName"]);
            }
            cn.Close();
        }
        private void cbCategory_SelectionChangeCommitted(object sender, EventArgs e)//HIEN THI CBOBOX FOOD THEO CBOBOX CATEGORY
        {   
            string id = cbCategory.GetItemText(cbCategory.SelectedItem);
            cn.Open();
            string chuoiSQL1 = "select FoodName from tbl_Food f inner join tbl_FCategory fc on f.FCategoryID = fc.FCategoryID where FCategoryName =  '" + id +"'";
            DataTable dt1 = provider.ExecuteQuerry(chuoiSQL1);
            cbFood.Items.Clear();
            cbFood.Text = "";
            for (int i = 0; i<dt1.Rows.Count;i++)
            {
                cbFood.Items.Add(dt1.Rows[i]["FoodName"]);
            }
            cn.Close();
        }
        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)//LOGOUT
        {
            this.Close();
        }

        private void personalInfoToolStripMenuItem_Click(object sender, EventArgs e)//CHANGE PASSWORD
        {
            fAccountProfile f = new fAccountProfile(txbDisplayName.Text);
            f.ShowDialog();
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)//TAB ADMIN
        {
            fAdmin f = new fAdmin();
            f.ShowDialog();
        }

        private void btnAddFood_Click(object sender, EventArgs e) //THEM MON VAO BILL + DIEU CHINH SO LUONG MOI MON
        {
            try
            {
                string chuoiSQL1 = "select Price from tbl_Food where FoodName = '" + txb + "'";
                DataTable dt1 = provider.ExecuteQuerry(chuoiSQL1);
                string price = dt1.Rows[0]["Price"].ToString();
                ListViewItem item1 = new ListViewItem();
                if (lsvBill.SelectedItems.Count > 0)
                {
                    int i = Convert.ToInt32(nmFoodCount.Value) + Convert.ToInt32(lsvBill.SelectedItems[0].SubItems[1].Text);
                    int temp = Convert.ToInt32(lsvBill.SelectedItems[0].SubItems[2].Text) * Convert.ToInt32(nmFoodCount.Value);
                    int total = Convert.ToInt32(lsvBill.SelectedItems[0].SubItems[2].Text) * i;
                    if (i < 0) return;
                    if (i == 0)
                    {
                        ls.Remove(lsvBill.SelectedItems[0].Text);
                        lsvBill.SelectedItems[0].Remove();
                        T += temp;
                        txbTOTAL.Text = T.ToString();
                        return;
                    }
                    lsvBill.SelectedItems[0].SubItems[1].Text = i.ToString();
                    lsvBill.SelectedItems[0].SubItems[3].Text = total.ToString();
                    T += temp;
                }
                else if (ls.Contains(txb) == false)
                {
                    if (Convert.ToInt32(nmFoodCount.Value) <= 0) return;
                    ls.Add(txb);
                    int total = Convert.ToInt32(dt1.Rows[0]["Price"]) * Convert.ToInt32(nmFoodCount.Value);
                    item1.Text = txb;
                    item1.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = nmFoodCount.Value.ToString() });
                    item1.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = price });
                    item1.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = total.ToString() });
                    lsvBill.Items.Add(item1);
                    T += total;
                }
                txbTOTAL.Text = NumberToMoney(T.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please select category and beverage/food !");
            }
        }
        private void btnDiscount_Click(object sender, EventArgs e)//DISCOUNT
        {
            D = T - T * Convert.ToInt32(nmDiscount.Value) / 100;
            txbTOTAL.Text = NumberToMoney(D.ToString());
        }
        private void btnUndoDiscount_Click(object sender, EventArgs e)//UNDO
        {
            txbTOTAL.Text = NumberToMoney(T.ToString());
        }      
        
        private void cbFood_TextChanged(object sender, EventArgs e)//Co dung
        {
            txb = cbFood.Text;
        }

        private void btnCheckOut_Click(object sender, EventArgs e)//CHECKOUT (THEM BILL + BILL INFO) (TICH DIEM MEMBER)
        {   
            if(txbTOTAL.Text =="")
            {
                MessageBox.Show("Add Food/Beverage first");
            }
            else if(nmDiscount.Value != 0 && txbTOTAL.Text!=NumberToMoney(D.ToString()))
            {
                MessageBox.Show("Press Discount first");
            }
            else
            {
                DataTable dtbill = provider.ExecuteQuerry("Select * from tbl_Bill");
                int x = dtbill.Rows.Count + 1;
                string z;
                if (x < 10)
                {
                    z = "B00" + x;
                }
                else
                {
                    z = "B0" + x;
                }

                if (txbCustomer.Text == "")
                {
                    string chuoiSQL = "INSERT INTO tbl_Bill Values ('" + z + "',GetDate(),'" + txbDisplayName.Text + "',DEFAULT,'" + nmDiscount.Value.ToString() + "')";
                    provider.ThucThi(chuoiSQL);
                }
                else
                {
                    string chuoiSQL = "INSERT INTO tbl_Bill Values ('" + z + "',GetDate(),'" + txbDisplayName.Text + "','" + txbCustomer.Text + "','" + nmDiscount.Value.ToString() + "')";
                    provider.ThucThi(chuoiSQL);
                }

                x++;
                foreach (ListViewItem item in lsvBill.Items)
                {
                    DataTable dt2 = provider.ExecuteQuerry("Select FoodID from tbl_Food where FoodName ='" + item.Text + "'");
                    string chuoiSQL1 = "INSERT INTO tbl_BillInfo Values ('" + z + "','" + dt2.Rows[0]["FoodID"].ToString() + "','" + item.SubItems[1].Text + "')";
                    provider.ThucThi(chuoiSQL1);
                }

                //UPDATE RANK
                if(txbRank.Text!="")
                {
                    DataTable data = provider.ExecuteQuerry("exec sp_MemberTotalSpend '" + txbCustomer.Text + "'");
                    int t = Convert.ToInt32(data.Rows[0]["Total"].ToString());
                    if (t>=500000 && t<1500000)
                    {
                        string chuoiSQL2 = "Update tbl_Members set MemberRank = N'Silver' where MemberName = N'"+txbCustomer.Text+"'";
                        provider.ThucThi(chuoiSQL2);
                    }
                    else if(t>=1500000)
                    {
                        string chuoiSQL2 = "Update tbl_Members set MemberRank = N'Gold' where MemberName = N'" + txbCustomer.Text + "'";
                        provider.ThucThi(chuoiSQL2);
                    }
                }

                DateTime dt = DateTime.Now;
                string date = dt.ToString();
                dtgvBill.DataSource = provider.ExecuteQuerry("exec sp_TodayBill '" + date + "'");
                btnClear_Click(sender, new EventArgs());    
            }
        }

        //Kieu int co the them nhu sau:
        /*
         string chuoiSQL = "...(...,@sl,...)"
         cn.Open();
         SqlCommand cmd = new SqlCommand(chuoiSQL1, cn);
         cmd.Parameters.AddWithValue("@sl", Convert.ToInt32(item.SubItems[1].Text)); 
         cmd.ExecuteNonQuery();
         cn.Close();
        */

        private void dtgvBill_CellClick(object sender, DataGridViewCellEventArgs e)//XEM BILLINFO THEO BILL DUOI DANG LISTVIEW
        {
            lsvBill.Items.Clear();
            txbTOTAL.Text.Remove(0);
            btnAddFood.Enabled = false;
            int ind = dtgvBill.CurrentRow.Index;
            string bid = dtgvBill.Rows[ind].Cells[0].Value.ToString();
            string chuoiSQL = "exec sp_binfoToListV'" + bid + "'";
            DataTable binfo = provider.ExecuteQuerry(chuoiSQL);
            DataTable dt = provider.ExecuteQuerry("Select Discount from tbl_Bill where BillID ='"+bid+"'");
            int disc = Convert.ToInt32(dt.Rows[0]["Discount"]);
            for (int i = 0; i < binfo.Rows.Count; i++)
            {
                ListViewItem item1 = new ListViewItem();
                item1.Text = binfo.Rows[i]["FoodName"].ToString();
                item1.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = binfo.Rows[i]["Amount"].ToString() });
                item1.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = binfo.Rows[i]["Price"].ToString() });
                item1.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = binfo.Rows[i]["Total"].ToString() });
                lsvBill.Items.Add(item1);
                Tb += Convert.ToInt32(binfo.Rows[i]["Total"]);
            }
            Tb = Tb - Tb * disc / 100;
            nmDiscount.Value = Convert.ToInt32(dt.Rows[0]["Discount"]);
            txbTOTAL.Text = NumberToMoney(Tb.ToString());
            Tb = 0;
        }
        private void btnClear_Click(object sender, EventArgs e)//CLEAR
        {
            btnAddFood.Enabled = true;
            lsvBill.Items.Clear();
            txbTOTAL.Text = "";
            T = 0;
            ls.Clear();
            txbCustomer.Text = txbPNum.Text = txbRank.Text = "";
            nmDiscount.Value = 0;
        }
        private void btnCheck_Click(object sender, EventArgs e)//CHECK MEMBER OR NOT
        {
            try
            {
                if(txbPNum.Text=="")
                {
                    string chuoiSQL = "Select * from tbl_Members where MemberName = '" + txbCustomer.Text + "'";
                    DataTable dt = provider.ExecuteQuerry(chuoiSQL);
                    if(dt.Rows.Count > 1)
                    {
                        MessageBox.Show("Please input phone number to clarify member!!");
                    }
                    else
                    {   
                        txbPNum.Text = dt.Rows[0]["MemberPNumber"].ToString();
                        txbRank.Text = dt.Rows[0]["MemberRank"].ToString();
                        if (txbRank.Text == "Silver")
                        {
                            nmDiscount.Value = 5;
                        }
                        else if (txbRank.Text == "Gold")
                        {
                            nmDiscount.Value = 10;
                        }
                    }
                }
                else
                {
                    string chuoiSQL = "Select * from tbl_Members where MemberName = '" + txbCustomer.Text + "'and MemberPNumber ='" + txbPNum.Text + "'";
                    DataTable dt = provider.ExecuteQuerry(chuoiSQL);
                    txbRank.Text = dt.Rows[0]["MemberRank"].ToString();
                    if (txbRank.Text == "Silver")
                    {
                        nmDiscount.Value = 5;
                    }
                    else if (txbRank.Text == "Gold")
                    {
                        nmDiscount.Value = 10;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Not a member!");
            }
        }
        private void btnAddMem_Click(object sender, EventArgs e)//ADD NEW MEMBER
        {   
            if(CheckPNumber()==true)
            {
                MessageBox.Show("Input new phone's number!!");
            }
            else if (txbCustomer.Text == "" || txbPNum.Text == "")
            {
                MessageBox.Show("Please input customer's info");
            }
            else
            {
                DataTable dt = provider.ExecuteQuerry("Select * from tbl_Members");
                int x = dt.Rows.Count + 1;
                string z;
                if (x < 10)
                {
                    z = "M00" + x;
                }
                else
                {
                    z = "M0" + x;
                }
                string chuoiSQL = "INSERT INTO tbl_Members values('" + z + "',N'" + txbCustomer.Text + "','" + txbPNum.Text + "',DEFAULT)";
                provider.ThucThi(chuoiSQL);
                MessageBox.Show("New member added!!");
            }
        }
        bool CheckPNumber()
        {   
            DataTable dt = provider.ExecuteQuerry("Select MemberPNumber from tbl_Members");
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                if (txbPNum.Text == dt.Rows[i]["MemberPNumber"].ToString())
                {
                    return true;
                }
            }
            return false;
        }


        //KHONG DUNG DEN!!!!!!!!!!!!!!!!!!!!!!!!
        private void dtgvBill_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        private void nmFoodCount_ValueChanged(object sender, EventArgs e)
        {
        }
        private void panelBill_Paint(object sender, PaintEventArgs e)
        {
        }
        private void cbCategory_TextChanged(object sender, EventArgs e)
        {
        }
        private void cbCategory_SelectedValueChanged(object sender, EventArgs e)
        {
        }
        private void txbCustomer_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
 
    }
}
