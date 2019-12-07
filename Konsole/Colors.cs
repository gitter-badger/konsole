using Goblinfactory.Konsole.Platform.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Goblinfactory.Konsole.Platform.Windows.Kernel32Draw;

namespace Konsole
{
    public class Colors
    {
        public ConsoleColor Foreground { get; } = ConsoleColor.White;
        public ConsoleColor Background { get; } = ConsoleColor.Black;

        public Colors()
        {
            
        }

        public Colors(ConsoleColor foreground, ConsoleColor background)
        {
            Foreground = foreground;
            Background = background;
        }

        public short ToAttributes()
        {
            int backAtt = (int)Foreground + (short)((int)Background << 4);
            return (short)backAtt;
        }

        public static Colors WhiteOnBlack
        {
            get
            {
                return new Colors(ConsoleColor.Gray, ConsoleColor.Black);
            }
        }

        public static Colors BlackOnWhite
        {
            get
            {
                return new Colors(ConsoleColor.Black, ConsoleColor.Gray);
            }
        }

        public CharAndColor SetChar(char c)
        {
            // something fish about this ...should not have to set attributes twice? TODO: take a closer look at the CharAndColor and CharInfo and see if we can simplify our representation of CHAR_INFO that works with unicode.
            // this is far too complicated for setting a simple screen char with some preset defaults.
            var @char = new CharAndColor() { Attributes = ToAttributes(), Char = new CharInfo() { UnicodeChar = c, Attributes = ToAttributes() } };
            return @char;
        }
    }
}
