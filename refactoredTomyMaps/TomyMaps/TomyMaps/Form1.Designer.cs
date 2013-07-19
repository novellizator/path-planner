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
            this.debugBox = new System.Windows.Forms.TextBox();
            this.zoomIn = new System.Windows.Forms.Button();
            this.zoomOut = new System.Windows.Forms.Button();
            this.viewPortControl1 = new TomyMaps.ViewPortControl();
            this.saveImage = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // loadMap
            // 
            this.loadMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.loadMap.Location = new System.Drawing.Point(755, 260);
            this.loadMap.Name = "loadMap";
            this.loadMap.Size = new System.Drawing.Size(109, 49);
            this.loadMap.TabIndex = 1;
            this.loadMap.Text = "Load Map";
            this.loadMap.UseVisualStyleBackColor = true;
            this.loadMap.Click += new System.EventHandler(this.loadMap_Click);
            // 
            // debugBox
            // 
            this.debugBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.debugBox.Location = new System.Drawing.Point(715, 30);
            this.debugBox.Multiline = true;
            this.debugBox.Name = "debugBox";
            this.debugBox.Size = new System.Drawing.Size(149, 191);
            this.debugBox.TabIndex = 2;
            // 
            // zoomIn
            // 
            this.zoomIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.zoomIn.Location = new System.Drawing.Point(755, 346);
            this.zoomIn.Name = "zoomIn";
            this.zoomIn.Size = new System.Drawing.Size(75, 23);
            this.zoomIn.TabIndex = 3;
            this.zoomIn.Text = "zoomIn";
            this.zoomIn.UseVisualStyleBackColor = true;
            this.zoomIn.Click += new System.EventHandler(this.zoomIn_Click);
            // 
            // zoomOut
            // 
            this.zoomOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.zoomOut.Location = new System.Drawing.Point(755, 406);
            this.zoomOut.Name = "zoomOut";
            this.zoomOut.Size = new System.Drawing.Size(75, 23);
            this.zoomOut.TabIndex = 4;
            this.zoomOut.Text = "zoom out";
            this.zoomOut.UseVisualStyleBackColor = true;
            this.zoomOut.Click += new System.EventHandler(this.zoomOut_Click);
            // 
            // viewPortControl1
            // 
            this.viewPortControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.viewPortControl1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.viewPortControl1.Location = new System.Drawing.Point(0, 0);
            this.viewPortControl1.Name = "viewPortControl1";
            this.viewPortControl1.Size = new System.Drawing.Size(587, 570);
            this.viewPortControl1.TabIndex = 0;
            this.viewPortControl1.Text = "viewPortControl1";
            this.viewPortControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.viewPortControl1_Paint);
            this.viewPortControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.viewPortControl1_MouseMove);
            this.viewPortControl1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.viewPortControl1_MouseUp);
            // 
            // saveImage
            // 
            this.saveImage.Location = new System.Drawing.Point(755, 465);
            this.saveImage.Name = "saveImage";
            this.saveImage.Size = new System.Drawing.Size(135, 23);
            this.saveImage.TabIndex = 5;
            this.saveImage.Text = "Ulož vidiťeľnú časť";
            this.saveImage.UseVisualStyleBackColor = true;
            this.saveImage.Click += new System.EventHandler(this.saveImage_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(902, 570);
            this.Controls.Add(this.saveImage);
            this.Controls.Add(this.zoomOut);
            this.Controls.Add(this.zoomIn);
            this.Controls.Add(this.debugBox);
            this.Controls.Add(this.loadMap);
            this.Controls.Add(this.viewPortControl1);
            this.Name = "Form1";
            this.Text = "Maps";
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ViewPortControl viewPortControl1;
        private System.Windows.Forms.Button loadMap;
        private System.Windows.Forms.TextBox debugBox;
        private System.Windows.Forms.Button zoomIn;
        private System.Windows.Forms.Button zoomOut;
        private System.Windows.Forms.Button saveImage;
    }
}

