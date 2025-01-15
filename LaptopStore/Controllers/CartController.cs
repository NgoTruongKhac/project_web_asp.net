using LaptopStore.Data;

using LaptopStore.Models.Account;
using LaptopStore.Models.Cart;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace LaptopStore.Controllers
{
	public class CartController : Controller
	{


		private ConnectDatabase connectDatabase;

		public CartController(ConnectDatabase connectDatabase)
		{
			this.connectDatabase = connectDatabase;
		}

		public async Task<IActionResult> ViewCart()
		{
			User user = GetUser();
			int userId = user.UserId;
			List<Cart> carts = await connectDatabase.carts.Include(c => c.Product).Where(c => c.User.UserId == userId).ToListAsync();
			ViewBag.listCarts = carts;
			ViewBag.btnHome = "tiếp tục mua";
			return View();


		}
		public async Task<IActionResult> updateQuantityCart(int cartId, int quantity)
		{

			User userSession = GetUser();

			Cart cart = await connectDatabase.carts.FirstOrDefaultAsync(c => c.CartId == cartId);
			if(cart != null)
			{
				cart.quantity = quantity;
				await connectDatabase.SaveChangesAsync();
			}
			int quantityCart = await connectDatabase.carts.Where(c => c.User.UserId == userSession.UserId).SumAsync(c => c.quantity);
			HttpContext.Session.SetInt32("quantityCart", quantityCart);

			return Json(new { quantityCart });

		}
		public async Task<IActionResult> deleteCartItem(int cartId)
		{
			User userSession = GetUser();

			Cart cart = await connectDatabase.carts.FirstOrDefaultAsync(c => c.CartId == cartId);
			if (cart != null)
			{
				connectDatabase.carts.Remove(cart);
				await connectDatabase.SaveChangesAsync(); // Lưu thay đổi vào database
			}
			int quantityCart = await connectDatabase.carts.Where(c => c.User.UserId == userSession.UserId).SumAsync(c => c.quantity);
			HttpContext.Session.SetInt32("quantityCart", quantityCart);

			return Json(new { quantityCart });

		}

		public async Task<IActionResult> OrderInfo(List<int> selectCartItem,int totalPrice)
		{

			User userSession = GetUser();
			int userId = userSession.UserId;

			Address address=await connectDatabase.address.FirstOrDefaultAsync(a=>a.UserId==userId & a.IsDefault==true);

			if (selectCartItem == null || !selectCartItem.Any())
			{
				// Không có cartId nào được chọn
				return RedirectToAction("Cart");
			}

			// Truy vấn danh sách Cart từ Entity Framework
			var selectedCarts = await connectDatabase.carts
										.Include(c=>c.Product).Where(c => selectCartItem.Contains(c.CartId))
										.ToListAsync();

            HttpContext.Session.SetString("listCart", JsonSerializer.Serialize(selectedCarts));
            ViewBag.selectedCarts = selectedCarts;
			ViewBag.address=address;
			HttpContext.Session.SetInt32("totalPrice", totalPrice);
			// Thực hiện các logic khác, ví dụ chuyển sang trang xác nhận đơn hàng
			return View();
		}


		public async Task<IActionResult> AddCart(int productId)
		{


			User userSession = GetUser();


			int userId = userSession.UserId;

			var product = await connectDatabase.products.FirstOrDefaultAsync(p => p.ProductId == productId);

			var user = await connectDatabase.Users.FirstOrDefaultAsync(p => p.UserId == userId);

			Cart cart = new Cart
			{
				Product = product,
				User = user,
				quantity = 1,
			};

			await connectDatabase.carts.AddAsync(cart);

			await connectDatabase.SaveChangesAsync();


			int quantityCart = await connectDatabase.carts.Where(c => c.User.UserId == userId).SumAsync(c => c.quantity);
			HttpContext.Session.SetInt32("quantityCart", quantityCart);


			return Json(new { quantityCart });

		}

		public async Task<IActionResult> Payment(OrderInfo orderInfo)
		{

			int? finalPrice = HttpContext.Session.GetInt32("totalPrice") + 15000;
			HttpContext.Session.SetString("orderInfo", JsonSerializer.Serialize(orderInfo));
			ViewBag.finalPrice = finalPrice;
			ViewBag.orderInfo = orderInfo;
			return View();

		}

		public async Task<IActionResult> OrderComplete(string payment, int finalPrice)
		{
			string orderStr = HttpContext.Session.GetString("orderInfo");
            string cartStr = HttpContext.Session.GetString("listCart");

            var listCartJson = HttpContext.Session.GetString("listCart");
            List<Cart> listCart = listCartJson != null
                ? JsonSerializer.Deserialize<List<Cart>>(listCartJson)
                : new List<Cart>();

            OrderInfo orderInfo = JsonSerializer.Deserialize<OrderInfo>(orderStr);

			Order order = new Order
			{
				Payment = payment,
				TotalPrice = finalPrice,
				State = "chưa xác nhận"
			};

			await connectDatabase.orders.AddAsync(order);


			await connectDatabase.SaveChangesAsync();

          
            // Thêm OrderInfo vào cơ sở dữ liệu
            // Lấy OrderId vừa tạo
            int orderId = order.OrderId;
            orderInfo.OrderId = orderId;

            await connectDatabase.orderInfo.AddAsync(orderInfo);
            // Tạo danh sách OrderDetail từ listCart
            var orderDetails = listCart.Select(cart => new OrderDetail
            {
                OrderId = orderId,
                ProductId = cart.Product.ProductId,
                Quantity = cart.quantity
            }).ToList();

            // Thêm danh sách OrderDetail vào cơ sở dữ liệu
            await connectDatabase.orderDetails.AddRangeAsync(orderDetails);

            // Lưu thay đổi
            await connectDatabase.SaveChangesAsync();


            return View();

		}

		public User GetUser()
		{
            string userJson = HttpContext.Session.GetString("User");
            User userSession = null;

			if (!string.IsNullOrEmpty(userJson))
			{
				userSession = JsonSerializer.Deserialize<User>(userJson);
			}
			return userSession;
		}
	}
}
