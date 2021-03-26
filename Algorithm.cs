using System;
using System.Collections.Generic;
using System.Text;

namespace Key_Generator
{
    class Algorithm
    {
        private int count = 0;
        private bool check = false;
        List<string[]> keys { get; set; }
        private SBox sbox = new SBox();
        private string cipherText = "";

        public Algorithm(List<string[]> keys)
        {
            this.keys = keys;
        }

        public void Encrypt()
        {
            var plaintext = "0100000101000010010000110100010001000101010001100100011101001000";
            //var plaintext2 = "0110011101100101011100100110011101110101011100110110001101101111";

            var result = Round(plaintext);
           // var result2 = Round(plaintext2);
            

            Console.WriteLine(result);
            //Console.WriteLine(result2);
            //Console.WriteLine(Avalanche(result, result2));
        }
        public void Decrypt()
        {
            var ciphertext = "0101010011011001100111111000001011110111000011101001010111110111";

            var result = ReverseRound(ciphertext);

            Console.WriteLine(result);
        }
        private string Round(string text)
        {
            string result = "";
            do
            {


                var pairOfKeys = keys[count];
                var keyOne = pairOfKeys[0];
                var keyTwo = pairOfKeys[1];

                var R = XOR(text, keyOne);
                var C = FX(R);
                var P = XOR(C, keyTwo);
                var cipher = PBoxFunction(P);
                result = cipher;
                
                if (count < 6)
                {
                    count++;
                    Round(cipher);
                }
                else
                {
                    check = true;
                }
                

                
            } while (!check);

            return result;
        }
        private char[] XOR(string text, string keyOne)
        {
            var R = new char[64];
            for (int i = 0; i < text.Length; i++)
            {
                var digit = text[i];
                var keyDigit = keyOne[i];
                //Adapted from Chris, 2012
                var result = Convert.ToBoolean(char.GetNumericValue(digit)) ^ Convert.ToBoolean(char.GetNumericValue(keyDigit));
                //End of Adapted Code

                char xorDigit;
                if (result is true)
                {
                    xorDigit = '1';
                }
                else
                {
                    xorDigit = '0';
                }
                R[i] = xorDigit;
            }

            return R;
        }
        private string FX(char[] R)
        {

            string[] chunks = new string[8];
            int index = 0;
            for (int i = 0; i < 8; i++)
            {
                var chunk = new string(R).Substring(index, 8);
                if (i < 6)
                {
                    index += 8;
                }
                else
                {
                    index += 7;
                }

                chunks[i] = chunk;
            }

            var order = PermuteChunks(chunks);

            var C = SBoxFunction(order);

            return C;
        }
        private string[] PermuteChunks(string[] chunks)
        {
            var pChunks = new string[8];

            pChunks[0] = chunks[3];
            pChunks[1] = chunks[4];
            pChunks[2] = chunks[0];
            pChunks[3] = chunks[1];
            pChunks[4] = chunks[7];
            pChunks[5] = chunks[6];
            pChunks[6] = chunks[5];
            pChunks[7] = chunks[2];

            return pChunks;
        }
        private string SBoxFunction(string[] chunks)
        {
            List<char[]> C = new List<char[]>();
            string concat = "";
            foreach(var chunk in chunks)
            {
                var MSN = chunk.Substring(4);
                var LSN = chunk.Substring(0,4);

                var value = sbox.getValue(MSN,LSN);

                C.Add(value);

            }

            foreach(var value in C)
            {
                concat += new string(value);
            }
            var x = concat.Length;
            return concat;
        }

        private string PBoxFunction(char[] text)
        {
            string[] chunks = new string[8];
            int index = 0;
            List<string> preConcat = new List<string>();
            var newR = "";
            for (int i = 0; i < 8; i++)
            {
                var chunk = new string(text).Substring(index, 8);
                if (i < 6)
                {
                    index += 8;
                }
                else
                {
                    index += 7;
                }

                chunks[i] = chunk;
            }

            foreach(var chunk in chunks)
            {
                var permuted = PBox(chunk);
                preConcat.Add(permuted);
            }

            foreach(var permutedChunk in preConcat)
            {
                newR += permutedChunk;
            }

            return newR;

        }
        private string PBox(string chunk)
        {
            var perm = new char[8];

            perm[0] = chunk[5];
            perm[1] = chunk[2];
            perm[2] = chunk[4];
            perm[3] = chunk[6];
            perm[4] = chunk[0];
            perm[5] = chunk[3];
            perm[6] = chunk[7];
            perm[7] = chunk[1];

            return new string(perm);
        }


        private string ReverseRound(string text)
        {
            string result = "";
            count = 6;
            do
            {

                var pairOfKeys = keys[count];
                var keyOne = pairOfKeys[0];
                var keyTwo = pairOfKeys[1];

                var cipher = ReversePBoxFunction(text);
                var P = XOR(new string(cipher), keyTwo);
                var C = ReverseFX(P);
                var R = XOR(C, keyOne);
                
                
                
                result = new string(R);

                if (count < 6)
                {
                    count--;
                    ReverseRound(new string(R));
                }
                else
                {
                    check = true;
                }



            } while (!check);

            return result;
        }

        private char[] ReversePBoxFunction(string text)
        {
           

            //Split into 8 chunks
            string[] chunks = new string[8];
            int index = 0;
            List<string> preConcat = new List<string>();
            var newR = "";
            for (int i = 0; i < 8; i++)
            {
                var chunk = (text).Substring(index, 8);
                if (i < 6)
                {
                    index += 8;
                }
                else
                {
                    index += 7;
                }

                chunks[i] = chunk;
            }
            //Do Reverse P-Box
            foreach (var chunk in chunks)
            {
                var permuted = ReversePBox(chunk);
                preConcat.Add(permuted);
            }
            //collate chunks and output char[]
            foreach (var permutedChunk in preConcat)
            {
                newR += permutedChunk;
            }

            return newR.ToCharArray();

        }
        private string ReversePBox(string chunk)
        {
            var perm = new char[8];

            perm[5] = chunk[0];
            perm[2] = chunk[1];
            perm[4] = chunk[2];
            perm[6] = chunk[3];
            perm[0] = chunk[4];
            perm[3] = chunk[5];
            perm[7] = chunk[6];
            perm[1] = chunk[7];

            return new string(perm);
        }

        private string ReverseFX(char[] R)
        {
            var text = new string(R);

            string[] chunks = new string[8];
            int index = 0;
            for (int i = 0; i < 8; i++)
            {
                var chunk = text.Substring(index, 8);
                if (i < 6)
                {
                    index += 8;
                }
                else
                {
                    index += 7;
                }

                chunks[i] = chunk;
            }

            var x = ReverseSBoxFunction(chunks);

        }
        private string ReverseSBoxFunction(string[] chunks)
        {

        }
        private string ReversePermuteChunks()
        {

        }
















        private int Avalanche(string text1, string text2)
        {
            int diffCount = 0; 

            for(int i =0; i< text1.Length; i++)
            {
                var p = text1[i];
                var c = text2[i];

                if(p != c)
                {
                    diffCount++;
                }
            }
            return diffCount;
        }


    }
}
