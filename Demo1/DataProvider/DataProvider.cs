using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAO
{
    internal class DataProvider
    {
        private static string chuoiKN = "server=.;database=QL_QUANCAFE;integrated security = SSPI";
        
        public DataTable ExecuteQuerry(string chuoiSQL)
        {
            SqlConnection cn = new SqlConnection(chuoiKN);
            cn.Open();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand(chuoiSQL, cn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            cn.Close();
            return dt;
        }
        public void ThucThi(string caulenh)
        {
            SqlConnection cn = new SqlConnection(chuoiKN);
            SqlCommand cm = new SqlCommand(caulenh, cn);
            cn.Open();
            cm.ExecuteNonQuery();
            cn.Close();
        }
        

    }
}
