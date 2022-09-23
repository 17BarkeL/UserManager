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
            CreateUser();
            // Menu();
        }

        static void CreateUser()
        {
            Console.Write("What's your username: ");
            string username = Console.ReadLine();

            Console.Write("Enter your password: ");
            SecureString password = new SecureString();
            ConsoleKeyInfo keyInfo;

            do
            {
                keyInfo = Console.ReadKey(true);

                if (!char.IsControl(keyInfo.KeyChar))
                {
                    password.AppendChar(keyInfo.KeyChar);
                    Console.Write("*");
                }

                else if (keyInfo.Key == ConsoleKey.Backspace && password.Length != 0)
                {
                    password.RemoveAt(password.Length - 1);
                    Console.Write("\b \b");
                }

            } while (keyInfo.Key != ConsoleKey.Enter);
        }
        
        static void Menu()
        {
            Console.WriteLine("What would you like to do:\n(C) Create new user,\n(L) Login as user");
            string choice = Console.ReadLine();
        }
    }
}
