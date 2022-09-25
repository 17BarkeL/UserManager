using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Security.Cryptography;

namespace UserManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Initialise();
            Globals.WelcomeMessage();
            Globals.Menu();

            Console.ReadLine();
        }

        /// <summary>
        /// Initialising the program
        /// </summary>
        static void Initialise()
        {
            DataFile.Initialise();
            Encryptor.Initialise();
        }
    }
}
