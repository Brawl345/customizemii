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
    partial class CustomizeMii_Transmit
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
            this.lbProtocol = new System.Windows.Forms.Label();
            this.cmbProtocol = new System.Windows.Forms.ComboBox();
            this.lbIP = new System.Windows.Forms.Label();
            this.tbIP = new System.Windows.Forms.TextBox();
            this.lbIOS = new System.Windows.Forms.Label();
            this.tbIOS = new System.Windows.Forms.TextBox();
            this.btnTransmit = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbProtocol
            // 
            this.lbProtocol.AutoSize = true;
            this.lbProtocol.Location = new System.Drawing.Point(12, 16);
            this.lbProtocol.Name = "lbProtocol";
            this.lbProtocol.Size = new System.Drawing.Size(49, 13);
            this.lbProtocol.TabIndex = 0;
            this.lbProtocol.Text = "Protocol:";
            // 
            // cmbProtocol
            // 
            this.cmbProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProtocol.FormattingEnabled = true;
            this.cmbProtocol.Items.AddRange(new object[] {
            "Homebrewchannel 1.0.5+ (JODI)",
            "Homebrewchannel -1.0.4 (HAXX)"});
            this.cmbProtocol.Location = new System.Drawing.Point(67, 13);
            this.cmbProtocol.Name = "cmbProtocol";
            this.cmbProtocol.Size = new System.Drawing.Size(194, 21);
            this.cmbProtocol.TabIndex = 1;
            // 
            // lbIP
            // 
            this.lbIP.AutoSize = true;
            this.lbIP.Location = new System.Drawing.Point(12, 48);
            this.lbIP.Name = "lbIP";
            this.lbIP.Size = new System.Drawing.Size(61, 13);
            this.lbIP.TabIndex = 2;
            this.lbIP.Text = "IP Address:";
            // 
            // tbIP
            // 
            this.tbIP.Location = new System.Drawing.Point(79, 45);
            this.tbIP.Name = "tbIP";
            this.tbIP.Size = new System.Drawing.Size(182, 20);
            this.tbIP.TabIndex = 3;
            // 
            // lbIOS
            // 
            this.lbIOS.AutoSize = true;
            this.lbIOS.Location = new System.Drawing.Point(12, 78);
            this.lbIOS.Name = "lbIOS";
            this.lbIOS.Size = new System.Drawing.Size(127, 13);
            this.lbIOS.TabIndex = 4;
            this.lbIOS.Text = "IOS to use for installation:";
            // 
            // tbIOS
            // 
            this.tbIOS.Location = new System.Drawing.Point(145, 75);
            this.tbIOS.Name = "tbIOS";
            this.tbIOS.Size = new System.Drawing.Size(116, 20);
            this.tbIOS.TabIndex = 5;
            this.tbIOS.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbIOS_KeyPress);
            // 
            // btnTransmit
            // 
            this.btnTransmit.Location = new System.Drawing.Point(15, 109);
            this.btnTransmit.Name = "btnTransmit";
            this.btnTransmit.Size = new System.Drawing.Size(120, 23);
            this.btnTransmit.TabIndex = 6;
            this.btnTransmit.Text = "Transmit";
            this.btnTransmit.UseVisualStyleBackColor = true;
            this.btnTransmit.Click += new System.EventHandler(this.btnTransmit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(141, 109);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // CustomizeMii_Transmit
            // 
            this.AcceptButton = this.btnTransmit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(273, 145);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnTransmit);
            this.Controls.Add(this.tbIOS);
            this.Controls.Add(this.lbIOS);
            this.Controls.Add(this.tbIP);
            this.Controls.Add(this.lbIP);
            this.Controls.Add(this.cmbProtocol);
            this.Controls.Add(this.lbProtocol);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CustomizeMii_Transmit";
            this.Text = "CustomizeMii_Transmit";
            this.Load += new System.EventHandler(this.CustomizeMii_Transmit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbProtocol;
        private System.Windows.Forms.ComboBox cmbProtocol;
        private System.Windows.Forms.Label lbIP;
        private System.Windows.Forms.TextBox tbIP;
        private System.Windows.Forms.Label lbIOS;
        private System.Windows.Forms.TextBox tbIOS;
        private System.Windows.Forms.Button btnTransmit;
        private System.Windows.Forms.Button btnCancel;
    }
}