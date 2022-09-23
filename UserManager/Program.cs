using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;

namespace UserManager
{
    class Program
    {
        static void Main(string[] args)
        {
            WelcomeMessage();
            Menu();

            Console.ReadLine();
        }

        static void Menu()
        {
            Console.WriteLine("What would you like to do:\n(C) Create new user,\n(L) Login as user");
            string choice = Console.ReadLine().ToUpper();

            switch (choice)
            {
                case "C":
                    Console.WriteLine(CreateUser());
                    break;
            }
        }

        static string CreateUser()
        {
            Console.Write("What's your username: ");
            string username = Console.ReadLine();

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

            return passwordBuilder.ToString();
        }

        static void WelcomeMessage()
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine(
                "  _    _                 __  __                                       \n" +
                " | |  | |               |  \\/  |                                     \n" +
                " | |  | |___  ___ _ __  | \\  / | __ _ _ __   __ _  __ _  ___ _ __    \n" +
                " | |  | / __|/ _ \\ '__| | |\\/| |/ _` | '_ \\ / _` |/ _` |/ _ \\ '__|\n" +
                " | |__| \\__ \\  __/ |    | |  | | (_| | | | | (_| | (_| |  __/ |     \n" +
                "  \\____/|___/\\___|_|    |_|  |_|\\__,_|_| |_|\\__,_|\\__, |\\___|_| \n" +
                "                                                   __/ |              \n" +
                " By Luke Barkess                                  |___/               \n\n\n");

            Console.ResetColor();
        }
    }
}
