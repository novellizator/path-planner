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

        private Bitmap cachedBitmap = null;


        public void DrawBitmapInto(Graphics g, Point TLPoint, Size ViewPortSize, int squareSize)
        {
            if (squareSize != _squareSize)
            {
                PrecomputeBitmap();
            }
            // if bitmap is not precomputed and is precomputable
            if (false)
            {
                PrecomputeBitmap();
            }


        }
        private void PrecomputeBitmap()
        {
        }
    }
}
