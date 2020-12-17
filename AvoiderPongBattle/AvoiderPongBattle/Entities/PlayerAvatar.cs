using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class PlayerAvatar : Entity
    {
        private Game game;

        private Bumper leftBumper;
        private Bumper rightBumper;

        private Beam beam;

        public bool isDestroyed = false;
        public int destroyedAnimation = 0;
        private int damageAnimation = 0;

        private string appearance;

        public bool beamIsActive { get; set; }
        public bool movingLeft { get; set; }
        public bool movingRight { get; set; }

        public PlayerAvatar(int x, int y, Game game) : base(x, y)
        {
            this.game = game;
            appearance = "==O==";
            leftBumper = new Bumper(x - 1, y);
            rightBumper = new Bumper(x + appearance.Length, y);
            beam = new Beam(x);
        }

        protected override void Erase()
        {
            base.Erase();
            ExtendedConsole.VirtualErase(x - 1, y, appearance.Length + 2, 1);
        }

        protected override void Draw()
        {
            base.Draw();
            if (damageAnimation > 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                damageAnimation--;
            }
            if (isDestroyed)
            {
                switch (destroyedAnimation)
                {
                    case 6:
                        ExtendedConsole.VirtualWrite(" ==¤== ", x - 1, y);
                        break;
                    case 5:
                        ExtendedConsole.VirtualWrite(" ¤xXx¤ ", x - 1, y);
                        break;
                    case 4:
                        ExtendedConsole.VirtualWrite("¤xXxXx¤", x - 1, y);
                        break;
                    case 3:
                        ExtendedConsole.VirtualWrite("xXx.xXx", x - 1, y);
                        break;
                    case 2:
                        ExtendedConsole.VirtualWrite("Xx. .xX", x - 1, y);
                        break;
                    case 1:
                        ExtendedConsole.VirtualWrite("x.   .x", x - 1, y);
                        break;
                    case 0:
                        ExtendedConsole.VirtualWrite(".     .", x - 1, y);
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
                destroyedAnimation--;
            }
            else
            {

                ExtendedConsole.VirtualWrite(appearance, x, y);

                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public override void Move(int x = 0, int y = 0)
        {
            leftBumper.Move(x, y);
            rightBumper.Move(x, y);
            base.Move(x, y);
        }

        internal override bool CheckCollision(int x, int y)
        {
            if (y == this.y && x >= this.x && x < this.x + appearance.Length)
                return true;
            return false;
        }

        internal override void Damage()
        {
            damageAnimation = 4;
            isDestroyed = game.RemoveLife();
            if (isDestroyed)
                destroyedAnimation = 6;
        }

        public override void Update()
        {
            leftBumper.Update();
            rightBumper.Update();

            if (isDestroyed || damageAnimation > 0)
            {
                Erase();
                needToDraw = true;
            }

            base.Update();

            if (beamIsActive && !beam.isActive)
            {
                beam.SetPosition(x + 2);
                beam.isActive = true;
                beam.Update();
            }
            else if (!beamIsActive && beam.isActive)
            {
                beam.isActive = false;
                beam.Update();
            }
        }

        public void HitLeft()
        {
            if(!isDestroyed)
                leftBumper.Hit();
        }

        public void HitRight()
        {
            if (!isDestroyed)
                rightBumper.Hit();
        }

        public int GetposX()
        {
            return x;
        }

        public override string ToString()
        {
            return "I'm you!";
        }
    }
}
