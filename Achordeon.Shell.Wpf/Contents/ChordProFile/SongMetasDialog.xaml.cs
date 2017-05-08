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
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Achordeon.Annotations;

namespace Achordeon.Shell.Wpf.Contents.ChordProFile
{
    /// <summary>
    /// Interaktionslogik für AboutWindow.xaml
    /// </summary>
    public partial class SongMetasDialog : INotifyPropertyChanged
    {
        private ChordProFileViewModel m_SongView;
        private string m_SongTitle;
        private string m_SongSubTitles;

        public string SongTitle
        {
            get { return m_SongTitle; }
            set { SetProperty(ref m_SongTitle, value, nameof(SongTitle)); }
        }

        public string SongSubTitles
        {
            get { return m_SongSubTitles; }
            set { SetProperty(ref m_SongSubTitles, value, nameof(SongSubTitles)); }
        }

        public ChordProFileViewModel SongView
        {
            get { return m_SongView; }
            set
            {
                SetProperty(ref m_SongView, value, nameof(SongView));
                LoadFromSongView();
            }
        }


        public SongMetasDialog()
        {
            InitializeComponent();
            SongTitle = string.Empty;
            SongSubTitles = string.Empty;
            SongView = null;
            ClosingFinished += (ASender, AArgs) => SaveToSongView();
            DataContext = this;
        }

        private void LoadFromSongView()
        {
            var Song = SongView?.GetSongBook().Songs.FirstOrDefault();
            if (Song == null)
                return;
            SongTitle = Song.Title;
            SongSubTitles = string.Join(Environment.NewLine, Song.Subtitles).Trim();
        }

        private void SaveToSongView()
        {
            SongView = null;
        }

        private void CloseButtonClick(object ASender, RoutedEventArgs AE)
        {
            Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string APropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(APropertyName));
        }

        protected bool SetProperty<T>(ref T AField, T AValue, [CallerMemberName] string APropertyName = null)
        {
            if (Equals(AField, AValue))
                return false;
            AField = AValue;
            OnPropertyChanged(APropertyName);
            return true;
        }
    }
}
