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
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Input;
using System.Windows.Threading;
using Achordeon.Common.Helpers;
using Achordeon.Lib.DataModel;
using Achordeon.Lib.Parser;
using Achordeon.Rendering.Ascii;
using Achordeon.Rendering.Pdf;
using Achordeon.Shell.Wpf.Helpers.Converters;
using Achordeon.Shell.Wpf.Properties;
using Common.Logging;
using DryIoc;
using DryIoc.Experimental;
using Microsoft.Win32;

namespace Achordeon.Shell.Wpf.Contents.ChordProFile
{
    public class ChordProFileViewModel : ViewModelBase, IDocViewModel
    {
        private Dispatcher m_Dispatcher;
        private CoreViewModel m_Core;
        private string m_FileName;
        private bool m_HasUnsavedChanges;
        private string m_Content;
        private Encoding m_Encoding;
        private ChordProFileView m_ChordProFileView;
        private LinkedSongOptionsViewModel m_SongOptionViewModel;
        private ZoomViewModel m_ZoomViewModel;
        private ICommand m_SavePdfCommand;
        private ICommand m_RunPdfCommand;
        private ICommand m_SaveCommand;
        private ICommand m_SaveAsCommand;
        private ICommand m_CloseCommand;
        private ICommand m_PrintCommand;
        private ICommand m_ImportPlainTextCommand;

        private ILog Log => Core.IoC.Get<Log>().Using(this);

        public ChordProFileViewModel(CoreViewModel ACore,
           string AContent,
           Encoding AEncoding)
        {
            Core = ACore;
            Dispatcher = Dispatcher.CurrentDispatcher;
            Content = AContent ?? Resources.EmptyFileTemplate;
            Encoding = AEncoding ?? Encoding.Default;
            ZoomViewModel = new ZoomViewModel(Core);
            ZoomViewModel.Dispatcher = Dispatcher;
            SaveCommand = new SimpleCommand(Save, CanSave);
            SaveAsCommand = new SimpleCommand(SaveAs, CanSaveAs);
            SavePdfCommand = new SimpleCommand(SavePdf);
            RunPdfCommand = new SimpleCommand(RunPdf);
            CloseCommand = new SimpleCommand(Close);
            PrintCommand = new SimpleCommand(Dummy);
            ImportPlainTextCommand = new SimpleCommand(ImportPlainText, CanImportPlainText);
            SongOptionViewModel = Core.SettingsViewModel.GlobalSongOptions.AsLinkedOptions();
        }

        public ChordProFileViewModel(CoreViewModel ACore) : this (ACore, null, null)
        {
        }

        private bool CanImportPlainText(object AParameter)
        {
            return false;
        }

        private void ImportPlainText(object AParameter)
        {
        }

        private bool CanSave(object AParameter)
        {
            return HasUnsavedChanges && !string.IsNullOrWhiteSpace(FileName) && File.Exists(FileName);
        }

        private bool CanSaveAs(object AParameter)
        {
            return !string.IsNullOrWhiteSpace(FileName);
        }

        private void Save(object AParameter)
        {
            var OverrideFileName = AParameter?.ToString();
            if (string.IsNullOrWhiteSpace(OverrideFileName))
            {
                OverrideFileName = null;
                if (!File.Exists(FileName))
                {
                    SaveAs(AParameter);
                    return;
                }
            }
            Core.DocumentsViewModel.SaveChordProFile(this, OverrideFileName);
            OnPropertyChanged(nameof(TabHeaderText));
        }

        private void SaveAs(object AParameter)
        {
            var NewFileName = AParameter?.ToString();
            if (string.IsNullOrWhiteSpace(NewFileName))
                NewFileName = FileName;
            var dialog = new SaveFileDialog();
            dialog.Filter = Resources.ChordProFileDialogFilter;
            dialog.FileName = NewFileName;
           var result = dialog.ShowDialog();
            if ((result.HasValue) && (result.Value))
                NewFileName = dialog.FileName;
            else
                NewFileName = null;
            if (string.IsNullOrWhiteSpace(NewFileName))
                return;
            Core.DocumentsViewModel.SaveChordProFile(this, NewFileName);
            OnPropertyChanged(nameof(TabHeaderText));
        }


