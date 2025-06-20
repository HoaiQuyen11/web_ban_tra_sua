using ShopManager.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace ShopManager.Areas.Admin.DAL
{
    public class OrderAdminDAL
    {
        private readonly string connectionString = "Data Source=DESKTOP-854NJDV\\SQLSERVER;Initial Catalog=MEETEA;Integrated Security=True;Trust Server Certificate=True";

        // Lấy danh sách đơn hàng (1 đơn = nhiều dòng cùng orderId)
        public List<OrderDetails> GetAllOrders()
        {
            var orders = new List<OrderDetails>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT orderId,
                           MIN(firstName) AS FirstName,
                           MIN(lastName) AS LastName,
                           MIN(address) AS Address,
                           MIN(phone) AS Phone,
                           MIN(orderDate) AS OrderDate,
                           MIN(status) AS Status,
                           SUM(total) AS TotalAmount
                    FROM order_detail
                    GROUP BY orderId
                    ORDER BY OrderDate DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    orders.Add(new OrderDetails
                    {
                        Id = (int)reader["orderId"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Address = reader["Address"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        OrderDate = (DateTime)reader["OrderDate"],
                        Status = reader["Status"].ToString(),
                        Total = Convert.ToDouble(reader["TotalAmount"])
                    });
                }
            }

            return orders;
        }

        // Chi tiết từng dòng trong đơn hàng
        public List<OrderDetails> GetOrderDetails(int orderId)
        {
            var details = new List<OrderDetails>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT id, orderId, productId, productTitle, quantity, unitPrice, total,
                           firstName, lastName, address, phone, orderDate, status
                    FROM order_detail
                    WHERE orderId = @orderId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@orderId", orderId);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    details.Add(new OrderDetails
                    {
                        Id = (int)reader["id"],
                        OrderId = (int)reader["orderId"],
                        ProductId = (int)reader["productId"],
                        ProductTitle = reader["productTitle"].ToString(),
                        Quantity = (int)reader["quantity"],
                        UnitPrice = Convert.ToDouble(reader["unitPrice"]),
                        Total = Convert.ToDouble(reader["total"]),
                        FirstName = reader["firstName"].ToString(),
                        LastName = reader["lastName"].ToString(),
                        Address = reader["address"].ToString(),
                        Phone = reader["phone"].ToString(),
                        OrderDate = (DateTime)reader["orderDate"],
                        Status = reader["status"].ToString()
                    });
                }
            }

            return details;
        }

        // Cập nhật trạng thái toàn bộ các dòng của đơn
        public void UpdateOrderStatus(int orderId, string newStatus)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE order_detail SET status = @status WHERE orderId = @orderId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@status", newStatus);
                cmd.Parameters.AddWithValue("@orderId", orderId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
