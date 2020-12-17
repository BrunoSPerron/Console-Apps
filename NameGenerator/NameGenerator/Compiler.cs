using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NameGenerator
{
    class Compiler
    {
        private struct Entry
        {
            public string Name;
            public bool IsMenName;
            public int Weight;

            public Entry(string name, bool isMenName, int weight)
            {
                Name = name;
                IsMenName = isMenName;
                Weight = weight;
            }
        }

        public static void Start()
        {
            DirectoryInfo rootDirectoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            
            bool inputIsValid = false;
            while (!inputIsValid)
            {
                Console.Clear();
                Console.WriteLine("--------------------" +
                Environment.NewLine + "Generate new Weights" +
                Environment.NewLine + "--------------------" +
                Environment.NewLine + "Weights are generated from .txt files using this syntax:" +
                Environment.NewLine + "{Name}, {Sex M/F}, {Occurence}" +
                Environment.NewLine +
                Environment.NewLine + "Select a folder containing raw data" +
                Environment.NewLine + "WARNING: THIS WILL OVERWRITE THE CURRENT WEIGHTS FILE" +
                Environment.NewLine);

                DirectoryInfo[] d = rootDirectoryInfo.GetDirectories();
                for (int i = 0; i < d.Length; i++)
                {
                    Console.WriteLine((i + 1) + " - " + d[i].Name);
                }
                Console.WriteLine((d.Length + 1) + " - RETURN");

                string input = Console.ReadLine();
                if (Int32.TryParse(input, out int parsedInput))
                {
                    if (parsedInput > 0 && parsedInput <= d.Length + 1)
                    {
                        inputIsValid = true;
                        if (parsedInput <= d.Length)
                        {
                            Console.Clear();
                            Console.WriteLine("-COMPILING-");
                            Compile(d[parsedInput - 1]);
                            Console.WriteLine("-FINISHED-");
                            Console.ReadKey(true);
                        }
                    }
                }
            }
        }

        static private void Compile(DirectoryInfo di)
        {
            string[] lines = new string[0];
            foreach (FileInfo file in di.GetFiles("*.txt"))
            {
                string[] toAdd = System.IO.File.ReadAllLines(di.FullName + @"/" + file.Name);
                int array1OriginalLength = lines.Length;
                Array.Resize<string>(ref lines, array1OriginalLength + toAdd.Length);
                Array.Copy(toAdd, 0, lines, array1OriginalLength, toAdd.Length);
            }

            Entry[] namesData = new Entry[lines.Length];
            int averageMenNameLength = 0;
            int averageWomenNameLength = 0;
            int numberOfMenName = 0;
            int numberOfWomenName = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                string s = lines[i];
                string[] data = s.Split(',');
                namesData[i] = new Entry(data[0], data[1][0] == 'M', int.Parse(data[2]));
                if (data[1][0] == 'M')
                {
                    numberOfMenName++;
                    averageMenNameLength += data[0].Length;
                }
                else
                {
                    numberOfWomenName++;
                    averageWomenNameLength += data[0].Length;
                }
            }
            averageMenNameLength /= numberOfMenName;
            averageWomenNameLength /= numberOfWomenName;



            Dictionary<char, Dictionary<char, int>> menCharWeights = new Dictionary<char, Dictionary<char, int>>();
            Dictionary<char, Dictionary<char, int>> womenCharWeights = new Dictionary<char, Dictionary<char, int>>();

            for (int i = 64; i < 91; i++)
            {
                menCharWeights.Add((char)i, new Dictionary<char, int>());
                womenCharWeights.Add((char)i, new Dictionary<char, int>());
                for (int j = 65; j < 91; j++)
                {
                    menCharWeights[(char)i].Add((char)j, 0);
                    womenCharWeights[(char)i].Add((char)j, 0);
                }
            }

            foreach (Entry e in namesData)
            {
                char precedent = '@';
                foreach (char c in e.Name)
                {
                    if (e.IsMenName)
                        menCharWeights[(char)precedent][char.ToUpper(c)] += e.Weight;
                    else
                        womenCharWeights[(char)precedent][char.ToUpper(c)] += e.Weight;

                    precedent = char.ToUpper(c);
                }
            }
            int currentLineIndex = 0;
            int headerLength = 2;
            string[] newLines = new string[1404 + headerLength];
            newLines[0] = "AverageMenLength:" + averageMenNameLength;
            newLines[1] = "AverageWomenLength:" + averageWomenNameLength;
            currentLineIndex = headerLength;

            foreach (KeyValuePair<char, Dictionary<char, int>> dMain in menCharWeights)
            {
                foreach (KeyValuePair<char, int> dSub in menCharWeights[dMain.Key])
                {
                    newLines[currentLineIndex] = 'M' + "&" + dMain.Key + "&" + dSub.Key + "&" + dSub.Value;
                    currentLineIndex++;
                }
            }

            foreach (KeyValuePair<char, Dictionary<char, int>> dMain in womenCharWeights)
            {
                foreach (KeyValuePair<char, int> dSub in womenCharWeights[dMain.Key])
                {
                    newLines[currentLineIndex] = 'F' + "&" + dMain.Key + "&" + dSub.Key + "&" + dSub.Value;
                    currentLineIndex++;
                }
            }
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"/" + "CharWeights.txt"))
            {
                foreach (string line in newLines)
                {
                    file.WriteLine(line);
                }
            }
        }
    }
}
