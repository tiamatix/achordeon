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
using System.Drawing;
using System.IO;
using System.Linq;
using Achordeon.Lib.Chords;
using Achordeon.Lib.DataModel;
using Achordeon.Lib.Lib.Rendering.ChordDiagrams;
using Achordeon.Lib.SongOptions;
using DryIoc;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.IO;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.Rendering;


namespace Achordeon.Rendering.Pdf
{
   
    public class PdfRenderer : Renderer
    {
        public Document Document { get; private set; }

        private const char NON_BREAKING_SPACE = '\xA0';
        private static readonly string _NonBreakingTab = new string(NON_BREAKING_SPACE, 3);


        private const string SONG_TITLE_STYLE = StyleNames.Heading1;
        private const string SUBTITLE_STYLE = StyleNames.Heading2;

        private const string COMMENT_STYLE = "CommentStyle";
        private const string TAB_STYLE = "TabStyle";
        private const string CHORD_STYLE = "ChordStyle";
        private const string TEXT_STYLE = "TextStyle";
        private const string CHORUS_BORDER_STYLE = "ChorusBorder";
        private const string MEDIUM_LINE_BREAK_STYLE = "MediumLineBreak";
        

        private class StyleContainer
        {
            public string TextStyle { get; set; }
            public string ChordStyle { get; set; }
            public bool DrawChorusBorder { get; set; }
        }

        private class FontContainer
        {
            public FontContainer(string AFontName, double ASize, UnitType ASizeUnit, bool ABold, bool AItalic)
            {
                FontName = AFontName;
                Size = ASize;
                SizeUnit = ASizeUnit;
                Bold = ABold;
                Italic = AItalic;
            }

            public string FontName { get; }
            public double Size { get;}
            public UnitType SizeUnit { get; }
            public bool Bold { get; }
            public bool Italic { get; }
        }

        private string ReplaceBreakingSpace(string AText)
        {
            //var Space = '\x255';
            return AText.Replace("\t", _NonBreakingTab).Replace(' ', NON_BREAKING_SPACE);
            /*
            var Preprend = AText.Length - AText.TrimStart().Length;
            var Append = AText.Length - AText.TrimEnd().Length;
            //return new string(_NonBreakingSpace, Preprend) + AText.Trim() + new string(_NonBreakingSpace, Append);
            */
        }

        private SizeF MeasureTextMm(Document ADocument, string AStyleName, string AText)
        {
            var style = ADocument.Styles[AStyleName];
            var tm = new TextMeasurement(style.Font);
            var m = tm.MeasureString(AText, UnitType.Millimeter);
            return new SizeF((float)m.Width, (float)m.Height);
        }

        private static string MigraDocFilenameFromByteArray(byte[] AImage)
        {
            return "base64:" + Convert.ToBase64String(AImage);
        }


