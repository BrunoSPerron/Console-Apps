using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Beam : Entity
    {
        internal bool isActive { get; set; }

        internal Beam(int x) : base(x, 38)
        {
            layer = 1;
            isActive = false;
        }

        internal void SetPosition(int x)
        {
            this.x = x;
        }

        protected override void Erase()
        {
            base.Erase();
            ExtendedConsole.VirtualErase(x, 0, 1, 38);
        }

        protected override void Draw()
        {
            base.Draw();
            for (int i = 1; i < 37; i++)
                ExtendedConsole.VirtualWrite('│', x, i);
            ExtendedConsole.VirtualWrite('^', x, 37);
        }

        public override void Update()
        {
            if (isActive)
                Draw();
            else
                Erase();
        }

        public override string ToString()
        {
            return "I am a Beam";
        }
    }
}
