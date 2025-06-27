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
            Console.WriteLine(Task1.Encode("amazing ability", "help me please help me"));
            Console.WriteLine(Task1.Encode("action actor", "help me please help me"));
            //Task2.FindPossibleKeys(Task1.Encode("amazing ability", "help me please help me"), Task1.Encode("action actor", "help me please help me"));
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

