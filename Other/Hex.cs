using System;
using System.Collections.Generic;
using System.Text;

namespace Patcher.Other
{
    internal class Hex
    {
        public static ReadOnlySpan<string> HexTable => new string[]
{
                "0", "1", "2", "3", "4", "5", "6", "7",
                "8", "9", "A", "B", "C", "D", "E", "F"
        };

        public static int GetDecimal(char hex)
        {
            if (Char.IsDigit(hex))
            {
                return Convert.ToInt32(hex);
            }
            else
            {
                for (int i = 0; i < HexTable.Length; i++)
                {
                    if (hex.ToString() == HexTable[i])
                    {
                        return i;
                    }
                }

                return -1;
            }
        }

        public static string HexFormat(byte[] bytes)
        {
            List<string> resulthex = new List<string>();
            foreach (byte @byte in bytes)
            {
                int lastd = @byte;
                for (int i = 0; i < 2; i++)
                {
                    int d = lastd / 16;
                    int p = lastd % 16;
                    resulthex.Add(Hex.HexTable[p].ToString());
                    lastd = d;
                }
                resulthex.Reverse();
            }
            return string.Join(' ', resulthex);
        }

        public static string HexFormat(int @decimal)
        {
            char[] resulthex = new char[2];
            int lastd = @decimal;
            for (int i = 0; i < 2; i++)
            {
                int d = lastd / 16;
                int p = lastd % 16;
                resulthex[i] = Convert.ToChar(Hex.HexTable[p].ToString());
                lastd = d;
            }
            Array.Reverse(resulthex);
            return new string(resulthex);
        }

        public static int DecimalFormat(string hex)
        {
            int result = 0;
            string formathex = hex.Replace("0x", "");

            for (int i = 0; i < formathex.Length; i++)
            {
                char c = formathex[formathex.Length - 1 - i];
                double pow = Math.Pow(16, i);
                if (Char.IsDigit(c))
                {
                    if (int.TryParse(c.ToString(), out int convert))
                    {
                        result += (convert * (int)pow);
                    }
                }
                else
                {
                    int getDecimal = Hex.GetDecimal(c);
                    if (getDecimal != -1)
                    {
                        result += (getDecimal * (int)pow);
                    }
                }
            }

            return result;
        }
    }
}
