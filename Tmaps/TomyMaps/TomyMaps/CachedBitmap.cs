using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;


using System.Windows.Forms; // for debug purposes only...

namespace TomyMaps
{
    // this implementation caches one bitmap only
    class CachedBitmap
    {

        private int squareSize = 1;

        // indicated whether to colorize the map or just use colors for passable areas
        private bool isBichromatic = false; 

        // this is what I'm manipulating with
        private Bitmap cachedBitmap = null;
        private Point cachedBitmapTLPoint = new Point(0, 0); // absolute 

        private Map map;
        public void setMap(Map m)
        {
            this.map = m;
        }

        // the cachedBitmap needs to be buffered(because cacheBitmapGraphics.Clear() shows for a millisecond the default background)
        // and the just drawn from...
        private Bitmap bufferedBitmap = null;
        private Graphics bufferedBitmapGraphics = null;


        public void DrawBitmapInto(Graphics g, Point TLPoint, Size ViewPortSize, int squareS, bool isBichrom, bool forcePrecomputing = false)
        {
            // squareSize has changed OR no map has been loaded so far
            if (cachedBitmap == null ||
                squareSize != squareS ||
                cachedBitmapTLPoint.X > TLPoint.X || cachedBitmapTLPoint.Y > TLPoint.Y ||
                (cachedBitmapTLPoint.X + cachedBitmap.Width < getMapPixelSize().Width &&
                 cachedBitmapTLPoint.X + cachedBitmap.Width < TLPoint.X + ViewPortSize.Width) ||
                (cachedBitmapTLPoint.Y + cachedBitmap.Height < getMapPixelSize().Height &&
                 cachedBitmapTLPoint.Y + cachedBitmap.Height < TLPoint.Y + ViewPortSize.Height) ||
                isBichrom != isBichromatic ||
                forcePrecomputing
            )
            {
                //System.Windows.Forms.MessageBox.Show("cachedBitmap == null");
                squareSize = squareS;
                isBichromatic = isBichrom;
                PrecomputeBitmap(TLPoint, ViewPortSize);
            }

    

            // Draw it on the buffered bitmap
            bufferedBitmapGraphics.Clear(Color.White);
            Point relativeTLPoint = new Point(cachedBitmapTLPoint.X - TLPoint.X, cachedBitmapTLPoint.Y - TLPoint.Y);
            Rectangle rect = new Rectangle(relativeTLPoint, ViewPortSize);
            bufferedBitmapGraphics.DrawImageUnscaled(cachedBitmap, rect);


            g.DrawImageUnscaled(bufferedBitmap, 0, 0);
        }

        private Color getColorByChar(char c)
        {

            Color col;


            if (isBichromatic)
            {
                switch (c)
                {
                    case 'S':
                    case '.':
                        col = Color.PapayaWhip;
                        break;

                    default:
                        col = Color.DarkSlateGray;
                        break;
                }
            }
            else
            {
                switch (c)
                {
                    case 'W': // Water
                        col = Color.Blue;
                        break;
                    case 'T': // Tree
                        col = Color.Green;
                        break;
                    case '@': // Outside
                        col = Color.Gray;
                        break;
                    case 'S': // Swamp (traversable)
                    case '.': // default traversable area
                        col = Color.PapayaWhip;
                        break;
                    default: // Error
                        col = Color.Gray;
                        break;
                }
            }

            // path coloring
            switch (c)
            {
                case 'C':
                    col = Color.DarkOrange;
                    break;
                case 'P':
                    col = Color.Red;
                    break;
            }


            return col;
        }
        private void PrecomputeBitmap(Point TLPoint, Size viewPortSize)
        {
            
            cachedBitmapTLPoint = new Point(Math.Max(0, floorToSquareSize(TLPoint.X - viewPortSize.Width / 2)),
                Math.Max(0, floorToSquareSize(TLPoint.Y - viewPortSize.Height/ 2)));

            // (in pixels) floored according to the multiple of SquareSize - hopefully correct...
            int width = Math.Min(viewPortSize.Width * 2, getMapPixelSize().Width - cachedBitmapTLPoint.X);
            int height = Math.Min(viewPortSize.Height * 2, getMapPixelSize().Height - cachedBitmapTLPoint.Y);

            

            // let's draw on the cached bitmap
            cachedBitmap = new Bitmap(width, height);
            Graphics cachedBitmapGraphics = Graphics.FromImage(cachedBitmap);

            SolidBrush currBrush = new SolidBrush(Color.White);


            int chWidth =  cachedBitmap.Width / squareSize;
            int chHeight = cachedBitmap.Height / squareSize;

            // topleft point of cachedBitmap in chars...
            int baseWidth = cachedBitmapTLPoint.X / squareSize;
            int baseHeight = cachedBitmapTLPoint.Y / squareSize;

            for (int i = 0; i < chHeight; i++) // pocet riadkov // nepojde presne lebo riadok != pixel!!!!
            {
                for (int j = 0; j < chWidth; j++)
                {

                    char charSquare = map.getRawMap()[baseHeight + i][baseWidth + j];
                    
                    // "default "color determined by the map
                    Color col = getColorByChar(charSquare);

                    // if the color is detemrined by data, then the "default" color will be overridden
                    char[,] rawdata = map.getRawData();
                    if (rawdata != null)
	                {
                        char dataSquare = map.getRawData()[baseHeight + i,baseWidth + j];
                        if (dataSquare != ' ')
                        {
                            col = getColorByChar(dataSquare);
                        }
		 
	                }
                    

                    currBrush.Color = col;
                    cachedBitmapGraphics.FillRectangle(currBrush, squareSize * j, squareSize * i, squareSize, squareSize);

                }
            }


            bufferedBitmap = new Bitmap(Math.Max(cachedBitmap.Width, viewPortSize.Width), Math.Max(cachedBitmap.Height, viewPortSize.Height));
            //bufferedBitmap = (Bitmap)cachedBitmap.Clone(); // bad experiment, then the rest wont be redrawn... (zoomout - leftovers)
            bufferedBitmapGraphics = Graphics.FromImage(bufferedBitmap);

        }



        private Size getMapPixelSize()
        {
            return new Size(map.getRawMapWidth() * squareSize, map.getRawMapHeight() * squareSize);
        }
        private int floorToSquareSize(int size)
        {
            return size / squareSize * squareSize;
        }
        private Size floorToSquareSize(Size s)
        {
            return new Size(floorToSquareSize(s.Width), floorToSquareSize(s.Height));
        }
    }
}
