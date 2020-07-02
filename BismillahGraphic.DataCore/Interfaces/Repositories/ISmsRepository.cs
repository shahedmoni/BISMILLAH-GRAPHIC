namespace BismillahGraphic.DataCore
{
    public interface ISmsRepository : IRepository<SmsSendRecord>
    {
        void SendMultipleToVendor(SmsSendMultipleVendorVM model);
        void SendSingleSMS(SmsSendSingleVM model);
        DataResult<SmsSendRecordViewModel> SendRecords(DataRequest request);
        int SmsBalance();
    }
}