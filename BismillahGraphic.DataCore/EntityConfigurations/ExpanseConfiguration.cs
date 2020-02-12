using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BismillahGraphic.DataCore
{
    public class ExpanseConfiguration : IEntityTypeConfiguration<Expanse>
    {
        public void Configure(EntityTypeBuilder<Expanse> entity)
        {
            entity.Property(e => e.ExpanseDate)
                .IsRequired()
                .HasColumnType("date")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.ExpanseFor).HasMaxLength(256);

            entity.Property(e => e.Expense_Payment_Method)
                .HasMaxLength(50);

            entity.Property(e => e.Insert_Date)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.RegistrationID).HasColumnName("RegistrationID");

            entity.HasOne(d => d.ExpanseCategory)
                .WithMany(p => p.Expanse)
                .HasForeignKey(d => d.ExpanseCategoryID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Expanse_Expanse_Category");

        }
    }
}
