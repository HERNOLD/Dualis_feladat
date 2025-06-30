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
        public static List<int> GetKeyBasedOnMessage(List<int> codedMessageNumbers, string decodedMessage)
        {
            var wordInNumbers = Task1.ConvertStringToNumbers(decodedMessage);
            List<int> partialKey = new List<int>();
            for (int i = 0; i < wordInNumbers.Count; i++)
            {
                int keyPart = codedMessageNumbers[i] - wordInNumbers[i];
                if (keyPart < 0)
                {
                    keyPart += 27;
                }
                partialKey.Add(keyPart);
            }
            return partialKey;
        }

        public static List<string> FindWordsBasedOnPart(string part)
        {
            string[] words = File.ReadAllLines("wordsShort.txt");
            return words.Where(w => w.StartsWith(part)).ToList();
        }

        public static void FindKeySegment(string codedMessage1, string codedMessage2, string decodedMessage1)
        {
            string[] words = File.ReadAllLines("wordsShort.txt");

            //making sure that the codedMessage1 string is always the longer one
            if (codedMessage1.Length < codedMessage2.Length)
            {
                string temp=codedMessage1;
                codedMessage1 = codedMessage2;
                codedMessage2 = temp;
            }

            //converting it to numbers
            var codedMessageNumbers1 = Task1.ConvertStringToNumbers(codedMessage1);

            Console.WriteLine("Eddigi megfejtett rész: " + decodedMessage1);

            foreach(string word in words)
            {
                if (decodedMessage1.Length + word.Length <= codedMessage1.Length)
                {
                    List<int> partialKey = GetKeyBasedOnMessage(codedMessageNumbers1, (decodedMessage1 + word));

                    string decodedMessage2 = Task1.Decode(codedMessage2.Substring(0, Math.Min(partialKey.Count, codedMessage2.Length)), Task1.ConvertNumbersToString(partialKey));
                    string[] decodedMessage2Split=decodedMessage2.Split(' ');

                    List<string> possibleWords = new List<string>();
                    int count = 0;

                    foreach (string s in decodedMessage2Split)
                    {
                        if (words.Contains(s))
                        {
                            possibleWords.Add(s);
                            count++;
                        }
                        else
                        {
                            possibleWords.AddRange(FindWordsBasedOnPart(s));
                        }
                    }

                    if (possibleWords.Count > count)
                    {
                        decodedMessage1 += word;
                        if (decodedMessage1.Length < codedMessage1.Length)
                        {
                            decodedMessage1 += " ";
                        }
                        Console.WriteLine(decodedMessage1);
                        FindKeySegment(codedMessage1, codedMessage2, decodedMessage1);
                    }
                }
            }
        }

        public static void FindFirstKeySegment(string codedMessage1, string codedMessage2)
        {
            string[] words = File.ReadAllLines("wordsShort.txt");
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

                            Console.WriteLine($"Word valami: {word}, key part: {Task1.ConvertNumbersToString(partialKey)}, second message: {partialMessage2}");
                            FindNextKeyPart(codedMessage1, codedMessage2, partialKey, word);
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

        static void FindNextKeyPart(string codedMessage1, string codedMessage2, List<int> key, string firstWord)
        {
            string[] words = File.ReadAllLines("wordsShort.txt");

            var codedMessageNumbers1 = Task1.ConvertStringToNumbers(codedMessage1);
            var codedMessageNumbers2 = Task1.ConvertStringToNumbers(codedMessage2);

            List<int> partialKey = key;

            List<int> firstMessageDecoded = new List<int>();
            int length = Math.Min(partialKey.Count, codedMessageNumbers1.Count);
            for (int i = 0; i < length; i++)
            {
                int decodedChar = codedMessageNumbers1[i] - partialKey[i];
                if (decodedChar < 0)
                {
                    decodedChar += 27;
                }
                firstMessageDecoded.Add(decodedChar);
            }

            string[] decodedMessageSplit = Task1.ConvertNumbersToString(firstMessageDecoded).Split(' ');
            string partialMessage = "";

            foreach (string s in decodedMessageSplit)
            {
                if (!words.Contains(s))
                {
                    List<string> matchingWords = words.Where(w => w.StartsWith(s)).ToList();

                    foreach(string word in matchingWords)
                    {
                        partialMessage =firstWord+' '+word;

                        partialKey = new List<int>();
                        var wordInNumbers = Task1.ConvertStringToNumbers(partialMessage);
                        length = Math.Min(wordInNumbers.Count, codedMessageNumbers1.Count);

                        for (int i = 0; i < length; i++)
                        {
                            int keyPart = codedMessageNumbers1[i] - wordInNumbers[i];
                            if (keyPart < 0)
                            {
                                keyPart += 27;
                            }
                            partialKey.Add(keyPart);
                        }

                        //inentől megint visszahelyetesítjük a második üzenetbe:

                        //decoding part of second message based on key calculated above
                        List<int> secondMessageDecodedPart = new List<int>();
                        length = Math.Min(partialKey.Count, codedMessageNumbers2.Count);
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
                        decodedMessageSplit = secondMessageDecodedString.Split(' ');
                        string partialMessage2 = "";

                        //going through every string after spliting
                        foreach (string f in decodedMessageSplit)
                        {
                            //if split part of word isnt word, we try to find what word it could be
                            if (!words.Contains(f))
                            {
                                string matchingWord = words.FirstOrDefault(w => w.StartsWith(f));

                                if (matchingWord != null && (partialMessage2+matchingWord).Length<=codedMessageNumbers2.Count)
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

                                    if (partialMessage2.Length == codedMessage2.Length)
                                    {
                                        Console.WriteLine("Második üzenet feltörve: " + partialMessage2);
                                        Console.WriteLine("\n");
                                    }
                                    else
                                    {
                                        //decoding part of second message based on key calculated above
                                        firstMessageDecoded = new List<int>();
                                        length = Math.Min(partialKey.Count, codedMessageNumbers1.Count);
                                        for (int i = 0; i < length; i++)
                                        {
                                            int decodedChar = codedMessageNumbers1[i] - partialKey[i];
                                            if (decodedChar < 0)
                                            {
                                                decodedChar += 27;
                                            }
                                            firstMessageDecoded.Add(decodedChar);
                                        }

                                        //spliting up decoded part of second message by the spaces
                                        string firstMessageDecodedString = Task1.ConvertNumbersToString(firstMessageDecoded);
                                        decodedMessageSplit = firstMessageDecodedString.Split(' ');

                                        partialMessage = "";

                                        //going through every string after spliting
                                        foreach (string t in decodedMessageSplit)
                                        {
                                            if (!words.Contains(t))
                                            {
                                                matchingWord = words.FirstOrDefault(w => w.StartsWith(t));

                                                if (matchingWord != null)
                                                {
                                                    partialMessage += matchingWord;
                                                    Console.WriteLine(partialMessage);

                                                    //calculating key based on word
                                                    wordInNumbers = Task1.ConvertStringToNumbers(partialMessage);
                                                    partialKey = new List<int>();
                                                    for (int i = 0; i < wordInNumbers.Count; i++)
                                                    {
                                                        int keyPart = codedMessageNumbers1[i] - wordInNumbers[i];
                                                        if (keyPart < 0)
                                                        {
                                                            keyPart += 27;
                                                        }
                                                        partialKey.Add(keyPart);
                                                    }

                                                    if (partialMessage.Length == codedMessage1.Length)
                                                    {
                                                        Console.WriteLine("Teljes 1. üzenet: " + partialMessage);
                                                    }
                                                    FindNextKeyPart(codedMessage1, codedMessage2, partialKey, partialMessage);
                                                }
                                            }
                                            else
                                            {
                                                partialMessage += t + " ";
                                            }
                                        }
                                        Console.WriteLine("\n");
                                    }
                                }
                            }
                            //if split part fo string is word we just save it
                            else
                            {
                                partialMessage2 += f + " ";
                            }
                        }
                    }
                }
                else
                {
                    partialMessage += s + " ";
                }
            }
        }
    }
}
