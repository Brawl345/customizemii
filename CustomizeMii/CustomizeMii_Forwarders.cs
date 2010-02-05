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
            private string[] paths = new string[16];
            private bool[] packs = new bool[3];
            private string image43;
            private string image169;
            public bool[] Packs { get { return packs; } set { packs = value; } }
            public string[] Paths { get { return paths; } set { paths = value; } }
            public string Image43 { get { return image43; } set { image43 = value; } }
            public string Image169 { get { return image169; } set { image169 = value; } }

            public Complex()
            {
                paths[0] = "SD:/apps/example/boot.dol";
                paths[1] = "SD:/apps/example/boot.elf";
                paths[2] = "USB:/apps/example/boot.dol";
                paths[3] = "USB:/apps/example/boot.elf";
            }

            public string GetPath(int pathNum)
            {
                return paths[pathNum];
            }

            public void SetPath(int pathNum, string forwardPath)
            {
                paths[pathNum] = forwardPath;
            }

            public void Save(string Destination)
            {
                GXForwarder Forwarder = new GXForwarder(image43, image169, paths);
                Forwarder.Save(Destination, true);
            }

            public void Clear()
            {
                paths[0] = "SD:/apps/example/boot.dol";
                paths[1] = "SD:/apps/example/boot.elf";
                paths[2] = "USB:/apps/example/boot.dol";
                paths[3] = "USB:/apps/example/boot.elf";

                packs[0] = false;
                packs[1] = false;
                packs[2] = false;

                for (int i = 4; i < paths.Length; i++)
                    paths[i] = string.Empty;

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
