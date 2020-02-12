using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BismillahGraphic.DataCore
{
    public class AspNetUserLoginsConfiguration : IEntityTypeConfiguration<AspNetUserLogins>
    {
        public void Configure(EntityTypeBuilder<AspNetUserLogins> entity)
        {

            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey, e.UserId })
                .HasName("PK_dbo.AspNetUserLogins");

            entity.HasIndex(e => e.UserId)
                .HasName("IX_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);

            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.Property(e => e.UserId).HasMaxLength(128);

            entity.HasOne(d => d.User)
                .WithMany(p => p.AspNetUserLogins)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId");

        }
    }
}
