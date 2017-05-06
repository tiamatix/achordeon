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
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace Achordeon.Shell.Wpf.Helpers.Converters
{
    public class MenuFileNameConverter : IValueConverter
    {
        private const int MAX_CHARACTERS = 40;


        public object Convert(object AValue, Type ATargetType, object AParameter, CultureInfo ACulture)
        {
            var FileName = AValue as string;
            if (!string.IsNullOrWhiteSpace(FileName))
            {
                FileName = Path.GetFileName(FileName);
                if (FileName.Length <= MAX_CHARACTERS)
                    return FileName;
                return FileName.Remove(MAX_CHARACTERS - 3) + "...";
            }
            return string.Empty;
        }

        public object ConvertBack(object AValue, Type ATargetType, object AParameter, CultureInfo ACulture)
        {
            throw new NotSupportedException();
        }
    }
}
