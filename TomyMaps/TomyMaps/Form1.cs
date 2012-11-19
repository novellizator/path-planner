using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

// lockBitmap --> pri zmene squares vypocitam celu mapu a potom posuvam
namespace TomyMaps
{
    public partial class Form1 : Form
    {
        private Map map = new Map();

        /// <summary>
        /// 
        /// </summary>
        private bool imageLoaded = false;
        private int squareSize = 3;
        
        private bool isMoving = false;

        private Point movedByVector;
        private Point TLPoint = new Point(0,0); // TopLeft point

        public Form1()
        {
            InitializeComponent();
        }

        public void DrawZoomedMap(Point tl) 
        {
            if (imageLoaded)
            {
                Canvas c = new Canvas(zoomedMap.Width, zoomedMap.Height);
                map.Draw(c, tl, squareSize);
                zoomedMap.Image = c.Finish();
            }
        }

        private void button1_Click(object sender, EventArgs e)
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

            // redraw a map after loading
            DrawZoomedMap(TLPoint);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            Canvas c = new Canvas(zoomedMap.Width, zoomedMap.Height);
            map.Draw(c);
            zoomedMap.Image = c.Finish();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawZoomedMap(TLPoint); // maybe when resizing - make it the same from all the directions
        }

        private void zoomInButton_Click(object sender, EventArgs e)
        {
            squareSize += 1;
            DrawZoomedMap(TLPoint);
        }

        private void zoomOutButton_Click(object sender, EventArgs e)
        {
            squareSize -= 1;
            DrawZoomedMap(TLPoint);
        }

        private void zoomedMap_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!isMoving)
                {
                    isMoving = true;
                    movedByVector = e.Location;
                }
            }
            Point newTLPoint = new Point(TLPoint.X + movedByVector.X, TLPoint.Y + movedByVector.Y);
            // DrawZoomedMap(newTLPoint);
            zoomedMap.Location = movedByVector;
        }

        private void zoomedMap_MouseUp(object sender, MouseEventArgs e)
        {
            if (isMoving)
            {
                TLPoint.X += movedByVector.X;
                TLPoint.Y += movedByVector.Y;
            }
            isMoving = false;
            
        }


     

    }
}
