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
            private string path1 = "SD:/apps/example/boot.dol";
            private string path2 = "SD:/apps/example/boot.elf";
            private string path3 = "USB:/apps/example/boot.dol";
            private string path4 = "USB:/apps/example/boot.elf";
            private string image43;
            private string image169;
            public string Path1 { get { return path1; } set { path1 = value; } }
            public string Path2 { get { return path2; } set { path2 = value; } }
            public string Path3 { get { return path3; } set { path3 = value; } }
            public string Path4 { get { return path4; } set { path4 = value; } }
            public string Image43 { get { return image43; } set { image43 = value; } }
            public string Image169 { get { return image169; } set { image169 = value; } }

            public Complex()
            {

            }

            public void Save(string Destination)
            {
                GXForwarder Forwarder = new GXForwarder(image43, image169, path1, path2, path3, path4);
                Forwarder.Save(Destination, true);
            }

            public void Clear()
            {
                path1 = "SD:/apps/example/boot.dol";
                path2 = "SD:/apps/example/boot.elf";
                path3 = "USB:/apps/example/boot.dol";
                path4 = "USB:/apps/example/boot.elf";
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
