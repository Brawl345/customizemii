using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CustomizeMii
{
    public partial class CustomizeMii_InputBox : Form
    {
        public string Input
        {
            get { return tbInput.Text; }
        }

        public CustomizeMii_InputBox()
        {
            InitializeComponent();
        }

        private void CustomizeMii_InputBox_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnOK_Click(object sender, EventArgs e)
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
    }
}
