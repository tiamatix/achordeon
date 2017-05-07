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
using System.Diagnostics;
using System.Globalization;
using Achordeon.Lib.Helpers;
using Achordeon.Shell.CmdLine.CommandlineArguments;
using Common.Logging;
using Common.Logging.Simple;
using DryIoc;
using DryIoc.Experimental;
using Achordeon.Common.Helpers;
using Achordeon.Lib.SongOptions;

namespace Achordeon.Shell.CmdLine
{
    internal class Program
    {
        [Conditional("RELEASE")]
        private static void DebugWait()
        {
            Console.ReadKey();
        }

        private static int Main(string[] args)
        {            
            try
            {
                var IoC = new Container();
                IoC.RegisterInstance(new Log(LogManager.Adapter = new NoOpLoggerFactoryAdapter()));
                Lib.Helpers.IoCRegistration.RegisterCommonIoCServices(IoC);
                IoC.Unregister<IMessageBoxService>();
                IoC.RegisterInstance<IMessageBoxService>(new ConsoleMessageBoxService());
                var options = new CommandlineArguments.CommandlineArguments();
                var Parser = new CommandLine.Parser((a) =>
                {
                    a.CaseSensitive = true;
                    a.IgnoreUnknownArguments = false;
                    a.MutuallyExclusive = true;
                    a.ParsingCulture = CultureInfo.InvariantCulture;                    
                });
                
                if (!Parser.ParseArguments(args, options))
                {
                    //Invalid commandline
                    Console.WriteLine(options.GetUsage());
                    Environment.Exit(CommandLine.Parser.DefaultExitCodeFail);
                }

                if (options.Verbose)
                {
                    //Switch to console logger to have a really ugly verbose console
                    IoC.Unregister(typeof (Log));
                    IoC.RegisterInstance(new Log(LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter()));
                }
                //Register commandline options
                IoC.RegisterInstance<ISongOptions>(options);
                IoC.RegisterInstance<ICommandlineArguments>(options);            
                //And let's start shaking
                IoC.RegisterInstance(new BusinessLogic(IoC));
                return IoC.Get<BusinessLogic>().Run();
            }
            finally
            {
                DebugWait();
            }  
        }
    }
}
