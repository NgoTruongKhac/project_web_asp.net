using LaptopStore.Data;
using Microsoft.AspNetCore.Mvc;
using LaptopStore.Models.Reviews;
namespace LaptopStore.Controllers
{
    public class ReviewController : Controller
    {
        private ConnectDatabase connectDatabase;


        public ReviewController(ConnectDatabase connectDatabase)
        {
            this.connectDatabase = connectDatabase;

        }

        public async Task<IActionResult> AddReview(Reviews review)
        {

            await connectDatabase.reviews.AddAsync(review);
            await connectDatabase.SaveChangesAsync();

            ViewBag.message = "thêm nhận xét thành công";
            ViewBag.type = "success";
            return RedirectToAction("Detail", "Home", new { id = review.ProductId });

        }







    }
}
