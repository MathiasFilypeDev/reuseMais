namespace ReuseMaisApi.Models
{
    public class Movimentacao
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public DateTime Data { get; set; } = DateTime.Now;
    }
}
