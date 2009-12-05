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
using System.IO;
using System.Reflection;

namespace ForwardMii
{
    public class SDSDHC_Forwarder
    {
        private string thisAppFolder;
        private bool toElf = false;
        public string AppFolder { get { return thisAppFolder; } set { thisAppFolder = value; } }
        public bool ForwardToElf { get { return toElf; } set { toElf = value; } }

        public SDSDHC_Forwarder()
        {

        }

        public SDSDHC_Forwarder(string AppFolder)
        {
            if (AppFolder.Length > 18 || AppFolder.Length < 3) throw new Exception("Application folder must be 3 - 18 chars!");
            thisAppFolder = AppFolder;
        }

        public SDSDHC_Forwarder(string AppFolder, bool ForwardToElf)
        {
            if (AppFolder.Length > 18 || AppFolder.Length < 3) throw new Exception("Application folder must be 3 - 18 chars!");
            thisAppFolder = AppFolder;
            toElf = ForwardToElf;
        }

        public void Clear()
        {
            thisAppFolder = string.Empty;
            toElf = false;
        }

        public void Save(string Destionation)
        {
            Save(Destionation, false);
        }

        public void Save(string Destination, bool Overwrite)
        {
            byte[] theForwarder = ToByteArray();

            if (Path.HasExtension(Destination) == false) Destination += ".dol";

            if (File.Exists(Destination))
            {
                if (Overwrite == true)
                    File.Delete(Destination);
                else
                    throw new Exception("The destination file already exists!");
            }

            using (FileStream fs = new FileStream(Destination, FileMode.Create))
            {
                fs.Write(theForwarder, 0, theForwarder.Length);
            }
        }

        public byte[] ToByteArray()
        {
            MemoryStream ms = this.ToMemoryStream();
            return ms.ToArray();
        }

        public MemoryStream ToMemoryStream()
        {
            MemoryStream BaseStream = GetBase();
            MemoryStream TailStream = GetTail();

            BaseStream = ConvertBase(BaseStream);
            TailStream = EditTailFolder(TailStream);

            byte[] BaseArray = BaseStream.ToArray();
            byte[] TailArray = TailStream.ToArray();

            MemoryStream Result = new MemoryStream();
            Result.Seek(0, SeekOrigin.End);
            Result.Write(BaseArray, 0, BaseArray.Length);
            Result.Write(TailArray, 0, TailArray.Length);

            return Result;
        }

        private MemoryStream EditTailFolder(MemoryStream TailStream)
        {
            char[] FolderChars = thisAppFolder.ToCharArray();
            int[] Offsets = GetOffsets(FolderChars.Length);

            TailStream.Seek(1, SeekOrigin.Begin);
            foreach (char thisChar in FolderChars)
                TailStream.WriteByte((byte)thisChar);

            TailStream.Seek(Offsets[0], SeekOrigin.Begin);
            foreach (char thisChar in FolderChars)
                TailStream.WriteByte((byte)thisChar);

            TailStream.Seek(Offsets[1], SeekOrigin.Begin);
            foreach (char thisChar in FolderChars)
                TailStream.WriteByte((byte)thisChar);

            if (toElf == true)
                TailStream = EditTailToElf(TailStream);

            return TailStream;
        }

        private MemoryStream EditTailToElf(MemoryStream TailStream)
        {
            int[] Offsets = GetDolOffsets(thisAppFolder.Length);
            byte[] Elf = new byte[] { 0x65, 0x6c, 0x66 };

            foreach (int thisOffset in Offsets)
            {
                TailStream.Seek(thisOffset, SeekOrigin.Begin);
                TailStream.Write(Elf, 0, Elf.Length);
            }

            return TailStream;
        }

