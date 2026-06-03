using System;

namespace ReusePlusApi.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.Now;
    }
}
