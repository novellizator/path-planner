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

        private Size _WindowSize;
        public Size WindowSize // size of the Window where the image is outputted
        {
            get
            {
                return _WindowSize;
            }
            set
            {
                _WindowSize = value;

                if (cachedBitmap == null)
                {
                    return;
                }

                // FIXME: later on....
                PrecomputeCachedBitmap();
                //Size maxPossibleImageSize = new Size(map[0].Length * SquareSize, map.Length * SquareSize);
                //// precomputing is not possible (at the end of a real bitmap)

                //if (cachedBitmap.Width + cachedBitmapTLPoint.X > maxPossibleImageSize.Width &&
                //    cachedBitmap.Height + cachedBitmapTLPoint.X > maxPossibleImageSize.Height)
                //{
                //    return;
                //}

                //// cele zle
                //if (WindowSize.Width > (cachedBitmap.Width + TLPoint.X )
                //    || WindowSize.Height > (cachedBitmap.Height + TLPoint.Y))
                //{
                //    PrecomputeCachedBitmap();
                //}
            }
        }

        // TODO: change the topleft point accordingly
        private int _SquareSize = 1;
        public int SquareSize // == ZoomSize
        {
            get
            {
                return _SquareSize;
            }
            set
            {
                if (value > 10 || value < 1)
                {
                    return;
                }
                else
                {
                    _SquareSize = value;

                    // The map has already been loaded with everything and consecvently 
                    // a cachedBitmap has been created. The problem in the past was, that I cannot precompute the bitmap when windowsize=0
                    if (cachedBitmap != null && WindowSize.Width > 0)
                    {
                        PrecomputeCachedBitmap();
                    }
                }
            }
        }

        // in pixels - the top left point in the non-existing whole map
        // I must remember the TLPoint of the part of the image - used for example when resizing with regard to actual TLPoint
        private Point _TLPoint = new Point(0, 0);
        public Point TLPoint
        {
            get
            {
                return _TLPoint;
            }
            set
            {
                _TLPoint = value;
            }
        }


        private Bitmap cachedBitmap = null;
        private Point cachedBitmapTLPoint = new Point(0, 0);

        // width/height of a map in characters (basic resolution)
        private int charWidth = 0;
        private int charHeight = 0;

        private Size getMapPixelSize()
        {
            System.Diagnostics.Debug.Assert(charWidth > 0 && charHeight  > 0);
            return new Size(charWidth * SquareSize, charHeight * SquareSize);
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

        /// <summary>
        /// With the given Canvas size and the TopLeft point, fills the canvas with squares. 
        /// Private method -> must be provided with the correct canvas size.
        /// </summary>
        /// <param name="c">Canvas</param>
        /// <param name="p">Top-Left point</param>
        private void DrawCachedBitmap(Canvas c, Point TLPoint)
        {
            c.SetPenWidth(1);
            c.SetColor(Color.Yellow);

            int chWidth = c.Width / SquareSize;
            int chHeight = c.Height / SquareSize;

            // topleft point of cachedBitmap in chars...
            int baseWidth = cachedBitmapTLPoint.X / SquareSize;
            int baseHeight = cachedBitmapTLPoint.Y / SquareSize;

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
                    c.SetColor(col);
                    c.gr.FillRectangle(c.currBrush, SquareSize * j, SquareSize * i, SquareSize, SquareSize);

                }
            }

        }
        /// <summary>
        /// Transform the Top-Left point to the coordinate of a Top-Left character. Uses SquareSize and .
        /// </summary>
        /// <param name="TLPoint">Top Left Point in pixels</param>
        /// <returns></returns>
        private Size TLPointToCoords(Point TLPoint)
        {
            return new Size(TLPoint.X / SquareSize, TLPoint.Y / SquareSize);
        }

        private Point CoordsToTLPoint(int charX, int charY)
        {
            return new Point(charX * SquareSize, charY * SquareSize);
        }

        private int floorToSquareSize(int size)
        {
            return size / SquareSize * SquareSize;
        }
        private Size floorToSquareSize(Size s)
        {
            return new Size(floorToSquareSize(s.Width), floorToSquareSize(s.Height));
        }

        /// <summary>
        /// Creates a canvas (computes the Canvas Size and lets the this.DrawCachedBitmap draw on the canvas) and the cachedBitmap. also the cachedBitmapTLPoint
        /// makes a canvas x*y so that the visible bitmap starts on [x/2, y/2]. The CachedbitmapSize is 2x times 2y.
        /// </summary>
        private int it = 1;
        private void PrecomputeCachedBitmap()
        {
            cachedBitmapTLPoint = new Point(Math.Max(0, floorToSquareSize(TLPoint.X - WindowSize.Width / 2)),
                Math.Max(0, floorToSquareSize(TLPoint.Y - WindowSize.Width / 2)));

            // (in pixels) floored according to the multiple of SquareSize - hopefully correct...
            int width = Math.Min(WindowSize.Width * 2, getMapPixelSize().Width - cachedBitmapTLPoint.X);
            int height = Math.Min(WindowSize.Height * 2, getMapPixelSize().Height - cachedBitmapTLPoint.Y);

            Canvas c = new Canvas(width, height);


            DrawCachedBitmap(c, TLPoint);
            cachedBitmap = c.Finish();

            // works like charm :)
            cachedBitmap.Save("D:/Temp/map"+(it++)+".png");
            //System.Windows.Forms.MessageBox.Show("New cache bitmap. width "+ width + " height " + height);
        }

        // FIXME for going back in the picture...
        public Bitmap DrawSelection(Point TLP, System.Windows.Forms.Control c = null)
        {
            TLPoint = TLP;

            // check if you can select the rectangle from the cachedBitmap
            // if you cannot, then precompute the cachedImage
            if (cachedBitmap == null || // no cached bitmap made so far OR


                // cachedBitmap needs to be counted (to fill the whole window) and
                (((TLPoint.X + WindowSize.Width) > (cachedBitmapTLPoint.X + cachedBitmap.Width)) ||
                 ((TLPoint.Y + WindowSize.Height) > (cachedBitmapTLPoint.Y + cachedBitmap.Height))) 
                 
                 &&
                // CAN actually be counted - I'm not an the edge of the map
                (
                    (charWidth - (cachedBitmapTLPoint.X + cachedBitmap.Width) / SquareSize > 0) ||
                    (charHeight - (cachedBitmapTLPoint.Y + cachedBitmap.Height) / SquareSize > 0))
                )
            {
                PrecomputeCachedBitmap();
            }

            int selectionRectX = TLPoint.X - cachedBitmapTLPoint.X;
            int selectionRectY = TLPoint.Y - cachedBitmapTLPoint.Y;
            int selectionRectWidth = Math.Min(WindowSize.Width, cachedBitmapTLPoint.X + cachedBitmap.Width - TLPoint.X);
            int selectionRectHeight = Math.Min(WindowSize.Height, cachedBitmapTLPoint.Y + cachedBitmap.Height - TLPoint.Y);

            Rectangle selectionRect = new Rectangle(selectionRectX, selectionRectY, selectionRectWidth, selectionRectHeight);

            if (c != null)
                c.Text = "(rect) t:" + selectionRect.Top + " l: " + selectionRect.Left + " w: " + selectionRect.Width + " h: " + selectionRect.Height;

            System.Drawing.Imaging.PixelFormat format = cachedBitmap.PixelFormat;

            //if (++drawIterator == 50)
            //{
            //    GC.Collect();
            //    GC.WaitForPendingFinalizers();
            //    drawIterator -= 50;
            //}
            

            return cachedBitmap.Clone(selectionRect, format);
        }
    }
}