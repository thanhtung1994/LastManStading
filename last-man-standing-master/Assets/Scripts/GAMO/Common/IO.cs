using System;
using System.Text;
using UnityEngine;
#if UNITY_WINRT
using UnityEngine.Windows;
#else
using System.IO;
#endif

namespace GAMO.Common
{
    public static class IO
    {
        /// <summary>
        /// Determines whether the specified file exists.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool Exists(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// Opens a binary file, reads the contents of the file into a byte array, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open for reading.</param>
        /// <returns></returns>
        public static byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        /// <summary>
        /// Opens a text file, reads all lines of the file into a string, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open for reading.</param>
        /// <returns></returns>
        public static string ReadAllText(string path)
        {
#if UNITY_WINRT
            var data = ReadAllBytes(path);
            return Encoding.UTF8.GetString(data, 0, data.Length);
#else
            return File.ReadAllText(path);
#endif
        }

        /// <summary>
        /// Creates a new file, writes the specified byte array to the file, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="path">The file to write to.</param>
        /// <param name="data">The bytes to write to the file.</param>
        public static void WriteAllBytes(string path, byte[] data)
        {
            File.WriteAllBytes(path, data);
        }

        /// <summary>
        /// Creates a new file, write the contents to the file, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="path">The file to write to.</param>
        /// <param name="data">The string to write to the file.</param>
        public static void WriteAllText(string path, string data)
        {
#if UNITY_WINRT
        WriteAllBytes   (path, Encoding.UTF8.GetBytes(data));
#else
            File.WriteAllText(path, data);
#endif
        }

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="path"></param>
        public static void Delete(string path)
        {
            File.Delete(path);
        }

        public static void SaveDataFile(string data, string fileName)
        {
            data = Ulti.Base64Encode(data);
            string path = GetDataPath(fileName);
            Debug.Log("Save file: " + path);
            WriteAllText(path, data);
        }

        public static string LoadDataFile(string fileName)
        {
            string path = GetDataPath(fileName);
            if (Exists(path))
            {
                Debug.Log("Load file: " + path);
                string data = ReadAllText(path);
                return Ulti.Base64Decode(data);
            }
            return string.Empty;
        }

        private static string GetDataPath(string fileName)
        {
            string path = "";
#if UNITY_IPHONE && !UNITY_EDITOR
        //string fileNameBase = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/')); 
        //path = fileNameBase.Substring(0, fileNameBase.LastIndexOf('/')) + "/Documents/" + fileName; 
        path = Application.persistentDataPath + "/" + fileName; 
#elif UNITY_ANDROID && !UNITY_EDITOR
        path = Application.persistentDataPath + "/" + fileName; 
#else
            path = Application.dataPath + "/" + fileName;
#endif
            return path;
        }
    }
}