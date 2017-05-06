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
using System.Linq;
using System.Text;
using DryIoc;

namespace Achordeon.Lib.DataModel
{
    public class LineRange : LineObject
    {
        protected readonly List<LineObject> m_Lines = new List<LineObject>();

        public override IEnumerable<ChordProObject> Subobjects => m_Lines.SelectMany(a => a.Subobjects).Union(m_Lines);

        public override void Transpose(int AHalfToneSteps)
        {
            m_Lines.ForEach(a => a.Transpose(AHalfToneSteps));
        }

        public IEnumerable<LineObject> Lines => m_Lines;

        public Line LastLine => Lines.LastOrDefault(a => a is Line) as Line;

        public virtual bool AcceptNonPlaintext => true;

        public void RemoveLastEmptyLine()
        {
            var PossibleLastLine = Lines.Last() as Line;
            if (PossibleLastLine?.Empty ?? false)
                m_Lines.Remove(PossibleLastLine);
        }

        public LineRange(Container AIoC, int ALineNo, int ALinePos) : base(AIoC, ALineNo, ALinePos)
        {

        }

        public void AddLineRange(LineRange ALineRange)
        {
            m_Lines.Add(ALineRange);
        }


        public void AddLine(Line ALine)
        {
            m_Lines.Add(ALine);
        }

        public void InsertBefore(LineObject AInsertBefore, LineObject AComment)
        {
            var Index = m_Lines.IndexOf(AInsertBefore);
            if (Index < 0)
                throw new IndexOutOfRangeException();
            m_Lines.Insert(Index, AComment);
        }

        public void Add(LineObject AComment)
        {
            m_Lines.Add(AComment);
        }

        public override void PrintChordPro(StringBuilder ABuilder)
        {
            m_Lines.ForEach(a => a.PrintChordPro(ABuilder));
        }
    }
}
