using System;

namespace ReusePlusApi.Models
{
    public class Movimentacao
    {
        public int Id { get; set; }
        public string Produto { get; set; } = "";
        public int Quantidade { get; set; }
        public string Tipo { get; set; } = ""; // "entrada" ou "saida"
        public decimal Valor { get; set; }
        public DateTime Data { get; set; } = DateTime.Now;
    }
}