        private void SavePdf(object AParameter)
        {
            try
            {
                SavePdf();
            }
            catch (Exception ex)
            {
                Log.Error("Exporting PDF file failed", ex);
                Core.IoC.Get<IMessageBoxService>().ShowErrorAsync("Export failed", ex.Message);
            }
        }

        private void RunPdf(object AParameter)
        {
            string PdfFileName;
            try
            {
                PdfFileName = SavePdf();
            }
            catch (Exception ex)
            {
                Log.Error("Exporting PDF file failed", ex);
                Core.IoC.Get<IMessageBoxService>().ShowErrorAsync("Export failed", ex.Message);
                return;
            }
            try
            {
                Process.Start(PdfFileName);
            }
            catch (Exception ex)
            {
                Log.Error("Starting PDF file failed", ex);
                throw new Exception(string.Format(Resources.CannotRunFile, PdfFileName), ex);
            }
        }

        private void Dummy(object AParameter)
        {
          
        }


        private void Close(object AParameter)
        {
            Log.TraceFormat("Closing '{0}'", FileName);
            Core.DocumentsViewModel.CloseDocument(this);
        }

        public LinkedSongOptionsViewModel SongOptionViewModel
        {
            get { return m_SongOptionViewModel; }
            set { SetProperty(ref m_SongOptionViewModel, value, nameof(SongOptionViewModel)); }
        }

        public ICommand ImportPlainTextCommand
        {
            get { return m_ImportPlainTextCommand; }
            set { SetProperty(ref m_ImportPlainTextCommand, value, nameof(ImportPlainTextCommand)); }
        }

        public ICommand CloseCommand
        {
            get { return m_CloseCommand; }
            set { SetProperty(ref m_CloseCommand, value, nameof(CloseCommand)); }
        }

        public ICommand SaveCommand
        {
            get { return m_SaveCommand; }
            set { SetProperty(ref m_SaveCommand, value, nameof(SaveCommand)); }
        }

        public ICommand SaveAsCommand
        {
            get { return m_SaveAsCommand; }
            set { SetProperty(ref m_SaveAsCommand, value, nameof(SaveAsCommand)); }
        }

        public ICommand PrintCommand
        {
            get { return m_PrintCommand; }
            set { SetProperty(ref m_PrintCommand, value, nameof(PrintCommand)); }
        }

        public ICommand RunPdfCommand
        {
            get { return m_RunPdfCommand; }
            set { SetProperty(ref m_RunPdfCommand, value, nameof(RunPdfCommand)); }
        }

        public ICommand SavePdfCommand
        {
            get { return m_SavePdfCommand; }
            set { SetProperty(ref m_SavePdfCommand, value, nameof(SavePdfCommand)); }
        }

        public ZoomViewModel ZoomViewModel
        {
            get { return m_ZoomViewModel; }
            set { SetProperty(ref m_ZoomViewModel, value, nameof(ZoomViewModel)); }
        }
      

        public Encoding Encoding
        {
            get { return m_Encoding; }
            set { SetProperty(ref m_Encoding, value, nameof(Encoding)); }
        }


        public string Content
        {
            get { return m_Content; }
            set { SetProperty(ref m_Content, value, nameof(Content)); }
        }

        public CoreViewModel Core
        {
            get { return m_Core; }
            set { SetProperty(ref m_Core, value, nameof(Core)); }
        }


        public Dispatcher Dispatcher
        {
            get { return m_Dispatcher; }
            set
            {
                SetProperty(ref m_Dispatcher, value, nameof(Dispatcher));
                if (ZoomViewModel != null)
                    ZoomViewModel.Dispatcher = value;
            }
        }


