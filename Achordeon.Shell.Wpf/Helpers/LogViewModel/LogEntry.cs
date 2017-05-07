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
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using Common.Logging;

namespace Achordeon.Shell.Wpf.Helpers.LogViewModel
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class LogEntry : INotifyPropertyChanged
    {
        public DateTime DateTime { get; set; }

        public LogLevel Category { get; set; }

        public string Component { get; set; }

        public int Index { get; set; }

        public string Message { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => $"{GetType().Name}, C:{Category}, N={Component}, dt:{DateTime.ToString("HH:mm:ss.fff")}, Msg={Message}";

        public Brush Background
        {
            get
            {
                switch (Category)
                {
                    case LogLevel.Fatal:
                    case LogLevel.Error:
                        return Brushes.Salmon;
                    case LogLevel.Warn:
                        return Brushes.Yellow;
                    case LogLevel.Info:
                        return Brushes.White;
                    case LogLevel.Debug:
                    case LogLevel.Trace:
                        return Brushes.LightGray;
                }
                return Brushes.Transparent;
            }
        }

        public LogEntry(DateTime ADateTime, LogLevel ACategory, string AComponent, string AMessage)
        {
            DateTime = ADateTime;
            Category = ACategory;
            Index = 0;
            Component = AComponent;
            Message = AMessage;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string APropertyName)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                var EventHandler = PropertyChanged;
                EventHandler?.Invoke(this, new PropertyChangedEventArgs(APropertyName));
            }));
        }
    }
}
