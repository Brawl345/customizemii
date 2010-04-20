namespace CustomizeMii
{
    partial class CustomizeMii_PaletteFormatBox
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnIA8 = new System.Windows.Forms.Button();
            this.btnRGB5A3 = new System.Windows.Forms.Button();
            this.btnRGB565 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(307, 83);
            this.label1.TabIndex = 0;
            this.label1.Text = "You\'re converting to an indexed format, please choose a format\r\nfor the Palette:\r" +
                "\n\r\nIA8:   Black & White + Alpha\r\nRGB5A3:   Color + Alpha\r\nRGB565:   Color";
            // 
            // btnIA8
            // 
            this.btnIA8.Location = new System.Drawing.Point(12, 101);
            this.btnIA8.Name = "btnIA8";
            this.btnIA8.Size = new System.Drawing.Size(75, 23);
            this.btnIA8.TabIndex = 1;
            this.btnIA8.Text = "IA8";
            this.btnIA8.UseVisualStyleBackColor = true;
            this.btnIA8.Click += new System.EventHandler(this.btnIA8_Click);
            // 
            // btnRGB5A3
            // 
            this.btnRGB5A3.Location = new System.Drawing.Point(128, 101);
            this.btnRGB5A3.Name = "btnRGB5A3";
            this.btnRGB5A3.Size = new System.Drawing.Size(75, 23);
            this.btnRGB5A3.TabIndex = 1;
            this.btnRGB5A3.Text = "RGB5A3";
            this.btnRGB5A3.UseVisualStyleBackColor = true;
            this.btnRGB5A3.Click += new System.EventHandler(this.btnRGB5A3_Click);
            // 
            // btnRGB565
            // 
            this.btnRGB565.Location = new System.Drawing.Point(244, 101);
            this.btnRGB565.Name = "btnRGB565";
            this.btnRGB565.Size = new System.Drawing.Size(75, 23);
            this.btnRGB565.TabIndex = 1;
            this.btnRGB565.Text = "RGB565";
            this.btnRGB565.UseVisualStyleBackColor = true;
            this.btnRGB565.Click += new System.EventHandler(this.btnRGB565_Click);
            // 
            // CustomizeMii_PaletteFormatBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 137);
            this.Controls.Add(this.btnRGB565);
            this.Controls.Add(this.btnRGB5A3);
            this.Controls.Add(this.btnIA8);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CustomizeMii_PaletteFormatBox";
            this.Text = "CustomizeMii_PaletteFormatBox";
            this.Load += new System.EventHandler(this.CustomizeMii_PaletteFormatBox_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnIA8;
        private System.Windows.Forms.Button btnRGB5A3;
        private System.Windows.Forms.Button btnRGB565;
    }
}