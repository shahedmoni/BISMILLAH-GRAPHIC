namespace BismillahGraphic.DataCore
{
    public class SellingListRepository : Repository<SellingList>, ISellingListRepository
    {
        public SellingListRepository(DataContext context) : base(context)
        {
        }
    }
}
