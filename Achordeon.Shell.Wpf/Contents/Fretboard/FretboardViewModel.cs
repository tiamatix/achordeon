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
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using Achordeon.Common.Helpers;
using Achordeon.Lib.Chords;
using Achordeon.Lib.MusicTheory;
using Achordeon.Shell.Wpf.Properties;
using Common.Logging;
using DryIoc.Experimental;
using Microsoft.Win32;

namespace Achordeon.Shell.Wpf.Contents.Home
{
    public class FretboardViewModel : ViewModelBase, IDocViewModel
    {
        private Dispatcher m_Dispatcher;
        private CoreViewModel m_CoreViewModel;
        private ICommand m_CloseCommand;
        private int m_NumberOfStrings = 6;
        private int m_NumberOfFrets = 18;
        private Tuning m_SelectedTuning = Tunings.Instance.GetDefault(6);
        private HarmonicMode m_SelectedHarmonicMode = HarmonicModes.Instance.Default;

        public FretboardViewModel(CoreViewModel ACore)
        {
            CoreViewModel = ACore;
            Dispatcher = Dispatcher.CurrentDispatcher;          
            CloseCommand = new SimpleCommand(Close);
        }

        private ILog Log => Core.IoC.Get<Log>().Using(this);

        public CommonChordLibrary CommonChordLibrary => CoreViewModel.IoC.Get<CommonChordLibrary>();

        public HarmonicModes HarmonicModes => HarmonicModes.Instance;

        public Tunings Tunings => Tunings.Instance;

        public int[] PossibleNumberOfStrings => new int[] {6,7};

        public int[] PossibleNumberOfFrets => Enumerable.Range(10,20).ToArray();

        public CoreViewModel Core
        {
            get { return m_CoreViewModel; }
            set { SetProperty(ref m_CoreViewModel, value, nameof(Core)); }
        }


        public Tuning SelectedTuning
        {
            get => m_SelectedTuning;
            set => SetProperty(ref m_SelectedTuning, value, nameof(SelectedTuning));
        }

        public HarmonicMode SelectedHarmonicMode
        {
            get => m_SelectedHarmonicMode;
            set => SetProperty(ref m_SelectedHarmonicMode, value, nameof(SelectedHarmonicMode));
        }

        public int NumberOfStrings
        {
            get => m_NumberOfStrings;
            set
            {
                if (value > 7)
                    value = 7;
                if (value < 6)
                    value = 6;
                SetProperty(ref m_NumberOfStrings, value, nameof(NumberOfStrings));
            }
        }

        public int NumberOfFrets
        {
            get => m_NumberOfFrets;
            set
            {
                if (value > 30)
                    value = 30;
                if (value < 10)
                    value = 10;
                SetProperty(ref m_NumberOfFrets, value, nameof(NumberOfFrets));
            }
        }

        private void Close(object AParameter)
        {            
            Core.DocumentsViewModel.CloseDocument(this);
        }


        public ICommand CloseCommand
        {
            get => m_CloseCommand;
            set => SetProperty(ref m_CloseCommand, value, nameof(CloseCommand));
        }



        
        public CoreViewModel CoreViewModel
        {
            get => m_CoreViewModel;
            set => SetProperty(ref m_CoreViewModel, value, nameof(CoreViewModel));
        }

        public Dispatcher Dispatcher
        {
            get => m_Dispatcher;
            set => SetProperty(ref m_Dispatcher, value, nameof(Dispatcher));
        }

        public string TabHeaderText => Resources.FretboardTitle;

        public string StatusBarText => string.Empty;
        public bool HasUnsavedChanges => false;

        public string UniqueKey => GetType().FullName;

        private FretboardView m_FretboardView;

        public object GetTabContent()
        {
            if (m_FretboardView == null)
                m_FretboardView = new FretboardView(this);
            return m_FretboardView;
        }

        public void BeforeClose(CloseEventArguments AArguments)
        {
            //Nothing to do            
        }
    }
}
