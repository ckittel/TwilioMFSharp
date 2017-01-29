using System;
using System.Collections;
using System.Net;

namespace TwilioMFSharp
{
    internal interface ITwilioHttpClient
    {
        HttpWebResponse MakeRequest(Uri uri, DictionaryEntry[] values);
        HttpWebResponse MakeRequest(Uri uri, DictionaryEntry[] values, int asyncTimeout);
    }
}