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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Achordeon.Annotations;
using Achordeon.Common.Extensions;
using Achordeon.Shell.Wpf.Helpers;
using Achordeon.Shell.Wpf.Helpers.FontViewModel;

namespace Achordeon.Shell.Wpf.Controls.LanguageCombo
{
    /// <summary>
    /// Interaktionslogik für FontCombo.xaml
    /// </summary>
    public partial class LanguageCombo : UserControl, INotifyPropertyChanged
    {
        public LanguageCombo()
        {
            DataContext = this;
            Languages = new ObservableCollection<LanguageInfo>(new List<LanguageInfo>(new[] { AchordeonConstants.DefaultLanguage }));
            SelectedLanguage = AchordeonConstants.DefaultLanguage;
            InitializeComponent();
        }


        [NotNull]
        public LanguageInfo SelectedLanguage
        {
            get { return cbLanguage?.SelectedItem as LanguageInfo ?? AchordeonConstants.DefaultLanguage; }
            set
            {
                if (cbLanguage == null)
                    return;
                var Selection = Languages.FirstOrDefault(a => a.LanguageCode.CiEquals(value.LanguageCode));
                cbLanguage.SelectedItem = Selection;
                OnPropertyChanged(nameof(SelectedLanguage));
            }
        }


        public static readonly DependencyProperty LanguagesProperty = DependencyProperty.Register(
            nameof(Languages), 
            typeof (ObservableCollection<LanguageInfo>), 
            typeof (LanguageCombo),
            new PropertyMetadata(null));


        public ObservableCollection<LanguageInfo> Languages
        {
            get { return (ObservableCollection<LanguageInfo>) GetValue(LanguagesProperty); }
            set
            {
                SetValue(LanguagesProperty, value);
                OnPropertyChanged(nameof(Languages));
            }
        }
                
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string APropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(APropertyName));
        }

        
        private void LanguageComboBoxOnSelectionChanged(object ASender, SelectionChangedEventArgs AE)
        {
            var sli = AE.AddedItems.OfType<LanguageInfo>().FirstOrDefault();
            if (sli != null)
                SelectedLanguage = sli;
        }
    }
}
