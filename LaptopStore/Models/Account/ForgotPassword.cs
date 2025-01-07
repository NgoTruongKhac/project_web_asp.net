using System.ComponentModel.DataAnnotations;

namespace LaptopStore.Models.Account
{
    public class ForgotPassword
    {
        [Required(ErrorMessage = "Không được trống")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Không được trống")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Không được trống")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "mật khẩu không trùng khớp")]
        public string ConfirmPassword { get; set; }
    }
}
