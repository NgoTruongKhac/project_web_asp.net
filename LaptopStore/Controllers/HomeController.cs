using LaptopStore.Data;
using LaptopStore.Models;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {


            return View();
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
    }
}
