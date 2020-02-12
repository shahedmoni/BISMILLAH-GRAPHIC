using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BismillahGraphic.DataCore
{
    public class SmsSendRecordConfiguration : IEntityTypeConfiguration<SmsSendRecord>
    {
        public void Configure(EntityTypeBuilder<SmsSendRecord> entity)
        {

            entity.HasKey(e => e.SMS_Send_ID);

            entity.ToTable("SMS_Send_Record");

            entity.Property(e => e.SMS_Send_ID)
                .ValueGeneratedNever();

            entity.Property(e => e.Date).HasColumnType("datetime");

            entity.Property(e => e.PhoneNumber).HasMaxLength(50);

            entity.Property(e => e.SMS_Response)
                .HasMaxLength(50);

            entity.Property(e => e.SMSCount)
                .HasDefaultValueSql("((0))");

            entity.Property(e => e.Status).HasMaxLength(50);

            entity.Property(e => e.TextCount).HasDefaultValueSql("((0))");


        }
    }
}
