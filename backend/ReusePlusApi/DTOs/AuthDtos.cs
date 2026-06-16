using System.ComponentModel.DataAnnotations;

namespace ReusePlusApi.DTOs
{
    /// <summary>
    /// DTO para requisição de login
    /// </summary>
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tipo de usuário é obrigatório")]
        public string Tipo { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para resposta de login
    /// </summary>
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para requisição de registro de usuário
    /// </summary>
    public class RegisterUserRequestDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 150 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Senha deve ter no mínimo 6 caracteres")]
        public string Senha { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para requisição de registro de admin
    /// </summary>
    public class RegisterAdminRequestDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 150 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Senha deve ter no mínimo 6 caracteres")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "Chave secreta é obrigatória")]
        public string SecretKey { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para resposta genérica de sucesso
    /// </summary>
    public class SuccessResponseDto
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
    }

    /// <summary>
    /// DTO para resposta genérica de erro
    /// </summary>
    public class ErrorResponseDto
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public string? ErrorCode { get; set; }
    }
}
