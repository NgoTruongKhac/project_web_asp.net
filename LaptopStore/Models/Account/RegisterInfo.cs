using System.ComponentModel.DataAnnotations;

namespace LaptopStore.Models.Account
{
	public class RegisterInfo
	{
		[Required(ErrorMessage = "Không được trống")]
		public string FirstName { get; set; }
		[Required(ErrorMessage = "Không được trống")]
		public string LastName { get; set; }
		[Required(ErrorMessage = "Không được trống")]
		[DataType(DataType.EmailAddress)]

		public string EmailOrPhone { get; set; }
		[Required(ErrorMessage = "Không được trống")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required(ErrorMessage = "Không được trống")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "mật khẩu không trùng khớp")]
		public string ConfirmPassword { get; set; }

	}
}
