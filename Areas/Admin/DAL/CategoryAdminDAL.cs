using Microsoft.Data.SqlClient;
using ShopManager.Areas.Admin.Models;
using ShopManager.Database;
using System.Data.Common;

namespace ShopManager.Areas.Admin.DAL
{
    public class CategoryAdminDAL
    {
        DBConnect connect = new DBConnect();
        public List<CategoryAdmin> getAll()
        {
            connect.OpenConnection();
            List<CategoryAdmin> list = new List<CategoryAdmin>();
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;
                string query = @"SELECT * FROM category";
                command.CommandText = query;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CategoryAdmin category = new CategoryAdmin()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Title = reader["Title"].ToString() ?? ""
                    };
                    list.Add(category);
                }
                connect.CloseConnection();
                return list;
            }
        }
        public bool AddNew(CategoryAdmin categoryAdd)
        {
            connect.OpenConnection();
            int Id = 0;
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;
                string query = @"insert into category(title) values(@title";
                command.CommandText = query;
                command.Parameters.AddWithValue("@title", categoryAdd.Title);
                Console.WriteLine("command insert category: " + command.CommandText);
                //ExecuteNonQuery trả về số dòng dữ liệu bị tác động
                Id = command.ExecuteNonQuery();
            }
            connect.CloseConnection();
            return Id > 0;
        }
        public CategoryAdmin getCategoryById(int id)
        {
            connect.OpenConnection();
            CategoryAdmin category = new CategoryAdmin();
            using(SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;
                string query = @"SELECT * FROM category WHERE Id = " + id;
                command.CommandText = query;
                SqlDataReader reader = command.ExecuteReader();
                while(reader.Read())
                {
                    category.Id = id;
                    category.Title = reader["Title"].ToString() ?? "";
                }    
            }
            connect.CloseConnection();
            return category;
        }
        public bool updateCategoryById(int id,CategoryAdmin categoryAdmin)
        {
            connect.OpenConnection();
            int isSuccess = 0;
            using(SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;
                string query = @"UPDATE category SET Title=@title  WHERE Id=@id";
                command.CommandText = query;
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@title", categoryAdmin.Title);
                Console.WriteLine("command update category: " + command.CommandText);
                isSuccess = command.ExecuteNonQuery();
            }
            connect.CloseConnection();
            return isSuccess > 0;
        }
        public bool deleteCategoryById(int id)
        {
            connect.OpenConnection();

            int isSuccess = 0;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;
                string query = @"DELETE FROM category WHERE id = @id";
                command.CommandText = query;
                command.Parameters.AddWithValue("@id", id);
                Console.WriteLine("command delete Category: " +command.CommandText);
                isSuccess = command.ExecuteNonQuery();
            }
            connect.CloseConnection();
            return isSuccess > 0;
        }
    }
}
