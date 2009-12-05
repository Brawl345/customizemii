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
 
using ForwardMii;

namespace CustomizeMii
{
    namespace Forwarder
    {
        //Let's use this kind of wrapper, so CustomizeMii can still run, if the ForwardMii.dll is missing...

        internal class Complex
        {
            private string thisAppFolder;
            private bool elfFirst = false;
            private bool usbFirst = false;
            private string image43;
            private string image169;
            public string AppFolder { get { return thisAppFolder; } set { thisAppFolder = value; } }
            public bool ElfFirst { get { return elfFirst; } set { elfFirst = value; } }
            public bool UsbFirst { get { return usbFirst; } set { usbFirst = value; } }
            public string Image43 { get { return image43; } set { image43 = value; } }
            public string Image169 { get { return image169; } set { image169 = value; } }

            public Complex()
            {

            }

            public void Save(string Destination)
            {
                GXForwarder Forwarder = new GXForwarder(thisAppFolder, usbFirst, elfFirst, image43, image169);
                Forwarder.Save(Destination, true);
            }

            public void Clear()
            {
                thisAppFolder = string.Empty;
                elfFirst = false;
                usbFirst = false;
                image43 = string.Empty;
                image169 = string.Empty;
            }
        }

        internal class Simple
        {
            private string thisAppFolder;
            private bool toElf = false;
            public string AppFolder { get { return thisAppFolder; } set { thisAppFolder = value; } }
            public bool ForwardToElf { get { return toElf; } set { toElf = value; } }

            public Simple()
            {

            }

            public void Save(string Destination)
            {
                SDSDHC_Forwarder Forwarder = new SDSDHC_Forwarder(thisAppFolder, toElf);
                Forwarder.Save(Destination, true);
            }

            public void Clear()
            {
                thisAppFolder = string.Empty;
                toElf = false;
            }
        }
    }
}
