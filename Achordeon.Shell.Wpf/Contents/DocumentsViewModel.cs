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
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using Achordeon.Common.Helpers;
using Achordeon.Lib.Properties;
using Achordeon.Shell.Wpf.Contents.ChordProFile;
using Achordeon.Shell.Wpf.Contents.Home;
using DryIoc.Experimental;

namespace Achordeon.Shell.Wpf.Contents
{
    public sealed class DocumentsViewModel : ViewModelBase
    {
        private ObservableCollection<DocumentsViewTabItem> m_OpenDocuments;
        private TabItem m_SelectedTab;
        private CoreViewModel m_Core;

        public CoreViewModel Core
        {
            get { return m_Core; }
            set { SetProperty(ref m_Core, value, nameof(Core)); }
        }

        public class DocumentsViewTabItem : TabItem
        {
            public IDocViewModel AssociatedModel { get; set; }
        }

        public TabItem SelectedTab
        {
            get { return m_SelectedTab; }
            set
            {
                var OldView = (SelectedTab as DocumentsViewTabItem)?.AssociatedModel;
                if (SetProperty(ref m_SelectedTab, value, nameof(SelectedTab)))
                {
                    if (OldView != null)
                        OldView.PropertyChanged -= CurrentSelectedDocViewPropertyChanged;
                    var NewView = (SelectedTab as DocumentsViewTabItem)?.AssociatedModel;
                    if (NewView != null)
                        NewView.PropertyChanged += CurrentSelectedDocViewPropertyChanged;
                    OnPropertyChanged(nameof(StatusBarText));
                }
            }
        }

        private void CurrentSelectedDocViewPropertyChanged(object ASender, System.ComponentModel.PropertyChangedEventArgs AArgs)
        {
            if (AArgs.PropertyName == nameof(IDocViewModel.StatusBarText))
                OnPropertyChanged(nameof(StatusBarText));
        }

        public string StatusBarText => (SelectedTab as DocumentsViewTabItem)?.AssociatedModel?.StatusBarText ?? string.Empty;

        public ObservableCollection<DocumentsViewTabItem> OpenDocuments
        {
            get { return m_OpenDocuments; }
            set { SetProperty(ref m_OpenDocuments, value, nameof(OpenDocuments)); }
        }

        public DocumentsViewModel(CoreViewModel ACore)
        {
            Core = ACore;
            OpenDocuments = new ObservableCollection<DocumentsViewTabItem>();
        }

        public IDocViewModel GetViewModel(string AUniqueKey)
        {
            return (GetTabItem(AUniqueKey) as DocumentsViewTabItem)?.AssociatedModel;
        }

        public TabItem GetTabItem(string AUniqueKey)
        {
            return OpenDocuments.FirstOrDefault(a => a.AssociatedModel?.UniqueKey == AUniqueKey);
        }

        public void AddDocumentView(IDocViewModel AViewModel)
        {
            var Key = AViewModel.UniqueKey;
            if (OpenDocuments.Any(c => c.AssociatedModel?.UniqueKey == Key))
                throw new DuplicateNameException($"A document with key '{Key}' is already open");
            var NewTab = new DocumentsViewTabItem
            {
                AssociatedModel = AViewModel,                
                Content = AViewModel.GetTabContent()
            };
            var binding = new Binding(nameof(IDocViewModel.TabHeaderText));
            binding.Source = AViewModel;
            binding.Mode = BindingMode.OneWay;
            NewTab.SetBinding(TabItem.HeaderProperty, binding);
            OpenDocuments.Add(NewTab);
        }

        public void OpenFretboardView()
        {
            var New = new FretboardViewModel(Core);
            if (GetTabItem(New.UniqueKey) == null)
                AddDocumentView(New);
            SelectedTab = GetTabItem(New.UniqueKey);
        }

        public void NewChordProFile()
        {
            var New = new ChordProFileViewModel(Core);
            var Counter = 0;
            New.FileName = $"Song {++Counter}.pro";
            while (GetViewModel(New.UniqueKey) != null)
                New.FileName = $"Song {++Counter}.pro";
            New.HasUnsavedChanges = false;
            AddDocumentView(New);
            SelectedTab = GetTabItem(New.UniqueKey);
        }

        public void OpenChordProFile(string AFileName)
        {
            if (GetViewModel(AFileName) != null)
                return;
            Encoding FileEncoding;
            string Content;
            using (var stream = new FileStream(AFileName, FileMode.Open))
            {
                FileEncoding = TextEncodingDetector.Detect(stream) ?? Encoding.Default;
                using (TextReader Reader = new StreamReader(stream, FileEncoding))
                {
                    Content = Reader.ReadToEnd();
                }
            }

            var New = new ChordProFileViewModel(Core, Content, FileEncoding);
            New.HasUnsavedChanges = false;
            New.FileName = AFileName;
            AddDocumentView(New);
            Core.SettingsViewModel.RecentFileList.AddFile(New.FileName);
            SelectedTab = GetTabItem(New.UniqueKey);
        }

        public void SaveChordProFile(ChordProFileViewModel AModel, string AOverrideFileName = null)
        {
            var FileName = AOverrideFileName ?? AModel.FileName;
            File.WriteAllText(FileName, AModel.Content, AModel.Encoding);
            AModel.FileName = FileName;
            AModel.HasUnsavedChanges = false;
            Core.SettingsViewModel.RecentFileList.AddFile(AModel.FileName);
        }

        public async void CloseDocument(IDocViewModel AModel)
        {
            var Tab = GetTabItem(AModel.UniqueKey) as DocumentsViewTabItem;
            if (Tab == null)
                return;
            var CloseArguments = new CloseEventArguments();
            Tab.AssociatedModel.BeforeClose(CloseArguments);
            if (CloseArguments.IsConfirmationRequired)
            {
                var BoxArguments = new ConfirmArguments(
                    Resources.Close,                    
                    CloseArguments.ConfirmationMessage + Environment.NewLine + Resources.Are_you_sure_,
                    Resources.Yes, 
                    Resources.No);
                await Core.IoC.Get<IMessageBoxService>().ConfirmAsync(BoxArguments);
                if (BoxArguments.Result ?? false)
                    CloseArguments.ConfirmClose();
            }
            if (!CloseArguments.CloseConfirmed)
                return;
            var cpv = (Tab.AssociatedModel as ChordProFileViewModel)?.GetTabContent() as ChordProFileView;
            if (cpv != null)
                Core.SettingsViewModel.ChordProEditorSplitPosition = cpv.SplitDistanceInPercent;
            OpenDocuments.Remove(Tab);
        }

        public async Task CloseAll(CloseEventArguments AArgs)
        {
            OpenDocuments.ToList().ForEach(a => a.AssociatedModel.BeforeClose(AArgs));
            if (!AArgs.IsConfirmationRequired)
            {
                await WaitFor.Nothing;
                AArgs.ConfirmClose();
            }
            else
            {
                var BoxArguments = new ConfirmArguments(
                    Resources.Close, 
                    AArgs.ConfirmationMessage + Environment.NewLine + Resources.Are_you_sure_,
                    Resources.Yes,
                    Resources.No);
                await Core.IoC.Get<IMessageBoxService>().ConfirmAsync(BoxArguments);
                if (BoxArguments.Result ?? false)
                    AArgs.ConfirmClose();
            }
        }
    }
}
