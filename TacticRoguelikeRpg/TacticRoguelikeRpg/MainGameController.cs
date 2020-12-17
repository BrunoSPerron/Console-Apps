using System;
using System.Collections.Generic;
using System.Text;

namespace TacticRoguelikeRpg
{
    enum GameScreen
    {
        MainMenu = 0,
        Load = 1,
        NewGame = 2,
        Settings = 3,
        CharacterCreation = 10
    }


    class MainGameController
    {
        public MainGameController()
        {
            WorldData.Initialize();
            MainMenu main = new MainMenu();
            GameScreen nextScreen = main.Start();

            switch (nextScreen)
            {
                case GameScreen.NewGame:
                    NewGame _new = new NewGame();
                    CharacterCreation _creation = new CharacterCreation();
                    _creation.Start();
                    break;
                case GameScreen.Settings:
                    //CombatTest();
                    break;

            }
            //Weapon Knife = new Weapon();
           
        }

        private void CombatTest()
        {
            
            List<Character> party = new List<Character>();
            Character hero = new Character();
            party.Add(hero);
            hero.SetName("Tay Zonday");
            hero.InitiateAsHero(1);
            hero.setMovement(5);
            for (int i = 2; i < 0; i++)
            {
                Character newAlly = new Character();
                newAlly.InitiateAsCreature(2);
                newAlly.SetPlayerControl(false);
                party.Add(newAlly);
            }

            Character[] enemies = new Character[10];

            for (int i = 0; i < enemies.Length; i++)
            {
                Character newEnemy = new Character();
                newEnemy.InitiateAsCreature(1);
                enemies[i] = newEnemy;
            }

            Combat fight = new Combat(party, enemies);

            Console.Clear();
            Console.WriteLine("gratz...");
            System.Threading.Thread.Sleep(1000);
        }
    }
}
