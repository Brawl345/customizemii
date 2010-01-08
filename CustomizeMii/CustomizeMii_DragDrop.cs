using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

namespace CustomizeMii
{
    partial class CustomizeMii_Main
    {
        private void CustomizeMii_Main_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop) && pbProgress.Value == 100)
            {
                string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (data.Length == 1 && data[0].ToLower().EndsWith(".wad"))
                    e.Effect = DragDropEffects.Copy;
            }
        }

        private void CustomizeMii_Main_DragDrop(object sender, DragEventArgs e)
        {
            string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (data.Length == 1 && data[0].ToLower().EndsWith(".wad"))
            {
                LoadChannel(data[0]);
            }
        }

        private void tbReplace_DragEnter(object sender, DragEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbSourceWad.Text))
            {
                if (pbProgress.Value == 100 && e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);

                    if (data.Length == 1)
                    {
                        if (data[0].ToLower().EndsWith(".wad") ||
                        data[0].ToLower().EndsWith("00000000.app"))
                            e.Effect = DragDropEffects.Copy;
                        else if ((cmbReplace.SelectedIndex == 2 && data[0].ToLower().EndsWith("sound.bin")) ||
                            (cmbReplace.SelectedIndex == 1 && data[0].ToLower().EndsWith("icon.bin")) ||
                            (cmbReplace.SelectedIndex == 0 && data[0].ToLower().EndsWith("banner.bin")))
                            e.Effect = DragDropEffects.Copy;
                    }
                }
            }
        }

        private void tbReplace_DragDrop(object sender, DragEventArgs e)
        {
            string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (data.Length == 1 && (data[0].ToLower().EndsWith(".wad") ||
                data[0].ToLower().EndsWith("00000000.app") ||
                data[0].ToLower().EndsWith("sound.bin") ||
                data[0].ToLower().EndsWith("icon.bin") ||
                data[0].ToLower().EndsWith("banner.bin")))
            {
                ReplacePart(data[0]);
            }
        }

        private void tbDol_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop) && pbProgress.Value == 100)
            {
                string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (data.Length == 1 && (data[0].ToLower().EndsWith(".wad") || data[0].ToLower().EndsWith(".dol")))
                    e.Effect = DragDropEffects.Copy;
            }
        }

        private void tbDol_DragDrop(object sender, DragEventArgs e)
        {
            string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (data[0].ToLower().EndsWith(".wad"))
            {
                try
                {
                    byte[] wad = Wii.Tools.LoadFileToByteArray(data[0]);

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
                            Wii.WadUnpack.UnpackWad(data[0], TempTempPath + "TempWad");

                            File.Copy(TempTempPath + "TempWad\\" + appFile, TempDolPath, true);
                            SetText(tbDol, data[0]);
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
            else if (data[0].ToLower().EndsWith(".dol"))
            {
                SetText(tbDol, data[0]);
                btnBrowseDol.Text = "Clear";
            }
        }

        private void tbSound_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop) && pbProgress.Value == 100)
            {
                string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (File.Exists(Application.StartupPath + "\\lame.exe"))
                {
                    if (data.Length == 1 && (data[0].ToLower().EndsWith(".wav") ||
                        data[0].ToLower().EndsWith(".bns") ||
                        data[0].ToLower().EndsWith(".mp3")))
                        e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    if (data.Length == 1 && (data[0].ToLower().EndsWith(".wav") ||
                        data[0].ToLower().EndsWith(".bns")))
                        e.Effect = DragDropEffects.Copy;
                }
            }
        }

        private void tbSound_DragDrop(object sender, DragEventArgs e)
        {
            string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (data[0].ToLower().EndsWith(".mp3"))
            {
                ConvertMp3ToWave(data[0]);
            }
            else
            {
                SetText(tbSound, data[0]);

                btnBrowseSound.Text = "Clear";

                if (!string.IsNullOrEmpty(SoundReplace))
                {
                    SoundReplace = string.Empty;
                    if (cmbReplace.SelectedIndex == 2) SetText(tbReplace, SoundReplace);
                    if (File.Exists(TempSoundPath)) File.Delete(TempSoundPath);
                }
            }
        }

        private void lbxBannerTpls_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop) && pbProgress.Value == 100)
            {
                string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);
                bool correct = true;

                for (int i = 0; i < data.Length; i++)
                    if (!(data[0].ToLower().EndsWith(".tpl") ||
                        data[0].ToLower().EndsWith(".png") ||
                        data[0].ToLower().EndsWith(".jpg") ||
                        data[0].ToLower().EndsWith(".gif") ||
                        data[0].ToLower().EndsWith(".bmp")))
                        correct = false;

                if (correct) e.Effect = DragDropEffects.Copy;
            }
        }

        private void lbxBannerTpls_DragDrop(object sender, DragEventArgs e)
        {
            string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);
            List<string> errors = new List<string>();

            foreach (string thisFile in data)
            {
                try
                {
                    AddTpl(lbxBannerTpls, thisFile);
                }
                catch { errors.Add(thisFile); }
            }

            if (errors.Count > 0)
            {
                ErrorBox(string.Format("These files were not added, because either they already exist or an error occured during conversion:\n\n{0}", string.Join("\n", (errors.ToArray()))));
            }
        }

        private void lbxIconTpls_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop) && pbProgress.Value == 100)
            {
                string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);
                bool correct = true;

                for (int i = 0; i < data.Length; i++)
                    if (!(data[0].ToLower().EndsWith(".tpl") ||
                        data[0].ToLower().EndsWith(".png") ||
                        data[0].ToLower().EndsWith(".jpg") ||
                        data[0].ToLower().EndsWith(".gif") ||
                        data[0].ToLower().EndsWith(".bmp")))
                        correct = false;

                if (correct) e.Effect = DragDropEffects.Copy;
            }
        }

        private void lbxIconTpls_DragDrop(object sender, DragEventArgs e)
        {
            string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);
            List<string> errors = new List<string>();

            foreach (string thisFile in data)
            {
                try
                {
                    AddTpl(lbxIconTpls, thisFile);
                }
                catch { errors.Add(thisFile); }
            }

            if (errors.Count > 0)
            {
                ErrorBox(string.Format("These files were not added, because either they already exist or an error occured during conversion:\n\n{0}", string.Join("\n", (errors.ToArray()))));
            }
        }
    }
}
