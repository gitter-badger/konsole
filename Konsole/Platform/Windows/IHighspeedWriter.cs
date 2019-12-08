using Konsole;
using System;

namespace Goblinfactory.Konsole.Platform.Windows
{

    public interface IHighspeedWriter : IDisposable
    {
        void ClearScreen();
        char ClearScreenChar { get; set; }

        Colors Colors { get; set; }
        void Flush();

        bool AutoFlush { get; }
    }
}
