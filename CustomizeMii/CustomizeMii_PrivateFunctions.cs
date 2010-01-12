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
using System.Windows.Forms;
using System.ComponentModel;
using System.IO;
using System.Drawing;

namespace CustomizeMii
{
    partial class CustomizeMii_Main
    {
        private void FixTpls()
        {
            string[] bannerTpls = Directory.GetFiles(GetCurBannerPath() + "timg\\", "*.tpl");
            string[] iconTpls = Directory.GetFiles(GetCurIconPath() + "timg\\", "*.tpl");

            foreach (string thisTpl in bannerTpls)
                Wii.TPL.FixFilter(thisTpl);
            
            foreach (string thisTpl in iconTpls)
                Wii.TPL.FixFilter(thisTpl);
        }

        private void EnableControls(object sender, EventArgs e)
        {
            for (int i = 0; i < tabControl.TabCount; i++)
            {
                if (tabControl.TabPages[i] != tabSource)
                {
                    foreach (Control Ctrl in tabControl.TabPages[i].Controls)
                    {
                        if (Ctrl is Button) Ctrl.Enabled = true;
                        else if ((Ctrl is TextBox) && (Ctrl.Tag != (object)"Disabled")) Ctrl.Enabled = true;
                        else if (Ctrl is CheckBox && Ctrl.Tag != (object)"Independent") Ctrl.Enabled = true;
                        else if (Ctrl is ComboBox) Ctrl.Enabled = true;
                        else if (Ctrl is LinkLabel && Ctrl.Tag != (object)"Independent") Ctrl.Enabled = true;
                    }
                }
            }
        }

