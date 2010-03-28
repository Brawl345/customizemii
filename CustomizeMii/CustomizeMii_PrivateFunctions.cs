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
using libWiiSharp;

namespace CustomizeMii
{
    partial class CustomizeMii_Main
    {
        private Forwarder.Simple simpleForwarder = new Forwarder.Simple();
        private Forwarder.Complex complexForwarder = new Forwarder.Complex();
        private delegate void messageInvoker(string message);
        private delegate void controlTextInvoker(Control ctrl, string text);

        private bool securityChecks()
        {
            if (cbSecurityChecksOff.Checked) return true;

            try
            {
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
                    errorBox("You must either enter a general Channel Title or one for each language!");
                    return false;
                }

                //Check Title ID Length + Chars
                if (tbTitleID.Text.Length != 4)
                { errorBox("The Title ID must be 4 characters long!"); return false; }

                Regex allowedchars = new Regex("[A-Z0-9]{4}$");
                if (!allowedchars.IsMatch(tbTitleID.Text.ToUpper()))
                { errorBox("Please enter a valid Title ID!"); return false; }

                //Check Startup IOS Box
                int tmp;
                if (!int.TryParse(tbStartupIos.Text, out tmp))
                { errorBox("Please enter a valid Startup IOS! (0 - 255)"); return false; }
                else if (tmp < 0 || tmp > 255)
                { errorBox("Please enter a valid Startup IOS! (0 - 255)"); return false; }

                //Check brlan files
                string[] validBrlans = new string[] { "banner.brlan", "icon.brlan", "banner_loop.brlan", "banner_start.brlan" };
                foreach (string thisBrlan in lbxBrlanBanner.Items)
                {
                    if (!Array.Exists(validBrlans, brlanName => brlanName.ToLower() == thisBrlan.ToLower()))
                    { errorBox(thisBrlan + " is not a valid brlan filename!"); return false; }
                }
                foreach (string thisBrlan in lbxBrlanIcon.Items)
                {
                    if (!Array.Exists(validBrlans, brlanName => brlanName.ToLower() == thisBrlan.ToLower()))
                    { errorBox(thisBrlan + " is not a valid brlan filename!"); return false; }
                }

                //Check TPLs
                string[] bannerRequiredTpls = new string[0];
                string[] iconRequiredTpls = new string[0];
                List<string> bannerTpls = new List<string>();
                List<string> iconTpls = new List<string>();

                if (string.IsNullOrEmpty(replacedBanner))
                {
                    for (int i = 0; i < bannerBin.NumOfNodes; i++)
                    {
                        if (bannerBin.StringTable[i].ToLower().EndsWith(".brlyt"))
                        { bannerRequiredTpls = Shared.MergeStringArrays(bannerRequiredTpls, Brlyt.GetBrlytTpls(bannerBin.Data[i])); }
                        else if (bannerBin.StringTable[i].ToLower().EndsWith(".brlan"))
                        { bannerRequiredTpls = Shared.MergeStringArrays(bannerRequiredTpls, Brlan.GetBrlanTpls(bannerBin.Data[i])); }
                        else if (bannerBin.StringTable[i].ToLower().EndsWith(".tpl"))
                        { bannerTpls.Add(bannerBin.StringTable[i]); }
                    }
                }
                else
                {
                    for (int i = 0; i < newBannerBin.NumOfNodes; i++)
                    {
                        if (newBannerBin.StringTable[i].ToLower().EndsWith(".brlyt"))
                        { bannerRequiredTpls = Shared.MergeStringArrays(bannerRequiredTpls, Brlyt.GetBrlytTpls(newBannerBin.Data[i])); }
                        else if (newBannerBin.StringTable[i].ToLower().EndsWith(".brlan"))
                        { bannerRequiredTpls = Shared.MergeStringArrays(bannerRequiredTpls, Brlan.GetBrlanTpls(newBannerBin.Data[i])); }
                        else if (newBannerBin.StringTable[i].ToLower().EndsWith(".tpl"))
                        { bannerTpls.Add(newBannerBin.StringTable[i]); }
                    }
                }

                if (string.IsNullOrEmpty(replacedIcon))
                {
                    for (int i = 0; i < iconBin.NumOfNodes; i++)
                    {
                        if (iconBin.StringTable[i].ToLower().EndsWith(".brlyt"))
                        { iconRequiredTpls = Shared.MergeStringArrays(iconRequiredTpls, Brlyt.GetBrlytTpls(iconBin.Data[i])); }
                        else if (iconBin.StringTable[i].ToLower().EndsWith(".brlan"))
                        { iconRequiredTpls = Shared.MergeStringArrays(iconRequiredTpls, Brlan.GetBrlanTpls(iconBin.Data[i])); }
                        else if (iconBin.StringTable[i].ToLower().EndsWith(".tpl"))
                        { iconTpls.Add(iconBin.StringTable[i]); }
                    }
                }
                else
                {
                    for (int i = 0; i < newIconBin.NumOfNodes; i++)
                    {
                        if (newIconBin.StringTable[i].ToLower().EndsWith(".brlyt"))
                        { iconRequiredTpls = Shared.MergeStringArrays(iconRequiredTpls, Brlyt.GetBrlytTpls(newIconBin.Data[i])); }
                        else if (newIconBin.StringTable[i].ToLower().EndsWith(".brlan"))
                        { iconRequiredTpls = Shared.MergeStringArrays(iconRequiredTpls, Brlan.GetBrlanTpls(newIconBin.Data[i])); }
                        else if (newIconBin.StringTable[i].ToLower().EndsWith(".tpl"))
                        { iconTpls.Add(newIconBin.StringTable[i]); }
                    }
                }

                //Check for missing TPLs
                List<string> missingTpls = new List<string>();

                for (int i = 0; i < bannerRequiredTpls.Length; i++)
                    if (!Array.Exists(bannerTpls.ToArray(), thisTpl => thisTpl.ToLower() == bannerRequiredTpls[i].ToLower()))
                        missingTpls.Add(bannerRequiredTpls[i]);

                if (missingTpls.Count > 0)
                {
                    errorBox("The following Banner TPLs are required by the banner.brlyt, but missing:\n\n" + string.Join("\n", missingTpls.ToArray()));
                    return false;
                }

                missingTpls.Clear();

                for (int i = 0; i < iconRequiredTpls.Length; i++)
                    if (!Array.Exists(iconTpls.ToArray(), thisTpl => thisTpl.ToLower() == iconRequiredTpls[i].ToLower()))
                        missingTpls.Add(iconRequiredTpls[i]);

                if (missingTpls.Count > 0)
                {
                    errorBox("The following Icon TPLs are required by the icon.brlyt, but missing:\n\n" + string.Join("\n", missingTpls.ToArray()));
                    return false;
                }

                //Check Sound length
                int soundLength = 0;
                if (!string.IsNullOrEmpty(replacedSound))
                {
                    if (!tbSound.Text.ToLower().EndsWith(".bns") && !tbSound.Text.StartsWith("BNS:"))
                    {
                        Wave w = new Wave(Headers.IMD5.RemoveHeader(newSoundBin));
                        soundLength = w.GetWaveLength();
                        if (soundLength > soundMaxLength)
                        {
                            errorBox(string.Format("Your wave sound is longer than {0} seconds and thus not supported.\nIt is recommended to use a sound shorter than {1} seconds, the maximum length is {0} seconds!\nThis limit doesn't affect BNS sounds!", soundMaxLength, soundWarningLength));
                            return false;
                        }
                    }
                }

                /*Errors till here..
                  From here only Warnings!*/

                if (soundLength > soundWarningLength)
                {
                    if (MessageBox.Show(string.Format("Your Sound is longer than {0} seconds.\nIt is recommended to use Sounds that are shorter than {0} seconds!\nDo you still want to continue?", soundWarningLength), "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                        return false;
                }

                //Check BNS sound length
                if (tbSound.Text.StartsWith("BNS:") || tbSound.Text.ToLower().EndsWith(".bns"))
                {
                    int bnsLength = BNS.GetBnsLength(Headers.IMD5.RemoveHeader(newSoundBin));
                    if (bnsLength > bnsWarningLength)
                    {
                        if (MessageBox.Show(string.Format("Your BNS Sound is longer than {0} seconds.\nIt is recommended to use Sounds that are shorter than {0} seconds!\nDo you still want to continue?", bnsWarningLength), "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                            return false;
                    }
                }

                //Check if brlyt or brlan were changed
                if (brlytChanged && !brlanChanged)
                {
                    if (MessageBox.Show("You have changed the brlyt, but didn't change the brlan.\nAre you sure this is correct?", "brlyt Changed", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return false;
                }
                else if (brlanChanged && !brlytChanged)
                {
                    if (MessageBox.Show("You have changed the brlan, but didn't change the brlyt.\nAre you sure this is correct?", "brlan Changed", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return false;
                }

                //Check for unused TPLs
                List<string> unusedTpls = new List<string>();

                for (int i = 0; i < bannerTpls.Count; i++)
                    if (!Array.Exists(bannerRequiredTpls, thisTpl => thisTpl.ToLower() == bannerTpls[i].ToLower()))
                        unusedTpls.Add(bannerTpls[i]);

                if (unusedTpls.Count > 0)
                {
                    DialogResult dlgresult = MessageBox.Show(
                        "The following Banner TPLs are unused by the banner.brlyt:\n\n" +
                        string.Join("\n", unusedTpls.ToArray()) +
                        "\n\nDo you want them to be deleted before the WAD is being created? (Saves space!)",
                        "Delete unused TPLs?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dlgresult == DialogResult.Yes)
                    {
                        if (string.IsNullOrEmpty(replacedBanner))
                        {
                            foreach (string thisTpl in unusedTpls)
                                bannerBin.RemoveFile("/arc/timg/" + thisTpl);
                        }
                        else
                        {
                            foreach (string thisTpl in unusedTpls)
                                newBannerBin.RemoveFile("/arc/timg/" + thisTpl);
                        }

                        addTpls();
                    }
                    else if (dlgresult == DialogResult.Cancel) return false;
                }

                unusedTpls.Clear();

                for (int i = 0; i < iconTpls.Count; i++)
                    if (!Array.Exists(iconRequiredTpls, thisTpl => thisTpl.ToLower() == iconTpls[i].ToLower()))
                        unusedTpls.Add(iconTpls[i]);


                if (unusedTpls.Count > 0)
                {
                    DialogResult dlgresult = MessageBox.Show(
                        "The following Icon TPLs are unused by the icon.brlyt:\n\n" +
                        string.Join("\n", unusedTpls.ToArray()) +
                        "\n\nDo you want them to be deleted before the WAD is being created? (Saves memory!)",
                        "Delete unused TPLs?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dlgresult == DialogResult.Yes)
                    {
                        if (string.IsNullOrEmpty(replacedIcon))
                        {
                            foreach (string thisTpl in unusedTpls)
                                iconBin.RemoveFile("/arc/timg/" + thisTpl);
                        }
                        else
                        {
                            foreach (string thisTpl in unusedTpls)
                                newIconBin.RemoveFile("/arc/timg/" + thisTpl);
                        }

                        addTpls();
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
                errorBox(ex.Message);
                return false;
            }
        }

        private void forwarderDialogSimple()
        {
            CustomizeMii_InputBox ib = new CustomizeMii_InputBox(false);
            ib.Size = new Size(ib.Size.Width, 120);
            ib.lbInfo.Text = "Enter the application folder where the forwarder will point to (3-18 chars)";
            ib.tbInput.MaxLength = 18;
            ib.btnExit.Text = "Cancel";
            ib.cbElf.Visible = true;

            ib.tbInput.Text = simpleForwarder.AppFolder;
            ib.cbElf.Checked = simpleForwarder.ForwardToElf;

            if (ib.ShowDialog() == DialogResult.OK)
            {
                simpleForwarder.ForwardToElf = ib.cbElf.Checked;
                simpleForwarder.AppFolder = ib.Input;
                setControlText(tbDol, string.Format("Simple Forwarder: \"SD:\\apps\\{0}\\boot.{1}\"",
                    simpleForwarder.AppFolder, simpleForwarder.ForwardToElf == true ? "elf" : "dol"));
                btnBrowseDol.Text = "Clear";
            }
        }

        private void forwarderDialogComplex()
        {
            CustomizeMii_ComplexForwarder cf = new CustomizeMii_ComplexForwarder();

            TextBox[] tbs = new TextBox[] { cf.tb1, cf.tb2, cf.tb3, cf.tb4, cf.tb5, cf.tb6, cf.tb7, cf.tb8,
                                            cf.tb9, cf.tb10, cf.tb11, cf.tb12, cf.tb13, cf.tb14, cf.tb15, cf.tb16};
            for (int i = 0; i < tbs.Length; i++)
                tbs[i].Text = complexForwarder.GetPath(i);

            cf.cbPack1.Checked = complexForwarder.Packs[0];
            cf.cbPack2.Checked = complexForwarder.Packs[1];
            cf.cbPack3.Checked = complexForwarder.Packs[2];

            if (!string.IsNullOrEmpty(complexForwarder.Image43))
            {
                cf.cbImage43.Checked = true;
                cf.tbImage43.Enabled = true;
                cf.btnBrowseImage43.Enabled = true;

                cf.tbImage43.Text = complexForwarder.Image43;
            }
            if (!string.IsNullOrEmpty(complexForwarder.Image169))
            {
                cf.cbImage169.Checked = true;
                cf.tbImage169.Enabled = true;
                cf.btnBrowseImage169.Enabled = true;

                cf.tbImage169.Text = complexForwarder.Image169;
            }

            if (cf.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < tbs.Length; i++)
                    complexForwarder.SetPath(i, tbs[i].Text.Replace('\\', '/'));

                complexForwarder.Packs[0] = cf.cbPack1.Checked;
                complexForwarder.Packs[1] = cf.cbPack2.Checked;
                complexForwarder.Packs[2] = cf.cbPack3.Checked;

                complexForwarder.Image43 = (cf.cbImage43.Checked) ? cf.tbImage43.Text : string.Empty;
                complexForwarder.Image169 = (cf.cbImage169.Checked) ? cf.tbImage169.Text : string.Empty;

                setControlText(tbDol, string.Format("Complex Forwarder"));
                btnBrowseDol.Text = "Clear";
            }
        }

        private void enableControls()
        {
            if (this.InvokeRequired)
            {
               this.Invoke(new MethodInvoker(enableControls));
               return;
            }

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

            llbBannerMultiReplace.Enabled = true;
            llbIconMultiReplace.Enabled = true;
        }

        private void disableControls()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(disableControls));
                return;
            }

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

            llbBannerMultiReplace.Enabled = false;
            llbIconMultiReplace.Enabled = false;
        }

        private Image resizeImage(Image img, int x, int y)
        {
            Image newimage = new Bitmap(x, y);
            using (Graphics gfx = Graphics.FromImage(newimage))
            {
                gfx.DrawImage(img, 0, 0, x, y);
            }
            return newimage;
        }

        private void setControlText(Control ctrl, string text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new controlTextInvoker(setControlText), ctrl, text);
                return;
            }

            ctrl.Text = text;
        }

        private void addTpls()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(addTpls));
                return;
            }

            List<string> bannerTpls = new List<string>();

            if (string.IsNullOrEmpty(replacedBanner))
            {
                for (int i = 0; i < bannerBin.NumOfNodes; i++)
                    if (bannerBin.StringTable[i].ToLower().EndsWith(".tpl"))
                    {
                        try
                        {
                            TPL tmpTpl = TPL.Load(bannerBin.Data[i]);
                            bannerTpls.Add(string.Format("{0}   ({3},   {1} x {2},   {4})", bannerBin.StringTable[i], tmpTpl.GetTextureSize(0).Width, tmpTpl.GetTextureSize(0).Height, tmpTpl.GetTextureFormat(0).ToString(), getSizeString(bannerBin.Data[i].Length)));
                        }
                        catch { }
                    }
            }
            else
            {
                for (int i = 0; i < newBannerBin.NumOfNodes; i++)
                    if (newBannerBin.StringTable[i].ToLower().EndsWith(".tpl"))
                    {
                        try
                        {
                            TPL tmpTpl = TPL.Load(newBannerBin.Data[i]);
                            bannerTpls.Add(string.Format("{0}   ({3},   {1} x {2},   {4})", newBannerBin.StringTable[i], tmpTpl.GetTextureSize(0).Width, tmpTpl.GetTextureSize(0).Height, tmpTpl.GetTextureFormat(0).ToString(), getSizeString(newBannerBin.Data[i].Length)));
                        }
                        catch { }
                    }
            }

            _addBannerTpls(bannerTpls.ToArray());

            List<string> iconTpls = new List<string>();

            if (string.IsNullOrEmpty(replacedIcon))
            {
                for (int i = 0; i < iconBin.NumOfNodes; i++)
                    if (iconBin.StringTable[i].ToLower().EndsWith(".tpl"))
                    {
                        try
                        {
                            TPL tmpTpl = TPL.Load(iconBin.Data[i]);
                            iconTpls.Add(string.Format("{0}   ({3},   {1} x {2},   {4})", iconBin.StringTable[i], tmpTpl.GetTextureSize(0).Width, tmpTpl.GetTextureSize(0).Height, tmpTpl.GetTextureFormat(0).ToString(), getSizeString(iconBin.Data[i].Length)));
                        }
                        catch { }
                    }
            }
            else
            {
                for (int i = 0; i < newIconBin.NumOfNodes; i++)
                    if (newIconBin.StringTable[i].ToLower().EndsWith(".tpl"))
                    {
                        try
                        {
                            TPL tmpTpl = TPL.Load(newIconBin.Data[i]);
                            iconTpls.Add(string.Format("{0}   ({3},   {1} x {2},   {4})", newIconBin.StringTable[i], tmpTpl.GetTextureSize(0).Width, tmpTpl.GetTextureSize(0).Height, tmpTpl.GetTextureFormat(0).ToString(), getSizeString(newIconBin.Data[i].Length)));
                        }
                        catch { }
                    }
            }

            _addIconTpls(iconTpls.ToArray());
        }

        private string getSizeString(int dataLength)
        {
            if (dataLength > 1022976)
                return string.Format("{0}MB", Math.Round(dataLength / 1024d / 1024d, 2)).Replace(',', ',');
            else
                return string.Format("{0}KB", Math.Round(dataLength / 1024d, 2)).Replace(',', '.');
        }

        private void _addBannerTpls(string[] tpls)
        {
            lbxBannerTpls.Items.Clear();

            if (tpls.Length > 0)
            {
                for (int i = 0; i < tpls.Length; i++)
                {
                    if (bannerTransparents.Contains(tpls[i].Remove(tpls[i].IndexOf('(') - 3)))
                        lbxBannerTpls.Items.Add(tpls[i] + " (Transparent)");
                    else
                        lbxBannerTpls.Items.Add(tpls[i]);
                }
            }
        }

        private void _addIconTpls(string[] tpls)
        {
            lbxIconTpls.Items.Clear();

            if (tpls.Length > 0)
            {
                for (int i = 0; i < tpls.Length; i++)
                {
                    if (iconTransparents.Contains(tpls[i].Remove(tpls[i].IndexOf('(') - 3)))
                        lbxIconTpls.Items.Add(tpls[i] + " (Transparent)");
                    else
                        lbxIconTpls.Items.Add(tpls[i]);
                }
            }
        }

        private void addBrlyts()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(addBrlyts));
                return;
            }

