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
using System.Reflection;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using Achordeon.Common.Helpers;
using Achordeon.Lib.Chords;
using Achordeon.Shell.Wpf.Controls.Preview;
using Common.Logging;
using DryIoc.Experimental;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;


namespace Achordeon.Shell.Wpf.Contents.ChordProFile
{
    /// <summary>
    /// Interaktionslogik für HomeView.xaml
    /// </summary>
    public partial class ChordProFileView : UserControl
    {
        private readonly Timer m_UpdatePreviewTimer;
        private bool m_SuppressTextChanged;
        private IPreviewControl m_PreviewControl;
        private CompletionWindow m_CurrentChordCompletionWindow;

        public ChordProFileViewModel View => DataContext as ChordProFileViewModel;

        private ILog Log => View.Core.IoC.Get<Log>().Using(this);


        private void ReCreatePreviewControl()
        {
            if (m_PreviewControl != null)
            {
                Log.Trace("Unregistering old preview control");
                m_PreviewControl.LayoutUpdated -= PreviewControlOnLayoutUpdated;
                m_PreviewControl = null;
                PreviewContainer.Child = null;
            }
            if (View.Core.SettingsViewModel.UsePdfPreview)
            {
                Log.Trace("Creating PDF preview control");
                var Control = new PreviewControlPdf(View);
                PreviewContainer.Child = Control;
                m_PreviewControl = Control;
            }
            else
            {
                Log.Trace("Creating ASCII preview control");
                var Control = new PreviewControlAscii(View);
                PreviewContainer.Child = Control;
                m_PreviewControl = Control;
            }
            Log.Trace("Registering new preview control");
            m_PreviewControl.Zoom = View.ZoomViewModel.Zoom;
            m_PreviewControl.LayoutUpdated += PreviewControlOnLayoutUpdated;
            DelayedUpdate();
        }

        private void PreviewControlOnLayoutUpdated(object ASender, EventArgs AEventArgs)
        {
            if (m_PreviewControl != null)
                View.ZoomViewModel.Zoom = m_PreviewControl.Zoom;
        }

