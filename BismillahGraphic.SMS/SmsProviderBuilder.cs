namespace BismillahGraphic.SMS
{
    public class SmsProviderBuilder
    {
        private readonly ISmsProvider _provider;
        public int SmsBalance()
        {
            return _provider.GetSmsBalance();
        }

        public void SendSms(string massage, string number)
        {
            _provider.SendSms(massage, number);
        }

        public SmsProviderBuilder()
        {
            _provider = new SmsProviderBanglaPhone();
        }
    }
}