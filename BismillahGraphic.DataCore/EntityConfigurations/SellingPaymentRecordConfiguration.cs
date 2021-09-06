using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BismillahGraphic.DataCore
{
    public class SellingPaymentRecordConfiguration : IEntityTypeConfiguration<SellingPaymentRecord>
    {
        public void Configure(EntityTypeBuilder<SellingPaymentRecord> entity)
        {
            entity.HasKey(e => e.SellingPaymentRecordID);
            entity.Property(e => e.Insert_Date)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Payment_Situation)
                .HasMaxLength(50);

            entity.Property(e => e.Description)
                .HasMaxLength(500);


            entity.Property(e => e.SellingPaid_Date)
                .HasColumnType("date")
                .HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Selling)
                .WithMany(p => p.SellingPaymentRecord)
                .HasForeignKey(d => d.SellingID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SellingPaymentRecord_Selling");

            entity.HasOne(d => d.Registration)
                .WithMany(p => p.SellingPaymentRecord)
                .HasForeignKey(d => d.RegistrationID);

            entity.HasOne(d => d.SellingPaymentReceipt)
                .WithMany(p => p.SellingPaymentRecord)
                .HasForeignKey(d => d.ReceiptID);

        }
    }
}
