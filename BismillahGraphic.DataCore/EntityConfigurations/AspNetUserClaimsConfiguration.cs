using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BismillahGraphic.DataCore
{
    public class AspNetUserClaimsConfiguration : IEntityTypeConfiguration<AspNetUserClaims>
    {
        public void Configure(EntityTypeBuilder<AspNetUserClaims> entity)
        {
            entity.HasIndex(e => e.UserId)
                    .HasName("IX_UserId");

            entity.Property(e => e.UserId)
                .IsRequired()
                .HasMaxLength(128);

            entity.HasOne(d => d.User)
                .WithMany(p => p.AspNetUserClaims)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId");
        }
    }
}
