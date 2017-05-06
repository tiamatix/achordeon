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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Achordeon.Annotations;
using Achordeon.Shell.Wpf.Helpers;
using Achordeon.Shell.Wpf.Helpers.FontViewModel;

namespace Achordeon.Shell.Wpf.Controls.FontCombo
{
    /// <summary>
    /// Interaktionslogik für FontCombo.xaml
    /// </summary>
    public partial class FontCombo : UserControl, INotifyPropertyChanged
    {
        public FontCombo()
        {
            DataContext = this;
            Fonts = new ObservableCollection<FontFamilyInfo>(new List<FontFamilyInfo>(new[] { FontViewModel.UltimateFallbackFont }));
            SelectedFont = FontViewModel.UltimateFallbackFont;
            AllowBuildinFonts = true;
            InitializeComponent();
        }


        [NotNull]
        public FontFamilyInfo SelectedFont
        {
            get { return cbFont?.SelectedItem as FontFamilyInfo ?? FontViewModel.UltimateFallbackFont; }
            set
            {
                if (cbFont != null && cbFont.SelectedItem != value)
                    cbFont.SelectedItem = value;
                OnPropertyChanged(nameof(SelectedFont));
            }
        }


        public static readonly DependencyProperty FontsProperty = DependencyProperty.Register(
            nameof(Fonts), 
            typeof (ObservableCollection<FontFamilyInfo>), 
            typeof (FontCombo),
            new PropertyMetadata(null));


        public ObservableCollection<FontFamilyInfo> Fonts
        {
            get { return (ObservableCollection<FontFamilyInfo>) GetValue(FontsProperty); }
            set
            {
                SetValue(FontsProperty, value);
                OnPropertyChanged(nameof(Fonts));
            }
        }
        

        public static readonly DependencyProperty AllowBuildinFontsProperty = DependencyProperty.Register(
            nameof(AllowBuildinFonts), 
            typeof (bool), 
            typeof (FontCombo), 
            new PropertyMetadata(true));


        public bool AllowBuildinFonts
        {
            get { return (bool) GetValue(AllowBuildinFontsProperty); }
            set
            {
                SetValue(AllowBuildinFontsProperty, value);
                OnPropertyChanged(nameof(AllowBuildinFonts));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string APropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(APropertyName));
        }

        private void FilterItems(object ASender, FilterEventArgs AE)
        {
            if (AllowBuildinFonts)
            {
                AE.Accepted = true;
                return;
            }
            var ffi = AE.Item as FontFamilyInfo;
            AE.Accepted = ffi != null && !ffi.IsBuildIn;
        }

        private void FontComboBoxOnSelectionChanged(object ASender, SelectionChangedEventArgs AE)
        {
            var ffi = AE.AddedItems.OfType<FontFamilyInfo>().FirstOrDefault();
            if (ffi != null)
                SelectedFont = ffi;
        }
    }
}
