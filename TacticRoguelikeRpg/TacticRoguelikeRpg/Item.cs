using System;
using System.Collections.Generic;
using System.Text;

namespace TacticRoguelikeRpg
{
    enum ItemIcon
    {
        sword = 1
    }
    class Item
    {
        string name = "E:UNNAMED";
        int value = 10;
        int weight = 1;
        ItemIcon icon = new ItemIcon();
        ConsoleColor color = ConsoleColor.Gray;

        public virtual void Initiate(string _name, int _value, int _weight, ItemIcon _icon, ConsoleColor _color = ConsoleColor.Gray)
        {
            name = _name;
            value = _value;
            weight = _weight;
            icon = _icon;
            color = _color;
        }

        public virtual string[][] GetAttackAreaForDirection(int _direction)
        {
            return null;
        }

        public ConsoleColor GetColor()
        {
            return color;
        }
    }
}
