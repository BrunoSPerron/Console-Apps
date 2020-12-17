using System;
using System.Collections.Generic;
using System.Text;

namespace TacticRoguelikeRpg
{
    enum SKILL
    {
        GrabAndThrow = 1,
        FlyingKick = 2,
        ImbueWeapon = 3,
        UnleashEnergy = 4,
        Protect = 5,
        Counter = 6,
        Roar = 7,
        Charge = 8,
        Root = 9,
        Thorns = 10,
        ThrowRock = 11,
        SecretRemedy = 12,
        Bite = 13,
        ThrowCoin = 14,
        PickACard = 15,
        Repair = 16,
        AnimatePuppet = 17,
        CraftTurret = 18,
        ArmorBreak = 19,
        Blink = 20,
        Tumble = 21,
        Daze = 22,
        TornadoKick = 23,
        DancingBlade = 24,
        PowerStrike = 25,
        Cover = 26,
        DoubleStrike = 27,
        Dervish = 28,
        ThrowGrenade = 29,
        ExplodeMinion = 30,
        ThrowWeapon = 31
    }

    enum PASSIVES
    {
        ClearMind = 1,
        Avenger = 2,
        EyeOfTheTiger = 3,
        ToughSkin = 4,
        Indefatigable = 5,
        Photosynthesis = 6,
        FateSmile = 7,
        PerfectBalance = 8,
        Counter = 9,
        QuickStep = 10,
        SoulBond = 11,
        MechanicalExpertise = 12,
        Maintenance = 13
    }

    class Skills
    {
        public static int GetSkillCost(SKILL _skill)
        {
            int cost = 0;

            switch (_skill)
            {
                /*case Skill.GrabAndThrow:
                    break;
                case Skill.FlyingKick:
                    break;
                case Skill.ImbueWeapon:
                    break;
                case Skill.UnleashEnergy:
                    break;
                case Skill.Protect:
                    break;
                case Skill.Counter:
                    break;
                case Skill.Roar:
                    break;
                case Skill.Charge:
                    break;
                case Skill.Root:
                    break;
                case Skill.Thorns:
                    break;
                case Skill.ThrowRock:
                    break;
                case Skill.SecretRemedy:
                    break;
                case Skill.Bite:
                    break;
                case Skill.ThrowCoin:
                    break;
                case Skill.PickACard:
                    break;
                case Skill.Repair:
                    break;
                case Skill.AnimatePuppet:
                    break;
                case Skill.CraftTurret:
                    break;
                case Skill.ArmorBreak:
                    break;
                case Skill.Blink:
                    break;*/
                default:
                    cost = 500;
                    break;
            }

            return cost;
        }

        public static string GetSkillName(SKILL _skill)
        {
            string name = "";

            switch (_skill)
            {
                case SKILL.GrabAndThrow:
                    name = "Throw";
                    break;
                default:
                    foreach (char c in _skill.ToString())
                    {
                        if (char.IsUpper(c))
                            name += " ";
                        name += c;
                    }
                    name = name.Substring(1);
                    break;
            }

            return name;
        }
        public static string GetPassiveName(PASSIVES _p)
        {
            string name = "";

            switch (_p)
            {
                default:    //add space before each Uppercase
                    foreach (char c in _p.ToString())
                    {
                        if (char.IsUpper(c))
                            name += " ";
                        name += c;
                    }
                    name = name.Substring(1);
                    break;
            }

            return name;
        }

        public static int GetPassiveCost(PASSIVES _passive)
        {
            int cost = 0;

            switch (_passive)
            {
                default:    //add space before each Uppercase
                    cost = 500;
                    break;
            }
            return cost;
        }
    }


}
