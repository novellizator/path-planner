namespace TomyMaps
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.loadMap = new System.Windows.Forms.Button();
            this.zoomIn = new System.Windows.Forms.Button();
            this.zoomOut = new System.Windows.Forms.Button();
            this.saveImage = new System.Windows.Forms.Button();
            this.viewPortControl1 = new TomyMaps.ViewPortControl();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // loadMap
            // 
            this.loadMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.loadMap.Location = new System.Drawing.Point(498, 27);
            this.loadMap.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.loadMap.Name = "loadMap";
            this.loadMap.Size = new System.Drawing.Size(80, 40);
            this.loadMap.TabIndex = 1;
            this.loadMap.Text = "Load Map";
            this.loadMap.UseVisualStyleBackColor = true;
            this.loadMap.Click += new System.EventHandler(this.loadMap_Click);
            // 
            // zoomIn
            // 
            this.zoomIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.zoomIn.Location = new System.Drawing.Point(504, 141);
            this.zoomIn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.zoomIn.Name = "zoomIn";
            this.zoomIn.Size = new System.Drawing.Size(70, 70);
            this.zoomIn.TabIndex = 3;
            this.zoomIn.Text = "+";
            this.zoomIn.UseVisualStyleBackColor = true;
            this.zoomIn.Click += new System.EventHandler(this.zoomIn_Click);
            // 
            // zoomOut
            // 
            this.zoomOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.zoomOut.Location = new System.Drawing.Point(504, 239);
            this.zoomOut.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.zoomOut.Name = "zoomOut";
            this.zoomOut.Size = new System.Drawing.Size(70, 70);
            this.zoomOut.TabIndex = 4;
            this.zoomOut.Text = "-";
            this.zoomOut.UseVisualStyleBackColor = true;
            this.zoomOut.Click += new System.EventHandler(this.zoomOut_Click);
            // 
            // saveImage
            // 
            this.saveImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveImage.Location = new System.Drawing.Point(498, 85);
            this.saveImage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.saveImage.Name = "saveImage";
            this.saveImage.Size = new System.Drawing.Size(80, 40);
            this.saveImage.TabIndex = 5;
            this.saveImage.Text = "Save visible area";
            this.saveImage.UseVisualStyleBackColor = true;
            this.saveImage.Click += new System.EventHandler(this.saveImage_Click);
            // 
            // viewPortControl1
            // 
            this.viewPortControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.viewPortControl1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.viewPortControl1.Location = new System.Drawing.Point(0, 0);
            this.viewPortControl1.Margin = new System.Windows.Forms.Padding(2);
            this.viewPortControl1.Name = "viewPortControl1";
            this.viewPortControl1.Size = new System.Drawing.Size(471, 463);
            this.viewPortControl1.TabIndex = 0;
            this.viewPortControl1.Text = "viewPortControl1";
            this.viewPortControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.viewPortControl1_Paint);
            this.viewPortControl1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.viewPortControl1_MouseDoubleClick);
            this.viewPortControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.viewPortControl1_MouseMove);
            this.viewPortControl1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.viewPortControl1_MouseUp);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(504, 340);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(607, 463);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.saveImage);
            this.Controls.Add(this.zoomOut);
            this.Controls.Add(this.zoomIn);
            this.Controls.Add(this.loadMap);
            this.Controls.Add(this.viewPortControl1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Maps";
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ViewPortControl viewPortControl1;
        private System.Windows.Forms.Button loadMap;
        private System.Windows.Forms.Button zoomIn;
        private System.Windows.Forms.Button zoomOut;
        private System.Windows.Forms.Button saveImage;
        private System.Windows.Forms.TextBox textBox1;
    }
}

