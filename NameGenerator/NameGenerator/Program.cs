using System;

namespace NameGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Generator.Initiate();

            Random rand = new Random();
            string randomName = "";

            bool endProgram = false;
            while (!endProgram)
            {
                Console.Clear();
                Console.WriteLine("--------------" +
                Environment.NewLine + "Name Generator" +
                Environment.NewLine + "--------------");
                Console.WriteLine("Choose an action:" +
                Environment.NewLine + "1 - Generate a name" +
                Environment.NewLine + "2 - Generate multiple names" +
                Environment.NewLine + "3 - Compile new weights" +
                Environment.NewLine + "4 - End Program" +
                Environment.NewLine);
                if (randomName != "")
                {
                    bool maleName = rand.Next(0, 10) < 5;
                    Console.WriteLine(randomName);
                }

                Char input = Console.ReadKey(true).KeyChar;
                switch (input)
                {
                    case '1':
                        bool maleName = rand.Next(0, 10) < 5;
                        randomName = Generator.GenerateName(maleName) + (maleName ? " - M" : " - F");
                        break;
                    case '2':
                        randomName = "";

                        bool inputIsValid = false;
                        while (!inputIsValid)
                        {
                            Console.Clear();
                            Console.WriteLine("How many?");
                            string input2 = Console.ReadLine();
                            if (Int32.TryParse(input2, out int parsedInput))
                            {
                                if(parsedInput > 0)
                                {
                                    GenerateNames(parsedInput);
                                    inputIsValid = true;
                                }
                            }
                        }
                        break;
                    case '3':
                        randomName = "";
                        Compiler.Start();
                        break;
                    case '4':
                        endProgram = true;
                        break;
                }
            } 
        }

        static private void GenerateNames(int amount = 50)
        {
            Random rand = new Random();
            for (int i = 1; i <= amount; i++)
            {
                bool maleName = rand.Next(0, 10) < 5;
                Console.WriteLine(Generator.GenerateName(maleName) + (maleName?" - Male":" - Female"));

                if (i % 10 == 0)
                {
                    Console.WriteLine("-----");
                    Console.ReadKey(true);
                }
            }
            Console.WriteLine("-----");
            Console.WriteLine("END");
            Console.ReadKey(true);
        }
    }
}
