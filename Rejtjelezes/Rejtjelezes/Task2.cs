using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Threading;
using System.Diagnostics.Eventing.Reader;

namespace Rejtjelezes
{
    public class Task2
    {

        public static void FindKey(string codedMessage1, string codedMessage2, List<int> key)
        {
            string[] words = File.ReadAllLines("words.txt");
            Console.WriteLine(words.Length);

            var codedMessageNumbers1 = Task1.ConvertStringToNumbers(codedMessage1);
            var codedMessageNumbers2 = Task1.ConvertStringToNumbers(codedMessage2);

            foreach (string word in words)
            {
                if (word.Length > codedMessage1.Length)
                {
                    Console.WriteLine(word + " kihagyva");
                    continue;
                }

                //calculating key based on word
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

                //decoding part of second message based on key calculated above
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

                //spliting up decoded part of second message by the spaces
                string secondMessageDecodedString = Task1.ConvertNumbersToString(secondMessageDecodedPart);
                string[] decodedMessageSplit = secondMessageDecodedString.Split(' ');
                string partialMessage2 = "";

                //going through every string after spliting
                foreach(string s in decodedMessageSplit)
                {
                    //if split part of word isnt word, we try to find what word it could be
                    if (!words.Contains(s))
                    {
                        string matchingWord = words.FirstOrDefault(w => w.StartsWith(s));

                        if (matchingWord != null)
                        {
                            partialMessage2 += matchingWord;

                            var partialMessage2Numbers = Task1.ConvertStringToNumbers(partialMessage2);

                            partialKey = new List<int>();
                            for (int i = 0; i < partialMessage2Numbers.Count; i++)
                            {
                                int keyPart = codedMessageNumbers2[i] - partialMessage2Numbers[i];
                                if (keyPart < 0)
                                {
                                    keyPart += 27;
                                }
                                partialKey.Add(keyPart);
                            }

                            Console.WriteLine($"Word: {word}, key part: {Task1.ConvertNumbersToString(partialKey)}, second message part: {secondMessageDecodedString}, the probable word: {matchingWord}, eddigi second message: {partialMessage2}");
                        }
                    }
                    //if split part fo string is word we just save it
                    else
                    {
                        partialMessage2 += s + " ";
                    }
                }
            } 
        }
    }
}
