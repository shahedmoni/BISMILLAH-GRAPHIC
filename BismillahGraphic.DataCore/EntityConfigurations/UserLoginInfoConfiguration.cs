using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BismillahGraphic.DataCore
{
    public class UserLoginInfoConfiguration : IEntityTypeConfiguration<UserLoginInfo>
    {
        public void Configure(EntityTypeBuilder<UserLoginInfo> entity)
        {

            entity.ToTable("User_Login_Info");

            entity.HasKey(e => e.UserLoginInfoID);

            entity.Property(e => e.Email).HasMaxLength(50);

            entity.Property(e => e.Password).HasMaxLength(50);

            entity.Property(e => e.UserName).HasMaxLength(50);

        }
    }
}
