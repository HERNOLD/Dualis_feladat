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
    //még optimalizálni kéne, maybe with hashset or that tri thingy

    public class Task2
    {
        public static List<string> possibleKeys= new List<string>();

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

        public static void FindKeySegment(string codedMessage1, string codedMessage2, string decodedMessage1, string[] words)
        {
            //making sure the first message is always longer or as long as the second message
            //this is needed because this code is mostly working witht the first message
            if (codedMessage1.Length < codedMessage2.Length)
            {
                string temp = codedMessage1;
                codedMessage1 = codedMessage2;
                codedMessage2 = temp;
            }

            var codedMessageNumbers1 = Task1.ConvertStringToNumbers(codedMessage1);

            //if the whole message got decoded we write it out
            if (decodedMessage1.Length == codedMessage1.Length)
            {
                //a sentence (normal) cant end with a space so we check for that
                if (!decodedMessage1.EndsWith(" "))
                {
                    string decodedMessage2 = Task1.Decode(codedMessage2, Task1.ConvertNumbersToString(GetKeyBasedOnMessage(codedMessageNumbers1, decodedMessage1)));

                    possibleKeys.Add(Task1.ConvertNumbersToString(GetKeyBasedOnMessage(codedMessageNumbers1, decodedMessage1)));
                }
                return;
            }

            //going through all the words
            foreach (string word in words)
            {
                //checking if word would still fit into the message
                if (decodedMessage1.Length + word.Length <= codedMessage1.Length)
                {
                    string newDecodedMessage = decodedMessage1 + word;

                    //because sentences consists of words and spaces we put a space at the end of the decoded message
                    if (newDecodedMessage.Length < codedMessage1.Length)
                    {
                        newDecodedMessage += " ";

                        //if that space would be the last character we just pass
                        if (newDecodedMessage.Length == codedMessage1.Length)
                        {
                            continue;
                        }
                    }

                    //getting key from the new decoded message
                    List<int> partialKey = GetKeyBasedOnMessage(codedMessageNumbers1, newDecodedMessage);

                    //getting a substring from the second message based on the partial key's length
                    int lengthToDecode = Math.Min(partialKey.Count, codedMessage2.Length);
                    string codedMessage2Part = codedMessage2.Substring(0, lengthToDecode);

                    //decoding part of second message
                    string decodedMessage2 = Task1.Decode(codedMessage2Part, Task1.ConvertNumbersToString(partialKey));

                    //checking if the decoded part of second message consists of words
                    string[] decodedWords = decodedMessage2.Split(' ');
                    bool isValid = true;

                    for (int i = 0; i < decodedWords.Length; i++)
                    {
                        string wordPart = decodedWords[i];

                        if (i == decodedWords.Length - 1 && lengthToDecode < codedMessage2.Length)
                        {
                            //the last item of the array can be a word fragment so we check if it can be the fragment of a normal word
                            if (!words.Any(w => w.StartsWith(wordPart)))
                            {
                                isValid = false;
                                break;
                            }
                        }
                        else
                        {
                            if (!words.Contains(wordPart))
                            {
                                isValid = false;
                                break;
                            }
                        }
                    }

                    //if everything is good than we just continue
                    if (isValid)
                    {
                        FindKeySegment(codedMessage1, codedMessage2, newDecodedMessage, words);
                    }
                }
            }
        }
    }
}
