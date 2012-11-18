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
        private Map map;
        private bool imageLoaded = false;
        public Form1()
        {
            InitializeComponent();
            this.map = new Map();
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
                map.loadMap(ofd.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            imageLoaded = true;
            

        }


        private void button2_Click(object sender, EventArgs e)
        {
            Canvas c = new Canvas(zoomedMap.Width, zoomedMap.Height);
            map.DrawMap(c);
            zoomedMap.Image = c.Finish();
        }

        private void zoomedMap_Click(object sender, EventArgs e)
        {
            MessageBox.Show(zoomedMap.Width.ToString());
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (imageLoaded)
            {
                Canvas c = new Canvas(zoomedMap.Width, zoomedMap.Height);
                map.DrawMap(c);
                zoomedMap.Image = c.Finish();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


     

    }
}
