using eShop.Vendas.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop.Vendas.Data.Mappings
{
    public class PedidoItemMapping : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.ProdutoNome)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.ToTable("PedidoItens");
        }
    }
}