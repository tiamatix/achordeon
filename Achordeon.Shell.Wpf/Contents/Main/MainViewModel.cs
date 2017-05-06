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
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Achordeon.Common.Helpers;
using Achordeon.Shell.Wpf.Contents.Home;
using DryIoc.Experimental;
using Squirrel;

namespace Achordeon.Shell.Wpf.Contents.Main
{
    public class MainViewModel : ViewModelBase
    {
        
        private ICommand m_GoToGitHubCommand;
        private Dispatcher m_Dispatcher;
        private CoreViewModel m_Core;

        public MainViewModel(CoreViewModel ACore, Dispatcher ADispatcher)
        {
            Core = ACore;
            Dispatcher = ADispatcher;
            
            GoToGitHubCommand = new SimpleCommand
                {
                    CanExecuteDelegate = x => true,                    
                    ExecuteDelegate = x => System.Diagnostics.Process.Start(@"https://github.com/tiamatix/achordeon")
                };

            Core.DocumentsViewModel.AddDocumentView(new HomeViewModel(Core));
        }

        public Dispatcher Dispatcher
        {
            get { return m_Dispatcher; }
            set { SetProperty(ref m_Dispatcher, value, nameof(Dispatcher)); }
        }

        public CoreViewModel Core
        {
            get { return m_Core; }
            set { SetProperty(ref m_Core, value, nameof(Core)); }
        }
     
        public ICommand GoToGitHubCommand
        {
            get { return m_GoToGitHubCommand; }
            set { SetProperty(ref m_GoToGitHubCommand, value, nameof(GoToGitHubCommand)); }
        }

        public async void CheckForUpdates()
        {
            using (var mgr = new UpdateManager("C:\\Projects\\MyApp\\Releases"))
            {
                await mgr.UpdateApp();
            }
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
