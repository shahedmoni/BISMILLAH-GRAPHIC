using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BismillahGraphic.DataCore
{
    public class SmsSendRecordConfiguration : IEntityTypeConfiguration<SmsSendRecord>
    {
        public void Configure(EntityTypeBuilder<SmsSendRecord> entity)
        {
            entity.ToTable("SMS_Send_Record");

            entity.HasKey(e => e.SmsSendId);

            entity.Property(e => e.Date).HasColumnType("datetime");

            entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(50);

            entity.Property(e => e.TextSMS).IsRequired();

            entity.Property(e => e.SMSCount).IsRequired();

            entity.Property(e => e.TextCount).IsRequired();

            entity.Property(e => e.SmsProviderSendId).HasMaxLength(50);



        }
    }
}
