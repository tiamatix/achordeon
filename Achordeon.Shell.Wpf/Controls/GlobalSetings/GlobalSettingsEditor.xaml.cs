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

namespace Achordeon.Shell.Wpf.Controls.GlobalSetings
{
    /// <summary>
    /// Interaktionslogik für GlobalSettingsEditor.xaml
    /// </summary>
    public partial class GlobalSettingsEditor : UserControl
    {
        public GlobalSettingsEditor()
        {
            InitializeComponent();
            DataContext = this;
        }


        public static readonly DependencyProperty SettingsProperty = DependencyProperty.Register(
            nameof(Settings),
            typeof (SettingsViewModel),
            typeof (GlobalSettingsEditor),
            new PropertyMetadata(null));


        public SettingsViewModel Settings
        {
            get { return (SettingsViewModel) GetValue(SettingsProperty); }
            set
            {
                SetValue(SettingsProperty, value);
                if (value == null)
                    return;
                ChordProFontComboBox.Fonts = value.Core.FontViewModel.AllFonts;
                ChordProFontComboBox.SelectedFont = value.ChordProFont;
                ChordProFontComboBox.PropertyChanged += (ASender, AArgs) =>
                {
                    if (AArgs.PropertyName == nameof(ChordProFontComboBox.SelectedFont))
                        Settings.ChordProFont = ChordProFontComboBox.SelectedFont;
                };
                soeGlobal.Settings = value.GlobalSongOptions;
                cbLanguage.Languages = value.SupportedLanguages;
                cbLanguage.SelectedLanguage = value.Language;
                cbLanguage.PropertyChanged += (ASender, AArgs) =>
                {
                    if (AArgs.PropertyName == nameof(cbLanguage.SelectedLanguage))
                        Settings.Language = cbLanguage.SelectedLanguage;
                };
            }
        }

        private void ResetButtonClick(object ASender, RoutedEventArgs AE)
        {
            Settings?.ResetToUserDefault();
            if (Settings != null)
                ChordProFontComboBox.SelectedFont = Settings.ChordProFont;
            soeGlobal.Refresh();
        }
    }
}