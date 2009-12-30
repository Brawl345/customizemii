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

//#define Mono //Change all "\\" to "/" (in all files, without quotes) while compiling for OS X / Linux (Mono)
//#define Debug //Always remember to turn off :)

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#if !Mono
using ForwardMii;
#endif

namespace CustomizeMii
{
    public partial class CustomizeMii_Main : Form
    {
        const string version = "2.1"; //Hint for myself: Never use a char in the Version (UpdateCheck)!
        const int SoundMaxLength = 30; //In seconds
        const int SoundWarningLength = 20; //In seconds
        const int BnsWarningLength = 45; //In seconds
        const int CreditsScrollSpeed = 85; //Timer Intervall for the scrolling Credits
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
        public static string[] ButtonTexts = new string[] { "Image", "Create WAD!", "Fire!", "Go Go Go!", "Let's do it!", "What are you waitin' for?", "I want my Channel!", "Houston, We've Got a Problem!", "Error, please contact anyone!", "Isn't she sweet?", "Is that milk?", "In your face!", "_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_", "Take me to a higher place!", "What's goin' on?", "I'm a Button!", "Click!", "Today's date is " + DateTime.Now.ToShortDateString(), "Launch Time: " + DateTime.Now.ToLongTimeString(), string.Format("My name is {0}", Environment.UserName) };
        public static string[] SourceWadUrls = new string[] { "StaticBase.wad", "MPlayer_CE_Short.wad", "MPlayer_CE_Long.wad", "Snes9xGX.wad", "FCE_Ultra_wilsoff.wad", "FCE_Ultra_Leathl.wad", "Wii64.wad", "WiiSX_Full.wad", "WiiSX_Retro.wad", "WADder_Base_1.wad", "WADder_Base_2.wad", "WADder_Base_3.wad", "UniiLoader.wad", "Backup_Channel.wad" };
        public static string[] SourceWadPreviewUrls = new string[] { "http://www.youtube.com/watch?v=pFNKldTYQq0", "http://www.youtube.com/watch?v=Up1RZebUc_U", "http://www.youtube.com/watch?v=Up1RZebUc_U", "http://www.youtube.com/watch?v=P-Mxd6DMvFY", "http://www.youtube.com/watch?v=wrbrg-DH_h4", "http://www.youtube.com/watch?v=MfiVbQaiXw8", "http://www.youtube.com/watch?v=krCQ2J7ZH8Y", "http://www.youtube.com/watch?v=rZC1DKUM6QI", "http://www.youtube.com/watch?v=Uiy8w-bp1kI", "http://www.youtube.com/watch?v=BbSYCSI8tz8", "http://www.youtube.com/watch?v=PIFZevHQ8lQ", "http://www.youtube.com/watch?v=OIhvDNjphhc", "http://www.youtube.com/watch?v=KLcncEArQLY&NR=1", "http://www.youtube.com/watch?v=xE_EgdCRV1I" };
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
        private bool BrlytChanged = false;
        private bool BrlanChanged = false;
        private Progress currentProgress;
        private EventHandler ProgressUpdate;
        private int UnpackFolderErrorCount = 0;
        private Stopwatch CreationTimer = new Stopwatch();
        private List<string> BannerTransparents = new List<string>();
        private List<string> IconTransparents = new List<string>();
        private string Mp3Path;
        private Forwarder.Simple SimpleForwarder = new Forwarder.Simple();
        private Forwarder.Complex ComplexForwarder = new Forwarder.Complex();
        private delegate void BoxInvoker(string message);
        private delegate void SetTextInvoker(string text, TextBox tb);
        double separatorBtn;
        Timer tmrCredits = new Timer();

        public CustomizeMii_Main()
        {
            InitializeComponent();
            this.Icon = global::CustomizeMii.Properties.Resources.CustomizeMii;
        }

