namespace BismillahGraphic.SMS
{
    public interface ISmsProvider
    {
        int GetSmsBalance();
        void SendSms(string massage, string number);
    }
}