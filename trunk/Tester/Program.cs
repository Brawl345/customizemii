using System;
using System.Collections.Generic;
using System.Text;
using Helpers;
using System.IO;
using System.IO.Compression;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            string command = args[0];
            
            if (command == "CSTUB")
            {
                Console.Out.WriteLine("Stub Compression>>");
                CompressStub(args[1], args[2]);
            }
            else if (command == "DSTUB")
            {
                Console.Out.WriteLine("Stub Decompression>>");
                DecompressStub(args[1], args[2]);
            }
            else if (command == "LOADC")
            {
                Console.Out.WriteLine("Installer creation>>");
                CreateInstaller(args[1], args[2]);
            }
        }

        static void CompressStub(string installerDol, string zippedResourceFileName)
        {
            using (FileStream fs = new FileStream(installerDol, FileMode.Open))
            {
                using (FileStream fsOut = new FileStream(zippedResourceFileName, FileMode.Create)) 
                {
                    using (GZipStream gzip = new GZipStream(fsOut, CompressionMode.Compress))
                    {
                        // Copy the source file into the compression stream.
                        byte[] buffer = new byte[4096];
                        int numRead;
                        while ((numRead = fs.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            gzip.Write(buffer, 0, numRead);
                        }
                    }
                }
            }
        }

        public static void DecompressStub(string compressedInstallerDol, string decompressedInstallerDol)
        {
            // Get the stream of the source file.
            using (FileStream inFile = new FileStream(compressedInstallerDol, FileMode.Open))
            {
                //Create the decompressed file.
                using (FileStream outFile = File.Create(decompressedInstallerDol))
                {
                    using (GZipStream Decompress = new GZipStream(inFile,CompressionMode.Decompress))
                    {
                        //Copy the decompression stream into the output file.
                        byte[] buffer = new byte[4096];
                        int numRead;
                        while ((numRead = Decompress.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            outFile.Write(buffer, 0, numRead);
                        }
                    }
                }
            }
        }

        static void CreateInstaller(string inWadFilename, string outDolFileName)
        {
            using (MemoryStream ms = InstallerHelper.CreateInstaller(inWadFilename, 249))
            {
                using (FileStream fs = new FileStream(outDolFileName, FileMode.Create))
                {
                    ms.WriteTo(fs);
                    fs.Close();
                }
            }
        }

    }
}
