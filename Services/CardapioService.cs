using Microsoft.EntityFrameworkCore;
using Padoka.DTOs;
using Padoka.Infraestrutura;

namespace Padoka.Services
{
    public class CardapioService : ICardapioService
    {
        private readonly PadokaContext _context;

        public CardapioService(PadokaContext context)
        {
            _context = context;
        }

        public async Task<List<CategoriaDTO>> ObterCardapioCompletoAsync()
        {
            var categorias = await _context.Categorias
                .Where(c => c.Ativo)
                .OrderBy(c => c.Ordem)
                .ThenBy(c => c.Nome)
                .Include(c => c.Itens.Where(i => i.Ativo))
                .ToListAsync();

            return categorias.Select(c => new CategoriaDTO
            {
                Id = c.Id,
                Nome = c.Nome,
                Descricao = c.Descricao,
                Ordem = c.Ordem,
                Itens = c.Itens.Select(i => new ItemCardapioResumoDTO
                {
                    Id = i.Id,
                    Nome = i.Nome,
                    DescricaoResumida = i.DescricaoResumida,
                    Preco = i.Preco,
                    ImagemUrl = i.ImagemUrl,
                    CategoriaId = c.Id,
                    CategoriaNome = c.Nome
                }).ToList()
            }).ToList();
        }

        public async Task<List<CategoriaDTO>> ObterCategoriasAsync()
        {
            var categorias = await _context.Categorias
                .Where(c => c.Ativo)
                .OrderBy(c => c.Ordem)
                .ThenBy(c => c.Nome)
                .ToListAsync();

            return categorias.Select(c => new CategoriaDTO
            {
                Id = c.Id,
                Nome = c.Nome,
                Descricao = c.Descricao,
                Ordem = c.Ordem
            }).ToList();
        }

        public async Task<CategoriaDTO?> ObterCategoriaComItensAsync(long categoriaId)
        {
            var categoria = await _context.Categorias
                .Where(c => c.Id == categoriaId && c.Ativo)
                .Include(c => c.Itens.Where(i => i.Ativo))
                .FirstOrDefaultAsync();

            if (categoria == null) return null;

            return new CategoriaDTO
            {
                Id = categoria.Id,
                Nome = categoria.Nome,
                Descricao = categoria.Descricao,
                Ordem = categoria.Ordem,
                Itens = categoria.Itens.Select(i => new ItemCardapioResumoDTO
                {
                    Id = i.Id,
                    Nome = i.Nome,
                    DescricaoResumida = i.DescricaoResumida,
                    Preco = i.Preco,
                    ImagemUrl = i.ImagemUrl,
                    CategoriaId = categoria.Id,
                    CategoriaNome = categoria.Nome
                }).ToList()
            };
        }

        public async Task<List<ItemCardapioResumoDTO>> ObterItensPorCategoriaAsync(long categoriaId)
        {
            var itens = await _context.ItensCardapio
                .Where(i => i.CategoriaId == categoriaId && i.Ativo)
                .Include(i => i.Categoria)
                .OrderBy(i => i.Nome)
                .ToListAsync();

            return itens.Select(i => new ItemCardapioResumoDTO
            {
                Id = i.Id,
                Nome = i.Nome,
                DescricaoResumida = i.DescricaoResumida,
                Preco = i.Preco,
                ImagemUrl = i.ImagemUrl,
                CategoriaId = i.CategoriaId,
                CategoriaNome = i.Categoria.Nome
            }).ToList();
        }

        public async Task<ItemCardapioDetalheDTO?> ObterItemDetalheAsync(long itemId)
        {
            var item = await _context.ItensCardapio
                .Where(i => i.Id == itemId && i.Ativo)
                .Include(i => i.Categoria)
                .Include(i => i.OpcoesAdicionais.Where(o => o.Ativo))
                .FirstOrDefaultAsync();

            if (item == null) return null;

            return new ItemCardapioDetalheDTO
            {
                Id = item.Id,
                Nome = item.Nome,
                Descricao = item.DescricaoResumida,
                DescricaoCompleta = item.DescricaoCompleta,
                Ingredientes = item.Ingredientes,
                Preco = item.Preco,
                ImagemUrl = item.ImagemUrl,
                CategoriaId = item.CategoriaId,
                Categoria = item.Categoria.Nome,
                OpcoesAdicionais = item.OpcoesAdicionais.Select(o => new OpcaoAdicionalDTO
                {
                    Id = o.Id,
                    Nome = o.Nome,
                    Descricao = o.Descricao,
                    Preco = o.PrecoAdicional
                }).ToList()
            };
        }

        public async Task<List<ItemCardapioResumoDTO>> BuscarItensAsync(string termo)
        {
            var itens = await _context.ItensCardapio
                .Where(i => i.Ativo && 
                    (i.Nome.ToLower().Contains(termo.ToLower()) || 
                     i.DescricaoResumida.ToLower().Contains(termo.ToLower())))
                .Include(i => i.Categoria)
                .OrderBy(i => i.Nome)
                .Take(20)
                .ToListAsync();

            return itens.Select(i => new ItemCardapioResumoDTO
            {
                Id = i.Id,
                Nome = i.Nome,
                DescricaoResumida = i.DescricaoResumida,
                Preco = i.Preco,
                ImagemUrl = i.ImagemUrl,
                CategoriaId = i.CategoriaId,
                CategoriaNome = i.Categoria.Nome
            }).ToList();
        }
    }
}
