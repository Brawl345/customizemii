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

//#define Mono //Change all "\\" to "/" (in all files; without quotes) while compiling for OS X / Linux (Mono)
//#define Debug //Always remember to turn off :)

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using ForwardMii;

namespace CustomizeMii
{
    public partial class CustomizeMii_Main : Form
    {
        #region Constants
        const string version = "2.3"; //Hint for myself: Never use a char in the Version (UpdateCheck)!
        const int SoundMaxLength = 30; //In seconds
        const int SoundWarningLength = 20; //In seconds
        const int BnsWarningLength = 45; //In seconds
        const int CreditsScrollSpeed = 75; //Timer Intervall for the scrolling Credits
        #endregion

        #region Variables
        public static bool Mono = false;
        public static string TempPath = Path.GetTempPath() + "CustomizeMii_Temp\\XXX\\";
        public static string TempWadPath = Path.GetTempPath() + "CustomizeMii_Temp\\XXX\\TempWad.wad";
        public static string TempUnpackPath = Path.GetTempPath() + "CustomizeMii_Temp\\XXX\\Unpack\\";
        public static string TempBannerPath = Path.GetTempPath() + "CustomizeMii_Temp\\XXX\\Banner\\";
        public static string TempIconPath = Path.GetTempPath() + "CustomizeMii_Temp\\XXX\\Icon\\";
        public static string TempSoundPath = Path.GetTempPath() + "CustomizeMii_Temp\\XXX\\sound.bin";
        public static string TempWavePath = Path.GetTempPath() + "CustomizeMii_Temp\\XXX\\Temp.wav";
        public static string TempBnsPath = Path.GetTempPath() + "CustomizeMii_Temp\\XXX\\Temp.bns";
        public static string TempDolPath = Path.GetTempPath() + "CustomizeMii_Temp\\XXX\\Temp.dol";
        public static string TempTempPath = Path.GetTempPath() + "CustomizeMii_Temp\\XXX\\Temp\\";
        public static string TempUnpackBannerTplPath = Path.GetTempPath() + "CustomizeMii_Temp\\XXX\\Unpack\\00000000.app_OUT\\meta\\banner.bin_OUT\\arc\\timg\\";
        public static string TempUnpackIconTplPath = Path.GetTempPath() + "CustomizeMii_Temp\\XXX\\Unpack\\00000000.app_OUT\\meta\\icon.bin_OUT\\arc\\timg\\";
        public static string TempUnpackBannerBrlytPath = Path.GetTempPath() + "CustomizeMii_Temp\\XXX\\Unpack\\00000000.app_OUT\\meta\\banner.bin_OUT\\arc\\blyt\\";
        public static string TempUnpackIconBrlytPath = Path.GetTempPath() + "CustomizeMii_Temp\\XXX\\Unpack\\00000000.app_OUT\\meta\\icon.bin_OUT\\arc\\blyt\\";
        public static string TempUnpackBannerBrlanPath = Path.GetTempPath() + "CustomizeMii_Temp\\XXX\\Unpack\\00000000.app_OUT\\meta\\banner.bin_OUT\\arc\\anim\\";
        public static string TempUnpackIconBrlanPath = Path.GetTempPath() + "CustomizeMii_Temp\\XXX\\Unpack\\00000000.app_OUT\\meta\\icon.bin_OUT\\arc\\anim\\";
        //public static string[] ButtonTexts = new string[] { "Create WAD!", "Fire!", "Go Go Go!", "Let's do it!", "What are you waitin' for?", "I want my Channel!", "Houston, We've Got a Problem!", "Error, please contact anyone!", "Isn't she sweet?", "Is that milk?", "In your face!", "_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_", "Take me to a higher place!", "What's goin' on?", "I'm a Button!", "Click!", "Today's date is " + DateTime.Now.ToShortDateString(), "Launch Time: " + DateTime.Now.ToLongTimeString(), string.Format("My name is {0}", Environment.UserName) };
        public static string[] SourceWadUrls = new string[] { "StaticBase.wad", "MPlayer_CE_Short.wad", "MPlayer_CE_Long.wad", "Snes9xGX.wad", "FCE_Ultra_wilsoff.wad", "FCE_Ultra_Leathl.wad", "Wii64.wad", "WiiSX_Full.wad", "WiiSX_Retro.wad", "WADder_Base_1.wad", "WADder_Base_2.wad", "WADder_Base_3.wad", "UniiLoader.wad", "Backup_Channel.wad" };
        public static string[] SourceWadPreviewUrls = new string[] { "http://www.youtube.com/watch?v=pFNKldTYQq0", "http://www.youtube.com/watch?v=Up1RZebUc_U", "http://www.youtube.com/watch?v=Up1RZebUc_U", "http://www.youtube.com/watch?v=P-Mxd6DMvFY", "http://www.youtube.com/watch?v=wrbrg-DH_h4", "http://www.youtube.com/watch?v=MfiVbQaiXw8", "http://www.youtube.com/watch?v=krCQ2J7ZH8Y", "http://www.youtube.com/watch?v=rZC1DKUM6QI", "http://www.youtube.com/watch?v=Uiy8w-bp1kI", "http://www.youtube.com/watch?v=BbSYCSI8tz8", "http://www.youtube.com/watch?v=PIFZevHQ8lQ", "http://www.youtube.com/watch?v=OIhvDNjphhc", "http://www.youtube.com/watch?v=KLcncEArQLY&NR=1", "http://www.youtube.com/watch?v=xE_EgdCRV1I" };
        public static string BannerReplace = string.Empty;
        public static string IconReplace = string.Empty;
        public static string BannerTplPath = string.Empty;
        public static string IconTplPath = string.Empty;
        private string SourceWad = string.Empty;
        private string BannerBrlytPath = string.Empty;
        private string IconBrlytPath = string.Empty;
        private string BannerBrlanPath = string.Empty;
        private string IconBrlanPath = string.Empty;
        private string SoundReplace = string.Empty;
        private bool BrlytChanged = false;
        private bool BrlanChanged = false;
        private Progress currentProgress;
        private EventHandler ProgressUpdate;
        private int UnpackFolderErrorCount = 0;
        private List<string> BannerTransparents = new List<string>();
        private List<string> IconTransparents = new List<string>();
        private string Mp3Path;
        private double separatorBtn;
        private Timer tmrCredits = new Timer();
        private ToolTip tTip = new ToolTip();
        #endregion

        public CustomizeMii_Main()
        {
            InitializeComponent();
            this.Icon = global::CustomizeMii.Properties.Resources.CustomizeMii;
        }

