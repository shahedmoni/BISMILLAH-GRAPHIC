using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BismillahGraphic.DataCore
{
    public class SellingListConfiguration : IEntityTypeConfiguration<SellingList>
    {
        public void Configure(EntityTypeBuilder<SellingList> entity)
        {

            entity.Property(e => e.Details).HasComputedColumnSql("((CONVERT([nvarchar](100),[Length])+'X')+CONVERT([nvarchar](100),[Width]))");

            entity.Property(e => e.SellingPrice).HasComputedColumnSql("(ceiling([SellingQuantity]*[SellingUnitPrice]))");


            entity.HasOne(d => d.Product)
                .WithMany(p => p.SellingList)
                .HasForeignKey(d => d.ProductID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SellingList_Product");

            entity.HasOne(d => d.Selling)
                .WithMany(p => p.SellingList)
                .HasForeignKey(d => d.SellingID)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_SellingList_Selling");

            entity.HasOne(d => d.MeasurementUnit)
                .WithMany(p => p.SellingLists)
                .HasForeignKey(d => d.MeasurementUnitId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_SellingList_MeasurementUnit");
        }
    }
}
