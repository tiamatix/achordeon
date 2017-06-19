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

namespace Achordeon.Lib.SongOptions
{    
    public interface ISongOptions : INotifyPropertyChanged
    {
        bool DrawChordGrids { get; set; }
        bool TwoUp { get; set; }
        bool FourUp { get; set; }
        string ChordFont { get; set; }
        int ChordSizePt { get; set; }
        int ChordGridSizeMm { get; set; }
        bool ChordGridSorted { get; set; }
        bool EvenPageNumberLeft { get; set; }
        bool LyricsOnly { get; set; }
        bool PageNumberLogical { get; set; }
        string PageSize { get; set; }
        bool SingleSpace { get; set; }
        int StartPageNumber { get; set; }
        string TextFont { get; set; }
        int TextSizePt { get; set; }
        bool CreateToc { get; set; }
        int TransposeByHalftones { get; set; }
        int VerticalSpace { get; set; }
        bool UseMusicalSymbols { get; set; }
    }
}
