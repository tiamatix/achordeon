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

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Achordeon.Lib.SongOptions
{
    public class DefaultSongOptions : ISongOptions
    {
        public static readonly DefaultSongOptions Default = new DefaultSongOptions();

        public const bool DEFAULT_DRAW_CHORD_GRIDS = true;
        public const bool DEFAULT_TWO_UP = false;
        public const bool DEFAULT_FOUR_UP = false;
        public const string DEFAULT_CHORD_FONT = "Times New Roman";
        public const int DEFAULT_CHORD_SIZE_PT = 10;
        public const int DEFAULT_CHORD_GRID_SIZE_MM = 20;
        public const bool DEFAULT_CHORD_GRID_SORTED = false;
        public const bool DEFAULT_EVEN_PAGE_NUMBER_LEFT = false;
        public const bool DEFAULT_LYRICS_ONLY = false;
        public const bool DEFAULT_PAGE_NUMBER_LOGICAL = false;
        public const string DEFAULT_PAGE_SIZE = "A4";
        public const string DEFAULT_TEXT_FONT = "Times New Roman";
        public const int DEFAULT_START_PAGE_NUMBER = 1;
        public const bool DEFAULT_SINGLE_SPACE = false;
        public const int DEFAULT_TEXT_SIZE_PT = 10;
        public const bool DEFAULT_CREATE_TOC = false;
        public const int DEFAULT_TRANSPOSE_BY_HALFTONES = 0;
        public const int DEFAULT_VERTICAL_SPACE = 0;

        public bool DrawChordGrids
        {
            get {return DEFAULT_DRAW_CHORD_GRIDS; }
            set {}
        }

        public bool TwoUp
        {
            get { return DEFAULT_TWO_UP; }
            set { }
        }

        public bool FourUp
        {
            get { return DEFAULT_FOUR_UP; }
            set { }
        }

        public string ChordFont
        {
            get { return DEFAULT_CHORD_FONT; }
            set { }
        }

        public int ChordSizePt
        {
            get { return DEFAULT_CHORD_SIZE_PT; }
            set { }
        }

        public int ChordGridSizeMm
        {
            get { return DEFAULT_CHORD_GRID_SIZE_MM; }
            set { }
        }

        public bool ChordGridSorted
        {
            get { return DEFAULT_CHORD_GRID_SORTED; }
            set { }
        }

        public bool EvenPageNumberLeft
        {
            get { return DEFAULT_EVEN_PAGE_NUMBER_LEFT; }
            set { }
        }

        public bool LyricsOnly
        {
            get { return DEFAULT_LYRICS_ONLY; }
            set { }
        }

        public bool PageNumberLogical
        {
            get { return DEFAULT_PAGE_NUMBER_LOGICAL; }
            set { }
        }

        public string PageSize
        {
            get { return DEFAULT_PAGE_SIZE; }
            set { }
        }

        public bool SingleSpace
        {
            get { return DEFAULT_SINGLE_SPACE; }
            set { }
        }

        public int StartPageNumber
        {
            get { return DEFAULT_START_PAGE_NUMBER; }
            set { }
        }

        public string TextFont
        {
            get { return DEFAULT_TEXT_FONT; }
            set { }
        }

        public int TextSizePt
        {
            get { return DEFAULT_TEXT_SIZE_PT; }
            set { }
        }

        public bool CreateToc
        {
            get { return DEFAULT_CREATE_TOC; }
            set { }
        }

        public int TransposeByHalftones
        {
            get { return DEFAULT_TRANSPOSE_BY_HALFTONES; }
            set { }
        }

        public int VerticalSpace
        {
            get { return DEFAULT_VERTICAL_SPACE; }
            set { }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string APropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(APropertyName));
        }
    }
}
