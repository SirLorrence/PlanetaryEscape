using System.IO;
using UnityEngine;

namespace Tools
{
    public class FileReaderWriter : MonoBehaviour
    {
        ///<summary>
        ///Writes string from a given File
        ///</summary>
        public static void WriteString(string path, string contents)
        {
            string fullPath = Application.persistentDataPath + "/" + path;
        
            if (File.Exists(fullPath))
            {
                StreamWriter writer = new StreamWriter(fullPath, true);
                writer.WriteLine(contents);
                writer.Close();
            }
            else
            {
                File.Create(fullPath);
                StreamWriter writer = new StreamWriter(fullPath, true);
                writer.WriteLine(contents);
                writer.Close();
            }
        }

        ///<summary>
        ///Reads string from a given File
        ///</summary>
        public static string ReadString(string path)
        {
            string fullPath = Application.persistentDataPath + "/" + path;

            if (File.Exists(fullPath))
            {
                StreamReader reader = new StreamReader(path);

                string contents = reader.ReadToEnd();
                reader.Close();

                return contents;
            }
            else
            {
                print("No file found at " + fullPath);
                return "";
            }
        }
    }
}
