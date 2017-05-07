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

using System.Diagnostics;
using System.Windows;
using Achordeon.Common.Extensions;
using Achordeon.Common.Helpers;
using Achordeon.Lib.SongOptions;

namespace Achordeon.Shell.Wpf.Contents
{
    public class LinkedSongOptionsViewModel : SongOptionsViewModel
    {
        private readonly ISongOptions m_LinkedOptions;
        private bool? m_TwoUp;
        private bool? m_FourUp;
        private string m_ChordFont;
        private int? m_ChordSizePt;
        private int? m_ChordGridSizeMm;
        private bool? m_ChordGridSorted;
        private bool? m_EvenPageNumberLeft;
        private bool? m_LyricsOnly;
        private bool? m_PageNumberLogical;
        private string m_PageSize;
        private bool? m_SingleSpace;
        private int? m_StartPageNumber;
        private string m_TextFont;
        private int? m_TextSizePt;
        private bool? m_CreateToc;
        private int? m_TransposeByHalftones;
        private int? m_VerticalSpace;
        private bool? m_DrawChordGrids;

        public LinkedSongOptionsViewModel(CoreViewModel ACore, ISongOptions ALinkedOptions) : base(ACore, ALinkedOptions)
        {
            ALinkedOptions.ThrowIfNullEx(nameof(ALinkedOptions));
            m_LinkedOptions = ALinkedOptions;
            Reset();
        }

        protected override void LoadFrom(ISongOptions AOtherOptions)
        {            
        }

        public void Reset()
        {
            m_TwoUp = null;
            m_FourUp = null;
            m_ChordFont = null;
            m_ChordSizePt = null;
            m_ChordGridSizeMm = null;
            m_ChordGridSorted = null;
            m_EvenPageNumberLeft = null;
            m_LyricsOnly = null;
            m_PageNumberLogical = null;
            m_PageSize = null;
            m_SingleSpace = null;
            m_StartPageNumber = null;
            m_TextFont = null;
            m_TextSizePt = null;
            m_CreateToc = null;
            m_TransposeByHalftones = null;
            m_VerticalSpace = null;
            m_DrawChordGrids = null;
        }

        public override SongOptionsViewModel Clone()
        {
            var res = new LinkedSongOptionsViewModel(CoreViewModel, m_LinkedOptions);
            res.m_TwoUp = m_TwoUp;
            res.m_FourUp = m_FourUp;
            res.m_ChordFont = m_ChordFont;
            res.m_ChordSizePt = m_ChordSizePt;
            res.m_ChordGridSizeMm = m_ChordGridSizeMm;
            res.m_ChordGridSorted = m_ChordGridSorted;
            res.m_EvenPageNumberLeft = m_EvenPageNumberLeft;
            res.m_LyricsOnly = m_LyricsOnly;
            res.m_PageNumberLogical = m_PageNumberLogical;
            res.m_PageSize = m_PageSize;
            res.m_SingleSpace = m_SingleSpace;
            res.m_StartPageNumber = m_StartPageNumber;
            res.m_TextFont = m_TextFont;
            res.m_TextSizePt = m_TextSizePt;
            res.m_CreateToc = m_CreateToc;
            res.m_TransposeByHalftones = m_TransposeByHalftones;
            res.m_VerticalSpace = m_VerticalSpace;
            res.m_DrawChordGrids = m_DrawChordGrids;
            return res;
        }


        public override bool DrawChordGrids
        {
            get { return m_DrawChordGrids ?? m_LinkedOptions.DrawChordGrids; }
            set { SetProperty(ref m_DrawChordGrids, value, nameof(DrawChordGrids)); }
        }

        public override bool TwoUp
        {
            get { return m_TwoUp ?? m_LinkedOptions.TwoUp; }
            set { SetProperty(ref m_TwoUp, value, nameof(TwoUp)); }
        }


        public override bool FourUp
        {
            get { return m_FourUp ?? m_LinkedOptions.FourUp; }
            set { SetProperty(ref m_FourUp, value, nameof(FourUp)); }
        }


        public override string ChordFont
        {
            get { return m_ChordFont ?? m_LinkedOptions.ChordFont; }
            set { SetProperty(ref m_ChordFont, value, nameof(ChordFont)); }
        }

        public override int ChordSizePt
        {
            get { return m_ChordSizePt ?? m_LinkedOptions.ChordSizePt; }
            set { SetProperty(ref m_ChordSizePt, value, nameof(ChordSizePt)); }
        }


        public override int ChordGridSizeMm
        {
            get { return m_ChordGridSizeMm ?? m_LinkedOptions.ChordGridSizeMm; }
            set { SetProperty(ref m_ChordGridSizeMm, value, nameof(ChordGridSizeMm)); }
        }


        public override  bool ChordGridSorted
        {
            get { return m_ChordGridSorted ?? m_LinkedOptions.ChordGridSorted; }
            set { SetProperty(ref m_ChordGridSorted, value, nameof(ChordGridSorted)); }
        }


        public override bool EvenPageNumberLeft
        {
            get { return m_EvenPageNumberLeft ?? m_LinkedOptions.EvenPageNumberLeft; }
            set { SetProperty(ref m_EvenPageNumberLeft, value, nameof(EvenPageNumberLeft)); }
        }


        public override bool LyricsOnly
        {
            get { return m_LyricsOnly ?? m_LinkedOptions.LyricsOnly; }
            set { SetProperty(ref m_LyricsOnly, value, nameof(LyricsOnly)); }
        }



        public override bool PageNumberLogical
        {
            get { return m_PageNumberLogical ?? m_LinkedOptions.PageNumberLogical; }
            set { SetProperty(ref m_PageNumberLogical, value, nameof(PageNumberLogical)); }
        }



        public override string PageSize
        {
            get { return m_PageSize ?? m_LinkedOptions.PageSize; }
            set { SetProperty(ref m_PageSize, value, nameof(PageSize)); }
        }


        public override bool SingleSpace
        {
            get { return m_SingleSpace ?? m_LinkedOptions.SingleSpace; }
            set { SetProperty(ref m_SingleSpace, value, nameof(SingleSpace)); }
        }



        public override int StartPageNumber
        {
            get { return m_StartPageNumber ?? m_LinkedOptions.StartPageNumber; }
            set { SetProperty(ref m_StartPageNumber, value, nameof(StartPageNumber)); }
        }



        public override string TextFont
        {
            get { return m_TextFont ?? m_LinkedOptions.TextFont; }
            set { SetProperty(ref m_TextFont, value, nameof(TextFont)); }
        }


        public override int TextSizePt
        {
            get { return m_TextSizePt ?? m_LinkedOptions.TextSizePt; }
            set { SetProperty(ref m_TextSizePt, value, nameof(TextSizePt)); }
        }


        public override bool CreateToc
        {
            get { return m_CreateToc ?? m_LinkedOptions.CreateToc; }
            set { SetProperty(ref m_CreateToc, value, nameof(CreateToc)); }
        }


        public override int TransposeByHalftones
        {
            get { return m_TransposeByHalftones ?? m_LinkedOptions.TransposeByHalftones; }
            set { SetProperty(ref m_TransposeByHalftones, value, nameof(TransposeByHalftones)); }
        }


        public override int VerticalSpace
        {
            get { return m_VerticalSpace ?? m_LinkedOptions.VerticalSpace; }
            set { SetProperty(ref m_VerticalSpace, value, nameof(VerticalSpace)); }
        }


    }
}
