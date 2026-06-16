using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReusePlusApi.Models
{
    /// <summary>
    /// Representa um movimento de entrada ou saída de item
    /// </summary>
    public class Movimentacao
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome do produto é obrigatório")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Produto deve ter entre 3 e 255 caracteres")]
        public string Produto { get; set; } = string.Empty;

        [Required(ErrorMessage = "Quantidade é obrigatória")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantidade deve ser maior que 0")]
        public int Quantidade { get; set; }

        [Required(ErrorMessage = "Tipo de movimento é obrigatório")]
        [StringLength(20)]
        public string Tipo { get; set; } = MovementType.Entrada;

        [Required(ErrorMessage = "Valor é obrigatório")]
        [Range(0, double.MaxValue, ErrorMessage = "Valor não pode ser negativo")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }

        [StringLength(500)]
        public string Observacoes { get; set; } = string.Empty;

        [Required]
        public DateTime Data { get; set; } = DateTime.UtcNow;

        public DateTime? DataAtualizacao { get; set; }
    }
}
