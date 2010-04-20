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
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using System.Media;
using libWiiSharp;

namespace CustomizeMii
{
    public partial class CustomizeMii_BnsConvert : Form
    {
        private bool lameExists;
        private int loopStartSample;
        private BackgroundWorker bwGatherInfo;
        private int bitDepth;
        private int sampleRate;
        private int channelCount;
        private int dataFormat;
        private int loopCount;
        private int loopStart;
        private bool error = false;
        private bool cancelled = false;
        private int sampleCount;
        private SoundPlayer sPlayer;
        private bool mp3LengthKnown = false;
        private byte[] sourceSound = new byte[0];

        public string AudioFile { get { return tbAudioFile.Text; } }
        public bool LoopNone { get { return rbNone.Checked; } }
        public bool LoopFromAudio { get { return rbFromAudioFile.Checked; } }
        public bool LoopManually { get { return rbEnterManually.Checked; } }
        public int LoopStartSample { get { return loopStartSample; } }
        public int ChannelCount { get { return channelCount; } }
        public byte[] SourceSound { get { return sourceSound; } set { sourceSound = value; } }

        public CustomizeMii_BnsConvert(bool lameExists)
        {
            InitializeComponent();
            this.lameExists = lameExists;
        }

        private void CustomizeMii_BnsConvert_Load(object sender, EventArgs e)
        {
            //this.Size = new System.Drawing.Size(358, 220);
            this.Size = new System.Drawing.Size(btnCancel.Location.X + btnCancel.Size.Width + 15, this.Size.Height);

            this.CenterToParent();

            bwGatherInfo = new BackgroundWorker();
            bwGatherInfo.WorkerSupportsCancellation = true;
            bwGatherInfo.DoWork += new DoWorkEventHandler(bwGatherInfo_DoWork);

            if (sourceSound[0] == 'R' && sourceSound[1] == 'I' && sourceSound[2] == 'F' && sourceSound[3] == 'F')
            { cbSourceSound.Enabled = true; }
            else if (sourceSound[0] == 'L' && sourceSound[1] == 'Z' && sourceSound[2] == '7' && sourceSound[3] == '7')
                if (sourceSound[9] == 'R' && sourceSound[10] == 'I' && sourceSound[11] == 'F' && sourceSound[12] == 'F')
                { cbSourceSound.Enabled = true; }
        }

        private void CustomizeMii_BnsConvert_FormClosing(object sender, FormClosingEventArgs e)
        {
            cancelled = true;

            if (bwGatherInfo.IsBusy)
                bwGatherInfo.CancelAsync();
        }

        private void rbSelectionChanged(object sender, EventArgs e)
        {
            tbLoopStart.Enabled = rbEnterManually.Checked;
            if (tbAudioFile.Text.ToLower().EndsWith(".mp3")) { tbarLoopStartSample.Enabled = (rbEnterManually.Checked && mp3LengthKnown); }
            else { tbarLoopStartSample.Enabled = rbEnterManually.Checked; }

            if (!rbNone.Checked && this.Size.Width > 400)
                this.Size = new System.Drawing.Size(gbPrelisten.Location.X + gbPrelisten.Size.Width + 15, this.Size.Height);
            else if (rbNone.Checked && this.Size.Width > 400)
                this.Size = new System.Drawing.Size(gbWaveInfo.Location.X + gbWaveInfo.Size.Width + 15, this.Size.Height);

        }

        private void tbLoopStart_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != '\b';
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (!File.Exists(tbAudioFile.Text) && tbAudioFile.Text != "Internal Sound")
            {
                tbAudioFile.Focus();
                tbAudioFile.SelectAll();
                return;
            }

            if (!int.TryParse(tbLoopStart.Text, out loopStartSample))
            {
                tbLoopStart.Focus();
                tbLoopStart.SelectAll();
            }

