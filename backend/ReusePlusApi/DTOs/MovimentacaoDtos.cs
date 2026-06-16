using System.ComponentModel.DataAnnotations;

namespace ReusePlusApi.DTOs
{
    /// <summary>
    /// DTO para criar uma movimentação
    /// </summary>
    public class CreateMovimentacaoRequestDto
    {
        [Required(ErrorMessage = "Nome do produto é obrigatório")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Produto deve ter entre 3 e 255 caracteres")]
        public string Produto { get; set; } = string.Empty;

        [Required(ErrorMessage = "Quantidade é obrigatória")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantidade deve ser maior que 0")]
        public int Quantidade { get; set; }

        [Required(ErrorMessage = "Tipo de movimento é obrigatório")]
        [RegularExpression("^(entrada|saida)$", ErrorMessage = "Tipo deve ser 'entrada' ou 'saida'")]
        public string Tipo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Valor é obrigatório")]
        [Range(0, double.MaxValue, ErrorMessage = "Valor não pode ser negativo")]
        public decimal Valor { get; set; }

        [StringLength(500)]
        public string? Observacoes { get; set; }
    }

    /// <summary>
    /// DTO para resposta de movimentação
    /// </summary>
    public class MovimentacaoResponseDto
    {
        public int Id { get; set; }
        public string Produto { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public string? Observacoes { get; set; }
        public DateTime Data { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}
