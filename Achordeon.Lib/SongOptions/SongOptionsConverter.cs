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
using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Achordeon.Common.Extensions;
using Achordeon.Common.Helpers;

namespace Achordeon.Lib.SongOptions
{
    public class SongOptionsConverter
    {
        private const string ABBREV_TWO_UP = "2";
        private const string ABBREV_FOUR_UP = "4";
        private const string ABBREV_DRAW_CHORD_GRIDS = "c";
        private const string ABBREV_CHORD_FONT = "cf";
        private const string ABBREV_CHORD_SIZE_PT = "cs";
        private const string ABBREV_EVEN_PAGE_NUMBER_LEFT = "e";
        private const string ABBREV_CHORD_GRID_SIZE_MM = "gs";
        private const string ABBREV_CHORD_GRID_SORTED = "g";
        private const string ABBREV_LYRICS_ONLY = "lo";
        private const string ABBREV_PAGE_NUMBER_LOGICAL = "pl";
        private const string ABBREV_PAGE_SIZE = "ps";
        private const string ABBREV_SINGLE_SPACE = "ss";
        private const string ABBREV_START_PAGE_NUMBER = "sp";
        private const string ABBREV_CREATE_TOC = "t";
        private const string ABBREV_TEXT_SIZE_PT = "ts";
        private const string ABBREV_TEXT_FONT = "tf";
        private const string ABBREV_VERTICAL_SPACE = "v";
        private const string ABBREV_TRANSPOSE_BY_HALFTONES = "x";
        private const string FALSE = "0";
        private const string TRUE = "1";

        private ISongOptions m_DefaultOptions;

        public ISongOptions DefaultSongOptions
        {
            get { return m_DefaultOptions; }
            set
            {
                value.ThrowIfNullEx(nameof(DefaultSongOptions));
                m_DefaultOptions = value;                
            }
        }

        public SongOptionsConverter(ISongOptions ADefaultOptions)
        {
            DefaultSongOptions = ADefaultOptions;
        }

        public string SaveToString(ISongOptions AOptions)
        {
            var Result = new StringBuilder();
            AddOption(Result, ABBREV_DRAW_CHORD_GRIDS, AOptions.DrawChordGrids, DefaultSongOptions.DrawChordGrids);
            AddOption(Result, ABBREV_TWO_UP, AOptions.TwoUp, DefaultSongOptions.TwoUp);
            AddOption(Result, ABBREV_FOUR_UP, AOptions.FourUp, DefaultSongOptions.FourUp);
            AddOption(Result, ABBREV_CHORD_FONT, AOptions.ChordFont, DefaultSongOptions.ChordFont);
            AddOption(Result, ABBREV_CHORD_SIZE_PT, AOptions.ChordSizePt, DefaultSongOptions.ChordSizePt);
            AddOption(Result, ABBREV_CHORD_GRID_SIZE_MM, AOptions.ChordGridSizeMm, DefaultSongOptions.ChordGridSizeMm);
            AddOption(Result, ABBREV_CHORD_GRID_SORTED, AOptions.ChordGridSorted, DefaultSongOptions.ChordGridSorted);
            AddOption(Result, ABBREV_EVEN_PAGE_NUMBER_LEFT, AOptions.EvenPageNumberLeft, DefaultSongOptions.EvenPageNumberLeft);
            AddOption(Result, ABBREV_LYRICS_ONLY, AOptions.LyricsOnly, DefaultSongOptions.LyricsOnly);
            AddOption(Result, ABBREV_PAGE_NUMBER_LOGICAL, AOptions.PageNumberLogical, DefaultSongOptions.PageNumberLogical);
            AddOption(Result, ABBREV_PAGE_SIZE, AOptions.PageSize, DefaultSongOptions.PageSize);
            AddOption(Result, ABBREV_SINGLE_SPACE, AOptions.SingleSpace, DefaultSongOptions.SingleSpace);
            AddOption(Result, ABBREV_START_PAGE_NUMBER, AOptions.StartPageNumber, DefaultSongOptions.StartPageNumber);
            AddOption(Result, ABBREV_TEXT_FONT, AOptions.TextFont, DefaultSongOptions.TextFont);
            AddOption(Result, ABBREV_TEXT_SIZE_PT, AOptions.TextSizePt, DefaultSongOptions.TextSizePt);
            AddOption(Result, ABBREV_CREATE_TOC, AOptions.CreateToc, DefaultSongOptions.CreateToc);
            AddOption(Result, ABBREV_TRANSPOSE_BY_HALFTONES, AOptions.TransposeByHalftones, DefaultSongOptions.TransposeByHalftones);
            AddOption(Result, ABBREV_VERTICAL_SPACE, AOptions.VerticalSpace, DefaultSongOptions.VerticalSpace);
            return Result.ToString();
        }

