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
#region MigraDoc - Creating Documents on the Fly
// MIT License
//
// Authors:
//   Stefan Lange
//   Klaus Potzesny
//   David Stephensen
//
// Copyright (c) 2001-2009 empira Software GmbH, Cologne (Germany)
//
// http://www.pdfsharp.com
// http://www.migradoc.com
// http://sourceforge.net/projects/pdfsharp

// Modifications by:
//   Thomas H�vel
//   Copyright (c) 2015 TH Software, Troisdorf (Germany), http://developer.th-soft.com/developer/
//
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
#endregion
using System;
using System.ComponentModel;
using System.Diagnostics;
using Achordeon.Common.Extensions;
using DryIoc;
using PdfSharp.Drawing;

namespace MigraDoc.DocumentObjectModel
{
    /// <summary>
    /// Provides functionality to measure the width of text during document design time.
    /// </summary>
    public sealed class TextMeasurement
    {
        /// <summary>
        /// Initializes a new instance of the TextMeasurement class with the specified font.
        /// </summary>
        public TextMeasurement(Font AFont)
        {
            AFont.ThrowIfNullEx(nameof(AFont));
            m_Font = AFont;
        }

        /// <summary>
        /// Returns the size of the bounding box of the specified text.
        /// </summary>
        public XSize MeasureString(string AText, UnitType AUnitType)
        {
            AText.ThrowIfNullEx(nameof(AText));

            if (!Enum.IsDefined(typeof(UnitType), AUnitType))
                throw new InvalidEnumArgumentException();

            var Graphics = Realize();

            var Size = Graphics.MeasureString(AText, m_GdiFont/*, new XPoint(0, 0), StringFormat.GenericTypographic*/);
            switch (AUnitType)
            {
                case UnitType.Point:
                    break;

                case UnitType.Centimeter:
                    Size.Width = (float)(Size.Width * 2.54 / 72);
                    Size.Height = (float)(Size.Height * 2.54 / 72);
                    break;

                case UnitType.Inch:
                    Size.Width = Size.Width / 72;
                    Size.Height = Size.Height / 72;
                    break;

                case UnitType.Millimeter:
                    Size.Width = (float)(Size.Width * 25.4 / 72);
                    Size.Height = (float)(Size.Height * 25.4 / 72);
                    break;

                case UnitType.Pica:
                    Size.Width = Size.Width / 12;
                    Size.Height = Size.Height / 12;
                    break;

                default:
                    Debug.Assert(false, "Missing unit type");
                    break;
            }
            return Size;
        }

        /// <summary>
        /// Returns the size of the bounding box of the specified text in point.
        /// </summary>
        public XSize MeasureString(string AText)
        {
            return MeasureString(AText, UnitType.Point);
        }

        /// <summary>
        /// Gets or sets the font used for measurement.
        /// </summary>
        public Font Font
        {
            get { return m_Font; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                if (m_Font != value)
                {
                    m_Font = value;
                    m_GdiFont = null;
                }
            }
        }
        Font m_Font;

        /// <summary>
        /// Initializes appropriate GDI+ objects.
        /// </summary>
        XGraphics Realize()
        {
            if (m_Graphics == null)
                m_Graphics = XGraphics.CreateMeasureContext(new XSize(2000, 2000), XGraphicsUnit.Point, XPageDirection.Downwards);

            //this.graphics.PageUnit = GraphicsUnit.Point;

            if (m_GdiFont == null)
            {
                var style = XFontStyle.Regular;
                if (m_Font.Bold)
                    style |= XFontStyle.Bold;
                if (m_Font.Italic)
                    style |= XFontStyle.Italic;

                m_GdiFont = new XFont(m_Font.Name, m_Font.Size, style/*, System.Drawing.GraphicsUnit.Point*/);
            }
            return m_Graphics;
        }

        XFont m_GdiFont;
        XGraphics m_Graphics;
    }
}
