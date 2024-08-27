using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class TimeIntervalConfiguration : IEntityTypeConfiguration<TimeInterval>
    {
        public void Configure(EntityTypeBuilder<TimeInterval> builder)
        {
            builder.HasKey(x => x.TimeIntervalId);

            builder.HasOne(x => x.Window)
                .WithMany(x => x.TimeIntervals)
                .HasForeignKey(x => x.WindowId);
        }
    }
}