        public void LoadFromString(ISongOptions AOptions, string ADataString)
        {
            var Rex = new Regex(@"(?<name>\w+)\s*:\s*(?<value>[^;]*?)\s*;");
            var M = Rex.Match(ADataString);
            while (M.Success)
            {
                var Key = M.Groups["name"].Value.ToLower();
                var Value = M.Groups["value"].Value;
                switch (Key)
                {
                    case ABBREV_DRAW_CHORD_GRIDS:
                        AOptions.DrawChordGrids = Value != FALSE;
                        break;
                    case ABBREV_TWO_UP:
                        AOptions.TwoUp = Value != FALSE;
                        break;
                    case ABBREV_FOUR_UP:
                        AOptions.FourUp = Value != FALSE;
                        break;
                    case ABBREV_CHORD_FONT:
                        AOptions.ChordFont = Value;
                        break;
                    case ABBREV_CHORD_SIZE_PT:
                        AOptions.ChordSizePt = Convert.ToInt32(Value, CultureInfo.InvariantCulture);
                        break;
                    case ABBREV_EVEN_PAGE_NUMBER_LEFT:
                        AOptions.EvenPageNumberLeft = Value != FALSE;
                        break;
                    case ABBREV_CHORD_GRID_SIZE_MM:
                        AOptions.ChordGridSizeMm = Convert.ToInt32(Value, CultureInfo.InvariantCulture);
                        break;
                    case ABBREV_CHORD_GRID_SORTED:
                        AOptions.ChordGridSorted = Value != FALSE;
                        break;
                    case ABBREV_LYRICS_ONLY:
                        AOptions.LyricsOnly = Value != FALSE;
                        break;
                    case ABBREV_PAGE_NUMBER_LOGICAL:
                        AOptions.PageNumberLogical = Value != FALSE;
                        break;
                    case ABBREV_PAGE_SIZE:
                        AOptions.PageSize = Value;
                        break;
                    case ABBREV_SINGLE_SPACE:
                        AOptions.SingleSpace = Value != FALSE;
                        break;
                    case ABBREV_START_PAGE_NUMBER:
                        AOptions.StartPageNumber = Convert.ToInt32(Value, CultureInfo.InvariantCulture);
                        break;
                    case ABBREV_CREATE_TOC:
                        AOptions.CreateToc = Value != FALSE;
                        break;
                    case ABBREV_TEXT_SIZE_PT:
                        AOptions.TextSizePt = Convert.ToInt32(Value, CultureInfo.InvariantCulture);
                        break;
                    case ABBREV_TEXT_FONT:
                        AOptions.TextFont = Value;
                        break;
                    case ABBREV_VERTICAL_SPACE:
                        AOptions.VerticalSpace = Convert.ToInt32(Value, CultureInfo.InvariantCulture);
                        break;
                    case ABBREV_TRANSPOSE_BY_HALFTONES:
                        AOptions.TransposeByHalftones = Convert.ToInt32(Value, CultureInfo.InvariantCulture);
                        break;
                }
                M = M.NextMatch();
            }
        }


        public void LoadFromXml(ISongOptions AOptions, XmlFile AXml)
        {
            AOptions.TwoUp = AXml.GetB("TwoUp", DefaultSongOptions.TwoUp);
            AOptions.FourUp = AXml.GetB("FourUp", DefaultSongOptions.FourUp);
            AOptions.ChordFont = AXml.Get("ChordFont", DefaultSongOptions.ChordFont);
            AOptions.ChordSizePt = AXml.GetI("ChordSizePt", DefaultSongOptions.ChordSizePt);
            AOptions.ChordGridSizeMm = AXml.GetI("ChordGridSizeMm", DefaultSongOptions.ChordGridSizeMm);
            AOptions.ChordGridSorted = AXml.GetB("ChordGridSorted", DefaultSongOptions.ChordGridSorted);
            AOptions.EvenPageNumberLeft = AXml.GetB("EvenPageNumberLeft", DefaultSongOptions.EvenPageNumberLeft);
            AOptions.LyricsOnly = AXml.GetB("LyricsOnly", DefaultSongOptions.LyricsOnly);
            AOptions.PageNumberLogical = AXml.GetB("PageNumberLogical", DefaultSongOptions.PageNumberLogical);
            AOptions.PageSize = AXml.Get("PageSize", DefaultSongOptions.PageSize);
            AOptions.SingleSpace = AXml.GetB("SingleSpace", DefaultSongOptions.SingleSpace);
            AOptions.StartPageNumber = AXml.GetI("StartPageNumber", DefaultSongOptions.StartPageNumber);
            AOptions.TextFont = AXml.Get("TextFont", DefaultSongOptions.TextFont);
            AOptions.TextSizePt = AXml.GetI("TextSizePt", DefaultSongOptions.TextSizePt);
            AOptions.CreateToc = AXml.GetB("CreateToc", DefaultSongOptions.CreateToc);
            AOptions.TransposeByHalftones = AXml.GetI("TransposeByHalftones", DefaultSongOptions.TransposeByHalftones);
            AOptions.VerticalSpace = AXml.GetI("VerticalSpace", DefaultSongOptions.VerticalSpace);
            AOptions.DrawChordGrids = AXml.GetB("DrawChordGrids", DefaultSongOptions.DrawChordGrids);
        }
        

