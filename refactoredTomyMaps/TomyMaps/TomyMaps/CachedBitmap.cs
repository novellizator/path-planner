using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

namespace TomyMaps
{
    // this implementation caches one bitmap only
    class CachedBitmap
    {

        private int _squareSize = 1;
        private Size _viewPortSize = new Size(1, 1);
        private Point _tlpoint = new Point(0, 0);


        // this is what I'm manipulating with
        private Bitmap cachedBitmap = null;
        private Point cachedBitmapTLPoint = new Point(0, 0); // relative 


        // the cachedBitmap needs to be buffered(because of g.clear())
        // and the just drawn...
        private Bitmap bufferedBitmap = null;
        private Graphics bufferedBitmapGraphics = null;

        public void DrawBitmapInto(Graphics g, Point TLPoint, Size ViewPortSize, int squareSize)
        {
            if (squareSize != _squareSize)
            {
                PrecomputeBitmap(TLPoint);
            }
            // if bitmap is not precomputed and is precomputable
            if (false)
            {
                PrecomputeBitmap();
            }


        }



        private void PrecomputeBitmap(Point TLPoint)
        {
            cachedBitmapTLPoint = new Point(Math.Max(0, floorToSquareSize(TLPoint.X - _viewPortSize.Width / 2)),
                Math.Max(0, floorToSquareSize(TLPoint.Y - _viewPortSize.Width / 2)));

            // (in pixels) floored according to the multiple of SquareSize - hopefully correct...
            int width = Math.Min(_viewPortSize.Width * 2, getMapPixelSize().Width - cachedBitmapTLPoint.X);
            int height = Math.Min(_viewPortSize.Height * 2, getMapPixelSize().Height - cachedBitmapTLPoint.Y);
            
            cachedBitmap = new Bitmap(width, height);
            Graphics cachedBitmapGraphics = Graphics.FromImage(cachedBitmap);




            SolidBrush currBrush = new SolidBrush(Color.White);


            int chWidth = width / _squareSize;
            int chHeight = height / _squareSize;

            // topleft point of cachedBitmap in chars...
            int baseWidth = cachedBitmapTLPoint.X / _squareSize;
            int baseHeight = cachedBitmapTLPoint.Y / _squareSize;

            for (int i = 0; i < chHeight; i++) // pocet riadkov // nepojde presne lebo riadok != pixel!!!!
            {
                for (int j = 0; j < chWidth; j++)
                {
                    Color col;

                    // map coordinates use topleftpoint for offset
                    switch (map[baseHeight + i][baseWidth + j])
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
                    cachedBitmapGraphics.FillRectangle(currBrush, _squareSize * j, _squareSize * i, _squareSize, _squareSize);

                }
            }

            bufferedBitmap = new Bitmap(Math.Max(cachedBitmap.Width, _viewPortSize.Width), Math.Max(cachedBitmap.Height, _viewPortSize.Height));
            bufferedBitmapGraphics = Graphics.FromImage(bufferedBitmap);

        }



        private Size getMapPixelSize()
        {
            return new Size(charWidth * SquareSize, charHeight * SquareSize);
        }
        private int floorToSquareSize(int size)
        {
            return size / _squareSize * _squareSize;
        }
        private Size floorToSquareSize(Size s)
        {
            return new Size(floorToSquareSize(s.Width), floorToSquareSize(s.Height));
        }
    }
}
