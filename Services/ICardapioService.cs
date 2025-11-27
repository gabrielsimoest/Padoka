using Padoka.DTOs;

namespace Padoka.Services
{
    public interface ICardapioService
    {
        Task<List<CategoriaDTO>> ObterCardapioCompletoAsync();
        Task<List<CategoriaDTO>> ObterCategoriasAsync();
        Task<CategoriaDTO?> ObterCategoriaComItensAsync(long categoriaId);
        Task<List<ItemCardapioResumoDTO>> ObterItensPorCategoriaAsync(long categoriaId);
        Task<ItemCardapioDetalheDTO?> ObterItemDetalheAsync(long itemId);
        Task<List<ItemCardapioResumoDTO>> BuscarItensAsync(string termo);
    }
}
