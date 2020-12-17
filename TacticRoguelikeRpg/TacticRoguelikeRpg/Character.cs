using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TacticRoguelikeRpg
{
    class Character
    {
        public string name { get; set; }
        public string surname { get; set; }
        public int god { get; set; }
        public int essence { get; set; }

        string shortDesignation = "E-";
        string skillType;
        int currentHP;
        int currentMP;
        int maxHP;
        int maxMP;

        public int might { get; set; } = 8;
        public int panache { get; set; } = 8;
        public int grit { get; set; } = 8;
        public int insight { get; set; } = 8;
        public int wisdom { get; set; } = 8;
        public int fortune { get; set; } = 15;
        public int mobility { get; set; } = 5;

        bool controlledByPlayer = false;
        public bool summonned { get; set; } = false;
        public bool isMale { get; set; } = true;

        Item rightHand = new Weapon();

        int basicAttackType = 0;
        int modifiedBasicAttackType;


        int armor = 0;
        int fireResist = 10;
        int coldResist = 10;
        int shockResist = 10;


        public List<int> skillList { get; set; } = new List<int>();
        List<int> skillWeight = new List<int>();
        
        public List<PASSIVES> passivesList { get; set; } = new List<PASSIVES>();


        public void InitiateAsHero(int _class)
        {
            controlledByPlayer = true;
            modifiedBasicAttackType = basicAttackType;

            //test
            Weapon FLAIL = new Weapon();
            FLAIL.Initiate("Flail", 100, 10, ItemIcon.sword, 1, WeaponAnimType.Axe);
            //test

            rightHand = FLAIL;
            switch (_class)
            {

                case 1:
                    essence = 0;
                    maxHP = 20;
                    maxMP = 0;
                    might = 6;
                    panache = 10;
                    mobility = 3;
                    skillType = "Prowess";
                    break;
                case 2:
                    essence = 1;
                    maxHP = 18;
                    maxMP = 10;
                    might = 4;
                    panache = 12;
                    mobility = 6;
                    skillType = "Skills";
                    break;
                case 3:
                    essence = 2;
                    maxHP = 15;
                    maxMP = 20;
                    might = 2;
                    panache = 8;
                    mobility = 4;
                    skillType = "Spells";
                    Random rand = new Random();
                    AddSkill(rand.Next(1,3));
                    break;
                default:
                    essence = 0;
                    maxHP = 20;
                    maxMP = 10;
                    might = 5;
                    panache = 10;
                    mobility = 5;
                    skillType = "Tricks";
                    break;
            }
            currentHP = maxHP;
            currentMP = maxMP;

        }

        /// <param name="_creature">0 - Slime, 1 - Goblin</param>
        public void InitiateAsCreature (int _creature = 0)
        {

            skillType = "Skills";
            Random rand = new Random();
            if (rand.Next(0, 1) < 0.5)
                isMale = false;
            switch (_creature)
            {
                case 0:
                    SetName("Bunny");
                    maxHP = 5;
                    maxMP = 0;
                    might = 1;
                    panache = 22;
                    mobility = 1;
                    break;
                case 1:
                    SetName("Slime");
                    maxHP = 6;
                    maxMP = 2;
                    might = 2;
                    panache = 6;
                    mobility = 2;
                    break;
                case 2:
                    SetName("Goblin");
                    maxHP = 10;
                    maxMP = 5;
                    might = 4;
                    panache = 11;
                    mobility = 3;
                    break;
                case 3:
                    SetName("Slimy God");
                    maxHP = 12;
                    maxMP = 5;
                    might = 4;
                    panache = 11;
                    mobility = 3;
                    break;
                default:
                    SetName("[NOT FOUND]");
                    maxHP = 1;
                    maxMP = 1;
                    might = 1;
                    panache = 1;
                    mobility = 1;
                    break;
            }
            modifiedBasicAttackType = basicAttackType;
            currentHP = maxHP;
            currentMP = maxMP;
        }
        
        public void AddSkill(int _skill, int _weight = 100)
        {
            //Skills.GetSkill();

            if (_skill < 1 || _skill > 3)
            {
                Console.WriteLine("ERROR: Spell reference is out of index");
                Console.ReadKey();
                return;
            }
            skillList.Add(_skill);
            skillWeight.Add(_weight);
        }

        public void Damage(int _damage, int _type, int _damager, int _combatIndex, Combat parent, int _alliesLength)
        {
            float mitigatedDamage = -1;
            if (_type == 0)
            {
                if ((_damage - armor) > 0)
                {
                    mitigatedDamage = _damage - armor;
                }
            }
            else if (_type == 1)
            {
                mitigatedDamage = _damage * (100 - fireResist) / 100;
            }
            else if (_type == 2)
            {
                mitigatedDamage = _damage * (100 - coldResist) / 100;
            }
            else if (_type == 3)
            {
                mitigatedDamage = _damage * (100 - shockResist) / 100;
            }
            else
            {
                parent.ShowMessage("ERROR: Damage type out of range exception", true);
                mitigatedDamage = -1;
            }
            
            currentHP-= (int)mitigatedDamage;

            int _positionX = parent.positionX[_combatIndex];
            int _positionY = parent.positionY[_combatIndex];
            bool isDead;

            if (_combatIndex < _alliesLength)
                isDead = parent.DrawAllyStats(_combatIndex);
            else
                isDead = parent.DrawEnemyStats(_combatIndex - _alliesLength);
            
            parent.ShowMessage(parent.fighters[_damager].name + " deals " + mitigatedDamage + " damage to " + name + "!", true);

            if (isDead)
            {
                parent.ShowMessage(name + " dies!", true);
                parent.battlefield.UpdatePosition(_positionX, _positionY);
            }
                
        }

        public void AssignRandomName(string fixedName = null, string fixedSurname = null)
        {
            Random rand = new Random();

            if (fixedName != null)
            {
                name = fixedName;
            }
            else
            {
                string fileLocation = "";
                int lineCount;

                if (isMale)
                    fileLocation = "txtfiles\\Male Names.txt";
                else
                    fileLocation = "txtfiles\\Female Names.txt";

                lineCount = File.ReadLines(fileLocation).Count();
                name = File.ReadLines(fileLocation).Skip(rand.Next(0,lineCount)).Take(1).First();
            }

            if (fixedSurname != null)
            {
                surname = fixedSurname;
            }
            else
            {
                int lineCount = File.ReadLines(@"txtfiles\Surnames.txt").Count(); ;
                surname = File.ReadLines(@"txtfiles\Surnames.txt").Skip(rand.Next(0, lineCount)).Take(1).First();
            }
        }

        public void InitiateAsEssence(int newEssence = -1)
        {
            if (newEssence != -1)
                essence = newEssence;
            switch (essence)
            {
                case 0:
                    might = WorldData.GodList[god].Class0Might;
                    panache = WorldData.GodList[god].Class0Panache;
                    grit = WorldData.GodList[god].Class0Grit;
                    insight = WorldData.GodList[god].Class0Insight;
                    wisdom = WorldData.GodList[god].Class0Wisdom;
                    fortune = WorldData.GodList[god].Class0Fortune;
                    break;
                case 1:
                    might = WorldData.GodList[god].Class1Might;
                    panache = WorldData.GodList[god].Class1Panache;
                    grit = WorldData.GodList[god].Class1Grit;
                    insight = WorldData.GodList[god].Class1Insight;
                    wisdom = WorldData.GodList[god].Class1Wisdom;
                    fortune = WorldData.GodList[god].Class1Fortune;
                    break;
                case 2:
                    might = WorldData.GodList[god].Class2Might;
                    panache = WorldData.GodList[god].Class2Panache;
                    grit = WorldData.GodList[god].Class2Grit;
                    insight = WorldData.GodList[god].Class2Insight;
                    wisdom = WorldData.GodList[god].Class2Wisdom;
                    fortune = WorldData.GodList[god].Class2Fortune;
                    break;
                default:
                    Console.WriteLine("");
                    Console.WriteLine("ERROR - " + name + " ESSENCE INDEX IS OUT OF BOUND");
                    Console.ReadKey(true);
                    break;
            }
        }
        
        //--------------------------------------------------

        public int GetBasicAttackDamage()
        {
            Weapon _weapon = (Weapon)rightHand;
            return (might + _weapon.damage);
        }
        public int GetBasicAttackType()
        {
            return (modifiedBasicAttackType);
        }

        public int GetHP()
        {
            return currentHP;
        }

        public int GetMaxHP()
        {
            return maxHP;
        }

        public int GetMaxMP()
        {
            return maxMP;
        }

        public int getMovement()
        {
            return mobility;
        }

        public void setMovement(int i)
        {
            mobility = i;
        }

        public int GetMP()
        {
            return currentMP;
        }

        public void SetName(string _name)
        {
            name = _name;

            //and update shortDesignation
            shortDesignation = "";
            string[] wordsInName = name.Split(' ');
            foreach (string word in wordsInName)
            {
                if (shortDesignation.Length < 2 && word.Length > 2 && word.ToLower() != "the")
                {
                    shortDesignation += word[0];
                }
            }

            if (shortDesignation.Length < 2)
            {
                foreach (string word in wordsInName)
                {
                    if (shortDesignation.Length < 2 && word.Length > 1 && word.ToLower() != "the")
                    {
                        shortDesignation += word[1];
                    }
                }
            }

            while (shortDesignation.Length < 2)
                shortDesignation += "X";
        }

        public string GetName()
        {
            return name;
        }

        public bool GetPlayerControl()
        {
            return controlledByPlayer;
        }

        public void SetPlayerControl(bool _isControlledByPlayer = true)
        {
            controlledByPlayer = _isControlledByPlayer;
        }

        public string GetShortDesignation()
        {
            return shortDesignation;
        }

        public string GetSkillType()
        {
            return skillType;
        }
        //--------------------------------------------------

        public string[][] getWeaponAnim(int _direction)
        {
            return rightHand.GetAttackAreaForDirection(_direction);
        }

        public void setColorForAttack()
        {
            Console.ForegroundColor = rightHand.GetColor();
        }

        public string GetEssenceAsString()
        {
            switch (essence)
            {
                case 0:
                    return WorldData.GodList[god].Class0;
                case 1:
                    return WorldData.GodList[god].Class1;
                case 2:
                    return WorldData.GodList[god].Class2;
            }
            return "OUT OF BOUND ERROR";
        }

        public bool HaveSkill(SKILL _sk)
        {
            foreach (SKILL currentSkill in skillList)
                if (_sk == currentSkill)
                    return true;
            return false;
        }
        public bool HavePassive(PASSIVES _p)
        {
            foreach (PASSIVES currentPassive in passivesList)
                if (_p == currentPassive)
                    return true;
            return false;
        }
    }
}