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
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Achordeon.Common.Extensions;

namespace Achordeon.Common.Helpers
{
    public static class TextEncodingDetector
    {
        private const long DEFAULT_SAMPLE_SIZE = 0x10000; //completely arbitrary - inappropriate for high numbers of files / high speed requirements

        public static Encoding Detect(string ATextData)
        {
            using (var strm = new StringStream(ATextData, true))
            {
                return Detect(strm);
            }
        }

        public static Encoding Detect(Stream AInputStream, long AHeuristicSampleSize = DEFAULT_SAMPLE_SIZE)
        {
            bool Dummy;
            return Detect(AInputStream, DEFAULT_SAMPLE_SIZE, out Dummy);
        }

        public static Encoding Detect(Stream AInputStream, long AHeuristicSampleSize, out bool AHasBom)
        {
            AInputStream.ThrowIfNullEx(nameof(AInputStream));

            if (!AInputStream.CanRead)
                throw new ArgumentException("Provided file stream is not readable!", nameof(AInputStream));

            if (!AInputStream.CanSeek)
                throw new ArgumentException("Provided file stream cannot seek!", nameof(AInputStream));

            var OriginalPosition = AInputStream.Position;
            try
            {

                AInputStream.Position = 0;


                //First read only what we need for BOM detection
                byte[] BomBytes = new byte[AInputStream.Length > 4 ? 4 : AInputStream.Length];
                AInputStream.Read(BomBytes, 0, BomBytes.Length);

                var Result = DetectBomBytes(BomBytes);

                if (Result != null)
                {
                    AHasBom = true;
                    return Result;
                }


                //BOM Detection failed, going for heuristics now.
                //  create sample byte array and populate it
                byte[] SampleBytes = new byte[AHeuristicSampleSize > AInputStream.Length ? AInputStream.Length : AHeuristicSampleSize];
                Array.Copy(BomBytes, SampleBytes, BomBytes.Length);
                if (AInputStream.Length > BomBytes.Length)
                    AInputStream.Read(SampleBytes, BomBytes.Length, SampleBytes.Length - BomBytes.Length);

                //test byte array content
                Result = DetectUnicodeInByteSampleByHeuristics(SampleBytes);

                AHasBom = false;
                return Result;
            }
            finally
            {
                AInputStream.Position = OriginalPosition;

            }
        }

        public static Encoding Detect(byte[] ATextData)
        {
            bool Dummy;
            return Detect(ATextData, out Dummy);
        }

        public static Encoding Detect(byte[] ATextData, out bool AHasBom)
        {
            if (ATextData == null)
                throw new ArgumentNullException(nameof(ATextData));

            var Result = DetectBomBytes(ATextData);

            if (Result != null)
            {
                AHasBom = true;
                return Result;
            }

            //test byte array content
            Result = DetectUnicodeInByteSampleByHeuristics(ATextData);

            AHasBom = false;
            return Result;
        }





        private static Encoding DetectBomBytes(byte[] ABomBytes)
        {
            ABomBytes.ThrowIfNullEx(nameof(ABomBytes));

            if (ABomBytes.Length < 2)
                return null;

            if (ABomBytes[0] == 0xff
                && ABomBytes[1] == 0xfe
                && (ABomBytes.Length < 4
                    || ABomBytes[2] != 0
                    || ABomBytes[3] != 0
                    )
                )
                return Encoding.Unicode;

            if (ABomBytes[0] == 0xfe
                && ABomBytes[1] == 0xff
                )
                return Encoding.BigEndianUnicode;

            if (ABomBytes.Length < 3)
                return null;

            if (ABomBytes[0] == 0xef && ABomBytes[1] == 0xbb && ABomBytes[2] == 0xbf)
                return Encoding.UTF8;

            if (ABomBytes[0] == 0x2b && ABomBytes[1] == 0x2f && ABomBytes[2] == 0x76)
                return Encoding.UTF7;

            if (ABomBytes.Length < 4)
                return null;

            if (ABomBytes[0] == 0xff && ABomBytes[1] == 0xfe && ABomBytes[2] == 0 && ABomBytes[3] == 0)
                return Encoding.UTF32;

            if (ABomBytes[0] == 0 && ABomBytes[1] == 0 && ABomBytes[2] == 0xfe && ABomBytes[3] == 0xff)
                return Encoding.GetEncoding(12001);

            return null;
        }

