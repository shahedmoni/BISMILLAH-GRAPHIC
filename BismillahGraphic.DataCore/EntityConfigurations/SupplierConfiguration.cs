using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BismillahGraphic.DataCore
{
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> entity)
        {

            entity.HasKey(e => e.SupplierID);

            entity.Property(e => e.Insert_Date)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.SupplierAddress).HasMaxLength(500);

            entity.Property(e => e.SupplierCompanyName)
                .IsRequired()
                .HasMaxLength(128);

            entity.Property(e => e.SupplierDue).HasComputedColumnSql("([TotalAmount]-([TotalDiscount]+[SupplierPaid]))");
            entity.Property(e => e.SupplierName).HasMaxLength(128);
            entity.Property(e => e.SupplierPhone).HasMaxLength(50);
            entity.Property(e => e.SmsNumber).HasMaxLength(50);

        }
    }
}
