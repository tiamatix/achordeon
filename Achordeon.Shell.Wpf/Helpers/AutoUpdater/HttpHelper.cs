/*! Achordeon - MIT License

Copyright (c) 2017 Wolf Robben

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
!*/
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Achordeon.Annotations;

namespace Achordeon.Shell.Wpf.Helpers.AutoUpdater
{
    public class HttpHelper
    {
        public HttpProxySettings ProxySettings { private get; set; }

        public IWebProxy WebProxy()
        {
            if (ProxySettings != null && ProxySettings.Enabled && !string.IsNullOrEmpty(ProxySettings.Server))
            {
                if (string.IsNullOrWhiteSpace(ProxySettings.UserName) || string.IsNullOrWhiteSpace(ProxySettings.Password))
                    return new WebProxy(ProxySettings.Server, ProxySettings.Port);
                var Proxy = new WebProxy(ProxySettings.Server, ProxySettings.Port)
                {
                    Credentials = new NetworkCredential(ProxySettings.UserName, ProxySettings.Password)
                };
                return Proxy;
            }
            return WebRequest.GetSystemWebProxy();
        }

        public async Task<string> HttpGet(string AUrl, string AEncodingTitle = "UTF-8")
        {
            var Request = WebRequest.CreateHttp(AUrl);
            Request.Method = "GET";
            Request.Timeout = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;
            Request.Proxy = WebProxy();
            Request.UserAgent = @"Mozilla/5.0 (Trident/7.0; rv:11.0) like Gecko";
            var Response = await Request.GetResponseAsync() as HttpWebResponse;
            var ResponseStream = Response?.GetResponseStream();
            if (ResponseStream == null)
                return string.Empty;
            using (var reader = new StreamReader(ResponseStream, Encoding.GetEncoding(AEncodingTitle)))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
