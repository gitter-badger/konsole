using System.Runtime.InteropServices;

namespace Goblinfactory.Konsole.Platform.Windows
{
        [StructLayout(LayoutKind.Sequential)]
        public struct XY
        {
            public short X;
            public short Y;

            public XY(short X, short Y)
            {
                this.X = X;
                this.Y = Y;
            }
        };
    }
