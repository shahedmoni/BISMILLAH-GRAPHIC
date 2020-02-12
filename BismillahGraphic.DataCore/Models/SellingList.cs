namespace BismillahGraphic.DataCore
{
    public partial class SellingList
    {
        public int SellingListID { get; set; }
        public int SellingID { get; set; }
        public int ProductID { get; set; }
        public int RegistrationID { get; set; }
        public double SellingQuantity { get; set; }
        public double SellingUnitPrice { get; set; }
        public double SellingPrice { get; set; }
        public string Details { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public virtual Product Product { get; set; }
        public virtual Selling Selling { get; set; }
    }
}
