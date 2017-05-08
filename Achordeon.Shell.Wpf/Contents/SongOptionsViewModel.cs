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
using Achordeon.Lib.SongOptions;

namespace Achordeon.Shell.Wpf.Contents
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class SongOptionsViewModel : ViewModelBase, ISongOptions
    {
        private CoreViewModel m_Core;
        private bool m_TwoUp;
        private bool m_FourUp;
        private string m_ChordFont;
        private int m_ChordSizePt;
        private int m_ChordGridSizeMm;
        private bool m_ChordGridSorted;
        private bool m_EvenPageNumberLeft;
        private bool m_LyricsOnly;
        private bool m_PageNumberLogical;
        private string m_PageSize;
        private bool m_SingleSpace;
        private int m_StartPageNumber;
        private string m_TextFont;
        private int m_TextSizePt;
        private bool m_CreateToc;
        private int m_TransposeByHalftones;
        private int m_VerticalSpace;
        private bool m_DrawChordGrids;

        public SongOptionsViewModel(CoreViewModel ACore) : this(ACore, DefaultSongOptions.Default)
        {
            CoreViewModel = ACore;
        }

        public void ResetToDefaults()
        {
            LoadFrom(DefaultSongOptions.Default);
        }

        public SongOptionsViewModel(CoreViewModel ACore, ISongOptions AOtherOptions)
        {
            CoreViewModel = ACore;
            LoadFrom(AOtherOptions);
        }

        protected virtual void LoadFrom(ISongOptions AOtherOptions)
        {
            TwoUp = AOtherOptions.TwoUp;
            FourUp = AOtherOptions.FourUp;
            ChordFont = AOtherOptions.ChordFont;
            ChordSizePt = AOtherOptions.ChordSizePt;
            ChordGridSizeMm = AOtherOptions.ChordGridSizeMm;
            ChordGridSorted = AOtherOptions.ChordGridSorted;
            EvenPageNumberLeft = AOtherOptions.EvenPageNumberLeft;
            LyricsOnly = AOtherOptions.LyricsOnly;
            PageNumberLogical = AOtherOptions.PageNumberLogical;
            PageSize = AOtherOptions.PageSize;
            SingleSpace = AOtherOptions.SingleSpace;
            StartPageNumber = AOtherOptions.StartPageNumber;
            TextFont = AOtherOptions.TextFont;
            TextSizePt = AOtherOptions.TextSizePt;
            CreateToc = AOtherOptions.CreateToc;
            TransposeByHalftones = AOtherOptions.TransposeByHalftones;
            VerticalSpace = AOtherOptions.VerticalSpace;
            DrawChordGrids = AOtherOptions.DrawChordGrids;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay
        {
            get
            {
                var Converter = new SongOptionsConverter(DefaultSongOptions.Default);
                if (Converter.IsAllDefault(this))
                    return $"{GetType().Name}, all default";
                return $"{GetType().Name}, {Converter.SaveToString(this)}";
            }
        }

        public virtual SongOptionsViewModel Clone()
        {
            return new SongOptionsViewModel(CoreViewModel, this);
        }

        public LinkedSongOptionsViewModel AsLinkedOptions()
        {
            return new LinkedSongOptionsViewModel(CoreViewModel, this);
        }

        public CoreViewModel CoreViewModel
        {
            get { return m_Core; }
            set { SetProperty(ref m_Core, value, nameof(CoreViewModel)); }
        }

        public virtual bool DrawChordGrids
        {
            get { return m_DrawChordGrids; }
            set { SetProperty(ref m_DrawChordGrids, value, nameof(DrawChordGrids)); }
        }

        public virtual bool TwoUp
        {
            get { return m_TwoUp; }
            set { SetProperty(ref m_TwoUp, value, nameof(TwoUp)); }
        }


        public virtual bool FourUp
        {
            get { return m_FourUp; }
            set { SetProperty(ref m_FourUp, value, nameof(FourUp)); }
        }


        public  virtual string ChordFont
        {
            get { return m_ChordFont; }
            set { SetProperty(ref m_ChordFont, value, nameof(ChordFont)); }
        }

        public virtual int ChordSizePt
        {
            get { return m_ChordSizePt; }
            set { SetProperty(ref m_ChordSizePt, value, nameof(ChordSizePt)); }
        }

        public virtual int ChordGridSizeMm
        {
            get { return m_ChordGridSizeMm; }
            set { SetProperty(ref m_ChordGridSizeMm, value, nameof(ChordGridSizeMm)); }
        }

        public virtual bool ChordGridSorted
        {
            get { return m_ChordGridSorted; }
            set { SetProperty(ref m_ChordGridSorted, value, nameof(ChordGridSorted)); }
        }


        public virtual bool EvenPageNumberLeft
        {
            get { return m_EvenPageNumberLeft; }
            set { SetProperty(ref m_EvenPageNumberLeft, value, nameof(EvenPageNumberLeft)); }
        }


        public virtual bool LyricsOnly
        {
            get { return m_LyricsOnly; }
            set { SetProperty(ref m_LyricsOnly, value, nameof(LyricsOnly)); }
        }

        public virtual bool PageNumberLogical
        {
            get { return m_PageNumberLogical; }
            set { SetProperty(ref m_PageNumberLogical, value, nameof(PageNumberLogical)); }
        }



        public virtual string PageSize
        {
            get { return m_PageSize; }
            set { SetProperty(ref m_PageSize, value, nameof(PageSize)); }
        }


        public virtual  bool SingleSpace
        {
            get { return m_SingleSpace; }
            set { SetProperty(ref m_SingleSpace, value, nameof(SingleSpace)); }
        }



        public virtual int StartPageNumber
        {
            get { return m_StartPageNumber; }
            set { SetProperty(ref m_StartPageNumber, value, nameof(StartPageNumber)); }
        }



        public virtual string TextFont
        {
            get { return m_TextFont; }
            set { SetProperty(ref m_TextFont, value, nameof(TextFont)); }
        }


        public virtual int TextSizePt
        {
            get { return m_TextSizePt; }
            set { SetProperty(ref m_TextSizePt, value, nameof(TextSizePt)); }
        }


        public virtual bool CreateToc
        {
            get { return m_CreateToc; }
            set { SetProperty(ref m_CreateToc, value, nameof(CreateToc)); }
        }


        public virtual int TransposeByHalftones
        {
            get { return m_TransposeByHalftones; }
            set { SetProperty(ref m_TransposeByHalftones, value, nameof(TransposeByHalftones)); }
        }


        public virtual int VerticalSpace
        {
            get { return m_VerticalSpace; }
            set { SetProperty(ref m_VerticalSpace, value, nameof(VerticalSpace)); }
        }

        public override string ToString()
        {
            var Converter = new SongOptionsConverter(DefaultSongOptions.Default);
            return Converter.SaveToString(this);
        }
    }
}
