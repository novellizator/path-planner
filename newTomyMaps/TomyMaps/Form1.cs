using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

// the goals of this class:
// provide the Map class with data (doesn't need to validate them - the Map class should take this job)
// ... and with default values
namespace TomyMaps
{
    public partial class Form1 : Form
    {
        private Map map = new Map();

        private bool imageLoaded = false;
        private int DefaultSquareSize = 1;

        private bool isDragged = false;

        private Point startDragLocation;
        private Point TLPoint = new Point(0, 0); // TopLeft point

        public Form1()
        {
            InitializeComponent();

            mapView1.map = map;
        }

        public void DrawZoomedMap(Point tl)
        {
            if (imageLoaded)
            {
                map.TLPoint = tl;
                //mapView1.Invalidate();
                //mapView1.Refresh(); // maybe
                //mapView1.DrawMap(textBox1);
                //mapView1.MapViewGraphics = mapView1.CreateGraphics();
                map.DrawSelection(mapView1.GetGraphics());
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
                textBox1.Text = ofd.FileName;
                MessageBox.Show(ofd.FileName);
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
            // redraw a map after loading
            map.SquareSize = DefaultSquareSize;
            map.WindowSize = mapView1.ClientSize;
            DrawZoomedMap(TLPoint);

        }

        // "proceed" button

        private void button2_Click(object sender, EventArgs e)
        {
            // textBox1.Text += TLPoint.X + " and " + TLPoint.Y;
            // textBox1.Text = "";

            //textBox1.Text = "clientsize " + button2.ClientSize.Width + " normal size" + button2.Size.Width;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // DrawZoomedMap(TLPoint); // feature to implement: when resizing - make it the same in all directions
            //textBox1.Text += "formpaintqqq" + TLPoint.X;
        }

        private void zoomInButton_Click(object sender, EventArgs e)
        {
            map.SquareSize += 1;
            textBox1.Text += map.SquareSize;
            DrawZoomedMap(TLPoint);
        }

        private void zoomOutButton_Click(object sender, EventArgs e)
        {
            map.SquareSize -= 1;
            DrawZoomedMap(TLPoint);
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

    

        private void zoomedMap_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void mapView1_MouseMove(object sender, MouseEventArgs e)
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
                if (TLPoint.X + dx < 0)
                {
                    newX = 0;
                }
                else
                {
                    newX = TLPoint.X + dx;
                }

                if (TLPoint.Y + dy < 0)
                {
                    newY = 0;
                }
                else
                {
                    newY = TLPoint.Y + dy;
                }
          
                textBox1.Text = dx + ";";
                // keviem pre jaku picu tam treba dat -newX miesto + ...
                Point newTLPoint = new Point(-newX, -newY);
                map.TLPoint = newTLPoint;

                DrawZoomedMap(newTLPoint);


            }
        }

        private void mapView1_MouseUp(object sender, MouseEventArgs e)
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

        private void mapView1_SizeChanged(object sender, EventArgs e)
        {
            map.WindowSize = mapView1.ClientSize;
            DrawZoomedMap(TLPoint);
        }

        private void mapView1_Click(object sender, EventArgs e)
        {

        }


    }
}
