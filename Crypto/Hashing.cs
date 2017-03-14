using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Vivacity
{
    class Hashing
    {
        /// <summary>
        /// Calculates hash code for a string input given a header size: Task 1
        /// </summary>
        public static string getCode(string input, int header, int max_iterations = 10000000, string code = "----------")
        {
            //Initialise
            int iteration = 0;
            Console.WriteLine("input:\t" + input + "\t header:\t" + header);

            using (MD5 md5Hash = MD5.Create())
            {
                while (iteration < max_iterations)
                {
                    iteration++;

                    //Generate new hash
                    string target = input + iteration.ToString();
                    string hash = GetMd5Hash(md5Hash, target);

                    //IF header is correct: proceed ELSE get new hash
                    if (testHeader(hash, header))
                    {
                        //Find which character is to be updated
                        int code_position = Convert.ToInt32(hash[header].ToString(), 16);

                        //Find updated character's new value
                        int hash_position = iteration % 32;
                        char hash_char = hash[hash_position];

                        //Update character
                        char[] code_array = code.ToCharArray();
                        if ((code_position < code_array.Length) && (code_array[code_position] == '-'))
                        {
                            code_array[code_position] = hash_char;
                            code = new string(code_array);

                            Console.WriteLine(code);

                            //IF all characters have been updated: END
                            if (testComplete(code))
                            {
                                Console.WriteLine("Completed in " + iteration + " iterations.");
                                break;
                            }
                        }
                    }
                }
            }

            return code;
        }

        //Returns true if input has the required header
        static bool testComplete(string input, char empty = '-')
        {
            int match = 0;

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == empty) match++;
            }

            return (match > 0) ? false : true;
        }

        //Returns true if input has the required header
        static bool testHeader(string input, int header)
        {
            int match = 0;

            for (int i = 0; i < header; i++)
            {
                if (input[i] == '0') match++;
            }

            return (match == header) ? true : false;
        }

        //Source: https://msdn.microsoft.com/en-us/library/s02tk69a(v=vs.110).aspx
        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}

