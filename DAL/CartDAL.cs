using Microsoft.Data.SqlClient;
using ShopManager.Database;
using ShopManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShopManager.DAL
{
    public class CartDAL
    {
        private readonly DBConnect _connect = new DBConnect();

        public bool CheckOut(Customer customer, List<CartItem> cartItems)
        {
            if (customer == null || cartItems == null || cartItems.Count == 0)
                throw new ArgumentException("Dữ liệu đầu vào không hợp lệ.");

            using (var conn = _connect.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Tạo OrderId mới dựa vào MAX(OrderId) trong bảng order_detail, hoặc bắt đầu từ 1 nếu chưa có
                        int newOrderId = 1;
                        string sqlGetMaxOrderId = "SELECT ISNULL(MAX(OrderId), 0) FROM order_detail";

                        using (var cmd = new SqlCommand(sqlGetMaxOrderId, conn, transaction))
                        {
                            var result = cmd.ExecuteScalar();
                            if (result != null && int.TryParse(result.ToString(), out int maxOrderId))
                            {
                                newOrderId = maxOrderId + 1;
                            }
                        }

                        // 2. Insert từng dòng chi tiết đơn hàng vào order_detail
                        string sqlInsertOrderDetail = @"
                            INSERT INTO order_detail 
                            (OrderId, OrderDate, Status, FirstName, LastName, Address, Phone,
                             ProductId, ProductTitle, Quantity, UnitPrice, Total)
                            VALUES
                            (@OrderId, GETDATE(), @Status, @FirstName, @LastName, @Address, @Phone,
                             @ProductId, @ProductTitle, @Quantity, @UnitPrice, @Total);";

                        foreach (var item in cartItems)
                        {
                            using (var cmd = new SqlCommand(sqlInsertOrderDetail, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@OrderId", newOrderId);
                                cmd.Parameters.AddWithValue("@Status", "Pending");
                                cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
                                cmd.Parameters.AddWithValue("@LastName", customer.LastName);
                                cmd.Parameters.AddWithValue("@Address", customer.Address);
                                cmd.Parameters.AddWithValue("@Phone", customer.Phone);

                                cmd.Parameters.AddWithValue("@ProductId", item.IdProduct);
                                cmd.Parameters.AddWithValue("@ProductTitle", item.Name);
                                cmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                                cmd.Parameters.AddWithValue("@UnitPrice", (decimal)item.Price);
                                cmd.Parameters.AddWithValue("@Total", (decimal)item.Total);

                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Lỗi khi lưu đơn hàng: " + ex.Message);
                        return false;
                    }
                }
            }
        }
    }
}
