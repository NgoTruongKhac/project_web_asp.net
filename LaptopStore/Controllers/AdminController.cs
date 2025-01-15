using LaptopStore.Data;

using LaptopStore.Models;
using LaptopStore.Models.Account;
using LaptopStore.Models.Cart;
using LaptopStore.Models.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;


namespace LaptopStore.Controllers
{

	[Authorize(Roles = "admin")]
	public class AdminController : Controller
	{
		private readonly IWebHostEnvironment _webHostEnvironment;
		private ConnectDatabase connectDatabase;
		public AdminController(ConnectDatabase connectDatabase, IWebHostEnvironment webHostEnvironment)
		{
			this.connectDatabase = connectDatabase;
			this._webHostEnvironment = webHostEnvironment;
		}
		public IActionResult Dashboard()
		{
			

			return View();
		}
		public IActionResult User(int page = 1)
		{

			List<User> users = connectDatabase.Users.ToList();

			const int pageSize = 5;

			if (page < 1)
			{
				page = 1;
			}
			int recsCount = users.Count();
			var pager = new Page(recsCount, page, pageSize);

			int recSkip = (page - 1) * pageSize;

			var data = users.Skip(recSkip).Take(pager.PageSize).ToList();

			ViewBag.Page = pager;
			ViewBag.action = "User";


			return View(data);
		}
		public async Task<IActionResult> DeleteUser(int userId, int page)
		{
			User user = await connectDatabase.Users.FirstOrDefaultAsync(u => u.UserId == userId);
			if (user != null)
			{
				connectDatabase.Users.Remove(user);
				await connectDatabase.SaveChangesAsync();
				TempData["message"] = "xoá tài khoản thành công";
				TempData["type"] = "success";
			}
			return RedirectToAction("User", new { page = page });

		}
		[HttpGet]
		public async Task<IActionResult> EditUser(int userId)
		{
			
		
			User user = await connectDatabase.Users.FirstOrDefaultAsync(u => u.UserId == userId);
			return View(user);

		}

		[HttpPost]
		public async Task<IActionResult> EditUser(User userEdit, int page)
		{
			User user = await connectDatabase.Users.FirstOrDefaultAsync(u => u.UserId == userEdit.UserId);
			if (user != null)
			{
				user.LastName = userEdit.LastName;
				user.FirstName = userEdit.FirstName;
				user.PhoneNumber = userEdit.PhoneNumber;
				user.Email = userEdit.Email;
				user.Birthday = userEdit.Birthday;
				user.Avatar = userEdit.Avatar;
				user.Role = userEdit.Role;
				user.Sex = userEdit.Sex;

				await connectDatabase.SaveChangesAsync();


			}
			TempData["message"] = "thay đổi tài khoản thành công";
			TempData["type"] = "success";
			return RedirectToAction("User", new { page = page });

		}

		[HttpGet]
		public IActionResult AddUser()
		{
			
			return View();
		}

		[HttpPost]

		public async Task<IActionResult> AddUser(User user)
		{
			await connectDatabase.Users.AddAsync(user);

			TempData["message"] = "thêm tài khoản thành công";
			TempData["type"] = "success";

			return RedirectToAction("User");

		}
		public async Task<IActionResult> Product(int page = 1)
		{
			List<Product> products =await connectDatabase.products.Include(p => p.productdetail).ToListAsync();

			const int pageSize = 5;

			if (page < 1)
			{
				page = 1;
			}
			int recsCount = products.Count();
			var pager = new Page(recsCount, page, pageSize);

			int recSkip = (page - 1) * pageSize;

			var data = products.Skip(recSkip).Take(pager.PageSize).ToList();

			ViewBag.Page = pager;
			ViewBag.action = "Product";

			return View(data);
		}
		public async Task<IActionResult> DeleteProduct(int productId, int page)
		{

			Product product = await connectDatabase.products.FirstOrDefaultAsync(u => u.ProductId == productId);
			if (product != null)
			{
				connectDatabase.products.Remove(product);
				await connectDatabase.SaveChangesAsync();
				TempData["message"] = "xoá sản phẩm thành công";
				TempData["type"] = "success";
			}
			return RedirectToAction("Product", new { page = page });

		}
		[HttpGet]
		public async Task<IActionResult> EditProduct(int productId,int page)
		{
			ViewBag.Page = page;

			Product product = await connectDatabase.products.Include(p=>p.productdetail).FirstOrDefaultAsync(u => u.ProductId == productId);

			ViewBag.product= product;
			return View();
		}
		[HttpGet]
		public IActionResult AddProduct()
		{
			return View();
		}

		[HttpPost]

		public async Task<IActionResult> AddProduct(ProductVM productVM)
		{
			//tạo tên file khi upload
			string uniqueFileName = null;
			if (productVM.ImageFile != null)
			{
				string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploadFile");
				uniqueFileName = Guid.NewGuid().ToString() + "_" + productVM.ImageFile.FileName;
				string filePath = Path.Combine(uploadsFolder, uniqueFileName);

				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					await productVM.ImageFile.CopyToAsync(fileStream);
				}
			}

			var product = new Product
			{
				Name = productVM.Name,
				Description = productVM.Description,
				Image = "/uploadFile/" + uniqueFileName, // Lưu đường dẫn hình ảnh
				Price = productVM.Price,
				Brand = productVM.Brand,
				productdetail = new ProductDetail
				{
					Category = productVM.productdetail.Category,
					Cpu = productVM.productdetail.Cpu,
					Gpu = productVM.productdetail.Gpu,
					Ram = productVM.productdetail.Ram,
					Drive = productVM.productdetail.Drive,
					Size = productVM.productdetail.Size,
					Resolution = productVM.productdetail.Resolution,

				}

			};

			await connectDatabase.products.AddAsync(product);
			await connectDatabase.SaveChangesAsync();
			TempData["message"] = "thêm sản phẩm thành công";
			TempData["type"] = "success";

			return RedirectToAction("Product");

		}

		[HttpPost]
		public async Task<IActionResult> EditProduct(ProductVM productVM, int page)
		{

			Product product = await connectDatabase.products.Include(p => p.productdetail).FirstOrDefaultAsync(p => p.ProductId == productVM.ProductId);

			if (product != null)
			{
				product.Name = productVM.Name;
				product.Description = productVM.Description;
				product.Price = productVM.Price;
				product.Brand = productVM.Brand;
				product.productdetail = productVM.productdetail;
			await connectDatabase.SaveChangesAsync();
			}
			TempData["message"] = "thay đổi phẩm thành công";
			TempData["type"] = "success";
			return RedirectToAction("Product", new { page = page });

		}


	public async Task<IActionResult> Order()
		{
			List<Order> listOrders = await connectDatabase.orders.ToListAsync(); 

			return View(listOrders);
		}

		[HttpPost]
		public async Task<IActionResult> CheckEmailRegister()
		{
			string Email = Request.Form["Email"];
			Console.WriteLine(Email);

			User user = await connectDatabase.Users.FirstOrDefaultAsync(u => u.Email == Email);

			if (user != null)
			{
				// gửi cho ajax nội dụng của span
				return Json(new { success = false, message = "email đã được sử dụng" });

			}
			return View("Register");

		}

	}
}
