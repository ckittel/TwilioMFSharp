using System.Text;

namespace TwilioMFSharp
{
    internal static class HttpUtils
    {
        public static string UrlEncodeDataString(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            var encodedData = string.Empty;
            foreach (var b in bytes)
            {
                var c = (char)b;
                if (IsSafeUriChar(c))
                {
                    encodedData += c;
                }
                else
                {
                    encodedData += '%' + b.ToString("X");
                }
            }
            return encodedData;
        }

        private static bool IsSafeUriChar(char c)
        {
            return '.' == c || '-' == c || '_' == c || '~' == c
                || (c >= 'a' && c <= 'z')
                || (c >= 'A' && c <= 'Z')
                || (c >= '0' && c <= '9');
        }
    }
}
