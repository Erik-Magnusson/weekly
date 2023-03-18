
namespace Web.Models.Models
{
    public class Session
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public Guid UserId { get; set; }
        public int ExpiresIn { get; set; }
        public DateTime ExpiryTimeStamp { get; set; }
    }
}
