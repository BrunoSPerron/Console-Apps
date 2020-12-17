using System;
using System.Collections.Generic;
using System.Text;

namespace TacticRoguelikeRpg
{
    class WorldData
    {
        public static List<GOD> GodList { get; set; } = new List<GOD>();
        public static void Initialize()
        {
            GOD Persay = new GOD();
            Persay.Name = "Persay, Mastery and perfection";
            Persay.Class0 = "Martial Artist";
            Persay.SetClass0Stats(8, 7, 9, 10, 8, 8);
            Persay.Class0Skills = new SKILL[] { SKILL.GrabAndThrow, SKILL.FlyingKick, SKILL.TornadoKick, SKILL.DoubleStrike };
            Persay.Class0Passives = new PASSIVES[] { PASSIVES.EyeOfTheTiger };
            Persay.Class1 = "Magi";
            Persay.SetClass1Stats(9, 10, 6, 9, 6, 10);
            Persay.Class1Skills = new SKILL[] { SKILL.ImbueWeapon, SKILL.UnleashEnergy, SKILL.DancingBlade };
            Persay.Class1Passives = new PASSIVES[] { PASSIVES.ClearMind };
            Persay.Class2 = "Sentinel";
            Persay.SetClass2Stats(8, 5, 12, 8, 8, 9);
            Persay.Class2Skills = new SKILL[] { SKILL.Protect, SKILL.Counter, SKILL.Cover, SKILL.PowerStrike };
            Persay.Class2Passives = new PASSIVES[] { PASSIVES.Counter, PASSIVES.Avenger };
            GodList.Add(Persay);

            GOD Grandfather = new GOD();
            Grandfather.Name = "Grandfather, Growth and Decay";
            Grandfather.Class0 = "Awakened Beast";
            Grandfather.SetClass0Stats(10, 9, 8, 6, 8, 12);
            Grandfather.Class0Skills = new SKILL[] { SKILL.Bite, SKILL.Roar, SKILL.Charge };
            Grandfather.Class1Passives = new PASSIVES[] { PASSIVES.ToughSkin };
            Grandfather.Class1 = "Hermit";
            Grandfather.SetClass1Stats(8, 9, 9, 6, 9, 6);
            Grandfather.Class1Skills = new SKILL[] { SKILL.Bite, SKILL.SecretRemedy, SKILL.ThrowRock };
            Grandfather.Class1Passives = new PASSIVES[] { PASSIVES.Indefatigable };
            Grandfather.Class2 = "Ent";
            Grandfather.SetClass2Stats(14, 2, 15, 4, 9, 4);
            Grandfather.Class2Skills = new SKILL[] { SKILL.Root, SKILL.Thorns, SKILL.PowerStrike };
            Grandfather.Class2Passives = new PASSIVES[] { PASSIVES.Photosynthesis };
            GodList.Add(Grandfather);

            GOD CatGod = new GOD();
            CatGod.Name = "The Cat God, Luck and Games";
            CatGod.Class0 = "Trickster";
            CatGod.SetClass0Stats(5, 10, 6, 9, 8, 12);
            CatGod.Class0Skills = new SKILL[] { SKILL.ThrowCoin, SKILL.PickACard, SKILL.Blink };
            CatGod.Class0Passives = new PASSIVES[] { PASSIVES.FateSmile };
            CatGod.Class1 = "Acrobat";
            CatGod.SetClass1Stats(7, 11, 7, 8, 6, 8);
            CatGod.Class1Skills = new SKILL[] { SKILL.FlyingKick, SKILL.Tumble, SKILL.DoubleStrike, SKILL.Dervish };
            CatGod.Class1Passives = new PASSIVES[] { PASSIVES.PerfectBalance };
            CatGod.Class2 = "Satyr";
            CatGod.SetClass1Stats(9, 8, 7, 9, 6, 13);
            CatGod.Class2Skills = new SKILL[] { SKILL.Charge, SKILL.Daze ,SKILL.Counter };
            CatGod.Class2Passives = new PASSIVES[] { PASSIVES.QuickStep };
            GodList.Add(CatGod);

            GOD Hynnec = new GOD();
            Hynnec.Name = "Hynnec, Craft and Invention";
            Hynnec.Class0 = "Doll Maker";
            Hynnec.SetClass0Stats(5, 8, 6, 11, 8, 12);
            Hynnec.Class0Skills = new SKILL[] { SKILL.AnimatePuppet, SKILL.Repair, SKILL.ExplodeMinion };
            CatGod.Class0Passives = new PASSIVES[] { PASSIVES.SoulBond };
            Hynnec.Class1 = "Inventor";
            Hynnec.SetClass1Stats(7, 10, 9, 8, 6, 10);
            Hynnec.Class1Skills = new SKILL[] { SKILL.CraftTurret, SKILL.Repair, SKILL.ThrowGrenade };
            CatGod.Class1Passives = new PASSIVES[] { PASSIVES.MechanicalExpertise };
            Hynnec.Class2 = "Blacksmith";
            Hynnec.SetClass1Stats(9, 7, 11, 8, 7, 8);
            Hynnec.Class2Skills = new SKILL[] { SKILL.ArmorBreak, SKILL.PowerStrike, SKILL.ThrowWeapon };
            CatGod.Class2Passives = new PASSIVES[] { PASSIVES.Maintenance };
            GodList.Add(Hynnec);
        }
    }

    struct GOD
    {
        public string Name;

        public string Class0;
        public int Class0Might;
        public int Class0Panache;
        public int Class0Grit;
        public int Class0Insight;
        public int Class0Wisdom;
        public int Class0Fortune;
        public SKILL[] Class0Skills;
        public PASSIVES[] Class0Passives;

        public string Class1;
        public int Class1Might;
        public int Class1Panache;
        public int Class1Grit;
        public int Class1Insight;
        public int Class1Wisdom;
        public int Class1Fortune;
        public SKILL[] Class1Skills;
        public PASSIVES[] Class1Passives;

        public string Class2;
        public int Class2Might;
        public int Class2Panache;
        public int Class2Grit;
        public int Class2Insight;
        public int Class2Wisdom;
        public int Class2Fortune;
        public SKILL[] Class2Skills;
        public PASSIVES[] Class2Passives;

        public void SetClass0Stats(int might, int panache, int grit, int insight, int wisdom, int fortune)
        {
            Class0Might = might;
            Class0Panache = panache;
            Class0Grit = grit;
            Class0Insight = insight;
            Class0Wisdom = wisdom;
            Class0Fortune = fortune;
        }
        public void SetClass1Stats(int might, int panache, int grit, int insight, int wisdom, int fortune)
        {
            Class1Might = might;
            Class1Panache = panache;
            Class1Grit = grit;
            Class1Insight = insight;
            Class1Wisdom = wisdom;
            Class1Fortune = fortune;
        }
        public void SetClass2Stats(int might, int panache, int grit, int insight, int wisdom, int fortune)
        {
            Class2Might = might;
            Class2Panache = panache;
            Class2Grit = grit;
            Class2Insight = insight;
            Class2Wisdom = wisdom;
            Class2Fortune = fortune;
        }
    }
}
