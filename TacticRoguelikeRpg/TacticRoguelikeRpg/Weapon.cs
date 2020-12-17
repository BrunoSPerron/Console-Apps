using System;
using System.Collections.Generic;
using System.Text;

namespace TacticRoguelikeRpg
{
    enum WeaponAnimType
    {
       ShortSword = 1,
       Dagger = 2,
       Spear = 3,
       GiantFlail = 4,
       Boomerang = 5,
       Axe = 6
    }

    class Weapon : Item
    {
        public int damage { get; set; } = 1;
        WeaponAnimType _animationIndex = WeaponAnimType.Boomerang;

        public override void Initiate(string _name, int _value, int _weight, ItemIcon _icon, ConsoleColor _color = ConsoleColor.Gray)
        {
            base.Initiate(_name, _value, _weight, _icon, _color);
        }

        public void Initiate(string _name, int _value, int _weight, ItemIcon _icon, int _damage, WeaponAnimType _attackAnimIndex, ConsoleColor _color = ConsoleColor.Gray)
        {
            base.Initiate(_name, _value, _weight, _icon, _color);
            damage = _damage;
            _animationIndex = _attackAnimIndex;
        }



        /*      How to add animation: 
         *      initiate _area[number of frames in animation][]
         *      initiate each frames in animation, new string[max distance from the character]
         *      assign each case to update for each frame
         *          - the interpreter Split('&') each line, and force the result to be odd numbered
         *          - the interpretor assign each splitted part a position to be drawn, counterclockwise centered on attack position
         *          - the second line is centered 2 tile away from character, the third 3 away, etc
         *          - the diagonal attack are drawn at a 90 degree angle for full tile cover
         */
        public override string[][] GetAttackAreaForDirection(int _direction)
        {
            string[][] _area = new string[1][];
            if (_animationIndex == WeaponAnimType.ShortSword)   
            {
                _area = new string[4][];
                _area[0] = new string[1];
                _area[1] = new string[1];
                _area[2] = new string[1];
                _area[3] = new string[1];
                if (_direction == 3 || _direction == 7)
                    _area[0][0] = "\\\\";
                if (_direction == 9 || _direction == 1)
                    _area[1][0] = "//";
                if (_direction == 8 || _direction == 2)
                    _area[2][0] = "||";
                if (_direction == 4 || _direction == 6)
                    _area[3][0] = "==";
            } 

            if (_animationIndex == WeaponAnimType.Dagger)   
            {
                _area = new string[5][];
                _area[0] = new string[1];
                _area[1] = new string[1];
                _area[2] = new string[1];
                _area[3] = new string[1];
                _area[4] = new string[1];
                if (_direction == 6 ||_direction == 8 || _direction == 9)
                {
                    _area[0][0] = "/ ";
                    _area[1][0] = " \\";
                    _area[2][0] = "";
                    _area[3][0] = "\\ ";
                    _area[4][0] = " / ";
                }
                if (_direction == 1 || _direction == 2 || _direction == 4)
                {
                    _area[0][0] = " /";
                    _area[1][0] = "\\/";
                    _area[2][0] = "";
                    _area[3][0] = " \\";
                    _area[4][0] = "/\\ ";
                }
                if (_direction == 7)
                {
                    _area[0][0] = " \\";
                    _area[1][0] = "/\\";
                    _area[2][0] = "";
                    _area[3][0] = " /";
                    _area[4][0] = "\\/ ";
                }
                if (_direction == 3)
                {
                    _area[0][0] = "\\ ";
                    _area[1][0] = "\\/";
                    _area[2][0] = "";
                    _area[3][0] = " /";
                    _area[4][0] = "\\/ ";
                }
            } 

            if (_animationIndex == WeaponAnimType.Spear)
            {
                _area = new string[3][];
                _area[0] = new string[1];
                _area[1] = new string[2];
                _area[2] = new string[2];
                switch (_direction)
                {
                    case 1:
                        _area[0][0] = " └";

                        _area[1][0] = "//";
                        _area[1][1] = " └";

                        _area[2][0] = " └";
                        break;
                    case 2:
                        _area[0][0] = "\\´";

                        _area[1][0] = "][";
                        _area[1][1] = "\\´";

                        _area[2][0] = "\\´";
                        break;
                    case 3:
                        _area[0][0] = "┘ ";

                        _area[1][0] = "\\\\";
                        _area[1][1] = "┘ ";

                        _area[2][0] = "┘ ";
                        break;
                    case 4:
                        _area[0][0] = " <";

                        _area[1][0] = "==";
                        _area[1][1] = " <";

                        _area[2][0] = " <";
                        break;
                    case 6:
                        _area[0][0] = "> ";

                        _area[1][0] = "==";
                        _area[1][1] = "> ";

                        _area[2][0] = "> ";
                        break;
                    case 7:
                        _area[0][0] = " ┌";

                        _area[1][0] = "\\\\";
                        _area[1][1] = " ┌";

                        _area[2][0] = " ┌";
                        break;
                    case 8:
                        _area[0][0] = ",\\";

                        _area[1][0] = "][";
                        _area[1][1] = ",\\";

                        _area[2][0] = ",\\";
                        break;
                    case 9:
                        _area[0][0] = "┐ ";

                        _area[1][0] = "//";
                        _area[1][1] = "┐ ";

                        _area[2][0] = "┐ ";
                        break;
                }
                _area[2][1] = "";
            } 

            if (_animationIndex == WeaponAnimType.GiantFlail)
            {
                _area = new string[2][];
                _area[0] = new string[2];
                _area[1] = new string[4];
                if (_direction == 3 || _direction == 7)
                {
                    _area[0][0] = "\\\\";
                    _area[0][1] = "\\\\";

                    _area[1][0] = "\\\\";
                    _area[1][1] = "//&<>&//";
                    _area[1][2] = "\\\\";
                }
                if (_direction == 9 || _direction == 1)
                {
                    _area[0][0] = "//";
                    _area[0][1] = "//";

                    _area[1][0] = "//";
                    _area[1][1] = "\\\\&<>&\\\\";
                    _area[1][2] = "//";
                }
                if (_direction == 8 || _direction == 2)
                {
                    _area[0][0] = "||";
                    _area[0][1] = "||";

                    _area[1][0] = "||";
                    _area[1][1] = "||";
                    _area[1][2] = "==&<>&==";
                    _area[1][3] = "||";
                }
                if (_direction == 4 || _direction == 6)
                {
                    _area[0][0] = "==";
                    _area[0][1] = "==";

                    _area[1][0] = "==";
                    _area[1][1] = "==";
                    _area[1][2] = "||&<>&||";
                    _area[1][3] = "==";
                }
            } 

            if (_animationIndex == WeaponAnimType.Boomerang)
            {
                _area = new string[7][];
                _area[0] = new string[1];
                _area[1] = new string[2];
                _area[2] = new string[3];
                _area[3] = new string[4];
                _area[4] = new string[4];
                _area[5] = new string[3];
                _area[6] = new string[2];

                _area[0][0] = "( &&";

                _area[1][0] = "  &&";
                _area[1][1] = " )&&";

                _area[2][1] = "  &&";
                _area[2][2] = "C &&";

                _area[3][2] = "  &-/&";
                //_area[3][2] = "";

                //_area[4][3] = "  ";
                _area[4][2] = "&&(";

                _area[5][2] = "&&";
                _area[5][1] = "&& ~\\";

                _area[6][1] = "&&  ";
                _area[6][0] = "&&/";
            } 
            
            if (_animationIndex == WeaponAnimType.Axe)
            {
                _area = new string[5][];
                _area[0] = new string[1];
                _area[1] = new string[1];
                _area[2] = new string[1];
                _area[3] = new string[1];
                _area[4] = new string[1];

                if (_direction == 1)
                {
                    _area[0][0] = "­­--&&";
                    _area[1][0] = "__&&";
                    _area[2][0] = "&'┴&=|";
                    _area[3][0] = "&& |";
                    _area[4][0] = "&& |";
                }

                if (_direction == 2)
                {
                    _area[0][0] = "/ &&";
                    _area[1][0] = " !&&";
                    _area[2][0] = " -&-=&=|";
                    _area[3][0] = "&& |";
                    _area[4][0] = "&& |";
                }

                if (_direction == 3)
                {
                    _area[0][0] = "| &&";
                    _area[1][0] = " |&&";
                    _area[2][0] = "&,┤&╗╔";
                    _area[3][0] = "&&──";
                    _area[4][0] = "&&──";
                }

                if (_direction == 4)
                {
                    _area[0][0] = "¯¯&&";
                    _area[1][0] = "¯¯&&";
                    _area[2][0] = " |&│ &╝╚";
                    _area[3][0] = "&&──";
                    _area[4][0] = "&&──";
                }

                if (_direction == 6)
                {
                    _area[0][0] = "__&&";
                    _area[1][0] = "__&&";
                    _area[2][0] = "| & |&╗╔";
                    _area[3][0] = "&&──";
                    _area[4][0] = "&&──";
                }

                if (_direction == 7)
                {
                    _area[0][0] = " |&&";
                    _area[1][0] = "|&&";
                    _area[2][0] = "&├´&╝╚";
                    _area[3][0] = "&&──";
                    _area[4][0] = "&&──";
                }

                if (_direction == 8)
                {
                    _area[0][0] = " /&&";
                    _area[1][0] = "| &&";
                    _area[2][0] = "&--&|=";
                    _area[3][0] = "&&| ";
                    _area[4][0] = "&&| ";
                }

                if (_direction == 9)
                {
                    _area[0][0] = "--&&";
                    _area[1][0] = "¯¯&&";
                    _area[2][0] = "&┬-&|=";
                    _area[3][0] = "&&|";
                    _area[4][0] = "&&|";
                }

            } 


            return _area;
        }
    }
}

