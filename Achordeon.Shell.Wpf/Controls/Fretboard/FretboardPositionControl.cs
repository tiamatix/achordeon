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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;
using Achordeon.Lib.MusicTheory;

namespace Achordeon.Shell.Wpf.Controls.Fretboard
{

    public class FretboardPositionControl : Button
    {
        private enum MuteOrSelectTriState : byte
        {
            Mute = 0,
            Select = 1,
            Unselect = 2
        }

        private Brush m_SelectedBackgroundBrush;
        private Brush m_UnselectedBackgroundBrush;
        private Brush m_SelectedForegroundBrush;
        private Brush m_UnselectedForegroundBrush;
        private Border m_NoteBody;
        private Label m_NoteLabel;
        private readonly double m_Size;
        private readonly FretboardString m_String;

        private bool m_IsMuted;
        private bool m_IsSelected;
        private MuteOrSelectTriState m_CurrentSelectAction;

        public int FretNumber { get; }
        public SemitoneModule CurrentSemitonesToKey => new SemitoneModule(CurrentNote.SemitoneIndex - CurrentKey.SemitoneIndex);
        public Note CurrentNote { get; }
        public Note CurrentKey { get; }
        public bool IsOpenFret => FretNumber == 0;
        public bool IsKeyNote => CurrentKey.SemitoneIndex == CurrentNote.SemitoneIndex;
        public bool NoteBelongsToMode { get; }

      

        public static Color ChangeColorBrightness(Color AColor, float AAdjustment)
        {
            var Red = (float)AColor.R;
            var Green = (float)AColor.G;
            var Blue = (float)AColor.B;

            if (AAdjustment < 0)
            {
                AAdjustment = 1 + AAdjustment;
                Red *= AAdjustment;
                Green *= AAdjustment;
                Blue *= AAdjustment;
            }
            else
            {
                Red = (255 - Red) * AAdjustment + Red;
                Green = (255 - Green) * AAdjustment + Green;
                Blue = (255 - Blue) * AAdjustment + Blue;
            }

            return Color.FromArgb(AColor.A, (byte)Red, (byte)Green, (byte)Blue);
        }

        private SolidColorBrush ChangeColorBrightness(SolidColorBrush ABrush, float AAdjustment)
        {
            return new SolidColorBrush(ChangeColorBrightness(ABrush.Color, AAdjustment));
        }

        protected override void OnInitialized(EventArgs AIniArgs)
        {
            base.OnInitialized(AIniArgs);
             m_NoteLabel = new Label()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Content = CurrentNote.Name,
                Foreground = m_UnselectedForegroundBrush
            };

            var ViewBox = new Viewbox() {Child = m_NoteLabel };
            
            m_SelectedBackgroundBrush = IsOpenFret ? Brushes.DarkSeaGreen : (NoteBelongsToMode ? ChangeColorBrightness(Brushes.Lime, -0.2f) : ChangeColorBrightness(Brushes.LightGreen, 0.2f));
            m_UnselectedBackgroundBrush = (NoteBelongsToMode ? Brushes.LightGoldenrodYellow : Brushes.WhiteSmoke);

            m_NoteBody = new Border()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Width = m_Size - 2,
                Height = m_Size - 2,
                CornerRadius = new CornerRadius(m_Size/2.0),
                BorderThickness = new Thickness(IsKeyNote ? 2.0 : 1.0),
                ToolTip = $"{CurrentNote.Name}: {Intervals.Instance.GetInterval(CurrentSemitonesToKey)}",
                Child = ViewBox
            };

           
                FocusVisualStyle = null;
                Style = null;
                Background = Brushes.Transparent;
                BorderBrush = Brushes.Transparent;
                BorderThickness = new Thickness();
                Width = m_Size;
                Height = m_Size;
            Content = m_NoteBody;       
            

            IsEnabledChanged += (ASender, AArgs) =>
            {
                Background = Brushes.Transparent;
                BorderBrush = Brushes.Transparent;
            };

            Click += (ASender, AArgs) =>
            {
                if (IsOpenFret)
                {
                    switch (m_CurrentSelectAction = (MuteOrSelectTriState)(((byte)m_CurrentSelectAction + 1) % 3))
                    {
                        case MuteOrSelectTriState.Select:
                            IsSelected = true;
                            break;
                        case MuteOrSelectTriState.Mute:
                            m_String.IsMuted = true;
                            break;
                        default:
                            m_CurrentSelectAction = MuteOrSelectTriState.Unselect;
                            IsSelected = false;
                            break;
                    }
                }
                else
                    IsSelected = !IsSelected;
            };

            m_SelectedForegroundBrush = Brushes.Black;
            m_UnselectedForegroundBrush = (NoteBelongsToMode || IsOpenFret ? Brushes.Black : Brushes.LightGray);

           
            IsSelected = false;
            IsMuted = false;

        }

        public FretboardPositionControl(FretboardString AString, int AFretNumber, Note ACurrentNode, double ASize, bool ANoteBelongsToMode, Note ACurrentRootNote)
        {
            m_String = AString;
            FretNumber = AFretNumber;
            NoteBelongsToMode = ANoteBelongsToMode;
            CurrentKey = ACurrentRootNote;
            CurrentNote = ACurrentNode;
            m_IsSelected = false;
            m_Size = ASize;
        }



        public bool IsSelected
        {
            get => m_IsSelected;
            set
            {
                try
                {
                    if (m_IsSelected == value)
                        return;
                    m_IsSelected = value;
                    if (m_IsSelected)
                    {
                        m_CurrentSelectAction = MuteOrSelectTriState.Select;
                        IsMuted = false;
                        foreach (var Pos in m_String.Except(new[] {this}))
                        {
                            Pos.IsMuted = false;
                            Pos.IsSelected = false;
                        }
                    }
                    else
                        m_CurrentSelectAction = MuteOrSelectTriState.Unselect;
                }
                finally
                {
                    UpdateColors();
                }
            }
        }


        public bool IsMuted
        {
            get => m_IsMuted;
            set
            {
                try
                {
                    if (m_IsMuted == value)
                        return;
                    m_IsMuted = value;
                    if (m_IsMuted)
                    {
                        m_CurrentSelectAction = MuteOrSelectTriState.Mute;
                        foreach (var Pos in m_String.Except(new[] { this }))
                        {
                            Pos.IsMuted = false;
                            Pos.IsSelected = false;
                        }
                    }
                }
                finally
                {
                    UpdateColors();
                }
            }
        }

        private void UpdateColors()
        {
            if (m_NoteBody == null || m_NoteLabel == null)
                return;
            m_NoteBody.Background = m_IsMuted ? Brushes.LightCoral : m_IsSelected ? m_SelectedBackgroundBrush : m_UnselectedBackgroundBrush;
            m_NoteBody.BorderBrush = IsKeyNote ? IsMuted ? Brushes.DarkGray : Brushes.DarkGreen : Brushes.Gray;
            m_NoteLabel.Foreground = m_IsSelected ? m_SelectedForegroundBrush : m_UnselectedForegroundBrush;
            m_NoteLabel.Content = m_IsMuted ? "X" : CurrentNote.Name;
        }

    }
}
