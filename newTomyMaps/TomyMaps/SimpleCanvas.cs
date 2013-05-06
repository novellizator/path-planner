using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace TomyMaps
{
    class SimpleCanvas
    {
        private Graphics gr;

        protected Color currCol = Color.Black;

        protected float currPenWidth = 1.0f;

        protected Pen currPen;

        public SolidBrush currBrush;

        protected GraphicsPath path = new GraphicsPath();
    }
}
