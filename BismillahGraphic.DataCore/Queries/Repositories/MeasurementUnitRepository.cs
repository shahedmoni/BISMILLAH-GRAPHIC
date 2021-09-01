using System.Collections.Generic;
using System.Linq;

namespace BismillahGraphic.DataCore
{
    public class MeasurementUnitRepository : Repository<MeasurementUnit>, IMeasurementUnitRepository
    {
        public MeasurementUnitRepository(DataContext context) : base(context)
        {
        }

        public ICollection<DDL> ddl()
        {
            return Context.MeasurementUnit.Select(e => new DDL
            {
                value = e.MeasurementUnitId,
                label = e.MeasurementUnitName
            }).ToList();
        }

        public bool RemoveCustom(int id)
        {
            if (Context.SellingList.Any(e => e.MeasurementUnitId == id)) return false;
            if (Context.PurchaseList.Any(e => e.MeasurementUnitId == id)) return false;
            Remove(Find(id));
            return true;
        }
    }
}