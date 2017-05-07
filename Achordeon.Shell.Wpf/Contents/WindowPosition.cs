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
using System.Windows;
using Achordeon.Common.Helpers;

namespace Achordeon.Shell.Wpf.Contents
{
    public class WindowPosition : IComparable<WindowPosition>, IEquatable<WindowPosition>
    {
        public double Top { get; private set; }

        public double Left { get; private set; }

        public double Height { get; private set; }

        public double Width { get; private set; }

        public WindowState State { get; private set; }

        public WindowPosition(Window AWindow)
        {
            Top = AWindow.Top;
            Left = AWindow.Left;
            Width = AWindow.Width;
            Height = AWindow.Height;
            State = AWindow.WindowState;
        }

        public WindowPosition(XmlFile AXml)
        {
            LoadFromXml(AXml);
        }

        private WindowPosition()
        {
            Top =
                Left =
                    Width =
                        Height = 0;
            State = WindowState.Minimized;
        }

        public static readonly WindowPosition Empty = new WindowPosition();

        /// <summary>
        /// If the saved window dimensions are larger than the current screen shrink the
        /// window to fit.
        /// </summary>
        public void SizeToFit()
        {
            if (Height > SystemParameters.VirtualScreenHeight)
                Height = SystemParameters.VirtualScreenHeight;
            if (Width > SystemParameters.VirtualScreenWidth)
                Width = SystemParameters.VirtualScreenWidth;
        }

        /// <summary>
        /// If the window is more than half off of the screen move it up and to the left 
        /// so half the height and half the width are visible.
        /// </summary>
        public void MoveIntoView()
        {
            if (Top + Height / 2 > SystemParameters.VirtualScreenHeight)
                Top = SystemParameters.VirtualScreenHeight - Height;

            if (Left + Width / 2 > SystemParameters.VirtualScreenWidth)
                Left = SystemParameters.VirtualScreenWidth - Width;

            if (Top < 0)
                Top = 0;

            if (Left < 0)
                Left = 0;
        }

        public void LoadFromXml(XmlFile AXml)
        {
            var NewWindowState = AXml.GetEnum("WindowState", State);
            if (NewWindowState == WindowState.Minimized)
                return;
            State = NewWindowState;
            Top = AXml.GetF("Top", Top);
            Left = AXml.GetF("Left", Left);
            Height = AXml.GetF("Height", Height);
            Width = AXml.GetF("Width", Width);
        }

        public void SaveToXml(XmlFile AXml)
        {
            if (State != WindowState.Minimized)
            {
                AXml.Set("Top", Top);
                AXml.Set("Left", Left);
                AXml.Set("Height", Height);
                AXml.Set("Width", Width);
            }
            AXml.SetEnum("State", State);
        }

        public void ApplyTo(Window AWindow)
        {
            AWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if (this == Empty)
                return;
            if (State == WindowState.Minimized)
                return;
            SizeToFit();
            MoveIntoView();
            AWindow.WindowStartupLocation = WindowStartupLocation.Manual;
            AWindow.Left = Left;
            AWindow.Top = Top;
            AWindow.Width = Width;
            AWindow.Height = Height;
        }

        public int CompareTo(WindowPosition AOther)
        {
            unchecked
            {
                return Top.CompareTo(AOther.Top) +
                       Left.CompareTo(AOther.Left) +
                       Height.CompareTo(AOther.Height) +
                       Width.CompareTo(AOther.Width) +
                       State.CompareTo(AOther.State);
            }
        }

        public bool Equals(WindowPosition AOther)
        {
            return CompareTo(AOther) == 0;
        }
    }
}
