/* This file is part of TransmitMii
 * Copyright (C) 2009 Leathl
 * 
 * TransmitMii is free software: you can redistribute it and/or
 * modify it under the terms of the GNU General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * TransmitMii is distributed in the hope that it will be
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
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TransmitMii
{
    public partial class TransmitMii_Main : Form
    {
        const string version = "1.0"; //Hint for myself: Never use a char in the Version (UpdateCheck)!
        private bool IsRunning = false;
        private string fileName;
        private string statusText;
        private bool JODI;
        EventHandler UpdateStatus;
        EventHandler EnableButtons;
        BackgroundWorker bwTransmit = new BackgroundWorker();
        Regex IpAdress;

        public TransmitMii_Main()
        {
            InitializeComponent();
            this.CenterToScreen();
            this.Icon = Properties.Resources.TransmitMii_Icon;
        }

        public TransmitMii_Main(string[] args)
        {
            InitializeComponent();
            this.CenterToScreen();

            if (args[0].EndsWith(".dol") || args[0].EndsWith(".elf"))
                tbFile.Text = args[0];
        }

        private void TransmitMii_Main_Load(object sender, EventArgs e)
        {
            this.Text = this.Text.Replace("X", version);
            UpdateCheck();

            UpdateStatus = new EventHandler(this.StatusUpdate);
            EnableButtons = new EventHandler(this.ButtonEnable);
            bwTransmit.DoWork += new DoWorkEventHandler(bwTransmit_DoWork);
            bwTransmit.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwTransmit_RunWorkerCompleted);
            bwTransmit.WorkerSupportsCancellation = true;

            string IpPattern = @"([01]?\d\d?|2[0-4]\d|25[0-5])\." +
                         @"([01]?\d\d?|2[0-4]\d|25[0-5])\." +
                         @"([01]?\d\d?|2[0-4]\d|25[0-5])\." +
                         @"([01]?\d\d?|2[0-4]\d|25[0-5])";
            IpAdress = new Regex(IpPattern);

            LoadSettings();

            if (!string.IsNullOrEmpty(tbIP.Text) && !string.IsNullOrEmpty(tbFile.Text))
                btnSend_Click(null, null);
        }

        private void LoadSettings()
        {
            tbIP.Text = Properties.Settings.Default.IPAddress;
            cmbProtocol.SelectedIndex = Properties.Settings.Default.Protocol;
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.IPAddress = tbIP.Text;
            Properties.Settings.Default.Protocol = cmbProtocol.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void UpdateCheck()
        {
            try
            {
                WebClient GetVersion = new WebClient();
                string NewVersion = GetVersion.DownloadString("http://customizemii.googlecode.com/svn/transmitmii-version.txt");

                int newVersion = Convert.ToInt32(NewVersion.Replace(".", string.Empty).Length == 2 ? (NewVersion.Replace(".", string.Empty) + "0") : NewVersion.Replace(".", string.Empty));
                int thisVersion = Convert.ToInt32(version.Replace(".", string.Empty).Length == 2 ? (version.Replace(".", string.Empty) + "0") : version.Replace(".", string.Empty));

                if (newVersion > thisVersion)
                {
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

        private void ErrorBox(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void StatusUpdate(string theMessage)
        {
            statusText = "Status: " + theMessage;
            this.Invoke(UpdateStatus);
        }

        private void StatusUpdate(object sender, EventArgs e)
        {
            lbStatus.Text = statusText;
        }

        private void ButtonEnable(object sender, EventArgs e)
        {
            IsRunning = false;
            btnBrowseFile.Enabled = true;
            btnSend.Enabled = true;
        }

        private bool Transmit(string fileName, byte[] fileData, bool JODI) //(byte[] theFile, int fileLength, byte[] args, int argsLength)
        {
            TcpClient theClient = new TcpClient();
            NetworkStream theStream;

            int Blocksize = 4 * 1024;
            if (!JODI) Blocksize = 16 * 1024;
            byte[] buffer = new byte[4];
            string theIP = tbIP.Text;

            StatusUpdate("Connecting...");
            try { theClient.Connect(theIP, 4299); }
            catch (Exception ex) { ErrorBox("Connection Failed:\n" + ex.Message); theClient.Close(); return false; }
            theStream = theClient.GetStream(); 

            StatusUpdate("Connected... Sending Magic...");
            buffer[0] = (byte)'H';
            buffer[1] = (byte)'A';
            buffer[2] = (byte)'X';
            buffer[3] = (byte)'X';
            try { theStream.Write(buffer, 0, 4); }
            catch (Exception ex) { ErrorBox("Error sending Magic:\n" + ex.Message); theStream.Close(); theClient.Close(); return false; }

            StatusUpdate("Magic Sent... Sending Version Info...");
            if (JODI)
            {
                buffer[0] = 0;
                buffer[1] = 5;
                buffer[2] = (byte)(((fileName.Length + 2) >> 8) & 0xff);
                buffer[3] = (byte)((fileName.Length + 2) & 0xff);
            }
            else
            {
                buffer[0] = 0;
                buffer[1] = 1;
                buffer[2] = 0;
                buffer[3] = 0;
            }
            try { theStream.Write(buffer, 0, 4); }
            catch (Exception ex) { ErrorBox("Error sending Version Info:\n" + ex.Message); theStream.Close(); theClient.Close(); return false; }

            StatusUpdate("Version Info Sent... Sending Filesize...");
            //First compressed filesize, then uncompressed filesize
            buffer[0] = (byte)((fileData.Length >> 24) & 0xff);
            buffer[1] = (byte)((fileData.Length >> 16) & 0xff);
            buffer[2] = (byte)((fileData.Length >> 8) & 0xff);
            buffer[3] = (byte)(fileData.Length & 0xff);
            try { theStream.Write(buffer, 0, 4); }
            catch (Exception ex) { ErrorBox("Error sending Filesize:\n" + ex.Message); theStream.Close(); theClient.Close(); return false; }

            if (JODI)
            {
                buffer[0] = 0;
                buffer[1] = 0;
                buffer[2] = 0;
                buffer[3] = 0;
                try { theStream.Write(buffer, 0, 4); }
                catch (Exception ex) { ErrorBox("Error sending Filesize:\n" + ex.Message); theStream.Close(); theClient.Close(); return false; }
            }

            StatusUpdate("Filesize Sent... Sending File...");
            int off = 0;
            int cur = 0;
            int count = fileData.Length / Blocksize;
            int left = fileData.Length % Blocksize;

            try
            {
                do
                {
                    StatusUpdate("Sending File: " + ((cur + 1) * 100 / count).ToString() + "%");
                    theStream.Write(fileData, off, Blocksize);
                    off += Blocksize;
                    cur++;
                } while (cur < count);

                if (left > 0)
                {
                    theStream.Write(fileData, off, fileData.Length - off);
                }
            }
            catch (Exception ex) { ErrorBox("Error sending File:\n" + ex.Message); theStream.Close(); theClient.Close(); return false; }

            if (JODI)
            {
                StatusUpdate("File Sent... Sending Arguments...");
                byte[] theArgs = new byte[fileName.Length + 2];
                for (int i = 0; i < fileName.Length; i++) { theArgs[i] = (byte)fileName.ToCharArray()[i]; }
                try { theStream.Write(theArgs, 0, theArgs.Length); }
                catch (Exception ex) { ErrorBox("Error sending Arguments:\n" + ex.Message); theStream.Close(); theClient.Close(); return false; }
            }

            theStream.Close();
            theClient.Close();

            StatusUpdate(string.Empty);

            return true;
        }

        private void btnBrowseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "DOL|*.dol|ELF|*.elf|WAD|*.wad|All|*.dol;*.elf;*.wad";
            ofd.FilterIndex = 4;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                tbFile.Text = ofd.FileName;
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (File.Exists(tbFile.Text))
            {
                if (IpAdress.IsMatch(tbIP.Text))
                {
                    if (tbFile.Text.EndsWith(".wad") && cmbProtocol.SelectedIndex != 2)
                    { ErrorBox("WAD files can only be sent to USB Loader GX!"); return; }

                    btnSend.Enabled = false;
                    btnBrowseFile.Enabled = false;
                    IsRunning = true;

                    FileStream fs = new FileStream(tbFile.Text, FileMode.Open);
                    byte[] theFile = new byte[fs.Length];
                    fs.Read(theFile, 0, theFile.Length);

                    fileName = Path.GetFileName(tbFile.Text);

                    JODI = cmbProtocol.SelectedIndex == 0 ? true : false;
                    bwTransmit.RunWorkerAsync(theFile);
                }
                else { tbIP.Focus(); tbIP.SelectAll(); }
            }
            else { tbFile.Focus(); tbFile.SelectAll(); }
        }

        void bwTransmit_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Invoke(EnableButtons);
            StatusUpdate(string.Empty);
        }

        void bwTransmit_DoWork(object sender, DoWorkEventArgs e)
        {
            byte[] theFile = e.Argument as byte[];

            if (Transmit(fileName, theFile, JODI))
            {
                MessageBox.Show("File Sent...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void tbFile_MouseEnter(object sender, EventArgs e)
        {
            if (IsRunning == false)
                lbStatus.Text = "The file to transmit...";
        }

        private void tbFile_MouseLeave(object sender, EventArgs e)
        {
            if (IsRunning == false)
                lbStatus.Text = "Status:";
        }

        private void tbIP_MouseEnter(object sender, EventArgs e)
        {
            if (IsRunning == false)
                lbStatus.Text = "The IP address of the Wii...";
        }

        private void cmbProtocol_MouseEnter(object sender, EventArgs e)
        {
            if (IsRunning == false)
                lbStatus.Text = "The protocol to use for transmitting...";
        }

        private void cmbProtocol_MouseLeave(object sender, EventArgs e)
        {
            if (IsRunning == false)
                lbStatus.Text = "Status:";
        }

        private void tbIP_MouseLeave(object sender, EventArgs e)
        {
            if (IsRunning == false)
                lbStatus.Text = "Status:";
        }

        private void TransmitMii_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            bwTransmit.CancelAsync();
            SaveSettings();
        }

        private void TransmitMii_Main_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length == 1)
                {
                    if (Path.GetExtension(files[0]) == ".dol" || Path.GetExtension(files[0]) == ".elf")
                        e.Effect = DragDropEffects.Copy;
                    else
                        e.Effect = DragDropEffects.None;
                }
                else e.Effect = DragDropEffects.None;
            }
            else e.Effect = DragDropEffects.None;
        }

        private void TransmitMii_Main_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            tbFile.Text = files[0];
        }
    }
}
