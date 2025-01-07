using LaptopStore.Models.Account;
using Microsoft.EntityFrameworkCore;

namespace LaptopStore.Data
{
	public class ConnectDatabase : DbContext

	{

		public ConnectDatabase(DbContextOptions<ConnectDatabase> options) : base(options) { }

		public DbSet<User> Users { get; set; }
		public DbSet<Address> addresses { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			modelBuilder.Entity<Address>(entity =>
			{
				entity.ToTable("address"); // Ánh xạ với bảng Address trong DB

				entity.HasKey(a => a.AddressId); // Khóa chính

				entity.Property(a => a.NameAddress).HasMaxLength(255);
				entity.Property(a => a.Province).HasMaxLength(255);
				entity.Property(a => a.District).HasMaxLength(255);
				entity.Property(a => a.Ward).HasMaxLength(255);
				entity.Property(a => a.Street).HasMaxLength(255);

				entity.HasOne(a => a.User)
			.WithMany(u => u.addresses)
			.HasForeignKey(a => a.UserId)
			.OnDelete(DeleteBehavior.Cascade);



			});



			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<User>().ToTable("Users");
		}
	}
}
