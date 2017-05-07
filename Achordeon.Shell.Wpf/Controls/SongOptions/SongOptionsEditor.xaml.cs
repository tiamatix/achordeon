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
using System.Windows;
using System.Windows.Controls;
using Achordeon.Shell.Wpf.Contents;
using Achordeon.Shell.Wpf.Helpers;

namespace Achordeon.Shell.Wpf.Controls.SongOptions
{
    /// <summary>
    /// Interaktionslogik für GlobalSettingsEditor.xaml
    /// </summary>
    public partial class SongOptionsEditor : UserControl
    {
        public SongOptionsEditor()
        {
            InitializeComponent();
            DataContext = this;
        }

        public static readonly DependencyProperty SettingsProperty = DependencyProperty.Register(
            nameof(Settings),
            typeof (SongOptionsViewModel),
            typeof (SongOptionsEditor),
            new PropertyMetadata(null));


        public SongOptionsViewModel Settings
        {
            get { return (SongOptionsViewModel) GetValue(SettingsProperty); }
            set
            {
                SetValue(SettingsProperty, value);
                if (value == null)
                    return;
                ChordFontComboBox.Fonts = value.CoreViewModel.FontViewModel.AllFonts;
                TextFontComboBox.Fonts = value.CoreViewModel.FontViewModel.AllFonts;
                ChordFontComboBox.SelectedFont = value.CoreViewModel.FontViewModel.GetFontInfo(value.ChordFont);
                TextFontComboBox.SelectedFont = value.CoreViewModel.FontViewModel.GetFontInfo(value.TextFont);
                ChordFontComboBox.PropertyChanged += (ASender, AArgs) =>
                {
                    if (AArgs.PropertyName == nameof(ChordFontComboBox.SelectedFont))
                        Settings.ChordFont = ChordFontComboBox.SelectedFont.Source;
                };
                TextFontComboBox.PropertyChanged += (ASender, AArgs) =>
                {
                    if (AArgs.PropertyName == nameof(TextFontComboBox.SelectedFont))
                        Settings.TextFont = TextFontComboBox.SelectedFont.Source;
                };
                cbPageSize.ItemsSource = AchordeonConstants.SupportedPaperSizes;
                cbPageSize.SelectedItem = value.PageSize;
            }
        }

        private void PaperFormatComboOnSelectionChanged(object ASender, SelectionChangedEventArgs AE)
        {
            var NewSize = cbPageSize.SelectedItem as string;
            if (string.IsNullOrWhiteSpace(NewSize))
                return;
            if (Settings.PageSize != NewSize)
                Settings.PageSize = NewSize;
        }
    }
}
