using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class SimpleEnemy : Enemy
    {
        private int targetHeight;
        private bool isMovingLeft;

        string appearance;

        public SimpleEnemy(int x, int targetHeight, Game parent) : base(x, parent)
        {
            appearance = "+-O-+";

            hp = 3;

            destroyAnimationLength = 5;

            this.targetHeight = targetHeight;

            shootCooldown = 50;
            shootTimeVariance = 20;
            threatValue = 100;

            SetNextShootDelay();
        }

        protected override void Shoot()
        {
            bool shootLeft = false;
            if ((!isMovingLeft && x != 1) || x == 39 - appearance.Length)
                shootLeft = true;

            if (shootLeft)
            {
                ExtendedConsole.VirtualWrite("\\", x, y);
                game.CreateProjectile(x - 1, y + 1, -1, 1);
            }
            else
            {
                ExtendedConsole.VirtualWrite("/", x + appearance.Length - 1, y);
                game.CreateProjectile(x + appearance.Length, y + 1, 1, 1);
            }

            base.Shoot();
        }

        protected override void Draw()
        {
            if (destroyed)
            {
                if (destroyAnimation > 0)
                {
                    if ((destroyAnimation / 2) % 2 == 1)
                        ExtendedConsole.VirtualWrite("-XxX-", x, y);
                    else
                        ExtendedConsole.VirtualWrite("Xx-xX", x, y);
                    destroyAnimation--;
                }
                else
                {
                    game.RemoveEntity(this);
                    Erase();
                }

            }
            else if (damageAnimation > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                damageAnimation--;
                base.Draw();
                ExtendedConsole.VirtualWrite(appearance, x, y);
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                base.Draw();
                ExtendedConsole.VirtualWrite(appearance, x, y);
            }

        }

        protected override void Erase()
        {
            base.Erase();
            ExtendedConsole.VirtualErase(x, y, appearance.Length, 1);
        }

        internal override bool CheckCollision(int x, int y)
        {
            if (y == this.y && x >= this.x && x <= this.x + appearance.Length)
                return true;
            return false;
        }

        public override void Update()
        {
            Erase();
            needToDraw = true;

            if (y < targetHeight)
                y++;
            else if (isMovingLeft)
                if (x <= 1)
                    isMovingLeft = false;
                else
                    x--;
            else if (x >= 39 - appearance.Length)
                isMovingLeft = true;
            else
                x++;

            base.Update();

            nextShoot--;
            if (nextShoot <= 0)
                Shoot();
        }

        public override string ToString()
        {
            return "I'm kinda dangerous";
        }

    }
}
