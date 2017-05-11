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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Achordeon.Lib.MusicTheory;


namespace Achordeon.Shell.Wpf.Controls.Fretboard
{
    public class FretboardControl : IEnumerable<FretboardString>
    {
        private readonly List<FretboardString> m_Strings = new List<FretboardString>();

        private Grid MainGrid { get; }
        private Grid NoteGrid { get; }

        public int NumberOfStrings { get; }

        public int NumberofFrets { get; }

        private double NoteBoxWidth { get; }

        private double NoteBoxHeight { get; }

        private double UniformNoteBoxSize { get; }
        private double NoteCircleSize { get; }

        public Note KeyNote { get;  }

        public Tuning Tuning { get; }

        public HarmonicMode Mode { get; }

        public FrameworkElement Visual => MainGrid;

        public FretboardControl(double AWidth, double AHeight, int ANumberOfStrings, int ANumberofFrets, Note AKeyNote, Tuning ATuning, HarmonicMode AMode)
        {
            MainGrid = new Grid()
            {
                VerticalAlignment = VerticalAlignment.Top,
                ShowGridLines = false
            };
            MainGrid.Width = AWidth;
            MainGrid.Height = AHeight;
            NoteGrid = new Grid()
            {
                VerticalAlignment = VerticalAlignment.Top,
                ShowGridLines = false
            };          
            NumberOfStrings = ANumberOfStrings;
            NumberofFrets = ANumberofFrets;
            NoteBoxWidth = MainGrid.Width / (double)ANumberofFrets;
            NoteBoxHeight = MainGrid.Height / (double)ANumberOfStrings;
            UniformNoteBoxSize = NoteBoxWidth <= NoteBoxHeight ? NoteBoxWidth : NoteBoxHeight;
            NoteCircleSize = UniformNoteBoxSize*0.8;
            KeyNote = AKeyNote;
            Tuning = ATuning;
            Mode = AMode;
            foreach (var x in Enumerable.Range(0, NumberofFrets + 1))
                NoteGrid.ColumnDefinitions.Add(new ColumnDefinition());
            foreach (var x in Enumerable.Range(0, NumberOfStrings + 1))
                NoteGrid.RowDefinitions.Add(new RowDefinition());
            CreateFrets();        
            CreateMarkers();
            CreateStrings();
            PoplatePositions();
            MainGrid.Children.Add(NoteGrid);
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
                    var Position = new FretboardPositionControl(String, FretIndex, NoteAtPosition, NoteCircleSize, Mode.ContainsInterval(ModalInterval), KeyNote);
                    Grid.SetColumn(Position.Visual, FretIndex);
                    Grid.SetRow(Position.Visual, StringIndex);
                    NoteGrid.Children.Add(Position.Visual);
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
                        NoteGrid.Children.Add(Marker.Visual);
                        Grid.SetColumn(Marker.Visual, Fret);
                        Grid.SetRow(Marker.Visual, NumberOfStrings);
                        break;
                }
                SemitonePosition++;
            }
        }

        private void CreateFrets()
        {
            NoteGrid.Height = UniformNoteBoxSize*NumberOfStrings;
            foreach (var FretIndex in Enumerable.Range(1, NumberofFrets + 1))
            {
                var Fret = new Line()
                {
                    X1 = FretIndex == 0 ? 3 : MainGrid.Width/ (NumberofFrets + 1.0)* FretIndex,
                    X2 = FretIndex == 0 ? 3 : MainGrid.Width / (NumberofFrets + 1.0)* FretIndex,
                    Y1 = 0,
                    Y2 = UniformNoteBoxSize * NumberOfStrings - (UniformNoteBoxSize - NumberOfStrings),
                    StrokeThickness = FretIndex == 1 ? 4 : 2,
                    Stroke = FretIndex < 2 ? Brushes.Silver : Brushes.Black,                    
                    StrokeEndLineCap = FretIndex == 1 ? PenLineCap.Round : PenLineCap.Triangle,
                    StrokeStartLineCap = FretIndex == 1 ? PenLineCap.Round : PenLineCap.Triangle
                };
                MainGrid.Children.Add(Fret);
            }          
        }

        private void CreateStrings()
        {
            var StringSize = NoteCircleSize / 2.0;
            foreach (var StringIndex in Enumerable.Range(0, NumberOfStrings))
            {
                var String = new Line()
                {
                    X1 = MainGrid.Width / (NumberofFrets + 1.0),
                    X2 = MainGrid.Width,
                    Y1 = StringIndex * (NoteCircleSize + 2.0) + StringSize + 0.5,
                    Y2 = StringIndex * (NoteCircleSize + 2.0) + StringSize + 0.5,
                    StrokeThickness = 1,
                    Stroke = Brushes.Silver,
                };

                MainGrid.Children.Add(String);
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

        public IEnumerable<int?> SelectedFrets
        {
            get {return this.Select(AString => AString.SelectedFret).Reverse().ToList();}
            set
            {
                m_Strings.ForEach(AString =>
                {
                    AString.SelectedFret = null;
                    AString.IsOpen = true;
                });
                var CurrentString = 0;
                foreach (var StringFret in value.Reverse().TakeWhile(AStringFret => CurrentString < m_Strings.Count))
                {
                    m_Strings[CurrentString].SelectedFret = StringFret;
                    if (StringFret == null)
                        m_Strings[CurrentString].IsMuted = true;
                    CurrentString++;
                }
            }
        }
    }
}
