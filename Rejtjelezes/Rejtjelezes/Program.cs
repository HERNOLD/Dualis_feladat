using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rejtjelezes
{
    internal class Program
    {
        static char[] hungarianLetters = { 'ö', 'ü', 'ó', 'ő', 'ú', 'é', 'á', 'ű', 'í' };

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Kérem válasszon az alábbi opciók közül!");
                Console.WriteLine("Titkosítás (1-es gomb), Dekódolás (2-es gomb), Kilépés (3-as gomb)");
                string i=Console.ReadLine();
                switch (i)
                {
                    case "1":
                        CodeMessage();
                        break;
                    case "2":
                        DecodeMessage(); 
                        break;
                    case "3":
                        Environment.Exit(0);
                        break;
                }
            }
        }

        static void DecodeMessage()
        {
            List<int> codedMessageNumbers = new List<int>();
            List<int> keyNumbers = new List<int>();
            List<int> decodedMessageNumbers = new List<int>();

            Console.WriteLine("Kérem adja meg a kódolt üzenetet!");
            string codedMessage = Console.ReadLine();
            codedMessage = codedMessage.ToLower();

            bool isMessageCorrectFormat = codedMessage.All(c => Char.IsLetter(c) && !hungarianLetters.Contains(c) || c == ' ');
            if (!isMessageCorrectFormat)
            {
                Console.Write("A kódolt üzenet csak angol betűkből és szóközökből állhat!");
                return;
            }

            Console.WriteLine("Kérem adja meg a kulcsot!");
            string key = Console.ReadLine();

            bool isKeyCorrectFormat = key.All(c => Char.IsLetter(c) && !hungarianLetters.Contains(c) || c == ' ');
            if (!isMessageCorrectFormat)
            {
                Console.Write("A kulcs csak angol betűkből és szóközökből állhat!");
                return;
            }
            if (key.Length < codedMessage.Length)
            {
                Console.WriteLine("A megadott kulcsnak legalább akkorának kell lennie mint a kódolt üzenetnek!");
                return;
            }

            ConvertStringToNumbers(codedMessage, codedMessageNumbers);
            ConvertStringToNumbers(key, keyNumbers);

            for (int i = 0; i < codedMessageNumbers.Count; i++)
            {
                int number = codedMessageNumbers[i] - keyNumbers[i];
                if (number < 0)
                {
                    number += 27;
                }
                decodedMessageNumbers.Add(number);
            }

            Console.WriteLine("A megfejtett üzenet: "+ConvertNumbersToString(decodedMessageNumbers));
        }

        static void CodeMessage()
        {
            List<int> messageNumbers = new List<int>();
            List<int> keyNumbers = new List<int>();
            List<int> codedMessageNumbers = new List<int>();

            Console.WriteLine("Kérem adja meg az üzenetet!");
            string message = Console.ReadLine();
            message = message.ToLower();

            bool isMessageCorrectFormat = message.All(c => Char.IsLetter(c) && !hungarianLetters.Contains(c) || c == ' ');
            if (!isMessageCorrectFormat)
            {
                Console.Write("Az üzenet csak angol betűkből és szóközökből állhat!");
                return;
            }

            Console.WriteLine("Kérem adja meg a kulcsot!");
            string key = Console.ReadLine();

            bool isKeyCorrectFormat = key.All(c => Char.IsLetter(c) && !hungarianLetters.Contains(c) || c == ' ');
            if (!isMessageCorrectFormat)
            {
                Console.Write("A kulcs csak angol betűkből és szóközökből állhat!");
                return;
            }
            if (key.Length < message.Length)
            {
                Console.WriteLine("A megadott kulcsnak legalább akkorának kell lennie mint az üzenetnek!");
                return;
            }

            ConvertStringToNumbers(message, messageNumbers);
            ConvertStringToNumbers(key, keyNumbers);

            for (int i = 0; i < messageNumbers.Count; i++)
            {
                int number = messageNumbers[i] + keyNumbers[i];
                if (number > 26)
                {
                    number = number % 27;
                }
                codedMessageNumbers.Add(number);
            }

            Console.WriteLine("A kódolt üzenet: "+ConvertNumbersToString(codedMessageNumbers));
        }

        static void ConvertStringToNumbers(string convertable, List<int> numbersList)
        {
            foreach (char c in convertable)
            {
                if (c == ' ')
                {
                    numbersList.Add(26);
                }
                else
                {
                    int index = (int)c % 32;
                    numbersList.Add(index-1);
                }
            }
        }

        static string ConvertNumbersToString(List<int> numbersList)
        {
            string solution = "";

            foreach (int number in numbersList)
            {
                var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

                if (number == 26)
                {
                    solution += ' ';
                }
                else
                {
                    char letter = alphabet[number];
                    solution += letter;
                }
            }

            return solution.ToLower();
        }
    }
}
