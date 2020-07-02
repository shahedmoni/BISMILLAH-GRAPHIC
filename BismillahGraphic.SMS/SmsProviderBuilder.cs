using System;

namespace BismillahGraphic.SMS
{
    public class SmsProviderBuilder
    {
        private readonly ISmsProvider _provider;
        public string Error { get; private set; }
        public int StatusCode { get; private set; }
        public bool IsSuccess { get; private set; }

        public SmsProviderBuilder()
        {
            _provider = new SmsProviderBanglaPhone();
        }
        public int SmsBalance()
        {
            try
            {
                this.IsSuccess = true;
                return _provider.GetSmsBalance();
            }
            catch (Exception e)
            {
                this.IsSuccess = false;
                this.Error = e.Message;
            }
            return 0;

        }

        public string SendSms(string massage, string number)
        {
            try
            {
                this.IsSuccess = true;
                return _provider.SendSms(massage, number);
            }
            catch (Exception e)
            {
                this.Error = e.Message;
                this.IsSuccess = false;
            }

            return string.Empty;
        }


    }
}