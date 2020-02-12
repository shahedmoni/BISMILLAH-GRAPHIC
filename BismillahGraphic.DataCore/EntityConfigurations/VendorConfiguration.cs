using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BismillahGraphic.DataCore
{
    public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
    {
        public void Configure(EntityTypeBuilder<Vendor> entity)
        {

            entity.HasKey(e => e.VendorID);

            entity.Property(e => e.Insert_Date)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.VendorAddress).HasMaxLength(500);

            entity.Property(e => e.VendorCompanyName)
                .IsRequired()
                .HasMaxLength(128);

            entity.Property(e => e.VendorDue).HasComputedColumnSql("([TotalAmount]-([TotalDiscount]+[VendorPaid]))");

            entity.Property(e => e.VendorName).HasMaxLength(128);

            entity.Property(e => e.VendorPhone).HasMaxLength(50);

        }
    }
}