            try
            {
                List<string> bannerBrlyts = new List<string>();
                for (int i = 0; i < bannerBin.NumOfNodes; i++)
                    if (bannerBin.StringTable[i].ToLower().EndsWith(".brlyt"))
                        bannerBrlyts.Add(bannerBin.StringTable[i]);

                List<string> iconBrlyts = new List<string>();
                for (int i = 0; i < iconBin.NumOfNodes; i++)
                    if (iconBin.StringTable[i].ToLower().EndsWith(".brlyt"))
                        iconBrlyts.Add(iconBin.StringTable[i]);

                _addBannerBrlyt(bannerBrlyts.ToArray());
                _addIconBrlyt(iconBrlyts.ToArray());

                if (lbxBrlytBanner.SelectedIndex == -1 && lbxBrlytIcon.SelectedIndex == -1)
                {
                    if (lbxBrlytBanner.Items.Count > 0) lbxBrlytBanner.SelectedIndex = 0;
                    else if (lbxBrlytIcon.Items.Count > 0) lbxBrlytIcon.SelectedIndex = 0;
                }
            }
            catch { }
        }

        private void addBrlans()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(addBrlans));
                return;
            }

            try
            {
                List<string> bannerBrlans = new List<string>();

                for (int i = 0; i < bannerBin.NumOfNodes; i++)
                    if (bannerBin.StringTable[i].ToLower().EndsWith(".brlan"))
                        bannerBrlans.Add(bannerBin.StringTable[i]);

                List<string> iconBrlans = new List<string>();
                for (int i = 0; i < iconBin.NumOfNodes; i++)
                    if (iconBin.StringTable[i].ToLower().EndsWith(".brlan"))
                        iconBrlans.Add(iconBin.StringTable[i]);

                _addBannerBrlan(bannerBrlans.ToArray());
                _addIconBrlan(iconBrlans.ToArray());

                if (lbxBrlanBanner.SelectedIndex == -1 && lbxBrlanIcon.SelectedIndex == -1)
                {
                    if (lbxBrlanBanner.Items.Count > 0) lbxBrlanBanner.SelectedIndex = 0;
                    else if (lbxBrlanIcon.Items.Count > 0) lbxBrlanIcon.SelectedIndex = 0;
                }
            }
            catch { }
        }

        private void _addBannerBrlyt(string[] brlyt)
        {
            if (brlyt.Length > 0)
            {
                lbxBrlytBanner.Items.Clear();

                for (int i = 0; i < brlyt.Length; i++)
                    lbxBrlytBanner.Items.Add(brlyt[i]);
            }
        }

        private void _addIconBrlyt(string[] brlyt)
        {
            if (brlyt.Length > 0)
            {
                lbxBrlytIcon.Items.Clear();

                for (int i = 0; i < brlyt.Length; i++)
                    lbxBrlytIcon.Items.Add(brlyt[i]);
            }
        }

        private void _addBannerBrlan(string[] brlan)
        {
            if (brlan.Length > 0)
            {
                lbxBrlanBanner.Items.Clear();

                for (int i = 0; i < brlan.Length; i++)
                    lbxBrlanBanner.Items.Add(brlan[i]);
            }
        }

        private void _addIconBrlan(string[] brlan)
        {
            if (brlan.Length > 0)
            {
                lbxBrlanIcon.Items.Clear();

                for (int i = 0; i < brlan.Length; i++)
                    lbxBrlanIcon.Items.Add(brlan[i]);
            }
        }

        private void errorBox(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new messageInvoker(errorBox), message);
                return;
            }

            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void infoBox(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new messageInvoker(infoBox), message);
                return;
            }

            MessageBox.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void makeBannerTplsTransparent()
        {
            foreach (string thisTpl in lbxBannerTpls.Items)
            {
                if (thisTpl.EndsWith("(Transparent)"))
                {
                    if (string.IsNullOrEmpty(replacedBanner))
                    {
                        for (int i = 0; i < bannerBin.NumOfNodes; i++)
                        {
                            if (thisTpl.Remove(thisTpl.IndexOf('(', 0) - 3).ToLower() == bannerBin.StringTable[i].ToLower())
                            {
                                TPL tmpTpl = TPL.Load(bannerBin.Data[i]);
                                Size tSize = tmpTpl.GetTextureSize(0);

                                Image tImg = new Bitmap(tSize.Width, tSize.Height);
                                tmpTpl.RemoveTexture(0);
                                tmpTpl.AddTexture(tImg, TPL_Format.IA4);

                                bannerBin.Data[i] = tmpTpl.ToByteArray();
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < newBannerBin.NumOfNodes; i++)
                        {
                            if (thisTpl.Remove(thisTpl.IndexOf('(', 0) - 3).ToLower() == newBannerBin.StringTable[i].ToLower())
                            {
                                TPL tmpTpl = TPL.Load(newBannerBin.Data[i]);
                                Size tSize = tmpTpl.GetTextureSize(0);

                                Image tImg = new Bitmap(tSize.Width, tSize.Height);
                                tmpTpl.RemoveTexture(0);
                                tmpTpl.AddTexture(tImg, TPL_Format.IA4);

                                newBannerBin.Data[i] = tmpTpl.ToByteArray();
                            }
                        }
                    }
                }
            }
        }

        private void makeIconTplsTransparent()
        {
            foreach (string thisTpl in lbxIconTpls.Items)
            {
                if (thisTpl.EndsWith("(Transparent)"))
                {
                    if (string.IsNullOrEmpty(replacedIcon))
                    {
                        for (int i = 0; i < iconBin.NumOfNodes; i++)
                        {
                            if (thisTpl.Remove(thisTpl.IndexOf('(', 0) - 3).ToLower() == iconBin.StringTable[i].ToLower())
                            {
                                TPL tmpTpl = TPL.Load(iconBin.Data[i]);
                                Size tSize = tmpTpl.GetTextureSize(0);

                                Image tImg = new Bitmap(tSize.Width, tSize.Height);
                                tmpTpl.RemoveTexture(0);
                                tmpTpl.AddTexture(tImg, TPL_Format.IA4);

                                iconBin.Data[i] = tmpTpl.ToByteArray();
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < newIconBin.NumOfNodes; i++)
                        {
                            if (thisTpl.Remove(thisTpl.IndexOf('(', 0) - 3).ToLower() == newIconBin.StringTable[i].ToLower())
                            {
                                TPL tmpTpl = TPL.Load(newIconBin.Data[i]);
                                Size tSize = tmpTpl.GetTextureSize(0);

                                Image tImg = new Bitmap(tSize.Width, tSize.Height);
                                tmpTpl.RemoveTexture(0);
                                tmpTpl.AddTexture(tImg, TPL_Format.IA4);

                                newIconBin.Data[i] = tmpTpl.ToByteArray();
                            }
                        }
                    }
                }
            }
        }

        private void addTpl(ListBox lbx)
        {
            addTpl(lbx, null);
        }

        private void addTpl(ListBox lbx, string inputFile)
        {
            try
            {
                int switchVal = lbx == lbxBannerTpls ? cmbFormatBanner.SelectedIndex : cmbFormatIcon.SelectedIndex;
                if (switchVal > 6)
                    throw new Exception("This format is not supported, you must choose a different one!");

                if (string.IsNullOrEmpty(inputFile))
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Filter = "TPL|*.tpl|PNG|*.png|JPG|*.jpg|GIF|*.gif|BMP|*.bmp|All|*.tpl;*.png;*.jpg;*.gif;*.bmp";
                    ofd.FilterIndex = 6;

                    if (ofd.ShowDialog() == DialogResult.OK)
                        inputFile = ofd.FileName;
                }

                if (!string.IsNullOrEmpty(inputFile))
                {
                    string tplName = Path.GetFileNameWithoutExtension(inputFile) + ".tpl";

                    if (lbx == lbxBannerTpls)
                    {
                        if (string.IsNullOrEmpty(replacedBanner))
                        {
                            for (int i = 0; i < bannerBin.NumOfNodes; i++)
                                if (bannerBin.StringTable[i].ToLower() == tplName.ToLower())
                                { errorBox("This TPL already exists, use the Replace button"); return; }
                        }
                        else
                        {
                            for (int i = 0; i < newBannerBin.NumOfNodes; i++)
                                if (newBannerBin.StringTable[i].ToLower() == tplName.ToLower())
                                { errorBox("This TPL already exists, use the Replace button"); return; }
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(replacedIcon))
                        {
                            for (int i = 0; i < iconBin.NumOfNodes; i++)
                                if (iconBin.StringTable[i].ToLower() == tplName.ToLower())
                                { errorBox("This TPL already exists, use the Replace button"); return; }
                        }
                        else
                        {
                            for (int i = 0; i < newIconBin.NumOfNodes; i++)
                                if (newIconBin.StringTable[i].ToLower() == tplName.ToLower())
                                { errorBox("This TPL already exists, use the Replace button"); return; }
                        }
                    }

                    string[] requiredTpls = new string[0];

                    if (lbx == lbxBannerTpls)
                    {
                        if (string.IsNullOrEmpty(replacedBanner))
                        {
                            for (int i = 0; i < bannerBin.NumOfNodes; i++)
                            {
                                if (bannerBin.StringTable[i].ToLower().EndsWith(".brlyt"))
                                { requiredTpls = Shared.MergeStringArrays(requiredTpls, Brlyt.GetBrlytTpls(bannerBin.Data[i])); }
                                else if (bannerBin.StringTable[i].ToLower().EndsWith(".brlan"))
                                { requiredTpls = Shared.MergeStringArrays(requiredTpls, Brlan.GetBrlanTpls(bannerBin.Data[i])); }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < newBannerBin.NumOfNodes; i++)
                            {
                                if (newBannerBin.StringTable[i].ToLower().EndsWith(".brlyt"))
                                { requiredTpls = Shared.MergeStringArrays(requiredTpls, Brlyt.GetBrlytTpls(newBannerBin.Data[i])); }
                                else if (newBannerBin.StringTable[i].ToLower().EndsWith(".brlan"))
                                { requiredTpls = Shared.MergeStringArrays(requiredTpls, Brlan.GetBrlanTpls(newBannerBin.Data[i])); }
                            }
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(replacedIcon))
                        {
                            for (int i = 0; i < iconBin.NumOfNodes; i++)
                            {
                                if (iconBin.StringTable[i].ToLower().EndsWith(".brlyt"))
                                { requiredTpls = Shared.MergeStringArrays(requiredTpls, Brlyt.GetBrlytTpls(iconBin.Data[i])); }
                                else if (iconBin.StringTable[i].ToLower().EndsWith(".brlan"))
                                { requiredTpls = Shared.MergeStringArrays(requiredTpls, Brlan.GetBrlanTpls(iconBin.Data[i])); }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < newIconBin.NumOfNodes; i++)
                            {
                                if (newIconBin.StringTable[i].ToLower().EndsWith(".brlyt"))
                                { requiredTpls = Shared.MergeStringArrays(requiredTpls, Brlyt.GetBrlytTpls(newIconBin.Data[i])); }
                                else if (newIconBin.StringTable[i].ToLower().EndsWith(".brlan"))
                                { requiredTpls = Shared.MergeStringArrays(requiredTpls, Brlan.GetBrlanTpls(newIconBin.Data[i])); }
                            }
                        }
                    }

                    if (!Array.Exists(requiredTpls, thisTpl => thisTpl.ToLower() == tplName.ToLower()))
                    {
                        if (MessageBox.Show(
                            string.Format("{0} is not required by your {1}.brlyt and thus only wastes memory!\n" +
                            "Do you still want to add it?", tplName, lbx == lbxBannerTpls ? "banner" : "icon"),
                            "TPL not required", MessageBoxButtons.YesNo, MessageBoxIcon.Information) ==
                            DialogResult.No)
                            return;
                    }

                    int tplFormat = 6;

                    switch (switchVal)
                    {
                        case 6: //I4
                            tplFormat = 0;
                            break;
                        case 5: //I8
                            tplFormat = 1;
                            break;
                        case 4: //IA4
                            tplFormat = 2;
                            break;
                        case 3: //IA8
                            tplFormat = 3;
                            break;
                        case 0:
                            tplFormat = 6;
                            break;
                        case 1:
                            tplFormat = 4;
                            break;
                        case 2:
                            tplFormat = 5;
                            break;
                        default:
                            if (!inputFile.ToLower().EndsWith(".tpl"))
                                throw new Exception("This format is not supported, you must choose a different one!");
                            break;
                    }

                    byte[] newTpl;
                    if (inputFile.ToLower().EndsWith(".tpl"))
                        newTpl = File.ReadAllBytes(inputFile);
                    else
                        newTpl = TPL.FromImage(inputFile, (TPL_Format)tplFormat).ToByteArray();

                    if (lbx == lbxBannerTpls)
                    {
                        if (string.IsNullOrEmpty(replacedBanner))
                        { bannerBin.AddFile("/arc/timg/" + tplName, newTpl); }
                        else
                        { newBannerBin.AddFile("/arc/timg/" + tplName, newTpl); }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(replacedIcon))
                        { iconBin.AddFile("/arc/timg/" + tplName, newTpl); }
                        else
                        { newIconBin.AddFile("/arc/timg/" + tplName, newTpl); }
                    }

                    addTpls();
                }
            }
            catch (Exception ex) { throw ex; }
        }

        private void loadChannel()
        {
            loadChannel(null);
        }

        private void loadChannel(string inputFile)
        {
            if (pbProgress.Value == 100)
            {
                if (string.IsNullOrEmpty(inputFile))
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Filter = "Wii Channels|*.wad";

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
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
                    BackgroundWorker bwLoadChannel = new BackgroundWorker();
                    bwLoadChannel.DoWork += new DoWorkEventHandler(bwLoadChannel_DoWork);
                    bwLoadChannel.ProgressChanged += new ProgressChangedEventHandler(bwLoadChannel_ProgressChanged);
                    bwLoadChannel.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoadChannel_RunWorkerCompleted);
                    bwLoadChannel.WorkerReportsProgress = true;
                    bwLoadChannel.RunWorkerAsync(inputFile);
                }
            }
        }

        private void replacePart()
        {
            replacePart(null);
        }

        private void replacePart(string inputFile)
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
                                    BackgroundWorker bwSoundReplace = new BackgroundWorker();
                                    bwSoundReplace.DoWork += new DoWorkEventHandler(bwSoundReplace_DoWork);
                                    bwSoundReplace.ProgressChanged += new ProgressChangedEventHandler(bwSoundReplace_ProgressChanged);
                                    bwSoundReplace.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSoundReplace_RunWorkerCompleted);
                                    bwSoundReplace.WorkerReportsProgress = true;
                                    bwSoundReplace.RunWorkerAsync(inputFile);
                                }
                            }

                            tbSound.Text = string.Empty;
                            btnBrowseSound.Text = "Browse...";
                        }
                        catch (Exception ex)
                        {
                            replacedSound = string.Empty;
                            setControlText(tbReplace, string.Empty);
                            errorBox(ex.Message);
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
                            replacedIcon = string.Empty;
                            setControlText(tbReplace, string.Empty);
                            errorBox(ex.Message);
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
                            replacedBanner = string.Empty;
                            setControlText(tbReplace, string.Empty);
                            errorBox(ex.Message);
                        }
                    }
                }
            }
        }

        private void multiReplace(bool banner)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Please select the folder where the images are in.\nThe images must have the same filename as the TPLs!";

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string imageDir = fbd.SelectedPath;

                List<string> tpls = new List<string>();
                List<string> replacedTpls = new List<string>();

                if (banner)
                {
                    if (string.IsNullOrEmpty(replacedBanner))
                    {
                        for (int i = 0; i < bannerBin.NumOfNodes; i++)
                            if (bannerBin.StringTable[i].ToLower().EndsWith(".tpl"))
                                tpls.Add(bannerBin.StringTable[i]);
                    }
                    else
                    {
                        for (int i = 0; i < newBannerBin.NumOfNodes; i++)
                            if (newBannerBin.StringTable[i].ToLower().EndsWith(".tpl"))
                                tpls.Add(newBannerBin.StringTable[i]);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(replacedIcon))
                    {
                        for (int i = 0; i < iconBin.NumOfNodes; i++)
                            if (iconBin.StringTable[i].ToLower().EndsWith(".tpl"))
                                tpls.Add(iconBin.StringTable[i]);
                    }
                    else
                    {
                        for (int i = 0; i < newIconBin.NumOfNodes; i++)
                            if (newIconBin.StringTable[i].ToLower().EndsWith(".tpl"))
                                tpls.Add(newIconBin.StringTable[i]);
                    }
                }

                foreach (string thisTpl in tpls)
                {
                    string image = string.Empty;

                    string path = imageDir + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(thisTpl);

                    if (File.Exists(path + ".png"))
                        image = path + ".png";
                    else if (File.Exists(path + ".jpg"))
                        image = path + ".jpg";
                    else if (File.Exists(path + ".gif"))
                        image = path + ".gif";
                    else if (File.Exists(path + ".bmp"))
                        image = path + ".bmp";
                    else continue;

                    try
                    {
                        if (banner)
                        {
                            if (string.IsNullOrEmpty(replacedBanner))
                            {
                                TPL tmpTpl = TPL.Load(bannerBin.Data[bannerBin.GetNodeIndex(thisTpl)]);
                                Image img = Image.FromFile(image);

                                TPL_Format tplFormat = tmpTpl.GetTextureFormat(0);
                                Size tplSize = tmpTpl.GetTextureSize(0);

                                if (tplSize.Width != img.Width ||
                                    tplSize.Height != img.Height)
                                    img = resizeImage(img, tplSize.Width, tplSize.Height);

                                tmpTpl.RemoveTexture(0);
                                tmpTpl.AddTexture(img, tplFormat);

                                bannerBin.ReplaceFile(bannerBin.GetNodeIndex(thisTpl), tmpTpl.ToByteArray());
                                replacedTpls.Add(thisTpl);
                            }
                            else
                            {
                                TPL tmpTpl = TPL.Load(newBannerBin.Data[newBannerBin.GetNodeIndex(thisTpl)]);
                                Image img = Image.FromFile(image);

                                TPL_Format tplFormat = tmpTpl.GetTextureFormat(0);
                                Size tplSize = tmpTpl.GetTextureSize(0);

                                if (tplSize.Width != img.Width ||
                                    tplSize.Height != img.Height)
                                    img = resizeImage(img, tplSize.Width, tplSize.Height);

                                tmpTpl.RemoveTexture(0);
                                tmpTpl.AddTexture(img, tplFormat);

                                newBannerBin.ReplaceFile(newBannerBin.GetNodeIndex(thisTpl), tmpTpl.ToByteArray());
                                replacedTpls.Add(thisTpl);
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(replacedIcon))
                            {
                                TPL tmpTpl = TPL.Load(iconBin.Data[iconBin.GetNodeIndex(thisTpl)]);
                                Image img = Image.FromFile(image);

                                TPL_Format tplFormat = tmpTpl.GetTextureFormat(0);
                                Size tplSize = tmpTpl.GetTextureSize(0);

                                if (tplSize.Width != img.Width ||
                                    tplSize.Height != img.Height)
                                    img = resizeImage(img, tplSize.Width, tplSize.Height);

                                tmpTpl.RemoveTexture(0);
                                tmpTpl.AddTexture(img, tplFormat);

                                iconBin.ReplaceFile(iconBin.GetNodeIndex(thisTpl), tmpTpl.ToByteArray());
                                replacedTpls.Add(thisTpl);
                            }
                            else
                            {
                                TPL tmpTpl = TPL.Load(newIconBin.Data[newIconBin.GetNodeIndex(thisTpl)]);
                                Image img = Image.FromFile(image);

                                TPL_Format tplFormat = tmpTpl.GetTextureFormat(0);
                                Size tplSize = tmpTpl.GetTextureSize(0);

                                if (tplSize.Width != img.Width ||
                                    tplSize.Height != img.Height)
                                    img = resizeImage(img, tplSize.Width, tplSize.Height);

                                tmpTpl.RemoveTexture(0);
                                tmpTpl.AddTexture(img, tplFormat);

                                newIconBin.ReplaceFile(newIconBin.GetNodeIndex(thisTpl), tmpTpl.ToByteArray());
                                replacedTpls.Add(thisTpl);
                            }
                        }
                    }
                    catch { }
                }

                if (replacedTpls.Count > 0)
                {
                    string replaced = string.Join("\n", replacedTpls.ToArray());
                    infoBox(string.Format("The following TPLs were successfully replaced:\n\n{0}", replaced));
                }
                else errorBox("No TPLs were replaced, did you name the images right?");
            }
        }
    }
}
