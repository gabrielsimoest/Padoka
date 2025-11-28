using Microsoft.EntityFrameworkCore;
using Padoka.DTOs;
using Padoka.Infraestrutura;
using Padoka.Models;

namespace Padoka.Services
{
    public class AdminCardapioService : IAdminCardapioService
    {
        private readonly PadokaContext _context;

        public AdminCardapioService(PadokaContext context)
        {
            _context = context;
        }

        public async Task<List<CategoriaAdminDTO>> ObterCategoriasAsync()
        {
            var categorias = await _context.Categorias
                .Include(c => c.Itens)
                .OrderBy(c => c.Ordem)
                .ThenBy(c => c.Nome)
                .ToListAsync();

            return categorias.Select(c => new CategoriaAdminDTO
            {
                Id = c.Id,
                Nome = c.Nome,
                Descricao = c.Descricao,
                Ordem = c.Ordem,
                Ativo = c.Ativo,
                TotalItens = c.Itens.Count,
                CriadoEm = c.CriadoEm
            }).ToList();
        }

        public async Task<CategoriaAdminDTO?> ObterCategoriaAsync(long id)
        {
            var categoria = await _context.Categorias
                .Include(c => c.Itens)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categoria == null) return null;

            return new CategoriaAdminDTO
            {
                Id = categoria.Id,
                Nome = categoria.Nome,
                Descricao = categoria.Descricao,
                Ordem = categoria.Ordem,
                Ativo = categoria.Ativo,
                TotalItens = categoria.Itens.Count,
                CriadoEm = categoria.CriadoEm
            };
        }

