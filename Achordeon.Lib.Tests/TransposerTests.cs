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
using System.Collections.Generic;
using Achordeon.Lib.Transposing;
using DryIoc;
using DryIoc.Experimental;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Achordeon.Lib.Tests
{
    [TestClass]
    public class TransposerTests
    {
        private Container IoC { get; set; } 

        private void Test(string AChord, int AHalftoneSteps, string AExpected)
        {
            var Result = IoC.Get<Transposer>().TransposeChord(AChord, AHalftoneSteps);
            if (!AExpected.Contains(Result))
                throw new Exception($"Transposed {AChord} {AHalftoneSteps} steps. Expected {AExpected} but result was {Result}");
        }

        private void TestSeries(string[] AChordChart)
        {
            for (int ChordIndex = 0; ChordIndex < AChordChart.Length; ChordIndex++)
            {
                var Chord = AChordChart[ChordIndex].Split(' ')[0].Trim();
                for (int TargetChordIndex = 0; TargetChordIndex < AChordChart.Length; TargetChordIndex++)
                {
                    var HalfTones = TargetChordIndex - ChordIndex;
                    var TargetIndex= ChordIndex + HalfTones;
                    if (TargetIndex >= 0 && TargetIndex < AChordChart.Length)
                    {
                        var ExpectedChord = AChordChart[TargetIndex];
                        Test(Chord, HalfTones, ExpectedChord);
                    }
                }

            }
        }

        [TestMethod]
        public void TransposerTests_ExtendedChordTests()
        {
            if (IoC == null)
                IoC = CommonTestEnvironment.CreateIoCForTests();
            Test("Fmaj7", 1, "F#maj7");
            Test("Fmaj7", 2, "Gmaj7");
            Test("Gmaj7", -2, "Fmaj7");
        }

        [TestMethod]
        public void TransposerTests_BasicTests()
        {
            if (IoC == null)
                IoC = CommonTestEnvironment.CreateIoCForTests();
            var Tests = new List<string[]>();
            Tests.Add(new string[] { "C", "C# (Db)", "D", "D# (Eb)", "E", "F", "F# (Gb)", "G", "G# (Ab)", "A", "A# (Bb)", "B", "C" });
            Tests.Add(new string[] { "C# (Db)", "D", "D# (Eb)", "E", "F", "F# (Gb)", "G", "G# (Ab)", "A", "A# (Bb)", "B", "C", "C# (Db)" });
            Tests.Add(new string[] { "D", "D# (Eb)", "E", "F", "F# (Gb)", "G", "G# (Ab)", "A", "A# (Bb)", "B", "C", "C# (Db)", "D" });
            Tests.Add(new string[] { "D# (Eb)", "E", "F", "F# (Gb)", "G", "G# (Ab)", "A", "A# (Bb)", "B", "C", "C# (Db)", "D", "D# (Eb)" });
            Tests.Add(new string[] { "E", "F", "F# (Gb)", "G", "G# (Ab)", "A", "A# (Bb)", "B", "C", "C# (Db)", "D", "D# (Eb)", "E" });
            Tests.Add(new string[] { "F", "F# (Gb)", "G", "G# (Ab)", "A", "A# (Bb)", "B", "C", "C# (Db)", "D", "D# (Eb)", "E", "F" });
            Tests.Add(new string[] { "F# (Gb)", "G", "G# (Ab)", "A", "A# (Bb)", "B", "C", "C# (Db)", "D", "D# (Eb)", "E", "F", "F# (Gb)" });
            Tests.Add(new string[] { "G", "G# (Ab)", "A", "A# (Bb)", "B", "C", "C# (Db)", "D", "D# (Eb)", "E", "F", "F# (Gb)", "G" });
            Tests.Add(new string[] { "G# (Ab)", "A", "A# (Bb)", "B", "C", "C# (Db)", "D", "D# (Eb)", "E", "F", "F# (Gb)", "G", "G# (Ab)" });
            Tests.Add(new string[] { "A", "A# (Bb)", "B", "C", "C# (Db)", "D", "D# (Eb)", "E", "F", "F# (Gb)", "G", "G# (Ab)", "A" });
            Tests.Add(new string[] { "A# (Bb)", "B", "C", "C# (Db)", "D", "D# (Eb)", "E", "F", "F# (Gb)", "G", "G# (Ab)", "A", "A# (Bb)" });
            Tests.Add(new string[] { "B", "C", "C# (Db)", "D", "D# (Eb)", "E", "F", "F# (Gb)", "G", "G# (Ab)", "A", "A# (Bb)", "B" });
            Tests.Add(new string[] { "C", "C# (Db)", "D", "D# (Eb)", "E", "F", "F# (Gb)", "G", "G# (Ab)", "A", "A# (Bb)", "B", "C" });
            foreach (var Test in Tests)
                TestSeries(Test);
        }
    }
}
