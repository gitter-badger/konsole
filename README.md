# Konsole library

home of the simple no-dependancy console libary consisting of:

### ProgressBar , Window  , Form , Menu , Draw & MockConsole

---



## Installing

![install-package Goblinfactory.Konsole](install-package.png)

## ProgressBar usage - simple syntax
```csharp
    using Konsole;
           
            var pb = new ProgressBar(PbStyle.DoubleLine, 50);
            pb.Refresh(0, "connecting to server to download 5 files asychronously.");
            Console.ReadLine();

            pb.Refresh(25, "downloading file number 25");
            Console.ReadLine();
            pb.Refresh(50, "finished.");
```

# ProgressBars

## ProgressBarTwoLine (alternative style)

![sample output](progressbar.gif)

## ProgressBar worked parallel example
```csharp
    using Konsole;
           
    Console.WriteLine("ready press enter.");
    Console.ReadLine();

    var dirCnt = 15;
    var filesPerDir = 100;
    var r = new Random();
    var q = new ConcurrentQueue<string>();
    foreach (var name in TestData.MakeNames(2000)) q.Enqueue(name);
    var dirs = TestData.MakeObjectNames(dirCnt).Select(dir => new
    {
        name = dir,
        cnt = r.Next(filesPerDir)
    });

    var tasks = new List<Task>();
    var bars = new ConcurrentBag<ProgressBar>();
    foreach (var d in dirs)
    {
        var files = q.Dequeue(d.cnt).ToArray();
        if (files.Length == 0) continue;
        tasks.Add(new Task(() =>
        {
            var bar = new ProgressBar(files.Count());
            bars.Add(bar);
            bar.Refresh(0, d.name);
            ProcessFakeFiles(d.name, files, bar);
        }));
    }

    foreach (var t in tasks) t.Start();
    Task.WaitAll(tasks.ToArray());
    Console.WriteLine("done.");
```
![sample output](progressbar2.gif)

# Windows

  - ( 100%-ish console compatible window, supporting all normal console writing to a windowed section of the screen) 
  - Supports scrolling and clipping of console output.
  - typical uses, for showing a scrolling output, e.g. build output in a window, while showing higher level progress in another window.
  - automatic borders
  - full color support

```csharp
            var con = new Window(200,50).LockConsoleResizing();
            con.WriteLine("starting client server demo");
            var client = new Window(1, 4, 20, 20, ConsoleColor.Gray, ConsoleColor.DarkBlue, con);
            var server = new Window(25, 4, 20, 20, con);
            client.WriteLine("CLIENT");
            client.WriteLine("------");
            server.WriteLine("SERVER");
            server.WriteLine("------");
            client.WriteLine("<-- PUT some long text to show wrapping");
            server.WriteLine(ConsoleColor.DarkYellow, "--> PUT some long text to show wrapping");
            server.WriteLine(ConsoleColor.Red, "<-- 404|Not Found|some long text to show wrapping|");
            client.WriteLine(ConsoleColor.Red, "--> 404|Not Found|some long text to show wrapping|");

            con.WriteLine("starting names demo");
            // let's open a window with a box around it by using Window.Open
            var names = Window.Open(50, 4, 40, 10, "names");
            TestData.MakeNames(40).OrderByDescending(n => n).ToList()
                 .ForEach(n => names.WriteLine(n));

            con.WriteLine("starting numbers demo");
            var numbers = Window.Open(50, 15, 40, 10, "numbers", 
                  LineThickNess.Double,ConsoleColor.White,ConsoleColor.Blue);
            Enumerable.Range(1,200).ToList()
                 .ForEach(i => numbers.WriteLine(i.ToString())); // shows scrolling
```
**gives you**

![window simple demo](docs/window-demo.png)

# LockConsoleResizing

  When using `Konsole` `windows` we strongly recommend you `LockConsoleResizing()` to prevent the console from re-formatting the text in the console as you resize, without which you may see scenes similar to the following maddness below.

  You only need to call `LockConsoleResizing` once.

<img src='./docs/resized.png' width='600'>

To fix, simply call `LockConsoleResizing()` after you create a window. Or call in explicitly.

```csharp

// fluently when creating a window
// returns the window reference.
var c = new Window().LockConsoleResizing();

// if you need to call it explicitly
new PlatformStuff().LockResizing();
```

# Advanced windows with `SplitRows` and `SplitColumns`

You can create advanced window layouts using `SplitRows` and `SplitColumns` passing in a collection of Splits. Pass in a size of `0` to indicate that `row` or `column` window must contain the remainder of the window space. See examples below:

