using ShopManager.Database;
using ShopManager.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System;

namespace ShopManager.DAL
{
    public class OrderDAL
    {
        DBConnect db = new DBConnect();

        public void InsertOrderDetails(List<OrderDetails> details)
        {
            string query = @"
            INSERT INTO order_detail 
            (OrderId, ProductId, ProductTitle, Quantity, UnitPrice, Total, FirstName, LastName, Address, Phone, OrderDate, Status)
            VALUES 
            (@OrderId, @ProductId, @ProductTitle, @Quantity, @UnitPrice, @Total, @FirstName, @LastName, @Address, @Phone, @OrderDate, @Status)";

            db.OpenConnection();

            foreach (var item in details)
            {
                SqlCommand cmd = new SqlCommand(query, db.GetConnection());
                cmd.Parameters.AddWithValue("@OrderId", item.OrderId);
                cmd.Parameters.AddWithValue("@ProductId", item.ProductId);
                cmd.Parameters.AddWithValue("@ProductTitle", item.ProductTitle ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                cmd.Parameters.AddWithValue("@UnitPrice", item.UnitPrice);
                cmd.Parameters.AddWithValue("@Total", item.Total);
                cmd.Parameters.AddWithValue("@FirstName", item.FirstName);
                cmd.Parameters.AddWithValue("@LastName", item.LastName);
                cmd.Parameters.AddWithValue("@Address", item.Address);
                cmd.Parameters.AddWithValue("@Phone", item.Phone);
                cmd.Parameters.AddWithValue("@OrderDate", item.OrderDate);
                cmd.Parameters.AddWithValue("@Status", item.Status ?? "Pending");

                cmd.ExecuteNonQuery();
            }

            db.CloseConnection();
        }
    }

}
