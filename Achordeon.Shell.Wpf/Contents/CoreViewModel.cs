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
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using Achordeon.Common.Helpers;
using Achordeon.Shell.Wpf.Helpers;
using Achordeon.Shell.Wpf.Helpers.FontViewModel;
using Achordeon.Shell.Wpf.Helpers.LogViewModel;
using Common.Logging;
using Common.Logging.Simple;
using DryIoc;
using DryIoc.Experimental;

namespace Achordeon.Shell.Wpf.Contents
{
    public class CoreViewModel : ViewModelBase, IDisposable
    {
        private Container m_IoC;
        private LogViewModel m_LogViewModel;
        private DocumentsViewModel m_DocumentsViewModel;
        private SettingsViewModel m_SettingsViewModel;
        private FontViewModel m_FontViewModel;
        private string m_AppVersion;
        private bool m_IsDisposed;

        public void Dispose()
        {
            if (m_IsDisposed)
                return;
            m_IsDisposed = true;
            m_SettingsViewModel.SaveSettings();
            m_IoC.Dispose();
            m_DocumentsViewModel = null;
            m_SettingsViewModel = null;
            m_FontViewModel = null;
            m_IoC = null;
        }

        public Container IoC
        {
            get { return m_IoC; }
            set { SetProperty(ref m_IoC, value, nameof(IoC)); }
        }

        public FontViewModel FontViewModel
        {
            get { return m_FontViewModel; }
            set { SetProperty(ref m_FontViewModel, value, nameof(FontViewModel)); }
        }

        public DocumentsViewModel DocumentsViewModel
        {
            get { return m_DocumentsViewModel; }
            set { SetProperty(ref m_DocumentsViewModel, value, nameof(DocumentsViewModel)); }
        }

        public SettingsViewModel SettingsViewModel
        {
            get { return m_SettingsViewModel; }
            set { SetProperty(ref m_SettingsViewModel, value, nameof(SettingsViewModel)); }
        }

        public LogViewModel LogViewModel
        {
            get { return m_LogViewModel; }
            set { SetProperty(ref m_LogViewModel, value, nameof(LogViewModel)); }
        }

        private void InitializeCultures()
        {
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }

        public void InitializeIoc(params string[] ACommandLineArguments)
        {
            var C = new Container();
            C.RegisterInstance(new Log(LogManager.Adapter = new LogViewModelLoggerFactoryAdapter(LogViewModel)));
            Lib.Helpers.IoCRegistration.RegisterCommonIoCServices(C);
            C.RegisterInstance(this);
            IoC = C;
        }

        public string AppVersion
        {
            get { return m_AppVersion; }
            set { SetProperty(ref m_AppVersion, value, nameof(AppVersion)); }
        }
               

        public CoreViewModel()
        {
            InitializeCultures();
            m_LogViewModel = new LogViewModel(this);
            InitializeIoc(Environment.GetCommandLineArgs());
            IoC.Get<Log>().Using(this).Info("Initialize");
            FontViewModel = new FontViewModel(this);
            DocumentsViewModel = new DocumentsViewModel(this);
            SettingsViewModel = new SettingsViewModel(this);
            AppVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            SettingsViewModel.LoadSettings();
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(SettingsViewModel.Language.LanguageCode);
        }
    }
}
