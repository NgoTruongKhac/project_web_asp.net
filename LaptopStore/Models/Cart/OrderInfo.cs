using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopStore.Models.Cart
{
	public class OrderInfo
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int OrderInfoId { get; set; }
		public int OrderId { get; set; }
		public int UserId { get; set; }
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }
		public string PhoneNumber { get; set; }

		public string Province { get; set; }

		public string District { get; set; }
		public string Ward { get; set; }

		public string Street { get; set; }

		public string note { get; set; }

		public DateTime Created { get; set; }

		public Order Order { get; set; }


	}
}
