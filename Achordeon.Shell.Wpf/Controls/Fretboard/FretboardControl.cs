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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Achordeon.Annotations;
using Achordeon.Lib.Chords;
using Achordeon.Lib.MusicTheory;


namespace Achordeon.Shell.Wpf.Controls.Fretboard
{
    public class FretboardControl : Grid, IEnumerable<FretboardString>, INotifyPropertyChanged
    {
        private readonly List<FretboardString> m_Strings = new List<FretboardString>();
        private Grid m_NoteGrid;
        private double m_NoteBoxWidth;
        private double m_NoteBoxHeight;
        private double m_UniformNoteBoxSize;
        private double m_NoteCircleSize;
        private int[] m_LastSelection;
        private Tuning m_Tuning;
        private HarmonicMode m_Mode;
        private int m_NumberofFrets;
        private int m_NumberOfStrings;

        public Note KeyNote { get;  }
        
        public event PropertyChangedEventHandler PropertyChanged;

        

        public FretboardControl() :this(6, 
            21, 
            Notes.Instance.DefaultRootNote, 
            Tunings.Instance.GetDefault(6), 
            HarmonicModes.Instance.Default)
        {

        }

        public FretboardControl(int ANumberOfStrings, int ANumberofFrets, Note AKeyNote, Tuning ATuning, HarmonicMode AMode)
        {
            m_LastSelection = new int[] { };
            VerticalAlignment = VerticalAlignment.Top;
            ShowGridLines = false;                
            NumberOfStrings = ANumberOfStrings;
            NumberofFrets = ANumberofFrets;
            KeyNote = AKeyNote;
            m_Tuning = ATuning;
            m_Mode = AMode;
        }

        public int NumberofFrets
        {
            get => m_NumberofFrets;
            set
            {                               
                m_NumberofFrets = value;
                OnPropertyChanged(nameof(NumberofFrets));
                CreateControls();
            }
        }

        public int NumberOfStrings
        {
            get => m_NumberOfStrings;
            set
            {                               
                m_NumberOfStrings = value;
                OnPropertyChanged(nameof(NumberOfStrings));
                CreateControls();
            }
        }

        public HarmonicMode Mode
        {
            get => m_Mode;
            set
            {                               
                m_Mode = value;
                OnPropertyChanged(nameof(HarmonicMode));
                CreateControls();
            }
        }

        public Tuning Tuning
        {
            get => m_Tuning;
            set
            {                               
                m_Tuning = value;
                OnPropertyChanged(nameof(Tuning));
                CreateControls();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string APropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(APropertyName));
        }

