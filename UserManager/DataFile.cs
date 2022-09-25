using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public static class DataFile
{
    private static string dataPath = Directory.GetCurrentDirectory() + @"\Data";

    /// <summary>
    /// Initialises the data file
    /// </summary>
    public static void Initialise()
    {
        if (!File.Exists(dataPath))
        {
            File.Create(dataPath).Close();
        }
    }

    /// <summary>
    /// Writes the inputted data to the data file
    /// </summary>
    /// <param name="toWrite">The string to write to the data file</param>
    public static void WriteData(string toWrite)
    {
        File.WriteAllText(dataPath, toWrite);
    }

    /// <summary>
    /// Gets the data files contents as a string
    /// </summary>
    /// <returns>The file contents as a string</returns>
    public static string ReadData()
    {
        return File.ReadAllText(dataPath);
    }

    /// <summary>
    /// Clears all of the data file
    /// </summary>
    public static void ClearData()
    {
        WriteData("");
    }
}
