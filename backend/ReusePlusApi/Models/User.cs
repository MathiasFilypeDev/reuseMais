using System;
using System.ComponentModel.DataAnnotations;

namespace ReusePlusApi.Models
{
    /// <summary>
    /// Representa um usuário do sistema
    /// </summary>
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 150 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(255, ErrorMessage = "Email não pode ter mais de 255 caracteres")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Senha deve ter no mínimo 6 caracteres")]
        public string Senha { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Tipo { get; set; } = UserType.Usuario;

        [Required]
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

        public DateTime? DataAtualizacao { get; set; }
    }
}