        private void RenderLine(Line ALine,
            Document ADocument,
            Section ASongSection,
            double ALineIndentMm,
            StyleContainer AStyle)
        {
            var ChordPositions = new Dictionary<LineChord, double>();
            var TextPositions = new Dictionary<LineText, double>();
            var CurrentXPositionMm = ALineIndentMm;
            foreach (var Run in ALine.Runs)
            {
                if (Run is LineText)
                {
                    var Text = (LineText) Run;
                    var TextWidthMm = MeasureTextMm(ADocument, AStyle.TextStyle, ReplaceBreakingSpace(Text.Text)).Width;
                    TextPositions[Text] = CurrentXPositionMm;
                    if (!SongOptions.LyricsOnly)
                    {
                        //If the previous chord needs more (or equal) space than the text
                        //we have to reserve extra space for the chord and one extra blank
                        var PreviousChordName = (ALine.GetPreviousElementOrNullOfType<LineChord>(Text) as LineChord)?.Name ?? string.Empty;
                        PreviousChordName = ChordNameBeautifier.BeautifySharpsAndFlatsOnly(PreviousChordName);
                        var PreviousChordNameWidthMm = MeasureTextMm(ADocument, AStyle.ChordStyle, ReplaceBreakingSpace(PreviousChordName)).Width;
                        if (PreviousChordNameWidthMm >= TextWidthMm)
                            CurrentXPositionMm += PreviousChordNameWidthMm + MeasureTextMm(ADocument, AStyle.ChordStyle, "_").Width;
                        else
                            CurrentXPositionMm += TextWidthMm;
                    }
                    else
                        CurrentXPositionMm += TextWidthMm;
                }
                if (Run is LineChord)
                {
                    if (!SongOptions.LyricsOnly)
                    {
                        var Chord = (LineChord) Run;
                        ChordPositions[Chord] = CurrentXPositionMm;
                    }
                }
            }
            var InsertMediumLinebreak = true;

            if (ChordPositions.Any() && !SongOptions.LyricsOnly)
            {
                foreach (var Chord in ALine.Runs.OfType<LineChord>())
                {
                    var PrettyName = ChordNameBeautifier.BeautifySharpsAndFlatsOnly(Chord.Name);

                    if (AStyle.DrawChorusBorder)
                    {
                        var BorderFrame = ASongSection.AddTextFrame();
                        BorderFrame.WrapFormat.Style = WrapStyle.None;
                        BorderFrame.WrapFormat.DistanceLeft = Unit.FromMillimeter(2.5);
                        BorderFrame.AddParagraph(" ").Style = CHORUS_BORDER_STYLE;
                    }

                    var ChordSize = MeasureTextMm(ADocument, AStyle.ChordStyle, ReplaceBreakingSpace(PrettyName));
                    var ChordFrame = ASongSection.AddTextFrame();
                    ChordFrame.WrapFormat.Style = WrapStyle.None;
                    ChordFrame.WrapFormat.DistanceLeft = Unit.FromMillimeter(ChordPositions[Chord]);
                    ChordFrame.Width = Unit.FromMillimeter(ChordSize.Width);
                    ChordFrame.Height = Unit.FromMillimeter(ChordSize.Height);
                    var paragraph = ChordFrame.AddParagraph(ReplaceBreakingSpace(PrettyName));
                    paragraph.Style = AStyle.ChordStyle;
                }
                InsertMediumLinebreak = false;
                ASongSection.AddParagraph();
            }

            if (AStyle.DrawChorusBorder)
                AddBorderFrame(ASongSection);

            var Texts = ALine.Runs.OfType<LineText>();
            var LineTexts = Texts as IList<LineText> ?? Texts.ToList();
            if (LineTexts.Any(a => !string.IsNullOrWhiteSpace(a.Text.Trim())))
            {
                foreach (var Text in LineTexts)
                {
                    if (!string.IsNullOrWhiteSpace(Text.Text.Trim()))
                        InsertMediumLinebreak = false;
                    var ChordFrame = ASongSection.AddTextFrame();
                    var TextSize = MeasureTextMm(ADocument, AStyle.TextStyle, ReplaceBreakingSpace(Text.Text));
                    ChordFrame.WrapFormat.Style = WrapStyle.None;
                    ChordFrame.WrapFormat.DistanceLeft = Unit.FromMillimeter(TextPositions[Text]);
                    ChordFrame.Width = Unit.FromMillimeter(TextSize.Width);
                    ChordFrame.Height = Unit.FromMillimeter(TextSize.Height);
                    Paragraph LastParagraph = ChordFrame.AddParagraph(ReplaceBreakingSpace(Text.Text));
                    LastParagraph.Style = AStyle.TextStyle;
                }
                var P = ASongSection.AddParagraph();
                if (InsertMediumLinebreak)
                    P.Style = MEDIUM_LINE_BREAK_STYLE;
            }
            else
            {
                var P = ASongSection.AddParagraph();
                    P.Style = MEDIUM_LINE_BREAK_STYLE;
            }
        }


        private void RenderLineRange(
            LineRange ARange, 
            Document ADocument,
            ref Section ASongSection,
            double ALineIndentMm,
            StyleContainer AStyle)
        {
            foreach (var obj in ARange.Lines)
            {
                if (obj is Line)
                {
                    RenderLine((Line) obj, ADocument, ASongSection, ALineIndentMm, AStyle);
                }
                else if (obj is PhysicalPageBreak || obj is LogicalPageBreak)
                {
                    ASongSection = ADocument.AddSection();
                }
                else if (obj is Comment)
                {
                    var Text = ReplaceBreakingSpace(ReplaceBreakingSpace(((Comment) obj).Text));
                    Paragraph CommentPara;
                    if (AStyle.DrawChorusBorder)
                    {
                        AddBorderFrame(ASongSection);
                        var ChordFrame = ASongSection.AddTextFrame();
                        ChordFrame.WrapFormat.Style = WrapStyle.None;
                        ChordFrame.WrapFormat.DistanceLeft = Unit.FromMillimeter(5);
                        CommentPara = ChordFrame.AddParagraph(Text);
                        ASongSection.AddParagraph().Style = MEDIUM_LINE_BREAK_STYLE;
                    }
                    else
                        CommentPara = ASongSection.AddParagraph(Text);
                    CommentPara.Style = COMMENT_STYLE;
                }
                else if (obj is Chorus)
                {
                    var sc = new StyleContainer();
                    sc.ChordStyle = CHORD_STYLE;
                    sc.TextStyle = TEXT_STYLE;
                    sc.DrawChorusBorder = true;
                    RenderLineRange((Chorus) obj, ADocument, ref ASongSection, ALineIndentMm + 5, sc);
                }
                else if (obj is Tab)
                {
                    if (!SongOptions.LyricsOnly)
                    {
                        var sc = new StyleContainer();
                        sc.ChordStyle = TAB_STYLE;
                        sc.TextStyle = TAB_STYLE;
                        sc.DrawChorusBorder = false;
                        RenderLineRange((Tab) obj, ADocument, ref ASongSection, ALineIndentMm, sc);
                    }
                }
                else if (obj is LineRange)
                    RenderLineRange((LineRange) obj, ADocument, ref ASongSection, ALineIndentMm, AStyle);
            }
        }

