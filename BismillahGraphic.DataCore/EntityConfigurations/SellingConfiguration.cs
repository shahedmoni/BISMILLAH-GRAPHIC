using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BismillahGraphic.DataCore
{
    public class SellingConfiguration : IEntityTypeConfiguration<Selling>
    {
        public void Configure(EntityTypeBuilder<Selling> entity)
        {

            entity.Property(e => e.Insert_Date)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.SellingDate)
                .HasColumnType("date")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.SellingDiscountAmount).HasDefaultValueSql("((0))");

            entity.Property(e => e.SellingDiscountPercentage).HasComputedColumnSql("(case when [SellingTotalPrice]=(0) then (0) else round(([SellingDiscountAmount]*(100))/[SellingTotalPrice],(2)) end)");

            entity.Property(e => e.SellingDueAmount).HasComputedColumnSql("(round([SellingTotalPrice]-([SellingDiscountAmount]+[SellingPaidAmount]),(2)))");

            entity.Property(e => e.SellingPaidAmount).HasDefaultValueSql("((0))");

            entity.Property(e => e.SellingPaymentStatus)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasComputedColumnSql("(case when round([SellingTotalPrice]-([SellingDiscountAmount]+[SellingPaidAmount]),(2))<=(0) then 'Paid' else 'Due' end)");

            entity.HasOne(d => d.Vendor)
                .WithMany(p => p.Selling)
                .HasForeignKey(d => d.VendorID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Selling_Vendor");

        }
    }
}