        private void CustomizeMii_Main_Load(object sender, EventArgs e)
        {
#if !Mono
            this.Text = this.Text.Replace("X", version);
            this.lbCreditVersion.Text = this.lbCreditVersion.Text.Replace("X", version);
            CommonKeyCheck();
#endif
#if Mono
            this.Text = this.Text.Replace("X", version + " (Mono)");
            this.lbCreditVersion.Text = this.lbCreditVersion.Text.Replace("X", version + " (Mono)");
            Mono = true;
            //TextBox.MaxLength is not implemented in Mono, so don't use it
            for (int i = 0; i < tabControl.TabPages.Count; i++)
                for(int j=0;j<tabControl.TabPages[i].Controls.Count;j++)
                    if (tabControl.TabPages[i].Controls[j] is TextBox) ((TextBox)tabControl.TabPages[i].Controls[j]).MaxLength = 32000;
#endif

            if (File.Exists(Application.StartupPath + "\\CustomizeMiiInstaller.dll"))
                this.lbCreditInstaller.Text = this.lbCreditInstaller.Text.Replace("X", GetInstallerVersion());
            else this.lbCreditInstaller.Text = this.lbCreditInstaller.Text.Replace(" X", string.Empty);

            MethodInvoker Update = new MethodInvoker(UpdateCheck);
            Update.BeginInvoke(null, null);

            UpdatePaths();
            InitializeStartup();
        }

        private void CustomizeMii_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            try { 
                if (Directory.Exists(TempPath)) Directory.Delete(TempPath, true);
                if (File.Exists("C:\\cmtempmp3wav.wav")) File.Delete("C:\\cmtempmp3wav.wav");
            }
            catch { }
            Environment.Exit(0);
        }

        private void UpdatePaths()
        {
            string thisGuid = Guid.NewGuid().ToString();

            TempPath = TempPath.Replace("XXX", thisGuid);
            TempWadPath = TempWadPath.Replace("XXX", thisGuid);
            TempUnpackPath = TempUnpackPath.Replace("XXX", thisGuid);
            TempBannerPath = TempBannerPath.Replace("XXX", thisGuid);
            TempIconPath = TempIconPath.Replace("XXX", thisGuid);
            TempSoundPath = TempSoundPath.Replace("XXX", thisGuid);
            TempWavePath = TempWavePath.Replace("XXX", thisGuid);
            TempBnsPath = TempBnsPath.Replace("XXX", thisGuid);
            TempDolPath = TempDolPath.Replace("XXX", thisGuid);
            TempTempPath = TempTempPath.Replace("XXX", thisGuid);
            TempUnpackBannerTplPath = TempUnpackBannerTplPath.Replace("XXX", thisGuid);
            TempUnpackIconTplPath = TempUnpackIconTplPath.Replace("XXX", thisGuid);
            TempUnpackBannerBrlytPath = TempUnpackBannerBrlytPath.Replace("XXX", thisGuid);
            TempUnpackIconBrlytPath = TempUnpackIconBrlytPath.Replace("XXX", thisGuid);
            TempUnpackBannerBrlanPath = TempUnpackBannerBrlanPath.Replace("XXX", thisGuid);
            TempUnpackIconBrlanPath = TempUnpackIconBrlanPath.Replace("XXX", thisGuid);
        }

        private void InitializeStartup()
        {
            if (Directory.Exists(TempPath)) Directory.Delete(TempPath, true);
            ProgressUpdate = new EventHandler(this.UpdateProgress);
            btnBrowseSource.Text = "Browse...";
            DrawCreateButton();
            cmbNandLoader.SelectedIndex = 0;
            cmbFormatBanner.SelectedIndex = 0;
            cmbFormatIcon.SelectedIndex = 0;
            cmbReplace.SelectedIndex = 0;
            pbProgress.Value = 100;
            BrlanChanged = false;
            BrlytChanged = false;
            SetToolTips();
            btnBrowseDol.Text = "Browse...";
            btnBrowseSound.Text = "Browse...";
            rtbInstructions.Rtf = Properties.Resources.Instructions;
            rtbInstructions.LinkClicked += new LinkClickedEventHandler(rtbInstructions_LinkClicked);
            tmrCredits.Interval = CreditsScrollSpeed;
            tmrCredits.Tick += new EventHandler(tmrCredits_Tick);

#if !Debug
            DisableControls(null, null);
#endif
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
            tbKorean.Text = string.Empty;
            tbJapanese.Text = string.Empty;
            tbReplace.Text = string.Empty;
            tbSound.Text = string.Empty;
            tbSourceWad.Text = string.Empty;
            tbSpanish.Text = string.Empty;
            tbTitleID.Text = string.Empty;
            tbStartupIos.Text = string.Empty;

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

            cbLz77.Checked = true;
            cbIconMakeTransparent.Checked = false;
            cbBannerMakeTransparent.Checked = false;

            BannerTransparents.Clear();
            IconTransparents.Clear();

            SimpleForwarder.Clear();
            ComplexForwarder.Clear();
        }

        private void SetToolTips()
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
            tTip.SetToolTip(cbFailureChecks, "Turn off the security checks...\nNot recommended, you may get a bricking WAD...");
        }

        private bool CheckForForwardMii()
        {
            if (File.Exists(Application.StartupPath + "\\ForwardMii.dll"))
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
            catch (Exception ex) { ErrorBox(ex.Message); }
        }

#if !Mono
        private bool CheckDevKit()
        {
            return ForwardMii_Plugin.CheckDevKit();
        }
