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

        private bool imageLoaded = false;
        private int squareSize = 3;
        
        private bool isDragged = false;

        private Point startDragLocation;
        private Point TLPoint = new Point(0,0); // TopLeft point

        public Form1()
        {
            InitializeComponent();
        }

        public void DrawZoomedMap(Point tl) 
        {
            if (imageLoaded)
            {
                // zoomedMap.Image may not be passed as an out or ref parameter blahblahbla...
                //map.DrawSelection(out zoomedMap.Image, zoomedMap.Size, tl);
                zoomedMap.Image = map.DrawSelection(zoomedMap.Size, tl);
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

        // "proceed" button
        private void button2_Click(object sender, EventArgs e)
        {
            //int sqS = 10;
            //Canvas c = new Canvas(map.Width * 10, map.Height * 10);
            //map.Draw(ref c, new Point(0, 0), sqS);
            //c.Finish().Save("D:/mapa.bmp", System.Drawing.Imaging.ImageFormat.Bmp );
            // map.PrecomputeMapPortion(3);
            // map.cachedBitmap.Save("D:/mapa.bmp", System.Drawing.Imaging.ImageFormat.Bmp );
            //DrawZoomedMap(TLPoint);
            //Point newTLPoint = new Point(TLPoint.X + 50, TLPoint.Y + 50);
            //TLPoint = newTLPoint;
            textBox1.Text += TLPoint.X + " and " + TLPoint.Y;
            textBox1.Text = "";
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // DrawZoomedMap(TLPoint); // feature to implement: when resizing - make it the same in all directions
            textBox1.Text += "formpaintqqq" + TLPoint.X;
        }

        private void zoomInButton_Click(object sender, EventArgs e)
        {
            if (squareSize == 10)
            {
                return;
            }
            squareSize += 1;
            map.setSquareSize(squareSize);
            DrawZoomedMap(TLPoint);
        }

        private void zoomOutButton_Click(object sender, EventArgs e)
        {
            if (squareSize == 1)
            {
                return;
            }
            squareSize -= 1;
            map.setSquareSize(squareSize);
            DrawZoomedMap(TLPoint);
        }

        private void zoomedMap_MouseMove(object sender, MouseEventArgs e)
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
                
                int newX, newY; 
                // cannot access these points in the real map (out of bound in either side)
                if (TLPoint.X + dx < 0 )
                {
                    newX = 0;
                }
                else
                {
                    newX = TLPoint.X + dx;
                }
 
                if (TLPoint.Y + dy < 0 )
                {
                    newY = 0;
                }
                else
                {
                    newY = TLPoint.Y + dy;
                }


                textBox1.Text = dx + ";";
                Point newTLPoint = new Point(newX, newY);
                DrawZoomedMap(newTLPoint);
                


            }
        }

        private void zoomedMap_MouseUp(object sender, MouseEventArgs e)
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

                // ked som uz za obrazom - tlpoint+zoomedimageWidth > "cachedimage??.width" -- i mean the width of the whole big map. 
                // heigth detto
                if (true)
                {
                    
                }
            }
            isDragged = false;
            
        }

        private void buttonSaveSelection_Click(object sender, EventArgs e)
        {
            //textBox1.Text = "width: " + pictureBox1.Size.Width + "height: " + pictureBox1.Size.Height;

            this.Size = new Size(this.Size.Width + 300, this.Size.Height); // resizujem len main window



            //pictureBox1.Size = new Size(pictureBox1.Size.Width+50, pictureBox1.Size.Height);
            //pictureBox1.Anchor = AnchorStyles.Right;
            

            //textBox1.Text += "+width: " + pictureBox1.Size.Width + "+height: " + pictureBox1.Size.Height; 
            //pictureBox1.Invalidate();

        
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //tst(ref pictureBox1.Image);
        }

        private void tst(ref Image i)
        {
            
        }
     

    }
}
