using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class ComboProductConfiguration : IEntityTypeConfiguration<ComboProduct>
    {
        public void Configure(EntityTypeBuilder<ComboProduct> builder)
        {
            builder.HasKey(c => c.ComboProductId);

            builder.HasMany(c => c.SingleProducts)
                .WithMany(s => s.ComboProducts)
                .UsingEntity<Dictionary<string, object>>(
                    "ComboSingleProduct",
                    j => j.HasOne<SingleProduct>().WithMany().HasForeignKey("SingleProductId"),
                    j => j.HasOne<ComboProduct>().WithMany().HasForeignKey("ComboProductId")
                );

            builder.HasMany(x => x.Windows)
                .WithOne(w => w.ComboProduct)
                .HasPrincipalKey(x => x.ComboProductId)
                .HasForeignKey(x => x.ComboProductId);
        }
    }
}
