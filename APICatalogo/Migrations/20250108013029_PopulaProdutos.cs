using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopulaProdutos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("INSERT INTO Produto (Nome, Descricao, Preco, ImageUrl, Estoque, DataCadastro, CategoriaId) " +
       "VALUES ('Suco de Laranja', 'Suco feito a partir de Laranjas Amazonicas', 10.00, 'sucoLaranja.jpg', 50, now(), 1);");

            mb.Sql("INSERT INTO Produto (Nome, Descricao, Preco, ImageUrl, Estoque, DataCadastro, CategoriaId) " +
                   "VALUES ('Cachorro Quente', 'Pão com salsicha e mostarda', 15.00, 'hotdog.jpg', 60, now(), 2);");

            mb.Sql("INSERT INTO Produto (Nome, Descricao, Preco, ImageUrl, Estoque, DataCadastro, CategoriaId) " +
                   "VALUES ('Pudim', 'Doce feito com doce de leite, açucar', 15.00, 'pudim.jpg', 30, now(), 3);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("DELETE FROM Produtos");
        }
    }
}
