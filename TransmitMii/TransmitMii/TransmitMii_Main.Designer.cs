/* This file is part of TransmitMii
 * Copyright (C) 2009 Leathl
 * 
 * TransmitMii is free software: you can redistribute it and/or
 * modify it under the terms of the GNU General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * TransmitMii is distributed in the hope that it will be
 * useful, but WITHOUT ANY WARRANTY; without even the implied warranty
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
 
namespace TransmitMii
{
    partial class TransmitMii_Main
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
            this.lbIP = new System.Windows.Forms.Label();
            this.lbFile = new System.Windows.Forms.Label();
            this.tbFile = new System.Windows.Forms.TextBox();
            this.btnBrowseFile = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.lbStatus = new System.Windows.Forms.Label();
            this.tbIP = new System.Windows.Forms.TextBox();
            this.lbProtocol = new System.Windows.Forms.Label();
            this.cmbProtocol = new System.Windows.Forms.ComboBox();
            this.llbLinkExtension = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // lbIP
            // 
            this.lbIP.AutoSize = true;
            this.lbIP.Location = new System.Drawing.Point(12, 76);
            this.lbIP.Name = "lbIP";
            this.lbIP.Size = new System.Drawing.Size(61, 13);
            this.lbIP.TabIndex = 2;
            this.lbIP.Text = "IP Address:";
            // 
            // lbFile
            // 
            this.lbFile.AutoSize = true;
            this.lbFile.Location = new System.Drawing.Point(12, 46);
            this.lbFile.Name = "lbFile";
            this.lbFile.Size = new System.Drawing.Size(26, 13);
            this.lbFile.TabIndex = 3;
            this.lbFile.Text = "File:";
            // 
            // tbFile
            // 
            this.tbFile.Location = new System.Drawing.Point(44, 43);
            this.tbFile.Name = "tbFile";
            this.tbFile.Size = new System.Drawing.Size(166, 20);
            this.tbFile.TabIndex = 4;
            this.tbFile.MouseLeave += new System.EventHandler(this.tbFile_MouseLeave);
            this.tbFile.MouseEnter += new System.EventHandler(this.tbFile_MouseEnter);
            // 
            // btnBrowseFile
            // 
            this.btnBrowseFile.Location = new System.Drawing.Point(216, 42);
            this.btnBrowseFile.Name = "btnBrowseFile";
            this.btnBrowseFile.Size = new System.Drawing.Size(75, 20);
            this.btnBrowseFile.TabIndex = 5;
            this.btnBrowseFile.Text = "Browse...";
            this.btnBrowseFile.UseVisualStyleBackColor = true;
            this.btnBrowseFile.Click += new System.EventHandler(this.btnBrowseFile_Click);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(216, 72);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 20);
            this.btnSend.TabIndex = 5;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Location = new System.Drawing.Point(12, 108);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(40, 13);
            this.lbStatus.TabIndex = 7;
            this.lbStatus.Text = "Status:";
            // 
            // tbIP
            // 
            this.tbIP.Location = new System.Drawing.Point(79, 72);
            this.tbIP.Name = "tbIP";
            this.tbIP.Size = new System.Drawing.Size(131, 20);
            this.tbIP.TabIndex = 8;
            this.tbIP.MouseLeave += new System.EventHandler(this.tbIP_MouseLeave);
            this.tbIP.MouseEnter += new System.EventHandler(this.tbIP_MouseEnter);
            // 
            // lbProtocol
            // 
            this.lbProtocol.AutoSize = true;
            this.lbProtocol.Location = new System.Drawing.Point(12, 16);
            this.lbProtocol.Name = "lbProtocol";
            this.lbProtocol.Size = new System.Drawing.Size(49, 13);
            this.lbProtocol.TabIndex = 9;
            this.lbProtocol.Text = "Protocol:";
            // 
            // cmbProtocol
            // 
            this.cmbProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProtocol.FormattingEnabled = true;
            this.cmbProtocol.Items.AddRange(new object[] {
            "HBC 1.0.5+ (JODI)",
            "HBC -1.0.4 (HAXX)",
            "USB Loader GX"});
            this.cmbProtocol.Location = new System.Drawing.Point(67, 13);
            this.cmbProtocol.Name = "cmbProtocol";
            this.cmbProtocol.Size = new System.Drawing.Size(224, 21);
            this.cmbProtocol.TabIndex = 10;
            this.cmbProtocol.MouseEnter += new System.EventHandler(this.cmbProtocol_MouseEnter);
            this.cmbProtocol.MouseLeave += new System.EventHandler(this.cmbProtocol_MouseLeave);
            // 
            // llbLinkExtension
            // 
            this.llbLinkExtension.AutoSize = true;
            this.llbLinkExtension.Location = new System.Drawing.Point(215, 108);
            this.llbLinkExtension.Name = "llbLinkExtension";
            this.llbLinkExtension.Size = new System.Drawing.Size(76, 13);
            this.llbLinkExtension.TabIndex = 11;
            this.llbLinkExtension.TabStop = true;
            this.llbLinkExtension.Text = "Link Extension";
            this.llbLinkExtension.MouseLeave += new System.EventHandler(this.llbLinkExtension_MouseLeave);
            this.llbLinkExtension.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llbLinkExtension_LinkClicked);
            this.llbLinkExtension.MouseEnter += new System.EventHandler(this.llbLinkExtension_MouseEnter);
            // 
            // TransmitMii_Main
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(303, 128);
            this.Controls.Add(this.llbLinkExtension);
            this.Controls.Add(this.cmbProtocol);
            this.Controls.Add(this.lbProtocol);
            this.Controls.Add(this.tbIP);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnBrowseFile);
            this.Controls.Add(this.tbFile);
            this.Controls.Add(this.lbFile);
            this.Controls.Add(this.lbIP);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "TransmitMii_Main";
            this.Text = "TransmitMii X by Leathl";
            this.Load += new System.EventHandler(this.TransmitMii_Main_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.TransmitMii_Main_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.TransmitMii_Main_DragEnter);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TransmitMii_Main_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbIP;
        private System.Windows.Forms.Label lbFile;
        private System.Windows.Forms.TextBox tbFile;
        private System.Windows.Forms.Button btnBrowseFile;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.TextBox tbIP;
        private System.Windows.Forms.Label lbProtocol;
        private System.Windows.Forms.ComboBox cmbProtocol;
        private System.Windows.Forms.LinkLabel llbLinkExtension;
    }
}

