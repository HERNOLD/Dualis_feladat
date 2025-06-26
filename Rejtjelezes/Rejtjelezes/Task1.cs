using System.Collections.Generic;
using System.Linq;
using System;

namespace Rejtjelezes
{
    public class Task1
    {
        static char[] hungarianLetters = { 'ö', 'ü', 'ó', 'ő', 'ú', 'é', 'á', 'ű', 'í' };

        public static bool IsValidMessage(string message)
        {
            return message.All(c => Char.IsLetter(c) && !hungarianLetters.Contains(c) || c == ' ');
        }

        public static List<int> ConvertStringToNumbers(string convertable)
        {
            List<int> numbersList = new List<int>();

            foreach (char c in convertable.ToLower())
            {
                if (c == ' ')
                {
                    numbersList.Add(26);
                }
                else
                {
                    numbersList.Add(((int)c % 32) - 1);
                }
            }
            return numbersList;
        }

        public static string ConvertNumbersToString(List<int> numbersList)
        {
            string solution = "";
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            foreach (int number in numbersList)
            {
                if (number == 26)
                {
                    solution += ' ';
                }
                else
                {
                    solution += alphabet[number];
                }

            }
            return solution.ToLower();
        }

        public static string Encode(string message, string key)
        {
            if (!IsValidMessage(message) || !IsValidMessage(key))
            {
                throw new ArgumentException("Az üzenet vagy a kulcs nem érvényes karaktereket tartalmaz!");
            }
            if (key.Length < message.Length)
            {
                throw new ArgumentException("A kulcsnak legalább olyan hosszúnak kell lennie mint az üzenetnek!");
            }

            var messageNumbers = ConvertStringToNumbers(message);
            var keyNumbers = ConvertStringToNumbers(key);

            List<int> codedNumbers = new List<int>();

            for (int i = 0; i < messageNumbers.Count; i++)
            {
                int number = messageNumbers[i] + keyNumbers[i];
                if (number > 26)
                    number %= 27;
                codedNumbers.Add(number);
            }
            return ConvertNumbersToString(codedNumbers);
        }

        public static string Decode(string codedMessage, string key)
        {
            if (!IsValidMessage(codedMessage) || !IsValidMessage(key))
            {
                throw new ArgumentException("Az üzenet vagy a kulcs nem érvényes karaktereket tartalmaz!");
            }
            if (key.Length < codedMessage.Length)
            {
                throw new ArgumentException("A kulcsnak legalább olyan hosszúnak kell lennie mint az üzenetnek!");
            }

            var codedNumbers = ConvertStringToNumbers(codedMessage);
            var keyNumbers = ConvertStringToNumbers(key);
            List<int> decodedNumbers = new List<int>();

            for (int i = 0; i < codedNumbers.Count; i++)
            {
                int number = codedNumbers[i] - keyNumbers[i];
                if (number < 0)
                {
                    number += 27;
                }
                decodedNumbers.Add(number);
            }

            return ConvertNumbersToString(decodedNumbers);
        }
    }
}
