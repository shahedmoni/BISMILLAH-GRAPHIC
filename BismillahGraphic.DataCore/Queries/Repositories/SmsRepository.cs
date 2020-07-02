using BismillahGraphic.SMS;

namespace BismillahGraphic.DataCore
{
    public class SmsRepository : Repository<SmsSendRecord>, ISmsRepository
    {
        public SmsRepository(DataContext context) : base(context)
        {

        }

        public void SendMultipleToVendor(SmsSendMultipleVendorVM model)
        {
            throw new System.NotImplementedException();
        }

        public void SendSingleSMS(SmsSendSingleVM model)
        {
            throw new System.NotImplementedException();
        }

        public DataResult<SmsSendRecordViewModel> SendRecords(DataRequest request)
        {
            throw new System.NotImplementedException();
        }

        public int SmsBalance()
        {
            var smsProvider = new SmsProviderBuilder();
            return smsProvider.SmsBalance();
        }
    }
}