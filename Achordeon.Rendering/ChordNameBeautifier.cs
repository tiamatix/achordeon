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
using System.Linq;

namespace Achordeon.Lib.DataModel
{
    public static class ChordNameBeautifier
    {
        private static bool ReplaceEnd(ref string AName, string AReplacement)
        {
            if (AName.EndsWith(AReplacement))
            {
                AName = AName.Substring(0, AName.Length - AReplacement.Length) + "_" + AReplacement;
                return true;
            }
            return false;
        }

        private static bool ReplaceEnd(ref string AName, string AEnding, string AReplacement)
        {
            if (AName.EndsWith(AEnding))
            {
                AName = AName.Substring(0, AName.Length - AEnding.Length) + AReplacement;
                return true;
            }
            return false;
        }

        private static string DoNamePart(string AName)
        {
            if (AName.Length == 1)
                return AName;
            if (AName.Length == 2)
            {
                if (char.IsDigit(AName[1]))
                {
                    if (char.IsLetter(AName[0]))
                        return AName[0] + "_" + AName[1];
                }
                switch (AName[1])
                {
                    case 'b':
                    case '#':
                    case 'm':
                        return AName;
                    case '+':
                    case '-':
                    case '*':
                    case '~':
                        return AName[0] + "_" + AName[1];
                }
            }
            if (AName.Length == 3)
            {
                if (char.IsDigit(AName[1]) && char.IsDigit(AName[2]))
                    return AName[0] + "_" + AName[1] + AName[2];
            }
            if ((AName.Count(a => a == '(') == 1) && (AName.Count(a => a == ')') == 1))
            {
                AName = string.Join("_", AName.Replace(")", "").Split('('));
                return AName;
            }
            if (ReplaceEnd(ref AName, "sus"))
                return AName;
            if (ReplaceEnd(ref AName, "sus2"))
                return AName;
            if (ReplaceEnd(ref AName, "sus4"))
                return AName;
            if (ReplaceEnd(ref AName, "sus9"))
                return AName;
            if (ReplaceEnd(ref AName, "maj"))
                return AName;
            if (ReplaceEnd(ref AName, "maj7", "maj_7"))
                return AName;
            if (ReplaceEnd(ref AName, "maj9", "maj_9"))
                return AName;
            if (ReplaceEnd(ref AName, "m6", "m_6"))
                return AName;
            if (ReplaceEnd(ref AName, "m7", "m_7"))
                return AName;
            if (ReplaceEnd(ref AName, "m9", "m_9"))
                return AName;
            if (ReplaceEnd(ref AName, "add9"))
                return AName;
            if (ReplaceEnd(ref AName, "dim"))
                return AName;
            if (ReplaceEnd(ref AName, "min"))
                return AName;
            if (ReplaceEnd(ref AName, "#7", "#_7"))
                return AName;
            if (ReplaceEnd(ref AName, "#9", "#_9"))
                return AName;
            if (ReplaceEnd(ref AName, "7+"))
                return AName;
            if (ReplaceEnd(ref AName, "b9", "b_9"))
                return AName;
            if (ReplaceEnd(ref AName, "#+", "#_+"))
                return AName;
            return AName;
        }

        public static string Beautify(string AChordName)
        {
            //Trivial chord names
            if (!AChordName.Contains("/"))
                return DoNamePart(AChordName);

            var Parts = AChordName.Split(new[] {'/'});
            var res = string.Empty;
                
            for (int i = 0; i < Parts.Length; i++)
            {
                var LastPart = string.Empty;
                if (i > 0)
                    LastPart = Parts[i - 1];
                if (LastPart.Contains("_"))
                    res += "_";
                if (i > 0)
                    res += "/";
                res += Parts[i];

            }
            return res;
        }
    }
}
