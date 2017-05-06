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
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Achordeon.Lib.Helpers
{
    public static class LibVersion
    {
        public static string About
        {
            get
            {
                var sb = new StringBuilder();
                sb.AppendLine($"About {ProductName}...");
                sb.AppendLine("(C) 2017 by Wolf Robben, licensed unter MIT License");
                sb.AppendLine(@"Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the ""Software""), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.");
                return sb.ToString();
            }
        }

        public static string ProductName
        {
            get
            {
                const string DEFAULT = "Achordeon";
                try
                {
                    var a = Assembly.GetAssembly(typeof(LibVersion));
                    if (a?.Location == null)
                        return DEFAULT;
                    var vi = FileVersionInfo.GetVersionInfo(a.Location);
                    return vi.ProductName ?? DEFAULT;
                }
                catch
                {
                    return DEFAULT;
                }
            }
        }

        public static string Version
        {
            get
            {
                try
                {
                    var a = Assembly.GetAssembly(typeof(LibVersion));
                    if (a?.Location == null)
                        return "?.?.?"; 
                    var vi = FileVersionInfo.GetVersionInfo(a.Location);                    
                    return $"{vi.ProductMajorPart}.{vi.ProductMinorPart}.{vi.ProductBuildPart}";
                }
                catch
                {
                    return "?.?.?"; 
                }
            }
        }

        public static string Copyright
        {
            get
            {
                try
                {
                    var a = Assembly.GetAssembly(typeof(LibVersion));
                    if (a?.Location == null)
                        return string.Empty;
                    var vi = FileVersionInfo.GetVersionInfo(a.Location);
                    return vi.LegalCopyright;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
    }
}
