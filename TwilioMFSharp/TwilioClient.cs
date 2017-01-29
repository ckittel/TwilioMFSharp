using System;
using System.Collections;

namespace TwilioMFSharp
{
    public class TwilioClient : ITwilioClient
    {
        private readonly ITwilioHttpClient _httpClient;
        private readonly Uri _messageUri;

        public TwilioClient(string accountId, string authId)
            : this(accountId, authId, -1) { }

        public TwilioClient(string accountId, string authId, int defaultRequestTimeout)
        {
            _httpClient = defaultRequestTimeout >= 1 ? 
                            new TwilioHttpClient(accountId, authId, defaultRequestTimeout) 
                            : new TwilioHttpClient(accountId, authId);

            _messageUri = new Uri("https://api.twilio.com/2010-04-01/Accounts/" + accountId + "/Messages.json", UriKind.Absolute);
        }

        public bool SendSmsMessage(SmsMessage message, int requestTimeout)
        {
            var dataValues = new DictionaryEntry[3];
            dataValues[0] = new DictionaryEntry("To", message.ToNumber);
            dataValues[1] = new DictionaryEntry("From", message.FromNumber);
            dataValues[2] = new DictionaryEntry("Body", message.Message);

            
            var response = requestTimeout >= 1
                ? _httpClient.MakeRequest(_messageUri, dataValues, requestTimeout)
                : _httpClient.MakeRequest(_messageUri, dataValues);
            return true; // TODO return based on response.SatatusCode
        }

        public bool SendSmsMessage(SmsMessage message)
        {
            return SendSmsMessage(message, -1);
        }
    }
}
