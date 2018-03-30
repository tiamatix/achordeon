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
using System.IO;
using System.Linq;

namespace Achordeon.Lib.MusicTheory
{
    [ImmutableObject(true)]
    public class Tunings : IEnumerable<Tuning>
    {
        private static readonly List<Tuning> _Tunings = new List<Tuning>();

        public static Tunings Instance => new Tunings();

        static Tunings()
        {
            _Tunings.Add(new Tuning("Standard E", new[] {4, 11, 7, 2, 9, 4}, true));
            _Tunings.Add(new Tuning("Standard D#/Eb", new[] {3, 10, 6, 1, 8, 3}));
            _Tunings.Add(new Tuning("Standard D", new[] {2, 9, 5, 0, 7, 2}));
            _Tunings.Add(new Tuning("Drop D", new[] {4, 11, 7, 2, 9, 2}));
            _Tunings.Add(new Tuning("Drop C#/Db", new[] {3, 10, 6, 1, 8, 1}));
            _Tunings.Add(new Tuning("Drop C", new[] {2, 9, 5, 0, 7, 0}));
            _Tunings.Add(new Tuning("DADGAD", new[] {2, 9, 7, 2, 9, 2}));
            _Tunings.Add(new Tuning("DADDAD", new[] {2, 9, 2, 2, 9, 2}));
            _Tunings.Add(new Tuning("Cello", new[] {11, 4, 9, 2, 7, 0}));
            _Tunings.Add(new Tuning("Open D", new[] {2, 9, 6, 2, 9, 2}));
            _Tunings.Add(new Tuning("Open D minor", new[] {2, 9, 5, 2, 9, 2}));
            _Tunings.Add(new Tuning("Open G", new[] {2, 11, 7, 2, 7, 2}));
            _Tunings.Add(new Tuning("Open G minor", new[] {2, 10, 7, 2, 7, 2}));
            _Tunings.Add(new Tuning("Dobro Open", new[] {2, 11, 7, 2, 11, 7}));

            _Tunings.Add(new Tuning("Standard B", new[] {4, 11, 7, 2, 9, 4, 11}, true));
            _Tunings.Add(new Tuning("Drop A", new[] {4, 11, 7, 2, 9, 4, 9}));
            _Tunings.Add(new Tuning("Standard F#", new[] {4, 11, 7, 2, 9, 4, 11, 6}));
            _Tunings.Add(new Tuning("Drop E", new[] {4, 11, 7, 2, 9, 4, 11, 4}));
        }

        public Tuning GetDefault(int? ANumberOfStrings = 6)
        {
            var res = _Tunings.FirstOrDefault(ATuning => (!ANumberOfStrings.HasValue || ATuning.NumberOfStrings == ANumberOfStrings.Value) && ATuning.IsDefault);
            if (res == null)
                throw new InvalidDataException($"no default tuning for {ANumberOfStrings} strings defined");
            return res;
        }

        public Tuning this[string AName, int? ANumberOfStrings = 6] => GetTuning(AName, ANumberOfStrings);

        public Tuning GetTuning(string AName, int? ANumberOfStrings = 6)
        {
            var res = _Tunings.FirstOrDefault(ATuning => (!ANumberOfStrings.HasValue || ATuning.NumberOfStrings == ANumberOfStrings.Value) && StringComparer.OrdinalIgnoreCase.Equals(ATuning.Name, AName));
            if (res == null)
                throw new IndexOutOfRangeException(nameof(AName));
            return res;
        }


        public IEnumerator<Tuning> GetEnumerator()
        {
            return _Tunings.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _Tunings).GetEnumerator();
        }
    }
}
