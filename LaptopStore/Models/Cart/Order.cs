using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LaptopStore.Migrations;

namespace LaptopStore.Models.Cart
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto Increment
        public int OrderId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(30)]
        public string Payment { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        public int TotalPrice { get; set; }

        [StringLength(30)]
        public string State { get; set; } = "chưa xác nhận";



    }
}
