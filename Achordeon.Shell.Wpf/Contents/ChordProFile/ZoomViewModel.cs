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
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;


namespace Achordeon.Shell.Wpf.Contents.ChordProFile
{
    public class ZoomViewModel : ViewModelBase
    {
        public const double MIN_ZOOM = 5;
        public const double MAX_ZOOM = 500;
        public const double DEFAULT_ZOOM = 100;

        private Dispatcher m_Dispatcher;
        private CoreViewModel m_Core;
        private double m_Zoom;
        private ICommand m_FitToWidthCommand;
        private ICommand m_FitToHeightCommand;
        private ICommand m_FitToPageCommand;
        private ICommand m_FitDefaultCommand;
        private ICommand m_ZoomInCommand;
        private ICommand m_ZoomOutCommand;
        private ICommand m_ResetZoomCommand;

        public double DefaultZoom => DEFAULT_ZOOM;

        public IReadOnlyList<string> DefaultZooms { get; }

        public ZoomViewModel(CoreViewModel ACore)
        {
            Core = ACore;
            Dispatcher = Dispatcher.CurrentDispatcher;
            DefaultZooms = new ReadOnlyCollection<string>(new[] { 200, 150, 125, DEFAULT_ZOOM, 75, 50, 25, 10 }
              .Select(d => string.Format(CultureInfo.CurrentCulture, "{0:P0}", d)).ToArray());
            ZoomInCommand = new SimpleCommand(ZoomIn, CanZoomIn);
            ZoomOutCommand = new SimpleCommand(ZoomOut, CanZoomOut);
            FitToWidthCommand = new SimpleCommand(DummyPlaceholder, CanPerformAnyZoom);
            FitToHeightCommand = new SimpleCommand(DummyPlaceholder, CanPerformAnyZoom);
            FitDefaultCommand = new SimpleCommand(DummyPlaceholder, CanPerformAnyZoom);
            FitToPageCommand = new SimpleCommand(DummyPlaceholder, CanPerformAnyZoom);
            ResetZoomCommand = new SimpleCommand(DummyPlaceholder, CanPerformAnyZoom);
            Zoom = DefaultZoom;
            Core.SettingsViewModel.PropertyChanged += (ASender, AArgs) =>
            {
                if (AArgs.PropertyName == nameof(SettingsViewModel.UsePdfPreview))
                    CommandManager.InvalidateRequerySuggested();
            };
        }

        private void DummyPlaceholder(object AParameter)
        {
        }


        private bool CanPerformAnyZoom(object AParameter)
        {
            return CanZoom;
        }
    

        private bool CanZoom => Core.SettingsViewModel.UsePdfPreview;

        private bool CanZoomIn(object AParameter)
        {
            return CanZoom && Zoom < MAX_ZOOM;
        }

        private void ZoomIn(object AParameter)
        {
            Zoom = Math.Floor(Math.Round((Zoom + 10.0) * 100.0, 0)) / 100.0;
        }

        private bool CanZoomOut(object AParameter)
        {
            return CanZoom && Zoom > MIN_ZOOM;
        }

        private void ZoomOut(object AParameter)
        {
            Zoom = Math.Floor(Math.Round((Zoom - 10.0) * 100.0, 0)) / 100.0;
        }

        public ICommand ResetZoomCommand
        {
            get { return m_ResetZoomCommand; }
            set { SetProperty(ref m_ResetZoomCommand, value, nameof(ResetZoomCommand)); }
        }

        public ICommand ZoomOutCommand
        {
            get { return m_ZoomOutCommand; }
            set { SetProperty(ref m_ZoomOutCommand, value, nameof(ZoomOutCommand)); }
        }

        public ICommand ZoomInCommand
        {
            get { return m_ZoomInCommand; }
            set { SetProperty(ref m_ZoomInCommand, value, nameof(ZoomInCommand)); }
        }


        public double Zoom
        {
            get { return m_Zoom; }
            set
            {
                var Value = m_Zoom;
                if (!(Math.Abs(Value - value) > Single.Epsilon * 10))
                    return;
                Value = Math.Max(value, MIN_ZOOM);
                Value = Math.Min(Value, MAX_ZOOM);
                if (SetProperty(ref m_Zoom, Value, nameof(Zoom)))
                {
                    (m_ZoomInCommand as SimpleCommand)?.RaiseCanExecuteChanged();
                    (m_ZoomOutCommand as SimpleCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand FitToWidthCommand
        {
            get { return m_FitToWidthCommand; }
            set { SetProperty(ref m_FitToWidthCommand, value, nameof(FitToWidthCommand)); }
        }

        public ICommand FitToHeightCommand
        {
            get { return m_FitToHeightCommand; }
            set { SetProperty(ref m_FitToHeightCommand, value, nameof(FitToHeightCommand)); }
        }

        public ICommand FitToPageCommand
        {
            get { return m_FitToPageCommand; }
            set { SetProperty(ref m_FitToPageCommand, value, nameof(FitToPageCommand)); }
        }

        public ICommand FitDefaultCommand
        {
            get { return m_FitDefaultCommand; }
            set { SetProperty(ref m_FitDefaultCommand, value, nameof(FitDefaultCommand)); }
        }

       
        public CoreViewModel Core
        {
            get { return m_Core; }
            set { SetProperty(ref m_Core, value, nameof(Core)); }
        }


        public Dispatcher Dispatcher
        {
            get { return m_Dispatcher; }
            set { SetProperty(ref m_Dispatcher, value, nameof(Dispatcher)); }
        }


    }
}
