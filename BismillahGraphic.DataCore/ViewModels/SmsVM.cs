using System;
using System.Collections.Generic;

namespace BismillahGraphic.DataCore
{

    public class SmsSendSingleVM
    {
        public string PhoneNumber { get; set; }
        public string TextSMS { get; set; }
    }
    public class SmsSendMultipleVendorVM
    {
        public ICollection<SmsSendVendor> Vendors { get; set; }
        public string TextSMS { get; set; }
    }

    public class SmsSendVendor
    {
        public int VendorID { get; set; }
        public string SmsNumber { get; set; }
    }

    public class SmsSendRecordViewModel
    {
        public string PhoneNumber { get; set; }
        public string TextSMS { get; set; }
        public int TextCount { get; set; }
        public int SMSCount { get; set; }
        public DateTime Date { get; set; }
    }
}