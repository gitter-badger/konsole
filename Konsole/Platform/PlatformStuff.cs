using Goblinfactory.Konsole.Platform.Windows;
using System;

namespace Goblinfactory.Konsole.Platform
{
    public enum Platform
    {
        Windows,
        Unix,
        IOS
    }
    public class PlatformStuff : IPlatformStuff
    {
        private IPlatformStuff _platformStuff;
        public PlatformStuff()
        {
            // detect platform and "wire up? " a provider?
            // hard code to windows for now
            _platformStuff = new WindowsPlatformStuff();
        }

        public void EnableNativeRendering(int width, int height, bool allowClose = true, bool allowMinimize = true)
        {
            CheckOS();
            new WindowsPlatformStuff().LockResizing(width, height, allowClose, allowMinimize);
        }

        internal static void CheckOS()
        {
            // fake checking OS (not available in .net 4.5) by simply calling SetCursor which will throw exception on non windows.
            try
            {
                Console.CursorVisible = !Console.CursorVisible;
                Console.CursorVisible = !Console.CursorVisible;
            }
            catch
            {
                throw new Exception("EnableNativeRendering and LockResizing with Konsole is currently only supported on windows.");
            }
        }

        public void LockResizing(int width, int height, bool allowClose = true, bool allowMinimize = true)
        {
            CheckOS();
            _platformStuff.LockResizing(width, height, allowClose, allowMinimize);
        }
    }
}
