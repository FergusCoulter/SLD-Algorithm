using System;
using System.Collections.Generic;
using System.Threading;

namespace Key_Generator
{
    class Program
    {


        static void Main(string[] args)
        {
            string masterKey = "1100001010011100110010001010010011001110101010101101011010110000110111000110010011100100011010101110101001110000111100000101111";

            var gen1 = new KeyGen();

            var KeySchedule = gen1.GenerateKeys(masterKey);


            Algorithm algorithm = new Algorithm(KeySchedule);

            algorithm.Encrypt();




        }


        private static string Avalanche(List<string[]> keyScheduleOne, List<string[]> keyScheduleTwo)
        {
            var difference = "";
            var diffCount = 0;
            var lengthCount = 1024;

            for (int i = 0; i < keyScheduleOne.Count; i++)
            {
                var keySet1 = keyScheduleOne[i];
                var keySet2 = keyScheduleTwo[i];

                for (int x = 0; x < keySet1.Length; x++)
                {
                    var ogKey = keySet1[x];
                    var checkKey = keySet2[x];

                    for (int y = 0; y < ogKey.Length; y++)
                    {
                        var a = ogKey[y];
                        var b = checkKey[y];

                        if (a != b)
                        {
                            diffCount++;
                        }
                    }
                }
            }






            difference = diffCount.ToString();

            return difference;

        }
    }
}




