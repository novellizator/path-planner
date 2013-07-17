using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TomyMaps
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();

            cachedBitmap.setMap(map);
        }


        private Map map = new Map();
        private CachedBitmap cachedBitmap = new CachedBitmap();


        private Point TLPoint = new Point(0, 0);
        private int squareSize = 1;
        private bool imageLoaded = false;


        // dragging features
        bool isDragged = false;
        Point startDragLocation = new Point(0, 0);

        private void setTLPoint(Point tl)
        {
            TLPoint = tl;
        }
        private void DrawBitmapAt(Point TL)
        {
            if (imageLoaded)
            {
                Graphics viewPortGraphics = viewPortControl1.GetGraphics();
                cachedBitmap.DrawBitmapInto(viewPortGraphics, TL, viewPortControl1.ClientSize, squareSize);
            }
        }

        private void DrawBitmap()
        {
            DrawBitmapAt(TLPoint);
        }

        private void loadMap_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Title = "Open Image File";
            ofd.Filter = ".map|*.map" +
                "|All types|*.*";

            ofd.FilterIndex = 0;
            ofd.FileName = "";
            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                map.Load(ofd.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }


            imageLoaded = true;
            //TEST
            //map.Load("D:/school/TRETIAK/bakalarka/github-path-planner/TomyMaps/battleground.map");
            //map.Load("D:/github-gppc/path-planner/newTomyMaps/battleground.map");

            setTLPoint(new Point(0, 0));
            DrawBitmapAt(TLPoint);

        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            DrawBitmap();
        }

        private void viewPortControl1_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDragged)
            {
                int dx = startDragLocation.X - e.X;
                int dy = startDragLocation.Y - e.Y;

                TLPoint.X += dx;
                TLPoint.Y += dy;

                if (TLPoint.X < 0)
                {
                    TLPoint.X = 0;
                }
                if (TLPoint.Y < 0)
                {
                    TLPoint.Y = 0;
                }

            }
            isDragged = false;
        }

        private void viewPortControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!isDragged)
                {
                    isDragged = true;
                    startDragLocation = e.Location;
                }

                int dx = startDragLocation.X - e.X;
                int dy = startDragLocation.Y - e.Y;


                Point newTLPoint = new Point(TLPoint.X + dx, TLPoint.Y + dy);
                
                // cannot access these points in the real map (out of bound in either side)
                if (newTLPoint.X < 0)
                {
                    newTLPoint.X = 0;
                }

                if (newTLPoint.Y < 0)
                {
                    newTLPoint.Y = 0;
                }

                DrawBitmapAt(newTLPoint);

            }
        }

        private void zoomIn_Click(object sender, EventArgs e)
        {
            ++squareSize;
            TLPoint = new Point(0, 0);
            DrawBitmap();
        }

        private void zoomOut_Click(object sender, EventArgs e)
        {
            --squareSize;
            TLPoint = new Point(0, 0);
            DrawBitmap();
        }
    }
}
