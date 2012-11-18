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
            this.button1 = new System.Windows.Forms.Button();
            this.zoomedMap = new System.Windows.Forms.PictureBox();
            this.button2 = new System.Windows.Forms.Button();
            this.buttonSaveWhole = new System.Windows.Forms.Button();
            this.buttonSaveSelection = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.zoomedMap)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(562, 32);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Load File";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // zoomedMap
            // 
            this.zoomedMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.zoomedMap.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.zoomedMap.Location = new System.Drawing.Point(12, 12);
            this.zoomedMap.Name = "zoomedMap";
            this.zoomedMap.Size = new System.Drawing.Size(528, 403);
            this.zoomedMap.TabIndex = 1;
            this.zoomedMap.TabStop = false;
            this.zoomedMap.Click += new System.EventHandler(this.zoomedMap_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(562, 113);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Proceed";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // buttonSaveWhole
            // 
            this.buttonSaveWhole.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveWhole.Location = new System.Drawing.Point(562, 226);
            this.buttonSaveWhole.Name = "buttonSaveWhole";
            this.buttonSaveWhole.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveWhole.TabIndex = 3;
            this.buttonSaveWhole.Text = "Save Image";
            this.buttonSaveWhole.UseVisualStyleBackColor = true;
            // 
            // buttonSaveSelection
            // 
            this.buttonSaveSelection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveSelection.Location = new System.Drawing.Point(562, 294);
            this.buttonSaveSelection.Name = "buttonSaveSelection";
            this.buttonSaveSelection.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveSelection.TabIndex = 4;
            this.buttonSaveSelection.Text = "Save Selection";
            this.buttonSaveSelection.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 439);
            this.Controls.Add(this.buttonSaveSelection);
            this.Controls.Add(this.buttonSaveWhole);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.zoomedMap);
            this.Controls.Add(this.button1);
            this.MinimumSize = new System.Drawing.Size(300, 200);
            this.Name = "Form1";
            this.Text = "TomyMaps";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.zoomedMap)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox zoomedMap;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button buttonSaveWhole;
        private System.Windows.Forms.Button buttonSaveSelection;
    }
}

