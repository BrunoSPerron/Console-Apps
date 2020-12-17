using System;
using System.Collections.Generic;
using System.Text;

namespace TacticRoguelikeRpg
{
    class CombatMap
    {
        String[] map = new string[28];
        String[,] detailledMap = new string[43, 28];
        int mapOffsetX = 17;
        int mapOffsetY = 5;

        List<int> possibleStartingAllyPositions = new List<int>(); // x1,y1,x2,y2, etc
        List<int> possibleStartingEnemyPositions = new List<int>(); // x1,y1,x2,y2, etc

        //palette:
        //¶ § ! $ %  ( ) * +  -  /  : ; < = > ? @ ^ _ { | } ~ Ç å ç É æ Æ ô ò ì ø Ø ×  ó ú ñ ª º ¿ ® ¬ ½ ¼ ¡ « » 
        //  ©  ¢ ¥  ã  ¤ ð Ð ı Ï █ ▄ ¦ ▀ Ó ß Ò õ µ þ Þ Ý ¯ ­­± ‗ ¾ ÷ ° ¹ ³ ² 
        //
        //    \\
        //
        //BLOCKED TERRAIN
        //?tree: £ ƒ Ì &
        //?Stone Wall: []    │ ┤ ├ ─ ┼ ├ ┐ └ ┴ ┬ ┘ ┌ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╣ ║ ╗ ╝ 
        //natural cavern wall: #
        //
        //PASSABLE TERRAIN:
        //?ground doodads (grass?): ' , ` ¨ · ´ ¸ . "
        //
        //MARKER
        // E: enemy spawn point
        // P: party spawn point

        public void GenerateNewMap()
        {
            //TODO - Set some procedural generation algorythms here
            SetCavernMap(); //Placeholder
            PopulatePossibleStartingPositions();
            VirtualDrawBattleBackground();
            //DrawBattleWalkable();
            VirtualDrawObstacle();
            SplitMap();

        }

        private void SplitMap()
        {
            for (int y = 0; y < 28; y++)
            {
                string[] infoLineY = map[y].Split('|');
                for (int x = 0; x < 43; x++)
                {
                    string _word = infoLineY[x].Replace('E', ' ');
                    _word = _word.Replace('P', ' ');
                    detailledMap[x, y] = _word;
                }
            }

        }

        private void PopulatePossibleStartingPositions()
        {
            for (int y = 0; y < map.Length; y++)
            {
                string[] lineData = map[y].Split('|');

                int x = 0;
                foreach (string word in lineData)
                {
                    if (word.Contains("E"))
                    {
                        possibleStartingEnemyPositions.Add(x);
                        possibleStartingEnemyPositions.Add(y);
                    }
                    if (word.Contains("P"))
                    {
                        possibleStartingAllyPositions.Add(x);
                        possibleStartingAllyPositions.Add(y);
                    }
                    x++;
                }
            }
        }

        private void VirtualDrawBattleBackground()
        {
            ExtendedConsole.SetActiveLayer(0);
            Console.BackgroundColor = Settings.CombatMapGridColor1;
            for (int i = 0; i < 28; i++)
                for (int j = 0; j < 43; j++)
                    if (j % 2 == 1 && i % 2 == 1 || j % 2 == 0 && i % 2 == 0)
                        ExtendedConsole.VirtualWrite("  ", mapOffsetX + j * 2, mapOffsetY + i);
            if (Settings.CombatMapGridColor2 != Settings.DefaultBackgroundColor)
            {
                Console.BackgroundColor = Settings.CombatMapGridColor2;
                for (int i = 0; i < 28; i++)
                    for (int j = 0; j < 43; j++)
                        if (!(j % 2 == 1 && i % 2 == 1 || j % 2 == 0 && i % 2 == 0))
                            ExtendedConsole.VirtualWrite("  ", mapOffsetX + j * 2, mapOffsetY + i);
            }
        }

