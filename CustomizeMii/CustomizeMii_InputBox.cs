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
using System.Windows.Forms;

namespace CustomizeMii
{
    public partial class CustomizeMii_InputBox : Form
    {
        //Wow, need to change this whole thing, but too lazy ~~

        public bool CommonKeyMode = true;
        public string Input { get { return tbInput.Text; } set { tbInput.Text = value; } }
        public string Description { get { return lbInfo.Text; } set { lbInfo.Text = value; } }
        public string Input2 { get { return tbInput2.Text; } set { tbInput2.Text = value; } }

        public CustomizeMii_InputBox()
        {
            InitializeComponent();
        }

        public CustomizeMii_InputBox(bool CommonKeyMode)
        {
            InitializeComponent();
            this.CommonKeyMode = CommonKeyMode;
        }

        private void CustomizeMii_InputBox_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
            if (this.Location.X == 0 && this.Location.Y == 0) this.CenterToScreen();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (CommonKeyMode == true)
            {
                Application.Exit();
            }
            else this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.CommonKeyMode == true)
            {
                if (tbInput.Text == "45e")
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    tbInput.Focus();
                    tbInput.SelectAll();
                }
            }
            else
            {
                if (tbInput.Text.Length > 0)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    tbInput.Focus();
                    tbInput.SelectAll();
                }
            }
        }
    }
}
