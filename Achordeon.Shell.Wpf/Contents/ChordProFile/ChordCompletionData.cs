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
using System.Text;
using System.Windows.Media.Imaging;
using Achordeon.Lib.Chords;
using Achordeon.Lib.DataModel;
using Achordeon.Lib.Lib.Rendering.ChordDiagrams;
using DryIoc;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

namespace Achordeon.Shell.Wpf.Contents.ChordProFile
{
    /// Implements AvalonEdit ICompletionData interface to provide the entries in the
    /// completion drop down.
    public class ChordCompletionData : ICompletionData
    {
        private Container IoC { get; }
        private IEnumerable<ChordList> ChordLists { get; }

        public ChordCompletionData(Container AIoC, IEnumerable<ChordList> AChordLists, int APriority, string AChordName)
        {
            IoC = AIoC;
            Text = AChordName;
            ChordLists = AChordLists;
            Priority = APriority;
        }

        private Chord GetChord()
        {
            Chord Result = null;
            foreach (var ChordList in ChordLists)
            {
                Result = ChordList?.GetChordOrNull(Text);
                if (Result != null)
                    break;
            }
            return Result;
        }

        private System.Windows.Media.ImageSource GetImage()
        {
            var Chord = GetChord();
            if (Chord == null)
                return null;
            var Gen = new ChordBoxImage(IoC, Chord, 1);
            using (var TempStream = new MemoryStream())
            {
                Gen.Save(TempStream);
                TempStream.Seek(0, SeekOrigin.Begin);
                var Result = new BitmapImage();
                Result.BeginInit();
                Result.StreamSource = TempStream;
                Result.CacheOption = BitmapCacheOption.OnLoad;
                Result.UriSource = null;
                Result.EndInit();
                return Result;
            }
        }

        public System.Windows.Media.ImageSource Image => null;

        public string Text { get; }

        public object Content => ChordNameBeautifier.BeautifySharpsAndFlatsOnly(Text);

        public object Description
        {
            get
            {
                var Chord = GetChord();
                if (Chord == null)
                    return Content;
                var sb = new StringBuilder();
                sb.AppendLine(ChordNameBeautifier.BeautifySharpsAndFlatsOnly(Text));
                sb.AppendLine();
                sb.Append(Chord.GetFretDisplay(Chord.Fret1));
                sb.Append("-");
                sb.Append(Chord.GetFretDisplay(Chord.Fret2));
                sb.Append("-");
                sb.Append(Chord.GetFretDisplay(Chord.Fret3));
                sb.Append("-");
                sb.Append(Chord.GetFretDisplay(Chord.Fret4));
                sb.Append("-");
                sb.Append(Chord.GetFretDisplay(Chord.Fret5));
                sb.Append("-");
                sb.Append(Chord.GetFretDisplay(Chord.Fret6));
                sb.AppendLine();
                return sb.ToString();  
                //return GetImage(); does not work
            }
        }

        public double Priority { get; }

        public void Complete(TextArea ATextArea, ISegment ACompletionSegment, EventArgs AInsertionRequestEventArgs)
        {
            ATextArea.Document.Replace(ACompletionSegment, Text + "]");
        }
    }
}
