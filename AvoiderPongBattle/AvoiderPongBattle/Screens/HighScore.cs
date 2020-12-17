using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Game
{
    public class HighScore
    {
        public HighScore()
        {
            Draw();
        }

        private void Draw()
        {
            XDocument xml = new XDocument();
            if (File.Exists("HighScore.xml"))
                xml = XDocument.Load("HighScore.xml");
            string[] names = xml.Descendants("entry").Descendants("name").Select(element => element.Value).ToArray();
            string[] scores = xml.Descendants("entry").Descendants("score").Select(element => element.Value).ToArray();

            ExtendedConsole.VirtualClear();
            ExtendedConsole.VirtualDrawBox(0, 0, 40, 40);
            ExtendedConsole.VirtualWrite("HIGHSCORES", 15, 3);

            int i = 0;
            while (i < 30 && i < names.Length)
            {
                ExtendedConsole.VirtualWrite(i + 1, 2, 5 + i);
                ExtendedConsole.VirtualWrite(names[i], 5, 5 + i);
                ExtendedConsole.VirtualWrite(scores[i], 30, 5 + i);
                i++;
            }

            ExtendedConsole.VirtualWrite("press any key to continue...", 10, 38);
            ExtendedConsole.Update();
            Console.ReadKey(true);
            GameController.nextScreen = -1;
        }

        public override string ToString()
        {
            return "I remember";
        }
    }
}
