using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using System.Drawing;
namespace TomyMaps
{
    class Helpers
    {
        public static void Swap<T>(ref T a, ref T b)
        {
            T tmp = a;
            a = b;
            b = tmp;
        }
    }
    class Map
    {
        //change later on to private 
        public string[] map;

        //public char this[int y, int x]
        //{
        //    get { return map[x][y]; }
        //    private set;
        //}

        private int width;
        private int height;

        
        public int Width
        {
            get { return width; }
            set { return; }
        }
        public int Height
        {
            get {return height;}
            set { return; }
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
            this.height = int.Parse(sline1[1]);
            this.width = int.Parse(sline2[1]);

            if (sline1[0] == "width")
            {
                Helpers.Swap(ref this.width, ref this.height);
            }
            line = sr.ReadLine(); // "map"

            this.map = new string[height];
            for (int i = 0; i < this.height; i++)
            {
                this.map[i] = sr.ReadLine();
            }

            sr.Close(); // so that I can edit the text file after loading it 
        }

        public void Draw(Canvas c, Point p, int squareSize)
        {
            if (p == null)
                p = new Point(0, 0);



            // MessageBox.Show(Width.ToString() + "--" + Height.ToString());
            c.SetPenWidth(1);

            c.SetColor(Color.Yellow);
            //c.DrawNormalRectangle(0, 0, c.Width, c.Height);

            int iTo = Math.Min(c.Height / squareSize, this.height);
            int jTo = Math.Min(c.Width / squareSize, this.width);

            for (int i = 0; i < iTo; i++) // pocet riadkov
            {
                for (int j = 0; j < jTo; j++)
                {
                    Color col;

                    switch (map[i][j])
                    {
                        case 'W':
                            col = Color.Blue;
                            break;
                        case 'T':
                            col = Color.Green;
                            break;
                        case '@':
                            col = Color.Gray;
                            break;
                        case 'S':
                        case '.':
                            col = Color.PapayaWhip;
                            break;
                        default:
                            col = Color.Red;
                            break;
                    }
                    c.SetColor(col);
                    c.gr.FillRectangle(c.currBrush, squareSize * j, squareSize * i, squareSize, squareSize);
                }
            }

        }

        public void Draw(Canvas c)
        {
            Draw(c, new Point(0, 0), 3);
        }
    }
}
