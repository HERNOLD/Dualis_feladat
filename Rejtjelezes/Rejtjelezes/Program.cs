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
            Task2.possibleKeys = new List<string>();

            Console.WriteLine("Kérem adja meg az első üzenetet! (még nem kódolva)");
            string message1=Console.ReadLine();
            Console.WriteLine("Kérem adja meg a második üzenetet! (még nem kódolva)");
            string message2 = Console.ReadLine();
            //nem kapja meg a kulcsot maga a kitaláló program, de így egyszerűbb a felhasználónak
            Console.WriteLine("Kérem adja meg a közös kulcsot!");
            string key=Console.ReadLine();

            string codedMessage1 = Task1.Encode(message1, key);
            string codedMessage2=Task1.Encode(message2 , key);

            Task2.StartFindingKeySegment(codedMessage1, codedMessage2);
            foreach (string possibleKey in Task2.possibleKeys)
            {
                Console.WriteLine($"Egyik lehetséges kulcs: {possibleKey}, az üzenetek a kulcs alapján: {Task1.Decode(codedMessage1, possibleKey)}, {Task1.Decode(codedMessage2, possibleKey)}");
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

