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
using System.IO;
using System.Windows.Input;
using System.Windows.Threading;
using Achordeon.Common.Helpers;
using Achordeon.Shell.Wpf.Properties;
using DryIoc.Experimental;
using Microsoft.Win32;

namespace Achordeon.Shell.Wpf.Contents.Home
{
    public class HomeViewModel : ViewModelBase, IDocViewModel
    {
        private Dispatcher m_Dispatcher;
        private CoreViewModel m_CoreViewModel;
        
        private ICommand m_OpenSongCommand;
        private ICommand m_OpenRecentSongCommand;
        private ICommand m_NewSongCommand;
        
        public HomeViewModel(CoreViewModel ACore)
        {
            CoreViewModel = ACore;
            Dispatcher = Dispatcher.CurrentDispatcher;
           

            NewSongCommand = new SimpleCommand
                {
                    CanExecuteDelegate = x => true,
                    ExecuteDelegate = x =>
                    {
                        Dispatcher.BeginInvoke((Action)(() =>
                        {
                            CoreViewModel.DocumentsViewModel.NewChordProFile();
                        }));                        
                    }
            };

            OpenSongCommand = new SimpleCommand
            {
                CanExecuteDelegate = x => true,
                ExecuteDelegate = x =>
                {
                    var FileName = x?.ToString() ?? string.Empty;
                    if (string.IsNullOrWhiteSpace(FileName))
                    {
                        var dialog = new OpenFileDialog();
                        dialog.Filter = Resources.ChordProFileDialogFilter;
                        bool? result = dialog.ShowDialog();
                        if ((result.HasValue) && (result.Value))
                            FileName = dialog.FileName;
                    }
                    if (string.IsNullOrWhiteSpace(FileName))
                        return;
                    if (!File.Exists(FileName))
                    {
                        CoreViewModel.SettingsViewModel.RecentFileList.Remove(FileName);
                        CoreViewModel.IoC.Get<IMessageBoxService>().ShowWarningAsync(
                           Resources.MissingFileHeader, 
                            string.Format(Resources.MissingFileBody, FileName));
                        return;
                    }
                    CoreViewModel.DocumentsViewModel.OpenChordProFile(FileName);
                }
            };

            OpenRecentSongCommand = new SimpleCommand
            {
                CanExecuteDelegate = x => OpenSongCommand.CanExecute(x),
                ExecuteDelegate = x => OpenSongCommand.Execute(x)
            };
        }

        
        public CoreViewModel CoreViewModel
        {
            get { return m_CoreViewModel; }
            set { SetProperty(ref m_CoreViewModel, value, nameof(CoreViewModel)); }
        }

        public Dispatcher Dispatcher
        {
            get { return m_Dispatcher; }
            set { SetProperty(ref m_Dispatcher, value, nameof(Dispatcher)); }
        }

        public ICommand NewSongCommand
        {
            get { return m_NewSongCommand; }
            set { SetProperty(ref m_NewSongCommand, value, nameof(NewSongCommand)); }
        }

        public ICommand OpenRecentSongCommand
        {
            get { return m_OpenRecentSongCommand; }
            set { SetProperty(ref m_OpenRecentSongCommand, value, nameof(OpenRecentSongCommand)); }
        }

        public ICommand OpenSongCommand
        {
            get { return m_OpenSongCommand; }
            set { SetProperty(ref m_OpenSongCommand, value, nameof(OpenSongCommand)); }
        }

        public string TabHeaderText => Resources.HomeButtonTitle;

        public string StatusBarText => string.Empty;

        public string UniqueKey => GetType().FullName;

        public bool HasUnsavedChanges => false;

        private HomeView m_HomeView;

        public object GetTabContent()
        {
            if (m_HomeView == null)
                m_HomeView = new HomeView(this);
            return m_HomeView;
        }

        public void BeforeClose(CloseEventArguments AArguments)
        {
            //Nothing to do            
        }
    }
}
