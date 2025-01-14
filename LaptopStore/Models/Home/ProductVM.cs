using System.ComponentModel.DataAnnotations;

namespace LaptopStore.Models.Home
{
	public class ProductVM
	{

		public int ProductId { get; set; } // Primary key
		public string Name { get; set; } // NVARCHAR(150)
		public string Description { get; set; } // TEXT
		public IFormFile? ImageFile { get; set; } // VARCHAR(255), nullable
		public int Price { get; set; } // INT
		public string? Brand { get; set; } // CHAR(50), nullable
		public DateTime CreatedAt { get; set; } // DATETIME

		// Navigation property for one-to-one relationship
		public ProductDetail productdetail { get; set; }



	}

}

