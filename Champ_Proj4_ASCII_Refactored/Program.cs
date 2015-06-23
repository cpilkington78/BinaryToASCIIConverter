using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Champ_Proj4_ASCII_Refactored
{
    class Program
    {
        enum ErrorCodes
        {
            NotCompleteByte,
            NoFile,
            BlankFile,
            InvalidFile,
            OK
        }

        static void Main(string[] args)
        {
            while (true)
            {
                string userInput = GetUserInput();

                CheckForQuit(userInput);

                if (!BinaryCheck(userInput))
                {
                    if (!ReadFile(userInput, ref userInput))
                    {
                        Console.WriteLine(GetErrorMessage(ErrorCodes.NoFile));
                        continue;
                    }
                }

                Process(userInput);
            }
        }

        private static string GetUserInput()
        {
            Console.WriteLine("Enter a binary message at the prompt, enter a path to a binary text file, or type \"Quit\" to exit the program.\n");
            return Console.ReadLine().ToLower();
        }

        private static void CheckForQuit(string userInput)
        {
            if (userInput == "quit")
            {
                Console.WriteLine("Goodbye.");
                Thread.Sleep(1500);
                Environment.Exit(0);
            }
        }

        private static bool BinaryCheck(string userInput)
        {
            foreach (char c in userInput)
            {
                if (c != '0' && c != '1')
                    return false;
            }
            return true;
        }

        private static int ByteCheck(string binString)
        {
            int bitCount = 0;
            int mod;

            foreach (char c in binString)
            {
                bitCount += 1;
            }

            mod = bitCount % 8;
            return mod;
        }

        private static bool ReadFile(string strFile, ref string data)
        {
            if (File.Exists(strFile))
            {
                using (StreamReader myReader = new StreamReader(strFile))
                {
                    data = myReader.ReadToEnd().Replace("\r", "").Replace("\n", "");
                }
                return true;
            }
            return false;
        }

        private static ErrorCodes BinValidate(string binString)
        {
            if (binString == "")
                return ErrorCodes.BlankFile;

            else if (!BinaryCheck(binString))
                return ErrorCodes.InvalidFile;

            else if (ByteCheck(binString) != 0)
                return ErrorCodes.NotCompleteByte;

            else
                return ErrorCodes.OK;
        }

        private static string MessageCreate(string binString)
        {
            StringBuilder userMessage = new StringBuilder();

            for (int i = 0; i < binString.Length; i += 8)
            {
                string userBinaryString = binString.Substring(i, 8);
                byte binaryInput = Convert.ToByte(userBinaryString, 2);
                char byteResult = Convert.ToChar(binaryInput);
                userMessage.Append(byteResult);
            }
            return userMessage.ToString();
        }

        private static void PrintMessage(string userMessage)
        {
            Console.WriteLine("\nYour message converted from binary to ASCII is:\n{0}\n", MessageCreate(userMessage));
        }

        private static string GetErrorMessage(ErrorCodes err)
        {
            switch (err)
            {
                case ErrorCodes.NotCompleteByte:
                    return "\nInvalid message.  Binary message must be byte incremented (8 bits) for binary to ASCII character conversion to function properly.\n";
                case ErrorCodes.NoFile:
                    return "\nFile does not exist.\n";
                case ErrorCodes.BlankFile:
                    return "\nInvalid binary text file.  File is blank.\n";
                case ErrorCodes.InvalidFile:
                    return "\nInvalid binary text file.  File does not contain binary values.\n";
                default:
                    return "\nUnknown error.\n";
            }
        }

        private static void Process(string userInput)
        {
            ErrorCodes errorCode = BinValidate(userInput);
            if (errorCode == ErrorCodes.OK)
                PrintMessage(userInput);
            else
                Console.WriteLine(GetErrorMessage(errorCode));
        }
    }
}
