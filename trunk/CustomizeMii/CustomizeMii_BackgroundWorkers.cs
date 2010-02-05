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
using BNS;
using System.Net.Sockets;

namespace CustomizeMii
{
    partial class CustomizeMii_Main
    {
        private Stopwatch CreationTimer = new Stopwatch();
        private Stopwatch TransmitTimer = new Stopwatch();
        private TransmitInfo transmitInfo;
        public int sendWadReady = 0;
        private bool sendToWii = false;
        private bool internalSound;
        private WadCreationInfo wadCreationInfo;

        void bwBannerReplace_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            currentProgress.progressValue = 100;
            currentProgress.progressState = " ";

            this.Invoke(ProgressUpdate);
            SetText(tbReplace, BannerReplace);
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
                EventHandler AddBrlyts = new EventHandler(this.AddBrlyts);
                EventHandler AddBrlans = new EventHandler(this.AddBrlans);
                this.Invoke(AddBannerTpls);
                this.Invoke(AddIconTpls);
                this.Invoke(AddBrlyts);
                this.Invoke(AddBrlans);
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
            currentProgress.progressValue = 100;
            currentProgress.progressState = " ";

            this.Invoke(ProgressUpdate);
            SetText(tbReplace, IconReplace);
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
                EventHandler AddBrlyts = new EventHandler(this.AddBrlyts);
                EventHandler AddBrlans = new EventHandler(this.AddBrlans);
                this.Invoke(AddBannerTpls);
                this.Invoke(AddIconTpls);
                this.Invoke(AddBrlyts);
                this.Invoke(AddBrlans);
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
            currentProgress.progressValue = 100;
            currentProgress.progressState = " ";

            this.Invoke(ProgressUpdate);
            SetText(tbReplace, SoundReplace);
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

        void bwConvertToBns_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (File.Exists(TempBnsPath))
            {
                if (internalSound == true)
                    SetText(tbSound, "BNS: Internal Sound");
                else
                    SetText(tbSound, "BNS: " + Mp3Path);

                btnBrowseSound.Text = "Clear";

                if (!string.IsNullOrEmpty(SoundReplace))
                {
                    SoundReplace = string.Empty;
                    if (cmbReplace.SelectedIndex == 2) SetText(tbReplace, SoundReplace);
                    if (File.Exists(TempSoundPath)) File.Delete(TempSoundPath);
                }
            }

            if (File.Exists(TempWavePath)) File.Delete(TempWavePath);

            Mp3Path = string.Empty;
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
                string audioFile;

                if (bnsInfo.AudioFile == "Internal Sound")
                { audioFile = TempWavePath; internalSound = true; }
                else
                    audioFile = bnsInfo.AudioFile;
                Mp3Path = audioFile; //Rather audioPath...

                bool mp3 = audioFile.EndsWith(".mp3");
                if (mp3 && bnsInfo.Loop == BnsConversionInfo.LoopType.FromWave) bnsInfo.Loop = BnsConversionInfo.LoopType.None;

                if (mp3)
                {
                    bwConvertToBns.ReportProgress(0, "Converting MP3...");

                    ProcessStartInfo lameI = new ProcessStartInfo(System.Windows.Forms.Application.StartupPath + "\\lame.exe",
                        string.Format("--decode \"{0}\" \"{1}\"", audioFile, "C:\\cmtempmp3wav.wav")); //Gotta go this step, cause the TempWavePath is too long for lame.exe
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

                    if (File.Exists("C:\\cmtempmp3wav.wav"))
                    {
                        FileInfo fi = new FileInfo("C:\\cmtempmp3wav.wav");
                        fi.MoveTo(TempWavePath);
                    }

                    if (!File.Exists(TempWavePath)) throw new Exception("Error converting MP3...");
                    else audioFile = TempWavePath;
                }

                bwConvertToBns.ReportProgress(0, "Converting To BNS...");
                
                BNS_File bns = new BNS_File(audioFile, bnsInfo.Loop == BnsConversionInfo.LoopType.FromWave);
                bns.ProgressChanged += new EventHandler<ProgressChangedEventArgs>(bns_ProgressChanged);