```csharp
            var c = new Window().LockConsoleResizing();
            var consoles = c.SplitRows(
                    new Split(4, "heading", LineThickNess.Single),
                    new Split(0),
                    new Split(4, "status", LineThickNess.Single)
            ); ; ;

            var headline = consoles[0];
            var status = consoles[2];

            var contents = consoles[1].SplitColumns(
                    new Split(20),
                    new Split(0, "content") { Foreground = ConsoleColor.White, Background = ConsoleColor.Cyan },
                    new Split(20)
            );
            var menu = contents[0];
            var content = contents[1];
            var sidebar = contents[2];

            headline.Write("my headline");
            content.WriteLine("content goes here");

            menu.WriteLine("Options A");
            menu.WriteLine("Options B");

            sidebar.WriteLine("20% off all items between 11am and midnight tomorrow!");

            status.Write("System offline!");
            Console.ReadLine();
```

Produces the following window. Each of the console(s) that you have a reference to can be written to like any normal console, and will scroll and clip correctly. You can create progress bar instances inside these windows like any console.

<img src='./docs/window-example.PNG' width='600' />

Configure the properties of each section of a window with the `Split` class.

```csharp
new Split(size) 
{
    title,
    lineThickNess, 
    foregroundColor,
    backgroundColor
};
```

# Side by side writing 

TBD : describe how Konsole workes side by side with existing code or apps that share the console.

# HighSpeedWriter

TBD

# Forms

  - quickly and neatly render an object and it's properties in a window or to the console.
  - support multiple border styles.
  - Support C# objects or dynamic objects

Readonly forms are currently rendered. Below are examples showing auto rendering of simple objects.
(Currently only text fields, readonly, simple objects.)
On the backlog; add additional field types, complex objects, and editing. 

```csharp
        using Konsole.Form;
        ...
            var form1 = new Form(80,new ThickBoxStyle());
            var person = new Person()
            {
                FirstName = "Fred",
                LastName = "Astair",
                FieldWithLongerName = "22 apples",
                FavouriteMovie = "Night of the Day of the Dawn of the Son 
                of the Bride of the Return of the Revenge of the Terror 
                of the Attack of the Evil, Mutant, Hellbound, Flesh-Eating 
                Subhumanoid Zombified Living Dead, Part 2: In Shocking 2-D"
            };
            form1.Write(person);
```

![sample output](Form-Person.png)


```csharp        

           // works with anonymous types
            new Form().Write(new {Height = "40px", Width = "200px"}, "Demo Box");
```
![sample output](Form-DemoBox.png)

```csharp        

            // change the box style, and width
            new Form(40, new ThickBoxStyle()).Show(new { AddUser= "true", CloseAccount = "false", OpenAccount = "true"}, "Permissions");
```
![sample output](Form-Permissions.png)


# Debugging problems with Konsole

## No visible output, blank screen

If you are using the `HighSpeedWriter` you must call `Flush()` to render the output.

<img src='docs/flush.PNG' width='400'/>

# MockConsole

## MockConsole (substitute), and IConsole (interface) usage

Use MockConsole as a real (fully functional) System.Console substitute, that renders with 100% fidelity to an internal buffer that can be used to assert correct console behavior of any object writing using IConsole.
MockConsole can even render out a text representation of the current state of the the console, including representations for the foreground and background color of anything written.

All the test for this library have been written using `MockConsole.` For a fully detailed examples of `MockConsole` being stretched to the limits, see `Konsole.Tests.WindowTests`.

```csharp
        
        using Konsole;
        ...
        public class Cat
        {
            private readonly IConsole _console;
            public Cat(IConsole console) { _console = console; }
            public void Greet()
            {
                _console.WriteLine("Prrr!");
                _console.WriteLine("Meow!");
            }
        }

        [Test]
        public void TestConsole_ConsoleWriter_and_IConsole_example_usage()
        {
            {
                // test the cat
                // ============
                var console = new TestConsole(80, 20);
                var cat = new Cat(console);
                cat.Greet();
                Assert.AreEqual(console.BufferWrittenTrimmed, new[] {"Prrr!", "Meow!"});
            }
            {
                // create an instance of a cat that will purr to the real Console
                // ==============================================================
                var cat = new Cat(new ConsoleWriter());
                cat.Greet(); // prints Prrr! aand Meow! to the console
            }
        }
```


## `MockConsole` vs `Mock<IConsole>`

