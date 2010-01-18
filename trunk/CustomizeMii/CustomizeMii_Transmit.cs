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

using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CustomizeMii
{
    public partial class CustomizeMii_Transmit : Form
    {
        public int Protocol { get { return cmbProtocol.SelectedIndex; } set { cmbProtocol.SelectedIndex = value; } }
        public string IPAddress { get { return tbIP.Text; } set { tbIP.Text = value; } }
        public string IOS { get { return tbIOS.Text; } set { tbIOS.Text = value; } }

        public CustomizeMii_Transmit()
        {
            InitializeComponent();
        }

        private void CustomizeMii_Transmit_Load(object sender, System.EventArgs e)
        {
            this.CenterToParent();

            try
            {
                cmbProtocol.SelectedIndex = Properties.Settings.Default.Protocol;
                tbIP.Text = Properties.Settings.Default.IP;
                tbIOS.Text = Properties.Settings.Default.IOS;
            }
            catch { }
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnTransmit_Click(object sender, System.EventArgs e)
        {
            string IpPattern = @"([01]?\d\d?|2[0-4]\d|25[0-5])\." +
             @"([01]?\d\d?|2[0-4]\d|25[0-5])\." +
             @"([01]?\d\d?|2[0-4]\d|25[0-5])\." +
             @"([01]?\d\d?|2[0-4]\d|25[0-5])";
            Regex IpAdress = new Regex(IpPattern);

            if (IpAdress.IsMatch(tbIP.Text))
            {
                int tmp = int.Parse(tbIOS.Text);
                if (tmp > 0 && tmp < 255)
                {
                    Properties.Settings.Default.Protocol = cmbProtocol.SelectedIndex;
                    Properties.Settings.Default.IP = tbIP.Text;
                    Properties.Settings.Default.IOS = tbIOS.Text;
                    Properties.Settings.Default.Save();

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    tbIOS.Focus();
                    tbIOS.SelectAll();
                }
            }
            else
            {
                tbIP.Focus();
                tbIP.SelectAll();
            }
        }

        private void tbIOS_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != '\b';
        }
    }
}
