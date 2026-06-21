namespace ReuseMaisApi.Models
{
    public class LoginLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? NomeUsuario { get; set; }
        public string? Role { get; set; }
        public DateTime DataLogin { get; set; } = DateTime.Now;
    }
}