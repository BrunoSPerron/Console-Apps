using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class MediumEnemy : Enemy
    {
        private int targetHeight;
        private int currentMovement;
        private bool isMovingLeft;
        

        public MediumEnemy(int x, int targetHeight, Game parent) : base(x, parent)
        {
            hp = 4;

            destroyAnimationLength = 5;

            this.targetHeight = targetHeight;

            shootCooldown = 50;
            shootTimeVariance = 20;
            threatValue = 150;

            SetNextShootDelay();
        }

        protected override void Shoot()
        {
            game.SpawnBomb(x + 3,y);
            base.Shoot();
        }

        protected override void Draw()
        {
            if (destroyed)
            {
                if (destroyAnimation > 0)
                {
                    switch (destroyAnimation)
                    {
                        case 6:
                            ExtendedConsole.VirtualWrite("-( + )-", x, y);
                            break;
                        case 5:
                            ExtendedConsole.VirtualWrite("( -+- )", x, y);
                            break;
                        case 4:
                            ExtendedConsole.VirtualWrite("-+X+-", x + 1, y);
                            break;
                        case 3:
                            ExtendedConsole.VirtualWrite("-+xXx+-", x, y);
                            break;
                        case 2:
                            ExtendedConsole.VirtualWrite("-+X+-", x + 1, y);
                            break;
                        case 1:
                            ExtendedConsole.VirtualWrite("-+-", x + 2, y);
                            break;
                        case 0:
                            ExtendedConsole.VirtualWrite("-", x + 3, y);
                            Console.ForegroundColor = ConsoleColor.White;
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
                DrawShip(x, y);

                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                base.Draw();
                DrawShip(x, y);
            }

        }

        void DrawShip(int x, int y)
        {
            ExtendedConsole.VirtualWrite("+-( )-+", x, y);
        }

        protected override void Erase()
        {
            base.Erase();
            ExtendedConsole.VirtualErase(x, y, 7, 1);
        }

        internal override bool CheckCollision(int x, int y)
        {
            if (y == this.y && x >= this.x && x <= this.x + 7)
                return true;
            return false;
        }

        public override void Update()
        {
            Erase();
            needToDraw = true;

            if (y < targetHeight)
                y++;

            if (currentMovement < 1)
            {
                if (isMovingLeft)
                {
                    if (x <= 1)
                        isMovingLeft = false;
                    else
                        x--;
                }
                else
                {
                    if (x >= 32)
                        isMovingLeft = true;
                    else
                        x++;
                }
            }

            currentMovement--;

            base.Update();
        }

        public override string ToString()
        {
            return "I am dangerous";
        }
    }
}
