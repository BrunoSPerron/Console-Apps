using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Projectile : Entity
    {
        int directionX;
        int directionY;

        Game game;
        bool canDamageEnemy;

        bool wasDeleted = false;

        bool isSplitted;

        public Projectile(int x, int y, int directionX, int directionY, Game game, bool splitted) : base(x, y)
        {
            layer = 1;
            this.directionX = directionX;
            this.directionY = directionY;
            this.game = game;
            canDamageEnemy = false;
            isSplitted = splitted;
            Draw();
        }
        public Projectile(int x, int y, int directionX, int directionY, Game game) : this (x,y, directionX, directionY, game, false)
        {
            
        }

        protected override void Draw()
        {
            base.Draw();
            if (isSplitted)
                Console.ForegroundColor = ConsoleColor.DarkGray;
            ExtendedConsole.VirtualWrite("O", x, y);
            Console.ForegroundColor = ConsoleColor.White;
        }

        protected override void Erase()
        {
            base.Erase();
            ExtendedConsole.VirtualErase(x, y, 1, 1);
        }

        internal void Split()
        {
            if (y < 30 && !isSplitted)
            {
                Erase();
                game.RemoveEntity(this);
                if (x < 38)
                    game.LateAddProjectile(x + 1, y + 1, 1, 1, true);
                if (x > 1)
                    game.LateAddProjectile(x - 1, y + 1, -1, 1, true);
                wasDeleted = true;
            }
            
        }

        public override void Update()
        {
            bool collision = false;
            Erase();
            game.CreateProjectileTrail(x, y, isSplitted);
            char nextPosition = ExtendedConsole.GetCharAtPosition(x + directionX, y + directionY);
            if (nextPosition == '║' || nextPosition == '│')
            {
                if (nextPosition == '│')
                    canDamageEnemy = true;
                if (directionY == 0)
                {
                    game.RemoveEntity(this);
                    wasDeleted = true;
                }
                directionX *= -1;
                nextPosition = ExtendedConsole.GetCharAtPosition(x + directionX, y + directionY);
            }
            if (nextPosition == '─')
            {
                directionY *= -1;
                canDamageEnemy = true;
                game.AlterScore(1);
            }

            if (directionY == 0)
            {
                Move(directionX, directionY);
                nextPosition = ExtendedConsole.GetCharAtPosition(x + directionX, y + directionY);
                if (nextPosition == '║' || nextPosition == '│')
                {
                    game.RemoveEntity(this);
                    wasDeleted = true;
                }
            }

            if (!wasDeleted)
            {
                Move(directionX, directionY);
                Draw();
            }

            if (y == 39 || y == 0)
            {
                Erase();
                game.RemoveEntity(this);
            }

            if (nextPosition != '\0')
            {
                collision = game.CheckProjectileCollision(x, y, canDamageEnemy);
                if (collision)
                {
                    Erase();
                    game.RemoveEntity(this);
                }
            }
        }

        public override string ToString()
        {
            return "I'm fast and deadly";
        }
    }
}
