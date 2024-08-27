using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class WindowConfiguration : IEntityTypeConfiguration<Window>
    {
        public void Configure(EntityTypeBuilder<Window> builder)
        {
            builder.HasKey(x => x.WindowId);

            builder.HasMany(x => x.TimeIntervals)
                .WithOne(x => x.Window)
                .HasPrincipalKey(x => x.WindowId);

            builder.HasOne(x => x.SingleProduct)
                .WithMany(p => p.Windows)
                .HasPrincipalKey(x => x.SingleProductId)
                .HasForeignKey(x => x.SingleProductId);

            builder.HasOne(x => x.ComboProduct)
                .WithMany(p => p.Windows)
                .HasPrincipalKey(x => x.ComboProductId)
                .HasForeignKey(x => x.ComboProductId);

            builder.HasMany(x => x.Prices)
                .WithOne(x => x.Window)
                .HasPrincipalKey(x => x.WindowId);
        }
    }
}
