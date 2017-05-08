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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Achordeon.Shell.Wpf.Helpers
{
    public static class AchordeonConstants
    {
        public const string ACHORDEON = "Achordeon";
        
        public static readonly string SettingsFolderName = ACHORDEON;
        public static readonly string SettingsFileRootNodeName = ACHORDEON + "Settings";
        public static readonly string SettingsFileName = ACHORDEON + ".Settings.xml";

        public const string GITHUB_OWNER = "tiamatix";
        public const string GITHUB_REPO = "achordeon";
        public static readonly string GithubBase = $"https://github.com/{GITHUB_OWNER}/{GITHUB_REPO}";
        public static readonly string GithubApiBase = $"https://api.github.com/repos/{GITHUB_OWNER}/{GITHUB_REPO}";
        public static readonly string GithubLatestReleaseNotes = GithubBase + "/releases/latest";
        public static readonly string GithubLatestReleaseApi = GithubApiBase + "/releases/latest";
        public static readonly string GithubLicense = GithubBase + "/blob/master/LICENSE";
        public static readonly string GithubProjectLink = GithubBase;
        public static readonly string GithubIssueTracker = GithubBase + @"/issues/new";

        public static readonly string ApplicationDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), SettingsFolderName);
        public static readonly string SettingsFileFullPath = Path.Combine(ApplicationDataDirectory, SettingsFileName);

        public static readonly LanguageInfo DefaultLanguage = new LanguageInfo("en-EN", "English");       

        private static readonly Assembly _Assembly = Assembly.GetExecutingAssembly();
        public static readonly string Version = FileVersionInfo.GetVersionInfo(_Assembly.Location).ProductVersion;

        public static readonly IEnumerable<string> SupportedPaperSizes = new List<string>(new[] {"A4", "A5", "Letter"});


    }
}
