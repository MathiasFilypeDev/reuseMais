using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReusePlusApi.Constants;
using ReusePlusApi.DTOs;
using ReusePlusApi.Services;

namespace ReusePlusApi.Controllers
{
    /// <summary>
    /// Controller responsável por autenticação e autorização
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Realiza login de usuário ou admin
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _authService.LoginAsync(request);
                if (result == null)
                    return Unauthorized(new ErrorResponseDto
                    {
                        Message = ErrorMessages.InvalidCredentials,
                        ErrorCode = "INVALID_CREDENTIALS"
                    });

                return Ok(result);
            }
            catch
            {
                return Unauthorized(new ErrorResponseDto
                {
                    Message = ErrorMessages.InvalidCredentials,
                    ErrorCode = "LOGIN_FAILED"
                });
            }
        }

        /// <summary>
        /// Registra um novo usuário
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(typeof(SuccessResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _authService.RegisterUserAsync(request);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ErrorResponseDto
                {
                    Message = ex.Message,
                    ErrorCode = "REGISTRATION_FAILED"
                });
            }
        }

        /// <summary>
        /// Registra um novo admin (requer chave secreta)
        /// </summary>
        [HttpPost("register-admin")]
        [ProducesResponseType(typeof(SuccessResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdminRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _authService.RegisterAdminAsync(request);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ErrorResponseDto
                {
                    Message = ex.Message,
                    ErrorCode = "INVALID_SECRET_KEY"
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ErrorResponseDto
                {
                    Message = ex.Message,
                    ErrorCode = "REGISTRATION_FAILED"
                });
            }
        }
    }
}