        private void DrawBattleWalkable()
        {
            ExtendedConsole.SetActiveLayer(0);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Random rand = new Random();
            for (int i = 0; i < 28; i++)
                for (int j = 0; j < 43; j++)
                {
                    string toWrite = "";

                    while (toWrite.Length < 2)
                    {
                        int symbol = rand.Next(0, 50);
                        switch (symbol)
                        {
                            case 0:
                                toWrite += "\"";
                                break;
                            case 1:
                                toWrite += ".";
                                break;
                            case 2:
                                toWrite += "¸";
                                break;
                            case 3:
                                toWrite += "´";
                                break;
                            case 4:
                                toWrite += "·";
                                break;
                            case 5:
                                toWrite += "¨";
                                break;
                            case 6:
                                toWrite += "`";
                                break;
                            case 7:
                                toWrite += ",";
                                break;
                            case 8:
                                toWrite += "'";
                                break;
                            default:
                                toWrite += " ";
                                break;
                        }

                    }
                    SetCorrectBGColorForPosition(j, i);
                    ExtendedConsole.VirtualWrite(toWrite, mapOffsetX + j * 2, mapOffsetY + i);
                }

        }
        private void VirtualDrawObstacle()
        {
            ExtendedConsole.SetActiveLayer(2);
            for (int i = 0; i < map.Length; i++)
            {
                string[] lineData = map[i].Split('|');

                int j = 0;
                foreach (string word in lineData)
                {
                    string _word = word.Replace('E', ' ');
                    _word = _word.Replace('P', ' ');

                    if (_word.Substring(0, 2) == "  ")
                    {
                        j++;
                        continue;
                    }
                    SetCorrectBGColorForPosition(i, j);

                    Console.ForegroundColor = ConsoleColor.Gray;

                    ExtendedConsole.VirtualWrite(_word.Substring(0, 2), mapOffsetX + j * 2, mapOffsetY + i);
                    j++;
                }
            }
        }

        public void DrawFighterOnBattleMap(Combat parent)
        {
            for (int i = 0; i < parent.fighters.Count; i++)
            {
                SetCorrectBGColorForPosition(parent.positionX[i], parent.positionY[i]);
                Console.ForegroundColor = parent.GetColorForCharacter(i);

                ExtendedConsole.SetActiveLayer(2);
                ExtendedConsole.VirtualWrite(parent.fighters[i].GetShortDesignation(), parent.positionX[i] * 2 + 17, parent.positionY[i] + 5);
            }

            Console.BackgroundColor = Settings.DefaultBackgroundColor;
            Console.ForegroundColor = Settings.DefaultForegroundColor;
        }

        public void UpdatePosition(int _x, int _y)
        {
            ExtendedConsole.Update(mapOffsetX + _x * 2, mapOffsetY + _y, 2, 1);
        }

