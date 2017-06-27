namespace UnpackZip
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
            this.btnServStart = new System.Windows.Forms.Button();
            this.lstServerInfoBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnServStart
            // 
            this.btnServStart.Location = new System.Drawing.Point(13, 13);
            this.btnServStart.Name = "btnServStart";
            this.btnServStart.Size = new System.Drawing.Size(259, 23);
            this.btnServStart.TabIndex = 0;
            this.btnServStart.Text = "Start Server";
            this.btnServStart.UseVisualStyleBackColor = true;
            this.btnServStart.Click += new System.EventHandler(this.btnServStart_Click);
            // 
            // lstServerInfoBox
            // 
            this.lstServerInfoBox.FormattingEnabled = true;
            this.lstServerInfoBox.Location = new System.Drawing.Point(13, 42);
            this.lstServerInfoBox.Name = "lstServerInfoBox";
            this.lstServerInfoBox.Size = new System.Drawing.Size(259, 199);
            this.lstServerInfoBox.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.lstServerInfoBox);
            this.Controls.Add(this.btnServStart);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnServStart;
        private System.Windows.Forms.ListBox lstServerInfoBox;
    }
}

