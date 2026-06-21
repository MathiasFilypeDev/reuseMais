namespace ReuseMaisApi.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public string? Categoria { get; set; }
        public int Quantidade { get; set; }
        public int CriadoPorUserId { get; set; }  // ← Quem criou
        public string? CriadoPorNome { get; set; }  // ← Nome de quem criou
        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}