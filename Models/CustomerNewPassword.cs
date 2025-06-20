using System.ComponentModel.DataAnnotations;

namespace ShopManager.Models
{
    public class CustomerNewPassword
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "*")]
        [MaxLength(50, ErrorMessage = "Tối đa 50 kí tự")]
        public string Email { get; set; }
        [Display(Name = "Mật khẩu mới")]
        [Required(ErrorMessage = "*")]
        [MaxLength(200, ErrorMessage = "Tối đa 200 kí tự")]
        public string NewPassWord { get; set; }
        [Display(Name = "Xác nhận mật khẩu mới")]
        [Required(ErrorMessage = "*")]
        [MaxLength(200, ErrorMessage = "Tối đa 200 kí tự")]
        public string Confirm_NewPassWord { get; set; }
        public string RandomKey { get; set; }
    }
}
