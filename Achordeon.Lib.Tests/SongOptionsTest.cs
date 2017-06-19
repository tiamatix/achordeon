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

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Achordeon.Lib.SongOptions;
using DryIoc.Experimental;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Achordeon.Lib.Tests
{
    [TestClass]
    public class SongOptionsTests
    {
        //Does not raise property changed events
        public class TestSongOptions : ISongOptions
        {
            public bool DrawChordGrids { get; set; } = DefaultSongOptions.Default.DrawChordGrids;
            public bool TwoUp { get; set; } = DefaultSongOptions.Default.TwoUp;
            public bool FourUp { get; set; } = DefaultSongOptions.Default.FourUp;
            public string ChordFont { get; set; } = DefaultSongOptions.Default.ChordFont;
            public int ChordSizePt { get; set; } = DefaultSongOptions.Default.ChordSizePt;
            public int ChordGridSizeMm { get; set; } = DefaultSongOptions.Default.ChordGridSizeMm;
            public bool ChordGridSorted { get; set; } = DefaultSongOptions.Default.ChordGridSorted;
            public bool EvenPageNumberLeft { get; set; } = DefaultSongOptions.Default.EvenPageNumberLeft;
            public bool LyricsOnly { get; set; } = DefaultSongOptions.Default.LyricsOnly;
            public bool PageNumberLogical { get; set; } = DefaultSongOptions.Default.PageNumberLogical;
            public string PageSize { get; set; } = DefaultSongOptions.Default.PageSize;
            public bool SingleSpace { get; set; } = DefaultSongOptions.Default.SingleSpace;
            public int StartPageNumber { get; set; } = DefaultSongOptions.Default.StartPageNumber;
            public string TextFont { get; set; } = DefaultSongOptions.Default.TextFont;
            public int TextSizePt { get; set; } = DefaultSongOptions.Default.TextSizePt;
            public bool CreateToc { get; set; } = DefaultSongOptions.Default.CreateToc;
            public int TransposeByHalftones { get; set; } = DefaultSongOptions.Default.TransposeByHalftones;
            public int VerticalSpace { get; set; } = DefaultSongOptions.Default.VerticalSpace;
            public bool UseMusicalSymbols { get; set; } = DefaultSongOptions.Default.UseMusicalSymbols;

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string APropertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(APropertyName));
            }
        }

        [TestMethod]
        public void SongOptionsTests_BasicTests()
        {
            var OptionsConverter = new SongOptionsConverter(DefaultSongOptions.Default);
            var Options = new TestSongOptions();
            Assert.AreEqual(string.Empty, OptionsConverter.SaveToString(Options));
            Assert.IsTrue(OptionsConverter.IsAllDefault(Options));
            Options.FourUp = true;
            Options.ChordFont = "My Funny Font";
            Options.TransposeByHalftones = 42;
            Assert.IsFalse(OptionsConverter.IsAllDefault(Options));
            var ClonedOptions = new TestSongOptions();
            var Data = OptionsConverter.SaveToString(Options);
            OptionsConverter.LoadFromString(ClonedOptions, Data);
            Assert.IsFalse(OptionsConverter.IsAllDefault(ClonedOptions));
            Assert.IsTrue(Options.ChordFont.Equals(ClonedOptions.ChordFont));
            Assert.IsTrue(ClonedOptions.ChordFont.Equals("My Funny Font"));
            Assert.IsTrue(ClonedOptions.TransposeByHalftones == 42);
        }
    }
}
