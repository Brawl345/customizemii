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
 
namespace CustomizeMii
{
    partial class CustomizeMii_BnsConvert
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnConvert = new System.Windows.Forms.Button();
            this.lbAudioFile = new System.Windows.Forms.Label();
            this.tbAudioFile = new System.Windows.Forms.TextBox();
            this.btnBrowseAudioFile = new System.Windows.Forms.Button();
            this.gbLoop = new System.Windows.Forms.GroupBox();
            this.tbarLoopStartSample = new System.Windows.Forms.TrackBar();
            this.lbStartSample = new System.Windows.Forms.Label();
            this.tbLoopStart = new System.Windows.Forms.TextBox();
            this.rbEnterManually = new System.Windows.Forms.RadioButton();
            this.rbFromAudioFile = new System.Windows.Forms.RadioButton();
            this.rbNone = new System.Windows.Forms.RadioButton();
            this.gbWaveInfo = new System.Windows.Forms.GroupBox();
            this.lbStatusValue = new System.Windows.Forms.Label();
            this.lbLoopStartValue = new System.Windows.Forms.Label();
            this.lbLoopCountValue = new System.Windows.Forms.Label();
            this.lbFormatValue = new System.Windows.Forms.Label();
            this.lbChannelCountValue = new System.Windows.Forms.Label();
            this.lbSamplerateValue = new System.Windows.Forms.Label();
            this.lbBitdepthValue = new System.Windows.Forms.Label();
            this.lbStatus = new System.Windows.Forms.Label();
            this.lbLoopStart = new System.Windows.Forms.Label();
            this.lbLoopCount = new System.Windows.Forms.Label();
            this.lbFormat = new System.Windows.Forms.Label();
            this.lbChannelCount = new System.Windows.Forms.Label();
            this.lbSamplerate = new System.Windows.Forms.Label();
            this.lbBitdepth = new System.Windows.Forms.Label();
            this.cbSourceSound = new System.Windows.Forms.CheckBox();
            this.gbPrelisten = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnPlay = new System.Windows.Forms.Button();
            this.gbLoop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbarLoopStartSample)).BeginInit();
            this.gbWaveInfo.SuspendLayout();
            this.gbPrelisten.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(186, 189);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(160, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConvert
            // 
            this.btnConvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnConvert.Location = new System.Drawing.Point(15, 189);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(160, 23);
            this.btnConvert.TabIndex = 9;
            this.btnConvert.Text = "Convert";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // lbAudioFile
            // 
            this.lbAudioFile.AutoSize = true;
            this.lbAudioFile.Location = new System.Drawing.Point(12, 24);
            this.lbAudioFile.Name = "lbAudioFile";
            this.lbAudioFile.Size = new System.Drawing.Size(56, 13);
            this.lbAudioFile.TabIndex = 11;
            this.lbAudioFile.Text = "Audio File:";
            // 
            // tbAudioFile
            // 
            this.tbAudioFile.Location = new System.Drawing.Point(74, 21);
            this.tbAudioFile.Name = "tbAudioFile";
            this.tbAudioFile.ReadOnly = true;
            this.tbAudioFile.Size = new System.Drawing.Size(191, 20);
            this.tbAudioFile.TabIndex = 12;
            // 
            // btnBrowseAudioFile
            // 
            this.btnBrowseAudioFile.Location = new System.Drawing.Point(271, 20);
            this.btnBrowseAudioFile.Name = "btnBrowseAudioFile";
            this.btnBrowseAudioFile.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseAudioFile.TabIndex = 13;
            this.btnBrowseAudioFile.Text = "Browse...";
            this.btnBrowseAudioFile.UseVisualStyleBackColor = true;
            this.btnBrowseAudioFile.Click += new System.EventHandler(this.btnBrowseAudioFile_Click);
            // 
            // gbLoop
            // 
            this.gbLoop.Controls.Add(this.tbarLoopStartSample);
            this.gbLoop.Controls.Add(this.lbStartSample);
            this.gbLoop.Controls.Add(this.tbLoopStart);
            this.gbLoop.Controls.Add(this.rbEnterManually);
            this.gbLoop.Controls.Add(this.rbFromAudioFile);
            this.gbLoop.Controls.Add(this.rbNone);
            this.gbLoop.Location = new System.Drawing.Point(15, 70);
            this.gbLoop.Name = "gbLoop";
            this.gbLoop.Size = new System.Drawing.Size(331, 113);
            this.gbLoop.TabIndex = 14;
            this.gbLoop.TabStop = false;
            this.gbLoop.Text = "Loop";
            // 
            // tbarLoopStartSample
            // 
            this.tbarLoopStartSample.Enabled = false;
            this.tbarLoopStartSample.Location = new System.Drawing.Point(107, 62);
            this.tbarLoopStartSample.Maximum = 1059620;
            this.tbarLoopStartSample.Name = "tbarLoopStartSample";
            this.tbarLoopStartSample.Size = new System.Drawing.Size(142, 45);
            this.tbarLoopStartSample.TabIndex = 3;
            this.tbarLoopStartSample.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbarLoopStartSample.Scroll += new System.EventHandler(this.tbarLoopStartSample_Scroll);
            // 
            // lbStartSample
            // 
            this.lbStartSample.AutoSize = true;
            this.lbStartSample.Location = new System.Drawing.Point(35, 74);
            this.lbStartSample.Name = "lbStartSample";
            this.lbStartSample.Size = new System.Drawing.Size(66, 13);
            this.lbStartSample.TabIndex = 2;
            this.lbStartSample.Text = "(startsample)";
            // 
            // tbLoopStart
            // 
            this.tbLoopStart.Enabled = false;
            this.tbLoopStart.Location = new System.Drawing.Point(256, 66);
            this.tbLoopStart.Name = "tbLoopStart";
            this.tbLoopStart.Size = new System.Drawing.Size(60, 20);
            this.tbLoopStart.TabIndex = 1;
            this.tbLoopStart.Text = "0";
            this.tbLoopStart.TextChanged += new System.EventHandler(this.tbLoopStart_TextChanged);
            this.tbLoopStart.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbLoopStart_KeyPress);
            // 
            // rbEnterManually
            // 
            this.rbEnterManually.AutoSize = true;
            this.rbEnterManually.Location = new System.Drawing.Point(6, 59);
            this.rbEnterManually.Name = "rbEnterManually";
            this.rbEnterManually.Size = new System.Drawing.Size(95, 17);
            this.rbEnterManually.TabIndex = 0;
            this.rbEnterManually.Text = "Enter Manually";
            this.rbEnterManually.UseVisualStyleBackColor = true;
            this.rbEnterManually.CheckedChanged += new System.EventHandler(this.rbSelectionChanged);
            // 
            // rbFromAudioFile
            // 
            this.rbFromAudioFile.AutoSize = true;
            this.rbFromAudioFile.Enabled = false;
            this.rbFromAudioFile.Location = new System.Drawing.Point(6, 39);
            this.rbFromAudioFile.Name = "rbFromAudioFile";
            this.rbFromAudioFile.Size = new System.Drawing.Size(281, 17);
            this.rbFromAudioFile.TabIndex = 0;
            this.rbFromAudioFile.Text = "From Audio File (works only with pre-looped wave files)";
            this.rbFromAudioFile.UseVisualStyleBackColor = true;
            this.rbFromAudioFile.CheckedChanged += new System.EventHandler(this.rbSelectionChanged);
            // 
            // rbNone
            // 
            this.rbNone.AutoSize = true;
            this.rbNone.Checked = true;
            this.rbNone.Location = new System.Drawing.Point(6, 19);
            this.rbNone.Name = "rbNone";
            this.rbNone.Size = new System.Drawing.Size(51, 17);
            this.rbNone.TabIndex = 0;
            this.rbNone.TabStop = true;
            this.rbNone.Text = "None";
            this.rbNone.UseVisualStyleBackColor = true;
            this.rbNone.CheckedChanged += new System.EventHandler(this.rbSelectionChanged);
            // 
            // gbWaveInfo
            // 
            this.gbWaveInfo.Controls.Add(this.lbStatusValue);
            this.gbWaveInfo.Controls.Add(this.lbLoopStartValue);
            this.gbWaveInfo.Controls.Add(this.lbLoopCountValue);
            this.gbWaveInfo.Controls.Add(this.lbFormatValue);
            this.gbWaveInfo.Controls.Add(this.lbChannelCountValue);
            this.gbWaveInfo.Controls.Add(this.lbSamplerateValue);
            this.gbWaveInfo.Controls.Add(this.lbBitdepthValue);
            this.gbWaveInfo.Controls.Add(this.lbStatus);
            this.gbWaveInfo.Controls.Add(this.lbLoopStart);
            this.gbWaveInfo.Controls.Add(this.lbLoopCount);
            this.gbWaveInfo.Controls.Add(this.lbFormat);
            this.gbWaveInfo.Controls.Add(this.lbChannelCount);
            this.gbWaveInfo.Controls.Add(this.lbSamplerate);
            this.gbWaveInfo.Controls.Add(this.lbBitdepth);
            this.gbWaveInfo.Location = new System.Drawing.Point(363, 20);
            this.gbWaveInfo.Name = "gbWaveInfo";
            this.gbWaveInfo.Size = new System.Drawing.Size(135, 192);
            this.gbWaveInfo.TabIndex = 15;
            this.gbWaveInfo.TabStop = false;
            this.gbWaveInfo.Text = "Wave Info";
            // 
            // lbStatusValue
            // 
            this.lbStatusValue.Location = new System.Drawing.Point(75, 157);
            this.lbStatusValue.Name = "lbStatusValue";
            this.lbStatusValue.Size = new System.Drawing.Size(54, 13);
            this.lbStatusValue.TabIndex = 7;
            // 
            // lbLoopStartValue
            // 
            this.lbLoopStartValue.Location = new System.Drawing.Point(75, 135);
            this.lbLoopStartValue.Name = "lbLoopStartValue";
            this.lbLoopStartValue.Size = new System.Drawing.Size(54, 13);
            this.lbLoopStartValue.TabIndex = 7;
            // 
            // lbLoopCountValue
            // 
            this.lbLoopCountValue.Location = new System.Drawing.Point(75, 113);
            this.lbLoopCountValue.Name = "lbLoopCountValue";
            this.lbLoopCountValue.Size = new System.Drawing.Size(54, 13);
            this.lbLoopCountValue.TabIndex = 7;
            // 
            // lbFormatValue
            // 
            this.lbFormatValue.Location = new System.Drawing.Point(75, 91);
            this.lbFormatValue.Name = "lbFormatValue";
            this.lbFormatValue.Size = new System.Drawing.Size(54, 13);
            this.lbFormatValue.TabIndex = 7;
            // 
            // lbChannelCountValue
            // 
            this.lbChannelCountValue.Location = new System.Drawing.Point(75, 69);
            this.lbChannelCountValue.Name = "lbChannelCountValue";
            this.lbChannelCountValue.Size = new System.Drawing.Size(54, 13);
            this.lbChannelCountValue.TabIndex = 7;
            // 
            // lbSamplerateValue
            // 
            this.lbSamplerateValue.Location = new System.Drawing.Point(75, 47);
            this.lbSamplerateValue.Name = "lbSamplerateValue";
            this.lbSamplerateValue.Size = new System.Drawing.Size(54, 13);
            this.lbSamplerateValue.TabIndex = 7;
            // 
            // lbBitdepthValue
            // 
            this.lbBitdepthValue.Location = new System.Drawing.Point(75, 25);
            this.lbBitdepthValue.Name = "lbBitdepthValue";
            this.lbBitdepthValue.Size = new System.Drawing.Size(54, 13);
            this.lbBitdepthValue.TabIndex = 7;
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Location = new System.Drawing.Point(6, 157);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(40, 13);
            this.lbStatus.TabIndex = 6;
            this.lbStatus.Text = "Status:";
            // 
            // lbLoopStart
            // 
            this.lbLoopStart.AutoSize = true;
            this.lbLoopStart.Location = new System.Drawing.Point(6, 135);
            this.lbLoopStart.Name = "lbLoopStart";
            this.lbLoopStart.Size = new System.Drawing.Size(59, 13);
            this.lbLoopStart.TabIndex = 5;
            this.lbLoopStart.Text = "Loop Start:";
            // 
            // lbLoopCount
            // 
            this.lbLoopCount.AutoSize = true;
            this.lbLoopCount.Location = new System.Drawing.Point(6, 113);
            this.lbLoopCount.Name = "lbLoopCount";
            this.lbLoopCount.Size = new System.Drawing.Size(39, 13);
            this.lbLoopCount.TabIndex = 4;
            this.lbLoopCount.Text = "Loops:";
            // 
            // lbFormat
            // 
            this.lbFormat.AutoSize = true;
            this.lbFormat.Location = new System.Drawing.Point(6, 91);
            this.lbFormat.Name = "lbFormat";
            this.lbFormat.Size = new System.Drawing.Size(42, 13);
            this.lbFormat.TabIndex = 3;
            this.lbFormat.Text = "Format:";
            // 
            // lbChannelCount
            // 
            this.lbChannelCount.AutoSize = true;
            this.lbChannelCount.Location = new System.Drawing.Point(6, 69);
            this.lbChannelCount.Name = "lbChannelCount";
            this.lbChannelCount.Size = new System.Drawing.Size(54, 13);
            this.lbChannelCount.TabIndex = 2;
            this.lbChannelCount.Text = "Channels:";
            // 
            // lbSamplerate
            // 
            this.lbSamplerate.AutoSize = true;
            this.lbSamplerate.Location = new System.Drawing.Point(6, 47);
            this.lbSamplerate.Name = "lbSamplerate";
            this.lbSamplerate.Size = new System.Drawing.Size(63, 13);
            this.lbSamplerate.TabIndex = 1;
            this.lbSamplerate.Text = "Samplerate:";
            // 
            // lbBitdepth
            // 
            this.lbBitdepth.AutoSize = true;
            this.lbBitdepth.Location = new System.Drawing.Point(6, 25);
            this.lbBitdepth.Name = "lbBitdepth";
            this.lbBitdepth.Size = new System.Drawing.Size(49, 13);
            this.lbBitdepth.TabIndex = 0;
            this.lbBitdepth.Text = "Bitdepth:";
            // 
            // cbSourceSound
            // 
            this.cbSourceSound.AutoSize = true;
            this.cbSourceSound.Enabled = false;
            this.cbSourceSound.Location = new System.Drawing.Point(15, 48);
            this.cbSourceSound.Name = "cbSourceSound";
            this.cbSourceSound.Size = new System.Drawing.Size(170, 17);
            this.cbSourceSound.TabIndex = 16;
            this.cbSourceSound.Text = "Take sound from source WAD";
            this.cbSourceSound.UseVisualStyleBackColor = true;
            this.cbSourceSound.CheckedChanged += new System.EventHandler(this.cbSourceSound_CheckedChanged);
            // 
            // gbPrelisten
            // 
            this.gbPrelisten.Controls.Add(this.label1);
            this.gbPrelisten.Controls.Add(this.btnPlay);
            this.gbPrelisten.Location = new System.Drawing.Point(519, 20);
            this.gbPrelisten.Name = "gbPrelisten";
            this.gbPrelisten.Size = new System.Drawing.Size(135, 192);
            this.gbPrelisten.TabIndex = 17;
            this.gbPrelisten.TabStop = false;
            this.gbPrelisten.Text = "Prelisten Loop";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 118);
            this.label1.TabIndex = 0;
            this.label1.Text = "Note:\r\n\r\nYou will only hear the looped part, not the part before.\r\nAlso, the Loop" +
                " might be inaccurate (only a few samples)";
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(6, 156);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(123, 23);
            this.btnPlay.TabIndex = 13;
            this.btnPlay.Text = "Play Loop";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // CustomizeMii_BnsConvert
            // 
            this.AcceptButton = this.btnConvert;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(674, 230);
            this.Controls.Add(this.gbPrelisten);
            this.Controls.Add(this.cbSourceSound);
            this.Controls.Add(this.gbWaveInfo);
            this.Controls.Add(this.gbLoop);
            this.Controls.Add(this.btnBrowseAudioFile);
            this.Controls.Add(this.tbAudioFile);
            this.Controls.Add(this.lbAudioFile);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConvert);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CustomizeMii_BnsConvert";
            this.Text = "CustomizeMii_BnsConvert";
            this.Load += new System.EventHandler(this.CustomizeMii_BnsConvert_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CustomizeMii_BnsConvert_FormClosing);
            this.gbLoop.ResumeLayout(false);
            this.gbLoop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbarLoopStartSample)).EndInit();
            this.gbWaveInfo.ResumeLayout(false);
            this.gbWaveInfo.PerformLayout();
            this.gbPrelisten.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.Label lbAudioFile;
        private System.Windows.Forms.TextBox tbAudioFile;
        private System.Windows.Forms.Button btnBrowseAudioFile;
        private System.Windows.Forms.GroupBox gbLoop;
        private System.Windows.Forms.RadioButton rbNone;
        private System.Windows.Forms.RadioButton rbEnterManually;
        private System.Windows.Forms.RadioButton rbFromAudioFile;
        private System.Windows.Forms.TextBox tbLoopStart;
        private System.Windows.Forms.GroupBox gbWaveInfo;
        private System.Windows.Forms.Label lbBitdepth;
        private System.Windows.Forms.Label lbSamplerate;
        private System.Windows.Forms.Label lbChannelCount;
        private System.Windows.Forms.Label lbFormat;
        private System.Windows.Forms.Label lbLoopCount;
        private System.Windows.Forms.Label lbLoopStart;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.Label lbStatusValue;
        private System.Windows.Forms.Label lbLoopStartValue;
        private System.Windows.Forms.Label lbLoopCountValue;
        private System.Windows.Forms.Label lbFormatValue;
        private System.Windows.Forms.Label lbChannelCountValue;
        private System.Windows.Forms.Label lbSamplerateValue;
        private System.Windows.Forms.Label lbBitdepthValue;
        private System.Windows.Forms.CheckBox cbSourceSound;
        private System.Windows.Forms.GroupBox gbPrelisten;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Label lbStartSample;
        private System.Windows.Forms.TrackBar tbarLoopStartSample;

    }
}