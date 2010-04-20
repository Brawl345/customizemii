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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using libWiiSharp;

namespace CustomizeMii
{
    public partial class CustomizeMii_Preview : Form
    {
        private TplImage[][] images = new TplImage[2][];
        private U8 bannerBin;
        private U8 iconBin;
        private string startTPL;
        private bool startIcon = false;
        Timer tipTimer = new Timer();

        public U8 BannerBin { get { return bannerBin; } set { bannerBin = value; } }
        public U8 IconBin { get { return iconBin; } set { iconBin = value; } }
        public string StartTPL { get { return startTPL; } set { startTPL = value; } }
        public bool StartIcon { get { return startIcon; } set { startIcon = value; } }

        public CustomizeMii_Preview()
        {
            InitializeComponent();
            this.Icon = global::CustomizeMii.Properties.Resources.CustomizeMii;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Preview_FormClosing(object sender, FormClosingEventArgs e)
        {
            images = null;
            cbBanner.Items.Clear();
            cbIcon.Items.Clear();
        }

        private void Preview_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
            cbBanner.Items.Clear();
            cbIcon.Items.Clear();

            int startIndex = -1;

            List<TplImage> bannerImages = new List<TplImage>();
            List<TplImage> iconImages = new List<TplImage>();

            for (int i = 0; i < bannerBin.NumOfNodes; i++)
            {
                if (bannerBin.StringTable[i].ToLower().EndsWith(".tpl"))
                {
                    TplImage tmpImage = new TplImage();
                    TPL tmpTpl = TPL.Load(bannerBin.Data[i]);

                    if (i == 10) { }

                    tmpImage.fileName = bannerBin.StringTable[i];
                    tmpImage.tplFormat = tmpTpl.GetTextureFormat(0).ToString();
                    tmpImage.tplImage = tmpTpl.ExtractTexture();
                    tmpImage.checkerBoard = createCheckerBoard(tmpImage.tplImage.Width, tmpImage.tplImage.Height);

                    if (tmpImage.tplFormat.StartsWith("CI"))
                        tmpImage.tplFormat += " + " + tmpTpl.GetPaletteFormat(0);

                    bannerImages.Add(tmpImage);
                }
            }

            for (int i = 0; i < iconBin.NumOfNodes; i++)
            {
                if (iconBin.StringTable[i].ToLower().EndsWith(".tpl"))
                {
                    TplImage tmpImage = new TplImage();
                    TPL tmpTpl = TPL.Load(iconBin.Data[i]);

                    tmpImage.fileName = iconBin.StringTable[i];
                    tmpImage.tplFormat = tmpTpl.GetTextureFormat(0).ToString();
                    tmpImage.tplImage = tmpTpl.ExtractTexture();
                    tmpImage.checkerBoard = createCheckerBoard(tmpImage.tplImage.Width, tmpImage.tplImage.Height);

                    if (tmpImage.tplFormat.StartsWith("CI"))
                        tmpImage.tplFormat += " + " + tmpTpl.GetPaletteFormat(0);

                    iconImages.Add(tmpImage);
                }
            }

            images[0] = bannerImages.ToArray();
            images[1] = iconImages.ToArray();

            for (int i = 0; i < images[0].Length; i++)
            {
                cbBanner.Items.Add(images[0][i].fileName);
                if (!startIcon)
                    if (images[0][i].fileName.ToLower() == startTPL.ToLower())
                        startIndex = i;
            }

            for (int i = 0; i < images[1].Length; i++)
            {
                cbIcon.Items.Add(images[1][i].fileName);
                if (startIcon)
                    if (images[1][i].fileName.ToLower() == startTPL.ToLower())
                        startIndex = i;
            }

            try
            {
                if (startIndex != -1)
                    if (!startIcon)
                        cbBanner.SelectedIndex = startIndex;
                    else
                        cbIcon.SelectedIndex = startIndex;
            }
            catch { }

            if (cbBanner.SelectedIndex != -1) cbBanner.Select();
            else if (cbIcon.SelectedIndex != -1) cbIcon.Select();

            tipTimer.Interval = 7000;
            tipTimer.Tag = 0;
            tipTimer.Tick += new EventHandler(tipTimer_Tick);
        }

        void tipTimer_Tick(object sender, EventArgs e)
        {
            lbTip.Visible = false;
            tipTimer.Stop();
        }

