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
using System.Text;
using Achordeon.Lib.Properties;

namespace Achordeon.Lib.Transposing
{
    public class Transposer
    {
        private readonly char[] m_Notes = new[] {'C', 'D', 'E', 'F', 'G', 'A', 'B'};
        private const string EMPTY_CHORD = "       ";
        private const char END_OF_STRING = '\0';

        private bool CiCompare(char AA, char AB)
        {
            return char.ToLowerInvariant(AA) == char.ToLowerInvariant(AB);
        }

        public string TransposeChord(string AInputChord, int AHalfToneSteps)
        {
            //attempting parsing
            //1) look for a known note name at start of the chord
            //2) copy literally until the delimiter was found (or at the end of the chord name)
            //3) repeat

            //Reserve enough space for any possible result
            var ResultChord = new StringBuilder(EMPTY_CHORD + ' ');

            //Prepare input chord
            AInputChord = AInputChord.Trim() + END_OF_STRING;

            var ChordIndex = 0;
            var ResultChordIndex = 0;

            while (true)
            {
                //Find root note
                var BaseNoteIndex = 0;
                while ((BaseNoteIndex < 7) && (!CiCompare(AInputChord[ChordIndex], m_Notes[BaseNoteIndex])))
                    BaseNoteIndex++;

                if (BaseNoteIndex >= 7)
                    throw new TransposeException(string.Format(Resources.Transposer_TransposeChord_Cannot_transpose__0__chord__base_note_not_recognized, AInputChord));


                ChordIndex++;

                //See if chord is sharp or flat
                var IsFlatChord = (AInputChord[ChordIndex] == 'b');
                var IsSharpChord = (AInputChord[ChordIndex] == '#');

                if (IsSharpChord || IsFlatChord)
                    ChordIndex++;


                //Build new chord name

                var NewChordIndex = BaseNoteIndex;

                if (AHalfToneSteps > 0)
                {
                    //moving upscale
                    for (var j = 0; j < AHalfToneSteps; j++)
                    {
                        if (IsSharpChord)
                        {
                            IsSharpChord = false;
                            NewChordIndex = (NewChordIndex + 1)%7;
                        }
                        else if (IsFlatChord)
                            IsFlatChord = false;
                        else if ((NewChordIndex == 2) || (NewChordIndex == 6))
                            NewChordIndex = (NewChordIndex + 1)%7;
                        else
                            IsSharpChord = true;
                    }
                }

                else
                {
                    //moving downscale
                    for (var j = 0; j > AHalfToneSteps; j--)
                    {
                        if (IsFlatChord)
                        {
                            IsFlatChord = false;
                            NewChordIndex = (NewChordIndex + 6)%7;
                        }
                        else if (IsSharpChord)
                            IsSharpChord = false;
                        else if ((NewChordIndex == 3) || (NewChordIndex == 0))
                            NewChordIndex = (NewChordIndex + 6)%7;
                        else
                            IsFlatChord = true;
                    }
                }
                ResultChord[ResultChordIndex] = m_Notes[NewChordIndex];
                ResultChordIndex++;

                if (IsSharpChord)
                    ResultChord[ResultChordIndex++] = '#';
                if (IsFlatChord)
                    ResultChord[ResultChordIndex++] = 'b';

                while ((AInputChord[ChordIndex] != '/') && (AInputChord[ChordIndex] != END_OF_STRING))
                    ResultChord[ResultChordIndex++] = AInputChord[ChordIndex++];

                if (AInputChord[ChordIndex] == END_OF_STRING)
                    return ResultChord.ToString().Trim();

                ChordIndex++;
                ResultChord[ResultChordIndex++] = '/';
            }
        }
    }
}
