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
using ServicesLibrary;

namespace Achordeon.Lib.MusicTheory
{
    [ImmutableObject(true)]
    public class Notes : IEnumerable<Note>
    {
        private static readonly List<Note> _Notes = new List<Note>();

        public static Notes Instance => new Notes();

        static Notes()
        {
            var i = 0;
            foreach (var s in new[] {"C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B"})
                _Notes.Add(new Note(s, i++));
        }

        public Note GetNote(string AName)
        {
            var res = _Notes.FirstOrDefault(ANote => StringComparer.OrdinalIgnoreCase.Equals(ANote.Name, AName));
            if (res == null)
                throw new IndexOutOfRangeException(nameof(AName));
            return res;
        }

        public Note GetNote(int AIntervalIndex)
        {
            if (AIntervalIndex < 0 || AIntervalIndex >= _Notes.Count)
                throw new IndexOutOfRangeException(nameof(AIntervalIndex));
            return _Notes[AIntervalIndex];
        }

        public Note GetNote(SemitoneModule AIntervalIndex)
        {
            return GetNote(AIntervalIndex.IntervalIndex);
        }

        public IEnumerator<Note> GetEnumerator()
        {
            return _Notes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _Notes).GetEnumerator();
        }
    }
}
