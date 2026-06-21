using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using BCryptNet = BCrypt.Net.BCrypt;

[ApiController]
[Route("api/users")]
#pragma warning disable CA1050 // Declare types in namespaces
public class UsersController(IConfiguration configuration, UserService userService) : ControllerBase
#pragma warning restore CA1050 // Declare types in namespaces
{
    private readonly IConfiguration _configuration = configuration;
    private readonly UserService _userService = userService;

    public string GetJwtSecret1(string jwtSecret1)
    {
        return jwtSecret1;
    }

    [HttpPost("login")]
    public ActionResult<LoginResponse> Login([FromBody] LoginRequest request, string jwtSecret1)
    {
        // Validação
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new { message = "Usuário e senha são obrigatórios." });
        }

        try
        {
            // Buscar usuário no banco de dados
            var user = _userService.GetUserByUsername(request.Username);

            if (user == null)
            {
                return Unauthorized(new { message = "Usuário ou senha inválidos." });
            }

            // Verificar senha (usar hashing em produção!)
            if (!BCryptNet.Verify(request.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Usuário ou senha inválidos." });
            }

            // Gerar JWT Token
            var token = GenerateJwtToken(user, jwtSecret1);

#pragma warning disable CS8629 // Nullable value type may be null.
            return Ok(new LoginResponse(user.Id.GetValueOrDefault())
            {
                Id = (int)user.Id,
                Nome = user.Nome,
                Email = user.Email,
                Token = token,
                Role = user.Role
            });
#pragma warning restore CS8629 // Nullable value type may be null.
        }
        catch (Exception ex)
        {
            // Log do erro
            Console.Error.WriteLine($"Erro no login: {ex.Message}");

            // Retornar mensagem genérica para o cliente
            return StatusCode(500, new { message = "Erro no servidor. Tente novamente mais tarde." });
        }
    }

    private string GenerateJwtToken(User user, string? jwtSecret1)
    {
        var jwtSecret = _configuration["Jwt:Secret"];
        var jwtIssuer = _configuration["Jwt:Issuer"];
        var jwtAudience = _configuration["Jwt:Audience"];

#pragma warning disable CS8604 // Possible null reference argument.
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret1));
#pragma warning restore CS8604 // Possible null reference argument.
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CA1305 // Specify IFormatProvider
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name,user.Nome),
            new(ClaimTypes.Email, user.Email),
            new("role", user.Role)
        };
#pragma warning restore CA1305 // Specify IFormatProvider
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8604 // Possible null reference argument.

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class UserService
{
    internal User GetUserByUsername(string username)
    {
        throw new NotImplementedException();
    }
}

// Classes de requisição/resposta
public class LoginRequest
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public LoginRequest()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
    }

    public string Username { get; set; }
    public string Password { get; set; }
}

public class LoginResponse
{
    private object value;

    public LoginResponse(object value)
    {
        this.value = value;
    }

    public int Id { get; set; }
    public string ? Nome { get; set; }
    public string ? Email { get; set; }
    public string ? Token { get; set; }
    public string ? Role { get; set; }
}

public class User
{
    public int ? Id { get; set; }
    public string ? Nome { get; set; }
    public string ? Email { get; set; }
    public string ? Username { get; set; }
    public string ? PasswordHash { get; set; }
    public string ? Role { get; set; }
}