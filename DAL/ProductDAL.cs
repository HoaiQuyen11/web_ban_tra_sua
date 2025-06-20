﻿using Microsoft.Data.SqlClient;
using ShopManager.Database;
using ShopManager.Models;

namespace ShopManager.DAL
{
    public class ProductDAL
    {
        DBConnect connect = new DBConnect();
        public List<Product> GetListProduct(int? idCategory)
        {
            connect.OpenConnection();
            List<Product> list = new List<Product>();
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;
                string query = @"select a.id as ProductId, a.title as ProductTitle,
                                        a.img as ProductImg, a.price as ProductPrice,
                                        a.rate as ProductRate, a.createAt as ProductCreateAt, a.updateAt as ProductUpdateAt,
                                        b.id as CategoryId, b.title as CategoryTitle from product a join category b on a.categoryId = b.Id ";
                //nếu IdCategory có giá trị thì thêm WHERE cho câu Query
                if (idCategory.HasValue)
                {
                    query = query + "where b.Id = " + idCategory;
                }
                command.CommandText = query;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    // ================== Cách 1 ===============
                    Product product = new Product()
                    {
                        Id = Convert.ToInt32(reader["ProductId"]),
                        Title = reader["ProductTitle"].ToString() ?? "",
                        Img = reader["ProductImg"].ToString() ?? "",
                        Price = Convert.ToInt32(reader["ProductPrice"]),
                        Rate = Convert.ToDouble(reader["ProductRate"].ToString()),
                        CreateAt = DateTime.Parse(reader["ProductCreateAt"]?.ToString()),
                        UpdateAt = DateTime.Parse(reader["ProductUpdateAt"]?.ToString()),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        CategoryTitle = reader["CategoryTitle"].ToString(),
                    };
                    list.Add(product);
                }
            }
            connect.CloseConnection();
            return list;
        }
        public List<Product> GetProducts_Pagination(int? idCategory, int pageIndex, int pageSize, string sortOrder)
        {
            connect.OpenConnection();

            List<Product> list = new List<Product>();

            //Điều kiện Category
            string categoryCondition = "";
            if (idCategory.HasValue)
            {
                categoryCondition = @" WHERE a.categoryId = " + idCategory;
            }

            //Truy vấn Sắp xếp
            string sortQuery = " ORDER BY a.id ";
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortOrder)
                {
                    case "price":
                        sortQuery = " ORDER BY a.price ";
                        break;
                    case "price_desc":
                        sortQuery = " ORDER BY a.price DESC ";
                        break;
                    case "rate_desc":
                        sortQuery = " ORDER BY a.rate DESC ";
                        break;

                    default:
                        sortQuery = " ORDER BY a.id ";
                        break;
                }
            }

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"SELECT * FROM (
                         SELECT ROW_NUMBER() OVER ( " + sortQuery + @" ) AS RowNumber,
                             a.id as ProductId, a.title as ProductTitle,  
                             a.img as ProductImg, a.price as ProductPrice,
                             a.rate as ProductRate, a.createAt as ProductCreateAt, a.updateAt as ProductUpdateAt, 
                             b.id as CategoryId, b.title as CategoryTitle
                             FROM product a join category b on a.categoryId = b.Id "
                             + categoryCondition + @"
                        ) TableResult
                        WHERE TableResult.RowNumber BETWEEN( @PageIndex -1) * @PageSize + 1 
                         AND @PageIndex * @PageSize 
                    ";

                command.CommandText = query;
                command.Parameters.AddWithValue("@PageIndex", pageIndex);
                command.Parameters.AddWithValue("@PageSize", pageSize);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Product product = new Product()
                    {
                        Id = Convert.ToInt32(reader["ProductId"]),
                        Title = reader["ProductTitle"].ToString() ?? "",
                        Img = reader["ProductImg"].ToString() ?? "",
                        Price = Convert.ToInt32(reader["ProductPrice"]),
                        Rate = Convert.ToDouble(reader["ProductRate"].ToString()),
                        CreateAt = DateTime.Parse(reader["ProductCreateAt"]?.ToString()),
                        UpdateAt = DateTime.Parse(reader["ProductUpdateAt"]?.ToString()),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        CategoryTitle = reader["CategoryTitle"].ToString(),
                    };

                    list.Add(product);
                }
            }
            connect.CloseConnection();
            return list;
        }


        public Product GetProductById(int Id)
        {
            connect.OpenConnection();
            Product product = new Product();
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;
                string query = @"select a.id as ProductId, a.title as ProductTitle,
                                        a.img as ProductImg, a.price as ProductPrice,
                                        a.rate as ProductRate, a.createAt as ProductCreateAt, a.updateAt as ProductUpdateAt,
                                        b.id as CategoryId, b.title as CategoryTitle
                                        from product a join category b on a.categoryId = b.Id where a.Id = @Id ";
                command.CommandText = query;
                command.Parameters.AddWithValue("@Id", Id);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    product = new Product()
                    {
                        Id = Convert.ToInt32(reader["ProductId"]),
                        Title = reader["ProductTitle"].ToString() ?? "",
                        Img = reader["ProductImg"].ToString() ?? "",
                        Price = Convert.ToInt32(reader["ProductPrice"]),
                        Rate = Convert.ToDouble(reader["ProductRate"].ToString()),
                        CreateAt = DateTime.Parse(reader["ProductCreateAt"]?.ToString()),
                        UpdateAt = DateTime.Parse(reader["ProductUpdateAt"]?.ToString()),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        CategoryTitle = reader["CategoryTitle"].ToString(),
                    };
                }
            }
            connect.CloseConnection();
            return product;
        }
        public List<Product> GetFeaturedProducts(int limitProduct)
        {
            // get from product table 
            // top limitProduct 
            // sort by product.rate desc 
            connect.OpenConnection();

            List<Product> list = new List<Product>();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"select top " + limitProduct + @" a.id as ProductId, a.title as ProductTitle,   
                    a.img as ProductImg, a.price as ProductPrice, 
                    a.rate as ProductRate, a.createAt as ProductCreateAt, a.updateAt as ProductUpdateAt,  
                    b.id as CategoryId, b.title as CategoryTitle 
                    from product a join category b on a.categoryId = b.Id 
                    order by a.rate desc;";

                command.CommandText = query;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    // ================== Cách 1 =============== 
                    Product product = new Product()
                    {
                        Id = Convert.ToInt32(reader["ProductId"]),
                        Title = reader["ProductTitle"].ToString() ?? "",
                        Img = reader["ProductImg"].ToString() ?? "",
                        Price = Convert.ToInt32(reader["ProductPrice"]),
                        Rate = Convert.ToDouble(reader["ProductRate"].ToString()),
                        CreateAt = DateTime.Parse(reader["ProductCreateAt"]?.ToString()),
                        UpdateAt = DateTime.Parse(reader["ProductUpdateAt"]?.ToString()),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        CategoryTitle = reader["CategoryTitle"].ToString(),
                    };

                    list.Add(product);
                }
            }
            connect.CloseConnection();
            return list;
        }
        public List<Product> GetRelatedProduct(int Id)
        {
            // get from product table 
            // Not display Id product 
            // sort by product.rate desc 
            connect.OpenConnection();

            List<Product> list = new List<Product>();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"select a.id as ProductId, a.title as ProductTitle,   
                    a.img as ProductImg, a.price as ProductPrice, 
                    a.rate as ProductRate, a.createAt as ProductCreateAt, a.updateAt as ProductUpdateAt,  
                    b.id as CategoryId, b.title as CategoryTitle 
                    from product a join category b on a.categoryId = b.Id 
                    where a.id != " + Id + @" order by a.rate desc;";

                command.CommandText = query;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    // ================== Cách 1 =============== 
                    Product product = new Product()
                    {
                        Id = Convert.ToInt32(reader["ProductId"]),
                        Title = reader["ProductTitle"].ToString() ?? "",
                        Img = reader["ProductImg"].ToString() ?? "",
                        Price = Convert.ToInt32(reader["ProductPrice"]),
                        Rate = Convert.ToDouble(reader["ProductRate"].ToString()),
                        CreateAt = DateTime.Parse(reader["ProductCreateAt"]?.ToString()),
                        UpdateAt = DateTime.Parse(reader["ProductUpdateAt"]?.ToString()),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        CategoryTitle = reader["CategoryTitle"].ToString(),
                    };

                    list.Add(product);
                }
            }
            connect.CloseConnection();
            return list;
        }
    }
}