Below is a comparison of how someone might test an Invoice class using a traditional `Mock<IConsole>` and the same test, using a `Konsole.MockConsole`. To make it a fair comparison I'm comparing to [NSubstitute](http://nsubstitute.github.io/) which is quite terse and one of my favourite mocking frameworks.

```csharp

        [Test]
        public void Test_Invoice_using_mocks()
        {
            // test the invoice
            // ============
            IConsole console = new Substitute.For<IConsole>();
            var invoice = new Invoice(console);
            invoice.AddLine(2, "Semi Skimmed Milk", "2 pints", "£",1.00);
            invoice.AddLine(3, "Warburtons Crumpets", "6 pack", "£",0.89);
            invoice.Print();
                
            // not really practical to test printed output using a mock console
            // ================================================================
            console.Received().SetCursorPosition(0,0);
            console.Received().WriteLine("ACME Wholesale Foody");
            console.Received().WriteLine("--------------------");
            console.Received().WriteLine("");
            console.Received().WriteLine("--------------------");
            console.Received().Write("qty ");
            console.Received().Write(2);
            console.Received().Write(" Semi Skimmed Milk");
            console.Received().Write(", ");
            console.Received().Write("{0} pints", 2);
            console.Received().Write("£ {0.00,-10}", 2.0m);
            .
            .
            . // and so on and so on ...for probably around another 12 or 13 lines.
            .
            .
             // having to mimick the exact formatted Write's and Writelines and SetCursor movements 
             // this is brittle, if the code is optimised to replace two Write's with a single formatted WriteLine for example
             // then this test fails even though the desired output is written to the console.
        }
        
```

using a Test Double like `Konsole.MockConsole` the test above becomes

```csharp
        [Test]
        public void testing_Invoice_class_using_MockConsole()
            {
                var expected = @"
                 ACME WHoleSale Foody 
                 -------------------- 
                 qty 2 Semi Skimmed Milk   , 2 pints     £ 2.00
                 qty 3 Warburtons Crumpets , 6 pack      £ 5.34
                 --------------
                 total   £ 7.34 
                 --------------
            
                * some random message on the footer
";
        
                var console = new MockConsole();
                var invoice = new Invoice(console);
                invoice.AddLine(2, "Semi Skimmed Milk", "2 pints", "£",1.00);
                invoice.AddLine(3, "Warburtons Crumpets", "6 pack", "£",0.89);
                invoice.Print();
                Assert.AreEqual(console.BufferString,expected);
                });
            }
                
            // Now, if someone accidentally changes your currency formatter, this test will wail
            // when the rendered output to the Console suddenly changes, bwaaam! Instant Fail.
            // Score one for MockConsole, sweetness, life is good!
        }


``` 

# Draw

TBD

## Cross platform notes
ProgressBar has been manually tested with Mono on Mac in version 1.0. I don't currently have any automated testing in place for OSX (mono) and Linux. That's all on the backlog.
It's possible I might split out the ProgressBar into a seperate nuget package, since that appeared to work remarkably well cross platform, while `Window` makes calls to some `System.Console` methods that are not supported in Mono.

The scrolling support currently uses `Console.MoveBufferArea` which is not implemented on Mono. I will be working on a suitable alrternative to this on Linux and OSX. (on the backlog) Biggest challenge will be doing crossplatform testing, ...mmm, I predict I will be eating [Cake](http://cakebuild.net/docs/tutorials/getting-started) and containers in the very near future!

# Source code

## Building the solution


### using visual studio

 1. `git clone https://github.com/goblinfactory/konsole.git`
 2. run the following commands from the root folder;

### requirements

Any version of .net core. Update `global.json` to the version of .net core you have installed and run the command below in order.

 > dotnet restore

 > dotnet build
 
 > dotnet test
 


## ChangeLog

* [changelog](change-log.md)

The format is based on [Keep a Changelog](http://keepachangelog.com/) 
and this project adheres to [Semantic Versioning](http://semver.org/).

## Support, Feedback, Suggestions

Please drop me a tweet if you find Konsole useful. Latest updates to Konsole was written at Snowcode 2017.
keep chillin!

    O__
    _/`.\
        `=( 

Alan

[p.s. join us at snowcode 2018! ](http://www.snowcode.com?refer=konsole) <br/>
[www.snowcode.com](http://www.snowcode.com?refer=konsole) <br/>
(free dev conf at great ski resort)<br/>
developers + party + snow + great learning

[@snowcode](https://twitter.com/snowcode)

![Alan Hemmings](https://pbs.twimg.com/profile_images/624901555532095488/j5dynw0i_bigger.png)