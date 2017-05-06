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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Achordeon.Common.Helpers;
using Achordeon.Lib.DataModel;
using Achordeon.Lib.SongOptions;
using DryIoc;
using DryIoc.Experimental;

namespace Achordeon.Rendering.Ascii
{
    public class AsciiRenderer : Renderer
    {
        private int m_CurrentLineLen = 0;
        private int m_MaxLineLen = 0;
        private StringBuilder m_Builder1 = new StringBuilder();

        private void Append(char AChar)
        {
            m_CurrentLineLen++;
            m_Builder1.Append(AChar);
        }

        private void Append(string AText)
        {
            m_CurrentLineLen += AText.Length;
            m_Builder1.Append(AText);
        }

        private void AppendLine(string AText)
        {
            Append(AText);
            AppendLine();
        }

        private void AppendLine()
        {
            if (m_CurrentLineLen > m_MaxLineLen)
                m_MaxLineLen = m_CurrentLineLen;
            m_CurrentLineLen = 0;
            m_Builder1.AppendLine();
        }

        private void RenderLine(Line ALine, int AIndent)
        {
            /*
             * Map chords and text accordingly:
             * 
             * Well A child arrived [G]just [Amaj7/G]the [C]other day
             * 
             * =>
             * 
             *                      G    Amaj7/C G  
             * Well A child arrived just the     other day
             */
            var ChordPositions = new Dictionary<LineChord, int>();
            var TextPositions = new Dictionary<LineText, int>();
            var CurrentPosition = AIndent;
            foreach (var Run in ALine.Runs)
            {
                if (Run is LineText)
                {
                    var Text = (LineText) Run;
                    TextPositions[Text] = CurrentPosition;
                    //If the previous chord needs more (or equal) space than the text
                    //we have to reserve extra space for the chord and one extra blank
                    var PreviousChordLength = ALine.GetPreviousElementOrNullOfType<LineChord>(Text)?.TextLength ?? 0;
                    if (PreviousChordLength >= Text.TextLength)
                        CurrentPosition += PreviousChordLength + 1;
                    else
                        CurrentPosition += Text.TextLength;
                }
                if (Run is LineChord)
                {
                    if (!SongOptions.LyricsOnly)
                    {
                        var Chord = (LineChord) Run;
                        ChordPositions[Chord] = CurrentPosition;
                    }
                }
            }
            if (ChordPositions.Any() && !SongOptions.LyricsOnly)
            {
                CurrentPosition = 0;
                foreach (var Chord in ALine.Runs.OfType<LineChord>())
                {
                    var ChordPos = ChordPositions[Chord];
                    while (CurrentPosition < ChordPos)
                    {
                        Append(' ');
                        CurrentPosition++;
                    }
                    Append(Chord.Name);
                    CurrentPosition += Chord.TextLength;
                }
                AppendLine();
            }
            CurrentPosition = 0;
            foreach (var Text in ALine.Runs.OfType<LineText>())
            {
                var TextPos = TextPositions[Text];
                while (CurrentPosition < TextPos)
                {
                    Append(' ');
                    CurrentPosition++;
                }
                Append(Text.Text);
                CurrentPosition += Text.TextLength;
            }
            AppendLine();
        }

        private string GetIndentString(int AIndent, char AChar = ' ')
        {
            return new string(AChar, AIndent);
        }

        private void RenderLineRange(LineRange ARange, int AIndent)
        {
            foreach (var obj in ARange.Lines)
            {
                if (obj is Line)
                    RenderLine((Line) obj, AIndent);
                else if (obj is Comment)
                {
                    Append(GetIndentString(AIndent));
                    AppendLine(((Comment) obj).Text);
                }
                else if (obj is Tab)
                {
                    if (!SongOptions.LyricsOnly)
                        RenderLineRange((LineRange)obj, AIndent);
                }
                else if (obj is Chorus)
                    RenderLineRange((Chorus) obj, AIndent + 2);
                else if (obj is LineRange)
                    RenderLineRange((LineRange) obj, AIndent);
            }
        }

        private void RenderSong(Song ASong, int AIndent)
        {
            m_MaxLineLen = 0;
            var StartPos = m_Builder1.Length;
            foreach (var Subtitle in ASong.Subtitles)
            {
                Append(GetIndentString(AIndent));
                AppendLine(Subtitle);
            }
            RenderLineRange(ASong, AIndent);
            var Indent = Math.Max(0, m_MaxLineLen/2 - ASong.Title.Length/2);
            m_Builder1.Insert(StartPos,
                GetIndentString(Indent) +
                ASong.Title.ToUpper() +
                Environment.NewLine +
                GetIndentString(Indent) +
                GetIndentString(ASong.Title.Length, '=') +
                Environment.NewLine);
        }

        public override void Render(Stream ATargetStream)
        {
            BeginRender();
            try
            {
                m_MaxLineLen = 0;
                m_Builder1 = new StringBuilder();
                foreach (var Song in Book.Songs)
                {
                    RenderSong(Song, 0);
                    if (Song != Book.Songs.LastOrDefault())
                        AppendLine();
                }
                var Encoding = IoC.Get<Encoding>() ?? System.Text.Encoding.Default;
                using (TextWriter tw = new StreamWriter(ATargetStream, Encoding, 1, true))
                {
                    tw.WriteLine(m_Builder1.ToString());
                }
            }
            finally
            {
                EndRender();
            }
        }

        public string Render()
        {
            using (var Stream = new StringStream())
            {
                Render(Stream);
                return Stream.ToString();
            }
        }

        public AsciiRenderer(Container AIoC, SongBook ABook, ISongOptions ASongOptions) : base(AIoC, ABook, ASongOptions)
        {
        }
    }
}