        public string FileName
        {
            get { return m_FileName; }
            set
            {
                if (SetProperty(ref m_FileName, value, nameof(FileName)))
                {
                    OnPropertyChanged(nameof(TabHeaderText));
                    OnPropertyChanged(nameof(StatusBarText));
                }
            }
        }

        public string TabHeaderText => (string) new TabFileNameConverter().Convert(new object[] {FileName, HasUnsavedChanges}, typeof (string), null, null);

        public string StatusBarText => FileName;

        public string UniqueKey => FileName;

        public bool HasUnsavedChanges
        {
            get { return m_HasUnsavedChanges; }
            set
            {
                if (SetProperty(ref m_HasUnsavedChanges, value, nameof(HasUnsavedChanges)))
                    OnPropertyChanged(nameof(TabHeaderText));
            }
        }


        public object GetTabContent()
        {
            if (m_ChordProFileView == null)
                m_ChordProFileView = new ChordProFileView(this);
            return m_ChordProFileView;
        }

        public SongBook GetSongBook()
        {
            var IoC = Core.IoC;
            using (var strm = new StringStream(Content))
            using (var Parser = new ChordProParser(IoC, strm))
            {
                Log.Trace("Reading file...");
                Parser.ReadFile();
                var DetectedEncoding = Parser.Encoding ?? Encoding.Default;
                Log.TraceFormat("File parsing complete. Detected encoding '{0}'", DetectedEncoding.EncodingName);
                if (DetectedEncoding.Equals(IoC.Get<Encoding>()))
                {
                    IoC.Unregister(typeof(Encoding));
                    IoC.RegisterInstance(DetectedEncoding);
                }
                return Parser.SongBook;
            }
        }

        public string GetAscii()
        {
            var IoC = Core.IoC;
            Log.Trace("Rendering ASCII...");
            var Book = GetSongBook();
            using (var Renderer = new AsciiRenderer(IoC, Book, SongOptionViewModel))
            {
                return Renderer.Render();
            }
        }

        public string GetDdl()
        {
            var IoC = Core.IoC;
            Log.Trace("Rendering Ddl...");
            var Book = GetSongBook();
            using (var Renderer = new PdfRenderer(IoC, Book, SongOptionViewModel))
            {
                return Renderer.RenderToDdl();
            }
        }

        public void SavePdf(string AFileName)
        {
            var IoC = Core.IoC;
            Log.Trace("Exporting PDF...");
            var Book = GetSongBook();
            var OutFile = new FileInfo(AFileName);
            Log.TraceFormat("Output file is {0}", OutFile.FullName);
            using (var Renderer = new PdfRenderer(IoC, Book, SongOptionViewModel))
            {
                Log.Trace("Creating outfile...");
                using (var OutStream = OutFile.Create())
                {
                    Log.Trace("Rendering...");
                    Renderer.Render(OutStream);
                    Log.Trace("Flushing...");
                }
            }
        }

        private string SavePdf()
        {
            var PdfFileName = FileName;
            if (string.IsNullOrWhiteSpace(PdfFileName) || !File.Exists(PdfFileName))
                throw new Exception(Resources.SaveBevorePdf);
            PdfFileName = Path.ChangeExtension(PdfFileName, ".pdf");
            Log.TraceFormat("Exporting PDF file {0}", PdfFileName);
            try
            {
                if (File.Exists(PdfFileName))
                {
                    Log.Trace($"PDF file {PdfFileName} exists, deleting");
                    File.Delete(PdfFileName);
                }
                SavePdf(PdfFileName);
                return PdfFileName;
            }
            catch (Exception ex)
            {
                Log.Error($"Exporting PDF file {PdfFileName} failed", ex);
                throw new Exception(string.Format(Resources.CannotSaveFile, PdfFileName), ex);
            }
        }

        public void BeforeClose(CloseEventArguments AArguments)
        {
            if (HasUnsavedChanges)
                AArguments.AddConfirmationMessage(string.Format(Lib.Properties.Resources.The_song_0_has_unsaved_changes_, Path.GetFileName(FileName)));
        }

    }
}
