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
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Achordeon.Common.Extensions;

namespace Achordeon.Shell.Wpf.Helpers.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        public static BoolToVisibilityConverter Default { get; } = new BoolToVisibilityConverter();

        public object Convert(object AValue, Type ATargetType, object AParameter, CultureInfo ACulture)
        {
            var IsVisible = (bool?)AValue;
            if (IsInvertParameterSet(AParameter))
            {
                var IsInverted = IsVisible;
                return (Visibility)((!IsInverted.GetValueOrDefault() ? 0 : (IsInverted.HasValue ? 1 : 0)) != 0 ? 2 : 0);
            }
            var IsVisible2 = IsVisible;
            return (Visibility)((!IsVisible2.GetValueOrDefault() ? 0 : (IsVisible2.HasValue ? 1 : 0)) != 0 ? 0 : 2);
        }

        
        public object ConvertBack(object AValue, Type ATargetType, object AParameter, CultureInfo ACulture)
        {
            var Visibility = (Visibility)AValue;
            if (IsInvertParameterSet(AParameter))
                return Visibility != Visibility.Visible;
            return Visibility == Visibility.Visible;
        }

        private static bool IsInvertParameterSet(object AParameter)
        {
            var StringValue = AParameter as string;
            return !string.IsNullOrWhiteSpace(StringValue) && StringValue.CiEquals("invert");
        }
    }
}
