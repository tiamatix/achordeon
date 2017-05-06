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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Achordeon.Lib.Helpers;
using Achordeon.Lib.SongOptions;
using CommandLine;
using CommandLine.Text;

namespace Achordeon.Shell.CmdLine.CommandlineArguments
{
    //Does not raise property changed events because they are not required in commandline app
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class CommandlineArguments : ISongOptions, ICommandlineArguments
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay
        {
            get
            {
                var Converter = new SongOptionsConverter(DefaultSongOptions.Default);
                if (Converter.IsAllDefault(this))
                    return $"{GetType().Name}, all default";
                return $"{GetType().Name}, {Converter.SaveToString(this)}";
            }
        }


        public bool DrawChordGrids { get; set; } = DefaultSongOptions.DEFAULT_DRAW_CHORD_GRIDS;

        [Option('2', "2-up", Required = false, HelpText = "2-up printing. Prints two logical pages per physical page.", DefaultValue = DefaultSongOptions.DEFAULT_TWO_UP)]
        public bool TwoUp { get; set; }

        [Option('4', "4-up", Required = false, HelpText = "4-up printing. Prints four logical pages per physical page.", DefaultValue = DefaultSongOptions.DEFAULT_FOUR_UP)]
        public bool FourUp { get; set; }

        [Option('C', "chord-font", Required = false, HelpText = "Sets the font used to print chords. The font must be installed on your system.", DefaultValue = DefaultSongOptions.DEFAULT_CHORD_FONT)]
        public string ChordFont { get; set; }

        [Option('c', "chord-size", Required = false, HelpText = "Sets the size, in points, of the font used to display chords.", DefaultValue = DefaultSongOptions.DEFAULT_CHORD_SIZE_PT)]
        public int ChordSizePt { get; set; }

        [Option('s', "chord-grid-size", Required = false, HelpText = "Sets the size (in mm) of the chord grids.", DefaultValue = DefaultSongOptions.DEFAULT_CHORD_GRID_SIZE_MM)]
        public int ChordGridSizeMm { get; set; }

        [Option('S', "chord-grids-sorted", Required = false, HelpText = "Prints chords grids alphabetically.", DefaultValue = DefaultSongOptions.DEFAULT_CHORD_GRID_SORTED)]
        public bool ChordGridSorted { get; set; }

        [Option('A', "about", Required = false, HelpText = "Prints the 'About...' message.")]
        public bool About { get; set; }

        [Option('D', "dump-chords", Required = false, HelpText = "Generates a PostScript Chord Chart of all internally known chords as well as chords defined in the preferences file.", MutuallyExclusiveSet = "dump")]
        public bool DumpChords { get; set; }

        [Option('d', "dump-chords-text", Required = false, HelpText = "Generates a textual representation of all internally known chords.", MutuallyExclusiveSet = "dump")]
        public bool DumpChordsText { get; set; }

        [Option('L', "even-pages-number-left", Required = false, HelpText = "Places the odd and even page numbers in the lower right and left corners respectively(for two-sided output).The default is all page numbers on the right side.", DefaultValue = DefaultSongOptions.DEFAULT_EVEN_PAGE_NUMBER_LEFT)]
        public bool EvenPageNumberLeft { get; set; }

        [Option('l', "lyrics-only", Required = false, HelpText = "Prints only the lyrics of the song.", DefaultValue = DefaultSongOptions.DEFAULT_LYRICS_ONLY, MutuallyExclusiveSet = "dump")]
        public bool LyricsOnly { get; set; }

        [Option('o', "output", Required = false, HelpText = "Sends output to the indicated file. An existing file will be overwritten without warning. The output can also specify an extension like '.pdf', '.txt' or '.pro'. If not specified, the resulting file will be �<input file name>.pdf�.", DefaultValue = "")]
        public string Output { get; set; }

        [Option('O', "output-folder", Required = false, HelpText = "Sends output filed to the indicated folder. File name in the folder will be �<input file name>.pdf�. An existing file will be overwritten without warning.", DefaultValue = "")]
        public string OutputFolder { get; set; }

