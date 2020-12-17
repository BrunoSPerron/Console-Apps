using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace TacticRoguelikeRpg
{
    class CharacterCreation
    {
        enum PointerLocation
        {
            Name = 1,
            Surname = 2,
            God = 3,
            Essence = 4,
            Gift = 5,
            Might = 6,
            Panache = 7,
            Grit = 8,
            Insight = 9,
            Wisdom = 10,
            Fortune = 11,
            Skills = 12,
            Passives = 13,
            Gear = 14,
            Others = 15,
            Sex = 16,
        }

        Character chara = new Character();
        int creationPoints = 1500;

        int assignedMight = 0;
        int assignedPanache = 0;
        int assignedGrit = 0;
        int assignedInsight = 0;
        int assignedWisdom = 0;
        int assignedFortune = 0;

        bool namedWasChanged = false;

        public Character Start()
        {
            Random rand = new Random();
            if (rand.Next(0, 2) == 0)
                chara.isMale = false;

            chara.god = rand.Next(0, WorldData.GodList.Count);
            chara.InitiateAsEssence(rand.Next(0, 3));
            chara.AssignRandomName();
            DrawInterface();

            PointerLocation _position = PointerLocation.Name;
            bool hasFinished = false;
            while (!hasFinished)
            {
                switch (_position)
                {
                    case PointerLocation.Name:
                        _position = PointerAtPosition_Name();
                        break;
                    case PointerLocation.Surname:
                        _position = PointerAtPosition_Surname();
                        break;
                    case PointerLocation.Sex:
                        _position = PointerAtPosition_Sex();
                        break;
                    case PointerLocation.God:
                        _position = PointerAtPosition_God();
                        break;
                    case PointerLocation.Essence:
                        _position = PointerAtPosition_Essence();
                        break;
                    case PointerLocation.Gift:
                        _position = PointerAtPosition_Gift();
                        break;
                    case PointerLocation.Might:
                        _position = PointerAtPosition_Might();
                        break;
                    case PointerLocation.Panache:
                        _position = PointerAtPosition_Panache();
                        break;
                    case PointerLocation.Grit:
                        _position = PointerAtPosition_Grit();
                        break;
                    case PointerLocation.Insight:
                        _position = PointerAtPosition_Insight();
                        break;
                    case PointerLocation.Wisdom:
                        _position = PointerAtPosition_Wisdom();
                        break;
                    case PointerLocation.Fortune:
                        _position = PointerAtPosition_Fortune();
                        break;
                    case PointerLocation.Skills:
                        _position = pointerAtPosition_Skills();
                        break;
                    case PointerLocation.Passives:
                        _position = pointerAtPosition_Passives();
                        break;
                    case PointerLocation.Gear:
                        _position = pointerAtPosition_Gear();
                        break;
                    case PointerLocation.Others:
                        _position = pointerAtPosition_Others();
                        break;
                    default:
                        Console.Write("ERROR - UNASSIGNED POINTER POSITION");
                        break;
                }
            }
            return chara;
        }

        void DrawInterface()
        {
            Console.ForegroundColor = Settings.UILineColor;
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("╔══════════════════════════════════════════════╦═══════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                                              ║                                                                       ║");
            Console.WriteLine("║                                              ║                                                                       ║");
            Console.WriteLine("║                                              ║                                                                       ║");
            Console.WriteLine("║                                              ║                                                                       ║");
            Console.WriteLine("║                                              ║                                                                       ║");
            Console.WriteLine("║                                              ║                                                                       ║");
            Console.WriteLine("╚══╦═════════════════════════╦══╦══════════════╩══════════╦══╦═════════════════════════╦══╦═════════════════════════╦══╝");
            Console.WriteLine("   ║                         ║  ║                         ║  ║                         ║  ║                         ║   ");
            Console.WriteLine("   ║                         ║  ║                         ║  ║                         ║  ║                         ║   ");
            Console.WriteLine("   ║                         ║  ║                         ║  ║                         ║  ║                         ║   ");
            Console.WriteLine("   ╠─────────────────────────╣  ╠─────────────────────────╣  ╠─────────────────────────╣  ╠─────────────────────────╣   ");
            Console.WriteLine("   ║                         ║  ║                         ║  ║                         ║  ║                         ║   ");
            Console.WriteLine("   ║                         ║  ║                         ║  ║                         ║  ║                         ║   ");
            Console.WriteLine("   ║                         ║  ║                         ║  ║                         ║  ║                         ║   ");
            Console.WriteLine("   ║                         ║  ║                         ║  ║                         ║  ║                         ║   ");
            Console.WriteLine("   ║                         ║  ║                         ║  ║                         ║  ║                         ║   ");
            Console.WriteLine("   ║                         ║  ║                         ║  ║                         ║  ║                         ║   ");
            Console.WriteLine("   ║                         ║  ║                         ║  ║                         ║  ║                         ║   ");
            Console.WriteLine("   ║                         ║  ║                         ║  ║                         ║  ║                         ║   ");
            Console.WriteLine("   ║                         ║  ║                         ║  ║                         ║  ║                         ║   ");
            Console.WriteLine("   ║                         ║  ║                         ║  ║                         ║  ║                         ║   ");
            Console.WriteLine("   ║                         ║  ║                         ║  ║                         ║  ║                         ║   ");
            Console.WriteLine("   ║                         ║  ║                         ║  ║                         ║  ║                         ║   ");
            Console.WriteLine("   ║                         ║  ║                         ║  ║                         ║  ║                         ║   ");
            Console.WriteLine("   ║                         ║  ║                         ║  ║                         ║  ║                         ║   ");
            Console.WriteLine("   ║                         ║  ║                         ║  ║                         ║  ║                         ║   ");
            Console.WriteLine("   ║                         ║  ║                         ║  ║                         ║  ║                         ║   ");
            Console.WriteLine("   ║                         ║  ║                         ║  ║                         ║  ║                         ║   ");
            Console.WriteLine("   ║                         ║  ║                         ║  ║                         ║  ║                         ║   ");
            Console.WriteLine("   ║                         ║  ║                         ║  ║                         ║  ║                         ║   ");
            Console.WriteLine("   ║                         ║  ║                         ║  ║                         ║  ║                         ║   ");
            Console.WriteLine("   ║                         ║  ║                         ║  ║                         ║  ║                         ║   ");
            Console.WriteLine("   ║                         ║  ║                         ║  ║                         ║  ║                         ║   ");
            Console.WriteLine("   ║                         ║  ║                         ║  ║                         ║  ║                         ║   ");
            Console.WriteLine("╔══╩═════════════════════════╩══╩═════════════════════════╩══╩═════════════════════════╩══╩═════════════════════════╩══╗");
            Console.WriteLine("║                                                                                                                      ║");
            Console.WriteLine("║                                                                                                                      ║");
            Console.WriteLine("║                                                                                                                      ║");
            Console.Write("╚══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝");

            Console.ForegroundColor = Settings.DefaultForegroundColor;
            Console.SetCursorPosition(2, 1);
            Console.Write("Name");
            Console.SetCursorPosition(2, 2);
            Console.Write("Surname");
            Console.SetCursorPosition(2, 3);
            Console.Write("Sex");
            Console.SetCursorPosition(2, 4);
            Console.Write("God");
            Console.SetCursorPosition(2, 5);
            Console.Write("Essence");
            Console.SetCursorPosition(2, 6);
            Console.Write("Gift");

            Console.SetCursorPosition(11, 5);
            Console.Write(chara.GetEssenceAsString());
            Console.SetCursorPosition(11, 4);
            Console.Write(WorldData.GodList[chara.god].Name);
            Console.SetCursorPosition(11, 3);
            if (chara.isMale)
                Console.Write("Male");
            else
                Console.Write("Female");
            Console.SetCursorPosition(11, 1);
            Console.Write(chara.name);
            Console.SetCursorPosition(11, 2);
            Console.Write(chara.surname);

            Console.SetCursorPosition(49, 1);
            Console.Write("Might");
            Console.SetCursorPosition(49, 2);
            Console.Write("Panache");
            Console.SetCursorPosition(49, 3);
            Console.Write("Grit");
            Console.SetCursorPosition(49, 4);
            Console.Write("Insight");
            Console.SetCursorPosition(49, 5);
            Console.Write("Wisdom");
            Console.SetCursorPosition(49, 6);
            Console.Write("Fortune");

            Console.SetCursorPosition(13, 9);
            Console.Write("Skills");
            Console.SetCursorPosition(41, 9);
            Console.Write("Passives");
            Console.SetCursorPosition(72, 9);
            Console.Write("Gear");
            Console.SetCursorPosition(100, 9);
            Console.Write("Others");

            Console.SetCursorPosition(77, 2);
            Console.Write("HP");
            Console.SetCursorPosition(77, 3);
            Console.Write("MP");
            Console.SetCursorPosition(77, 6);
            Console.Write("Move");

            Console.SetCursorPosition(103, 1);
            Console.Write("Creation Points");

            UpdateAllStatsVisual();
            UpdateCreationPointsVisual();
        }



        PointerLocation PointerAtPosition_Name()
        {
            PointerLocation _nextLocation = PointerLocation.Name;
            UpdateNameVisual(true);

            ShowMessage("", "", "                                                                                              r:Randomize name");

            bool changePosition = false;
            while (!changePosition)
            {
                Console.BackgroundColor = Settings.DefaultSelectionColor;
                ConsoleKeyInfo input = Helper.GetUserInput(new ConsoleKey[] { ConsoleKey.RightArrow, ConsoleKey.NumPad6, ConsoleKey.DownArrow, ConsoleKey.NumPad2, ConsoleKey.R }, true);

                switch (input.Key)
                {
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.NumPad2:
                        _nextLocation = PointerLocation.Surname;
                        changePosition = true;
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.NumPad6:
                        _nextLocation = PointerLocation.Might;
                        changePosition = true;
                        break;
                    case ConsoleKey.R:
                        chara.AssignRandomName(null, chara.surname);
                        UpdateNameVisual(true);
                        namedWasChanged = false;
                        break;
                    case ConsoleKey.Spacebar:
                    case ConsoleKey.NumPad5:
                    case ConsoleKey.Enter:
                        Console.SetCursorPosition(11, 1);
                        string _newLine = Helper.ReadAtPosition(11, 1, 35);
                        if (_newLine != "")
                        {
                            chara.name = _newLine;
                            namedWasChanged = true;
                        }
                        Console.Write("                                   ");
                        Console.SetCursorPosition(11, 1);
                        Console.Write(chara.GetName());
                        break;
                }
            }
            Console.BackgroundColor = Settings.DefaultBackgroundColor;
            UpdateNameVisual();
            return _nextLocation;
        }

        PointerLocation PointerAtPosition_Surname()
        {
            PointerLocation _nextLocation = PointerLocation.Name;

            ShowMessage("", "", "                                                                                                 r:Randomize surname");

            Console.SetCursorPosition(11, 2);
            Console.BackgroundColor = Settings.DefaultSelectionColor;
            Console.Write("                                   ");
            Console.SetCursorPosition(11, 2);
            Console.Write(chara.surname);

            bool changePosition = false;
            while (!changePosition)
            {
                Console.BackgroundColor = Settings.DefaultSelectionColor;
                ConsoleKeyInfo input = Helper.GetUserInput(new ConsoleKey[] { ConsoleKey.UpArrow, ConsoleKey.NumPad8, ConsoleKey.RightArrow, ConsoleKey.NumPad6, ConsoleKey.DownArrow, ConsoleKey.NumPad2, ConsoleKey.R }, true);

                switch (input.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.NumPad8:
                        _nextLocation = PointerLocation.Name;
                        changePosition = true;
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.NumPad2:
                        _nextLocation = PointerLocation.Sex;
                        changePosition = true;
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.NumPad6:
                        _nextLocation = PointerLocation.Panache;
                        changePosition = true;
                        break;
                    case ConsoleKey.R:
                        chara.AssignRandomName(chara.name);
                        UpdateSurnameVisual(true);
                        break;
                    case ConsoleKey.Spacebar:
                    case ConsoleKey.NumPad5:
                    case ConsoleKey.Enter:
                        Console.SetCursorPosition(11, 2);
                        string _newLine = Helper.ReadAtPosition(11, 2, 35);
                        if (_newLine != "")
                            chara.surname = _newLine;
                        Console.Write("                                   ");
                        Console.SetCursorPosition(11, 2);
                        Console.Write(chara.surname);
                        break;
                }
            }

            Console.BackgroundColor = Settings.DefaultBackgroundColor;
            Console.SetCursorPosition(11, 2);
            Console.Write("                                   ");
            Console.SetCursorPosition(11, 2);
            Console.Write(chara.surname);
            return _nextLocation;
        }

        PointerLocation PointerAtPosition_Sex()
        {
            PointerLocation _nextLocation = PointerLocation.Sex;
            Console.BackgroundColor = Settings.DefaultSelectionColor;
            Console.SetCursorPosition(11, 3);
            if (chara.isMale)
                Console.Write("Male                               ");
            else
                Console.Write("Female                             ");

            ShowMessage();

            bool changePosition = false;
            while (!changePosition)
            {
                Console.BackgroundColor = Settings.DefaultSelectionColor;
                ConsoleKeyInfo input = Helper.GetUserInput(new ConsoleKey[] { ConsoleKey.UpArrow, ConsoleKey.NumPad8, ConsoleKey.RightArrow, ConsoleKey.NumPad6, ConsoleKey.DownArrow, ConsoleKey.NumPad2 }, true);

                switch (input.Key)
                {
                    case ConsoleKey.NumPad8:
                    case ConsoleKey.UpArrow:
                        _nextLocation = PointerLocation.Surname;
                        changePosition = true;
                        break;
                    case ConsoleKey.NumPad2:
                    case ConsoleKey.DownArrow:
                        _nextLocation = PointerLocation.God;
                        changePosition = true;
                        break;
                    case ConsoleKey.NumPad6:
                    case ConsoleKey.RightArrow:
                        _nextLocation = PointerLocation.Grit;
                        changePosition = true;
                        break;
                    case ConsoleKey.Spacebar:
                    case ConsoleKey.NumPad5:
                    case ConsoleKey.Enter:
                        chara.isMale = !chara.isMale;
                        Console.SetCursorPosition(11, 3);
                        if (chara.isMale)
                            Console.Write("Male                               ");
                        else
                            Console.Write("Female                             ");
                        if (!namedWasChanged)
                        {
                            chara.AssignRandomName(null, chara.surname);
                            UpdateNameVisual();
                        }
                        break;
                }
            }
            Console.SetCursorPosition(11, 3);
            Console.BackgroundColor = Settings.DefaultBackgroundColor;
            if (chara.isMale)
                Console.Write("Male                               ");
            else
                Console.Write("Female                             ");
            return _nextLocation;
        }

        PointerLocation PointerAtPosition_God()
        {
            PointerLocation _nextLocation = PointerLocation.God;
            Console.BackgroundColor = Settings.DefaultSelectionColor;
            UpdateGodVisual();

            ShowMessage();

            bool changePosition = false;
            while (!changePosition)
            {
                Console.BackgroundColor = Settings.DefaultSelectionColor;
                ConsoleKeyInfo input = Helper.GetUserInput(new ConsoleKey[] { ConsoleKey.UpArrow, ConsoleKey.NumPad8, ConsoleKey.RightArrow, ConsoleKey.NumPad6, ConsoleKey.DownArrow, ConsoleKey.NumPad2 }, true);

                switch (input.Key)
                {
                    case ConsoleKey.NumPad8:
                    case ConsoleKey.UpArrow:
                        _nextLocation = PointerLocation.Sex;
                        changePosition = true;
                        break;
                    case ConsoleKey.NumPad2:
                    case ConsoleKey.DownArrow:
                        _nextLocation = PointerLocation.Essence;
                        changePosition = true;
                        break;
                    case ConsoleKey.NumPad6:
                    case ConsoleKey.RightArrow:
                        _nextLocation = PointerLocation.Insight;
                        changePosition = true;
                        break;
                    case ConsoleKey.Spacebar:
                    case ConsoleKey.NumPad5:
                    case ConsoleKey.Enter:
                        string[] _godList = new string[WorldData.GodList.Count];

                        for (int i = 0; i < WorldData.GodList.Count; i++)
                            _godList[i] = WorldData.GodList[i].Name;

                        int choosenGod = Helper.SingleLineSelection(_godList, 11, 4, 35, chara.god);

                        if (choosenGod != -1)
                        {
                            chara.god = choosenGod;
                            chara.InitiateAsEssence();

                            chara.might += assignedMight;
                            chara.panache += assignedPanache;
                            chara.grit += assignedGrit;
                            chara.insight += assignedInsight;
                            chara.wisdom += assignedWisdom;
                            chara.fortune += assignedFortune;
                        }

                        UpdateGodVisual();
                        RefundAllSkill();

                        break;
                }
            }
            Console.BackgroundColor = Settings.DefaultBackgroundColor;
            UpdateGodVisual();
            return _nextLocation;
        }

        PointerLocation PointerAtPosition_Essence()
        {
            PointerLocation _nextLocation = PointerLocation.Essence;
            Console.BackgroundColor = Settings.DefaultSelectionColor;
            UpdateEssenceVisual();

            ShowMessage();

            bool changePosition = false;
            while (!changePosition)
            {
                Console.BackgroundColor = Settings.DefaultSelectionColor;
                ConsoleKeyInfo input = Helper.GetUserInput(new ConsoleKey[] { ConsoleKey.UpArrow, ConsoleKey.NumPad8, ConsoleKey.RightArrow, ConsoleKey.NumPad6, ConsoleKey.DownArrow, ConsoleKey.NumPad2 }, true);

                switch (input.Key)
                {
                    case ConsoleKey.NumPad8:
                    case ConsoleKey.UpArrow:
                        _nextLocation = PointerLocation.God;
                        changePosition = true;
                        break;
                    case ConsoleKey.NumPad2:
                    case ConsoleKey.DownArrow:
                        _nextLocation = PointerLocation.Gift;
                        changePosition = true;
                        break;
                    case ConsoleKey.NumPad6:
                    case ConsoleKey.RightArrow:
                        _nextLocation = PointerLocation.Wisdom;
                        changePosition = true;
                        break;
                    case ConsoleKey.Spacebar:
                    case ConsoleKey.NumPad5:
                    case ConsoleKey.Enter:
                        string[] essencesList = new string[3];
                        essencesList[0] = WorldData.GodList[chara.god].Class0;
                        essencesList[1] = WorldData.GodList[chara.god].Class1;
                        essencesList[2] = WorldData.GodList[chara.god].Class2;
                        int choosenEssence = Helper.SingleLineSelection(essencesList, 11, 5, 35, chara.essence);
                        if (choosenEssence != -1)
                        {
                            chara.essence = choosenEssence;
                            chara.InitiateAsEssence(choosenEssence);

                            chara.might += assignedMight;
                            chara.panache += assignedPanache;
                            chara.grit += assignedGrit;
                            chara.insight += assignedInsight;
                            chara.wisdom += assignedWisdom;
                            chara.fortune += assignedFortune;
                        }

                        UpdateEssenceVisual();
                        UpdateAllStatsVisual();
                        RefundAllSkill();
                        break;
                }
            }


            Console.BackgroundColor = Settings.DefaultBackgroundColor;
            UpdateEssenceVisual();
            UpdateAllStatsVisual();
            return _nextLocation;
        }

        PointerLocation PointerAtPosition_Gift()
        {
            string gift = "UNIMPLEMENTED";
            PointerLocation _nextLocation = PointerLocation.Gift;
            Console.BackgroundColor = Settings.DefaultSelectionColor;
            Console.SetCursorPosition(11, 6);
            Console.Write("                                   ");
            Console.SetCursorPosition(11, 6);
            Console.Write(gift);

            ShowMessage();

            bool changePosition = false;
            while (!changePosition)
            {
                ConsoleKeyInfo input = Helper.GetUserInput(new ConsoleKey[] { ConsoleKey.UpArrow, ConsoleKey.NumPad8, ConsoleKey.RightArrow, ConsoleKey.NumPad6, ConsoleKey.DownArrow, ConsoleKey.NumPad2 }, true);

                switch (input.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.NumPad8:
                        _nextLocation = PointerLocation.Essence;
                        changePosition = true;
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.NumPad2:
                        _nextLocation = PointerLocation.Skills;
                        changePosition = true;
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.NumPad6:
                        _nextLocation = PointerLocation.Fortune;
                        changePosition = true;
                        break;
                    case ConsoleKey.Spacebar:
                    case ConsoleKey.NumPad5:
                    case ConsoleKey.Enter:
                        changePosition = true;
                        break;
                }
            }
            Console.BackgroundColor = Settings.DefaultBackgroundColor;
            Console.SetCursorPosition(11, 6);
            Console.Write("                                   ");
            Console.SetCursorPosition(11, 6);
            Console.Write(gift);
            return _nextLocation;
        }



        PointerLocation PointerAtPosition_Might()
        {
            PointerLocation _nextLocation = PointerLocation.Might;
            UpdateStatVisual("Might", chara.might, 1, assignedMight, true);

            ShowMessage("Might represent the raw physical strength. Its the ability to bash heads and crush bones.");

            bool changePosition = false;
            while (!changePosition)
            {
                ConsoleKeyInfo input = Helper.GetUserInput(new ConsoleKey[] { ConsoleKey.LeftArrow, ConsoleKey.NumPad4, ConsoleKey.DownArrow, ConsoleKey.NumPad2, ConsoleKey.OemPlus, ConsoleKey.Add, ConsoleKey.OemMinus, ConsoleKey.Subtract });

                switch (input.Key)
                {
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.NumPad4:
                        _nextLocation = PointerLocation.Name;
                        changePosition = true;
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.NumPad2:
                        _nextLocation = PointerLocation.Panache;
                        changePosition = true;
                        break;
                    case ConsoleKey.OemPlus:
                    case ConsoleKey.Add:
                        if ((assignedMight + 1) * 50 <= creationPoints)
                        {
                            creationPoints -= ((assignedMight + 1) * 50);
                            chara.might++;
                            assignedMight++;
                            UpdateStatVisual("Might", chara.might, 1, assignedMight, true);
                            UpdateCreationPointsVisual();
                        }

                        break;
                    case ConsoleKey.OemMinus:
                    case ConsoleKey.Subtract:
                        if (assignedMight > 0)
                        {
                            creationPoints += assignedMight * 50;
                            chara.might--;
                            assignedMight--;
                            UpdateStatVisual("Might", chara.might, 1, assignedMight, true);
                            UpdateCreationPointsVisual();
                        }
                        break;
                }
            }
            UpdateStatVisual("Might", chara.might, 1, assignedMight);
            return _nextLocation;
        }

        PointerLocation PointerAtPosition_Panache()
        {
            PointerLocation _nextLocation = PointerLocation.Panache;
            UpdateStatVisual("Panache", chara.panache, 2, assignedPanache, true);

            ShowMessage("Panache represent self-confidence and flambloyance. It's the ability to impose your presence on the battlefield, the", "courage, and will, to seize the most dangerous opportunities.");

            bool changePosition = false;
            while (!changePosition)
            {
                ConsoleKeyInfo input = Helper.GetUserInput(new ConsoleKey[]
                {
                    ConsoleKey.UpArrow,
                    ConsoleKey.NumPad8,
                    ConsoleKey.LeftArrow,
                    ConsoleKey.NumPad4,
                    ConsoleKey.DownArrow,
                    ConsoleKey.NumPad2,
                    ConsoleKey.OemPlus,
                    ConsoleKey.Add,
                    ConsoleKey.OemMinus,
                    ConsoleKey.Subtract
                });

                switch (input.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.NumPad8:
                        _nextLocation = PointerLocation.Might;
                        changePosition = true;
                        break;
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.NumPad4:
                        _nextLocation = PointerLocation.Surname;
                        changePosition = true;
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.NumPad2:
                        _nextLocation = PointerLocation.Grit;
                        changePosition = true;
                        break;
                    case ConsoleKey.OemPlus:
                    case ConsoleKey.Add:
                        if ((assignedPanache + 1) * 50 <= creationPoints)
                        {
                            creationPoints -= ((assignedPanache + 1) * 50);
                            chara.panache++;
                            assignedPanache++;
                            UpdateStatVisual("Panache", chara.panache, 2, assignedPanache, true);
                            UpdateCreationPointsVisual();
                        }

                        break;
                    case ConsoleKey.OemMinus:
                    case ConsoleKey.Subtract:
                        if (assignedPanache > 0)
                        {
                            creationPoints += assignedPanache * 50;
                            chara.panache--;
                            assignedPanache--;
                            UpdateStatVisual("Panache", chara.panache, 2, assignedPanache, true);
                            UpdateCreationPointsVisual();
                        }
                        break;
                }
            }
            UpdateStatVisual("Panache", chara.panache, 2, assignedPanache);
            return _nextLocation;
        }

        PointerLocation PointerAtPosition_Grit()
        {
            PointerLocation _nextLocation = PointerLocation.Grit;
            UpdateStatVisual("Grit", chara.grit, 3, assignedGrit, true);

            ShowMessage("Grit represent resolve and strength of character. It's the ability to thriumph in the face of adversity, the", "tenacity to keep fighting when its impossible to do so!");

            bool changePosition = false;
            while (!changePosition)
            {
                ConsoleKeyInfo input = Helper.GetUserInput(new ConsoleKey[]
                {
                    ConsoleKey.UpArrow,
                    ConsoleKey.NumPad8,
                    ConsoleKey.LeftArrow,
                    ConsoleKey.NumPad4,
                    ConsoleKey.DownArrow,
                    ConsoleKey.NumPad2,
                    ConsoleKey.OemPlus,
                    ConsoleKey.Add,
                    ConsoleKey.OemMinus,
                    ConsoleKey.Subtract
                });

                switch (input.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.NumPad8:
                        _nextLocation = PointerLocation.Panache;
                        changePosition = true;
                        break;
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.NumPad4:
                        _nextLocation = PointerLocation.Sex;
                        changePosition = true;
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.NumPad2:
                        _nextLocation = PointerLocation.Insight;
                        changePosition = true;
                        break;
                    case ConsoleKey.OemPlus:
                    case ConsoleKey.Add:
                        if ((assignedGrit + 1) * 50 <= creationPoints)
                        {
                            creationPoints -= ((assignedGrit + 1) * 50);
                            chara.grit++;
                            assignedGrit++;
                            UpdateStatVisual("Grit", chara.grit, 3, assignedGrit, true);
                            UpdateCreationPointsVisual();
                        }

                        break;
                    case ConsoleKey.OemMinus:
                    case ConsoleKey.Subtract:
                        if (assignedGrit > 0)
                        {
                            creationPoints += assignedGrit * 50;
                            chara.grit--;
                            assignedGrit--;
                            UpdateStatVisual("Grit", chara.grit, 3, assignedGrit, true);
                            UpdateCreationPointsVisual();
                        }
                        break;
                }
            }
            UpdateStatVisual("Grit", chara.grit, 3, assignedGrit);
            return _nextLocation;
        }

        PointerLocation PointerAtPosition_Insight()
        {
            PointerLocation _nextLocation = PointerLocation.Insight;
            UpdateStatVisual("Insight", chara.insight, 4, assignedInsight, true);

            ShowMessage("Insight represent the mastery of your art. It's the ability to execute your skills with precision and the experience", "to react adequatly in any situation.");

            bool changePosition = false;
            while (!changePosition)
            {
                ConsoleKeyInfo input = Helper.GetUserInput(new ConsoleKey[]
                {
                    ConsoleKey.UpArrow,
                    ConsoleKey.NumPad8,
                    ConsoleKey.LeftArrow,
                    ConsoleKey.NumPad4,
                    ConsoleKey.DownArrow,
                    ConsoleKey.NumPad2,
                    ConsoleKey.OemPlus,
                    ConsoleKey.Add,
                    ConsoleKey.OemMinus,
                    ConsoleKey.Subtract
                });

                switch (input.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.NumPad8:
                        _nextLocation = PointerLocation.Grit;
                        changePosition = true;
                        break;
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.NumPad4:
                        _nextLocation = PointerLocation.God;
                        changePosition = true;
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.NumPad2:
                        _nextLocation = PointerLocation.Wisdom;
                        changePosition = true;
                        break;
                    case ConsoleKey.OemPlus:
                    case ConsoleKey.Add:
                        if ((assignedInsight + 1) * 50 <= creationPoints)
                        {
                            creationPoints -= ((assignedInsight + 1) * 50);
                            chara.insight++;
                            assignedInsight++;
                            UpdateStatVisual("Insight", chara.insight, 4, assignedInsight, true);
                            UpdateCreationPointsVisual();
                        }

                        break;
                    case ConsoleKey.OemMinus:
                    case ConsoleKey.Subtract:
                        if (assignedInsight > 0)
                        {
                            creationPoints += assignedInsight * 50;
                            chara.insight--;
                            assignedInsight--;
                            UpdateStatVisual("Insight", chara.insight, 4, assignedInsight, true);
                            UpdateCreationPointsVisual();
                        }
                        break;
                }
            }

            UpdateStatVisual("Insight", chara.insight, 4, assignedInsight);
            return _nextLocation;
        }

        PointerLocation PointerAtPosition_Wisdom()
        {
            PointerLocation _nextLocation = PointerLocation.Wisdom;
            UpdateStatVisual("Wisdom", chara.wisdom, 5, assignedWisdom, true);

            ShowMessage("Wisdom represent knowledge and self-awareness. It's the ability to manage your ressources, the", "judgement needed to precisely pace yourself in combat.");

            bool changePosition = false;
            while (!changePosition)
            {
                ConsoleKeyInfo input = Helper.GetUserInput(new ConsoleKey[]
                {
                    ConsoleKey.UpArrow,
                    ConsoleKey.NumPad8,
                    ConsoleKey.LeftArrow,
                    ConsoleKey.NumPad4,
                    ConsoleKey.DownArrow,
                    ConsoleKey.NumPad2,
                    ConsoleKey.OemPlus,
                    ConsoleKey.Add,
                    ConsoleKey.OemMinus,
                    ConsoleKey.Subtract
                });

                switch (input.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.NumPad8:
                        _nextLocation = PointerLocation.Insight;
                        changePosition = true;
                        break;
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.NumPad4:
                        _nextLocation = PointerLocation.Essence;
                        changePosition = true;
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.NumPad2:
                        _nextLocation = PointerLocation.Fortune;
                        changePosition = true;
                        break;
                    case ConsoleKey.OemPlus:
                    case ConsoleKey.Add:
                        if ((assignedWisdom + 1) * 50 <= creationPoints)
                        {
                            creationPoints -= ((assignedWisdom + 1) * 50);
                            chara.wisdom++;
                            assignedWisdom++;
                            UpdateStatVisual("Wisdom", chara.wisdom, 5, assignedWisdom, true);
                            UpdateCreationPointsVisual();
                        }

                        break;
                    case ConsoleKey.OemMinus:
                    case ConsoleKey.Subtract:
                        if (assignedWisdom > 0)
                        {
                            creationPoints += assignedWisdom * 50;
                            chara.wisdom--;
                            assignedWisdom--;
                            UpdateStatVisual("Wisdom", chara.wisdom, 5, assignedWisdom, true);
                            UpdateCreationPointsVisual();
                        }
                        break;
                }
            }
            UpdateStatVisual("Wisdom", chara.wisdom, 5, assignedWisdom);
            return _nextLocation;
        }

        PointerLocation PointerAtPosition_Fortune()
        {
            PointerLocation _nextLocation = PointerLocation.Fortune;
            UpdateStatVisual("Fortune", chara.fortune, 6, assignedFortune, true);

            ShowMessage("Fortune represent sheer luck. It's the ability to do nothing, and be rewarded for it.", "                                                           =^_^=");

            bool changePosition = false;
            while (!changePosition)
            {
                ConsoleKeyInfo input = Helper.GetUserInput(new ConsoleKey[]
                {
                    ConsoleKey.UpArrow,
                    ConsoleKey.NumPad8,
                    ConsoleKey.LeftArrow,
                    ConsoleKey.NumPad4,
                    ConsoleKey.DownArrow,
                    ConsoleKey.NumPad2,
                    ConsoleKey.OemPlus,
                    ConsoleKey.Add,
                    ConsoleKey.OemMinus,
                    ConsoleKey.Subtract
                });

                switch (input.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.NumPad8:
                        _nextLocation = PointerLocation.Wisdom;
                        changePosition = true;
                        break;
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.NumPad4:
                        _nextLocation = PointerLocation.Gift;
                        changePosition = true;
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.NumPad2:
                        _nextLocation = PointerLocation.Passives;
                        changePosition = true;
                        break;
                    case ConsoleKey.OemPlus:
                    case ConsoleKey.Add:
                        if ((assignedFortune + 1) * 50 <= creationPoints)
                        {
                            creationPoints -= ((assignedFortune + 1) * 50);
                            chara.fortune++;
                            assignedFortune++;
                            UpdateStatVisual("Fortune", chara.fortune, 6, assignedFortune, true);
                            UpdateCreationPointsVisual();
                        }

                        break;
                    case ConsoleKey.OemMinus:
                    case ConsoleKey.Subtract:
                        if (assignedFortune > 0)
                        {
                            creationPoints += assignedFortune * 50;
                            chara.fortune--;
                            assignedFortune--;
                            UpdateStatVisual("Fortune", chara.fortune, 6, assignedFortune, true);
                            UpdateCreationPointsVisual();
                        }
                        break;
                }
            }
            UpdateStatVisual("Fortune", chara.fortune, 6, assignedFortune);
            return _nextLocation;
        }



        PointerLocation pointerAtPosition_Skills()
        {
            PointerLocation _nextLocation = PointerLocation.Skills;
            Console.BackgroundColor = Settings.DefaultSelectionColor;
            Console.SetCursorPosition(4, 8);
            Console.Write("                         ");
            Console.SetCursorPosition(4, 9);
            Console.Write("         Skills          ");
            Console.SetCursorPosition(4, 10);
            Console.Write("                         ");

            bool changePosition = false;
            while (!changePosition)
            {
                ConsoleKeyInfo input = Helper.GetUserInput(new ConsoleKey[] { ConsoleKey.UpArrow, ConsoleKey.NumPad8, ConsoleKey.RightArrow, ConsoleKey.NumPad6 }, true);

                switch (input.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.NumPad8:
                        _nextLocation = PointerLocation.Gift;
                        changePosition = true;
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.NumPad6:
                        _nextLocation = PointerLocation.Passives;
                        changePosition = true;
                        break;
                    case ConsoleKey.Spacebar:
                    case ConsoleKey.NumPad5:
                    case ConsoleKey.Enter:
                        string[] options = new string[chara.skillList.Count + 1];
                        for (int i = 0; i < chara.skillList.Count; i++)
                        {
                            options[i] = " ";
                            options[i] += Skills.GetSkillName((SKILL)chara.skillList[i]);
                            options[i] += " ";
                        }
                        options[chara.skillList.Count] = "--------Add Skill--------";
                        int choice = Helper.ShowMenuAndGetChoice(options, 4, 12, 25);

                        if (choice != -1)
                        {
                            if (choice <= chara.skillList.Count)
                            {
                                creationPoints += Skills.GetSkillCost((SKILL)chara.skillList[choice - 1]);
                                chara.skillList.RemoveAt(choice - 1);
                                UpdateCreationPointsVisual();
                            }
                            else
                            {
                                ChooseNewSkill();
                            }
                        }
                        DrawSkills();
                        break;
                }
            }

            Console.BackgroundColor = Settings.DefaultBackgroundColor;
            Console.SetCursorPosition(4, 8);
            Console.Write("                         ");
            Console.SetCursorPosition(4, 9);
            Console.Write("         Skills          ");
            Console.SetCursorPosition(4, 10);
            Console.Write("                         ");
            return _nextLocation;
        }

        void ChooseNewSkill()
        {
            //Get the correct skilllist
            List<SKILL> availableSkills = new List<SKILL>();
            switch (chara.essence)
            {
                case 0:
                    foreach (SKILL sk in WorldData.GodList[chara.god].Class0Skills)
                        if (!chara.HaveSkill(sk))
                            availableSkills.Add(sk);
                    break;
                case 1:
                    foreach (SKILL sk in WorldData.GodList[chara.god].Class1Skills)
                        if (!chara.HaveSkill(sk))
                            availableSkills.Add(sk);
                    break;
                case 2:
                    foreach (SKILL sk in WorldData.GodList[chara.god].Class2Skills)
                        if (!chara.HaveSkill(sk))
                            availableSkills.Add(sk);
                    break;
            }


            string[] options = new string[availableSkills.Count];
            bool[] disabledOptions = new bool[options.Length];

            //populate and format the choices
            for (int i = 0; i < options.Length; i++)
            {
                double nbOfDigitInCost = Math.Floor(Math.Log10(Skills.GetSkillCost(availableSkills[i])) + 1);

                options[i] = " ";
                options[i] += Skills.GetSkillName(availableSkills[i]);
                while (options[i].Length < 22 - nbOfDigitInCost)
                    options[i] += " ";
                options[i] = options[i].Substring(0, 22 - (int)nbOfDigitInCost);
                options[i] += "[" + Skills.GetSkillCost(availableSkills[i]) + "]";

                if (creationPoints < Skills.GetSkillCost(availableSkills[i]))
                    disabledOptions[i] = true;
            }
            
            int choice = Helper.ExtendedBoxSelection(options, 4, 12 + chara.skillList.Count, 25, 23 - chara.skillList.Count, disabledOptions);
            if (choice != -1)
            {
                if (Skills.GetSkillCost(availableSkills[choice - 1]) <= creationPoints)
                {
                    chara.skillList.Add((int)availableSkills[choice - 1]);
                    creationPoints -= Skills.GetSkillCost(availableSkills[choice - 1]);
                    UpdateCreationPointsVisual();
                }
            }
            DrawSkills();
        }

        void RefundAllSkill()
        {
            while (chara.skillList.Count > 0)
            {
                creationPoints += Skills.GetSkillCost((SKILL)chara.skillList[0]);
                chara.skillList.RemoveAt(0);
            }
            DrawSkills();
            UpdateCreationPointsVisual();
        }

        void DrawSkills()
        {
            Console.BackgroundColor = Settings.DefaultBackgroundColor;
            Console.ForegroundColor = Settings.DefaultForegroundColor;
            for (int i = 0; i < 23; i++)
            {
                Console.SetCursorPosition(4, 12 + i);
                Console.Write("                         ");
            }

            for (int i = 0; i < chara.skillList.Count; i++)
            {
                Console.SetCursorPosition(5, 12 + i);
                Console.Write(Skills.GetSkillName((SKILL)chara.skillList[i]));
            }
        }

        PointerLocation pointerAtPosition_Passives()
        {
            PointerLocation _nextLocation = PointerLocation.Passives;
            Console.BackgroundColor = Settings.DefaultSelectionColor;
            Console.SetCursorPosition(33, 8);
            Console.Write("                         ");
            Console.SetCursorPosition(33, 9);
            Console.Write("        Passives         ");
            Console.SetCursorPosition(33, 10);
            Console.Write("                         ");

            bool changePosition = false;
            while (!changePosition)
            {
                ConsoleKeyInfo input = Helper.GetUserInput(new ConsoleKey[] { ConsoleKey.UpArrow, ConsoleKey.NumPad8, ConsoleKey.LeftArrow, ConsoleKey.NumPad4, ConsoleKey.RightArrow, ConsoleKey.NumPad6 }, true);

                switch (input.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.NumPad8:
                        _nextLocation = PointerLocation.Fortune;
                        changePosition = true;
                        break;
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.NumPad4:
                        _nextLocation = PointerLocation.Skills;
                        changePosition = true;
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.NumPad6:
                        _nextLocation = PointerLocation.Gear;
                        changePosition = true;
                        break;
                    case ConsoleKey.Spacebar:
                    case ConsoleKey.NumPad5:
                    case ConsoleKey.Enter:
                        changePosition = true;
                        break;
                }
            }

            Console.BackgroundColor = Settings.DefaultBackgroundColor;
            Console.SetCursorPosition(33, 8);
            Console.Write("                         ");
            Console.SetCursorPosition(33, 9);
            Console.Write("        Passives         ");
            Console.SetCursorPosition(33, 10);
            Console.Write("                         ");
            return _nextLocation;
        }

        void ChooseNewPassive()
        {
            //Get the correct skilllist
            List<PASSIVES> availablePassives = new List<PASSIVES>();
            switch (chara.essence)
            {
                case 0:
                    foreach (PASSIVES p in WorldData.GodList[chara.god].Class0Passives)
                        if (!chara.HavePassive(p))
                            availablePassives.Add(p);
                    break;
                case 1:
                    foreach (PASSIVES p in WorldData.GodList[chara.god].Class1Passives)
                        if (!chara.HavePassive(p))
                            availablePassives.Add(p);
                    break;
                case 2:
                    foreach (PASSIVES p in WorldData.GodList[chara.god].Class2Passives)
                        if (!chara.HavePassive(p))
                            availablePassives.Add(p);
                    break;
            }


            string[] options = new string[availablePassives.Count];
            bool[] disabledOptions = new bool[options.Length];

            //populate and format the choices
            for (int i = 0; i < options.Length; i++)
            {
                double nbOfDigitInCost = Math.Floor(Math.Log10(Skills.GetPassiveCost(availablePassives[i])) + 1);

                options[i] = " ";
                options[i] += Skills.GetPassiveName(availablePassives[i]);
                while (options[i].Length < 22 - nbOfDigitInCost)
                    options[i] += " ";
                options[i] = options[i].Substring(0, 22 - (int)nbOfDigitInCost);
                options[i] += "[" + Skills.GetPassiveName(availablePassives[i]) + "]";

                if (creationPoints < Skills.GetPassiveCost(availablePassives[i]))
                    disabledOptions[i] = true;
            }

            int choice = Helper.ExtendedBoxSelection(options, 4, 12 + chara.passivesList.Count, 25, 23 - chara.passivesList.Count, disabledOptions);
            if (choice != -1)
            {
                if (Skills.GetPassiveCost(availablePassives[choice - 1]) <= creationPoints)
                {
                    chara.passivesList.Add(availablePassives[choice - 1]);
                    creationPoints -= Skills.GetPassiveCost(availablePassives[choice - 1]);
                    UpdateCreationPointsVisual();
                }
            }
            DrawSkills();
        }

        void RefundAllPassives()
        {
            while (chara.skillList.Count > 0)
            {
                creationPoints += Skills.GetPassiveCost((PASSIVES)chara.passivesList[0]);
                chara.passivesList.RemoveAt(0);
            }
            DrawPassives();
            UpdateCreationPointsVisual();
        }

        void DrawPassives()
        {
            Console.BackgroundColor = Settings.DefaultBackgroundColor;
            Console.ForegroundColor = Settings.DefaultForegroundColor;
            for (int i = 0; i < 23; i++)
            {
                Console.SetCursorPosition(4, 12 + i);
                Console.Write("                         ");
            }

            for (int i = 0; i < chara.passivesList.Count; i++)
            {
                Console.SetCursorPosition(5, 12 + i);
                Console.Write(Skills.GetPassiveName((PASSIVES)chara.skillList[i]));
            }
        }


        PointerLocation pointerAtPosition_Gear()
        {
            PointerLocation _nextLocation = PointerLocation.Gear;
            Console.BackgroundColor = Settings.DefaultSelectionColor;
            Console.SetCursorPosition(62, 8);
            Console.Write("                         ");
            Console.SetCursorPosition(62, 9);
            Console.Write("          Gear           ");
            Console.SetCursorPosition(62, 10);
            Console.Write("                         ");

            bool changePosition = false;
            while (!changePosition)
            {
                ConsoleKeyInfo input = Helper.GetUserInput(new ConsoleKey[] { ConsoleKey.UpArrow, ConsoleKey.NumPad8, ConsoleKey.LeftArrow, ConsoleKey.NumPad4, ConsoleKey.RightArrow, ConsoleKey.NumPad6 }, true);

                switch (input.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.NumPad8:
                        _nextLocation = PointerLocation.Fortune;
                        changePosition = true;
                        break;
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.NumPad4:
                        _nextLocation = PointerLocation.Passives;
                        changePosition = true;
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.NumPad6:
                        _nextLocation = PointerLocation.Others;
                        changePosition = true;
                        break;
                    case ConsoleKey.Spacebar:
                    case ConsoleKey.NumPad5:
                    case ConsoleKey.Enter:
                        changePosition = true;
                        break;
                }
            }

            Console.BackgroundColor = Settings.DefaultBackgroundColor;
            Console.SetCursorPosition(62, 8);
            Console.Write("                         ");
            Console.SetCursorPosition(62, 9);
            Console.Write("          Gear           ");
            Console.SetCursorPosition(62, 10);
            Console.Write("                         ");
            return _nextLocation;
        }

        PointerLocation pointerAtPosition_Others()
        {
            PointerLocation _nextLocation = PointerLocation.Others;
            Console.BackgroundColor = Settings.DefaultSelectionColor;
            Console.SetCursorPosition(91, 8);
            Console.Write("                         ");
            Console.SetCursorPosition(91, 9);
            Console.Write("         Others          ");
            Console.SetCursorPosition(91, 10);
            Console.Write("                         ");

            bool changePosition = false;
            while (!changePosition)
            {
                ConsoleKeyInfo input = Helper.GetUserInput(new ConsoleKey[] { ConsoleKey.UpArrow, ConsoleKey.NumPad8, ConsoleKey.LeftArrow, ConsoleKey.NumPad4 }, true);

                switch (input.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.NumPad8:
                        _nextLocation = PointerLocation.Fortune;
                        changePosition = true;
                        break;
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.NumPad4:
                        _nextLocation = PointerLocation.Gear;
                        changePosition = true;
                        break;
                    case ConsoleKey.Spacebar:
                    case ConsoleKey.NumPad5:
                    case ConsoleKey.Enter:
                        changePosition = true;
                        break;
                }
            }

            Console.BackgroundColor = Settings.DefaultBackgroundColor;
            Console.SetCursorPosition(91, 8);
            Console.Write("                         ");
            Console.SetCursorPosition(91, 9);
            Console.Write("         Others          ");
            Console.SetCursorPosition(91, 10);
            Console.Write("                         ");
            return _nextLocation;
        }

        
        void ShowMessage(string _lineTop = "", string _lineMid = "", string _lineBot = "")
        {
            ConsoleColor _backgroundColor = Console.BackgroundColor;
            ConsoleColor _foregroundColor = Console.ForegroundColor;
            Console.BackgroundColor = Settings.DefaultBackgroundColor;
            Console.ForegroundColor = Settings.DefaultForegroundColor;
            string eraser = "";
            while (eraser.Length < Console.BufferWidth - 3)
                eraser += " ";

            Console.SetCursorPosition(1, Console.BufferHeight - 4);
            Console.Write(eraser);
            Console.SetCursorPosition(1, Console.BufferHeight - 3);
            Console.Write(eraser);
            Console.SetCursorPosition(1, Console.BufferHeight - 2);
            Console.Write(eraser);

            Console.SetCursorPosition(Console.BufferWidth / 2 - _lineTop.Length / 2, Console.BufferHeight - 4);
            Console.Write(_lineTop);
            Console.SetCursorPosition(Console.BufferWidth / 2 - _lineMid.Length / 2, Console.BufferHeight - 3);
            Console.Write(_lineMid);
            Console.SetCursorPosition(Console.BufferWidth / 2 - _lineBot.Length / 2, Console.BufferHeight - 2);
            Console.Write(_lineBot);

            Console.BackgroundColor = _backgroundColor;
            Console.ForegroundColor = _foregroundColor;
        }

        void UpdateNameVisual(bool isSelected = false)
        {
            Console.BackgroundColor = Settings.DefaultBackgroundColor;
            if (isSelected)
                Console.BackgroundColor = Settings.DefaultSelectionColor;
            Console.SetCursorPosition(11, 1);
            Console.Write("                                   ");
            Console.SetCursorPosition(11, 1);
            Console.Write(chara.name);
            Console.BackgroundColor = Settings.DefaultBackgroundColor;
        }

        void UpdateSurnameVisual(bool isSelected = false)
        {
            Console.BackgroundColor = Settings.DefaultBackgroundColor;
            if (isSelected)
                Console.BackgroundColor = Settings.DefaultSelectionColor;
            Console.SetCursorPosition(11, 2);
            Console.Write("                                   ");
            Console.SetCursorPosition(11, 2);
            Console.Write(chara.surname);
            Console.BackgroundColor = Settings.DefaultBackgroundColor;
        }

        void UpdateCreationPointsVisual()
        {
            Console.BackgroundColor = Settings.DefaultBackgroundColor;
            Console.SetCursorPosition(98, 1);
            string toWrite = "";
            if (creationPoints < 1000)
                toWrite += " ";
            if (creationPoints < 100)
                toWrite += " ";
            if (creationPoints < 10)
                toWrite += " ";
            toWrite += creationPoints;
            Console.Write(toWrite);
        }

        void UpdateEssenceVisual()
        {
            Console.SetCursorPosition(11, 5);
            Console.Write("                                   ");
            Console.SetCursorPosition(11, 5);
            Console.Write(chara.GetEssenceAsString());
        }

        void UpdateGodVisual()
        {
            ConsoleColor _currentBGColor = Console.BackgroundColor;
            Console.SetCursorPosition(11, 4);
            Console.Write("                                   ");
            Console.SetCursorPosition(11, 4);
            Console.Write(WorldData.GodList[chara.god].Name);

            Console.BackgroundColor = Settings.DefaultBackgroundColor;
            UpdateEssenceVisual();
            UpdateAllStatsVisual();

            Console.BackgroundColor = _currentBGColor;
        }

        void UpdateStatVisual(String statName, int statsValue, int yPosition, int _alreadyAssigned = 0, bool selected = false)
        {
            while (statName.Length < 8)
                statName += " ";

            Console.BackgroundColor = Settings.DefaultBackgroundColor;
            if (selected)
                Console.BackgroundColor = Settings.DefaultSelectionColor;

            Console.SetCursorPosition(49, yPosition);
            Console.Write(statName.Substring(0, 8));

            string toWrite = "";
            if (statsValue < 10)
                toWrite += " ";
            toWrite += statsValue;
            Console.Write(toWrite);

            Console.Write(" ");
            if (selected)
            {
                if ((_alreadyAssigned + 1) * 50 > creationPoints)
                    Console.ForegroundColor = Settings.DisabledColor;
                Console.Write("+");
                Console.ForegroundColor = Settings.DefaultForegroundColor;
                Console.Write("/");
                if (_alreadyAssigned <= 0)
                    Console.ForegroundColor = Settings.DisabledColor;
                Console.Write("- ");
                Console.ForegroundColor = Settings.DefaultForegroundColor;
                if (_alreadyAssigned < 1)
                    Console.Write("(cost:  " + (_alreadyAssigned + 1) * 50 + ") ");
                else
                    Console.Write("(cost: " + (_alreadyAssigned + 1) * 50 + ") ");
            }
            else if (_alreadyAssigned != 0)
            {
                Console.ForegroundColor = Settings.FadingColor;
                Console.Write("[");
                Console.ForegroundColor = Settings.DefaultSelectionColor;
                Console.Write("+" + _alreadyAssigned);
                Console.ForegroundColor = Settings.FadingColor;
                Console.Write("]            ");
                Console.ForegroundColor = Settings.DefaultForegroundColor;
            }
            else
                Console.Write("                ");
        }
        

        void UpdateAllStatsVisual()
        {
            UpdateStatVisual("Might", chara.might, 1, assignedMight);
            UpdateStatVisual("Panache", chara.panache, 2, assignedPanache);
            UpdateStatVisual("Grit", chara.grit, 3, assignedGrit);
            UpdateStatVisual("Insight", chara.insight, 4, assignedInsight);
            UpdateStatVisual("Wisdom", chara.wisdom, 5, assignedWisdom);
            UpdateStatVisual("Fortune", chara.fortune, 6, assignedFortune);
        }
    }
}
