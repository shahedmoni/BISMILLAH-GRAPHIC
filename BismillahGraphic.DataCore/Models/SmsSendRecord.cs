using System;

namespace BismillahGraphic.DataCore
{
    public partial class SmsSendRecord
    {
        public int SmsSendId { get; set; }
        public string PhoneNumber { get; set; }
        public string TextSMS { get; set; }
        public int TextCount { get; set; }
        public int SMSCount { get; set; }
        public int? VendorID { get; set; }
        public string SmsProviderSendId { get; set; }
        public DateTime? Date { get; set; }
    }
}
