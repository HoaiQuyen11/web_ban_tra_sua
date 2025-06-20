using Microsoft.Data.SqlClient;
using ShopManager.Database;
using ShopManager.Models;

namespace ShopManager.DAL
{
    public class CategoryDAL
    {
        DBConnect connect = new DBConnect();

        public List<CategoryMenu> getAllWithCount()
        {
            connect.OpenConnection();

            List<CategoryMenu> list = new List<CategoryMenu>();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"select c.id, c.title, count(p.id) as soluong 
                    from category c left join product p  
                    on c.id = p.categoryId 
                    group by c.id,c.title
                    order by soluong desc;";

                command.CommandText = query;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    CategoryMenu category = new CategoryMenu()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Title = reader["Title"].ToString() ?? "",
                        Count = Convert.ToInt32(reader["soluong"].ToString())
                    };
                    list.Add(category);
                }
            }
            connect.CloseConnection();
            return list;
        }
    }
}
