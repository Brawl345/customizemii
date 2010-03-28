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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace CustomizeMii
{
    public partial class CustomizeMii_ComplexForwarder : Form
    {
        public CustomizeMii_ComplexForwarder()
        {
            InitializeComponent();
        }

        private void CustomizeMii_ComplexForwarder_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
        }

        private void cbImage43_CheckedChanged(object sender, EventArgs e)
        {
            tbImage43.Enabled = cbImage43.Checked;
            btnBrowseImage43.Enabled = cbImage43.Checked;
        }

        private void cbImage169_CheckedChanged(object sender, EventArgs e)
        {
            tbImage169.Enabled = cbImage169.Checked;
            btnBrowseImage169.Enabled = cbImage169.Checked;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            TextBox[] tbPack1 = new TextBox[] { tb1, tb2, tb3, tb4 };
            TextBox[] tbPack2 = new TextBox[] { tb5, tb6, tb7, tb8 };
            TextBox[] tbPack3 = new TextBox[] { tb9, tb10, tb11, tb12 };
            TextBox[] tbPack4 = new TextBox[] { tb13, tb14, tb15, tb16 };

            foreach (TextBox tbThis in tbPack1)
            {
                if ((!tbThis.Text.StartsWith("USB:/") && !tbThis.Text.StartsWith("SD:/")) ||
                    (!tbThis.Text.EndsWith(".dol") && !tbThis.Text.EndsWith(".elf")))
                { tabPaths.SelectedTab = tabRequired; tbThis.Focus(); tbThis.SelectAll(); return; }
            }

            if (cbPack1.Checked)
            {
                foreach (TextBox tbThis in tbPack2)
                {
                    if ((!tbThis.Text.StartsWith("USB:/") && !tbThis.Text.StartsWith("SD:/")) ||
                        (!tbThis.Text.EndsWith(".dol") && !tbThis.Text.EndsWith(".elf")))
                    { tabPaths.SelectedTab = tabOptional1; tbThis.Focus(); tbThis.SelectAll(); return; }
                }
            }

            if (cbPack2.Checked)
            {
                foreach (TextBox tbThis in tbPack3)
                {
                    if ((!tbThis.Text.StartsWith("USB:/") && !tbThis.Text.StartsWith("SD:/")) ||
                        (!tbThis.Text.EndsWith(".dol") && !tbThis.Text.EndsWith(".elf")))
                    { tabPaths.SelectedTab = tabOptional2; tbThis.Focus(); tbThis.SelectAll(); return; }
                }
            }

            if (cbPack3.Checked)
            {
                foreach (TextBox tbThis in tbPack4)
                {
                    if ((!tbThis.Text.StartsWith("USB:/") && !tbThis.Text.StartsWith("SD:/")) ||
                        (!tbThis.Text.EndsWith(".dol") && !tbThis.Text.EndsWith(".elf")))
                    { tabPaths.SelectedTab = tabOptional3; tbThis.Focus(); tbThis.SelectAll(); return; }
                }
            }

            if (!File.Exists(tbImage43.Text)) tbImage43.Text = string.Empty;
            if (!File.Exists(tbImage169.Text)) tbImage169.Text = string.Empty;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PNG|*.png|JPG|*.jpg|GIF|*.gif|BMP|*.bmp|All|*.png;*.jpg;*.gif;*.bmp";
            ofd.FilterIndex = 5;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (sender == btnBrowseImage43) tbImage43.Text = ofd.FileName;
                else tbImage169.Text = ofd.FileName;
            }
        }

        private void cbPack1_CheckedChanged(object sender, EventArgs e)
        {
            TextBox[] tbPack2 = new TextBox[] { tb5, tb6, tb7, tb8 };

            foreach (TextBox thisTb in tbPack2)
            {
                thisTb.Enabled = cbPack1.Checked;
                if (!cbPack1.Checked) thisTb.Text = string.Empty;
            }
        }

        private void cbPack2_CheckedChanged(object sender, EventArgs e)
        {
            TextBox[] tbPack3 = new TextBox[] { tb9, tb10, tb11, tb12 };

            foreach (TextBox thisTb in tbPack3)
            {
                thisTb.Enabled = cbPack2.Checked;
                if (!cbPack2.Checked) thisTb.Text = string.Empty;
            }
        }

        private void cbPack3_CheckedChanged(object sender, EventArgs e)
        {
            TextBox[] tbPack4 = new TextBox[] { tb13, tb14, tb15, tb16 };

            foreach (TextBox thisTb in tbPack4)
            {
                thisTb.Enabled = cbPack3.Checked;
                if (!cbPack3.Checked) thisTb.Text = string.Empty;
            }
        }

        private void tb_TextChanged(object sender, EventArgs e)
        {
            TextBox tbSender = sender as TextBox;

            int curPos = tbSender.SelectionStart;

            if (tbSender.Text.Contains("\\"))
                tbSender.Text = tbSender.Text.Replace("\\", "/");

            if (tbSender.Text.Contains("sd:"))
                tbSender.Text = tbSender.Text.Replace("sd:", "SD:");

            if (tbSender.Text.Contains("usb:"))
                tbSender.Text = tbSender.Text.Replace("usb:", "USB:");

            if (tbSender.Text.Contains(".DOL"))
                tbSender.Text = tbSender.Text.Replace(".DOL", ".dol");

            if (tbSender.Text.Contains(".ELF"))
                tbSender.Text = tbSender.Text.Replace(".ELF", ".elf");

            tbSender.SelectionStart = curPos;
        }
    }
}