        private void DisableControls(object sender, EventArgs e)
        {
            for (int i = 0; i < tabControl.TabCount; i++)
            {
                if (tabControl.TabPages[i] != tabSource)
                {
                    foreach (Control Ctrl in tabControl.TabPages[i].Controls)
                    {
                        if (Ctrl is Button) Ctrl.Enabled = false;
                        else if ((Ctrl is TextBox) && (Ctrl.Tag != (object)"Disabled")) Ctrl.Enabled = false;
                        else if (Ctrl is CheckBox && Ctrl.Tag != (object)"Independent") Ctrl.Enabled = false;
                        else if (Ctrl is ComboBox) Ctrl.Enabled = false;
                        else if (Ctrl is LinkLabel && Ctrl.Tag != (object)"Independent") Ctrl.Enabled = false;
                    }
                }
            }
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

        private string GetCurBannerPath()
        {
            if (string.IsNullOrEmpty(BannerReplace))
                return TempUnpackPath + "00000000.app_OUT\\meta\\banner.bin_OUT\\arc\\";
            else
                return TempBannerPath + "arc\\";
        }

        private string GetCurIconPath()
        {
            if (string.IsNullOrEmpty(IconReplace))
                return TempUnpackPath + "00000000.app_OUT\\meta\\icon.bin_OUT\\arc\\";
            else
                return TempIconPath + "arc\\";
        }

        private void SetText(TextBox tb, string text)
        {
            SetTextInvoker invoker = new SetTextInvoker(this.SetText);
            this.Invoke(invoker, text, tb);
        }

        private void SetText(string text, TextBox tb)
        {
            tb.Text = text;
        }

        private void SetLabel(Label lb, string text)
        {
            SetLabelInvoker invoker = new SetLabelInvoker(this.SetLabel);
            this.Invoke(invoker, text, lb);
        }

        private void SetLabel(string text, Label lb)
        {
            lb.Text = text;
        }

        private void AddBannerTpls(object sender, EventArgs e)
        {
            try
            {
                string[] BannerTpls;
                if (string.IsNullOrEmpty(BannerReplace))
                    BannerTpls = Directory.GetFiles(TempUnpackBannerTplPath);
                else
                    BannerTpls = Directory.GetFiles(TempBannerPath + "arc\\timg");

                AddBannerTpls(BannerTpls);
            }
            catch { }
        }

        private void AddIconTpls(object sender, EventArgs e)
        {
            try
            {
                string[] IconTpls;
                if (string.IsNullOrEmpty(IconReplace))
                    IconTpls = Directory.GetFiles(TempUnpackIconTplPath);
                else
                    IconTpls = Directory.GetFiles(TempIconPath + "arc\\timg");

                AddIconTpls(IconTpls);
            }
            catch { }
        }

        private void AddBannerTpls(string[] tpls)
        {
            if (tpls.Length > 0)
            {
                lbxBannerTpls.Items.Clear();
                BannerTplPath = tpls[0].Remove(tpls[0].LastIndexOf('\\') + 1);

                for (int i = 0; i < tpls.Length; i++)
                {
                    if (BannerTransparents.Contains(tpls[i].Remove(0, tpls[i].LastIndexOf('\\') + 1)))
                        lbxBannerTpls.Items.Add(tpls[i].Remove(0, tpls[i].LastIndexOf('\\') + 1) + " (Transparent)");
                    else
                        lbxBannerTpls.Items.Add(tpls[i].Remove(0, tpls[i].LastIndexOf('\\') + 1));
                }
            }
        }

        private void AddIconTpls(string[] tpls)
        {
            if (tpls.Length > 0)
            {
                lbxIconTpls.Items.Clear();
                IconTplPath = tpls[0].Remove(tpls[0].LastIndexOf('\\') + 1);

                for (int i = 0; i < tpls.Length; i++)
                {
                    if (IconTransparents.Contains(tpls[i].Remove(0, tpls[i].LastIndexOf('\\') + 1)))
                        lbxIconTpls.Items.Add(tpls[i].Remove(0, tpls[i].LastIndexOf('\\') + 1) + " (Transparent)");
                    else
                        lbxIconTpls.Items.Add(tpls[i].Remove(0, tpls[i].LastIndexOf('\\') + 1));
                }
            }
        }

        private void AddBrlyts(object sender, EventArgs e)
        {
            try
            {
                string[] BannerBrlyt;
                if (string.IsNullOrEmpty(BannerReplace))
                    BannerBrlyt = Directory.GetFiles(TempUnpackBannerBrlytPath);
                else
                    BannerBrlyt = Directory.GetFiles(TempBannerPath + "arc\\blyt");

                string[] IconBrlyt;
                if (string.IsNullOrEmpty(IconReplace))
                    IconBrlyt = Directory.GetFiles(TempUnpackIconBrlytPath);
                else
                    IconBrlyt = Directory.GetFiles(TempIconPath + "arc\\blyt");

                AddBannerBrlyt(BannerBrlyt);
                AddIconBrlyt(IconBrlyt);

                if (lbxBrlytBanner.SelectedIndex == -1 && lbxBrlytIcon.SelectedIndex == -1)
                {
                    if (lbxBrlytBanner.Items.Count > 0) lbxBrlytBanner.SelectedIndex = 0;
                    else if (lbxBrlytIcon.Items.Count > 0) lbxBrlytIcon.SelectedIndex = 0;
                }
            }
            catch { }
        }

        private void AddBrlans(object sender, EventArgs e)
        {
            try
            {
                string[] BannerBrlan;
                if (string.IsNullOrEmpty(BannerReplace))
                    BannerBrlan = Directory.GetFiles(TempUnpackBannerBrlanPath);
                else
                    BannerBrlan = Directory.GetFiles(TempBannerPath + "arc\\anim");

                string[] IconBrlan;
                if (string.IsNullOrEmpty(IconReplace))
                    IconBrlan = Directory.GetFiles(TempUnpackIconBrlanPath);
                else
                    IconBrlan = Directory.GetFiles(TempIconPath + "arc\\anim");

                AddBannerBrlan(BannerBrlan);
                AddIconBrlan(IconBrlan);

                if (lbxBrlanBanner.SelectedIndex == -1 && lbxBrlanIcon.SelectedIndex == -1)
                {
                    if (lbxBrlanBanner.Items.Count > 0) lbxBrlanBanner.SelectedIndex = 0;
                    else if (lbxBrlanIcon.Items.Count > 0) lbxBrlanIcon.SelectedIndex = 0;
                }
            }
            catch { }
        }

        private void AddBannerBrlyt(string[] brlyt)
        {
            if (brlyt.Length > 0)
            {
                lbxBrlytBanner.Items.Clear();
                BannerBrlytPath = brlyt[0].Remove(brlyt[0].LastIndexOf('\\') + 1);

                for (int i = 0; i < brlyt.Length; i++)
                {
                    lbxBrlytBanner.Items.Add(brlyt[i].Remove(0, brlyt[i].LastIndexOf('\\') + 1));
                }
            }
        }

        private void AddIconBrlyt(string[] brlyt)
        {
            if (brlyt.Length > 0)
            {
                lbxBrlytIcon.Items.Clear();
                IconBrlytPath = brlyt[0].Remove(brlyt[0].LastIndexOf('\\') + 1);

                for (int i = 0; i < brlyt.Length; i++)
                {
                    lbxBrlytIcon.Items.Add(brlyt[i].Remove(0, brlyt[i].LastIndexOf('\\') + 1));
                }
            }
        }

        private void AddBannerBrlan(string[] brlan)
        {
            if (brlan.Length > 0)
            {
                lbxBrlanBanner.Items.Clear();
                BannerBrlanPath = brlan[0].Remove(brlan[0].LastIndexOf('\\') + 1);

                for (int i = 0; i < brlan.Length; i++)
                {
                    lbxBrlanBanner.Items.Add(brlan[i].Remove(0, brlan[i].LastIndexOf('\\') + 1));
                }
            }
        }

        private void AddIconBrlan(string[] brlan)
        {
            if (brlan.Length > 0)
            {
                lbxBrlanIcon.Items.Clear();
                IconBrlanPath = brlan[0].Remove(brlan[0].LastIndexOf('\\') + 1);

                for (int i = 0; i < brlan.Length; i++)
                {
                    lbxBrlanIcon.Items.Add(brlan[i].Remove(0, brlan[i].LastIndexOf('\\') + 1));
                }
            }
        }

        private void ErrorBox(string message)
        {
            BoxInvoker invoker = new BoxInvoker(this.errorBox);
            this.Invoke(invoker, new object[] { message });
        }

        private void errorBox(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void InfoBox(string message)
        {
            BoxInvoker invoker = new BoxInvoker(this.infoBox);
            this.Invoke(invoker, new object[] { message });
        }

        private void infoBox(string message)
        {
            MessageBox.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MakeBannerTplsTransparent()
        {
            foreach (string thisTpl in lbxBannerTpls.Items)
            {
                if (thisTpl.EndsWith("(Transparent)"))
                {
                    string Tpl = GetCurBannerPath() + "timg\\" + thisTpl.Replace(" (Transparent)", string.Empty);
                    byte[] TplArray = Wii.Tools.LoadFileToByteArray(Tpl);
                    int Width = Wii.TPL.GetTextureWidth(TplArray);
                    int Height = Wii.TPL.GetTextureHeight(TplArray);

                    Image Img = new Bitmap(Width, Height);
                    Wii.TPL.ConvertToTPL(Img, Tpl, 5);
                }
            }
        }

        private void MakeIconTplsTransparent()
        {
            foreach (string thisTpl in lbxIconTpls.Items)
            {
                if (thisTpl.EndsWith("(Transparent)"))
                {
                    string Tpl = GetCurIconPath() + "timg\\" + thisTpl.Replace(" (Transparent)", string.Empty);
                    byte[] TplArray = Wii.Tools.LoadFileToByteArray(Tpl);
                    int Width = Wii.TPL.GetTextureWidth(TplArray);
                    int Height = Wii.TPL.GetTextureHeight(TplArray);

                    Image Img = new Bitmap(Width, Height);
                    Wii.TPL.ConvertToTPL(Img, Tpl, 5);
                }
            }
        }

        private void AddTpl(ListBox lbx)
        {
            AddTpl(lbx, null);
        }

        private void AddTpl(ListBox lbx, string inputFile)
        {
            try
            {
                if (string.IsNullOrEmpty(inputFile))
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Filter = "TPL|*.tpl|PNG|*.png|JPG|*.jpg|GIF|*.gif|BMP|*.bmp|All|*.tpl;*.png;*.jpg;*.gif;*.bmp";
                    ofd.FilterIndex = 6;

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        inputFile = ofd.FileName;
                    }
                }

                if (!string.IsNullOrEmpty(inputFile))
                {
                    for (int i = 0; i < lbx.Items.Count; i++)
                        if (lbx.Items[i].ToString().ToLower() == Path.GetFileNameWithoutExtension(inputFile).ToLower() + ".tpl")
                            throw new Exception("This TPL already exists, use the Replace button");

                    string CurPath;
                    if (lbx == lbxBannerTpls) CurPath = GetCurBannerPath();
                    else CurPath = GetCurIconPath();

                    string[] brlytTpls = Wii.Brlyt.GetBrlytTpls(CurPath + string.Format("blyt\\{0}.brlyt", lbx == lbxBannerTpls ? "banner" : "icon"));
                    string TplName = Path.GetFileNameWithoutExtension(inputFile) + ".tpl";
                    int TplFormat = 6;

                    int switchVal = lbx == lbxBannerTpls ? cmbFormatBanner.SelectedIndex : cmbFormatIcon.SelectedIndex;
                    switch (switchVal)
                    {
                        case 0:
                            TplFormat = 6;
                            break;
                        case 1:
                            TplFormat = 4;
                            break;
                        case 2:
                            TplFormat = 5;
                            break;
                        default:
                            if (!inputFile.ToLower().EndsWith(".tpl"))
                                throw new Exception("This format is not supported, you must choose either RGBA8, RGB565 or RGB5A3!");
                            break;
                    }

                    if (!Wii.Tools.StringExistsInStringArray(TplName, brlytTpls))
                    {
                        if (MessageBox.Show(string.Format("{0} is not required by your {1}.brlyt and thus only wastes memory!\nDo you still want to add it?", TplName, lbx == lbxBannerTpls ? "banner" : "icon"), "TPL not required", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                            return;
                    }

                    if (inputFile.ToLower().EndsWith(".tpl"))
                    {
                        File.Copy(inputFile, CurPath + "timg\\" + TplName, true);
                        lbx.Items.Add(TplName);
                    }
                    else
                    {
                        using (Image img = Image.FromFile(inputFile))
                        {
                            Wii.TPL.ConvertToTPL(img, CurPath + "timg\\" + TplName, TplFormat);
                            lbx.Items.Add(TplName);
                        }
                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }

        private void LoadChannel()
        {
            LoadChannel(null);
        }

        private void LoadChannel(string inputFile)
        {
            if (Mono) CommonKeyCheck();

            if (pbProgress.Value == 100)
            {
                if (string.IsNullOrEmpty(inputFile))
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Filter = "Wii Channels|*.wad";

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        SourceWad = ofd.FileName;
                        BackgroundWorker bwLoadChannel = new BackgroundWorker();
                        bwLoadChannel.DoWork += new DoWorkEventHandler(bwLoadChannel_DoWork);
                        bwLoadChannel.ProgressChanged += new ProgressChangedEventHandler(bwLoadChannel_ProgressChanged);
                        bwLoadChannel.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoadChannel_RunWorkerCompleted);
                        bwLoadChannel.WorkerReportsProgress = true;
                        bwLoadChannel.RunWorkerAsync(ofd.FileName);
                    }
                }
                else
                {
                    SourceWad = inputFile;
                    BackgroundWorker bwLoadChannel = new BackgroundWorker();
                    bwLoadChannel.DoWork += new DoWorkEventHandler(bwLoadChannel_DoWork);
                    bwLoadChannel.ProgressChanged += new ProgressChangedEventHandler(bwLoadChannel_ProgressChanged);
                    bwLoadChannel.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoadChannel_RunWorkerCompleted);
                    bwLoadChannel.WorkerReportsProgress = true;
                    bwLoadChannel.RunWorkerAsync(inputFile);
                }
            }
        }

        private void ReplacePart()
        {
            ReplacePart(null);
        }

        private void ReplacePart(string inputFile)
        {
            if (!string.IsNullOrEmpty(tbSourceWad.Text))
            {
                if (pbProgress.Value == 100)
                {
                    if (cmbReplace.SelectedIndex == 2) //sound
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(inputFile))
                            {
                                OpenFileDialog ofd = new OpenFileDialog();
                                ofd.Filter = "Wii Channels|*.wad|00000000.app|00000000.app|sound.bin|sound.bin|All|*.wad;00000000.app;sound.bin";
                                ofd.FilterIndex = 4;

                                if (ofd.ShowDialog() == DialogResult.OK)
                                {
                                    if (ofd.FileName != tbSourceWad.Text)
                                    {
                                        SoundReplace = ofd.FileName;
                                        SetText(tbReplace, SoundReplace);
                                        BackgroundWorker bwSoundReplace = new BackgroundWorker();
                                        bwSoundReplace.DoWork += new DoWorkEventHandler(bwSoundReplace_DoWork);
                                        bwSoundReplace.ProgressChanged += new ProgressChangedEventHandler(bwSoundReplace_ProgressChanged);
                                        bwSoundReplace.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSoundReplace_RunWorkerCompleted);
                                        bwSoundReplace.WorkerReportsProgress = true;
                                        bwSoundReplace.RunWorkerAsync(ofd.FileName);
                                    }
                                }
                            }
                            else
                            {
                                if (inputFile != tbSourceWad.Text)
                                {
                                    SoundReplace = inputFile;
                                    SetText(tbReplace, SoundReplace);
                                    BackgroundWorker bwSoundReplace = new BackgroundWorker();
                                    bwSoundReplace.DoWork += new DoWorkEventHandler(bwSoundReplace_DoWork);
                                    bwSoundReplace.ProgressChanged += new ProgressChangedEventHandler(bwSoundReplace_ProgressChanged);
                                    bwSoundReplace.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSoundReplace_RunWorkerCompleted);
                                    bwSoundReplace.WorkerReportsProgress = true;
                                    bwSoundReplace.RunWorkerAsync(inputFile);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            SoundReplace = string.Empty;
                            SetText(tbReplace, SoundReplace);
                            ErrorBox(ex.Message);
                        }
                    }
                    else if (cmbReplace.SelectedIndex == 1) //icon
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(inputFile))
                            {
                                OpenFileDialog ofd = new OpenFileDialog();
                                ofd.Filter = "Wii Channels|*.wad|00000000.app|00000000.app|icon.bin|icon.bin|All|*.wad;00000000.app;icon.bin";
                                ofd.FilterIndex = 4;

                                if (ofd.ShowDialog() == DialogResult.OK)
                                {
                                    if (ofd.FileName != tbSourceWad.Text)
                                    {
                                        IconReplace = ofd.FileName;
                                        SetText(tbReplace, IconReplace);
                                        BackgroundWorker bwIconReplace = new BackgroundWorker();
                                        bwIconReplace.DoWork += new DoWorkEventHandler(bwIconReplace_DoWork);
                                        bwIconReplace.ProgressChanged += new ProgressChangedEventHandler(bwIconReplace_ProgressChanged);
                                        bwIconReplace.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwIconReplace_RunWorkerCompleted);
                                        bwIconReplace.WorkerReportsProgress = true;
                                        bwIconReplace.RunWorkerAsync(ofd.FileName);
                                    }
                                }
                            }
                            else
                            {
                                if (inputFile != tbSourceWad.Text)
                                {
                                    IconReplace = inputFile;
                                    SetText(tbReplace, IconReplace);
                                    BackgroundWorker bwIconReplace = new BackgroundWorker();
                                    bwIconReplace.DoWork += new DoWorkEventHandler(bwIconReplace_DoWork);
                                    bwIconReplace.ProgressChanged += new ProgressChangedEventHandler(bwIconReplace_ProgressChanged);
                                    bwIconReplace.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwIconReplace_RunWorkerCompleted);
                                    bwIconReplace.WorkerReportsProgress = true;
                                    bwIconReplace.RunWorkerAsync(inputFile);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            IconReplace = string.Empty;
                            SetText(tbReplace, IconReplace);
                            ErrorBox(ex.Message);
                        }
                    }
                    else //banner
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(inputFile))
                            {
                                OpenFileDialog ofd = new OpenFileDialog();
                                ofd.Filter = "Wii Channels|*.wad|00000000.app|00000000.app|banner.bin|banner.bin|All|*.wad;00000000.app;banner.bin";
                                ofd.FilterIndex = 4;

                                if (ofd.ShowDialog() == DialogResult.OK)
                                {
                                    if (ofd.FileName != tbSourceWad.Text)
                                    {
                                        BannerReplace = ofd.FileName;
                                        SetText(tbReplace, BannerReplace);
                                        BackgroundWorker bwBannerReplace = new BackgroundWorker();
                                        bwBannerReplace.DoWork += new DoWorkEventHandler(bwBannerReplace_DoWork);
                                        bwBannerReplace.ProgressChanged += new ProgressChangedEventHandler(bwBannerReplace_ProgressChanged);
                                        bwBannerReplace.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwBannerReplace_RunWorkerCompleted);
                                        bwBannerReplace.WorkerReportsProgress = true;
                                        bwBannerReplace.RunWorkerAsync(ofd.FileName);
                                    }
                                }
                            }
                            else
                            {
                                if (inputFile != tbSourceWad.Text)
                                {
                                    BannerReplace = inputFile;
                                    SetText(tbReplace, BannerReplace);
                                    BackgroundWorker bwBannerReplace = new BackgroundWorker();
                                    bwBannerReplace.DoWork += new DoWorkEventHandler(bwBannerReplace_DoWork);
                                    bwBannerReplace.ProgressChanged += new ProgressChangedEventHandler(bwBannerReplace_ProgressChanged);
                                    bwBannerReplace.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwBannerReplace_RunWorkerCompleted);
                                    bwBannerReplace.WorkerReportsProgress = true;
                                    bwBannerReplace.RunWorkerAsync(inputFile);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            BannerReplace = string.Empty;
                            SetText(tbReplace, BannerReplace);
                            ErrorBox(ex.Message);
                        }
                    }
                }
            }
        }
    }
}