        private int[] GetDolOffsets(int CharCount)
        {
            int[] Offsets = new int[5];

            switch (CharCount)
            {
                case 3:
                    Offsets[0] = 10;
                    Offsets[1] = 25;
                    Offsets[2] = 87;
                    Offsets[3] = 336;
                    Offsets[4] = 344;
                    break;
                case 4:
                    Offsets[0] = 11;
                    Offsets[1] = 25;
                    Offsets[2] = 87;
                    Offsets[3] = 336;
                    Offsets[4] = 344;
                    break;
                case 5:
                    Offsets[0] = 12;
                    Offsets[1] = 25;
                    Offsets[2] = 91;
                    Offsets[3] = 340;
                    Offsets[4] = 348;
                    break;
                case 6:
                    Offsets[0] = 13;
                    Offsets[1] = 29;
                    Offsets[2] = 95;
                    Offsets[3] = 348;
                    Offsets[4] = 356;
                    break;
                case 7:
                    Offsets[0] = 14;
                    Offsets[1] = 29;
                    Offsets[2] = 95;
                    Offsets[3] = 348;
                    Offsets[4] = 356;
                    break;
                case 8:
                    Offsets[0] = 15;
                    Offsets[1] = 29;
                    Offsets[2] = 95;
                    Offsets[3] = 348;
                    Offsets[4] = 356;
                    break;
                case 9:
                    Offsets[0] = 16;
                    Offsets[1] = 29;
                    Offsets[2] = 99;
                    Offsets[3] = 352;
                    Offsets[4] = 360;
                    break;
                case 10:
                    Offsets[0] = 17;
                    Offsets[1] = 33;
                    Offsets[2] = 103;
                    Offsets[3] = 360;
                    Offsets[4] = 368;
                    break;
                case 11:
                    Offsets[0] = 18;
                    Offsets[1] = 33;
                    Offsets[2] = 103;
                    Offsets[3] = 360;
                    Offsets[4] = 368;
                    break;
                case 12:
                    Offsets[0] = 19;
                    Offsets[1] = 33;
                    Offsets[2] = 103;
                    Offsets[3] = 360;
                    Offsets[4] = 368;
                    break;
                case 13:
                    Offsets[0] = 20;
                    Offsets[1] = 33;
                    Offsets[2] = 107;
                    Offsets[3] = 364;
                    Offsets[4] = 372;
                    break;
                case 14:
                    Offsets[0] = 21;
                    Offsets[1] = 37;
                    Offsets[2] = 111;
                    Offsets[3] = 372;
                    Offsets[4] = 380;
                    break;
                case 15:
                    Offsets[0] = 22;
                    Offsets[1] = 37;
                    Offsets[2] = 11;
                    Offsets[3] = 372;
                    Offsets[4] = 380;
                    break;
                case 16:
                    Offsets[0] = 23;
                    Offsets[1] = 37;
                    Offsets[2] = 111;
                    Offsets[3] = 372;
                    Offsets[4] = 380;
                    break;
                case 17:
                    Offsets[0] = 24;
                    Offsets[1] = 37;
                    Offsets[2] = 115;
                    Offsets[3] = 376;
                    Offsets[4] = 384;
                    break;
                case 18:
                    Offsets[0] = 25;
                    Offsets[1] = 41;
                    Offsets[2] = 119;
                    Offsets[3] = 384;
                    Offsets[4] = 392;
                    break;
                default:
                    throw new Exception();
            }

            return Offsets;
        }

        private int[] GetOffsets(int CharCount)
        {
            int First = 0;
            int Second = 0;

            switch (CharCount)
            {
                case 3:
                case 4:
                    First = 57;
                    Second = 100;
                    break;
                case 5:
                    First = 57;
                    Second = 104;
                    break;
                case 6:
                case 7:
                case 8:
                    First = 61;
                    Second = 108;
                    break;
                case 9:
                    First = 61;
                    Second = 112;
                    break;
                case 10:
                case 11:
                case 12:
                case 13:
                    First = 65;
                    Second = 120;
                    break;
                case 14:
                case 15:
                case 16:
                    First = 69;
                    Second = 124;
                    break;
                case 17:
                    First = 69;
                    Second = 128;
                    break;
                case 18:
                    First = 73;
                    Second = 132;
                    break;
                default:
                    throw new Exception();
            }

            return new int[] { First, Second };
        }

        private MemoryStream GetBase()
        {
            switch (thisAppFolder.Length)
            {
                case 3:
                case 4:
                case 5:
                    return GetReourceStream("ForwardMii_Plugin.Resources.SDSDHC.3CharsBase.bin");
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                    return GetReourceStream("ForwardMii_Plugin.Resources.SDSDHC.6CharsBase.bin");
                case 17:
                case 18:
                    return GetReourceStream("ForwardMii_Plugin.Resources.SDSDHC.17CharsBase.bin");
                default:
                    throw new Exception();
            }
        }