            if (this.sPlayer != null) { this.sPlayer.Stop(); this.sPlayer.Dispose(); }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (this.sPlayer != null) { this.sPlayer.Stop(); this.sPlayer.Dispose(); }
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void cbSourceSound_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSourceSound.Checked)
            {
                tbAudioFile.Text = "Internal Sound";

                if (Lz77.IsLz77Compressed(sourceSound))
                { Lz77 l = new Lz77(); sourceSound = l.Decompress(sourceSound); }

                foreach (Label thisLabel in gbWaveInfo.Controls)
                    if (thisLabel.Name.ToLower().Contains("value"))
                    {
                        thisLabel.ForeColor = System.Drawing.Color.Black;
                        thisLabel.Text = "Gathering";
                    }

                bwGatherInfo.RunWorkerAsync(sourceSound);

                //this.Size = new System.Drawing.Size(510, 220);
                this.Size = new System.Drawing.Size(gbWaveInfo.Location.X + gbWaveInfo.Size.Width + 15, this.Size.Height);
            }
            else
            {
                if (tbAudioFile.Text == "Internal Sound")
                    tbAudioFile.Text = string.Empty;

                //this.Size = new System.Drawing.Size(358, 220);
                this.Size = new System.Drawing.Size(btnCancel.Location.X + btnCancel.Size.Width + 15, this.Size.Height);
            }
        }

        private void btnBrowseAudioFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (lameExists)
            { ofd.Filter = "Wave|*.wav|MP3|*.mp3|All|*.wav;*.mp3"; ofd.FilterIndex = 3; }
            else
            { ofd.Filter = "Wave|*.wav"; }

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (ofd.FileName != tbAudioFile.Text)
                {
                    btnConvert.Enabled = false;
                    tbarLoopStartSample.Enabled = false;
                    mp3LengthKnown = false;

                    cbSourceSound.Checked = false;
                    tbAudioFile.Text = ofd.FileName;

                    if (ofd.FileName.EndsWith(".wav"))
                    {
                        rbFromAudioFile.Enabled = true;

                        foreach (Label thisLabel in gbWaveInfo.Controls)
                            if (thisLabel.Name.ToLower().Contains("value"))
                            {
                                thisLabel.ForeColor = System.Drawing.Color.Black;
                                thisLabel.Text = "Gathering";
                            }

                        bwGatherInfo.RunWorkerAsync(ofd.FileName);

                        this.Size = new System.Drawing.Size(gbWaveInfo.Location.X + gbWaveInfo.Size.Width + 15, this.Size.Height);
                    }
                    else if (ofd.FileName.EndsWith(".mp3"))
                    {
                        channelCount = 0;
                        btnConvert.Enabled = true;

                        if (rbFromAudioFile.Checked) rbNone.Checked = true;
                        rbFromAudioFile.Enabled = false;

                        this.Size = new System.Drawing.Size(btnCancel.Location.X + btnCancel.Size.Width + 15, this.Size.Height);

                        try
                        {
                            MP3Info mp3Info = new MP3Info(ofd.FileName);
                            tbarLoopStartSample.Maximum = mp3Info.AudioSamples;
                            mp3LengthKnown = true;
                            if (rbEnterManually.Checked) tbarLoopStartSample.Enabled = true;
                        }
                        catch { }
                    }
                }
            }
        }

        void bwGatherInfo_DoWork(object sender, DoWorkEventArgs e)
        {
            EventHandler UpdateValues = new EventHandler(this.UpdateValues);

            try
            {
                Wave wave;
                if (e.Argument is byte[])
                { wave = new Wave((byte[])e.Argument); }
                else
                { wave = new Wave((string)e.Argument); }

                try { this.sampleCount = wave.NumSamples; }
                catch { }

                try { bitDepth = wave.BitDepth; }
                catch { bitDepth = -1; }

                try { sampleRate = wave.SampleRate; }
                catch { sampleRate = -1; }

                try { channelCount = wave.NumChannels; }
                catch { channelCount = -1; }

                try { dataFormat = wave.DataFormat; }
                catch { dataFormat = -1; }

                try { loopCount = wave.NumLoops; }
                catch { loopCount = -1; }

                try { loopStart = wave.LoopStart; }
                catch { loopStart = -1; }

                if (!cancelled)
                    this.Invoke(UpdateValues);
            }
            catch
            {
                error = true;

                if (!cancelled)
                    this.Invoke(UpdateValues);
            }
        }

        void UpdateValues(object sender, EventArgs e)
        {
            if (error == true)
            {
                foreach (Label thisLabel in gbWaveInfo.Controls)
                    if (thisLabel.Name.ToLower().Contains("value"))
                    {
                        thisLabel.Text = "Error";
                        thisLabel.ForeColor = System.Drawing.Color.Red;
                    }

                error = false;
                return;
            }

            bool statusOk = true;
            tbarLoopStartSample.Maximum = this.sampleCount;
            if (rbEnterManually.Checked) tbarLoopStartSample.Enabled = true;

            if (bitDepth == -1) lbBitdepthValue.Text = "Error";
            else lbBitdepthValue.Text = bitDepth.ToString();

            if (sampleRate == -1) lbSamplerateValue.Text = "Error";
            else lbSamplerateValue.Text = sampleRate.ToString();

            if (dataFormat == -1) lbFormatValue.Text = "Error";
            else if (dataFormat == 1) lbFormatValue.Text = "1 (PCM)";
            else lbFormatValue.Text = dataFormat.ToString();

            if (channelCount == -1) lbChannelCountValue.Text = "Error";
            else if (channelCount == 1) lbChannelCountValue.Text = "1 (Mono)";
            else if (channelCount == 2) lbChannelCountValue.Text = "2 (Stereo)";
            else lbChannelCountValue.Text = channelCount.ToString();

            if (loopCount == -1) lbLoopCountValue.Text = "Error";
            else lbLoopCountValue.Text = loopCount.ToString();

            if (loopCount == -1) lbLoopStartValue.Text = "Error";
            else if (loopCount == 1) { lbLoopStartValue.Text = loopStart == -1 ? "Error" : loopStart.ToString(); }
            else lbLoopStartValue.Text = "-";



            if (lbBitdepthValue.Text == "Error" || bitDepth != 16)
            {
                lbBitdepthValue.ForeColor = System.Drawing.Color.Red;
                statusOk = false;
            }
            else lbBitdepthValue.ForeColor = System.Drawing.Color.Green;

            if (lbSamplerateValue.Text == "Error")
            {
                lbSamplerateValue.ForeColor = System.Drawing.Color.Red;
                statusOk = false;
            }
            else lbSamplerateValue.ForeColor = System.Drawing.Color.Green;

            if (lbFormatValue.Text == "Error" || dataFormat != 1)
            {
                lbFormatValue.ForeColor = System.Drawing.Color.Red;
                statusOk = false;
            }
            else lbFormatValue.ForeColor = System.Drawing.Color.Green;

            if (lbChannelCountValue.Text == "Error" || (channelCount > 2 || channelCount < 1))
            {
                lbChannelCountValue.ForeColor = System.Drawing.Color.Red;
                statusOk = false;
            }
            else lbChannelCountValue.ForeColor = System.Drawing.Color.Green;

            if (lbLoopCountValue.Text == "Error" || loopCount > 1)
                lbLoopCountValue.ForeColor = System.Drawing.Color.Orange;
            else lbLoopCountValue.ForeColor = System.Drawing.Color.Green;

            if (lbLoopStartValue.Text == "Error" || lbLoopStartValue.Text == "-")
                lbLoopStartValue.ForeColor = System.Drawing.Color.Orange;
            else lbLoopStartValue.ForeColor = System.Drawing.Color.Green;

            if (!statusOk)
            {
                lbStatusValue.Text = "Not OK!";
                lbStatusValue.ForeColor = System.Drawing.Color.Red;

                btnConvert.Enabled = false;
            }
            else
            {
                btnConvert.Enabled = true;

                if (lbLoopCountValue.ForeColor == System.Drawing.Color.Orange ||
                    lbLoopStartValue.ForeColor == System.Drawing.Color.Orange)
                {
                    lbStatusValue.Text = "No Loop!";
                    lbStatusValue.ForeColor = System.Drawing.Color.Orange;

                    if (rbFromAudioFile.Checked) rbNone.Checked = true;
                    rbFromAudioFile.Enabled = false;
                }
                else
                {
                    rbFromAudioFile.Enabled = true;

                    lbStatusValue.Text = "OK!";
                    lbStatusValue.ForeColor = System.Drawing.Color.Green;
                }
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (btnPlay.Text == "Stop")
            {
                if (sPlayer != null)
                {
                    sPlayer.Stop();
                    sPlayer = null;

                    btnPlay.Text = "Play Loop";
                }
            }
            else
            {
                try
                {
                    int loopStart = rbFromAudioFile.Checked ? int.Parse(lbLoopStartValue.Text) : int.Parse(tbLoopStart.Text);

                    Wave wave;
                    if (cbSourceSound.Checked)
                        wave = new Wave(sourceSound);
                    else 
                        wave = new Wave(tbAudioFile.Text);

                    wave.TrimStart(loopStart);
                    MemoryStream waveFile = wave.ToMemoryStream();
                    wave.Dispose();

                    waveFile.Seek(0, SeekOrigin.Begin);

                    sPlayer = new SoundPlayer(waveFile);
                    sPlayer.PlayLooping();

                    btnPlay.Text = "Stop";
                }
                catch (Exception ex)
                {
                    sPlayer = null;
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tbarLoopStartSample_Scroll(object sender, EventArgs e)
        {
            try { tbLoopStart.Text = tbarLoopStartSample.Value.ToString(); }
            catch { }
        }

        private void tbLoopStart_TextChanged(object sender, EventArgs e)
        {
            if (tbarLoopStartSample.Enabled)
                try { tbarLoopStartSample.Value = int.Parse(tbLoopStart.Text); }
                catch { }
        }
    }
}
