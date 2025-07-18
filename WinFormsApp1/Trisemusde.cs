using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    internal class Trisemusde
    {
        public static string Decrypt(string input)
        {
            char[,] alfrus = {     {'Z', 'Y', 'X', 'W', 'V', 'U', 'T'},
                                   {'S', 'R', 'Q', 'P', 'O', 'N', 'M'},
                                   {'L', 'K', 'J', 'I', 'H', 'G', 'F'},
                                   {'E', 'D', 'C', 'B', 'A', '9', '8'},
                                   { '7','6', '5', '4', '3', '2', '1'},
                                   {'0', ' ', ',', '.', '!', '?', ';'}

                               };


            string message = input;
            string new_message = "";



            for (int i = 0; i < message.Length; i++)
            {
                for (int j = 0; j < alfrus.GetLength(0); j++)
                    for (int k = 0; k < alfrus.GetLength(1); k++)
                        if (Char.ToLower(alfrus[j, k]) == message[i] 
                            || Char.ToUpper(alfrus[j, k]) == message[i])
                        {
                            int x = j+ 1;
                            int y = k;
                            if (x == 7)
                            {
                                x = 1;
                            }
                            new_message += alfrus[x, y];
                            break;
                        }

            }
            return new_message;
        }
    }
}
   
