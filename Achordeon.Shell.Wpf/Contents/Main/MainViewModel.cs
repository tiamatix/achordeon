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
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Achordeon.Common.Helpers;
using Achordeon.Shell.Wpf.Contents.Home;
using Achordeon.Shell.Wpf.Helpers;
using Achordeon.Shell.Wpf.Helpers.AutoUpdater;
using DryIoc.Experimental;
using Squirrel;

namespace Achordeon.Shell.Wpf.Contents.Main
{
    public class MainViewModel : ViewModelBase
    {
        private string m_ProjectUrl;
        private string m_LicenseUrl;
        private string m_ReleaseNotesUrl;
        private Dispatcher m_Dispatcher;
        private CoreViewModel m_Core;

        public MainViewModel(CoreViewModel ACore, Dispatcher ADispatcher)
        {
            Core = ACore;
            Dispatcher = ADispatcher;
            Core.DocumentsViewModel.AddDocumentView(new HomeViewModel(Core));
            ReleaseNotesUrl = AchordeonConstants.GithubLatestReleaseNotes;
            ProjectUrl = AchordeonConstants.GithubProjectLink;
            LicenseUrl = AchordeonConstants.GithubLicense;
        }

        public Dispatcher Dispatcher
        {
            get => m_Dispatcher;
            set => SetProperty(ref m_Dispatcher, value, nameof(Dispatcher));
        }

        public CoreViewModel Core
        {
            get => m_Core;
            set => SetProperty(ref m_Core, value, nameof(Core));
        }

        public string ProjectUrl
        {
            get => m_ProjectUrl;
            set => SetProperty(ref m_ProjectUrl, value, nameof(ProjectUrl));
        }

        public string LicenseUrl
        {
            get => m_LicenseUrl;
            set => SetProperty(ref m_LicenseUrl, value, nameof(LicenseUrl));
        }

        public string ReleaseNotesUrl
        {
            get => m_ReleaseNotesUrl;
            set => SetProperty(ref m_ReleaseNotesUrl, value, nameof(ReleaseNotesUrl));
        }


        public async void StartAutoUpdateCheck()
        {
            await Task.Delay(5000);
            if (Core.SettingsViewModel.AutoUpdates)
                new UpdateEngine(Core).StartAutoUpdateCheck();
        }

        public async Task Closing(System.ComponentModel.CancelEventArgs AArgs)
        {
            var Args = new CloseEventArguments();
            await Core.DocumentsViewModel.CloseAll(Args);
            AArgs.Cancel = !Args.CloseConfirmed;
        }

        public void Closed()
        {
        }

    }
}
