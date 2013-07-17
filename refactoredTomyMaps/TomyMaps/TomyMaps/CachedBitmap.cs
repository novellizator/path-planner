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

        public void DrawBitmapInto(Graphics g, Point TLPoint, Size ViewPortSize, int squareS)
        {
            // squareSize has changed OR no map has been loaded so far
            if (cachedBitmap == null)
            {
                PrecomputeBitmap(TLPoint, ViewPortSize);
            }
            if (squareSize != squareS)
            {
                squareSize = squareS;
                PrecomputeBitmap(TLPoint, ViewPortSize);
            }
            

            // if the area is precomputable and is not precomputed
            if (false && false)
            {
                PrecomputeBitmap(TLPoint, ViewPortSize);
            }

            //g.DrawLine(new Pen(Color.Red), new Point(0, 0), new Point(100, 100));
            //g.DrawImageUnscaled(cachedBitmap,0,0,1000,1000);

            // Draw it on the buffered bitmap
            bufferedBitmapGraphics.Clear(Color.Red);
            Point relativeTLPoint = new Point(cachedBitmapTLPoint.X - TLPoint.X, cachedBitmapTLPoint.Y - TLPoint.Y);
            Rectangle rect = new Rectangle(relativeTLPoint, ViewPortSize);
            bufferedBitmapGraphics.DrawImageUnscaled(cachedBitmap, rect);


            g.DrawImageUnscaled(bufferedBitmap, 0, 0);
        }



        private void PrecomputeBitmap(Point TLPoint, Size viewPortSize)
        {
            
            cachedBitmapTLPoint = new Point(Math.Max(0, floorToSquareSize(TLPoint.X - viewPortSize.Width / 2)),
                Math.Max(0, floorToSquareSize(TLPoint.Y - viewPortSize.Height/ 2)));

            // (in pixels) floored according to the multiple of SquareSize - hopefully correct...
            int width = Math.Min(viewPortSize.Width * 2, getMapPixelSize().Width - cachedBitmapTLPoint.X);
            int height = Math.Min(viewPortSize.Height * 2, getMapPixelSize().Height - cachedBitmapTLPoint.Y);

            System.Windows.Forms.MessageBox.Show("precomputed!" + width);

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
                    Color col;

                    // map coordinates use topleftpoint for offset
                    switch (map.getRawMap()[baseHeight + i][baseWidth + j])
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
                            col = Color.Red;
                            break;
                    }
                    currBrush.Color = col;
                    cachedBitmapGraphics.FillRectangle(currBrush, squareSize * j, squareSize * i, squareSize, squareSize);

                }
            }

            cachedBitmap.Save("D:/bitmap.bmp");

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
