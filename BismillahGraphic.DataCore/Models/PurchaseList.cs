namespace BismillahGraphic.DataCore
{
    public class PurchaseList
    {
        public int PurchaseListID { get; set; }
        public int PurchaseID { get; set; }
        public int ProductID { get; set; }
        public int RegistrationID { get; set; }
        public int MeasurementUnitId { get; set; }
        public double PurchaseQuantity { get; set; }
        public double PurchaseUnitPrice { get; set; }
        public double PurchasePrice { get; set; }
        public virtual Product Product { get; set; }
        public virtual Purchase Purchase { get; set; }
        public virtual MeasurementUnit MeasurementUnit { get; set; }
    }
}