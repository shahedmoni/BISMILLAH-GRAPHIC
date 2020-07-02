namespace BismillahGraphic.SMS
{
    public interface ISmsProvider
    {
        int GetSmsBalance();
        string SendSms(string massage, string number);
    }
}