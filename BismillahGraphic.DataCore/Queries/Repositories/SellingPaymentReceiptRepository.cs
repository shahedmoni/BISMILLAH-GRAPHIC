namespace BismillahGraphic.DataCore
{
    public class SellingPaymentReceiptRepository : Repository<SellingPaymentReceipt>, ISellingPaymentReceiptRepository
    {
        public SellingPaymentReceiptRepository(DataContext context) : base(context)
        {
        }
    }
}
