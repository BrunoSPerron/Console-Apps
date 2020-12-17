using System;
using System.Collections.Generic;
using System.Text;

namespace TacticRoguelikeRpg
{

    class Settings
    {
        public static bool animatedMainMenu = true;
        public static float MenuOpeningSpeedMult { get; set; } = 1;
        
        public static ConsoleColor UILineColor { get; set; } = ConsoleColor.DarkMagenta;
        public static ConsoleColor DefaultBackgroundColor { get; set; } = ConsoleColor.Black;
        public static ConsoleColor DefaultForegroundColor { get; set; } = ConsoleColor.White;
        public static ConsoleColor FadingColor { get; set; } = ConsoleColor.Gray;
        public static ConsoleColor DisabledColor { get; set; } = ConsoleColor.DarkGray;
        public static ConsoleColor DefaultSelectionColor { get; set; } = ConsoleColor.DarkGreen;
        public static ConsoleColor CombatMapGridColor1 { get; set; } = ConsoleColor.DarkBlue;
        public static ConsoleColor CombatMapGridColor2 { get; set; } = ConsoleColor.Black;
        public static ConsoleColor CombatMoveGridColor1 { get; set; } = ConsoleColor.DarkMagenta;
        public static ConsoleColor CombatMoveGridColor2 { get; set; } = ConsoleColor.Magenta;
        public static ConsoleColor CombatControlledAllyColor { get; set; } = ConsoleColor.Green;
        public static ConsoleColor CombatUncontrolledAllyColor { get; set; } = ConsoleColor.Yellow;
        public static ConsoleColor CombatSummonedAllyColor { get; set; } = ConsoleColor.Cyan;
        public static ConsoleColor CombatControlledSummonedAllyColor { get; set; } = ConsoleColor.Blue;
        public static ConsoleColor CombatEnemyColor { get; set; } = ConsoleColor.Red;
        public static ConsoleColor CombatSummonedEnemyColor { get; set; } = ConsoleColor.Magenta;
        public static ConsoleColor WarningColor { get; set; } = ConsoleColor.DarkYellow;
        public static ConsoleColor PathColor { get; set; } = ConsoleColor.White;
        public static bool CombatEnemyUIPlacementAlternate { get; set; } = true;
        public static string EmptyATB { get; set; } = "█             █";
        public static string ATB { get; set; } = "█▒▒▒▒▒▒▒▒▒▒▒█";
        public static string QuickATB { get; set; } = "█░░░░░░░░░░░█";
        public static string SlowATB { get; set; } = "█▓▓▓▓▓▓▓▓▓▓▓█";
        public static string ATBLucky { get; set; } = "▓▒░ =^_^= ░▒▓";
        public static int CombatMoveWaitTime { get; set; } = 100;
        public static int CombatMessageWaitTime { get; set; } = 150;
        public static int ATBUpdateWaitTime { get; set; } = 3;
        public static int CombatAnimationSpeed { get; set; } = 100;

        public static void InitializeSettings()
        {
            Console.CursorVisible = false;
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            Console.SetWindowSize(Console.WindowWidth + 1, Console.WindowHeight);
            CorrectPotentialErrors();
        }

        private static void CorrectPotentialErrors()
        {
            if (EmptyATB.Length < 13)
                EmptyATB = "█Er:SETTINGS█";
            if (ATB.Length < 13)
                ATB = "█Er:SETTINGS█";
            if (QuickATB.Length < 13)
                QuickATB += "█Er:SETTINGS█";
            while (SlowATB.Length < 13)
                SlowATB += "█Er:SETTINGS█";
        }
    }
}



