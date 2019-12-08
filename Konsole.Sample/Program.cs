using System;
using System.Threading;
using System.Threading.Tasks;
using Goblinfactory.Konsole.Platform.Windows;
using Konsole.Menus;
using Konsole.Sample.Demos;

namespace Konsole.Sample
{


    class Program
    {
        private static void Main(string[] args)
        {
            TestCursor();
            //PrintAtExample();
            //SlowTest();
            //Console.Clear();
            //QuickTest();  
        }

        private static void TestCursor()
        {
            var r = new Random();
            int i = 1;
            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(r.Next(3000));
                    Console.Write($"apple {i++}");
                }

            });
            char key;
            while ((key = Console.ReadKey().KeyChar)!='q')
            {
            }
        }
        private static void TestCursors()
        {
            using (var writer = new HighSpeedWriter())
            {
                var w = new Window();
                var left = w.SplitLeft("output");
                var right = w.SplitRight("console input");
                // need to blick Cursor
                while(true)
                {
                    if(Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true);
                    }
                }
            }
        }



        private static void TestNestedWindows(IConsole w)
        {
            var left = w.SplitLeft("left", ConsoleColor.Black);
            var right = w.SplitRight("right", ConsoleColor.Black);
            var nestedTop = left.SplitTop("importing", ConsoleColor.White);
            var nestedBottom = left.SplitBottom("exporting", ConsoleColor.DarkRed);
            void Writelines(IConsole con)
            {
                for (int i = 1; i < 11; i++)
                {
                    con.WriteLine($"Batch number :{i}");
                    con.WriteLine($"----------------");
                    con.WriteLine("one");
                    con.WriteLine("two");
                    con.WriteLine("three");
                    con.WriteLine("four");
                    con.WriteLine("-----------------");
                }
            }

            for (int i = 0; i < 100; i++) right.WriteLine(i.ToString());

            Writelines(nestedTop);
            Writelines(nestedBottom);
        }

        private static void QuickTest1()
        {
            Console.CursorVisible = false;
            var c = new Window();
            var consoles = c.SplitRows(
                    new Split(4, "headline", LineThickNess.Single, ConsoleColor.Yellow),
                    new Split(0, "content", LineThickNess.Single),
                    new Split(4, "status", LineThickNess.Single, ConsoleColor.Red)
            );

            var headline = consoles[0];
            var content = consoles[1];
            var status = consoles[2];

            headline.Write("my headline");
            content.WriteLine("content goes here");
            status.Write("System offline!");
            Console.ReadLine();
        }


        private static void QuickTest()
        {
            using (var writer = new HighSpeedWriter())
            {
                var window = new Window(writer);
                RunTheTest(window, writer.Flush);
            }
        }
        private static void SlowTest()
        {
            var window = new Window();
            RunTheTest(window, ()=> { });
        }

        private static void RunTheTest(IConsole window, Action flush)
        {
            Console.CursorVisible = false;

            var consoles = window.SplitRows(
                    new Split(4, "heading", LineThickNess.Single),
                    new Split(10),
                    new Split(0),
                    new Split(4, "status", LineThickNess.Single)
            ); ; ;

            var headline = consoles[0];
            var contentTop = consoles[1];
            var contentBottom = consoles[2];
            var status = consoles[3];

            var longText = "Let's see if these do? heres more text to see if this eventually wraps? text to see if this eventually wraps?text to see if this eventually wraps?";

            var menu = contentTop.SplitLeft("menu");
            var content = contentTop.SplitRight("content");

            var splits = contentBottom.SplitColumns(
                    new Split(20, "menu2"),
                    new Split(0),
                    new Split(20, "content3")
                );
            var menu2 = splits[0];
            menu2.WriteLine(longText);
            var content2a = splits[1];
            var content2parts = content2a.SplitRows(
                new Split(0, "content 2"),
                new Split(5, "demo REPL input")
                );
            var content2 = content2parts[0];
            var input = content2parts[1];
            var content3 = splits[2];
            content2.WriteLine(longText);
            content3.WriteLine(longText);
            headline.WriteLine("my headline");
            headline.WriteLine("line2");
            headline.WriteLine("line3");
            content.WriteLine("content goes here");
            content.WriteLine("Do these lines ╢╖╣║╗╟  print?");
            content.WriteLine(longText);
            menu.WriteLine("Options A");
            menu.WriteLine("Options B");
            menu.WriteLine(longText);
            menu.WriteLine("Options D");
            status.Write("System offline!");
            flush();
            input.WriteLine(ConsoleColor.Green, "  press 'esc' to quit");
            
            //input.CursorVisible = true;
            //input.CursorLeft = 0;
            //input.CursorTop = 0;
            
            char key = 'x';
            int color = 0;
            var statusProgress = new ProgressBar(status, 100);
            
            while (key != 'q')
            {
                color++;
                content2.WriteLine((ConsoleColor)(color % 15), longText);
                var fruit = $"apples {color}";
                menu2.WriteLine(fruit);
                menu.WriteLine(fruit);
                statusProgress.Refresh(color % 100, fruit);
                flush();
                var readKey = Console.ReadKey(true);
                if (readKey.Key == ConsoleKey.Escape)
                {
                    input.Colors = new Colors(ConsoleColor.White, ConsoleColor.Red);
                    input.WriteLine("   --- GOOD BYE!! ---   ");
                    flush();
                    Thread.Sleep(1000);
                    return;
                }
                if (readKey.Key == ConsoleKey.Enter)
                {
                    input.WriteLine("");
                    continue;
                }
                key = readKey.KeyChar;

                input.Write(new string(new[] { key }));
            }

        }




        private static void PrintAtExample()
        {

            var r = new Random();

            var window = new Window();

            var t1 = Task.Run(() => {
                int i = 1;
                while (true)
                {
                    Thread.Sleep(r.Next(3000));
                    window.WriteLine($"new token request {i++}");
                }
            });

            var t2 = Task.Run(() => {
                while (true)
                {
                    Thread.Sleep(500);
                    window.PrintAt(50, 0, $"CPU {r.Next(100)}% ");
                }
            });

            Task.WaitAll(t1, t2);
        }


        private static void Mainzz(string[] args)
        {
            var con = new Window(28, 1, 70, 30, ConsoleColor.Yellow, ConsoleColor.DarkGreen, K.Clipping);

            PrintNumberedBox(con);
            Console.WriteLine();
            var menu = new Menu("Konsole Samples", ConsoleKey.X, 25,

                new MenuItem('1', "Forms", () => FormDemos.Run(con)),
                new MenuItem('2', "Boxes", () => BoxeDemos.Run(con)),
                new MenuItem('3', "Scrolling", () => WindowDemo.Run2(con)),
                new MenuItem('4', "ProgressBar", () => ProgressBarDemos.ProgressBarDemo(con)),
                new MenuItem('5', "ProgressBarTwoLine", () => ProgressBarDemos.ProgressBarTwoLineDemo(con)),
                new MenuItem('6', "Test data", () => TestDataDemo.Run(con)),
                new MenuItem('7', "SplitLeft, SplitRight", () => SplitDemo.DemoSplitLeftRight(con)),
                new MenuItem('8', "SplitTop, SplitBottom", () => SplitDemo.DemoSplitTopBottom(con)),
                new MenuItem('9', "Nested window-scroll", () => TestNestedWindows(con)),
                new MenuItem('c', "clear screen", () => con.Clear()),
                new MenuItem('x', "Exit", () => { })

            );

            menu.OnBeforeMenuItem += (i) => { con.Clear(); };

            PrintNumberedBox(con);

            Console.WriteLine("\nPress 'X' to exit the demo.");

            menu.Run();
        }

        private static void PrintNumberedBox(IConsole c)
        {
            c.ForegroundColor = ConsoleColor.Green;
            // print a numbered box so that I can see where the menu is being printed
            for (int y = 0; y < 30; y += 5)
            {
                c.PrintAt(0, y, y.ToString());
                c.PrintAt(10, y, "10");
                c.PrintAt(20, y, "20");
                c.PrintAt(30, y, "30");
                c.PrintAt(40, y, "40");
                c.PrintAt(50, y, "50");
                c.PrintAt(60, y, "60");
            }
        }

    }
}