                bns.StereoToMono = bnsInfo.StereoToMono;
                bns.Convert();

                if (bnsInfo.Loop == BnsConversionInfo.LoopType.Manual && bnsInfo.LoopStartSample > -1 && bnsInfo.LoopStartSample < bns.TotalSampleCount)
                    bns.SetLoop(bnsInfo.LoopStartSample);

                bns.Save(TempBnsPath);

                if (File.Exists(TempWavePath))
                    File.Delete(TempWavePath);
            }
            catch (Exception ex)
            {
                ErrorBox("Error during conversion:\n" + ex.Message);
            }
        }

        void bwConvertMp3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (File.Exists(TempWavePath))
            {
                SetText(tbSound, Mp3Path);

                btnBrowseSound.Text = "Clear";

                if (!string.IsNullOrEmpty(SoundReplace))
                {
                    SoundReplace = string.Empty;
                    if (cmbReplace.SelectedIndex == 2) SetText(tbReplace, SoundReplace);
                    if (File.Exists(TempSoundPath)) File.Delete(TempSoundPath);
                }
            }

            Mp3Path = string.Empty;
            currentProgress.progressValue = 100;
            currentProgress.progressState = " ";

            this.Invoke(ProgressUpdate);
        }

        void bwConvertMp3_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            currentProgress.progressValue = e.ProgressPercentage;

            this.Invoke(ProgressUpdate);
        }

        void bwConvertMp3_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker bwConvertMp3 = sender as BackgroundWorker;
                Mp3Path = (string)e.Argument;
                ProcessStartInfo lameI = new ProcessStartInfo(System.Windows.Forms.Application.StartupPath + "\\lame.exe",
                    string.Format("--decode \"{0}\" \"{1}\"", e.Argument, "C:\\cmtempmp3wav.wav")); //Gotta go this step, cause the TempWavePath is too long for lame.exe
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
                            bwConvertMp3.ReportProgress(thisProgress);
                        }
                    }
                }

                lame.WaitForExit();
                lame.Close();

                if (File.Exists("C:\\cmtempmp3wav.wav"))
                {
                    FileInfo fi = new FileInfo("C:\\cmtempmp3wav.wav");
                    fi.MoveTo(TempWavePath);
                }
            }
            catch (Exception ex)
            {
                if (File.Exists(TempWavePath)) File.Delete(TempWavePath);
                Mp3Path = string.Empty;
                ErrorBox("Error during conversion:\n" + ex.Message);
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
                EventHandler Initialize = new EventHandler(this.Initialize);
                EventHandler SetSourceWad = new EventHandler(this.SetSourceWad);
                byte[] WadFile = Wii.Tools.LoadFileToByteArray((string)e.Argument);
                bool hashesMatch = true;

                this.Invoke(Initialize);

                if (Directory.Exists(TempUnpackPath)) Directory.Delete(TempUnpackPath, true);

                bwLoadChannel.ReportProgress(0, "Loading WAD...");
                Wii.WadUnpack.UnpackWad(WadFile, TempUnpackPath, out hashesMatch);
                if (Wii.U8.CheckU8(TempUnpackPath + "00000000.app") == false)
                    throw new Exception("CustomizeMii only edits Channel WADs!");

                SourceWad = (string)e.Argument;
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

                bool allLangs = true;
                SetText(tbTitleID, TitleID);

                if (ChannelTitles[0] != ChannelTitles[1]) SetText(tbJapanese, ChannelTitles[0]);
                else allLangs = false;
                if (ChannelTitles[2] != ChannelTitles[1]) SetText(tbGerman, ChannelTitles[2]);
                else allLangs = false;
                if (ChannelTitles[3] != ChannelTitles[1]) SetText(tbFrench, ChannelTitles[3]);
                else allLangs = false;
                if (ChannelTitles[4] != ChannelTitles[1]) SetText(tbSpanish, ChannelTitles[4]);
                else allLangs = false;
                if (ChannelTitles[5] != ChannelTitles[1]) SetText(tbItalian, ChannelTitles[5]);
                else allLangs = false;
                if (ChannelTitles[6] != ChannelTitles[1]) SetText(tbDutch, ChannelTitles[6]);
                else allLangs = false;

                if (allLangs) SetText(tbEnglish, ChannelTitles[1]);
                else SetText(tbAllLanguages, ChannelTitles[1]);

                string[] trailer = Directory.GetFiles(TempUnpackPath, "*.trailer");
                if (trailer.Length > 0)
                {
                    DateTime timestamp = Wii.WadInfo.GetCreationTime(trailer[0]);

                    if (timestamp > new DateTime(1970, 1, 1))
                    {
                        SetLabel(lbCreatedValue, timestamp.ToString() + " (UTC)");
                    }
                    else SetLabel(lbCreatedValue, "No Timestamp!");
                }
                else SetLabel(lbCreatedValue, "No Timestamp!");

                EventHandler AddBannerTpls = new EventHandler(this.AddBannerTpls);
                EventHandler AddIconTpls = new EventHandler(this.AddIconTpls);
                EventHandler AddBrlyts = new EventHandler(this.AddBrlyts);
                EventHandler AddBrlans = new EventHandler(this.AddBrlans);
                this.Invoke(AddBannerTpls);
                this.Invoke(AddIconTpls);
                this.Invoke(AddBrlyts);
                this.Invoke(AddBrlans);

                bwLoadChannel.ReportProgress(100);
                EventHandler EnableCtrls = new EventHandler(this.EnableControls);
                this.Invoke(EnableCtrls);

                SetButton(btnBrowseSource, "Clear");

                if (!hashesMatch)
                    System.Windows.Forms.MessageBox.Show("At least one content's hash doesn't match the hash in the TMD!\n" +
                        "Some files of the WAD might be corrupted, thus it might brick your Wii!", "Warning",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                if (Directory.Exists(TempUnpackPath)) Directory.Delete(TempUnpackPath, true);
                SourceWad = string.Empty;
                SetText(tbSourceWad, string.Empty);
                ErrorBox(ex.Message);
            }
        }

        void bwTransmit_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            EventHandler EnableControls = new EventHandler(this.EnableControls);
            EventHandler Initialize = new EventHandler(this.Initialize);
            currentProgress.progressValue = 100;
            currentProgress.progressState = " ";

            this.Invoke(ProgressUpdate);
            this.Invoke(EnableControls);

            if (transmitInfo.timeElapsed > 0)
            {
                System.Windows.Forms.DialogResult dlg;

                if (transmitInfo.usedCompression)
                    dlg = System.Windows.Forms.MessageBox.Show(
                        string.Format("Transmitted {0} kB in {1} milliseconds...\nCompression Ratio: {2}%\n\nDo you want to save the wad file?",
                        transmitInfo.transmittedLength, transmitInfo.timeElapsed, transmitInfo.compressionRatio),
                        "Save File?", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);
                else
                    dlg = System.Windows.Forms.MessageBox.Show(
                        string.Format("Transmitted {0} kB in {1} milliseconds...\n\nDo you want to save the wad file?",
                        transmitInfo.transmittedLength, transmitInfo.timeElapsed),
                        "Save File?", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

                if (dlg == System.Windows.Forms.DialogResult.Yes)
                {
                    System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
                    sfd.Filter = "Wii Channels|*.wad";

                    if (!string.IsNullOrEmpty(tbAllLanguages.Text))
                        sfd.FileName = tbAllLanguages.Text + " - " + tbTitleID.Text.ToUpper() + ".wad";
                    else
                        sfd.FileName = tbEnglish.Text + " - " + tbTitleID.Text.ToUpper() + ".wad";

                    if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        File.Copy(TempPath + "SendToWii.wad", sfd.FileName, true);
                }

                this.Invoke(Initialize);
            }

            try { File.Delete(TempPath + "SendToWii.wad"); }
            catch { }
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
                byte[] fileData = CustomizeMiiInstaller.InstallerHelper.CreateInstaller(TempPath + "SendToWii.wad", (byte)wadCreationInfo.transmitIos).ToArray();

                //Transmit installer
                transmitInfo.timeElapsed = 0; TransmitTimer.Reset(); TransmitTimer.Start();
                bool compress = File.Exists(System.Windows.Forms.Application.StartupPath + "\\zlib1.dll");

                if (!Environment.OSVersion.ToString().Contains("Windows"))
                    compress = false;

                if ((int)(wadCreationInfo.transmitProtocol) == 1) compress = false;

                TcpClient theClient = new TcpClient();

                byte[] compFileData;
                int Blocksize = 4 * 1024;
                if (wadCreationInfo.transmitProtocol != TransmitProtocol.JODI) Blocksize = 16 * 1024;
                byte[] buffer = new byte[4];
                string theIP = wadCreationInfo.transmitIp;

                bwTransmit.ReportProgress(0, "Connecting...");
                //StatusUpdate("Connecting...");
                try { theClient.Connect(theIP, 4299); }
                catch (Exception ex) { theClient.Close(); throw new Exception("Connection Failed:\n" + ex.Message); }
                NetworkStream theStream = theClient.GetStream();

                bwTransmit.ReportProgress(0, "Connected... Sending Magic...");
                //StatusUpdate("Connected... Sending Magic...");
                buffer[0] = (byte)'H';
                buffer[1] = (byte)'A';
                buffer[2] = (byte)'X';
                buffer[3] = (byte)'X';
                try { theStream.Write(buffer, 0, 4); }
                catch (Exception ex) { theStream.Close(); theClient.Close(); throw new Exception("Error sending Magic:\n" + ex.Message); }

                bwTransmit.ReportProgress(0, "Magic Sent... Sending Version Info...");
                //StatusUpdate("Magic Sent... Sending Version Info...");
                buffer[0] = 0;
                buffer[1] = wadCreationInfo.transmitProtocol == TransmitProtocol.JODI ? (byte)5 : (byte)4;
                buffer[2] = (byte)(((fileName.Length + 2) >> 8) & 0xff);
                buffer[3] = (byte)((fileName.Length + 2) & 0xff);

                try { theStream.Write(buffer, 0, 4); }
                catch (Exception ex) { theStream.Close(); theClient.Close(); throw new Exception("Error sending Version Info:\n" + ex.Message); }

                if (compress)
                {
                    bwTransmit.ReportProgress(0, "Version Info Sent... Compressing File...");
                    //StatusUpdate("Version Info Sent... Compressing File...");
                    try { compFileData = TransmitMii.zlib.Compress(fileData); }
                    catch (Exception ex)
                    {
                        ErrorBox(ex.Message);
                        //Compression failed, let's continue without compression
                        compFileData = fileData;
                        fileData = new byte[0];
                    }

                    bwTransmit.ReportProgress(0, "Compressed File... Sending Filesize...");
                    //StatusUpdate("Compressed File... Sending Filesize...");
                }
                else
                {
                    compFileData = fileData;
                    fileData = new byte[0];

                    bwTransmit.ReportProgress(0, "Version Info Sent... Sending Filesize...");
                    //StatusUpdate("Version Info Sent... Sending Filesize...");
                }

                //First compressed filesize, then uncompressed filesize
                buffer[0] = (byte)((compFileData.Length >> 24) & 0xff);
                buffer[1] = (byte)((compFileData.Length >> 16) & 0xff);
                buffer[2] = (byte)((compFileData.Length >> 8) & 0xff);
                buffer[3] = (byte)(compFileData.Length & 0xff);
                try { theStream.Write(buffer, 0, 4); }
                catch (Exception ex) { theStream.Close(); theClient.Close(); throw new Exception("Error sending Filesize:\n" + ex.Message); }

                if (wadCreationInfo.transmitProtocol != TransmitProtocol.HAXX)
                {
                    buffer[0] = (byte)((fileData.Length >> 24) & 0xff);
                    buffer[1] = (byte)((fileData.Length >> 16) & 0xff);
                    buffer[2] = (byte)((fileData.Length >> 8) & 0xff);
                    buffer[3] = (byte)(fileData.Length & 0xff);
                    try { theStream.Write(buffer, 0, 4); }
                    catch (Exception ex) { theStream.Close(); theClient.Close(); throw new Exception("Error sending Filesize:\n" + ex.Message); }
                }

                bwTransmit.ReportProgress(0, "Filesize Sent... Sending File...");
                //StatusUpdate("Filesize Sent... Sending File...");
                int off = 0;
                int cur = 0;
                int count = compFileData.Length / Blocksize;
                int left = compFileData.Length % Blocksize;

                try
                {
                    do
                    {
                        bwTransmit.ReportProgress((cur + 1) * 100 / count, "Sending File...");
                        //StatusUpdate("Sending File: " + ((cur + 1) * 100 / count).ToString() + "%");
                        theStream.Write(compFileData, off, Blocksize);
                        off += Blocksize;
                        cur++;
                    } while (cur < count);

                    if (left > 0)
                    {
                        theStream.Write(compFileData, off, compFileData.Length - off);
                    }
                }
                catch (Exception ex) { theStream.Close(); theClient.Close(); throw new Exception("Error sending File:\n" + ex.Message); }

                bwTransmit.ReportProgress(0, "File Sent... Sending Arguments...");
                //StatusUpdate("File Sent... Sending Arguments...");
                byte[] theArgs = new byte[fileName.Length + 2];
                for (int i = 0; i < fileName.Length; i++) { theArgs[i] = (byte)fileName.ToCharArray()[i]; }
                try { theStream.Write(theArgs, 0, theArgs.Length); }
                catch (Exception ex) { theStream.Close(); theClient.Close(); throw new Exception("Error sending Arguments:\n" + ex.Message); }

                theStream.Close();
                theClient.Close();

                bwTransmit.ReportProgress(0, string.Empty);
                //StatusUpdate(string.Empty);

                TransmitTimer.Stop();
                transmitInfo.timeElapsed = (int)TransmitTimer.ElapsedMilliseconds;
                transmitInfo.usedCompression = compress;
                transmitInfo.transmittedLength = Math.Round(compFileData.Length * 0.0009765625, 2);
                if (compress && fileData.Length != 0)
                    transmitInfo.compressionRatio = (compFileData.Length * 100) / fileData.Length;
            }
            catch (Exception ex)
            {
                ErrorBox(ex.Message);
            }
        }

        void bwCreateWad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!this.sendToWii)
            {
                EventHandler EnableControls = new EventHandler(this.EnableControls);
                currentProgress.progressValue = 100;
                currentProgress.progressState = " ";

                this.Invoke(ProgressUpdate);
                this.Invoke(EnableControls);

                if (wadCreationInfo.success)
                {
                    EventHandler Initialize = new EventHandler(this.Initialize);
                    this.Invoke(Initialize);
                }
            }
            else
            {
                if (sendWadReady == 1)
                {
                    //Start new BackgroundWorker to Transmit
                    BackgroundWorker bwTransmit = new BackgroundWorker();
                    bwTransmit.WorkerReportsProgress = true;
                    bwTransmit.DoWork += new DoWorkEventHandler(bwTransmit_DoWork);
                    bwTransmit.ProgressChanged += new ProgressChangedEventHandler(bwTransmit_ProgressChanged);
                    bwTransmit.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwTransmit_RunWorkerCompleted);
                    bwTransmit.RunWorkerAsync();
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
            try
            {
                BackgroundWorker bwCreateWad = sender as BackgroundWorker;
                WadCreationInfo wadInfo = (WadCreationInfo)e.Argument;
                EventHandler DisableControls = new EventHandler(this.DisableControls);
                this.Invoke(DisableControls);

                this.sendToWii = wadInfo.sendToWii;
                sendWadReady = 0;

                wadCreationInfo = wadInfo;
                wadCreationInfo.success = false;

                bwCreateWad.ReportProgress(0, "Making TPLs transparent");
                MakeBannerTplsTransparent();
                MakeIconTplsTransparent();

                bwCreateWad.ReportProgress(3, "Fixing TPL Filters...");
                FixTpls();

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
                        if (tbSound.Text.EndsWith(".bns"))
                        {
                            Wii.Sound.BnsToSoundBin(tbSound.Text, TempUnpackPath + "00000000.app_OUT\\meta\\sound.bin", false);
                        }
                        else if (tbSound.Text.StartsWith("BNS:"))
                        {
                            Wii.Sound.BnsToSoundBin(TempBnsPath, TempUnpackPath + "00000000.app_OUT\\meta\\sound.bin", false);
                        }
                        else
                        {
                            string SoundFile = tbSound.Text;
                            if (tbSound.Text.EndsWith(".mp3")) SoundFile = TempWavePath;

                            Wii.Sound.WaveToSoundBin(SoundFile, TempUnpackPath + "00000000.app_OUT\\meta\\sound.bin", false);
                        }
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

                    if (wadInfo.nandLoader == 0)
                    {
                        using (BinaryReader nandloader = new BinaryReader(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("CustomizeMii.Resources.comex.app")))
                        {
                            using (FileStream fs = new FileStream(TempUnpackPath + "\\00000001.app", FileMode.Create))
                            {
                                byte[] temp = nandloader.ReadBytes((int)nandloader.BaseStream.Length);
                                fs.Write(temp, 0, temp.Length);
                            }
                        }

                        if (tbDol.Text.StartsWith("Simple Forwarder:"))
                        {
                            CreateForwarderSimple(TempUnpackPath + "\\00000002.app");
                        }
                        else if (tbDol.Text.StartsWith("Complex Forwarder"))
                        {
                            bwCreateWad.ReportProgress(82, "Compiling Forwarder...");
                            CreateForwarderComplex(TempUnpackPath + "\\00000002.app");
                        }
                        else if (tbDol.Text == "Internal" || tbDol.Text.EndsWith(".wad"))
                        {
                            File.Copy(TempDolPath, TempUnpackPath + "\\00000002.app");
                        }
                        else
                        {
                            File.Copy(tbDol.Text, TempUnpackPath + "\\00000002.app");
                        }

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

                        if (tbDol.Text.StartsWith("Simple Forwarder:"))
                        {
                            CreateForwarderSimple(TempUnpackPath + "\\00000001.app");
                        }
                        else if (tbDol.Text.StartsWith("Complex Forwarder"))
                        {
                            bwCreateWad.ReportProgress(82, "Compiling Forwarder...");
                            CreateForwarderComplex(TempUnpackPath + "\\00000001.app");
                        }
                        else if (tbDol.Text == "Internal")
                        {
                            File.Copy(TempDolPath, TempUnpackPath + "\\00000001.app");
                        }
                        else
                        {
                            File.Copy(tbDol.Text, TempUnpackPath + "\\00000001.app");
                        }

                        tmd = Wii.WadEdit.ChangeTmdBootIndex(tmd, 2);
                    }

                    tmd = Wii.WadEdit.ChangeTmdContentCount(tmd, 3);

                    bwCreateWad.ReportProgress(85, "Updating TMD...");
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
                if (File.Exists(wadInfo.outFile)) File.Delete(wadInfo.outFile);
                Wii.WadPack.PackWad(TempUnpackPath, wadInfo.outFile);

                bwCreateWad.ReportProgress(100, " ");
                CreationTimer.Stop();

                if (!sendToWii)
                {
                    FileInfo fi = new FileInfo(wadInfo.outFile);
                    double fileSize = Math.Round(fi.Length * 0.0009765625 * 0.0009765625, 2);

                    InfoBox(string.Format("Successfully created custom channel!\nTime elapsed: {0} ms\nFilesize: {1} MB\nApprox. Blocks: {2}", CreationTimer.ElapsedMilliseconds, fileSize, Wii.WadInfo.GetNandBlocks(wadInfo.outFile)));
                }
                else sendWadReady = 1;

                wadCreationInfo = wadInfo;
                wadCreationInfo.success = true;
            }
            catch (Exception ex)
            {
                sendWadReady = -1;
                CreationTimer.Stop();
                EventHandler EnableControls = new EventHandler(this.EnableControls);
                this.Invoke(EnableControls);
                ErrorBox(ex.Message);
            }
        }
    }
}
