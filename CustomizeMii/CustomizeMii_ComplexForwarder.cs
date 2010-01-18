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
            TextBox[] tbs = new TextBox[] { tb1, tb2, tb3, tb4 };
            foreach (TextBox tbThis in tbs)
            {
                if ((!tbThis.Text.StartsWith("USB:/") && !tbThis.Text.StartsWith("SD:/")) ||
                    (!tbThis.Text.EndsWith(".dol") && !tbThis.Text.EndsWith(".elf")))
                { tbThis.Focus(); tbThis.SelectAll(); return; }
            }

            tb1.Focus();

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
    }
}
