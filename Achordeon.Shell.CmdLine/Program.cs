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
        private static int Main(string[] args)
        {
            var IoC = new Container();
            try
            {
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
                Console.ReadKey();
            }


   

            var song1 = @"{title: Everybody Hurts}
{subtitle: REM}
{define G: base-fret 1 frets 3 2 0 0 3 3}
{define D4: base-fret 0 frets - - 0 0 3 -}
{define: E 0 0 0 2 3 3 -}
{start_of_tab}
Intro: E----------2-----------2-------------3-----------3-------
       B--------3---3-------3---3---------3---3-------3---3-----
       G------2-------2---2-------2-----0-------0---0-----------
       D----0-----------0---------------------------------------
       A--------------------------------------------------------
       E------------------------------3-----------3------------- (repeat)
{end_of_tab}

[D]When your day is [G]long and the [D]night, the night is [G]yours a[D]lone
[D]When you're sure you've had e[G]nough of this [D]life, well [G]hang on
{start_of_tab}
   E(low)-3-2-0-
{end_of_tab}
[E]Don't let yourself [A]go, [E]cause everybody [A]cries [E]and everybody[A] hurts some[D]times [G]
Sometimes everything is [D]wrong,   [G]now it's time to sing a[D]long
When your day is night alone [G]          (hold [D]on, hold on)
If you feel like letting go [G]           (hold [D]on)
If you think you've had too [G]much of this [D]life, well hang [G]on

{start_of_tab}
   E(low)-3-2-0-
{end_of_tab}
[E]Cause everybody [A]hurts, [E]take comfort in your [A]friends
[E]Everybody [A]hurts, [E]don't throw your [A]hands, oh [E]now, don't throw your [A]hands
[C]If you feel like you're [D4]alone, no, no, no, you're not [A]alone
{start_of_tab}
           D4 ->   E-0-----0-----0-----0--
                   B---3-----3-----3------
                   G-----0-----0-----0----
{end_of_tab}
 [D]If you're on your [G]own in this [D]life, the days and nights are [G]long
[D]When you think you've had too [G]much, with this [D]life, to hang [G]on

{start_of_tab}
   E(low)-3-2-0-
{end_of_tab}
[E]Well everybody [A]hurts, some[E]times 
Everybody [A]cries, [E]and everybody [A]hurts,[N.C.] ... some[D]times [G]
But everybody [D]hurts [G]sometimes so hold [D]on, hold [G]on, hold [D]on
Hold on, [G]hold on, [D]hold on, [G]hold on, [D]hold on
[G]Everybody [D]hurts [G]     [D]     [G]
[D]You are not alone [G]     [D]     [G]     [D]     [G]";

            var song2 = @"{t:Ganz da hinten, wo der uralte Leuchtturm steht den keiner mehr kennt}
{st:Subtitel 1}
{st:Subtitel 2}
{c:1.)}
[C]Ganz da hinten, wo der [F]Leuchtturm steht 
[C]Wo das weite Meer zu[F]ende geht 
{st:Subtitel 3}
[C]Liegt ein kleiner Ort [Amaj7/G]und [F]dort ist mein Zu[G]hause. 

{c:2.)}
[C]Gleich beim ersten Kilo[F]meterstein 
{comment:Bla Bla}
[C]Ganz versteckt im Grün vom [F]wilden Wein 
[C]Steht ein Haus allein und [F]das ist mein Zu[G]hause. 

{sot}
|: C - F
   C - F
   C - F - G :|
{eot}

{soc}
Chorus [C]Zwei alte Leute ganz für [F]sich allein 
Chorus! [C]Sitzen dort in ihrem [F]Stübelein 
{ng}
Chorus!! [Am]Sehen stumm und bang zum [F]Leuchtturm hin - und [G]warten
{eoc}

{c:4.)}
[Am]Ach wär ich doch - ein Jun[F]ge noch
{npp}
[C]Im Dorfe wo mein Mädel imme[F]rnoch.
[Am]Auf ihren Liebsten wartet, [F]käm er doch - nach [G]Hause.  

{sot}
|: C - F
   C - F
   C - F - G :|
{eot}

{c:5.)}
[Am]Denn ganz da hinten, wo der [F]Leuchtturm steht 
[Am]Wo das weite Meer zu [F]Ende geht 
[Am]Dort blieb ein großes Stück von [F]meinem Glück - zu[Am]rück.";

            var song3 = 
            @"{title:Love me tender}
{subtitle:Presley/Matson}

[G]Love me tender [A7]love me sweet
[D7]Never let me [G]go
[G]You have made my [A7]life complete
[D7]and I love you [G]so

{start_of_chorus}
[G]Love me [B7]tender [Em]love me [G7]true
[C]all my [Cm]dreams ful[G]fill
[G]For [Dm6]my [E+]dar[E7]ling [A7]I love you
[D7]and I always [G]will [Am7] [D7]
{end_of_chorus}

[G]Love me tender [A7]love me long
[D7]Take me to your [G]heart
[G]For it's there that [A7]I belong
[D7]and will never [G]part

{comment:Chorus}

[G]Love me tender [A7]Love me dear
[D7]Tell me you are [G]mine
[G]I'll be yours through [A7]all the years
[D7]till the end of [G]time

{comment:Chorus}";
            var s = new StringStream(song3);
            /*
            foreach (var c in CommonChords.Chords)
                Console.WriteLine($"{c.Name} => {ChordNameBeautifier.Beautify(c.Name)}");

            {
                var x = CommonChords.Chords.GetChordOrNull("C#");
                x = new Chord("C#", -1, 4, 6, 6, 6, -1, 4, ChordOrigin.Defined, Difficulty.Hard);
                x = CommonChords.Chords.GetChordOrNull("B7(#9)");
                var cbi = new ChordBoxImage(x, 5);
                cbi.Save(@"C:\Temp\chord.png");
                System.Diagnostics.Process.Start(@"C:\Temp\chord.png");
            }
            
            var p = new ChordProParser(IoC, s);
            p.ReadFile();
            p.SongBook.Transpose(2);
            var r = new AsciiRenderer(IoC, p.SongBook);
            r.Render();
            Console.Write(r.Result);
            Console.WriteLine(">>>");
            Console.Write(PlainTextSongToChordProConverter.Convert(r.Result));
            Console.WriteLine("<<<");

            var cpr = new ChordProRenderer(IoC, p.SongBook);
            cpr.Render();
            Console.WriteLine(">>>");
            Console.Write(cpr.Result);
            Console.WriteLine("<<<");


            var pr = new PdfRenderer(IoC, p.SongBook);
            pr.Render();
            File.Delete(@"c:\temp\chord.pdf");
            pr.SavePdfFile(@"c:\temp\chord.pdf");
            System.Diagnostics.Process.Start(@"c:\temp\chord.pdf");
            Console.ReadKey();
            */
        }
    }
}
