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
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Achordeon.Common.Extensions;
using Achordeon.Common.Helpers;
using Achordeon.Lib.SongOptions;
using Achordeon.Shell.Wpf.Helpers;
using Achordeon.Shell.Wpf.Helpers.FontViewModel;
using Achordeon.Shell.Wpf.Helpers.RecentFileList;


namespace Achordeon.Shell.Wpf.Contents
{
    public class SettingsViewModel : ViewModelBase
    {
        private CoreViewModel m_Core;
        private double m_ChordProFontSize;
        private FontFamilyInfo m_ChordProFont;
        private RecentFileList m_RecentFileList;
        private WindowPosition m_MainWindowPosition;
        private double m_ChordProEditorSplitPosition;
        private LanguageInfo m_Language;
        private bool m_UsePdfPreview;
        private bool m_ShowLogger;
        private bool m_AutoUpdates;
        private SongOptionsViewModel m_GlobalSongOptions;

        public ObservableCollection<LanguageInfo> SupportedLanguages { get; } = new ObservableCollection<LanguageInfo>();

        public SettingsViewModel(CoreViewModel ACore)
        {
            Core = ACore;
            RecentFileList = new RecentFileList();
            MainWindowPosition = WindowPosition.Empty;
            ChordProEditorSplitPosition = 0;
            GlobalSongOptions = new SongOptionsViewModel(Core);
            SetupSupportedLanguages();
            Language = AchordeonConstants.DefaultLanguage;
            ResetToUserDefault();
        }

        private void SetupSupportedLanguages()
        {
            SupportedLanguages.Add(AchordeonConstants.DefaultLanguage);
            SupportedLanguages.Add(new LanguageInfo("de-DE", "Deutsch"));
        }

        public void ResetToUserDefault()
        {
            ChordProFont = Core.FontViewModel.GetFontInfo(FontViewModel.BUILDIN_SERIFE);
            ChordProFontSize = 16;
            UsePdfPreview = true;
            ShowLogger = false;
            AutoUpdates = true;
            GlobalSongOptions.ResetToDefaults();
        }

        public CoreViewModel Core
        {
            get { return m_Core; }
            set { SetProperty(ref m_Core, value, nameof(Core)); }
        }

        public SongOptionsViewModel GlobalSongOptions
        {
            get { return m_GlobalSongOptions; }
            set { SetProperty(ref m_GlobalSongOptions, value, nameof(GlobalSongOptions)); }
        }

        public LanguageInfo Language
        {
            get { return m_Language; }
            set { SetProperty(ref m_Language, value, nameof(Language)); }
        }

        public bool ShowLogger
        {
            get { return m_ShowLogger; }
            set { SetProperty(ref m_ShowLogger, value, nameof(ShowLogger)); }
        }

        public bool AutoUpdates
        {
            get { return m_AutoUpdates; }
            set { SetProperty(ref m_AutoUpdates, value, nameof(AutoUpdates)); }
        }

        public WindowPosition MainWindowPosition
        {
            get { return m_MainWindowPosition; }
            set { SetProperty(ref m_MainWindowPosition, value, nameof(MainWindowPosition)); }
        }

        public double ChordProFontSize
        {
            get { return m_ChordProFontSize; }
            set { SetProperty(ref m_ChordProFontSize, value, nameof(ChordProFontSize)); }
        }

        public double ChordProEditorSplitPosition
        {
            get { return m_ChordProEditorSplitPosition; }
            set { SetProperty(ref m_ChordProEditorSplitPosition, value, nameof(ChordProEditorSplitPosition)); }
        }

        public bool UsePdfPreview
        {
            get { return m_UsePdfPreview; }
            set { SetProperty(ref m_UsePdfPreview, value, nameof(UsePdfPreview)); }
        }

        public FontFamilyInfo ChordProFont
        {
            get { return m_ChordProFont; }
            set { SetProperty(ref m_ChordProFont, value, nameof(ChordProFont)); }
        }

        public RecentFileList RecentFileList
        {
            get { return m_RecentFileList; }
            set { SetProperty(ref m_RecentFileList, value, nameof(RecentFileList)); }
        }

