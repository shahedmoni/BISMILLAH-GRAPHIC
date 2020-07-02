using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace BismillahGraphic.SMS
{
    public class SmsProviderBanglaPhone : ISmsProvider
    {

        private const string HostUrl = "http://loopsitbd.powersms.net.bd/httpapi/";
        private const string UserId = "Sikkhaloy";
        private const string Password = "Sikkhaloy@SMS_345";

        public int GetSmsBalance()
        {
            const string actionUrl = "accountinfo";
            const string info = "package";
            const string dataFormat = "?userId={0}&password={1}&info={2}";
            var dataUrl = string.Format(dataFormat, UserId, Password, info);
            var balance = 0;

            Uri address = new Uri(HostUrl + actionUrl + dataUrl);

            // Create the web request
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;

            // Set type to POST
            request.Method = "GET";
            request.ContentType = "text/xml";

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            // Get the response stream
            StreamReader reader = new StreamReader(response.GetResponseStream());

            dynamic jsonObj = JsonConvert.DeserializeObject(reader.ReadToEnd());

            balance += (int)jsonObj.AvailableExternalSmsCount;

            return balance;
        }

        public void SendSms(string massage, string number)
        {
            throw new System.NotImplementedException();
        }
    }
}