        public static readonly DependencyProperty TuningProperty = DependencyProperty.Register(
            nameof(Tuning), 
            typeof (Tuning), 
            typeof (FretboardControl),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(
            nameof(Mode), 
            typeof (HarmonicMode), 
            typeof (FretboardControl),
            new PropertyMetadata(null));

        public static readonly DependencyProperty NumberOfStringsProperty = DependencyProperty.Register(
            nameof(NumberOfStrings), 
            typeof (int), 
            typeof (FretboardControl),
            new PropertyMetadata(null));

        public static readonly DependencyProperty NumberofFretsProperty = DependencyProperty.Register(
            nameof(NumberofFrets), 
            typeof (int), 
            typeof (FretboardControl),
            new PropertyMetadata(null));

        private void CreateControls()
        {
            Children.Clear();
            m_Strings.Clear();            
            m_NoteGrid = new Grid()
            {
                VerticalAlignment = VerticalAlignment.Top,
                ShowGridLines = false
            };    
            if (double.IsNaN(Width) || double.IsNaN(Height))
                return;
            m_NoteBoxWidth = Width / (double)NumberOfStrings;
            m_NoteBoxHeight = Height / (double)NumberofFrets;
            m_UniformNoteBoxSize = m_NoteBoxWidth <= m_NoteBoxHeight ? m_NoteBoxWidth : m_NoteBoxHeight;
            m_NoteCircleSize = m_UniformNoteBoxSize*0.8;
            foreach (var x in Enumerable.Range(0, NumberofFrets + 1))
                m_NoteGrid.ColumnDefinitions.Add(new ColumnDefinition());
            foreach (var x in Enumerable.Range(0, NumberOfStrings + 1))
                m_NoteGrid.RowDefinitions.Add(new RowDefinition());
            CreateFrets();        
            CreateMarkers();
            CreateStrings();
            PoplatePositions();
            Children.Add(m_NoteGrid);
            if (m_LastSelection.Length > 0)
                SelectedFrets = m_LastSelection;
        }

        protected override void OnInitialized(EventArgs AArgs)
        {
            base.OnInitialized(AArgs);
            CreateControls();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo ASizeInfo)
        {
            base.OnRenderSizeChanged(ASizeInfo);
            Width = Math.Max(300, ASizeInfo.NewSize.Width);
            Height = Math.Max(100, ASizeInfo.NewSize.Height);
            CreateControls();
        }

        private void PoplatePositions()
        {
            foreach (var StringIndex in Enumerable.Range(0, NumberOfStrings))
            {
                var String = new FretboardString(StringIndex);
                var StringTuning = Tuning.GetStringTuning(StringIndex);
                foreach (var FretIndex  in Enumerable.Range(0, NumberofFrets + 1))
                {
                    var EffectiveSemitones = new SemitoneModule(FretIndex + StringTuning.NumberOfSemitones);
                    var NoteAtPosition = Notes.Instance.GetNote(EffectiveSemitones);
                    var ModalInterval = new SemitoneModule(EffectiveSemitones.IntervalIndex - KeyNote.SemitoneIndex);
                    var Position = new FretboardPositionControl(String, FretIndex, NoteAtPosition, m_NoteCircleSize, Mode.ContainsInterval(ModalInterval), KeyNote);
                    Grid.SetColumn(Position, FretIndex);
                    Grid.SetRow(Position, StringIndex);
                    m_NoteGrid.Children.Add(Position);
                    String.AddPosition(Position);
                }
                m_Strings.Add(String);
            }
        }
     

        private void CreateMarkers()
        {
            var SemitonePosition = new SemitoneModule();
            foreach (var Fret in Enumerable.Range(0, NumberofFrets+1))
            {
                switch (SemitonePosition.Value)
                {
                    case 0:
                    case 3:
                    case 5:
                    case 7:
                    case 9:
                    case 12:
                        var Marker = new FretMarkerControl(Fret == 0 ? "open" : Fret.ToString());
                        m_NoteGrid.Children.Add(Marker);
                        Grid.SetColumn(Marker, Fret);
                        Grid.SetRow(Marker, NumberOfStrings);
                        break;
                }
                SemitonePosition++;
            }
        }

        private void CreateFrets()
        {
            m_NoteGrid.Height = m_UniformNoteBoxSize*NumberOfStrings;
            foreach (var FretIndex in Enumerable.Range(1, NumberofFrets + 1))
            {
                var Fret = new Line()
                {
                    X1 = FretIndex == 0 ? 3 : Width/ (NumberofFrets + 1.0)* FretIndex,
                    X2 = FretIndex == 0 ? 3 : Width / (NumberofFrets + 1.0)* FretIndex,
                    Y1 = 0,
                    Y2 = m_UniformNoteBoxSize * NumberOfStrings - (m_UniformNoteBoxSize - NumberOfStrings),
                    StrokeThickness = FretIndex == 1 ? 4 : 2,
                    Stroke = FretIndex < 2 ? Brushes.Silver : Brushes.Black,                    
                    StrokeEndLineCap = FretIndex == 1 ? PenLineCap.Round : PenLineCap.Triangle,
                    StrokeStartLineCap = FretIndex == 1 ? PenLineCap.Round : PenLineCap.Triangle
                };
                Children.Add(Fret);
            }          
        }

        private void CreateStrings()
        {
            var StringSize = m_NoteCircleSize / 2.0;
            foreach (var StringIndex in Enumerable.Range(0, NumberOfStrings))
            {
                var String = new Line()
                {
                    X1 = Width / (NumberofFrets + 1.0),
                    X2 = Width,
                    Y1 = StringIndex * (m_NoteCircleSize + 2.0) + StringSize + 0.5,
                    Y2 = StringIndex * (m_NoteCircleSize + 2.0) + StringSize + 0.5,
                    StrokeThickness = 1,
                    Stroke = Brushes.Silver,
                };
                Children.Add(String);
            }
        }


        public IEnumerator<FretboardString> GetEnumerator()
        {
            return m_Strings.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) m_Strings).GetEnumerator();
        }

        public void ShowChord(Chord AChord)
        {
            if (AChord == null)
            {
                SelectedFrets = new int[] {};
                return;
            }
            SelectedFrets = AChord.GetAbsoluteFrets(NumberOfStrings);
        }

        public IEnumerable<int> SelectedFrets
        {
            get {return this.Select(AString => AString.SelectedFret).Reverse().ToList();}
            set
            {
                m_Strings.ForEach(AString =>
                {
                    AString.SelectedFret = CommonConstants.UNUSED_FRET;
                    AString.IsOpen = true;
                });
                var CurrentString = 0;
                foreach (var StringFret in value.Reverse().TakeWhile(AStringFret => CurrentString < m_Strings.Count))
                {
                    m_Strings[CurrentString].SelectedFret = StringFret;
                    if (StringFret == CommonConstants.UNUSED_FRET)
                        m_Strings[CurrentString].IsMuted = true;
                    CurrentString++;
                }
                m_LastSelection = value.ToArray();
            }
        }
    }
}