        private static Encoding DetectUnicodeInByteSampleByHeuristics(byte[] ASampleBytes)
        {
            long OddBinaryNullsInSample = 0;
            long EvenBinaryNullsInSample = 0;
            long SuspiciousUtf8SequenceCount = 0;
            long SuspiciousUtf8BytesTotal = 0;
            long LikelyUsasciiBytesInSample = 0;

            //Cycle through, keeping count of binary null positions, possible UTF-8 
            //  sequences from upper ranges of Windows-1252, and probable US-ASCII 
            //  character counts.

            long CurrentPosition = 0;
            int SkippedUtf8Bytes = 0;

            while (CurrentPosition < ASampleBytes.Length)
            {
                //binary null distribution
                if (ASampleBytes[CurrentPosition] == 0)
                {
                    if (CurrentPosition % 2 == 0)
                        EvenBinaryNullsInSample++;
                    else
                        OddBinaryNullsInSample++;
                }

                //likely US-ASCII characters
                if (IsCommonUsAsciiByte(ASampleBytes[CurrentPosition]))
                    LikelyUsasciiBytesInSample++;

                //suspicious sequences (look like UTF-8)
                if (SkippedUtf8Bytes == 0)
                {
                    var DetectSuspiciousUtf8SequenceLength = TextEncodingDetector.DetectSuspiciousUtf8SequenceLength(ASampleBytes, CurrentPosition);

                    if (DetectSuspiciousUtf8SequenceLength > 0)
                    {
                        SuspiciousUtf8SequenceCount++;
                        SuspiciousUtf8BytesTotal += DetectSuspiciousUtf8SequenceLength;
                        SkippedUtf8Bytes = DetectSuspiciousUtf8SequenceLength - 1;
                    }
                }
                else
                {
                    SkippedUtf8Bytes--;
                }

                CurrentPosition++;
            }

            //1: UTF-16 LE - in english / european environments, this is usually characterized by a 
            //  high proportion of odd binary nulls (starting at 0), with (as this is text) a low 
            //  proportion of even binary nulls.
            //  The thresholds here used (less than 20% nulls where you expect non-nulls, and more than
            //  60% nulls where you do expect nulls) are completely arbitrary.

            if (((EvenBinaryNullsInSample * 2.0) / ASampleBytes.Length) < 0.2
                && ((OddBinaryNullsInSample * 2.0) / ASampleBytes.Length) > 0.6
                )
                return Encoding.Unicode;


            //2: UTF-16 BE - in english / european environments, this is usually characterized by a 
            //  high proportion of even binary nulls (starting at 0), with (as this is text) a low 
            //  proportion of odd binary nulls.
            //  The thresholds here used (less than 20% nulls where you expect non-nulls, and more than
            //  60% nulls where you do expect nulls) are completely arbitrary.

            if (((OddBinaryNullsInSample * 2.0) / ASampleBytes.Length) < 0.2
                && ((EvenBinaryNullsInSample * 2.0) / ASampleBytes.Length) > 0.6
                )
                return Encoding.BigEndianUnicode;


            //3: UTF-8 - Martin D�rst outlines a method for detecting whether something CAN be UTF-8 content 
            //  using regexp, in his w3c.org unicode FAQ entry: 
            //  http://www.w3.org/International/questions/qa-forms-utf-8
            //  adapted here for C#.
            string PotentiallyMangledString = Encoding.ASCII.GetString(ASampleBytes);
            Regex Utf8Validator = new Regex(@"\A("
                                            + @"[\x09\x0A\x0D\x20-\x7E]"
                                            + @"|[\xC2-\xDF][\x80-\xBF]"
                                            + @"|\xE0[\xA0-\xBF][\x80-\xBF]"
                                            + @"|[\xE1-\xEC\xEE\xEF][\x80-\xBF]{2}"
                                            + @"|\xED[\x80-\x9F][\x80-\xBF]"
                                            + @"|\xF0[\x90-\xBF][\x80-\xBF]{2}"
                                            + @"|[\xF1-\xF3][\x80-\xBF]{3}"
                                            + @"|\xF4[\x80-\x8F][\x80-\xBF]{2}"
                                            + @")*\z");
            if (Utf8Validator.IsMatch(PotentiallyMangledString))
            {
                //Unfortunately, just the fact that it CAN be UTF-8 doesn't tell you much about probabilities.
                //If all the characters are in the 0-127 range, no harm done, most western charsets are same as UTF-8 in these ranges.
                //If some of the characters were in the upper range (western accented characters), however, they would likely be mangled to 2-byte by the UTF-8 encoding process.
                // So, we need to play stats.

                // The "Random" likelihood of any pair of randomly generated characters being one 
                //   of these "suspicious" character sequences is:
                //     128 / (256 * 256) = 0.2%.
                //
                // In western text data, that is SIGNIFICANTLY reduced - most text data stays in the <127 
                //   character range, so we assume that more than 1 in 500,000 of these character 
                //   sequences indicates UTF-8. The number 500,000 is completely arbitrary - so sue me.
                //
                // We can only assume these character sequences will be rare if we ALSO assume that this
                //   IS in fact western text - in which case the bulk of the UTF-8 encoded data (that is 
                //   not already suspicious sequences) should be plain US-ASCII bytes. This, I 
                //   arbitrarily decided, should be 80% (a random distribution, eg binary data, would yield 
                //   approx 40%, so the chances of hitting this threshold by accident in random data are 
                //   VERY low). 

                if ((SuspiciousUtf8SequenceCount * 500000.0 / ASampleBytes.Length >= 1) //suspicious sequences
                    && (
                        //all suspicious, so cannot evaluate proportion of US-Ascii
                        ASampleBytes.Length - SuspiciousUtf8BytesTotal == 0
                        ||
                        LikelyUsasciiBytesInSample * 1.0 / (ASampleBytes.Length - SuspiciousUtf8BytesTotal) >= 0.8
                        )
                    )
                    return Encoding.UTF8;
            }

            return null;
        }

