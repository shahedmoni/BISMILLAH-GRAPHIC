using BismillahGraphic.SMS;
using System.Linq;

namespace BismillahGraphic.DataCore
{
    public class SmsRepository : Repository<SmsSendRecord>, ISmsRepository
    {
        public SmsRepository(DataContext context) : base(context)
        {

        }

        public string SendMultipleToVendor(SmsSendMultipleVendorVM model)
        {
            var massageLength = SmsValidator.MassageLength(model.TextSMS);
            var smsCount = SmsValidator.TotalSmsCount(model.TextSMS);
            var vendorCount = model.Vendors.Count();

            var totalSMS = smsCount * vendorCount;

            var numbers = string.Join(",", model.Vendors.Select(v => v.SmsNumber));
            var smsProvider = new SmsProviderBuilder();

            var smsBalance = smsProvider.SmsBalance();
            if (smsBalance < totalSMS) return "No SMS Balance";

            var providerSendId = smsProvider.SendSms(model.TextSMS, numbers);

            if (!smsProvider.IsSuccess) return smsProvider.Error;

            var smsRecord = model.Vendors.Select(v => new SmsSendRecord
            {
                PhoneNumber = v.SmsNumber,
                TextSMS = model.TextSMS,
                TextCount = massageLength,
                SMSCount = smsCount,
                VendorID = v.VendorID,
                SmsProviderSendId = providerSendId,
            }).ToList();
            Context.SmsSendRecord.AddRange(smsRecord);

            return "Success";
        }

        public string SendSingleSMS(SmsSendSingleVM model)
        {
            var massageLength = SmsValidator.MassageLength(model.TextSMS);
            var smsCount = SmsValidator.TotalSmsCount(model.TextSMS);

            var smsProvider = new SmsProviderBuilder();

            var smsBalance = smsProvider.SmsBalance();
            if (smsBalance < smsCount) return "No SMS Balance";

            var providerSendId = smsProvider.SendSms(model.TextSMS, model.PhoneNumber);

            if (!smsProvider.IsSuccess) return smsProvider.Error;

            var smsRecord = new SmsSendRecord
            {
                PhoneNumber = model.PhoneNumber,
                TextSMS = model.TextSMS,
                TextCount = massageLength,
                SMSCount = smsCount,
                SmsProviderSendId = providerSendId,
            };
            Context.SmsSendRecord.Add(smsRecord);

            return "Success";
        }

        public DataResult<SmsSendRecordViewModel> SendRecords(DataRequest request)
        {
            var r = Context.SmsSendRecord.Select(s => new SmsSendRecordViewModel
            {
                PhoneNumber = s.PhoneNumber,
                TextSMS = s.TextSMS,
                TextCount = s.TextCount,
                SMSCount = s.SMSCount,
                Date = s.Date.GetValueOrDefault()
            });

            return r.ToDataResult(request);
        }

        public int SmsBalance()
        {
            var smsProvider = new SmsProviderBuilder();
            return smsProvider.SmsBalance();
        }
    }
}