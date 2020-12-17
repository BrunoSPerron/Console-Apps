using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class ProjTrail : Entity
    {
        int showTime;
        Game game;
        bool isSplitted;

        public ProjTrail(int x, int y, int time, bool splitted, Game game) : base(x, y)
        {
            isSplitted = splitted;
            showTime = time;
            this.game = game;
            layer = 0;
        }

        protected override void Draw()
        {
            base.Draw();

            if (isSplitted)
                Console.ForegroundColor = ConsoleColor.DarkGray;

            if (showTime > 4)
                ExtendedConsole.VirtualWrite("o", x, y);
            else
                ExtendedConsole.VirtualWrite(".", x, y);
            
            Console.ForegroundColor = ConsoleColor.White;
        }

        protected override void Erase()
        {
            base.Erase();
            ExtendedConsole.VirtualErase(x, y, 1, 1);
        }

        public override void Update()
        {
            if (showTime == 0)
            {
                Erase();
                game.RemoveEntity(this);
            }
            else
                Draw();

            showTime--;
        }

        public override string ToString()
        {
            return "I'm dying in " + showTime + " tick";
        }
    }
}
