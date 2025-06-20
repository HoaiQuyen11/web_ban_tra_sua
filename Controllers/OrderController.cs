using Microsoft.AspNetCore.Mvc;
using ShopManager.DAL;
using ShopManager.Models;
using System;
using System.Collections.Generic;

namespace ShopManager.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderDAL _orderDAL = new OrderDAL();

        [HttpPost]
        public IActionResult PlaceOrder(List<OrderDetails> cartItems, string firstName, string lastName, string address, string phone)
        {
            if (cartItems == null || cartItems.Count == 0)
                return BadRequest("Giỏ hàng trống.");

            DateTime orderDate = DateTime.Now;
            int orderId = new Random().Next(100000, 999999); // tạo ID tạm thời, nên dùng giải pháp tốt hơn như GUID

            foreach (var item in cartItems)
            {
                item.OrderId = orderId;
                item.Total = item.Quantity * item.UnitPrice;
                item.FirstName = firstName;
                item.LastName = lastName;
                item.Address = address;
                item.Phone = phone;
                item.OrderDate = orderDate;
            }

            _orderDAL.InsertOrderDetails(cartItems);

            return Content("Bạn đã đặt hàng thành công! Mã đơn hàng: " + orderId);
        }

    }
}
