using Padoka.DTOs;

namespace Padoka.Services
{
    public interface IAdminCardapioService
    {
        Task<List<CategoriaAdminDTO>> ObterCategoriasAsync();
        Task<CategoriaAdminDTO?> ObterCategoriaAsync(long id);
        Task<CategoriaAdminDTO> CriarCategoriaAsync(CriarCategoriaDTO dto);
        Task<CategoriaAdminDTO?> AtualizarCategoriaAsync(long id, AtualizarCategoriaDTO dto);
        Task<bool> ExcluirCategoriaAsync(long id);
        Task<List<ItemCardapioAdminDTO>> ObterItensAsync(long? categoriaId = null);
        Task<ItemCardapioAdminDTO?> ObterItemAsync(long id);
        Task<ItemCardapioAdminDTO> CriarItemAsync(CriarItemCardapioDTO dto);
        Task<ItemCardapioAdminDTO?> AtualizarItemAsync(long id, AtualizarItemCardapioDTO dto);
        Task<bool> ExcluirItemAsync(long id);
        Task<bool> AlterarDisponibilidadeAsync(long id, bool disponivel);
        Task<List<OpcaoAdicionalAdminDTO>> ObterOpcoesDoItemAsync(long itemId);
        Task<OpcaoAdicionalAdminDTO> CriarOpcaoAsync(CriarOpcaoAdicionalDTO dto);
        Task<OpcaoAdicionalAdminDTO?> AtualizarOpcaoAsync(long id, AtualizarOpcaoAdicionalDTO dto);
        Task<bool> ExcluirOpcaoAsync(long id);
    }
}
