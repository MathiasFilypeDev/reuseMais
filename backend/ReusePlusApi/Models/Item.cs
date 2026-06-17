namespace ReusePlusApi.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
        public string? Descricao { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
        public DateTime? DataAtualizacao { get; set; }
    }
}
