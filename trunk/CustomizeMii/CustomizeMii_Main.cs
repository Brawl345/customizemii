/* This file is part of CustomizeMii
 * Copyright (C) 2009 Leathl
 * 
 * CustomizeMii is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * CustomizeMii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */
 
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CustomizeMii
{
    public partial class CustomizeMii_Main : Form
    {
        const string Version = "1.0";
        protected string TempWadPath = Path.GetTempPath() + "CustomizeMii_Temp\\TempWad.wad";
        protected string TempUnpackPath = Path.GetTempPath() + "CustomizeMii_Temp\\Unpack\\";
        protected string TempBannerPath = Path.GetTempPath() + "CustomizeMii_Temp\\Banner\\";
        protected string TempIconPath = Path.GetTempPath() + "CustomizeMii_Temp\\Icon\\";
        protected string TempSoundPath = Path.GetTempPath() + "CustomizeMii_Temp\\sound.bin";
        protected string TempTempPath = Path.GetTempPath() + "CustomizeMii_Temp\\Temp\\";
        protected string[] ButtonTexts = new string[] { "Create WAD!", "Fire!", "Go Go Go!", "Let's do it!", "What are you waitin' for?", "I want my Channel!", "Houston, We've Got a Problem!", "Error, please contact anyone!", "Isn't she sweet?", "Is that milk?", "In your face!", "F***!", "_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_", "Was steht hier?", "Take me to a higher place!", "What's goin' on?", "I'm a Button!", "Click!" };
        protected string[] SourceWadUrls = new string[] { "http://wadder.net/bases/StaticBase.wad", "http://wadder.net/bases/GenesisGX.wad", "http://wadder.net/bases/MP-CE-Std.wad", "http://wadder.net/bases/MP-CE-Ext.wad", "http://wadder.net/bases/SNES9XGX.wad", "http://wadder.net/bases/FCEUGX-wilsoff.wad", "http://wadder.net/bases/FCEUGX.wad", "http://wadder.net/bases/Wii64.wad", "http://wadder.net/bases/WiiSXFull.wad", "http://wadder.net/bases/WiiSXRetro.wad", "http://wadder.net/bases/WADderBase1.wad", "http://wadder.net/bases/WADderBase2.wad", "http://wadder.net/bases/WADderBase3.wad", "http://wadder.net/bases/UniiLoader.wad", "http://wadder.net/bases/BackupChannel.wad" };
        protected string[] SourceWadPreviewUrls = new string[] { "http://www.youtube.com/watch?v=pFNKldTYQq0", "http://www.youtube.com/watch?v=p9A6iEQvv9w", "http://www.youtube.com/watch?v=Up1RZebUc_U", "http://www.youtube.com/watch?v=Up1RZebUc_U", "http://www.youtube.com/watch?v=P-Mxd6DMvFY", "http://www.youtube.com/watch?v=wrbrg-DH_h4", "http://www.youtube.com/watch?v=MfiVbQaiXw8", "http://www.youtube.com/watch?v=krCQ2J7ZH8Y", "http://www.youtube.com/watch?v=rZC1DKUM6QI", "http://www.youtube.com/watch?v=Uiy8w-bp1kI", "http://www.youtube.com/watch?v=BbSYCSI8tz8", "http://www.youtube.com/watch?v=PIFZevHQ8lQ", "http://www.youtube.com/watch?v=OIhvDNjphhc", "", "http://www.youtube.com/watch?v=xE_EgdCRV1I" };
        private string BannerTplPath = string.Empty;
        private string IconTplPath = string.Empty;
        private string SourceWad = string.Empty;
        private string BannerReplace = string.Empty;
        private string IconReplace = string.Empty;
        private string SoundReplace = string.Empty;
        private int NandLoader = 0;
        private TextBox tbToChange;
        private string textToChange;

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
            SetButtonText();
            cmbNandLoader.SelectedIndex = 0;
            cmbFormatBanner.SelectedIndex = 0;
            cmbFormatIcon.SelectedIndex = 0;
            cmbReplace.SelectedIndex = 0;
            pbProgress.Value = 100;
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

        private void AddBannerTpls(string[] tpls)
        {
            if (tpls.Length > 0)
            {
                lbBannerTpls.Items.Clear();
                BannerTplPath = tpls[0].Remove(tpls[0].LastIndexOf('\\') + 1);

                for (int i = 0; i < tpls.Length; i++)
                {
                    lbBannerTpls.Items.Add(tpls[i].Remove(0, tpls[i].LastIndexOf('\\') + 1));
                }
            }
        }

        private void AddIconTpls(string[] tpls)
        {
            if (tpls.Length > 0)
            {
                lbIconTpls.Items.Clear();
                IconTplPath = tpls[0].Remove(tpls[0].LastIndexOf('\\') + 1);

                for (int i = 0; i < tpls.Length; i++)
                {
                    lbIconTpls.Items.Add(tpls[i].Remove(0, tpls[i].LastIndexOf('\\') + 1));
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
                try
                {
                    string[] BannerTpls;
                    if (string.IsNullOrEmpty(BannerReplace))
                        BannerTpls = Directory.GetFiles(TempUnpackPath + "00000000.app_OUT\\meta\\banner.bin_OUT\\arc\\timg");
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
                        IconTpls = Directory.GetFiles(TempUnpackPath + "00000000.app_OUT\\meta\\icon.bin_OUT\\arc\\timg");
                    else
                        IconTpls = Directory.GetFiles(TempIconPath + "arc\\timg");

                    AddIconTpls(IconTpls);
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
                            BannerTpls = Directory.GetFiles(TempUnpackPath + "00000000.app_OUT\\meta\\banner.bin_OUT\\arc\\timg");
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
                            IconTpls = Directory.GetFiles(TempUnpackPath + "00000000.app_OUT\\meta\\icon.bin_OUT\\arc\\timg");
                        else
                            IconTpls = Directory.GetFiles(TempIconPath + "arc\\timg");

                        AddIconTpls(IconTpls);
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

        private void EnableControls(object sender, EventArgs e)
        {
            tbAllLanguages.Enabled = true;
            tbEnglish.Enabled = true;
            tbJapanese.Enabled = true;
            tbGerman.Enabled = true;
            tbFrench.Enabled = true;
            tbSpanish.Enabled = true;
            tbItalian.Enabled = true;
            tbDutch.Enabled = true;
            tbTitleID.Enabled = true;

            cbLz77.Enabled = true;

            btnBrowseSource.Enabled = true;
            btnLoadBaseWad.Enabled = true;
            btnPreviewBaseWad.Enabled = true;
            btnSaveBaseWad.Enabled = true;
            btnBrowseReplace.Enabled = true;
            btnClearReplace.Enabled = true;
            btnBrowseDol.Enabled = true;
            btnBrowseSound.Enabled = true;
            btnReplaceBanner.Enabled = true;
            btnReplaceIcon.Enabled = true;
            btnExtractBanner.Enabled = true;
            btnExtractIcon.Enabled = true;
            btnPreviewBanner.Enabled = true;
            btnPreviewIcon.Enabled = true;
            btnCreateWad.Enabled = true;

            cmbFormatBanner.Enabled = true;
            cmbFormatIcon.Enabled = true;
            cmbNandLoader.Enabled = true;
            cmbReplace.Enabled = true;
        }

        private void DisableControls(object sender, EventArgs e)
        {
            tbAllLanguages.Enabled = false;
            tbEnglish.Enabled = false;
            tbJapanese.Enabled = false;
            tbGerman.Enabled = false;
            tbFrench.Enabled = false;
            tbSpanish.Enabled = false;
            tbItalian.Enabled = false;
            tbDutch.Enabled = false;
            tbTitleID.Enabled = false;

            cbLz77.Enabled = false;

            btnBrowseSource.Enabled = false;
            btnLoadBaseWad.Enabled = false;
            btnPreviewBaseWad.Enabled = false;
            btnSaveBaseWad.Enabled = false;
            btnBrowseReplace.Enabled = false;
            btnClearReplace.Enabled = false;
            btnBrowseDol.Enabled = false;
            btnBrowseSound.Enabled = false;
            btnReplaceBanner.Enabled = false;
            btnReplaceIcon.Enabled = false;
            btnExtractBanner.Enabled = false;
            btnExtractIcon.Enabled = false;
            btnPreviewBanner.Enabled = false;
            btnPreviewIcon.Enabled = false;
            btnCreateWad.Enabled = false;

            cmbFormatBanner.Enabled = false;
            cmbFormatIcon.Enabled = false;
            cmbNandLoader.Enabled = false;
            cmbReplace.Enabled = false;
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
            pbProgress.Value = e.ProgressPercentage;

            if (!string.IsNullOrEmpty((string)e.UserState))
                lbStatusText.Text = (string)e.UserState;
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

                bwLoadChannel.ReportProgress(100);
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
            if (lbBaseWads.SelectedIndex != -1)
            {
                if (CheckInet() == true)
                {
                    if (pbProgress.Value == 100)
                    {
                        try
                        {
                            SourceWad = SourceWadUrls[lbBaseWads.SelectedIndex];
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
            if (lbBaseWads.SelectedIndex != -1)
            {
                if (CheckInet() == true)
                {
                    if (!string.IsNullOrEmpty(SourceWadPreviewUrls[lbBaseWads.SelectedIndex]))
                    {
                        try
                        {
                            Process.Start(SourceWadPreviewUrls[lbBaseWads.SelectedIndex]);
                        }
                        catch (Exception ex) { ErrorBox(ex.Message); }
                    }
                }
                else
                {
                    ErrorBox("You're not connected to the Internet!");
                }
            }
        }

        private void btnSaveBaseWad_Click(object sender, EventArgs e)
        {
            if (lbBaseWads.SelectedIndex != -1)
            {
                if (CheckInet() == true)
                {
                    if (pbProgress.Value == 100)
                    {
                        string Url = SourceWadUrls[lbBaseWads.SelectedIndex];
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
            pbProgress.Value = e.ProgressPercentage;

            if (!string.IsNullOrEmpty((string)e.UserState))
                lbStatusText.Text = (string)e.UserState;
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
                    bwBannerReplace.ReportProgress(30, "Loading 00000000.app...");
                    Wii.U8.UnpackU8(TempTempPath + "00000000.app", TempTempPath + "00000000.app_OUT");
                    bwBannerReplace.ReportProgress(60, "Loading banner.bin...");
                    Wii.U8.UnpackU8(TempTempPath + "00000000.app_OUT\\meta\\banner.bin", TempBannerPath);
                    Directory.Delete(TempTempPath, true);
                }
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
            pbProgress.Value = e.ProgressPercentage;

            if (!string.IsNullOrEmpty((string)e.UserState))
                lbStatusText.Text = (string)e.UserState;
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
                    bwIconReplace.ReportProgress(30, "Loading 00000000.app...");
                    Wii.U8.UnpackU8(TempTempPath + "00000000.app", TempTempPath + "00000000.app_OUT");
                    bwIconReplace.ReportProgress(60, "Loading icon.bin...");
                    Wii.U8.UnpackU8(TempTempPath + "00000000.app_OUT\\meta\\icon.bin", TempIconPath);
                    Directory.Delete(TempTempPath, true);
                }
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
            pbProgress.Value = e.ProgressPercentage;

            if (!string.IsNullOrEmpty((string)e.UserState))
                lbStatusText.Text = (string)e.UserState;
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
                    bwSoundReplace.ReportProgress(50, "Loading 00000000.app...");
                    Wii.U8.UnpackU8(TempTempPath + "00000000.app", TempTempPath + "00000000.app_OUT");
                    bwSoundReplace.ReportProgress(90, "Copying sound.bin...");
                    File.Copy(TempTempPath + "00000000.app_OUT\\meta\\sound.bin", TempSoundPath);
                    Directory.Delete(TempTempPath, true);
                }
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
                }
            }
        }

        private void lbBannerTpls_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbBannerTpls.SelectedIndex != -1)
            {
                try
                {
                    string TplFile = BannerTplPath + lbBannerTpls.SelectedItem.ToString();
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
        }

        private void lbIconTpls_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbIconTpls.SelectedIndex != -1)
            {
                try
                {
                    string TplFile = IconTplPath + lbIconTpls.SelectedItem.ToString();
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
        }

        private void btnReplaceBanner_Click(object sender, EventArgs e)
        {
            if (lbBannerTpls.SelectedIndex != -1)
            {
                int Format = cmbFormatBanner.SelectedIndex;

                if (Format == 0 || Format == 1 || Format == 2)
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Filter = "Png|*.png|Jpg|*.jpg|Gif|*.gif|Bmp|*.bmp|All|*.png;*.jpg;*.gif;*.bmp";
                    ofd.FilterIndex = 5;

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            string Tpl = BannerTplPath + lbBannerTpls.SelectedItem.ToString();
                            byte[] TplArray = Wii.Tools.LoadFileToByteArray(Tpl);
                            Image Img = Image.FromFile(ofd.FileName);
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
            if (lbBannerTpls.SelectedIndex != -1)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Png|*.png|Jpg|*.jpg|Gif|*.gif|Bmp|*.bmp|All|*.png;*.jpg;*.gif;*.bmp";
                sfd.FilterIndex = 5;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string Tpl = BannerTplPath + lbBannerTpls.SelectedItem.ToString();
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
                    catch (Exception ex)
                    {
                        ErrorBox(ex.Message);
                    }
                }
            }
        }

        private void btnPreviewBanner_Click(object sender, EventArgs e)
        {
            if (lbBannerTpls.SelectedIndex != -1)
            {
                try
                {
                    string Tpl = BannerTplPath + lbBannerTpls.SelectedItem.ToString();
                    Image Img = Wii.TPL.ConvertFromTPL(Tpl);

                    CustomizeMii_Preview pvw = new CustomizeMii_Preview();

                    if (Img.Width > 150) pvw.Width = Img.Width + 50;
                    else pvw.Width = 200;
                    if (Img.Height > 150) pvw.Height = Img.Height + 50;
                    else pvw.Height = 200;

                    pvw.pbImage.Image = Img;

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
            if (lbIconTpls.SelectedIndex != -1)
            {
                int Format = cmbFormatIcon.SelectedIndex;

                if (Format == 0 || Format == 1 || Format == 2)
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Filter = "Png|*.png|Jpg|*.jpg|Gif|*.gif|Bmp|*.bmp|All|*.png;*.jpg;*.gif;*.bmp";
                    ofd.FilterIndex = 5;

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            string Tpl = IconTplPath + lbIconTpls.SelectedItem.ToString();
                            byte[] TplArray = Wii.Tools.LoadFileToByteArray(Tpl);
                            Image Img = Image.FromFile(ofd.FileName);
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
            if (lbIconTpls.SelectedIndex != -1)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Png|*.png|Jpg|*.jpg|Gif|*.gif|Bmp|*.bmp|All|*.png;*.jpg;*.gif;*.bmp";
                sfd.FilterIndex = 5;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string Tpl = IconTplPath + lbIconTpls.SelectedItem.ToString();
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
                    catch (Exception ex)
                    {
                        ErrorBox(ex.Message);
                    }
                }
            }
        }

        private void btnPreviewIcon_Click(object sender, EventArgs e)
        {
            if (lbIconTpls.SelectedIndex != -1)
            {
                try
                {
                    string Tpl = IconTplPath + lbIconTpls.SelectedItem.ToString();
                    Image Img = Wii.TPL.ConvertFromTPL(Tpl);

                    CustomizeMii_Preview pvw = new CustomizeMii_Preview();

                    if (Img.Width > 150) pvw.Width = Img.Width + 50;
                    else pvw.Width = 200;
                    if (Img.Height > 150) pvw.Height = Img.Height + 50;
                    else pvw.Height = 200;

                    pvw.pbImage.Image = Img;

                    pvw.ShowDialog();
                }
                catch (Exception ex)
                {
                    ErrorBox(ex.Message);
                }
            }
        }

        private void btnCreateWad_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbSourceWad.Text))
            {
                if (!string.IsNullOrEmpty(tbAllLanguages.Text) ||
                    (!string.IsNullOrEmpty(tbEnglish.Text) &&
                    !string.IsNullOrEmpty(tbJapanese.Text) &&
                    !string.IsNullOrEmpty(tbGerman.Text) &&
                    !string.IsNullOrEmpty(tbFrench.Text) &&
                    !string.IsNullOrEmpty(tbSpanish.Text) &&
                    !string.IsNullOrEmpty(tbItalian.Text) &&
                    !string.IsNullOrEmpty(tbDutch.Text)))
                {
                    if (tbTitleID.Text.Length == 4)
                    {
                        Regex allowedchars = new Regex("[A-Z0-9]{4}$");
                        if (allowedchars.IsMatch(tbTitleID.Text.ToUpper()))
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
                                    ErrorBox(ex.Message);
                                }
                            }
                        }
                        else
                        {
                            ErrorBox("Please enter a valid Title ID!");
                        }
                    }
                    else
                    {
                        ErrorBox("The Title ID must be 4 characters long!");
                    }
                }
                else
                {
                    ErrorBox("You must either enter a general Channel Title or one for each language!");
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
            pbProgress.Value = e.ProgressPercentage;

            if (!string.IsNullOrEmpty((string)e.UserState))
                lbStatusText.Text = (string)e.UserState;
        }

        void bwCreateWad_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker bwCreateWad = sender as BackgroundWorker;
                EventHandler DisableControls = new EventHandler(this.DisableControls);
                this.Invoke(DisableControls);

                bwCreateWad.ReportProgress(0, "Packing icon.bin...");
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
                        byte[] sound = Wii.Tools.LoadFileToByteArray(tbSound.Text);
                        if (cbLz77.Checked == true) sound = Wii.Lz77.Compress(sound);
                        sound = Wii.U8.AddHeaderIMD5(sound);
                        Wii.Tools.SaveFileFromByteArray(sound, TempUnpackPath + "00000000.app_OUT\\meta\\sound.bin");
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
                }

                bwCreateWad.ReportProgress(85, "Updating TMD...");
                File.Delete(tmdfile[0]);
                using (FileStream fs = new FileStream(tmdfile[0], FileMode.Create))
                {
                    using (BinaryReader tmdcontents = new BinaryReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("CustomizeMii.Resources.Tmd_Contents")))
                    {
                        {
                            byte[] tmdconts = tmdcontents.ReadBytes((int)tmdcontents.BaseStream.Length);
                            fs.Write(tmd, 0, 484);
                            fs.Write(tmdconts, 0, tmdconts.Length);
                        }
                    }
                }

                Wii.WadEdit.UpdateTmdContents(tmdfile[0]);

                Wii.WadEdit.ChangeTitleID(tikfile[0], 0, tbTitleID.Text.ToUpper());
                Wii.WadEdit.ChangeTitleID(tmdfile[0], 1, tbTitleID.Text.ToUpper());

                bwCreateWad.ReportProgress(90, "Trucha Signing...");
                Wii.WadEdit.TruchaSign(tmdfile[0], 1);
                Wii.WadEdit.TruchaSign(tikfile[0], 0);

                bwCreateWad.ReportProgress(95, "Packing WAD...");
                Wii.WadPack.PackWad(TempUnpackPath, (string)e.Argument, false);

                bwCreateWad.ReportProgress(100, " ");
                InfoBox("Successfully created Custom Channel!");
            }
            catch (Exception ex)
            {
                EventHandler EnableControls = new EventHandler(this.EnableControls);
                this.Invoke(EnableControls);
                ErrorBox(ex.Message);
            }
        }
    }
}
