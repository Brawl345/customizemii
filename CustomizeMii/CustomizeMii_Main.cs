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
using System.Windows.Forms;
using ForwardMii;
using libWiiSharp;

namespace CustomizeMii
{
    public partial class CustomizeMii_Main : Form
    {
        #region Constants
        private const string version = "3.11.1"; //Hint for myself: Never use a char in the Version (UpdateCheck)!
        private const int soundMaxLength = 40; //In seconds
        private const int soundWarningLength = 25; //In seconds
        private const int bnsWarningLength = 45; //In seconds
        private const int creditsScrollSpeed = 75; //Timer Intervall for the scrolling Credits
        private const int creditsReturnValue = -150; //Need to change this when the credits label gets longer
        #endregion

        #region Variables
        public static string[] SourceWadUrls = new string[] { "StaticBase.wad", "MPlayer_CE_Short.wad", "MPlayer_CE_Long.wad", "Snes9xGX.wad", "FCE_Ultra_wilsoff.wad", "FCE_Ultra_Leathl.wad", "Wii64.wad", "WiiSX_Full.wad", "WiiSX_Retro.wad", "WADder_Base_1.wad", "WADder_Base_2.wad", "WADder_Base_3.wad", "UniiLoader.wad", "Backup_Channel.wad" };
        public static string[] SourceWadPreviewUrls = new string[] { "http://www.youtube.com/watch?v=pFNKldTYQq0", "http://www.youtube.com/watch?v=Up1RZebUc_U", "http://www.youtube.com/watch?v=Up1RZebUc_U", "http://www.youtube.com/watch?v=P-Mxd6DMvFY", "http://www.youtube.com/watch?v=wrbrg-DH_h4", "http://www.youtube.com/watch?v=MfiVbQaiXw8", "http://www.youtube.com/watch?v=krCQ2J7ZH8Y", "http://www.youtube.com/watch?v=rZC1DKUM6QI", "http://www.youtube.com/watch?v=Uiy8w-bp1kI", "http://www.youtube.com/watch?v=BbSYCSI8tz8", "http://www.youtube.com/watch?v=PIFZevHQ8lQ", "http://www.youtube.com/watch?v=OIhvDNjphhc", "http://www.youtube.com/watch?v=KLcncEArQLY&NR=1", "http://www.youtube.com/watch?v=xE_EgdCRV1I" };
        private bool brlytChanged = false;
        private bool brlanChanged = false;
        private Progress currentProgress;
        private EventHandler ProgressUpdate;
        private List<string> bannerTransparents = new List<string>();
        private List<string> iconTransparents = new List<string>();
        private double separatorBtn;
        private Timer tmrCredits = new Timer();
        private ToolTip tTip = new ToolTip();
        #endregion
        
        public CustomizeMii_Main(string[] args)
        {
            InitializeComponent();
            this.Icon = global::CustomizeMii.Properties.Resources.CustomizeMii;

            if (args.Length > 0 && File.Exists(args[0]) && args[0].ToLower().EndsWith(".wad"))
            {
                loadChannel(args[0]);
            }
        }

        private void CustomizeMii_Main_Load(object sender, EventArgs e)
        {
            this.Text = this.Text.Replace("X", version);
            this.lbCreditVersion.Text = this.lbCreditVersion.Text.Replace("X", version);

            if (!Environment.OSVersion.VersionString.ToLower().Contains("windows"))
            {
                //TextBox.MaxLength is not implemented in Mono, so don't use it
                for (int i = 0; i < tabControl.TabPages.Count; i++)
                    for (int j = 0; j < tabControl.TabPages[i].Controls.Count; j++)
                        if (tabControl.TabPages[i].Controls[j] is TextBox) ((TextBox)tabControl.TabPages[i].Controls[j]).MaxLength = 32000;
            }

            if (File.Exists(Application.StartupPath + Path.DirectorySeparatorChar + "CustomizeMiiInstaller.dll"))
                this.lbCreditInstaller.Text = this.lbCreditInstaller.Text.Replace("X", getInstallerVersion());
            else this.lbCreditInstaller.Text = this.lbCreditInstaller.Text.Replace(" X", string.Empty);

            if (!version.ToLower().Contains("beta"))
            {
                MethodInvoker Update = new MethodInvoker(updateCheck);
                Update.BeginInvoke(null, null);
            }

            byte[] t = new byte[45];

            initialize();
        }

        private void CustomizeMii_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void sourceWad_Warning(object sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void initialize()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(initialize));
                return;
            }

            drawCreateButton();
            setToolTips();

            ProgressUpdate = new EventHandler(this.updateProgress);
            tmrCredits.Tick += new EventHandler(tmrCredits_Tick);
            rtbInstructions.LinkClicked += new LinkClickedEventHandler(rtbInstructions_LinkClicked);

            cmbNandLoader.SelectedIndex = 0;
            cmbFormatBanner.SelectedIndex = 0;
            cmbFormatIcon.SelectedIndex = 0;
            cmbReplace.SelectedIndex = 0;

            pbProgress.Value = 100;
            tmrCredits.Interval = creditsScrollSpeed;

            brlanChanged = false;
            brlytChanged = false;

            btnBrowseSource.Text = "Browse...";
            btnBrowseDol.Text = "Browse...";
            btnBrowseSound.Text = "Browse...";

            rtbInstructions.Rtf = Properties.Resources.Instructions;

