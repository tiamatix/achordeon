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
using System.Threading.Tasks;
using Achordeon.Common.Helpers;

namespace Achordeon.Shell.CmdLine
{
    public class ConsoleMessageBoxService : IMessageBoxService
    {
        public Task ShowInfoAsync(string ATitle, string AMessage)
        {
            Console.WriteLine($"{ATitle}: {AMessage}");
            return WaitFor.Nothing;
        }

        public Task ShowWarningAsync(string ATitle, string AMessage)
        {
            Console.WriteLine($"Warning: {ATitle}: {AMessage}");
            return WaitFor.Nothing;
        }

        public Task ShowErrorAsync(string ATitle, string AMessage)
        {
            Console.WriteLine($"Error: {ATitle}: {AMessage}");
            return WaitFor.Nothing;
        }

        public Task ShowErrorAsync(string ATitle, string AMessage, Exception AException)
        {
            Console.WriteLine($"Error: {ATitle}: {AMessage}, {AException.ToString()}");
            return WaitFor.Nothing;
        }

        public Task ConfirmAsync(ConfirmArguments AArguments)
        {
            return WaitFor.This(() =>
            {
                Console.WriteLine($"Error: {AArguments.Title}: {AArguments.Message}");
                Console.WriteLine("1: " + AArguments.OkButtonText);
                Console.WriteLine("2: " + AArguments.CancelButtonText);
                Console.WriteLine("3: Cancel");
                while (true)
                {
                    Console.WriteLine("Press [1], [2] or [3]:");
                    switch (Console.ReadKey().KeyChar)
                    {
                        case '1':
                            AArguments.Result = true;
                            return;
                        case '2':
                            AArguments.Result = false;
                            return;
                        case '3':
                            AArguments.Result = null;
                            return;
                    }
                }
            });
        }
    }
}
