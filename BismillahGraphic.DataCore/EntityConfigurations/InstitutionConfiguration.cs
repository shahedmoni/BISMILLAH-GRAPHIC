using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BismillahGraphic.DataCore
{
    public class InstitutionConfiguration : IEntityTypeConfiguration<Institution>
    {
        public void Configure(EntityTypeBuilder<Institution> entity)
        {
            entity.Property(e => e.Address).HasMaxLength(500);

            entity.Property(e => e.City).HasMaxLength(128);

            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Dialog_Title)
                .HasMaxLength(256);

            entity.Property(e => e.Email).HasMaxLength(50);

            entity.Property(e => e.Established).HasMaxLength(50);

            entity.Property(e => e.InstitutionName).HasMaxLength(500);

            entity.Property(e => e.LocalArea).HasMaxLength(128);

            entity.Property(e => e.Phone).HasMaxLength(50);

            entity.Property(e => e.PostalCode).HasMaxLength(50);

            entity.Property(e => e.State).HasMaxLength(128);

            entity.Property(e => e.Website).HasMaxLength(50);

        }
    }
}
