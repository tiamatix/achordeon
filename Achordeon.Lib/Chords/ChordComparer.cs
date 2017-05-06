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

namespace Achordeon.Lib.Chords
{
    public class ChordComparer : IComparer<string>
    {
        public int Compare(string AChordA, string AChordB)
        {
            //Chords must be terminated with the same suffix
            AChordA = AChordA + '\0';
            AChordB = AChordB + '\0';
            while (true)
            {
                var FirstCharA = char.ToLower(AChordA[0]);
                var FirstCharB = char.ToLower(AChordB[0]);

                if (FirstCharA != FirstCharB)
                    return FirstCharA.CompareTo(FirstCharB);

                var SecondCharA = char.ToLower(AChordA[1]);
                var SecondCharB = char.ToLower(AChordB[1]);

                switch (SecondCharA)
                {
                    case 'b':
                        switch (SecondCharB)
                        {
                            case 'b':
                                AChordA = AChordA.Substring(1);
                                AChordB = AChordB.Substring(1);
                                continue;
                            case '#':
                                return -1;
                            case 'm':
                                return -1;
                            default:
                                return -1;
                        }
                    case '#':
                        switch (SecondCharB)
                        {
                            case 'b':
                                return 1;
                            case '#':
                                AChordA = AChordA.Substring(1);
                                AChordB = AChordB.Substring(1);
                                continue;
                            case 'm':
                                return 1;
                            default:
                                return 1;
                        }
                    case 'm':
                        switch (SecondCharB)
                        {
                            case 'b':
                                return 1;
                            case '#':
                                return -1;
                            case 'm':
                                AChordA = AChordA.Substring(1);
                                AChordB = AChordB.Substring(1);
                                continue;
                            default:
                                return 1;
                        }
                    default:
                        switch (SecondCharB)
                        {
                            case 'b':
                                return 1;
                            case '#':
                                return -1;
                            case 'm':
                                return -1;
                            default:
                                return StringComparer.Ordinal.Compare(AChordA, AChordB);
                        }
                }
            }
        }
    }
}
