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
using ServicesLibrary;

namespace Achordeon.Lib.MusicTheory
{
    [ImmutableObject(true)]
    public class Note : IComparable<Note>
    {
        public string Name { get; }
        public int SemitoneIndex { get; }

        public const int MIN_INTERVAL = 0;
        public const int MAX_INTERVAL = 11;

        public Note(string AName, int ASemitoneIndex)
        {
            if (ASemitoneIndex < MIN_INTERVAL || ASemitoneIndex > MAX_INTERVAL)
                throw new IndexOutOfRangeException(nameof(ASemitoneIndex));
            Name = AName ?? throw new NullReferenceException(nameof(AName));
            SemitoneIndex = ASemitoneIndex;
        }

        public Interval GetIntervalTo(Note AOtherNote)
        {
            if (AOtherNote == null)
                throw new NullReferenceException(nameof(AOtherNote));
            return Intervals.Instance.GetInterval(this, AOtherNote);
        }

        public SemitoneModule GetSemitonesTo(Note AOtherNote)
        {
            if (AOtherNote == null)
                throw new NullReferenceException(nameof(AOtherNote));
            return new SemitoneModule(AOtherNote.SemitoneIndex - SemitoneIndex);
        }

        public int CompareTo(Note AOther)
        {
            return AOther == null ? 1 : SemitoneIndex.CompareTo(AOther.SemitoneIndex);
        }

        public override bool Equals(object AOther)
        {
            return CompareTo(AOther as Note) == 0;
        }

        public override int GetHashCode()
        {
            return SemitoneIndex.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
