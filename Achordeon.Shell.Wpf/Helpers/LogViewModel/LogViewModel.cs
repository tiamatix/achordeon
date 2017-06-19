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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows.Threading;
using Achordeon.Annotations;
using Achordeon.Common.Extensions;
using Achordeon.Common.Helpers;
using Achordeon.Shell.Wpf.Contents;
using Common.Logging;
using DryIoc.Experimental;

namespace Achordeon.Shell.Wpf.Helpers.LogViewModel
{
    public class LogViewModel : ViewModelBase
    {
        private readonly CoreViewModel m_Core;

        private static volatile int _Index;

        public ObservableCollection<LogEntry> LogEntries { get; } = new ObservableCollection<LogEntry>();

        public int ErrorCount { get; private set; } = 0;

        public int WarningCount { get; private set; } = 0;

        private const int MAX_LOG_CAPACITY = 5000;

        public LogViewModel(CoreViewModel ACore)
        {
            m_Core = ACore;
            LogEntries.CollectionChanged += (ASender, AArgs) =>
            {
                OnPropertyChanged(nameof(LogEntries));
            };
        }

        public void AddEntry(LogEntry AEntry)
        {
            try
            {

                AEntry.Index = ++_Index;
                switch (AEntry.Category)
                {
                    case LogLevel.Error:
                        ErrorCount++;
                        OnPropertyChanged(nameof(ErrorCount));
                        break;
                    case LogLevel.Warn:
                        WarningCount++;
                        OnPropertyChanged(nameof(WarningCount));
                        break;
                }
                LogEntries.Insert(0, AEntry);
                if (LogEntries.Count > MAX_LOG_CAPACITY)
                {
                    for (int i = 0; i < MAX_LOG_CAPACITY * 0.1; i++)
                        LogEntries.RemoveAt(LogEntries.Count - 1);
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public void Clear()
        {
            ErrorCount =
                WarningCount = 0;
            LogEntries.Clear();
            OnPropertyChanged(nameof(WarningCount));
            OnPropertyChanged(nameof(ErrorCount));
            OnPropertyChanged(nameof(LogEntries));
        }

    }
}
