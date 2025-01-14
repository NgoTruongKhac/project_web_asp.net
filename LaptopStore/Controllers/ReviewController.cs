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

            return RedirectToAction("Detail", "Home", new { id = review.ProductId });

        }





        private double Rate(List<Reviews> listReview)
        {
            if (listReview.Count == 0)
            {
                return 0.0; // Nếu không có review, trả về 0
            }

            int totalRate = 0;
            foreach (var review in listReview)
            {
                totalRate += review.Rate;
            }

            return (double)totalRate / listReview.Count; // Chia cho số lượng review
        }

        private Dictionary<int, int> CalculateStarStatistics(List<Reviews> listReview)
        {
            var starStatistics = new Dictionary<int, int>();

            // Khởi tạo từ 1 đến 5 sao với giá trị ban đầu là 0
            for (int i = 1; i <= 5; i++)
            {
                starStatistics[i] = 0;
            }

            // Duyệt qua danh sách review và tính số lượng đánh giá cho từng sao
            foreach (var review in listReview)
            {
                int rate = review.Rate;
                if (starStatistics.ContainsKey(rate))
                {
                    starStatistics[rate]++;
                }
            }

            return starStatistics;
        }


    }
}
