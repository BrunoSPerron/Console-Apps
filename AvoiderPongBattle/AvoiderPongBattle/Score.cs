using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Score
    {
        internal int currentScore { get; set; }
        public Score()
        {
            currentScore = 0;
            Draw();
        }

        internal void Draw()
        {
            ExtendedConsole.SetActiveLayer(3);
            ExtendedConsole.VirtualWrite(ToString(), 2, 1);
        }

        internal void Add(int i)
        {
            currentScore += i;
            Draw();
        }

        internal void Substract(int i)
        {
            currentScore -= i;
            if (currentScore < 0)
                currentScore = 0;
            Draw();
        }

        public override string ToString()
        {
            return "Score: " + currentScore;
        }
        
    }
}
