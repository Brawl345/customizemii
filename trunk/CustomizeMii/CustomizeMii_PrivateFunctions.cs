using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.IO;
using System.Drawing;

namespace CustomizeMii
{
    partial class CustomizeMii_Main
    {
        private void AddTpl(ListBox lbx)
        {
            AddTpl(lbx, null);
        }

        private void AddTpl(ListBox lbx, string inputFile)
        {
            try
            {
                if (string.IsNullOrEmpty(inputFile))
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Filter = "TPL|*.tpl|PNG|*.png|JPG|*.jpg|GIF|*.gif|BMP|*.bmp|All|*.tpl;*.png;*.jpg;*.gif;*.bmp";
                    ofd.FilterIndex = 6;

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        inputFile = ofd.FileName;
                    }
                }

                if (!string.IsNullOrEmpty(inputFile))
                {
                    for (int i = 0; i < lbx.Items.Count; i++)
                        if (lbx.Items[i].ToString().ToLower() == Path.GetFileNameWithoutExtension(inputFile).ToLower() + ".tpl")
                            throw new Exception("This TPL already exists, use the Replace button");

                    string CurPath;
                    if (lbx == lbxBannerTpls) CurPath = GetCurBannerPath();
                    else CurPath = GetCurIconPath();

                    string[] brlytTpls = Wii.Brlyt.GetBrlytTpls(CurPath + string.Format("blyt\\{0}.brlyt", lbx == lbxBannerTpls ? "banner" : "icon"));
                    string TplName = Path.GetFileNameWithoutExtension(inputFile) + ".tpl";
                    int TplFormat = 6;

                    int switchVal = lbx == lbxBannerTpls ? cmbFormatBanner.SelectedIndex : cmbFormatIcon.SelectedIndex;
                    switch (switchVal)
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
                            if (!inputFile.ToLower().EndsWith(".tpl"))
                                throw new Exception("This format is not supported, you must choose either RGBA8, RGB565 or RGB5A3!");
                            break;
                    }

                    if (!Wii.Tools.StringExistsInStringArray(TplName, brlytTpls))
                    {
                        if (MessageBox.Show(string.Format("{0} is not required by your {1}.brlyt and thus only wastes memory!\nDo you still want to add it?", TplName, lbx == lbxBannerTpls ? "banner" : "icon"), "TPL not required", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                            return;
                    }

                    if (inputFile.ToLower().EndsWith(".tpl"))
                    {
                        File.Copy(inputFile, CurPath + "timg\\" + TplName, true);
                        lbx.Items.Add(TplName);
                    }
                    else
                    {
                        using (Image img = Image.FromFile(inputFile))
                        {
                            Wii.TPL.ConvertToTPL(img, CurPath + "timg\\" + TplName, TplFormat);
                            lbx.Items.Add(TplName);
                        }
                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }

        private void LoadChannel()
        {
            LoadChannel(null);
        }

        private void LoadChannel(string inputFile)
        {
            if (this.Mono) CommonKeyCheck();

            if (pbProgress.Value == 100)
            {
                if (string.IsNullOrEmpty(inputFile))
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
                else
                {
                    SourceWad = inputFile;
                    BackgroundWorker bwLoadChannel = new BackgroundWorker();
                    bwLoadChannel.DoWork += new DoWorkEventHandler(bwLoadChannel_DoWork);
                    bwLoadChannel.ProgressChanged += new ProgressChangedEventHandler(bwLoadChannel_ProgressChanged);
                    bwLoadChannel.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoadChannel_RunWorkerCompleted);
                    bwLoadChannel.WorkerReportsProgress = true;
                    bwLoadChannel.RunWorkerAsync(inputFile);
                }
            }
        }

        private void ReplacePart()
        {
            ReplacePart(null);
        }

        private void ReplacePart(string inputFile)
        {
            if (!string.IsNullOrEmpty(tbSourceWad.Text))
            {
                if (pbProgress.Value == 100)
                {
                    if (cmbReplace.SelectedIndex == 2) //sound
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(inputFile))
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
                            else
                            {
                                if (inputFile != tbSourceWad.Text)
                                {
                                    SoundReplace = inputFile;
                                    SetText(tbReplace, SoundReplace);
                                    BackgroundWorker bwSoundReplace = new BackgroundWorker();
                                    bwSoundReplace.DoWork += new DoWorkEventHandler(bwSoundReplace_DoWork);
                                    bwSoundReplace.ProgressChanged += new ProgressChangedEventHandler(bwSoundReplace_ProgressChanged);
                                    bwSoundReplace.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSoundReplace_RunWorkerCompleted);
                                    bwSoundReplace.WorkerReportsProgress = true;
                                    bwSoundReplace.RunWorkerAsync(inputFile);
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
                            if (string.IsNullOrEmpty(inputFile))
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
                            else
                            {
                                if (inputFile != tbSourceWad.Text)
                                {
                                    IconReplace = inputFile;
                                    SetText(tbReplace, IconReplace);
                                    BackgroundWorker bwIconReplace = new BackgroundWorker();
                                    bwIconReplace.DoWork += new DoWorkEventHandler(bwIconReplace_DoWork);
                                    bwIconReplace.ProgressChanged += new ProgressChangedEventHandler(bwIconReplace_ProgressChanged);
                                    bwIconReplace.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwIconReplace_RunWorkerCompleted);
                                    bwIconReplace.WorkerReportsProgress = true;
                                    bwIconReplace.RunWorkerAsync(inputFile);
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
                            if (string.IsNullOrEmpty(inputFile))
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
                            else
                            {
                                if (inputFile != tbSourceWad.Text)
                                {
                                    BannerReplace = inputFile;
                                    SetText(tbReplace, BannerReplace);
                                    BackgroundWorker bwBannerReplace = new BackgroundWorker();
                                    bwBannerReplace.DoWork += new DoWorkEventHandler(bwBannerReplace_DoWork);
                                    bwBannerReplace.ProgressChanged += new ProgressChangedEventHandler(bwBannerReplace_ProgressChanged);
                                    bwBannerReplace.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwBannerReplace_RunWorkerCompleted);
                                    bwBannerReplace.WorkerReportsProgress = true;
                                    bwBannerReplace.RunWorkerAsync(inputFile);
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
    }
}
