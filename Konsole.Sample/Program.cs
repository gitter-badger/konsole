using System;
using Goblinfactory.Konsole.Platform.Windows;
using Konsole.Menus;
using Konsole.Sample.Demos;

namespace Konsole.Sample
{


    class Program
    {
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

        private static void QuickTest2()
        {
            Console.CursorVisible = false;

            // need to clean up a bit, ... window should be able to take either a window OR a HighSpeedWriter
            // currently because HighSpeedWriter implements IConsole, you'll get errors if you try to use 
            // all the IConsole functionality.
            // should split out the interface that windows have to depend on?


            using (var writer = new HighSpeedWriter())
            {
                var window = new Window(writer);
                //var window = new Window();

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
                        new Split(0, "content2"),
                        new Split(20, "content3")
                    );
                var menu2 = splits[0];
                menu2.WriteLine(longText);
                var content2 = splits[1];
                var content3 = splits[2];

                content2.WriteLine(longText);
                content3.WriteLine(longText);

                headline.WriteLine("my headline");
                // test scrolling
                headline.WriteLine("line2");
                headline.WriteLine("line3");
                content.WriteLine("content goes here");

                // ascii characters dont print when printed as unicode....mmm, so close? 

                content.WriteLine("these lines ╢╖╣║╗╟  don't print!");
                
                // content doesnt wrap??
                content.WriteLine("Let's see if these do? heres more text to see if this eventually wraps? text to see if this eventually wraps?text to see if this eventually wraps?");

                menu.WriteLine("Options A");
                menu.WriteLine("Options B");
                menu.WriteLine("Vvery very long option C that should wrap quite a bit! Vvery very long option C that should wrap quite a bit! Vvery very long option C that should wrap quite a bit!");
                menu.WriteLine("Options D");

               // sidebar.WriteLine("20% off all items between 11am and midnight tomorrow!");

                status.Write("System offline!");
                writer.Flush();
                menu2.WriteLine(ConsoleColor.Red, "press any key to print a bunch of text into content2, press 'q' to quit");

                char key = 'x';
                int color = 0;
                while(key != 'q')
                {
                    color++;
                    content2.WriteLine((ConsoleColor)(color % 15), longText);
                    menu2.WriteLine($"apples {color}");
                    menu.WriteLine($"apples {color}");
                    writer.Flush();
                    key = Console.ReadKey(true).KeyChar;

                }
                
            }
        }

        private static void Main(string[] args)
        {

            // need split columns tests!

            //Spikes.Test1();
            //QuickTest1();
            //var console = new Window(80,20);
            //new PlatformStuff().
            QuickTest2();
            ////Kernel32Draw.SelfTest();
            Console.ReadLine();
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
                new MenuItem('7', "SplitLeft, SplitRight", () =>  SplitDemo.DemoSplitLeftRight(con)),
                new MenuItem('8', "SplitTop, SplitBottom", () =>  SplitDemo.DemoSplitTopBottom(con)),
                new MenuItem('9', "Nested window-scroll", () => TestNestedWindows(con)),
                new MenuItem('c', "clear screen", () => con.Clear()),
                new MenuItem('x', "Exit", () => { })

            );
            
            menu.OnBeforeMenuItem += (i) => { con.Clear();  };

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
                c.PrintAt(0,y, y.ToString());
                c.PrintAt(10,y, "10");
                c.PrintAt(20,y, "20");
                c.PrintAt(30,y, "30");
                c.PrintAt(40,y, "40");
                c.PrintAt(50,y, "50");
                c.PrintAt(60,y, "60");
            }
        }

    }
}

