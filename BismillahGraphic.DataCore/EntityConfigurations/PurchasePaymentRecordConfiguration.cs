using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BismillahGraphic.DataCore
{
    public class PurchasePaymentRecordConfiguration : IEntityTypeConfiguration<PurchasePaymentRecord>
    {
        public void Configure(EntityTypeBuilder<PurchasePaymentRecord> entity)
        {
            entity.HasKey(e => e.PurchasePaymentRecordID);
            entity.Property(e => e.Insert_Date)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Payment_Situation)
                .HasMaxLength(50);


            entity.Property(e => e.PurchasePaid_Date)
                .HasColumnType("date")
                .HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Purchase)
                .WithMany(p => p.PurchasePaymentRecord)
                .HasForeignKey(d => d.PurchaseID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchasePaymentRecord_Purchase");

            entity.HasOne(d => d.Registration)
                .WithMany(p => p.PurchasePaymentRecord)
                .HasForeignKey(d => d.RegistrationID);

            entity.HasOne(d => d.PurchasePaymentReceipt)
                .WithMany(p => p.PurchasePaymentRecord)
                .HasForeignKey(d => d.PurchaseReceiptID);

        }
    }
}