        private void cbBanner_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbBanner.SelectedIndex != -1)
            {
                pbPic.Image = images[0][cbBanner.SelectedIndex].tplImage;
                lbFormat.Text = images[0][cbBanner.SelectedIndex].tplFormat;
                lbSize.Text = string.Format("{0} x {1}", images[0][cbBanner.SelectedIndex].tplImage.Width, images[0][cbBanner.SelectedIndex].tplImage.Height);

                if (cbCheckerBoard.Checked) pbPic.BackgroundImage = images[0][cbBanner.SelectedIndex].checkerBoard;

                cbIcon.SelectedIndex = -1;
            }
        }

        private void cbIcon_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbIcon.SelectedIndex != -1)
            {
                pbPic.Image = images[1][cbIcon.SelectedIndex].tplImage;
                lbFormat.Text = images[1][cbIcon.SelectedIndex].tplFormat;
                lbSize.Text = string.Format("{0} x {1}", images[1][cbIcon.SelectedIndex].tplImage.Width, images[1][cbIcon.SelectedIndex].tplImage.Height);

                if (cbCheckerBoard.Checked) pbPic.BackgroundImage = images[1][cbIcon.SelectedIndex].checkerBoard;

                cbBanner.SelectedIndex = -1;
            }
        }

        private Image resizeImage(Image img, int x, int y)
        {
            Image newimage = new Bitmap(x, y);

            using (Graphics gfx = Graphics.FromImage(newimage))
                gfx.DrawImage(img, 0, 0, x, y);

            return newimage;
        }

        private Image createCheckerBoard(int w, int h)
        {
            Color darkColor = Color.DarkGray;
            Color lightColor = Color.White;
            int tileSize = 10;

            Bitmap img = new Bitmap(w, h);

            using (Graphics g = Graphics.FromImage(img))
            {
                g.Clear(lightColor);

                for (int col = 0; col < w; col += tileSize)
                {
                    for (int row = 0; row < h; row += tileSize)
                    {
                        Color curColor;

                        if ((col / tileSize) % 2 == 0)
                            curColor = (row / tileSize) % 2 == 0 ? darkColor : lightColor;
                        else
                            curColor = (row / tileSize) % 2 == 0 ? lightColor : darkColor;


                        if (curColor == lightColor) continue;

                        Rectangle rect = new Rectangle(col, row, tileSize, tileSize);
                        g.FillRectangle(new SolidBrush(darkColor), rect);
                    }
                }
            }

            return (Image)img;
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            cmFormat.Show(MousePosition);
        }

        private void cmFormat_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PNG|*.png|JPG|*.jpg|GIF|*.gif|BMP|*.bmp|TPL|*.tpl|All|*.png;*.jpg;*.gif;*.bmp;*.tpl";
            ofd.FilterIndex = 6;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string tplName;
                    TPL tmpTpl = new TPL();
                    Image newImg;

                    if (cbIcon.SelectedIndex == -1)
                    {
                        tplName = cbBanner.SelectedItem.ToString().ToLower();
                        tmpTpl.LoadFile(bannerBin.Data[bannerBin.GetNodeIndex(tplName)]);
                    }
                    else
                    {
                        tplName = cbIcon.SelectedItem.ToString().ToLower();
                        tmpTpl.LoadFile(iconBin.Data[iconBin.GetNodeIndex(tplName)]);
                    }

                    if (!ofd.FileName.ToLower().EndsWith(".tpl")) newImg = Image.FromFile(ofd.FileName);
                    else
                    {
                        TPL newTpl = TPL.Load(ofd.FileName);
                        newImg = newTpl.ExtractTexture();
                    }

                    Size tplSize = tmpTpl.GetTextureSize(0);

                    if (newImg.Width != tplSize.Width ||
                        newImg.Height != tplSize.Height)
                        newImg = resizeImage(newImg, tplSize.Width, tplSize.Height);

                    ToolStripMenuItem cmSender = sender as ToolStripMenuItem;
                    TPL_TextureFormat tplFormat;
                    TPL_PaletteFormat pFormat = TPL_PaletteFormat.RGB5A3;

                    switch (cmSender.Tag.ToString().ToLower())
                    {
                        case "i4":
                            tplFormat = TPL_TextureFormat.I4;
                            break;
                        case "i8":
                            tplFormat = TPL_TextureFormat.I8;
                            break;
                        case "ia4":
                            tplFormat = TPL_TextureFormat.IA4;
                            break;
                        case "ia8":
                            tplFormat = TPL_TextureFormat.IA8;
                            break;
                        case "rgb565":
                            tplFormat = TPL_TextureFormat.RGB565;
                            break;
                        case "rgb5a3":
                            tplFormat = TPL_TextureFormat.RGB5A3;
                            break;
                        case "ci8rgb5a3":
                            tplFormat = TPL_TextureFormat.CI8;
                            pFormat = TPL_PaletteFormat.RGB5A3;
                            break;
                        case "ci8rgb565":
                            tplFormat = TPL_TextureFormat.CI8;
                            pFormat = TPL_PaletteFormat.RGB565;
                            break;
                        case "ci8ia8":
                            tplFormat = TPL_TextureFormat.CI8;
                            pFormat = TPL_PaletteFormat.IA8;
                            break;
                        case "ci4rgb5a3":
                            tplFormat = TPL_TextureFormat.CI4;
                            pFormat = TPL_PaletteFormat.RGB5A3;
                            break;
                        case "ci4rgb565":
                            tplFormat = TPL_TextureFormat.CI4;
                            pFormat = TPL_PaletteFormat.RGB565;
                            break;
                        case "ci4ia8":
                            tplFormat = TPL_TextureFormat.CI4;
                            pFormat = TPL_PaletteFormat.IA8;
                            break;
                        default:
                            tplFormat = TPL_TextureFormat.RGBA8;
                            break;
                    }

                    tmpTpl.RemoveTexture(0);
                    tmpTpl.AddTexture(newImg, tplFormat, pFormat);

                    if (cbBanner.SelectedIndex != -1)
                    {
                        bannerBin.ReplaceFile(bannerBin.GetNodeIndex(tplName), tmpTpl.ToByteArray());
                        images[0][cbBanner.SelectedIndex].tplImage = tmpTpl.ExtractTexture();
                        images[0][cbBanner.SelectedIndex].tplFormat = tmpTpl.GetTextureFormat(0).ToString();

                        if (images[0][cbBanner.SelectedIndex].tplFormat.StartsWith("CI"))
                            images[0][cbBanner.SelectedIndex].tplFormat += " + " + tmpTpl.GetPaletteFormat(0);

                        pbPic.Image = images[0][cbBanner.SelectedIndex].tplImage;
                        lbFormat.Text = images[0][cbBanner.SelectedIndex].tplFormat;
                        lbSize.Text = string.Format("{0} x {1}", images[0][cbBanner.SelectedIndex].tplImage.Width, images[0][cbBanner.SelectedIndex].tplImage.Height);
                    }
                    else
                    {
                        iconBin.ReplaceFile(iconBin.GetNodeIndex(tplName), tmpTpl.ToByteArray());
                        images[1][cbIcon.SelectedIndex].tplImage = tmpTpl.ExtractTexture();
                        images[1][cbIcon.SelectedIndex].tplFormat = tmpTpl.GetTextureFormat(0).ToString();

                        if (images[1][cbIcon.SelectedIndex].tplFormat.StartsWith("CI"))
                            images[1][cbIcon.SelectedIndex].tplFormat += " + " + tmpTpl.GetPaletteFormat(0);

                        pbPic.Image = images[1][cbIcon.SelectedIndex].tplImage;
                        lbFormat.Text = images[1][cbIcon.SelectedIndex].tplFormat;
                        lbSize.Text = string.Format("{0} x {1}", images[1][cbIcon.SelectedIndex].tplImage.Width, images[1][cbIcon.SelectedIndex].tplImage.Height);
                    }

                    if (cbBanner.SelectedIndex != -1) cbBanner.Select();
                    else if (cbIcon.SelectedIndex != -1) cbIcon.Select();

                    if (tplFormat == TPL_TextureFormat.CI4 || tplFormat == TPL_TextureFormat.CI8)
                    {
                        lbTip.Visible = true;
                        tipTimer.Start();
                    }
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private void cmChangeFormat_Click(object sender, EventArgs e)
        {
            try
            {
                string tplName;
                TPL tmpTpl = new TPL();
                Image newImg;

                if (cbIcon.SelectedIndex == -1)
                {
                    tplName = cbBanner.SelectedItem.ToString().ToLower();
                    tmpTpl.LoadFile(bannerBin.Data[bannerBin.GetNodeIndex(tplName)]);
                }
                else
                {
                    tplName = cbIcon.SelectedItem.ToString().ToLower();
                    tmpTpl.LoadFile(iconBin.Data[iconBin.GetNodeIndex(tplName)]);
                }

                newImg = tmpTpl.ExtractTexture();
                TPL_TextureFormat tplFormat;
                TPL_PaletteFormat pFormat = TPL_PaletteFormat.RGB5A3;

                ToolStripMenuItem cmSender = sender as ToolStripMenuItem;
                switch (cmSender.Tag.ToString().ToLower())
                {
                    case "i4":
                        tplFormat = TPL_TextureFormat.I4;
                        break;
                    case "i8":
                        tplFormat = TPL_TextureFormat.I8;
                        break;
                    case "ia4":
                        tplFormat = TPL_TextureFormat.IA4;
                        break;
                    case "ia8":
                        tplFormat = TPL_TextureFormat.IA8;
                        break;
                    case "rgb565":
                        tplFormat = TPL_TextureFormat.RGB565;
                        break;
                    case "rgb5a3":
                        tplFormat = TPL_TextureFormat.RGB5A3;
                        break;
                    case "ci8rgb5a3":
                        tplFormat = TPL_TextureFormat.CI8;
                        pFormat = TPL_PaletteFormat.RGB5A3;
                        break;
                    case "ci8rgb565":
                        tplFormat = TPL_TextureFormat.CI8;
                        pFormat = TPL_PaletteFormat.RGB565;
                        break;
                    case "ci8ia8":
                        tplFormat = TPL_TextureFormat.CI8;
                        pFormat = TPL_PaletteFormat.IA8;
                        break;
                    case "ci4rgb5a3":
                        tplFormat = TPL_TextureFormat.CI4;
                        pFormat = TPL_PaletteFormat.RGB5A3;
                        break;
                    case "ci4rgb565":
                        tplFormat = TPL_TextureFormat.CI4;
                        pFormat = TPL_PaletteFormat.RGB565;
                        break;
                    case "ci4ia8":
                        tplFormat = TPL_TextureFormat.CI4;
                        pFormat = TPL_PaletteFormat.IA8;
                        break;
                    default:
                        tplFormat = TPL_TextureFormat.RGBA8;
                        break;
                }

                if (tmpTpl.GetTextureFormat(0) == tplFormat) return;

                tmpTpl.RemoveTexture(0);
                tmpTpl.AddTexture(newImg, tplFormat, pFormat);

                if (cbBanner.SelectedIndex != -1)
                {
                    bannerBin.ReplaceFile(bannerBin.GetNodeIndex(tplName), tmpTpl.ToByteArray());
                    images[0][cbBanner.SelectedIndex].tplImage = tmpTpl.ExtractTexture();
                    images[0][cbBanner.SelectedIndex].tplFormat = tmpTpl.GetTextureFormat(0).ToString();

                    if (images[0][cbBanner.SelectedIndex].tplFormat.StartsWith("CI"))
                        images[0][cbBanner.SelectedIndex].tplFormat += " + " + tmpTpl.GetPaletteFormat(0);

                    pbPic.Image = images[0][cbBanner.SelectedIndex].tplImage;
                    lbFormat.Text = images[0][cbBanner.SelectedIndex].tplFormat;
                    lbSize.Text = string.Format("{0} x {1}", images[0][cbBanner.SelectedIndex].tplImage.Width, images[0][cbBanner.SelectedIndex].tplImage.Height);
                }
                else
                {
                    iconBin.ReplaceFile(iconBin.GetNodeIndex(tplName), tmpTpl.ToByteArray());
                    images[1][cbIcon.SelectedIndex].tplImage = tmpTpl.ExtractTexture();
                    images[1][cbIcon.SelectedIndex].tplFormat = tmpTpl.GetTextureFormat(0).ToString();

                    if (images[1][cbIcon.SelectedIndex].tplFormat.StartsWith("CI"))
                        images[1][cbIcon.SelectedIndex].tplFormat += " + " + tmpTpl.GetPaletteFormat(0);

                    pbPic.Image = images[1][cbIcon.SelectedIndex].tplImage;
                    lbFormat.Text = images[1][cbIcon.SelectedIndex].tplFormat;
                    lbSize.Text = string.Format("{0} x {1}", images[1][cbIcon.SelectedIndex].tplImage.Width, images[1][cbIcon.SelectedIndex].tplImage.Height);
                }

                if (cbBanner.SelectedIndex != -1) cbBanner.Select();
                else if (cbIcon.SelectedIndex != -1) cbIcon.Select();

                if (tplFormat == TPL_TextureFormat.CI4 || tplFormat == TPL_TextureFormat.CI8)
                {
                    lbTip.Visible = true;
                    tipTimer.Start();
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void cbCheckerBoard_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCheckerBoard.Checked)
            {
                if (cbBanner.SelectedIndex != -1)
                    pbPic.BackgroundImage = images[0][cbBanner.SelectedIndex].checkerBoard;
                else
                    pbPic.BackgroundImage = images[1][cbIcon.SelectedIndex].checkerBoard;
            }
            else pbPic.BackgroundImage = null;

            if (cbBanner.SelectedIndex != -1)
                cbBanner.Focus();
            else
                cbIcon.Focus();
        }
    }
}
