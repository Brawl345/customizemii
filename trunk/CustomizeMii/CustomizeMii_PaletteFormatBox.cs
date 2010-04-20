using System.Windows.Forms;
using libWiiSharp;

namespace CustomizeMii
{
    public partial class CustomizeMii_PaletteFormatBox : Form
    {
        private TPL_PaletteFormat pFormat = TPL_PaletteFormat.RGB5A3;

        public TPL_PaletteFormat PaletteFormat { get { return pFormat; } }

        public CustomizeMii_PaletteFormatBox()
        {
            InitializeComponent();
        }

        private void btnIA8_Click(object sender, System.EventArgs e)
        {
            pFormat = TPL_PaletteFormat.IA8;
            this.Close();
        }

        private void btnRGB5A3_Click(object sender, System.EventArgs e)
        {
            pFormat = TPL_PaletteFormat.RGB5A3;
            this.Close();
        }

        private void btnRGB565_Click(object sender, System.EventArgs e)
        {
            pFormat = TPL_PaletteFormat.RGB565;
            this.Close();
        }

        private void CustomizeMii_PaletteFormatBox_Load(object sender, System.EventArgs e)
        {
            CenterToParent();
        }
    }
}
