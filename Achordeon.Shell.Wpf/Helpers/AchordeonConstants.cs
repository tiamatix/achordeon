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
using System.IO;
using System.Reflection;

namespace Achordeon.Shell.Wpf.Helpers
{
    public class LanguageInfo
    {
        public LanguageInfo(string ALanguageCode, string ADisplayName)
        {
            LanguageCode = ALanguageCode;
            DisplayName = ADisplayName;
        }

        public string LanguageCode { get; }
        public string DisplayName { get; }
    }

    public static class AchordeonConstants
    {
        public const string ACHORDEON = "Achordeon";
        
        public static readonly string SettingsFolderName = ACHORDEON;
        public static readonly string SettingsFileRootNodeName = ACHORDEON + "Settings";
        public static readonly string SettingsFileName = ACHORDEON + ".Settings.xml";
                
        public static readonly string ApplicationDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), SettingsFolderName);
        public static readonly string SettingsFileFullPath = Path.Combine(ApplicationDataDirectory, SettingsFileName);

        public const string DEFAULT_LANGUAGE = "en-EN";
        public const string GITHUB = "https://github.com/Wox-launcher/Wox";
        public const string ISSUE = "https://github.com/Wox-launcher/Wox/issues/new";

        private static readonly Assembly _Assembly = Assembly.GetExecutingAssembly();
        public static readonly string Version = FileVersionInfo.GetVersionInfo(_Assembly.Location).ProductVersion;

        public static readonly IEnumerable<string> SupportedPaperSizes = new List<string>(new[] {"A4", "A5", "Letter"});

        public static readonly IEnumerable<LanguageInfo> SupportedLanguages = new List<LanguageInfo>(new[]
        {
            new LanguageInfo(DEFAULT_LANGUAGE, "English"),
            new LanguageInfo("de-DE", "Deutsch")
        });
    }
}
