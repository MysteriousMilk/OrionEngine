using Orion.Platform.Win32;
using System;

namespace PhysicsExample
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ModuleLoader moduleLoader = new ModuleLoader(
                Orion.Core.Module.ModuleAccessMode.Streaming,
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.None) + @"\Orion\"
                );

            using (var game = new Game1(moduleLoader, new SQLite.Net.Platform.Win32.SQLitePlatformWin32()))
            {
                game.Content.RootDirectory = "Data";
                game.Run();
            }
        }
    }
#endif
}
