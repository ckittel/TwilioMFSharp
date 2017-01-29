namespace TwilioMFSharp
{
    public class SmsMessage
    {
        private string _message = string.Empty;

        /// <summary>
        /// The destination phone number.  Format with a + and country code (E.164 format)
        /// E.g. +16175551212
        /// </summary>
        public string ToNumber { get; set; }

        /// <summary>
        /// A Twilio phone number (E.164 format) or alphanumeric sender ID enabled for 
        /// the type of message you wish to send. Phone numbers or short codes 
        /// purchased from Twilio work here. You cannot (for example) spoof messages 
        /// from your own cell phone number.
        /// </summary>
        public string FromNumber { get; set; }

        /// <summary>
        /// The text of the message you want to send, truncated to a max of 1600 characters.
        /// </summary>
        public string Message
        {
            get { return _message; }
            set
            {
                if (value == null)
                {
                    _message = string.Empty;
                }
                else if (value.Length > 1600)
                {
                    _message = value.Substring(0, 1600);
                }
                else
                {
                    _message = value;
                }

            }
        }
    }
}