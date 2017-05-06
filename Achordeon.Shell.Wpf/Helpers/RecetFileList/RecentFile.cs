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
using System.ComponentModel;
using Achordeon.Common.Helpers;
using Achordeon.Shell.Wpf.Contents;

namespace Achordeon.Shell.Wpf.Helpers.RecetFileList
{
    public sealed class RecentFile : ViewModelBase
    {
        private bool m_IsPinned;

        public bool IsPinned
        {
            get
            {
                return m_IsPinned;
            }
            set
            {
                SetProperty(ref m_IsPinned, value, nameof(IsPinned));
            }
        }

        public string Path { get; private set; }


        public RecentFile(XmlFile AXml)
        {
            LoadFromXml(AXml);
        }

        public RecentFile(string APath)
        {
            if (string.IsNullOrEmpty(APath))
                throw new ArgumentException("The argument path must not be null or empty.");
            this.Path = APath;
        }

        public void SaveToXml(XmlFile AXml)
        {
            AXml.Set("Path", Path);
            if (IsPinned)
                AXml.Set("IsPinned", IsPinned);
        }

        public void LoadFromXml(XmlFile AXml)
        {
            Path = AXml.Get("Path", Path);
            IsPinned = AXml.GetB("IsPinned", IsPinned);
        }

    }
}
