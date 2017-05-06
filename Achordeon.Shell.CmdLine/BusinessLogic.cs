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
using System.IO;
using System.Linq;
using System.Text;
using Achordeon.Common.Helpers;
using Achordeon.Lib.DataModel;
using Achordeon.Lib.Helpers;
using Achordeon.Lib.Parser;
using Achordeon.Lib.SongOptions;
using Achordeon.Rendering;
using Achordeon.Rendering.Ascii;
using Achordeon.Rendering.ChordPro;
using Achordeon.Rendering.Pdf;
using Achordeon.Shell.CmdLine.CommandlineArguments;
using DryIoc;
using DryIoc.Experimental;

namespace Achordeon.Shell.CmdLine
{
    internal class BusinessLogic
    {
        private Container IoC { get; }

        public BusinessLogic(Container AIoC)
        {
            IoC = AIoC;
        }

        private void DumpChords()
        {
            throw new NotImplementedException();
        }


        private void DumpCordTexts()
        {
            throw new NotImplementedException();
        }

        private Renderer CreateRenderer(string AFileType, SongBook ASongBook, ISongOptions ASongOptions)
        {
            switch (AFileType.ToLower())
            {
                case "pdf":
                    return new PdfRenderer(IoC, ASongBook, ASongOptions);
                case "txt":
                    return new AsciiRenderer(IoC, ASongBook, ASongOptions);
                case "pro":
                case "cho":
                    return new ChordProRenderer(IoC, ASongBook, ASongOptions);
            }
            throw new Exception($"Don't know how to handle target file type '{AFileType}'");
        }

        private string DetermineOutFileName(string AInFileName)
        {
            var Options = IoC.Get<ICommandlineArguments>();
            var SpecifiedOutput = Options.Output.Trim();
            var OutputDirectory =!string.IsNullOrWhiteSpace(Options.OutputFolder) ? Options.OutputFolder.Trim() :  Path.GetDirectoryName(AInFileName) ?? string.Empty;
            var InputFileNameWithoutPath = Path.GetFileName(AInFileName);

            string ResultFileNameWithoutPath;
            if (string.IsNullOrWhiteSpace(SpecifiedOutput))
            {
                //No output specified
                ResultFileNameWithoutPath = Path.ChangeExtension(InputFileNameWithoutPath, ".pdf");
            }
            else
            {
                if (SpecifiedOutput.StartsWith("."))
                {
                    //Only extension specified
                    ResultFileNameWithoutPath = Path.GetFileNameWithoutExtension(InputFileNameWithoutPath) + SpecifiedOutput;
                }
                else
                {
                    //Complete file name
                    ResultFileNameWithoutPath = SpecifiedOutput;
                }
            }
            return Path.Combine(OutputDirectory, ResultFileNameWithoutPath);
        }

        private void ProcessSingleChordProFile(string AInputFileName)
        {
            var OutputFileName = DetermineOutFileName(AInputFileName);
            IoC.Get<Log>().Using(this).Trace("ProcessSingleChordProFile()");
            var InFile = new FileInfo(AInputFileName);
            IoC.Get<Log>().Using(this).TraceFormat("Input file is {0}", InFile.FullName);
            if (string.IsNullOrWhiteSpace(OutputFileName))
                OutputFileName = Path.ChangeExtension(InFile.FullName, ".pdf");
            var OutFile = new FileInfo(OutputFileName);
            IoC.Get<Log>().Using(this).TraceFormat("Output file is {0}", OutFile.FullName);
            if (InFile.FullName.ToLower() == OutFile.FullName.ToLower())
                throw new Exception("Cannot overwrite input file");
            if (OutFile.Exists)
            {
                IoC.Get<Log>().Using(this).TraceFormat("Output file exists, deleting", OutputFileName);
                File.Delete(OutputFileName);
            }
            SongBook SongBook;
            using (var strm = InFile.Open(FileMode.Open, FileAccess.Read))
            using (var Parser = new ChordProParser(IoC, strm))
            {
                IoC.Get<Log>().Using(this).Trace("Reading file...");
                Parser.ReadFile();
                var DetectedEncoding = Parser.Encoding ?? Encoding.Default;
                IoC.Get<Log>().Using(this).TraceFormat("File parsing complete. Detected encoding '{0}'", DetectedEncoding.EncodingName);
                if (DetectedEncoding.Equals(IoC.Get<Encoding>()))
                {
                    IoC.Unregister(typeof (Encoding));
                    IoC.RegisterInstance(DetectedEncoding);
                }
                SongBook = Parser.SongBook;
            }
            var Options = IoC.Get<ISongOptions>();
            //Rendering output
            IoC.Get<Log>().Using(this).Trace("Creating outfile...");
            using (var OutStream = OutFile.Create())
            {
                IoC.Get<Log>().Using(this).Trace("Creating renderer...");
                using (var Renderer = CreateRenderer(OutFile.Extension.TrimStart('.'), SongBook, Options))
                {
                    IoC.Get<Log>().Using(this).Trace("Rendering...");
                    Renderer.Render(OutStream);
                    IoC.Get<Log>().Using(this).Trace("Flushing...");
                }
            }
            IoC.Get<Log>().Using(this).Trace("Done!");
        }

