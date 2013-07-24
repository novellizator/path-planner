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
        private void setButtonsEnabled(bool enabled)
        {
            zoomIn.Enabled = enabled;
            zoomOut.Enabled = enabled;
            saveImage.Enabled = enabled;
            checkBox1.Enabled = enabled;
            loadData.Enabled = enabled;
        }

        public Form1()
        {
            InitializeComponent();

            cachedBitmap.setMap(map);
            setButtonsEnabled(false);
            
        }


        private Map map = new Map();
        private CachedBitmap cachedBitmap = new CachedBitmap();


        private Point TLPoint = new Point(0, 0);
        private int squareSize = 1;
        private bool imageLoaded = false;
        private bool isBichromatic = false;

        // dragging features
        bool isDragged = false;
        Point startDragLocation = new Point(0, 0);

        private void setTLPoint(Point tl)
        {
            TLPoint = tl;
        }
        private void DrawBitmapAt(Point TL, bool forcePrecomputing = false)
        {
            if (imageLoaded)
            {
                Graphics viewPortGraphics = viewPortControl1.GetGraphics();
                cachedBitmap.DrawBitmapInto(viewPortGraphics, TL, viewPortControl1.ClientSize, squareSize, isBichromatic, forcePrecomputing);
            }
        }

        private void DrawBitmap(bool forcePrecomputing = false)
        {
            DrawBitmapAt(TLPoint, forcePrecomputing);
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

            setButtonsEnabled(true);
            setTLPoint(new Point(0, 0));
            DrawBitmap(true);

        }

        private void loadData_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Load the file containing information about the path and scanned vertices";
            ofd.Filter = ".data| *.data";
            ofd.Filter += "|All types|*.*";

            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                map.LoadData(ofd.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            DrawBitmap(true);
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
            Point charMapTLPoint = new Point(TLPoint.X / squareSize, TLPoint.Y / squareSize);
            ++squareSize;

            TLPoint = new Point(charMapTLPoint.X * squareSize, charMapTLPoint.Y * squareSize);
            DrawBitmap();
        }

        private void zoomOut_Click(object sender, EventArgs e)
        {
            if (squareSize == 1)
            {
                return;
            }

            Point charMapTLPoint = new Point(TLPoint.X / squareSize, TLPoint.Y / squareSize);
            --squareSize;
            TLPoint = new Point(charMapTLPoint.X * squareSize, charMapTLPoint.Y * squareSize);


            DrawBitmap();
        }

        private void viewPortControl1_Paint(object sender, PaintEventArgs e)
        {
            DrawBitmap();
        }

        private void saveImage_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Uložiť zobrazený výrez";
            sfd.Filter = "GIF súbor | *.gif";
            sfd.Filter += "|PNG súbor | *.png";
            sfd.Filter += "|BMP súbor | *.bmp";
            sfd.Filter += "|JPEG súbor (neodporúčané) | *.jpg";


            if (sfd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }



            Bitmap bitmapToSave = new Bitmap(viewPortControl1.Width, viewPortControl1.Height);
            Graphics bitmapToSaveGraphics = Graphics.FromImage(bitmapToSave);
            if (imageLoaded)
            {
                cachedBitmap.DrawBitmapInto(bitmapToSaveGraphics, TLPoint, viewPortControl1.ClientSize, squareSize, isBichromatic);
            }

            bitmapToSave.Save(sfd.FileName);
        }


        // this is quite different from classical zoomIn button
        private void viewPortControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Point distanceFromCenter = new Point(viewPortControl1.Width / 2 - e.X,
            //    viewPortControl1.Height / 2 - e.Y);


            zoomIn_Click(sender, e);

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            isBichromatic = !isBichromatic;
            DrawBitmap();
        }

    }
}
