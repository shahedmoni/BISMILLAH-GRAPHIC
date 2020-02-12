using System;

namespace BismillahGraphic.DataCore
{
    public partial class SmsBalance
    {
        public int SMSBalanceID { get; set; }
        public int Insert_RegistrationID { get; set; }
        public int? SMS_Balance { get; set; }
        public DateTime Insert_Date { get; set; }
    }
}
