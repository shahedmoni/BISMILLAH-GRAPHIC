using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BismillahGraphic.DataCore
{
    public class MeasurementUnitConfiguration : IEntityTypeConfiguration<MeasurementUnit>
    {
        public void Configure(EntityTypeBuilder<MeasurementUnit> entity)
        {
            entity.Property(e => e.MeasurementUnitName)
                .HasMaxLength(128)
                .IsRequired();

            entity.Property(e => e.InsertDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");
        }
    }
}