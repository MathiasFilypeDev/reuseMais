namespace ReusePlusApi.Models
{
    public class Movimentacao
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty; // "entrada" ou "saida"
        public int ItemId { get; set; }
        public int Quantidade { get; set; }
    }
}
