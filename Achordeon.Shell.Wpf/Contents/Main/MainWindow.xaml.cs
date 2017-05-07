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
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Achordeon.Annotations;
using Achordeon.Common.Helpers;
using DryIoc;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.SimpleChildWindow;
using Timer = System.Timers.Timer;

namespace Achordeon.Shell.Wpf.Contents.Main
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, IMessageBoxService, INotifyPropertyChanged
    {
        private volatile bool m_IsAsyncClosingDialogOpen;

        public MainViewModel View => DataContext as MainViewModel;

        public MainWindow(CoreViewModel ACore)
        {
            MainViewModel = new MainViewModel(ACore, Dispatcher);
            DataContext = MainViewModel;
            InitializeComponent();
            Closing += MainWindowClosing;
            Closed += MainWindowClosed;
            Loaded += (ASender, AArgs) => MainViewModel.StartAutoUpdateCheck();
            View.Core.SettingsViewModel.MainWindowPosition.ApplyTo(this);
            ACore.IoC.Unregister<IMessageBoxService>();
            ACore.IoC.RegisterInstance<IMessageBoxService>(this);
            GlobalSettingsEditor.Settings = MainViewModel.Core.SettingsViewModel;
            lvLog.ViewModel = MainViewModel.Core.LogViewModel;
        }


        public static readonly DependencyProperty MainViewModelProperty = DependencyProperty.Register(
           nameof(MainViewModel),
           typeof(MainViewModel),
           typeof(MainWindow), 
           new PropertyMetadata(null));

        public MainViewModel MainViewModel
        {
            get { return (MainViewModel)GetValue(MainViewModelProperty); }
            set
            {
                SetValue(MainViewModelProperty, value);                
                OnPropertyChanged(nameof(MainViewModel));
            }
        }


        private void MainWindowClosed(object ASender, EventArgs AArgs)
        {
            MainViewModel.Core.SettingsViewModel.MainWindowPosition = new WindowPosition(this);
            View.Closed();
        }


        private async void PerformMainWindowClosingAsync()
        {
            m_IsAsyncClosingDialogOpen = true;
            var CloseArguments = new CancelEventArgs();
            await View.Closing(CloseArguments);
            if (CloseArguments.Cancel)
            {
                //User cancelled closing. As we already cancelled the main window close,
                //we only have to reset m_IsAsyncClosingDialogOpen to be able to pop up the dialog again.
                m_IsAsyncClosingDialogOpen = false;
                return;
            }
            //User confirmed the close - we invoke a second Close(), leaving m_IsAsyncClosingDialogOpen set TRUE. 
            //This will lead MainWindowClosing() to not cancel the closing.
            Dispatcher.Invoke(Close);
        }

        private void MainWindowClosing(object ASender, System.ComponentModel.CancelEventArgs AArgs)
        {
            //Closing is a bit tricky.
            //We have to keep the main window alive whily any dialogs are shown.
            //So we cancel the closing on first try, and pop up the confirmation dialog. We set m_IsAsyncClosingDialogOpen TRUE.
            //After the dialog was confirmed, it invokes Close() again.
            //This time, we know it's coming from the confirmation dialog by looking at m_IsAsyncClosingDialogOpen and don't cancel.
            if (m_IsAsyncClosingDialogOpen)
                return;
            AArgs.Cancel = true;
            //We have to use a timer, because its not allowed to change control's visibility in a window's show/hide action.
            //But MahApps uses a control for the dialogs
            var DelayedClose = new Timer(1);
            DelayedClose.Elapsed += (AO, AEventArgs) =>
            {
                //After a tick, the window is not on closing state anymore, and we can bring up the dialog
                DelayedClose.Stop();
                Dispatcher.Invoke(PerformMainWindowClosingAsync);
            };
            DelayedClose.Start();
        }

        public async Task ShowInfoAsync(string ATitle, string AMessage)
        {
            await this.ShowMessageAsync(ATitle, AMessage);
        }


        public async Task ShowWarningAsync(string ATitle, string AMessage)
        {
            var Sets = new MetroDialogSettings();
            Sets.ColorScheme = MetroDialogColorScheme.Accented;
            await this.ShowMessageAsync(ATitle, AMessage, MessageDialogStyle.Affirmative, Sets);
        }


        public async Task ShowErrorAsync(string ATitle, string AMessage)
        {
            var Sets = new MetroDialogSettings();
            Sets.ColorScheme = MetroDialogColorScheme.Inverted;
            await this.ShowMessageAsync(ATitle, AMessage, MessageDialogStyle.Affirmative, Sets);
        }

     
        public async Task ShowErrorAsync(string ATitle, string AMessage, Exception AException)
        {
            var dlg = new ErrorDialog.ErrorDialog(AException);
            dlg.ShowDialog();
            await WaitFor.Nothing;
        }

        public async Task ConfirmAsync(ConfirmArguments AArguments)
        {
            var Sets = new MetroDialogSettings()
            {
                AffirmativeButtonText = AArguments.OkButtonText,
                AnimateShow = true,
                NegativeButtonText = AArguments.CancelButtonText,
                FirstAuxiliaryButtonText = "Cancel",
                ColorScheme = MetroDialogColorScheme.Accented
            };
            var result = await this.ShowMessageAsync(
                AArguments.Title,
                AArguments.Message,
                MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary,
                Sets);
            AArguments.Result = null;
            if (result == MessageDialogResult.Affirmative)
                AArguments.Result = true;
            if (result == MessageDialogResult.Negative)
                AArguments.Result = false;            
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string APropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(APropertyName));
        }

        private async void AboutButtonClick(object ASender, RoutedEventArgs AE)
        {
            var About = new AboutWindow() {IsModal = true};
            About.DataContext = View;
            await this.ShowChildWindowAsync(About);
        }
    }
}
