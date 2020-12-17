using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using Game;

namespace AvoiderPongBattle
{
    class Program
    {
        static void Main(string[] args)
        {
            //Set window size, remove scrollbar
            Console.SetWindowSize(40, 40);
            Console.SetBufferSize(40, 40);

            //remove window rescaling
            IntPtr handle = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(handle, false);
            DeleteMenu(sysMenu, 0xF000, 0x00000000);    //resize
            DeleteMenu(sysMenu, 0xF030, 0x00000000);    //maximize


            Console.Title = "Pongy Battle";
            Console.CursorVisible = false;

            ExtendedConsole.UpdateVirtualConsoleSize();


            GameController game = new GameController();
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
