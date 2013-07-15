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
    public partial class MapView : Control
    {
        public Map map = null;
        public Graphics g;
        public Graphics MapViewGraphics;
        public MapView()
        {
            InitializeComponent();
            //MapViewGraphics = this.CreateGraphics();
        }


        public Graphics GetGraphics()
        {
            return this.CreateGraphics();
        }

        public void DrawMap(Control c)
        {
            g = this.CreateGraphics();
            //map.kktina(g);
            map.DrawSelection(g, c);
            //MessageBox.Show("drawmap");
            
        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            //MessageBox.Show("onpaint");
            //pe.Graphics
           
            //map.DrawSelection(pe.Graphics);
            //map.kktina(pe.Graphics);
            //MessageBox.Show("onpaint");
            //Graphics g = pe.Graphics;
            //g.Clear(Color.Red);
            //g.DrawLine(Pens.Black, 0, 0, Width, Height);
        }
    }
}
