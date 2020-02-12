using System;

namespace BismillahGraphic.DataCore
{
    public partial class SmsSendRecord
    {
        public Guid SMS_Send_ID { get; set; }
        public string PhoneNumber { get; set; }
        public string TextSMS { get; set; }
        public double? TextCount { get; set; }
        public double? SMSCount { get; set; }
        public string PurposeOfSMS { get; set; }
        public string SMS_Response { get; set; }
        public string Status { get; set; }
        public DateTime? Date { get; set; }
    }
}
