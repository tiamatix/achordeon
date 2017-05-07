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
using System.Globalization;
using System.IO;
using System.Text;
using Achordeon.Common.Extensions;
using Achordeon.Common.Helpers;
using Achordeon.Lib.Chords;
using Achordeon.Lib.DataModel;
using Achordeon.Lib.Helpers;
using Achordeon.Lib.Properties;
using DryIoc;

namespace Achordeon.Lib.Parser
{
    public class ChordProParser : IDisposable
    {
        private Container IoC { get; }
        private TextReader m_Reader;
        private const char EOF = '\uffff';
        private const char EOL = '\n';
        private int m_CurrentLineNo ;
        private int m_CurrentLinePos;
        private char Current { get; set; }
        private char Peek => (char) (m_Reader?.Peek() ?? EOF);
        private readonly SongBook m_CurrentBook;
        private Song m_CurrentSong;
        private readonly Stack<LineRange> m_RangeStack = new Stack<LineRange>();
        private LineRange CurrentRange => m_RangeStack.Peek();
        private Line CurrentLine => CurrentRange.LastLine;

        public SongBook SongBook => m_CurrentBook;
        public Encoding Encoding { get; }

        private void BeginSong()
        {
            m_CurrentBook.ThrowIfNullEx(nameof(m_CurrentBook));
            m_CurrentBook.AddSong(m_CurrentSong = new Song(IoC, m_CurrentLineNo, m_CurrentLinePos));
            m_RangeStack.Push(m_CurrentSong);
            BeginRange(new LineRange(IoC, m_CurrentLineNo, m_CurrentLinePos), false);
        }

        private void BeginRange(LineRange ANewLineRange, bool AInsertBeforeCurrentLine)
        {
            m_CurrentSong.ThrowIfNullEx(nameof(m_CurrentSong));
            if (AInsertBeforeCurrentLine && CurrentLine != null)
                CurrentRange.InsertBefore(CurrentLine, ANewLineRange);
            else
                CurrentRange.AddLineRange(ANewLineRange);
            m_RangeStack.Push(ANewLineRange);
            BeginLine();
        }

        private void EndRange()
        {
            m_RangeStack.Pop();
        }

        private void BeginLine()
        {
            CurrentRange.ThrowIfNullEx(nameof(CurrentRange));
            CurrentRange.AddLine(new Line(IoC, m_CurrentLineNo, m_CurrentLinePos));
        }

        private void ReadNextChar()
        {
            Current = Peek;
            m_Reader?.Read();
            if (Current == EOL)
            {
                m_CurrentLineNo++;
                m_CurrentLinePos = 0;
            }
            m_CurrentLinePos++;
        }


        public ChordProParser(Container AIoC, Stream AInputStream) : this(AIoC, AInputStream, TextEncodingDetector.Detect(AInputStream) ?? Encoding.Default)
        {

        }


        public ChordProParser(Container AIoC, Stream AInputStream, Encoding AEncoding)
        {
            IoC = AIoC;
            m_Reader = new StreamReader(AInputStream, AEncoding);
            m_CurrentBook = new SongBook(IoC);
            Encoding = AEncoding;
        }

        public void Dispose()
        {
            try
            {
                m_Reader?.Dispose();
            }
            finally
            {
                m_Reader = null;
            }

        }

        private void SkipWhitespace()
        {
            while (Current != '\r' && Current != '\n' && char.IsWhiteSpace(Current))
                ReadNextChar();
        }

        private string ReadString()
        {
            var res = new StringBuilder();
            while (char.IsLetter(Current) || Current == '_')
            {
                res.Append(Current);
                ReadNextChar();
            }
            return res.ToString();
        }

        private void Expect(char AChar)
        {
            if (Current != AChar)
                throw new Exception(string.Format(Resources.ChordProParser_Expect___0___expected__but___1___found_, AChar, Current));
        }

        private string ReadUntil(params char[] AStopChars)
        {
            var res = new StringBuilder();
            while (Current != EOF && Array.IndexOf(AStopChars, Current) < 0)
            {
                res.Append(Current);
                ReadNextChar();
            }
            return res.ToString();
        }

        private void ReadWhile(params char[] AStopChars)
        {
            while (Current != EOF && Array.IndexOf(AStopChars, Current) >= 0)
                ReadNextChar();
        }

