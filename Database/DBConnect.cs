using Microsoft.Data.SqlClient;

namespace ShopManager.Database
{
    public class DBConnect
    {
        SqlConnection connect = new SqlConnection("Data Source=DESKTOP-854NJDV\\SQLSERVER;Initial Catalog=MEETEA;Integrated Security=True;Trust Server Certificate=True");
        public SqlConnection GetConnection()
        {
            return connect;
        }
        public void OpenConnection()
        {
            if(connect.State == System.Data.ConnectionState.Closed)
            {
                connect.Open();
            }    
        }
        public void CloseConnection()
        {
            if (connect.State == System.Data.ConnectionState.Open)
            {
                connect.Close();
            }
        }
    }
}
