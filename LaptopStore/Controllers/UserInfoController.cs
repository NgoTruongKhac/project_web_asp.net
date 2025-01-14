using LaptopStore.Data;
using LaptopStore.Models.Account;
using LaptopStore.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace LaptopStore.Controllers
{
    public class UserInfoController : Controller
    {

        private ConnectDatabase connectDatabase;


        public UserInfoController(ConnectDatabase connectDatabase)
        {
            this.connectDatabase = connectDatabase;

        }

        public async Task<IActionResult> ViewUserInfo()
        {
            string userJson = HttpContext.Session.GetString("User");
            User user = null;

            if (!string.IsNullOrEmpty(userJson))
            {
                user = JsonSerializer.Deserialize<User>(userJson);
            }
            List<Address> ListAddress = await connectDatabase.address
              .Where(a => a.UserId == user.UserId)
                .ToListAsync();

            ViewBag.User = user;
            ViewBag.ListAddress = ListAddress;

            return View();
        }
        [HttpPost]

        public async Task<IActionResult> UpdateUserInfo(UserInfoUpdate userInfo)
        {
            User user = await connectDatabase.Users.FirstOrDefaultAsync(u => u.UserId == userInfo.UserId);


            if (user != null)
            {
                user.FirstName = userInfo.Firtsname;
                user.LastName = userInfo.LastName;
                user.Email = userInfo.Email;
                user.PhoneNumber = userInfo.PhoneNumber;
                user.Sex = userInfo.Sex;
                user.Avatar = userInfo.Avatar;
                user.Birthday = userInfo.Birthday;
            }

            await connectDatabase.SaveChangesAsync();
            HttpContext.Session.SetString("User", JsonSerializer.Serialize(user));

            TempData["message"] = "lưu thông tin thành công";
            TempData["type"] = "success";

            return RedirectToAction("ViewUserInfo");

        }

        [HttpPost]

        public async Task<IActionResult> AddAddress(Address address)
        {
            if (address.IsDefault)
            {
                // Lấy danh sách tất cả các địa chỉ của user này
                var userAddresses = await connectDatabase.address.Where(a => a.UserId == address.UserId).ToListAsync();

                // Cập nhật tất cả các địa chỉ khác thành không mặc định
                foreach (var userAddress in userAddresses)
                {
                    userAddress.IsDefault = false;
                }
            }

            await connectDatabase.address.AddAsync(address);


            await connectDatabase.SaveChangesAsync();

            TempData["message"] = "thêm địa chỉ thành công";
            TempData["type"] = "success";

            return RedirectToAction("ViewUserInfo");

        }

        [HttpGet]

        public async Task<IActionResult> DeleteAddress(int addressId)
        {


            var address = await connectDatabase.address.FirstOrDefaultAsync(a => a.AddressId == addressId);



            if (address != null)
            {
                connectDatabase.address.Remove(address);
                await connectDatabase.SaveChangesAsync();
            }
            TempData["message"] = "xoá địa chỉ thành công";
            TempData["type"] = "success";
            return RedirectToAction("ViewUserInfo");
        }

        [HttpPost]

        public async Task<IActionResult> ChangeEmail()
        {
            String newEmail = Request.Form["newEmail"];

            User user = await connectDatabase.Users.FirstOrDefaultAsync(u => u.Email == newEmail);

            if (user != null)
            {

                TempData["message"] = "Email đã tồn tại";
                TempData["type"] = "error";

                return RedirectToAction("ViewUserInfo");
            }

            long verifyTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();// lay thoi gian hien tai
            Random random = new Random();
            int verificationCode = random.Next(100000, 1000000); // Tạo số ngẫu nhiên từ 100000 đến 999999
            string verificationCodeStr = verificationCode.ToString(); // Chuyển đổi sang chuỗi

            //gui email
            string message = "Mã xác nhận đăng ký";
            EmailUtil.SendEmail(newEmail, message, verificationCodeStr);
            HttpContext.Session.SetString("verifyTime", verifyTime.ToString());
            HttpContext.Session.SetString("verifyCode", verificationCodeStr);
            HttpContext.Session.SetString("newEmail", newEmail);

            return View();





        }

        [HttpPost]
        public async Task<IActionResult> CompleteChangeEmail()
        {

            string newEmail = HttpContext.Session.GetString("newEmail");
            long verifyTime = long.Parse(HttpContext.Session.GetString("verifyTime"));
            string verifyCode = HttpContext.Session.GetString("verifyCode");

            string ConfirmVerifyCode = Request.Form["ConfirmVerifyCode"];

            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            if (currentTime - verifyTime > 300000)
            {
                ViewBag.message = "mã xác nhận đã hết hạn";
                ViewBag.type = "error";
                return View("ChangeEmail");

            }
            if (!verifyCode.Equals(ConfirmVerifyCode))
            {
                ViewBag.message = "mã xác nhận không đúng";
                ViewBag.type = "error";
                return View("ChangeEmail");
            }

            User user = await connectDatabase.Users.FirstOrDefaultAsync(u => u.Email == newEmail);

            user.Email = newEmail;
            await connectDatabase.SaveChangesAsync();

            TempData["message"] = "Thay đổi Email thành công";
            TempData["type"] = "success";

            HttpContext.Session.Remove("newEmail");
            HttpContext.Session.Remove("verifyTime");
            HttpContext.Session.Remove("verifyCode");

            return RedirectToAction("ViewUserInfo");


        }


    }
}