        public void SaveToXml(ISongOptions AOptions, XmlFile AXml)
        {
            XmlSet(AXml, "TwoUp", AOptions.TwoUp, DefaultSongOptions.TwoUp);
            XmlSet(AXml, "FourUp", AOptions.FourUp, DefaultSongOptions.FourUp);
            XmlSet(AXml, "ChordFont", AOptions.ChordFont, DefaultSongOptions.ChordFont);
            XmlSet(AXml, "ChordSizePt", AOptions.ChordSizePt, DefaultSongOptions.ChordSizePt);
            XmlSet(AXml, "ChordGridSizeMm", AOptions.ChordGridSizeMm, DefaultSongOptions.ChordGridSizeMm);
            XmlSet(AXml, "ChordGridSorted", AOptions.ChordGridSorted, DefaultSongOptions.ChordGridSorted);
            XmlSet(AXml, "EvenPageNumberLeft", AOptions.EvenPageNumberLeft, DefaultSongOptions.EvenPageNumberLeft);
            XmlSet(AXml, "LyricsOnly", AOptions.LyricsOnly, DefaultSongOptions.LyricsOnly);
            XmlSet(AXml, "PageNumberLogical", AOptions.PageNumberLogical, DefaultSongOptions.PageNumberLogical);
            XmlSet(AXml, "PageSize", AOptions.PageSize, DefaultSongOptions.PageSize);
            XmlSet(AXml, "SingleSpace", AOptions.SingleSpace, DefaultSongOptions.SingleSpace);
            XmlSet(AXml, "StartPageNumber", AOptions.StartPageNumber, DefaultSongOptions.StartPageNumber);
            XmlSet(AXml, "TextFont", AOptions.TextFont, DefaultSongOptions.TextFont);
            XmlSet(AXml, "TextSizePt", AOptions.TextSizePt, DefaultSongOptions.TextSizePt);
            XmlSet(AXml, "CreateToc", AOptions.CreateToc, DefaultSongOptions.CreateToc);
            XmlSet(AXml, "TransposeByHalftones", AOptions.TransposeByHalftones, DefaultSongOptions.TransposeByHalftones);
            XmlSet(AXml, "VerticalSpace", AOptions.VerticalSpace, DefaultSongOptions.VerticalSpace);
            XmlSet(AXml, "DrawChordGrids", AOptions.DrawChordGrids, DefaultSongOptions.DrawChordGrids);
        }


        public bool IsAllDefault(ISongOptions AOptions)
        {
            return string.IsNullOrWhiteSpace(SaveToString(AOptions));
        }

        private void AddOption(StringBuilder ABuilder, string AAbbreviation, bool AValue, bool ADefault)
        {
            if (AValue == ADefault)
                return;
            ABuilder.Append(AAbbreviation);
            ABuilder.Append(':');
            ABuilder.Append(AValue ? TRUE : FALSE);
            ABuilder.Append(';');
        }

        private void AddOption(StringBuilder ABuilder, string AAbbreviation, int AValue, int ADefault)
        {
            if (AValue == ADefault)
                return;
            ABuilder.Append(AAbbreviation);
            ABuilder.Append(':');
            ABuilder.Append(AValue);
            ABuilder.Append(';');
        }

        private void AddOption(StringBuilder ABuilder, string AAbbreviation, string AValue, string ADefault)
        {
            if (String.IsNullOrWhiteSpace(AValue))
                throw new Exception($"malformed string >>{AValue}<< for {AAbbreviation}: must not be empty");
            if (AValue.CiEquals(ADefault))
                return;
            ABuilder.Append(AAbbreviation);
            ABuilder.Append(':');
            if (AValue.Contains(";"))
                throw new Exception($"malformed string >>{AValue}<<: for {AAbbreviation}: must not contain semicolon (;)");
            ABuilder.Append(AValue);
            ABuilder.Append(';');
        }

        private void XmlSet(XmlFile AXml, string AName, int AValue, int ADefault)
        {
            if (AValue.Equals(ADefault))
                return;
            AXml.Set(AName, AValue);
        }

        private void XmlSet(XmlFile AXml, string AName, bool AValue, bool ADefault)
        {
            if (AValue.Equals(ADefault))
                return;
            AXml.Set(AName, AValue);
        }

        private void XmlSet(XmlFile AXml, string AName, string AValue, string ADefault)
        {
            if (AValue.Equals(ADefault))
                return;
            AXml.Set(AName, AValue);
        }
    }
}
