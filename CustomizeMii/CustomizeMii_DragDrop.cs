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
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using libWiiSharp;

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
                loadChannel(data[0]);
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
                replacePart(data[0]);
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
                    WAD tmpWad = WAD.Load(data[0]);

                    if (tmpWad.NumOfContents == 3)
                    {
                        int appIndex = 0;
                        if (tmpWad.BootIndex == 1) appIndex = 2;
                        else if (tmpWad.BootIndex == 2) appIndex = 1;

                        if (appIndex > 0)
                        {
                            newDol = tmpWad.Contents[appIndex];
                            setControlText(tbDol, data[0]);
                            btnBrowseDol.Text = "Clear";
                        }
                        else errorBox("The DOL file couldn't be found!");
                    }
                    else errorBox("The DOL file couldn't be found!");
                }
                catch (Exception ex)
                {
                    setControlText(tbDol, string.Empty);
                    btnBrowseDol.Text = "Browse...";
                    errorBox(ex.Message);
                }
            }
            else if (data[0].ToLower().EndsWith(".dol"))
            {
                newDol = File.ReadAllBytes(data[0]);
                setControlText(tbDol, data[0]);
                btnBrowseDol.Text = "Clear";
            }
        }

        private void tbSound_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop) && pbProgress.Value == 100)
            {
                string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (File.Exists(Application.StartupPath + Path.DirectorySeparatorChar + "lame.exe"))
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
                convertMp3ToWave(data[0]);
            }
            else
            {
                replacedSound = data[0];
                setControlText(tbSound, data[0]);
                btnBrowseSound.Text = "Clear";

                newSoundBin = Headers.IMD5.AddHeader(File.ReadAllBytes(data[0]));

                if (cmbReplace.SelectedIndex == 2) setControlText(tbReplace, string.Empty);
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
                    addTpl(lbxBannerTpls, thisFile);
                }
                catch { errors.Add(thisFile); }
            }

            if (errors.Count > 0)
            {
                errorBox(string.Format("These files were not added, because either they already exist or an error occured during conversion:\n\n{0}", string.Join("\n", (errors.ToArray()))));
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
                    addTpl(lbxIconTpls, thisFile);
                }
                catch { errors.Add(thisFile); }
            }

            if (errors.Count > 0)
            {
                errorBox(string.Format("These files were not added, because either they already exist or an error occured during conversion:\n\n{0}", string.Join("\n", (errors.ToArray()))));
            }
        }
    }
}
