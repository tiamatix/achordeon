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

namespace Achordeon.Lib.MusicTheory
{
    [ImmutableObject(true)]
    public class HarmonicModes : IEnumerable<HarmonicMode>
    {
        private static readonly List<HarmonicMode> _Modes = new List<HarmonicMode>();

        public static HarmonicModes Instance => new HarmonicModes();

        static HarmonicModes()
        {
            _Modes.Add(new HarmonicMode("Ionian", new[] {0, 2, 4, 5, 7, 9, 11}));
            _Modes.Add(new HarmonicMode("Dorian", new[] {0, 2, 3, 5, 7, 9, 10}));
            _Modes.Add(new HarmonicMode("Phrygian", new[] {0, 1, 3, 5, 7, 8, 10}));
            _Modes.Add(new HarmonicMode("Lydian", new[] {0, 2, 4, 6, 7, 9, 11}));
            _Modes.Add(new HarmonicMode("Mixolydian", new[] {0, 2, 4, 5, 7, 8, 10}));
            _Modes.Add(new HarmonicMode("Aiolian", new[] {0, 2, 3, 5, 7, 8, 10}));
            _Modes.Add(new HarmonicMode("Locrian", new[] {0, 1, 3, 5, 6, 8, 10}));
            _Modes.Add(new HarmonicMode("Melodic Minor", new[] {0, 2, 3, 5, 7, 9, 11}));
            _Modes.Add(new HarmonicMode("Pentatonic Minor", new[] {0, 3, 5, 7, 10}));
            _Modes.Add(new HarmonicMode("Pentatonic Major", new[] {0, 2, 4, 7, 9}));
            _Modes.Add(new HarmonicMode("Harmonic Minor", new[] {0, 2, 3, 5, 7, 8, 11}));
            _Modes.Add(new HarmonicMode("Harmonic Major", new[] {0, 2, 4, 5, 7, 8, 11}));
            _Modes.Add(new HarmonicMode("Blues", new[] {0, 3, 4, 5, 6, 7, 10}));
            _Modes.Add(new HarmonicMode("Whole tone", new[] {0, 2, 4, 6, 8, 10}));
            _Modes.Add(new HarmonicMode("Diminished", new[] {0, 1, 3, 4, 6, 7, 9, 10}));
        }

        public HarmonicMode GetMode(string AName)
        {
            var res = _Modes.FirstOrDefault(AMode => StringComparer.OrdinalIgnoreCase.Equals((string) AMode.Name, AName));
            if (res == null)
                throw new IndexOutOfRangeException(nameof(AName));
            return res;
        }


        public IEnumerator<HarmonicMode> GetEnumerator()
        {
            return _Modes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _Modes).GetEnumerator();
        }
    }
}
