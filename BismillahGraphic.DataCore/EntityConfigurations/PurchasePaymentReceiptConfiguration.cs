using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BismillahGraphic.DataCore
{
    class PurchasePaymentReceiptConfiguration : IEntityTypeConfiguration<PurchasePaymentReceipt>
    {
        public void Configure(EntityTypeBuilder<PurchasePaymentReceipt> entity)
        {
            entity.HasKey(e => e.PurchaseReceiptID);
            entity.Property(e => e.PaidAmount).IsRequired();
            entity.Property(e => e.ReceiptSN).IsRequired();

            entity.Property(e => e.Insert_Date)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Payment_Situation)
                .HasMaxLength(50);

            entity.Property(e => e.Description)
                .HasMaxLength(500);

            entity.Property(e => e.Paid_Date).IsRequired()
                .HasColumnType("date")
                .HasDefaultValueSql("(getdate())");


            entity.HasOne(d => d.Registration)
                .WithMany(p => p.PurchasePaymentReceipt)
                .HasForeignKey(d => d.RegistrationID);


            entity.HasOne(d => d.Supplier)
                .WithMany(p => p.PurchasePaymentReceipt)
                .HasForeignKey(d => d.SupplierID);
        }
    }
}
