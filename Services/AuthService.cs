using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Padoka.DTOs;
using Padoka.Infraestrutura;
using Padoka.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Padoka.Services
{
    public class AuthService : IAuthService
    {
        private readonly PadokaContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(PadokaContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO request)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.Ativo);

            if (usuario == null)
            {
                return new LoginResponseDTO
                {
                    Sucesso = false,
                    Mensagem = "E-mail ou senha inválidos"
                };
            }

            if (!VerificarSenha(request.Senha, usuario.SenhaHash))
            {
                return new LoginResponseDTO
                {
                    Sucesso = false,
                    Mensagem = "E-mail ou senha inválidos"
                };
            }

            usuario.UltimoAcesso = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var token = GerarToken(usuario.Id, usuario.Email, usuario.Tipo.ToString());

            return new LoginResponseDTO
            {
                Sucesso = true,
                Mensagem = "Login realizado com sucesso",
                Token = token,
                Usuario = new UsuarioDTO
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Tipo = usuario.Tipo.ToString()
                }
            };
        }

        public async Task<LoginResponseDTO> RegistrarAsync(RegistroRequestDTO request)
        {
            var emailExiste = await _context.Usuarios.AnyAsync(u => u.Email == request.Email);
            if (emailExiste)
            {
                return new LoginResponseDTO
                {
                    Sucesso = false,
                    Mensagem = "Este e-mail já está cadastrado"
                };
            }

            var usuario = new Usuario
            {
                Nome = request.Nome,
                Email = request.Email,
                SenhaHash = HashSenha(request.Senha),
                Tipo = TipoUsuario.Cliente,
                Ativo = true,
                CriadoEm = DateTime.UtcNow
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var token = GerarToken(usuario.Id, usuario.Email, usuario.Tipo.ToString());

            return new LoginResponseDTO
            {
                Sucesso = true,
                Mensagem = "Cadastro realizado com sucesso",
                Token = token,
                Usuario = new UsuarioDTO
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Tipo = usuario.Tipo.ToString()
                }
            };
        }

        public async Task<UsuarioDTO?> ObterUsuarioPorIdAsync(long id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return null;

            return new UsuarioDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Tipo = usuario.Tipo.ToString()
            };
        }

        public Task<bool> ValidarTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"] ?? "PadokaSecretKey123456789012345678901234");

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"] ?? "Padoka",
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"] ?? "PadokaApp",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out _);

                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        public string GerarToken(long usuarioId, string email, string tipo)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"] ?? "PadokaSecretKey123456789012345678901234");

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, usuarioId.ToString()),
                new(ClaimTypes.Email, email),
                new(ClaimTypes.Role, tipo),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(24),
                Issuer = _configuration["Jwt:Issuer"] ?? "Padoka",
                Audience = _configuration["Jwt:Audience"] ?? "PadokaApp",
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private static string HashSenha(string senha)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
            return Convert.ToBase64String(bytes);
        }

        private static bool VerificarSenha(string senha, string hash)
        {
            var senhaHash = HashSenha(senha);
            return senhaHash == hash;
        }
    }
}
