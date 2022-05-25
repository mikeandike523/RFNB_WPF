using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Ink;

namespace RFNB_UWP
{
    internal class Utilities
    {

        public static readonly string ApplicationRoot = "RealFeelNotebook";

        public static string GetFolderPath(string relativePath, bool createIfNotExists =  true) {
            string LocalAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string absolutePath = Path.Combine(LocalAppData, ApplicationRoot, relativePath);
            if (!Directory.Exists(absolutePath))
            {
                if (createIfNotExists)
                {
                    Directory.CreateDirectory(absolutePath);
                }
                else {
                    throw new DirectoryNotFoundException("Could not find directory and createIfNotExists was not specified.");
                }
            }
            return absolutePath;
        }

        public static string GetFilePath(string relativePath) {
            return Path.Combine(GetFolderPath(""), relativePath);
        }

        public static void SaveInkCanvasStrokesToFile(string relativePath, InkCanvas canvas) { 
            FileStream fs = new FileStream(GetFilePath(relativePath), FileMode.Create);
            canvas.Strokes.Save(fs);
            fs.Close();
        }

        public static void LoadStrokesFromFile(string relativePath, InkCanvas canvas) {
            FileStream fs = new FileStream(GetFilePath(relativePath),FileMode.Open);
            canvas.Strokes = new System.Windows.Ink.StrokeCollection(fs);
            fs.Close();
        }

        public static System.Windows.Ink.StrokeCollection LoadStrokeCollectionFromFile(string relativePath) {
            FileStream fs = new FileStream(GetFilePath(relativePath),FileMode.Create);
            System.Windows.Ink.StrokeCollection collection = new System.Windows.Ink.StrokeCollection(fs);
            fs.Close();
            return collection;
        }

        public static System.Windows.Ink.StrokeCollection SaveStrokeCollectionToFile(string relativePath, StrokeCollection collection)
        {
            FileStream fs = new FileStream(GetFilePath(relativePath), FileMode.Create);
            collection.Save(fs);
            fs.Close();
            return collection;
        }

        public static string JoinPaths(params string[] paths) {
            return Path.Combine(paths);
        }

    }
}
