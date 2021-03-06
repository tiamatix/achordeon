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
using System.Linq;
using ServicesLibrary;

namespace Achordeon.Lib.MusicTheory
{
    [ImmutableObject(true)]
    public class ChordShape : IEnumerable<Interval>
    {
        public string Name { get; }

        private readonly List<Interval> m_Intervals = new List<Interval>();

        public ChordShape(string AName, IEnumerable<int> AIntervals)
        {
            Name = AName;
            foreach (var Interval in AIntervals)
                m_Intervals.Add(Intervals.Instance.GetInterval(Interval));
        }

        public bool ContainsInterval(int AInterval)
        {
            return ContainsInterval(Intervals.Instance.GetInterval(AInterval));
        }

        public bool ContainsInterval(SemitoneModule AInterval)
        {
            return ContainsInterval(Intervals.Instance.GetInterval(AInterval));
        }

        public bool ContainsInterval(Interval AInterval)
        {
            return m_Intervals.Any(AInt => AInt.NumberOfSemitones == AInterval.NumberOfSemitones);
        }

        public IEnumerator<Interval> GetEnumerator()
        {
            return m_Intervals.GetEnumerator();
        }

        public override string ToString()
        {
            return Name;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) m_Intervals).GetEnumerator();
        }
    }
}
