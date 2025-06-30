using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Rejtjelezes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Kérem válasszon az alábbi opciók közül!");
                Console.WriteLine("Titkosítás (1-es gomb), Dekódolás (2-es gomb), Kulcs kitalálás (3-as gomb), Kilépés (4-es gomb)");

                string i = Console.ReadLine();

                switch (i)
                {
                    case "1":
                        EncodeMenu();
                        break;
                    case "2":
                        DecodeMenu();
                        break;
                    case "3":
                        KeyDecodeMenu();
                        break;
                    case "4":
                        Environment.Exit(0);
                        break;
                }
            }
        }

        static void KeyDecodeMenu()
        {
            //Console.WriteLine(Task1.Encode("kill me please", "help me please help me please"));
            //Console.WriteLine(Task1.Encode("want to die very much", "help me please help me please"));

            string[] words = File.ReadAllLines("words.txt");

            Task2.FindKeySegment(Task1.Encode("cat play on the tree", "csacska macska feher nyuszi"), Task1.Encode("we love rain very much", "csacska macska feher nyuszi"), "", words);
            foreach (string key in Task2.possibleKeys)
            {
                Console.WriteLine($"Possible key: {key}, the messages based on it: {Task1.Decode(Task1.Encode("cat play on the tree", "csacska macska feher nyuszi"), key)}, the other: {Task1.Decode(Task1.Encode("we love rain very much", "csacska macska feher nyuszi"), key)}");
            }
        }

        static void EncodeMenu()
        {
            Console.WriteLine("Kérem adja meg az üzenetet!");
            string message = Console.ReadLine().ToLower();

            Console.WriteLine("Kérem adja meg a kulcsot!");
            string key = Console.ReadLine().ToLower();

            try
            {
                string encoded = Task1.Encode(message, key);
                Console.WriteLine($"A kódolt üzenet: {encoded}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void DecodeMenu()
        {
            Console.WriteLine("Kérem adja meg a kódolt üzenetet!");
            string codedMessage = Console.ReadLine().ToLower();

            Console.WriteLine("Kérem adja meg a kulcsot!");
            string key = Console.ReadLine().ToLower();

            try
            {
                string decoded = Task1.Decode(codedMessage, key);
                Console.WriteLine($"A megfejtett üzenet: {decoded}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

