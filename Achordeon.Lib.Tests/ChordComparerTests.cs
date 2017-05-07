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
using Achordeon.Lib.Chords;
using DryIoc.Experimental;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Achordeon.Lib.Tests
{
    [TestClass]
    public class ChordComparerTests
    {
        [TestMethod]
        public void ChordComparerTests_BasicTests()
        {
            var IoC = CommonTestEnvironment.CreateIoCForTests();
            var cc = IoC.Get<ChordComparer>();
            Assert.AreEqual(0, cc.Compare("A", "A"));
            Assert.AreEqual(-1, cc.Compare("A", "B"));
            Assert.AreEqual(1, cc.Compare("B", "A"));
            Assert.AreEqual(0, cc.Compare("Bm", "Bm"));
            Assert.AreEqual(1, cc.Compare("Bm", "A#"));
            Assert.AreEqual(-1, cc.Compare("Am", "B#"));
        }
    }
}
