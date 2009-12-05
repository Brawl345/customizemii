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
    partial class CustomizeMii_InputBox
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
            this.tbInput = new System.Windows.Forms.TextBox();
            this.lbInfo = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.cbElf = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // tbInput
            // 
            this.tbInput.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.tbInput.Location = new System.Drawing.Point(7, 34);
            this.tbInput.MaxLength = 3;
            this.tbInput.Name = "tbInput";
            this.tbInput.Size = new System.Drawing.Size(222, 20);
            this.tbInput.TabIndex = 0;
            // 
            // lbInfo
            // 
            this.lbInfo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbInfo.Location = new System.Drawing.Point(4, 9);
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.Size = new System.Drawing.Size(225, 32);
            this.lbInfo.TabIndex = 1;
            this.lbInfo.Text = "Please enter \"45e\" to create the Common-Key";
            this.lbInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOK.Location = new System.Drawing.Point(7, 62);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(108, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point(121, 62);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(108, 23);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // cbElf
            // 
            this.cbElf.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cbElf.AutoSize = true;
            this.cbElf.Location = new System.Drawing.Point(61, 16);
            this.cbElf.Name = "cbElf";
            this.cbElf.Size = new System.Drawing.Size(114, 17);
            this.cbElf.TabIndex = 4;
            this.cbElf.Text = "Forward to boot.elf";
            this.cbElf.UseVisualStyleBackColor = true;
            this.cbElf.Visible = false;
            // 
            // CustomizeMii_InputBox
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(238, 92);
            this.Controls.Add(this.cbElf);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tbInput);
            this.Controls.Add(this.lbInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CustomizeMii_InputBox";
            this.ShowInTaskbar = false;
            this.Text = "InputBox";
            this.Load += new System.EventHandler(this.CustomizeMii_InputBox_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        public System.Windows.Forms.Label lbInfo;
        public System.Windows.Forms.TextBox tbInput;
        public System.Windows.Forms.Button btnExit;
        public System.Windows.Forms.CheckBox cbElf;
    }
}