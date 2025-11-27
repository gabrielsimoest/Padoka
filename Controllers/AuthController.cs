using Microsoft.AspNetCore.Mvc;
using Padoka.DTOs;
using Padoka.Services;

namespace Padoka.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            var token = Request.Cookies["auth_token"];
            if (!string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Cardapio");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [Route("api/auth/login")]
        public async Task<IActionResult> LoginApi([FromBody] LoginRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new LoginResponseDTO
                {
                    Sucesso = false,
                    Mensagem = string.Join(", ", errors)
                });
            }

            var resultado = await _authService.LoginAsync(request);

            if (resultado.Sucesso && resultado.Token != null)
            {
                Response.Cookies.Append("auth_token", resultado.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddHours(24)
                });
            }

            return resultado.Sucesso ? Ok(resultado) : Unauthorized(resultado);
        }

        [HttpPost]
        [Route("api/auth/registro")]
        public async Task<IActionResult> RegistroApi([FromBody] RegistroRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new LoginResponseDTO
                {
                    Sucesso = false,
                    Mensagem = string.Join(", ", errors)
                });
            }

            var resultado = await _authService.RegistrarAsync(request);

            if (resultado.Sucesso && resultado.Token != null)
            {
                Response.Cookies.Append("auth_token", resultado.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddHours(24)
                });
            }

            return resultado.Sucesso ? Ok(resultado) : BadRequest(resultado);
        }

        [HttpPost]
        [Route("api/auth/logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("auth_token");
            return Ok(new { sucesso = true, mensagem = "Logout realizado com sucesso" });
        }

        [HttpGet]
        [Route("api/auth/verificar")]
        public async Task<IActionResult> Verificar()
        {
            var token = Request.Cookies["auth_token"];
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { autenticado = false });
            }

            var valido = await _authService.ValidarTokenAsync(token);
            return Ok(new { autenticado = valido });
        }
    }
}
