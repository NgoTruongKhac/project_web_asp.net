using LaptopStore.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectString = builder.Configuration.GetConnectionString("ConnectString");

builder.Services.AddDbContext<ConnectDatabase>(options =>
{

	options.UseMySql(connectString, ServerVersion.AutoDetect(connectString));

});



builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(15);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});
builder.Services.AddAuthentication("CookieAuth")
	.AddCookie("CookieAuth", config =>
	{
		config.LoginPath = "/Home/Login"; 
		config.AccessDeniedPath = "/Home/AccessDenied";
	});


builder.Services.AddDistributedMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}"

	);



app.Run();
