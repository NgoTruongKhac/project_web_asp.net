using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using LaptopStore.Models.Account;
using System.Text.Json;

public class AdminAuthorizeAttribute : ActionFilterAttribute
{
	public override void OnActionExecuting(ActionExecutingContext context)
	{
		// Kiểm tra session
		var userSession = context.HttpContext.Session.GetString("User");

		// Deserialize user từ session
		User user = JsonSerializer.Deserialize<User>(userSession);

		// Kiểm tra role
		if (user == null || user.Role != "admin")
		{
			// Nếu không phải admin, chuyển hướng đến trang lỗi hoặc trang chủ
			context.Result = new RedirectToActionResult("AccessDenied", "Home", null);
			return;
		}

		base.OnActionExecuting(context);
	}
}
