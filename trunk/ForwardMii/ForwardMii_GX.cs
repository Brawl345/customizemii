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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Runtime.InteropServices;

namespace ForwardMii
{
    public class GXForwarder
    {
        private readonly string TempDir = Path.GetTempPath() + "ForwardMii_Temp\\" + Guid.NewGuid() + "\\";
        private string path1;
        private string path2;
        private string path3;
        private string path4;
        private string image43;
        private string image169;
        public string Path1 { get { return path1; } set { path1 = value; } }
        public string Path2 { get { return path2; } set { path2 = value; } }
        public string Path3 { get { return path3; } set { path3 = value; } }
        public string Path4 { get { return path4; } set { path4 = value; } }
        public string Image43 { get { return image43; } set { image43 = value; } }
        public string Image169 { get { return image169; } set { image169 = value; } }

        public GXForwarder()
        {

        }

        public GXForwarder(params string[] paths)
        {
            if (ForwardMii_Plugin.CheckDevKit() == false) throw new Exception("DevkitPro or one of it's components wasn't found!");
            path1 = paths[0];
            path2 = paths[1];
            path3 = paths[2];
            path4 = paths[3];
        }

        public GXForwarder(string Image43, string Image169, params string[] paths)
        {
            if (ForwardMii_Plugin.CheckDevKit() == false) throw new Exception("DevkitPro or one of it's components wasn't found!");
            path1 = paths[0];
            path2 = paths[1];
            path3 = paths[2];
            path4 = paths[3];
            image43 = Image43;
            image169 = Image169;
        }

        public void Clear()
        {
            path1 = string.Empty;
            path2 = string.Empty;
            path3 = string.Empty;
            path4 = string.Empty;
            image43 = string.Empty;
            image169 = string.Empty;
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
            try
            {
                if (Directory.Exists(TempDir)) Directory.Delete(TempDir, true);
                CopyResources();
                EditMainCpp();
                CopyImages();
                if (Compile() == false) throw new Exception("An error occured during compiling!");
                byte[] fileTemp = File.ReadAllBytes(TempDir + "boot.dol");

                try { Directory.Delete(TempDir, true); }
                catch { }

                return new MemoryStream(fileTemp);
            }
            catch (Exception ex)
            {
                if (Directory.Exists(TempDir)) Directory.Delete(TempDir, true);
                throw new Exception(ex.Message);
            }
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern uint GetShortPathName([MarshalAs(UnmanagedType.LPTStr)] string lpszLongPath,
        [MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpszShortPath, uint cchBuffer);

        public static string GetShortPathName(string longPath)
        {
            uint size = 256;
            StringBuilder buffer = new StringBuilder((int)size);
            uint result = GetShortPathName(longPath, buffer, size);
            return buffer.ToString();
        }

        private bool Compile()
        {
            try
            {
                string tempDir = TempDir;
                if (tempDir.Contains(" ")) { tempDir = GetShortPathName(tempDir); }

                ProcessStartInfo makeI = new ProcessStartInfo("make", "-C " + tempDir);
                makeI.UseShellExecute = false;
                makeI.CreateNoWindow = true;
                
                Process make = Process.Start(makeI);
                make.WaitForExit();
                make.Close();

                if (File.Exists(TempDir + "boot.dol") && File.Exists(TempDir + "boot.elf"))
                {
                    return true;
                }
                else return false;
            }
            catch { return false; }
        }

        private void CopyImages()
        {
            if (!string.IsNullOrEmpty(image43) && File.Exists(image43))
            {
                try
                {
                    Image img = Image.FromFile(image43);
                    if (img.Width != 640 || img.Height != 480)
                        img = ResizeImage(img, 640, 480);
                    img.Save(TempDir + "\\source\\images\\background.png", System.Drawing.Imaging.ImageFormat.Png);
                }
                catch
                {
                    Bitmap img = new Bitmap(640, 480);
                    img.Save(TempDir + "\\source\\images\\background.png", System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            else
            {
                Bitmap img = new Bitmap(640, 480);
                img.Save(TempDir + "\\source\\images\\background.png", System.Drawing.Imaging.ImageFormat.Png);
            }

            if (!string.IsNullOrEmpty(image169) && File.Exists(image169))
            {
                try
                {
                    Image img = Image.FromFile(image169);
                    if (img.Width != 640 || img.Height != 480)
                        img = ResizeImage(img, 640, 480);
                    img.Save(TempDir + "\\source\\images\\background169.png", System.Drawing.Imaging.ImageFormat.Png);
                }
                catch
                {
                    Bitmap img = new Bitmap(640, 480);
                    img.Save(TempDir + "\\source\\images\\background169.png", System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            else
            {
                Bitmap img = new Bitmap(640, 480);
                img.Save(TempDir + "\\source\\images\\background169.png", System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        private Image ResizeImage(Image img, int x, int y)
        {
            Image newimage = new Bitmap(x, y);
            using (Graphics gfx = Graphics.FromImage(newimage))
            {
                gfx.DrawImage(img, 0, 0, x, y);
            }
            return newimage;
        }

        private void EditMainCpp()
        {
            Stream maincpp = GetReourceStream("main.cpp");
            StreamReader reader = new StreamReader(maincpp);
            List<string> tempLines = new List<string>();
            string tempLine;

            while ((tempLine = reader.ReadLine()) != null)
            {
                tempLines.Add(tempLine);
            }

            string[] lines = tempLines.ToArray();

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("---path1---"))
                    lines[i] = lines[i].Replace("---path1---", path1);
                else if (lines[i].Contains("---path2---"))
                    lines[i] = lines[i].Replace("---path2---", path2);
                else if (lines[i].Contains("---path3---"))
                    lines[i] = lines[i].Replace("---path3---", path3);
                else if (lines[i].Contains("---path4---"))
                    lines[i] = lines[i].Replace("---path4---", path4);
            }

            using (FileStream fs = new FileStream(TempDir + "source\\main.cpp", FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    foreach (string thisLine in lines)
                    {
                        writer.WriteLine(thisLine);
                    }
                }
            }
        }

        private void CopyResources()
        {
            if (Directory.Exists(TempDir)) Directory.Delete(TempDir, true);
            Directory.CreateDirectory(TempDir + "source\\images");
            string[] Resources = new string[] { "dolloader.c", "dolloader.h", "elf_abi.h", "elfloader.c",
                "elfloader.h", "fatmounter.c", "fatmounter.h", "filelist.h", "video.cpp", "video.h" };

            foreach (string thisResource in Resources)
            {
                Stream tempStream = GetReourceStream(thisResource);
                StreamToFile(tempStream, TempDir + "source\\" + thisResource);
            }

            Stream makefile = GetReourceStream("Makefile");
            StreamToFile(makefile, TempDir + "Makefile");
        }

        private void StreamToFile(Stream theStream, string Destination)
        {
            using (FileStream fs = new FileStream(Destination, FileMode.Create))
            {
                byte[] temp = new byte[theStream.Length];
                theStream.Read(temp, 0, temp.Length);
                fs.Write(temp, 0, temp.Length);

                //int count = 0;
                //int length = 4096;
                //byte[] buffer = new byte[length];

                //while ((count = theStream.Read(buffer, 0, length)) != 0)
                //    fs.Write(buffer, 0, length);
            }

        }

        private MemoryStream GetReourceStream(string theResource)
        {
            Stream thisStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ForwardMii.Resources.GX." + theResource);
            byte[] thisArray = new byte[thisStream.Length];
            thisStream.Read(thisArray, 0, thisArray.Length);
            return new MemoryStream(thisArray);
        }
    }
}
