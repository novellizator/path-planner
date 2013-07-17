using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TomyMaps
{
    class Helpers
    {
        public static void Swap<T>(T a, T b)
        {
            T tmp = a;
            a = b;
            b = tmp;
        }
    }
    /// <summary>
    /// This class takes care about the algebraic (physical) representation
    /// of the map only
    /// </summary>
    class Map
    {
        private string[] map; // the map itself
        public string[] getRawMap()
        {
            return map;
        }

        // width/height of a map in characters (basic resolution)
        private int charWidth = 0;
        private int charHeight = 0;
        public int getRawMapWidth()
        {
            return charWidth;
        }
        public int getRawMapHeight()
        {
            return charHeight;
        }

        public void Load(string filename)
        {
            StreamReader sr = new StreamReader(filename);
            string line;
            line = sr.ReadLine();
            if (line != "type octile")
            {
                throw new Exception("Wrong file format!");
            }

            // load the width and height
            string l1 = sr.ReadLine();
            string l2 = sr.ReadLine();
            string[] sline1 = l1.Split();
            string[] sline2 = l2.Split();

            // assuming I first read the height
            this.charHeight = int.Parse(sline1[1]);
            this.charWidth = int.Parse(sline2[1]);

            if (sline1[0] == "width")
            {
                Helpers.Swap(charWidth, charHeight);
            }


            line = sr.ReadLine(); // reads the word "map"

            this.map = new string[this.charHeight];
            for (int i = 0; i < this.charHeight; i++)
            {
                this.map[i] = sr.ReadLine();
            }

            sr.Close(); // so that I can edit the text file right after loading it 
        }

    }

}