        private void CustomizeMii_Main_Load(object sender, EventArgs e)
        {
            UpdatePaths();
            UpdateCheck();

#if !Mono
            UpdateCheckForwardMii();
            CommonKeyCheck();
#endif

            InitializeStartup();

#if Mono
            //TextBox.MaxLength is not implemented in Mono, so don't use it
            for (int i = 0; i < tabControl.TabPages.Count; i++)
                for(int j=0;j<tabControl.TabPages[i].Controls.Count;j++)
                    if (tabControl.TabPages[i].Controls[j] is TextBox) ((TextBox)tabControl.TabPages[i].Controls[j]).MaxLength = 32000;
#endif
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
            this.Text = this.Text.Replace("X", version);
            this.lbCreditVersion.Text = this.lbCreditVersion.Text.Replace("X", version);
            if (Directory.Exists(TempPath)) Directory.Delete(TempPath, true);
            ProgressUpdate = new EventHandler(this.UpdateProgress);
            SetButtonText();
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

            if (File.Exists(Application.StartupPath + "\\ForwardMii.dll") && lbForwardMiiVersion.Tag != (object)"Update")
            {
                SetForwardMiiLabel();
            }

#if !Debug
            DisableControls(null, null);
#endif
        }

        void rtbInstructions_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                Process.Start(e.LinkText);
            }
            catch (Exception ex) { ErrorBox(ex.Message); }
        }

        private void SetForwardMiiLabel()
        {
            try
            {
                lbForwardMiiVersion.Text = lbForwardMiiVersion.Text.Replace("X", ForwardMii_Plugin.GetVersion());
                lbForwardMiiVersion.Visible = true;
            }
            catch { }
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
            ToolTip TTip = new ToolTip();
            TTip.SetToolTip(btnCreateWad, "Save WAD or send it directly to the Wii...");
            TTip.SetToolTip(btnBrowseSource, "Browse for a WAD that is used as a Base...");
            TTip.SetToolTip(btnLoadBaseWad, "Load the selected Base WAD...");
            TTip.SetToolTip(btnPreviewBaseWad, "Preview the selected Base WAD, a Browserwindow will be opened...");
            TTip.SetToolTip(btnSaveBaseWad, "Download and save the selected Base WAD to your HDD...");
            TTip.SetToolTip(btnBrowseReplace, "Browse for a Banner / Icon / Sound to use instead of the one within the Base WAD...\nWAD's, 00000000.app's and banner.bin's / icon.bin's / sound.bin's can be loaded...");
            TTip.SetToolTip(btnClearReplace, "Clear the replaced Banner / Icon / Sound and use the one within the Base WAD...");
            TTip.SetToolTip(btnBrowseDol, "Browse for a DOL file that will be inserted into the WAD\nor choose the DOL form the source WAD to switch the NAND Loader...");
            TTip.SetToolTip(btnBrowseSound, "Browse for a sound that will be inserted into the WAD\nor convert a sound to BNS format...");
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
            TTip.SetToolTip(btnOptionsExtract, "Extract contents of the WAD...");
            TTip.SetToolTip(btnForwarder, "Create a forwarder that will be inserted as a DOL...");
            
            TTip.SetToolTip(cbLz77, "Use Lz77 compression for the banner.bin and icon.bin...\nIf the created WAD does not work, try it without compression first...");
            TTip.SetToolTip(cbFailureChecks, "Turn off the security checks...\nNot recommended, you may get a bricking WAD...");
        }

        private bool CheckForForwardMii()
        {
            if (File.Exists(Application.StartupPath + "\\ForwardMii.dll"))
                return true;
            else
                return false;
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

        private void SetButtonText()
        {
            //Random Randomizer = new Random();
            //btnCreateWad.Text = ButtonTexts[Randomizer.Next(0, ButtonTexts.Length - 1)];

            //if (btnCreateWad.Text == "Image")
            //{
            //    btnCreateWad.Text = string.Empty;
            //    btnCreateWad.Image = Properties.Resources.btnCreateWad;
            //}

            btnCreateWad.Text = string.Empty;

            Image tmpImg = new Bitmap(btnCreateWad.Width, btnCreateWad.Height);
            Graphics gImg = Graphics.FromImage(tmpImg);

            gImg.Clear(Color.Transparent);

            separatorBtn = btnCreateWad.Width * 0.5;

            gImg.DrawLine(Pens.Gray, new Point((int)separatorBtn, 0), new Point((int)separatorBtn, btnCreateWad.Height));

            string sSend = "Send";
            string sSave = "Save";

            gImg.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit; 
            gImg.DrawString(sSend, btnCreateWad.Font, Brushes.Black, new PointF(95, 10));
            gImg.DrawString(sSave, btnCreateWad.Font, Brushes.Black, new PointF(320, 10));

            btnCreateWad.Image = tmpImg;
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
            pbProgress.Value = currentProgress.progressValue;

            if (!string.IsNullOrEmpty(currentProgress.progressState))
            {
                lbStatusText.Text = currentProgress.progressState;
                currentProgress.progressState = string.Empty;
            }
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

        private void SetText(TextBox tb, string text)
        {
            SetTextInvoker invoker = new SetTextInvoker(this.SetText);
            this.Invoke(invoker, text, tb);
        }

        private void SetText(string text, TextBox tb)
        {
            tb.Text = text;
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

        private void SetSourceWad(object sender, EventArgs e)
        {
            tbSourceWad.Text = SourceWad;
        }

        private void ForwarderDialogSimple()
        {
            CustomizeMii_InputBox ib = new CustomizeMii_InputBox(false);
            ib.Size = new Size(ib.Size.Width, 120);
            ib.lbInfo.Text = "Enter the application folder where the forwarder will point to (3-18 chars)";
            ib.tbInput.MaxLength = 18;
            ib.btnExit.Text = "Close";
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
            cf.tbAppFolder.Text = ComplexForwarder.AppFolder;
            //cf.rbSD.Checked = !ComplexForwarder.UsbFirst;
            //cf.rbUSB.Checked = ComplexForwarder.UsbFirst;
            //cf.rbDOL.Checked = !ComplexForwarder.ElfFirst;
            //cf.rbELF.Checked = ComplexForwarder.ElfFirst;

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
                ComplexForwarder.AppFolder = cf.tbAppFolder.Text;
                //ComplexForwarder.UsbFirst = cf.rbUSB.Checked;
                //ComplexForwarder.ElfFirst = cf.rbELF.Checked;
                ComplexForwarder.Image43 = cf.tbImage43.Text;
                ComplexForwarder.Image169 = cf.tbImage169.Text;

                SetText(tbDol, string.Format("Complex Forwarder: \"{0}\"", ComplexForwarder.AppFolder));
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

        void tmrCredits_Tick(object sender, EventArgs e)
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
                        else if (Ctrl is CheckBox && Ctrl.Tag != (object)"Independent") Ctrl.Enabled = true;
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
                        else if (Ctrl is CheckBox && Ctrl.Tag != (object)"Independent") Ctrl.Enabled = false;
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
#if Mono
                    string NewVersion = GetVersion.DownloadString("http://customizemii.googlecode.com/svn/monoversion.txt");
#else
                    string NewVersion = GetVersion.DownloadString("http://customizemii.googlecode.com/svn/version.txt");
#endif
                    int newVersion = Convert.ToInt32(NewVersion.Replace(".", string.Empty).Length == 2 ? (NewVersion.Replace(".", string.Empty) + "0") : NewVersion.Replace(".", string.Empty));
                    int thisVersion = Convert.ToInt32(version.Replace(".", string.Empty).Length == 2 ? (version.Replace(".", string.Empty) + "0") : version.Replace(".", string.Empty));

                    if (newVersion > thisVersion)
                    {
                        llbUpdateAvailabe.Text = llbUpdateAvailabe.Text.Replace("X", NewVersion);
                        llbUpdateAvailabe.Visible = true;
                        lbForwardMiiVersion.Tag = "Update";

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

#if !Mono
        private string GetForwardMiiVersion()
        {
            return ForwardMii_Plugin.GetVersion();
        }

        private void UpdateCheckForwardMii()
        {
            if (File.Exists(Application.StartupPath + "\\ForwardMii.dll"))
            {
                if (CheckInet() == true)
                {
                    try
                    {
                        string CurrentVersion = GetForwardMiiVersion();

                        if (!string.IsNullOrEmpty(CurrentVersion))
                        {
                            WebClient GetVersion = new WebClient();
                            string NewVersion = GetVersion.DownloadString("http://customizemii.googlecode.com/svn/forwardmii-version.txt");

                            int newVersion = Convert.ToInt32(NewVersion.Replace(".", string.Empty).Length == 2 ? (NewVersion.Replace(".", string.Empty) + "0") : NewVersion.Replace(".", string.Empty));
                            int thisVersion = Convert.ToInt32(CurrentVersion.Replace(".", string.Empty).Length == 2 ? (CurrentVersion.Replace(".", string.Empty) + "0") : CurrentVersion.Replace(".", string.Empty));

                            if (newVersion > thisVersion)
                            {
                                if (MessageBox.Show("Version " + NewVersion +
                                    " of the ForwardMii-Plugin is availabe.\nDo you want the download page to be opened?",
                                    "ForwardMii Update availabe", MessageBoxButtons.YesNo, MessageBoxIcon.Information) ==
                                    DialogResult.Yes)
                                {
                                    Process.Start("http://code.google.com/p/customizemii/downloads/list");
                                }
                            }
                        }
                    }
                    catch { }
                }
            }
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
#if Mono
            CommonKeyCheck();
#endif

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

        private void btnLoadBaseWad_Click(object sender, EventArgs e)
        {
            if (lbxBaseWads.SelectedIndex != -1)
            {
                if (CheckInet() == true)
                {
                    if (pbProgress.Value == 100)
                    {
#if Mono
                        CommonKeyCheck();
#endif

                        if (tbSourceWad.Text != SourceWadUrls[lbxBaseWads.SelectedIndex])
                        {
                            try
                            {
                                SourceWad = "http://customizemii.googlecode.com/svn/branches/Base_WADs/" + SourceWadUrls[lbxBaseWads.SelectedIndex];
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
            pbProgress.Value = 100;
            lbStatusText.Text = string.Empty;

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
                            if (tbSourceWad.Text == SourceWadUrls[lbxBaseWads.SelectedIndex] && File.Exists(TempWadPath))
                            {
                                File.Copy(TempWadPath, sfd.FileName, true);
                                InfoBox(string.Format("Saved channel as {0}", Path.GetFileName(sfd.FileName)));
                            }
                            else
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
            if (!string.IsNullOrEmpty(tbSourceWad.Text))
            {
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
                    ofd.Filter = "PNG|*.png|JPG|*.jpg|GIF|*.gif|BMP|*.bmp|TPL|*.tpl|All|*.png;*.jpg;*.gif;*.bmp;*.tpl";
                    ofd.FilterIndex = 6;

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            string Tpl = BannerTplPath + lbxBannerTpls.SelectedItem.ToString().Replace(" (Transparent)", string.Empty);
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
                sfd.Filter = "PNG|*.png|JPG|*.jpg|GIF|*.gif|BMP|*.bmp|TPL|*.tpl|All|*.png;*.jpg;*.gif;*.bmp;*.tpl";
                sfd.FilterIndex = 6;
                sfd.FileName = lbxBannerTpls.SelectedItem.ToString().Replace(".tpl", string.Empty);

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (!sfd.FileName.ToLower().EndsWith(".tpl"))
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
                    pvw.Text = string.Format("CustomizeMii - Preview ({0} x {1})", Img.Width, Img.Height);

                    pvw.ShowDialog();
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

                if (Format == 0 || Format == 1 || Format == 2)
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Filter = "PNG|*.png|JPG|*.jpg|GIF|*.gif|BMP|*.bmp|TPL|*.tpl|All|*.png;*.jpg;*.gif;*.bmp;*.tpl";
                    ofd.FilterIndex = 6;

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            string Tpl = IconTplPath + lbxIconTpls.SelectedItem.ToString().Replace(" (Transparent)", string.Empty);
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
                sfd.Filter = "PNG|*.png|JPG|*.jpg|GIF|*.gif|BMP|*.bmp|TPL|*.tpl|All|*.png;*.jpg;*.gif;*.bmp;*.tpl";
                sfd.FilterIndex = 6;
                sfd.FileName = lbxIconTpls.SelectedItem.ToString().Replace(".tpl", string.Empty);

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (!sfd.FileName.ToLower().EndsWith(".tpl"))
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
                    pvw.Text = string.Format("CustomizeMii - Preview ({0} x {1})", Img.Width, Img.Height);
                    
                    pvw.ShowDialog();
                }
                catch (Exception ex)
                {
                    ErrorBox(ex.Message);
                }

                lbxIconTpls.Focus();
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
                if (tbSound.Text.StartsWith("BNS:") || tbSound.Text.EndsWith(".bns"))
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
                    if (!thisFile.ToLower().EndsWith(".app") &&
                        !thisFile.ToLower().EndsWith(".cert") &&
                        !thisFile.ToLower().EndsWith(".tik") &&
                        !thisFile.ToLower().EndsWith(".tmd"))
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
                    if (!thisDir.ToLower().EndsWith("meta"))
                        Directory.Delete(thisDir, true);

                string[] AppFiles = Directory.GetFiles(TempUnpackPath + "00000000.app_OUT\\meta");
                string[] AppDirs = Directory.GetDirectories(TempUnpackPath + "00000000.app_OUT\\meta");

                foreach (string thisFile in AppFiles)
                {
                    if (!thisFile.ToLower().EndsWith("banner.bin") &&
                        !thisFile.ToLower().EndsWith("icon.bin") &&
                        !thisFile.ToLower().EndsWith("sound.bin"))
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
                        if (!thisDir.ToLower().EndsWith("arc"))
                            Directory.Delete(thisDir, true);

                    string[] BannerFiles = Directory.GetFiles(TempUnpackPath + "00000000.app_OUT\\meta\\banner.bin_OUT\\arc");
                    string[] BannerDirs = Directory.GetDirectories(TempUnpackPath + "00000000.app_OUT\\meta\\banner.bin_OUT\\arc");

                    foreach (string thisFile in BannerFiles)
                        File.Delete(thisFile);

                    if (BannerDirs.Length > 3)
                    {
                        foreach (string thisDir in BannerDirs)
                        {
                            if (!thisDir.ToLower().EndsWith("anim") &&
                                !thisDir.ToLower().EndsWith("blyt") &&
                                !thisDir.ToLower().EndsWith("timg"))
                                Directory.Delete(thisDir, true);
                        }
                    }

                    string[] AnimFiles = Directory.GetFiles(TempUnpackPath + "00000000.app_OUT\\meta\\banner.bin_OUT\\arc\\anim");
                    string[] AnimDirs = Directory.GetDirectories(TempUnpackPath + "00000000.app_OUT\\meta\\banner.bin_OUT\\arc\\anim");

                    foreach (string thisFile in AnimFiles)
                    {
                        if (!thisFile.ToLower().EndsWith(".brlan"))
                            File.Delete(thisFile);
                    }

                    foreach (string thisDir in AnimDirs)
                        Directory.Delete(thisDir, true);

                    string[] BlytFiles = Directory.GetFiles(TempUnpackPath + "00000000.app_OUT\\meta\\banner.bin_OUT\\arc\\blyt");
                    string[] BlytDirs = Directory.GetDirectories(TempUnpackPath + "00000000.app_OUT\\meta\\banner.bin_OUT\\arc\\blyt");

                    foreach (string thisFile in BlytFiles)
                    {
                        if (!thisFile.ToLower().EndsWith(".brlyt"))
                            File.Delete(thisFile);
                    }

                    foreach (string thisDir in BlytDirs)
                        Directory.Delete(thisDir, true);

                    string[] TimgFiles = Directory.GetFiles(TempUnpackPath + "00000000.app_OUT\\meta\\banner.bin_OUT\\arc\\timg");
                    string[] TimgDirs = Directory.GetDirectories(TempUnpackPath + "00000000.app_OUT\\meta\\banner.bin_OUT\\arc\\timg");

                    foreach (string thisFile in TimgFiles)
                    {
                        if (!thisFile.ToLower().EndsWith(".tpl"))
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
                        if (!thisDir.ToLower().EndsWith("arc"))
                            Directory.Delete(thisDir, true);

                    string[] BannerFiles = Directory.GetFiles(TempBannerPath + "arc");
                    string[] BannerDirs = Directory.GetDirectories(TempBannerPath + "arc");

                    foreach (string thisFile in BannerFiles)
                        File.Delete(thisFile);

                    if (BannerDirs.Length > 3)
                    {
                        foreach (string thisDir in BannerDirs)
                        {
                            if (!thisDir.ToLower().EndsWith("anim") &&
                                !thisDir.ToLower().EndsWith("blyt") &&
                                !thisDir.ToLower().EndsWith("timg"))
                                Directory.Delete(thisDir, true);
                        }
                    }

                    string[] AnimFiles = Directory.GetFiles(TempBannerPath + "arc\\anim");
                    string[] AnimDirs = Directory.GetDirectories(TempBannerPath + "arc\\anim");

                    foreach (string thisFile in AnimFiles)
                    {
                        if (!thisFile.ToLower().EndsWith(".brlan"))
                            File.Delete(thisFile);
                    }

                    foreach (string thisDir in AnimDirs)
                        Directory.Delete(thisDir, true);

                    string[] BlytFiles = Directory.GetFiles(TempBannerPath + "arc\\blyt");
                    string[] BlytDirs = Directory.GetDirectories(TempBannerPath + "arc\\blyt");

                    foreach (string thisFile in BlytFiles)
                    {
                        if (!thisFile.ToLower().EndsWith(".brlyt"))
                            File.Delete(thisFile);
                    }

                    foreach (string thisDir in BlytDirs)
                        Directory.Delete(thisDir, true);

                    string[] TimgFiles = Directory.GetFiles(TempBannerPath + "arc\\timg");
                    string[] TimgDirs = Directory.GetDirectories(TempBannerPath + "arc\\timg");

                    foreach (string thisFile in TimgFiles)
                    {
                        if (!thisFile.ToLower().EndsWith(".tpl"))
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
                        if (!thisDir.ToLower().EndsWith("arc"))
                            Directory.Delete(thisDir, true);

                    string[] IconFiles = Directory.GetFiles(TempUnpackPath + "00000000.app_OUT\\meta\\icon.bin_OUT\\arc");
                    string[] IconDirs = Directory.GetDirectories(TempUnpackPath + "00000000.app_OUT\\meta\\icon.bin_OUT\\arc");

                    foreach (string thisFile in IconFiles)
                        File.Delete(thisFile);

                    if (IconDirs.Length > 3)
                    {
                        foreach (string thisDir in IconDirs)
                        {
                            if (!thisDir.ToLower().EndsWith("anim") &&
                                !thisDir.ToLower().EndsWith("blyt") &&
                                !thisDir.ToLower().EndsWith("timg"))
                                Directory.Delete(thisDir, true);
                        }
                    }

                    string[] AnimFiles = Directory.GetFiles(TempUnpackPath + "00000000.app_OUT\\meta\\icon.bin_OUT\\arc\\anim");
                    string[] AnimDirs = Directory.GetDirectories(TempUnpackPath + "00000000.app_OUT\\meta\\icon.bin_OUT\\arc\\anim");

                    foreach (string thisFile in AnimFiles)
                    {
                        if (!thisFile.ToLower().EndsWith(".brlan"))
                            File.Delete(thisFile);
                    }

                    foreach (string thisDir in AnimDirs)
                        Directory.Delete(thisDir, true);

                    string[] BlytFiles = Directory.GetFiles(TempUnpackPath + "00000000.app_OUT\\meta\\icon.bin_OUT\\arc\\blyt");
                    string[] BlytDirs = Directory.GetDirectories(TempUnpackPath + "00000000.app_OUT\\meta\\icon.bin_OUT\\arc\\blyt");

                    foreach (string thisFile in BlytFiles)
                    {
                        if (!thisFile.ToLower().EndsWith(".brlyt"))
                            File.Delete(thisFile);
                    }

                    foreach (string thisDir in BlytDirs)
                        Directory.Delete(thisDir, true);

                    string[] TimgFiles = Directory.GetFiles(TempUnpackPath + "00000000.app_OUT\\meta\\icon.bin_OUT\\arc\\timg");
                    string[] TimgDirs = Directory.GetDirectories(TempUnpackPath + "00000000.app_OUT\\meta\\icon.bin_OUT\\arc\\timg");

                    foreach (string thisFile in TimgFiles)
                    {
                        if (!thisFile.ToLower().EndsWith(".tpl"))
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
                        if (!thisDir.ToLower().EndsWith("arc"))
                            Directory.Delete(thisDir, true);

                    string[] IconFiles = Directory.GetFiles(TempIconPath + "arc");
                    string[] IconDirs = Directory.GetDirectories(TempIconPath + "arc");

                    foreach (string thisFile in IconFiles)
                        File.Delete(thisFile);

                    if (IconDirs.Length > 3)
                    {
                        foreach (string thisDir in IconDirs)
                        {
                            if (!thisDir.ToLower().EndsWith("anim") &&
                                !thisDir.ToLower().EndsWith("blyt") &&
                                !thisDir.ToLower().EndsWith("timg"))
                                Directory.Delete(thisDir, true);
                        }
                    }

                    string[] AnimFiles = Directory.GetFiles(TempIconPath + "arc\\anim");
                    string[] AnimDirs = Directory.GetDirectories(TempIconPath + "arc\\anim");

                    foreach (string thisFile in AnimFiles)
                    {
                        if (!thisFile.ToLower().EndsWith(".brlan"))
                            File.Delete(thisFile);
                    }

                    foreach (string thisDir in AnimDirs)
                        Directory.Delete(thisDir, true);

                    string[] BlytFiles = Directory.GetFiles(TempIconPath + "arc\\blyt");
                    string[] BlytDirs = Directory.GetDirectories(TempIconPath + "arc\\blyt");

                    foreach (string thisFile in BlytFiles)
                    {
                        if (!thisFile.ToLower().EndsWith(".brlyt"))
                            File.Delete(thisFile);
                    }

                    foreach (string thisDir in BlytDirs)
                        Directory.Delete(thisDir, true);

                    string[] TimgFiles = Directory.GetFiles(TempIconPath + "arc\\timg");
                    string[] TimgDirs = Directory.GetDirectories(TempIconPath + "arc\\timg");

                    foreach (string thisFile in TimgFiles)
                    {
                        if (!thisFile.ToLower().EndsWith(".tpl"))
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
            Point mousePos = MousePosition;

            if (mousePos.X < (this.Location.X + btnCreateWad.Location.X + Math.Ceiling(separatorBtn) + 3))
            {
                if (pbProgress.Value == 100)
                {
                    if (!string.IsNullOrEmpty(tbSourceWad.Text))
                    {
                        if (cbFailureChecks.Checked == true || FailureCheck() == true)
                        {
                            try
                            {
                                WadCreationInfo wadInfo = new WadCreationInfo();
                                wadInfo.outFile = TempPath + "SendToWii.wad";
                                wadInfo.nandLoader = (WadCreationInfo.NandLoader)cmbNandLoader.SelectedIndex;
                                wadInfo.sendToWii = true;

                                BackgroundWorker bwCreateWad = new BackgroundWorker();
                                bwCreateWad.DoWork += new DoWorkEventHandler(bwCreateWad_DoWork);
                                bwCreateWad.ProgressChanged += new ProgressChangedEventHandler(bwCreateWad_ProgressChanged);
                                bwCreateWad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwCreateWad_RunWorkerCompleted);
                                bwCreateWad.WorkerReportsProgress = true;
                                bwCreateWad.RunWorkerAsync(wadInfo);

                                // @WiiCrazy: The WAD will be saved to >> TempTempPath + "SendToWii.wad" <<
                                // here. Now a loop that waits for the BackgroundWorker to finish and then opens
                                // a new window (wiiload - window or whatever) ?!
                                // If it finishes successfully, the variable >> sendWadReady << will turn into 1,
                                // if it errors, it will turn into -1, as long as it's running it is 0.
                            }
                            catch (Exception ex)
                            {
                                ErrorBox(ex.Message);
                            }
                        }
                    }
                }
            }
            else
            {
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
                                    wadInfo.outFile = sfd.FileName;
                                    wadInfo.nandLoader = (WadCreationInfo.NandLoader)cmbNandLoader.SelectedIndex;

                                    BackgroundWorker bwCreateWad = new BackgroundWorker();
                                    bwCreateWad.DoWork += new DoWorkEventHandler(bwCreateWad_DoWork);
                                    bwCreateWad.ProgressChanged += new ProgressChangedEventHandler(bwCreateWad_ProgressChanged);
                                    bwCreateWad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwCreateWad_RunWorkerCompleted);
                                    bwCreateWad.WorkerReportsProgress = true;
                                    bwCreateWad.RunWorkerAsync(wadInfo);
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
                            if (!ofd.FileName.ToLower().EndsWith(".tpl"))
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

                    if (ofd.FileName.ToLower().EndsWith(".tpl"))
                    {
                        File.Copy(ofd.FileName, CurBannerPath + "timg\\" + TplName, true);
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
                            if (!ofd.FileName.ToLower().EndsWith(".tpl"))
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

                    if (ofd.FileName.ToLower().EndsWith(".tpl"))
                    {
                        File.Copy(ofd.FileName, CuriconPath + "timg\\" + TplName, true);
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
                            BannerTransparents.Add(thisItem);
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
                        BannerTransparents.Remove(thisItem.Replace(" (Transparent)", string.Empty));
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
                            string thisItem = lbxIconTpls.SelectedItem.ToString().Replace(" (Transparent)", string.Empty);
                            lbxIconTpls.Items.Remove(lbxIconTpls.SelectedItem);
                            lbxIconTpls.Items.Add(thisItem + " (Transparent)");
                            lbxIconTpls.SelectedItem = thisItem + " (Transparent)";
                            IconTransparents.Add(thisItem);
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
                        IconTransparents.Remove(thisItem.Replace(" (Transparent)", string.Empty));
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
                if (MessageBox.Show("You don't have the ForwardMii.dll in your application folder.\nYou can download it on the project page, do you want the page to be opened?", "Plugin not availabe", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    Process.Start("http://code.google.com/p/customizemii/downloads/list");
                }
            }
#endif
        }

        private void cmForwarderItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem cmSender = sender as ToolStripMenuItem;

            if (cmSender == cmSimpleForwarder)
                ForwarderDialogSimple();
            else
                ForwarderDialogComplex();
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
                if (ofd.FileName.EndsWith(".wad"))
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
    }
}
