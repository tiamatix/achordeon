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

using System.Diagnostics;
using System.Windows.Media;
using Achordeon.Annotations;
using Achordeon.Common.Extensions;

namespace Achordeon.Shell.Wpf.Helpers.FontViewModel
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class FontFamilyInfo
    {
        public FontFamilyInfo(FontFamily AFamily, bool AIsBuildIn, string ADisplayText)
        {
            AFamily.ThrowIfNullEx(nameof(AFamily));
            ADisplayText.ThrowIfNullEx(nameof(AFamily));
            Family = AFamily;
            IsBuildIn = AIsBuildIn;
            DisplayText = ADisplayText;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => $"{GetType().Name}, Display:{DisplayText}, BuildIn={IsBuildIn}, Source={Source}";

        [NotNull]
        public FontFamily Family { get; }

        [NotNull]
        public string DisplayText { get; }

        public string Source => Family.Source;

        public bool IsBuildIn { get; }

        public bool IsSystem => !IsBuildIn;
    }
}
