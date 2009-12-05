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
            this.lbAppFolder = new System.Windows.Forms.Label();
            this.tbAppFolder = new System.Windows.Forms.TextBox();
            this.panSdUsb = new System.Windows.Forms.Panel();
            this.rbUSB = new System.Windows.Forms.RadioButton();
            this.rbSD = new System.Windows.Forms.RadioButton();
            this.panDolElf = new System.Windows.Forms.Panel();
            this.rbELF = new System.Windows.Forms.RadioButton();
            this.rbDOL = new System.Windows.Forms.RadioButton();
            this.cbImage43 = new System.Windows.Forms.CheckBox();
            this.cbImage169 = new System.Windows.Forms.CheckBox();
            this.tbImage43 = new System.Windows.Forms.TextBox();
            this.tbImage169 = new System.Windows.Forms.TextBox();
            this.btnBrowseImage43 = new System.Windows.Forms.Button();
            this.btnBrowseImage169 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panSdUsb.SuspendLayout();
            this.panDolElf.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbAppFolder
            // 
            this.lbAppFolder.AutoSize = true;
            this.lbAppFolder.Location = new System.Drawing.Point(12, 13);
            this.lbAppFolder.Name = "lbAppFolder";
            this.lbAppFolder.Size = new System.Drawing.Size(107, 13);
            this.lbAppFolder.TabIndex = 0;
            this.lbAppFolder.Text = "Application Directory:";
            // 
            // tbAppFolder
            // 
            this.tbAppFolder.Location = new System.Drawing.Point(125, 10);
            this.tbAppFolder.Name = "tbAppFolder";
            this.tbAppFolder.Size = new System.Drawing.Size(221, 20);
            this.tbAppFolder.TabIndex = 1;
            // 
            // panSdUsb
            // 
            this.panSdUsb.Controls.Add(this.rbUSB);
            this.panSdUsb.Controls.Add(this.rbSD);
            this.panSdUsb.Location = new System.Drawing.Point(15, 38);
            this.panSdUsb.Name = "panSdUsb";
            this.panSdUsb.Size = new System.Drawing.Size(157, 64);
            this.panSdUsb.TabIndex = 2;
            // 
            // rbUSB
            // 
            this.rbUSB.AutoSize = true;
            this.rbUSB.Location = new System.Drawing.Point(24, 35);
            this.rbUSB.Name = "rbUSB";
            this.rbUSB.Size = new System.Drawing.Size(87, 17);
            this.rbUSB.TabIndex = 0;
            this.rbUSB.TabStop = true;
            this.rbUSB.Text = "Try USB First";
            this.rbUSB.UseVisualStyleBackColor = true;
            // 
            // rbSD
            // 
            this.rbSD.AutoSize = true;
            this.rbSD.Location = new System.Drawing.Point(24, 12);
            this.rbSD.Name = "rbSD";
            this.rbSD.Size = new System.Drawing.Size(80, 17);
            this.rbSD.TabIndex = 0;
            this.rbSD.TabStop = true;
            this.rbSD.Text = "Try SD First";
            this.rbSD.UseVisualStyleBackColor = true;
            // 
            // panDolElf
            // 
            this.panDolElf.Controls.Add(this.rbELF);
            this.panDolElf.Controls.Add(this.rbDOL);
            this.panDolElf.Location = new System.Drawing.Point(189, 38);
            this.panDolElf.Name = "panDolElf";
            this.panDolElf.Size = new System.Drawing.Size(157, 64);
            this.panDolElf.TabIndex = 2;
            // 
            // rbELF
            // 
            this.rbELF.AutoSize = true;
            this.rbELF.Location = new System.Drawing.Point(41, 35);
            this.rbELF.Name = "rbELF";
            this.rbELF.Size = new System.Drawing.Size(84, 17);
            this.rbELF.TabIndex = 0;
            this.rbELF.TabStop = true;
            this.rbELF.Text = "Try ELF First";
            this.rbELF.UseVisualStyleBackColor = true;
            // 
            // rbDOL
            // 
            this.rbDOL.AutoSize = true;
            this.rbDOL.Location = new System.Drawing.Point(41, 12);
            this.rbDOL.Name = "rbDOL";
            this.rbDOL.Size = new System.Drawing.Size(87, 17);
            this.rbDOL.TabIndex = 0;
            this.rbDOL.TabStop = true;
            this.rbDOL.Text = "Try DOL First";
            this.rbDOL.UseVisualStyleBackColor = true;
            // 
            // cbImage43
            // 
            this.cbImage43.AutoSize = true;
            this.cbImage43.Location = new System.Drawing.Point(15, 141);
            this.cbImage43.Name = "cbImage43";
            this.cbImage43.Size = new System.Drawing.Size(73, 17);
            this.cbImage43.TabIndex = 3;
            this.cbImage43.Text = "Image 4:3";
            this.cbImage43.UseVisualStyleBackColor = true;
            this.cbImage43.CheckedChanged += new System.EventHandler(this.cbImage43_CheckedChanged);
            // 
            // cbImage169
            // 
            this.cbImage169.AutoSize = true;
            this.cbImage169.Location = new System.Drawing.Point(15, 171);
            this.cbImage169.Name = "cbImage169";
            this.cbImage169.Size = new System.Drawing.Size(79, 17);
            this.cbImage169.TabIndex = 3;
            this.cbImage169.Text = "Image 16:9";
            this.cbImage169.UseVisualStyleBackColor = true;
            this.cbImage169.CheckedChanged += new System.EventHandler(this.cbImage169_CheckedChanged);
            // 
            // tbImage43
            // 
            this.tbImage43.Enabled = false;
            this.tbImage43.Location = new System.Drawing.Point(94, 139);
            this.tbImage43.Name = "tbImage43";
            this.tbImage43.Size = new System.Drawing.Size(171, 20);
            this.tbImage43.TabIndex = 4;
            // 
            // tbImage169
            // 
            this.tbImage169.Enabled = false;
            this.tbImage169.Location = new System.Drawing.Point(94, 168);
            this.tbImage169.Name = "tbImage169";
            this.tbImage169.Size = new System.Drawing.Size(171, 20);
            this.tbImage169.TabIndex = 5;
            // 
            // btnBrowseImage43
            // 
            this.btnBrowseImage43.Enabled = false;
            this.btnBrowseImage43.Location = new System.Drawing.Point(271, 139);
            this.btnBrowseImage43.Name = "btnBrowseImage43";
            this.btnBrowseImage43.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseImage43.TabIndex = 6;
            this.btnBrowseImage43.Text = "Browse...";
            this.btnBrowseImage43.UseVisualStyleBackColor = true;
            this.btnBrowseImage43.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnBrowseImage169
            // 
            this.btnBrowseImage169.Enabled = false;
            this.btnBrowseImage169.Location = new System.Drawing.Point(271, 168);
            this.btnBrowseImage169.Name = "btnBrowseImage169";
            this.btnBrowseImage169.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseImage169.TabIndex = 6;
            this.btnBrowseImage169.Text = "Browse...";
            this.btnBrowseImage169.UseVisualStyleBackColor = true;
            this.btnBrowseImage169.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 116);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(358, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "The images must be 640 x 480, else they will be resized!";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(15, 209);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(160, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(186, 209);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(160, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // CustomizeMii_ComplexForwarder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 245);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnBrowseImage169);
            this.Controls.Add(this.btnBrowseImage43);
            this.Controls.Add(this.tbImage169);
            this.Controls.Add(this.tbImage43);
            this.Controls.Add(this.cbImage169);
            this.Controls.Add(this.cbImage43);
            this.Controls.Add(this.panDolElf);
            this.Controls.Add(this.panSdUsb);
            this.Controls.Add(this.tbAppFolder);
            this.Controls.Add(this.lbAppFolder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CustomizeMii_ComplexForwarder";
            this.Text = "CustomizeMii_ComplexForwarder";
            this.panSdUsb.ResumeLayout(false);
            this.panSdUsb.PerformLayout();
            this.panDolElf.ResumeLayout(false);
            this.panDolElf.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbAppFolder;
        private System.Windows.Forms.Panel panSdUsb;
        private System.Windows.Forms.Panel panDolElf;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox tbAppFolder;
        public System.Windows.Forms.RadioButton rbUSB;
        public System.Windows.Forms.RadioButton rbSD;
        public System.Windows.Forms.RadioButton rbELF;
        public System.Windows.Forms.RadioButton rbDOL;
        public System.Windows.Forms.CheckBox cbImage43;
        public System.Windows.Forms.CheckBox cbImage169;
        public System.Windows.Forms.TextBox tbImage43;
        public System.Windows.Forms.TextBox tbImage169;
        public System.Windows.Forms.Button btnBrowseImage43;
        public System.Windows.Forms.Button btnBrowseImage169;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}