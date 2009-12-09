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
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TransmitMii
{
    public partial class TransmitMii_Main : Form
    {
        const string version = "1.1"; //Hint for myself: Never use a char in the Version (UpdateCheck)!
        private bool IsRunning = false;
        private string fileName;
        private string statusText;
        private bool JODI;
        private bool Aborted = false;
        private bool directStart = false;
        EventHandler UpdateStatus;
        EventHandler EnableButtons;
        BackgroundWorker bwTransmit = new BackgroundWorker();
        Regex IpAdress;

        public TransmitMii_Main()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        public TransmitMii_Main(string[] args)
        {
            InitializeComponent();
            this.CenterToScreen();

            if (args[0].EndsWith(".dol") || args[0].EndsWith(".elf") || args[0].EndsWith(".wad"))
            {
                tbFile.Text = args[0];
                directStart = true;
            }
        }

        private void TransmitMii_Main_Load(object sender, EventArgs e)
        {
            this.Text = this.Text.Replace("X", version);
            this.Icon = Properties.Resources.TransmitMii_Icon;
            UpdateCheck();
            ExtensionCheck();

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

            if ((!string.IsNullOrEmpty(tbIP.Text) && !string.IsNullOrEmpty(tbFile.Text)) && directStart)
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

        private void ExtensionCheck()
        {
            string oldPath = TransmitMii_Associations.AssociationPath().ToLower();
            string newPath = Application.ExecutablePath.ToLower();

            if (!string.IsNullOrEmpty(oldPath) && !string.IsNullOrEmpty(newPath))
            {
                if (oldPath != newPath)
                {
                    if (TransmitMii_Associations.CheckAssociation(TransmitMii_Associations.Extension.DOL))
                        TransmitMii_Associations.AddAssociation(TransmitMii_Associations.Extension.DOL, true, newPath, false);
                    if (TransmitMii_Associations.CheckAssociation(TransmitMii_Associations.Extension.ELF))
                        TransmitMii_Associations.AddAssociation(TransmitMii_Associations.Extension.ELF, true, newPath, false);
                }
            }
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
            btnSend.Text = "Send";
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
            if (btnSend.Text == "Abort")
            {
                Aborted = true;

                bwTransmit.CancelAsync();

                try { theStream.Close(); ; }
                catch { }
                try { theClient.Close(); }
                catch { }

                this.Invoke(EnableButtons);
                StatusUpdate(string.Empty);
            }
            else
            {
                if (File.Exists(tbFile.Text))
                {
                    if (IpAdress.IsMatch(tbIP.Text))
                    {
                        if (tbFile.Text.EndsWith(".wad") && cmbProtocol.SelectedIndex != 2)
                        {
                            if (directStart)
                            { cmbProtocol.SelectedIndex = 2; }
                            else
                            { ErrorBox("WAD files can only be sent to USB Loader GX!"); return; }
                        }
                        else if (!tbFile.Text.EndsWith(".wad") && cmbProtocol.SelectedIndex == 2)
                        {
                            if (directStart)
                            { cmbProtocol.SelectedIndex = 0; }
                            else
                            { ErrorBox("The USB Loader GX only accepts WAD files!"); return; }
                        }

                        Aborted = false;

                        btnSend.Text = "Abort";
                        btnBrowseFile.Enabled = false;
                        IsRunning = true;

                        FileStream fs = new FileStream(tbFile.Text, FileMode.Open);
                        byte[] theFile = new byte[fs.Length];
                        fs.Read(theFile, 0, theFile.Length);
                        fs.Close();

                        fileName = Path.GetFileName(tbFile.Text);

                        JODI = cmbProtocol.SelectedIndex == 0 ? true : false;
                        bwTransmit.RunWorkerAsync(theFile);
                    }
                    else { tbIP.Focus(); tbIP.SelectAll(); }
                }
                else { tbFile.Focus(); tbFile.SelectAll(); }
            }
        }

        void bwTransmit_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Invoke(EnableButtons);
            StatusUpdate(string.Empty);
        }

        void bwTransmit_DoWork(object sender, DoWorkEventArgs e)
        {
            byte[] theFile = e.Argument as byte[];

            if (Transmit_Compress(fileName, theFile, JODI, File.Exists(Application.StartupPath + "\\zlib1.dll")))
            {
                if (usedCompression)
                    MessageBox.Show(string.Format("Transmitted {0} kB in {1} milliseconds...\nCompression Ratio: {2}%", transmittedLength, timeElapsed, compressionRatio), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show(string.Format("Transmitted {0} kB in {1} milliseconds...", transmittedLength, timeElapsed), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void llbLinkExtension_MouseEnter(object sender, EventArgs e)
        {
            if (IsRunning == false)
                lbStatus.Text = "Link extensions with TransmitMii...";
        }

        private void llbLinkExtension_MouseLeave(object sender, EventArgs e)
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

        private void llbLinkExtension_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ContextMenuStrip cmMenu = new ContextMenuStrip();

            ToolStripMenuItem cmDol = new ToolStripMenuItem("DOL");
            cmDol.Name = "cmDol";
            cmDol.Click +=new EventHandler(cmMenu_Click);
            if (TransmitMii_Associations.CheckAssociation(TransmitMii_Associations.Extension.DOL)) cmDol.Checked = true;

            ToolStripMenuItem cmElf = new ToolStripMenuItem("ELF");
            cmElf.Name = "cmElf";
            cmElf.Click += new EventHandler(cmMenu_Click);
            if (TransmitMii_Associations.CheckAssociation(TransmitMii_Associations.Extension.ELF)) cmElf.Checked = true;

            ToolStripMenuItem cmWad = new ToolStripMenuItem("WAD");
            cmWad.Name = "cmWad";
            cmWad.Click += new EventHandler(cmMenu_Click);
            if (TransmitMii_Associations.CheckAssociation(TransmitMii_Associations.Extension.WAD)) cmWad.Checked = true;

            cmMenu.Items.Add(cmDol);
            cmMenu.Items.Add(cmElf);
            cmMenu.Items.Add(cmWad);

            cmMenu.Show(MousePosition);
        }

        private void cmMenu_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripMenuItem cmSender = sender as ToolStripMenuItem;
                TransmitMii_Associations.Extension thisExt;

                //CopyIcon();

                switch (cmSender.Name)
                {
                    case "cmElf":
                        thisExt = TransmitMii_Associations.Extension.ELF;
                        break;
                    case "cmWad":
                        thisExt = TransmitMii_Associations.Extension.WAD;
                        break;
                    default:
                        thisExt = TransmitMii_Associations.Extension.DOL;
                        break;
                }

                if (cmSender.Checked == false)
                {
                    if (TransmitMii_Associations.AddAssociation(thisExt, true, Application.ExecutablePath, false))
                        MessageBox.Show("Extension linked!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        ErrorBox("An error occured!");
                }
                else
                {
                    if (TransmitMii_Associations.DeleteAssociation(thisExt))
                        MessageBox.Show("Extension unlinked!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        ErrorBox("An error occured!");
                }
            }
            catch (Exception ex) { ErrorBox(ex.Message); }
        }
    }
}
