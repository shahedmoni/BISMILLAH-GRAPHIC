using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BismillahGraphic.DataCore
{
    class SellingPaymentReceiptConfiguration : IEntityTypeConfiguration<SellingPaymentReceipt>
    {
        public void Configure(EntityTypeBuilder<SellingPaymentReceipt> entity)
        {
            entity.HasKey(e => e.ReceiptID);
            entity.Property(e => e.PaidAmount).IsRequired();
            entity.Property(e => e.ReceiptSN).IsRequired();

            entity.Property(e => e.Insert_Date)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Payment_Situation)
                .HasMaxLength(50);


            entity.Property(e => e.Paid_Date).IsRequired()
                .HasColumnType("date")
                .HasDefaultValueSql("(getdate())");


            entity.HasOne(d => d.Registration)
                .WithMany(p => p.SellingPaymentReceipt)
                .HasForeignKey(d => d.RegistrationID);


            entity.HasOne(d => d.Vendor)
                .WithMany(p => p.SellingPaymentReceipt)
                .HasForeignKey(d => d.VendorID);
        }
    }
}
