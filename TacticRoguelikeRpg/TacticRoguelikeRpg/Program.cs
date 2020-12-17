using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace TacticRoguelikeRpg
{
    class Program
    {
        static void Main(string[] args)
        {

            //remove window rescaling
            IntPtr handle = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(handle, false);
            DeleteMenu(sysMenu, 0xF000, 0x00000000);    //resize
            DeleteMenu(sysMenu, 0xF030, 0x00000000);    //maximize
            
            //Set window size, remove scrollbar
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            Console.SetWindowSize(Console.WindowWidth + 1, 40);

            Console.Title = "Divine Remnant";
            Console.CursorVisible = false;
            
            ExtendedConsole.lineUIColor = Settings.UILineColor;

            System.Threading.Thread.Sleep(1000);

            MainGameController gameController = new MainGameController();
        }

        //Pinvoke used to prevent window rescaling
        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

    }

}
