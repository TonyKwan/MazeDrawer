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
            this.resetMazeBtn = new System.Windows.Forms.Button();
            this.messagebox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
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
            // messagebox
            // 
            this.messagebox.FormattingEnabled = true;
            this.messagebox.Location = new System.Drawing.Point(1018, 12);
            this.messagebox.Name = "messagebox";
            this.messagebox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.messagebox.Size = new System.Drawing.Size(159, 433);
            this.messagebox.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1189, 624);
            this.Controls.Add(this.messagebox);
            this.Controls.Add(this.resetMazeBtn);
            this.Name = "Form1";
            this.Text = "MazeDrawer";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button resetMazeBtn;
        private System.Windows.Forms.ListBox messagebox;
    }
}

