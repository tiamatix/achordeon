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
using Achordeon.Lib.Properties;
using Achordeon.Lib.Transposing;
using DryIoc;
using DryIoc.Experimental;

namespace Achordeon.Lib.Chords
{
    public static class CommonConstants
    {
        public const int UNUSED_FRET = -1;
    }

    public class Chord
    {
        public Chord(
            string AName,
            int AFret1,
            int AFret2,
            int AFret3,
            int AFret4,
            int AFret5,
            int AFret6,
            int ABaseFret,
            ChordOrigin AOrigin,
            Difficulty ADifficulty)
        {
            Name = AName;
            Fret1 = AFret1;
            Fret2 = AFret2;
            Fret3 = AFret3;
            Fret4 = AFret4;
            Fret5 = AFret5;
            Fret6 = AFret6;
            BaseFret = ABaseFret;
            Origin = AOrigin;
            Difficulty = ADifficulty;
        }

        public Chord Clone()
        {
            var res = new Chord(Name,
                Fret1,
                Fret2,
                Fret3,
                Fret4,
                Fret5,
                Fret6,
                BaseFret,
                Origin,
                Difficulty);
            return res;
        }

        public int MaxFinger => Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(Fret1, Fret2), Fret3), Fret4), Fret5), Fret6);

        public int MaxFret => BaseFret + MaxFinger;

        public Chord Transpose(Container AIoC, int AHalfToneSteps)
        {
            if (Origin != ChordOrigin.Defined)
                throw new TransposeException(Resources.Chord_Transpose_Can_only_transpose_defined_chords);
            var NewName = AIoC.Get<Transposer>().TransposeChord(Name, AHalfToneSteps);
            var NewBaseFret = BaseFret + AHalfToneSteps;
            if (NewBaseFret < 0 || NewBaseFret + MaxFinger > 24)
                throw new TransposeException(Resources.Chord_Transpose_Resulting_chord_would_exceed_the_fretboard);
            return new Chord(NewName,
                Fret1,
                Fret2,
                Fret3,
                Fret4,
                Fret5,
                Fret6,
                NewBaseFret,
                ChordOrigin.Defined,
                Difficulty.Hard);
        }

        public string Name { get; }
        public int BaseFret { get; }
        public int Fret1 { get; }
        public int Fret2 { get; }
        public int Fret3 { get; }
        public int Fret4 { get; }
        public int Fret5 { get; }
        public int Fret6 { get; }
        public ChordOrigin Origin { get; }
        public Difficulty Difficulty { get; }

        public int[] GetAbsoluteFrets(int ANumberOfStrings)
        {
            var res = new List<int>()
            {
                Fret1 == CommonConstants.UNUSED_FRET ? Fret1 : (BaseFret + Fret1) - 1,
                Fret2 == CommonConstants.UNUSED_FRET ? Fret2 : (BaseFret + Fret2) - 1,
                Fret3 == CommonConstants.UNUSED_FRET ? Fret3 : (BaseFret + Fret3) - 1,
                Fret4 == CommonConstants.UNUSED_FRET ? Fret4 : (BaseFret + Fret4) - 1,
                Fret5 == CommonConstants.UNUSED_FRET ? Fret5 : (BaseFret + Fret5) - 1,
                Fret6 == CommonConstants.UNUSED_FRET ? Fret6 : (BaseFret + Fret6) - 1,
            };
            while (res.Count < ANumberOfStrings)
                res.Add(CommonConstants.UNUSED_FRET);
            return res.ToArray();
        }

        public string GetFretDisplay(int AFret)
        {
            return AFret == CommonConstants.UNUSED_FRET ? "-" : AFret.ToString();
        }

        public override string ToString()
        {
            return $"{Name}: base-fret {BaseFret} frets {GetFretDisplay(Fret1)}  {GetFretDisplay(Fret2)}  {GetFretDisplay(Fret3)}  {GetFretDisplay(Fret4)}  {GetFretDisplay(Fret5)}  {GetFretDisplay(Fret6)}";
        }
    }
}
