/* This file is part of CustomizeMii
 * Copyright (C) 2009 Leathl
 * 
 * CustomizeMii is free software: you can redistribute it and/or
 * modify it under the terms of the GNU General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * CustomizeMii is distributed in the hope that it will be
 * useful, but WITHOUT ANY WARRANTY; without even the implied warranty
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
 
namespace CustomizeMii
{
    partial class CustomizeMii_ComplexForwarder
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.tb1 = new System.Windows.Forms.TextBox();
            this.cbImage43 = new System.Windows.Forms.CheckBox();
            this.cbImage169 = new System.Windows.Forms.CheckBox();
            this.tbImage43 = new System.Windows.Forms.TextBox();
            this.tbImage169 = new System.Windows.Forms.TextBox();
            this.btnBrowseImage43 = new System.Windows.Forms.Button();
            this.btnBrowseImage169 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tb2 = new System.Windows.Forms.TextBox();
            this.tb3 = new System.Windows.Forms.TextBox();
            this.tb4 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tb1
            // 
            this.tb1.Location = new System.Drawing.Point(12, 62);
            this.tb1.Name = "tb1";
            this.tb1.Size = new System.Drawing.Size(334, 20);
            this.tb1.TabIndex = 1;
            this.tb1.Text = "SD:/apps/example/boot.dol";
            // 
            // cbImage43
            // 
            this.cbImage43.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cbImage43.AutoSize = true;
            this.cbImage43.Location = new System.Drawing.Point(15, 224);
            this.cbImage43.Name = "cbImage43";
            this.cbImage43.Size = new System.Drawing.Size(73, 17);
            this.cbImage43.TabIndex = 3;
            this.cbImage43.Text = "Image 4:3";
            this.cbImage43.UseVisualStyleBackColor = true;
            this.cbImage43.CheckedChanged += new System.EventHandler(this.cbImage43_CheckedChanged);
            // 
            // cbImage169
            // 
            this.cbImage169.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cbImage169.AutoSize = true;
            this.cbImage169.Location = new System.Drawing.Point(15, 254);
            this.cbImage169.Name = "cbImage169";
            this.cbImage169.Size = new System.Drawing.Size(79, 17);
            this.cbImage169.TabIndex = 3;
            this.cbImage169.Text = "Image 16:9";
            this.cbImage169.UseVisualStyleBackColor = true;
            this.cbImage169.CheckedChanged += new System.EventHandler(this.cbImage169_CheckedChanged);
            // 
            // tbImage43
            // 
            this.tbImage43.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.tbImage43.Enabled = false;
            this.tbImage43.Location = new System.Drawing.Point(94, 222);
            this.tbImage43.Name = "tbImage43";
            this.tbImage43.Size = new System.Drawing.Size(171, 20);
            this.tbImage43.TabIndex = 4;
            // 
            // tbImage169
            // 
            this.tbImage169.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.tbImage169.Enabled = false;
            this.tbImage169.Location = new System.Drawing.Point(94, 251);
            this.tbImage169.Name = "tbImage169";
            this.tbImage169.Size = new System.Drawing.Size(171, 20);
            this.tbImage169.TabIndex = 5;
            // 
            // btnBrowseImage43
            // 
            this.btnBrowseImage43.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnBrowseImage43.Enabled = false;
            this.btnBrowseImage43.Location = new System.Drawing.Point(271, 222);
            this.btnBrowseImage43.Name = "btnBrowseImage43";
            this.btnBrowseImage43.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseImage43.TabIndex = 6;
            this.btnBrowseImage43.Text = "Browse...";
            this.btnBrowseImage43.UseVisualStyleBackColor = true;
            this.btnBrowseImage43.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnBrowseImage169
            // 
            this.btnBrowseImage169.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnBrowseImage169.Enabled = false;
            this.btnBrowseImage169.Location = new System.Drawing.Point(271, 251);
            this.btnBrowseImage169.Name = "btnBrowseImage169";
            this.btnBrowseImage169.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseImage169.TabIndex = 6;
            this.btnBrowseImage169.Text = "Browse...";
            this.btnBrowseImage169.UseVisualStyleBackColor = true;
            this.btnBrowseImage169.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label1.Location = new System.Drawing.Point(0, 174);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(358, 40);
            this.label1.TabIndex = 7;
            this.label1.Text = "The images must be 640 x 480, else they will be resized!\r\nThe 16:9 image should b" +
                "e made in 832 x 480 and resized\r\nto 640 x 480, it will be streched on the Wii!";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOK.Location = new System.Drawing.Point(15, 292);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(160, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(186, 292);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(160, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.Location = new System.Drawing.Point(0, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(358, 40);
            this.label2.TabIndex = 7;
            this.label2.Text = "The forwarder will try to load in this order.\r\nEntries must begin with \"SD:/\" or " +
                "\"USB:/\" and end with \".dol\" or \".elf\"!\r\nYou may enter whatever path you want.";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tb2
            // 
            this.tb2.Location = new System.Drawing.Point(12, 88);
            this.tb2.Name = "tb2";
            this.tb2.Size = new System.Drawing.Size(334, 20);
            this.tb2.TabIndex = 1;
            this.tb2.Text = "SD:/apps/example/boot.elf";
            // 
            // tb3
            // 
            this.tb3.Location = new System.Drawing.Point(12, 114);
            this.tb3.Name = "tb3";
            this.tb3.Size = new System.Drawing.Size(334, 20);
            this.tb3.TabIndex = 1;
            this.tb3.Text = "USB:/apps/example/boot.dol";
            // 
            // tb4
            // 
            this.tb4.Location = new System.Drawing.Point(12, 140);
            this.tb4.Name = "tb4";
            this.tb4.Size = new System.Drawing.Size(334, 20);
            this.tb4.TabIndex = 1;
            this.tb4.Text = "USB:/apps/example/boot.elf";
            // 
            // CustomizeMii_ComplexForwarder
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(358, 333);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnBrowseImage169);
            this.Controls.Add(this.btnBrowseImage43);
            this.Controls.Add(this.tbImage169);
            this.Controls.Add(this.tbImage43);
            this.Controls.Add(this.cbImage169);
            this.Controls.Add(this.cbImage43);
            this.Controls.Add(this.tb4);
            this.Controls.Add(this.tb3);
            this.Controls.Add(this.tb2);
            this.Controls.Add(this.tb1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CustomizeMii_ComplexForwarder";
            this.Text = "CustomizeMii_ComplexForwarder";
            this.Load += new System.EventHandler(this.CustomizeMii_ComplexForwarder_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox tb1;
        public System.Windows.Forms.CheckBox cbImage43;
        public System.Windows.Forms.CheckBox cbImage169;
        public System.Windows.Forms.TextBox tbImage43;
        public System.Windows.Forms.TextBox tbImage169;
        public System.Windows.Forms.Button btnBrowseImage43;
        public System.Windows.Forms.Button btnBrowseImage169;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox tb2;
        public System.Windows.Forms.TextBox tb3;
        public System.Windows.Forms.TextBox tb4;
    }
}