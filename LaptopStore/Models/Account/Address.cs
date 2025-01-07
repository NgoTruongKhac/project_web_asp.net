using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopStore.Models.Account
{
	public class Address
	{

		public int AddressId { get; set; }


		public int UserId { get; set; }

		public string NameAddress { get; set; }

		public string Province { get; set; }

		public string District { get; set; }
		public string Ward { get; set; }

		public string Street { get; set; }

		public bool IsDefault { get; set; }

		public User User { get; set; }

	}
}
