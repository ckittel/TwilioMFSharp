using System;
using System.Collections;

namespace TwilioMFSharp
{
    public class TwilioClient
    {
        private readonly TwilioHttpClient _httpClient;
        private readonly Uri _messageUri;

        public TwilioClient(string accountId, string authId)
        {
            _httpClient = new TwilioHttpClient(accountId, authId);
            _messageUri = new Uri("https://api.twilio.com/2010-04-01/Accounts/" + accountId + "/Messages.json", UriKind.Absolute);
        }

        public bool SendSmsMessage(SmsMessage message)
        {
            var dataValues = new DictionaryEntry[3];
            dataValues[0] = new DictionaryEntry("To", message.ToNumber);
            dataValues[1] = new DictionaryEntry("From", message.FromNumber);
            dataValues[2] = new DictionaryEntry("Body", message.Message);

            var response = _httpClient.MakeRequest(_messageUri, dataValues);
            return true; // TODO return based on response.SatatusCode
        }
    }
}
