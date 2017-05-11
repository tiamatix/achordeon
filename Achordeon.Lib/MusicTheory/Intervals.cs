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
    public class Intervals : IEnumerable<Interval>
    {
        private static readonly List<Interval> _Intervals = new List<Interval>();

        public static Intervals Instance => new Intervals();

        static Intervals()
        {
            var Semi = -1;
            _Intervals.Add(new Interval(++Semi, "perfect unison", "P1/P8, d2/A7"));
            _Intervals.Add(new Interval(++Semi, "minor second (half tone)", "m2/A1"));
            _Intervals.Add(new Interval(++Semi, "major second (whole tone)", "M2/d3"));
            _Intervals.Add(new Interval(++Semi, "minor third", "m3/A2"));
            _Intervals.Add(new Interval(++Semi, "major third", "M3/d4"));
            _Intervals.Add(new Interval(++Semi, "perfect fourth", "P4/A3"));
            _Intervals.Add(new Interval(++Semi, "tritone", "A4/d5"));
            _Intervals.Add(new Interval(++Semi, "perfect fifth", "P5/d6"));
            _Intervals.Add(new Interval(++Semi, "minor sixth", "m6/A5"));
            _Intervals.Add(new Interval(++Semi, "major sixth", "M6/d7"));
            _Intervals.Add(new Interval(++Semi, "minor seventh", "m7/A6"));
            _Intervals.Add(new Interval(++Semi, "major seventh", "M7/d8"));
            _Intervals.Add(new Interval(++Semi, "perfect octave", "P8/A7"));
        }

        public Interval GetInterval(int ASemitones)
        {
            var res = _Intervals.FirstOrDefault(AInterval => AInterval.NumberOfSemitones == ASemitones);
            if (res == null)
                throw new IndexOutOfRangeException(nameof(ASemitones));
            return res;
        }

        public Interval GetInterval(SemitoneModule ASemitones)
        {
            return GetInterval(ASemitones.Value);
        }

        public Interval GetInterval(Note ANoteA, Note ANoteB)
        {
            return GetInterval(ANoteA.GetSemitonesTo(ANoteB));
        }

        public IEnumerator<Interval> GetEnumerator()
        {
            return _Intervals.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _Intervals).GetEnumerator();
        }
    }
}
