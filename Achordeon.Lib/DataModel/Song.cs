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
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using Achordeon.Lib.Chords;
using Achordeon.Lib.Transposing;
using DryIoc;
using DryIoc.Experimental;

namespace Achordeon.Lib.DataModel
{
    public class Song : LineRange
    {
        private const string DEFAULT_TITLE = "";
        private const string DEFAULT_TEXT_FONT = "Times New Roman";
        private const string DEFAULT_CHORD_FONT = "Helvetica";
        private const double DEFAULT_TEXT_SIZE = 12.0;
        private const double DEFAULT_CHORD_SIZE = 12.0;
        private const bool DEFAULT_NO_GRID = false;
        private const string DEFAULT_CHORD_COLOR = "Black";
        private const string DEFAULT_TITLES = "Center";
        private const string DEFAULT_PAGE_TYPE = "A4";
        private const int DEFAULT_COLUMNS = 1;

        private readonly List<string> m_Subtitles = new List<string>();
        private readonly ChordList m_OriginalDefinedChords = new ChordList();
        private readonly ChordList m_TransposedDefinedChords = new ChordList();

        public override bool AcceptNonPlaintext => true;

        public ChordList UsedChords { get; private set; } = new ChordList();

        public IEnumerable<string> Subtitles => m_Subtitles;

        public string Title { get; set; } = DEFAULT_TITLE;
        public string TextFont { get; set; } = DEFAULT_TEXT_FONT;
        public string ChordFont { get; set; } = DEFAULT_CHORD_FONT;
        public double TextSize { get; set; } = DEFAULT_TEXT_SIZE;
        public double ChordSize { get; set; } = DEFAULT_CHORD_SIZE;
        public bool NoGrid { get; set; } = DEFAULT_NO_GRID;
        public string ChordColor  { get; set; } = DEFAULT_CHORD_COLOR;
        public string Titles { get; set; } = DEFAULT_TITLES;
        public string PageType { get; set; } = DEFAULT_PAGE_TYPE;
        public int Columns { get; set; } = DEFAULT_COLUMNS;
        public int TransposedByHalftoneSteps { get; private set; } = 0;

        public override void Transpose(int AHalfToneSteps)
        {
            TransposedByHalftoneSteps += AHalfToneSteps;
            m_Lines.ForEach(a => a.Transpose(AHalfToneSteps));
            m_TransposedDefinedChords.Clear();
            foreach (var OriginalChord in m_OriginalDefinedChords)
            {
                try
                {
                    m_TransposedDefinedChords.AddChord(OriginalChord.Transpose(IoC, AHalfToneSteps));
                }
                catch (TransposeException ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            RebuildBuildChordList();
        }

        public void DefineChord(string AChordDefinition, ChordDefinitionFormat AFormat = ChordDefinitionFormat.Default)
        {
            var C = m_OriginalDefinedChords.DefineChord(AChordDefinition, AFormat);
            m_TransposedDefinedChords.AddChord(C.Clone());
        }

        public void AddSubtitle(string ASubtitle)
        {
            m_Subtitles.Add(ASubtitle);
        }

        public void RebuildBuildChordList()
        {
            var CommonChords = IoC.Get<CommonChordLibrary>();
            UsedChords = new ChordList();
            var ChordsUsed = Subobjects.OfType<LineChord>().Select(a => a.Name).Distinct();
            foreach (var LineChord in ChordsUsed)
            {
                if (!UsedChords.ContainsCord(LineChord))
                {
                    ChordList FirstChoiceList, SecondChoiceList;
                    if (TransposedByHalftoneSteps == 0)
                    {
                        //If the song was not transposed, chord definitions superseed the library
                        FirstChoiceList = m_OriginalDefinedChords;
                        SecondChoiceList = CommonChords.Chords;
                    }
                    else
                    {
                        //If the song was transposed, we try to find an easy chord from the lib.
                        //If that fails, we use the auto-transposed chord definition instead.
                        FirstChoiceList = CommonChords.Chords;
                        SecondChoiceList = m_TransposedDefinedChords;
                    }
                    var Chord = FirstChoiceList.GetChordOrNull(LineChord) ?? SecondChoiceList.GetChordOrNull(LineChord);
                    if (Chord != null)
                        UsedChords.AddChord(Chord.Clone());
                }
            }
        }

        public Song(Container AIoC, int ALineNo, int ALinePos) : base(AIoC, ALineNo, ALinePos)
        {
            
        }

        public override void PrintChordPro(StringBuilder ABuilder)
        {
            const double EPSILON  = Single.Epsilon*10;
            if (Title != DEFAULT_TITLE)
                ABuilder.AppendLine("{title:" + Title + "}");
            foreach (var Subtitle in Subtitles)
                ABuilder.AppendLine("{subtitle:" + Subtitle + "}");
            if (TextFont != DEFAULT_TEXT_FONT)
                ABuilder.AppendLine("{textfont:" + TextFont + "}");
            if (Math.Abs(TextSize - DEFAULT_TEXT_SIZE) > EPSILON)
                ABuilder.AppendLine("{textsize:" + TextSize.ToString(CultureInfo.InvariantCulture) + "}");
            if (ChordFont != DEFAULT_CHORD_FONT)
                ABuilder.AppendLine("{chordfont:" + ChordFont + "}");
            if (Math.Abs(ChordSize - DEFAULT_CHORD_SIZE) > EPSILON)
                ABuilder.AppendLine("{chordsize:" + ChordSize.ToString(CultureInfo.InvariantCulture) + "}");
            if (ChordColor != DEFAULT_CHORD_COLOR)
                ABuilder.AppendLine("{chordcolour:" + ChordColor + "}");
            if (NoGrid != DEFAULT_NO_GRID)
                ABuilder.AppendLine(NoGrid ? "{no_grid}" : "{grid}");
            if (Titles != DEFAULT_TITLES)
                ABuilder.AppendLine("{titles:" + Titles + "}");
            if (Columns != DEFAULT_COLUMNS)
                ABuilder.AppendLine("{columns:" + Columns.ToString() + "}");
            if (PageType != DEFAULT_PAGE_TYPE)
                ABuilder.AppendLine("{pagetype:" + PageType + "}");
            foreach (var Chord in UsedChords)
            {
                if (Chord.Origin == ChordOrigin.Defined)
                    ABuilder.AppendLine("{define " + Chord.ToString() + "}");
            }
            base.PrintChordPro(ABuilder);
        }
    }
}