        private void AddBorderFrame(Section ASongSection)
        {
            var BorderFrame = ASongSection.AddTextFrame();
            BorderFrame.WrapFormat.Style = WrapStyle.None;
            BorderFrame.WrapFormat.DistanceLeft = Unit.FromMillimeter(2.5);
            BorderFrame.AddParagraph(" ").Style = CHORUS_BORDER_STYLE;
        }


        private void DefineStyles(Document ADocument,
            FontContainer ATextFont,
            FontContainer AChordFont, 
            FontContainer ATabFont,
            FontContainer ACommentFont)
        {
            Style style = ADocument.Styles[StyleNames.Normal];
            style.Font.Name = ATextFont.FontName;
            style.Font.Size = new Unit(ATextFont.Size, ATextFont.SizeUnit);
            style.Font.Bold = false;
            style.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            //==========================
            style = ADocument.Styles[SONG_TITLE_STYLE];
            style.Font.Name = ATextFont.FontName;
            style.Font.Size = new Unit(ATextFont.Size * 1.6, ATextFont.SizeUnit);
            style.Font.Bold = true;
            style.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            //==========================
            style = ADocument.Styles[SUBTITLE_STYLE];
            style.Font.Name = ATextFont.FontName;
            style.Font.Size = new Unit(ATextFont.Size * 1.2, ATextFont.SizeUnit);
            style.Font.Bold = false;
            style.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            

            style = ADocument.Styles[StyleNames.Header];
            style.Font.Name = ATextFont.FontName;
            style.Font.Size = new Unit(ATextFont.Size, ATextFont.SizeUnit);
            style.Font.Bold = false;
            style.Font.Italic = false;
            style.ParagraphFormat.Alignment = ParagraphAlignment.Left;


            style = ADocument.Styles[StyleNames.Footer];
            style.Font.Name = ATextFont.FontName;
            style.Font.Size = new Unit(ATextFont.Size, ATextFont.SizeUnit);
            style.Font.Bold = false;
            style.Font.Italic = false;
            style.ParagraphFormat.Alignment = ParagraphAlignment.Left;


            style = ADocument.Styles.AddStyle(CHORD_STYLE, StyleNames.Normal);
            style.Font.Name = AChordFont.FontName;
            style.Font.Size = new Unit(AChordFont.Size, AChordFont.SizeUnit);
            style.Font.Bold = AChordFont.Bold;
            style.Font.Italic = AChordFont.Italic;
            style.ParagraphFormat.Alignment = ParagraphAlignment.Left;


            style = ADocument.Styles.AddStyle(TEXT_STYLE, StyleNames.Normal);
            style.Font.Name = ATextFont.FontName;
            style.Font.Size = new Unit(ATextFont.Size, ATextFont.SizeUnit);
            style.Font.Bold = ATextFont.Bold;
            style.Font.Italic = ATextFont.Italic;
            style.ParagraphFormat.Alignment = ParagraphAlignment.Left;


            style = ADocument.Styles.AddStyle(COMMENT_STYLE, StyleNames.Normal);
            style.Font.Name = ACommentFont.FontName;
            style.Font.Size = new Unit(ACommentFont.Size, ACommentFont.SizeUnit);
            style.Font.Bold = ACommentFont.Bold;
            style.Font.Italic = ACommentFont.Italic;
            style.ParagraphFormat.Alignment = ParagraphAlignment.Left;


            style = ADocument.Styles.AddStyle(TAB_STYLE, StyleNames.Normal);
            style.Font.Name = ATabFont.FontName;
            style.Font.Size = new Unit(ATabFont.Size, ATabFont.SizeUnit);
            style.Font.Bold = ATabFont.Bold;
            style.Font.Italic = ATabFont.Italic;
            style.ParagraphFormat.Alignment = ParagraphAlignment.Left;


            var HorBor = new Border();
            HorBor.Width = "2pt";
            HorBor.Color = Colors.Black;
            HorBor.Style = BorderStyle.Single;


            style = ADocument.Styles.AddStyle(CHORUS_BORDER_STYLE, StyleNames.Normal);
            style.ParagraphFormat.Borders.Left = HorBor.Clone();
            style.ParagraphFormat.LineSpacing = 0;
            style.ParagraphFormat.SpaceBefore = 0;
            style.ParagraphFormat.SpaceAfter = 0;

            style = ADocument.Styles.AddStyle(MEDIUM_LINE_BREAK_STYLE, StyleNames.Normal);
            style.Font.Name = ATextFont.FontName;
            style.Font.Size = new Unit(ATextFont.Size * 0.66, ATextFont.SizeUnit);
            style.Font.Bold = false;
            style.Font.Italic = false;
            style.ParagraphFormat.Alignment = ParagraphAlignment.Left;
        }

