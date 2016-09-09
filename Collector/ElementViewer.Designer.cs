namespace Collector
{
    partial class ElementViewer
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.nextBtn = new System.Windows.Forms.Button();
            this.previousBtn = new System.Windows.Forms.Button();
            this.nextImg = new System.Windows.Forms.PictureBox();
            this.previousImg = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nextImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.previousImg)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button1.Location = new System.Drawing.Point(466, 711);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Fermer";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.Location = new System.Drawing.Point(145, 100);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(494, 494);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // nextBtn
            // 
            this.nextBtn.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.nextBtn.Image = global::Collector.Properties.Resources.btn_next;
            this.nextBtn.Location = new System.Drawing.Point(929, 409);
            this.nextBtn.Name = "nextBtn";
            this.nextBtn.Size = new System.Drawing.Size(40, 30);
            this.nextBtn.TabIndex = 1;
            this.nextBtn.UseVisualStyleBackColor = true;
            this.nextBtn.Click += new System.EventHandler(this.button_Click);
            // 
            // previousBtn
            // 
            this.previousBtn.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.previousBtn.Image = global::Collector.Properties.Resources.btn_previous;
            this.previousBtn.Location = new System.Drawing.Point(32, 409);
            this.previousBtn.Name = "previousBtn";
            this.previousBtn.Size = new System.Drawing.Size(40, 30);
            this.previousBtn.TabIndex = 2;
            this.previousBtn.UseVisualStyleBackColor = true;
            this.previousBtn.Click += new System.EventHandler(this.button_Click);
            // 
            // nextImg
            // 
            this.nextImg.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.nextImg.BackColor = System.Drawing.Color.Black;
            this.nextImg.Location = new System.Drawing.Point(901, 303);
            this.nextImg.Name = "nextImg";
            this.nextImg.Size = new System.Drawing.Size(100, 100);
            this.nextImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.nextImg.TabIndex = 4;
            this.nextImg.TabStop = false;
            // 
            // previousImg
            // 
            this.previousImg.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.previousImg.BackColor = System.Drawing.Color.Black;
            this.previousImg.Location = new System.Drawing.Point(5, 303);
            this.previousImg.Name = "previousImg";
            this.previousImg.Size = new System.Drawing.Size(100, 100);
            this.previousImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.previousImg.TabIndex = 5;
            this.previousImg.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.AutoScrollMinSize = new System.Drawing.Size(400, 400);
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(111, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(784, 695);
            this.panel1.TabIndex = 6;
            // 
            // ElementViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.ClientSize = new System.Drawing.Size(1008, 746);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.previousImg);
            this.Controls.Add(this.nextImg);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.nextBtn);
            this.Controls.Add(this.previousBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ElementViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Collector";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nextImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.previousImg)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button previousBtn;
        private System.Windows.Forms.Button nextBtn;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox nextImg;
        private System.Windows.Forms.PictureBox previousImg;
        private System.Windows.Forms.Panel panel1;
    }
}