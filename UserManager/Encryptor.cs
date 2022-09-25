using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

public static class Encryptor
{
    private static SHA256 sha256Encryptor;

    /// <summary>
    /// Initialises the encryptor class
    /// </summary>
    public static void Initialise()
    {
        sha256Encryptor = SHA256.Create();
    }

    /// <summary>
    /// Encrypts an inputted string into the SHA256 format hash
    /// </summary>
    /// <param name="toEncrypt">The string to encrypt</param>
    /// <returns>The encrypted string in a hexadecimal format</returns>
    public static string sha256Encrypt(string toEncrypt)
    {
        byte[] hashBytes = sha256Encryptor.ComputeHash(Encoding.UTF8.GetBytes(toEncrypt));

        StringBuilder hashBuilder = new StringBuilder();

        foreach (byte hashByte in hashBytes)
        {
            hashBuilder.Append(hashByte.ToString("X2")); // X2 means to 2 digit hexadecimal
        }

        return hashBuilder.ToString().ToLower();
    }
}
