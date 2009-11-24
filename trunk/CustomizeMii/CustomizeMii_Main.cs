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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CustomizeMii
{
    public partial class CustomizeMii_Main : Form
    {
        const string Version = "1.1"; //Hint for myself: Never use a char in the Version (UpdateCheck)!
        const int SoundMaxLength = 30; //In seconds
        const int SoundWarningLength = 20; //In seconds
        protected string TempWadPath = Path.GetTempPath() + "CustomizeMii_Temp\\TempWad.wad";
        protected string TempUnpackPath = Path.GetTempPath() + "CustomizeMii_Temp\\Unpack\\";
        protected string TempBannerPath = Path.GetTempPath() + "CustomizeMii_Temp\\Banner\\";
        protected string TempIconPath = Path.GetTempPath() + "CustomizeMii_Temp\\Icon\\";
        protected string TempSoundPath = Path.GetTempPath() + "CustomizeMii_Temp\\sound.bin";
        protected string TempTempPath = Path.GetTempPath() + "CustomizeMii_Temp\\Temp\\";
        protected string TempUnpackBannerTplPath = Path.GetTempPath() + "CustomizeMii_Temp\\Unpack\\00000000.app_OUT\\meta\\banner.bin_OUT\\arc\\timg\\";
        protected string TempUnpackIconTplPath = Path.GetTempPath() + "CustomizeMii_Temp\\Unpack\\00000000.app_OUT\\meta\\icon.bin_OUT\\arc\\timg\\";
        protected string TempUnpackBannerBrlytPath = Path.GetTempPath() + "CustomizeMii_Temp\\Unpack\\00000000.app_OUT\\meta\\banner.bin_OUT\\arc\\blyt\\";
        protected string TempUnpackIconBrlytPath = Path.GetTempPath() + "CustomizeMii_Temp\\Unpack\\00000000.app_OUT\\meta\\icon.bin_OUT\\arc\\blyt\\";
        protected string TempUnpackBannerBrlanPath = Path.GetTempPath() + "CustomizeMii_Temp\\Unpack\\00000000.app_OUT\\meta\\banner.bin_OUT\\arc\\anim\\";
        protected string TempUnpackIconBrlanPath = Path.GetTempPath() + "CustomizeMii_Temp\\Unpack\\00000000.app_OUT\\meta\\icon.bin_OUT\\arc\\anim\\";
        protected string[] ButtonTexts = new string[] { "Create WAD!", "Fire!", "Go Go Go!", "Let's do it!", "What are you waitin' for?", "I want my Channel!", "Houston, We've Got a Problem!", "Error, please contact anyone!", "Isn't she sweet?", "Is that milk?", "In your face!", "_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_", "Wo ist der Notausgang?", "Take me to a higher place!", "What's goin' on?", "I'm a Button!", "Click!", "Today's date is " + DateTime.Now.ToShortDateString(), "Launch Time: " + DateTime.Now.ToLongTimeString() };
        protected string[] SourceWadUrls = new string[] { "http://wadder.net/bases/StaticBase.wad", "http://wadder.net/bases/GenesisGX.wad", "http://wadder.net/bases/MP-CE-Std.wad", "http://wadder.net/bases/MP-CE-Ext.wad", "http://wadder.net/bases/SNES9XGX.wad", "http://wadder.net/bases/FCEUGX-wilsoff.wad", "http://wadder.net/bases/FCEUGX.wad", "http://wadder.net/bases/Wii64.wad", "http://wadder.net/bases/WiiSXFull.wad", "http://wadder.net/bases/WiiSXRetro.wad", "http://wadder.net/bases/WADderBase1.wad", "http://wadder.net/bases/WADderBase2.wad", "http://wadder.net/bases/WADderBase3.wad", "http://wadder.net/bases/UniiLoader.wad", "http://wadder.net/bases/BackupChannel.wad" };
        protected string[] SourceWadPreviewUrls = new string[] { "http://www.youtube.com/watch?v=pFNKldTYQq0", "http://www.youtube.com/watch?v=p9A6iEQvv9w", "http://www.youtube.com/watch?v=Up1RZebUc_U", "http://www.youtube.com/watch?v=Up1RZebUc_U", "http://www.youtube.com/watch?v=P-Mxd6DMvFY", "http://www.youtube.com/watch?v=wrbrg-DH_h4", "http://www.youtube.com/watch?v=MfiVbQaiXw8", "http://www.youtube.com/watch?v=krCQ2J7ZH8Y", "http://www.youtube.com/watch?v=rZC1DKUM6QI", "http://www.youtube.com/watch?v=Uiy8w-bp1kI", "http://www.youtube.com/watch?v=BbSYCSI8tz8", "http://www.youtube.com/watch?v=PIFZevHQ8lQ", "http://www.youtube.com/watch?v=OIhvDNjphhc", "http://www.youtube.com/watch?v=KLcncEArQLY&NR=1", "http://www.youtube.com/watch?v=xE_EgdCRV1I" };
        private string BannerTplPath = string.Empty;
        private string IconTplPath = string.Empty;
        private string SourceWad = string.Empty;
        private string BannerBrlytPath = string.Empty;
        private string IconBrlytPath = string.Empty;
        private string BannerBrlanPath = string.Empty;
        private string IconBrlanPath = string.Empty;
        private string BannerReplace = string.Empty;
        private string IconReplace = string.Empty;
        private string SoundReplace = string.Empty;
        private int NandLoader = 0;
        private TextBox tbToChange;
        private string textToChange;
        private bool BrlytChanged = false;
        private bool BrlanChanged = false;
        private int ProgressValue;
        private string ProgressState;
        private EventHandler ProgressUpdate;
        private ToolTip TTip = new ToolTip();
        private int UnpackFolderErrorCount = 0;
        private Stopwatch CreationTimer = new Stopwatch();
        List<string> Transparents = new List<string>();

        public CustomizeMii_Main()
        {
            InitializeComponent();
            this.Icon = global::CustomizeMii.Properties.Resources.CustomizeMii;
        }

        private void CustomizeMii_Main_Load(object sender, EventArgs e)
        {
            InitializeStartup();
            CommonKeyCheck();
        }

        private void CustomizeMii_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Directory.Exists(Path.GetTempPath() + "CustomizeMii_Temp")) Directory.Delete(Path.GetTempPath() + "CustomizeMii_Temp", true);
        }

        private void InitializeStartup()
        {
            this.Text = this.Text.Replace("X", Version);
            this.lbCreditVersion.Text = this.lbCreditVersion.Text.Replace("X", Version);
            ProgressUpdate = new EventHandler(this.UpdateProgress);
            SetButtonText();
            cmbNandLoader.SelectedIndex = 0;
            cmbFormatBanner.SelectedIndex = 0;
            cmbFormatIcon.SelectedIndex = 0;
            cmbReplace.SelectedIndex = 0;
            pbProgress.Value = 100;
            BrlanChanged = false;
            BrlytChanged = false;
            UpdateCheck();
            DisableControls(null, null);
            SetToolTips();
            btnBrowseDol.Text = "Browse...";
            btnBrowseSound.Text = "Browse...";
            rtbInstructions.Rtf = Properties.Resources.Instructions;
            rtbInstructions.LinkClicked += new LinkClickedEventHandler(rtbInstructions_LinkClicked);
        }

        void rtbInstructions_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                Process.Start(e.LinkText);
            }
            catch (Exception ex) { ErrorBox(ex.Message); }
        }

        private void CommonKeyCheck()
        {
            if (!File.Exists(Application.StartupPath + "\\common-key.bin") && !File.Exists(Application.StartupPath + "\\key.bin"))
            {
                CustomizeMii_InputBox ib = new CustomizeMii_InputBox();

                if (ib.ShowDialog() == DialogResult.OK)
                {
                    Wii.Tools.CreateCommonKey(ib.Input, Application.StartupPath);
                }
            }
            else
            {
                if (File.Exists(Application.StartupPath + "\\common-key.bin"))
                {
                    if (Wii.Tools.CheckCommonKey(Application.StartupPath + "\\common-key.bin") == false)
                    {
                        File.Delete(Application.StartupPath + "\\common-key.bin");
                        CommonKeyCheck();
                    }
                }
                else if (File.Exists(Application.StartupPath + "\\key.bin"))
                {
                    if (Wii.Tools.CheckCommonKey(Application.StartupPath + "\\key.bin") == false)
                    {
                        File.Delete(Application.StartupPath + "\\key.bin");
                        CommonKeyCheck();
                    }
                }
            }
        }

        private void Initialize(object sender, EventArgs e)
        {
            InitializeStartup();
            ClearAll();
        }

        private void ClearAll()
        {
            tbAllLanguages.Text = string.Empty;
            tbDol.Text = string.Empty;
            tbDutch.Text = string.Empty;
            tbEnglish.Text = string.Empty;
            tbFrench.Text = string.Empty;
            tbGerman.Text = string.Empty;
            tbItalian.Text = string.Empty;
            tbJapanese.Text = string.Empty;
            tbReplace.Text = string.Empty;
            tbSound.Text = string.Empty;
            tbSourceWad.Text = string.Empty;
            tbSpanish.Text = string.Empty;
            tbTitleID.Text = string.Empty;

            BannerReplace = string.Empty;
            IconReplace = string.Empty;
            SoundReplace = string.Empty;
            SourceWad = string.Empty;
            BannerTplPath = string.Empty;
            IconTplPath = string.Empty;

            lbxBannerTpls.Items.Clear();
            lbxBrlanBanner.Items.Clear();
            lbxBrlanIcon.Items.Clear();
            lbxBrlytBanner.Items.Clear();
            lbxBrlytIcon.Items.Clear();
            lbxIconTpls.Items.Clear();

            cbLz77.Checked = false;
            cbIconMakeTransparent.Checked = false;
            cbBannerMakeTransparent.Checked = false;
        }

        private void SetToolTips()
        {
            TTip.SetToolTip(btnCreateWad, "Create WAD...");
            TTip.SetToolTip(btnBrowseSource, "Browse for a WAD that is used as a Base...");
            TTip.SetToolTip(btnLoadBaseWad, "Load the selected Base WAD...");
            TTip.SetToolTip(btnPreviewBaseWad, "Preview the selected Base WAD, a Browserwindow will be opened...");
            TTip.SetToolTip(btnSaveBaseWad, "Download and save the selected Base WAD to your HDD...");
            TTip.SetToolTip(btnBrowseReplace, "Browse for a Banner / Icon / Sound to use instead of the one within the Base WAD...\nWAD's, 00000000.app's and banner.bin's / icon.bin's / sound.bin's can be loaded...");
            TTip.SetToolTip(btnClearReplace, "Clear the replaced Banner / Icon / Sound and use the one within the Base WAD...");
            TTip.SetToolTip(btnBrowseDol, "Browse for a DOL file that will be inserted into the WAD...");
            TTip.SetToolTip(btnBrowseSound, "Browse for a WAV sound that will be inserted into the WAD...");
            TTip.SetToolTip(btnAddBanner, "Add an image or TPL to the Banner...");
            TTip.SetToolTip(btnAddIcon, "Add an image or TPL to the Icon...");
            TTip.SetToolTip(btnDeleteBanner, "Delete the selected TPL...\nRequired TPLs can't be deleted...");
            TTip.SetToolTip(btnDeleteIcon, "Delete the selected TPL...\nRequired TPLs can't be deleted...");
            TTip.SetToolTip(btnReplaceBanner, "Replace the selected TPL with any image...\nThe image wil be stretched to fit the size of the TPL...");
            TTip.SetToolTip(btnReplaceIcon, "Replace the selected TPL with any image...\nThe image wil be stretched to fit the size of the TPL...");
            TTip.SetToolTip(btnExtractBanner, "Extract the selected TPL as an image...");
            TTip.SetToolTip(btnExtractIcon, "Extract the selected TPL as an image...");
            TTip.SetToolTip(btnPreviewBanner, "Preview the selected TPL...");
            TTip.SetToolTip(btnPreviewIcon, "Preview the selected TPL...");
            TTip.SetToolTip(btnBrlytReplace, "Replace the selected brlyt with any other...\nThis is for advanced users only!");
            TTip.SetToolTip(btnBrlytExtract, "Extract the selected brlyt...");
            TTip.SetToolTip(btnBrlytListTpls, "List the TPLs required by the selected brlyt...");
            TTip.SetToolTip(btnBrlanAdd, "Add a brlan file...\nThis is for advanced users only!");
            TTip.SetToolTip(btnBrlanDelete, "Delete the selected brlan file...\nThis is for advanced users only!");
            TTip.SetToolTip(btnBrlanReplace, "Replace the selected brlan file...\nThis is for advanced users only!");
            TTip.SetToolTip(btnBrlanExtract, "Extract the selected brlan file...");

            TTip.SetToolTip(cbLz77, "Use Lz77 compression for the banner.bin, icon.bin and sound.bin...\nIf the created WAD does not work, try it without compression first...");
        }

        private void SetButtonText()
        {
            Random randomize = new Random();
            btnCreateWad.Text = ButtonTexts[randomize.Next(0, ButtonTexts.Length - 1)];
        }

        private void ErrorBox(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void InfoBox(string message)
        {
            MessageBox.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool CheckInet()
        {
            try
            {
                System.Net.IPHostEntry IpHost = System.Net.Dns.GetHostEntry("www.google.com");
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void UpdateProgress(object sender, EventArgs e)
        {
            pbProgress.Value = ProgressValue;

            if (!string.IsNullOrEmpty(ProgressState))
                lbStatusText.Text = ProgressState;
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
                    if (Transparents.Contains(tpls[i].Remove(0, tpls[i].LastIndexOf('\\') + 1)))
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
                    if (Transparents.Contains(tpls[i].Remove(0, tpls[i].LastIndexOf('\\') + 1)))
                        lbxIconTpls.Items.Add(tpls[i].Remove(0, tpls[i].LastIndexOf('\\') + 1) + " (Transparent)");
                    else
                        lbxIconTpls.Items.Add(tpls[i].Remove(0, tpls[i].LastIndexOf('\\') + 1));
                }
            }
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

        private void SetText(TextBox tb, string text)
        {
            EventHandler ChangeText = new EventHandler(this.ChangeText);
            tbToChange = tb;
            textToChange = text;
            this.Invoke(ChangeText);
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

        private void ChangeText(object sender, EventArgs e)
        {
            tbToChange.Text = textToChange;
        }

        private void SetSourceWad(object sender, EventArgs e)
        {
            tbSourceWad.Text = SourceWad;
        }

        private void tabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (tabControl.SelectedTab == tabBanner)
            {
                lbxBannerTpls.SelectedIndex = -1;
                cbBannerMakeTransparent.Checked = false;
                cbBannerMakeTransparent.Enabled = false;
                AddBannerTpls(null, null);
            }
            else if (tabControl.SelectedTab == tabIcon)
            {
                lbxIconTpls.SelectedIndex = -1;
                cbIconMakeTransparent.Checked = false;
                cbIconMakeTransparent.Enabled = false;
                AddIconTpls(null, null);
            }
            else if (tabControl.SelectedTab == tabBrlyt)
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
            else if (tabControl.SelectedTab == tabBrlan)
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
        }

        private void lbStatusText_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lbStatusText.Text) && pbProgress.Value == 100)
            {
                if (tabControl.SelectedTab == tabBanner)
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
                else if (tabControl.SelectedTab == tabIcon)
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
                else if (tabControl.SelectedTab == tabBrlyt)
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
                    }
                    catch { }
                }
                else if (tabControl.SelectedTab == tabBrlan)
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
                    }
                    catch { }
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

        private void MakeBannerTplsTransparent()
        {
            foreach (string thisTpl in lbxBannerTpls.Items)
            {
                if (thisTpl.EndsWith("(Transparent)"))
                {
                    string Tpl = BannerTplPath + thisTpl.Replace(" (Transparent)", string.Empty);
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
                    string Tpl = IconTplPath + thisTpl.Replace(" (Transparent)", string.Empty);
                    byte[] TplArray = Wii.Tools.LoadFileToByteArray(Tpl);
                    int Width = Wii.TPL.GetTextureWidth(TplArray);
                    int Height = Wii.TPL.GetTextureHeight(TplArray);

                    Image Img = new Bitmap(Width, Height);
                    Wii.TPL.ConvertToTPL(Img, Tpl, 5);
                }
            }
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
                        else if (Ctrl is CheckBox && Ctrl == cbLz77) Ctrl.Enabled = true;
                        else if (Ctrl is ComboBox) Ctrl.Enabled = true;
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
                        else if (Ctrl is CheckBox) Ctrl.Enabled = false;
                        else if (Ctrl is ComboBox) Ctrl.Enabled = false;
                    }
                }
            }
        }

        private void UpdateCheck()
        {
            if (CheckInet() == true)
            {
                try
                {
                    WebClient GetVersion = new WebClient();
                    string NewVersion = GetVersion.DownloadString("http://customizemii.googlecode.com/svn/version.txt"); //TODO: Replace
                    
                    int newVersion = Convert.ToInt32(NewVersion.Replace(".", string.Empty).Length == 2 ? (NewVersion.Replace(".", string.Empty) + "0") : NewVersion.Replace(".", string.Empty));
                    int thisVersion = Convert.ToInt32(Version.Replace(".", string.Empty).Length == 2 ? (Version.Replace(".", string.Empty) + "0") : Version.Replace(".", string.Empty));

                    if (newVersion > thisVersion)
                    {
                        llbUpdateAvailabe.Text = llbUpdateAvailabe.Text.Replace("X", NewVersion);
                        llbUpdateAvailabe.Visible = true;

                        if (MessageBox.Show("Version " + NewVersion +
                            " is availabe.\nDo you want the download page to be opened?",
                            "Update availabe", MessageBoxButtons.YesNo, MessageBoxIcon.Information) ==
                            DialogResult.Yes)
                        {
                            Process.Start("http://code.google.com/p/customizemii/downloads/list");
                        }
                    }
                }
                catch { }
            }
        }

        private void llbSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                llbSite.LinkVisited = true;
                Process.Start("http://customizemii.googlecode.com");
            }
            catch (Exception ex) { ErrorBox(ex.Message); }
        }

        private void btnBrowseSource_Click(object sender, EventArgs e)
        {
            if (pbProgress.Value == 100)
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
        }

        void bwLoadChannel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pbProgress.Value = 100;
            lbStatusText.Text = string.Empty;
        }

        void bwLoadChannel_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressValue = e.ProgressPercentage;
            ProgressState = (string)e.UserState;

            this.Invoke(ProgressUpdate);
        }

        void bwLoadChannel_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker bwLoadChannel = sender as BackgroundWorker;
                EventHandler SetSourceWad = new EventHandler(this.SetSourceWad);
                byte[] WadFile = Wii.Tools.LoadFileToByteArray((string)e.Argument);

                if (Directory.Exists(TempUnpackPath)) Directory.Delete(TempUnpackPath, true);

                bwLoadChannel.ReportProgress(0, "Loading WAD...");
                Wii.WadUnpack.UnpackWad(WadFile, TempUnpackPath);
                if (Wii.U8.CheckU8(TempUnpackPath + "00000000.app") == false)
                    throw new Exception("CustomizeMii only edits Channel WADs!");

                this.Invoke(SetSourceWad);

                bwLoadChannel.ReportProgress(25, "Loading 00000000.app...");
                Wii.U8.UnpackU8(TempUnpackPath + "00000000.app", TempUnpackPath + "00000000.app_OUT");

                bwLoadChannel.ReportProgress(50, "Loading banner.bin...");
                Wii.U8.UnpackU8(TempUnpackPath + "00000000.app_OUT\\meta\\banner.bin", TempUnpackPath + "00000000.app_OUT\\meta\\banner.bin_OUT");

                bwLoadChannel.ReportProgress(75, "Loading icon.bin...");
                Wii.U8.UnpackU8(TempUnpackPath + "00000000.app_OUT\\meta\\icon.bin", TempUnpackPath + "00000000.app_OUT\\meta\\icon.bin_OUT");

                bwLoadChannel.ReportProgress(90, "Gathering Information...");
                string[] ChannelTitles = Wii.WadInfo.GetChannelTitles(WadFile);
                string TitleID = Wii.WadInfo.GetTitleID(WadFile, 1);

                SetText(tbAllLanguages, ChannelTitles[1]);

                if (ChannelTitles[0] != ChannelTitles[1])
                    SetText(tbJapanese, ChannelTitles[0]);
                if (ChannelTitles[2] != ChannelTitles[1])
                    SetText(tbGerman, ChannelTitles[2]);
                if (ChannelTitles[3] != ChannelTitles[1])
                    SetText(tbFrench, ChannelTitles[3]);
                if (ChannelTitles[4] != ChannelTitles[1])
                    SetText(tbSpanish, ChannelTitles[4]);
                if (ChannelTitles[5] != ChannelTitles[1])
                    SetText(tbItalian, ChannelTitles[5]);
                if (ChannelTitles[6] != ChannelTitles[1])
                    SetText(tbDutch, ChannelTitles[6]);

                SetText(tbTitleID, TitleID);

                EventHandler AddBannerTpls = new EventHandler(this.AddBannerTpls);
                EventHandler AddIconTpls = new EventHandler(this.AddIconTpls);
                this.Invoke(AddBannerTpls);
                this.Invoke(AddIconTpls);

                bwLoadChannel.ReportProgress(100);
                EventHandler EnableCtrls = new EventHandler(this.EnableControls);
                this.Invoke(EnableCtrls);
            }
            catch (Exception ex)
            {
                if (Directory.Exists(TempUnpackPath)) Directory.Delete(TempUnpackPath, true);
                SourceWad = string.Empty;
                ErrorBox(ex.Message);
            }
        }

        private void btnLoadBaseWad_Click(object sender, EventArgs e)
        {
            if (lbxBaseWads.SelectedIndex != -1)
            {
                if (CheckInet() == true)
                {
                    if (pbProgress.Value == 100)
                    {
                        try
                        {
                            SourceWad = SourceWadUrls[lbxBaseWads.SelectedIndex];
                            tbSourceWad.Text = SourceWad;
                            WebClient Client = new WebClient();
                            Client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(Client_DownloadProgressChanged);
                            Client.DownloadFileCompleted += new AsyncCompletedEventHandler(Client_DownloadFileCompleted);

                            lbStatusText.Text = "Downloading Base WAD...";
                            pbProgress.Value = 0;
                            if (!Directory.Exists(TempWadPath.Remove(TempWadPath.LastIndexOf('\\'))))
                                Directory.CreateDirectory(TempWadPath.Remove(TempWadPath.LastIndexOf('\\')));
                            Client.DownloadFileAsync(new Uri(SourceWad), TempWadPath);
                        }
                        catch (Exception ex)
                        {
                            tbSourceWad.Text = string.Empty;
                            ErrorBox(ex.Message);
                        }
                    }

                }
                else
                {
                    ErrorBox("You're not connected to the Internet!");
                }
            }
        }

        void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            pbProgress.Value = e.ProgressPercentage;
        }

        void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            BackgroundWorker bwLoadChannel = new BackgroundWorker();
            bwLoadChannel.DoWork += new DoWorkEventHandler(bwLoadChannel_DoWork);
            bwLoadChannel.ProgressChanged += new ProgressChangedEventHandler(bwLoadChannel_ProgressChanged);
            bwLoadChannel.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoadChannel_RunWorkerCompleted);
            bwLoadChannel.WorkerReportsProgress = true;
            bwLoadChannel.RunWorkerAsync(TempWadPath);
        }

        private void btnPreviewBaseWad_Click(object sender, EventArgs e)
        {
            if (lbxBaseWads.SelectedIndex != -1)
            {
                if (CheckInet() == true)
                {
                    if (!string.IsNullOrEmpty(SourceWadPreviewUrls[lbxBaseWads.SelectedIndex]))
                    {
                        try
                        {
                            Process.Start(SourceWadPreviewUrls[lbxBaseWads.SelectedIndex]);
                        }
                        catch (Exception ex) { ErrorBox(ex.Message); }
                    }
                    else
                        InfoBox("There's no preview of this channel availabe, sorry!");
                }
                else
                {
                    ErrorBox("You're not connected to the Internet!");
                }
            }
        }

        private void btnSaveBaseWad_Click(object sender, EventArgs e)
        {
            if (lbxBaseWads.SelectedIndex != -1)
            {
                if (CheckInet() == true)
                {
                    if (pbProgress.Value == 100)
                    {
                        string Url = SourceWadUrls[lbxBaseWads.SelectedIndex];
                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.Filter = "Wii Channels|*.wad";
                        sfd.FileName = Url.Remove(0, Url.LastIndexOf('/') + 1);

                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            try
                            {
                                WebClient SaveClient = new WebClient();
                                SaveClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(SaveClient_DownloadProgressChanged);
                                SaveClient.DownloadFileCompleted += new AsyncCompletedEventHandler(SaveClient_DownloadFileCompleted);

                                lbStatusText.Text = "Downloading Base WAD...";
                                pbProgress.Value = 0;
                                SaveClient.DownloadFileAsync(new Uri(Url), sfd.FileName);
                            }
                            catch (Exception ex)
                            {
                                ErrorBox(ex.Message);
                            }
                        }
                    }

                }
                else
                {
                    ErrorBox("You're not connected to the Internet!");
                }
            }
        }

        void SaveClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            pbProgress.Value = e.ProgressPercentage;
        }

        void SaveClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            lbStatusText.Text = string.Empty;
            pbProgress.Value = 100;
        }

        private void cmbReplace_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbReplace.SelectedIndex)
            {
                case 1:
                    tbReplace.Text = IconReplace;
                    break;
                case 2:
                    tbReplace.Text = SoundReplace;
                    break;
                default:
                    tbReplace.Text = BannerReplace;
                    break;
            }
        }

        private void btnBrowseReplace_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbSourceWad.Text)){
                if (pbProgress.Value == 100)
                {
                    if (cmbReplace.SelectedIndex == 2) //sound
                    {
                        try
                        {
                            OpenFileDialog ofd = new OpenFileDialog();
                            ofd.Filter = "Wii Channels|*.wad|00000000.app|00000000.app|sound.bin|sound.bin|All|*.wad;00000000.app;sound.bin";
                            ofd.FilterIndex = 4;

                            if (ofd.ShowDialog() == DialogResult.OK)
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
                            OpenFileDialog ofd = new OpenFileDialog();
                            ofd.Filter = "Wii Channels|*.wad|00000000.app|00000000.app|icon.bin|icon.bin|All|*.wad;00000000.app;icon.bin";
                            ofd.FilterIndex = 4;

                            if (ofd.ShowDialog() == DialogResult.OK)
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
                            OpenFileDialog ofd = new OpenFileDialog();
                            ofd.Filter = "Wii Channels|*.wad|00000000.app|00000000.app|banner.bin|banner.bin|All|*.wad;00000000.app;banner.bin";
                            ofd.FilterIndex = 4;

                            if (ofd.ShowDialog() == DialogResult.OK)
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

        void bwBannerReplace_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pbProgress.Value = 100;
            lbStatusText.Text = string.Empty;
            SetText(tbReplace, BannerReplace);
        }

        void bwBannerReplace_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressValue = e.ProgressPercentage;
            ProgressState = (string)e.UserState;

            this.Invoke(ProgressUpdate);
        }

        void bwBannerReplace_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker bwBannerReplace = sender as BackgroundWorker;
                string thisFile = (string)e.Argument;
                if (Directory.Exists(TempTempPath)) Directory.Delete(TempTempPath, true);
                if (Directory.Exists(TempBannerPath)) Directory.Delete(TempBannerPath, true);

                if (thisFile.EndsWith(".bin"))
                {
                    bwBannerReplace.ReportProgress(0, "Loading banner.bin...");
                    Wii.U8.UnpackU8(thisFile, TempBannerPath);
                }
                else if (thisFile.EndsWith(".app"))
                {
                    bwBannerReplace.ReportProgress(0, "Loading 00000000.app...");
                    Wii.U8.UnpackU8(thisFile, TempTempPath);
                    bwBannerReplace.ReportProgress(50, "Loading banner.bin...");
                    Wii.U8.UnpackU8(TempTempPath + "meta\\banner.bin", TempBannerPath);
                    Directory.Delete(TempTempPath, true);
                }
                else
                {
                    bwBannerReplace.ReportProgress(0, "Loading WAD...");
                    Wii.WadUnpack.UnpackWad(thisFile, TempTempPath);
                    if (Wii.U8.CheckU8(TempTempPath + "00000000.app") == false)
                        throw new Exception("CustomizeMii only handles Channel WADs!");
                    bwBannerReplace.ReportProgress(30, "Loading 00000000.app...");
                    Wii.U8.UnpackU8(TempTempPath + "00000000.app", TempTempPath + "00000000.app_OUT");
                    bwBannerReplace.ReportProgress(60, "Loading banner.bin...");
                    Wii.U8.UnpackU8(TempTempPath + "00000000.app_OUT\\meta\\banner.bin", TempBannerPath);
                    Directory.Delete(TempTempPath, true);
                }

                EventHandler AddBannerTpls = new EventHandler(this.AddBannerTpls);
                EventHandler AddIconTpls = new EventHandler(this.AddIconTpls);
                this.Invoke(AddBannerTpls);
                this.Invoke(AddIconTpls);
            }
            catch (Exception ex)
            {
                if (Directory.Exists(TempTempPath)) Directory.Delete(TempTempPath, true);
                if (Directory.Exists(TempBannerPath)) Directory.Delete(TempBannerPath, true);
                BannerReplace = string.Empty;
                ErrorBox(ex.Message);
            }
        }

        void bwIconReplace_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pbProgress.Value = 100;
            lbStatusText.Text = string.Empty;
            SetText(tbReplace, IconReplace);
        }

        void bwIconReplace_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressValue = e.ProgressPercentage;
            ProgressState = (string)e.UserState;

            this.Invoke(ProgressUpdate);
        }

        void bwIconReplace_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker bwIconReplace = sender as BackgroundWorker;
                string thisFile = (string)e.Argument;
                if (Directory.Exists(TempTempPath)) Directory.Delete(TempTempPath, true);
                if (Directory.Exists(TempIconPath)) Directory.Delete(TempIconPath, true);

                if (thisFile.EndsWith(".bin"))
                {
                    bwIconReplace.ReportProgress(0, "Loading icon.bin...");
                    Wii.U8.UnpackU8(thisFile, TempIconPath);
                }
                else if (thisFile.EndsWith(".app"))
                {
                    bwIconReplace.ReportProgress(0, "Loading 00000000.app...");
                    Wii.U8.UnpackU8(thisFile, TempTempPath);
                    bwIconReplace.ReportProgress(50, "Loading icon.bin...");
                    Wii.U8.UnpackU8(TempTempPath + "meta\\icon.bin", TempIconPath);
                    Directory.Delete(TempTempPath, true);
                }
                else
                {
                    bwIconReplace.ReportProgress(0, "Loading WAD...");
                    Wii.WadUnpack.UnpackWad(thisFile, TempTempPath);
                    if (Wii.U8.CheckU8(TempTempPath + "00000000.app") == false)
                        throw new Exception("CustomizeMii only handles Channel WADs!");
                    bwIconReplace.ReportProgress(30, "Loading 00000000.app...");
                    Wii.U8.UnpackU8(TempTempPath + "00000000.app", TempTempPath + "00000000.app_OUT");
                    bwIconReplace.ReportProgress(60, "Loading icon.bin...");
                    Wii.U8.UnpackU8(TempTempPath + "00000000.app_OUT\\meta\\icon.bin", TempIconPath);
                    Directory.Delete(TempTempPath, true);
                }

                EventHandler AddBannerTpls = new EventHandler(this.AddBannerTpls);
                EventHandler AddIconTpls = new EventHandler(this.AddIconTpls);
                this.Invoke(AddBannerTpls);
                this.Invoke(AddIconTpls);
            }
            catch (Exception ex)
            {
                if (Directory.Exists(TempTempPath)) Directory.Delete(TempTempPath, true);
                if (Directory.Exists(TempIconPath)) Directory.Delete(TempIconPath, true);
                IconReplace = string.Empty;
                ErrorBox(ex.Message);
            }
        }

        void bwSoundReplace_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pbProgress.Value = 100;
            lbStatusText.Text = string.Empty;
            SetText(tbReplace, SoundReplace);
        }

        void bwSoundReplace_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressValue = e.ProgressPercentage;
            ProgressState = (string)e.UserState;

            this.Invoke(ProgressUpdate);
        }

        void bwSoundReplace_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker bwSoundReplace = sender as BackgroundWorker;
                string thisFile = (string)e.Argument;
                if (File.Exists(TempSoundPath)) File.Delete(TempSoundPath);
                if (Directory.Exists(TempTempPath)) Directory.Delete(TempTempPath, true);

                if (thisFile.EndsWith(".bin"))
                {
                    bwSoundReplace.ReportProgress(0, "Copying sound.bin...");
                    File.Copy(thisFile, TempSoundPath);
                }
                else if (thisFile.EndsWith(".app"))
                {
                    bwSoundReplace.ReportProgress(0, "Loading 00000000.app...");
                    Wii.U8.UnpackU8(thisFile, TempTempPath);
                    bwSoundReplace.ReportProgress(80, "Copying sound.bin...");
                    File.Copy(TempTempPath + "meta\\sound.bin", TempSoundPath);
                    Directory.Delete(TempTempPath, true);
                }
                else
                {
                    bwSoundReplace.ReportProgress(0, "Loading WAD...");
                    Wii.WadUnpack.UnpackWad(thisFile, TempTempPath);
                    if (Wii.U8.CheckU8(TempTempPath + "00000000.app") == false)
                        throw new Exception("CustomizeMii only handles Channel WADs!");
                    bwSoundReplace.ReportProgress(50, "Loading 00000000.app...");
                    Wii.U8.UnpackU8(TempTempPath + "00000000.app", TempTempPath + "00000000.app_OUT");
                    bwSoundReplace.ReportProgress(90, "Copying sound.bin...");
                    File.Copy(TempTempPath + "00000000.app_OUT\\meta\\sound.bin", TempSoundPath);
                    Directory.Delete(TempTempPath, true);
                }

                SetText(tbSound, SoundReplace);
            }
            catch (Exception ex)
            {
                if (Directory.Exists(TempTempPath)) Directory.Delete(TempTempPath, true);
                if (File.Exists(TempSoundPath)) File.Delete(TempSoundPath);
                SoundReplace = string.Empty;
                ErrorBox(ex.Message);
            }
        }

        private void btnClearReplace_Click(object sender, EventArgs e)
        {
            if (cmbReplace.SelectedIndex == 2) //sound
            {
                SoundReplace = string.Empty;
                SetText(tbReplace, SoundReplace);
                if (File.Exists(TempSoundPath)) File.Delete(TempSoundPath);
                SetText(tbSound, string.Empty);
            }
            else if (cmbReplace.SelectedIndex == 1) //icon
            {
                IconReplace = string.Empty;
                SetText(tbReplace, IconReplace);
                if (Directory.Exists(TempIconPath)) Directory.Delete(TempIconPath, true);
            }
            else //banner
            {
                BannerReplace = string.Empty;
                SetText(tbReplace, BannerReplace);
                if (Directory.Exists(TempBannerPath)) Directory.Delete(TempBannerPath, true);
            }
        }

        private void btnBrowseDol_Click(object sender, EventArgs e)
        {
            if (btnBrowseDol.Text == "Clear")
            {
                SetText(tbDol, string.Empty);
                btnBrowseDol.Text = "Browse...";
            }
            else
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "DOL Files|*.dol";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    SetText(tbDol, ofd.FileName);
                    btnBrowseDol.Text = "Clear";
                }
            }
        }

        private void btnBrowseSound_Click(object sender, EventArgs e)
        {
            if (btnBrowseSound.Text == "Clear")
            {
                SetText(tbSound, string.Empty);
                btnBrowseSound.Text = "Browse...";
            }
            else
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Wave Files|*.wav";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    SetText(tbSound, ofd.FileName);
                    btnBrowseSound.Text = "Clear";

                    if (!string.IsNullOrEmpty(SoundReplace))
                    {
                        SoundReplace = string.Empty;
                        if (cmbReplace.SelectedIndex == 2) SetText(tbReplace, SoundReplace);
                        if (File.Exists(TempSoundPath)) File.Delete(TempSoundPath);
                    }
                }
            }
        }

        private void lbxBannerTpls_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxBannerTpls.SelectedIndex != -1)
            {
                try
                {
                    cbBannerMakeTransparent.Enabled = true;
                    cbBannerMakeTransparent.Checked = lbxBannerTpls.SelectedItem.ToString().EndsWith("(Transparent)");

                    string TplFile = BannerTplPath + lbxBannerTpls.SelectedItem.ToString().Replace(" (Transparent)", string.Empty);
                    int TplFormat = Wii.TPL.GetTextureFormat(TplFile);

                    switch (TplFormat)
                    {
                        case 0:
                            cmbFormatBanner.SelectedIndex = 6;
                            break;
                        case 1:
                            cmbFormatBanner.SelectedIndex = 5;
                            break;
                        case 2:
                            cmbFormatBanner.SelectedIndex = 4;
                            break;
                        case 3:
                            cmbFormatBanner.SelectedIndex = 3;
                            break;
                        case 4:
                            cmbFormatBanner.SelectedIndex = 1;
                            break;
                        case 5:
                            cmbFormatBanner.SelectedIndex = 2;
                            break;
                        case 6:
                            cmbFormatBanner.SelectedIndex = 0;
                            break;
                        case 14:
                            cmbFormatBanner.SelectedIndex = 7;
                            break;
                        default:
                            cmbFormatBanner.SelectedIndex = -1;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    ErrorBox(ex.Message);
                }
            }
            else
                cbBannerMakeTransparent.Enabled = false;
        }

        private void lbxIconTpls_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxIconTpls.SelectedIndex != -1)
            {
                try
                {
                    cbIconMakeTransparent.Enabled = true;
                    cbIconMakeTransparent.Checked = lbxIconTpls.SelectedItem.ToString().EndsWith("(Transparent)");

                    string TplFile = IconTplPath + lbxIconTpls.SelectedItem.ToString().Replace(" (Transparent)", string.Empty);
                    int TplFormat = Wii.TPL.GetTextureFormat(TplFile);

                    switch (TplFormat)
                    {
                        case 0:
                            cmbFormatIcon.SelectedIndex = 6;
                            break;
                        case 1:
                            cmbFormatIcon.SelectedIndex = 5;
                            break;
                        case 2:
                            cmbFormatIcon.SelectedIndex = 4;
                            break;
                        case 3:
                            cmbFormatIcon.SelectedIndex = 3;
                            break;
                        case 4:
                            cmbFormatIcon.SelectedIndex = 1;
                            break;
                        case 5:
                            cmbFormatIcon.SelectedIndex = 2;
                            break;
                        case 6:
                            cmbFormatIcon.SelectedIndex = 0;
                            break;
                        case 14:
                            cmbFormatIcon.SelectedIndex = 7;
                            break;
                        default:
                            cmbFormatIcon.SelectedIndex = -1;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    ErrorBox(ex.Message);
                }
            }
            else
                cbIconMakeTransparent.Enabled = false;
        }

        private void btnReplaceBanner_Click(object sender, EventArgs e)
        {
            if (lbxBannerTpls.SelectedIndex != -1)
            {
                int Format = cmbFormatBanner.SelectedIndex;

                if (Format == 0 || Format == 1 || Format == 2)
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Filter = "Png|*.png|JPG|*.jpg|GIF|*.gif|BMP|*.bmp|TPL|*.tpl|All|*.png;*.jpg;*.gif;*.bmp;*.tpl";
                    ofd.FilterIndex = 6;

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            string Tpl = BannerTplPath + lbxBannerTpls.SelectedItem.ToString().Replace(" (Transparent)", string.Empty);
                            byte[] TplArray = Wii.Tools.LoadFileToByteArray(Tpl);
                            Image Img;

                            if (!ofd.FileName.EndsWith(".tpl")) Img = Image.FromFile(ofd.FileName);
                            else Img = Wii.TPL.ConvertFromTPL(ofd.FileName);

                            int TplFormat;
                            int X = Wii.TPL.GetTextureWidth(TplArray);
                            int Y = Wii.TPL.GetTextureHeight(TplArray);

                            if (X != Img.Width ||
                                Y != Img.Height)
                            {
                                Img = ResizeImage(Img, X, Y);
                            }

                            switch (Format)
                            {
                                case 1:
                                    TplFormat = 4;
                                    break;
                                case 2:
                                    TplFormat = 5;
                                    break;
                                default:
                                    TplFormat = 6;
                                    break;
                            }

                            Wii.TPL.ConvertToTPL(Img, Tpl, TplFormat);
                        }
                        catch (Exception ex)
                        {
                            ErrorBox(ex.Message);
                        }
                    }
                }
                else
                {
                    ErrorBox("This format is not supported, you must choose either RGBA8, RGB565 or RGB5A3!");
                }
            }
        }

        private void btnExtractBanner_Click(object sender, EventArgs e)
        {
            if (lbxBannerTpls.SelectedIndex != -1)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Png|*.png|JPG|*.jpg|GIF|*.gif|BMP|*.bmp|TPL|*.tpl|All|*.png;*.jpg;*.gif;*.bmp;*.tpl";
                sfd.FilterIndex = 6;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (!sfd.FileName.EndsWith(".tpl"))
                        {
                            string Tpl = BannerTplPath + lbxBannerTpls.SelectedItem.ToString().Replace(" (Transparent)", string.Empty);
                            Image Img = Wii.TPL.ConvertFromTPL(Tpl);

                            switch (sfd.FileName.Remove(0, sfd.FileName.LastIndexOf('.')))
                            {
                                case ".jpg":
                                    Img.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    break;
                                case ".gif":
                                    Img.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Gif);
                                    break;
                                case ".bmp":
                                    Img.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                                    break;
                                default:
                                    Img.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png);
                                    break;
                            }
                        }
                        else
                        {
                            string Tpl = BannerTplPath + lbxBannerTpls.SelectedItem.ToString().Replace(" (Transparent)", string.Empty);
                            File.Copy(Tpl, sfd.FileName, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorBox(ex.Message);
                    }
                }
            }
        }

        private void btnPreviewBanner_Click(object sender, EventArgs e)
        {
            if (lbxBannerTpls.SelectedIndex != -1)
            {
                try
                {
                    string Tpl = BannerTplPath + lbxBannerTpls.SelectedItem.ToString().Replace(" (Transparent)", string.Empty);
                    Image Img = Wii.TPL.ConvertFromTPL(Tpl);

                    CustomizeMii_Preview pvw = new CustomizeMii_Preview();

                    if (Img.Width > 200) pvw.Width = Img.Width + 50;
                    else pvw.Width = 250;
                    if (Img.Height > 200) pvw.Height = Img.Height + 50;
                    else pvw.Height = 250;

                    pvw.pbImage.Image = Img;
                    pvw.Text = "CustomizeMii - Preview (" + Img.Width.ToString() + " x " + Img.Height.ToString() + ")";

                    pvw.ShowDialog();
                }
                catch (Exception ex)
                {
                    ErrorBox(ex.Message);
                }
            }
        }

        private void btnReplaceIcon_Click(object sender, EventArgs e)
        {
            if (lbxIconTpls.SelectedIndex != -1)
            {
                int Format = cmbFormatIcon.SelectedIndex;

                if (Format == 0 || Format == 1 || Format == 2)
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Filter = "Png|*.png|JPG|*.jpg|GIF|*.gif|BMP|*.bmp|TPL|*.tpl|All|*.png;*.jpg;*.gif;*.bmp;*.tpl";
                    ofd.FilterIndex = 6;

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            string Tpl = IconTplPath + lbxIconTpls.SelectedItem.ToString().Replace(" (Transparent)", string.Empty);
                            byte[] TplArray = Wii.Tools.LoadFileToByteArray(Tpl);
                            Image Img;

                            if (!ofd.FileName.EndsWith(".tpl")) Img = Image.FromFile(ofd.FileName);
                            else Img = Wii.TPL.ConvertFromTPL(ofd.FileName);

                            int TplFormat;
                            int X = Wii.TPL.GetTextureWidth(TplArray);
                            int Y = Wii.TPL.GetTextureHeight(TplArray);

                            if (X != Img.Width ||
                                Y != Img.Height)
                            {
                                Img = ResizeImage(Img, X, Y);
                            }

                            switch (Format)
                            {
                                case 1:
                                    TplFormat = 4;
                                    break;
                                case 2:
                                    TplFormat = 5;
                                    break;
                                default:
                                    TplFormat = 6;
                                    break;
                            }

                            Wii.TPL.ConvertToTPL(Img, Tpl, TplFormat);
                        }
                        catch (Exception ex)
                        {
                            ErrorBox(ex.Message);
                        }
                    }
                }
                else
                {
                    ErrorBox("This format is not supported, you must choose either RGBA8, RGB565 or RGB5A3!");
                }
            }
        }

        private void btnExtractIcon_Click(object sender, EventArgs e)
        {
            if (lbxIconTpls.SelectedIndex != -1)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Png|*.png|JPG|*.jpg|GIF|*.gif|BMP|*.bmp|TPL|*.tpl|All|*.png;*.jpg;*.gif;*.bmp;*.tpl";
                sfd.FilterIndex = 6;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (!sfd.FileName.EndsWith(".tpl"))
                        {
                            string Tpl = IconTplPath + lbxIconTpls.SelectedItem.ToString().Replace(" (Transparent)", string.Empty);
                            Image Img = Wii.TPL.ConvertFromTPL(Tpl);

                            switch (sfd.FileName.Remove(0, sfd.FileName.LastIndexOf('.')))
                            {
                                case ".jpg":
                                    Img.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    break;
                                case ".gif":
                                    Img.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Gif);
                                    break;
                                case ".bmp":
                                    Img.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                                    break;
                                default:
                                    Img.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png);
                                    break;
                            }
                        }
                        else
                        {
                            string Tpl = IconTplPath + lbxIconTpls.SelectedItem.ToString().Replace(" (Transparent)", string.Empty);
                            File.Copy(Tpl, sfd.FileName, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorBox(ex.Message);
                    }
                }
            }
        }

        private void btnPreviewIcon_Click(object sender, EventArgs e)
        {
            if (lbxIconTpls.SelectedIndex != -1)
            {
                try
                {
                    string Tpl = IconTplPath + lbxIconTpls.SelectedItem.ToString().Replace(" (Transparent)", string.Empty);
                    Image Img = Wii.TPL.ConvertFromTPL(Tpl);

                    CustomizeMii_Preview pvw = new CustomizeMii_Preview();

                    if (Img.Width > 200) pvw.Width = Img.Width + 50;
                    else pvw.Width = 250;
                    if (Img.Height > 200) pvw.Height = Img.Height + 50;
                    else pvw.Height = 250;

                    pvw.pbImage.Image = Img;
                    pvw.Text = "CustomizeMii - Preview (" + Img.Width.ToString() + " x " + Img.Height.ToString() + ")";

                    pvw.ShowDialog();
                }
                catch (Exception ex)
                {
                    ErrorBox(ex.Message);
                }
            }
        }

        private bool FailureCheck()
        {
            try
            {
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
                string[] ValidBrlans = new string[] { "banner.brlan", "icon.brlan", "banner_Loop.brlan", "icon_Loop.brlan", "banner_Start.brlan", "icon_Start.brlan" };
                foreach (string thisBrlan in lbxBrlanBanner.Items)
                {
                    if (!Wii.Tools.StringExistsInStringArray(thisBrlan, ValidBrlans))
                    {
                        ErrorBox(thisBrlan + " is not a valid brlan filename!");
                        return false;
                    }
                }
                foreach (string thisBrlan in lbxBrlanIcon.Items)
                {
                    if (!Wii.Tools.StringExistsInStringArray(thisBrlan, ValidBrlans))
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
                string[] IconBrlytPath;

                if (string.IsNullOrEmpty(BannerReplace))
                    BannerBrlytPath = Directory.GetFiles(TempUnpackBannerBrlytPath);
                else
                    BannerBrlytPath = Directory.GetFiles(TempBannerPath + "arc\\blyt");
                if (string.IsNullOrEmpty(IconReplace))
                    IconBrlytPath = Directory.GetFiles(TempUnpackIconBrlytPath);
                else
                    IconBrlytPath = Directory.GetFiles(TempIconPath + "arc\\blyt");

                string[] BannerMissing;
                string[] BannerUnused;
                string[] IconMissing;
                string[] IconUnused;

                //Check for missing TPLs
                if (Wii.Brlyt.CheckForMissingTpls(BannerBrlytPath[0], BannerTpls.ToArray(), out BannerMissing) == true)
                {
                    ErrorBox("The following Banner TPLs are required by the banner.brlyt, but missing:\n\n" + string.Join("\n", BannerMissing));
                    return false;
                }
                if (Wii.Brlyt.CheckForMissingTpls(IconBrlytPath[0], IconTpls.ToArray(), out IconMissing) == true)
                {
                    ErrorBox("The following Icon TPLs are required by the icon.brlyt, but missing:\n\n" + string.Join("\n", IconMissing));
                    return false;
                }

                //Check Sound length
                int soundlength = -1;
                if (!string.IsNullOrEmpty(tbSound.Text) && string.IsNullOrEmpty(SoundReplace))
                {
                    soundlength = Wii.Sound.GetWaveLength(tbSound.Text);
                    if (soundlength > SoundMaxLength)
                    {
                        ErrorBox(string.Format("Your Sound is longer than {0} seconds and thus not supported.\nIt is recommended to use a Sound shorter than {1} seconds, the maximum length is {0} seconds!", SoundMaxLength, SoundWarningLength));
                        return false;
                    }
                }

                /*Errors till here..
                  From here only Warnings!*/

                if (soundlength != -1)
                {
                    if (soundlength > SoundWarningLength)
                    {
                        if (MessageBox.Show(string.Format("Your Sound is longer than {0} seconds.\nIt is recommended to use Sounds that are shorter than {0} seconds!\nDo you still want to continue?", SoundWarningLength), "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
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
                if (Wii.Brlyt.CheckForUnusedTpls(BannerBrlytPath[0], BannerTpls.ToArray(), out BannerUnused) == true)
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
                if (Wii.Brlyt.CheckForUnusedTpls(IconBrlytPath[0], IconTpls.ToArray(), out IconUnused) == true)
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

                return true;
            }
            catch (Exception ex)
            {
                ErrorBox(ex.Message);
                return false;
            }
        }

        private bool CheckUnpackFolder()
        {
            try
            {
                //Check Unpack Root
                string[] RootFiles = Directory.GetFiles(TempUnpackPath);
                string[] RootDirs = Directory.GetDirectories(TempUnpackPath);

                foreach (string thisFile in RootFiles)
                {
                    if (!thisFile.EndsWith(".app") &&
                        !thisFile.EndsWith(".cert") &&
                        !thisFile.EndsWith(".tik") &&
                        !thisFile.EndsWith(".tmd"))
                        File.Delete(thisFile);
                }

                if (RootDirs.Length > 1)
                {
                    foreach (string thisDir in RootDirs)
                    {
                        if (!thisDir.EndsWith("00000000.app_OUT"))
                            Directory.Delete(thisDir, true);
                    }
                }

                //Check 00000000.app_OUT
                string[] MetaFiles = Directory.GetFiles(TempUnpackPath + "00000000.app_OUT");
                string[] MetaDirs = Directory.GetDirectories(TempUnpackPath + "00000000.app_OUT");

                foreach (string thisFile in MetaFiles)
                    File.Delete(thisFile);
                foreach (string thisDir in MetaDirs)
                    if (!thisDir.EndsWith("meta"))
                        Directory.Delete(thisDir, true);

                string[] AppFiles = Directory.GetFiles(TempUnpackPath + "00000000.app_OUT\\meta");
                string[] AppDirs = Directory.GetDirectories(TempUnpackPath + "00000000.app_OUT\\meta");

                foreach (string thisFile in AppFiles)
                {
                    if (!thisFile.EndsWith("banner.bin") &&
                        !thisFile.EndsWith("icon.bin") &&
                        !thisFile.EndsWith("sound.bin"))
                        File.Delete(thisFile);
                }
                
                if (AppDirs.Length > 2)
                {
                    foreach (string thisDir in AppDirs)
                    {
                        if (!thisDir.EndsWith("banner.bin_OUT") &&
                            !thisDir.EndsWith("icon.bin_OUT"))
                            Directory.Delete(thisDir, true);
                    }
                }
                
                //Check banner.bin_OUT / Banner Replace Path
                if (string.IsNullOrEmpty(BannerReplace))
                {
                    string[] ArcFiles = Directory.GetFiles(TempUnpackPath + "00000000.app_OUT\\meta\\banner.bin_OUT");
                    string[] ArcDirs = Directory.GetDirectories(TempUnpackPath + "00000000.app_OUT\\meta\\banner.bin_OUT");

                    foreach (string thisFile in ArcFiles)
                        File.Delete(thisFile);
                    foreach (string thisDir in ArcDirs)
                        if (!thisDir.EndsWith("arc"))
                            Directory.Delete(thisDir, true);

                    string[] BannerFiles = Directory.GetFiles(TempUnpackPath + "00000000.app_OUT\\meta\\banner.bin_OUT\\arc");
                    string[] BannerDirs = Directory.GetDirectories(TempUnpackPath + "00000000.app_OUT\\meta\\banner.bin_OUT\\arc");

                    foreach (string thisFile in BannerFiles)
                        File.Delete(thisFile);

                    if (BannerDirs.Length > 3)
                    {
                        foreach (string thisDir in BannerDirs)
                        {
                            if (!thisDir.EndsWith("anim") &&
                                !thisDir.EndsWith("blyt") &&
                                !thisDir.EndsWith("timg"))
                                Directory.Delete(thisDir, true);
                        }
                    }

                    string[] AnimFiles = Directory.GetFiles(TempUnpackPath + "00000000.app_OUT\\meta\\banner.bin_OUT\\arc\\anim");
                    string[] AnimDirs = Directory.GetDirectories(TempUnpackPath + "00000000.app_OUT\\meta\\banner.bin_OUT\\arc\\anim");

                    foreach (string thisFile in AnimFiles)
                    {
                        if (!thisFile.EndsWith(".brlan"))
                            File.Delete(thisFile);
                    }

                    foreach (string thisDir in AnimDirs)
                        Directory.Delete(thisDir, true);

                    string[] BlytFiles = Directory.GetFiles(TempUnpackPath + "00000000.app_OUT\\meta\\banner.bin_OUT\\arc\\blyt");
                    string[] BlytDirs = Directory.GetDirectories(TempUnpackPath + "00000000.app_OUT\\meta\\banner.bin_OUT\\arc\\blyt");

                    foreach (string thisFile in BlytFiles)
                    {
                        if (!thisFile.EndsWith(".brlyt"))
                            File.Delete(thisFile);
                    }

                    foreach (string thisDir in BlytDirs)
                        Directory.Delete(thisDir, true);

                    string[] TimgFiles = Directory.GetFiles(TempUnpackPath + "00000000.app_OUT\\meta\\banner.bin_OUT\\arc\\timg");
                    string[] TimgDirs = Directory.GetDirectories(TempUnpackPath + "00000000.app_OUT\\meta\\banner.bin_OUT\\arc\\timg");

                    foreach (string thisFile in TimgFiles)
                    {
                        if (!thisFile.EndsWith(".tpl"))
                            File.Delete(thisFile);
                    }

                    foreach (string thisDir in TimgDirs)
                        Directory.Delete(thisDir, true);
                }
                else
                {
                    string[] ArcFiles = Directory.GetFiles(TempBannerPath);
                    string[] ArcDirs = Directory.GetDirectories(TempBannerPath);

                    foreach (string thisFile in ArcFiles)
                        File.Delete(thisFile);
                    foreach (string thisDir in ArcDirs)
                        if (!thisDir.EndsWith("arc"))
                            Directory.Delete(thisDir, true);

                    string[] BannerFiles = Directory.GetFiles(TempBannerPath + "arc");
                    string[] BannerDirs = Directory.GetDirectories(TempBannerPath + "arc");

                    foreach (string thisFile in BannerFiles)
                        File.Delete(thisFile);

                    if (BannerDirs.Length > 3)
                    {
                        foreach (string thisDir in BannerDirs)
                        {
                            if (!thisDir.EndsWith("anim") &&
                                !thisDir.EndsWith("blyt") &&
                                !thisDir.EndsWith("timg"))
                                Directory.Delete(thisDir, true);
                        }
                    }

                    string[] AnimFiles = Directory.GetFiles(TempBannerPath + "arc\\anim");
                    string[] AnimDirs = Directory.GetDirectories(TempBannerPath + "arc\\anim");

                    foreach (string thisFile in AnimFiles)
                    {
                        if (!thisFile.EndsWith(".brlan"))
                            File.Delete(thisFile);
                    }

                    foreach (string thisDir in AnimDirs)
                        Directory.Delete(thisDir, true);

                    string[] BlytFiles = Directory.GetFiles(TempBannerPath + "arc\\blyt");
                    string[] BlytDirs = Directory.GetDirectories(TempBannerPath + "arc\\blyt");

                    foreach (string thisFile in BlytFiles)
                    {
                        if (!thisFile.EndsWith(".brlyt"))
                            File.Delete(thisFile);
                    }

                    foreach (string thisDir in BlytDirs)
                        Directory.Delete(thisDir, true);

                    string[] TimgFiles = Directory.GetFiles(TempBannerPath + "arc\\timg");
                    string[] TimgDirs = Directory.GetDirectories(TempBannerPath + "arc\\timg");

                    foreach (string thisFile in TimgFiles)
                    {
                        if (!thisFile.EndsWith(".tpl"))
                            File.Delete(thisFile);
                    }

                    foreach (string thisDir in TimgDirs)
                        Directory.Delete(thisDir, true);
                }

                //Check icon.bin_OUT / Icon Replace Path
                if (string.IsNullOrEmpty(IconReplace))
                {
                    string[] ArcFiles = Directory.GetFiles(TempUnpackPath + "00000000.app_OUT\\meta\\icon.bin_OUT");
                    string[] ArcDirs = Directory.GetDirectories(TempUnpackPath + "00000000.app_OUT\\meta\\icon.bin_OUT");

                    foreach (string thisFile in ArcFiles)
                        File.Delete(thisFile);
                    foreach (string thisDir in ArcDirs)
                        if (!thisDir.EndsWith("arc"))
                            Directory.Delete(thisDir, true);

                    string[] IconFiles = Directory.GetFiles(TempUnpackPath + "00000000.app_OUT\\meta\\icon.bin_OUT\\arc");
                    string[] IconDirs = Directory.GetDirectories(TempUnpackPath + "00000000.app_OUT\\meta\\icon.bin_OUT\\arc");

                    foreach (string thisFile in IconFiles)
                        File.Delete(thisFile);

                    if (IconDirs.Length > 3)
                    {
                        foreach (string thisDir in IconDirs)
                        {
                            if (!thisDir.EndsWith("anim") &&
                                !thisDir.EndsWith("blyt") &&
                                !thisDir.EndsWith("timg"))
                                Directory.Delete(thisDir, true);
                        }
                    }

                    string[] AnimFiles = Directory.GetFiles(TempUnpackPath + "00000000.app_OUT\\meta\\icon.bin_OUT\\arc\\anim");
                    string[] AnimDirs = Directory.GetDirectories(TempUnpackPath + "00000000.app_OUT\\meta\\icon.bin_OUT\\arc\\anim");

                    foreach (string thisFile in AnimFiles)
                    {
                        if (!thisFile.EndsWith(".brlan"))
                            File.Delete(thisFile);
                    }

                    foreach (string thisDir in AnimDirs)
                        Directory.Delete(thisDir, true);

                    string[] BlytFiles = Directory.GetFiles(TempUnpackPath + "00000000.app_OUT\\meta\\icon.bin_OUT\\arc\\blyt");
                    string[] BlytDirs = Directory.GetDirectories(TempUnpackPath + "00000000.app_OUT\\meta\\icon.bin_OUT\\arc\\blyt");

                    foreach (string thisFile in BlytFiles)
                    {
                        if (!thisFile.EndsWith(".brlyt"))
                            File.Delete(thisFile);
                    }

                    foreach (string thisDir in BlytDirs)
                        Directory.Delete(thisDir, true);

                    string[] TimgFiles = Directory.GetFiles(TempUnpackPath + "00000000.app_OUT\\meta\\icon.bin_OUT\\arc\\timg");
                    string[] TimgDirs = Directory.GetDirectories(TempUnpackPath + "00000000.app_OUT\\meta\\icon.bin_OUT\\arc\\timg");

                    foreach (string thisFile in TimgFiles)
                    {
                        if (!thisFile.EndsWith(".tpl"))
                            File.Delete(thisFile);
                    }

                    foreach (string thisDir in TimgDirs)
                        Directory.Delete(thisDir, true);
                }
                else
                {
                    string[] ArcFiles = Directory.GetFiles(TempIconPath);
                    string[] ArcDirs = Directory.GetDirectories(TempIconPath);

                    foreach (string thisFile in ArcFiles)
                        File.Delete(thisFile);
                    foreach (string thisDir in ArcDirs)
                        if (!thisDir.EndsWith("arc"))
                            Directory.Delete(thisDir, true);

                    string[] IconFiles = Directory.GetFiles(TempIconPath + "arc");
                    string[] IconDirs = Directory.GetDirectories(TempIconPath + "arc");

                    foreach (string thisFile in IconFiles)
                        File.Delete(thisFile);

                    if (IconDirs.Length > 3)
                    {
                        foreach (string thisDir in IconDirs)
                        {
                            if (!thisDir.EndsWith("anim") &&
                                !thisDir.EndsWith("blyt") &&
                                !thisDir.EndsWith("timg"))
                                Directory.Delete(thisDir, true);
                        }
                    }

                    string[] AnimFiles = Directory.GetFiles(TempIconPath + "arc\\anim");
                    string[] AnimDirs = Directory.GetDirectories(TempIconPath + "arc\\anim");

                    foreach (string thisFile in AnimFiles)
                    {
                        if (!thisFile.EndsWith(".brlan"))
                            File.Delete(thisFile);
                    }

                    foreach (string thisDir in AnimDirs)
                        Directory.Delete(thisDir, true);

                    string[] BlytFiles = Directory.GetFiles(TempIconPath + "arc\\blyt");
                    string[] BlytDirs = Directory.GetDirectories(TempIconPath + "arc\\blyt");

                    foreach (string thisFile in BlytFiles)
                    {
                        if (!thisFile.EndsWith(".brlyt"))
                            File.Delete(thisFile);
                    }

                    foreach (string thisDir in BlytDirs)
                        Directory.Delete(thisDir, true);

                    string[] TimgFiles = Directory.GetFiles(TempIconPath + "arc\\timg");
                    string[] TimgDirs = Directory.GetDirectories(TempIconPath + "arc\\timg");

                    foreach (string thisFile in TimgFiles)
                    {
                        if (!thisFile.EndsWith(".tpl"))
                            File.Delete(thisFile);
                    }

                    foreach (string thisDir in TimgDirs)
                        Directory.Delete(thisDir, true);
                }

                return true;
            }
            catch (Exception ex) { ErrorBox(ex.Message); return false; }
        }

        private void btnCreateWad_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbSourceWad.Text))
            {
                if (FailureCheck() == true)
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "Wii Channels|*.wad";

                    if (!string.IsNullOrEmpty(tbAllLanguages.Text))
                        sfd.FileName = tbAllLanguages.Text + " - " + tbTitleID.Text.ToUpper() + ".wad";
                    else
                        sfd.FileName = tbEnglish.Text + " - " + tbTitleID.Text.ToUpper() + ".wad";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            CreationTimer.Reset();
                            CreationTimer.Start();
                            NandLoader = cmbNandLoader.SelectedIndex;
                            BackgroundWorker bwCreateWad = new BackgroundWorker();
                            bwCreateWad.DoWork += new DoWorkEventHandler(bwCreateWad_DoWork);
                            bwCreateWad.ProgressChanged += new ProgressChangedEventHandler(bwCreateWad_ProgressChanged);
                            bwCreateWad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwCreateWad_RunWorkerCompleted);
                            bwCreateWad.WorkerReportsProgress = true;
                            bwCreateWad.RunWorkerAsync(sfd.FileName);
                        }
                        catch (Exception ex)
                        {
                            CreationTimer.Stop();
                            ErrorBox(ex.Message);
                        }
                    }
                }
            }
        }

        void bwCreateWad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            EventHandler EnableControls = new EventHandler(this.EnableControls);
            EventHandler Initialize = new EventHandler(this.Initialize);
            pbProgress.Value = 100;
            lbStatusText.Text = string.Empty;
            this.Invoke(EnableControls);
            this.Invoke(Initialize);
        }

        void bwCreateWad_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressValue = e.ProgressPercentage;
            ProgressState = (string)e.UserState;

            this.Invoke(ProgressUpdate);
        }

        void bwCreateWad_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker bwCreateWad = sender as BackgroundWorker;
                EventHandler DisableControls = new EventHandler(this.DisableControls);
                this.Invoke(DisableControls);

                bwCreateWad.ReportProgress(0, "Making TPLs transparent");
                MakeBannerTplsTransparent();
                MakeIconTplsTransparent();

                bwCreateWad.ReportProgress(5, "Packing icon.bin...");
                byte[] iconbin;

                if (!string.IsNullOrEmpty(IconReplace))
                    iconbin = Wii.U8.PackU8(TempIconPath);
                else
                    iconbin = Wii.U8.PackU8(TempUnpackPath + "00000000.app_OUT\\meta\\icon.bin_OUT");

                if (cbLz77.Checked == true) iconbin = Wii.Lz77.Compress(iconbin);
                iconbin = Wii.U8.AddHeaderIMD5(iconbin);
                Wii.Tools.SaveFileFromByteArray(iconbin, TempUnpackPath + "00000000.app_OUT\\meta\\icon.bin");
                Directory.Delete(TempUnpackPath + "00000000.app_OUT\\meta\\icon.bin_OUT", true);

                bwCreateWad.ReportProgress(25, "Packing banner.bin...");
                byte[] bannerbin;

                if (!string.IsNullOrEmpty(BannerReplace))
                    bannerbin = Wii.U8.PackU8(TempBannerPath);
                else
                    bannerbin = Wii.U8.PackU8(TempUnpackPath + "00000000.app_OUT\\meta\\banner.bin_OUT");

                if (cbLz77.Checked == true) bannerbin = Wii.Lz77.Compress(bannerbin);
                bannerbin = Wii.U8.AddHeaderIMD5(bannerbin);
                Wii.Tools.SaveFileFromByteArray(bannerbin, TempUnpackPath + "00000000.app_OUT\\meta\\banner.bin");
                Directory.Delete(TempUnpackPath + "00000000.app_OUT\\meta\\banner.bin_OUT", true);

                if (!string.IsNullOrEmpty(SoundReplace) || !string.IsNullOrEmpty(tbSound.Text))
                {
                    bwCreateWad.ReportProgress(50, "Packing sound.bin...");

                    if (!string.IsNullOrEmpty(SoundReplace))
                    {
                        File.Delete(TempUnpackPath + "00000000.app_OUT\\meta\\sound.bin");
                        File.Copy(TempSoundPath, TempUnpackPath + "00000000.app_OUT\\meta\\sound.bin");
                    }
                    else if (!string.IsNullOrEmpty(tbSound.Text))
                    {
                        Wii.Sound.WaveToSoundBin(tbSound.Text, TempUnpackPath + "00000000.app_OUT\\meta\\sound.bin", cbLz77.Checked);
                    }
                }

                bwCreateWad.ReportProgress(60, "Packing 00000000.app...");
                int[] Sizes = new int[3];
                string[] Titles = new string[] { tbJapanese.Text, tbEnglish.Text, tbGerman.Text, tbFrench.Text, tbSpanish.Text, tbItalian.Text, tbDutch.Text };

                for (int i = 0; i < Titles.Length; i++)
                    if (string.IsNullOrEmpty(Titles[i])) Titles[i] = tbAllLanguages.Text;

                byte[] nullapp = Wii.U8.PackU8(TempUnpackPath + "00000000.app_OUT", out Sizes[0], out Sizes[1], out Sizes[2]);
                nullapp = Wii.U8.AddHeaderIMET(nullapp, Titles, Sizes);
                Wii.Tools.SaveFileFromByteArray(nullapp, TempUnpackPath + "00000000.app");
                Directory.Delete(TempUnpackPath + "00000000.app_OUT", true);

                string[] tikfile = Directory.GetFiles(TempUnpackPath, "*.tik");
                string[] tmdfile = Directory.GetFiles(TempUnpackPath, "*.tmd");
                byte[] tmd = Wii.Tools.LoadFileToByteArray(tmdfile[0]);

                if (!string.IsNullOrEmpty(tbDol.Text))
                {
                    bwCreateWad.ReportProgress(80, "Inserting new DOL...");
                    string[] AppFiles = Directory.GetFiles(TempUnpackPath, "*.app");

                    foreach (string thisApp in AppFiles)
                        if (!thisApp.EndsWith("00000000.app")) File.Delete(thisApp);

                    if (NandLoader == 0)
                    {
                        using (BinaryReader nandloader = new BinaryReader(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("CustomizeMii.Resources.comex.app")))
                        {
                            using (FileStream fs = new FileStream(TempUnpackPath + "\\00000001.app", FileMode.Create))
                            {
                                byte[] temp = nandloader.ReadBytes((int)nandloader.BaseStream.Length);
                                fs.Write(temp, 0, temp.Length);
                            }
                        }

                        File.Copy(tbDol.Text, TempUnpackPath + "\\00000002.app");
                        tmd = Wii.WadEdit.ChangeTmdBootIndex(tmd, 1);
                    }
                    else
                    {
                        using (BinaryReader nandloader = new BinaryReader(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("CustomizeMii.Resources.Waninkoko.app")))
                        {
                            using (FileStream fs = new FileStream(TempUnpackPath + "\\00000002.app", FileMode.Create))
                            {
                                byte[] temp = nandloader.ReadBytes((int)nandloader.BaseStream.Length);
                                fs.Write(temp, 0, temp.Length);
                            }
                        }

                        File.Copy(tbDol.Text, TempUnpackPath + "\\00000001.app");
                        tmd = Wii.WadEdit.ChangeTmdBootIndex(tmd, 2);
                    }

                    tmd = Wii.WadEdit.ChangeTmdContentCount(tmd, 3);

                    bwCreateWad.ReportProgress(80, "Updating TMD...");
                    File.Delete(tmdfile[0]);
                    using (FileStream fs = new FileStream(tmdfile[0], FileMode.Create))
                    {
                        byte[] tmdconts = new byte[108];
                        tmdconts[7] = 0x01;
                        tmdconts[39] = 0x01;
                        tmdconts[41] = 0x01;
                        tmdconts[43] = 0x01;
                        tmdconts[75] = 0x02;
                        tmdconts[77] = 0x02;
                        tmdconts[79] = 0x01;

                        fs.Write(tmd, 0, 484);
                        fs.Write(tmdconts, 0, tmdconts.Length);
                    }
                }

                bwCreateWad.ReportProgress(85, "Updating TMD...");
                Wii.WadEdit.UpdateTmdContents(tmdfile[0]);

                Wii.WadEdit.ChangeTitleID(tikfile[0], 0, tbTitleID.Text.ToUpper());
                Wii.WadEdit.ChangeTitleID(tmdfile[0], 1, tbTitleID.Text.ToUpper());

                bwCreateWad.ReportProgress(90, "Trucha Signing...");
                Wii.WadEdit.TruchaSign(tmdfile[0], 1);
                Wii.WadEdit.TruchaSign(tikfile[0], 0);

                bwCreateWad.ReportProgress(95, "Packing WAD...");
                if (File.Exists((string)e.Argument)) File.Delete((string)e.Argument);
                Wii.WadPack.PackWad(TempUnpackPath, (string)e.Argument, false);

                bwCreateWad.ReportProgress(100, " ");
                CreationTimer.Stop();
                InfoBox(string.Format("Successfully created custom channel!\nTime elapsed: {0} ms", CreationTimer.ElapsedMilliseconds));
            }
            catch (Exception ex)
            {
                CreationTimer.Stop();
                EventHandler EnableControls = new EventHandler(this.EnableControls);
                this.Invoke(EnableControls);
                ErrorBox(ex.Message);
            }
        }

        private void lbxBrlytBanner_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxBrlytBanner.SelectedIndex != -1)
                lbxBrlytIcon.SelectedIndex = -1;

            if (lbxBrlytIcon.SelectedIndex == -1)
                lbBrlytActions.Text = "Banner";
            else
                lbBrlytActions.Text = "Icon";
        }

        private void lbxBrlytIcon_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxBrlytIcon.SelectedIndex != -1)
                lbxBrlytBanner.SelectedIndex = -1;

            if (lbxBrlytIcon.SelectedIndex == -1)
                lbBrlytActions.Text = "Banner";
            else
                lbBrlytActions.Text = "Icon";
        }

        private void lbxBrlanBanner_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxBrlanBanner.SelectedIndex != -1)
                lbxBrlanIcon.SelectedIndex = -1;

            if (lbxBrlanIcon.SelectedIndex == -1)
                lbBrlanActions.Text = "Banner";
            else
                lbBrlanActions.Text = "Icon";
        }

        private void lbxBrlanIcon_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxBrlanIcon.SelectedIndex != -1)
                lbxBrlanBanner.SelectedIndex = -1;

            if (lbxBrlanIcon.SelectedIndex == -1)
                lbBrlanActions.Text = "Banner";
            else
                lbBrlanActions.Text = "Icon";
        }

        //private void btnBrlytAdd_Click(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(lbBrlytActions.Text))
        //    {
        //        OpenFileDialog ofd = new OpenFileDialog();
        //        ofd.Filter = "brlyt Files|*.brlyt";

        //        if (ofd.ShowDialog() == DialogResult.OK)
        //        {
        //            try
        //            {
        //                bool Exists = false;

        //                if (lbBrlytActions.Text == "Banner")
        //                {
        //                    for (int i = 0; i < lbxBrlytBanner.Items.Count; i++)
        //                        if (lbxBrlytBanner.Items[i].ToString() == ofd.FileName.Remove(0, ofd.FileName.LastIndexOf('\\') + 1))
        //                            Exists = true;

        //                    if (Exists == true)
        //                        ErrorBox("This file already exists, use the Replace button!");
        //                    else
        //                    {
        //                        if (string.IsNullOrEmpty(BannerReplace))
        //                            File.Copy(ofd.FileName, TempUnpackBannerBrlytPath + ofd.FileName.Remove(0, ofd.FileName.LastIndexOf('\\') + 1));
        //                        else
        //                            File.Copy(ofd.FileName, BannerBrlytPath + ofd.FileName.Remove(0, ofd.FileName.LastIndexOf('\\') + 1));
        //                        lbxBrlytBanner.Items.Add(ofd.FileName.Remove(0, ofd.FileName.LastIndexOf('\\') + 1));
        //                        BrlytChanged = true;
        //                    }
        //                }
        //                else
        //                {
        //                    for (int i = 0; i < lbxBrlytIcon.Items.Count; i++)
        //                        if (lbxBrlytIcon.Items[i].ToString() == ofd.FileName.Remove(0, ofd.FileName.LastIndexOf('\\') + 1))
        //                            Exists = true;

        //                    if (Exists == true)
        //                        ErrorBox("This file already exists, use the Replace button!");
        //                    else
        //                    {
        //                        if (string.IsNullOrEmpty(IconReplace))
        //                            File.Copy(ofd.FileName, TempUnpackIconBrlytPath + ofd.FileName.Remove(0, ofd.FileName.LastIndexOf('\\') + 1));
        //                        else
        //                            File.Copy(ofd.FileName, IconBrlytPath + ofd.FileName.Remove(0, ofd.FileName.LastIndexOf('\\') + 1));
        //                        lbxBrlytIcon.Items.Add(ofd.FileName.Remove(0, ofd.FileName.LastIndexOf('\\') + 1));
        //                        BrlytChanged = true;
        //                    }
        //                }
        //            }
        //            catch (Exception ex) { ErrorBox(ex.Message); }
        //        }
        //    }
        //}

        private void btnBrlanAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lbBrlanActions.Text))
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "brlan Files|*.brlan";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        bool Exists = false;

                        if (lbBrlanActions.Text == "Banner")
                        {
                            for (int i = 0; i < lbxBrlanBanner.Items.Count; i++)
                                if (lbxBrlanBanner.Items[i].ToString() == ofd.FileName.Remove(0, ofd.FileName.LastIndexOf('\\') + 1))
                                    Exists = true;

                            if (Exists == true)
                                ErrorBox("This file already exists, use the Replace button!");
                            else
                            {
                                if (string.IsNullOrEmpty(BannerReplace))
                                    File.Copy(ofd.FileName, TempUnpackBannerBrlanPath + ofd.FileName.Remove(0, ofd.FileName.LastIndexOf('\\') + 1));
                                else
                                    File.Copy(ofd.FileName, BannerBrlanPath + ofd.FileName.Remove(0, ofd.FileName.LastIndexOf('\\') + 1));
                                lbxBrlanBanner.Items.Add(ofd.FileName.Remove(0, ofd.FileName.LastIndexOf('\\') + 1));
                                BrlanChanged = true;
                            }
                        }
                        else
                        {
                            for (int i = 0; i < lbxBrlanIcon.Items.Count; i++)
                                if (lbxBrlanIcon.Items[i].ToString() == ofd.FileName.Remove(0, ofd.FileName.LastIndexOf('\\') + 1))
                                    Exists = true;

                            if (Exists == true)
                                ErrorBox("This file already exists, use the Replace button!");
                            else
                            {
                                if (string.IsNullOrEmpty(IconReplace))
                                    File.Copy(ofd.FileName, TempUnpackIconBrlanPath + ofd.FileName.Remove(0, ofd.FileName.LastIndexOf('\\') + 1));
                                else
                                    File.Copy(ofd.FileName, IconBrlanPath + ofd.FileName.Remove(0, ofd.FileName.LastIndexOf('\\') + 1));
                                lbxBrlanIcon.Items.Add(ofd.FileName.Remove(0, ofd.FileName.LastIndexOf('\\') + 1));
                                BrlanChanged = true;
                            }
                        }
                    }
                    catch (Exception ex) { ErrorBox(ex.Message); }
                }
            }
        }

        private void btnBrlytExtract_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lbBrlytActions.Text))
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "brlyt Files|*.brlyt";

                if (lbBrlytActions.Text == "Banner")
                    sfd.FileName = lbxBrlytBanner.SelectedItem.ToString();
                else
                    sfd.FileName = lbxBrlytIcon.SelectedItem.ToString();

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string brlytFile;

                        if (lbBrlytActions.Text == "Banner")
                        {
                            if (string.IsNullOrEmpty(BannerReplace))
                                brlytFile = TempUnpackBannerBrlytPath + lbxBrlytBanner.SelectedItem.ToString();
                            else
                                brlytFile = BannerBrlytPath + lbxBrlytBanner.Items.ToString();
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(IconReplace))
                                brlytFile = TempUnpackIconBrlytPath + lbxBrlytIcon.SelectedItem.ToString();
                            else
                                brlytFile = IconBrlytPath + lbxBrlytIcon.Items.ToString();
                        }

                        File.Copy(brlytFile, sfd.FileName);
                    }
                    catch (Exception ex) { ErrorBox(ex.Message); }
                }
            }
        }

        private void btnBrlanExtract_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lbBrlanActions.Text))
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "brlan Files|*.brlan";

                if (lbBrlanActions.Text == "Banner")
                    sfd.FileName = lbxBrlanBanner.SelectedItem.ToString();
                else
                    sfd.FileName = lbxBrlanIcon.SelectedItem.ToString();

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string brlanFile;

                        if (lbBrlanActions.Text == "Banner")
                        {
                            if (string.IsNullOrEmpty(BannerReplace))
                                brlanFile = TempUnpackBannerBrlanPath + lbxBrlanBanner.SelectedItem.ToString();
                            else
                                brlanFile = BannerBrlanPath + lbxBrlanBanner.Items.ToString();
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(IconReplace))
                                brlanFile = TempUnpackIconBrlanPath + lbxBrlanIcon.SelectedItem.ToString();
                            else
                                brlanFile = IconBrlanPath + lbxBrlanIcon.Items.ToString();
                        }

                        File.Copy(brlanFile, sfd.FileName);
                    }
                    catch (Exception ex) { ErrorBox(ex.Message); }
                }
            }
        }

        //private void btnBrlytDelete_Click(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(lbBrlytActions.Text))
        //    {
        //        try
        //        {
        //            string brlytFile;

        //            if (lbBrlytActions.Text == "Banner")
        //            {
        //                if (lbxBrlytBanner.Items.Count > 1)
        //                {
        //                    if (string.IsNullOrEmpty(BannerReplace))
        //                        brlytFile = TempUnpackBannerBrlytPath + lbxBrlytBanner.SelectedItem.ToString();
        //                    else
        //                        brlytFile = BannerBrlytPath + lbxBrlytBanner.SelectedItem.ToString();

        //                    lbxBrlytBanner.Items.Remove(lbxBrlytBanner.SelectedItem);
        //                    File.Delete(brlytFile);
        //                    BrlytChanged = true;
        //                }
        //                else
        //                {
        //                    ErrorBox("You can't delete the last file.\nAdd a new one first in order to delete this one.");
        //                }
        //            }
        //            else
        //            {
        //                if (lbxBrlytIcon.Items.Count > 1)
        //                {
        //                    if (string.IsNullOrEmpty(IconReplace))
        //                        brlytFile = TempUnpackIconBrlytPath + lbxBrlytIcon.SelectedItem.ToString();
        //                    else
        //                        brlytFile = IconBrlytPath + lbxBrlytIcon.SelectedItem.ToString();

        //                    lbxBrlytIcon.Items.Remove(lbxBrlytIcon.SelectedItem);
        //                    File.Delete(brlytFile);
        //                    BrlytChanged = true;
        //                }
        //                else
        //                {
        //                    ErrorBox("You can't delete the last file.\nAdd a new one first in order to delete this one.");
        //                }
        //            }                  
        //        }
        //        catch { }
        //    }
        //}

        private void btnBrlanDelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lbBrlanActions.Text))
            {
                try
                {
                    string brlanFile;

                    if (lbBrlanActions.Text == "Banner")
                    {
                        if (lbxBrlanBanner.Items.Count > 1)
                        {
                            if (string.IsNullOrEmpty(BannerReplace))
                                brlanFile = TempUnpackBannerBrlanPath + lbxBrlanBanner.SelectedItem.ToString();
                            else
                                brlanFile = BannerBrlanPath + lbxBrlanBanner.SelectedItem.ToString();

                            lbxBrlanBanner.Items.Remove(lbxBrlanBanner.SelectedItem);
                            File.Delete(brlanFile);
                            BrlanChanged = true;
                        }
                        else
                        {
                            ErrorBox("You can't delete the last file.\nAdd a new one first in order to delete this one.");
                        }
                    }
                    else
                    {
                        if (lbxBrlanIcon.Items.Count > 1)
                        {
                            if (string.IsNullOrEmpty(IconReplace))
                                brlanFile = TempUnpackIconBrlanPath + lbxBrlanIcon.SelectedItem.ToString();
                            else
                                brlanFile = IconBrlanPath + lbxBrlanIcon.SelectedItem.ToString();

                            lbxBrlanIcon.Items.Remove(lbxBrlanIcon.SelectedItem);
                            File.Delete(brlanFile);
                            BrlanChanged = true;
                        }
                        else
                        {
                            ErrorBox("You can't delete the last file.\nAdd a new one first in order to delete this one.");
                        }
                    }
                }
                catch { }
            }
        }

        private void btnBrlytReplace_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lbBrlytActions.Text))
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "brlyt Files|*.brlyt";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (lbBrlytActions.Text == "Banner")
                        {
                            if (string.IsNullOrEmpty(BannerReplace))
                            {
                                File.Delete(TempUnpackBannerBrlytPath + lbxBrlytBanner.SelectedItem.ToString());
                                File.Copy(ofd.FileName, TempUnpackBannerBrlytPath + lbxBrlytBanner.SelectedItem.ToString());
                                BrlytChanged = true;
                            }
                            else
                            {
                                File.Delete(BannerBrlytPath + lbxBrlytBanner.SelectedItem.ToString());
                                File.Copy(ofd.FileName, BannerBrlytPath + lbxBrlytBanner.SelectedItem.ToString());
                                BrlytChanged = true;
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(IconReplace))
                            {
                                File.Delete(TempUnpackIconBrlytPath + lbxBrlytIcon.SelectedItem.ToString());
                                File.Copy(ofd.FileName, TempUnpackIconBrlytPath + lbxBrlytIcon.SelectedItem.ToString());
                                BrlytChanged = true;
                            }
                            else
                            {
                                File.Delete(IconBrlytPath + lbxBrlytIcon.SelectedItem.ToString());
                                File.Copy(ofd.FileName, IconBrlytPath + lbxBrlytIcon.SelectedItem.ToString());
                                BrlytChanged = true;
                            }
                        }
                    }
                    catch (Exception ex) { ErrorBox(ex.Message); }
                }
            }
        }

        private void btnBrlanReplace_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lbBrlanActions.Text))
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "brlan Files|*.brlan";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (lbBrlanActions.Text == "Banner")
                        {
                            if (string.IsNullOrEmpty(BannerReplace))
                            {
                                File.Delete(TempUnpackBannerBrlanPath + lbxBrlanBanner.SelectedItem.ToString());
                                File.Copy(ofd.FileName, TempUnpackBannerBrlanPath + lbxBrlanBanner.SelectedItem.ToString());
                                BrlanChanged = true;
                            }
                            else
                            {
                                File.Delete(BannerBrlanPath + lbxBrlanBanner.SelectedItem.ToString());
                                File.Copy(ofd.FileName, BannerBrlanPath + lbxBrlanBanner.SelectedItem.ToString());
                                BrlanChanged = true;
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(IconReplace))
                            {
                                File.Delete(TempUnpackIconBrlanPath + lbxBrlanIcon.SelectedItem.ToString());
                                File.Copy(ofd.FileName, TempUnpackIconBrlanPath + lbxBrlanIcon.SelectedItem.ToString());
                                BrlanChanged = true;
                            }
                            else
                            {
                                File.Delete(IconBrlanPath + lbxBrlanIcon.SelectedItem.ToString());
                                File.Copy(ofd.FileName, IconBrlanPath + lbxBrlanIcon.SelectedItem.ToString());
                                BrlanChanged = true;
                            }
                        }
                    }
                    catch (Exception ex) { ErrorBox(ex.Message); }
                }
            }
        }

        private void llbUpdateAvailabe_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                llbUpdateAvailabe.LinkVisited = true;
                Process.Start("http://code.google.com/p/customizemii/downloads/list");
            }
            catch (Exception ex) { ErrorBox(ex.Message); }
        }

        private void btnDeleteBanner_Click(object sender, EventArgs e)
        {
            try
            {
                string TplName = lbxBannerTpls.SelectedItem.ToString().Replace(" (Transparent)", string.Empty);
                string CurBannerPath = GetCurBannerPath();
                string[] brlytTpls = Wii.Brlyt.GetBrlytTpls(CurBannerPath + "blyt\\banner.brlyt");

                if (!Wii.Tools.StringExistsInStringArray(TplName, brlytTpls))
                {
                    File.Delete(CurBannerPath + "timg\\" + TplName);
                    lbxBannerTpls.Items.Remove(lbxBannerTpls.SelectedItem);
                }
                else
                {
                    ErrorBox("This TPL is required by your banner.brlyt and thus can't be deleted!\nYou can still replace the image using the Replace button!");
                }
            }
            catch (Exception ex) { ErrorBox(ex.Message); }
        }

        private void btnDeleteIcon_Click(object sender, EventArgs e)
        {
            try
            {
                string TplName = lbxIconTpls.SelectedItem.ToString().Replace(" (Transparent)", string.Empty);
                string CurIconPath = GetCurIconPath();

                string[] brlytTpls = Wii.Brlyt.GetBrlytTpls(CurIconPath + "blyt\\icon.brlyt");

                if (!Wii.Tools.StringExistsInStringArray(TplName, brlytTpls))
                {
                    File.Delete(CurIconPath + "timg\\" + TplName);
                    lbxIconTpls.Items.Remove(lbxIconTpls.SelectedItem);
                }
                else
                {
                    ErrorBox("This TPL is required by your icon.brlyt and thus can't be deleted!\nYou can still replace the image using the Replace button!");
                }
            }
            catch (Exception ex) { ErrorBox(ex.Message); }
        }

        private void btnAddBanner_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "TPL|*.tpl|PNG|*.png|JPG|*.jpg|GIF|*.gif|BMP|*.bmp|All|*.tpl;*.png;*.jpg;*.gif;*.bmp";
                ofd.FilterIndex = 6;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    for (int i = 0; i < lbxBannerTpls.Items.Count; i++)
                    {
                        if (lbxBannerTpls.Items[i].ToString().ToLower() == Path.GetFileNameWithoutExtension(ofd.FileName).ToLower() + ".tpl")
                        {
                            ErrorBox("This TPL already exists, use the Replace button");
                            return;
                        }
                    }

                    string CurBannerPath = GetCurBannerPath();
                    string[] brlytTpls = Wii.Brlyt.GetBrlytTpls(CurBannerPath + "blyt\\banner.brlyt");
                    string TplName = Path.GetFileNameWithoutExtension(ofd.FileName) + ".tpl";
                    int TplFormat = 6;

                    switch (cmbFormatBanner.SelectedIndex)
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
                            if (!ofd.FileName.EndsWith(".tpl"))
                            {
                                ErrorBox("This format is not supported, you must choose either RGBA8, RGB565 or RGB5A3!");
                                return;
                            }
                            break;
                    }

                    if (!Wii.Tools.StringExistsInStringArray(TplName, brlytTpls))
                    {
                        if (MessageBox.Show("This TPL is not required by your banner.brlyt and thus only wastes memory!\nDo you still want to add it?", "TPL not required", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                            return;
                    }

                    if (ofd.FileName.EndsWith(".tpl"))
                    {
                        File.Copy(ofd.FileName, CurBannerPath + "timg\\" + TplName);
                        lbxBannerTpls.Items.Add(TplName);
                    }
                    else
                    {
                        Image img = Image.FromFile(ofd.FileName);
                        Wii.TPL.ConvertToTPL(img, CurBannerPath + "timg\\" + TplName, TplFormat);
                        lbxBannerTpls.Items.Add(TplName);
                    }
                }
            }
            catch (Exception ex) { ErrorBox(ex.Message); }
        }

        private void btnAddIcon_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "TPL|*.tpl|PNG|*.png|JPG|*.jpg|GIF|*.gif|BMP|*.bmp|All|*.tpl;*.png;*.jpg;*.gif;*.bmp";
                ofd.FilterIndex = 6;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    for (int i = 0; i < lbxIconTpls.Items.Count; i++)
                    {
                        if (lbxIconTpls.Items[i].ToString().ToLower() == Path.GetFileNameWithoutExtension(ofd.FileName).ToLower() + ".tpl")
                        {
                            ErrorBox("This TPL already exists, use the Replace button");
                            return;
                        }
                    }

                    string CuriconPath = GetCurIconPath();
                    string[] brlytTpls = Wii.Brlyt.GetBrlytTpls(CuriconPath + "blyt\\icon.brlyt");
                    string TplName = Path.GetFileNameWithoutExtension(ofd.FileName) + ".tpl";
                    int TplFormat = 6;

                    switch (cmbFormatIcon.SelectedIndex)
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
                            if (!ofd.FileName.EndsWith(".tpl"))
                            {
                                ErrorBox("This format is not supported, you must choose either RGBA8, RGB565 or RGB5A3!");
                                return;
                            }
                            break;
                    }

                    if (!Wii.Tools.StringExistsInStringArray(TplName, brlytTpls))
                    {
                        if (MessageBox.Show("This TPL is not required by your icon.brlyt and thus only wastes memory!\nDo you still want to add it?", "TPL not required", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                            return;
                    }

                    if (ofd.FileName.EndsWith(".tpl"))
                    {
                        File.Copy(ofd.FileName, CuriconPath + "timg\\" + TplName);
                        lbxIconTpls.Items.Add(TplName);
                    }
                    else
                    {
                        Image img = Image.FromFile(ofd.FileName);
                        Wii.TPL.ConvertToTPL(img, CuriconPath + "timg\\" + TplName, TplFormat);
                        lbxIconTpls.Items.Add(TplName);
                    }
                }
            }
            catch (Exception ex) { ErrorBox(ex.Message); }
        }

        private void btnBrlytListTpls_Click(object sender, EventArgs e)
        {
            if (lbBrlytActions.Text == "Banner")
            {
                string CurBannerPath = GetCurBannerPath();
                string[] brlytTpls = Wii.Brlyt.GetBrlytTpls(CurBannerPath + "blyt\\banner.brlyt");

                MessageBox.Show("These are the TPLs required by your banner.brlyt:\n\n" +
                    string.Join("\n", brlytTpls), "TPLs specified in banner.brlyt", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                string CurIconPath = GetCurIconPath();
                string[] brlytTpls = Wii.Brlyt.GetBrlytTpls(CurIconPath + "blyt\\icon.brlyt");

                MessageBox.Show("These are the TPLs required by your icon.brlyt:\n\n" +
                    string.Join("\n", brlytTpls), "TPLs specified in icon.brlyt", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void cbBannerMakeTransparent_CheckedChanged(object sender, EventArgs e)
        {
            if (lbxBannerTpls.SelectedIndex != -1)
            {
                if (cbBannerMakeTransparent.Checked == true)
                {
                    try
                    {
                        if (!lbxBannerTpls.SelectedItem.ToString().EndsWith("(Transparent)"))
                        {
                            string thisItem = lbxBannerTpls.SelectedItem.ToString().Replace(" (Transparent)", string.Empty);
                            lbxBannerTpls.Items.Remove(lbxBannerTpls.SelectedItem);
                            lbxBannerTpls.Items.Add(thisItem + " (Transparent)");
                            lbxBannerTpls.SelectedItem = thisItem + " (Transparent)";
                            Transparents.Add(thisItem);
                        }
                    }
                    catch { }
                }
                else
                {
                    try
                    {
                        string thisItem = lbxBannerTpls.SelectedItem.ToString().Replace(" (Transparent)", string.Empty);
                        lbxBannerTpls.Items.Remove(lbxBannerTpls.SelectedItem);
                        lbxBannerTpls.Items.Add(thisItem.Replace(" (Transparent)", string.Empty));
                        lbxBannerTpls.SelectedItem = thisItem.Replace(" (Transparent)", string.Empty);
                        Transparents.Remove(thisItem.Replace(" (Transparent)", string.Empty));
                    }
                    catch { }
                }
            }
        }

        private void cbIconMakeTransparent_CheckedChanged(object sender, EventArgs e)
        {
            if (lbxIconTpls.SelectedIndex != -1)
            {
                if (cbIconMakeTransparent.Checked == true)
                {
                    try
                    {
                        if (!lbxIconTpls.SelectedItem.ToString().EndsWith("(Transparent)"))
                        {
                            string thisItem = lbxIconTpls.SelectedItem.ToString().Replace(" (Transparent)", string.Empty);
                            lbxIconTpls.Items.Remove(lbxIconTpls.SelectedItem);
                            lbxIconTpls.Items.Add(thisItem + " (Transparent)");
                            lbxIconTpls.SelectedItem = thisItem + " (Transparent)";
                            Transparents.Add(thisItem);
                        }
                    }
                    catch { }
                }
                else
                {
                    try
                    {
                        string thisItem = lbxIconTpls.SelectedItem.ToString().Replace(" (Transparent)", string.Empty);
                        lbxIconTpls.Items.Remove(lbxIconTpls.SelectedItem);
                        lbxIconTpls.Items.Add(thisItem.Replace(" (Transparent)", string.Empty));
                        lbxIconTpls.SelectedItem = thisItem.Replace(" (Transparent)", string.Empty);
                        Transparents.Remove(thisItem.Replace(" (Transparent)", string.Empty));
                    }
                    catch { }
                }
            }
        }
    }
}