            clearAll();
            disableControls();
        }

        private void clearAll()
        {
            tbAllLanguages.Text = string.Empty;
            tbDol.Text = string.Empty;
            tbDutch.Text = string.Empty;
            tbEnglish.Text = string.Empty;
            tbFrench.Text = string.Empty;
            tbGerman.Text = string.Empty;
            tbItalian.Text = string.Empty;
            tbKorean.Text = string.Empty;
            tbJapanese.Text = string.Empty;
            tbReplace.Text = string.Empty;
            tbSound.Text = string.Empty;
            tbSourceWad.Text = string.Empty;
            tbSpanish.Text = string.Empty;
            tbTitleID.Text = string.Empty;
            tbStartupIos.Text = string.Empty;

            lbCreatedValue.Text = string.Empty;

            lbxBannerTpls.Items.Clear();
            lbxBrlanBanner.Items.Clear();
            lbxBrlanIcon.Items.Clear();
            lbxBrlytBanner.Items.Clear();
            lbxBrlytIcon.Items.Clear();
            lbxIconTpls.Items.Clear();

            replacedBanner = string.Empty;
            replacedIcon = string.Empty;
            replacedSound = string.Empty;

            sourceWad = new WAD();
            bannerBin = new U8();
            newBannerBin = new U8();
            iconBin = new U8();
            newIconBin = new U8();
            newSoundBin = new byte[0];
            newDol = new byte[0];

            sourceWad.Warning += new EventHandler<MessageEventArgs>(sourceWad_Warning);

            cbLz77.Checked = true;
            cbIconMakeTransparent.Checked = false;
            cbBannerMakeTransparent.Checked = false;

            bannerTransparents.Clear();
            iconTransparents.Clear();

            simpleForwarder.Clear();
            complexForwarder.Clear();
        }

        private void setToolTips()
        {
            tTip.SetToolTip(btnCreateWad, "Create and save the WAD or send it directly to the Wii...\nBe sure the Homebrew Channel is running and connected if you want to send the WAD...");
            tTip.SetToolTip(btnBrowseSource, "Browse for a WAD that is used as a Base...");
            tTip.SetToolTip(btnLoadBaseWad, "Load the selected Base WAD...");
            tTip.SetToolTip(btnPreviewBaseWad, "Preview the selected Base WAD, a Browserwindow will be opened...");
            tTip.SetToolTip(btnSaveBaseWad, "Download and save the selected Base WAD to your HDD...");
            tTip.SetToolTip(btnBrowseReplace, "Browse for a Banner / Icon / Sound to use instead of the one within the Base WAD...\nWAD's, 00000000.app's and banner.bin's / icon.bin's / sound.bin's can be loaded...");
            //TTip.SetToolTip(btnClearReplace, "Clear the replaced Banner / Icon / Sound and use the one within the Base WAD...");
            tTip.SetToolTip(btnBrowseDol, "Browse for a DOL file that will be inserted into the WAD\nor choose the DOL form the source WAD to switch the NAND Loader...");
            tTip.SetToolTip(btnBrowseSound, "Browse for a sound that will be inserted into the WAD\nor convert a sound to BNS format...");
            tTip.SetToolTip(btnAddBanner, "Add an image or TPL to the Banner...");
            tTip.SetToolTip(btnAddIcon, "Add an image or TPL to the Icon...");
            tTip.SetToolTip(btnDeleteBanner, "Delete the selected TPL...\nRequired TPLs can't be deleted...");
            tTip.SetToolTip(btnDeleteIcon, "Delete the selected TPL...\nRequired TPLs can't be deleted...");
            tTip.SetToolTip(btnReplaceBanner, "Replace the selected TPL with any image...\nThe image wil be stretched to fit the size of the TPL...");
            tTip.SetToolTip(btnReplaceIcon, "Replace the selected TPL with any image...\nThe image wil be stretched to fit the size of the TPL...");
            tTip.SetToolTip(btnExtractBanner, "Extract the selected TPL as an image...");
            tTip.SetToolTip(btnExtractIcon, "Extract the selected TPL as an image...");
            tTip.SetToolTip(btnPreviewBanner, "Preview the selected TPL...");
            tTip.SetToolTip(btnPreviewIcon, "Preview the selected TPL...");
            tTip.SetToolTip(btnBrlytReplace, "Replace the selected brlyt with any other...\nThis is for advanced users only!");
            tTip.SetToolTip(btnBrlytExtract, "Extract the selected brlyt...");
            tTip.SetToolTip(btnBrlytListTpls, "List the TPLs required by the selected brlyt...");
            tTip.SetToolTip(btnBrlanAdd, "Add a brlan file...\nThis is for advanced users only!");
            tTip.SetToolTip(btnBrlanDelete, "Delete the selected brlan file...\nThis is for advanced users only!");
            tTip.SetToolTip(btnBrlanReplace, "Replace the selected brlan file...\nThis is for advanced users only!");
            tTip.SetToolTip(btnBrlanExtract, "Extract the selected brlan file...");
            tTip.SetToolTip(btnOptionsExtract, "Extract contents of the WAD...");
            tTip.SetToolTip(btnForwarder, "Create a forwarder that will be inserted as a DOL...");

            tTip.SetToolTip(llbTranslateChannel, "Translates the word \"Channel\" to each language...");

            tTip.SetToolTip(cbLz77, "Use Lz77 compression for the banner.bin and icon.bin...\nIf the created WAD does not work, try it without compression first...");
            tTip.SetToolTip(cbSecurityChecksOff, "Turn off the security checks...\nNot recommended, you may get a bricking WAD...");
        }

        private bool checkForForwardMii()
        {
            if (File.Exists(Application.StartupPath + Path.DirectorySeparatorChar + "ForwardMii.dll"))
                return true;
            else
                return false;
        }

        private void rtbInstructions_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                Process.Start(e.LinkText);
            }
            catch (Exception ex) { errorBox(ex.Message); }
        }

        private bool checkDevKit()
        {
            return ForwardMii_Plugin.CheckDevKit();
        }

        private byte[] createForwarderSimple()
        {
            return simpleForwarder.ToByteArray();
        }

        private byte[] createForwarderComplex()
        {
            return complexForwarder.ToByteArray();
        }

        private void convertMp3ToWave(string mp3File)
        {
            if (File.Exists(Application.StartupPath + Path.DirectorySeparatorChar + "lame.exe"))
            {
                try
                {
                    BackgroundWorker bwConvertMp3 = new BackgroundWorker();
                    bwConvertMp3.DoWork += new DoWorkEventHandler(bwConvertMp3_DoWork);
                    bwConvertMp3.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwConvertMp3_RunWorkerCompleted);
                    bwConvertMp3.ProgressChanged += new ProgressChangedEventHandler(bwConvertMp3_ProgressChanged);
                    bwConvertMp3.WorkerReportsProgress = true;

                    bwConvertMp3.RunWorkerAsync(mp3File);
                }
                catch (Exception ex)
                {
                    errorBox(ex.Message);
                }
            }
        }

        private void drawCreateButton()
        {
            btnCreateWad.Text = string.Empty;

            Image tmpImg = new Bitmap(btnCreateWad.Width, btnCreateWad.Height);
            using (Graphics gImg = Graphics.FromImage(tmpImg))
            {
                gImg.Clear(Color.Transparent);

                separatorBtn = btnCreateWad.Width * 0.5;

                gImg.DrawLine(Pens.Gray, new Point((int)separatorBtn, 0), new Point((int)separatorBtn, btnCreateWad.Height));

                Image tmpCreate = Properties.Resources.btnCreate;
                Image tmpSend = Properties.Resources.btnSend;
                gImg.DrawImage(resizeImage(tmpCreate, tmpCreate.Width, tmpCreate.Height), new Point(280, 0));
                gImg.DrawImage(resizeImage(tmpSend, tmpSend.Width, tmpSend.Height), new Point(55, 0));

                btnCreateWad.Image = tmpImg;
            }
        }

        private bool checkInet()
        {
            try
            {
                System.Net.IPHostEntry ipHost = System.Net.Dns.GetHostEntry("www.google.com");
                return true;
            }
            catch { return false; }
        }

        private void updateProgress(object sender, EventArgs e)
        {
            pbProgress.Value = currentProgress.progressValue;

            if (!string.IsNullOrEmpty(currentProgress.progressState))
            {
                lbStatusText.Text = currentProgress.progressState;
                currentProgress.progressState = string.Empty;
            }
        }

        private void tabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            tmrCredits.Stop();

            if (tabControl.SelectedTab == tabBanner)
            {
                lbxBannerTpls.SelectedIndex = -1;
                cbBannerMakeTransparent.Checked = false;
                cbBannerMakeTransparent.Enabled = false;

                try { addTpls(); }
                catch { }
            }
            else if (tabControl.SelectedTab == tabIcon)
            {
                lbxIconTpls.SelectedIndex = -1;
                cbIconMakeTransparent.Checked = false;
                cbIconMakeTransparent.Enabled = false;

                try { addTpls(); }
                catch { }
            }
            else if (tabControl.SelectedTab == tabBrlyt)
            {
                try { addBrlyts(); }
                catch { }
            }
            else if (tabControl.SelectedTab == tabBrlan)
            {
                try { addBrlans(); }
                catch { }
            }
            else if (tabControl.SelectedTab == tabCredits)
            {
                lbCreditThanks.Location = new Point(lbCreditThanks.Location.X, panCredits.Height);
                tmrCredits.Start();
            }
        }

        private void tmrCredits_Tick(object sender, EventArgs e)
        {
            if (lbCreditThanks.Location.Y == creditsReturnValue) lbCreditThanks.Location = new Point(lbCreditThanks.Location.X, panCredits.Height);
            lbCreditThanks.Location = new Point(lbCreditThanks.Location.X, lbCreditThanks.Location.Y - 1);
        }

        private void lbStatusText_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lbStatusText.Text) && pbProgress.Value == 100)
            {
                if (tabControl.SelectedTab == tabBanner ||
                    tabControl.SelectedTab == tabIcon)
                    addTpls();
                else if (tabControl.SelectedTab == tabBrlyt)
                    addBrlyts();
                else if (tabControl.SelectedTab == tabBrlan)
                    addBrlans();
            }
        }

        private void updateCheck()
        {
            if (checkInet() == true)
            {
                try
                {
                    WebClient GetVersion = new WebClient();
                    string NewVersion = GetVersion.DownloadString("https://raw.githubusercontent.com/Brawl345/customizemii/master/version.txt");

                    int newVersion = Convert.ToInt32(NewVersion.Replace(".", string.Empty).Length == 2 ? (NewVersion.Replace(".", string.Empty) + "0") : NewVersion.Replace(".", string.Empty));
                    int thisVersion = Convert.ToInt32(version.Replace(".", string.Empty).Length == 2 ? (version.Replace(".", string.Empty) + "0") : version.Replace(".", string.Empty));

                    if (newVersion > thisVersion)
                    {
                        llbUpdateAvailable.Text = llbUpdateAvailable.Text.Replace("X", NewVersion);
                        llbUpdateAvailable.Visible = true;

                        if (MessageBox.Show("Version " + NewVersion +
                            " is available.\nDo you want the download page to be opened?",
                            "Update available", MessageBoxButtons.YesNo, MessageBoxIcon.Information) ==
                            DialogResult.Yes)
                        {
                            Process.Start("https://github.com/Brawl345/customizemii/releases");
                        }
                    }
                }
                catch { }
            }
        }

        private string getInstallerVersion()
        {
            return CustomizeMiiInstaller.CustomizeMiiInstaller_Plugin.GetVersion();
        }

        private string getForwardMiiVersion()
        {
            return ForwardMii_Plugin.GetVersion();
        }

        private void llbSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                llbSite.LinkVisited = true;
                Process.Start("https://github.com/Brawl345/customizemii/");
            }
            catch (Exception ex) { errorBox(ex.Message); }
        }

        private void btnBrowseSource_Click(object sender, EventArgs e)
        {
            if (btnBrowseSource.Text.ToLower() == "clear") { initialize(); }
            else loadChannel();
        }

        private void btnLoadBaseWad_Click(object sender, EventArgs e)
        {
            if (lbxBaseWads.SelectedIndex != -1)
            {
                if (pbProgress.Value == 100)
                {
                    if (checkInet() == true)
                    {
                        if (tbSourceWad.Text != SourceWadUrls[lbxBaseWads.SelectedIndex])
                        {
                            tbSourceWad.Text = "https://raw.githubusercontent.com/Brawl345/customizemii/master/Base_WADs/" + SourceWadUrls[lbxBaseWads.SelectedIndex];

                            System.Threading.Thread dlThread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(downloadBaseWad));
                            dlThread.Start(tbSourceWad.Text);
                        }
                    }
                    else
                    {
                        errorBox("You're not connected to the Internet!");
                    }
                }
            }
        }

        private void downloadBaseWad(object uri)
        {
            try
            {
                WebClient client = new WebClient();
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                client.DownloadDataCompleted += new DownloadDataCompletedEventHandler(client_DownloadDataCompleted);

                currentProgress.progressState = "Downloading Base WAD...";
                currentProgress.progressValue = 0;
                this.Invoke(ProgressUpdate);

                client.DownloadDataAsync(new Uri((string)uri), uri);
            }
            catch (Exception ex)
            {
                setControlText(tbSourceWad, string.Empty);
                errorBox(ex.Message);
            }
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            currentProgress.progressValue = e.ProgressPercentage;
            currentProgress.progressState = "Downloading Base WAD...";
            this.Invoke(ProgressUpdate);
        }

        void client_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            currentProgress.progressValue = 0;
            currentProgress.progressState = string.Empty;
            this.Invoke(ProgressUpdate);

            if (e.Error != null)
            {
                setControlText(tbSourceWad, string.Empty);
                errorBox("The requested file couldn't be downloaded: " + e.Error.Message);
            }
            else
            {
                BackgroundWorker bwLoadChannel = new BackgroundWorker();
                bwLoadChannel.DoWork += new DoWorkEventHandler(bwLoadChannel_DoWork);
                bwLoadChannel.ProgressChanged += new ProgressChangedEventHandler(bwLoadChannel_ProgressChanged);
                bwLoadChannel.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoadChannel_RunWorkerCompleted);
                bwLoadChannel.WorkerReportsProgress = true;
                bwLoadChannel.RunWorkerAsync(e);
            }
        }

        private void btnPreviewBaseWad_Click(object sender, EventArgs e)
        {
            if (lbxBaseWads.SelectedIndex != -1)
            {
                if (checkInet() == true)
                {
                    if (!string.IsNullOrEmpty(SourceWadPreviewUrls[lbxBaseWads.SelectedIndex]))
                    {
                        try { Process.Start(SourceWadPreviewUrls[lbxBaseWads.SelectedIndex]); }
                        catch (Exception ex) { errorBox(ex.Message); }
                    }
                    else infoBox("There's no preview of this channel available, sorry!");
                }
                else errorBox("You're not connected to the Internet!");
            }
        }

        private void btnSaveBaseWad_Click(object sender, EventArgs e)
        {
            if (lbxBaseWads.SelectedIndex != -1)
            {
                if (checkInet() == true)
                {
                    if (pbProgress.Value == 100)
                    {
                        string Url = "https://raw.githubusercontent.com/Brawl345/customizemii/master/Base_WADs/" + SourceWadUrls[lbxBaseWads.SelectedIndex];
                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.Filter = "Wii Channels|*.wad";
                        sfd.FileName = Url.Remove(0, Url.LastIndexOf('/') + 1);

                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            if (tbSourceWad.Text == Url)
                            {
                                sourceWad.Save(sfd.FileName);
                                infoBox(string.Format("Saved channel as {0}", Path.GetFileName(sfd.FileName)));
                            }
                            else
                            {
                                System.Threading.Thread saveThread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(saveBaseWad));
                                saveThread.Start((object)new string[] { Url, sfd.FileName });
                            }
                        }
                    }

                }
                else errorBox("You're not connected to the Internet!");
            }
        }

        private void saveBaseWad(object urlAndSavePath)
        {
            try
            {
                WebClient saveClient = new WebClient();
                saveClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(saveClient_DownloadProgressChanged);
                saveClient.DownloadFileCompleted += new AsyncCompletedEventHandler(saveClient_DownloadFileCompleted);

                currentProgress.progressValue = 0;
                currentProgress.progressState = "Downloading Base WAD...";
                this.Invoke(ProgressUpdate);

                saveClient.DownloadFileAsync(new Uri(((string[])urlAndSavePath)[0]), ((string[])urlAndSavePath)[1]);
            }
            catch (Exception ex)
            {
                errorBox(ex.Message);
            }
        }

        void saveClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            currentProgress.progressValue = e.ProgressPercentage;
            currentProgress.progressState = "Downloading Base WAD...";
            this.Invoke(ProgressUpdate);
        }

        void saveClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            currentProgress.progressValue = 100;
            currentProgress.progressState = " ";
            this.Invoke(ProgressUpdate);
        }

        private void cmbReplace_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbReplace.SelectedIndex)
            {
                case 1:
                    tbReplace.Text = replacedIcon;
                    break;
                case 2:
                    if (replacedSound.ToLower().EndsWith(".wad") ||
                        replacedSound.ToLower().EndsWith(".app") ||
                        replacedSound.ToLower().EndsWith(".bin"))
                        tbReplace.Text = replacedSound;
                    else tbReplace.Text = string.Empty;
                    break;
                default:
                    tbReplace.Text = replacedBanner;
                    break;
            }
        }

        private void tbReplace_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbReplace.Text))
            {
                btnBrowseReplace.Text = "Browse...";
                tTip.SetToolTip(btnBrowseReplace, "Browse for a Banner / Icon / Sound to use instead of the one within the Base WAD...\nWAD's, 00000000.app's and banner.bin's / icon.bin's / sound.bin's can be loaded...");
            }
            else
            {
                btnBrowseReplace.Text = "Clear";
                tTip.SetToolTip(btnBrowseReplace, "Clear the replaced Banner / Icon / Sound and use the one within the Base WAD...");
            }
        }

        private void btnBrowseReplace_Click(object sender, EventArgs e)
        {
            if (btnBrowseReplace.Text.ToLower() == "clear")
            {
                if (cmbReplace.SelectedIndex == 2) //sound
                {
                    replacedSound = string.Empty;
                    setControlText(tbReplace, string.Empty);

                    newSoundBin = new byte[0];
                }
                else if (cmbReplace.SelectedIndex == 1) //icon
                {
                    replacedIcon = string.Empty;
                    setControlText(tbReplace, string.Empty);

                    newIconBin = new U8();

                    iconTransparents.Clear();

                    addTpls();
                    addBrlans();
                    addBrlyts();
                }
                else //banner
                {
                    replacedBanner = string.Empty;
                    setControlText(tbReplace, string.Empty);

                    newBannerBin = new U8();

                    bannerTransparents.Clear();

                    addTpls();
                    addBrlans();
                    addBrlyts();
                }
            }
            else replacePart();
        }

        private void btnBrowseDol_Click(object sender, EventArgs e)
        {
            if (btnBrowseDol.Text.ToLower() == "clear")
            {
                setControlText(tbDol, string.Empty);
                btnBrowseDol.Text = "Browse...";
                newDol = new byte[0];

                simpleForwarder.Clear();
                complexForwarder.Clear();
            }
            else cmDol.Show(MousePosition);
        }

        private void btnBrowseSound_Click(object sender, EventArgs e)
        {
            if (btnBrowseSound.Text.ToLower() == "clear")
            {
                setControlText(tbSound, string.Empty);
                btnBrowseSound.Text = "Browse...";
                newSoundBin = new byte[0];
            }
            else cmSound.Show(MousePosition);
        }

        private void lbxBannerTpls_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxBannerTpls.SelectedIndex != -1)
            {
                cbBannerMakeTransparent.Enabled = true;
                cbBannerMakeTransparent.Checked = lbxBannerTpls.SelectedItem.ToString().EndsWith("(Transparent)");

                if (lbxBannerTpls.SelectedItem.ToString().Substring(lbxBannerTpls.SelectedItem.ToString().IndexOf('(')).ToUpper().Contains("(RGBA8"))
                { cmbFormatBanner.SelectedIndex = 0; }
                else if (lbxBannerTpls.SelectedItem.ToString().Substring(lbxBannerTpls.SelectedItem.ToString().IndexOf('(')).ToUpper().Contains("(RGB565"))
                { cmbFormatBanner.SelectedIndex = 1; }
                else if (lbxBannerTpls.SelectedItem.ToString().Substring(lbxBannerTpls.SelectedItem.ToString().IndexOf('(')).ToUpper().Contains("(RGB5A3"))
                { cmbFormatBanner.SelectedIndex = 2; }
                else if (lbxBannerTpls.SelectedItem.ToString().Substring(lbxBannerTpls.SelectedItem.ToString().IndexOf('(')).ToUpper().Contains("(IA8"))
                { cmbFormatBanner.SelectedIndex = 3; }
                else if (lbxBannerTpls.SelectedItem.ToString().Substring(lbxBannerTpls.SelectedItem.ToString().IndexOf('(')).ToUpper().Contains("(IA4"))
                { cmbFormatBanner.SelectedIndex = 4; }
                else if (lbxBannerTpls.SelectedItem.ToString().Substring(lbxBannerTpls.SelectedItem.ToString().IndexOf('(')).ToUpper().Contains("(I8"))
                { cmbFormatBanner.SelectedIndex = 5; }
                else if (lbxBannerTpls.SelectedItem.ToString().Substring(lbxBannerTpls.SelectedItem.ToString().IndexOf('(')).ToUpper().Contains("(I4"))
                { cmbFormatBanner.SelectedIndex = 6; }
                else if (lbxBannerTpls.SelectedItem.ToString().Substring(lbxBannerTpls.SelectedItem.ToString().IndexOf('(')).ToUpper().Contains("(CI4"))
                { cmbFormatBanner.SelectedIndex = 7; }
                else if (lbxBannerTpls.SelectedItem.ToString().Substring(lbxBannerTpls.SelectedItem.ToString().IndexOf('(')).ToUpper().Contains("(CI8"))
                { cmbFormatBanner.SelectedIndex = 8; }
                else if (lbxBannerTpls.SelectedItem.ToString().Substring(lbxBannerTpls.SelectedItem.ToString().IndexOf('(')).ToUpper().Contains("(CI14X2"))
                { cmbFormatBanner.SelectedIndex = 9; }
                else if (lbxBannerTpls.SelectedItem.ToString().Substring(lbxBannerTpls.SelectedItem.ToString().IndexOf('(')).ToUpper().Contains("(CMP"))
                { cmbFormatBanner.SelectedIndex = 10; }
            }
            else cbBannerMakeTransparent.Enabled = false;
        }

        private void lbxIconTpls_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxIconTpls.SelectedIndex != -1)
            {
                cbIconMakeTransparent.Enabled = true;
                cbIconMakeTransparent.Checked = lbxIconTpls.SelectedItem.ToString().EndsWith("(Transparent)");

                if (lbxIconTpls.SelectedItem.ToString().Substring(lbxIconTpls.SelectedItem.ToString().IndexOf('(')).ToUpper().Contains("(RGBA8"))
                { cmbFormatIcon.SelectedIndex = 0; }
                else if (lbxIconTpls.SelectedItem.ToString().Substring(lbxIconTpls.SelectedItem.ToString().IndexOf('(')).ToUpper().Contains("(RGB565"))
                { cmbFormatIcon.SelectedIndex = 1; }
                else if (lbxIconTpls.SelectedItem.ToString().Substring(lbxIconTpls.SelectedItem.ToString().IndexOf('(')).ToUpper().Contains("(RGB5A3"))
                { cmbFormatIcon.SelectedIndex = 2; }
                else if (lbxIconTpls.SelectedItem.ToString().Substring(lbxIconTpls.SelectedItem.ToString().IndexOf('(')).ToUpper().Contains("(IA8"))
                { cmbFormatIcon.SelectedIndex = 3; }
                else if (lbxIconTpls.SelectedItem.ToString().Substring(lbxIconTpls.SelectedItem.ToString().IndexOf('(')).ToUpper().Contains("(IA4"))
                { cmbFormatIcon.SelectedIndex = 4; }
                else if (lbxIconTpls.SelectedItem.ToString().Substring(lbxIconTpls.SelectedItem.ToString().IndexOf('(')).ToUpper().Contains("(I8"))
                { cmbFormatIcon.SelectedIndex = 5; }
                else if (lbxIconTpls.SelectedItem.ToString().Substring(lbxIconTpls.SelectedItem.ToString().IndexOf('(')).ToUpper().Contains("(I4"))
                { cmbFormatIcon.SelectedIndex = 6; }
                else if (lbxIconTpls.SelectedItem.ToString().Substring(lbxIconTpls.SelectedItem.ToString().IndexOf('(')).ToUpper().Contains("(CI4"))
                { cmbFormatIcon.SelectedIndex = 7; }
                else if (lbxIconTpls.SelectedItem.ToString().Substring(lbxIconTpls.SelectedItem.ToString().IndexOf('(')).ToUpper().Contains("(CI8"))
                { cmbFormatIcon.SelectedIndex = 8; }
                else if (lbxIconTpls.SelectedItem.ToString().Substring(lbxIconTpls.SelectedItem.ToString().IndexOf('(')).ToUpper().Contains("(CI14X2"))
                { cmbFormatIcon.SelectedIndex = 9; }
                else if (lbxIconTpls.SelectedItem.ToString().Substring(lbxIconTpls.SelectedItem.ToString().IndexOf('(')).ToUpper().Contains("(CMP"))
                { cmbFormatIcon.SelectedIndex = 10; }
            }
            else cbIconMakeTransparent.Enabled = false;
        }

        private void btnReplaceBanner_Click(object sender, EventArgs e)
        {
            if (lbxBannerTpls.SelectedIndex != -1)
            {
                int format = cmbFormatBanner.SelectedIndex;

                if (format < 9)
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Filter = "PNG|*.png|JPG|*.jpg|GIF|*.gif|BMP|*.bmp|TPL|*.tpl|All|*.png;*.jpg;*.gif;*.bmp;*.tpl";
                    ofd.FilterIndex = 6;

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            string tplName = lbxBannerTpls.SelectedItem.ToString().Remove(lbxBannerTpls.SelectedItem.ToString().IndexOf('(', 0) - 3);
                            TPL tmpTpl;
                            Image img;

                            if (string.IsNullOrEmpty(replacedBanner))
                                tmpTpl = TPL.Load(bannerBin.Data[bannerBin.GetNodeIndex(tplName)]);
                            else tmpTpl = TPL.Load(newBannerBin.Data[newBannerBin.GetNodeIndex(tplName)]);

                            if (!ofd.FileName.ToLower().EndsWith(".tpl")) img = Image.FromFile(ofd.FileName);
                            else
                            {
                                TPL newTpl = TPL.Load(ofd.FileName);
                                img = newTpl.ExtractTexture();
                            }

                            if (img.Width != tmpTpl.GetTextureSize(0).Width ||
                                img.Height != tmpTpl.GetTextureSize(0).Height)
                                img = resizeImage(img, tmpTpl.GetTextureSize(0).Width, tmpTpl.GetTextureSize(0).Height);

                            TPL_PaletteFormat pFormat = TPL_PaletteFormat.RGB5A3;

                            switch (format)
                            {
                                case 6:
                                    format = (int)TPL_TextureFormat.I4;
                                    break;
                                case 5:
                                    format = (int)TPL_TextureFormat.I8;
                                    break;
                                case 4:
                                    format = (int)TPL_TextureFormat.IA4;
                                    break;
                                case 3:
                                    format = (int)TPL_TextureFormat.IA8;
                                    break;
                                case 1:
                                    format = (int)TPL_TextureFormat.RGB565;
                                    break;
                                case 2:
                                    format = (int)TPL_TextureFormat.RGB5A3;
                                    break;
                                case 7:
                                    format = (int)TPL_TextureFormat.CI4;

                                    CustomizeMii_PaletteFormatBox pfb = new CustomizeMii_PaletteFormatBox();
                                    pfb.ShowDialog();

                                    pFormat = pfb.PaletteFormat;
                                    break;
                                case 8:
                                    format = (int)TPL_TextureFormat.CI8;

                                    CustomizeMii_PaletteFormatBox pfb2 = new CustomizeMii_PaletteFormatBox();
                                    pfb2.ShowDialog();

                                    pFormat = pfb2.PaletteFormat;
                                    break;
                                case 9:
                                    format = (int)TPL_TextureFormat.CI14X2;

                                    CustomizeMii_PaletteFormatBox pfb3 = new CustomizeMii_PaletteFormatBox();
                                    pfb3.ShowDialog();

                                    pFormat = pfb3.PaletteFormat;
                                    break;
                                default:
                                    format = (int)TPL_TextureFormat.RGBA8;
                                    break;
                            }

                            tmpTpl.RemoveTexture(0);
                            tmpTpl.AddTexture(img, (TPL_TextureFormat)format, pFormat);

                            if (string.IsNullOrEmpty(replacedBanner))
                                bannerBin.ReplaceFile(bannerBin.GetNodeIndex(tplName), tmpTpl.ToByteArray());
                            else newBannerBin.ReplaceFile(newBannerBin.GetNodeIndex(tplName), tmpTpl.ToByteArray());

                            addTpls();
                        }
                        catch (Exception ex) { errorBox(ex.Message); }
                    }
                }
                else { errorBox("This format is not supported, you must choose a different one!"); }
            }
        }

        private void btnExtractBanner_Click(object sender, EventArgs e)
        {
            if (lbxBannerTpls.SelectedIndex != -1)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "PNG|*.png|JPG|*.jpg|GIF|*.gif|BMP|*.bmp|TPL|*.tpl|All|*.png;*.jpg;*.gif;*.bmp;*.tpl";
                sfd.FilterIndex = 6;
                sfd.FileName = lbxBannerTpls.SelectedItem.ToString().Remove(lbxBannerTpls.SelectedItem.ToString().IndexOf('(', 0) - 3).Replace(".tpl", string.Empty);

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string tplName = lbxBannerTpls.SelectedItem.ToString().Remove(lbxBannerTpls.SelectedItem.ToString().IndexOf('(', 0) - 3);
                        TPL tmpTpl;

                        if (string.IsNullOrEmpty(replacedBanner))
                            tmpTpl = TPL.Load(bannerBin.Data[bannerBin.GetNodeIndex(tplName)]);
                        else tmpTpl = TPL.Load(newBannerBin.Data[newBannerBin.GetNodeIndex(tplName)]);

                        if (File.Exists(sfd.FileName)) File.Delete(sfd.FileName);
                        if (!sfd.FileName.ToLower().EndsWith(".tpl"))
                        {
                            Image img = tmpTpl.ExtractTexture();

                            switch (sfd.FileName.Remove(0, sfd.FileName.LastIndexOf('.')))
                            {
                                case ".jpg":
                                    img.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    break;
                                case ".gif":
                                    img.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Gif);
                                    break;
                                case ".bmp":
                                    img.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                                    break;
                                default:
                                    img.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png);
                                    break;
                            }
                        }
                        else tmpTpl.Save(sfd.FileName);
                    }
                    catch (Exception ex) { errorBox(ex.Message); }
                }
            }
        }

        private void btnPreviewBanner_Click(object sender, EventArgs e)
        {
            if (lbxBannerTpls.SelectedIndex != -1)
            {
                try
                {
                    CustomizeMii_Preview pvw = new CustomizeMii_Preview();

                    pvw.StartTPL = lbxBannerTpls.SelectedItem.ToString().Remove(lbxBannerTpls.SelectedItem.ToString().IndexOf('(', 0) - 3);

                    if (string.IsNullOrEmpty(replacedBanner)) pvw.BannerBin = bannerBin;
                    else pvw.BannerBin = newBannerBin;
                    if (string.IsNullOrEmpty(replacedIcon)) pvw.IconBin = iconBin;
                    else pvw.IconBin = newIconBin;

                    pvw.ShowDialog();
                    pvw = null;

                    addTpls();
                }
                catch (Exception ex) { errorBox(ex.Message); }

                lbxBannerTpls.Focus();
            }
        }

        private void btnReplaceIcon_Click(object sender, EventArgs e)
        {
            if (lbxIconTpls.SelectedIndex != -1)
            {
                int format = cmbFormatIcon.SelectedIndex;

                if (format < 9)
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Filter = "PNG|*.png|JPG|*.jpg|GIF|*.gif|BMP|*.bmp|TPL|*.tpl|All|*.png;*.jpg;*.gif;*.bmp;*.tpl";
                    ofd.FilterIndex = 6;

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            string tplName = lbxIconTpls.SelectedItem.ToString().Remove(lbxIconTpls.SelectedItem.ToString().IndexOf('(', 0) - 3);
                            TPL tmpTpl;
                            Image img;

                            if (string.IsNullOrEmpty(replacedIcon))
                                tmpTpl = TPL.Load(iconBin.Data[iconBin.GetNodeIndex(tplName)]);
                            else tmpTpl = TPL.Load(newIconBin.Data[newIconBin.GetNodeIndex(tplName)]);

                            if (!ofd.FileName.ToLower().EndsWith(".tpl")) img = Image.FromFile(ofd.FileName);
                            else
                            {
                                TPL newTpl = TPL.Load(ofd.FileName);
                                img = newTpl.ExtractTexture();
                            }

                            if (img.Width != tmpTpl.GetTextureSize(0).Width ||
                                img.Height != tmpTpl.GetTextureSize(0).Height)
                                img = resizeImage(img, tmpTpl.GetTextureSize(0).Width, tmpTpl.GetTextureSize(0).Height);

                            TPL_PaletteFormat pFormat = TPL_PaletteFormat.RGB5A3;

                            switch (format)
                            {
                                case 6:
                                    format = (int)TPL_TextureFormat.I4;
                                    break;
                                case 5:
                                    format = (int)TPL_TextureFormat.I8;
                                    break;
                                case 4:
                                    format = (int)TPL_TextureFormat.IA4;
                                    break;
                                case 3:
                                    format = (int)TPL_TextureFormat.IA8;
                                    break;
                                case 1:
                                    format = (int)TPL_TextureFormat.RGB565;
                                    break;
                                case 2:
                                    format = (int)TPL_TextureFormat.RGB5A3;
                                    break;
                                case 7:
                                    format = (int)TPL_TextureFormat.CI4;

                                    CustomizeMii_PaletteFormatBox pfb = new CustomizeMii_PaletteFormatBox();
                                    pfb.ShowDialog();

                                    pFormat = pfb.PaletteFormat;
                                    break;
                                case 8:
                                    format = (int)TPL_TextureFormat.CI8;

                                    CustomizeMii_PaletteFormatBox pfb2 = new CustomizeMii_PaletteFormatBox();
                                    pfb2.ShowDialog();

                                    pFormat = pfb2.PaletteFormat;
                                    break;
                                case 9:
                                    format = (int)TPL_TextureFormat.CI14X2;

                                    CustomizeMii_PaletteFormatBox pfb3 = new CustomizeMii_PaletteFormatBox();
                                    pfb3.ShowDialog();

                                    pFormat = pfb3.PaletteFormat;
                                    break;
                                default:
                                    format = (int)TPL_TextureFormat.RGBA8;
                                    break;
                            }

                            tmpTpl.RemoveTexture(0);
                            tmpTpl.AddTexture(img, (TPL_TextureFormat)format, pFormat);

                            if (string.IsNullOrEmpty(replacedIcon))
                                iconBin.ReplaceFile(iconBin.GetNodeIndex(tplName), tmpTpl.ToByteArray());
                            else newIconBin.ReplaceFile(newIconBin.GetNodeIndex(tplName), tmpTpl.ToByteArray());

                            addTpls();
                        }
                        catch (Exception ex) { errorBox(ex.Message); }
                    }
                }
                else errorBox("This format is not supported, you must choose a different one!");
            }
        }

        private void btnExtractIcon_Click(object sender, EventArgs e)
        {
            if (lbxIconTpls.SelectedIndex != -1)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "PNG|*.png|JPG|*.jpg|GIF|*.gif|BMP|*.bmp|TPL|*.tpl|All|*.png;*.jpg;*.gif;*.bmp;*.tpl";
                sfd.FilterIndex = 6;
                sfd.FileName = lbxIconTpls.SelectedItem.ToString().Remove(lbxIconTpls.SelectedItem.ToString().IndexOf('(', 0) - 3).Replace(".tpl", string.Empty);

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string tplName = lbxIconTpls.SelectedItem.ToString().Remove(lbxIconTpls.SelectedItem.ToString().IndexOf('(', 0) - 3);
                        TPL tmpTpl;

                        if (string.IsNullOrEmpty(replacedIcon))
                            tmpTpl = TPL.Load(iconBin.Data[iconBin.GetNodeIndex(tplName)]);
                        else tmpTpl = TPL.Load(newIconBin.Data[newIconBin.GetNodeIndex(tplName)]);

                        if (File.Exists(sfd.FileName)) File.Delete(sfd.FileName);
                        if (!sfd.FileName.ToLower().EndsWith(".tpl"))
                        {
                            Image img = tmpTpl.ExtractTexture();

                            switch (sfd.FileName.Remove(0, sfd.FileName.LastIndexOf('.')))
                            {
                                case ".jpg":
                                    img.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    break;
                                case ".gif":
                                    img.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Gif);
                                    break;
                                case ".bmp":
                                    img.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                                    break;
                                default:
                                    img.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png);
                                    break;
                            }
                        }
                        else tmpTpl.Save(sfd.FileName);
                    }
                    catch (Exception ex) { errorBox(ex.Message); }
                }
            }
        }

        private void btnPreviewIcon_Click(object sender, EventArgs e)
        {
            if (lbxIconTpls.SelectedIndex != -1)
            {
                try
                {
                    CustomizeMii_Preview pvw = new CustomizeMii_Preview();

                    pvw.StartIcon = true;
                    pvw.StartTPL = lbxIconTpls.SelectedItem.ToString().Remove(lbxIconTpls.SelectedItem.ToString().IndexOf('(', 0) - 3);

                    if (string.IsNullOrEmpty(replacedBanner)) pvw.BannerBin = bannerBin;
                    else pvw.BannerBin = newBannerBin;
                    if (string.IsNullOrEmpty(replacedIcon)) pvw.IconBin = iconBin;
                    else pvw.IconBin = newIconBin;
                 
                    pvw.ShowDialog();
                    pvw = null;

                    addTpls();
                }
                catch (Exception ex) { errorBox(ex.Message); }

                lbxIconTpls.Focus();
            }
        }

        private void btnCreateWad_Click(object sender, EventArgs e)
        {
            Point mousePos = MousePosition;

            if (mousePos.X < (this.Location.X + btnCreateWad.Location.X + Math.Ceiling(separatorBtn) + 3))
            {
                //SEND WAD
                if (pbProgress.Value == 100)
                {
                    if (!string.IsNullOrEmpty(tbSourceWad.Text))
                    {
                        if (!File.Exists(Application.StartupPath + Path.DirectorySeparatorChar + "CustomizeMiiInstaller.dll"))
                            errorBox("CustomizeMiiInstaller.dll wasn't found!");
                        else
                        {
                            if (cbSecurityChecksOff.Checked == true || securityChecks() == true)
                            {
                                CustomizeMii_Transmit cmt = new CustomizeMii_Transmit();

                                if (cmt.ShowDialog() == DialogResult.OK)
                                {
                                    try
                                    {
                                        WadCreationInfo wadInfo = new WadCreationInfo();

                                        if (!int.TryParse(tbStartupIos.Text, out wadInfo.startupIos)) { errorBox("Please enter a valid startup IOS! (0 - 255)"); return; }
                                        wadInfo.titles = new string[] { tbJapanese.Text, tbEnglish.Text, tbGerman.Text, tbFrench.Text, tbSpanish.Text, tbItalian.Text, tbDutch.Text, tbKorean.Text };
                                        wadInfo.allLangTitle = tbAllLanguages.Text;
                                        wadInfo.nandLoader = (WadCreationInfo.NandLoader)cmbNandLoader.SelectedIndex;
                                        wadInfo.sendToWii = true;
                                        wadInfo.transmitProtocol = (cmt.Protocol == 1) ? Protocol.HAXX : Protocol.JODI;
                                        wadInfo.transmitIp = cmt.IPAddress;
                                        wadInfo.transmitIos = int.Parse(cmt.IOS);
                                        wadInfo.titleId = tbTitleID.Text;
                                        wadInfo.sound = tbSound.Text;
                                        wadInfo.dol = tbDol.Text;
                                        wadInfo.lz77 = cbLz77.Checked;

                                        BackgroundWorker bwCreateWad = new BackgroundWorker();
                                        bwCreateWad.DoWork += new DoWorkEventHandler(bwCreateWad_DoWork);
                                        bwCreateWad.ProgressChanged += new ProgressChangedEventHandler(bwCreateWad_ProgressChanged);
                                        bwCreateWad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwCreateWad_RunWorkerCompleted);
                                        bwCreateWad.WorkerReportsProgress = true;
                                        bwCreateWad.RunWorkerAsync(wadInfo);
                                    }
                                    catch (Exception ex) { errorBox(ex.Message); }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                //CREATE WAD
                if (pbProgress.Value == 100)
                {
                    if (!string.IsNullOrEmpty(tbSourceWad.Text))
                    {
                        if (cbSecurityChecksOff.Checked == true || securityChecks() == true)
                        {
                            SaveFileDialog sfd = new SaveFileDialog();
                            sfd.Filter = "Wii Channels|*.wad";

                            string fileName;

                            if (!string.IsNullOrEmpty(tbAllLanguages.Text))
                                fileName = tbAllLanguages.Text + " - " + tbTitleID.Text.ToUpper() + ".wad";
                            else
                                fileName = tbEnglish.Text + " - " + tbTitleID.Text.ToUpper() + ".wad";

                            foreach (char invalidChar in Path.GetInvalidFileNameChars())
                                fileName = fileName.Replace(invalidChar.ToString(), string.Empty);

                            sfd.FileName = fileName;

                            if (sfd.ShowDialog() == DialogResult.OK)
                            {
                                try
                                {
                                    WadCreationInfo wadInfo = new WadCreationInfo();

                                    if (!int.TryParse(tbStartupIos.Text, out wadInfo.startupIos)) { errorBox("Please enter a valid startup IOS! (0 - 255)"); return; }
                                    wadInfo.titles = new string[] { tbJapanese.Text, tbEnglish.Text, tbGerman.Text, tbFrench.Text, tbSpanish.Text, tbItalian.Text, tbDutch.Text, tbKorean.Text };
                                    wadInfo.allLangTitle = tbAllLanguages.Text;
                                    wadInfo.titleId = tbTitleID.Text;
                                    wadInfo.sound = tbSound.Text;
                                    wadInfo.dol = tbDol.Text;
                                    wadInfo.lz77 = cbLz77.Checked;

                                    wadInfo.outFile = sfd.FileName;
                                    wadInfo.nandLoader = (WadCreationInfo.NandLoader)cmbNandLoader.SelectedIndex;

                                    BackgroundWorker bwCreateWad = new BackgroundWorker();
                                    bwCreateWad.DoWork += new DoWorkEventHandler(bwCreateWad_DoWork);
                                    bwCreateWad.ProgressChanged += new ProgressChangedEventHandler(bwCreateWad_ProgressChanged);
                                    bwCreateWad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwCreateWad_RunWorkerCompleted);
                                    bwCreateWad.WorkerReportsProgress = true;
                                    bwCreateWad.RunWorkerAsync(wadInfo);

                                    return;
                                }
                                catch (Exception ex)
                                {
                                    errorBox(ex.Message);
                                }
                            }
                        }
                    }
                }

                currentProgress.progressValue = 100;
                currentProgress.progressState = " ";
                this.Invoke(ProgressUpdate);
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
                        if (lbBrlanActions.Text.ToLower() == "banner")
                        {
                            for (int i = 0; i < lbxBrlanBanner.Items.Count; i++)
                                if (lbxBrlanBanner.Items[i].ToString().ToLower() == Path.GetFileName(ofd.FileName).ToLower())
                                { errorBox("This file already exists, use the Replace button!"); return; }

                            if (string.IsNullOrEmpty(replacedBanner))
                            {
                                bannerBin.AddFile("/arc/anim/" + Path.GetFileName(ofd.FileName), File.ReadAllBytes(ofd.FileName));
                            }
                            else
                            {
                                newBannerBin.AddFile("/arc/anim/" + Path.GetFileName(ofd.FileName), File.ReadAllBytes(ofd.FileName));
                            }

                            brlanChanged = true;
                            addBrlans();
                        }
                        else
                        {
                            for (int i = 0; i < lbxBrlanIcon.Items.Count; i++)
                                if (lbxBrlanIcon.Items[i].ToString().ToLower() == Path.GetFileName(ofd.FileName).ToLower())
                                { errorBox("This file already exists, use the Replace button!"); return; }

                            if (string.IsNullOrEmpty(replacedIcon))
                            {
                                iconBin.AddFile("/arc/anim/" + Path.GetFileName(ofd.FileName), File.ReadAllBytes(ofd.FileName));
                            }
                            else
                            {
                                newIconBin.AddFile("/arc/anim/" + Path.GetFileName(ofd.FileName), File.ReadAllBytes(ofd.FileName)); ;
                            }

                            brlanChanged = true;
                            addBrlans();
                        }
                    }
                    catch (Exception ex) { errorBox(ex.Message); }
                }
            }
        }

        private void btnBrlytExtract_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lbBrlytActions.Text))
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "brlyt Files|*.brlyt";

                if (lbBrlytActions.Text.ToLower() == "banner")
                    sfd.FileName = lbxBrlytBanner.SelectedItem.ToString();
                else
                    sfd.FileName = lbxBrlytIcon.SelectedItem.ToString();

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (lbBrlytActions.Text.ToLower() == "banner")
                        {
                            if (string.IsNullOrEmpty(replacedBanner))
                                File.WriteAllBytes(sfd.FileName, bannerBin.Data[bannerBin.GetNodeIndex(lbxBrlytBanner.SelectedItem.ToString())]);
                            else
                                File.WriteAllBytes(sfd.FileName, newBannerBin.Data[newBannerBin.GetNodeIndex(lbxBrlytBanner.SelectedItem.ToString())]);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(replacedIcon))
                                File.WriteAllBytes(sfd.FileName, iconBin.Data[iconBin.GetNodeIndex(lbxBrlytIcon.SelectedItem.ToString())]);
                            else
                                File.WriteAllBytes(sfd.FileName, newIconBin.Data[newIconBin.GetNodeIndex(lbxBrlytIcon.SelectedItem.ToString())]);
                        }
                    }
                    catch (Exception ex) { errorBox(ex.Message); }
                }
            }
        }

        private void btnBrlanExtract_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lbBrlanActions.Text))
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "brlan Files|*.brlan";

                if (lbBrlanActions.Text.ToLower() == "banner")
                    sfd.FileName = lbxBrlanBanner.SelectedItem.ToString();
                else
                    sfd.FileName = lbxBrlanIcon.SelectedItem.ToString();

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (lbBrlanActions.Text.ToLower() == "banner")
                        {
                            if (string.IsNullOrEmpty(replacedBanner))
                                File.WriteAllBytes(sfd.FileName, bannerBin.Data[bannerBin.GetNodeIndex(lbxBrlanBanner.SelectedItem.ToString())]);
                            else
                                File.WriteAllBytes(sfd.FileName, newBannerBin.Data[newBannerBin.GetNodeIndex(lbxBrlanBanner.SelectedItem.ToString())]);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(replacedIcon))
                                File.WriteAllBytes(sfd.FileName, iconBin.Data[iconBin.GetNodeIndex(lbxBrlanIcon.SelectedItem.ToString())]);
                            else
                                File.WriteAllBytes(sfd.FileName, newIconBin.Data[newIconBin.GetNodeIndex(lbxBrlanIcon.SelectedItem.ToString())]);
                        }
                    }
                    catch (Exception ex) { errorBox(ex.Message); }
                }
            }
        }

        private void btnBrlanDelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lbBrlanActions.Text))
            {
                try
                {
                    if (lbBrlanActions.Text.ToLower() == "banner")
                    {
                        if (lbxBrlanBanner.Items.Count > 1)
                        {
                            if (string.IsNullOrEmpty(replacedBanner))
                            {
                                bannerBin.RemoveFile("/arc/anim/" + lbxBrlanBanner.SelectedItem.ToString());
                            }
                            else
                            {
                                newBannerBin.RemoveFile("/arc/anim/" + lbxBrlanBanner.SelectedItem.ToString());
                            }

                            brlanChanged = true;
                            addBrlans();
                        }
                        else
                            errorBox("You can't delete the last file.\nAdd a new one first in order to delete this one.");
                    }
                    else
                    {
                        if (lbxBrlanIcon.Items.Count > 1)
                        {
                            if (string.IsNullOrEmpty(replacedIcon))
                            {
                                iconBin.RemoveFile("/arc/anim/" + lbxBrlanBanner.SelectedItem.ToString());
                            }
                            else
                            {
                                newIconBin.RemoveFile("/arc/anim/" + lbxBrlanBanner.SelectedItem.ToString());
                            }

                            brlanChanged = true;
                            addBrlans();
                        }
                        else
                            errorBox("You can't delete the last file.\nAdd a new one first in order to delete this one.");
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
                        if (lbBrlytActions.Text.ToLower() == "banner")
                        {
                            if (string.IsNullOrEmpty(replacedBanner))
                                bannerBin.ReplaceFile(bannerBin.GetNodeIndex(lbxBrlytBanner.SelectedItem.ToString()), File.ReadAllBytes(ofd.FileName));
                            else
                                newBannerBin.ReplaceFile(newBannerBin.GetNodeIndex(lbxBrlytBanner.SelectedItem.ToString()), File.ReadAllBytes(ofd.FileName));

                            brlytChanged = true;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(replacedIcon))
                                iconBin.ReplaceFile(iconBin.GetNodeIndex(lbxBrlytIcon.SelectedItem.ToString()), File.ReadAllBytes(ofd.FileName));
                            else
                                newIconBin.ReplaceFile(newIconBin.GetNodeIndex(lbxBrlytIcon.SelectedItem.ToString()), File.ReadAllBytes(ofd.FileName));

                            brlytChanged = true;
                        }
                    }
                    catch (Exception ex) { errorBox(ex.Message); }
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
                        if (lbBrlanActions.Text.ToLower() == "banner")
                        {
                            if (string.IsNullOrEmpty(replacedBanner))
                                bannerBin.ReplaceFile(bannerBin.GetNodeIndex(lbxBrlanBanner.SelectedItem.ToString()), File.ReadAllBytes(ofd.FileName));
                            else
                                newBannerBin.ReplaceFile(newBannerBin.GetNodeIndex(lbxBrlanBanner.SelectedItem.ToString()), File.ReadAllBytes(ofd.FileName));

                            brlanChanged = true;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(replacedIcon))
                                iconBin.ReplaceFile(iconBin.GetNodeIndex(lbxBrlanIcon.SelectedItem.ToString()), File.ReadAllBytes(ofd.FileName));
                            else
                                newIconBin.ReplaceFile(newIconBin.GetNodeIndex(lbxBrlanIcon.SelectedItem.ToString()), File.ReadAllBytes(ofd.FileName));

                            brlanChanged = true;
                        }
                    }
                    catch (Exception ex) { errorBox(ex.Message); }
                }
            }
        }

        private void llbUpdateAvailable_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                llbUpdateAvailable.LinkVisited = true;
                Process.Start("https://github.com/Brawl345/customizemii/releases");
            }
            catch (Exception ex) { errorBox(ex.Message); }
        }

        private void btnDeleteBanner_Click(object sender, EventArgs e)
        {
            if (lbxBannerTpls.SelectedIndex != -1)
            {
                int idx = lbxBannerTpls.SelectedIndex;

                try
                {
                    string tplName = lbxBannerTpls.SelectedItem.ToString().Remove(lbxBannerTpls.SelectedItem.ToString().IndexOf('(', 0) - 3);
                    string[] requiredTpls = new string[0];

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

                    if (!Array.Exists(requiredTpls, thisTpl => thisTpl.ToLower() == tplName.ToLower()) || cbSecurityChecksOff.Checked)
                    {
                        if (string.IsNullOrEmpty(replacedBanner))
                        {
                            bannerBin.RemoveFile("/arc/timg/" + tplName);
                        }
                        else
                        {
                            newBannerBin.RemoveFile("/arc/timg/" + tplName);
                        }

                        addTpls();
                    }
                    else
                        errorBox("This TPL is required by your banner.brlyt and thus can't be deleted!\nYou can still replace the image using the Replace button!");
                }
                catch (Exception ex) { errorBox(ex.Message); }

                for (; ; )
                {
                    try
                    {
                        lbxBannerTpls.SelectedIndex = idx;
                        break;
                    }
                    catch { idx--; }

                    if (idx < -1) break;
                }
            }

            lbxBannerTpls.Select();
        }

        private void btnDeleteIcon_Click(object sender, EventArgs e)
        {
            if (lbxIconTpls.SelectedIndex != -1)
            {
                int idx = lbxIconTpls.SelectedIndex;

                try
                {
                    string tplName = lbxIconTpls.SelectedItem.ToString().Remove(lbxIconTpls.SelectedItem.ToString().IndexOf('(', 0) - 3);
                    string[] requiredTpls = new string[0];

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

                    if (!Array.Exists(requiredTpls, thisTpl => thisTpl.ToLower() == tplName.ToLower()) || cbSecurityChecksOff.Checked)
                    {
                        if (string.IsNullOrEmpty(replacedIcon))
                        {
                            iconBin.RemoveFile("/arc/timg/" + tplName);
                        }
                        else
                        {
                            newIconBin.RemoveFile("/arc/timg/" + tplName);
                        }

                        addTpls();
                    }
                    else
                        errorBox("This TPL is required by your icon.brlyt and thus can't be deleted!\nYou can still replace the image using the Replace button!");
                }
                catch (Exception ex) { errorBox(ex.Message); }

                for (; ; )
                {
                    try
                    {
                        lbxIconTpls.SelectedIndex = idx;
                        break;
                    }
                    catch { idx--; }

                    if (idx < -1) break;
                }
            }

            lbxIconTpls.Select();
        }

        private void btnAddBanner_Click(object sender, EventArgs e)
        {
            try { addTpl(lbxBannerTpls); }
            catch (Exception ex) { errorBox(ex.Message); }
        }

        private void btnAddIcon_Click(object sender, EventArgs e)
        {
            try { addTpl(lbxIconTpls); }
            catch (Exception ex) { errorBox(ex.Message); }
        }

        private void btnBrlytListTpls_Click(object sender, EventArgs e)
        {
            if (lbBrlytActions.Text.ToLower() == "banner")
            {
                string[] requiredTpls = new string[0];

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

                MessageBox.Show("These are the TPLs required by your banner.brlyt:\n\n" +
                    string.Join("\n", requiredTpls), "TPLs specified in banner.brlyt", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                string[] requiredTpls = new string[0];

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

                MessageBox.Show("These are the TPLs required by your icon.brlyt:\n\n" +
                    string.Join("\n", requiredTpls), "TPLs specified in icon.brlyt", MessageBoxButtons.OK,
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
                            string thisItem = lbxBannerTpls.SelectedItem.ToString();
                            lbxBannerTpls.Items.Remove(lbxBannerTpls.SelectedItem);
                            lbxBannerTpls.Items.Add(thisItem + " (Transparent)");
                            lbxBannerTpls.SelectedItem = thisItem + " (Transparent)";
                            bannerTransparents.Add(thisItem.Remove(thisItem.IndexOf('(', 0) - 3));
                        }
                    }
                    catch { }
                }
                else
                {
                    try
                    {
                        string thisItem = lbxBannerTpls.SelectedItem.ToString();
                        lbxBannerTpls.Items.Remove(lbxBannerTpls.SelectedItem);
                        lbxBannerTpls.Items.Add(thisItem.Replace(" (Transparent)", string.Empty));
                        lbxBannerTpls.SelectedItem = thisItem.Replace(" (Transparent)", string.Empty);
                        bannerTransparents.Remove(thisItem.Remove(thisItem.IndexOf('(', 0) - 3));
                    }
                    catch { }
                }

                lbxBannerTpls.Focus();
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
                            string thisItem = lbxIconTpls.SelectedItem.ToString();
                            lbxIconTpls.Items.Remove(lbxIconTpls.SelectedItem);
                            lbxIconTpls.Items.Add(thisItem + " (Transparent)");
                            lbxIconTpls.SelectedItem = thisItem + " (Transparent)";
                            iconTransparents.Add(thisItem.Remove(thisItem.IndexOf('(', 0) - 3));
                        }
                    }
                    catch { }
                }
                else
                {
                    try
                    {
                        string thisItem = lbxIconTpls.SelectedItem.ToString();
                        lbxIconTpls.Items.Remove(lbxIconTpls.SelectedItem);
                        lbxIconTpls.Items.Add(thisItem.Replace(" (Transparent)", string.Empty));
                        lbxIconTpls.SelectedItem = thisItem.Replace(" (Transparent)", string.Empty);
                        iconTransparents.Remove(thisItem.Remove(thisItem.IndexOf('(', 0) - 3));
                    }
                    catch { }
                }

                lbxIconTpls.Focus();
            }
        }

        private void btnForwarder_Click(object sender, EventArgs e)
        {
            if (checkForForwardMii() == true)
            {
                if (checkDevKit() == false)
                    forwarderDialogSimple();
                else
                    cmForwarder.Show(MousePosition);
            }
            else errorBox("ForwardMii.dll wasn't found!");
        }

        private void cmForwarderItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem cmSender = sender as ToolStripMenuItem;

            if (cmSender == cmSimpleForwarder)
                forwarderDialogSimple();
            else
            {
                try { forwarderDialogComplex(); }
                catch (Exception ex) { errorBox(ex.Message); }
            }
        }

        private void btnOptionsExtract_Click(object sender, EventArgs e)
        {
            cmOptionsExtract.Show(MousePosition);
        }

        private void cmOptionsExtract_MouseClick(object sender, EventArgs e)
        {
            ToolStripMenuItem cmSender = sender as ToolStripMenuItem;

            if (cmSender.OwnerItem == tsExtractImages)
            {
                try
                {
                    FolderBrowserDialog fbd = new FolderBrowserDialog();

                    if (cmSender.Text.ToLower() == "both") { fbd.Description = "Select the path where the images will be extracted to. Two folders \"Banner\" and \"Icon\" will be created."; }
                    else { fbd.Description = "Select the path where the images will be extracted to."; }

                    if (fbd.ShowDialog() == DialogResult.OK)
                    {
                        string bannerPath;
                        string iconPath;

                        switch (cmSender.Text.ToLower())
                        {
                            case "banner":
                                bannerPath = fbd.SelectedPath;
                                iconPath = string.Empty;
                                break;
                            case "icon":
                                bannerPath = string.Empty;
                                iconPath = fbd.SelectedPath;
                                break;
                            default: //both
                                bannerPath = fbd.SelectedPath + Path.DirectorySeparatorChar + "Banner";
                                iconPath = fbd.SelectedPath + Path.DirectorySeparatorChar + "Icon";
                                break;
                        }

                        if (!string.IsNullOrEmpty(bannerPath))
                        {
                            if (!Directory.Exists(bannerPath)) Directory.CreateDirectory(bannerPath);

                            if (string.IsNullOrEmpty(replacedBanner))
                            {
                                for (int i = 0; i < bannerBin.NumOfNodes; i++)
                                {
                                    if (bannerBin.StringTable[i].ToLower().EndsWith(".tpl"))
                                    {
                                        Image img = TPL.Load(bannerBin.Data[i]).ExtractTexture();
                                        img.Save(bannerPath + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(bannerBin.StringTable[i]) + ".png");
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < newBannerBin.NumOfNodes; i++)
                                {
                                    if (newBannerBin.StringTable[i].ToLower().EndsWith(".tpl"))
                                    {
                                        Image img = TPL.Load(newBannerBin.Data[i]).ExtractTexture();
                                        img.Save(bannerPath + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(newBannerBin.StringTable[i]) + ".png");
                                    }
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(iconPath))
                        {
                            if (!Directory.Exists(iconPath)) Directory.CreateDirectory(iconPath);

                            if (string.IsNullOrEmpty(replacedIcon))
                            {
                                for (int i = 0; i < iconBin.NumOfNodes; i++)
                                {
                                    if (iconBin.StringTable[i].ToLower().EndsWith(".tpl"))
                                    {
                                        Image img = TPL.Load(iconBin.Data[i]).ExtractTexture();
                                        img.Save(iconPath + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(iconBin.StringTable[i]) + ".png");
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < newIconBin.NumOfNodes; i++)
                                {
                                    if (newIconBin.StringTable[i].ToLower().EndsWith(".tpl"))
                                    {
                                        Image img = TPL.Load(newIconBin.Data[i]).ExtractTexture();
                                        img.Save(iconPath + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(newIconBin.StringTable[i]) + ".png");
                                    }
                                }
                            }
                        }

                        infoBox("Extracted images successfully!");
                    }
                }
                catch (Exception ex) { errorBox(ex.Message); }
            }
            else if (cmSender.OwnerItem == tsExtractSound)
            {
                try
                {
                    byte[] soundFile = new byte[0];

                    if (string.IsNullOrEmpty(replacedSound))
                    {
                        for (int i = 0; i < sourceWad.BannerApp.NumOfNodes; i++)
                        {
                            if (sourceWad.BannerApp.StringTable[i].ToLower() == "sound.bin")
                                soundFile = sourceWad.BannerApp.Data[i];
                        }
                    }
                    else
                    { soundFile = newSoundBin; }

                    SaveFileDialog sfd = new SaveFileDialog();
                    if (cmSender.Name.ToLower() == "cmextractsoundasbin") { sfd.Filter = "BIN|*.bin"; sfd.FileName = "sound.bin"; }
                    else if (cmSender.Name.ToLower() == "cmextractsoundasaudio")
                    {
                        if (soundFile[32] == 'R' && soundFile[33] == 'I' && soundFile[34] == 'F' && soundFile[35] == 'F')
                        { sfd.Filter = "Wave|*.wav"; sfd.FileName = "sound.wav"; }
                        else if (soundFile[32] == 'B' && soundFile[33] == 'N' && soundFile[34] == 'S' && soundFile[35] == ' ')
                        { sfd.Filter = "Wave|*.wav"; sfd.FileName = "sound.wav"; }
                        else if (soundFile[32] == 'F' && soundFile[33] == 'O' && soundFile[34] == 'R' && soundFile[35] == 'M')
                        { sfd.Filter = "AIFF|*.aif;*.aiff"; sfd.FileName = "sound.aif"; }
                        else if (soundFile[32] == 'L' && soundFile[33] == 'Z' && soundFile[34] == '7' && soundFile[35] == '7')
                        {
                            if (soundFile[41] == 'R' && soundFile[42] == 'I' && soundFile[43] == 'F' && soundFile[44] == 'F')
                            { sfd.Filter = "Wave|*.wav"; sfd.FileName = "sound.wav"; }
                            else if (soundFile[41] == 'B' && soundFile[42] == 'N' && soundFile[43] == 'S' && soundFile[44] == ' ')
                            { sfd.Filter = "Wave|*.wav"; sfd.FileName = "sound.wave"; }
                            else if (soundFile[41] == 'F' && soundFile[42] == 'O' && soundFile[43] == 'R' && soundFile[44] == 'M')
                            { sfd.Filter = "AIFF|*.aif;*.aiff"; sfd.FileName = "sound.aif"; }
                            else throw new Exception("Unsupported Audio Format!");
                        }
                        else throw new Exception("Unsupported Audio Format!");
                    }

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        if (File.Exists(sfd.FileName)) File.Delete(sfd.FileName);

                        if (sfd.FileName.ToLower().EndsWith(".bin"))
                        {
                            File.WriteAllBytes(sfd.FileName, soundFile);
                            infoBox("The sound.bin was successfully extraced!");
                        }
                        else
                        {
                            soundFile = Headers.IMD5.RemoveHeader(soundFile);

                            if (Lz77.IsLz77Compressed(soundFile))
                            { Lz77 l = new Lz77(); soundFile = l.Decompress(soundFile); }

                            if (soundFile[0] == 'B' && soundFile[1] == 'N' && soundFile[2] == 'S' && soundFile[3] == ' ')
                            {
                                Wave w = BNS.BnsToWave(soundFile);
                                w.Save(sfd.FileName);
                                w.Dispose();
                            }
                            else
                                File.WriteAllBytes(sfd.FileName, soundFile);

                            infoBox(string.Format("The sound.bin was successfully converted to {0}!", Path.GetFileName(sfd.FileName)));
                        }
                    }
                }
                catch (Exception ex) { errorBox(ex.Message); }
            }
            else if (cmSender.OwnerItem == tsExtractBrl)
            {
                try
                {
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    fbd.Description = "Select the path where the files will be extracted to.";

                    if (fbd.ShowDialog() == DialogResult.OK)
                    {
                        if (cmSender == cmExtractBrlyt || cmSender == cmExtractBothBrl)
                        {
                            //Extract brlyts
                            if (string.IsNullOrEmpty(replacedBanner))
                            {
                                for (int i = 0; i < bannerBin.NumOfNodes; i++)
                                {
                                    if (bannerBin.StringTable[i].ToLower().EndsWith(".brlyt"))
                                    { File.WriteAllBytes(fbd.SelectedPath + Path.DirectorySeparatorChar + bannerBin.StringTable[i], bannerBin.Data[i]); }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < newBannerBin.NumOfNodes; i++)
                                {
                                    if (newBannerBin.StringTable[i].ToLower().EndsWith(".brlyt"))
                                    { File.WriteAllBytes(fbd.SelectedPath + Path.DirectorySeparatorChar + newBannerBin.StringTable[i], newBannerBin.Data[i]); }
                                }
                            }

                            if (string.IsNullOrEmpty(replacedIcon))
                            {
                                for (int i = 0; i < iconBin.NumOfNodes; i++)
                                {
                                    if (iconBin.StringTable[i].ToLower().EndsWith(".brlyt"))
                                    { File.WriteAllBytes(fbd.SelectedPath + Path.DirectorySeparatorChar + iconBin.StringTable[i], iconBin.Data[i]); }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < newIconBin.NumOfNodes; i++)
                                {
                                    if (newIconBin.StringTable[i].ToLower().EndsWith(".brlyt"))
                                    { File.WriteAllBytes(fbd.SelectedPath + Path.DirectorySeparatorChar + newIconBin.StringTable[i], newIconBin.Data[i]); }
                                }
                            }
                        }

                        if (cmSender == cmExtractBothBrl || cmSender == cmExtractBrlan)
                        {
                            //Extract brlans
                            if (string.IsNullOrEmpty(replacedBanner))
                            {
                                for (int i = 0; i < bannerBin.NumOfNodes; i++)
                                {
                                    if (bannerBin.StringTable[i].ToLower().EndsWith(".brlan"))
                                    { File.WriteAllBytes(fbd.SelectedPath + Path.DirectorySeparatorChar + bannerBin.StringTable[i], bannerBin.Data[i]); }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < newBannerBin.NumOfNodes; i++)
                                {
                                    if (newBannerBin.StringTable[i].ToLower().EndsWith(".brlan"))
                                    { File.WriteAllBytes(fbd.SelectedPath + Path.DirectorySeparatorChar + newBannerBin.StringTable[i], newBannerBin.Data[i]); }
                                }
                            }

                            if (string.IsNullOrEmpty(replacedIcon))
                            {
                                for (int i = 0; i < iconBin.NumOfNodes; i++)
                                {
                                    if (iconBin.StringTable[i].ToLower().EndsWith(".brlan"))
                                    { File.WriteAllBytes(fbd.SelectedPath + Path.DirectorySeparatorChar + iconBin.StringTable[i], iconBin.Data[i]); }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < newIconBin.NumOfNodes; i++)
                                {
                                    if (newIconBin.StringTable[i].ToLower().EndsWith(".brlan"))
                                    { File.WriteAllBytes(fbd.SelectedPath + Path.DirectorySeparatorChar + newIconBin.StringTable[i], newIconBin.Data[i]); }
                                }
                            }
                        }

                        infoBox("Extracted files successfully!");
                    }
                }
                catch (Exception ex) { errorBox(ex.Message); }
            }
            else //DOL
            {
                try
                {
                    if (sourceWad.NumOfContents == 3)
                    {
                        int appIndex = 0;
                        if (sourceWad.BootIndex == 1) appIndex = 2;
                        else if (sourceWad.BootIndex == 2) appIndex = 1;

                        if (appIndex > 0)
                        {
                            SaveFileDialog sfd = new SaveFileDialog();
                            sfd.Filter = "Wii Executables|*.dol"; sfd.FileName = (string.IsNullOrEmpty(tbAllLanguages.Text) ? tbEnglish.Text : tbAllLanguages.Text) + ".dol";

                            if (sfd.ShowDialog() == DialogResult.OK)
                            {
                                if (File.Exists(sfd.FileName)) File.Delete(sfd.FileName);
                                File.WriteAllBytes(sfd.FileName, sourceWad.Contents[appIndex]);
                                infoBox(string.Format("The DOL file was successfully extracted to {0}!", Path.GetFileName(sfd.FileName)));
                            }
                        }
                        else errorBox("The DOL file couldn't be found!");
                    }
                    else errorBox("The DOL file couldn't be found!");
                }
                catch (Exception ex) { errorBox(ex.Message); }
            }
        }

        private void cmLoadAudioFile_Click(object sender, EventArgs e)
        {
            if (pbProgress.Value == 100)
            {
                OpenFileDialog ofd = new OpenFileDialog();

                if (File.Exists(Application.StartupPath + Path.DirectorySeparatorChar + "lame.exe"))
                {
                    ofd.Filter = "Wave Files|*.wav|Mp3 Files|*.mp3|BNS Files|*.bns|All|*.wav;*.mp3;*.bns";
                    ofd.FilterIndex = 4;
                }
                else
                {
                    ofd.Filter = "Wave Files|*.wav|BNS Files|*.bns|All|*.wav;*.bns";
                    ofd.FilterIndex = 3;
                }

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (ofd.FileName.ToLower().EndsWith(".mp3"))
                    {
                        convertMp3ToWave(ofd.FileName);
                    }
                    else
                    {
                        replacedSound = ofd.FileName;
                        setControlText(tbSound, ofd.FileName);
                        btnBrowseSound.Text = "Clear";

                        newSoundBin = Headers.IMD5.AddHeader(File.ReadAllBytes(ofd.FileName));

                        if (cmbReplace.SelectedIndex == 2) setControlText(tbReplace, string.Empty);
                    }
                }
            }
        }

        private void cmConvertToBns_Click(object sender, EventArgs e)
        {
            if (pbProgress.Value == 100)
            {
                CustomizeMii_BnsConvert bnsConvert = new CustomizeMii_BnsConvert(File.Exists(Application.StartupPath + Path.DirectorySeparatorChar + "lame.exe"));

                for (int i = 0; i < sourceWad.BannerApp.NumOfNodes; i++)
                {
                    if (sourceWad.BannerApp.StringTable[i].ToLower() == "sound.bin")
                        bnsConvert.SourceSound = Headers.IMD5.RemoveHeader(sourceWad.BannerApp.Data[i]);
                }

                if (bnsConvert.ShowDialog() == DialogResult.OK)
                {
                    BnsConversionInfo bnsInfo = new BnsConversionInfo();

                    bnsInfo.audioFile = bnsConvert.AudioFile;
                    bnsInfo.stereoToMono = false;

                    if (bnsConvert.ChannelCount == 2)
                    {
                        if (MessageBox.Show("Do you want to convert the stereo Wave file to a mono BNS file?\nOnly the left channel will be taken.",
                            "Convert to Mono?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            bnsInfo.stereoToMono = true;
                    }

                    if (bnsConvert.LoopFromAudio)
                    { bnsInfo.loopType = BnsConversionInfo.LoopType.FromWave; }
                    else if (bnsConvert.LoopManually)
                    { bnsInfo.loopStartSample = bnsConvert.LoopStartSample; bnsInfo.loopType = BnsConversionInfo.LoopType.Manual; }
                    else bnsInfo.loopType = BnsConversionInfo.LoopType.None;

                    BackgroundWorker bwConvertToBns = new BackgroundWorker();
                    bwConvertToBns.WorkerReportsProgress = true;
                    bwConvertToBns.DoWork += new DoWorkEventHandler(bwConvertToBns_DoWork);
                    bwConvertToBns.ProgressChanged += new ProgressChangedEventHandler(bwConvertToBns_ProgressChanged);
                    bwConvertToBns.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwConvertToBns_RunWorkerCompleted);

                    bwConvertToBns.RunWorkerAsync(bnsInfo);
                }
            }
        }

        private void cmExtractWad_Click(object sender, EventArgs e)
        {
            string unpackDir = string.Format("{0} - {1}", sourceWad.ChannelTitles[1], sourceWad.UpperTitleID);

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = string.Format("Choose the path where the WAD will be extracted to. A folder called \"{0}\" containing the contents will be created!", unpackDir);

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                if (!Directory.Exists(fbd.SelectedPath + Path.DirectorySeparatorChar + unpackDir)) Directory.CreateDirectory(fbd.SelectedPath + Path.DirectorySeparatorChar + unpackDir);
                sourceWad.Unpack(fbd.SelectedPath + Path.DirectorySeparatorChar + unpackDir, false);

                infoBox("Successfully extracted WAD file!");
            }
        }

        private void cmLoadDol_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Wii Executables|*.dol|Wii Channels|*.wad|All|*.dol;*.wad";
            ofd.FilterIndex = 3;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (ofd.FileName.ToLower().EndsWith(".wad"))
                {
                    try
                    {
                        WAD tmpWad = WAD.Load(ofd.FileName);

                        if (tmpWad.NumOfContents == 3)
                        {
                            int appIndex = 0;
                            if (tmpWad.BootIndex == 1) appIndex = 2;
                            else if (tmpWad.BootIndex == 2) appIndex = 1;

                            if (appIndex > 0)
                            {
                                newDol = tmpWad.Contents[appIndex];
                                setControlText(tbDol, ofd.FileName);
                                btnBrowseDol.Text = "Clear";
                            }
                            else errorBox("The DOL file couldn't be found!");
                        }
                        else errorBox("The DOL file couldn't be found!");
                    }
                    catch (Exception ex)
                    {
                        setControlText(tbDol, string.Empty);
                        btnBrowseDol.Text = "Browse...";
                        errorBox(ex.Message);
                    }
                }
                else
                {
                    newDol = File.ReadAllBytes(ofd.FileName);
                    setControlText(tbDol, ofd.FileName);
                    btnBrowseDol.Text = "Clear";
                }
            }
        }

        private void cmDolFromSource_Click(object sender, EventArgs e)
        {
            try
            {
                if (sourceWad.NumOfContents == 3)
                {
                    int appIndex = 0;
                    if (sourceWad.BootIndex == 1) appIndex = 2;
                    else if (sourceWad.BootIndex == 2) appIndex = 1;

                    if (appIndex > 0)
                    {
                        newDol = sourceWad.Contents[appIndex];
                        setControlText(tbDol, "Internal");
                        btnBrowseDol.Text = "Clear";
                    }
                    else errorBox("The DOL file couldn't be found!");
                }
                else errorBox("The DOL file couldn't be found!");
            }
            catch (Exception ex)
            {
                setControlText(tbDol, string.Empty);
                btnBrowseDol.Text = "Browse...";
                errorBox(ex.Message);
            }
        }

        private void llbTranslateChannel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!tbAllLanguages.Text.ToLower().Contains("channel"))
            {
                tbAllLanguages.Focus();
                tbAllLanguages.SelectAll();
                return;
            }

            string tempText = tbAllLanguages.Text;
            int startIndex = tempText.ToLower().IndexOf("channel");

            try
            {
                tbEnglish.Text = tempText.Remove(startIndex, 7).Insert(startIndex, "Channel");
                tbJapanese.Text = tempText.Remove(startIndex, 7).Insert(startIndex, "チャンネル");
                tbGerman.Text = tempText.Remove(startIndex, 7).Insert(startIndex, "Kanal");
                tbDutch.Text = tempText.Remove(startIndex, 7).Insert(startIndex, "Kanaal");
                tbKorean.Text = tempText.Remove(startIndex, 7).Insert(startIndex, "채널");

                try
                {
                    if (tempText[startIndex - 1] != ' ') tbFrench.Text = tempText.Remove(startIndex, 7).Insert(0, "Chaîne ");
                    else tbFrench.Text = tempText.Remove(startIndex - 1, 8).Insert(0, "Chaîne ");
                }
                catch { tbFrench.Text = tempText.Remove(startIndex, 7).Insert(0, "Chaîne"); }
                try
                {
                    if (tempText[startIndex - 1] != ' ') tbSpanish.Text = tempText.Remove(startIndex, 7).Insert(0, "Canal ");
                    else tbSpanish.Text = tempText.Remove(startIndex - 1, 8).Insert(0, "Canal ");
                }
                catch { tbSpanish.Text = tempText.Remove(startIndex, 7).Insert(0, "Canal"); }
                try
                {
                    if (tempText[startIndex - 1] != ' ') tbItalian.Text = tempText.Remove(startIndex, 7).Insert(0, "Canale ");
                    else tbItalian.Text = tempText.Remove(startIndex - 1, 8).Insert(0, "Canale ");
                }
                catch { tbItalian.Text = tempText.Remove(startIndex, 7).Insert(0, "Canale"); }

                tbAllLanguages.Text = string.Empty;
            }
            catch (Exception ex)
            {
                tbAllLanguages.Text = tempText;

                tbEnglish.Text = string.Empty;
                tbJapanese.Text = string.Empty;
                tbGerman.Text = string.Empty;
                tbFrench.Text = string.Empty;
                tbSpanish.Text = string.Empty;
                tbItalian.Text = string.Empty;
                tbDutch.Text = string.Empty;
                tbKorean.Text = string.Empty;

                errorBox(ex.Message);
            }
        }

        private void tbAllLanguages_TextChanged(object sender, EventArgs e)
        {
            llbTranslateChannel.Enabled = tbAllLanguages.Text.ToLower().Contains("channel");
        }

        private void tbStartupIos_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != '\b';
        }

        private void llbMultiReplace_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            multiReplace(sender == llbBannerMultiReplace);
        }

        private void lbxBannerTpls_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                lbxBannerTpls.SelectedIndex = lbxBannerTpls.IndexFromPoint(e.X, e.Y);
                cmTpls.Tag = "banner";
                cmTpls.Show(MousePosition);
            }
        }

        private void lbxIconTpls_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                lbxIconTpls.SelectedIndex = lbxIconTpls.IndexFromPoint(e.X, e.Y);
                cmTpls.Tag = "icon";
                cmTpls.Show(MousePosition);
            }
        }

        private void cmRename_Click(object sender, EventArgs e)
        {
            try
            {
                ListBox lbx = (((ToolStripMenuItem)sender).Owner.Tag.ToString().ToLower().Contains("banner")) ? lbxBannerTpls : lbxIconTpls;

                string oldNameFull = lbx.SelectedItem.ToString();
                string oldName = lbx.SelectedItem.ToString().Remove(lbx.SelectedItem.ToString().IndexOf('(') - 3);
                CustomizeMii_InputBox ib = new CustomizeMii_InputBox();
                ib.Description = "Please enter a new name:";
                ib.Input = oldName;
                ib.CommonKeyMode = false;
                ib.tbInput.MaxLength = 127;
                ib.btnExit.Text = "Close";

                if (ib.ShowDialog() == DialogResult.OK && oldName != ib.Input && !string.IsNullOrEmpty(ib.Input))
                {
                    string newName = ib.Input;
                    if (!newName.ToLower().EndsWith(".tpl")) newName += ".tpl";

                    foreach (char thisChar in Path.GetInvalidFileNameChars())
                        if (newName.Contains(thisChar.ToString()))
                            throw new Exception(string.Format("Invalid path char \"{0}\" found in new name!", thisChar));

                    if (lbx == lbxBannerTpls)
                        if (string.IsNullOrEmpty(replacedBanner))
                            bannerBin.RenameNode(oldName, newName);
                        else
                            newBannerBin.RenameNode(oldName, newName);
                    else
                        if (string.IsNullOrEmpty(replacedIcon))
                            iconBin.RenameNode(oldName, newName);
                        else
                            newIconBin.RenameNode(oldName, newName);

                    addTpls();

                    try { lbx.SelectedItem = oldNameFull.Replace(oldName, newName); }
                    catch { }
                }
            }
            catch (Exception ex) { errorBox(ex.Message); }
        }

        private void cmResize_Click(object sender, EventArgs e)
        {
            try
            {
                ListBox lbx = (((ToolStripMenuItem)sender).Owner.Tag.ToString().ToLower().Contains("banner")) ? lbxBannerTpls : lbxIconTpls;

                int selectedIndex = lbx.SelectedIndex;
                string tplName = lbx.SelectedItem.ToString().Remove(lbx.SelectedItem.ToString().IndexOf('(') - 3);

                Size oldSize;

                if (lbx == lbxBannerTpls)
                    if (string.IsNullOrEmpty(replacedBanner))
                    { oldSize = TPL.Load(bannerBin.Data[bannerBin.GetNodeIndex(tplName)]).GetTextureSize(0); }
                    else
                    { oldSize = TPL.Load(newBannerBin.Data[newBannerBin.GetNodeIndex(tplName)]).GetTextureSize(0); }
                else
                    if (string.IsNullOrEmpty(replacedIcon))
                    { oldSize = TPL.Load(iconBin.Data[iconBin.GetNodeIndex(tplName)]).GetTextureSize(0); }
                    else
                    { oldSize = TPL.Load(newIconBin.Data[newIconBin.GetNodeIndex(tplName)]).GetTextureSize(0); }
                
                CustomizeMii_InputBox ib = new CustomizeMii_InputBox();
                ib.Description = "Please enter a new size:";
                ib.tbInput.Size = new Size(ib.btnExit.Width, ib.tbInput.Height);
                ib.tbInput2.Visible = true;
                ib.CommonKeyMode = false;
                ib.tbInput.MaxLength = 127;
                ib.btnExit.Text = "Close";
                ib.Input = oldSize.Width.ToString();
                ib.Input2 = oldSize.Height.ToString();

                if (ib.ShowDialog() == DialogResult.OK)
                {
                    Size newSize = new Size(int.Parse(ib.Input), int.Parse(ib.Input2));
                    if (newSize == oldSize) return;

                    TPL tmpTpl;

                    if (lbx == lbxBannerTpls)
                        if (string.IsNullOrEmpty(replacedBanner))
                            tmpTpl = TPL.Load(bannerBin.Data[bannerBin.GetNodeIndex(tplName)]);
                        else
                            tmpTpl = TPL.Load(newBannerBin.Data[newBannerBin.GetNodeIndex(tplName)]);
                    else
                        if (string.IsNullOrEmpty(replacedIcon))
                            tmpTpl = TPL.Load(iconBin.Data[iconBin.GetNodeIndex(tplName)]);
                        else
                            tmpTpl = TPL.Load(newIconBin.Data[newIconBin.GetNodeIndex(tplName)]);

                    TPL_TextureFormat tplFormat = tmpTpl.GetTextureFormat(0);
                    Image newImg = resizeImage(tmpTpl.ExtractTexture(0), newSize.Width, newSize.Height);

                    tmpTpl.RemoveTexture(0);
                    tmpTpl.AddTexture(newImg, tplFormat);

                    if (lbx == lbxBannerTpls)
                        if (string.IsNullOrEmpty(replacedBanner))
                            bannerBin.ReplaceFile(bannerBin.GetNodeIndex(tplName), tmpTpl.ToByteArray());
                        else
                            newBannerBin.ReplaceFile(newBannerBin.GetNodeIndex(tplName), tmpTpl.ToByteArray());
                    else
                        if (string.IsNullOrEmpty(replacedIcon))
                            iconBin.ReplaceFile(iconBin.GetNodeIndex(tplName), tmpTpl.ToByteArray());
                        else
                            newIconBin.ReplaceFile(newIconBin.GetNodeIndex(tplName), tmpTpl.ToByteArray());

                    addTpls();

                    try { lbx.SelectedIndex = selectedIndex; }
                    catch { }
                }
            }
            catch (Exception ex) { errorBox(ex.Message); }
        }

        private void cmMakeSilent_Click(object sender, EventArgs e)
        {
            Wave w = new Wave(1, 8, 500, new byte[] { 0x80, 0x80, 0x80, 0x80, 0x80 });
            newSoundBin = Headers.IMD5.AddHeader(w.ToByteArray());
            w.Dispose();

            replacedSound = "Silence";
            setControlText(tbSound, "Silence");
            btnBrowseSound.Text = "Clear";

            if (cmbReplace.SelectedIndex == 2) setControlText(tbReplace, string.Empty);
        }

        private void lbxBaseWads_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabCredits_Click(object sender, EventArgs e)
        {

        }
    }
}
