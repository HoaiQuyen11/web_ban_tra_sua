using Microsoft.AspNetCore.Mvc;

namespace ShopManager.Helper
{
    public class HomeController : Controller
    {
        private readonly EmailService _emailService;

        public HomeController(EmailService emailService)
        {
            _emailService = emailService;
        }

        public IActionResult SendEmail()
        {
            _emailService.SendEmail("recipient@example.com", "Chủ đề email", "Nội dung email");
            return View();
        }

    }
}
