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
using System.Windows;
using System.Windows.Controls;
using Achordeon.Common.Helpers;
using Achordeon.Shell.Wpf.Contents.ChordProFile;
using Common.Logging;
using DryIoc.Experimental;

namespace Achordeon.Shell.Wpf.Controls.Preview
{
    public partial class PreviewControlPdf : UserControl, IPreviewControl
    {
        public ChordProFileViewModel View => DataContext as ChordProFileViewModel;
        private ILog Log => View.Core.IoC.Get<Log>().Using(this);

        public PreviewControlPdf()
        {
        }

        public PreviewControlPdf(ChordProFileViewModel AView)
        {
            DataContext = AView;
            View.Dispatcher = Dispatcher;

            InitializeComponent();
        }

        public void Update()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    Log.Trace(nameof(Update));
                    //Unfortunally, MigraDoc preview does not expose the scroll viewer, and also no option to preserve scroll position
                    //So we apply this ugly hack and do this manually
                    var HiddenScrollViewerProperty = dpPreview.viewer.GetType().GetProperty("ScrollViewer", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    var ScrollViewer = HiddenScrollViewerProperty?.GetValue(dpPreview.viewer) as ScrollViewer;
                    var HorizontalOffset = ScrollViewer?.HorizontalOffset;
                    var VerticalOffset = ScrollViewer?.VerticalOffset;
                    var Ddl = View.GetDdl();
                    dpPreview.Ddl = Ddl;
                    ScrollViewer?.ScrollToHorizontalOffset(HorizontalOffset ?? 0);
                    ScrollViewer?.ScrollToVerticalOffset(VerticalOffset ?? 0);
                }
                catch (Exception ex)
                {
                    Log.Error(nameof(Update) + " failed", ex);
                }
            });
        }

        public void FitToHeight()
        {
            Dispatcher.BeginInvoke((Action) (() =>
            {
                try
                {
                    Log.Trace(nameof(FitToHeight));
                    dpPreview.viewer.FitToHeight();
                }
                catch (Exception ex)
                {
                    Log.Error(nameof(FitToHeight) + " failed", ex);
                }
            }));
        }

        public void FitToWidth()
        {
            Dispatcher.BeginInvoke((Action) (() =>
            {
                try
                {
                    Log.Trace(nameof(FitToWidth));
                    dpPreview.viewer.FitToWidth();
                }
                catch (Exception ex)
                {
                    Log.Error(nameof(FitToWidth) + " failed", ex);
                }
            }));
        }

        public void FitDefault()
        {
            Dispatcher.BeginInvoke((Action) (() =>
            {
                try
                {
                    Log.Trace(nameof(FitDefault));
                    dpPreview.viewer.FitToMaxPagesAcross();
                }
                catch (Exception ex)
                {
                    Log.Error(nameof(FitDefault) + " failed", ex);
                }
            }));
        }

        public void FitToPage()
        {
            Dispatcher.BeginInvoke((Action) (() =>
            {
                try
                {
                    Log.Trace(nameof(FitToPage));
                    dpPreview.viewer.FitToMaxPagesAcross();
                }
                catch (Exception ex)
                {
                    Log.Error(nameof(FitToPage) + " failed", ex);
                }
            }));
        }

        public void Print()
        {
            Dispatcher.BeginInvoke((Action) (() =>
            {
                try
                {
                    Log.Trace(nameof(Print));
                    dpPreview.viewer.Print();
                }
                catch (Exception ex)
                {
                    Log.Error(nameof(Print) + " failed", ex);
                }
            }));
        }

        public double Zoom
        {
            get { return dpPreview.viewer.Zoom; }
            set { dpPreview.viewer.Zoom = value; }
        }

        public new event EventHandler LayoutUpdated
        {
            add { dpPreview.viewer.LayoutUpdated += value; }
            remove { dpPreview.viewer.LayoutUpdated -= value; }
        }
    }
}
