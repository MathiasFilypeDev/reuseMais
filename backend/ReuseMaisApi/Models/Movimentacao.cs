namespace ReuseMaisApi.Models
{
    public class Movimentacao
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public string? NomeProduto { get; set; }
        public string? Categoria { get; set; }
        public int Quantidade { get; set; }
        public string? Tipo { get; set; }  // "entrada" ou "saida"
        public int? UsuarioId { get; set; }  // Quem fez a movimentação
        public DateTime Data { get; set; } = DateTime.Now;
    }
}