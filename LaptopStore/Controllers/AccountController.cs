
using LaptopStore.Data;

using LaptopStore.Models.Account;
using LaptopStore.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;


namespace LaptopStore.Controllers
{
	public class AccountController : Controller


	{
		private ConnectDatabase connectDatabase;


		public AccountController(ConnectDatabase connectDatabase)
		{
			this.connectDatabase = connectDatabase;

		}


		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(UserLogin userLogin)
		{

			String Email = userLogin.Email;
			String Password = userLogin.Password;


			User user = await connectDatabase.Users.FirstOrDefaultAsync(u => u.Email == Email);

			if (user == null)
			{
				ViewBag.message = "email không đúng";
				ViewBag.type = "error";
			}

			if (ModelState.IsValid && user != null)
			{
				bool isPassword = BCrypt.Net.BCrypt.Verify(Password, user.Pass);

				if (isPassword)
				{

					// Thiết lập Claims
					var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.Email),
				new Claim(ClaimTypes.Role, user.Role)
			};

					var identity = new ClaimsIdentity(claims, "CookieAuth");
					var principal = new ClaimsPrincipal(identity);

					// Đăng nhập người dùng
					await HttpContext.SignInAsync("CookieAuth", principal);


					TempData["message"] = "đăng nhập thành công";
					TempData["type"] = "success";

					HttpContext.Session.SetString("User", JsonSerializer.Serialize(user));




					if (user.Role == "admin")
					{


						return RedirectToAction("Dashboard", "Admin");
					}
					else if (user.Role == "customer")
					{

                        int quantityCart = await connectDatabase.carts.Where(c => c.User.UserId == user.UserId).SumAsync(c => c.quantity);
                        HttpContext.Session.SetInt32("quantityCart", quantityCart);

                        return RedirectToAction("Index", "Home");
					}
				}
				else
				{

					ViewBag.message = "mật khẩu không đúng";
					ViewBag.type = "error";
				}

			}

