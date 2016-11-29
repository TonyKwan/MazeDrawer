namespace MazeDrawer
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
            this.roboKITTTiles = new System.Windows.Forms.ListBox();
            this.resetMazeBtn = new System.Windows.Forms.Button();
            this.roboHALTiles = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // roboKITTTiles
            // 
            this.roboKITTTiles.FormattingEnabled = true;
            this.roboKITTTiles.Location = new System.Drawing.Point(1018, 230);
            this.roboKITTTiles.Name = "roboKITTTiles";
            this.roboKITTTiles.Size = new System.Drawing.Size(159, 212);
            this.roboKITTTiles.TabIndex = 0;
            // 
            // resetMazeBtn
            // 
            this.resetMazeBtn.Location = new System.Drawing.Point(1065, 448);
            this.resetMazeBtn.Name = "resetMazeBtn";
            this.resetMazeBtn.Size = new System.Drawing.Size(75, 23);
            this.resetMazeBtn.TabIndex = 1;
            this.resetMazeBtn.Text = "Reset";
            this.resetMazeBtn.UseVisualStyleBackColor = true;
            this.resetMazeBtn.Click += new System.EventHandler(this.resetMazeBtn_Click);
            // 
            // roboHALTiles
            // 
            this.roboHALTiles.FormattingEnabled = true;
            this.roboHALTiles.Location = new System.Drawing.Point(1018, 12);
            this.roboHALTiles.Name = "roboHALTiles";
            this.roboHALTiles.Size = new System.Drawing.Size(159, 212);
            this.roboHALTiles.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(1000, 600);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1189, 624);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.roboHALTiles);
            this.Controls.Add(this.resetMazeBtn);
            this.Controls.Add(this.roboKITTTiles);
            this.Name = "Form1";
            this.Text = "MazeDrawer";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox roboKITTTiles;
        private System.Windows.Forms.Button resetMazeBtn;
        private System.Windows.Forms.ListBox roboHALTiles;
        private System.Windows.Forms.Button button1;
    }
}

