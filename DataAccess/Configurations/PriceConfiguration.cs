using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class PriceConfiguration : IEntityTypeConfiguration<Price>
    {
        public void Configure(EntityTypeBuilder<Price> builder)
        {
            builder.HasKey(x => x.PriceId);

            builder.HasOne(x => x.Window)
                .WithMany(w => w.Prices)
                .HasPrincipalKey(w => w.WindowId)
                .HasForeignKey(x => x.WindowId);

            builder.HasOne(x => x.TimeInterval)
                .WithOne(t => t.Price)
                .IsRequired();
        }
    }
}