        private void ExpecteEndOfDirective()
        {
            SkipWhitespace();
            Expect('}');
            ReadNextChar();
        }

        private void SkipSingleLineBreak()
        {
            SkipWhitespace();
            if (Current == '\r' && Peek == '\n')
            {
                ReadNextChar();
                ReadNextChar();
            }
            else if (Current == '\n')
                ReadNextChar();
        }

        private void ReadDirective()
        {
            Expect('{');
            ReadNextChar();
            SkipWhitespace();
            var FirstString = ReadString();
            switch (FirstString.ToLower())
            {
                case "new_page":
                case "np":
                    CurrentRange.InsertBefore(CurrentLine, new LogicalPageBreak(IoC, m_CurrentLineNo, m_CurrentLinePos));
                    ExpecteEndOfDirective();
                    SkipSingleLineBreak();
                    BeginLine();
                    return;
                case "new_physical_page":
                case "npp":
                    CurrentRange.InsertBefore(CurrentLine, new PhysicalPageBreak(IoC, m_CurrentLineNo, m_CurrentLinePos));
                    ExpecteEndOfDirective();
                    SkipSingleLineBreak();
                    BeginLine();
                    return;
                case "column_break":
                case "colb":
                    CurrentRange.InsertBefore(CurrentLine, new ColumnBreak(IoC, m_CurrentLineNo, m_CurrentLinePos));
                    ExpecteEndOfDirective();
                    SkipSingleLineBreak();
                    BeginLine();
                    return;
                case "no_grid":
                case "ng":
                    ExpecteEndOfDirective();
                    SkipSingleLineBreak();
                    m_CurrentSong.NoGrid = true;
                    return;
                case "grid":
                case "g":
                    ExpecteEndOfDirective();
                    SkipSingleLineBreak();
                    m_CurrentSong.NoGrid = false;
                    return;
                case "new_song":
                case "ns":
                    ExpecteEndOfDirective();
                    SkipSingleLineBreak();
                    BeginSong();
                    return;
                case "start_of_chorus":
                case "soc":
                    ExpecteEndOfDirective();
                    SkipSingleLineBreak();
                    BeginRange(new Chorus(IoC, m_CurrentLineNo, m_CurrentLinePos), true);
                    return;
                case "end_of_chorus":
                case "eoc":
                    ExpecteEndOfDirective();
                    SkipSingleLineBreak();
                    CurrentRange.RemoveLastEmptyLine();
                    EndRange();
                    //BeginLine();
                    if (CurrentRange == null)
                        throw new Exception(string.Format(Resources.ChordProParser_ReadDirective_Unexpected___0_____not_within_a_chorus_, FirstString));
                    return;
                case "start_of_tab":
                case "sot":
                    ExpecteEndOfDirective();
                    SkipSingleLineBreak();
                    BeginRange(new Tab(IoC, m_CurrentLineNo, m_CurrentLinePos), true);
                    return;
                case "end_of_tab":
                case "eot":
                    ExpecteEndOfDirective();
                    SkipSingleLineBreak();
                    CurrentRange.RemoveLastEmptyLine();
                    EndRange();
                    if (CurrentRange == null)
                        throw new Exception(string.Format(Resources.ChordProParser_ReadDirective_Unexpected___0_____not_within_a_tab_, FirstString));
                    return;
                case "define":
                    SkipWhitespace();
                    if (Current != ':') //Check if its the old format!
                    {
                        var RestOfLine = ReadUntil('}', EOL);
                        ExpecteEndOfDirective();
                        SkipSingleLineBreak();
                        m_CurrentSong.DefineChord(RestOfLine.Trim());
                        return;
                    }
                    break;
            }
            SkipWhitespace();
            if (Current == ':')
            {
                ReadNextChar();
                SkipWhitespace();
                var RestOfLine = ReadUntil('}', EOL);
                ExpecteEndOfDirective();
                SkipSingleLineBreak();
                switch (FirstString.ToLower())
                {
                    case "define":
                        m_CurrentSong.DefineChord(RestOfLine.Trim(), ChordDefinitionFormat.Obsolete);
                        return;
                    case "textfont":
                    case "tf":
                        m_CurrentSong.TextFont = RestOfLine;
                        return;
                    case "textsize":
                    case "ts":
                        m_CurrentSong.TextSize = double.Parse(RestOfLine, NumberStyles.Float, CultureInfo.InvariantCulture);
                        return;
                    case "chordfont":
                    case "cf":
                        m_CurrentSong.ChordFont = RestOfLine;
                        return;
                    case "chordsize":
                    case "cs":
                        m_CurrentSong.ChordSize = double.Parse(RestOfLine, NumberStyles.Float, CultureInfo.InvariantCulture);
                        return;
                    case "columns":
                    case "col":
                        m_CurrentSong.Columns = int.Parse(RestOfLine, NumberStyles.Integer, CultureInfo.InvariantCulture);
                        return;
                    case "chordcolour":
                        m_CurrentSong.ChordColor = RestOfLine;
                        return;
                    case "pagetype":
                        m_CurrentSong.PageType = RestOfLine;
                        return;
                    case "title":
                    case "t":
                        m_CurrentSong.Title = RestOfLine;
                        return;
                    case "subtitle":
                    case "st":
                        m_CurrentSong.AddSubtitle(RestOfLine);
                        return;
                    case "comment":
                    case "c":
                    case "comment_italic":
                    case "ci":
                    case "comment_box":
                    case "cb":
                        CommentStyle Style;
                        switch (FirstString.ToLower())
                        {
                            case "comment":
                            case "c":
                                Style = CommentStyle.Comment;
                                break;
                            case "comment_italic":
                            case "ci":
                                Style = CommentStyle.CommentItalic;
                                break;
                            case "comment_box":
                            case "cb":
                                Style = CommentStyle.ComentBox;
                                break;
                            default:
                                throw new Exception(string.Format(Resources.ChordProParser_ReadDirective_Comment_style__0__unsupported, FirstString));
                        }
                        CurrentRange.InsertBefore(CurrentLine, new Comment(IoC, Style, RestOfLine, m_CurrentLineNo, m_CurrentLinePos));
                        return;
                    default:
                        throw new Exception(string.Format(Resources.ChordProParser_ReadDirective_Directive___0___unsupported, FirstString));
                }
            }
            throw new Exception(string.Format(Resources.ChordProParser_ReadDirective_Directive___0___unsupported, FirstString));
        }


