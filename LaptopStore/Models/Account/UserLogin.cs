using System.ComponentModel.DataAnnotations;

namespace LaptopStore.Models.Account
{
	public class UserLogin
	{
		[Required(ErrorMessage = "không được trống")]
		[DataType(DataType.EmailAddress, ErrorMessage = "email không hợp lệ ")]
		public string Email { get; set; }

		[Required(ErrorMessage = "không được trống")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
