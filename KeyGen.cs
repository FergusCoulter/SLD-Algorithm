using System;
using System.Collections.Generic;
using System.Text;

namespace Key_Generator
{
    class KeyGen
    {
        public  int count = 0;
        public  bool check = false;
        public  List<string[]> subs = new List<string[]>();

        public KeyGen()
        {

        }
        public List<string[]> GenerateKeys(string key)
        {
            do
            {
                string[] subKeys = new string[2];

                //Split MK into chunks
                var chunks = ChunkifyKey(key);
                //Scramble bits
                subKeys = ScrambleBits(chunks);
                //Output 2 subkeys
                //recurse with new key
                subs.Add(subKeys);

                var newKey = subKeys[0] + subKeys[1];
                count++;
                if (count < 7)
                {
                    GenerateKeys(newKey);
                }
                else
                {
                    check = true;
                }
            } while (!check);


            return subs;
        }
        string[] ChunkifyKey(string key)
        {
            string[] chunks = new string[16];
            int index = 0;
            for (int i = 0; i < 16; i++)
            {
                var chunk = key.Substring(index, 8);
                if (i < 14)
                {
                    index += 8;
                }
                else
                {
                    index += 7;
                }

                chunks[i] = chunk;
            }

            return chunks;
        }

        string[] ScrambleBits(string[] chunks)
        {
            bool check = false;
            string[] subKeys = new string[2];

            int i = 0;

            List<string[]> pValues = new List<string[]>();
            string[] p0 = new string[8];
            string[] p1 = new string[8];




            for (int x = 0; x < 16; x += 2)
            {
                var sequence = chunks[x] + chunks[x + 1];
                char[] newSequence = new char[16];

                var charsOfSequence = sequence.ToCharArray();

                //Permute
                newSequence[0] = charsOfSequence[1];
                newSequence[1] = charsOfSequence[5];
                newSequence[2] = charsOfSequence[15];
                newSequence[3] = charsOfSequence[7];
                newSequence[4] = charsOfSequence[11];
                newSequence[5] = charsOfSequence[3];
                newSequence[6] = charsOfSequence[0];
                newSequence[7] = charsOfSequence[9];
                newSequence[8] = charsOfSequence[10];
                newSequence[9] = charsOfSequence[4];
                newSequence[10] = charsOfSequence[14];
                newSequence[11] = charsOfSequence[6];
                newSequence[12] = charsOfSequence[13];
                newSequence[13] = charsOfSequence[8];
                newSequence[14] = charsOfSequence[12];
                newSequence[15] = charsOfSequence[2];

                var stringify = new string(newSequence);

                string p = stringify.Substring(0, 8);
                string q = stringify.Substring(8, 8);
                p0[i] = p;
                p1[i] = q;
                i++;
            }
            pValues.Add(p0);
            pValues.Add(p1);


            //Create SubKey 1
            var subkey1 = CreateSubKey(0, pValues);

            //Create SubKey 2
            var subkey2 = CreateSubKey(1, pValues);

            subKeys[0] = subkey1;
            subKeys[1] = subkey2;
            return subKeys;
        }
         string CreateSubKey(int index, List<string[]> pValues)
        {
            string subKey = "";
            var placeHolder = "00000000";
            string[] subKeyChunks = new string[8];
            for (int y = 0; y < subKeyChunks.Length; y++)
            {
                subKeyChunks[y] = placeHolder;
            }

            if (index == 0)
            {
                var p0 = pValues[index];
                int[] vals = { 0, 1, 2, 3, 4, 5, 6, 7 };
                LinkedList<int> place = new LinkedList<int>(vals);
                var last = place.Last;
                place.RemoveLast();
                place.AddFirst(last);

                int asc = 0;
                int dsc = 7;

                for (int x = 0; x < subKeyChunks.Length; x++)
                {
                    var chunk = subKeyChunks[x];
                    var charChunk = chunk.ToCharArray();

                    var part1 = p0[asc];
                    var part2 = p0[dsc];

                    for (int i = 0; i < charChunk.Length; i++)
                    {
                        var n = place.First.Value;
                        //Adapted from Chris,2012
                        var result = Convert.ToBoolean(char.GetNumericValue(part1[i])) ^ Convert.ToBoolean(char.GetNumericValue(part2[n]));
                        //End of Adapted Code
                        var convertedResult = ' ';
                        if (result is true)
                        {
                            convertedResult = '1';
                        }
                        else
                        {
                            convertedResult = '0';
                        }
                        charChunk[i] = convertedResult;



                        var hld = place.Last;
                        place.RemoveLast();
                        place.AddFirst(hld);
                    }
                    subKeyChunks[x] = new string(charChunk);
                    asc++;
                    dsc--;

                }

                foreach (var chunk in subKeyChunks)
                {
                    subKey += chunk.ToString();
                }

                return subKey;
            }
            else
            {
                var p0 = pValues[index];
                int[] vals = { 0, 1, 2, 3, 4, 5, 6, 7 };
                LinkedList<int> place = new LinkedList<int>(vals);
                var last = place.Last;
                place.RemoveLast();
                place.AddFirst(last);

                int asc = 0;
                int dsc = 7;

                for (int x = 0; x < subKeyChunks.Length; x++)
                {
                    var chunk = subKeyChunks[x];
                    var charChunk = chunk.ToCharArray();

                    var part1 = p0[asc];
                    var part2 = p0[dsc];

                    for (int i = 0; i < charChunk.Length; i++)
                    {
                        var n = place.First.Value;
                        //Adapted from Chris, 2012
                        var result = Convert.ToBoolean(char.GetNumericValue(part1[i])) ^ Convert.ToBoolean(char.GetNumericValue(part2[n]));
                        //End of Adapted Code
                        var convertedResult = ' ';
                        if (result is true)
                        {
                            convertedResult = '1';
                        }
                        else
                        {
                            convertedResult = '0';
                        }
                        charChunk[i] = convertedResult;



                        var hld = place.Last;
                        place.RemoveLast();
                        place.AddFirst(hld);
                    }
                    subKeyChunks[x] = new string(charChunk);
                    asc++;
                    dsc--;

                }

                foreach (var chunk in subKeyChunks)
                {
                    subKey += chunk.ToString();
                }

                return subKey;
            }

        }
    }
}