#endif

        private void CreateForwarderSimple(string Output)
        {
            SimpleForwarder.Save(Output);
        }

        private void CreateForwarderComplex(string Output)
        {
            ComplexForwarder.Save(Output);
        }

        private void ConvertMp3ToWave(string mp3File)
        {
            if (File.Exists(Application.StartupPath + "\\lame.exe"))
            {
                try
                {
                    BackgroundWorker bwConvertMp3 = new BackgroundWorker();
                    bwConvertMp3.DoWork += new DoWorkEventHandler(bwConvertMp3_DoWork);
                    bwConvertMp3.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwConvertMp3_RunWorkerCompleted);
                    bwConvertMp3.ProgressChanged += new ProgressChangedEventHandler(bwConvertMp3_ProgressChanged);
                    bwConvertMp3.WorkerReportsProgress = true;

                    lbStatusText.Text = "Converting MP3...";
                    bwConvertMp3.RunWorkerAsync(mp3File);
                }
                catch (Exception ex)
                {
                    ErrorBox(ex.Message);
                }
            }
        }

        private void DrawCreateButton()
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
                gImg.DrawImage(ResizeImage(tmpCreate, tmpCreate.Width, tmpCreate.Height), new Point(280, 0));
                gImg.DrawImage(ResizeImage(tmpSend, tmpSend.Width, tmpSend.Height), new Point(55, 0));

                btnCreateWad.Image = tmpImg;
            }
        }

        private bool CheckInet()
        {
            try
            {
                System.Net.IPHostEntry ipHost = System.Net.Dns.GetHostEntry("www.google.com");
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void UpdateProgress(object sender, EventArgs e)
        {
            pbProgress.Value = currentProgress.progressValue;

            if (!string.IsNullOrEmpty(currentProgress.progressState))
            {
                if (currentProgress.progressState == " ") currentProgress.progressState = string.Empty;
                lbStatusText.Text = currentProgress.progressState;
                currentProgress.progressState = string.Empty;
            }
        }

        private void SetSourceWad(object sender, EventArgs e)
        {
            tbSourceWad.Text = SourceWad;
        }

        private void tabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            tmrCredits.Stop();

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
                AddBrlyts(null, null);
            }
            else if (tabControl.SelectedTab == tabBrlan)
            {
                AddBrlans(null, null);
            }
            else if (tabControl.SelectedTab == tabCredits)
            {
                lbCreditThanks.Location = new Point(lbCreditThanks.Location.X, panCredits.Height);
                tmrCredits.Start();
            }
        }

        private void tmrCredits_Tick(object sender, EventArgs e)
        {
            if (lbCreditThanks.Location.Y == -130) lbCreditThanks.Location = new Point(lbCreditThanks.Location.X, panCredits.Height);
            lbCreditThanks.Location = new Point(lbCreditThanks.Location.X, lbCreditThanks.Location.Y - 1);
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

        private void UpdateCheck()
        {
            if (CheckInet() == true)
            {
                try
                {
                    WebClient GetVersion = new WebClient();
#if Mono
                    string NewVersion = GetVersion.DownloadString("http://customizemii.googlecode.com/svn/monoversion.txt");
#else
                    string NewVersion = GetVersion.DownloadString("http://customizemii.googlecode.com/svn/version.txt");
#endif
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
                            Process.Start("http://code.google.com/p/customizemii/downloads/list");
                        }
                    }
                }
                catch { }
            }
        }

        private string GetInstallerVersion()
        {
            return CustomizeMiiInstaller.CustomizeMiiInstaller_Plugin.GetVersion();
        }

#if !Mono
        private string GetForwardMiiVersion()
        {
            return ForwardMii_Plugin.GetVersion();
        }
