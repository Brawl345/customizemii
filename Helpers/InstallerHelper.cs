using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;

namespace Helpers
{
    public class InstallerHelper
    {
        public static MemoryStream CreateInstaller(string wadFile, byte iosToUse)
        {
            const int injectionPosition = 0x5A78C;

            //1. Open the stub installer from resources
            MemoryStream compressedStubInstallerStream = LoadCompressedStubInstaller("CustomizeMiiInstaller.dol.z");
            compressedStubInstallerStream.Seek(0, SeekOrigin.Begin);

            //2. Decompress compressed installer
            MemoryStream uncompressedStubInstallerStream = new MemoryStream();

            using (GZipStream gzipStream = new GZipStream(compressedStubInstallerStream, CompressionMode.Decompress)) 
            {
                byte[] decompressedBuff = new byte[1024];
                while (true) 
                {
                    int length = gzipStream.Read(decompressedBuff, 0, 1024);

                    if (length == 0) break;

                    uncompressedStubInstallerStream.Write(decompressedBuff, 0, length);
                }

            }            

            //3. Take SHA of the wad and store it in the stub installer along with the size of the wad

            byte[] wadFileBytes = File.ReadAllBytes(wadFile);
            uint wadLength = (uint) wadFileBytes.Length;

            byte[] shaHash;
            using (SHA1 shaGen = SHA1.Create())
            {
                shaHash = shaGen.ComputeHash(wadFileBytes);
            }

            //4. Inject the data into the installer

            //Write out the wad size
            uncompressedStubInstallerStream.Seek(injectionPosition, SeekOrigin.Begin);            
            uncompressedStubInstallerStream.WriteByte((byte)((wadLength >> 24) & 0xff));
            uncompressedStubInstallerStream.WriteByte((byte)((wadLength >> 16) & 0xff));
            uncompressedStubInstallerStream.WriteByte((byte)((wadLength >> 8) & 0xff));
            uncompressedStubInstallerStream.WriteByte((byte)(wadLength  & 0xff));

            //Write out the SHA1 value (Against corruption of the file on the network, this value will be checked by the installer)
            uncompressedStubInstallerStream.Write(shaHash, 0, 20);

            //Write out the ios to be used... 
            uncompressedStubInstallerStream.WriteByte(iosToUse);

            //pad it with three zeroes (to align it into 32-bit)
            uncompressedStubInstallerStream.WriteByte(0); uncompressedStubInstallerStream.WriteByte(0); uncompressedStubInstallerStream.WriteByte(0);


            //Write out to be installed wad file's contents...
            uncompressedStubInstallerStream.Write(wadFileBytes, 0, (int)wadLength);

            return uncompressedStubInstallerStream;
        }

        private static MemoryStream LoadCompressedStubInstaller(string installerResourceName)
        {
            using (BinaryReader resLoader = new BinaryReader(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Helpers.Resources." + installerResourceName)))
            {
                MemoryStream ms = new MemoryStream();
                byte[] temp = resLoader.ReadBytes((int)resLoader.BaseStream.Length);
                ms.Write(temp, 0, temp.Length);
                return ms;
            }
        }
    }
}
