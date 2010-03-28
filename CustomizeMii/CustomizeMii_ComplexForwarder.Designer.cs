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
            this.tabPaths = new System.Windows.Forms.TabControl();
            this.tabRequired = new System.Windows.Forms.TabPage();
            this.tabOptional1 = new System.Windows.Forms.TabPage();
            this.cbPack1 = new System.Windows.Forms.CheckBox();
            this.tb5 = new System.Windows.Forms.TextBox();
            this.tb6 = new System.Windows.Forms.TextBox();
            this.tb7 = new System.Windows.Forms.TextBox();
            this.tb8 = new System.Windows.Forms.TextBox();
            this.tabOptional2 = new System.Windows.Forms.TabPage();
            this.cbPack2 = new System.Windows.Forms.CheckBox();
            this.tb9 = new System.Windows.Forms.TextBox();
            this.tb10 = new System.Windows.Forms.TextBox();
            this.tb11 = new System.Windows.Forms.TextBox();
            this.tb12 = new System.Windows.Forms.TextBox();
            this.tabOptional3 = new System.Windows.Forms.TabPage();
            this.cbPack3 = new System.Windows.Forms.CheckBox();
            this.tb13 = new System.Windows.Forms.TextBox();
            this.tb14 = new System.Windows.Forms.TextBox();
            this.tb15 = new System.Windows.Forms.TextBox();
            this.tb16 = new System.Windows.Forms.TextBox();
            this.tabPaths.SuspendLayout();
            this.tabRequired.SuspendLayout();
            this.tabOptional1.SuspendLayout();
            this.tabOptional2.SuspendLayout();
            this.tabOptional3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tb1
            // 
            this.tb1.Location = new System.Drawing.Point(6, 17);
            this.tb1.Name = "tb1";
            this.tb1.Size = new System.Drawing.Size(311, 20);
            this.tb1.TabIndex = 1;
            this.tb1.Text = "SD:/apps/example/boot.dol";
            this.tb1.TextChanged += new System.EventHandler(this.tb_TextChanged);
            // 
            // cbImage43
            // 
            this.cbImage43.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cbImage43.AutoSize = true;
            this.cbImage43.Location = new System.Drawing.Point(15, 282);
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
            this.cbImage169.Location = new System.Drawing.Point(15, 312);
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
            this.tbImage43.Location = new System.Drawing.Point(94, 280);
            this.tbImage43.Name = "tbImage43";
            this.tbImage43.ReadOnly = true;
            this.tbImage43.Size = new System.Drawing.Size(171, 20);
            this.tbImage43.TabIndex = 4;
            // 
            // tbImage169
            // 
            this.tbImage169.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.tbImage169.Enabled = false;
            this.tbImage169.Location = new System.Drawing.Point(94, 309);
            this.tbImage169.Name = "tbImage169";
            this.tbImage169.ReadOnly = true;
            this.tbImage169.Size = new System.Drawing.Size(171, 20);
            this.tbImage169.TabIndex = 5;
            // 
            // btnBrowseImage43
            // 
            this.btnBrowseImage43.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnBrowseImage43.Enabled = false;
            this.btnBrowseImage43.Location = new System.Drawing.Point(271, 280);
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
            this.btnBrowseImage169.Location = new System.Drawing.Point(271, 309);
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
            this.label1.Location = new System.Drawing.Point(0, 232);
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
            this.btnOK.Location = new System.Drawing.Point(15, 347);
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
            this.btnCancel.Location = new System.Drawing.Point(186, 347);
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
            this.label2.Location = new System.Drawing.Point(0, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(358, 40);
            this.label2.TabIndex = 7;
            this.label2.Text = "The forwarder will try to load in this order.\r\nEntries must begin with \"SD:/\" or " +
                "\"USB:/\" and end with \".dol\" or \".elf\"!\r\nYou may enter whatever path you want.";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tb2
            // 
            this.tb2.Location = new System.Drawing.Point(6, 46);
            this.tb2.Name = "tb2";
            this.tb2.Size = new System.Drawing.Size(311, 20);
            this.tb2.TabIndex = 1;
            this.tb2.Text = "SD:/apps/example/boot.elf";
            this.tb2.TextChanged += new System.EventHandler(this.tb_TextChanged);
            // 
            // tb3
            // 
            this.tb3.Location = new System.Drawing.Point(6, 75);
            this.tb3.Name = "tb3";
            this.tb3.Size = new System.Drawing.Size(311, 20);
            this.tb3.TabIndex = 1;
            this.tb3.Text = "USB:/apps/example/boot.dol";
            this.tb3.TextChanged += new System.EventHandler(this.tb_TextChanged);
            // 
            // tb4
            // 
            this.tb4.Location = new System.Drawing.Point(6, 104);
            this.tb4.Name = "tb4";
            this.tb4.Size = new System.Drawing.Size(311, 20);
            this.tb4.TabIndex = 1;
            this.tb4.Text = "USB:/apps/example/boot.elf";
            this.tb4.TextChanged += new System.EventHandler(this.tb_TextChanged);
            // 
            // tabPaths
            // 
            this.tabPaths.Controls.Add(this.tabRequired);
            this.tabPaths.Controls.Add(this.tabOptional1);
            this.tabPaths.Controls.Add(this.tabOptional2);
            this.tabPaths.Controls.Add(this.tabOptional3);
            this.tabPaths.Location = new System.Drawing.Point(15, 62);
            this.tabPaths.Name = "tabPaths";
            this.tabPaths.SelectedIndex = 0;
            this.tabPaths.Size = new System.Drawing.Size(331, 159);
            this.tabPaths.TabIndex = 9;
            // 
            // tabRequired
            // 
            this.tabRequired.Controls.Add(this.tb1);
            this.tabRequired.Controls.Add(this.tb2);
            this.tabRequired.Controls.Add(this.tb3);
            this.tabRequired.Controls.Add(this.tb4);
            this.tabRequired.Location = new System.Drawing.Point(4, 22);
            this.tabRequired.Name = "tabRequired";
            this.tabRequired.Padding = new System.Windows.Forms.Padding(3);
            this.tabRequired.Size = new System.Drawing.Size(323, 133);
            this.tabRequired.TabIndex = 0;
            this.tabRequired.Text = "Required";
            this.tabRequired.UseVisualStyleBackColor = true;
            // 
            // tabOptional1
            // 
            this.tabOptional1.Controls.Add(this.cbPack1);
            this.tabOptional1.Controls.Add(this.tb5);
            this.tabOptional1.Controls.Add(this.tb6);
            this.tabOptional1.Controls.Add(this.tb7);
            this.tabOptional1.Controls.Add(this.tb8);
            this.tabOptional1.Location = new System.Drawing.Point(4, 22);
            this.tabOptional1.Name = "tabOptional1";
            this.tabOptional1.Padding = new System.Windows.Forms.Padding(3);
            this.tabOptional1.Size = new System.Drawing.Size(323, 133);
            this.tabOptional1.TabIndex = 1;
            this.tabOptional1.Text = "Optional 1";
            this.tabOptional1.UseVisualStyleBackColor = true;
            // 
            // cbPack1
            // 
            this.cbPack1.AutoSize = true;
            this.cbPack1.Location = new System.Drawing.Point(6, 6);
            this.cbPack1.Name = "cbPack1";
            this.cbPack1.Size = new System.Drawing.Size(59, 17);
            this.cbPack1.TabIndex = 6;
            this.cbPack1.Text = "Enable";
            this.cbPack1.UseVisualStyleBackColor = true;
            this.cbPack1.CheckedChanged += new System.EventHandler(this.cbPack1_CheckedChanged);
            // 
            // tb5
            // 
            this.tb5.Enabled = false;
            this.tb5.Location = new System.Drawing.Point(6, 29);
            this.tb5.Name = "tb5";
            this.tb5.Size = new System.Drawing.Size(311, 20);
            this.tb5.TabIndex = 4;
            this.tb5.TextChanged += new System.EventHandler(this.tb_TextChanged);
            // 
            // tb6
            // 
            this.tb6.Enabled = false;
            this.tb6.Location = new System.Drawing.Point(6, 54);
            this.tb6.Name = "tb6";
            this.tb6.Size = new System.Drawing.Size(311, 20);
            this.tb6.TabIndex = 5;
            this.tb6.TextChanged += new System.EventHandler(this.tb_TextChanged);
            // 
            // tb7
            // 
            this.tb7.Enabled = false;
            this.tb7.Location = new System.Drawing.Point(6, 79);
            this.tb7.Name = "tb7";
            this.tb7.Size = new System.Drawing.Size(311, 20);
            this.tb7.TabIndex = 2;
            this.tb7.TextChanged += new System.EventHandler(this.tb_TextChanged);
            // 
            // tb8
            // 
            this.tb8.Enabled = false;
            this.tb8.Location = new System.Drawing.Point(6, 104);
            this.tb8.Name = "tb8";
            this.tb8.Size = new System.Drawing.Size(311, 20);
            this.tb8.TabIndex = 3;
            this.tb8.TextChanged += new System.EventHandler(this.tb_TextChanged);
            // 
            // tabOptional2
            // 
            this.tabOptional2.Controls.Add(this.cbPack2);
            this.tabOptional2.Controls.Add(this.tb9);
            this.tabOptional2.Controls.Add(this.tb10);
            this.tabOptional2.Controls.Add(this.tb11);
            this.tabOptional2.Controls.Add(this.tb12);
            this.tabOptional2.Location = new System.Drawing.Point(4, 22);
            this.tabOptional2.Name = "tabOptional2";
            this.tabOptional2.Padding = new System.Windows.Forms.Padding(3);
            this.tabOptional2.Size = new System.Drawing.Size(323, 133);
            this.tabOptional2.TabIndex = 2;
            this.tabOptional2.Text = "Optional 2";
            this.tabOptional2.UseVisualStyleBackColor = true;
            // 
            // cbPack2
            // 
            this.cbPack2.AutoSize = true;
            this.cbPack2.Location = new System.Drawing.Point(6, 6);
            this.cbPack2.Name = "cbPack2";
            this.cbPack2.Size = new System.Drawing.Size(59, 17);
            this.cbPack2.TabIndex = 11;
            this.cbPack2.Text = "Enable";
            this.cbPack2.UseVisualStyleBackColor = true;
            this.cbPack2.CheckedChanged += new System.EventHandler(this.cbPack2_CheckedChanged);
            // 
            // tb9
            // 
            this.tb9.Enabled = false;
            this.tb9.Location = new System.Drawing.Point(6, 29);
            this.tb9.Name = "tb9";
            this.tb9.Size = new System.Drawing.Size(311, 20);
            this.tb9.TabIndex = 9;
            this.tb9.TextChanged += new System.EventHandler(this.tb_TextChanged);
            // 
            // tb10
            // 
            this.tb10.Enabled = false;
            this.tb10.Location = new System.Drawing.Point(6, 54);
            this.tb10.Name = "tb10";
            this.tb10.Size = new System.Drawing.Size(311, 20);
            this.tb10.TabIndex = 10;
            this.tb10.TextChanged += new System.EventHandler(this.tb_TextChanged);
            // 
            // tb11
            // 
            this.tb11.Enabled = false;
            this.tb11.Location = new System.Drawing.Point(6, 79);
            this.tb11.Name = "tb11";
            this.tb11.Size = new System.Drawing.Size(311, 20);
            this.tb11.TabIndex = 7;
            this.tb11.TextChanged += new System.EventHandler(this.tb_TextChanged);
            // 
            // tb12
            // 
            this.tb12.Enabled = false;
            this.tb12.Location = new System.Drawing.Point(6, 104);
            this.tb12.Name = "tb12";
            this.tb12.Size = new System.Drawing.Size(311, 20);
            this.tb12.TabIndex = 8;
            this.tb12.TextChanged += new System.EventHandler(this.tb_TextChanged);
            // 
            // tabOptional3
            // 
            this.tabOptional3.Controls.Add(this.cbPack3);
            this.tabOptional3.Controls.Add(this.tb13);
            this.tabOptional3.Controls.Add(this.tb14);
            this.tabOptional3.Controls.Add(this.tb15);
            this.tabOptional3.Controls.Add(this.tb16);
            this.tabOptional3.Location = new System.Drawing.Point(4, 22);
            this.tabOptional3.Name = "tabOptional3";
            this.tabOptional3.Padding = new System.Windows.Forms.Padding(3);
            this.tabOptional3.Size = new System.Drawing.Size(323, 133);
            this.tabOptional3.TabIndex = 3;
            this.tabOptional3.Text = "Optional 3";
            this.tabOptional3.UseVisualStyleBackColor = true;
            // 
            // cbPack3
            // 
            this.cbPack3.AutoSize = true;
            this.cbPack3.Location = new System.Drawing.Point(6, 6);
            this.cbPack3.Name = "cbPack3";
            this.cbPack3.Size = new System.Drawing.Size(59, 17);
            this.cbPack3.TabIndex = 11;
            this.cbPack3.Text = "Enable";
            this.cbPack3.UseVisualStyleBackColor = true;
            this.cbPack3.CheckedChanged += new System.EventHandler(this.cbPack3_CheckedChanged);
            // 
            // tb13
            // 
            this.tb13.Enabled = false;
            this.tb13.Location = new System.Drawing.Point(6, 29);
            this.tb13.Name = "tb13";
            this.tb13.Size = new System.Drawing.Size(311, 20);
            this.tb13.TabIndex = 9;
            this.tb13.TextChanged += new System.EventHandler(this.tb_TextChanged);
            // 
            // tb14
            // 
            this.tb14.Enabled = false;
            this.tb14.Location = new System.Drawing.Point(6, 54);
            this.tb14.Name = "tb14";
            this.tb14.Size = new System.Drawing.Size(311, 20);
            this.tb14.TabIndex = 10;
            this.tb14.TextChanged += new System.EventHandler(this.tb_TextChanged);
            // 
            // tb15
            // 
            this.tb15.Enabled = false;
            this.tb15.Location = new System.Drawing.Point(6, 79);
            this.tb15.Name = "tb15";
            this.tb15.Size = new System.Drawing.Size(311, 20);
            this.tb15.TabIndex = 7;
            this.tb15.TextChanged += new System.EventHandler(this.tb_TextChanged);
            // 
            // tb16
            // 
            this.tb16.Enabled = false;
            this.tb16.Location = new System.Drawing.Point(6, 104);
            this.tb16.Name = "tb16";
            this.tb16.Size = new System.Drawing.Size(311, 20);
            this.tb16.TabIndex = 8;
            this.tb16.TextChanged += new System.EventHandler(this.tb_TextChanged);
            // 
            // CustomizeMii_ComplexForwarder
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(358, 383);
            this.Controls.Add(this.tabPaths);
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
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CustomizeMii_ComplexForwarder";
            this.Text = "CustomizeMii_ComplexForwarder";
            this.Load += new System.EventHandler(this.CustomizeMii_ComplexForwarder_Load);
            this.tabPaths.ResumeLayout(false);
            this.tabRequired.ResumeLayout(false);
            this.tabRequired.PerformLayout();
            this.tabOptional1.ResumeLayout(false);
            this.tabOptional1.PerformLayout();
            this.tabOptional2.ResumeLayout(false);
            this.tabOptional2.PerformLayout();
            this.tabOptional3.ResumeLayout(false);
            this.tabOptional3.PerformLayout();
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
        private System.Windows.Forms.TabControl tabPaths;
        private System.Windows.Forms.TabPage tabRequired;
        private System.Windows.Forms.TabPage tabOptional1;
        private System.Windows.Forms.TabPage tabOptional2;
        private System.Windows.Forms.TabPage tabOptional3;
        public System.Windows.Forms.TextBox tb5;
        public System.Windows.Forms.TextBox tb6;
        public System.Windows.Forms.TextBox tb7;
        public System.Windows.Forms.TextBox tb8;
        public System.Windows.Forms.TextBox tb9;
        public System.Windows.Forms.TextBox tb10;
        public System.Windows.Forms.TextBox tb11;
        public System.Windows.Forms.TextBox tb12;
        public System.Windows.Forms.TextBox tb13;
        public System.Windows.Forms.TextBox tb14;
        public System.Windows.Forms.TextBox tb15;
        public System.Windows.Forms.TextBox tb16;
        public System.Windows.Forms.CheckBox cbPack1;
        public System.Windows.Forms.CheckBox cbPack2;
        public System.Windows.Forms.CheckBox cbPack3;
    }
}