        [Option('n', "page-number-logical", Required = false, HelpText = "", DefaultValue = DefaultSongOptions.DEFAULT_PAGE_NUMBER_LOGICAL)]
        public bool PageNumberLogical { get; set; }

        [Option('P', "page-size", Required = false, HelpText = "Specify the paper size. Recognized values are 'a4' for ISO standard A4 paper(210mm * 297mm), 'a5' for ISO standard A5 paper(143mm � 210mm), and 'lette' (or 'us') for US Letter paper(8.5in � 11in).", DefaultValue = DefaultSongOptions.DEFAULT_PAGE_SIZE)]
        public string PageSize { get; set; }

        [Option('a', "single-space", Required = false, HelpText = "Automatically single spaces lines that have no chords.", DefaultValue = DefaultSongOptions.DEFAULT_SINGLE_SPACE)]
        public bool SingleSpace { get; set; }

        [Option('p', "start-page-number", Required = false, HelpText = "Numbers the pages consecutively starting with first_page(e.g. 1). Without this option, each song restarts the page numbering at 1, and page numbers are only put on subsequent pages of multiple page songs.", DefaultValue = DefaultSongOptions.DEFAULT_START_PAGE_NUMBER)]
        public int StartPageNumber { get; set; }

        [Option('T', "text-font", Required = false, HelpText = "Sets the font used to print text to the specified name. The font must be installed on your machine.", DefaultValue = DefaultSongOptions.DEFAULT_TEXT_FONT)]
        public string TextFont { get; set; }

        [Option('t', "text-size", Required = false, HelpText = "Sets the size, in points, of the font used to display the lyrics to the specified integer value.The title line is displayed using that point size * 1.6. The subtitle is displayed using point size * 1.2.", DefaultValue = DefaultSongOptions.DEFAULT_TEXT_SIZE_PT)]
        public int TextSizePt { get; set; }

        [Option('i', "toc", Required = false, HelpText = "Generates a table of contents with the song titles and page numbers.It implies page numbering through the document.", DefaultValue = DefaultSongOptions.DEFAULT_CREATE_TOC, MutuallyExclusiveSet = "dump")]
        public bool CreateToc { get; set; }

        [Option('x', "transpose", Required = false, HelpText = "Sets up transposition to that number of semitones.", DefaultValue = DefaultSongOptions.DEFAULT_TRANSPOSE_BY_HALFTONES)]
        public int TransposeByHalftones { get; set; }

        [Option('v', "version", Required = false, HelpText = "Prints the program version.")]
        public bool Version { get; set; }

        [Option('V', "verbose", Required = false, HelpText = "Prints additional debugging info.")]
        public bool Verbose { get; set; }

        [Option('w', "vertical-space", Required = false, HelpText = "Inserts extra vertical space, in points, between lines.", DefaultValue = DefaultSongOptions.DEFAULT_VERTICAL_SPACE)]
        public int VerticalSpace { get; set; }

        [ValueList(typeof(List<string>))]
        public IList<string> InputFiles { get; set; }

        [HelpOption('h', "help")]
        public string GetUsage()
        {
            var Result = new HelpText
            {
                Heading = new HeadingInfo(LibVersion.ProductName, LibVersion.Version),
                Copyright = new CopyrightInfo(false, "Wolf Robben", 2017),
                AdditionalNewLineAfterOption = false,
                AddDashesToOption = true
            };
            Result.AddPreOptionsLine("Usage: achordeon <option> filename1.cp [filename2.cp]...");
            Result.AddOptions(this);
            Result.AddPostOptionsLine("Groove on!");
            return Result;
        }

        public RunMode RunMode
        {
            get
            {
                if (!DumpChords && !DumpChordsText && !InputFiles.Any())
                {
                    //No input files
                    return RunMode.ShowUsage;
                }

                if (About)
                    return RunMode.About;
                if (Version)
                    return RunMode.Version;
                if (DumpChords)
                    return RunMode.DumpChords;
                if (DumpChordsText)
                    return RunMode.DumpCordTexts;
                return RunMode.ProcessChordPro;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string APropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(APropertyName));
        }

        public override string ToString()
        {
            var Converter = new SongOptionsConverter(DefaultSongOptions.Default);
            return Converter.SaveToString(this);
        }

    }
}
