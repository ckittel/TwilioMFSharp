using System;
using System.Collections;
using System.Net;
using System.Text;
using System.Threading;
using Microsoft.SPOT;

namespace TwilioMFSharp
{
    internal class TwilioHttpClient
    {
        private readonly string _authHeaderValue;
        private readonly int _defaultAsyncMsTimeout;
        private const string HttpMethodPost = "POST";

        public TwilioHttpClient(string accountId, string authId, int asyncTimeout)
        {
            _authHeaderValue = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(accountId + ":" + authId));
            _defaultAsyncMsTimeout = asyncTimeout;
        }

        public TwilioHttpClient(string accountId, string authId)
            : this(accountId, authId, 2000) { }

        public HttpWebResponse MakeRequest(Uri uri, DictionaryEntry[] values, int asyncTimeout)
        {
            var dataStringAsBytes = BuildHttpBodyBytes(values);

            Debug.Print("POST Request: " + uri + " (with timeout of " + asyncTimeout + "ms)");
            using (var request = CreateHttpPostRequest(uri, asyncTimeout))
            {
                if (dataStringAsBytes != null && dataStringAsBytes.Length > 0)
                {
                    request.ContentLength = dataStringAsBytes.Length;
                    using (var postStream = request.GetRequestStream())
                    {
                        postStream.Write(dataStringAsBytes, 0, dataStringAsBytes.Length);
                    }
                }

                var response = GetResponseAsync(request, asyncTimeout);
                if (response != null)
                {
                    Debug.Print("Complete.  Response code: " + response.StatusCode);
                }
                else
                {
                    Debug.Print("Complete.  No Response");
                }

                return response;
            }
        }

        public HttpWebResponse MakeRequest(Uri uri, DictionaryEntry[] values)
        {
            return MakeRequest(uri, values, _defaultAsyncMsTimeout);
        }

        private HttpWebRequest CreateHttpPostRequest(Uri uri, int asyncTimeout)
        {
            HttpWebRequest request = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(uri);
                request.Headers.Add("Authorization", _authHeaderValue);
                request.Method = HttpMethodPost;
                request.ContentType = "application/x-www-form-urlencoded";
                request.KeepAlive = false;
                request.ReadWriteTimeout = asyncTimeout;
                request.Accept = "application/json";
            }
            catch
            {
                request?.Dispose();
                throw;
            }

            return request;
        }

        private static byte[] BuildHttpBodyBytes(DictionaryEntry[] values)
        {
            var dataString = string.Empty;
            var first = true;
            if (values != null && values.Length > 0)
            {
                foreach (var dataValue in values)
                {
                    dataString += string.Concat((first ? string.Empty : "&"), (string)dataValue.Key, '=', HttpUtils.UrlEncodeDataString((string)dataValue.Value));
                    first = false;
                }
            }
            return Encoding.UTF8.GetBytes(dataString);
        }

        private HttpWebResponse GetResponseAsync(HttpWebRequest request, int asyncTimeout)
        {
            HttpWebResponse response = null;

            var bw = new Thread(() =>
            {
                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                }
                catch (Exception ex)
                {
                    Debug.Print("Failed to get HTTP Response.  Ex: " + ex);
                    // don't rethrow
                }
            });
            bw.Start();

            if (!bw.Join(asyncTimeout))
            {
                bw.Abort();

                throw new WebException("HTTP Connection Timeout. Exceeded " + asyncTimeout + "ms");
            }
            return response;
        }
    }
}