        public void SaveSettings()
        {            
            if (!Directory.Exists(AchordeonConstants.ApplicationDataDirectory))
                Directory.CreateDirectory(AchordeonConstants.ApplicationDataDirectory);
            var Xml = XmlFile.CreateEmpty(AchordeonConstants.SettingsFileRootNodeName);
            var ChordProEditorNode = Xml.Add("ChordProEditor");
            ChordProEditorNode.Set("ChordProFontSize", ChordProFontSize);
            ChordProEditorNode.Set("ChordProFontName", ChordProFont.Source);
            ChordProEditorNode.Set("ChordProEditorSplitPosition", ChordProEditorSplitPosition);
            ChordProEditorNode.Set("UsePdfPreview", UsePdfPreview);
            RecentFileList.SaveToXml(Xml);            
            if (MainWindowPosition != WindowPosition.Empty)
            {
                var MainWindowPositionNode = Xml.Add("MainWindowPosition");
                MainWindowPosition.SaveToXml(MainWindowPositionNode);
            }
            var LoggerNode = Xml.Add("Logger");
            LoggerNode.Set("ShowLogger", ShowLogger);
            var GlobalSongOptionsNode = Xml.Add("GlobalSongOptions");
            new SongOptionsConverter(DefaultSongOptions.Default).SaveToXml(GlobalSongOptions, GlobalSongOptionsNode);
            var LanguageNode = Xml.Add("Language");
            LanguageNode.Set("SelectedLanguageCode", Language.LanguageCode);
            var AutoUpdatesNode = Xml.Add("AutoUpdates");
            AutoUpdatesNode.Set("EnableAutomaticUpdate", AutoUpdates);
            Xml.Save(AchordeonConstants.SettingsFileFullPath, true);
        }

        public void LoadSettings()
        {
            if (!File.Exists(AchordeonConstants.SettingsFileFullPath))
                return;
            var Xml = XmlFile.CreateFromFile(AchordeonConstants.SettingsFileFullPath);
            if (!Xml.Root.Name.LocalName.CiEquals(AchordeonConstants.SettingsFileRootNodeName))
                return;
            var ChordProEditorNode = Xml.SelectSingle("ChordProEditor");
            if (ChordProEditorNode != null)
            {
                ChordProFontSize = ChordProEditorNode.GetF("ChordProFontSize", ChordProFontSize);
                ChordProFont = Core.FontViewModel.GetFontInfo(ChordProEditorNode.Get("ChordProFontName", ChordProFont.Source));
                ChordProEditorSplitPosition = ChordProEditorNode.GetF("ChordProEditorSplitPosition", ChordProEditorSplitPosition);
                UsePdfPreview = ChordProEditorNode.GetB("UsePdfPreview", UsePdfPreview);
            }
            RecentFileList.LoadFromXml(Xml);
            var MainWindowPositionNode = Xml.SelectSingle("MainWindowPosition");
            if (MainWindowPositionNode != null)
                MainWindowPosition = new WindowPosition(MainWindowPositionNode);
            else
                MainWindowPosition = WindowPosition.Empty;
            var LoggerNode = Xml.SelectSingle("Logger");
            if (LoggerNode != null)
            {
                ShowLogger = LoggerNode.GetB("ShowLogger", ShowLogger);
            }
            var GlobalSongOptionsNode = Xml.SelectSingle("GlobalSongOptions");
            if (GlobalSongOptionsNode != null)
            {
                new SongOptionsConverter(DefaultSongOptions.Default).LoadFromXml(GlobalSongOptions, GlobalSongOptionsNode);
            }
            var LanguageNode = Xml.SelectSingle("Language");
            if (LanguageNode != null)
            {
                var LanguageCode = LanguageNode.Get("SelectedLanguageCode", Language.LanguageCode);
                Language = SupportedLanguages.FirstOrDefault(a => a.LanguageCode.CiEquals(LanguageCode)) ?? AchordeonConstants.DefaultLanguage;
            }
            var AutoUpdatesNode = Xml.SelectSingle("AutoUpdates");
            if (AutoUpdatesNode != null)
            {
                AutoUpdates = AutoUpdatesNode.GetB("EnableAutomaticUpdate", AutoUpdates);
            }
        }

    }
}
