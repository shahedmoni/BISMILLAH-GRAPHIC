using System;
using System.Collections.Generic;

namespace BismillahGraphic.DataCore
{
    public class MeasurementUnit
    {
        public MeasurementUnit()
        {
            SellingLists = new HashSet<SellingList>();
            PurchaseLists = new HashSet<PurchaseList>();
        }
        public int MeasurementUnitId { get; set; }
        public string MeasurementUnitName { get; set; }
        public DateTime InsertDate { get; set; }
        public virtual ICollection<SellingList> SellingLists { get; set; }
        public virtual ICollection<PurchaseList> PurchaseLists { get; set; }
    }
}