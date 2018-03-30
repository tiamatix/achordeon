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
using Achordeon.Lib.Chords;

namespace Achordeon.Shell.Wpf.Controls.Fretboard
{
    public class FretboardString : IEnumerable<FretboardPositionControl>
    {
        private readonly List<FretboardPositionControl> m_Positions = new List<FretboardPositionControl>();

        public int StringNumber { get; }

        public FretboardString(int AStringNumber)
        {
            StringNumber = AStringNumber;
        }

        public void AddPosition(FretboardPositionControl APosition)
        {
            m_Positions.Add(APosition);
        }

        public FretboardPositionControl this[int AIndex] => m_Positions[AIndex];


        public IEnumerator<FretboardPositionControl> GetEnumerator()
        {
            return m_Positions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) m_Positions).GetEnumerator();
        }

        public bool IsMuted
        {
            get => m_Positions.FirstOrDefault()?.IsMuted ?? false;
            set
            {
                var FirstPos = m_Positions.FirstOrDefault();
                if (FirstPos != null)
                    FirstPos.IsMuted = value;
            }
        }

        public bool IsOpen
        {
            get { return !IsMuted && !m_Positions.Any(APos => APos.IsSelected); }
            set => SelectedFret = 0;
        }

        public int SelectedFret
        {
            get { return m_Positions.FirstOrDefault(APos => APos.IsSelected)?.FretNumber ?? CommonConstants.UNUSED_FRET; }
            set
            {
                m_Positions.ForEach(APos => APos.IsSelected = false);
                if (value == CommonConstants.UNUSED_FRET)
                    return;
                var Pos = m_Positions.FirstOrDefault(APos => APos.FretNumber == value);
                if (Pos != null)
                    Pos.IsSelected = true;
            }
        }
    }
}