#endif

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
            if (btnBrowseSource.Text.ToLower() == "clear") { Initialize(null, null); }
            else LoadChannel();
        }

        private void btnLoadBaseWad_Click(object sender, EventArgs e)
        {
            if (lbxBaseWads.SelectedIndex != -1)
            {
                if (pbProgress.Value == 100)
                {
                    if (CheckInet() == true)
                    {
#if Mono
                        CommonKeyCheck();
#endif

                        if (tbSourceWad.Text != SourceWadUrls[lbxBaseWads.SelectedIndex])
                        {
                            SourceWad = "http://customizemii.googlecode.com/svn/branches/Base_WADs/" + SourceWadUrls[lbxBaseWads.SelectedIndex];
                            tbSourceWad.Text = SourceWad;

                            System.Threading.Thread dlThread = new System.Threading.Thread(new System.Threading.ThreadStart(DownloadBaseWad));
                            dlThread.Start();
                        }
                    }
                    else
                    {
                        ErrorBox("You're not connected to the Internet!");
                    }
                }
            }
        }

        void DownloadBaseWad()
        {
            try
            {
                WebClient Client = new WebClient();
                Client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(Client_DownloadProgressChanged);
                Client.DownloadFileCompleted += new AsyncCompletedEventHandler(Client_DownloadFileCompleted);

                currentProgress.progressState = "Downloading Base WAD...";
                currentProgress.progressValue= 0;
                this.Invoke(ProgressUpdate);

                if (!Directory.Exists(TempWadPath.Remove(TempWadPath.LastIndexOf('\\'))))
                    Directory.CreateDirectory(TempWadPath.Remove(TempWadPath.LastIndexOf('\\')));
                Client.DownloadFileAsync(new Uri(SourceWad), TempWadPath);
            }
            catch (Exception ex)
            {
                SetText(tbSourceWad, string.Empty);
                ErrorBox(ex.Message);
            }
        }

        void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            currentProgress.progressValue = e.ProgressPercentage;
            currentProgress.progressState = "Downloading Base WAD...";
            this.Invoke(ProgressUpdate);
        }

        void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            currentProgress.progressValue = 0;
            currentProgress.progressState = string.Empty;
            this.Invoke(ProgressUpdate);

            if (File.Exists(TempWadPath))
            {
                FileInfo fi = new FileInfo(TempWadPath);

                if (fi.Length > 0)
                {
                    BackgroundWorker bwLoadChannel = new BackgroundWorker();
                    bwLoadChannel.DoWork += new DoWorkEventHandler(bwLoadChannel_DoWork);
                    bwLoadChannel.ProgressChanged += new ProgressChangedEventHandler(bwLoadChannel_ProgressChanged);
                    bwLoadChannel.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoadChannel_RunWorkerCompleted);
                    bwLoadChannel.WorkerReportsProgress = true;
                    bwLoadChannel.RunWorkerAsync(TempWadPath);
                }
                else
                {
                    SetText(tbSourceWad, string.Empty);
                    File.Delete(TempWadPath);
                    ErrorBox("The requested file couldn't be downloaded.");
                }
            }
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
                        InfoBox("There's no preview of this channel available, sorry!");
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
                        string Url = "http://customizemii.googlecode.com/svn/branches/Base_WADs/" + SourceWadUrls[lbxBaseWads.SelectedIndex];
                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.Filter = "Wii Channels|*.wad";
                        sfd.FileName = Url.Remove(0, Url.LastIndexOf('/') + 1);

                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            if (tbSourceWad.Text == Url && File.Exists(TempWadPath))
                            {
                                File.Copy(TempWadPath, sfd.FileName, true);
                                InfoBox(string.Format("Saved channel as {0}", Path.GetFileName(sfd.FileName)));
                            }
                            else
                            {
                                System.Threading.Thread saveThread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(SaveBaseWad));
                                saveThread.Start((object) new string[] { Url, sfd.FileName });
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

        void SaveBaseWad(object urlAndSavePath)
        {
            try
            {
                WebClient SaveClient = new WebClient();
                SaveClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(SaveClient_DownloadProgressChanged);
                SaveClient.DownloadFileCompleted += new AsyncCompletedEventHandler(SaveClient_DownloadFileCompleted);

                currentProgress.progressValue = 0;
                currentProgress.progressState = "Downloading Base WAD...";
                this.Invoke(ProgressUpdate);

                SaveClient.DownloadFileAsync(new Uri(((string[])urlAndSavePath)[0]), ((string[])urlAndSavePath)[1]);
            }
            catch (Exception ex)
            {
                ErrorBox(ex.Message);
            }
        }

        void SaveClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            currentProgress.progressValue = e.ProgressPercentage;
            currentProgress.progressState = "Downloading Base WAD...";
            this.Invoke(ProgressUpdate);
        }

        void SaveClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
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
            if (btnBrowseReplace.Text == "Clear")
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
            else
            {
                ReplacePart();
            }
        }

        private void btnBrowseDol_Click(object sender, EventArgs e)
        {
            if (btnBrowseDol.Text == "Clear")
            {
                if (Directory.Exists(TempTempPath + "TempWad")) Directory.Delete(TempTempPath + "TempWad", true);
                if (File.Exists(TempDolPath)) File.Delete(TempDolPath);

                SetText(tbDol, string.Empty);
                btnBrowseDol.Text = "Browse...";

                SimpleForwarder.Clear();
                ComplexForwarder.Clear();
            }
            else
            {
                cmDol.Show(MousePosition);
            }
        }

        private void btnBrowseSound_Click(object sender, EventArgs e)
        {
            if (btnBrowseSound.Text == "Clear")
            {
                if (File.Exists(TempWavePath)) File.Delete(TempWavePath);
                if (File.Exists(TempBnsPath)) File.Delete(TempBnsPath);
                SetText(tbSound, string.Empty);
                btnBrowseSound.Text = "Browse...";
            }
            else
                cmSound.Show(MousePosition);
        }

        private void lbxBannerTpls_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxBannerTpls.SelectedIndex != -1)
            {
                try
                {
                    cbBannerMakeTransparent.Enabled = true;
                    cbBannerMakeTransparent.Checked = lbxBannerTpls.SelectedItem.ToString().EndsWith("(Transparent)");

                    string TplFile = BannerTplPath + lbxBannerTpls.SelectedItem.ToString().Remove(lbxBannerTpls.SelectedItem.ToString().IndexOf('(', 0) - 1);
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
                        case 8:
                            cmbFormatBanner.SelectedIndex = 7;
                            break;
                        case 9:
                            cmbFormatBanner.SelectedIndex = 8;
                            break;
                        case 10:
                            cmbFormatBanner.SelectedIndex = 9;
                            break;
                        case 14:
                            cmbFormatBanner.SelectedIndex = 10;
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

                    string TplFile = IconTplPath + lbxIconTpls.SelectedItem.ToString().Remove(lbxIconTpls.SelectedItem.ToString().IndexOf('(', 0) - 1);
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
                        case 8:
                            cmbFormatBanner.SelectedIndex = 7;
                            break;
                        case 9:
                            cmbFormatBanner.SelectedIndex = 8;
                            break;
                        case 10:
                            cmbFormatBanner.SelectedIndex = 9;
                            break;
                        case 14:
                            cmbFormatBanner.SelectedIndex = 10;
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

                if (Format < 7)
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Filter = "PNG|*.png|JPG|*.jpg|GIF|*.gif|BMP|*.bmp|TPL|*.tpl|All|*.png;*.jpg;*.gif;*.bmp;*.tpl";
                    ofd.FilterIndex = 6;

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            string Tpl = BannerTplPath + lbxBannerTpls.SelectedItem.ToString().Remove(lbxBannerTpls.SelectedItem.ToString().IndexOf('(', 0) - 1);
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

                            switch (Format)
                            {
                                case 6: //I4
                                    TplFormat = 0;
                                    break;
                                case 5: //I8
                                    TplFormat = 1;
                                    break;
                                case 4: //IA4
                                    TplFormat = 2;
                                    break;
                                case 3: //IA8
                                    TplFormat = 3;
                                    break;
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
                    ErrorBox("This format is not supported, you must choose a different one!");
                }
            }
        }

        private void btnExtractBanner_Click(object sender, EventArgs e)
        {
            if (lbxBannerTpls.SelectedIndex != -1)
            {
                string Tpl = BannerTplPath + lbxBannerTpls.SelectedItem.ToString().Remove(lbxBannerTpls.SelectedItem.ToString().IndexOf('(', 0) - 1);
                int format = Wii.TPL.GetTextureFormat(Tpl);
                if (format == 8 || format == 9 || format == 10) { ErrorBox("This format is not supported!"); return; }

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "PNG|*.png|JPG|*.jpg|GIF|*.gif|BMP|*.bmp|TPL|*.tpl|All|*.png;*.jpg;*.gif;*.bmp;*.tpl";
                sfd.FilterIndex = 6;
                sfd.FileName = lbxBannerTpls.SelectedItem.ToString().Replace(".tpl", string.Empty);

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (!sfd.FileName.ToLower().EndsWith(".tpl"))
                        {
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
                    CustomizeMii_Preview pvw = new CustomizeMii_Preview();
                    pvw.startTPL = lbxBannerTpls.SelectedItem.ToString().Remove(lbxBannerTpls.SelectedItem.ToString().IndexOf('(', 0) - 1);
                    pvw.ShowDialog();
                    pvw = null;
                }
                catch (Exception ex)
                {
                    ErrorBox(ex.Message);
                }

                lbxBannerTpls.Focus();
            }
        }

        private void btnReplaceIcon_Click(object sender, EventArgs e)
        {
            if (lbxIconTpls.SelectedIndex != -1)
            {
                int Format = cmbFormatIcon.SelectedIndex;

                if (Format < 7)
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Filter = "PNG|*.png|JPG|*.jpg|GIF|*.gif|BMP|*.bmp|TPL|*.tpl|All|*.png;*.jpg;*.gif;*.bmp;*.tpl";
                    ofd.FilterIndex = 6;

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            string Tpl = IconTplPath + lbxIconTpls.SelectedItem.ToString().Remove(lbxIconTpls.SelectedItem.ToString().IndexOf('(', 0) - 1);
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

                            switch (Format)
                            {
                                case 6: //I4
                                    TplFormat = 0;
                                    break;
                                case 5: //I8
                                    TplFormat = 1;
                                    break;
                                case 4: //IA4
                                    TplFormat = 2;
                                    break;
                                case 3: //IA8
                                    TplFormat = 3;
                                    break;
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
                    ErrorBox("This format is not supported, you must choose a different one!");
                }
            }
        }

        private void btnExtractIcon_Click(object sender, EventArgs e)
        {
            if (lbxIconTpls.SelectedIndex != -1)
            {
                string Tpl = IconTplPath + lbxIconTpls.SelectedItem.ToString().Remove(lbxIconTpls.SelectedItem.ToString().IndexOf('(', 0) - 1);
                int format = Wii.TPL.GetTextureFormat(Tpl);
                if (format == 8 || format == 9 || format == 10) { ErrorBox("This format is not supported!"); return; }

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "PNG|*.png|JPG|*.jpg|GIF|*.gif|BMP|*.bmp|TPL|*.tpl|All|*.png;*.jpg;*.gif;*.bmp;*.tpl";
                sfd.FilterIndex = 6;
                sfd.FileName = lbxIconTpls.SelectedItem.ToString().Replace(".tpl", string.Empty);

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (!sfd.FileName.ToLower().EndsWith(".tpl"))
                        {
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
                    CustomizeMii_Preview pvw = new CustomizeMii_Preview();
                    pvw.startIcon = true;
                    pvw.startTPL = lbxIconTpls.SelectedItem.ToString().Remove(lbxIconTpls.SelectedItem.ToString().IndexOf('(', 0) - 1);
                    pvw.ShowDialog();
                    pvw = null;
                }
                catch (Exception ex)
                {
                    ErrorBox(ex.Message);
                }

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
                        if (!File.Exists(Application.StartupPath + "\\CustomizeMiiInstaller.dll"))
                        {
                            ErrorBox("CustomizeMiiInstaller.dll wasn't found!");
                        }
                        else
                        {
                            if (cbFailureChecks.Checked == true || FailureCheck() == true)
                            {
                                CustomizeMii_Transmit cmt = new CustomizeMii_Transmit();

                                if (cmt.ShowDialog() == DialogResult.OK)
                                {
                                    try
                                    {
                                        WadCreationInfo wadInfo = new WadCreationInfo();
                                        wadInfo.outFile = TempPath + "SendToWii.wad";
                                        wadInfo.nandLoader = (WadCreationInfo.NandLoader)cmbNandLoader.SelectedIndex;
                                        wadInfo.sendToWii = true;
                                        wadInfo.transmitProtocol = (TransmitProtocol)cmt.Protocol;
                                        wadInfo.transmitIp = cmt.IPAddress;
                                        wadInfo.transmitIos = int.Parse(cmt.IOS);

                                        BackgroundWorker bwCreateWad = new BackgroundWorker();
                                        bwCreateWad.DoWork += new DoWorkEventHandler(bwCreateWad_DoWork);
                                        bwCreateWad.ProgressChanged += new ProgressChangedEventHandler(bwCreateWad_ProgressChanged);
                                        bwCreateWad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwCreateWad_RunWorkerCompleted);
                                        bwCreateWad.WorkerReportsProgress = true;
                                        bwCreateWad.RunWorkerAsync(wadInfo);
                                    }
                                    catch (Exception ex)
                                    {
                                        ErrorBox(ex.Message);
                                    }
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
                        if (cbFailureChecks.Checked == true || FailureCheck() == true)
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

                                    WadCreationInfo wadInfo = new WadCreationInfo();

                                    if (!int.TryParse(tbStartupIos.Text, out wadInfo.requiredIos)) { ErrorBox("Please enter a valid Required IOS! (0 - 255)"); return; }
                                    wadInfo.titles = new string[] { tbJapanese.Text, tbEnglish.Text, tbGerman.Text, tbFrench.Text, tbSpanish.Text, tbItalian.Text, tbDutch.Text, tbKorean.Text };
                                    wadInfo.allLangTitle = tbAllLanguages.Text;
                                    wadInfo.titleId = tbTitleID.Text;
                                    wadInfo.sound = tbSound.Text;
                                    wadInfo.dol = tbDol.Text;

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
                                    CreationTimer.Stop();
                                    ErrorBox(ex.Message);
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
                                    File.Copy(ofd.FileName, TempUnpackBannerBrlanPath + ofd.FileName.Remove(0, ofd.FileName.LastIndexOf('\\') + 1), true);
                                else
                                    File.Copy(ofd.FileName, BannerBrlanPath + ofd.FileName.Remove(0, ofd.FileName.LastIndexOf('\\') + 1), true);
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
                                    File.Copy(ofd.FileName, TempUnpackIconBrlanPath + ofd.FileName.Remove(0, ofd.FileName.LastIndexOf('\\') + 1), true);
                                else
                                    File.Copy(ofd.FileName, IconBrlanPath + ofd.FileName.Remove(0, ofd.FileName.LastIndexOf('\\') + 1), true);
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

                        File.Copy(brlytFile, sfd.FileName, true);
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

                        File.Copy(brlanFile, sfd.FileName, true);
                    }
                    catch (Exception ex) { ErrorBox(ex.Message); }
                }
            }
        }

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
                                File.Copy(ofd.FileName, TempUnpackBannerBrlytPath + lbxBrlytBanner.SelectedItem.ToString(), true);
                                BrlytChanged = true;
                            }
                            else
                            {
                                File.Delete(BannerBrlytPath + lbxBrlytBanner.SelectedItem.ToString());
                                File.Copy(ofd.FileName, BannerBrlytPath + lbxBrlytBanner.SelectedItem.ToString(), true);
                                BrlytChanged = true;
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(IconReplace))
                            {
                                File.Delete(TempUnpackIconBrlytPath + lbxBrlytIcon.SelectedItem.ToString());
                                File.Copy(ofd.FileName, TempUnpackIconBrlytPath + lbxBrlytIcon.SelectedItem.ToString(), true);
                                BrlytChanged = true;
                            }
                            else
                            {
                                File.Delete(IconBrlytPath + lbxBrlytIcon.SelectedItem.ToString());
                                File.Copy(ofd.FileName, IconBrlytPath + lbxBrlytIcon.SelectedItem.ToString(), true);
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
                                File.Copy(ofd.FileName, TempUnpackBannerBrlanPath + lbxBrlanBanner.SelectedItem.ToString(), true);
                                BrlanChanged = true;
                            }
                            else
                            {
                                File.Delete(BannerBrlanPath + lbxBrlanBanner.SelectedItem.ToString());
                                File.Copy(ofd.FileName, BannerBrlanPath + lbxBrlanBanner.SelectedItem.ToString(), true);
                                BrlanChanged = true;
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(IconReplace))
                            {
                                File.Delete(TempUnpackIconBrlanPath + lbxBrlanIcon.SelectedItem.ToString());
                                File.Copy(ofd.FileName, TempUnpackIconBrlanPath + lbxBrlanIcon.SelectedItem.ToString(), true);
                                BrlanChanged = true;
                            }
                            else
                            {
                                File.Delete(IconBrlanPath + lbxBrlanIcon.SelectedItem.ToString());
                                File.Copy(ofd.FileName, IconBrlanPath + lbxBrlanIcon.SelectedItem.ToString(), true);
                                BrlanChanged = true;
                            }
                        }
                    }
                    catch (Exception ex) { ErrorBox(ex.Message); }
                }
            }
        }

        private void llbUpdateAvailable_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                llbUpdateAvailable.LinkVisited = true;
                Process.Start("http://code.google.com/p/customizemii/downloads/list");
            }
            catch (Exception ex) { ErrorBox(ex.Message); }
        }

        private void btnDeleteBanner_Click(object sender, EventArgs e)
        {
            if (lbxBannerTpls.SelectedIndex != -1)
            {
                int idx = lbxBannerTpls.SelectedIndex;

                try
                {
                    string TplName = lbxBannerTpls.SelectedItem.ToString().Remove(lbxBannerTpls.SelectedItem.ToString().IndexOf('(', 0) - 1);
                    string CurBannerPath = GetCurBannerPath();
                    string[] brlytTpls = Wii.Brlyt.GetBrlytTpls(CurBannerPath + "blyt\\banner.brlyt", (File.Exists(CurBannerPath + "anim\\banner.brlan")) ? CurBannerPath + "anim\\banner.brlan" : CurBannerPath + "anim\\banner_loop.brlan");

                    if (!Wii.Tools.StringExistsInStringArray(TplName, brlytTpls) || cbFailureChecks.Checked)
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

                for (; ; )
                {
                    try
                    {
                        lbxBannerTpls.SelectedIndex = idx;
                        break;
                    }
                    catch { idx--; }
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
                    string TplName = lbxIconTpls.SelectedItem.ToString().Remove(lbxIconTpls.SelectedItem.ToString().IndexOf('(', 0) - 1);
                    string CurIconPath = GetCurIconPath();

                    string[] brlytTpls = Wii.Brlyt.GetBrlytTpls(CurIconPath + "blyt\\icon.brlyt", CurIconPath + "anim\\icon.brlan");

                    if (!Wii.Tools.StringExistsInStringArray(TplName, brlytTpls) || cbFailureChecks.Checked)
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

                for (; ; )
                {
                    try
                    {
                        lbxIconTpls.SelectedIndex = idx;
                        break;
                    }
                    catch { idx--; }
                }
            }

            lbxIconTpls.Select();
        }

        private void btnAddBanner_Click(object sender, EventArgs e)
        {
            try { AddTpl(lbxBannerTpls); }
            catch (Exception ex) { ErrorBox(ex.Message); }
        }

        private void btnAddIcon_Click(object sender, EventArgs e)
        {
            try { AddTpl(lbxIconTpls); }
            catch (Exception ex) { ErrorBox(ex.Message); }
        }

        private void btnBrlytListTpls_Click(object sender, EventArgs e)
        {
            if (lbBrlytActions.Text == "Banner")
            {
                string CurBannerPath = GetCurBannerPath();
                string[] brlytTpls = Wii.Brlyt.GetBrlytTpls(CurBannerPath + "blyt\\banner.brlyt", (File.Exists(CurBannerPath + "anim\\banner.brlan")) ? CurBannerPath + "anim\\banner.brlan" : CurBannerPath + "anim\\banner_loop.brlan");

                MessageBox.Show("These are the TPLs required by your banner.brlyt:\n\n" +
                    string.Join("\n", brlytTpls), "TPLs specified in banner.brlyt", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                string CurIconPath = GetCurIconPath();
                string[] brlytTpls = Wii.Brlyt.GetBrlytTpls(CurIconPath + "blyt\\icon.brlyt", CurIconPath + "anim\\icon.brlan");

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
                            string thisItem = lbxBannerTpls.SelectedItem.ToString();
                            lbxBannerTpls.Items.Remove(lbxBannerTpls.SelectedItem);
                            lbxBannerTpls.Items.Add(thisItem + " (Transparent)");
                            lbxBannerTpls.SelectedItem = thisItem + " (Transparent)";
                            BannerTransparents.Add(thisItem.Remove(thisItem.IndexOf('(', 0) - 1));
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
                        BannerTransparents.Remove(thisItem.Remove(thisItem.IndexOf('(', 0) - 1));
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
                            IconTransparents.Add(thisItem.Remove(thisItem.IndexOf('(', 0) - 1));
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
                        IconTransparents.Remove(thisItem.Remove(thisItem.IndexOf('(', 0) - 1));
                    }
                    catch { }
                }

                lbxIconTpls.Focus();
            }
        }

        private void btnForwarder_Click(object sender, EventArgs e)
        {
#if Mono
            ErrorBox("This feature doesn't work under Mono!");
#endif

#if !Mono
            if (CheckForForwardMii() == true)
            {
                if (CheckDevKit() == false)
                {
                    ForwarderDialogSimple();
                }
                else
                {
                    cmForwarder.Show(MousePosition);
                }
            }
            else
            {
                if (MessageBox.Show("You don't have the ForwardMii.dll in your application folder.\nYou can download it on the project page, do you want the page to be opened?", "Plugin not available", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    Process.Start("http://code.google.com/p/customizemii/downloads/list");
                }
            }
#endif
        }

        private void cmForwarderItem_Click(object sender, EventArgs e)
        {
#if !Mono
            ToolStripMenuItem cmSender = sender as ToolStripMenuItem;

            if (cmSender == cmSimpleForwarder)
                ForwarderDialogSimple();
            else
            {
                try
                {
                    string CurrentVersion = GetForwardMiiVersion();
                    int cVersion = Convert.ToInt32(CurrentVersion.Replace(".", string.Empty).Length == 2 ? (CurrentVersion.Replace(".", string.Empty) + "0") : CurrentVersion.Replace(".", string.Empty));

                    if (cVersion < 110)
                    {
                        ErrorBox("Version 1.1 or higher of the ForwardMii.dll is required!");
                        return;
                    }
                    ForwarderDialogComplex();
                }
                catch (Exception ex)
                {
                    ErrorBox(ex.Message);
                }
            }
#endif
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
                                bannerPath = fbd.SelectedPath + "\\Banner";
                                iconPath = fbd.SelectedPath + "\\Icon";
                                break;
                        }

                        if (!string.IsNullOrEmpty(bannerPath))
                        {
                            if (!Directory.Exists(bannerPath)) Directory.CreateDirectory(bannerPath);
                            string[] tplFiles = Directory.GetFiles(BannerTplPath, "*.tpl");
                            Image img;

                            foreach (string thisTpl in tplFiles)
                            {
                                img = Wii.TPL.ConvertFromTPL(thisTpl);
                                img.Save(bannerPath + "\\" + Path.GetFileNameWithoutExtension(thisTpl) + ".png",
                                    System.Drawing.Imaging.ImageFormat.Png);
                            }
                        }
                        if (!string.IsNullOrEmpty(iconPath))
                        {
                            if (!Directory.Exists(iconPath)) Directory.CreateDirectory(iconPath);
                            string[] tplFiles = Directory.GetFiles(IconTplPath, "*.tpl");
                            Image img;

                            foreach (string thisTpl in tplFiles)
                            {
                                img = Wii.TPL.ConvertFromTPL(thisTpl);
                                img.Save(iconPath + "\\" + Path.GetFileNameWithoutExtension(thisTpl) + ".png",
                                    System.Drawing.Imaging.ImageFormat.Png);
                            }
                        }

                        InfoBox("Extracted images successfully!");
                    }
                }
                catch (Exception ex) { ErrorBox(ex.Message); }
            }
            else if (cmSender.OwnerItem == tsExtractSound)
            {
                try
                {
                SaveFileDialog sfd = new SaveFileDialog();
                if (cmSender.Name.ToLower() == "cmextractsoundasbin") { sfd.Filter = "BIN|*.bin"; sfd.FileName = "sound.bin"; }
                else if (cmSender.Name.ToLower() == "cmextractsoundasaudio")
                {
                    byte[] soundBin = Wii.Tools.LoadFileToByteArray(TempUnpackPath + "00000000.app_OUT\\meta\\sound.bin", 32, 16);

                    if (soundBin[0] == 'R' && soundBin[1] == 'I' && soundBin[2] == 'F' && soundBin[3] == 'F')
                    { sfd.Filter = "Wave|*.wav"; sfd.FileName = "sound.wav"; }
                    else if (soundBin[0] == 'B' && soundBin[1] == 'N' && soundBin[2] == 'S' && soundBin[3] == ' ')
                    { sfd.Filter = "BNS|*.bns"; sfd.FileName = "sound.bns"; }
                    else if (soundBin[0] == 'F' && soundBin[1] == 'O' && soundBin[2] == 'R' && soundBin[3] == 'M')
                    { sfd.Filter = "AIFF|*.aif;*.aiff"; sfd.FileName = "sound.aif"; }
                    else if (soundBin[0] == 'L' && soundBin[1] == 'Z' && soundBin[2] == '7' && soundBin[3] == '7')
                    {
                        if (soundBin[9] == 'R' && soundBin[10] == 'I' && soundBin[11] == 'F' && soundBin[12] == 'F')
                        { sfd.Filter = "Wave|*.wav"; sfd.FileName = "sound.wav"; }
                        else if (soundBin[9] == 'B' && soundBin[10] == 'N' && soundBin[11] == 'S' && soundBin[12] == ' ')
                        { sfd.Filter = "BNS|*.bns"; sfd.FileName = "sound.bns"; }
                        else if (soundBin[9] == 'F' && soundBin[10] == 'O' && soundBin[11] == 'R' && soundBin[12] == 'M')
                        { sfd.Filter = "AIFF|*.aif;*.aiff"; sfd.FileName = "sound.aif"; }
                        else throw new Exception("Unsupported Audio Format!");
                    }
                    else throw new Exception("Unsupported Audio Format!");

                }

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (sfd.FileName.ToLower().EndsWith(".bin"))
                    {
                        File.Copy(TempUnpackPath + "00000000.app_OUT\\meta\\sound.bin", sfd.FileName, true);
                        InfoBox("The sound.bin was successfully extraced!");
                    }
                    else
                    {
                        Wii.Sound.SoundBinToAudio(TempUnpackPath + "00000000.app_OUT\\meta\\sound.bin", sfd.FileName);
                        InfoBox(string.Format("The sound.bin was successfully converted to {0}!", Path.GetFileName(sfd.FileName)));
                    }
                }
                }
                catch (Exception ex) { ErrorBox(ex.Message); }
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
                            string[] bannerFiles = Directory.GetFiles(GetCurBannerPath() + "blyt", "*.brlyt");
                            string[] iconFiles = Directory.GetFiles(GetCurIconPath() + "blyt", "*.brlyt");

                            foreach (string thisFile in bannerFiles)
                                File.Copy(thisFile, fbd.SelectedPath + "\\" + Path.GetFileName(thisFile),true);

                            foreach (string thisFile in iconFiles)
                                File.Copy(thisFile, fbd.SelectedPath + "\\" + Path.GetFileName(thisFile), true);
                        }

                        if (cmSender == cmExtractBothBrl || cmSender == cmExtractBrlan)
                        {
                            //Extract brlans
                            string[] bannerFiles = Directory.GetFiles(GetCurBannerPath() + "anim", "*.brlan");
                            string[] iconFiles = Directory.GetFiles(GetCurIconPath() + "anim", "*.brlan");

                            foreach (string thisFile in bannerFiles)
                                File.Copy(thisFile, fbd.SelectedPath + "\\" + Path.GetFileName(thisFile), true);

                            foreach (string thisFile in iconFiles)
                                File.Copy(thisFile, fbd.SelectedPath + "\\" + Path.GetFileName(thisFile), true);
                        }

                        InfoBox("Extracted files successfully!");
                    }
                }
                catch (Exception ex) { ErrorBox(ex.Message); }
            }
            else //DOL
            {
                try
                {
                    string[] tmdFile = Directory.GetFiles(TempUnpackPath, "*.tmd");
                    byte[] tmd = Wii.Tools.LoadFileToByteArray(tmdFile[0]);

                    int numContents = Wii.WadInfo.GetContentNum(tmd);

                    if (numContents == 3)
                    {
                        int bootIndex = Wii.WadInfo.GetBootIndex(tmd);
                        string appFile = string.Empty;

                        if (bootIndex == 1)
                        { appFile = "00000002.app"; }
                        else if (bootIndex == 2)
                        { appFile = "00000001.app"; }

                        if (!string.IsNullOrEmpty(appFile))
                        {
                            SaveFileDialog sfd = new SaveFileDialog();
                            sfd.Filter = "Wii Executables|*.dol"; sfd.FileName = (string.IsNullOrEmpty(tbAllLanguages.Text) ? tbEnglish.Text : tbAllLanguages.Text) + ".dol";

                            if (sfd.ShowDialog() == DialogResult.OK)
                            {
                                File.Copy(TempUnpackPath + appFile, sfd.FileName, true);
                                InfoBox(string.Format("The DOL file was successfully extracted to {0}!", Path.GetFileName(sfd.FileName)));
                            }
                        }
                        else
                            ErrorBox("The DOL file couldn't be found!");
                    }
                    else
                        ErrorBox("The DOL file couldn't be found!");
                }
                catch (Exception ex) { ErrorBox(ex.Message); }
            }
        }

        private void cmLoadAudioFile_Click(object sender, EventArgs e)
        {
            if (pbProgress.Value == 100)
            {
                OpenFileDialog ofd = new OpenFileDialog();

                if (File.Exists(Application.StartupPath + "\\lame.exe"))
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
                        ConvertMp3ToWave(ofd.FileName);
                    }
                    else
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
        }

        private void cmConvertToBns_Click(object sender, EventArgs e)
        {
            if (pbProgress.Value == 100)
            {
                CustomizeMii_BnsConvert bnsConvert = new CustomizeMii_BnsConvert(File.Exists(Application.StartupPath + "\\lame.exe"));

                if (bnsConvert.ShowDialog() == DialogResult.OK)
                {
                    BnsConversionInfo bnsInfo = new BnsConversionInfo();

                    bnsInfo.AudioFile = bnsConvert.AudioFile;
                    bnsInfo.StereoToMono = false;

                    if (bnsConvert.ChannelCount == 2)
                    {
                        if (MessageBox.Show("Do you want to convert the stereo Wave file to a mono BNS file?\nOnly the left channel will be taken.",
                            "Convert to Mono?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            bnsInfo.StereoToMono = true;
                    }

                    if (bnsConvert.LoopFromAudio)
                    { bnsInfo.Loop = BnsConversionInfo.LoopType.FromWave; }
                    else if (bnsConvert.LoopManually)
                    { bnsInfo.LoopStartSample = bnsConvert.LoopStartSample; bnsInfo.Loop = BnsConversionInfo.LoopType.Manual; }
                    else bnsInfo.Loop = BnsConversionInfo.LoopType.None;

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
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            string unpackDir = string.Format("{0} - {1}", Wii.WadInfo.GetChannelTitlesFromApp(TempUnpackPath + "00000000.app")[1], Wii.WadInfo.GetTitleID(Directory.GetFiles(TempUnpackPath, "*.tmd")[0], 1));
            fbd.Description = string.Format("Choose the path where the WAD will be extracted to. A folder called \"{0}\" containing the contents will be created!", unpackDir);

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                unpackDir = fbd.SelectedPath + "\\" + unpackDir;
                string[] files = Directory.GetFiles(TempUnpackPath);
                if (!Directory.Exists(unpackDir)) Directory.CreateDirectory(unpackDir);
                
                foreach (string thisFile in files)
                    File.Copy(thisFile, unpackDir + "\\" + Path.GetFileName(thisFile), true);

                InfoBox("Successfully extracted WAD file!");
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
                        byte[] wad = Wii.Tools.LoadFileToByteArray(ofd.FileName);

                        int numContents = Wii.WadInfo.GetContentNum(wad);

                        if (numContents == 3)
                        {
                            int bootIndex = Wii.WadInfo.GetBootIndex(wad);
                            string appFile = string.Empty;

                            if (bootIndex == 1)
                            { appFile = "00000002.app"; }
                            else if (bootIndex == 2)
                            { appFile = "00000001.app"; }

                            if (!string.IsNullOrEmpty(appFile))
                            {
                                if (Directory.Exists(TempTempPath + "TempWad")) Directory.Delete(TempTempPath + "TempWad", true);
                                Wii.WadUnpack.UnpackWad(ofd.FileName, TempTempPath + "TempWad");

                                File.Copy(TempTempPath + "TempWad\\" + appFile, TempDolPath, true);
                                SetText(tbDol, ofd.FileName);
                                btnBrowseDol.Text = "Clear";
                            }
                            else
                                ErrorBox("The DOL file couldn't be found!");
                        }
                        else
                            ErrorBox("The DOL file couldn't be found!");
                    }
                    catch (Exception ex)
                    {
                        SetText(tbDol, string.Empty);
                        ErrorBox(ex.Message);
                    }
                }
                else
                {
                    SetText(tbDol, ofd.FileName);
                    btnBrowseDol.Text = "Clear";
                }
            }
        }

        private void cmDolFromSource_Click(object sender, EventArgs e)
        {
            try
            {
                string[] tmdFile = Directory.GetFiles(TempUnpackPath, "*.tmd");
                byte[] tmd = Wii.Tools.LoadFileToByteArray(tmdFile[0]);

                int numContents = Wii.WadInfo.GetContentNum(tmd);

                if (numContents == 3)
                {
                    int bootIndex = Wii.WadInfo.GetBootIndex(tmd);
                    string appFile = string.Empty;

                    if (bootIndex == 1)
                    { appFile = "00000002.app"; }
                    else if (bootIndex == 2)
                    { appFile = "00000001.app"; }

                    if (!string.IsNullOrEmpty(appFile))
                    {
                        File.Copy(TempUnpackPath + appFile, TempDolPath, true);
                        SetText(tbDol, "Internal");
                        btnBrowseDol.Text = "Clear";
                    }
                    else
                        ErrorBox("The DOL file couldn't be found!");
                }
                else
                    ErrorBox("The DOL file couldn't be found!");
            }
            catch (Exception ex) 
            {
                SetText(tbDol, string.Empty);
                btnBrowseDol.Text = "Browse...";
                ErrorBox(ex.Message);
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

                ErrorBox(ex.Message);
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
            MultiReplace(sender == llbBannerMultiReplace);
        }
    }
}