        private void SetCavernMap()
        {
            map[0] = "##|##|##|##|##|##|##|##|##|##|##|##|##|##|##|##|##|##|##|##|##|##|##|##|##|##|##|##|##|##|##|##|##|##|##|##|##|##|##|##|##|##|##";
            map[1] = "##|##|##|##|##|##|# | E|##|##|##|##|# |  |  | #|##|# |  |  |  | #|##|##|##|##|##|##|##|##|##|##|##|##|##|  |  |##|##|##|##|##|##";
            map[2] = "##|# |  |  |##|# | E| E| E|##|# | E|  |  | E|  |  |  |  |  |  |  |  |  |  |##|##|##|##|##|##|##|##|##|  |  |  | #|##|##|##|##|##";
            map[3] = "  |  |  |  |##|##|# | E| E| E| E| E|  | #|##| E|  | #|##|##|##|##|# |  |  |  |  |##|##|##|##|##|##|# |  |  |  |  |##|##|##|##|##";
            map[4] = "  |  |  |  |  |##|##| E| E| E| E| E| #|##|##|# |  | E| E| #|##|##|##|##|##|# |  | #|##|##|# | E|  |  |  | E|  |  |##|##|##|##|##";
            map[5] = "# |  |  |  |  |  |##| E| E| E| E|  |  | #|##|  |  | E| E| E|##|##|##|##|##|##|  |  |##| E| E|  |  |  |  |  |  |  | #|##|##|##|##";
            map[6] = "##|  |  |  |  |  |##|##| E| E| E|  |  |  |  |  |  | E| E| E|##|##|##|##|##|##|  |##|##| E|  |  |  |  |  |  |  |  |  |##|##|##|##";
            map[7] = "##|  |  |  |  |  | #|##|# | E| E|  |  |  |  |  | E| E| E| E| E|##|##|##|##|# |  | #|##| E|  |  |  |  | #|# |  |  |  | #|##|##|##";
            map[8] = "##|  |  |  |  |  | P|##|##| E|  |  |  |  |  |  |  |  |  | E| E| E| #|##|##|  |  |  |##|##|  |  |  |  |##|##|##|##|  | E|##|##|##";
            map[9] = "##|# |  |  |  |  | P| P| #|##|##|  |  |  |  |  |  |  |  | E| E| E|##|##|##|  |  | #|##|##|  |  | #|##|##|##|##|##|##|  |  |##|##";
            map[10] = "##|##|  |  |  |##| P| P| P|##| P|  |  |  |  |  |  |  |  |  |  |##|##|##|# |  |  |##|##|# |  |  |##|##|##|# | E| #|# |  | E|##|##";
            map[11] = "##|##|##|##|##|##|##| P| P| P| P| P|  |  |  |  |  |  |  |  |  | #|##|##| P|  |  |##|# |  |  |  | #|##|##|  |  | #|# |  | #|##|##";
            map[12] = "##|##|##|##|##|##|##| P| P| P| P| P|# |  |  |  |  |  | E| E|  |  | #|# |  |  |  | P|  |  |  |  |  |  |  |  |  | E|##|##|##|##|##";
            map[13] = "##|##|##|##|##|##|##|# | P| P| P| P|##|# |  |  | E| E| E| E|##|  |  |  |  |  | P|  |  |  |  | E|  |  |  |  |  |  |##|##|##|##|##";
            map[14] = "##|##|##|##|##|##|##|##| P| P| P| P|##|##|# | E| E| E| #|##|##| P|  |  |  | P|  |  |  |  |  |  |  |  |  |  |  | #|##|##|##|##|##";
            map[15] = "##|##|##|##|##|##|##|##|##|# | P| P| #|##|##|##|##|##|##|##|##|# |  |  | P|  |  |  |  |  |##|##|  | E|  |  |##|##|##|##|##|##|##";
            map[16] = "##|##|##|##|##|##|##|##|##|##|##| P|  |##|##|##|##|##|##|##|##|##|##|##| P| P| P| P| #|##|##|##|##|##|##|##|##|##|##|##|##|##|##";
            map[17] = "##|##|##|##|##|##|##|##|##|##|##|# |  |  |##|##|##|##|##|##|##|##|##|##|##| P| P| P|##|##|##|##|##|##|##|##|##|##|##|##|##|##|##";
            map[18] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[19] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[20] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[21] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[22] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[23] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[24] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[25] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[26] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |##|  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[27] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
        }
        private void SetEmptyMap()
        {
            map[0] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[1] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[2] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[3] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[4] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[5] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[6] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[7] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[8] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[9] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[10] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[11] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[12] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[13] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[14] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[15] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[16] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[17] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[18] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[19] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[20] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[21] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[22] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[23] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[24] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[25] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[26] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
            map[27] = "  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  ";
        }

        public String[] GetMap()
        {
            return map;
        }

        public int[] GetPossibleEnemyStartingPoints()
        {
            return possibleStartingEnemyPositions.ToArray();
        }
        public int[] GetPossibleAllyStartingPoints()
        {
            return possibleStartingAllyPositions.ToArray();
        }

        public int[] GetAndRemoveAllyStartingPosition()
        {
            int[] randomlyChoosenStartingPosition = new int[2];
            Random rand = new Random();
            int i;
            i = rand.Next(0, possibleStartingAllyPositions.Count / 2);
            randomlyChoosenStartingPosition[0] = possibleStartingAllyPositions[2 * i];
            randomlyChoosenStartingPosition[1] = possibleStartingAllyPositions[2 * i + 1];
            possibleStartingAllyPositions.RemoveAt(2 * i);
            possibleStartingAllyPositions.RemoveAt(2 * i);
            return (randomlyChoosenStartingPosition);
        }

