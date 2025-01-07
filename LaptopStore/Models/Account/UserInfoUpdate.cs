namespace LaptopStore.Models.Account
{
    public class UserInfoUpdate
    {

        public int UserId { get; set; }
        public string Firtsname { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Avatar { get; set; }
        public string Sex { get; set; }
        public DateTime Birthday { get; set; }
    }
}
