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

        private Map map = new Map();
        private CachedBitmap cachedBitmap = new CachedBitmap();

        private Point TLPoint = new Point(0, 0);
        private int squareSize = 1;
        private bool imageLoaded = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void DrawBitmap(Point tl)
        {
            if (imageLoaded)
            {
                Graphics viewPortGraphics = viewPortControl1.GetGraphics();
                cachedBitmap.DrawBitmapInto(viewPortGraphics, tl, viewPortControl1.ClientSize, squareSize);
            }
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

            DrawBitmap(TLPoint);

        }
    }
}
