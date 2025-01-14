using System.ComponentModel.DataAnnotations;

namespace LaptopStore.Models.Home
{
    public class ProductDetail
    {
        [Key]
        public int ProductId { get; set; } // Primary key and foreign key

        public string? Category { get; set; } // CHAR(20), nullable
        public string? Cpu { get; set; } // CHAR(20), nullable
        public string? Gpu { get; set; } // CHAR(20), nullable
        public string? Ram { get; set; } // CHAR(20), nullable
        public string? Drive { get; set; } // CHAR(20), nullable
        public string? Size { get; set; } // CHAR(20), nullable
        public string? Resolution { get; set; } // CHAR(20), nullable

        // Navigation property for one-to-one relationship
        public Product product { get; set; }

        public ProductDetail() { }
    }
}
