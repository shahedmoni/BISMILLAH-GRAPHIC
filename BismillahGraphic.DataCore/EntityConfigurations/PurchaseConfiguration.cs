using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BismillahGraphic.DataCore
{
    public class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
    {
        public void Configure(EntityTypeBuilder<Purchase> entity)
        {

            entity.Property(e => e.Insert_Date)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.PurchaseDate)
                .HasColumnType("date")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Description).HasMaxLength(500);

            entity.Property(e => e.PurchaseDiscountAmount).HasDefaultValueSql("((0))");

            entity.Property(e => e.PurchaseDiscountPercentage).HasComputedColumnSql("(case when [PurchaseTotalPrice]=(0) then (0) else round(([PurchaseDiscountAmount]*(100))/[PurchaseTotalPrice],(2)) end)");

            entity.Property(e => e.PurchaseDueAmount).HasComputedColumnSql("(round([PurchaseTotalPrice]-([PurchaseDiscountAmount]+[PurchasePaidAmount]),(2)))");

            entity.Property(e => e.PurchasePaidAmount).HasDefaultValueSql("((0))");

            entity.Property(e => e.PurchasePaymentStatus)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasComputedColumnSql("(case when round([PurchaseTotalPrice]-([PurchaseDiscountAmount]+[PurchasePaidAmount]),(2))<=(0) then 'Paid' else 'Due' end)");

            entity.HasOne(d => d.Supplier)
                .WithMany(p => p.Purchase)
                .HasForeignKey(d => d.SupplierID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Purchase_Supplier");

        }
    }
}
