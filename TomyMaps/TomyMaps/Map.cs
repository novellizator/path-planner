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
        public static void Swap<T>(T a, T b)
        {
            T tmp = a;
            a = b;
            b = tmp;
        }
    }
    /// <summary>
    /// This class takes care of the map algebraic representation 
    /// and about drawing it on the canvas using graphic library.
    /// It takes arguments like desired squareSize, algebraic representation of the map, and resolution of the field.
    /// Returns map image...
    /// </summary>
    class Map
    {
        private string[] map; // the most important data structure holds the whole character map

        public Size WindowSize // size of the Window where the image is outputted
        {
            get;
            set;
        }

        public int SquareSize // == ZoomSize
        {
            get;
            set{
                if (value > 10 || value <1)
	            {
		            return;
            	}
            }
        }

        // maybe redundant...
        private Bitmap cachedBitmap = null;

        // width/height of a map in characters (basic resolution)
        private int charWidth;
        private int charHeight;

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

        public void Draw(ref Canvas c, Point p, int squareSize)
        {
            c.SetPenWidth(1);

            c.SetColor(Color.Yellow);

            int iTo = Math.Min(c.Height / squareSize, this.charHeight);
            int jTo = Math.Min(c.Width / squareSize, this.charWidth);

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
        private void PrecomputeMapPortion()
        {
            Canvas c = new Canvas(charWidth * SquareSize, charHeight * SquareSize);
            Draw(ref c, new Point(0, 0), SquareSize);
            cachedBitmap = c.Finish();
            // System.Windows.Forms.MessageBox.Show("prepomputed portion");

        }

        public Bitmap DrawSelection(Size zoomedMapSize, Point TLPoint)
        {
            if (cachedBitmap == null)
            {
                // throw new Exception("no cached bitmap");
                PrecomputeMapPortion();
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
