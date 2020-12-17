using System;
using System.Collections.Generic;
using System.Text;

namespace TacticRoguelikeRpg
{
    class GameOverScreen
    {
        public void Start()
        {
            Console.Clear();
            Console.WriteLine("you suck...");
            System.Threading.Thread.Sleep(1000);
            Environment.Exit(0);
        }
    }
}