        private void ReadChord()
        {
            Expect('[');
            ReadNextChar();
            SkipWhitespace();
            var Chord = ReadUntil(']', EOL);
            CurrentLine.AddChord(new LineChord(IoC, Chord, m_CurrentLineNo, m_CurrentLinePos));
            Expect(']');
            ReadNextChar();
        }

        private void ReadLineBreak(int AMaxAmoutOfConsecutiveLineBreaks, bool ABeginLine)
        {
            int Count = 1;
            while (Current != EOF && Current == '\r' || (Current == EOL) && Count <= AMaxAmoutOfConsecutiveLineBreaks)
            {
                if (Current == EOL)
                    Count++;
                ReadNextChar();
            }
            ReadWhile(EOL, '\r');
            if (ABeginLine)
            {
                for (int i = 0; i < Count - 1; i++)
                    BeginLine();
            }
        }


        private void ReadPlainText()
        {
            var Text = string.Empty;
            switch (Current)
            {
                case '[':
                case '{':
                case '\r':
                case EOL:
                    Text += Current;
                    ReadNextChar();
                    break;
            }            
            Text += ReadUntil('[', '{', EOL, '\r');
            CurrentLine.AddText(new LineText(IoC, Text, m_CurrentLineNo, m_CurrentLinePos));
        }

        public void ReadFile()
        {
            BeginSong();
            try
            {
                ReadNextChar();
                while (true)
                {
                    switch (Current)
                    {
                        case EOF:
                            return;
                        case '[':
                            if (CurrentRange.AcceptNonPlaintext)
                                ReadChord();
                            else
                                ReadPlainText();
                            break;
                        case '{':
                            ReadDirective();
                            break;
                        case '\r':
                        case EOL:
                            ReadLineBreak(Int32.MaxValue, true);
                            break;
                        default:
                            ReadPlainText();
                            break;
                    }
                }
            }
            finally
            {
                SongBook.RebuildBuildChordList();
            }
        }
    }
}
