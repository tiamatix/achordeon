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
using System.Text;
using System.Windows;
using System.Windows.Documents;
using Achordeon.Shell.Wpf.Helpers;

namespace Achordeon.Shell.Wpf.Contents.ErrorDialog
{
    internal partial class ErrorDialog
    {
        public ErrorDialog(Exception AException)
        {
            InitializeComponent();
            ErrorTextbox.Document.Blocks.FirstBlock.Margin = new Thickness(0);
            SetException(AException);
        }

        private void SetException(Exception AException)
        {
            /*
            string path = Path.Combine(AchordeonConstants.ApplicationDataDirectory, Log.DirectoryName, Constant.Version);
            var directory = new DirectoryInfo(path);
            var log = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First();

            var paragraph  = Hyperlink("Please open new issue in: " , Constant.Issue);
            paragraph.Inlines.Add($"1. upload log file: {log.FullName}\n");
            paragraph.Inlines.Add($"2. copy below exception message");
            ErrorTextbox.Document.Blocks.Add(paragraph);
            */
            var Builder = new StringBuilder();
            Builder.AppendLine($"{AchordeonConstants.ACHORDEON} app version: {AchordeonConstants.Version}");
            Builder.AppendLine($"OS Version: {Environment.OSVersion.VersionString}");
            Builder.AppendLine($"Date: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            Builder.AppendLine("Exception:");
            Builder.AppendLine(AException.Source);
            Builder.AppendLine(AException.GetType().ToString());
            Builder.AppendLine(AException.Message);
            Builder.AppendLine(AException.StackTrace);
            var paragraph = new Paragraph();
            paragraph.Inlines.Add(Builder.ToString());
            ErrorTextbox.Document.Blocks.Add(paragraph);
        }

        private Paragraph CreateHyperlinkParagraph(string ATextBeforeUrl, string AUrl)
        {
            var Para = new Paragraph();
            Para.Margin = new Thickness(0);

            var Link = new Hyperlink {IsEnabled = true};
            Link.Inlines.Add(AUrl);
            Link.NavigateUri = new Uri(AUrl);
            Link.RequestNavigate += (s, e) => Process.Start(e.Uri.ToString());
            Link.Click += (s, e) => Process.Start(AUrl);

            Para.Inlines.Add(ATextBeforeUrl);
            Para.Inlines.Add(Link);
            Para.Inlines.Add("\n");

            return Para;
        }

        private void CloseButtonClick(object ASender, RoutedEventArgs AE)
        {
            Close();
        }
    }
}
