using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rejtjelezes;
using System;
using System.Collections.Generic;
using System.IO;

namespace AutoTesting
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void EncodeingDecodingTest1()
        {
            string message = "hello world";
            string key = "xmckl xmckl";

            string encoded = Task1.Encode(message, key);
            string decoded = Task1.Decode(encoded, key);

            Assert.AreEqual(message, decoded);
        }

        [TestMethod]
        public void EncodeingDecodingTest2()
        {
            string message = "my name is not important";
            string key = "guggaga baba yaga bish bash";

            string encoded = Task1.Encode(message, key);
            string decoded = Task1.Decode(encoded, key);

            Assert.AreEqual(message, decoded);
        }

        [TestMethod]
        public void FindingKeyTest1()
        {
            string message1 = "chair like the dog and the cat";
            string message2 = "classic hate is the cause";
            string key = "gugu gaga baba yaga bish bash bosh";

            string encoded1 = Task1.Encode(message1, key);
            string encoded2 = Task1.Encode(message2, key);

            Task2.StartFindingKeySegment(encoded1, encoded2);

            foreach (string possibleKey in Task2.possibleKeys)
            {
                //sok lehetséges kulcs lehet ami értelmes szavakból álló mondatot ad vissza
                if(Task1.Decode(encoded1, possibleKey) == message1 && Task1.Decode(encoded2, possibleKey) == message2)
                {
                    Assert.IsTrue(true);
                    return;
                }
            }
            Assert.Fail("Nem találtam kulcsot");
        }

        [TestMethod]
        public void FindingKeyTest2()
        {
            string message1 = "hey i just meet you and this is crazy";
            string message2 = "but here is my number so call me maybe";
            string key = "wahahahahajekjfl awkjela akwjelakja asdawdasdwas";

            string encoded1 = Task1.Encode(message1, key);
            string encoded2 = Task1.Encode(message2, key);

            Task2.StartFindingKeySegment(encoded1, encoded2);

            foreach (string possibleKey in Task2.possibleKeys)
            {
                //sok lehetséges kulcs lehet ami értelmes szavakból álló mondatot ad vissza
                if (Task1.Decode(encoded1, possibleKey) == message1 && Task1.Decode(encoded2, possibleKey) == message2)
                {
                    Assert.IsTrue(true);
                    return;
                }
            }
            Assert.Fail("Nem találtam kulcsot");
        }

        [TestMethod]
        public void FindingKeyTest3()
        {
            string message1 = "bad";
            string message2 = "hell";
            string key = "asdefasjdlka  as";

            string encoded1 = Task1.Encode(message1, key);
            string encoded2 = Task1.Encode(message2, key);

            Task2.StartFindingKeySegment(encoded1, encoded2);

            foreach (string possibleKey in Task2.possibleKeys)
            {
                //sok lehetséges kulcs lehet ami értelmes szavakból álló mondatot ad vissza
                if (Task1.Decode(encoded1, possibleKey) == message1 && Task1.Decode(encoded2, possibleKey) == message2)
                {
                    Assert.IsTrue(true);
                    return;
                }
            }
            Assert.Fail("Nem találtam kulcsot");
        }

        [TestMethod]
        public void FindingKeyTest4()
        {
            string message1 = "curiosity killed the cat";
            string message2 = "early bird catches the worm";
            string key = "i walked to burger king than i walked back home from burger king";

            string encoded1 = Task1.Encode(message1, key);
            string encoded2 = Task1.Encode(message2, key);

            Task2.StartFindingKeySegment(encoded1, encoded2);

            foreach (string possibleKey in Task2.possibleKeys)
            {
                //sok lehetséges kulcs lehet ami értelmes szavakból álló mondatot ad vissza
                if (Task1.Decode(encoded1, possibleKey) == message1 && Task1.Decode(encoded2, possibleKey) == message2)
                {
                    Assert.IsTrue(true);
                    return;
                }
            }
            Assert.Fail("Nem találtam kulcsot");
        }
    }
}
