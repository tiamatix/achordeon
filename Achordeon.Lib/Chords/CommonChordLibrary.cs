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
using System.Linq;

namespace Achordeon.Lib.Chords
{
    public class CommonChordLibrary
    {
        public ChordList Chords { get; } = new ChordList();

        public Chord FindChord(string AName)
        {
            return Chords.FirstOrDefault(AChord => StringComparer.OrdinalIgnoreCase.Equals(AChord.Name, AName));
        }

        private void LearnBuildinChords()
        {
            const int N = -1;
            Chords.LearnChord("Ab", 1, 3, 3, 2, 1, 1, 4, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Ab+", N, N, 2, 1, 1, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Ab4", N, N, 1, 1, 2, 4, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Ab7", N, N, 1, 1, 1, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Ab11", 1, 3, 1, 3, 1, 1, 4, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Absus", N, N, 1, 1, 2, 4, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Absus4", N, N, 1, 1, 2, 4, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Abdim", N, N, 0, 1, 0, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Abmaj", 1, 3, 3, 2, 1, 1, 4, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Abmaj7", N, N, 1, 1, 1, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Abm", 1, 3, 3, 1, 1, 1, 4, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Abmin", 1, 3, 3, 1, 1, 1, 4, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Abm7", N, N, 1, 1, 1, 1, 4, ChordOrigin.BuildIn, Difficulty.Hard);

            Chords.LearnChord("A", N, 0, 2, 2, 2, 0, 1, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("A+", N, 0, 3, 2, 2, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("A4", 0, 0, 2, 2, 0, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("A6", N, N, 2, 2, 2, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("A7", N, 0, 2, 0, 2, 0, 1, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("A7+", N, N, 3, 2, 2, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("A7(9+)", N, 2, 2, 2, 2, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("A9", N, 0, 2, 1, 0, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("A11", N, 4, 2, 4, 3, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("A13", N, 0, 1, 2, 3, 1, 5, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("A7sus4", 0, 0, 2, 0, 3, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("A9sus", N, 0, 2, 1, 0, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Asus", N, N, 2, 2, 3, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Asus2", 0, 0, 2, 2, 0, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Asus4", N, N, 2, 2, 3, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Adim", N, N, 1, 2, 1, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Amaj", N, 0, 2, 2, 2, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Amaj7", N, 0, 2, 1, 2, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Am", N, 0, 2, 2, 1, 0, 1, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("Amin", N, 0, 2, 2, 1, 0, 1, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("A/D", N, N, 0, 0, 2, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("A/F#", 2, 0, 2, 2, 2, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("A/G#", 4, 0, 2, 2, 2, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);

            Chords.LearnChord("Am#7", N, N, 2, 1, 1, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Am(7#)", N, 0, 2, 2, 1, 4, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Am6", N, 0, 2, 2, 1, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Am7", N, 0, 2, 2, 1, 3, 1, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("Am7sus4", 0, 0, 0, 0, 3, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Am9", N, 0, 1, 1, 1, 3, 5, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Am/G", 3, 0, 2, 2, 1, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Amadd9", 0, 2, 2, 2, 1, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Am(add9)", 0, 2, 2, 2, 1, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);

            Chords.LearnChord("A#", N, 1, 3, 3, 3, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("A#+", N, N, 0, 3, 3, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("A#4", N, N, 3, 3, 4, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("A#7", N, N, 1, 1, 1, 2, 3, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("A#sus", N, N, 3, 3, 4, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("A#sus4", N, N, 3, 3, 4, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("A#maj", N, 1, 3, 3, 3, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("A#maj7", N, 1, 3, 2, 3, N, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("A#dim", N, N, 2, 3, 2, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("A#m", N, 1, 3, 3, 2, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("A#min", N, 1, 3, 3, 2, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("A#m7", N, 1, 3, 1, 2, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);

            Chords.LearnChord("Bb", N, 1, 3, 3, 3, 1, 1, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("Bb+", N, N, 0, 3, 3, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Bb4", N, N, 3, 3, 4, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Bb6", N, N, 3, 3, 3, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Bb7", N, N, 1, 1, 1, 2, 3, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Bb9", 1, 3, 1, 2, 1, 3, 6, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Bb11", 1, 3, 1, 3, 4, 1, 6, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Bbsus", N, N, 3, 3, 4, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Bbsus4", N, N, 3, 3, 4, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Bbmaj", N, 1, 3, 3, 3, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Bbmaj7", N, 1, 3, 2, 3, N, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Bbdim", N, N, 2, 3, 2, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Bbm", N, 1, 3, 3, 2, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Bbmin", N, 1, 3, 3, 2, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Bbm7", N, 1, 3, 1, 2, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Bbm9", N, N, N, 1, 1, 3, 6, ChordOrigin.BuildIn, Difficulty.Hard);

            Chords.LearnChord("B", N, 2, 4, 4, 4, 2, 1, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("B+", N, N, 1, 0, 0, 4, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("B4", N, N, 3, 3, 4, 1, 2, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("B7", 0, 2, 1, 2, 0, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("B7+", N, 2, 1, 2, 0, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("B7+5", N, 2, 1, 2, 0, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("B7#9", N, 2, 1, 2, 3, N, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("B7(#9)", N, 2, 1, 2, 3, N, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("B9", 1, 3, 1, 2, 1, 3, 7, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("B11", 1, 3, 3, 2, 0, 0, 7, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("B11/13", N, 1, 1, 1, 1, 3, 2, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("B13", N, 2, 1, 2, 0, 4, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Bsus", N, N, 3, 3, 4, 1, 2, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Bsus4", N, N, 3, 3, 4, 1, 2, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Bmaj", N, 2, 4, 3, 4, N, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Bmaj7", N, 2, 4, 3, 4, N, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Bdim", N, N, 0, 1, 0, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Bm", N, 2, 4, 4, 3, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Bmin", N, 2, 4, 4, 3, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("B/F#", 0, 2, 2, 2, 0, 0, 2, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("BaddE", N, 2, 4, 4, 0, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("B(addE)", N, 2, 4, 4, 0, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("BaddE/F#", 2, N, 4, 4, 0, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);

            Chords.LearnChord("Bm6", N, N, 4, 4, 3, 4, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Bm7", N, 1, 3, 1, 2, 1, 2, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("Bmmaj7", N, 1, 4, 4, 3, N, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Bm(maj7)", N, 1, 4, 4, 3, N, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Bmsus9", N, N, 4, 4, 2, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Bm(sus9)", N, N, 4, 4, 2, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Bm7b5", 1, 2, 4, 2, 3, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);

            Chords.LearnChord("C", N, 3, 2, 0, 1, 0, 1, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("C+", N, N, 2, 1, 1, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("C4", N, N, 3, 0, 1, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("C6", N, 0, 2, 2, 1, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("C7", 0, 3, 2, 3, 1, 0, 1, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("C9", 1, 3, 1, 2, 1, 3, 8, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("C9(11)", N, 3, 3, 3, 3, N, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("C11", N, 1, 3, 1, 4, 1, 3, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Csus", N, N, 3, 0, 1, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Csus2", N, 3, 0, 0, 1, N, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Csus4", N, N, 3, 0, 1, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Csus9", N, N, 4, 1, 2, 4, 7, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Cmaj", 0, 3, 2, 0, 1, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Cmaj7", N, 3, 2, 0, 0, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Cm", N, 1, 3, 3, 2, 1, 3, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Cmin", N, 1, 3, 3, 2, 1, 3, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Cdim", N, N, 1, 2, 1, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("C/B", N, 2, 2, 0, 1, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Cadd2/B", N, 2, 0, 0, 1, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("CaddD", N, 3, 2, 0, 3, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("C(addD)", N, 3, 2, 0, 3, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Cadd9", N, 3, 2, 0, 3, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("C(add9)", N, 3, 2, 0, 3, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);

            Chords.LearnChord("C3", N, 1, 3, 3, 2, 1, 3, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("Cm7", N, 1, 3, 1, 2, 1, 3, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("Cm11", N, 1, 3, 1, 4, N, 3, ChordOrigin.BuildIn, Difficulty.Hard);

            Chords.LearnChord("C#", N, N, 3, 1, 2, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("C#+", N, N, 3, 2, 2, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("C#4", N, N, 3, 3, 4, 1, 4, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("C#7", N, N, 3, 4, 2, 4, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("C#7(b5)", N, 2, 1, 2, 1, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("C#sus", N, N, 3, 3, 4, 1, 4, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("C#sus4", N, N, 3, 3, 4, 1, 4, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("C#maj", N, 4, 3, 1, 1, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("C#maj7", N, 4, 3, 1, 1, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("C#dim", N, N, 2, 3, 2, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("C#m", N, N, 2, 1, 2, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("C#min", N, N, 2, 1, 2, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("C#add9", N, 1, 3, 3, 1, 1, 4, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("C#(add9)", N, 1, 3, 3, 1, 1, 4, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("C#m7", N, N, 2, 4, 2, 4, 1, ChordOrigin.BuildIn, Difficulty.Hard);

            Chords.LearnChord("Db", N, N, 3, 1, 2, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Db+", N, N, 3, 2, 2, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Db7", N, N, 3, 4, 2, 4, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Dbsus", N, N, 3, 3, 4, 1, 4, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Dbsus4", N, N, 3, 3, 4, 1, 4, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Dbmaj", N, N, 3, 1, 2, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Dbmaj7", N, 4, 3, 1, 1, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Dbdim", N, N, 2, 3, 2, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Dbm", N, N, 2, 1, 2, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Dbmin", N, N, 2, 1, 2, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Dbm7", N, N, 2, 4, 2, 4, 1, ChordOrigin.BuildIn, Difficulty.Hard);

            Chords.LearnChord("D", N, N, 0, 2, 3, 2, 1, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("D+", N, N, 0, 3, 3, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("D4", N, N, 0, 2, 3, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("D6", N, 0, 0, 2, 0, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("D7", N, N, 0, 2, 1, 2, 1, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("D7#9", N, 2, 1, 2, 3, 3, 4, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("D7(#9)", N, 2, 1, 2, 3, 3, 4, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("D9", 1, 3, 1, 2, 1, 3, 10, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("D11", 3, 0, 0, 2, 1, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Dsus", N, N, 0, 2, 3, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Dsus2", 0, 0, 0, 2, 3, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Dsus4", N, N, 0, 2, 3, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("D7sus2", N, 0, 0, 2, 1, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("D7sus4", N, 0, 0, 2, 1, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Dmaj", N, N, 0, 2, 3, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Dmaj7", N, N, 0, 2, 2, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Ddim", N, N, 0, 1, 0, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Dm", N, N, 0, 2, 3, 1, 1, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("Dmin", N, N, 0, 2, 3, 1, 1, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("D/A", N, 0, 0, 2, 3, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("D/B", N, 2, 0, 2, 3, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("D/C", N, 3, 0, 2, 3, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("D/C#", N, 4, 0, 2, 3, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("D/E", N, 1, 1, 1, 1, N, 7, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("D/G", 3, N, 0, 2, 3, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("D5/E", 0, 1, 1, 1, N, N, 7, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Dadd9", 0, 0, 0, 2, 3, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("D(add9)", 0, 0, 0, 2, 3, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("D9add6", 1, 3, 3, 2, 0, 0, 10, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("D9(add6)", 1, 3, 3, 2, 0, 0, 10, ChordOrigin.BuildIn, Difficulty.Hard);

            Chords.LearnChord("Dm6(5b)", N, N, 0, 1, 0, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Dm7", N, N, 0, 2, 1, 1, 1, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("Dm#5", N, N, 0, 3, 3, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Dm(#5)", N, N, 0, 3, 3, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Dm#7", N, N, 0, 2, 2, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Dm(#7)", N, N, 0, 2, 2, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Dm/A", N, 0, 0, 2, 3, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Dm/B", N, 2, 0, 2, 3, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Dm/C", N, 3, 0, 2, 3, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Dm/C#", N, 4, 0, 2, 3, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Dm9", N, N, 3, 2, 1, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);

            Chords.LearnChord("D#", N, N, 3, 1, 2, 1, 3, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("D#+", N, N, 1, 0, 0, 4, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("D#4", N, N, 1, 3, 4, 4, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("D#7", N, N, 1, 3, 2, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("D#sus", N, N, 1, 3, 4, 4, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("D#sus4", N, N, 1, 3, 4, 4, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("D#maj", N, N, 3, 1, 2, 1, 3, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("D#maj7", N, N, 1, 3, 3, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("D#dim", N, N, 1, 2, 1, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("D#m", N, N, 4, 3, 4, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("D#min", N, N, 4, 3, 4, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("D#m7", N, N, 1, 3, 2, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);

            Chords.LearnChord("Eb", N, N, 3, 1, 2, 1, 3, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Eb+", N, N, 1, 0, 0, 4, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Eb4", N, N, 1, 3, 4, 4, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Eb7", N, N, 1, 3, 2, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Ebsus", N, N, 1, 3, 4, 4, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Ebsus4", N, N, 1, 3, 4, 4, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Ebmaj", N, N, 1, 3, 3, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Ebmaj7", N, N, 1, 3, 3, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Ebdim", N, N, 1, 2, 1, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Ebadd9", N, 1, 1, 3, 4, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Eb(add9)", N, 1, 1, 3, 4, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Ebm", N, N, 4, 3, 4, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Ebmin", N, N, 4, 3, 4, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Ebm7", N, N, 1, 3, 2, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);

            Chords.LearnChord("E", 0, 2, 2, 1, 0, 0, 1, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("E+", N, N, 2, 1, 1, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("E5", 0, 1, 3, 3, N, N, 7, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("E6", N, N, 3, 3, 3, 3, 9, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("E7", 0, 2, 2, 1, 3, 0, 1, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("E7#9", 0, 2, 2, 1, 3, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("E7(#9)", 0, 2, 2, 1, 3, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("E7(5b)", N, 1, 0, 1, 3, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("E7b9", 0, 2, 0, 1, 3, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("E7(b9)", 0, 2, 0, 1, 3, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("E7(11)", 0, 2, 2, 2, 3, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("E9", 1, 3, 1, 2, 1, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("E11", 1, 1, 1, 1, 2, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Esus", 0, 2, 2, 2, 0, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Esus4", 0, 2, 2, 2, 0, 0, 0, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Emaj", 0, 2, 2, 1, 0, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Emaj7", 0, 2, 1, 1, 0, N, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Edim", N, N, 2, 3, 2, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Em", 0, 2, 2, 0, 0, 0, 1, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("Emin", 0, 2, 2, 0, 0, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);

            Chords.LearnChord("Em6", 0, 2, 2, 0, 2, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Em7", 0, 2, 2, 0, 3, 0, 1, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("Em/B", N, 2, 2, 0, 0, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Em/D", N, N, 0, 0, 0, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Em7/D", N, N, 0, 0, 0, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Emsus4", 0, 0, 2, 0, 0, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Em(sus4)", 0, 0, 2, 0, 0, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Emadd9", 0, 2, 4, 0, 0, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Em(add9)", 0, 2, 4, 0, 0, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);

            Chords.LearnChord("F", 1, 3, 3, 2, 1, 1, 1, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("F+", N, N, 3, 2, 2, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("F+7+11", 1, 3, 3, 2, 0, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("F4", N, N, 3, 3, 1, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("F6", N, 3, 3, 2, 3, N, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("F7", 1, 3, 1, 2, 1, 1, 1, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("F9", 2, 4, 2, 3, 2, 4, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("F11", 1, 3, 1, 3, 1, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Fsus", N, N, 3, 3, 1, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Fsus4", N, N, 3, 3, 1, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Fmaj", 1, 3, 3, 2, 1, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Fmaj7", N, 3, 3, 2, 1, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Fdim", N, N, 0, 1, 0, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Fm", 1, 3, 3, 1, 1, 1, 1, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("Fmin", 1, 3, 3, 1, 1, 1, 1, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("F/A", N, 0, 3, 2, 1, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("F/C", N, N, 3, 2, 1, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("F/D", N, N, 0, 2, 1, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("F/G", 3, 3, 3, 2, 1, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("F7/A", N, 0, 3, 0, 1, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Fmaj7/A", N, 0, 3, 2, 1, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Fmaj7/C", N, 3, 3, 2, 1, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Fmaj7(+5)", N, N, 3, 2, 2, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Fadd9", 3, 0, 3, 2, 1, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("F(add9)", 3, 0, 3, 2, 1, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("FaddG", 1, N, 3, 2, 1, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("FaddG", 1, N, 3, 2, 1, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);

            Chords.LearnChord("Fm6", N, N, 0, 1, 1, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Fm7", 1, 3, 1, 1, 1, 1, 1, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("Fmmaj7", N, 3, 3, 1, 1, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);

            Chords.LearnChord("F#", 2, 4, 4, 3, 2, 2, 1, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("F#+", N, N, 4, 3, 3, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("F#7", N, N, 4, 3, 2, 0, 1, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("F#9", N, 1, 2, 1, 2, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("F#11", 2, 4, 2, 4, 2, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("F#sus", N, N, 4, 4, 2, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("F#sus4", N, N, 4, 4, 2, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("F#maj", 2, 4, 4, 3, 2, 2, 0, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("F#maj7", N, N, 4, 3, 2, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("F#dim", N, N, 1, 2, 1, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("F#m", 2, 4, 4, 2, 2, 2, 1, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("F#min", 2, 4, 4, 2, 2, 2, 1, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("F#/E", 0, 4, 4, 3, 2, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("F#4", N, N, 4, 4, 2, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("F#m6", N, N, 1, 2, 2, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("F#m7", N, N, 2, 2, 2, 2, 1, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("F#m7-5", 1, 0, 2, 3, 3, 3, 2, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("F#m/C#m", N, N, 4, 2, 2, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);

            Chords.LearnChord("Gb", 2, 4, 4, 3, 2, 2, 1, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("Gb+", N, N, 4, 3, 3, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Gb7", N, N, 4, 3, 2, 0, 1, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("Gb9", N, 1, 2, 1, 2, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Gbsus", N, N, 4, 4, 2, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Gbsus4", N, N, 4, 4, 2, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Gbmaj", 2, 4, 4, 3, 2, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Gbmaj7", N, N, 4, 3, 2, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Gbdim", N, N, 1, 2, 1, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Gbm", 2, 4, 4, 2, 2, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Gbmin", 2, 4, 4, 2, 2, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Gbm7", N, N, 2, 2, 2, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);

            Chords.LearnChord("G", 3, 2, 0, 0, 0, 3, 1, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("G+", N, N, 1, 0, 0, 4, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("G4", N, N, 0, 0, 1, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("G6", 3, N, 0, 0, 0, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("G7", 3, 2, 0, 0, 0, 1, 1, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("G7+", N, N, 4, 3, 3, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("G7b9", N, N, 0, 1, 0, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("G7(b9)", N, N, 0, 1, 0, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("G7#9", 1, 3, N, 2, 4, 4, 3, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("G7(#9)", 1, 3, N, 2, 4, 4, 3, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("G9", 3, N, 0, 2, 0, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("G9(11)", 1, 3, 1, 3, 1, 3, 3, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("G11", 3, N, 0, 2, 1, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Gsus", N, N, 0, 0, 1, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Gsus4", N, N, 0, 0, 1, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("G6sus4", 0, 2, 0, 0, 1, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("G6(sus4)", 0, 2, 0, 0, 1, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("G7sus4", 3, 3, 0, 0, 1, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("G7(sus4)", 3, 3, 0, 0, 1, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Gmaj", 3, 2, 0, 0, 0, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Gmaj7", N, N, 4, 3, 2, 1, 2, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Gmaj7sus4", N, N, 0, 0, 1, 2, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Gmaj9", 1, 1, 4, 1, 2, 1, 2, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Gm", 1, 3, 3, 1, 1, 1, 3, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("Gmin", 1, 3, 3, 1, 1, 1, 3, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("Gdim", N, N, 2, 3, 2, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Gadd9", 1, 3, N, 2, 1, 3, 3, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("G(add9)", 1, 3, N, 2, 1, 3, 3, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("G/A", N, 0, 0, 0, 0, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("G/B", N, 2, 0, 0, 0, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("G/D", N, 2, 2, 1, 0, 0, 4, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("G/F#", 2, 2, 0, 0, 0, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);

            Chords.LearnChord("Gm6", N, N, 2, 3, 3, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("Gm7", 1, 3, 1, 1, 1, 1, 3, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("Gm/Bb", 3, 2, 2, 1, N, N, 4, ChordOrigin.BuildIn, Difficulty.Hard);

            Chords.LearnChord("G#", 1, 3, 3, 2, 1, 1, 4, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("G#+", N, N, 2, 1, 1, 0, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("G#4", 1, 3, 3, 1, 1, 1, 4, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("G#7", N, N, 1, 1, 1, 2, 1, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("G#sus", N, N, 1, 1, 2, 4, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("G#sus4", N, N, 1, 1, 2, 4, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("G#maj", 1, 3, 3, 2, 1, 1, 4, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("G#maj7", N, N, 1, 1, 1, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("G#dim", N, N, 0, 1, 0, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("G#m", 1, 3, 3, 1, 1, 1, 4, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("G#min", 1, 3, 3, 1, 1, 1, 4, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("G#m6", N, N, 1, 1, 0, 1, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("G#m7", N, N, 1, 1, 1, 1, 4, ChordOrigin.BuildIn, Difficulty.Easy);
            Chords.LearnChord("G#m9maj7", N, N, 1, 3, 0, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
            Chords.LearnChord("G#m9(maj7)", N, N, 1, 3, 0, 3, 1, ChordOrigin.BuildIn, Difficulty.Hard);
        }

        public CommonChordLibrary()
        {
            LearnBuildinChords();
        }
    }
}
