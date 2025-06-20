﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopManager.DAL;
using ShopManager.Helper;
using ShopManager.Models;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace ShopManager.Controllers
{
    public class CartController : Controller
    {
        //Khai báo DAL 
        ProductDAL productDAL = new ProductDAL();
        CustomerDAL customerDAL = new CustomerDAL();
        CartDAL cartDAL = new CartDAL();

        public List<CartItem> Cart
        {
            get
            {
                var sessionCart = HttpContext.Session.Get<List<CartItem>>(MyConst.CART_KEY);
                return sessionCart ?? new List<CartItem>();
            }
        }


        public IActionResult Index()
        {
            return View(Cart);
        }
        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.IdProduct == id);
            if (item == null)
            {
                Product? productById = productDAL.GetProductById(id);

                if (productById == null)
                {
                    TempData["Message"] = "Khong tim thay san pham";
                    return Redirect("/404");
                }

                item = new CartItem
                {
                    IdProduct = productById.Id,
                    Img = productById.Img,
                    Name = productById.Title,
                    Price = productById.Price,
                    Rate = productById.Rate,
                    Quantity = quantity
                };
                gioHang.Add(item);
            }
            else
            {
                item.Quantity += quantity;
            }
            HttpContext.Session.Set(MyConst.CART_KEY, gioHang);

            return RedirectToAction("Index");
        }

        public IActionResult ChangeQuantityCart(int id, bool isIncrement = true, int quantity = 1)
        {
            // Lấy toàn bộ giỏ hàng 
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.IdProduct == id);

            //kiểm tra tồn tại Product 
            if (item == null)
            {

                TempData["Message"] = "Khong tim thay san pham";
                return Redirect("/404");
            }
            else
            {
                // Nếu là button tăng số lượng 
                if (isIncrement)
                {
                    item.Quantity += quantity;
                }
                // Nếu là button giảm số lượng 
                else
                {
                    item.Quantity -= quantity;
                    // Nếu khách hàng nhập số lượng <= 0 thì xóa sản phẩm đó ra khỏi giỏ hàng
                    if (item.Quantity <= 0)
                    {
                        gioHang.Remove(item);
                    }
                }
            }

            // Lưu thay đổi 
            HttpContext.Session.Set(MyConst.CART_KEY, gioHang);

            return RedirectToAction("Index");
        }
        public IActionResult RemoveCart(int id)
        {
            var gioHang = Cart;

            var item = gioHang.SingleOrDefault(p => p.IdProduct == id);
            if (item != null)
            {
                gioHang.Remove(item);
                HttpContext.Session.Set(MyConst.CART_KEY, gioHang);
            }
            return RedirectToAction("Index");
        }

        [Authorize] //Kiểm tra user đăng nhập ?  
        public IActionResult CheckOut()
        {
            try
            {
                //Không thanh toán khi không có sản phẩm trong giỏ hàng 
                if (Cart.Count() == 0)
                {
                    TempData["CheckOutErrorMessage"] = "Thanh toán thất bại";
                    return RedirectToAction("Index");
                }
                // Lấy Customer id nếu người dùng đã đăng nhập 
                string? idTam = null;
                if (HttpContext.User.FindFirstValue("CustomerId") != null)
                {
                    idTam = HttpContext.User.FindFirstValue("CustomerId");
                }

                if (idTam == null)
                {
                    return Redirect("/Customer/SignIn");
                }

                //Lấy thông tin User 
                int idCustomer = Convert.ToInt32(idTam!);

                Customer? customer = new Customer();
                customer = customerDAL.GetCustomerById(idCustomer);

                // Nếu không tìm thấy User thì trả về trang 404 - Not Found 
                if (customer == null)
                {
                    return Redirect("/404");
                }

                //Insert Vào Database 
                bool isSuccess = cartDAL.CheckOut(customer, Cart);
                if (isSuccess)
                {
                    //Insert Database thành công 
                    TempData["CheckOutSuccessMessage"] = "Thanh toán thành công";

                    var gioHang = Cart;
                    gioHang = new List<CartItem>();
                    HttpContext.Session.Set(MyConst.CART_KEY, gioHang);
                }
                else
                {
                    //Insert Database thất bại 
                    TempData["CheckOutErrorMessage"] = "Thanh toán thất bại";
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                TempData["CheckOutErrorMessage"] = "Lỗi hệ thống: " + ex.Message.ToString();
                return RedirectToAction("Index");
            }

        }
        //Lấy tổng tiền thanh toán
        public IActionResult GetTotalAmount()
        {
            // Giả sử bạn có phương thức GetTotal để tính tổng số tiền của giỏ hàng
            int totalAmount = Cart.Sum(item => item.Total);
            return Json(totalAmount);
        }
        //Refresh Cart View Component
        public IActionResult RefreshCartViewComponent()
        {
            // Trả về ViewComponent("Tên ViewComponent muốn gọi", tham số truyền vào nếu có)
            return ViewComponent("Cart");
        }
        //Lấy tổng tiền theo từng Product
        public IActionResult GetTotalProduct(int idProduct)
        {
            // Giả sử bạn có phương thức GetTotal để tính tổng số tiền của giỏ hàng
            var productFind = Cart.Find(item => item.IdProduct == idProduct);

            int totalAmount = 0;
            if (productFind != null)
            {
                totalAmount = productFind.Total;
            }
            return Json(totalAmount);
        }

    }
}