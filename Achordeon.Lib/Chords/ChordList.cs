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
using System.Text.RegularExpressions;

namespace Achordeon.Lib.Chords
{
    public class ChordList : IEnumerable<Chord>
    {
        private static readonly Regex _DefineRexNewFormat = new Regex(@"\s*(?<name>.+)\s*:\s*base-fret\s+(?<basefret>\d+)\s+frets\s*(?<f1>[-x0-9])\s*(?<f2>[-x0-9])\s*(?<f3>[-x0-9])\s*(?<f4>[-x0-9])\s*(?<f5>[-x0-9])\s*(?<f6>[-x0-9])", RegexOptions.IgnoreCase);
        private static readonly Regex _DefineRexOldFormat = new Regex(@"\s*(?<name>.+)\s+(?<basefret>\d+)\s+(?<f6>[-x0-9])\s*(?<f5>[-x0-9])\s*(?<f4>[-x0-9])\s*(?<f3>[-x0-9])\s*(?<f2>[-x0-9])\s*(?<f1>[-x0-9])", RegexOptions.IgnoreCase);

        private readonly List<Chord> m_Chords = new List<Chord>();
        private bool m_StatsDirty = true;
        private int m_BuildinCount;
        private int m_HardCount;
        private int m_EasyCount;
        private int m_DefinedCount;
        private int m_RcCount;

        public int Count => m_Chords.Count;

        public ChordList Clone()
        {
            var res = new ChordList();
            foreach (var c in this)
                res.AddChord(c.Clone());
            return res;
        }

        public int BuildinCount
        {
            get
            {
                CalcStats();
                return m_BuildinCount;
            }
        }

        public int HardCount
        {
            get
            {
                CalcStats();
                return m_HardCount;
            }
        }

        public int EasyCount
        {
            get
            {
                CalcStats();
                return m_EasyCount;
            }
        }

        public int DefinedCount
        {
            get
            {
                CalcStats();
                return m_DefinedCount;
            }
        }

        public int RcCount
        {
            get
            {
                CalcStats();
                return m_RcCount;
            }
        }

        private void ClearStats()
        {
            m_StatsDirty = true;
            m_HardCount = 0;
            m_DefinedCount = 0;
            m_RcCount = 0;
        }

        private void CalcStats()
        {
            if (!m_StatsDirty)
                return;
            ClearStats();
            foreach (var c in this)
            {
                if (c.Difficulty == Difficulty.Hard)
                    m_HardCount++;
                else
                    m_EasyCount++;
                switch (c.Origin)
                {
                    case ChordOrigin.BuildIn:
                        m_BuildinCount++;
                        break;
                    case ChordOrigin.Defined:
                        m_DefinedCount++;
                        break;
                    case ChordOrigin.Library:
                        m_RcCount++;
                        break;
                }
            }
            m_StatsDirty = false;
        }

        public void Clear()
        {
            ClearStats();
            m_Chords.Clear();
        }

        public Chord GetChordOrNull(string AChordName)
        {
            var cc = new ChordComparer();
            return m_Chords.Find(a => cc.Compare(a.Name, AChordName) == 0);
        }

        public void RemoveAll(Predicate<Chord> AMatch)
        {
            if (m_Chords.RemoveAll(AMatch) > 0)
                ClearStats();
        }

        public bool ContainsCord(string AChordName)
        {
            return GetChordOrNull(AChordName) != null;
        }

        public ChordList Sort()
        {
            var res = Clone();
            var cc = new ChordComparer();
            res.m_Chords.Sort((a, b) => cc.Compare(a.Name, b.Name));
            return res;
        }

        public void AddChord(Chord AChord)
        {
            if (ContainsCord(AChord.Name))
                return;
            ClearStats();
            m_Chords.Add(AChord);
        }

        public void RemoveChord(string AChordName)
        {
            var ExistingChord = GetChordOrNull(AChordName);
            if (ExistingChord != null)
            {
                ClearStats();
                m_Chords.Remove(ExistingChord);
            }
        }

        public void AddOrOverrideChord(Chord AChord)
        {
            RemoveChord(AChord.Name);
            AddChord(AChord);
        }

        private int ConvFret(string AValue)
        {
            if (AValue == "x" || AValue == "X" || AValue == "-")
                return Chord.UNUSED_FRET;
            return Convert.ToInt32(AValue);
        }

        public Chord DefineChord(string ADefinition, ChordDefinitionFormat AFormat = ChordDefinitionFormat.Default)
        {
            var Rex = AFormat == ChordDefinitionFormat.Default ? _DefineRexNewFormat : _DefineRexOldFormat;
            return DefineChord(Rex, ADefinition);
        }

        private Chord DefineChord(Regex ARegex, string ADefinition)
        {
            var M = ARegex.Match(ADefinition);
            if (!M.Success)
                throw new Exception($"'{ADefinition}' is not a valid definition");
            var Chord = new Chord(
                M.Groups["name"].Value,
                ConvFret(M.Groups["f1"].Value),
                ConvFret(M.Groups["f2"].Value),
                ConvFret(M.Groups["f3"].Value),
                ConvFret(M.Groups["f4"].Value),
                ConvFret(M.Groups["f5"].Value),
                ConvFret(M.Groups["f6"].Value),
                Convert.ToInt32(M.Groups["basefret"].Value),
                ChordOrigin.Defined,
                Difficulty.Hard);
            AddOrOverrideChord(Chord);
            return Chord;
        }

        public void LearnChord(
            string AChordName,
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
            if (ABaseFret == 0)
                ABaseFret = 1;
            var NewChord = new Chord(AChordName,
                AFret1,
                AFret2,
                AFret3,
                AFret4,
                AFret5,
                AFret6,
                ABaseFret,
                AOrigin,
                ADifficulty);
            AddChord(NewChord);
        }

        public IEnumerator<Chord> GetEnumerator()
        {
            return m_Chords.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
