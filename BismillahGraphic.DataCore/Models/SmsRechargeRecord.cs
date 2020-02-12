using System;

namespace BismillahGraphic.DataCore
{
    public partial class SmsRechargeRecord
    {
        public int SMS_Recharge_RecordID { get; set; }
        public int? RechargeSMS { get; set; }
        public double? PerSMS_Price { get; set; }
        public double? Total_Price { get; set; }
        public DateTime? Date { get; set; }
        public bool? Is_Paid { get; set; }
    }
}
