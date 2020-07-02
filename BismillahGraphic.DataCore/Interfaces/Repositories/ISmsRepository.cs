namespace BismillahGraphic.DataCore
{
    public interface ISmsRepository : IRepository<SmsSendRecord>
    {
        string SendMultipleToVendor(SmsSendMultipleVendorVM model);
        string SendSingleSMS(SmsSendSingleVM model);
        DataResult<SmsSendRecordViewModel> SendRecords(DataRequest request);
        int SmsBalance();
    }
}