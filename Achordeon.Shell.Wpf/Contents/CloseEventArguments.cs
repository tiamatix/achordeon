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

namespace Achordeon.Shell.Wpf.Contents
{
    public class CloseEventArguments
    {
        private readonly StringBuilder m_ConfirmationMessageStringBuilder = new StringBuilder();

        public string ConfirmationMessage => m_ConfirmationMessageStringBuilder.ToString();

        public bool IsConfirmationRequired { get; private set; }

        public bool CloseConfirmed { get; private set; }

        public CloseEventArguments()
        {
            CloseConfirmed = true;
            IsConfirmationRequired = false;
        }

        public void AddConfirmationMessage(string AMessage)
        {
            CloseConfirmed = false;
            IsConfirmationRequired |= !string.IsNullOrWhiteSpace(AMessage);
            m_ConfirmationMessageStringBuilder.AppendLine(AMessage);
        }

        public void ConfirmClose()
        {
            CloseConfirmed = true;
        }
    }
}
