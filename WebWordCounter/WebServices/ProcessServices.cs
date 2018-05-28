using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Threading.Tasks;
using System.IO;
namespace WebWordCounter.WebServices
{

    public static class ProcessWebService 
    {
        public static string APIHttpRequest(string url)
        {
            var NumberOfRetries = 2;
            HttpWebRequest requestProvider = null;
            for (int i = 1; i <= NumberOfRetries; ++i)
            {
                requestProvider = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
                requestProvider.Method = "GET";
                requestProvider.Timeout = 15000;

                requestProvider.Headers.Add("Accept-Language", "en-gb,en;q=0.5");
                requestProvider.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                requestProvider.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");

                requestProvider.Headers.Add("Accept-Charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.7");
                requestProvider.KeepAlive = true;
                requestProvider.Headers.Add("X-Same-Domain", "1");
                requestProvider.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                    
                requestProvider.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

                requestProvider.Headers.Add("Pragma", "no-cache");
                requestProvider.Headers.Add("Cache-Control", "no-cache");
                requestProvider.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.94 Safari/537.36";

               
                try
                {
                    var response = requestProvider.GetResponse(); ;
                    var contentStream = response.GetResponseStream();
                    var resp = (HttpWebResponse)response;
                    var stIn = new StreamReader(contentStream);

                    var responseFromProvider = stIn.ReadToEnd();
                    return responseFromProvider;

                }
                catch (TimeoutException t)
                {
                   
                    return "";
                }
                catch (Exception ex)
                {
                    if (i == NumberOfRetries)
                        return "";
                }
                finally
                {
                    requestProvider.Abort();

                }
                Task.Delay(3000);
            }

            return "";
        }
    }
}