using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Game
{
    public class Game
    {
        PlayerAvatar player;
        List<Entity> entities = new List<Entity>();
        List<Entity> entitiesToRemove = new List<Entity>();
        List<Entity> entitiesToAdd = new List<Entity>();
        Score score;
        LifeCounter lifeCounter;
        Random rand;

        int level;
        int nextLevelThreshold;


        public Game()
        {
            rand = new Random();
            ExtendedConsole.VirtualClear();
            ExtendedConsole.SetActiveLayer(0);
            ExtendedConsole.VirtualDrawBox(0, 0, 40, 40);
            score = new Score();
            score.Add(1915);
            lifeCounter = new LifeCounter();
            rand = new Random();
            level = 1;
            StartGame();
        }

        void StartGame()
        {
            player = new PlayerAvatar(19, 38, this);
            entities.Add(player);
            //entities.Add(new BigEnemy(rand.Next(1, 33), Math.Min(rand.Next(3, 15 + level), 34), this));
            ShowLevelScreen("Level " + level, "Get ready");
            nextLevelThreshold = 100;
            GameLoop();
        }

        void GameLoop()
        {
            bool extraTurn = false;
            bool gameIsOver = false;
            while (!gameIsOver)
            {
                int tickTime = 50;
                Stack<ConsoleKeyInfo> keyPressed = new Stack<ConsoleKeyInfo>();
                while (Console.KeyAvailable)
                    keyPressed.Push(Console.ReadKey(true));

                gameIsOver = Update(keyPressed, extraTurn);

                extraTurn = !extraTurn;

                if (score.currentScore > nextLevelThreshold)
                {
                    level++;
                    nextLevelThreshold += level * 100;
                    tickTime--;
                    lifeCounter.AddLive();
                    ShowLevelScreen();
                }

                ExtendedConsole.Update();
                System.Threading.Thread.Sleep(tickTime);
            }
            GameOverScreen();
            GameController.nextScreen = -1;
        }
        
        bool Update(Stack<ConsoleKeyInfo> keyPressed, bool playerOnly)
        {
            bool gameIsOver = false;

            int currentThreatLevel = 0;
            int nbOfEnemy = 0;
            foreach (Entity e in entities)
            {
                if (e is SimpleEnemy)
                    nbOfEnemy++;
                currentThreatLevel += e.threatValue;
            }

            if (currentThreatLevel == 0 || (currentThreatLevel < level * 100 && rand.Next(0, 500) < level && nbOfEnemy < 5))
            {
                SpawnEnemy(level * 100 - currentThreatLevel);
            }

            bool moveLeft = false;
            bool moveRight = false;
            bool stepLeft = false;
            bool stepRight = false;
            bool stopMoving = false;
            bool shootBeam = false;
            bool pause = false;

            foreach (ConsoleKeyInfo key in keyPressed)
            {
                if (key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.A || key.Key == ConsoleKey.NumPad4)
                    moveLeft = true;
                else if (key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.D || key.Key == ConsoleKey.NumPad6)
                    moveRight = true;
                else if (key.Key == ConsoleKey.Q || key.Key == ConsoleKey.NumPad7)
                    player.HitLeft();
                else if (key.Key == ConsoleKey.E || key.Key == ConsoleKey.NumPad9)
                    player.HitRight();
                else if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W || key.Key == ConsoleKey.NumPad8)
                {
                    shootBeam = true;
                    stopMoving = true;
                }
                else if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S || key.Key == ConsoleKey.NumPad5)
                    stopMoving = true;
                else if (key.Key == ConsoleKey.Z || key.Key == ConsoleKey.NumPad1)
                {
                    stopMoving = true;
                    stepLeft = true;
                }
                else if (key.Key == ConsoleKey.C || key.Key == ConsoleKey.NumPad3)
                {
                    stopMoving = true;
                    stepRight = true;
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    pause = true;
                }
            }

            if (pause)
                gameIsOver = PauseGame();

            if (moveLeft && moveRight || stopMoving)
            {
                player.movingLeft = false;
                player.movingRight = false;
                player.beamIsActive = false;
            }
            else if (moveLeft)
            {
                player.movingLeft = true;
                player.movingRight = false;
                player.beamIsActive = false;
            }
            else if (moveRight)
            {
                player.movingLeft = false;
                player.movingRight = true;
                player.beamIsActive = false;
            }

            if (shootBeam)
                player.beamIsActive = true;


            if ((player.movingRight || stepRight) && player.GetposX() < 32)
                player.Move(1);
            else if ((player.movingLeft || stepLeft) && player.GetposX() > 3)
                player.Move(-1);


            if (playerOnly)
                entities[0].Update();
            else
            {
                int i = 0;
                while (i < entities.Count)
                {
                    entities[i].Update();
                    i++;
                }
            }

            foreach (Entity e in entitiesToRemove)
                entities.Remove(e);
            entitiesToRemove = new List<Entity>();

            if (player.isDestroyed && player.destroyedAnimation < 0)
                gameIsOver = true;

            return gameIsOver;
        }

        void ShowLevelScreen(string message, string submessage)
        {
            ExtendedConsole.SetActiveLayer(3);
            ExtendedConsole.VirtualDrawBox(0, 0, 40, 40);
            ExtendedConsole.VirtualFill(1, 1, 38, 38);
            ExtendedConsole.VirtualWrite(message, 20 - (message.Length) / 2, 10);
            ExtendedConsole.VirtualWrite(submessage, 20 - (submessage.Length) / 2, 12);
            ExtendedConsole.Update();
            System.Threading.Thread.Sleep(1000);
            ExtendedConsole.VirtualLayerReset();
            score.Draw();
            lifeCounter.Draw();
        }
        void ShowLevelScreen(string message)
        {
            ShowLevelScreen(message, "");
        }
        void ShowLevelScreen()
        {
            ShowLevelScreen("Level " + level);
        }

        void GameOverScreen()
        {
            ExtendedConsole.SetActiveLayer(3);
            ExtendedConsole.VirtualDrawBox(0, 0, 40, 40);
            ExtendedConsole.VirtualFill(1, 1, 38, 38);
            ExtendedConsole.VirtualWrite("GAME OVER", 16, 10);
            string finalScore = "Final" + score.ToString();
            ExtendedConsole.VirtualWrite(finalScore, 20 - finalScore.Length / 2, 12);
            ExtendedConsole.VirtualWrite("Your name: ", 5, 16);
            ExtendedConsole.Update();

            Console.CursorVisible = true;
            Console.SetCursorPosition(5, 18);
            string playername = Console.ReadLine();
            Console.CursorVisible = false;
            
            if (playername.Length > 0)
            {
                XDocument xml = new XDocument();
                if (File.Exists("HighScore.xml"))
                    xml = XDocument.Load("HighScore.xml");
                string[] names = xml.Descendants("entry").Descendants("name").Select(element => element.Value).ToArray();
                string[] scores = xml.Descendants("entry").Descendants("score").Select(element => element.Value).ToArray();

                string[] newNames = new string[names.Length + 1];
                string[] newScores = new string[scores.Length + 1];

                int i = 0;
                bool isAdded = false;
                while (i < newScores.Length)
                {
                    if (i == scores.Length && !isAdded)
                    {
                        newNames[i] = playername;
                        newScores[i] = score.currentScore.ToString();
                    }
                    else
                    {
                        if (!isAdded)
                            if (int.Parse(scores[i]) < score.currentScore)
                            {
                                newNames[i] = playername;
                                newScores[i] = score.currentScore.ToString();
                                isAdded = true;
                                i++;
                            }
                            else
                            {
                                newScores[i] = scores[i];
                                newNames[i] = names[i];
                            }
                        if (isAdded)
                        {
                            newScores[i] = scores[i - 1];
                            newNames[i] = names[i - 1];
                        }
                    }
                    i++;
                }

                XDocument newXml = new XDocument();
                XElement root = new XElement("root");
                for (i = 0; i < newNames.Length; i++)
                {
                    XElement e = new XElement("entry", new XElement("name", newNames[i]), new XElement("score", newScores[i]));
                    root.Add(e);
                }
                newXml.Add(root);

                ExtendedConsole.VirtualWrite("Saving", 18, 25);
                ExtendedConsole.Update();

                newXml.Save("HighScore.xml");

                ExtendedConsole.VirtualErase(15, 25, 11, 1);
                ExtendedConsole.VirtualWrite("Score Saved", 15, 25);
                ExtendedConsole.Update();
            }
            
            System.Threading.Thread.Sleep(1000);
            ExtendedConsole.VirtualLayerReset();
            score.Draw();
            lifeCounter.Draw();
        }

        bool PauseGame()
        {
            ExtendedConsole.SetActiveLayer(3);
            ExtendedConsole.VirtualFill(10, 10, 20, 8);
            ExtendedConsole.VirtualDrawBox(10, 10, 20, 8);
            ExtendedConsole.VirtualWrite("Paused", 17, 12);
            string[] options = new string[2];
            options[0] = "Continue";
            options[1] = "  Exit";
            if (ExtendedConsole.ShowMenuAndGetChoice(options, 15, 13, 0, true, null, false) == 1)
                return true;

            ExtendedConsole.Update();
            ExtendedConsole.VirtualLayerReset();
            score.Draw();
            lifeCounter.Draw();
            return false;

        }

        internal void AlterScore(int i)
        {
            if (i > 0)
                score.Add(i);
            else
                score.Substract(i);
        }

        private void SpawnEnemy(int maxThreatLevel)
        {
            List<Enemy> e = new List<Enemy>();

            if (maxThreatLevel >= 100)
                e.Add(new SimpleEnemy(rand.Next(1, 33), Math.Min(rand.Next(3, 15 + level), 34), this));
            if (maxThreatLevel >= 150)
                e.Add(new MediumEnemy(rand.Next(1, 30), Math.Min(rand.Next(3, 15 + level), 34), this));
            if (maxThreatLevel >= 300)
                e.Add(new BigEnemy(rand.Next(3, 27), Math.Min(rand.Next(3, 15 + level), 34), this));

            int spawnIndex = rand.Next(0, e.Count);

            if (e.Count != 0)
                entities.Add(e[spawnIndex]);

        }

        public void RemoveEntity(Entity toRemove)
        {
            entitiesToRemove.Add(toRemove);
        }

        internal void SpawnBomb(int x, int y)
        {
            entities.Add(new BombProjectile(x, y, this));
        }

        internal void LateAddProjectile(int x, int y, int dirX, int dirY, bool isSplitted)
        {
            entitiesToAdd.Add(new Projectile(x, y, dirX, dirY, this, isSplitted));
        }

        internal void CreateProjectile(int x, int y, int dirX, int dirY, bool splitted = false)
        {
            entities.Add(new Projectile(x, y, dirX, dirY, this, splitted));
        }

        internal void CreateProjectileTrail(int x, int y, bool splitted)
        {
            entities.Add(new ProjTrail(x, y, 6, splitted, this));
        }

        internal void SplitProjectiles()
        {
            foreach (Entity e in entities)
                if (e is Projectile)
                    ((Projectile)e).Split();

            foreach (Entity e in entitiesToAdd)
                entities.Add(e);
            entitiesToAdd = new List<Entity>();
        }

        internal bool CheckProjectileCollision(int x, int y, bool canDamageEnemy)
        {
            bool collision = false;
            int i = entities.Count - 1;
            while (i >= 0)
            {
                if (entities[i].CheckCollision(x, y) && (i == 0 || canDamageEnemy))
                {
                    collision = true;
                    entities[i].Damage();
                }
                i--;
            }
            return collision;
        }

        internal bool RemoveLife()
        {
            lifeCounter.RemoveLive();
            if (lifeCounter.nbOfLife < 1)
                return true;
            return false;
        }

        public override string ToString()
        {
            return "I am the world";
        }
    }
}
