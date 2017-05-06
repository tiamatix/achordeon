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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Achordeon.Lib.Parser
{
    public class PlainTextSongToChordProConverter
    {
        private readonly Regex m_ChordRex = new Regex(@"^[CDEFGABH]{1}[#B]?(MAJ)?[M]?(SUS)?(DIM)?[245679]?[#B]?(/[CDEFGABH]{1}[#B]?)?(\(\w{1,5}\))?$");
        private readonly Regex m_NoBlankTokenizerRex = new Regex(@"\S+");
        private readonly Regex m_BlankTokenizerRex = new Regex(@"\s+");

        public string Convert(string APlainTextSong)
        {
            var InputLines = APlainTextSong.Split(new [] { "\r\n", "\n" }, StringSplitOptions.None);
            var LineCount = InputLines.Length;
            var CurrentLineNo = 0;
            var Result = new StringBuilder();
            while (CurrentLineNo < LineCount)
            {
                var ThisLine = InputLines[CurrentLineNo];
                var NextLine = CurrentLineNo + 1 < LineCount ? InputLines[CurrentLineNo+1] : string.Empty;

                if (CurrentLineNo + 1 < LineCount && IsChordLine(ThisLine) && !IsChordLine(NextLine))
                {
                    Result.AppendLine(IntegrateChords(InputLines[CurrentLineNo], InputLines[CurrentLineNo + 1]));
                    CurrentLineNo += 2;
                }
                else
                {
                    Result.AppendLine(ThisLine);
                    CurrentLineNo += 1;
                }
            }

            return Result.ToString();
        }

        private string IntegrateChords(string AChordLine, string ATextLine)
        {            
            var Result = new StringBuilder();
            var ChordsLength = AChordLine.Length;
            var TextLength = ATextLine.Length;
            while (ChordsLength > TextLength)
            {
                ATextLine += " ";
                TextLength++;
            }

            var Last = 0;
            foreach (Match Token in m_NoBlankTokenizerRex.Matches(AChordLine))
            {
                var Position = Token.Index;
                var ChordProChord = "[" + Token.Value + "]";

                Result.Append(ATextLine.Substring(Last, Position - Last));
                Result.Append(ChordProChord);

                Last = Position;
            }
            Result.Append(ATextLine.Substring(Last));

            return Result.ToString();
        }


        private bool IsChordLine(string ATextLine)
        {
            if (string.IsNullOrWhiteSpace(ATextLine))
                return false;
            var Parts = m_BlankTokenizerRex.Split(ATextLine);
            //At least one not empty element in the line, that has a chord symbol inside
            return Parts.Any(a => !string.IsNullOrWhiteSpace(a)) && 
                Parts.All(a => string.IsNullOrWhiteSpace(a) || m_ChordRex.IsMatch(a.ToUpper()));
        }
    }
}
