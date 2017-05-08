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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media;
using Achordeon.Annotations;
using Achordeon.Common.Extensions;
using Achordeon.Common.Helpers;
using Achordeon.Shell.Wpf.Contents;
using Common.Logging;
using DryIoc.Experimental;

namespace Achordeon.Shell.Wpf.Helpers.FontViewModel
{
    public class FontViewModel : ViewModelBase
    {
        public const string BUILDIN_CONSOLA = "Consola Mono"; //Must match exact font name in resource!
        public const string BUILDIN_SERIFE = "Source Serif Pro";//Must match exact font name in resource!
        public const string BUILDIN_CODEPRO = "Source Code Pro";//Must match exact font name in resource!

        private readonly CoreViewModel m_Core;
        private bool m_PreloadComplete;
        private ObservableCollection<FontFamilyInfo> m_AllFonts = new ObservableCollection<FontFamilyInfo>();
        private static readonly FontFamilyInfo _UltimateFallbackFont;

        static FontViewModel()
        {
            var FallbackFont = new FontFamily("Times New Roman");
            _UltimateFallbackFont = new FontFamilyInfo(FallbackFont, false, FallbackFont.Source);
        }

        public static FontFamilyInfo UltimateFallbackFont => _UltimateFallbackFont;

        private ILog Log => m_Core.IoC.Get<Log>().Using(this);

        public FontViewModel(CoreViewModel ACore)
        {
            ACore.ThrowIfNullEx(nameof(ACore));
            m_Core = ACore;            
        }

        public ObservableCollection<FontFamilyInfo> AllFonts
        {
            get
            {
                PreloadFonts();
                return m_AllFonts;
            }
            set { SetProperty(ref m_AllFonts, value, nameof(AllFonts)); }
        }

        private void PreloadFonts()
        {
            if (m_PreloadComplete)
                return;
            m_PreloadComplete = true;
            Log.Trace(nameof(PreloadFonts) + ", system fonts");
            Fonts.SystemFontFamilies.Select(a => new FontFamilyInfo(a, false, a.Source)).ToList().ForEach(a => m_AllFonts.Add(a));
            Log.Trace(nameof(PreloadFonts) + ", build-in fonts");
            PreloadFont(BUILDIN_CONSOLA);
            PreloadFont(BUILDIN_SERIFE);
            PreloadFont(BUILDIN_CODEPRO);
        }

        private static string GetBuildinFontName(string AResourceName)
        {
            return Regex.Match(AResourceName, @"#\s*(?<name>[^,]+)\s*,").Groups["name"].Value.Trim();
        }
        
        private void PreloadFont(string AFontName)
        {
            if (m_AllFonts.Any(a => a.DisplayText.CiEquals(AFontName) || a.Family.Source.CiEquals(AFontName)))
                return;
            try
            {
                var UriName = $"./resources/#{AFontName} , Arial";
                Log.TraceFormat(nameof(PreloadFont) + ", build-in font '{0}'", UriName);
                var Font = new FontFamily(new Uri("pack://application:,,,/"), UriName);
                m_AllFonts.Add(new FontFamilyInfo(Font, true, GetBuildinFontName(Font.Source)));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        [NotNull]
        public FontFamilyInfo GetFontInfo(string AFontName)
        {
            PreloadFonts();
            return AllFonts.FirstOrDefault(a => a.DisplayText.CiEquals(AFontName) || a.Family.Source.CiEquals(AFontName)) ?? _UltimateFallbackFont;
        }
    
    }
}