        private static bool IsCommonUsAsciiByte(byte ATestByte)
        {
            if (ATestByte == 0x0A //lf
                || ATestByte == 0x0D //cr
                || ATestByte == 0x09 //tab
                || (ATestByte >= 0x20 && ATestByte <= 0x2F) //common punctuation
                || (ATestByte >= 0x30 && ATestByte <= 0x39) //digits
                || (ATestByte >= 0x3A && ATestByte <= 0x40) //common punctuation
                || (ATestByte >= 0x41 && ATestByte <= 0x5A) //capital letters
                || (ATestByte >= 0x5B && ATestByte <= 0x60) //common punctuation
                || (ATestByte >= 0x61 && ATestByte <= 0x7A) //lowercase letters
                || (ATestByte >= 0x7B && ATestByte <= 0x7E) //common punctuation
                )
                return true;
            else
                return false;
        }

        private static int DetectSuspiciousUtf8SequenceLength(byte[] ASampleBytes, long ACurrentPos)
        {
            int Result = 0;

            if (ASampleBytes.Length >= ACurrentPos + 1
                && ASampleBytes[ACurrentPos] == 0xC2
                )
            {
                if (ASampleBytes[ACurrentPos + 1] == 0x81
                    || ASampleBytes[ACurrentPos + 1] == 0x8D
                    || ASampleBytes[ACurrentPos + 1] == 0x8F
                    )
                    Result = 2;
                else if (ASampleBytes[ACurrentPos + 1] == 0x90
                         || ASampleBytes[ACurrentPos + 1] == 0x9D
                    )
                    Result = 2;
                else if (ASampleBytes[ACurrentPos + 1] >= 0xA0
                         && ASampleBytes[ACurrentPos + 1] <= 0xBF
                    )
                    Result = 2;
            }
            else if (ASampleBytes.Length >= ACurrentPos + 1
                     && ASampleBytes[ACurrentPos] == 0xC3
                )
            {
                if (ASampleBytes[ACurrentPos + 1] >= 0x80
                    && ASampleBytes[ACurrentPos + 1] <= 0xBF
                    )
                    Result = 2;
            }
            else if (ASampleBytes.Length >= ACurrentPos + 1
                     && ASampleBytes[ACurrentPos] == 0xC5
                )
            {
                if (ASampleBytes[ACurrentPos + 1] == 0x92
                    || ASampleBytes[ACurrentPos + 1] == 0x93
                    )
                    Result = 2;
                else if (ASampleBytes[ACurrentPos + 1] == 0xA0
                         || ASampleBytes[ACurrentPos + 1] == 0xA1
                    )
                    Result = 2;
                else if (ASampleBytes[ACurrentPos + 1] == 0xB8
                         || ASampleBytes[ACurrentPos + 1] == 0xBD
                         || ASampleBytes[ACurrentPos + 1] == 0xBE
                    )
                    Result = 2;
            }
            else if (ASampleBytes.Length >= ACurrentPos + 1
                     && ASampleBytes[ACurrentPos] == 0xC6
                )
            {
                if (ASampleBytes[ACurrentPos + 1] == 0x92)
                    Result = 2;
            }
            else if (ASampleBytes.Length >= ACurrentPos + 1
                     && ASampleBytes[ACurrentPos] == 0xCB
                )
            {
                if (ASampleBytes[ACurrentPos + 1] == 0x86
                    || ASampleBytes[ACurrentPos + 1] == 0x9C
                    )
                    Result = 2;
            }
            else if (ASampleBytes.Length >= ACurrentPos + 2
                     && ASampleBytes[ACurrentPos] == 0xE2
                )
            {
                if (ASampleBytes[ACurrentPos + 1] == 0x80)
                {
                    if (ASampleBytes[ACurrentPos + 2] == 0x93
                        || ASampleBytes[ACurrentPos + 2] == 0x94
                        )
                        Result = 3;
                    if (ASampleBytes[ACurrentPos + 2] == 0x98
                        || ASampleBytes[ACurrentPos + 2] == 0x99
                        || ASampleBytes[ACurrentPos + 2] == 0x9A
                        )
                        Result = 3;
                    if (ASampleBytes[ACurrentPos + 2] == 0x9C
                        || ASampleBytes[ACurrentPos + 2] == 0x9D
                        || ASampleBytes[ACurrentPos + 2] == 0x9E
                        )
                        Result = 3;
                    if (ASampleBytes[ACurrentPos + 2] == 0xA0
                        || ASampleBytes[ACurrentPos + 2] == 0xA1
                        || ASampleBytes[ACurrentPos + 2] == 0xA2
                        )
                        Result = 3;
                    if (ASampleBytes[ACurrentPos + 2] == 0xA6)
                        Result = 3;
                    if (ASampleBytes[ACurrentPos + 2] == 0xB0)
                        Result = 3;
                    if (ASampleBytes[ACurrentPos + 2] == 0xB9
                        || ASampleBytes[ACurrentPos + 2] == 0xBA
                        )
                        Result = 3;
                }
                else if (ASampleBytes[ACurrentPos + 1] == 0x82
                         && ASampleBytes[ACurrentPos + 2] == 0xAC
                    )
                    Result = 3;
                else if (ASampleBytes[ACurrentPos + 1] == 0x84
                         && ASampleBytes[ACurrentPos + 2] == 0xA2
                    )
                    Result = 3;
            }

            return Result;
        }

    }
}
