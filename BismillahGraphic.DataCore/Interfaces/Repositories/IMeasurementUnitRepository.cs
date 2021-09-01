using System.Collections.Generic;

namespace BismillahGraphic.DataCore
{
    public interface IMeasurementUnitRepository : IRepository<MeasurementUnit>
    {
        ICollection<DDL> ddl();
        bool RemoveCustom(int id);
    }
}