        public ChordProFileView(ChordProFileViewModel AView)
        {
            DataContext = AView;
            View.Dispatcher = Dispatcher;

            InitializeComponent();

            m_UpdatePreviewTimer = new Timer(TimeSpan.FromMilliseconds(500).Milliseconds);
            m_UpdatePreviewTimer.Elapsed += (ASender, AArgs) =>
            {
                m_UpdatePreviewTimer.Stop();
                m_PreviewControl?.Update();
            };
            Loaded += FirstTimeLoadedHandler;
            TextEditor.TextChanged += (ASender, AArgs) =>
            {
                Log.Trace("TextEditor.TextChanged");
                if (!m_SuppressTextChanged)
                    View.HasUnsavedChanges = true;
                View.Content = TextEditor.Text;
                DelayedUpdate();
            };
            TextEditor.TextArea.TextEntering += TextAreaTextEntering;
            TextEditor.TextArea.TextEntered += TextAreaTextEntered;
            using (var SyntaxHighlightStream = Assembly.GetAssembly(GetType()).GetManifestResourceStream("Achordeon.Shell.Wpf.ChordPro.xshd"))
            {
                if (SyntaxHighlightStream != null)
                {
                    using (var Reader = new XmlTextReader(SyntaxHighlightStream))
                    {
                        TextEditor.SyntaxHighlighting = HighlightingLoader.Load(Reader, HighlightingManager.Instance);
                    }
                }
            }

            ((SimpleCommand) View.ZoomViewModel.ResetZoomCommand).ExecuteDelegate = AO =>
            {
                Dispatcher.BeginInvoke((Action) (() =>
                {
                    View.ZoomViewModel.Zoom = View.ZoomViewModel.DefaultZoom;
                }));
            };
            ((SimpleCommand) View.ZoomViewModel.FitToHeightCommand).ExecuteDelegate = AO => m_PreviewControl?.FitToHeight();
            ((SimpleCommand) View.ZoomViewModel.FitToWidthCommand).ExecuteDelegate = AO => m_PreviewControl?.FitToWidth();
            ((SimpleCommand) View.ZoomViewModel.FitDefaultCommand).ExecuteDelegate = AO => m_PreviewControl?.FitDefault();
            ((SimpleCommand) View.ZoomViewModel.FitToPageCommand).ExecuteDelegate = AO => m_PreviewControl?.FitToPage();
            ((SimpleCommand) View.PrintCommand).ExecuteDelegate = AO => { m_PreviewControl?.Print(); };

            ((SimpleCommand)View.TabUnTabSelectionCommand).CanExecuteDelegate = AO => View.CanTabUnTabSelection(new CommandWorkingOnSelectionTextArguments(TextEditor.SelectionStart, TextEditor.SelectionLength, TextEditor.SelectedText));
            ((SimpleCommand)View.TabUnTabSelectionCommand).ExecuteDelegate = AO =>
            {
                try
                {
                    Log.Trace(nameof(View.TabUnTabSelectionCommand));
                    var Args = new CommandWorkingOnSelectionTextArguments(TextEditor.SelectionStart, TextEditor.SelectionLength, TextEditor.SelectedText);
                    View.TabUnTabSelection(Args);
                    if (Args.Success)
                    {
                        TextEditor.TextArea.Document.Replace(Args.SelectionStart,
                            Args.SelectionLength,
                            Args.Result,
                            OffsetChangeMappingType.RemoveAndInsert);
                        TextEditor.Select(Args.SelectionStart, Args.Result.Length);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    View.Core.IoC.Get<IMessageBoxService>().ShowErrorAsync(
                          Properties.Resources.ErrorDialogTitle,
                           Properties.Resources.FailedToConvertTheSelection,
                           ex);
                }
            };

            ((SimpleCommand)View.ChorusUnchorusSelectionCommand).CanExecuteDelegate = AO => View.CanChorusUnchorusSelection(new CommandWorkingOnSelectionTextArguments(TextEditor.SelectionStart, TextEditor.SelectionLength, TextEditor.SelectedText));
            ((SimpleCommand)View.ChorusUnchorusSelectionCommand).ExecuteDelegate = AO =>
            {
                try
                {
                    Log.Trace(nameof(View.ChorusUnchorusSelectionCommand));
                    var Args = new CommandWorkingOnSelectionTextArguments(TextEditor.SelectionStart, TextEditor.SelectionLength, TextEditor.SelectedText);
                    View.ChorusUnchorusSelection(Args);
                    if (Args.Success)
                    {
                        TextEditor.TextArea.Document.Replace(Args.SelectionStart,
                            Args.SelectionLength,
                            Args.Result,
                            OffsetChangeMappingType.RemoveAndInsert);
                        TextEditor.Select(Args.SelectionStart, Args.Result.Length);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    View.Core.IoC.Get<IMessageBoxService>().ShowErrorAsync(
                          Properties.Resources.ErrorDialogTitle,
                           Properties.Resources.FailedToConvertTheSelection,
                           ex);
                }
            };

            ((SimpleCommand)View.CommentUncommentSelectionCommand).CanExecuteDelegate = AO => View.CanCommentUncommentSelection(new CommandWorkingOnSelectionTextArguments(TextEditor.SelectionStart, TextEditor.SelectionLength, TextEditor.SelectedText));
            ((SimpleCommand) View.CommentUncommentSelectionCommand).ExecuteDelegate = AO =>
            {
                try
                {
                    Log.Trace(nameof(View.CommentUncommentSelectionCommand));
                    var Args = new CommandWorkingOnSelectionTextArguments(TextEditor.SelectionStart, TextEditor.SelectionLength, TextEditor.SelectedText);
                    View.CommentUncommentSelection(Args);
                    if (Args.Success)
                    {
                        TextEditor.TextArea.Document.Replace(Args.SelectionStart,
                            Args.SelectionLength,
                            Args.Result,
                            OffsetChangeMappingType.RemoveAndInsert);
                        TextEditor.Select(Args.SelectionStart, Args.Result.Length);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    View.Core.IoC.Get<IMessageBoxService>().ShowErrorAsync(
                          Properties.Resources.ErrorDialogTitle,
                           Properties.Resources.FailedToConvertTheSelection,
                           ex);
                }
            };

            ((SimpleCommand) View.ImportPlainTextCommand).CanExecuteDelegate = AO => View.CanImportPlainText(new CommandWorkingOnSelectionTextArguments(TextEditor.SelectionStart, TextEditor.SelectionLength, TextEditor.SelectedText));
            ((SimpleCommand)View.ImportPlainTextCommand).ExecuteDelegate = AO =>
            {
                try
                {
                    Log.Trace(nameof(View.ImportPlainTextCommand));
                    var Args = new CommandWorkingOnSelectionTextArguments(TextEditor.SelectionStart, TextEditor.SelectionLength, TextEditor.SelectedText);
                    View.ImportPlainText(Args);
                    if (Args.Success)
                        TextEditor.TextArea.Document.Replace(Args.SelectionStart,
                            Args.SelectionLength,
                            Args.Result,
                            OffsetChangeMappingType.RemoveAndInsert);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    View.Core.IoC.Get<IMessageBoxService>().ShowErrorAsync(
                        Properties.Resources.ErrorDialogTitle,
                        Properties.Resources.FailedToConvertPlainText,
                        ex);
                }
            };
            View.ZoomViewModel.PropertyChanged += (ASender, AArgs) =>
            {
                if (AArgs.PropertyName == nameof(ZoomViewModel.Zoom))
                {
                    Log.TraceFormat("{0} changed, adjusting zoom", nameof(ZoomViewModel.Zoom));
                    if (m_PreviewControl != null)
                        m_PreviewControl.Zoom = View.ZoomViewModel.Zoom;
                }
            };
            View.Core.SettingsViewModel.PropertyChanged += (ASender, AArgs) =>
            {
                if (AArgs.PropertyName == nameof(SettingsViewModel.UsePdfPreview))
                {
                    Log.TraceFormat("{0} changed, recreate preview control", nameof(SettingsViewModel.UsePdfPreview));
                    Dispatcher.BeginInvoke((Action)(ReCreatePreviewControl));
                }
            };
            AView.SongOptionViewModel.PropertyChanged += (ASender, AArgs) =>
            {
                DelayedUpdate();
            };
        }

        //TODO this should be moved to the view model, but acutally I don't know how to bind to a percentage
        public double SplitDistanceInPercent
        {
            get
            {
                var res = ColChordProEditor.ActualWidth / Math.Max(1, gridMain.ActualWidth);
                return Math.Min(1, Math.Max(0, res));
            }
            set
            {
                if (value < 0.05 || value > 1)
                    return;
                var NewValue = Math.Min(0.95, Math.Max(0.05, value)) * gridMain.ActualWidth;
                ColChordProEditor.Width = new GridLength(NewValue, GridUnitType.Pixel);
            }
        }
        
        private void DelayedUpdate()
        {
            Log.Trace("Resetting/starting delayed update");
            m_UpdatePreviewTimer.Stop();
            m_UpdatePreviewTimer.Start();            
        }

        private void FirstTimeLoadedHandler(object ASender, RoutedEventArgs AArgs)
        {
            // Ensure that this handler is called only once.
            Log.Trace("main initialization after load");
            Loaded -= FirstTimeLoadedHandler;
            m_SuppressTextChanged = true;
            TextEditor.Text = View.Content;
            m_SuppressTextChanged = false;
            DelayedUpdate();
            ReCreatePreviewControl();            
            SplitDistanceInPercent = View.Core.SettingsViewModel.ChordProEditorSplitPosition;
        }


        private void TextEditorIsVisibleChanged(object ASender, DependencyPropertyChangedEventArgs AArgs)
        {
            if (TextEditor.IsVisible)
                TextEditor.Focus();
        }

        private void TextAreaTextEntered(object ASender, TextCompositionEventArgs AArgs)
        {
            if (AArgs.Text == "[")
            {
                try
                {
                    //Chord completion window requested
                    m_CurrentChordCompletionWindow = new CompletionWindow(TextEditor.TextArea);
                    var CompletionData = m_CurrentChordCompletionWindow.CompletionList.CompletionData;

                    var CommonChordLibrary = View.Core.IoC.Get<CommonChordLibrary>();

                    foreach (var Chord in CommonChordLibrary.Chords)
                        CompletionData.Add(new ChordCompletionData(View.Core.IoC, new[] {CommonChordLibrary.Chords}, 1, Chord.Name));
                    m_CurrentChordCompletionWindow.Show();
                    m_CurrentChordCompletionWindow.Closed += delegate { m_CurrentChordCompletionWindow = null; };
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }
        }

        void TextAreaTextEntering(object ASender, TextCompositionEventArgs AArgs)
        {
            if (AArgs.Text.Length > 0 && m_CurrentChordCompletionWindow != null)
            {
                if (!char.IsLetterOrDigit(AArgs.Text[0]))
                {
                    // Whenever a non-letter is typed while the completion window is open,
                    // insert the currently selected element.
                    m_CurrentChordCompletionWindow.CompletionList.RequestInsertion(AArgs);
                }
            }
            // Do not set e.Handled=true.
            // We still want to insert the character that was typed.
        }
    }
}
