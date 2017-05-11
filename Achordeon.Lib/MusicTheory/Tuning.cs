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
using System.ComponentModel;

namespace Achordeon.Lib.MusicTheory
{
    [ImmutableObject(true)]
    public class Tuning : IEnumerable<Interval>
    {
        public string Name { get; }
        public int NumberOfStrings => m_StringIntervals.Count;

        private readonly List<Interval> m_StringIntervals = new List<Interval>();

        public Tuning(string AName, IEnumerable<int> AStringTuningIntervals)
        {
            Name = AName;
            foreach (var Interval in AStringTuningIntervals)
                m_StringIntervals.Add(Intervals.Instance.GetInterval(Interval));
        }

        public Interval GetStringTuning(int AStringIndex)
        {
            return m_StringIntervals[AStringIndex];
        }

        public IEnumerator<Interval> GetEnumerator()
        {
            return m_StringIntervals.GetEnumerator();
        }

        public override string ToString()
        {
            return Name;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) m_StringIntervals).GetEnumerator();
        }
    }
}
