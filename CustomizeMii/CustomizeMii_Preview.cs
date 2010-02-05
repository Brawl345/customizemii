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
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CustomizeMii
{
    public partial class CustomizeMii_Preview : Form
    {
        public string startTPL;
        public bool startIcon = false;
        private TplImage[,] images;

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

            string[] bannerpics;
            string[] iconpics;

            if (string.IsNullOrEmpty(CustomizeMii_Main.BannerReplace))
                bannerpics = Directory.GetFiles(CustomizeMii_Main.TempUnpackBannerTplPath, "*.tpl");
            else bannerpics = Directory.GetFiles(CustomizeMii_Main.BannerTplPath, "*.tpl");

            if (string.IsNullOrEmpty(CustomizeMii_Main.IconReplace))
                iconpics = Directory.GetFiles(CustomizeMii_Main.TempUnpackIconTplPath, "*.tpl");
            else iconpics = Directory.GetFiles(CustomizeMii_Main.IconTplPath, "*.tpl");

            int startIndex = -1;
            if (!startIcon)
            {
                for (int i = 0; i < bannerpics.Length; i++)
                    if (Path.GetFileName(bannerpics[i]) == startTPL)
                        startIndex = i;
            }
            else
            {
                for (int i = 0; i < iconpics.Length; i++)
                    if (Path.GetFileName(iconpics[i]) == startTPL)
                        startIndex = i;
            }

            foreach (string thispic in bannerpics)
            {
                string picname = thispic.Remove(0, thispic.LastIndexOf('\\') + 1);
                picname = picname.Remove(picname.LastIndexOf('.'));
                cbBanner.Items.Add((object)picname);
            }

            foreach (string thispic in iconpics)
            {
                string picname = thispic.Remove(0, thispic.LastIndexOf('\\') + 1);
                picname = picname.Remove(picname.LastIndexOf('.'));
                cbIcon.Items.Add((object)picname);
            }

            if (bannerpics.Length > iconpics.Length)
                images = new TplImage[2, bannerpics.Length];
            else
                images = new TplImage[2, iconpics.Length];

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
        }

        private void cbBanner_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbBanner.SelectedIndex != -1)
            {
                if (images[0, cbBanner.SelectedIndex].tplImage == null) 
                {
                    byte[] tpl;

                    if (string.IsNullOrEmpty(CustomizeMii_Main.BannerReplace))
                        tpl = Wii.Tools.LoadFileToByteArray(CustomizeMii_Main.TempUnpackBannerTplPath + cbBanner.SelectedItem.ToString() + ".tpl");
                    else tpl = Wii.Tools.LoadFileToByteArray(CustomizeMii_Main.BannerTplPath + cbBanner.SelectedItem.ToString() + ".tpl");

                    lbSize.Text = Wii.TPL.GetTextureWidth(tpl).ToString() + " x " + Wii.TPL.GetTextureHeight(tpl).ToString();
                    images[0, cbBanner.SelectedIndex].tplFormat = Wii.TPL.GetTextureFormatName(tpl);
                    lbFormat.Text = images[0, cbBanner.SelectedIndex].tplFormat;

                    if (images[0, cbBanner.SelectedIndex].tplFormat.ToLower() == "ci4" ||
                        images[0, cbBanner.SelectedIndex].tplFormat.ToLower() == "ci8" ||
                        images[0, cbBanner.SelectedIndex].tplFormat.ToLower() == "ci14x2")
                    {
                        int tempW = Wii.TPL.GetTextureWidth(tpl);
                        int tempH = Wii.TPL.GetTextureHeight(tpl);

                        images[0, cbBanner.SelectedIndex].tplImage = new Bitmap(tempW, tempH);
                    }
                    else
                    {
                        images[0, cbBanner.SelectedIndex].tplImage = Wii.TPL.ConvertFromTPL(tpl);
                    }

                    tpl = null;
                }

                pbPic.Image = images[0, cbBanner.SelectedIndex].tplImage;
                lbFormat.Text = images[0, cbBanner.SelectedIndex].tplFormat;
                lbSize.Text = string.Format("{0} x {1}", images[0, cbBanner.SelectedIndex].tplImage.Width, images[0, cbBanner.SelectedIndex].tplImage.Height);

                if (images[0, cbBanner.SelectedIndex].tplFormat.ToLower() == "ci4" ||
                    images[0, cbBanner.SelectedIndex].tplFormat.ToLower() == "ci8" ||
                    images[0, cbBanner.SelectedIndex].tplFormat.ToLower() == "ci14x2")
                    lbNoPreview.Visible = true;
                else lbNoPreview.Visible = false;
            }
        }

        private void cbIcon_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbIcon.SelectedIndex != -1)
            {
                if (images[1, cbIcon.SelectedIndex].tplImage == null)
                {
                    byte[] tpl;

                    if (string.IsNullOrEmpty(CustomizeMii_Main.IconReplace))
                        tpl = Wii.Tools.LoadFileToByteArray(CustomizeMii_Main.TempUnpackIconTplPath + cbIcon.SelectedItem.ToString() + ".tpl");
                    else tpl = Wii.Tools.LoadFileToByteArray(CustomizeMii_Main.IconTplPath + cbIcon.SelectedItem.ToString() + ".tpl");

                    lbSize.Text = Wii.TPL.GetTextureWidth(tpl).ToString() + " x " + Wii.TPL.GetTextureHeight(tpl).ToString();
                    images[1, cbIcon.SelectedIndex].tplFormat = Wii.TPL.GetTextureFormatName(tpl);
                    lbFormat.Text = images[1, cbIcon.SelectedIndex].tplFormat;

                    if (images[1, cbIcon.SelectedIndex].tplFormat.ToLower() == "ci4" ||
                        images[1, cbIcon.SelectedIndex].tplFormat.ToLower() == "ci8" ||
                        images[1, cbIcon.SelectedIndex].tplFormat.ToLower() == "ci14x2")
                    {
                        int tempW = Wii.TPL.GetTextureWidth(tpl);
                        int tempH = Wii.TPL.GetTextureHeight(tpl);

                        images[1, cbIcon.SelectedIndex].tplImage = new Bitmap(tempW, tempH);
                    }
                    else
                    {
                        images[1, cbIcon.SelectedIndex].tplImage = Wii.TPL.ConvertFromTPL(tpl);
                    }

                    tpl = null;
                }

                pbPic.Image = images[1, cbIcon.SelectedIndex].tplImage;
                lbFormat.Text = images[1, cbIcon.SelectedIndex].tplFormat;
                lbSize.Text = string.Format("{0} x {1}", images[1, cbIcon.SelectedIndex].tplImage.Width, images[1, cbIcon.SelectedIndex].tplImage.Height);

                if (images[1, cbIcon.SelectedIndex].tplFormat.ToLower() == "ci4" ||
                    images[1, cbIcon.SelectedIndex].tplFormat.ToLower() == "ci8" ||
                    images[1, cbIcon.SelectedIndex].tplFormat.ToLower() == "ci14x2")
                    lbNoPreview.Visible = true;
                else lbNoPreview.Visible = false;
            }
        }

        private void cmSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = pbPic.ImageLocation.Remove(0, pbPic.ImageLocation.LastIndexOf('\\') + 1);
            sfd.Filter = "PNG|*.png";
            if (sfd.ShowDialog() == DialogResult.OK)
                File.Copy(pbPic.ImageLocation, sfd.FileName);
        }

        private Image ResizeImage(Image img, int x, int y)
        {
            Image newimage = new Bitmap(x, y);
            using (Graphics gfx = Graphics.FromImage(newimage))
            {
                gfx.DrawImage(img, 0, 0, x, y);
            }
            return newimage;
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
                    string Tpl;

                    if (cbBanner.SelectedIndex != -1) { Tpl = CustomizeMii_Main.BannerTplPath + cbBanner.SelectedItem + ".tpl"; }
                    else { Tpl = CustomizeMii_Main.IconTplPath + cbIcon.SelectedItem + ".tpl"; }

                    byte[] TplArray = Wii.Tools.LoadFileToByteArray(Tpl);
                    Image Img;

                    if (!ofd.FileName.ToLower().EndsWith(".tpl")) Img = Image.FromFile(ofd.FileName);
                    else Img = Wii.TPL.ConvertFromTPL(ofd.FileName);

                    int TplFormat;
                    int X = Wii.TPL.GetTextureWidth(TplArray);
                    int Y = Wii.TPL.GetTextureHeight(TplArray);

                    if (X != Img.Width ||
                        Y != Img.Height)
                    {
                        Img = ResizeImage(Img, X, Y);
                    }

                    ToolStripMenuItem cmSender = sender as ToolStripMenuItem;
                    switch (cmSender.Tag.ToString().ToLower())
                    {
                        case "i4":
                            TplFormat = 0;
                            lbFormat.Text = "I4";
                            break;
                        case "i8":
                            TplFormat = 1;
                            lbFormat.Text = "I8";
                            break;
                        case "ia4":
                            TplFormat = 2;
                            lbFormat.Text = "IA4";
                            break;
                        case "ia8":
                            TplFormat = 3;
                            lbFormat.Text = "IA8";
                            break;
                        case "rgb565":
                            TplFormat = 4;
                            lbFormat.Text = "RGB565";
                            break;
                        case "rgb5a3":
                            TplFormat = 5;
                            lbFormat.Text = "RGB5A3";
                            break;
                        default:
                            TplFormat = 6;
                            lbFormat.Text = "RGBA8";
                            break;
                    }

                    Wii.TPL.ConvertToTPL(Img, Tpl, TplFormat);

                    if (cbBanner.SelectedIndex != -1)
                    {
                        images[0, cbBanner.SelectedIndex].tplImage = Wii.TPL.ConvertFromTPL(Tpl);
                        images[0, cbBanner.SelectedIndex].tplFormat = Wii.TPL.GetTextureFormatName(File.ReadAllBytes(Tpl));

                        pbPic.Image = images[0, cbBanner.SelectedIndex].tplImage;
                        lbFormat.Text = images[0, cbBanner.SelectedIndex].tplFormat;
                        lbSize.Text = string.Format("{0} x {1}", images[0, cbBanner.SelectedIndex].tplImage.Width, images[0, cbBanner.SelectedIndex].tplImage.Height);
                    }
                    else
                    {
                        images[1, cbIcon.SelectedIndex].tplImage = Wii.TPL.ConvertFromTPL(Tpl);
                        images[1, cbIcon.SelectedIndex].tplFormat = Wii.TPL.GetTextureFormatName(File.ReadAllBytes(Tpl));

                        pbPic.Image = images[1, cbIcon.SelectedIndex].tplImage;
                        lbFormat.Text = images[1, cbIcon.SelectedIndex].tplFormat;
                        lbSize.Text = string.Format("{0} x {1}", images[1, cbIcon.SelectedIndex].tplImage.Width, images[1, cbIcon.SelectedIndex].tplImage.Height);
                    }

                    if (cbBanner.SelectedIndex != -1) cbBanner.Select();
                    else if (cbIcon.SelectedIndex != -1) cbIcon.Select();
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }
    }
}
