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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;
using libWiiSharp;

namespace CustomizeMii
{
    partial class CustomizeMii_Main
    {
        private TransmitInfo transmitInfo;
        private WadCreationInfo wadCreationInfo;
        private bool internalSound;

        private WAD sourceWad = new WAD();
        private U8 bannerBin = new U8();
        private U8 newBannerBin = new U8();
        private U8 iconBin = new U8();
        private U8 newIconBin = new U8();
        private byte[] newSoundBin = new byte[0];
        private byte[] newDol = new byte[0];
        private string replacedBanner = string.Empty;
        private string replacedIcon = string.Empty;
        private string replacedSound = string.Empty;

        void bwBannerReplace_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            currentProgress.progressValue = 100;
            currentProgress.progressState = " ";

            this.Invoke(ProgressUpdate);
            setControlText(tbReplace, replacedBanner);
        }

        void bwBannerReplace_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            currentProgress.progressValue = e.ProgressPercentage;
            currentProgress.progressState = (string)e.UserState;

            this.Invoke(ProgressUpdate);
        }

        void bwBannerReplace_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker bwBannerReplace = sender as BackgroundWorker;
                replacedBanner = (string)e.Argument;

                if (replacedBanner.ToLower().EndsWith(".bin"))
                {
                    bwBannerReplace.ReportProgress(0, "Loading banner.bin...");
                    newBannerBin.LoadFile(replacedBanner);
                }
                else if (replacedBanner.ToLower().EndsWith(".app"))
                {
                    bwBannerReplace.ReportProgress(0, "Loading 00000000.app...");
                    U8 tmpU8 = U8.Load(replacedBanner);

                    bwBannerReplace.ReportProgress(50, "Loading banner.bin...");
                    for (int i = 0; i < tmpU8.NumOfNodes; i++)
                        if (tmpU8.StringTable[i].ToLower() == "banner.bin")
                        { newBannerBin.LoadFile(tmpU8.Data[i]); break; }
                }
                else //wad
                {
                    bwBannerReplace.ReportProgress(0, "Loading WAD...");
                    WAD tmpWad = WAD.Load(replacedBanner);

                    if (!tmpWad.HasBanner)
                        throw new Exception("CustomizeMii only handles Channel WADs!");

                    bwBannerReplace.ReportProgress(60, "Loading banner.bin...");
                    for (int i = 0; i < tmpWad.BannerApp.NumOfNodes; i++)
                        if (tmpWad.BannerApp.StringTable[i].ToLower() == "banner.bin")
                        { newBannerBin.LoadFile(tmpWad.BannerApp.Data[i]); break; }
                }

                bannerTransparents.Clear();

                addTpls();
                addBrlyts();
                addBrlans();
            }
            catch (Exception ex)
            {
                replacedBanner = string.Empty;
                errorBox(ex.Message);
            }
        }

        void bwIconReplace_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            currentProgress.progressValue = 100;
            currentProgress.progressState = " ";

            this.Invoke(ProgressUpdate);
            setControlText(tbReplace, replacedIcon);
        }

        void bwIconReplace_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            currentProgress.progressValue = e.ProgressPercentage;
            currentProgress.progressState = (string)e.UserState;

            this.Invoke(ProgressUpdate);
        }

        void bwIconReplace_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker bwIconReplace = sender as BackgroundWorker;
                replacedIcon = (string)e.Argument;

                bwIconReplace.ReportProgress(0);

                if (replacedIcon.ToLower().EndsWith(".bin"))
                {
                    bwIconReplace.ReportProgress(0, "Loading icon.bin...");
                    newIconBin.LoadFile(replacedIcon);
                }
                else if (replacedIcon.ToLower().EndsWith(".app"))
                {
                    bwIconReplace.ReportProgress(0, "Loading 00000000.app...");
                    U8 tmpU8 = U8.Load(replacedIcon);

                    bwIconReplace.ReportProgress(50, "Loading icon.bin...");
                    for (int i = 0; i < tmpU8.NumOfNodes; i++)
                        if (tmpU8.StringTable[i].ToLower() == "icon.bin")
                        { newIconBin.LoadFile(tmpU8.Data[i]); break; }
                }
                else //wad
                {
                    bwIconReplace.ReportProgress(0, "Loading WAD...");
                    WAD tmpWad = WAD.Load(replacedIcon);

                    if (!tmpWad.HasBanner)
                        throw new Exception("CustomizeMii only handles Channel WADs!");

                    bwIconReplace.ReportProgress(60, "Loading icon.bin...");
                    for (int i = 0; i < tmpWad.BannerApp.NumOfNodes; i++)
                        if (tmpWad.BannerApp.StringTable[i].ToLower() == "icon.bin")
                        { newIconBin.LoadFile(tmpWad.BannerApp.Data[i]); break; }
                }

                iconTransparents.Clear();

                addTpls();
                addBrlyts();
                addBrlans();
            }
            catch (Exception ex)
            {
                replacedIcon = string.Empty;
                errorBox(ex.Message);
            }
        }

        void bwSoundReplace_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            currentProgress.progressValue = 100;
            currentProgress.progressState = " ";

            this.Invoke(ProgressUpdate);
            setControlText(tbReplace, replacedSound);
        }

        void bwSoundReplace_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            currentProgress.progressValue = e.ProgressPercentage;
            currentProgress.progressState = (string)e.UserState;

            this.Invoke(ProgressUpdate);
        }

        void bwSoundReplace_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker bwSoundReplace = sender as BackgroundWorker;
                replacedSound = (string)e.Argument;

                bwSoundReplace.ReportProgress(0);

                if (replacedSound.ToLower().EndsWith(".bin"))
                {
                    bwSoundReplace.ReportProgress(0, "Loading sound.bin...");
                    newSoundBin = File.ReadAllBytes(replacedSound);
                }
                else if (replacedSound.ToLower().EndsWith(".app"))
                {
                    bwSoundReplace.ReportProgress(0, "Loading 00000000.app...");
                    U8 tmpU8 = U8.Load(replacedSound);

                    bwSoundReplace.ReportProgress(80, "Loading sound.bin...");
                    for (int i = 0; i < tmpU8.NumOfNodes; i++)
                        if (tmpU8.StringTable[i].ToLower() == "sound.bin")
                        { newSoundBin = tmpU8.Data[i]; break; }
                }
                else
                {
                    bwSoundReplace.ReportProgress(0, "Loading WAD...");
                    WAD tmpWad = WAD.Load(replacedSound);

                    if (!tmpWad.HasBanner)
                        throw new Exception("CustomizeMii only handles Channel WADs!");

                    bwSoundReplace.ReportProgress(90, "Loading sound.bin...");
                    for (int i = 0; i < tmpWad.BannerApp.NumOfNodes; i++)
                        if (tmpWad.BannerApp.StringTable[i].ToLower() == "sound.bin")
                        { newSoundBin = tmpWad.BannerApp.Data[i]; break; }
                }
            }
            catch (Exception ex)
            {
                replacedSound = string.Empty;
                errorBox(ex.Message);
            }
        }

        void bwConvertToBns_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!string.IsNullOrEmpty(replacedSound))
            {
                if (internalSound == true)
                    setControlText(tbSound, "BNS: Internal Sound");
                else
                    setControlText(tbSound, "BNS: " + replacedSound);

                btnBrowseSound.Text = "Clear";

                if (cmbReplace.SelectedIndex == 2) setControlText(tbReplace, string.Empty);
            }

            currentProgress.progressValue = 100;
            currentProgress.progressState = " ";

            this.Invoke(ProgressUpdate);
        }

        void bwConvertToBns_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            currentProgress.progressValue = e.ProgressPercentage;
            if (!string.IsNullOrEmpty((string)e.UserState)) currentProgress.progressState = (string)e.UserState;

            this.Invoke(ProgressUpdate);
        }

        void bns_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            currentProgress.progressValue = e.ProgressPercentage;

            this.Invoke(ProgressUpdate);
        }

        void bwConvertToBns_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker bwConvertToBns = sender as BackgroundWorker;
                BnsConversionInfo bnsInfo = (BnsConversionInfo)e.Argument;
                byte[] audioData = new byte[0];

                if (bnsInfo.audioFile.ToLower() == "internal sound")
                {
                    for (int i = 0; i < sourceWad.BannerApp.NumOfNodes; i++)
                    {
                        if (sourceWad.BannerApp.StringTable[i].ToLower() == "sound.bin")
                            audioData = Headers.IMD5.RemoveHeader(sourceWad.BannerApp.Data[i]);
                    }

                    internalSound = true;
                }
                else
                    audioData = File.ReadAllBytes(bnsInfo.audioFile);

                bool mp3 = bnsInfo.audioFile.EndsWith(".mp3");
                if (mp3 && bnsInfo.loopType == BnsConversionInfo.LoopType.FromWave) bnsInfo.loopType = BnsConversionInfo.LoopType.None;

                if (mp3)
                {
                    bwConvertToBns.ReportProgress(0, "Converting MP3...");

                    ProcessStartInfo lameI = new ProcessStartInfo(Application.StartupPath + Path.DirectorySeparatorChar + "lame.exe",
                        string.Format("--decode \"{0}\" \"{1}\"", bnsInfo.audioFile, Application.StartupPath + Path.DirectorySeparatorChar + "customizemii_temp.wav"));
                    lameI.CreateNoWindow = true;
                    lameI.UseShellExecute = false;
                    lameI.RedirectStandardError = true;

                    Process lame = Process.Start(lameI);

                    string thisLine = string.Empty;
                    while (lame.HasExited == false)
                    {
                        thisLine = lame.StandardError.ReadLine();
                        if (!string.IsNullOrEmpty(thisLine))
                        {
                            if (thisLine.StartsWith("Frame#"))
                            {
                                string thisFrame = thisLine.Remove(thisLine.IndexOf('/'));
                                thisFrame = thisFrame.Remove(0, thisFrame.LastIndexOf(' ') + 1);
                                string Frames = thisLine.Remove(0, thisLine.IndexOf('/') + 1);
                                Frames = Frames.Remove(Frames.IndexOf(' '));

                                int thisProgress = (int)((Convert.ToDouble(thisFrame) / Convert.ToDouble(Frames)) * 100);
                                bwConvertToBns.ReportProgress(thisProgress);
                            }
                        }
                    }

                    lame.WaitForExit();
                    lame.Close();

                    if (File.Exists(Application.StartupPath + Path.DirectorySeparatorChar + "customizemii_temp.wav"))
                    {
                        audioData = File.ReadAllBytes(Application.StartupPath + Path.DirectorySeparatorChar + "customizemii_temp.wav");
                        File.Delete(Application.StartupPath + Path.DirectorySeparatorChar + "customizemii_temp.wav");
                    }
                    else throw new Exception("Error converting MP3...");
                }

                bwConvertToBns.ReportProgress(0, "Converting to BNS...");

                BNS bns = new BNS(audioData, bnsInfo.loopType == BnsConversionInfo.LoopType.FromWave);
                bns.Progress += new EventHandler<ProgressChangedEventArgs>(bns_ProgressChanged);

                bns.StereoToMono = bnsInfo.stereoToMono;
                bns.Convert();

                if (bnsInfo.loopType == BnsConversionInfo.LoopType.Manual && bnsInfo.loopStartSample > -1 && bnsInfo.loopStartSample < bns.TotalSampleCount)
                    bns.SetLoop(bnsInfo.loopStartSample);

                newSoundBin = Headers.IMD5.AddHeader(bns.ToByteArray());
                replacedSound = bnsInfo.audioFile;
            }
            catch (Exception ex)
            {
                replacedSound = string.Empty;
                if (File.Exists(Application.StartupPath + Path.DirectorySeparatorChar + "customizemii_temp.wav")) File.Delete(Application.StartupPath + Path.DirectorySeparatorChar + "customizemii_temp.wav");
                errorBox("Error during conversion:\n" + ex.Message);
            }
        }

        void bwConvertMp3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!string.IsNullOrEmpty(replacedSound))
            {
                setControlText(tbSound, replacedSound);
                btnBrowseSound.Text = "Clear";

                if (cmbReplace.SelectedIndex == 2) setControlText(tbReplace, string.Empty);
            }

            currentProgress.progressValue = 100;
            currentProgress.progressState = " ";

            this.Invoke(ProgressUpdate);
        }

        void bwConvertMp3_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            currentProgress.progressValue = e.ProgressPercentage;
            currentProgress.progressState = (string)e.UserState;

            this.Invoke(ProgressUpdate);
        }

        void bwConvertMp3_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker bwConvertMp3 = sender as BackgroundWorker;

                ProcessStartInfo lameI = new ProcessStartInfo(Application.StartupPath + Path.DirectorySeparatorChar + "lame.exe",
                    string.Format("--decode \"{0}\" \"{1}\"", e.Argument, Application.StartupPath + Path.DirectorySeparatorChar + "customizemii_temp.wav"));
                lameI.CreateNoWindow = true;
                lameI.UseShellExecute = false;
                lameI.RedirectStandardError = true;

                Process lame = Process.Start(lameI);

                string thisLine = string.Empty;
                while (lame.HasExited == false)
                {
                    thisLine = lame.StandardError.ReadLine();
                    if (!string.IsNullOrEmpty(thisLine))
                    {
                        if (thisLine.StartsWith("Frame#"))
                        {
                            string thisFrame = thisLine.Remove(thisLine.IndexOf('/'));
                            thisFrame = thisFrame.Remove(0, thisFrame.LastIndexOf(' ') + 1);

                            string Frames = thisLine.Remove(0, thisLine.IndexOf('/') + 1);
                            Frames = Frames.Remove(Frames.IndexOf(' '));

                            int thisProgress = (int)((Convert.ToDouble(thisFrame) / Convert.ToDouble(Frames)) * 100);
                            bwConvertMp3.ReportProgress(thisProgress, "Converting MP3...");
                        }
                    }
                }

                lame.WaitForExit();
                lame.Close();

                if (File.Exists(Application.StartupPath + Path.DirectorySeparatorChar + "customizemii_temp.wav"))
                {
                    newSoundBin = Headers.IMD5.AddHeader(File.ReadAllBytes(Application.StartupPath + Path.DirectorySeparatorChar + "customizemii_temp.wav"));
                    File.Delete(Application.StartupPath + Path.DirectorySeparatorChar + "customizemii_temp.wav");

                    replacedSound = (string)e.Argument;
                }
            }
            catch (Exception ex)
            {
                if (File.Exists(Application.StartupPath + Path.DirectorySeparatorChar + "customizemii_temp.wav")) File.Delete(Application.StartupPath + Path.DirectorySeparatorChar + "customizemii_temp.wav");
                replacedSound = string.Empty;
                errorBox("Error during conversion:\n" + ex.Message);
            }
        }

        void bwLoadChannel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            currentProgress.progressValue = 100;
            currentProgress.progressState = " ";

            this.Invoke(ProgressUpdate);
        }

        void bwLoadChannel_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            currentProgress.progressValue = e.ProgressPercentage;
            currentProgress.progressState = (string)e.UserState;

            this.Invoke(ProgressUpdate);
        }

        void bwLoadChannel_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker bwLoadChannel = sender as BackgroundWorker;
                initialize();

                if (e.Argument is string) setControlText(tbSourceWad, (string)e.Argument);
                else if (e.Argument is DownloadDataCompletedEventArgs) setControlText(tbSourceWad, (string)((DownloadDataCompletedEventArgs)e.Argument).UserState);

                bwLoadChannel.ReportProgress(0, "Loading WAD...");

                if (e.Argument is string) sourceWad.LoadFile((string)e.Argument);
                else if (e.Argument is DownloadDataCompletedEventArgs) sourceWad.LoadFile(((DownloadDataCompletedEventArgs)e.Argument).Result);

                if (!sourceWad.HasBanner)
                    throw new Exception("CustomizeMii only edits Channel WADs!");

                int progressValue = 30;
                for (int i = 0; i < sourceWad.BannerApp.NumOfNodes; i++)
                {
                    if (sourceWad.BannerApp.StringTable[i].ToLower() == "banner.bin")
                    {
                        progressValue += 20;
                        bwLoadChannel.ReportProgress(progressValue, "Loading banner.bin...");
                        bannerBin.LoadFile(sourceWad.BannerApp.Data[i]);
                    }
                    else if (sourceWad.BannerApp.StringTable[i].ToLower() == "icon.bin")
                    {
                        progressValue += 20;
                        bwLoadChannel.ReportProgress(progressValue, "Loading icon.bin...");
                        iconBin.LoadFile(sourceWad.BannerApp.Data[i]);
                    }
                }

                bwLoadChannel.ReportProgress(90, "Loading Channel Information...");
                setControlText(tbTitleID, sourceWad.UpperTitleID);
                setControlText(tbStartupIos, ((int)sourceWad.StartupIOS).ToString());

                string[] channelTitles = ((Headers.IMET)sourceWad.BannerApp.Header).GetTitles();
                bool allLangs = true;

                if (channelTitles[0] != channelTitles[1]) setControlText(tbJapanese, channelTitles[0]);
                else allLangs = false;
                if (channelTitles[2] != channelTitles[1]) setControlText(tbGerman, channelTitles[2]);
                else allLangs = false;
                if (channelTitles[3] != channelTitles[1]) setControlText(tbFrench, channelTitles[3]);
                else allLangs = false;
                if (channelTitles[4] != channelTitles[1]) setControlText(tbSpanish, channelTitles[4]);
                else allLangs = false;
                if (channelTitles[5] != channelTitles[1]) setControlText(tbItalian, channelTitles[5]);
                else allLangs = false;
                if (channelTitles[6] != channelTitles[1]) setControlText(tbDutch, channelTitles[6]);
                else allLangs = false;
                if (channelTitles[7] != channelTitles[1]) setControlText(tbKorean, channelTitles[7]);

                if (allLangs) setControlText(tbEnglish, channelTitles[1]);
                else setControlText(tbAllLanguages, channelTitles[1]);

                bwLoadChannel.ReportProgress(95, "Loading Footer...");
                if (sourceWad.CreationTimeUTC > new DateTime(1970, 1, 1))
                    setControlText(lbCreatedValue, sourceWad.CreationTimeUTC.ToString() + " (UTC)");
                else setControlText(lbCreatedValue, "No Timestamp!");

                addTpls();
                addBrlyts();
                addBrlans();

                enableControls();

                setControlText(btnBrowseSource, "Clear");
            }
            catch (Exception ex)
            {
                setControlText(tbSourceWad, string.Empty);
                errorBox(ex.Message);
            }
        }

        void bwTransmit_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            currentProgress.progressValue = 100;
            currentProgress.progressState = " ";

            this.Invoke(ProgressUpdate);
            enableControls();

            if (transmitInfo.timeElapsed > 0)
            {
                DialogResult dlg;

                if (transmitInfo.compressionRatio > 0)
                    dlg = MessageBox.Show(
                        string.Format("Transmitted {0} kB in {1} milliseconds...\nCompression Ratio: {2}%\n\nDo you want to save the wad file?",
                        transmitInfo.transmittedLength, transmitInfo.timeElapsed, transmitInfo.compressionRatio),
                        "Save File?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                else
                    dlg = MessageBox.Show(
                        string.Format("Transmitted {0} kB in {1} milliseconds...\n\nDo you want to save the wad file?",
                        transmitInfo.transmittedLength, transmitInfo.timeElapsed),
                        "Save File?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dlg == DialogResult.Yes)
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "Wii Channels|*.wad";

                    if (!string.IsNullOrEmpty(tbAllLanguages.Text))
                        sfd.FileName = tbAllLanguages.Text + " - " + tbTitleID.Text.ToUpper() + ".wad";
                    else
                        sfd.FileName = tbEnglish.Text + " - " + tbTitleID.Text.ToUpper() + ".wad";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        if (File.Exists(sfd.FileName)) File.Delete(sfd.FileName);
                        File.WriteAllBytes(sfd.FileName, wadCreationInfo.wadFile);
                    }
                }

                initialize();
            }
        }

        void bwTransmit_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            currentProgress.progressValue = e.ProgressPercentage;
            currentProgress.progressState = (string)e.UserState;

            this.Invoke(ProgressUpdate);
        }

        void bwTransmit_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker bwTransmit = sender as BackgroundWorker;

                //Insert wad into stub dol
                string fileName = "CMiiInstaller.dol";
                byte[] fileData = CustomizeMiiInstaller.InstallerHelper.CreateInstaller(wadCreationInfo.wadFile, (byte)wadCreationInfo.transmitIos).ToArray();

                //Transmit installer
                Stopwatch transmitTimer = new Stopwatch();

                HbcTransmitter transmitter = new HbcTransmitter(wadCreationInfo.transmitProtocol, wadCreationInfo.transmitIp);
                transmitter.Progress += new EventHandler<ProgressChangedEventArgs>(transmitter_Progress);

                transmitter_Progress(null, new ProgressChangedEventArgs(0, null));
                transmitTimer.Start();

                bool success = transmitter.TransmitFile(fileName, fileData);

                transmitTimer.Stop();

                if (!success) errorBox(transmitter.LastError);
                else
                {
                    transmitInfo.timeElapsed = (int)transmitTimer.ElapsedMilliseconds;
                    transmitInfo.compressionRatio = transmitter.CompressionRatio;
                    transmitInfo.transmittedLength = Math.Round(transmitter.TransmittedLength * 0.0009765625, 2);
                }
            }
            catch (Exception ex) { errorBox(ex.Message); }
        }

        void transmitter_Progress(object sender, ProgressChangedEventArgs e)
        {
            currentProgress.progressValue = e.ProgressPercentage;
            currentProgress.progressState = "Sending File...";

            this.Invoke(ProgressUpdate);
        }

        void bwCreateWad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!wadCreationInfo.sendToWii)
            {
                currentProgress.progressValue = 100;
                currentProgress.progressState = " ";

                this.Invoke(ProgressUpdate);
                enableControls();

                if (wadCreationInfo.success)
                    initialize();
            }
            else
            {
                if (wadCreationInfo.sendWadReady == true)
                {
                    //Start new BackgroundWorker to Transmit
                    BackgroundWorker bwTransmit = new BackgroundWorker();
                    bwTransmit.WorkerReportsProgress = true;
                    bwTransmit.DoWork += new DoWorkEventHandler(bwTransmit_DoWork);
                    bwTransmit.ProgressChanged += new ProgressChangedEventHandler(bwTransmit_ProgressChanged);
                    bwTransmit.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwTransmit_RunWorkerCompleted);
                    bwTransmit.RunWorkerAsync();
                }
                else
                {
                    currentProgress.progressValue = 100;
                    currentProgress.progressState = " ";

                    this.Invoke(ProgressUpdate);
                    enableControls();
                }
            }
        }

        void bwCreateWad_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            currentProgress.progressValue = e.ProgressPercentage;
            currentProgress.progressState = (string)e.UserState;

            this.Invoke(ProgressUpdate);
        }

        void bwCreateWad_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bwCreateWad = sender as BackgroundWorker;
            Stopwatch creationTimer = new Stopwatch();
            creationTimer.Start();

            try
            {
                WadCreationInfo wadInfo = (WadCreationInfo)e.Argument;


                disableControls();

                //Check Startup IOS
                if (wadInfo.startupIos < 0 || wadInfo.startupIos > 255)
                    throw new Exception("Startup IOS must be between 0 and 255!");

                wadInfo.success = false;
                wadInfo.sendWadReady = false;
                wadCreationInfo = wadInfo;

                //Make TPLs transparent
                makeBannerTplsTransparent();
                makeIconTplsTransparent();

                //Pack icon.bin
                bwCreateWad.ReportProgress(0, "Packing icon.bin...");
                if (string.IsNullOrEmpty(replacedIcon))
                {
                    iconBin.AddHeaderImd5();
                    iconBin.Lz77Compress = wadInfo.lz77;

                    for (int i = 0; i < sourceWad.BannerApp.NumOfNodes; i++)
                        if (sourceWad.BannerApp.StringTable[i].ToLower() == "icon.bin")
                        { sourceWad.BannerApp.ReplaceFile(i, iconBin.ToByteArray()); break; }
                }
                else
                {
                    newIconBin.AddHeaderImd5();
                    newIconBin.Lz77Compress = wadInfo.lz77;

                    for (int i = 0; i < sourceWad.BannerApp.NumOfNodes; i++)
                        if (sourceWad.BannerApp.StringTable[i].ToLower() == "icon.bin")
                        { sourceWad.BannerApp.ReplaceFile(i, newIconBin.ToByteArray()); break; }
                }

                //Pack banner.bin
                bwCreateWad.ReportProgress(20, "Packing banner.bin...");
                if (string.IsNullOrEmpty(replacedBanner))
                {
                    bannerBin.AddHeaderImd5();
                    bannerBin.Lz77Compress = wadInfo.lz77;

                    for (int i = 0; i < sourceWad.BannerApp.NumOfNodes; i++)
                        if (sourceWad.BannerApp.StringTable[i].ToLower() == "banner.bin")
                        { sourceWad.BannerApp.ReplaceFile(i, bannerBin.ToByteArray()); break; }
                }
                else
                {
                    newBannerBin.AddHeaderImd5();
                    bannerBin.Lz77Compress = wadInfo.lz77;

                    for (int i = 0; i < sourceWad.BannerApp.NumOfNodes; i++)
                        if (sourceWad.BannerApp.StringTable[i].ToLower() == "banner.bin")
                        { sourceWad.BannerApp.ReplaceFile(i, newBannerBin.ToByteArray()); break; }
                }

                //Pack sound.bin
                bwCreateWad.ReportProgress(40, "Packing sound.bin...");
                if (!string.IsNullOrEmpty(replacedSound) || !string.IsNullOrEmpty(wadInfo.sound))
                {
                    for (int i = 0; i < sourceWad.BannerApp.NumOfNodes; i++)
                        if (sourceWad.BannerApp.StringTable[i].ToLower() == "sound.bin")
                        { sourceWad.BannerApp.ReplaceFile(i, newSoundBin); break; }
                }

                //Insert new dol
                if (!string.IsNullOrEmpty(wadInfo.dol))
                {
                    bwCreateWad.ReportProgress(50, "Inserting DOL...");
                    sourceWad.RemoveAllContents();

                    if (wadInfo.nandLoader == WadCreationInfo.NandLoader.comex)
                    {
                        sourceWad.AddContent(Properties.Resources.comex, 1, 1, ContentType.Normal);

                        if (wadInfo.dol.StartsWith("Simple Forwarder:"))
                            sourceWad.AddContent(createForwarderSimple(), 2, 2, ContentType.Normal);
                        else if (wadInfo.dol.StartsWith("Complex Forwarder"))
                        {
                            bwCreateWad.ReportProgress(55, "Compiling Forwarder...");
                            sourceWad.AddContent(createForwarderComplex(), 2, 2, ContentType.Normal);
                        }
                        else
                            sourceWad.AddContent(newDol, 2, 2, ContentType.Normal);

                        sourceWad.BootIndex = 1;
                    }
                    else
                    {
                        sourceWad.AddContent(Properties.Resources.Waninkoko, 2, 2, ContentType.Normal);

                        if (wadInfo.dol.StartsWith("Simple Forwarder:"))
                            sourceWad.AddContent(createForwarderSimple(), 1, 1, ContentType.Normal);
                        else if (wadInfo.dol.StartsWith("Complex Forwarder"))
                        {
                            bwCreateWad.ReportProgress(55, "Compiling Forwarder...");
                            sourceWad.AddContent(createForwarderComplex(), 1, 1, ContentType.Normal);
                        }
                        else
                            sourceWad.AddContent(newDol, 1, 1, ContentType.Normal);

                        sourceWad.BootIndex = 2;
                    }
                }

                //Change channel information
                for (int i = 0; i < wadInfo.titles.Length; i++)
                    if (string.IsNullOrEmpty(wadInfo.titles[i]))
                        wadInfo.titles[i] = wadInfo.allLangTitle;

                bwCreateWad.ReportProgress(75, "Updating Channel Information...");
                sourceWad.ChangeStartupIOS(wadInfo.startupIos);
                sourceWad.ChangeChannelTitles(wadInfo.titles);
                if (!string.IsNullOrEmpty(wadInfo.titleId))
                    sourceWad.ChangeTitleID(LowerTitleID.Channel, wadInfo.titleId);

                sourceWad.FakeSign = true;
                sourceWad.ChangeTitleKey("GottaGetSomeBeer");

                //Pack WAD
                bwCreateWad.ReportProgress(80, "Packing WAD...");
                if (!wadInfo.sendToWii) sourceWad.Save(wadInfo.outFile);
                else wadInfo.wadFile = sourceWad.ToByteArray();

                bwCreateWad.ReportProgress(100, " ");
                creationTimer.Stop();

                if (!wadInfo.sendToWii)
                {
                    FileInfo fi = new FileInfo(wadInfo.outFile);
                    double fileSize = Math.Round(fi.Length * 0.0009765625 * 0.0009765625, 2);

                    infoBox(string.Format("Successfully created custom channel!\nTime elapsed: {0} ms\nFilesize: {1} MB\nApprox. Blocks: {2}", creationTimer.ElapsedMilliseconds, fileSize, sourceWad.NandBlocks));
                }
                else wadInfo.sendWadReady = true;

                wadCreationInfo = wadInfo;
                wadCreationInfo.success = true;
            }
            catch (Exception ex)
            {
                wadCreationInfo.sendWadReady = false;
                creationTimer.Stop();

                enableControls();
                errorBox(ex.Message);
            }
        }
    }
}
