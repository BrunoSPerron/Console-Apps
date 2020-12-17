using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Bumper : Entity
    {
        int animationFrame = 0;
        public Bumper(int x, int y) : base(x, y)
        {
            layer = 1;
        }

        protected override void Erase()
        {
            base.Erase();
            ExtendedConsole.VirtualErase(x - 1, y - 1, 3, 2);
        }

        protected override void Draw()
        {
            Erase();
            base.Draw();
            if (animationFrame > 0)
                ExtendedConsole.VirtualWrite("───", x - 1, y - 1);
            else
                ExtendedConsole.VirtualWrite("-", x, y);
        }

        public void Hit()
        {
            if (animationFrame == 0)
            {
                animationFrame += 15;
                needToDraw = true;
            }
        }

        public override void Update()
        {
            Erase();
            Draw();
            if (animationFrame > 0)
                animationFrame--;
        }

        public override string ToString()
        {
            return "I am a bumper";
        }
    }
}
