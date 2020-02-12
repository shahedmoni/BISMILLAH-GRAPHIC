namespace BismillahGraphic.DataCore
{
    public class SellingPaymentRecordRepository : Repository<SellingPaymentRecord>, ISellingPaymentRecordRepository
    {
        public SellingPaymentRecordRepository(DataContext context) : base(context)
        {
        }
    }
}
