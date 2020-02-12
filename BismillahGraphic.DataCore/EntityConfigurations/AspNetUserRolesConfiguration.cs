using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BismillahGraphic.DataCore
{
    public class AspNetUserRolesConfiguration : IEntityTypeConfiguration<AspNetUserRoles>
    {
        public void Configure(EntityTypeBuilder<AspNetUserRoles> entity)
        {

            entity.HasKey(e => new { e.UserId, e.RoleId })
                .HasName("PK_dbo.AspNetUserRoles");

            entity.HasIndex(e => e.RoleId)
                .HasName("IX_RoleId");

            entity.HasIndex(e => e.UserId)
                .HasName("IX_UserId");

            entity.Property(e => e.UserId).HasMaxLength(128);

            entity.Property(e => e.RoleId).HasMaxLength(128);

            entity.HasOne(d => d.Role)
                .WithMany(p => p.AspNetUserRoles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId");

            entity.HasOne(d => d.User)
                .WithMany(p => p.AspNetUserRoles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId");

        }
    }
}
