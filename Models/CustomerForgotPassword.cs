using System.ComponentModel.DataAnnotations;

namespace ShopManager.Models
{
    public class CustomerForgotPassword
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "*")]
        [MaxLength(50, ErrorMessage = "Tối đa 50 kí tự")]
        public string Email { get; set; }
        public string OTP { get; set; }
        public string RandomCode { get; set; }
    }
}
