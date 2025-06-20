using Microsoft.AspNetCore.Mvc;
using ShopManager.Models;
using System.Collections.Generic;
using ShopManager.Areas.Admin.DAL;

namespace ShopManager.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderAdminController : Controller
    {
        private readonly OrderAdminDAL orderAdminDAL = new OrderAdminDAL();

        public IActionResult Index()
        {
            var orders = orderAdminDAL.GetAllOrders(); // Lấy danh sách đơn hàng từ order_detail
            return View(orders);
        }

        public IActionResult Details(int id)
        {
            var details = orderAdminDAL.GetOrderDetails(id); // Lấy chi tiết đơn hàng theo orderId

            if (details == null || details.Count == 0)
            {
                return NotFound("Không tìm thấy đơn hàng.");
            }

            var firstItem = details[0]; // Lấy thông tin khách hàng từ dòng đầu (vì trùng nhau)

            ViewBag.OrderInfo = new
            {
                OrderId = id,
                CustomerName = $"{firstItem.FirstName} {firstItem.LastName}",
                Address = firstItem.Address,
                Phone = firstItem.Phone,
                OrderDate = firstItem.OrderDate,
                TotalAmount = details.Sum(d => d.Total),
                Status = firstItem.Status
            };

            return View(details);
        }

        public IActionResult ChangeStatus(int id, string newStatus)
        {
            orderAdminDAL.UpdateOrderStatus(id, newStatus);
            return RedirectToAction("Index");
        }
    }
}
