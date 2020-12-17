using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class BombProjectile : Entity
    {
        int countDown;
        Game game;
        public BombProjectile(int x, int y, Game game) : base(x, y)
        {
            Random rand = new Random();
            countDown = rand.Next(3, 15);
            this.game = game;
        }

        protected override void Draw()
        {
            base.Draw();
            ExtendedConsole.VirtualWrite("¤", x, y);
        }

        protected override void Erase()
        {
            base.Erase();
            ExtendedConsole.VirtualErase(x, y, 1, 1);
        }

        private void Explode()
        {
            game.CreateProjectile(x, y, 1, 1, true);
            game.CreateProjectile(x, y, -1, 1, true);
            game.CreateProjectile(x, y, 1, -1, true);
            game.CreateProjectile(x, y, -1, -1, true);
            game.CreateProjectile(x, y, 1, 0, true);
            game.CreateProjectile(x, y, -1, 0, true);
            game.RemoveEntity(this);
        }

        public override void Update()
        {
            Erase();
            needToDraw = true;

            game.CreateProjectileTrail(x, y, false);
            
            y++;

            base.Update();

            countDown--;
            if (countDown < 1)
            {
                Erase();
                Explode();
            }
        }

        public override string ToString()
        {
            return "I am a bomb";
        }
    }
}
