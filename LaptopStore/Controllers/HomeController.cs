using LaptopStore.Data;
using LaptopStore.Models;
using LaptopStore.Models.Reviews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;

namespace LaptopStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ConnectDatabase connectDatabase;

        public HomeController(ILogger<HomeController> logger, ConnectDatabase connectDatabase)
        {
            _logger = logger;
            this.connectDatabase = connectDatabase;

        }

        public async Task<IActionResult> Index() // Thêm tham số page
        {
            int pageSize = 8; // Số sản phẩm mỗi lần hiển thị

            var productList = await connectDatabase.products
                .Include(p => p.productdetail) // Include để lấy thông tin chi tiết sản phẩm
                .Take(pageSize) // Lấy 8 sản phẩm của trang hiện tại
                .ToListAsync();
            var topSellingProducts = await connectDatabase.orderDetails
                .GroupBy(od => od.ProductId) // Nhóm theo ProductId
                .OrderByDescending(g => g.Sum(od => od.Quantity)) // Sắp xếp theo tổng số lượng bán (lượt mua)
                .Take(10) // Lấy 8 sản phẩm bán chạy nhất
                .Select(g => g.Key) // Lấy ProductId của các sản phẩm bán chạy
                .ToListAsync();
            var topSellingProductList = await connectDatabase.products
                .Where(p => topSellingProducts.Contains(p.ProductId)) // Lọc các sản phẩm bán chạy
                .Include(p => p.productdetail)
                .ToListAsync();

            ViewBag.ProductList = productList; // Gửi danh sách sản phẩm vào View
            ViewBag.TopSellingProductList = topSellingProductList;
           


            return View();
        }
        [HttpGet]
        public async Task<IActionResult> LoadMoreProducts(int page, int pageSize)
        {
            var productList = await connectDatabase.products
                .Include(p => p.productdetail)
                .Skip(page * pageSize) // Bỏ qua số lượng sản phẩm đã hiển thị
                .Take(pageSize) // Lấy số sản phẩm theo pageSize
                .ToListAsync();

            return View(productList); // Trả về kết quả dưới dạng JSON
        }
        public async Task<IActionResult> Detail(int id)
        {
            var product = await connectDatabase.products
                .Include(p => p.productdetail) // Bao gồm thông tin chi tiết sản phẩm
                .FirstOrDefaultAsync(p => p.ProductId == id); // Tìm sản phẩm với ID tương ứng
            ViewBag.ProductDetail = product;
            List<Reviews> listReviews = await connectDatabase.reviews.Include(r=>r.User).Where(r => r.ProductId == id).ToListAsync();
            double rate = Rate(listReviews);
            Dictionary<int, int> starStatistics=CalculateStarStatistics(listReviews);
            foreach (Reviews r in listReviews)
            {
                Console.WriteLine(r.UserId);
                Console.WriteLine(r.ProductId);
            }
            ViewBag.ListReviews = listReviews;
            ViewBag.StarStatistics = starStatistics;
            ViewBag.rate=rate;
            return View(); // Truyền sản phẩm vào view
        }

        public async Task<IActionResult> Search(string keyWord)
        {
            var searchResults = await connectDatabase.products.Where(p => p.Name.Contains(keyWord)).ToListAsync();


            return View(searchResults);


        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        private double Rate(List<Reviews> listReview)
        {
            if (listReview == null || listReview.Count == 0)
            {
                return 0.0; // Nếu danh sách rỗng hoặc null, trả về 0
            }

            int totalRate = 0;
            foreach (var review in listReview)
            {
                totalRate += review.Rate;
            }

            return (double)totalRate / listReview.Count;
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
