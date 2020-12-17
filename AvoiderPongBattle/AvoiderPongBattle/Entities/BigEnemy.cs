using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class BigEnemy : Enemy
    {
        private int targetHeight;
        private bool isMovingLeft;

        int splitProjectilesCooldown;

        string appearance;

        public BigEnemy(int x, int targetHeight, Game parent) : base(x, parent)
        {
            appearance = "+-\\( )/-+";

            hp = 5;

            destroyAnimationLength = 9;

            this.targetHeight = targetHeight;

            shootCooldown = 50;
            shootTimeVariance = 20;
            threatValue = 300;

            SetNextShootDelay();
            splitProjectilesCooldown = nextShoot + 2; 
        }

        protected override void Shoot()
        {
            ExtendedConsole.SetActiveLayer(layer);
            ExtendedConsole.VirtualWrite("\\", x, y);
            ExtendedConsole.VirtualWrite("/", x + 8, y);
            game.CreateProjectile(x + 9, y + 1, 1, 1);
            game.CreateProjectile(x - 1, y + 1, -1, 1);
            base.Shoot();
            splitProjectilesCooldown = 15;
        }

        void SplitProjectiles()
        {
            game.SplitProjectiles();
            splitProjectilesCooldown = 50;
        }

        protected override void Draw()
        {
            if (destroyed)
            {
                if (destroyAnimation > 0)
                {
                    switch (destroyAnimation)
                    {
                        case 9:
                            ExtendedConsole.VirtualWrite("-/(   )\\-", x, y);
                            break;
                        case 8:
                            ExtendedConsole.VirtualWrite("\\(     )/", x, y);
                            break;
                        case 7:
                            ExtendedConsole.VirtualWrite("(       )", x, y);
                            break;
                        case 6:
                            ExtendedConsole.VirtualWrite("(     )", x + 1, y);
                            break;
                        case 5:
                            ExtendedConsole.VirtualWrite("(   )", x + 2, y);
                            break;
                        case 4:
                            ExtendedConsole.VirtualWrite("( )", x + 3, y);
                            break;
                        case 3:
                            ExtendedConsole.VirtualWrite("0", x + 4, y);
                            break;
                        case 2:
                            ExtendedConsole.VirtualWrite("X", x + 4, y);
                            break;
                        case 1:
                            ExtendedConsole.VirtualWrite("+", x + 4, y);

                            break;
                        case 0:
                            ExtendedConsole.VirtualWrite(".", x + 4, y);
                            break;
                    }
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
            ExtendedConsole.VirtualErase(x, y, 9, 1);
        }

        internal override bool CheckCollision(int x, int y)
        {
            if (y == this.y && x >= this.x && x <= this.x + 9)
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
                if (x <= 5)
                    isMovingLeft = false;
                else
                    x--;
            else if (x >= 25)
                isMovingLeft = true;
            else
                x++;

            base.Update();

            nextShoot--;
            if (nextShoot <= 0)
                Shoot();

            base.Update();
            splitProjectilesCooldown--;
            if (splitProjectilesCooldown <= 0)
                SplitProjectiles();
        }
    }
}