        public int[] GetAndRemoveEnemyStartingPosition()
        {
            int[] randomlyChoosenStartingPosition = new int[2];
            Random rand = new Random();
            int i;
            i = rand.Next(0, possibleStartingEnemyPositions.Count / 2);
            randomlyChoosenStartingPosition[0] = possibleStartingEnemyPositions[2 * i];
            randomlyChoosenStartingPosition[1] = possibleStartingEnemyPositions[2 * i + 1];
            possibleStartingEnemyPositions.RemoveAt(2 * i);
            possibleStartingEnemyPositions.RemoveAt(2 * i);
            return (randomlyChoosenStartingPosition);
        }

        public void SetCorrectBGColorForPosition(int x, int y)
        {
            if (x % 2 == 1 && y % 2 == 1 || x % 2 == 0 && y % 2 == 0)
                Console.BackgroundColor = Settings.CombatMapGridColor1;
            else
                Console.BackgroundColor = Settings.CombatMapGridColor2;
        }

        public bool GetTerrainPassable(int x, int y)
        {
            if (ExtendedConsole.GetCHARINFOAtPosition(x * 2 + mapOffsetX, y + mapOffsetY, 2).charData != null && ExtendedConsole.GetCHARINFOAtPosition(x * 2 + mapOffsetX + 1, y + mapOffsetY, 2).charData != null)
                return false;
            return true;
        }

        public bool[,] ShowPossibleMoves(int x, int y, int move)
        {
            int[,] movesCost = new int[43, 28];
            List<Position> newPositions = new List<Position>();
            newPositions.Add(new Position(x, y));
            movesCost[x, y] = (move + 1) * 2;
            while (newPositions.Count > 0)
            {
                List<Position> nextNewPositions = new List<Position>();
                foreach (Position p in newPositions)
                {
                    int nextCardinalCost = movesCost[p.x, p.y] - 2;
                    int nextDiagonalCost = movesCost[p.x, p.y] - 3;
                    if (nextCardinalCost > 0)
                    {
                        if (GetTerrainPassable(p.x, p.y - 1))
                            if (movesCost[p.x, p.y - 1] < nextCardinalCost)
                            {
                                movesCost[p.x, p.y - 1] = nextCardinalCost;
                                nextNewPositions.Add(new Position(p.x, p.y - 1));
                            }
                        if (GetTerrainPassable(p.x, p.y + 1))
                            if (movesCost[p.x, p.y + 1] < nextCardinalCost)
                            {
                                movesCost[p.x, p.y + 1] = nextCardinalCost;
                                nextNewPositions.Add(new Position(p.x, p.y + 1));
                            }
                        if (GetTerrainPassable(p.x - 1, p.y))
                            if (movesCost[p.x - 1, p.y] < nextCardinalCost)
                            {
                                movesCost[p.x - 1, p.y] = nextCardinalCost;
                                nextNewPositions.Add(new Position(p.x - 1, p.y));
                            }
                        if (GetTerrainPassable(p.x + 1, p.y))
                            if (movesCost[p.x + 1, p.y] < nextCardinalCost)
                            {
                                movesCost[p.x + 1, p.y] = nextCardinalCost;
                                nextNewPositions.Add(new Position(p.x + 1, p.y));
                            }
                    }
                    if (nextDiagonalCost > 0)
                    {
                        if (GetTerrainPassable(p.x - 1, p.y - 1))
                            if (movesCost[p.x - 1, p.y - 1] < nextDiagonalCost)
                            {
                                movesCost[p.x - 1, p.y - 1] = nextDiagonalCost;
                                nextNewPositions.Add(new Position(p.x - 1, p.y - 1));
                            }
                        if (GetTerrainPassable(p.x - 1, p.y + 1))
                            if (movesCost[p.x - 1, p.y + 1] < nextDiagonalCost)
                            {
                                movesCost[p.x - 1, p.y + 1] = nextDiagonalCost;
                                nextNewPositions.Add(new Position(p.x - 1, p.y + 1));
                            }
                        if (GetTerrainPassable(p.x + 1, p.y - 1))
                            if (movesCost[p.x + 1, p.y - 1] < nextDiagonalCost)
                            {
                                movesCost[p.x + 1, p.y - 1] = nextDiagonalCost;
                                nextNewPositions.Add(new Position(p.x + 1, p.y - 1));
                            }
                        if (GetTerrainPassable(p.x + 1, p.y + 1))
                            if (movesCost[p.x + 1, p.y + 1] < nextDiagonalCost)
                            {
                                movesCost[p.x + 1, p.y + 1] = nextDiagonalCost;
                                nextNewPositions.Add(new Position(p.x + 1, p.y + 1));
                            }
                    }
                }
                newPositions = nextNewPositions;
            }

            bool[,] possibleMoves = new bool[43, 28];

            Console.BackgroundColor = Settings.CombatMoveGridColor1;
            ExtendedConsole.SetActiveLayer(1);
            for (int i = 0; i < 43; i++)
            {
                for (int j = 0; j < 28; j++)
                {
                    if (movesCost[i, j] > 0)
                    {
                        possibleMoves[i, j] = true;
                        if (i % 2 == 1 && j % 2 == 1 || i % 2 == 0 && j % 2 == 0)
                            Console.BackgroundColor = Settings.CombatMoveGridColor1;
                        else
                            Console.BackgroundColor = Settings.CombatMoveGridColor2;

                        ExtendedConsole.VirtualWrite("  ", i * 2 + mapOffsetX, j + mapOffsetY);
                    }
                }
            }

            return possibleMoves;
        }

