﻿using System;
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
        private int squareSize;
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



        private void zoomOut_Click(object sender, EventArgs e)
        {
            if (!imageLoaded)
            {
                return;
            }

            if (squareSize == 1)
            {
                return;
            }

            //Point charMapTLPoint = new Point(TLPoint.X / squareSize, TLPoint.Y / squareSize);
            //--squareSize;
            //TLPoint = new Point(charMapTLPoint.X * squareSize, charMapTLPoint.Y * squareSize);

            // 1) compute he centerPoint - this won't change as long as possible
            Point centerPoint = new Point(TLPoint.X + viewPortControl1.Width / 2, TLPoint.Y + viewPortControl1.Height / 2);
            Point charMapCenterPoint = new Point(centerPoint.X / squareSize, centerPoint.Y / squareSize);

            --squareSize;
            Point newCenterPoint = new Point(charMapCenterPoint.X * squareSize, charMapCenterPoint.Y * squareSize);
            TLPoint.X = Math.Max(0, newCenterPoint.X - viewPortControl1.Width / 2);
            TLPoint.Y = Math.Max(0, newCenterPoint.Y - viewPortControl1.Height / 2);

            DrawBitmap();
        }


        // zoomINn - the charCenter stays the same
        private void zoomIn_Click(object sender, EventArgs e)
        {
            if (!imageLoaded)
            {
                return;
            }
            // 1) compute he centerPoint - this won't change
            Point centerPoint = new Point(TLPoint.X + viewPortControl1.Width /2, TLPoint.Y + viewPortControl1.Height /2);
            Point charMapCenterPoint = new Point(centerPoint.X / squareSize, centerPoint.Y / squareSize);

            ++squareSize;

            // 2) get back the new TLPoint
            Point newCenterPoint = new Point(charMapCenterPoint.X * squareSize, charMapCenterPoint.Y * squareSize);
            //TLPoint = new Point(newCenterPoint.X - viewPortControl1.Width/2, newCenterPoint.Y - viewPortControl1.Height/2);

            TLPoint.X = Math.Max(0, newCenterPoint.X - viewPortControl1.Width / 2);
            TLPoint.Y = Math.Max(0, newCenterPoint.Y - viewPortControl1.Height/ 2);
            //MessageBox.Show(centerPoint.ToString());
            //MessageBox.Show(newCenterPoint.ToString());
            DrawBitmap();
        }


        // centralize & zoom in
        private void viewPortControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Point distanceFromCenter = new Point(viewPortControl1.Width / 2 - e.X,
                viewPortControl1.Height / 2 - e.Y);

            TLPoint.X = Math.Max(0, TLPoint.X - distanceFromCenter.X);
            TLPoint.Y = Math.Max(0, TLPoint.Y - distanceFromCenter.Y);

            zoomIn_Click(sender, e);
        }



        private void viewPortControl1_Paint(object sender, PaintEventArgs e)
        {
            DrawBitmap();
        }

        private void bichromaticToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bichromaticToolStripMenuItem.Checked = !bichromaticToolStripMenuItem.Checked;

            isBichromatic = !isBichromatic;
            DrawBitmap();
        }

        private void loadMapToolStripMenuItem_Click(object sender, EventArgs e)
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
            squareSize = Math.Max(1, viewPortControl1.Width / map.getRawMap()[0].Length);
            DrawBitmap(true);
        }

        private void loadPathToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void saveVisibleAreaToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zoomIn_Click(sender, e);
        }

        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zoomOut_Click(sender, e);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Created by Tomas Novella, 2013");
        }

    }
}
