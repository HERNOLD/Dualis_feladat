using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace Rejtjelezes
{
    public class Task2
    {
        public static void GetFirstKeyFragment(string codedMessage1, string codedMessage2)
        {
            string[] words = File.ReadAllLines("words.txt");
            Console.WriteLine(words.Length);

            var codedMessageNumbers1 = Task1.ConvertStringToNumbers(codedMessage1);
            var codedMessageNumbers2 = Task1.ConvertStringToNumbers(codedMessage2);

            List<string> possibleKeys = new List<string>();

            foreach (string word in words)
            {
                if (word.Length > codedMessage1.Length)
                {
                    Console.WriteLine(word + "kihagyva");
                    continue;
                }

                var wordInNumbers = Task1.ConvertStringToNumbers(word);
                List<int> partialKey = new List<int>();

                for (int i = 0; i < wordInNumbers.Count; i++)
                {
                    int keyPart = codedMessageNumbers1[i] - wordInNumbers[i];
                    if (keyPart < 0)
                    {
                        keyPart += 27;
                    }
                    partialKey.Add(keyPart);
                }

                List<int> secondMessageDecodedPart = new List<int>();
                int length = Math.Min(wordInNumbers.Count, codedMessageNumbers2.Count);

                for (int i = 0; i < length; i++)
                {
                    int decodedChar = codedMessageNumbers2[i] - partialKey[i];
                    if (decodedChar < 0)
                    {
                        decodedChar += 27;
                    }
                    secondMessageDecodedPart.Add(decodedChar);
                }

                string secondMessageDecodedString = Task1.ConvertNumbersToString(secondMessageDecodedPart);
                string[] decodedMessageSplit = secondMessageDecodedString.Split(' ');
                foreach (string s in decodedMessageSplit)
                {
                    string matchingWord = words.FirstOrDefault(w => w.StartsWith(s));
                    if (matchingWord != null)
                    {
                        Console.WriteLine($"Word: {word}, key part: {Task1.ConvertNumbersToString(partialKey)}, second message part: {secondMessageDecodedString}, the probable word: {matchingWord}");
                    }
                }
            } 
        }

       
    }
}
