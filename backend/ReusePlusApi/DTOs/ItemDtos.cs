using System.ComponentModel.DataAnnotations;

namespace ReusePlusApi.DTOs
{
    /// <summary>
    /// DTO para criar um novo item
    /// </summary>
    public class CreateItemRequestDto
    {
        [Required(ErrorMessage = "Nome do item é obrigatório")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 255 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Quantidade é obrigatória")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantidade não pode ser negativa")]
        public int Quantidade { get; set; }

        [Required(ErrorMessage = "Valor é obrigatório")]
        [Range(0, double.MaxValue, ErrorMessage = "Valor não pode ser negativo")]
        public decimal Valor { get; set; }

        [StringLength(500)]
        public string? Descricao { get; set; }
    }

    /// <summary>
    /// DTO para atualizar um item existente
    /// </summary>
    public class UpdateItemRequestDto
    {
        [Required(ErrorMessage = "ID é obrigatório")]
        public int Id { get; set; }

        [StringLength(255, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 255 caracteres")]
        public string? Nome { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantidade não pode ser negativa")]
        public int? Quantidade { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Valor não pode ser negativo")]
        public decimal? Valor { get; set; }

        [StringLength(500)]
        public string? Descricao { get; set; }
    }

    /// <summary>
    /// DTO para resposta de item
    /// </summary>
    public class ItemResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
        public string? Descricao { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}
