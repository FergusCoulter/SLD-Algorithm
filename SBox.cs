using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Key_Generator
{
    class SBox
    {
        private static string[,] sbox = new string[16,16];
        private static List<string> values = new List<string>();

        //Code Adapted from Configurater, 2011
        private static readonly Dictionary<char, string> binary = new Dictionary<char, string> {
    { '0', "0000" },
    { '1', "0001" },
    { '2', "0010" },
    { '3', "0011" },
    { '4', "0100" },
    { '5', "0101" },
    { '6', "0110" },
    { '7', "0111" },
    { '8', "1000" },
    { '9', "1001" },
    { 'a', "1010" },
    { 'b', "1011" },
    { 'c', "1100" },
    { 'd', "1101" },
    { 'e', "1110" },
    { 'f', "1111" }
};
        //End of Adapted Code

        //Code Adapted from Configurater, 2011
        private static readonly Dictionary<string, string> rowscols = new Dictionary<string, string> {
    {  "0000","0" },
    { "0001","1" },
    {  "0010","2" },
    { "0011","3" },
    { "0100","4" },
    { "0101","5" },
    {"0110","6" },
    { "0111","7" },
    { "1000","8" },
    { "1001","9" },
    { "1010","10" },
    {  "1011","11" },
    {  "1100","12" },
    {  "1101","13" },
    { "1110","14" },
    { "1111","15" }
};
        //End of Adapted Code
        public SBox()
        {
            Init();
        }
        private void Init() {
            var count = 0;
            using (var reader = new TextFieldParser(@"D:\sbox.csv"))
            {
                reader.SetDelimiters(",");

                while(!reader.EndOfData)
                {
                    string[] line = reader.ReadFields();
                    foreach(var value in line)
                    {
                        values.Add(value);
                    }

                }
            }

            for(int r =0; r<16; r++)
            {
                for(int c=0; c<16; c++)
                {
                    sbox[r, c] = values[count];
                    count++;
                }
            }
            Console.WriteLine(sbox);
        }

        public char[] getValue(string MSN, string LSN)
        {
            char[] returnValue = new char[8];

            var hexVal = sbox[int.Parse(rowscols[LSN]), int.Parse(rowscols[MSN])];

            string half1 = "";
            string half2 = "";
            if(hexVal.Length == 2)
            {
                 half1 = binary[hexVal[0]];
                 half2 = binary[hexVal[1]];
            }
            else if (hexVal.Length == 1)
            {
                 half1 = "0000";
                 half2 = binary[hexVal[0]];
            }
            var full = half1 + half2;
            returnValue = full.ToCharArray();
            return returnValue;



        }
    }
}
