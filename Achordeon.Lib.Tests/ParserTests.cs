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
using System.Linq;
using Achordeon.Common.Helpers;
using Achordeon.Lib.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Achordeon.Lib.Tests
{
    [TestClass]
    public class ParserTests
    {

        private const string TEST_SONG_LOVE_ME_TENDER = @"{title:Love me tender}
{subtitle:Presley/Matson}

[G]Love me tender [A7]love me sweet
[D7]Never let me [G]go
[G]You have made my [A7]life complete
[D7]and I love you [G]so

{start_of_chorus}
[G]Love me [B7]tender [Em]love me [G7]true
[C]all my [Cm]dreams ful[G]fill
[G]For [Dm6]my [E+]dar[E7]ling [A7]I love you
[D7]and I always [G]will [Am7] [D7]
{end_of_chorus}

[G]Love me tender [A7]love me long
[D7]Take me to your [G]heart
[G]For it's there that [A7]I belong
[D7]and will never [G]part

{comment:Chorus}

[G]Love me tender [A7]Love me dear
[D7]Tell me you are [G]mine
[G]I'll be yours through [A7]all the years
[D7]till the end of [G]time

{comment:Chorus}";


        private const string TEST_SONG_EVERYBODY_HURTS = @"{title: Everybody Hurts}
{subtitle: REM}
{define G: base-fret 1 frets 3 2 0 0 3 3}
{define D4: base-fret 0 frets - - 0 0 3 -}
{define: E 0 0 0 2 3 3 -}
{start_of_tab}
Intro: E----------2-----------2-------------3-----------3-------
       B--------3---3-------3---3---------3---3-------3---3-----
       G------2-------2---2-------2-----0-------0---0-----------
       D----0-----------0---------------------------------------
       A--------------------------------------------------------
       E------------------------------3-----------3------------- (repeat)
{end_of_tab}

[D]When your day is [G]long and the [D]night, the night is [G]yours a[D]lone
[D]When you're sure you've had e[G]nough of this [D]life, well [G]hang on
{start_of_tab}
   E(low)-3-2-0-
{end_of_tab}
[E]Don't let yourself [A]go, [E]cause everybody [A]cries [E]and everybody[A] hurts some[D]times [G]
Sometimes everything is [D]wrong,   [G]now it's time to sing a[D]long
When your day is night alone [G]          (hold [D]on, hold on)
If you feel like letting go [G]           (hold [D]on)
If you think you've had too [G]much of this [D]life, well hang [G]on

{start_of_tab}
   E(low)-3-2-0-
{end_of_tab}
[E]Cause everybody [A]hurts, [E]take comfort in your [A]friends
[E]Everybody [A]hurts, [E]don't throw your [A]hands, oh [E]now, don't throw your [A]hands
[C]If you feel like you're [D4]alone, no, no, no, you're not [A]alone
{start_of_tab}
           D4 ->   E-0-----0-----0-----0--
                   B---3-----3-----3------
                   G-----0-----0-----0----
{end_of_tab}
 [D]If you're on your [G]own in this [D]life, the days and nights are [G]long
[D]When you think you've had too [G]much, with this [D]life, to hang [G]on

{start_of_tab}
   E(low)-3-2-0-
{end_of_tab}
[E]Well everybody [A]hurts, some[E]times 
Everybody [A]cries, [E]and everybody [A]hurts,[N.C.] ... some[D]times [G]
But everybody [D]hurts [G]sometimes so hold [D]on, hold [G]on, hold [D]on
Hold on, [G]hold on, [D]hold on, [G]hold on, [D]hold on
[G]Everybody [D]hurts [G]     [D]     [G]
[D]You are not alone [G]     [D]     [G]     [D]     [G]";

        [TestMethod]
        public void ParserTests_EverybodyHurts()
        {
            var IoC = CommonTestEnvironment.CreateIoCForTests();
            var s = new StringStream(TEST_SONG_EVERYBODY_HURTS);
            var p = new ChordProParser(IoC, s);
            p.ReadFile();
            var Song = p.SongBook.Songs.First();
            Assert.AreEqual("Everybody Hurts", Song.Title);
            Assert.AreEqual("REM", Song.Subtitles.First());
            Assert.AreEqual(3, Song.UsedChords.DefinedCount);
        }

        [TestMethod]
        public void ParserTests_LoveMeTender()
        {
            var IoC = CommonTestEnvironment.CreateIoCForTests();
            var s = new StringStream(TEST_SONG_LOVE_ME_TENDER);
            var p = new ChordProParser(IoC, s);
            p.ReadFile();
            var Song = p.SongBook.Songs.First();
            Assert.AreEqual("Love me tender", Song.Title);
            Assert.AreEqual("Presley/Matson", Song.Subtitles.First());
            Assert.AreEqual(11, Song.UsedChords.Count);
            Assert.AreEqual(0, Song.UsedChords.DefinedCount);
        }

    }
}