        private int ProcessChordPro(ICommandlineArguments AArgs)
        {
            if (AArgs.InputFiles.Count < 1)
            {
                Console.WriteLine(AArgs.GetUsage());
                return CommandLine.Parser.DefaultExitCodeFail;
            }
            IEnumerable<string> InputFileNames = AArgs.InputFiles;
            var MultipleInfilesSpecified = AArgs.InputFiles.Count > 1;
            if (AArgs.InputFiles.Count == 1)
            {
                var OnlyFile = AArgs.InputFiles.First();
                if (OnlyFile.Contains("?") || OnlyFile.Contains("*"))
                {
                    //It's a wildcard!
                    MultipleInfilesSpecified = true;
                    var WorkingDir = Path.GetDirectoryName(OnlyFile) ?? Environment.CurrentDirectory;
                    var Wildcard = Path.GetFileName(OnlyFile);
                    IoC.Get<Log>().Using(this).TraceFormat("Input specified a wildcard '{0}'. Working directory is {1}", Wildcard, WorkingDir);
                    InputFileNames = Directory.GetFiles(WorkingDir, Wildcard);
                }
            }
            else
            {
                if (InputFileNames.Any(a => a.Contains("?") || a.Contains("*")))
                {
                    Console.WriteLine("if more than one input file is specified, none of them may contain wildcards.");
                    return CommandLine.Parser.DefaultExitCodeFail;
                }
            }
            if (MultipleInfilesSpecified)
            {
                if (!string.IsNullOrWhiteSpace(AArgs.Output) && !AArgs.Output.StartsWith("."))
                {
                    Console.WriteLine("if more than one input file is specified, the option --output or -o must indicate an extension like '.txt' or '.pdf', or has to be omitted.");
                    return CommandLine.Parser.DefaultExitCodeFail;
                }
            }
            var ExitCode = 0;
            foreach (var InputFileName in InputFileNames)
            {
                try
                {
                    ProcessSingleChordProFile(InputFileName);
                }
                catch (Exception ex)
                {
                    IoC.Get<Log>().Using(this).Error(ex);
                    Console.WriteLine("Error parsing '{0}' : {1}", InputFileName, ex.Message);
                    ExitCode = CommandLine.Parser.DefaultExitCodeFail;
                }
            }
            return ExitCode;
        }


        public int Run()
        {
            var args = IoC.Get<ICommandlineArguments>();
            switch (args.RunMode)
            {
                case RunMode.DumpChords:
                    DumpChords();
                    return 0;
                case RunMode.DumpCordTexts:
                    DumpCordTexts();
                    return 0;
                case RunMode.ProcessChordPro:
                    return ProcessChordPro(args);
                case RunMode.About:
                case RunMode.Version:
                    Console.WriteLine($"*** {LibVersion.ProductName}  {LibVersion.Version} *** ");
                    if (args.RunMode == RunMode.About)
                        Console.WriteLine(LibVersion.About);
                    return 0;
                case RunMode.ShowUsage:
                    Console.WriteLine(args.GetUsage());
                    return CommandLine.Parser.DefaultExitCodeFail;
                default:
                    throw new Exception($"Unknown run mode: {args.RunMode}");
            }
        }
    }
}
