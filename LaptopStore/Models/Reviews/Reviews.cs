using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LaptopStore.Models.Account;
using LaptopStore.Models.Home;

namespace LaptopStore.Models.Reviews
{
	public class Reviews
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ReviewId { get; set; }

		public int UserId { get; set; }

		public int ProductId { get; set; }

		public int Rate { get; set; }

		public string comment { get; set; }

		public DateTime Created {  get; set; }

		public User User { get; set; }

		public Product Product { get; set; }

	}
}
