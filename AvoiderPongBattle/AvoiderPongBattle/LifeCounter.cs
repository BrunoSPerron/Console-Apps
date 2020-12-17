using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class LifeCounter
    {
        internal int nbOfLife;
        private char lifeSymbol;
        internal LifeCounter(int nbOfLife = 3)
        {
            this.nbOfLife = nbOfLife;
            lifeSymbol = '¤';
            Draw();
        }

        internal void AddLive(int i = 1)
        {
            nbOfLife += i;
            Draw();
        }
        internal void RemoveLive(int i = 1)
        {
            nbOfLife -= i;
            Draw();
        }

        internal void Draw()
        {
            ExtendedConsole.SetActiveLayer(3);
            ExtendedConsole.VirtualErase(1, 2, 39, 1);
            ExtendedConsole.VirtualWrite("lives", 33, 1);
            for (int i = 0; i < nbOfLife; i++)
                ExtendedConsole.VirtualWrite(lifeSymbol, 37 - i * 2, 2);
        }

        public override string ToString()
        {
            return "I am your destiny";
        }
    }
}
