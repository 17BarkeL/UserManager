using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Security.Cryptography;
using System.IO;

namespace UserManager
{
    class Program
    {
        static SHA256 sha256Encrypter;
        const string dataPath = @"W:\008. Computing\Student work\2022-2023\Y12\Luke Barkess\UserManager\Data";

        static void Main(string[] args)
        {
            Initialise();
            WelcomeMessage();
            //Menu();
            LoginUser();

            Console.ReadLine();
        }

        /// <summary>
        /// Initialising the program
        /// </summary>
        static void Initialise()
        {
            if (!File.Exists(dataPath))
            {
                File.Create(dataPath).Close();
            }

            sha256Encrypter = SHA256.Create();
        }

        /// <summary>
        /// Welcomes the user at the start of the program
        /// </summary>
        static void WelcomeMessage()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;

            Console.WriteLine(
                @"  _    _                 __  __                                       " + "\n" +
                @" | |  | |               |  \/  |                                      " + "\n" +
                @" | |  | |___  ___ _ __  | \  / | __ _ _ __   __ _  __ _  ___ _ __     " + "\n" +
                @" | |  | / __|/ _ \ '__| | |\/| |/ _` | '_ \ / _` |/ _` |/ _ \ '__|    " + "\n" +
                @" | |__| \__ \  __/ |    | |  | | (_| | | | | (_| | (_| |  __/ |       " + "\n" +
                @"  \____/|___/\___|_|    |_|  |_|\__,_|_| |_|\__,_|\__, |\___|_|       " + "\n" +
                @"                                                   __/ |              " + "\n" +
                @" By Luke Barkess                                  |___/               " + "\n\n\n");

            Console.ResetColor();
        }

        /// <summary>
        /// Shows the user the menu and handles the input
        /// </summary>
        static void Menu()
        {
            Console.Write("What would you like to do:\n(C) Create new user,\n(L) Login as user\n");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("--------------");
            Console.ResetColor();

            string choice = Console.ReadLine().ToUpper();
            Console.WriteLine();

            switch (choice)
            {
                case "C":
                    CreateUser();
                    break;
                case "L":
                    break;
            }
        }

        /// <summary>
        /// Creates a new user with inputted username and password and writes their data
        /// </summary>
        static void CreateUser()
        {
            Console.Write("What's your username: ");
            string username = Console.ReadLine();

            List<StringBuilder> passwordBuilders = new List<StringBuilder>();
            int passwordCreationFails = -1;

            while (passwordBuilders.Count == 0 || passwordBuilders[0].ToString() != passwordBuilders[1].ToString())
            {
                passwordCreationFails++;

                if (passwordCreationFails != 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Password must match\n");
                    Console.ResetColor();
                }

                passwordBuilders.Clear();

                for (int i = 0; i < 2; i++)
                {
                    if (i == 0)
                    {
                        Console.Write("Enter your password: ");
                    }

                    else
                    {
                        Console.Write("Enter your password again: ");
                    }

                    StringBuilder passwordBuilder = new StringBuilder();
                    ConsoleKeyInfo keyInfo;

                    do
                    {
                        keyInfo = Console.ReadKey(true);

                        if (!char.IsControl(keyInfo.KeyChar))
                        {
                            passwordBuilder.Append(keyInfo.KeyChar);
                            Console.Write("*");
                        }

                        else if (keyInfo.Key == ConsoleKey.Backspace && passwordBuilder.Length != 0)
                        {
                            passwordBuilder.Remove(passwordBuilder.Length - 1, 1);
                            Console.Write("\b \b");
                        }

                    } while (keyInfo.Key != ConsoleKey.Enter);

                    Console.WriteLine();

                    passwordBuilders.Add(passwordBuilder);
               }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("User created successfully");
            Console.ResetColor();

            string previousData = ReadData();
            WriteData(previousData + username + ":" + sha256Encrypt(passwordBuilders[0].ToString()) + "\n");
        }

        static void LoginUser()
        {
            do
            {
                Console.Write("Enter your username: ");
                string username = Console.ReadLine();
            } while (!ReadData().Contains(username + ":"));
            
            //Console.WriteLine("Error: There is no user with that name");

            Console.Write("Enter your password: ");

            StringBuilder passwordBuilder = new StringBuilder();
            ConsoleKeyInfo keyInfo;

            do
            {
                keyInfo = Console.ReadKey(true);

                if (!char.IsControl(keyInfo.KeyChar))
                {
                    passwordBuilder.Append(keyInfo.KeyChar);
                    Console.Write("*");
                }

                else if (keyInfo.Key == ConsoleKey.Backspace && passwordBuilder.Length != 0)
                {
                    passwordBuilder.Remove(passwordBuilder.Length - 1, 1);
                    Console.Write("\b \b");
                }

            } while (keyInfo.Key != ConsoleKey.Enter);

            string password = passwordBuilder.ToString();
        }
        
        /// <summary>
        /// Writes the inputted data to the data file
        /// </summary>
        /// <param name="toWrite">The string to write to the data file</param>
        static void WriteData(string toWrite)
        {
            File.WriteAllText(dataPath, toWrite);
        }

        /// <summary>
        /// Gets the data files contents as a string
        /// </summary>
        /// <returns>The file contents as a string</returns>
        static string ReadData()
        {
            return File.ReadAllText(dataPath);
        }

        /// <summary>
        /// Clears all of the data file
        /// </summary>
        static void ClearData()
        {
            WriteData("");
        }

        /// <summary>
        /// Encrypts an inputted string into the SHA256 format hash
        /// </summary>
        /// <param name="toEncrypt">The string to encrypt</param>
        /// <returns>The encrypted string in a hexadecimal format</returns>
        static string sha256Encrypt(string toEncrypt)
        {
            byte[] hashBytes = sha256Encrypter.ComputeHash(Encoding.UTF8.GetBytes(toEncrypt));

            StringBuilder hashBuilder = new StringBuilder();

            foreach (byte hashByte in hashBytes)
            {
                hashBuilder.Append(hashByte.ToString("X2")); // X2 means to 2 digit hexadecimal
            }

            return hashBuilder.ToString().ToLower();
        }
    }
}
