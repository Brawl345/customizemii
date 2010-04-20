/* This file is part of ShowMiiWads
 * Copyright (C) 2009 Leathl
 * 
 * ShowMiiWads is free software: you can redistribute it and/or
 * modify it under the terms of the GNU General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * ShowMiiWads is distributed in the hope that it will be
 * useful, but WITHOUT ANY WARRANTY; without even the implied warranty
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

namespace CustomizeMii
{
    partial class CustomizeMii_Preview
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
            this.Panel = new System.Windows.Forms.Panel();
            this.cbCheckerBoard = new System.Windows.Forms.CheckBox();
            this.btnReplace = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.cbIcon = new System.Windows.Forms.ComboBox();
            this.lbIcon = new System.Windows.Forms.Label();
            this.lbBanner = new System.Windows.Forms.Label();
            this.cbBanner = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbFormat = new System.Windows.Forms.Label();
            this.lbFormatText = new System.Windows.Forms.Label();
            this.lbSize = new System.Windows.Forms.Label();
            this.lbSizeText = new System.Windows.Forms.Label();
            this.pbPic = new System.Windows.Forms.PictureBox();
            this.cmFormat = new System.Windows.Forms.ContextMenuStrip();
            this.cmRGBA8 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmRGB565 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmRGB5A3 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmIA8 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmIA4 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmI8 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmI4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmChangeFormat = new System.Windows.Forms.ToolStripMenuItem();
            this.cmToRGBA8 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmToRGB565 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmToRGB5A3 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmToIA8 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmToIA4 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmToI8 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmToI4 = new System.Windows.Forms.ToolStripMenuItem();
            this.lbNoPreview = new System.Windows.Forms.Label();
            this.cmCI8 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmCI4 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmCI8RGB5A3 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmCI8RGB565 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmCI8IA8 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmCI4RGB5A3 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmCI4RGB565 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmCI4IA8 = new System.Windows.Forms.ToolStripMenuItem();
            this.lbTip = new System.Windows.Forms.Label();
            this.cmToCI8 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmToCI4 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmToCI8RGB5A3 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmToCI8RGB565 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmToCI8IA8 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmToCI4RGB5A3 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmToCI4RGB565 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmToCI4IA8 = new System.Windows.Forms.ToolStripMenuItem();
            this.Panel.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPic)).BeginInit();
            this.cmFormat.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel
            // 
            this.Panel.Controls.Add(this.cbCheckerBoard);
            this.Panel.Controls.Add(this.btnReplace);
            this.Panel.Controls.Add(this.btnClose);
            this.Panel.Controls.Add(this.cbIcon);
            this.Panel.Controls.Add(this.lbIcon);
            this.Panel.Controls.Add(this.lbBanner);
            this.Panel.Controls.Add(this.cbBanner);
            this.Panel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Panel.Location = new System.Drawing.Point(0, 434);
            this.Panel.Name = "Panel";
            this.Panel.Size = new System.Drawing.Size(817, 28);
            this.Panel.TabIndex = 0;
            // 
            // cbCheckerBoard
            // 
            this.cbCheckerBoard.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cbCheckerBoard.AutoSize = true;
            this.cbCheckerBoard.Checked = true;
            this.cbCheckerBoard.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCheckerBoard.Location = new System.Drawing.Point(390, 7);
            this.cbCheckerBoard.Name = "cbCheckerBoard";
            this.cbCheckerBoard.Size = new System.Drawing.Size(93, 17);
            this.cbCheckerBoard.TabIndex = 4;
            this.cbCheckerBoard.Text = "Checkerboard";
            this.cbCheckerBoard.UseVisualStyleBackColor = true;
            this.cbCheckerBoard.CheckedChanged += new System.EventHandler(this.cbCheckerBoard_CheckedChanged);
            // 
            // btnReplace
            // 
            this.btnReplace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReplace.Location = new System.Drawing.Point(513, 4);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(137, 21);
            this.btnReplace.TabIndex = 3;
            this.btnReplace.Text = "Replace";
            this.btnReplace.UseVisualStyleBackColor = true;
            this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(670, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(137, 21);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // cbIcon
            // 
            this.cbIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbIcon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIcon.FormattingEnabled = true;
            this.cbIcon.Location = new System.Drawing.Point(234, 4);
            this.cbIcon.MaxDropDownItems = 20;
            this.cbIcon.Name = "cbIcon";
            this.cbIcon.Size = new System.Drawing.Size(121, 21);
            this.cbIcon.TabIndex = 1;
            this.cbIcon.SelectedIndexChanged += new System.EventHandler(this.cbIcon_SelectedIndexChanged);
            // 
            // lbIcon
            // 
            this.lbIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbIcon.AutoSize = true;
            this.lbIcon.Location = new System.Drawing.Point(198, 7);
            this.lbIcon.Name = "lbIcon";
            this.lbIcon.Size = new System.Drawing.Size(31, 13);
            this.lbIcon.TabIndex = 2;
            this.lbIcon.Text = "Icon:";
            // 
            // lbBanner
            // 
            this.lbBanner.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbBanner.AutoSize = true;
            this.lbBanner.Location = new System.Drawing.Point(3, 7);
            this.lbBanner.Name = "lbBanner";
            this.lbBanner.Size = new System.Drawing.Size(44, 13);
            this.lbBanner.TabIndex = 1;
            this.lbBanner.Text = "Banner:";
            // 
            // cbBanner
            // 
            this.cbBanner.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbBanner.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBanner.FormattingEnabled = true;
            this.cbBanner.Location = new System.Drawing.Point(51, 4);
            this.cbBanner.MaxDropDownItems = 20;
            this.cbBanner.Name = "cbBanner";
            this.cbBanner.Size = new System.Drawing.Size(121, 21);
            this.cbBanner.TabIndex = 0;
            this.cbBanner.SelectedIndexChanged += new System.EventHandler(this.cbBanner_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbTip);
            this.panel1.Controls.Add(this.lbFormat);
            this.panel1.Controls.Add(this.lbFormatText);
            this.panel1.Controls.Add(this.lbSize);
            this.panel1.Controls.Add(this.lbSizeText);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(817, 22);
            this.panel1.TabIndex = 2;
            // 
            // lbFormat
            // 
            this.lbFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbFormat.AutoSize = true;
            this.lbFormat.Location = new System.Drawing.Point(729, 5);
            this.lbFormat.Name = "lbFormat";
            this.lbFormat.Size = new System.Drawing.Size(43, 13);
            this.lbFormat.TabIndex = 3;
            this.lbFormat.Text = "RGBA8";
            // 
            // lbFormatText
            // 
            this.lbFormatText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbFormatText.Location = new System.Drawing.Point(523, 5);
            this.lbFormatText.Name = "lbFormatText";
            this.lbFormatText.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbFormatText.Size = new System.Drawing.Size(205, 13);
            this.lbFormatText.TabIndex = 2;
            this.lbFormatText.Text = "Texture Format:";
            this.lbFormatText.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbSize
            // 
            this.lbSize.AutoSize = true;
            this.lbSize.Location = new System.Drawing.Point(39, 5);
            this.lbSize.Name = "lbSize";
            this.lbSize.Size = new System.Drawing.Size(30, 13);
            this.lbSize.TabIndex = 1;
            this.lbSize.Text = "0 x 0";
            // 
            // lbSizeText
            // 
            this.lbSizeText.AutoSize = true;
            this.lbSizeText.Location = new System.Drawing.Point(4, 5);
            this.lbSizeText.Name = "lbSizeText";
            this.lbSizeText.Size = new System.Drawing.Size(30, 13);
            this.lbSizeText.TabIndex = 0;
            this.lbSizeText.Text = "Size:";
            // 
            // pbPic
            // 
            this.pbPic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbPic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbPic.Location = new System.Drawing.Point(0, 22);
            this.pbPic.Name = "pbPic";
            this.pbPic.Size = new System.Drawing.Size(817, 412);
            this.pbPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbPic.TabIndex = 3;
            this.pbPic.TabStop = false;
            // 
            // cmFormat
            // 
            this.cmFormat.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmRGBA8,
            this.cmRGB565,
            this.cmRGB5A3,
            this.cmIA8,
            this.cmIA4,
            this.cmI8,
            this.cmI4,
            this.cmCI8,
            this.cmCI4,
            this.toolStripSeparator1,
            this.cmChangeFormat});
            this.cmFormat.Name = "cmFormat";
            this.cmFormat.Size = new System.Drawing.Size(267, 252);
            // 
            // cmRGBA8
            // 
            this.cmRGBA8.Name = "cmRGBA8";
            this.cmRGBA8.Size = new System.Drawing.Size(266, 22);
            this.cmRGBA8.Tag = "rgba8";
            this.cmRGBA8.Text = "As RGBA8 (High Quality with Alpha)";
            this.cmRGBA8.Click += new System.EventHandler(this.cmFormat_Click);
            // 
            // cmRGB565
            // 
            this.cmRGB565.Name = "cmRGB565";
            this.cmRGB565.Size = new System.Drawing.Size(266, 22);
            this.cmRGB565.Tag = "rgb565";
            this.cmRGB565.Text = "As RGB565 (Moderate Quality)";
            this.cmRGB565.Click += new System.EventHandler(this.cmFormat_Click);
            // 
            // cmRGB5A3
            // 
            this.cmRGB5A3.Name = "cmRGB5A3";
            this.cmRGB5A3.Size = new System.Drawing.Size(266, 22);
            this.cmRGB5A3.Tag = "rgb5a3";
            this.cmRGB5A3.Text = "As RGB5A3 (Low Quality with Alpha)";
            this.cmRGB5A3.Click += new System.EventHandler(this.cmFormat_Click);
            // 
            // cmIA8
            // 
            this.cmIA8.Name = "cmIA8";
            this.cmIA8.Size = new System.Drawing.Size(266, 22);
            this.cmIA8.Tag = "ia8";
            this.cmIA8.Text = "As IA8 (B/W with Alpha)";
            this.cmIA8.Click += new System.EventHandler(this.cmFormat_Click);
            // 
            // cmIA4
            // 
            this.cmIA4.Name = "cmIA4";
            this.cmIA4.Size = new System.Drawing.Size(266, 22);
            this.cmIA4.Tag = "ia4";
            this.cmIA4.Text = "As IA4 (B/W with Alpha)";
            this.cmIA4.Click += new System.EventHandler(this.cmFormat_Click);
            // 
            // cmI8
            // 
            this.cmI8.Name = "cmI8";
            this.cmI8.Size = new System.Drawing.Size(266, 22);
            this.cmI8.Tag = "i8";
            this.cmI8.Text = "As I8 (B/W)";
            this.cmI8.Click += new System.EventHandler(this.cmFormat_Click);
            // 
            // cmI4
            // 
            this.cmI4.Name = "cmI4";
            this.cmI4.Size = new System.Drawing.Size(266, 22);
            this.cmI4.Tag = "i4";
            this.cmI4.Text = "As I4 (B/W)";
            this.cmI4.Click += new System.EventHandler(this.cmFormat_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(263, 6);
            // 
            // cmChangeFormat
            // 
            this.cmChangeFormat.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmToRGBA8,
            this.cmToRGB565,
            this.cmToRGB5A3,
            this.cmToIA8,
            this.cmToIA4,
            this.cmToI8,
            this.cmToI4,
            this.cmToCI8,
            this.cmToCI4});
            this.cmChangeFormat.Name = "cmChangeFormat";
            this.cmChangeFormat.Size = new System.Drawing.Size(266, 22);
            this.cmChangeFormat.Text = "Change Format";
            // 
            // cmToRGBA8
            // 
            this.cmToRGBA8.Name = "cmToRGBA8";
            this.cmToRGBA8.Size = new System.Drawing.Size(267, 22);
            this.cmToRGBA8.Tag = "rgba8";
            this.cmToRGBA8.Text = "To RGBA8 (High Quality with Alpha)";
            this.cmToRGBA8.Click += new System.EventHandler(this.cmChangeFormat_Click);
            // 
            // cmToRGB565
            // 
            this.cmToRGB565.Name = "cmToRGB565";
            this.cmToRGB565.Size = new System.Drawing.Size(267, 22);
            this.cmToRGB565.Tag = "rgb565";
            this.cmToRGB565.Text = "To RGB565 (Moderate Quality)";
            this.cmToRGB565.Click += new System.EventHandler(this.cmChangeFormat_Click);
            // 
            // cmToRGB5A3
            // 
            this.cmToRGB5A3.Name = "cmToRGB5A3";
            this.cmToRGB5A3.Size = new System.Drawing.Size(267, 22);
            this.cmToRGB5A3.Tag = "rgb5a3";
            this.cmToRGB5A3.Text = "To RGB5A3 (Low Quality with Alpha)";
            this.cmToRGB5A3.Click += new System.EventHandler(this.cmChangeFormat_Click);
            // 
            // cmToIA8
            // 
            this.cmToIA8.Name = "cmToIA8";
            this.cmToIA8.Size = new System.Drawing.Size(267, 22);
            this.cmToIA8.Tag = "ia8";
            this.cmToIA8.Text = "To IA8 (B/W with Alpha)";
            this.cmToIA8.Click += new System.EventHandler(this.cmChangeFormat_Click);
            // 
            // cmToIA4
            // 
            this.cmToIA4.Name = "cmToIA4";
            this.cmToIA4.Size = new System.Drawing.Size(267, 22);
            this.cmToIA4.Tag = "ia4";
            this.cmToIA4.Text = "To IA4 (B/W with Alpha)";
            this.cmToIA4.Click += new System.EventHandler(this.cmChangeFormat_Click);
            // 
            // cmToI8
            // 
            this.cmToI8.Name = "cmToI8";
            this.cmToI8.Size = new System.Drawing.Size(267, 22);
            this.cmToI8.Tag = "i8";
            this.cmToI8.Text = "To I8 (B/W)";
            this.cmToI8.Click += new System.EventHandler(this.cmChangeFormat_Click);
            // 
            // cmToI4
            // 
            this.cmToI4.Name = "cmToI4";
            this.cmToI4.Size = new System.Drawing.Size(267, 22);
            this.cmToI4.Tag = "i4";
            this.cmToI4.Text = "To I4 (B/W)";
            this.cmToI4.Click += new System.EventHandler(this.cmChangeFormat_Click);
            // 
            // lbNoPreview
            // 
            this.lbNoPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbNoPreview.Location = new System.Drawing.Point(0, 22);
            this.lbNoPreview.Name = "lbNoPreview";
            this.lbNoPreview.Size = new System.Drawing.Size(817, 412);
            this.lbNoPreview.TabIndex = 4;
            this.lbNoPreview.Text = "No Preview";
            this.lbNoPreview.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbNoPreview.Visible = false;
            // 
            // cmCI8
            // 
            this.cmCI8.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmCI8RGB5A3,
            this.cmCI8RGB565,
            this.cmCI8IA8});
            this.cmCI8.Name = "cmCI8";
            this.cmCI8.Size = new System.Drawing.Size(266, 22);
            this.cmCI8.Text = "As CI8 (Indexed, max. 256 Colors)";
            // 
            // cmCI4
            // 
            this.cmCI4.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmCI4RGB5A3,
            this.cmCI4RGB565,
            this.cmCI4IA8});
            this.cmCI4.Name = "cmCI4";
            this.cmCI4.Size = new System.Drawing.Size(266, 22);
            this.cmCI4.Text = "As CI4 (Indexed, max. 16 Colors)";
            // 
            // cmCI8RGB5A3
            // 
            this.cmCI8RGB5A3.Name = "cmCI8RGB5A3";
            this.cmCI8RGB5A3.Size = new System.Drawing.Size(283, 22);
            this.cmCI8RGB5A3.Tag = "ci8rgb5a3";
            this.cmCI8RGB5A3.Text = "With RGB5A3 Palette (Color with Alpha)";
            this.cmCI8RGB5A3.Click += new System.EventHandler(this.cmFormat_Click);
            // 
            // cmCI8RGB565
            // 
            this.cmCI8RGB565.Name = "cmCI8RGB565";
            this.cmCI8RGB565.Size = new System.Drawing.Size(283, 22);
            this.cmCI8RGB565.Tag = "ci8rgb565";
            this.cmCI8RGB565.Text = "With RGB565 Palette (Color)";
            this.cmCI8RGB565.Click += new System.EventHandler(this.cmFormat_Click);
            // 
            // cmCI8IA8
            // 
            this.cmCI8IA8.Name = "cmCI8IA8";
            this.cmCI8IA8.Size = new System.Drawing.Size(283, 22);
            this.cmCI8IA8.Tag = "ci8ia8";
            this.cmCI8IA8.Text = "With IA8 Palette (B/W with Alpha)";
            this.cmCI8IA8.Click += new System.EventHandler(this.cmFormat_Click);
            // 
            // cmCI4RGB5A3
            // 
            this.cmCI4RGB5A3.Name = "cmCI4RGB5A3";
            this.cmCI4RGB5A3.Size = new System.Drawing.Size(283, 22);
            this.cmCI4RGB5A3.Tag = "ci4rgb5a3";
            this.cmCI4RGB5A3.Text = "With RGB5A3 Palette (Color with Alpha)";
            this.cmCI4RGB5A3.Click += new System.EventHandler(this.cmFormat_Click);
            // 
            // cmCI4RGB565
            // 
            this.cmCI4RGB565.Name = "cmCI4RGB565";
            this.cmCI4RGB565.Size = new System.Drawing.Size(283, 22);
            this.cmCI4RGB565.Tag = "ci4rgb565";
            this.cmCI4RGB565.Text = "With RGB565 Palette (Color)";
            this.cmCI4RGB565.Click += new System.EventHandler(this.cmFormat_Click);
            // 
            // cmCI4IA8
            // 
            this.cmCI4IA8.Name = "cmCI4IA8";
            this.cmCI4IA8.Size = new System.Drawing.Size(283, 22);
            this.cmCI4IA8.Tag = "ci4ia8";
            this.cmCI4IA8.Text = "With IA8 Palette (B/W with Alpha)";
            this.cmCI4IA8.Click += new System.EventHandler(this.cmFormat_Click);
            // 
            // lbTip
            // 
            this.lbTip.AutoSize = true;
            this.lbTip.ForeColor = System.Drawing.Color.Red;
            this.lbTip.Location = new System.Drawing.Point(149, 6);
            this.lbTip.Name = "lbTip";
            this.lbTip.Size = new System.Drawing.Size(470, 13);
            this.lbTip.TabIndex = 4;
            this.lbTip.Text = "Tip: Reduce the colors of an image with Photoshop or GIMP to gain better results " +
                "with CI formats...";
            this.lbTip.Visible = false;
            // 
            // cmToCI8
            // 
            this.cmToCI8.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmToCI8RGB5A3,
            this.cmToCI8RGB565,
            this.cmToCI8IA8});
            this.cmToCI8.Name = "cmToCI8";
            this.cmToCI8.Size = new System.Drawing.Size(267, 22);
            this.cmToCI8.Text = "To CI8 (Indexed, max. 256 Colors)";
            // 
            // cmToCI4
            // 
            this.cmToCI4.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmToCI4RGB5A3,
            this.cmToCI4RGB565,
            this.cmToCI4IA8});
            this.cmToCI4.Name = "cmToCI4";
            this.cmToCI4.Size = new System.Drawing.Size(267, 22);
            this.cmToCI4.Text = "To CI4 (Indexed, max. 16 Colors)";
            // 
            // cmToCI8RGB5A3
            // 
            this.cmToCI8RGB5A3.Name = "cmToCI8RGB5A3";
            this.cmToCI8RGB5A3.Size = new System.Drawing.Size(283, 22);
            this.cmToCI8RGB5A3.Tag = "ci8rgb5a3";
            this.cmToCI8RGB5A3.Text = "With RGB5A3 Palette (Color with Alpha)";
            this.cmToCI8RGB5A3.Click += new System.EventHandler(this.cmChangeFormat_Click);
            // 
            // cmToCI8RGB565
            // 
            this.cmToCI8RGB565.Name = "cmToCI8RGB565";
            this.cmToCI8RGB565.Size = new System.Drawing.Size(283, 22);
            this.cmToCI8RGB565.Tag = "ci8rgb565";
            this.cmToCI8RGB565.Text = "With RGB565 Palette (Color)";
            this.cmToCI8RGB565.Click += new System.EventHandler(this.cmChangeFormat_Click);
            // 
            // cmToCI8IA8
            // 
            this.cmToCI8IA8.Name = "cmToCI8IA8";
            this.cmToCI8IA8.Size = new System.Drawing.Size(283, 22);
            this.cmToCI8IA8.Tag = "ci8ia8";
            this.cmToCI8IA8.Text = "With IA8 Palette (B/W with Alpha)";
            this.cmToCI8IA8.Click += new System.EventHandler(this.cmChangeFormat_Click);
            // 
            // cmToCI4RGB5A3
            // 
            this.cmToCI4RGB5A3.Name = "cmToCI4RGB5A3";
            this.cmToCI4RGB5A3.Size = new System.Drawing.Size(283, 22);
            this.cmToCI4RGB5A3.Tag = "ci4rgb5a3";
            this.cmToCI4RGB5A3.Text = "With RGB5A3 Palette (Color with Alpha)";
            this.cmToCI4RGB5A3.Click += new System.EventHandler(this.cmChangeFormat_Click);
            // 
            // cmToCI4RGB565
            // 
            this.cmToCI4RGB565.Name = "cmToCI4RGB565";
            this.cmToCI4RGB565.Size = new System.Drawing.Size(283, 22);
            this.cmToCI4RGB565.Tag = "ci4rgb565";
            this.cmToCI4RGB565.Text = "With RGB565 Palette (Color)";
            this.cmToCI4RGB565.Click += new System.EventHandler(this.cmChangeFormat_Click);
            // 
            // cmToCI4IA8
            // 
            this.cmToCI4IA8.Name = "cmToCI4IA8";
            this.cmToCI4IA8.Size = new System.Drawing.Size(283, 22);
            this.cmToCI4IA8.Tag = "ci4ia8";
            this.cmToCI4IA8.Text = "With IA8 Palette (B/W with Alpha)";
            this.cmToCI4IA8.Click += new System.EventHandler(this.cmChangeFormat_Click);
            // 
            // CustomizeMii_Preview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(817, 462);
            this.Controls.Add(this.lbNoPreview);
            this.Controls.Add(this.pbPic);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.Panel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(833, 500);
            this.Name = "CustomizeMii_Preview";
            this.Text = "Preview";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Preview_FormClosing);
            this.Load += new System.EventHandler(this.Preview_Load);
            this.Panel.ResumeLayout(false);
            this.Panel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPic)).EndInit();
            this.cmFormat.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Panel;
        private System.Windows.Forms.Label lbIcon;
        private System.Windows.Forms.Label lbBanner;
        public System.Windows.Forms.ComboBox cbBanner;
        public System.Windows.Forms.Button btnClose;
        public System.Windows.Forms.ComboBox cbIcon;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pbPic;
        private System.Windows.Forms.Label lbSize;
        private System.Windows.Forms.Label lbFormat;
        public System.Windows.Forms.Label lbSizeText;
        public System.Windows.Forms.Label lbFormatText;
        public System.Windows.Forms.Button btnReplace;
        private System.Windows.Forms.ContextMenuStrip cmFormat;
        private System.Windows.Forms.ToolStripMenuItem cmRGBA8;
        private System.Windows.Forms.ToolStripMenuItem cmRGB565;
        private System.Windows.Forms.ToolStripMenuItem cmRGB5A3;
        private System.Windows.Forms.ToolStripMenuItem cmIA8;
        private System.Windows.Forms.ToolStripMenuItem cmIA4;
        private System.Windows.Forms.ToolStripMenuItem cmI8;
        private System.Windows.Forms.ToolStripMenuItem cmI4;
        private System.Windows.Forms.Label lbNoPreview;
        private System.Windows.Forms.ToolStripMenuItem cmChangeFormat;
        private System.Windows.Forms.ToolStripMenuItem cmToRGBA8;
        private System.Windows.Forms.ToolStripMenuItem cmToRGB565;
        private System.Windows.Forms.ToolStripMenuItem cmToRGB5A3;
        private System.Windows.Forms.ToolStripMenuItem cmToIA8;
        private System.Windows.Forms.ToolStripMenuItem cmToIA4;
        private System.Windows.Forms.ToolStripMenuItem cmToI8;
        private System.Windows.Forms.ToolStripMenuItem cmToI4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.CheckBox cbCheckerBoard;
        private System.Windows.Forms.ToolStripMenuItem cmCI8;
        private System.Windows.Forms.ToolStripMenuItem cmCI4;
        private System.Windows.Forms.ToolStripMenuItem cmCI8RGB5A3;
        private System.Windows.Forms.ToolStripMenuItem cmCI8RGB565;
        private System.Windows.Forms.ToolStripMenuItem cmCI8IA8;
        private System.Windows.Forms.ToolStripMenuItem cmCI4RGB5A3;
        private System.Windows.Forms.ToolStripMenuItem cmCI4RGB565;
        private System.Windows.Forms.ToolStripMenuItem cmCI4IA8;
        private System.Windows.Forms.Label lbTip;
        private System.Windows.Forms.ToolStripMenuItem cmToCI8;
        private System.Windows.Forms.ToolStripMenuItem cmToCI4;
        private System.Windows.Forms.ToolStripMenuItem cmToCI8RGB5A3;
        private System.Windows.Forms.ToolStripMenuItem cmToCI8RGB565;
        private System.Windows.Forms.ToolStripMenuItem cmToCI8IA8;
        private System.Windows.Forms.ToolStripMenuItem cmToCI4RGB5A3;
        private System.Windows.Forms.ToolStripMenuItem cmToCI4RGB565;
        private System.Windows.Forms.ToolStripMenuItem cmToCI4IA8;
    }
}