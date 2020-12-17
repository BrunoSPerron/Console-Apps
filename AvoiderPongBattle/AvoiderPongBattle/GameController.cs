using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class GameController
    {
        static public int nextScreen { get; set; } = -1;

        bool gameIsOver = false;
        public GameController()
        {
            while (!gameIsOver)
                Navigate();
        }

        public void Navigate()
        {
            switch (nextScreen)
            {
                case 0:
                    gameIsOver = true;
                    break;
                case 1:
                    Game game = new Game();
                    break;
                case 2:
                    HighScore highScore = new HighScore();
                    break;
                default:
                    MainMenu main = new MainMenu();
                    break;
            }
        }

        public override string ToString()
        {
            return "Am I god?";
        }
    }
}
