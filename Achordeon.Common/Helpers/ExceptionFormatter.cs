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
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace Achordeon.Common.Helpers
{
    public class ExceptionFormatter
    {
        public static string FormatExcpetion(System.Exception AException)
        {
            var Result = new StringBuilder();
            Result.AppendLine();
            Result.AppendLine("## Exception");
            Result.AppendLine();
            Result.AppendLine("```");

            var ExceptionList = new List<StringBuilder>();

            while (AException != null)
            {
                var sb = new StringBuilder();
                sb.Append(AException.GetType().FullName);
                sb.Append(": ");
                sb.AppendLine(AException.Message);
                if (AException.Source != null)
                {
                    sb.Append("   Source: ");
                    sb.AppendLine(AException.Source);
                }
                if (AException.TargetSite != null)
                {
                    sb.Append("   TargetAssembly: ");
                    sb.AppendLine(AException.TargetSite.Module.Assembly.ToString());
                    sb.Append("   TargetModule: ");
                    sb.AppendLine(AException.TargetSite.Module.ToString());
                    sb.Append("   TargetSite: ");
                    sb.AppendLine(AException.TargetSite.ToString());
                }
                sb.AppendLine(AException.StackTrace);
                ExceptionList.Add(sb);

                AException = AException.InnerException;
            }

            foreach (var result in ExceptionList.Select(o => o.ToString()).Reverse())
                Result.AppendLine(result);

            Result.AppendLine("```");
            Result.AppendLine();

            Result.AppendLine("## Environment");
            Result.AppendLine();
            Result.Append("* Command Line: ");
            Result.AppendLine(Environment.CommandLine);
            Result.Append("* Timestamp: ");
            Result.AppendLine(DateTime.Now.ToString(CultureInfo.InvariantCulture));
            Result.Append("* IntPtr Length: ");
            Result.AppendLine(IntPtr.Size.ToString());
            Result.Append("* System Version: ");
            Result.AppendLine(Environment.OSVersion.VersionString);
            Result.Append("* CLR Version: ");
            Result.AppendLine(Environment.Version.ToString());
            Result.AppendLine("* Installed .NET Framework: ");
            foreach (var result in GetFrameworkVersionFromRegistry())
            {
                Result.Append("   * ");
                Result.AppendLine(result);
            }

            Result.AppendLine();
            Result.AppendLine("## Assemblies - " + AppDomain.CurrentDomain.FriendlyName);
            Result.AppendLine();
            foreach (var ass in AppDomain.CurrentDomain.GetAssemblies().OrderBy(o => o.GlobalAssemblyCache ? 50 : 0))
            {
                Result.Append("* ");
                Result.Append(ass.FullName);
                Result.Append(" (");

                if (ass.IsDynamic)
                {
                    Result.Append("dynamic assembly doesn't has location");
                }
                else if (string.IsNullOrEmpty(ass.Location))
                {
                    Result.Append("location is null or empty");

                }
                else
                {
                    Result.Append(ass.Location);

                }
                Result.AppendLine(")");
            }

            return Result.ToString();
        }

        // http://msdn.microsoft.com/en-us/library/hh925568%28v=vs.110%29.aspx
        private static List<string> GetFrameworkVersionFromRegistry()
        {
            try
            {
                var result = new List<string>();
                using (RegistryKey ndpKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
                {
                    foreach (string versionKeyName in ndpKey.GetSubKeyNames())
                    {
                        if (versionKeyName.StartsWith("v"))
                        {
                            RegistryKey versionKey = ndpKey.OpenSubKey(versionKeyName);
                            string name = (string)versionKey.GetValue("Version", "");
                            string sp = versionKey.GetValue("SP", "").ToString();
                            string install = versionKey.GetValue("Install", "").ToString();
                            if (install != "")
                                if (sp != "" && install == "1")
                                    result.Add(string.Format("{0} {1} SP{2}", versionKeyName, name, sp));
                                else
                                    result.Add(string.Format("{0} {1}", versionKeyName, name));

                            if (name != "")
                            {
                                continue;
                            }
                            foreach (string subKeyName in versionKey.GetSubKeyNames())
                            {
                                RegistryKey subKey = versionKey.OpenSubKey(subKeyName);
                                name = (string)subKey.GetValue("Version", "");
                                if (name != "")
                                    sp = subKey.GetValue("SP", "").ToString();
                                install = subKey.GetValue("Install", "").ToString();
                                if (install != "")
                                {
                                    if (sp != "" && install == "1")
                                        result.Add(string.Format("{0} {1} {2} SP{3}", versionKeyName, subKeyName, name, sp));
                                    else if (install == "1")
                                        result.Add(string.Format("{0} {1} {2}", versionKeyName, subKeyName, name));
                                }

                            }

                        }
                    }
                }
                using (RegistryKey ndpKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\"))
                {
                    int releaseKey = (int)ndpKey.GetValue("Release");
                    {
                        if (releaseKey == 378389)
                            result.Add("v4.5");

                        if (releaseKey == 378675)
                            result.Add("v4.5.1 installed with Windows 8.1");

                        if (releaseKey == 378758)
                            result.Add("4.5.1 installed on Windows 8, Windows 7 SP1, or Windows Vista SP2");
                    }
                }
                return result;
            }
            catch (System.Exception e)
            {
                Debug.WriteLine(e.ToString());
                return new List<string>();
            }

        }
    }
}