        public Stack<Position> GetPath(Position origin, Position destination, int maxMove = -1)
        {
            if (maxMove == -1)
                maxMove = 1000;

            int[,] estimatedDistanceToDestination = new int[43, 28];
            int[,] distanceFromOrigin = new int[43, 23];
            List<Position> positionsToVerify = new List<Position>();

            estimatedDistanceToDestination[origin.x, origin.y] = simplifiedPythagoreTimeTwo(origin, destination);
            distanceFromOrigin[origin.x, origin.y] = 1;
            positionsToVerify.Add(origin);

            bool pathFound = false;
            while (positionsToVerify.Count > 1 && !pathFound)
            {
                int indexToRemove = -1;
                int smallestDestinationDistance = 1001;
                Position closestToDestination = new Position(0, 0);

                for (int i = 0; i < positionsToVerify.Count; i++)
                {
                    if (estimatedDistanceToDestination[positionsToVerify[i].x, positionsToVerify[i].y] <= smallestDestinationDistance)
                    {
                        closestToDestination = positionsToVerify[i];
                        indexToRemove = i;
                    }
                }

                if ((distanceFromOrigin[closestToDestination.x, closestToDestination.y] - 1) / 2 + 1 <= maxMove)
                {
                    AddPosition(closestToDestination.x + 1, closestToDestination.y, distanceFromOrigin[closestToDestination.x, closestToDestination.y] + 2);
                    AddPosition(closestToDestination.x - 1, closestToDestination.y, distanceFromOrigin[closestToDestination.x, closestToDestination.y] + 2);
                    AddPosition(closestToDestination.x, closestToDestination.y + 1, distanceFromOrigin[closestToDestination.x, closestToDestination.y] + 2);
                    AddPosition(closestToDestination.x, closestToDestination.y - 1, distanceFromOrigin[closestToDestination.x, closestToDestination.y] + 2);

                    AddPosition(closestToDestination.x + 1, closestToDestination.y + 1, distanceFromOrigin[closestToDestination.x, closestToDestination.y] + 3);
                    AddPosition(closestToDestination.x + 1, closestToDestination.y - 1, distanceFromOrigin[closestToDestination.x, closestToDestination.y] + 3);
                    AddPosition(closestToDestination.x - 1, closestToDestination.y + 1, distanceFromOrigin[closestToDestination.x, closestToDestination.y] + 3);
                    AddPosition(closestToDestination.x - 1, closestToDestination.y - 1, distanceFromOrigin[closestToDestination.x, closestToDestination.y] + 3);
                }

                positionsToVerify.RemoveAt(indexToRemove);

                void AddPosition(int posX, int posY, int distOrigin)
                {
                    if (ExtendedConsole.GetCHARINFOAtPosition(posX * 2 + mapOffsetX, posY + mapOffsetY, 2).charData != null ||
                        ExtendedConsole.GetCHARINFOAtPosition(posX * 2 + mapOffsetX + 1, posY + mapOffsetY, 2).charData != null)
                        return;

                    Position newPos = new Position(posX, posY);
                    if (distanceFromOrigin[posX, posY] == 0)
                    {
                        distanceFromOrigin[posX, posY] = distOrigin;
                        positionsToVerify.Add(newPos);
                    }
                    else return;

                    if (estimatedDistanceToDestination[posX, posY] == 0)
                    {
                        estimatedDistanceToDestination[posX, posY] = simplifiedPythagoreTimeTwo(newPos, destination);
                        if (estimatedDistanceToDestination[posX, posY] == 0)
                            pathFound = true;
                    }
                }
            }
            if (!pathFound)
                return null;

            Stack<Position> path = new Stack<Position>();

            path.Push(destination);
            bool pathIsComplete = false;
            while (!pathIsComplete)
            {
                Position nextPosition = new Position(path.Peek().x, path.Peek().y);

                if (CheckPosition(path.Peek().x + 1, path.Peek().y) < CheckPosition(nextPosition.x, nextPosition.y))
                    nextPosition = new Position(path.Peek().x + 1, path.Peek().y);
                if (CheckPosition(path.Peek().x - 1, path.Peek().y) < CheckPosition(nextPosition.x, nextPosition.y))
                    nextPosition = new Position(path.Peek().x - 1, path.Peek().y);
                if (CheckPosition(path.Peek().x, path.Peek().y + 1) < CheckPosition(nextPosition.x, nextPosition.y))
                    nextPosition = new Position(path.Peek().x, path.Peek().y + 1);
                if (CheckPosition(path.Peek().x, path.Peek().y - 1) < CheckPosition(nextPosition.x, nextPosition.y))
                    nextPosition = new Position(path.Peek().x, path.Peek().y - 1);

                if (CheckPosition(path.Peek().x + 1, path.Peek().y + 1) < CheckPosition(nextPosition.x, nextPosition.y))
                    nextPosition = new Position(path.Peek().x + 1, path.Peek().y + 1);
                if (CheckPosition(path.Peek().x + 1, path.Peek().y - 1) < CheckPosition(nextPosition.x, nextPosition.y))
                    nextPosition = new Position(path.Peek().x + 1, path.Peek().y - 1);

                if (CheckPosition(path.Peek().x - 1, path.Peek().y + 1) < CheckPosition(nextPosition.x, nextPosition.y))
                    nextPosition = new Position(path.Peek().x - 1, path.Peek().y + 1);
                if (CheckPosition(path.Peek().x - 1, path.Peek().y - 1) < CheckPosition(nextPosition.x, nextPosition.y))
                    nextPosition = new Position(path.Peek().x - 1, path.Peek().y - 1);

                if (distanceFromOrigin[nextPosition.x, nextPosition.y] == 1)
                    pathIsComplete = true;
                else
                    path.Push(nextPosition);

                int CheckPosition(int posX, int posY)
                {
                    if (distanceFromOrigin[posX, posY] == 0)
                        return 1000;
                    else return distanceFromOrigin[posX, posY];
                }
            }
            return path;
        }

        public void DrawPath(Stack<Position> path, Position target)
        {
            ExtendedConsole.SetActiveLayer(3);
            ExtendedConsole.VirtualLayerReset();
            Console.ForegroundColor = Settings.PathColor;
            if (path.Count > 0)
            {
                Position currentPosition = path.Pop();
                ExtendedConsole.VirtualWrite("[]", mapOffsetX + currentPosition.x, mapOffsetY + currentPosition.y);
            }
            else return;

            while (path.Count > 0)
            {
                Position currentPosition = path.Pop();
                ExtendedConsole.VirtualWrite("::", mapOffsetX + currentPosition.x, mapOffsetY + currentPosition.y);
            }
        }

        int simplifiedPythagoreTimeTwo(Position a, Position b)
        {

            int distanceVertical = Math.Abs(a.y - b.y);
            int distanceHorizontal = Math.Abs(a.x - b.x);
            int smallerCardinalDistance = Math.Min(distanceVertical, distanceHorizontal);
            return (distanceHorizontal + distanceVertical) * 2 - smallerCardinalDistance;
        }
    }
    struct Position
    {
        internal int x;
        internal int y;
        internal Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
