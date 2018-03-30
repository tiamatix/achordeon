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
using System.Windows;
using System.Windows.Controls;
using Achordeon.Lib.Chords;
using Achordeon.Shell.Wpf.Helpers.RecentFileList;


namespace Achordeon.Shell.Wpf.Contents.Home
{
    /// <summary>
    /// Interaktionslogik für HomeView.xaml
    /// </summary>
    public partial class FretboardView : UserControl
    {
        public FretboardView(FretboardViewModel AFretboardView)
        {            
            DataContext = AFretboardView;
            View.Dispatcher = Dispatcher;
            InitializeComponent();                       
            View.PropertyChanged += ViewOnPropertyChanged;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            ApplyView();
        }

        private void ViewOnPropertyChanged(object ASender, PropertyChangedEventArgs APropertyChangedEventArgs)
        {
            ApplyView();
        }

        private void ApplyView()
        {
            fcFretboard.Tuning = View.SelectedTuning;
            fcFretboard.Mode = View.SelectedHarmonicMode;
            fcFretboard.NumberOfStrings = View.NumberOfStrings;
            fcFretboard.NumberofFrets = View.NumberOfFrets;
        }

        public FretboardViewModel View => DataContext as FretboardViewModel;


        private void CbChords_OnSelectionChanged(object ASender, SelectionChangedEventArgs AE)
        {
            if (AE.AddedItems.Count != 1)
                return;
            var Chord = AE.AddedItems[0] as Chord;
            if (Chord == null)
                return;
            fcFretboard.ShowChord(Chord);
        }
    }
}
