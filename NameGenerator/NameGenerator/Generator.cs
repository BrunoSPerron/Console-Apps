using System;
using System.Collections.Generic;
using System.Text;

namespace NameGenerator
{
    class Generator
    {
        static private Dictionary<char, Dictionary<char, int>> maleCharWeights;
        static private Dictionary<char, Dictionary<char, int>> femaleCharWeights;
        static private int averageNameLength;
        static private int nameLengthVariance;
        static private Random rand;

        static private char precedentChar;
        static private char currentChar;

        public static void Initiate()
        {
            rand = new Random();
            averageNameLength = 6;
            nameLengthVariance = 3;

            maleCharWeights = new Dictionary<char, Dictionary<char, int>>();
            femaleCharWeights = new Dictionary<char, Dictionary<char, int>>();

            for (int i = 64; i < 91; i++)
            {
                maleCharWeights.Add((char)i, new Dictionary<char, int>());
                femaleCharWeights.Add((char)i, new Dictionary<char, int>());
            }

            string[] lines = System.IO.File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "CharWeights.txt");

            for (int i = 2; i < lines.Length; i++)
            {
                string s = (string)lines[i];
                string[] data = s.Split('&');

                if (data[0] == "M")
                    maleCharWeights[data[1][0]].Add(data[2][0], int.Parse(data[3]));
                else
                    femaleCharWeights[data[1][0]].Add(data[2][0], int.Parse(data[3]));
            }
        }

        public static string GenerateName(bool forMale = true)
        {
            string newName = "";
            int nameLength = averageNameLength + rand.Next(-nameLengthVariance, nameLengthVariance) + rand.Next(0, 2);

            precedentChar = ' ';
            currentChar = '@';

            while (newName.Length < nameLength)
            {
                char c = currentChar;
                currentChar = GetRandomCharAfter(forMale);
                precedentChar = c;
                newName += currentChar;
            }

            return newName[0] + newName.Substring(1).ToLower();

        }

        static char GetRandomCharAfter(bool forMaleName)
        {
            char c;
            do
            {
                c = '-';

                int TotalWeight = 0;
                foreach (KeyValuePair<char, int> kvp in forMaleName ? maleCharWeights[currentChar] : femaleCharWeights[currentChar])
                    TotalWeight += kvp.Value;
                int rollValue = rand.Next(0, TotalWeight);

                foreach (KeyValuePair<char, int> kvp in forMaleName ? maleCharWeights[currentChar] : femaleCharWeights[currentChar])
                {
                    rollValue -= kvp.Value;
                    if (rollValue < 0 && c == '-')
                    {
                        c = kvp.Key;
                    }
                }
            } while (currentChar == c && precedentChar == c);   //avoid thrice in a row

            return c;
        }
    }
}
