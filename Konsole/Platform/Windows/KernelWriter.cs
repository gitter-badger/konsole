using Konsole;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}


// good references : http://cecilsunkure.blogspot.com/2011/11/windows-console-game-writing-to-console.html
