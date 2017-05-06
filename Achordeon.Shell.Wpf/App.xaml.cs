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
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Achordeon.Common.Helpers;
using Achordeon.Shell.Wpf.Contents;
using Achordeon.Shell.Wpf.Contents.ErrorDialog;
using Achordeon.Shell.Wpf.Contents.Main;
using DryIoc.Experimental;
using Squirrel;

namespace Achordeon
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application, IDisposable
    {
        private static CoreViewModel _Core;
        private bool m_IsDisposed;

        private void OnStartup(object sender, StartupEventArgs e)
        {           
            RegisterAppDomainExceptions();
            if (_Core == null)
                _Core = new CoreViewModel();
            Current.MainWindow = new MainWindow(_Core);
            RegisterExitEvents();
            Current.MainWindow.Visibility = Visibility.Visible;
        }

        private void RegisterExitEvents()
        {
            AppDomain.CurrentDomain.ProcessExit += (s, e) => Dispose();
            Current.Exit += (s, e) => Dispose();
            Current.SessionEnding += (s, e) => Dispose();
        }

        public static void HandleUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //handle non-ui thread exceptions
            Application.Current.MainWindow.Dispatcher.Invoke(() =>
            {
                _Core?.IoC?.Get<Log>()?.Using<Application>().Error(e);
                var Dlg = new ErrorDialog((Exception) e.ExceptionObject);
                Dlg.ShowDialog();
                Environment.Exit(0);
            });
        }

        public static void HandleDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            //handle ui thread exceptions
            _Core?.IoC?.Get<Log>()?.Using<Application>().Error(e);
            var Dlg = new ErrorDialog(e.Exception);
            Dlg.ShowDialog();
            //prevent crash
            e.Handled = true;
        }

        [Conditional("RELEASE")]
        private void RegisterDispatcherUnhandledException()
        {
            DispatcherUnhandledException += HandleDispatcherUnhandledException;
        }

        [Conditional("RELEASE")]
        private static void RegisterAppDomainExceptions()
        {
            AppDomain.CurrentDomain.UnhandledException += HandleUnhandledException;
            AppDomain.CurrentDomain.FirstChanceException += (s, e) => _Core?.IoC?.Get<Log>()?.Using<Application>().Error(e);
        }

        public void Dispose()
        {
            if (m_IsDisposed)
                return;
            Current.Dispatcher.Invoke(() => _Core.Dispose());
            m_IsDisposed = true;
        }
    }
}
