/*! Achordeon - MIT License

Copyright (c) 2017 tiamatix / Wolf Robben

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
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using Achordeon.Common.Extensions;
using Achordeon.Common.Helpers;
using Achordeon.Shell.Wpf.Contents;
using Common.Logging;
using DryIoc.Experimental;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Squirrel;

namespace Achordeon.Shell.Wpf.Helpers.AutoUpdater
{
    public class UpdateEngine
    {
        private readonly CoreViewModel m_Core;

        private readonly HttpHelper m_HttpHelper;
        private ILog Log => m_Core.IoC.Get<Log>().Using(this);

        public UpdateEngine(CoreViewModel ACore)
        {
            m_Core = ACore;
            m_HttpHelper = new HttpHelper();
        }



        [Conditional("RELEASE")]
        public async void StartAutoUpdateCheck()
        {
            var Client = new WebClient { Proxy = m_HttpHelper.WebProxy() };
            var Downloader = new FileDownloader(Client);
            try
            {
                using (var SquirrelUpdater = await UpdateManager.GitHubUpdateManager(AchordeonConstants.GithubProjectLink, urlDownloader: Downloader))
                {
                    await SquirrelUpdater.UpdateApp();
                }
            }
            catch (HttpRequestException ex)
            {
                Log.Error(ex);
            }
            catch (WebException ex)
            {
                Log.Error(ex);
            }
            catch (SocketException ex)
            {
                Log.Error("Socket exception occurred", ex);
            }
            catch (Exception ex)
            {
                var Msg = ex.Message;
                if (Msg.CiContains("Update.exe") && Msg.CiContains("Squirrel"))
                    Log.Warn(Msg);
                else
                    throw;
            }
        }


        public async Task<string> HandleNewVersion()
        {
            string Response;
            try
            {
                Response = await m_HttpHelper.HttpGet(AchordeonConstants.GithubLatestReleaseApi);
            }
            catch (WebException ex)
            {
                Log.Error("Can't connect to Github API to check for a new new version", ex);
                return string.Empty;
            }

            if (!string.IsNullOrEmpty(Response))
            {
                JContainer Json;
                try
                {
                    Json = (JContainer) JsonConvert.DeserializeObject(Response);
                }
                catch (JsonSerializationException ex)
                {
                    Log.Error(ex);
                    return string.Empty;
                }
                var version = Json?["tag_name"]?.ToString();
                if (!string.IsNullOrEmpty(version))
                {
                    return version;
                }
                Log.Warn("Unable to find 'tag_name' in Github API response message");
                return string.Empty;
            }
            Log.Warn("Github API did not return a result");
            return string.Empty;
        }
    }
}
