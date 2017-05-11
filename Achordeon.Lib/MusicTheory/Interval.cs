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

namespace Achordeon.Lib.MusicTheory
{
    [ImmutableObject(true)]
    public class Interval : IComparable<Interval>
    {
        public int NumberOfSemitones { get; }
        public string Name { get; }
        public string Value { get; }

        public Interval(int ANumberOfSemitones, string AName, string AValue)
        {
            NumberOfSemitones = ANumberOfSemitones;
            Name = AName;
            Value = AValue;
        }

        public int CompareTo(Interval AOther)
        {
            if (AOther == null)
                return 1;
            return NumberOfSemitones.CompareTo(AOther.NumberOfSemitones);
        }

        public override bool Equals(object AOther)
        {
            return CompareTo(AOther as Interval) == 0;
        }

        public override int GetHashCode()
        {
            return NumberOfSemitones.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Name} (short: {Value}), {NumberOfSemitones} semitones";
        }
    }
}
