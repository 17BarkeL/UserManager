using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.IO;

namespace UserManager
{
    class Program
    {
        const string dataPath = @"W:\008. Computing\Student work\2022-2023\Y12\Luke Barkess\UserManager\Data";

        static void Main(string[] args)
        {
            Initialise();
            WelcomeMessage();
            Menu();

            Console.ReadLine();
        }

        static void Initialise()
        {
            if (!File.Exists(dataPath))
            {
                File.Create(dataPath).Close();
            }
        }

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

        static string CreateUser()
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

            return passwordBuilders[0].ToString();
        }

        static void WriteData(string toWrite)
        {
            File.WriteAllText(dataPath, toWrite);
        }

        static string ReadData()
        {
            return File.ReadAllText(dataPath);
        }
    }
}
