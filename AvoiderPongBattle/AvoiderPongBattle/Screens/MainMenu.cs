using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class MainMenu
    {
        public MainMenu()
        {
            Start();
        }

        void Start()
        {
            ExtendedConsole.VirtualClear();
            ExtendedConsole.AnimatedMenuBoxOpening(0, 0, 40, 40);
            SelectionMenu();
        }

        void SelectionMenu()
        {
            string[] choices = new string[3];
            choices[0] = "New Game";
            choices[1] = "HighScore";
            choices[2] = "Quit";
            int nextScreen = ExtendedConsole.ShowMenuAndGetChoice(choices, 24, 23);

            switch (nextScreen)
            {
                case 0:
                    GameController.nextScreen = 1;
                    break;
                case 1:
                    GameController.nextScreen = 2;
                    break;
                case 2:
                case (-1):
                    Environment.Exit(0);
                    break;

            }
        }

        public override string ToString()
        {
            return "I am the menu";
        }
    }
}
