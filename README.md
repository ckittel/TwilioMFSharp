# TwilioMFSharp
A Twilio API client targeting NetMF 4.3

## Usage

```csharp
var msg = new SmsMessage
{
    FromNumber = "+19091234321",  // Must be a Twilio-purchased number
    ToNumber = "+13033216587",
    Message = "Panic, it's all falling apart!"
};
var twilioClient = new TwilioClient("My Account ID", "My Auth ID");
twilioClient.SendSmsMessage(msg);
```
