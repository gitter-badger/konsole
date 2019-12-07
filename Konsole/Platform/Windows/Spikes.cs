using Konsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goblinfactory.Konsole.Platform.Windows
{
    // TODO: Remove or make private
    public static class Spikes
    {
        public static void Test1()
        {
            using (var writer = new HighSpeedWriter(90, 30))
            {
                var window = new Window(writer);
                window.PrintAt(10,10, "1 -> ╩ ╧ ╤ ╧ ╧ ╤ ╪  <- 3");
                writer.Flush();
            }
        }
    }
}
