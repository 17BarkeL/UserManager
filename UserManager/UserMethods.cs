using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class UserMethods
{
    /// <summary>
    /// Creates a new user with inputted username and password and writes their data
    /// </summary>
    public static void CreateUser()
    {
        string username = "";

        do
        {
            Console.Write("What's your username: ");
            username = Console.ReadLine();

            if (GetUsers().Contains(username))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: User already exists\n");
                Console.ResetColor();
            }
        } while (GetUsers().Contains(username));

        

        for (int attempts = 1; attempts <= 3; attempts++)
        {
            string password1 = GetPasswordInput();
            string password2 = GetPasswordInput();
            string finalPassword = "";
            
            if (password1 == password2)
            {
                finalPassword = password1;
                string previousData = DataFile.ReadData();
                DataFile.WriteData(previousData + username + ":" + Encryptor.sha256Encrypt(finalPassword) + "\n");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(username + " created successfully\n");
                Console.ResetColor();
                break;
            }

            else
            {
                if (attempts == 3)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Attempts failed\n");
                    Console.ResetColor();
                }

                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Passwords must match\n");
                    Console.ResetColor();
                }
            }
        }

        Globals.Menu();
    }

    /// <summary>
    /// Deletes a user by username with verification
    /// </summary>
    public static void DeleteUser()
    {
        string userToDelete = "";
        int usernameAttempts = -1;

        while (userToDelete == "" || !DataFile.ReadData().Contains(userToDelete + ":"))
        {
            usernameAttempts++;

            if (usernameAttempts > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: username does not exist\n");
                Console.ResetColor();
            }

            Console.Write("Enter the username of the user: ");
            userToDelete = Console.ReadLine();
        }

        if (VerifyAsUser(userToDelete))
        {
            List<string> dataLines = DataFile.ReadData().Split(Char.Parse("\n")).ToList();

            foreach (string line in dataLines.ToArray())
            {
                if (line.Contains(userToDelete))
                {
                    Console.WriteLine();
                    dataLines.Remove(line);
                }
            }

            string dataToWrite = "";

            foreach (string dataLine in dataLines)
            {
                if (dataLines.Last() != dataLine)
                {
                    dataToWrite += dataLine + "\n";
                }

                else
                {
                    dataToWrite += dataLine;
                }
            }

            DataFile.WriteData(dataToWrite);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("User successfully deleted\n");
            Console.ResetColor();
        }

        Globals.Menu();
    }

    /// <summary>
    /// Attempts to login to an existing user using inputted username and password
    /// </summary>
    public static void LoginUser()
    {
        string username = "";
        int usernameAttempts = -1;

        while (username == "" || !GetUsers().Contains(username))
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

        List<string> fileDataLines = DataFile.ReadData().Split(Char.Parse("\n")).ToList();
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

        VerifyAsUser(username);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Successfully logged in as " + username + "\n");
        Console.ResetColor();

        Globals.Menu();
    }

    /// <summary>
    /// Displays all users usernames
    /// </summary>
    public static void ListUsers()
    {
        string[] dataLines = DataFile.ReadData().Split(Char.Parse("\n"));

        Console.WriteLine("Users:");

        foreach (string line in dataLines)
        {
            string[] splitLine = line.Split(Char.Parse(":"));
            Console.WriteLine(splitLine[0]);
        }

        Globals.Menu();
    }

    /// <summary>
    /// Returns a list of all usernames in the data file
    /// </summary>
    /// <returns>The list of usernames</returns>
    public static List<string> GetUsers()
    {
        List<string> usernames = new List<string>();
        string[] dataLines = DataFile.ReadData().Split(Char.Parse("\n"));

        foreach (string line in dataLines)
        {
            string[] splitLine = line.Split(Char.Parse(":"));
            usernames.Add(splitLine[0]);
        }

        return usernames;
    }

    /// <summary>
    /// Asks the user to input a password
    /// </summary>
    /// <returns>The password inputted</returns>
    public static string GetPasswordInput()
    {
        StringBuilder passwordBuilder = new StringBuilder();
        ConsoleKeyInfo keyInfo;

        Console.Write("Enter your password: ");

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

        return passwordBuilder.ToString();
    }

    /// <summary>
    /// Verifies if someone has the password for a user
    /// </summary>
    /// <param name="username">The user to verify for</param>
    /// <returns>If the user was verified</returns>
    public static bool VerifyAsUser(string username)
    {
        int passwordFails = 0;

        while (passwordFails < 3)
        {
            string password = GetPasswordInput();

            Console.WriteLine();

            List<string> fileDataLines = DataFile.ReadData().Split(Char.Parse("\n")).ToList();
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

            if (Encryptor.sha256Encrypt(password) == passwordHash)
            {
                return true;
            }

            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Password incorrect\n");
                Console.ResetColor();
                passwordFails++;
            }
        }

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Password attempts failed verification cancelled.\n");
        Console.ResetColor();

        return false;
    }
}
