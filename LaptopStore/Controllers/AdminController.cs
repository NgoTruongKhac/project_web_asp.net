using Microsoft.AspNetCore.Mvc;

namespace LaptopStore.Controllers
{

	[AdminAuthorize]
	public class AdminController : Controller
	{

		public IActionResult Dashboard()
		{
			ViewBag.Dashboard = "active";

			return View();
		}
		public IActionResult User()
		{
			ViewBag.User = "active";
			return View();
		}
		public IActionResult Product()
		{
			ViewBag.Product = "active";
			return View();
		}
		public IActionResult Order()
		{
			ViewBag.Order = "active";
			return View();
		}

	}
}
