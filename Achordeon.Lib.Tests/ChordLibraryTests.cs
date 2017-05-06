/*! Achordeon - MIT License

Copyright (c) 2017 Wolf Robben

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
using Achordeon.Lib.Chords;
using DryIoc.Experimental;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Achordeon.Lib.Tests
{
    [TestClass]
    public class ChordLibraryTests
    {


        [TestMethod]
        public void ChordLibraryTests_BasicTests()
        {
            var IoC = CommonTestEnvironment.CreateIoCForTests();
            var lib = IoC.Get<CommonChordLibrary>();
            Assert.AreEqual(lib.Chords.Count, lib.Chords.BuildinCount);
            Assert.AreEqual(true, lib.Chords.ContainsCord("D7(#9)"));
            Assert.AreEqual(false, lib.Chords.ContainsCord("A(V)"));
            lib.Chords.DefineChord(@"A(V): base-fret 5 frets - 0 3 2 0 -");
            Assert.AreEqual(true, lib.Chords.ContainsCord("A(V)"));
        }

        [TestMethod]
        public void ChordLibraryTests_DefineTests()
        {
            var l1 = new ChordList();
            l1.DefineChord("G: base-fret 1 frets 3 2 0 0 3 3");
            var c1 = l1.GetChordOrNull("G");
            Assert.AreEqual(1, c1.BaseFret);
            Assert.AreEqual(Difficulty.Hard, c1.Difficulty);
            Assert.AreEqual(3, c1.Fret1);
            Assert.AreEqual(2, c1.Fret2);
            Assert.AreEqual(0, c1.Fret3);
            Assert.AreEqual(0, c1.Fret4);
            Assert.AreEqual(3, c1.Fret5);
            Assert.AreEqual(3, c1.Fret6);

            var l2 = new ChordList();
            l2.DefineChord("G 1 3 3 0 0 2 3", ChordDefinitionFormat.Obsolete);
            var c2 = l2.GetChordOrNull("G");
            Assert.AreEqual(c1.Name, c2.Name);
            Assert.AreEqual(c1.BaseFret, c2.BaseFret);
            Assert.AreEqual(c1.Fret1, c2.Fret1);
            Assert.AreEqual(c1.Fret2, c2.Fret2);
            Assert.AreEqual(c1.Fret3, c2.Fret3);
            Assert.AreEqual(c1.Fret4, c2.Fret4);
            Assert.AreEqual(c1.Fret5, c2.Fret5);
            Assert.AreEqual(c1.Fret6, c2.Fret6);
        }

    }
}
