using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public abstract class Entity
    {
        protected int layer;
        protected int x;
        protected int y;
        protected bool needToDraw;
        internal int threatValue;
        public Entity(int x, int y, int layer = 2)
        {
            this.x = x;
            this.y = y;
            this.layer = layer;
            needToDraw = true;
            threatValue = 0;
        }

        public virtual void Move(int x = 0, int y = 0)
        {
            Erase();
            this.x += x;
            this.y += y;
            needToDraw = true;
        }

        protected virtual void Erase()
        {
            ExtendedConsole.SetActiveLayer(layer);
        }
        protected virtual void Draw()
        {
            ExtendedConsole.SetActiveLayer(layer);
        }

        internal virtual bool CheckCollision(int x, int y)
        {
            return false;
        }

        internal virtual void Damage()
        {

        }

        public virtual void Update()
        {
            if (needToDraw)
            {
                Draw();
                needToDraw = false;
            }
        }

        public override string ToString()
        {
            return "I am, I think";
        }
    }
}
