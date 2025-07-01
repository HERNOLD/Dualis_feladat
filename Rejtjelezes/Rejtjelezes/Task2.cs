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
        public static List<string> possibleKeys= new List<string>();
        private static HashSet<string> words;
        private static Trie trie;

        private static List<int> GetKeyBasedOnMessage(List<int> codedMessageNumbers, string decodedMessage)
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

        public static void StartFindingKeySegment(string codedMessage1, string codedMessage2)
        {
            string[] wordsArray = File.ReadAllLines("words.txt");
            words=new HashSet<string>(wordsArray);
            trie = new Trie();
            foreach (var word in wordsArray)
            {
                trie.Insert(word);
            }

            FindKeySegment(codedMessage1, codedMessage2, "");
        }

        private static void FindKeySegment(string codedMessage1, string codedMessage2, string decodedMessage1)
        {
            if (codedMessage1.Length < codedMessage2.Length)
            {
                string temp = codedMessage1;
                codedMessage1 = codedMessage2;
                codedMessage2 = temp;
            }

            var codedMessageNumbers1 = Task1.ConvertStringToNumbers(codedMessage1);

            if (decodedMessage1.Length == codedMessage1.Length)
            {
                if (!decodedMessage1.EndsWith(" "))
                {
                    string decodedMessage2 = Task1.Decode(codedMessage2, Task1.ConvertNumbersToString(GetKeyBasedOnMessage(codedMessageNumbers1, decodedMessage1)));

                    possibleKeys.Add(Task1.ConvertNumbersToString(GetKeyBasedOnMessage(codedMessageNumbers1, decodedMessage1)));
                }
                return;
            }

            foreach (string word in words)
            {
                if (decodedMessage1.Length + word.Length <= codedMessage1.Length)
                {
                    string newDecodedMessage = decodedMessage1 + word;

                    if (newDecodedMessage.Length < codedMessage1.Length)
                    {
                        newDecodedMessage += " ";

                        if (newDecodedMessage.Length == codedMessage1.Length)
                        {
                            continue;
                        }
                    }

                    List<int> partialKey = GetKeyBasedOnMessage(codedMessageNumbers1, newDecodedMessage);

                    int lengthToDecode = Math.Min(partialKey.Count, codedMessage2.Length);
                    string codedMessage2Part = codedMessage2.Substring(0, lengthToDecode);

                    string decodedMessage2 = Task1.Decode(codedMessage2Part, Task1.ConvertNumbersToString(partialKey));

                    string[] decodedWords = decodedMessage2.Split(' ');
                    bool isValid = true;

                    for (int i = 0; i < decodedWords.Length; i++)
                    {
                        string wordPart = decodedWords[i];

                        if (i == decodedWords.Length - 1 && lengthToDecode < codedMessage2.Length)
                        {
                            if (!trie.StartsWith(wordPart))
                            {
                                isValid = false;
                                break;
                            }
                        }
                        else
                        {
                            if (!trie.Search(wordPart))
                            {
                                isValid = false;
                                break;
                            }
                        }
                    }

                    if (isValid)
                    {
                        FindKeySegment(codedMessage1, codedMessage2, newDecodedMessage);
                    }
                }
            }
        }
    }

    public class TrieNode
    {
        public Dictionary<char, TrieNode> children = new Dictionary<char, TrieNode>();
        public bool isLeaf = false;
    }

    public class Trie
    {
        private TrieNode root = new TrieNode();

        public void Insert(string word)
        {
            TrieNode currentNode = root;
            foreach (char c in word)
            {
                if (!currentNode.children.ContainsKey(c))
                {
                    currentNode.children[c] = new TrieNode();
                }
                currentNode = currentNode.children[c];
            }
            currentNode.isLeaf = true;
        }

        public bool Search(string word)
        {
            TrieNode currentNode = root;
            foreach (char c in word)
            {
                if (!currentNode.children.ContainsKey(c))
                {
                    return false;
                }
                currentNode = currentNode.children[c];
            }
            return currentNode.isLeaf;
        }

        public bool StartsWith(string prefix)
        {
            TrieNode currentNode = root;
            foreach (char c in prefix)
            {
                if (!currentNode.children.ContainsKey(c))
                {
                    return false;
                }
                currentNode = currentNode.children[c];
            }
            return true;
        }
    }
}