        private MemoryStream GetTail()
        {
            switch (thisAppFolder.Length)
            {
                case 3:
                    return GetReourceStream("ForwardMii_Plugin.Resources.SDSDHC.3CharsTail.bin");
                case 4:
                    return GetReourceStream("ForwardMii_Plugin.Resources.SDSDHC.4CharsTail.bin");
                case 5:
                    return GetReourceStream("ForwardMii_Plugin.Resources.SDSDHC.5CharsTail.bin");
                case 6:
                    return GetReourceStream("ForwardMii_Plugin.Resources.SDSDHC.6CharsTail.bin");
                case 7:
                    return GetReourceStream("ForwardMii_Plugin.Resources.SDSDHC.7CharsTail.bin");
                case 8:
                    return GetReourceStream("ForwardMii_Plugin.Resources.SDSDHC.8CharsTail.bin");
                case 9:
                    return GetReourceStream("ForwardMii_Plugin.Resources.SDSDHC.9CharsTail.bin");
                case 10:
                    return GetReourceStream("ForwardMii_Plugin.Resources.SDSDHC.10CharsTail.bin");
                case 11:
                    return GetReourceStream("ForwardMii_Plugin.Resources.SDSDHC.11CharsTail.bin");
                case 12:
                    return GetReourceStream("ForwardMii_Plugin.Resources.SDSDHC.12CharsTail.bin");
                case 13:
                    return GetReourceStream("ForwardMii_Plugin.Resources.SDSDHC.13CharsTail.bin");
                case 14:
                    return GetReourceStream("ForwardMii_Plugin.Resources.SDSDHC.14CharsTail.bin");
                case 15:
                    return GetReourceStream("ForwardMii_Plugin.Resources.SDSDHC.15CharsTail.bin");
                case 16:
                    return GetReourceStream("ForwardMii_Plugin.Resources.SDSDHC.16CharsTail.bin");
                case 17:
                    return GetReourceStream("ForwardMii_Plugin.Resources.SDSDHC.17CharsTail.bin");
                case 18:
                    return GetReourceStream("ForwardMii_Plugin.Resources.SDSDHC.18CharsTail.bin");
                default:
                    throw new Exception();
            }
        }

        private MemoryStream ConvertBase(MemoryStream BaseStream)
        {
            switch (thisAppFolder.Length)
            {
                case 3:
                    return BaseStream;
                case 4:
                    return BaseStream;
                case 5:
                    return SDSDHC_ConvertBaseStream.ConvertTo5Chars(BaseStream);
                case 6:
                    return BaseStream;
                case 7:
                    return BaseStream;
                case 8:
                    return BaseStream;
                case 9:
                    return SDSDHC_ConvertBaseStream.ConvertTo9Chars(BaseStream);
                case 10:
                    return SDSDHC_ConvertBaseStream.ConvertTo10Chars(BaseStream);
                case 11:
                    return SDSDHC_ConvertBaseStream.ConvertTo10Chars(BaseStream);
                case 12:
                    return SDSDHC_ConvertBaseStream.ConvertTo10Chars(BaseStream);
                case 13:
                    return SDSDHC_ConvertBaseStream.ConvertTo13Chars(BaseStream);
                case 14:
                    return SDSDHC_ConvertBaseStream.ConvertTo14Chars(BaseStream);
                case 15:
                    return SDSDHC_ConvertBaseStream.ConvertTo14Chars(BaseStream);
                case 16:
                    return SDSDHC_ConvertBaseStream.ConvertTo14Chars(BaseStream);
                case 17:
                    return BaseStream;
                case 18:
                    return SDSDHC_ConvertBaseStream.ConvertTo18Chars(BaseStream);
                default:
                    throw new Exception();
            }
        }

        private MemoryStream GetReourceStream(string theResource)
        {
            Stream thisStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(theResource);
            byte[] thisArray = new byte[thisStream.Length];
            thisStream.Read(thisArray, 0, thisArray.Length);
            return new MemoryStream(thisArray);
        }
    }
}
