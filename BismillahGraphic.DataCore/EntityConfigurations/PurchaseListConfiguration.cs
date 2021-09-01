using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BismillahGraphic.DataCore
{
    public class PurchaseListConfiguration : IEntityTypeConfiguration<PurchaseList>
    {
        public void Configure(EntityTypeBuilder<PurchaseList> entity)
        {
            entity.Property(e => e.PurchasePrice).HasComputedColumnSql("(ceiling([PurchaseQuantity]*[PurchaseUnitPrice]))");


            entity.HasOne(d => d.Product)
                .WithMany(p => p.PurchaseList)
                .HasForeignKey(d => d.ProductID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseList_Product");

            entity.HasOne(d => d.Purchase)
                .WithMany(p => p.PurchaseList)
                .HasForeignKey(d => d.PurchaseID)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PurchaseList_Purchase");

            entity.HasOne(d => d.MeasurementUnit)
                .WithMany(p => p.PurchaseLists)
                .HasForeignKey(d => d.MeasurementUnitId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_PurchaseList_MeasurementUnit");
        }
    }
}
