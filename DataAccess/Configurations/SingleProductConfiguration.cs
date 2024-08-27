using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class SingleProductConfiguration : IEntityTypeConfiguration<SingleProduct>
    {
        public void Configure(EntityTypeBuilder<SingleProduct> builder)
        {
            builder.HasKey(x => x.SingleProductId);

            builder.HasMany(x => x.Windows)
                .WithOne(w => w.SingleProduct)
                .HasPrincipalKey(x => x.SingleProductId)
                .HasForeignKey(x => x.SingleProductId);
        }
    }
}
