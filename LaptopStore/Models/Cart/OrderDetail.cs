using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LaptopStore.Models.Home;

namespace LaptopStore.Models.Cart
{
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto Increment
        public int OrderDetailId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        // Khóa ngoại với bảng Orders
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        // Khóa ngoại với bảng Product (bảng này chưa được định nghĩa trong ví dụ)
        [ForeignKey("ProductId")]
        public virtual Product product { get; set; } // Lớp Product chưa được định nghĩa trong ví dụ này

    }
}
