using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TacticRoguelikeRpg
{
    class Combat
    {
        String currentMessage = "";
        String logOne = "";
        String logTwo = "";

        public List<Character> fighters { get; set; } = new List<Character>();
        List<Character> allies = new List<Character>();
        List<Character> enemies = new List<Character>();
        List<Character> alliesBackup = new List<Character>(); //store over limit characters, unused otherwise
        List<Character> enemiesBackup = new List<Character>(); //store over limit characters, unused otherwise

        Character[,] fightersGridPosition = new Character[43, 38];
        public int[] positionX { get; set; } //same index order as fighters
        public int[] positionY { get; set; } //same index order as fighters
        int[] aTBGauge;  //same index order as fighters
        bool[] fastATB;  //same index order as fighters
        bool[] slowATB;  //same index order as fighters
        //are updated in Repopulate();

        public CombatMap battlefield { get; set; } = new CombatMap();
        bool combatIsOngoing = true;


        public Combat(List<Character> _allies, Character[] _enemies)
        {
            //populate fighters
            int i = 0;
            foreach (Character dude in _allies)
            {
                if (i < 6)
                {
                    allies.Add(dude);
                    fighters.Add(dude);
                }
                else
                {
                    alliesBackup.Add(dude);
                }
                i++;
            }
            foreach (Character dude in _enemies)
            {
                if (i < allies.Count + 14)
                {
                    enemies.Add(dude);
                    fighters.Add(dude);
                }
                else
                {
                    enemiesBackup.Add(dude);
                }
            }

            aTBGauge = new int[fighters.Count];
            positionX = new int[fighters.Count];
            positionY = new int[fighters.Count];
            fastATB = new bool[fighters.Count];
            slowATB = new bool[fighters.Count];

            //Randomly set starting ATB
            i = 0;
            Random rand = new Random();
            foreach (Character dude in fighters)
            {
                aTBGauge[i] = rand.Next(0, 500);
                i++;
            }

            ExtendedConsole.VirtualClear();
            DrawCombatInterface();
            battlefield.GenerateNewMap();
            SetStartingFightersPosition();
            battlefield.DrawFighterOnBattleMap(this);

            //combat loop
            while (combatIsOngoing)
            {
                int currentlyActive = NextCharacter();
                Act(currentlyActive);
            }
        }

        private void SetStartingFightersPosition()
        {
            int i = 0;
            foreach (Character dude in allies)
            {
                if (battlefield.GetPossibleAllyStartingPoints().Length == 0)
                    ShowMessage("ERROR - NOT ENOUGH STARTING ALLY POSITIONS ON MAP", true);

                int[] _position = battlefield.GetAndRemoveAllyStartingPosition();

                positionX[i] = _position[0];
                positionY[i] = _position[1];
                fightersGridPosition[_position[0], _position[1]] = dude;
                i++;
            }

            foreach (Character dude in enemies)
            {
                if (battlefield.GetPossibleEnemyStartingPoints().Length == 0)
                    ShowMessage("ERROR - NOT ENOUGH STARTING ENEMY POSITIONS ON MAP", true);

                int[] _position = battlefield.GetAndRemoveEnemyStartingPosition();

                positionX[i] = _position[0];
                positionY[i] = _position[1];
                fightersGridPosition[_position[0], _position[1]] = dude;
                i++;
            }
        }

        public void DrawCombatInterface(bool isTargeting = false)
        {
            ExtendedConsole.SetActiveLayer(0);
            Console.BackgroundColor = Settings.DefaultBackgroundColor;

            ExtendedConsole.VirtualDrawBox(0, 0, 120, 5);
            ExtendedConsole.VirtualDrawHorizontalLine(33, 16, 88, false);
            ExtendedConsole.VirtualDrawBox(16, 4, 88, 36);
            ExtendedConsole.Update();

            int i = 0;
            foreach (Character dude in enemies)
            {
                DrawEnemyBox(i);
                DrawEnemyStats(i);
                i++;
            }

            for (int j = 0; j < allies.Count; j++)
            {
                DrawAllyStats(j);
            }
            ExtendedConsole.Update();

        }

        private void ResetInterface()
        {
            DrawCombatInterface();
            UpdateATB();
        }

        public void ShowMessage(string _newMessage = "", bool waitForConfirm = false)
        {
            ExtendedConsole.SetActiveLayer(1);
            if (_newMessage == "")
                return;

            if (_newMessage.Length > 112)
                _newMessage = _newMessage.Substring(0, 109) + "...";

            Console.BackgroundColor = Settings.DefaultBackgroundColor;
            ExtendedConsole.VirtualErase(1, 1, 118, 3);

            Console.ForegroundColor = Settings.DefaultForegroundColor;
            ExtendedConsole.VirtualWrite(_newMessage, 6, 3);


            Console.ForegroundColor = Settings.FadingColor;
            ExtendedConsole.VirtualWrite(currentMessage, 4, 2);

            Console.ForegroundColor = Settings.DisabledColor;
            ExtendedConsole.VirtualWrite(currentMessage, 2, 1);

            logTwo = logOne;
            logOne = currentMessage;
            currentMessage = _newMessage;

            Console.BackgroundColor = Settings.DefaultBackgroundColor;
            Console.ForegroundColor = Settings.DefaultForegroundColor;

            if (waitForConfirm)
            {
                ConsoleKey[] _empty = new ConsoleKey[0];
                Helper.GetUserInput(_empty, true);
            }
            else
            {
                System.Threading.Thread.Sleep(Settings.CombatMessageWaitTime);
            }
            ExtendedConsole.Update(1, 1, 118, 3);

        }

        private void DrawEnemyBox(int _position)
        {
            ExtendedConsole.SetActiveLayer(1);
            if (_position < 7 && !Settings.CombatEnemyUIPlacementAlternate)
            {
                ExtendedConsole.VirtualDrawBox(0, 4 + _position * 5, 17, 6);
                if (_position != 0)
                    ExtendedConsole.VirtualDrawHorizontalLine(4 + _position * 5, 1, 15, false);
            }
            else if (_position < 14 && !Settings.CombatEnemyUIPlacementAlternate)
            {
                ExtendedConsole.VirtualDrawBox(103, (4 + (_position - 7) * 5), 17, 6);
                if (_position != 7)
                    ExtendedConsole.VirtualDrawHorizontalLine(4 + (_position - 7) * 5, 104, 15, false);
            }
            else if (_position % 2 == 0 && _position < 14)
            {
                ExtendedConsole.VirtualDrawBox(0, (4 + (_position / 2 * 5)), 17, 6);
                if (_position != 0)
                    ExtendedConsole.VirtualDrawHorizontalLine((4 + (_position / 2 * 5)), 1, 15, false);
            }
            else if (_position < 14)
            {
                ExtendedConsole.VirtualDrawBox(103, (4 + _position / 2 * 5), 17, 6);
                if (_position != 1)
                    ExtendedConsole.VirtualDrawHorizontalLine((4 + _position / 2 * 5), 104, 15, false);
            }
            else
            {
                ShowMessage("ERROR - Can't generate more than 14 enemies statsbox");
            }
        }

        public bool DrawEnemyStats(int _position)
        {
            if (enemies[_position].GetHP() <= 0)
            {
                Repopulate();
                return true; ;
            }
            string _name = enemies[_position].GetName();
            if (_name.Length > 10)
                _name = _name.Substring(0, 10);

            string _shortDesignation = enemies[_position].GetShortDesignation();

            String _HP = enemies[_position].GetHP().ToString();
            while (_HP.Length < 3)
                _HP = " " + _HP;

            String _MaxHP = enemies[_position].GetMaxHP().ToString();
            while (_MaxHP.Length < 3)
                _MaxHP = _MaxHP + " ";

            String _MP = enemies[_position].GetMP().ToString();
            while (_MP.Length < 3)
                _MP = " " + _MP;

            String _MaxMP = enemies[_position].GetMaxMP().ToString();
            while (_MaxHP.Length < 3)
                _MaxMP = " " + _MaxMP;

            if (!Settings.CombatEnemyUIPlacementAlternate)
            {
                ExtendedConsole.SetActiveLayer(1);
                if (_position < 7)
                {
                    Console.ForegroundColor = GetColorForCharacter(_position + allies.Count);
                    ExtendedConsole.VirtualWrite(_name, 2, 5 + _position * 5);

                    Console.BackgroundColor = Settings.CombatMapGridColor1;
                    ExtendedConsole.VirtualWrite(_shortDesignation, 13, 5 + _position * 5);

                    Console.BackgroundColor = Settings.DefaultBackgroundColor;
                    Console.ForegroundColor = Settings.DefaultForegroundColor;
                    ExtendedConsole.VirtualWrite("HP: " + _HP, 2, 6 + _position * 5);
                    ExtendedConsole.VirtualWrite("/ " + _MaxHP, 10, 6 + _position * 5);
                    ExtendedConsole.VirtualWrite("MP: " + _MP, 2, 7 + _position * 5); ;
                    ExtendedConsole.VirtualWrite("/ " + _MaxMP, 10, 7 + _position * 5);
                }
                else if (_position < 14)
                {
                    Console.ForegroundColor = GetColorForCharacter(_position + allies.Count);
                    ExtendedConsole.VirtualWrite(_name, 105, 5 + (_position - 7) * 5);

                    Console.BackgroundColor = Settings.CombatMapGridColor1;
                    ExtendedConsole.VirtualWrite(_shortDesignation, 116, 5 + (_position - 7) * 5);

                    Console.BackgroundColor = Settings.DefaultBackgroundColor;
                    Console.ForegroundColor = Settings.DefaultForegroundColor;
                    ExtendedConsole.VirtualWrite("HP: " + _HP, 105, 6 + (_position - 7) * 5);
                    ExtendedConsole.VirtualWrite("/ " + _MaxHP, 113, 6 + (_position - 7) * 5);
                    ExtendedConsole.VirtualWrite("MP: " + _MP, 105, 7 + (_position - 7) * 5);
                    ExtendedConsole.VirtualWrite("/ " + _MaxMP, 113, 7 + (_position - 7) * 5);
                }
            }
            else if (_position % 2 == 0 && _position < 14)
            {
                Console.ForegroundColor = GetColorForCharacter(_position + allies.Count);
                ExtendedConsole.VirtualWrite(_name, 2, 5 + _position / 2 * 5);

                Console.BackgroundColor = Settings.CombatMapGridColor1;
                ExtendedConsole.VirtualWrite(_shortDesignation, 13, 5 + _position / 2 * 5);

                Console.BackgroundColor = Settings.DefaultBackgroundColor;
                Console.ForegroundColor = Settings.DefaultForegroundColor;
                ExtendedConsole.VirtualWrite("HP: " + _HP, 2, 6 + _position / 2 * 5);
                ExtendedConsole.VirtualWrite("/ " + _MaxHP, 10, 6 + _position / 2 * 5);
                ExtendedConsole.VirtualWrite("MP: " + _MP, 2, 7 + _position / 2 * 5);
                ExtendedConsole.VirtualWrite("/ " + _MaxMP, 10, 7 + _position / 2 * 5);
            }
            else if (_position < 14)
            {
                Console.ForegroundColor = GetColorForCharacter(_position + allies.Count);
                ExtendedConsole.VirtualWrite(_name, 105, 5 + _position / 2 * 5);

                Console.BackgroundColor = Settings.CombatMapGridColor1;
                ExtendedConsole.VirtualWrite(_shortDesignation, 116, 5 + _position / 2 * 5);

                Console.BackgroundColor = Settings.DefaultBackgroundColor;
                Console.ForegroundColor = Settings.DefaultForegroundColor;
                ExtendedConsole.VirtualWrite("HP: " + _HP, 105, 6 + _position / 2 * 5);
                ExtendedConsole.VirtualWrite("/ " + _MaxHP, 113, 6 + _position / 2 * 5);
                ExtendedConsole.VirtualWrite("MP: " + _MP, 105, 7 + _position / 2 * 5);
                ExtendedConsole.VirtualWrite("/ " + _MaxMP, 113, 7 + _position / 2 * 5);
            }
            else
            {
                ShowMessage("ERROR - Can't place more enemies", true);
            }

            Console.BackgroundColor = Settings.DefaultBackgroundColor;
            Console.ForegroundColor = Settings.DefaultForegroundColor;
            return false;
        }

        public bool DrawAllyStats(int _allyPosition)
        {

            if (allies[_allyPosition].GetHP() <= 0)
            {
                Repopulate();
                return true;
            }

            int spaceBetweenCharacters = 1;

            if (allies.Count > 6)
                ShowMessage("ERROR - Can't show more than 6 allies", true);
            else
                spaceBetweenCharacters = (86 - 13 * allies.Count) / (allies.Count + 1);

            int offset = 13 * _allyPosition + spaceBetweenCharacters * (_allyPosition + 1) + 17;


            string _name = allies[_allyPosition].GetName();
            if (_name.Length > 10)
                _name = _name.Substring(0, 10);

            string _shortDesignation = allies[_allyPosition].GetShortDesignation();

            String _HP = allies[_allyPosition].GetHP().ToString();
            while (_HP.Length < 3)
                _HP = " " + _HP;

            String _MaxHP = allies[_allyPosition].GetMaxHP().ToString();
            while (_MaxHP.Length < 3)
                _MaxHP = _MaxHP + " ";

            String _MP = allies[_allyPosition].GetMP().ToString();
            while (_MP.Length < 3)
                _MP = " " + _MP;

            String _MaxMP = allies[_allyPosition].GetMaxMP().ToString();
            while (_MaxHP.Length < 3)
                _MaxMP = " " + _MaxMP;

            ExtendedConsole.SetActiveLayer(1);
            ExtendedConsole.VirtualErase(offset, 34, 13, 4);

            Console.ForegroundColor = GetColorForCharacter(_allyPosition);
            ExtendedConsole.VirtualWrite(_name.Split(' ')[0], offset, 34);

            Console.BackgroundColor = Settings.CombatMapGridColor1;
            ExtendedConsole.VirtualWrite(_shortDesignation, 11 + offset, 34);
            Console.BackgroundColor = Settings.DefaultBackgroundColor;
            Console.ForegroundColor = Settings.DefaultForegroundColor;
            ExtendedConsole.VirtualWrite("HP: " + _HP, offset, 36);
            ExtendedConsole.VirtualWrite("/ " + _MaxHP, 8 + offset, 36);
            ExtendedConsole.VirtualWrite("MP: " + _MP, offset, 37);
            ExtendedConsole.VirtualWrite("/ " + _MaxMP, 8 + offset, 37);
            return false;
        }

        /*public void DrawVirtualPath(int charIndex, int[] path)
        {
            int[] marker = new int[2];
            marker[0] = positionX[charIndex];
            marker[1] = positionY[charIndex];
            for (int i = 0; i < path.Length / 2; i++)
            {
                if (path[i * 2] == 1)
                {
                    if (path[i * 2 + 1] == 1)
                    {
                        marker[0]++;
                        marker[1]++;
                    }
                    else if (path[i * 2 + 1] == 0)
                    {
                        marker[0]++;
                    }
                    if (path[i * 2 + 1] == -1)
                    {
                        marker[0]++;
                        marker[1]--;
                    }
                }
                else if (path[i * 2] == 0)
                {
                    if (path[i * 2 + 1] == 1)
                    {
                        marker[1]++;
                    }
                    else if (path[i * 2 + 1] == 0)
                    {
                    }
                    if (path[i * 2 + 1] == -1)
                    {
                        marker[1]--;
                    }
                }
                else if (path[i * 2] == -1)
                {
                    if (path[i * 2 + 1] == 1)
                    {
                        marker[0]--;
                        marker[1]++;
                    }
                    else if (path[i * 2 + 1] == 0)
                    {
                        marker[0]--;
                    }
                    if (path[i * 2 + 1] == -1)
                    {
                        marker[0]--;
                        marker[1]--;
                    }
                }

                battlefield.SetCorrectBGColorForPosition(17 + marker[0], 5 + marker[1]);
                Console.ForegroundColor = Settings.DefaultForegroundColor;
                ExtendedConsole.VirtualWrite("::", 17 + marker[0] * 2, 5 + marker[1]);

            }
            Console.SetCursorPosition(17 + marker[0] * 2, 5 + marker[1]);
            if (marker[0] == positionX[charIndex] && marker[1] == positionY[charIndex])
            {
                battlefield.SetCorrectBGColorForPosition(positionX[charIndex], positionY[charIndex]);
                ExtendedConsole.VirtualWrite(fighters[charIndex].GetShortDesignation(), 17 + marker[0] * 2, 5 + marker[1]);
            }
            else
            {
                ExtendedConsole.VirtualWrite("[]", 17 + marker[0] * 2, 5 + marker[1]);
            }
            Console.BackgroundColor = Settings.DefaultBackgroundColor;
        }*/

        public void UpdateATB(bool[] affectedByLuck = null)
        {
            if (affectedByLuck == null)
            {
                affectedByLuck = new bool[fighters.Count];
                for (int i = 0; i < affectedByLuck.Length; i++)
                {
                    affectedByLuck[i] = false;
                }
            }

            for (int i = 0; i < allies.Count; i++)
            {
                string _ATB = "";

                if (affectedByLuck[i])
                {
                    _ATB = Settings.ATBLucky;
                }
                else
                {
                    //Calculate x / 11
                    int aTBOn11 = aTBGauge[i] * 11 / 500;

                    //Build string accordingly

                    for (int j = 0; j < aTBOn11 + 1; j++)
                    {
                        if (slowATB[i])
                            _ATB += Settings.SlowATB[j];
                        else if (fastATB[i])
                            _ATB += Settings.QuickATB[j];
                        else
                            _ATB += Settings.ATB[j];
                    }
                    for (int j = aTBOn11; j < 11; j++)
                        _ATB += Settings.EmptyATB[j];
                    if (slowATB[i])
                        _ATB += Settings.SlowATB[12];
                    else if (fastATB[i])
                        _ATB += Settings.QuickATB[12];
                    else
                        _ATB += Settings.ATB[12];
                }

                //Figure the character ATB gauge position and write the new string over it
                int offset = (13 * i) + ((86 - 13 * allies.Count) / (allies.Count + 1) * (i + 1));
                ExtendedConsole.VirtualWrite(_ATB, 17 + offset, 38);

            }

            for (int i = 0; i < enemies.Count; i++)
            {
                string _ATB = "";
                if (affectedByLuck[i])
                {
                    _ATB = Settings.ATBLucky;
                }
                else
                {
                    //Calculate x / 11
                    int aTBOn11 = aTBGauge[i + allies.Count] * 11 / 500;

                    //Build string accordingly
                    for (int j = 0; j < aTBOn11 + 1; j++)
                    {
                        if (slowATB[i])
                            _ATB += Settings.SlowATB[j];
                        else if (fastATB[i])
                            _ATB += Settings.QuickATB[j];
                        else
                            _ATB += Settings.ATB[j];
                    }
                    for (int j = aTBOn11; j < 11; j++)
                        _ATB += Settings.EmptyATB[j];
                    if (slowATB[i])
                        _ATB += Settings.SlowATB[12];
                    else if (fastATB[i])
                        _ATB += Settings.QuickATB[12];
                    else
                        _ATB += Settings.ATB[12];
                }

                //Figure the character ATB gauge position and write the new string over it
                if (i < 5 && !Settings.CombatEnemyUIPlacementAlternate)
                    ExtendedConsole.VirtualWrite(_ATB, 2, i * 5 + 8);
                else if (i < 10 && !Settings.CombatEnemyUIPlacementAlternate)
                    ExtendedConsole.VirtualWrite(_ATB, 105, 8 + (i - 5) * 5);
                else if (i % 2 == 0 && i < 10)
                    ExtendedConsole.VirtualWrite(_ATB, 2, i / 2 * 5 + 8);
                else
                    ExtendedConsole.VirtualWrite(_ATB, 105, 8 + i / 2 * 5);
            }
            ExtendedConsole.Update();
        }

        private void ShowPossibleDirectionChoices(int charIndex, bool[] _possibleMoves, int _remainingMoves = -1, bool _diagonalCostDouble = false)
        {
            Character _movingCharacter = allies[charIndex];

            int spaceBetweenCharacters = (86 - 13 * allies.Count) / (allies.Count + 1);

            if (allies.Count > 6)
            {
                ShowMessage("ERROR - Can't show more than 6 allies");
                spaceBetweenCharacters = 1;
            }

            int offset = (13 * charIndex) + (spaceBetweenCharacters * (charIndex + 1));

            ExtendedConsole.VirtualErase(17 + offset, 35, 13, 3);

            if ((_possibleMoves[0] && !_diagonalCostDouble) || (_possibleMoves[0] && _diagonalCostDouble && _remainingMoves > 1))
            {
                if (_diagonalCostDouble)
                    Console.ForegroundColor = Settings.WarningColor;

                ExtendedConsole.VirtualWrite("7", 21 + offset, 35);
                Console.ForegroundColor = Settings.DefaultForegroundColor;
            }

            if (_possibleMoves[1])
                ExtendedConsole.VirtualWrite("8", 23 + offset, 35);

            if ((_possibleMoves[2] && !_diagonalCostDouble) || (_possibleMoves[2] && _diagonalCostDouble && _remainingMoves > 1))
            {
                if (_diagonalCostDouble)
                    Console.ForegroundColor = Settings.WarningColor;

                ExtendedConsole.VirtualWrite("9", 25 + offset, 35);
                Console.ForegroundColor = Settings.DefaultForegroundColor;
            }

            if (_possibleMoves[3])
                ExtendedConsole.VirtualWrite("4", 21 + offset, 36);

            Console.BackgroundColor = Settings.DefaultSelectionColor;
            ExtendedConsole.VirtualWrite("+", 23 + offset, 36);
            Console.BackgroundColor = Settings.DefaultBackgroundColor;

            if (_possibleMoves[4])
                ExtendedConsole.VirtualWrite("6", 25 + offset, 36);

            if ((_possibleMoves[5] && !_diagonalCostDouble) || (_possibleMoves[5] && _diagonalCostDouble && _remainingMoves > 1))
            {
                if (_diagonalCostDouble)
                    Console.ForegroundColor = Settings.WarningColor;

                ExtendedConsole.VirtualWrite("1", 21 + offset, 37);
                Console.ForegroundColor = Settings.DefaultForegroundColor;
            }

            if (_possibleMoves[6])
                ExtendedConsole.VirtualWrite("2", 23 + offset, 37);

            if ((_possibleMoves[7] && !_diagonalCostDouble) || (_possibleMoves[7] && _diagonalCostDouble && _remainingMoves > 1))
            {
                if (_diagonalCostDouble)
                    Console.ForegroundColor = Settings.WarningColor;

                ExtendedConsole.VirtualWrite("3", 25 + offset, 37);
                Console.ForegroundColor = Settings.DefaultForegroundColor;
            }

            if (_remainingMoves != -1)
            {
                ExtendedConsole.VirtualWrite("   Moves:    ", 17 + offset, 38);
                ExtendedConsole.VirtualWrite(_remainingMoves.ToString(), 26 + offset, 38);
            }
        }

        private int NextCharacter()
        {
            bool[] boostedByLuck = new bool[fighters.Count];
            bool andAnHalf = false;
            while (true)
            {
                Random rand = new Random();
                for (int i = 0; i < fighters.Count; i++)
                {
                    if (!andAnHalf && slowATB[i] || !slowATB[i])
                        aTBGauge[i] += fighters[i].panache;

                    if (andAnHalf)
                    {
                        if (fastATB[i])
                            aTBGauge[i] += fighters[i].panache;

                        //Check for luck boost
                        boostedByLuck[i] = false;
                        int currentvalue = rand.Next(0, 200) + 10;
                        if (fighters[i].fortune > currentvalue)
                        {
                            boostedByLuck[i] = true;
                            aTBGauge[i] += fighters[i].panache * 5;
                        }
                    }
                }
                andAnHalf = !andAnHalf;
                ExtendedConsole.SetActiveLayer(1);
                UpdateATB(boostedByLuck);
                System.Threading.Thread.Sleep(Settings.ATBUpdateWaitTime);

                foreach (bool b in boostedByLuck)
                {
                    if (b)
                        System.Threading.Thread.Sleep(Settings.ATBUpdateWaitTime * 32);
                }

                int j = 0;
                foreach (Character dude in fighters)
                {
                    if (aTBGauge[j] >= 500)
                    {
                        aTBGauge[j] -= 500;
                        fastATB[j] = false;
                        slowATB[j] = false;
                        return j;
                    }
                    j++;
                }
            }
        }

        private void Act(int charIndex)
        {
            fastATB[charIndex] = false;
            slowATB[charIndex] = false;

            //Highlight Currently active position; 
            Character dude = fighters[charIndex];
            if (charIndex < allies.Count)
                Console.BackgroundColor = Settings.DefaultSelectionColor;
            else
                Console.BackgroundColor = Settings.CombatEnemyColor;
            ExtendedConsole.VirtualWrite(dude.GetShortDesignation(), 17 + positionX[charIndex] * 2, 5 + positionY[charIndex]);
            Console.BackgroundColor = Settings.DefaultBackgroundColor;

            ShowMessage(dude.GetName() + "'s turn");

            if (dude.GetPlayerControl())
            {
                int _xMenuStartPosition = 17 + (13 * charIndex) + (((86 - 13 * allies.Count) / (allies.Count + 1)) * (charIndex + 1));

                string[] _options = new string[4];
                _options[0] = " Move";
                _options[1] = " Attack";
                _options[2] = " " + fighters[charIndex].GetSkillType();
                _options[3] = " Items";

                bool actionCompleted = false;
                bool moveCompleted = false;

                while (!actionCompleted || !moveCompleted)
                {
                    bool[] _disabledOptions = new bool[4];
                    if (moveCompleted)
                        _disabledOptions[0] = true;
                    if (actionCompleted)
                    {
                        _disabledOptions[1] = true;

                        _disabledOptions[2] = true;

                        _disabledOptions[3] = true;
                    }

                    int choice = Helper.ShowMenuAndGetChoice(_options, _xMenuStartPosition, 35, 13, 1, _disabledOptions);
                    switch (choice)
                    {
                        case 1:     //Move
                            bool[,] possibleMoves = ShowPossibleMoves(charIndex);
                            Stack<Position> path = InputMovements(charIndex, possibleMoves);
                            moveCompleted = true;
                            break;
                        case 2:     //Attack
                            int _attackDirection = ChooseAttackDirection(charIndex);
                            LaunchAttack(charIndex, _attackDirection, fighters[charIndex].getWeaponAnim(_attackDirection));
                            actionCompleted = true;
                            break;
                        case 3:     //Skills
                            actionCompleted = true;
                            break;
                        case 4:     //Items
                            actionCompleted = true;
                            break;
                        case -1:     //Skip
                            if (!actionCompleted)
                                fastATB[charIndex] = true;
                            actionCompleted = true;
                            moveCompleted = true;
                            break;
                    }
                }


                for (int i = 0; i < allies.Count; i++)
                {
                    if (dude == allies[i])
                        DrawAllyStats(i);
                }
            }
            else //if player isn't controlling this character
            {

            }

            //Redraw CharacterPosition on battlemap
            Console.ForegroundColor = GetColorForCharacter(charIndex);
            battlefield.SetCorrectBGColorForPosition(positionX[charIndex], positionY[charIndex]);
            ExtendedConsole.VirtualWrite(dude.GetShortDesignation(), 17 + positionX[charIndex] * 2, 5 + positionY[charIndex]);

            Console.ForegroundColor = Settings.DefaultForegroundColor;
            Console.BackgroundColor = Settings.DefaultBackgroundColor;

            ExtendedConsole.Update();
        }

        bool[,] ShowPossibleMoves(int charIndex)
        {
            bool[,] possibleMoves = battlefield.ShowPossibleMoves(positionX[charIndex], positionY[charIndex], fighters[charIndex].getMovement());
            ExtendedConsole.Update();
            return possibleMoves;
        }

        private Stack<Position> InputMovements(int charIndex, bool[,] possibleMoves)
        {
            Position currentPosition = new Position(positionX[charIndex], positionY[charIndex]);
            Stack<Position> path = new Stack<Position>();

            bool pathIsChosen = false;
            while (!pathIsChosen)
            {
                ConsoleKeyInfo choice = Helper.GetUserInput(new ConsoleKey[] {
                    ConsoleKey.NumPad1,
                    ConsoleKey.NumPad2,
                    ConsoleKey.NumPad3,
                    ConsoleKey.NumPad4,
                    ConsoleKey.NumPad6,
                    ConsoleKey.NumPad7,
                    ConsoleKey.NumPad8,
                    ConsoleKey.NumPad9,
                    ConsoleKey.UpArrow,
                    ConsoleKey.DownArrow,
                    ConsoleKey.LeftArrow,
                    ConsoleKey.RightArrow }, true, true);

                Position nextPosition = currentPosition;

                switch (choice.Key)
                {
                    case ConsoleKey.NumPad1:
                        nextPosition.x -= 1;
                        nextPosition.y += 1;
                        break;
                    case ConsoleKey.NumPad2:
                    case ConsoleKey.DownArrow:
                        break;
                    case ConsoleKey.NumPad3:
                        nextPosition.x += 1;
                        nextPosition.y += 1;
                        break;
                    case ConsoleKey.NumPad4:
                    case ConsoleKey.LeftArrow:
                        break;
                    case ConsoleKey.NumPad6:
                    case ConsoleKey.RightArrow:
                        break;
                    case ConsoleKey.NumPad7:
                        nextPosition.x -= 1;
                        nextPosition.y -= 1;
                        break;
                    case ConsoleKey.NumPad8:
                    case ConsoleKey.UpArrow:
                        break;
                    case ConsoleKey.NumPad9:
                        nextPosition.x += 1;
                        nextPosition.y -= 1;
                        break;
                    case ConsoleKey.Enter:
                        pathIsChosen = true;
                        break;
                }

                if (possibleMoves[nextPosition.x, nextPosition.y])
                {
                    path = battlefield.GetPath(new Position(positionX[charIndex], positionY[charIndex]), nextPosition);
                    battlefield.DrawPath(path, nextPosition);
                    currentPosition = nextPosition;
                }
            }
            return path;

            /*Character movingCharacter = fighters[charIndex];
            int remainingMoves = movingCharacter.getMovement();
            int numberOfInputs = 0;
            bool diagonalCostDouble = false;
            int virtualPositionX = positionX[charIndex];
            int virtualPositionY = positionY[charIndex];
            int[] _path = new int[fighters[charIndex].getMovement() * 2];

            while (remainingMoves > 0)
            {
                bool[] possibleMoves = GetPossibleMoves(virtualPositionX, virtualPositionY);
                ShowPossibleDirectionChoices(charIndex, possibleMoves, remainingMoves, diagonalCostDouble);

                List<ConsoleKey> validInputs = new List<ConsoleKey>();

                if (possibleMoves[0])
                {
                    if ((remainingMoves > 1 && diagonalCostDouble) || !diagonalCostDouble)
                        validInputs.Add(ConsoleKey.NumPad7);
                }
                if (possibleMoves[1])
                {
                    validInputs.Add(ConsoleKey.NumPad8);
                    validInputs.Add(ConsoleKey.UpArrow);
                }
                if (possibleMoves[2])
                {
                    if ((remainingMoves > 1 && diagonalCostDouble) || !diagonalCostDouble)
                        validInputs.Add(ConsoleKey.NumPad9);
                }
                if (possibleMoves[3])
                {
                    validInputs.Add(ConsoleKey.NumPad4);
                    validInputs.Add(ConsoleKey.LeftArrow);
                }
                if (possibleMoves[4])
                {
                    validInputs.Add(ConsoleKey.NumPad6);
                    validInputs.Add(ConsoleKey.RightArrow);
                }
                if (possibleMoves[5])
                {
                    if ((remainingMoves > 1 && diagonalCostDouble) || !diagonalCostDouble)
                        validInputs.Add(ConsoleKey.NumPad1);
                }
                if (possibleMoves[6])
                {
                    validInputs.Add(ConsoleKey.NumPad2);
                    validInputs.Add(ConsoleKey.DownArrow);
                }
                if (possibleMoves[7])
                {
                    if ((remainingMoves > 1 && diagonalCostDouble) || !diagonalCostDouble)
                        validInputs.Add(ConsoleKey.NumPad3);
                }

                ConsoleKeyInfo choice = Helper.GetUserInput(validInputs.ToArray(), true, true);

                switch (choice.Key)
                {
                    case ConsoleKey.NumPad7:
                        if (diagonalCostDouble)
                        {
                            remainingMoves--;
                        }

                        diagonalCostDouble = !diagonalCostDouble;
                        _path[numberOfInputs * 2] = -1;
                        _path[numberOfInputs * 2 + 1] = -1;
                        virtualPositionX--;
                        virtualPositionY--;
                        remainingMoves--;
                        break;
                    case ConsoleKey.NumPad8:
                    case ConsoleKey.UpArrow:
                        _path[numberOfInputs * 2] = 0;
                        _path[numberOfInputs * 2 + 1] = -1;
                        virtualPositionY--;
                        remainingMoves--;
                        break;
                    case ConsoleKey.NumPad9:
                        if (diagonalCostDouble)
                        {
                            remainingMoves--;
                        }
                        diagonalCostDouble = !diagonalCostDouble;
                        _path[numberOfInputs * 2] = 1;
                        _path[numberOfInputs * 2 + 1] = -1;
                        virtualPositionX++;
                        virtualPositionY--;
                        remainingMoves--;
                        break;
                    case ConsoleKey.NumPad4:
                    case ConsoleKey.LeftArrow:
                        _path[numberOfInputs * 2] = -1;
                        _path[numberOfInputs * 2 + 1] = 0;
                        virtualPositionX--;
                        remainingMoves--;
                        break;
                    case ConsoleKey.NumPad6:
                    case ConsoleKey.RightArrow:
                        _path[numberOfInputs * 2] = 1;
                        _path[numberOfInputs * 2 + 1] = 0;
                        virtualPositionX++;
                        remainingMoves--;
                        break;
                    case ConsoleKey.NumPad1:
                        if (diagonalCostDouble)
                        {
                            remainingMoves--;
                        }
                        diagonalCostDouble = !diagonalCostDouble;
                        _path[numberOfInputs * 2] = -1;
                        _path[numberOfInputs * 2 + 1] = 1;
                        virtualPositionX--;
                        virtualPositionY++;
                        remainingMoves--;
                        break;
                    case ConsoleKey.NumPad2:
                    case ConsoleKey.DownArrow:
                        _path[numberOfInputs * 2] = 0;
                        _path[numberOfInputs * 2 + 1] = 1;
                        virtualPositionY++;
                        remainingMoves--;
                        break;
                    case ConsoleKey.NumPad3:
                        if (diagonalCostDouble)
                        {
                            remainingMoves--;
                        }
                        diagonalCostDouble = !diagonalCostDouble;
                        _path[numberOfInputs * 2] = 1;
                        _path[numberOfInputs * 2 + 1] = 1;
                        virtualPositionX++;
                        virtualPositionY++;
                        remainingMoves--;
                        break;
                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
                    case ConsoleKey.NumPad5:
                        remainingMoves = 0;
                        break;
                    case ConsoleKey.Escape:
                    case ConsoleKey.Backspace:
                        virtualPositionX = positionX[charIndex];
                        virtualPositionY = positionY[charIndex];
                        for (int i = 0; i < _path.Length / 2; i++)
                            if (_path[i * 2] != 0 || _path[i * 2 + 1] != 0)
                            {
                                battlefield.UpdatePosition(virtualPositionX + _path[i * 2], virtualPositionY + _path[i * 2 + 1]);
                                virtualPositionX += _path[i * 2];
                                virtualPositionY += _path[i * 2 + 1];
                            }

                        remainingMoves = movingCharacter.getMovement();
                        numberOfInputs = 0;
                        diagonalCostDouble = false;
                        virtualPositionX = positionX[charIndex];
                        virtualPositionY = positionY[charIndex];
                        _path = new int[fighters[charIndex].getMovement() * 2];
                        break;
                    default:
                        break;
                }
                DrawVirtualPath(charIndex, _path);
                numberOfInputs++;
            }

            List<int> _croppedPath = _path.ToList();

            while (true)
            {
                if (_croppedPath.Count == 0)
                    break;
                if (_croppedPath[_croppedPath.Count - 1] == 0 && _croppedPath[_croppedPath.Count - 2] == 0)
                {
                    _croppedPath.RemoveAt(_croppedPath.Count - 1);
                    _croppedPath.RemoveAt(_croppedPath.Count - 1);
                }
                else
                {
                    break;
                }
            }
            Move(charIndex, _croppedPath.ToArray(), !fighters[charIndex].GetPlayerControl());*/
        }

        /*private bool[] GetPossibleMoves(int x, int y)
        {
            * 012
             * 3 4
             * 567 
            bool[] possiblemoves = new bool[8];
            if (y - 1 > 0)
            {
                if (x - 1 > 0)
                {
                    if (fightersGridPosition[x - 1, y - 1] == null)
                        if (battlefield.GetTerrainPassable(x - 1, y - 1))
                            possiblemoves[0] = true;
                }

                if (fightersGridPosition[x, y - 1] == null)
                    if (battlefield.GetTerrainPassable(x, y - 1))
                        possiblemoves[1] = true;

                if (x + 1 < 43)
                {
                    if (fightersGridPosition[x + 1, y - 1] == null)
                        if (battlefield.GetTerrainPassable(x + 1, y - 1))
                            possiblemoves[2] = true;
                }
            }

            if (x - 1 > 0)
            {
                if (fightersGridPosition[x - 1, y] == null)
                    if (battlefield.GetTerrainPassable(x - 1, y))
                        possiblemoves[3] = true;
            }

            if (x + 1 < 43)
            {
                if (fightersGridPosition[x + 1, y] == null)
                    if (battlefield.GetTerrainPassable(x + 1, y))
                        possiblemoves[4] = true;
            }

            if (y + 1 < 28)
            {
                if (x - 1 >= 0)
                {
                    if (fightersGridPosition[x - 1, y + 1] == null)
                        if (battlefield.GetTerrainPassable(x - 1, y + 1))
                            possiblemoves[5] = true;
                }

                if (fightersGridPosition[x, y + 1] == null)
                    if (battlefield.GetTerrainPassable(x, y + 1))
                        possiblemoves[6] = true;

                if (x + 1 < 43)
                {
                    if (fightersGridPosition[x + 1, y + 1] == null)
                        if (battlefield.GetTerrainPassable(x + 1, y + 1))
                            possiblemoves[7] = true;
                }
            }
            return possiblemoves;
        }*/


        /*
                private void Move(int charIndex, int[] path, bool withTrail = true)
                {
                    int[] positionsToUpdate = new int[path.Length];
                    int[] virtualPosition = new int[2] { positionX[charIndex], positionY[charIndex] };

                    for (int i = 0; i < path.Length / 2; i++)
                    {
                        if (withTrail)
                        {
                            Array.Copy(virtualPosition, 0, positionsToUpdate, i * 2, 1);
                            Array.Copy(virtualPosition, 1, positionsToUpdate, i * 2 + 1, 1);
                            Console.ForegroundColor = Settings.DefaultForegroundColor;
                            if (i == 0)
                                battlefield.UpdatePosition(positionX[charIndex], positionY[charIndex], "[]");
                            else
                                battlefield.UpdatePosition(virtualPosition[0], virtualPosition[1], "::");
                        }
                        else
                        {
                            battlefield.UpdatePosition(virtualPosition[0], virtualPosition[1]);
                        }

                        if (path[i * 2] == -1)
                            virtualPosition[0] -= 1;
                        if (path[i * 2] == 1)
                            virtualPosition[0] += 1;

                        if (path[i * 2 + 1] == -1)
                            virtualPosition[1] -= 1;
                        if (path[i * 2 + 1] == 1)
                            virtualPosition[1] += 1;

                        Console.ForegroundColor = GetColorForCharacter(charIndex);
                        battlefield.SetCorrectBGColorForPosition(virtualPosition[0], virtualPosition[1]);
                        battlefield.UpdatePosition(virtualPosition[0], virtualPosition[1], fighters[charIndex].GetShortDesignation());

                        System.Threading.Thread.Sleep(Settings.CombatMoveWaitTime);
                    }

                    fightersGridPosition[positionX[charIndex], positionY[charIndex]] = null;
                    positionX[charIndex] = virtualPosition[0];
                    positionY[charIndex] = virtualPosition[1];
                    fightersGridPosition[positionX[charIndex], positionY[charIndex]] = fighters[charIndex];

                    if (withTrail)
                    {
                        Console.ForegroundColor = Settings.DefaultForegroundColor;
                        for (int i = 0; i < positionsToUpdate.Length / 2; i++)
                        {
                            if (i + 1 < positionsToUpdate.Length / 2)
                                battlefield.UpdatePosition(positionsToUpdate[i * 2 + 2], positionsToUpdate[i * 2 + 3], "[]");

                            battlefield.UpdatePosition(positionsToUpdate[i * 2], positionsToUpdate[i * 2 + 1]);
                            System.Threading.Thread.Sleep(Settings.CombatMoveWaitTime);
                        }
                    }
                    Console.BackgroundColor = Settings.DefaultBackgroundColor;
                }*/

        private int ChooseAttackDirection(int _attackerIndex)
        {
            ShowPossibleDirectionChoices(_attackerIndex, new bool[8] { true, true, true, true, true, true, true, true });
            ConsoleKeyInfo _input = Helper.GetUserInput(new ConsoleKey[12] {
                    ConsoleKey.NumPad1,
                    ConsoleKey.NumPad2,
                    ConsoleKey.UpArrow,
                    ConsoleKey.NumPad3,
                    ConsoleKey.NumPad4,
                    ConsoleKey.LeftArrow,
                    ConsoleKey.NumPad6,
                    ConsoleKey.RightArrow,
                    ConsoleKey.NumPad7,
                    ConsoleKey.NumPad8,
                    ConsoleKey.DownArrow,
                    ConsoleKey.NumPad9 }, false, true);


            int _attackDirection = -1;

            switch (_input.Key)
            {
                case ConsoleKey.NumPad7:
                    _attackDirection = 7;
                    break;
                case ConsoleKey.NumPad8:
                case ConsoleKey.UpArrow:
                    _attackDirection = 8;
                    break;
                case ConsoleKey.NumPad9:
                    _attackDirection = 9;
                    break;
                case ConsoleKey.NumPad4:
                case ConsoleKey.LeftArrow:
                    _attackDirection = 4;
                    break;
                case ConsoleKey.NumPad6:
                case ConsoleKey.RightArrow:
                    _attackDirection = 6;
                    break;
                case ConsoleKey.NumPad1:
                    _attackDirection = 1;
                    break;
                case ConsoleKey.NumPad2:
                case ConsoleKey.DownArrow:
                    _attackDirection = 2;
                    break;
                case ConsoleKey.NumPad3:
                    _attackDirection = 3;
                    break;
                case ConsoleKey.Escape:
                case ConsoleKey.Backspace:
                    break;
                default:
                    break;
            }

            return _attackDirection;
        }

        private void LaunchAttack(int charIndex, int _direction, string[][] _animation)
        {
            fighters[charIndex].setColorForAttack();

            switch (_direction)
            {
                case 1:
                    DrawAttack(charIndex, _animation, -1, 1);
                    break;
                case 2:
                    DrawAttack(charIndex, _animation, 0, 1);
                    break;
                case 3:
                    DrawAttack(charIndex, _animation, 1, 1);
                    break;
                case 4:
                    DrawAttack(charIndex, _animation, -1, 0);
                    break;
                case 6:
                    DrawAttack(charIndex, _animation, 1, 0);
                    break;
                case 7:
                    DrawAttack(charIndex, _animation, -1, -1);
                    break;
                case 8:
                    DrawAttack(charIndex, _animation, 0, -1);
                    break;
                case 9:
                    DrawAttack(charIndex, _animation, 1, -1);
                    break;
                default:
                    ShowMessage("ERROR: \"LaunchAttack()\" recieved out of bound attack direction value");
                    break;
            }
        }

        private void DrawAttack(int charIndex, string[][] _animation, int stepX, int stepY)
        {
            /*List<Character> charactersDamagedByAttack = new List<Character>();
            List<int> _positionsToUpdate = new List<int>();

            for (int i = 0; i < _animation.Length; i++)   // for each frames in _animation
            {
                for (int j = 0; j < _animation[i].Length; j++) // for each horizontal line in frame
                {
                    if (_animation[i][j] == null)
                        continue;

                    string[] _currentLine = _animation[i][j].Split('&');

                    //ensure there's an odd number of tile info
                    if ((_currentLine.Length) % 2 == 0)
                    {
                        string[] _updatedLine = new string[_currentLine.Length + 1];
                        int k = 0;
                        foreach (string caseData in _currentLine)
                        {
                            _updatedLine[k] = caseData;
                            k++;
                        }
                        _currentLine = _updatedLine;
                    }

                    //  Long code to find where each tile info will be drawn
                    //  Based on the currentline middle tile info position
                    for (int k = 0; k < _currentLine.Length; k++) //for each tile info in _currentline
                    {
                        int _offsetX = 0;
                        int _offsetY = 0;

                        int _sideOffsetX = 0;
                        int _sideOffsetY = 0;

                        int lineCenterCase = (_currentLine.Length - 1) / 2;

                        if (stepX < 0)
                        {
                            if (stepY == 0) // 4
                            {
                                _sideOffsetY = k - lineCenterCase;
                            }
                            else if (stepY > 0) // 1
                            {
                                /*to allow full tile coverage, diagonals attack are drawn at  
                                a 90 degree anglefrom the current middle tile info position
                                if (k > lineCenterCase)
                                    _sideOffsetX = k - lineCenterCase;
                                else if (k < lineCenterCase)
                                    _sideOffsetY = k - lineCenterCase;
                            }
                            else // 7
                            {
                                if (k > lineCenterCase)
                                    _sideOffsetY = k - lineCenterCase;
                                else if (k < lineCenterCase)
                                    _sideOffsetX = -(k - lineCenterCase);
                            }
                        }
                        else if (stepX > 0)
                        {
                            if (stepY == 0) //6
                            {
                                _sideOffsetY = -(k - lineCenterCase);
                            }
                            else if (stepY > 0) //3
                            {
                                if (k > lineCenterCase)
                                    _sideOffsetY = -(k - lineCenterCase);
                                else if (k < lineCenterCase)
                                    _sideOffsetX = k - lineCenterCase;
                            }
                            else //9
                            {
                                if (k > lineCenterCase)
                                    _sideOffsetX = -(k - lineCenterCase);
                                else if (k < lineCenterCase)
                                    _sideOffsetY = -(k - lineCenterCase);
                            }
                        }
                        else
                        {
                            if (stepY < 0) //8
                            {
                                _sideOffsetX = -(k - lineCenterCase);
                            }
                            else if (stepY > 0) //2
                            {
                                _sideOffsetX = k - lineCenterCase;
                            }
                            else
                                ShowMessage("ERROR: \"DrawAttack()\" recieved no attack direction !", true);
                        }

                        if (stepX < 0)
                            _offsetX = -j - 1;
                        else if (stepX > 0)
                            _offsetX = j + 1;

                        if (stepY < 0)
                            _offsetY = -j - 1;
                        else if (stepY > 0)
                            _offsetY = j + 1;

                        int _positionX = positionX[charIndex] + _offsetX + _sideOffsetX;
                        int _positionY = positionY[charIndex] + _offsetY + _sideOffsetY;

                        _positionsToUpdate.Add(_positionX);
                        _positionsToUpdate.Add(_positionY);
                        if (fightersGridPosition[_positionX, _positionY] != null)
                        {
                            foreach (Character dude in fighters)
                            {
                                if (dude == fightersGridPosition[_positionX, _positionY])
                                {
                                    charactersDamagedByAttack.Add(dude);
                                }
                            }
                        }
                        battlefield.UpdatePosition(_positionX, _positionY, _currentLine[k]);
                    }
                }
                System.Threading.Thread.Sleep(Settings.CombatAnimationSpeed);   //on each frame
            }

            for (int i = 0; i < _positionsToUpdate.Count / 2; i++)
            {

                for (int j = 0; j < fighters.Count; j++)
                {
                    Character dude = fighters[j];
                    if (dude == fightersGridPosition[_positionsToUpdate[i * 2], _positionsToUpdate[i * 2 + 1]])
                    {
                        Console.ForegroundColor = GetColorForCharacter(j);
                    }
                }
                battlefield.UpdatePosition(_positionsToUpdate[i * 2], _positionsToUpdate[i * 2 + 1], "");
            }

            for (int i = 0; i < charactersDamagedByAttack.Count; i++)
                for (int j = 0; j < fighters.Count; j++)
                    if (fighters[j] == charactersDamagedByAttack[i])
                        charactersDamagedByAttack[i].Damage(fighters[charIndex].GetBasicAttackDamage(), fighters[charIndex].GetBasicAttackType(), charIndex, j, this, allies.Count);
            */
        }

        private void Repopulate()
        {
            List<Character> _allies = new List<Character>();
            List<Character> _enemies = new List<Character>();
            fighters = new List<Character>();
            int[] _aTBGauge = new int[aTBGauge.Length];
            bool[] _fastATB = new bool[fastATB.Length];
            bool[] _slowATB = new bool[slowATB.Length];
            int[] _positionX = new int[positionX.Length];
            int[] _positionY = new int[positionY.Length];
            int i = 0;
            int j = 0;
            foreach (Character dude in allies)
            {
                if (dude.GetHP() > 0)
                {
                    _allies.Add(dude);
                    fighters.Add(dude);
                    _aTBGauge[i] = aTBGauge[j];
                    _fastATB[i] = fastATB[j];
                    _slowATB[i] = slowATB[j];
                    _positionX[i] = positionX[j];
                    _positionY[i] = positionY[j];

                    i++;
                }
                else
                    fightersGridPosition[positionX[j], positionY[j]] = null;

                j++;
            }

            foreach (Character dude in enemies)
            {
                if (dude.GetHP() > 0)
                {
                    _enemies.Add(dude);
                    fighters.Add(dude);
                    _aTBGauge[i] = aTBGauge[j];
                    _fastATB[i] = fastATB[j];
                    _slowATB[i] = slowATB[j];
                    _positionX[i] = positionX[j];
                    _positionY[i] = positionY[j];
                    i++;
                }
                else
                    fightersGridPosition[positionX[j], positionY[j]] = null;

                j++;
            }
            allies = _allies;
            enemies = _enemies;
            aTBGauge = _aTBGauge;
            fastATB = _fastATB;
            slowATB = _slowATB;
            positionX = _positionX;
            positionY = _positionY;

            if (allies.Count == 0)
            {
                GameOverScreen _gameOver = new GameOverScreen();
                _gameOver.Start();
            }

            if (enemies.Count == 0)
                combatIsOngoing = false;

            ResetInterface();
        }

        public int GetCharIndexAtGridPosition(int x, int y)
        {
            if (fightersGridPosition[x, y] == null)
                return -1;

            for (int i = 0; i < fighters.Count; i++)
            {
                if (fightersGridPosition[x, y] == fighters[i])
                    return i;
            }

            return -1;
        }

        public string GetShortDesignationAtPosition(int _x, int _y)
        {
            if (fightersGridPosition[_x, _y] == null)
                return "";
            return fightersGridPosition[_x, _y].GetShortDesignation();
        }

        public ConsoleColor GetColorForCharacter(int charIndex)
        {
            if (charIndex < allies.Count)
            {
                if (!allies[charIndex].summonned && allies[charIndex].GetPlayerControl())
                    return Settings.CombatControlledAllyColor;
                else if (allies[charIndex].summonned && allies[charIndex].GetPlayerControl())
                    return Settings.CombatControlledSummonedAllyColor;
                else if (allies[charIndex].summonned && !allies[charIndex].GetPlayerControl())
                    return Settings.CombatSummonedAllyColor;
                else
                    return Settings.CombatUncontrolledAllyColor;
            }
            else
            {
                if (!fighters[charIndex].summonned)
                    return Console.ForegroundColor = Settings.CombatEnemyColor;
                else
                    return Console.ForegroundColor = Settings.CombatSummonedEnemyColor;
            }
        }
    }
}
