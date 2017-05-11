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
    public class ChordShapes : IEnumerable<ChordShape>
    {
        private static readonly List<ChordShape> _ChordShapes = new List<ChordShape>();

        public static ChordShapes Instance => new ChordShapes();

        static ChordShapes()
        {
            _ChordShapes.Add(new ChordShape("maj", new[] {0, 4, 7}));
            _ChordShapes.Add(new ChordShape("6", new[] {0, 4, 7, 9}));
            _ChordShapes.Add(new ChordShape("maj7", new[] {0, 4, 7, 11}));
            _ChordShapes.Add(new ChordShape("sus4", new[] {0, 5, 7}));
            _ChordShapes.Add(new ChordShape("sus2", new[] {0, 2, 7}));
            _ChordShapes.Add(new ChordShape("min", new[] {0, 3, 7,}));
            _ChordShapes.Add(new ChordShape("min6", new[] {0, 3, 7, 9}));
            _ChordShapes.Add(new ChordShape("min7", new[] {0, 3, 7, 10}));
            _ChordShapes.Add(new ChordShape("7", new[] {0, 4, 7, 10}));
            _ChordShapes.Add(new ChordShape("7+", new[] {0, 4, 8, 10}));
            _ChordShapes.Add(new ChordShape("7sus", new[] {0, 5, 7, 10}));
            _ChordShapes.Add(new ChordShape("9", new[] {0, 2, 4, 7, 10}));
            _ChordShapes.Add(new ChordShape("7#9", new[] {0, 3, 4, 7, 10}));
            _ChordShapes.Add(new ChordShape("7b9", new[] {0, 1, 4, 7, 10}));
            _ChordShapes.Add(new ChordShape("13", new[] {0, 2, 4, 7, 9, 10}));
        }

        public ChordShape GetChordShape(string AName)
        {
            var Result = _ChordShapes.FirstOrDefault(AShape => StringComparer.OrdinalIgnoreCase.Equals(AShape.Name, AName));
            if (Result == null)
                throw new IndexOutOfRangeException(nameof(AName));
            return Result;
        }


        public IEnumerator<ChordShape> GetEnumerator()
        {
            return _ChordShapes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _ChordShapes).GetEnumerator();
        }
    }
}
