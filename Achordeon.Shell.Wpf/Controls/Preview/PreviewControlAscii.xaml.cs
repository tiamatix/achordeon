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
using System.IO;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Achordeon.Common.Helpers;
using Achordeon.Shell.Wpf.Contents.ChordProFile;
using Common.Logging;
using DryIoc.Experimental;

namespace Achordeon.Shell.Wpf.Controls.Preview
{
    public partial class PreviewControlAscii : UserControl, IPreviewControl
    {
        public ChordProFileViewModel View => DataContext as ChordProFileViewModel;
        private ILog Log => View.Core.IoC.Get<Log>().Using(this);

        public PreviewControlAscii()
        {
        }

        public PreviewControlAscii(ChordProFileViewModel AView)
        {
            DataContext = AView;
            View.Dispatcher = Dispatcher;

            InitializeComponent();
        }

        public void Update()
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Log.Trace("Updating ASCII preview");
                    tbPreview.Document.Blocks.Clear();
                    tbPreview.Document.Blocks.Add(new Paragraph(new Run(View.GetAscii())));
                });
            }
            catch (Exception ex)
            {
                Log.Error("Update failed", ex);
            }
        }

        public void FitToHeight()
        {

        }

        public void FitToWidth()
        {

        }

        public void FitDefault()
        {

        }

        public void FitToPage()
        {

        }

        public void Print()
        {
            try
            {
                Log.Trace(nameof(Print) + " saving");
                var SourceDocument = new TextRange(tbPreview.Document.ContentStart, tbPreview.Document.ContentEnd);
                var TempStream = new MemoryStream();
                SourceDocument.Save(TempStream, DataFormats.Xaml);
                Log.Trace(nameof(Print) + " loading into flow document...");
                var FlowDocument = new FlowDocument();
                var CopyDocumentRange = new TextRange(FlowDocument.ContentStart, FlowDocument.ContentEnd);
                CopyDocumentRange.Load(TempStream, DataFormats.Xaml);
                PrintDocumentImageableArea Area = null;
                Log.Trace(nameof(Print) + " creating XPS...");
                var DocumentWriter = PrintQueue.CreateXpsDocumentWriter(ref Area);
                if (DocumentWriter == null || Area == null)
                    return;
                var Paginator = ((IDocumentPaginatorSource) FlowDocument).DocumentPaginator;
                Paginator.PageSize = new Size(Area.MediaSizeWidth, Area.MediaSizeHeight);
                var PagePadding = FlowDocument.PagePadding;
                FlowDocument.PagePadding = new Thickness(
                    Math.Max(Area.OriginWidth, PagePadding.Left),
                    Math.Max(Area.OriginHeight, PagePadding.Top),
                    Math.Max(Area.MediaSizeWidth - (Area.OriginWidth + Area.ExtentWidth), PagePadding.Right),
                    Math.Max(Area.MediaSizeHeight - (Area.OriginHeight + Area.ExtentHeight), PagePadding.Bottom));
                FlowDocument.ColumnWidth = double.PositiveInfinity;
                Log.Trace(nameof(Print) + " printing to XPS...");
                DocumentWriter.Write(Paginator);
            }
            catch (Exception ex)
            {
                Log.Error(nameof(Print) + " failed", ex);
            }
        }

        public double Zoom { get; set; } = 1;

        public new event EventHandler LayoutUpdated
        {
            add { }
            remove { }
        }
    }
}