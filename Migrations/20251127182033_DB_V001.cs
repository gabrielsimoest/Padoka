using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Padoka.Migrations
{
    /// <inheritdoc />
    public partial class DB_V001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true),
                    Ordem = table.Column<int>(type: "integer", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "VARCHAR(150)", maxLength: 150, nullable: false),
                    SenhaHash = table.Column<string>(type: "VARCHAR(256)", maxLength: 256, nullable: false),
                    Tipo = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UltimoAcesso = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItensCardapio",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: false),
                    DescricaoResumida = table.Column<string>(type: "VARCHAR(300)", maxLength: 300, nullable: false),
                    DescricaoCompleta = table.Column<string>(type: "VARCHAR(1000)", maxLength: 1000, nullable: true),
                    Ingredientes = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true),
                    Preco = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    ImagemUrl = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CategoriaId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensCardapio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensCardapio_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NumeroPedido = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    Mesa = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true),
                    Status = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    ValorTotal = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Observacoes = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pedidos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OpcoesAdicionais",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "VARCHAR(300)", maxLength: 300, nullable: true),
                    PrecoAdicional = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    ItemCardapioId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpcoesAdicionais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpcoesAdicionais_ItensCardapio_ItemCardapioId",
                        column: x => x.ItemCardapioId,
                        principalTable: "ItensCardapio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistoricoStatusPedidos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StatusAnterior = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    StatusNovo = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    AlteradoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PedidoId = table.Column<long>(type: "bigint", nullable: false),
                    AlteradoPorId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoStatusPedidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricoStatusPedidos_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistoricoStatusPedidos_Usuarios_AlteradoPorId",
                        column: x => x.AlteradoPorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ItensPedido",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Quantidade = table.Column<int>(type: "integer", nullable: false),
                    PrecoUnitario = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    PrecoTotal = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Observacoes = table.Column<string>(type: "VARCHAR(300)", maxLength: 300, nullable: true),
                    PedidoId = table.Column<long>(type: "bigint", nullable: false),
                    ItemCardapioId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensPedido", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensPedido_ItensCardapio_ItemCardapioId",
                        column: x => x.ItemCardapioId,
                        principalTable: "ItensCardapio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItensPedido_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItensPedidoOpcao",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PrecoAdicional = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    ItemPedidoId = table.Column<long>(type: "bigint", nullable: false),
                    OpcaoAdicionalId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensPedidoOpcao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensPedidoOpcao_ItensPedido_ItemPedidoId",
                        column: x => x.ItemPedidoId,
                        principalTable: "ItensPedido",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItensPedidoOpcao_OpcoesAdicionais_OpcaoAdicionalId",
                        column: x => x.OpcaoAdicionalId,
                        principalTable: "OpcoesAdicionais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoStatusPedidos_AlteradoPorId",
                table: "HistoricoStatusPedidos",
                column: "AlteradoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoStatusPedidos_PedidoId",
                table: "HistoricoStatusPedidos",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensCardapio_CategoriaId",
                table: "ItensCardapio",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensPedido_ItemCardapioId",
                table: "ItensPedido",
                column: "ItemCardapioId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensPedido_PedidoId",
                table: "ItensPedido",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensPedidoOpcao_ItemPedidoId",
                table: "ItensPedidoOpcao",
                column: "ItemPedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensPedidoOpcao_OpcaoAdicionalId",
                table: "ItensPedidoOpcao",
                column: "OpcaoAdicionalId");

            migrationBuilder.CreateIndex(
                name: "IX_OpcoesAdicionais_ItemCardapioId",
                table: "OpcoesAdicionais",
                column: "ItemCardapioId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_NumeroPedido",
                table: "Pedidos",
                column: "NumeroPedido",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_UsuarioId",
                table: "Pedidos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoricoStatusPedidos");

            migrationBuilder.DropTable(
                name: "ItensPedidoOpcao");

            migrationBuilder.DropTable(
                name: "ItensPedido");

            migrationBuilder.DropTable(
                name: "OpcoesAdicionais");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "ItensCardapio");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Categorias");
        }
    }
}
