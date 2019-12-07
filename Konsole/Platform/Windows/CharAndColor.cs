using Konsole;
using System;
using System.Runtime.InteropServices;

namespace Goblinfactory.Konsole.Platform.Windows
{
    public partial class Kernel32Draw
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct CharAndColor
        {
            [FieldOffset(0)] public CharInfo Char;
            [FieldOffset(2)] public short Attributes;
            public static CharAndColor From(char c, Colors colors)
            {
                return new CharAndColor()
                {
                    Char = new CharInfo() { UnicodeChar = c },
                    Attributes = colors.ToAttributes()
                };
            }
        }

    }
}