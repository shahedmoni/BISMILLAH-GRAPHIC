using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BismillahGraphic.DataCore
{
    public class SmsRechargeRecordConfiguration : IEntityTypeConfiguration<SmsRechargeRecord>
    {
        public void Configure(EntityTypeBuilder<SmsRechargeRecord> entity)
        {


            entity.ToTable("SMS_Recharge_Record");

            entity.HasKey(e => e.SMS_Recharge_RecordID);

            entity.Property(e => e.Date)
                .HasColumnType("date")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Is_Paid)
                .HasDefaultValueSql("((0))");

            entity.Property(e => e.PerSMS_Price)
                .HasDefaultValueSql("((0))");

            entity.Property(e => e.RechargeSMS)
                .HasDefaultValueSql("((0))");

            entity.Property(e => e.Total_Price)
                .HasComputedColumnSql("([RechargeSMS]*[PerSMS_Price])");

        }
    }
}
