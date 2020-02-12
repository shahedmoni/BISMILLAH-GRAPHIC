using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BismillahGraphic.DataCore
{
    public class SmsBalanceConfiguration : IEntityTypeConfiguration<SmsBalance>
    {
        public void Configure(EntityTypeBuilder<SmsBalance> entity)
        {
            entity.ToTable("SMS_Balance");
            entity.HasKey(e => e.SMSBalanceID);

            entity.Property(e => e.Insert_Date)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.SMS_Balance)
                .HasDefaultValueSql("((0))");

        }
    }
}
