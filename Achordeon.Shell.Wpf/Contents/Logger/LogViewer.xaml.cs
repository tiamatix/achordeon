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
using System.Collections.Specialized;
using System.ComponentModel;
using Achordeon.Annotations;
using Achordeon.Shell.Wpf.Helpers.LogViewModel;

namespace Achordeon.Shell.Wpf.Contents.Logger
{
    public partial class LogViewer : INotifyPropertyChanged, INotifyCollectionChanged
    {

        public LogViewer()
        {
            InitializeComponent();
            
        }

        private LogViewModel m_ViewModel;

        public LogViewModel ViewModel
        {
            get { return m_ViewModel; }
            set
            {
                if (m_ViewModel == value)
                    return;
                if (m_ViewModel != null)
                    m_ViewModel.LogEntries.CollectionChanged -= LogEntriesCollectionChanged;
                m_ViewModel = value;
                if (m_ViewModel != null)
                    m_ViewModel.LogEntries.CollectionChanged += LogEntriesCollectionChanged;
                OnPropertyChanged(nameof(ViewModel));
            }
        }

        private void LogEntriesCollectionChanged(object ASender, NotifyCollectionChangedEventArgs AArgs)
        {
            CollectionChanged?.Invoke(this, AArgs);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string APropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(APropertyName));
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}

