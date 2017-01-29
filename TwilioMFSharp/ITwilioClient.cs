namespace TwilioMFSharp
{
    public interface ITwilioClient
    {
        bool SendSmsMessage(SmsMessage message);
        bool SendSmsMessage(SmsMessage message, int requestTimeout);
    }
}