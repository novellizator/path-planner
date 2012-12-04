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
    /// <summary>
    /// This class takes care of the map algebraic representation 
    /// and about drawing it on the canvas it using graphic library
    /// </summary>
    class Map
    {
        private string[] map;
        public Bitmap cachedBitmap = null;

        // width/height of a map in characters (basic resolution)
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

        public void setSquareSize(int size)
        {
            PrecomputeMapPortion(size);
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

        public void Draw(ref Canvas c, Point p, int squareSize)
        {
            c.SetPenWidth(1);

            c.SetColor(Color.Yellow);

            int iTo = Math.Min(c.Height / squareSize, this.height);
            int jTo = Math.Min(c.Width / squareSize, this.width);

            for (int i = 0; i < iTo; i++) // pocet riadkov // nepojde presne lebo riadok != pixel!!!!
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

        public void Draw(ref Canvas c)
        {
            Draw(ref c, new Point(0, 0), 3);
        }


        /// <summary>
        /// Creates a bitmap (at least a large so that it fits in the zoomedMap picturebox. (In alpha version it is the whole map - up to 76MB)
        /// Of which we later draw only a selection.
        /// </summary>
        public void PrecomputeMapPortion(int squareSize)
        {
            Canvas c = new Canvas(width * squareSize, height * squareSize);
            Draw(ref c, new Point(0, 0), squareSize);
            cachedBitmap = c.Finish();
            // System.Windows.Forms.MessageBox.Show("prepomputed portion");

        }

        public Bitmap DrawSelection(Size zoomedMapSize, Point TLPoint)
        {
            if (cachedBitmap == null)
            {
                // throw new Exception("no cached bitmap");
                PrecomputeMapPortion(1);
            }

        
            //System.Windows.Forms.MessageBox.Show("-" + TLPoint.X+" "+ TLPoint.Y+" "+ zoomedMapSize.Width+" "+ zoomedMapSize.Height);
            Rectangle selectionRect = new Rectangle(TLPoint.X, TLPoint.Y,
                Math.Min(zoomedMapSize.Width, cachedBitmap.Width) - TLPoint.X,
                Math.Min(zoomedMapSize.Height, cachedBitmap.Height) - TLPoint.Y);

            // Rectangle selectionRect = new Rectangle(200, 200, 500, 500);
            System.Drawing.Imaging.PixelFormat format = cachedBitmap.PixelFormat;

           return cachedBitmap.Clone(selectionRect, format);

            // cachedBitmap.Save("D:/cach.bmp", System.Drawing.Imaging.ImageFormat.Bmp);

        
        }
    }
}
