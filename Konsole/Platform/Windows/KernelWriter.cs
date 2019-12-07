﻿using Goblinfactory.Konsole.Internal;
using Konsole;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Goblinfactory.Konsole.Platform.Windows.Kernel32Draw;

namespace Goblinfactory.Konsole.Platform.Windows
{

    public interface IHighspeedWriter : IConsole
    {
        void ClearScreen();
        char ClearScreenChar { get; set; }
        short ColorAttributes { get; }
        void Flush();

        bool AutoFlush { get; }
    }

    /// <summary>
    /// HighSpeedWriter is native to each platform. Currently only windows supported. The plan is to create more highspeed writers for more platforms.
    /// The highspeed writer completely replaces the native Console and unlike the normal Konsole.Window is not compatible with side by side console writing.
    /// i.e. you have to use Konsole for ALL console writing.
    /// </summary>
    public class HighSpeedWriter : IDisposable, IConsole
    {
        private bool disposedValue = false;
        private readonly short _height;
        private readonly short _width;
        private ConsoleRegion _consoleWriteArea;
        CharAndColor[] _buffer;

        SafeFileHandle _consoleFileHandle;

        public HighSpeedWriter() : this((short)Console.WindowWidth, (short)Console.WindowHeight)
        {

        }
        public HighSpeedWriter(short width, short height, Colors defaultColors = null, char clearScreenChar = ' ')
        {
           new PlatformStuff().LockResizing(width, height);
            _consoleFileHandle = OpenConsole();
            _height = height;
            _width = width;
            Colors = defaultColors ?? new Colors(ConsoleColor.Gray, ConsoleColor.Black);
            ClearScreenChar = CharAndColor.From(clearScreenChar, Colors);
            _buffer = new CharAndColor[_width * _height];
            _consoleWriteArea = new ConsoleRegion(0, 0, (short)(_width - 1), (short)(_height - 1));
            ClearScreen();
        }

        public bool Autoflush { get; set; } = false;
        public CharAndColor ClearScreenChar { get; set; }

        public void ClearScreen()
        {
            for(int x = 0; x< _width; x++)
                for(int y = 0; y<_height; y++)
                {
                    int xy = y * _width + x;
                    _buffer[xy] = ClearScreenChar;
                }
        }

        private void doFlush()
        {
            if (Autoflush) Flush();
        }
        public void Flush()
        {
            WriteConsoleOutputW(
                _consoleFileHandle, 
                _buffer, 
                new XY() { X = _width, Y = _height }, 
                new XY() { X = 0, Y = 0 }, ref _consoleWriteArea);
        }

        public Colors Colors
        {
            get
            {
                return new Colors(ForegroundColor, BackgroundColor);
            }
            set
            {
                ForegroundColor = value.Foreground;
                BackgroundColor = value.Background;
            }
        }

        private short _backgroundColor;
        private short _foregroundColor;
        public short ColorAttributes {
            get { return (short)(_foregroundColor + ((short)(_backgroundColor << 4))); }
            }
        public ConsoleColor BackgroundColor { 
            get { return (ConsoleColor)_backgroundColor; }
            set { _backgroundColor = (short)value; }
        }

        public ConsoleColor ForegroundColor {
            get { return (ConsoleColor) _foregroundColor; }
            set { _foregroundColor = (short)value; }
        }

        //TODO: Move this to default interface implementations C#8
        public ConsoleState State
        {
            get { return new ConsoleState(ForegroundColor, BackgroundColor, CursorTop, CursorLeft, CursorVisible); }
            set
            {
                ForegroundColor = value.ForegroundColor;
                BackgroundColor = value.BackgroundColor;
                CursorTop = value.Top;
                CursorLeft = value.Left.CheckWidth();
            }
        }


        public int AbsoluteX => 0;

        public int AbsoluteY => 0;

        public int WindowWidth => _width;

        public int WindowHeight => _height;

        public int CursorTop { get; set; }
        public int CursorLeft { get; set; }

        public bool CursorVisible { get => false; set { } }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                _consoleFileHandle.Dispose();
                disposedValue = true;
            }
        }

         ~HighSpeedWriter()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void DoCommand(IConsole console, Action action)
        {
            throw new NotImplementedException();
        }

        public void PrintAt(int x, int y, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void PrintAt(int x, int y, string text)
        {
            // int theory there should be no overflow because Window._write has already taken care of that. 
            // so all I need to do is handle printing one line of text (just the buffer for that text)
            int span = text.Length;
            for(int i = 0; i< span; i++)
            {
                PrintAt(x + i, y, text[i]);
            }
            doFlush();
        }

        private CharAndColor ToCell(char c)
        {
            return new CharAndColor { Char = new CharInfo { UnicodeChar = c }, Attributes = ColorAttributes };
        }
        public void PrintAt(int x, int y, char c)
        {
            _buffer[x + y * _width] = ToCell(c);
        }

        public void PrintAtColor(ConsoleColor foreground, int x, int y, string text, ConsoleColor? background)
        {
            throw new NotImplementedException();
        }

        public void ScrollUp()
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Clear(ConsoleColor? backgroundColor)
        {
            throw new NotImplementedException();
        }

        public void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop, char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor)
        {
            throw new NotImplementedException();
        }

        public void WriteLine(ConsoleColor color, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void WriteLine(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Write(ConsoleColor color, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Write(string format, params object[] args)
        {
            throw new NotImplementedException();
        }
    }
}


// good references : http://cecilsunkure.blogspot.com/2011/11/windows-console-game-writing-to-console.html
