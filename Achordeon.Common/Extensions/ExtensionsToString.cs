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

namespace Achordeon.Common.Extensions
{
    public static class ExtensionsToString
    {
    
        public static int CiCompare(this string AString, string AOther)
        {
            return string.Compare(AString, AOther, StringComparison.OrdinalIgnoreCase);
        }

        public static bool CiEquals(this string AString, string AOther)
        {
            return string.Compare(AString, AOther, StringComparison.OrdinalIgnoreCase) == 0;
        }

        public static int CiIndexOf(this string AString, string AOther)
        {
            return AString.IndexOf(AOther, StringComparison.OrdinalIgnoreCase);
        }

        public static int CiIndexOf(this string AString, string AOther, int AStartIndex)
        {
            return AString.IndexOf(AOther, AStartIndex, StringComparison.OrdinalIgnoreCase);
        }

        public static int CiLastIndexOf(this string AString, string AOther, int AStartIndex)
        {
            return AString.LastIndexOf(AOther, AStartIndex, StringComparison.OrdinalIgnoreCase);
        }

        public static int CiLastIndexOf(this string AString, string AOther)
        {
            return AString.LastIndexOf(AOther, StringComparison.OrdinalIgnoreCase);
        }

        public static bool CiContains(this string AString, string AOther)
        {
            return AString.CiIndexOf(AOther) >= 0;
        }

    
        public static string Reverse(this string AString)
        {
            var CharArray = AString.ToCharArray();
            Array.Reverse(CharArray);
            return new string(CharArray);
        }

        public static int IndexOf(this string AString, Func<char, bool> AFunc)
        {
            if (AString == null || AFunc == null)
                return -1;

            for (int i = 0; i < AString.Length; i++)
            {
                if (AFunc(AString[i]))
                    return i;
            }

            return -1;
        }
        
    }
}
