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
    public partial class ViewPortControl : Control
    {
        public ViewPortControl()
        {
            InitializeComponent();
        }

        public Graphics GetGraphics()
        {
            return this.CreateGraphics();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
