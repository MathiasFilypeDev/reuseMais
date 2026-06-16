using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReusePlusApi.Models
{
    /// <summary>
    /// Representa um item de inventário
    /// </summary>
    public class Item
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome do item é obrigatório")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 255 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Quantidade é obrigatória")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantidade não pode ser negativa")]
        public int Quantidade { get; set; }

        [Required(ErrorMessage = "Valor é obrigatório")]
        [Range(0, double.MaxValue, ErrorMessage = "Valor não pode ser negativo")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }

        [StringLength(500)]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

        public DateTime? DataAtualizacao { get; set; }

        [Timestamp]
        public byte[]? RowVersion { get; set; }
    }
}
