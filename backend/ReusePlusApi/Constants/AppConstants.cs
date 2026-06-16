namespace ReusePlusApi.Constants
{
    /// <summary>
    /// Tipos de usuários do sistema
    /// </summary>
    public static class UserType
    {
        public const string Admin = "admin";
        public const string Usuario = "usuario";
    }

    /// <summary>
    /// Tipos de movimentos de inventário
    /// </summary>
    public static class MovementType
    {
        public const string Entrada = "entrada";
        public const string Saida = "saida";
    }

    /// <summary>
    /// Constantes JWT e Autenticação
    /// </summary>
    public static class AuthConstants
    {
        public const int TokenExpirationHours = 2;
        public const string AdminRole = "admin";
        public const string UserRole = "usuario";
    }

    /// <summary>
    /// Mensagens de erro padrão
    /// </summary>
    public static class ErrorMessages
    {
        public const string InvalidCredentials = "Credenciais inválidas.";
        public const string EmailAlreadyRegistered = "Email já cadastrado.";
        public const string InvalidSecretKey = "Chave secreta inválida.";
        public const string Unauthorized = "Não autorizado.";
        public const string NotFound = "Recurso não encontrado.";
        public const string BadRequest = "Requisição inválida.";
        public const string InternalServerError = "Erro interno do servidor.";
    }

    /// <summary>
    /// Mensagens de sucesso padrão
    /// </summary>
    public static class SuccessMessages
    {
        public const string UserRegistered = "Usuário cadastrado com sucesso!";
        public const string AdminRegistered = "Admin cadastrado com sucesso!";
        public const string ItemCreated = "Item criado com sucesso!";
        public const string ItemUpdated = "Item atualizado com sucesso!";
        public const string ItemDeleted = "Item deletado com sucesso!";
    }
}
