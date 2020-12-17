using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    abstract class Enemy : Entity
    {
        protected int hp;
        protected int damageAnimation;
        protected int destroyAnimation;
        protected int destroyAnimationLength;
        protected bool destroyed;

        protected int shootCooldown;
        protected int shootTimeVariance;
        protected int nextShoot;
        protected Game game;

        public Enemy(int x, Game parent) : base(x, 0)
        {
            damageAnimation = 0;
            destroyAnimation = 0;
            destroyed = false;

            game = parent;
        }

        public void SetAgressivity(int shootCooldown, int variance)
        {
            if (shootCooldown < 2)
                shootCooldown = 2;
            if (variance > shootCooldown)
                variance = shootCooldown - 1;

            this.shootCooldown = shootCooldown;
            shootTimeVariance = variance;

            SetNextShootDelay();
        }

        internal override void Damage()
        {
            hp--;
            game.AlterScore(10);
            if (hp > 0)
                damageAnimation = 3;
            else
            {
                game.AlterScore(threatValue);
                destroyAnimation = destroyAnimationLength;
                destroyed = true;
            }
        }

        protected void SetNextShootDelay()
        {
            Random rand = new Random();
            nextShoot = rand.Next(shootCooldown - shootTimeVariance, shootCooldown + shootTimeVariance);
        }

        protected virtual void Shoot()
        {
            SetNextShootDelay();
        }

        protected override void Draw()
        {
            base.Draw();
        }

        protected override void Erase()
        {
            base.Erase();
        }

        public override void Update()
        {
            base.Update();

            nextShoot--;
            if (nextShoot <= 0)
                Shoot();

        }

        public override string ToString()
        {
            return "I am your DOOM";
        }
    }
}
