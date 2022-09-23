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
        static string dataPath = Directory.GetCurrentDirectory() + @"\Data";

        // I know i'm not using multiple files its bad

        static void Main(string[] args)
        {
            Initialise();
            WelcomeMessage();
            Menu();

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
            Console.Write("What would you like to do:\n(C) Create new user\n(L) Login as user\n(U) List users\n(Q) Quit\n");
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
                    LoginUser();
                    break;
                case "U":
                    ListUsers();
                    break;
                case "Q":
                    Environment.Exit(0);
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
            Console.WriteLine("User created successfully\n");
            Console.ResetColor();

            string previousData = ReadData();
            WriteData(previousData + username + ":" + sha256Encrypt(passwordBuilders[0].ToString()) + "\n");

            Menu();
        }

        /// <summary>
        /// Attempts to login to an existing user using inputted username and password
        /// </summary>
        static void LoginUser()
        {
            string username = "";
            int usernameAttempts = -1;

            while (username == "" || !ReadData().Contains(username + ":"))
            {
                usernameAttempts++;

                if (usernameAttempts > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: There is no user with that name\n");
                    Console.ResetColor();
                }

                Console.Write("Enter your username: ");
                username = Console.ReadLine();
            }

            List<string> fileDataLines = ReadData().Split(new string[] {"\n"}, StringSplitOptions.None).ToList();
            string passwordHash = "";

            foreach (string line in fileDataLines)
            {
                if (line.Contains(username + ":"))
                {
                    int indexOfData = fileDataLines.IndexOf(line);
                    string[] splitData = fileDataLines[indexOfData].Split(Char.Parse(":"));
                    passwordHash = splitData[1];
                }
            }

            StringBuilder passwordBuilder = new StringBuilder();
            ConsoleKeyInfo keyInfo;

            string password = "";

            Console.Write("Enter your password: ");

            while (true)
            {
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
                password = passwordBuilder.ToString();

                if (sha256Encrypt(password) == passwordHash)
                {
                    break;
                }

                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Password incorrect\n");
                    Console.ResetColor();

                    passwordBuilder.Clear();

                    Console.Write("Enter your password: ");
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Successfully logged in as " + username + "\n");
            Console.ResetColor();

            Menu();
        }

        /// <summary>
        /// Displays all users usernames
        /// </summary>
        static void ListUsers()
        {
            string[] dataLines = ReadData().Split(Char.Parse("\n"));

            Console.WriteLine("Users:");

            foreach (string line in dataLines)
            {
                string[] splitLine = line.Split(Char.Parse(":"));
                Console.WriteLine(splitLine[0]);
            }

            Menu();
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