			return View();
		}

		public IActionResult Logout()
		{

			HttpContext.Session.Remove("User");

			return RedirectToAction("Index", "Home");
		}


		public IActionResult Register()
		{


			return View();
		}
		[HttpPost]
		public async Task<IActionResult> VerifyRegister(RegisterInfo registerInfo)
		{
			string Email = registerInfo.EmailOrPhone;

			User user = await connectDatabase.Users.FirstOrDefaultAsync(u => u.Email == Email);

			if (ModelState.IsValid && user == null)
			{

				long verifyTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();// lay thoi gian hien tai
				Random random = new Random();
				int verificationCode = random.Next(100000, 1000000); // Tạo số ngẫu nhiên từ 100000 đến 999999
				string verificationCodeStr = verificationCode.ToString(); // Chuyển đổi sang chuỗi

				//gui email
				string message = "Mã xác nhận đăng ký";
				EmailUtil.SendEmail(registerInfo.EmailOrPhone, message, verificationCodeStr);

				HttpContext.Session.SetString("registerInfo", JsonSerializer.Serialize(registerInfo));

				HttpContext.Session.SetString("verifyTime", verifyTime.ToString());
				HttpContext.Session.SetString("verifyCode", verificationCodeStr);



				return View();
			}




			return View("Register");

		}
		[HttpPost]

		public async Task<IActionResult> CompleteRegister()
		{


			string registerInfoJson = HttpContext.Session.GetString("registerInfo");


			RegisterInfo registerInfo = JsonSerializer.Deserialize<RegisterInfo>(registerInfoJson);

			long verifyTime = long.Parse(HttpContext.Session.GetString("verifyTime"));
			string verifyCode = HttpContext.Session.GetString("verifyCode");

			String hashPassword = BCrypt.Net.BCrypt.HashPassword(registerInfo.Password);

			string ConfirmVerifyCode = Request.Form["ConfirmVerifyCode"];

			long currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

			if (currentTime - verifyTime > 300000)
			{
				ViewBag.message = "mã xác nhận đã hết hạn";
				ViewBag.type = "error";
				return View("VerifyRegister");

			}
			if (!verifyCode.Equals(ConfirmVerifyCode))
			{
				ViewBag.message = "mã xác nhận không đúng";
				ViewBag.type = "error";
				return View("VerifyRegister");
			}

			User user = new User(registerInfo.FirstName, registerInfo.LastName, hashPassword, registerInfo.EmailOrPhone);

			await connectDatabase.Users.AddAsync(user);

			await connectDatabase.SaveChangesAsync();

			TempData["message"] = "Đăng ký thành công, bạn có thể đăng nhập";
			TempData["type"] = "success";

			HttpContext.Session.Remove("registerInfo");
			HttpContext.Session.Remove("verifyTime");
			HttpContext.Session.Remove("verifyCode");


			return RedirectToAction("Login", "Account");



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

		[HttpPost]
		public async Task<IActionResult> CheckEmailLogin()
		{
			string Email = Request.Form["Email"];
			Console.WriteLine(Email);

			User user = await connectDatabase.Users.FirstOrDefaultAsync(u => u.Email == Email);

			if (user == null)
			{
				// gửi cho ajax nội dụng của span
				return Json(new { success = false, message = "email không đúng hoặc không tồn tại" });

			}
			return View("Login");

		}

		public IActionResult ForgotPassword()
		{
			return View();
		}

		[HttpPost]

		public async Task<IActionResult> VerifyForgotPassword(ForgotPassword forgetPassword)
		{
			string Email = forgetPassword.Email;

			User user = await connectDatabase.Users.FirstOrDefaultAsync(u => u.Email == Email);

			if (user != null && ModelState.IsValid)
			{
				long verifyTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();// lay thoi gian hien tai
				Random random = new Random();
				int verificationCode = random.Next(100000, 1000000); // Tạo số ngẫu nhiên từ 100000 đến 999999
				string verificationCodeStr = verificationCode.ToString(); // Chuyển đổi sang chuỗi

				//gui email
				string message = "Mã xác nhận đăng ký";
				EmailUtil.SendEmail(forgetPassword.Email, message, verificationCodeStr);

				HttpContext.Session.SetString("forgetPassword", JsonSerializer.Serialize(forgetPassword));

				HttpContext.Session.SetString("verifyTime", verifyTime.ToString());
				HttpContext.Session.SetString("verifyCode", verificationCodeStr);

				return View();
			}
			return View("ForgotPassword");
		}

		[HttpPost]

		public async Task<IActionResult> CompleteForgotPassword()
		{
			string registerInfoJson = HttpContext.Session.GetString("forgetPassword");

			ForgotPassword forgotPassword = JsonSerializer.Deserialize<ForgotPassword>(registerInfoJson);

			long verifyTime = long.Parse(HttpContext.Session.GetString("verifyTime"));

			string verifyCode = HttpContext.Session.GetString("verifyCode");

			String hashPassword = BCrypt.Net.BCrypt.HashPassword(forgotPassword.Password);

			string ConfirmVerifyCode = Request.Form["ConfirmVerifyCode"];

			long currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

			if (currentTime - verifyTime > 300000)
			{
				ViewBag.message = "mã xác nhận đã hết hạn";
				ViewBag.type = "error";
				return View("VerifyForgotPassword");

			}
			if (!verifyCode.Equals(ConfirmVerifyCode))
			{
				ViewBag.message = "mã xác nhận không đúng";
				ViewBag.type = "error";
				return View("VerifyForgotPassword");
			}

			//change email
			string Email = forgotPassword.Email;

			User user = await connectDatabase.Users.FirstOrDefaultAsync(u => u.Email == Email);

			user.Pass = hashPassword;

			await connectDatabase.SaveChangesAsync();

			TempData["message"] = "Đổi Email thành công, bạn có thể đăng nhập";
			TempData["type"] = "success";

			HttpContext.Session.Remove("forgetPassword");
			HttpContext.Session.Remove("verifyTime");
			HttpContext.Session.Remove("verifyCode");

			return RedirectToAction("Login", "Account");



		}








	}
}