        public async Task<CategoriaAdminDTO> CriarCategoriaAsync(CriarCategoriaDTO dto)
        {
            var categoria = new Categoria
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                Ordem = dto.Ordem,
                Ativo = dto.Ativo,
                CriadoEm = DateTime.UtcNow
            };

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            return new CategoriaAdminDTO
            {
                Id = categoria.Id,
                Nome = categoria.Nome,
                Descricao = categoria.Descricao,
                Ordem = categoria.Ordem,
                Ativo = categoria.Ativo,
                TotalItens = 0,
                CriadoEm = categoria.CriadoEm
            };
        }

        public async Task<CategoriaAdminDTO?> AtualizarCategoriaAsync(long id, AtualizarCategoriaDTO dto)
        {
            var categoria = await _context.Categorias
                .Include(c => c.Itens)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categoria == null) return null;

            categoria.Nome = dto.Nome;
            categoria.Descricao = dto.Descricao;
            categoria.Ordem = dto.Ordem;
            categoria.Ativo = dto.Ativo;

            await _context.SaveChangesAsync();

            return new CategoriaAdminDTO
            {
                Id = categoria.Id,
                Nome = categoria.Nome,
                Descricao = categoria.Descricao,
                Ordem = categoria.Ordem,
                Ativo = categoria.Ativo,
                TotalItens = categoria.Itens.Count,
                CriadoEm = categoria.CriadoEm
            };
        }

        public async Task<bool> ExcluirCategoriaAsync(long id)
        {
            var categoria = await _context.Categorias
                .Include(c => c.Itens)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categoria == null) return false;

            if (categoria.Itens.Any())
            {
                throw new InvalidOperationException("Não é possível excluir uma categoria que possui itens.");
            }

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ItemCardapioAdminDTO>> ObterItensAsync(long? categoriaId = null)
        {
            var query = _context.ItensCardapio
                .Include(i => i.Categoria)
                .Include(i => i.OpcoesAdicionais)
                .AsQueryable();

            if (categoriaId.HasValue)
            {
                query = query.Where(i => i.CategoriaId == categoriaId.Value);
            }

            var itens = await query
                .OrderBy(i => i.Categoria.Ordem)
                .ThenBy(i => i.Nome)
                .ToListAsync();

            return itens.Select(i => new ItemCardapioAdminDTO
            {
                Id = i.Id,
                Nome = i.Nome,
                DescricaoResumida = i.DescricaoResumida,
                DescricaoCompleta = i.DescricaoCompleta,
                Ingredientes = i.Ingredientes,
                Preco = i.Preco,
                ImagemUrl = i.ImagemUrl,
                CategoriaId = i.CategoriaId,
                CategoriaNome = i.Categoria.Nome,
                Ativo = i.Ativo,
                TotalOpcoes = i.OpcoesAdicionais.Count,
                CriadoEm = i.CriadoEm
            }).ToList();
        }

        public async Task<ItemCardapioAdminDTO?> ObterItemAsync(long id)
        {
            var item = await _context.ItensCardapio
                .Include(i => i.Categoria)
                .Include(i => i.OpcoesAdicionais)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (item == null) return null;

            return new ItemCardapioAdminDTO
            {
                Id = item.Id,
                Nome = item.Nome,
                DescricaoResumida = item.DescricaoResumida,
                DescricaoCompleta = item.DescricaoCompleta,
                Ingredientes = item.Ingredientes,
                Preco = item.Preco,
                ImagemUrl = item.ImagemUrl,
                CategoriaId = item.CategoriaId,
                CategoriaNome = item.Categoria.Nome,
                Ativo = item.Ativo,
                TotalOpcoes = item.OpcoesAdicionais.Count,
                CriadoEm = item.CriadoEm
            };
        }

        public async Task<ItemCardapioAdminDTO> CriarItemAsync(CriarItemCardapioDTO dto)
        {
            var categoria = await _context.Categorias.FindAsync(dto.CategoriaId);
            if (categoria == null)
            {
                throw new InvalidOperationException("Categoria não encontrada.");
            }

            var item = new ItemCardapio
            {
                Nome = dto.Nome,
                DescricaoResumida = dto.DescricaoResumida,
                DescricaoCompleta = dto.DescricaoCompleta,
                Ingredientes = dto.Ingredientes,
                Preco = dto.Preco,
                ImagemUrl = dto.ImagemUrl,
                CategoriaId = dto.CategoriaId,
                Ativo = dto.Ativo,
                CriadoEm = DateTime.UtcNow
            };

            _context.ItensCardapio.Add(item);
            await _context.SaveChangesAsync();

            return new ItemCardapioAdminDTO
            {
                Id = item.Id,
                Nome = item.Nome,
                DescricaoResumida = item.DescricaoResumida,
                DescricaoCompleta = item.DescricaoCompleta,
                Ingredientes = item.Ingredientes,
                Preco = item.Preco,
                ImagemUrl = item.ImagemUrl,
                CategoriaId = item.CategoriaId,
                CategoriaNome = categoria.Nome,
                Ativo = item.Ativo,
                TotalOpcoes = 0,
                CriadoEm = item.CriadoEm
            };
        }

        public async Task<ItemCardapioAdminDTO?> AtualizarItemAsync(long id, AtualizarItemCardapioDTO dto)
        {
            var item = await _context.ItensCardapio
                .Include(i => i.Categoria)
                .Include(i => i.OpcoesAdicionais)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (item == null) return null;

            var categoria = await _context.Categorias.FindAsync(dto.CategoriaId);
            if (categoria == null)
            {
                throw new InvalidOperationException("Categoria não encontrada.");
            }

            item.Nome = dto.Nome;
            item.DescricaoResumida = dto.DescricaoResumida;
            item.DescricaoCompleta = dto.DescricaoCompleta;
            item.Ingredientes = dto.Ingredientes;
            item.Preco = dto.Preco;
            item.ImagemUrl = dto.ImagemUrl;
            item.CategoriaId = dto.CategoriaId;
            item.Ativo = dto.Ativo;

            await _context.SaveChangesAsync();

            return new ItemCardapioAdminDTO
            {
                Id = item.Id,
                Nome = item.Nome,
                DescricaoResumida = item.DescricaoResumida,
                DescricaoCompleta = item.DescricaoCompleta,
                Ingredientes = item.Ingredientes,
                Preco = item.Preco,
                ImagemUrl = item.ImagemUrl,
                CategoriaId = item.CategoriaId,
                CategoriaNome = categoria.Nome,
                Ativo = item.Ativo,
                TotalOpcoes = item.OpcoesAdicionais.Count,
                CriadoEm = item.CriadoEm
            };
        }

        public async Task<bool> ExcluirItemAsync(long id)
        {
            var item = await _context.ItensCardapio.FindAsync(id);
            if (item == null) return false;

            _context.ItensCardapio.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AlterarDisponibilidadeAsync(long id, bool disponivel)
        {
            var item = await _context.ItensCardapio.FindAsync(id);
            if (item == null) return false;

            item.Ativo = disponivel;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<OpcaoAdicionalAdminDTO>> ObterOpcoesDoItemAsync(long itemId)
        {
            var opcoes = await _context.OpcoesAdicionais
                .Where(o => o.ItemCardapioId == itemId)
                .OrderBy(o => o.Nome)
                .ToListAsync();

            return opcoes.Select(o => new OpcaoAdicionalAdminDTO
            {
                Id = o.Id,
                Nome = o.Nome,
                Descricao = o.Descricao,
                PrecoAdicional = o.PrecoAdicional,
                ItemCardapioId = o.ItemCardapioId,
                Ativo = o.Ativo
            }).ToList();
        }

        public async Task<OpcaoAdicionalAdminDTO> CriarOpcaoAsync(CriarOpcaoAdicionalDTO dto)
        {
            var item = await _context.ItensCardapio.FindAsync(dto.ItemCardapioId);
            if (item == null)
            {
                throw new InvalidOperationException("Item do cardápio não encontrado.");
            }

            var opcao = new OpcaoAdicional
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                PrecoAdicional = dto.PrecoAdicional,
                ItemCardapioId = dto.ItemCardapioId,
                Ativo = dto.Ativo
            };

            _context.OpcoesAdicionais.Add(opcao);
            await _context.SaveChangesAsync();

            return new OpcaoAdicionalAdminDTO
            {
                Id = opcao.Id,
                Nome = opcao.Nome,
                Descricao = opcao.Descricao,
                PrecoAdicional = opcao.PrecoAdicional,
                ItemCardapioId = opcao.ItemCardapioId,
                Ativo = opcao.Ativo
            };
        }

        public async Task<OpcaoAdicionalAdminDTO?> AtualizarOpcaoAsync(long id, AtualizarOpcaoAdicionalDTO dto)
        {
            var opcao = await _context.OpcoesAdicionais.FindAsync(id);
            if (opcao == null) return null;

            opcao.Nome = dto.Nome;
            opcao.Descricao = dto.Descricao;
            opcao.PrecoAdicional = dto.PrecoAdicional;
            opcao.Ativo = dto.Ativo;

            await _context.SaveChangesAsync();

            return new OpcaoAdicionalAdminDTO
            {
                Id = opcao.Id,
                Nome = opcao.Nome,
                Descricao = opcao.Descricao,
                PrecoAdicional = opcao.PrecoAdicional,
                ItemCardapioId = opcao.ItemCardapioId,
                Ativo = opcao.Ativo
            };
        }

        public async Task<bool> ExcluirOpcaoAsync(long id)
        {
            var opcao = await _context.OpcoesAdicionais.FindAsync(id);
            if (opcao == null) return false;

            _context.OpcoesAdicionais.Remove(opcao);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
