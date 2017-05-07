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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DryIoc;

namespace Achordeon.Lib.DataModel
{
    public class Line : LineObject
    {
        private readonly List<LineRun> m_Runs = new List<LineRun>();

        public override IEnumerable<ChordProObject> Subobjects => m_Runs.SelectMany(a => a.Subobjects).Union(m_Runs);

        public override void Transpose(int AHalfToneSteps)
        {
            m_Runs.ForEach(a => a.Transpose(AHalfToneSteps));
        }

        public IEnumerable<LineRun> Runs => m_Runs;

        public bool Empty => m_Runs.Count < 1;

        public LineRun GetNextElementOrNullOfType<T>(LineRun APivot)
            where T : LineRun
        {
            var Ok = false;
            foreach (var LineRun in m_Runs)
            {
                if (LineRun is T && Ok)
                    return (T)LineRun;
                if (LineRun == APivot)
                    Ok = true;
            }
            return default(T);
        }

        public LineRun GetPreviousElementOrNullOfType<T>(LineRun APivot)
            where T : LineRun
        {
            var Result = default(T);
            foreach (var LineRun in m_Runs)
            {
                if (LineRun is T)
                    Result = (T) LineRun;
                if (LineRun == APivot)
                    break;
            }
            return Result;
        }

        public void AddChord(LineChord ALineChord)
        {
            m_Runs.Add(ALineChord);
        }

        public void AddText(LineText AText)
        {
            m_Runs.Add(AText);
        }

        public Line(Container AIoC, int ALineNo, int ALinePos) : base(AIoC, ALineNo, ALinePos)
        {
        }

        public override void PrintChordPro(StringBuilder ABuilder)
        {
            m_Runs.ForEach(a => a.PrintChordPro(ABuilder));
            ABuilder.AppendLine();
        }

    }
}
