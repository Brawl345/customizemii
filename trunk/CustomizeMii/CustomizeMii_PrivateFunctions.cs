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
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CustomizeMii
{
    partial class CustomizeMii_Main
    {
        private bool FailureCheck()
        {
            try
            {
                currentProgress.progressValue = 100;
                currentProgress.progressState = "Error Checks...";
                this.Invoke(ProgressUpdate);

                //Check Unpack Folder
                if (CheckUnpackFolder() == false)
                {
                    if (UnpackFolderErrorCount > 0)
                    {
                        ErrorBox("Something's wrong with the temporary files.\nYou may have to start again." +
                            "\n\nIf this message keeps bothering you, please report as much information " +
                            "as possible at the issue tracker: http://code.google.com/p/customizemii/issues/list");
                    }
                    else
                    {
                        ErrorBox("Something's wrong with the temporary files.\nYou may have to start again.");
                    }

                    UnpackFolderErrorCount++;
                    return false;
                }

                //Check Channel Title Boxes
                if (!(!string.IsNullOrEmpty(tbAllLanguages.Text) ||
                    (!string.IsNullOrEmpty(tbEnglish.Text) &&
                    !string.IsNullOrEmpty(tbJapanese.Text) &&
                    !string.IsNullOrEmpty(tbGerman.Text) &&
                    !string.IsNullOrEmpty(tbFrench.Text) &&
                    !string.IsNullOrEmpty(tbSpanish.Text) &&
                    !string.IsNullOrEmpty(tbItalian.Text) &&
                    !string.IsNullOrEmpty(tbDutch.Text))))
                {
                    ErrorBox("You must either enter a general Channel Title or one for each language!");
                    return false;
                }

                //Check Title ID Length + Chars
                if (tbTitleID.Text.Length != 4)
                {
                    ErrorBox("The Title ID must be 4 characters long!"); return false;
                }

                Regex allowedchars = new Regex("[A-Z0-9]{4}$");
                if (!allowedchars.IsMatch(tbTitleID.Text.ToUpper()))
                {
                    ErrorBox("Please enter a valid Title ID!"); return false;
                }

                //Check brlan files
                string[] ValidBrlans = new string[] { "banner.brlan", "icon.brlan", "banner_loop.brlan", "icon_loop.brlan", "banner_start.brlan", "icon_start.brlan" };
                foreach (string thisBrlan in lbxBrlanBanner.Items)
                {
                    if (!Wii.Tools.StringExistsInStringArray(thisBrlan.ToLower(), ValidBrlans))
                    {
                        ErrorBox(thisBrlan + " is not a valid brlan filename!");
                        return false;
                    }
                }
                foreach (string thisBrlan in lbxBrlanIcon.Items)
                {
                    if (!Wii.Tools.StringExistsInStringArray(thisBrlan.ToLower(), ValidBrlans))
                    {
                        ErrorBox(thisBrlan + " is not a valid brlan filename!");
                        return false;
                    }
                }

                //Check TPLs
                List<string> BannerTpls = new List<string>();
                List<string> IconTpls = new List<string>();
                foreach (string thisTpl in lbxBannerTpls.Items) BannerTpls.Add(thisTpl.Replace(" (Transparent)", string.Empty));
                foreach (string thisTpl in lbxIconTpls.Items) IconTpls.Add(thisTpl.Replace(" (Transparent)", string.Empty));

                string[] BannerBrlytPath;
                string[] BannerBrlanPath;
                string[] IconBrlytPath;
                string[] IconBrlanPath;

                if (string.IsNullOrEmpty(BannerReplace))
                {
                    BannerBrlytPath = Directory.GetFiles(TempUnpackBannerBrlytPath);
                    BannerBrlanPath = Directory.GetFiles(TempUnpackBannerBrlanPath);
                }
                else
                {
                    BannerBrlytPath = Directory.GetFiles(TempBannerPath + "arc\\blyt");
                    BannerBrlanPath = Directory.GetFiles(TempBannerPath + "arc\\anim");
                }
                if (string.IsNullOrEmpty(IconReplace))
                {
                    IconBrlytPath = Directory.GetFiles(TempUnpackIconBrlytPath);
                    IconBrlanPath = Directory.GetFiles(TempUnpackIconBrlanPath);
                }
                else
                {
                    IconBrlytPath = Directory.GetFiles(TempIconPath + "arc\\blyt");
                    IconBrlanPath = Directory.GetFiles(TempIconPath + "arc\\anim");
                }

                string[] BannerMissing;
                string[] BannerUnused;
                string[] IconMissing;
                string[] IconUnused;

                //Check for missing TPLs
                if (Wii.Brlyt.CheckForMissingTpls(BannerBrlytPath[0], BannerBrlanPath[0], BannerTpls.ToArray(), out BannerMissing) == true)
                {
                    ErrorBox("The following Banner TPLs are required by the banner.brlyt, but missing:\n\n" + string.Join("\n", BannerMissing));
                    return false;
                }
                if (Wii.Brlyt.CheckForMissingTpls(IconBrlytPath[0], IconBrlanPath[0], IconTpls.ToArray(), out IconMissing) == true)
                {
                    ErrorBox("The following Icon TPLs are required by the icon.brlyt, but missing:\n\n" + string.Join("\n", IconMissing));
                    return false;
                }

                //Check Sound length
                int soundLength = 0;
                if (!string.IsNullOrEmpty(tbSound.Text) && string.IsNullOrEmpty(SoundReplace))
                {
                    if (!tbSound.Text.ToLower().EndsWith(".bns") && !tbSound.Text.StartsWith("BNS:"))
                    {
                        string SoundFile = tbSound.Text;
                        if (tbSound.Text.ToLower().EndsWith(".mp3")) SoundFile = TempWavePath;

                        soundLength = Wii.Sound.GetWaveLength(SoundFile);
                        if (soundLength > SoundMaxLength)
                        {
                            ErrorBox(string.Format("Your Sound is longer than {0} seconds and thus not supported.\nIt is recommended to use a Sound shorter than {1} seconds, the maximum length is {0} seconds!", SoundMaxLength, SoundWarningLength));
                            return false;
                        }
                    }
                }

                /*Errors till here..
                  From here only Warnings!*/

                if (soundLength > SoundWarningLength)
                {
                    if (MessageBox.Show(string.Format("Your Sound is longer than {0} seconds.\nIt is recommended to use Sounds that are shorter than {0} seconds!\nDo you still want to continue?", SoundWarningLength), "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                        return false;
                }

                //Check BNS sound length
                if (tbSound.Text.StartsWith("BNS:") || tbSound.Text.ToLower().EndsWith(".bns"))
                {
                    string bnsFile = tbSound.Text;
                    if (tbSound.Text.StartsWith("BNS:")) bnsFile = TempBnsPath;

                    int bnsLength = Wii.Sound.GetBnsLength(bnsFile);
                    if (bnsLength > BnsWarningLength)
                    {
                        if (MessageBox.Show(string.Format("Your BNS Sound is longer than {0} seconds.\nIt is recommended to use Sounds that are shorter than {0} seconds!\nDo you still want to continue?", BnsWarningLength), "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                            return false;
                    }
                }

                //Check if brlyt or brlan were changed
                if (BrlytChanged == true && BrlanChanged == false)
                {
                    if (MessageBox.Show("You have changed the brlyt, but didn't change the brlan.\nAre you sure this is correct?", "brlyt Changed", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return false;
                }
                else if (BrlanChanged == true && BrlytChanged == false)
                {
                    if (MessageBox.Show("You have changed the brlan, but didn't change the brlyt.\nAre you sure this is correct?", "brlan Changed", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return false;
                }

                //Check for unused TPLs (Do this at the end of the failure checks, because we don't want a
                //                       MessageBox asking if unused TPLs should be deleted and after that any error)           
                if (Wii.Brlyt.CheckForUnusedTpls(BannerBrlytPath[0], BannerBrlanPath[0], BannerTpls.ToArray(), out BannerUnused) == true)
                {
                    DialogResult dlgresult = MessageBox.Show(
                        "The following Banner TPLs are unused by the banner.brlyt:\n\n" +
                        string.Join("\n", BannerUnused) +
                        "\n\nDo you want them to be deleted before the WAD is being created? (Saves space!)",
                        "Delete unused TPLs?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dlgresult == DialogResult.Yes)
                    {
                        foreach (string thisTpl in BannerUnused)
                        {
                            if (string.IsNullOrEmpty(BannerReplace))
                                File.Delete(TempUnpackBannerTplPath + thisTpl);
                            else
                                File.Delete(TempBannerPath + "arc\\timg\\" + thisTpl);
                        }
                    }
                    else if (dlgresult == DialogResult.Cancel) return false;
                }
                if (Wii.Brlyt.CheckForUnusedTpls(IconBrlytPath[0], IconBrlanPath[0], IconTpls.ToArray(), out IconUnused) == true)
                {
                    DialogResult dlgresult = MessageBox.Show(
                        "The following Icon TPLs are unused by the icon.brlyt:\n\n" +
                        string.Join("\n", IconUnused) +
                        "\n\nDo you want them to be deleted before the WAD is being created? (Saves memory!)",
                        "Delete unused TPLs?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dlgresult == DialogResult.Yes)
                    {
                        foreach (string thisTpl in IconUnused)
                        {
                            if (string.IsNullOrEmpty(IconReplace))
                                File.Delete(TempUnpackIconTplPath + thisTpl);
                            else
                                File.Delete(TempIconPath + "arc\\timg\\" + thisTpl);
                        }
                    }
                    else if (dlgresult == DialogResult.Cancel) return false;
                }

                currentProgress.progressState = " ";
                currentProgress.progressValue = 100;
                this.Invoke(ProgressUpdate);

                return true;
            }
            catch (Exception ex)
            {
                ErrorBox(ex.Message);
                return false;
            }
        }

        private void ForwarderDialogSimple()
        {
            CustomizeMii_InputBox ib = new CustomizeMii_InputBox(false);
            ib.Size = new Size(ib.Size.Width, 120);
            ib.lbInfo.Text = "Enter the application folder where the forwarder will point to (3-18 chars)";
            ib.tbInput.MaxLength = 18;
            ib.btnExit.Text = "Cancel";
            ib.cbElf.Visible = true;

            ib.tbInput.Text = SimpleForwarder.AppFolder;
            ib.cbElf.Checked = SimpleForwarder.ForwardToElf;

            if (ib.ShowDialog() == DialogResult.OK)
            {
                SimpleForwarder.ForwardToElf = ib.cbElf.Checked;
                SimpleForwarder.AppFolder = ib.Input;
                SetText(tbDol, string.Format("Simple Forwarder: \"SD:\\apps\\{0}\\boot.{1}\"",
                    SimpleForwarder.AppFolder, SimpleForwarder.ForwardToElf == true ? "elf" : "dol"));
                btnBrowseDol.Text = "Clear";
            }
        }

        private void ForwarderDialogComplex()
        {
            CustomizeMii_ComplexForwarder cf = new CustomizeMii_ComplexForwarder();
            cf.tb1.Text = ComplexForwarder.Path1;
            cf.tb2.Text = ComplexForwarder.Path2;
            cf.tb3.Text = ComplexForwarder.Path3;
            cf.tb4.Text = ComplexForwarder.Path4;

            if (!string.IsNullOrEmpty(ComplexForwarder.Image43))
            {
                cf.cbImage43.Checked = true;
                cf.tbImage43.Enabled = true;
                cf.btnBrowseImage43.Enabled = true;

                cf.tbImage43.Text = ComplexForwarder.Image43;
            }
            if (!string.IsNullOrEmpty(ComplexForwarder.Image169))
            {
                cf.cbImage169.Checked = true;
                cf.tbImage169.Enabled = true;
                cf.btnBrowseImage169.Enabled = true;

                cf.tbImage169.Text = ComplexForwarder.Image169;
            }

            if (cf.ShowDialog() == DialogResult.OK)
            {
                ComplexForwarder.Path1 = cf.tb1.Text;
                ComplexForwarder.Path2 = cf.tb2.Text;
                ComplexForwarder.Path3 = cf.tb3.Text;
                ComplexForwarder.Path4 = cf.tb4.Text;

                ComplexForwarder.Image43 = cf.tbImage43.Text;
                ComplexForwarder.Image169 = cf.tbImage169.Text;

                SetText(tbDol, string.Format("Complex Forwarder"));
                btnBrowseDol.Text = "Clear";
            }
        }

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
                        //else if (Ctrl is LinkLabel && Ctrl.Tag != (object)"Independent") Ctrl.Enabled = true;
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
                        //else if (Ctrl is LinkLabel && Ctrl.Tag != (object)"Independent") Ctrl.Enabled = false;
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

                    string[] brlytTpls = Wii.Brlyt.GetBrlytTpls(CurPath + string.Format("blyt\\{0}.brlyt", lbx == lbxBannerTpls ? "banner" : "icon"),
                        CurPath + string.Format("anim\\{0}.brlan", lbx == lbxBannerTpls ? (File.Exists(CurPath + "anim\\banner.brlan")) ? "banner" : "banner_loop" : "icon"));
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
