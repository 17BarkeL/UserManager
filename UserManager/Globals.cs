using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Globals
{
    /// <summary>
    /// Welcomes the user at the start of the program
    /// </summary>
    public static void WelcomeMessage()
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;

        Console.WriteLine(
            @"  _    _                 __  __                                       " + "\n" +
            @" | |  | |               |  \/  |                                      " + "\n" +
            @" | |  | |___  ___ _ __  | \  / | __ _ _ __   __ _  __ _  ___ _ __     " + "\n" +
            @" | |  | / __|/ _ \ '__| | |\/| |/ _` | '_ \ / _` |/ _` |/ _ \ '__|    " + "\n" +
            @" | |__| \__ \  __/ |    | |  | | (_| | | | | (_| | (_| |  __/ |       " + "\n" +
            @"  \____/|___/\___|_|    |_|  |_|\__,_|_| |_|\__,_|\__, |\___|_|       " + "\n" +
            @" By                                                __/ |  Messy Code  " + "\n" +
            @" Luke Barkess                                     |___/   Edition     " + "\n\n\n");

        Console.ResetColor();
    }

    /// <summary>
    /// Shows the user the menu and handles the input
    /// </summary>
    public static void Menu()
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("--------------");
        Console.ResetColor();
        Console.Write("What would you like to do:\n(C) Create new user\n(L) Login as user\n(U) List users\n(D) Delete user\n(Q) Quit\n");
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("--------------");
        Console.ResetColor();

        string choice = Console.ReadLine().ToUpper();
        Console.WriteLine();

        switch (choice)
        {
            case "C":
                UserMethods.CreateUser();
                break;
            case "L":
                UserMethods.LoginUser();
                break;
            case "U":
                UserMethods.ListUsers();
                break;
            case "D":
                UserMethods.DeleteUser();
                break;
            case "Q":
                Environment.Exit(0);
                break;
            default:
                Menu();
                break;
        }
    }
}
