using Padoka.DTOs;

namespace Padoka.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> LoginAsync(LoginRequestDTO request);
        Task<LoginResponseDTO> RegistrarAsync(RegistroRequestDTO request);
        Task<UsuarioDTO?> ObterUsuarioPorIdAsync(long id);
        Task<bool> ValidarTokenAsync(string token);
        string GerarToken(long usuarioId, string email, string tipo);
    }
}