        private void RenderSong(Song ASong, Document ADocument)
        {
            var SongSection = ADocument.AddSection();
            switch (SongOptions.PageSize.ToLower())
            {
                case "a5":
                    SongSection.PageSetup.PageFormat = PageFormat.A5;
                    break;
                case "letter":
                case "us":
                    SongSection.PageSetup.PageFormat = PageFormat.Letter;
                    break;
                default:
                    SongSection.PageSetup.PageFormat = PageFormat.A4;
                    break;
            }
            if (!string.IsNullOrWhiteSpace(ASong.Title))
            {
                var paragraph = SongSection.AddParagraph(ASong.Title);
                paragraph.Style = SONG_TITLE_STYLE;
            }
            foreach (var Subtitle in ASong.Subtitles)
            {
                var paragraph = SongSection.AddParagraph(Subtitle);
                paragraph.Style = SUBTITLE_STYLE;
            }
            var sc = new StyleContainer();
            sc.ChordStyle = CHORD_STYLE;
            sc.TextStyle = TEXT_STYLE;
            sc.DrawChorusBorder = false;
            RenderLineRange(ASong, ADocument, ref SongSection, 0, sc);
                SongSection.AddParagraph();
            if (!SongOptions.DrawChordGrids || SongOptions.LyricsOnly)
                return;
            var p = SongSection.AddParagraph();
            foreach (var Chord in ASong.UsedChords)
            {
                if (Chord.Difficulty == Difficulty.Easy && Chord.Origin == ChordOrigin.BuildIn)
                    continue;

                using (var ImageGen = new ChordBoxImage(IoC, Chord, 4))
                {
                    var MigraName = MigraDocFilenameFromByteArray(ImageGen.GetBytes());
                    var img = p.AddImage(MigraName);
                    img.Height = Unit.FromMillimeter(20);
                }
            }
        }

        public override void Render(Stream ATargetStream)
        {
            RenderToPdf().PdfDocument.Save(ATargetStream, false);
        }


        public string RenderToDdl()
        {
            RenderToDocument();
            return DdlWriter.WriteToString(Document);
        }

        private void RenderToDocument()
        {
            BeginRender();
            try
            {
                Document = null;
                var WorkingDocument = new Document();
                DefineStyles(WorkingDocument,
                    new FontContainer(SongOptions.TextFont, SongOptions.TextSizePt, UnitType.Point, false, false),
                    new FontContainer(SongOptions.ChordFont, SongOptions.ChordSizePt, UnitType.Point, true, false),
                    new FontContainer("Courier New", SongOptions.ChordSizePt, UnitType.Point, false, false),
                    new FontContainer(SongOptions.TextFont, SongOptions.TextSizePt, UnitType.Point, false, true));
                foreach (var Song in Book.Songs)
                {
                    RenderSong(Song, WorkingDocument);
                }
                Document = WorkingDocument;
            }
            finally
            {
                EndRender();
            }
        }

        private PdfDocumentRenderer RenderToPdf()
        {
            RenderToDocument();
            var Renderer = new PdfDocumentRenderer(false);
            Renderer.Document = Document;
            Renderer.RenderDocument();
            return Renderer;
        }

        public PdfRenderer(Container AIoC, SongBook ABook, ISongOptions ASongOptions) : base(AIoC, ABook, ASongOptions)
        {
        }
    }
}

