﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BismillahGraphic.DataCore
{
    class RegistrationConfiguration : IEntityTypeConfiguration<Registration>
    {
        public void Configure(EntityTypeBuilder<Registration> entity)
        {

            entity.Property(e => e.Address).HasMaxLength(1000);

            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.DateofBirth).HasMaxLength(50);

            entity.Property(e => e.Designation).HasMaxLength(128);

            entity.Property(e => e.Email).HasMaxLength(50);

            entity.Property(e => e.FatherName).HasMaxLength(128);

            entity.Property(e => e.Name).HasMaxLength(128);

            entity.Property(e => e.NationalID)
                .HasMaxLength(128);

            entity.Property(e => e.Phone).HasMaxLength(50);

            entity.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Validation)
                .IsRequired()
                .HasDefaultValue(true);


        }
    }
}
