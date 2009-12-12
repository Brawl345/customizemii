/* This file is part of TransmitMii
 * Copyright (C) 2009 Leathl
 * 
 * TransmitMii is free software: you can redistribute it and/or
 * modify it under the terms of the GNU General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * TransmitMii is distributed in the hope that it will be
 * useful, but WITHOUT ANY WARRANTY; without even the implied warranty
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Net.Sockets;

namespace TransmitMii
{
    partial class TransmitMii_Main
    {
        const bool JODI_Compress = true;
        const bool HAXX_Compress = false;
        const bool USBX_Compress = false;

        TcpClient theClient;
        NetworkStream theStream;
        System.Diagnostics.Stopwatch stopper = new System.Diagnostics.Stopwatch();
        private double transmittedLength;
        private int timeElapsed;
        private bool usedCompression;
        private double compressionRatio;

        private enum Protocol : int
        {
            // 0 = No Compression
            // 1 = Compression
            JODI = 1,
            HAXX = 0,
            USBX = 0
        }

        private Protocol IntToProtocol(int theInt)
        {
            switch (theInt)
            {
                default:
                    return Protocol.JODI;
                case 1:
                    return Protocol.HAXX;
                case 2:
                    return Protocol.USBX;
            }
        }

        private bool Transmit_Compress(string fileName, byte[] fileData, Protocol protocol, bool compress)
        {
            stopper.Reset(); stopper.Start();

            if (!Environment.OSVersion.ToString().Contains("Windows"))
                compress = false;

            if ((int)(protocol) == 0) compress = false;

            theClient = new TcpClient();

            byte[] compFileData;
            int Blocksize = 4 * 1024;
            if (protocol != Protocol.JODI) Blocksize = 16 * 1024;
            byte[] buffer = new byte[4];
            string theIP = tbIP.Text;

            StatusUpdate("Connecting...");
            try { theClient.Connect(theIP, 4299); }
            catch (Exception ex) { if (!Aborted) ErrorBox("Connection Failed:\n" + ex.Message); theClient.Close(); return false; }
            theStream = theClient.GetStream();

            StatusUpdate("Connected... Sending Magic...");
            buffer[0] = (byte)'H';
            buffer[1] = (byte)'A';
            buffer[2] = (byte)'X';
            buffer[3] = (byte)'X';
            try { theStream.Write(buffer, 0, 4); }
            catch (Exception ex) { if (!Aborted) ErrorBox("Error sending Magic:\n" + ex.Message); theStream.Close(); theClient.Close(); return false; }

            StatusUpdate("Magic Sent... Sending Version Info...");
            buffer[0] = 0;
            buffer[1] = protocol == Protocol.JODI ? (byte)5 : (byte)4;
            buffer[2] = (byte)(((fileName.Length + 2) >> 8) & 0xff);
            buffer[3] = (byte)((fileName.Length + 2) & 0xff);

            try { theStream.Write(buffer, 0, 4); }
            catch (Exception ex) { if (!Aborted) ErrorBox("Error sending Version Info:\n" + ex.Message); theStream.Close(); theClient.Close(); return false; }

            if (compress)
            {
                StatusUpdate("Version Info Sent... Compressing File...");
                try { compFileData = zlib.Compress(fileData); }
                catch
                {
                    //Compression failed, let's continue without compression
                    if (!Aborted) ErrorBox("a");
                    compFileData = fileData;
                    fileData = new byte[0];
                }

                StatusUpdate("Compressed File... Sending Filesize...");
            }
            else
            {
                compFileData = fileData;
                fileData = new byte[0];

                StatusUpdate("Version Info Sent... Sending Filesize...");
            }

            //First compressed filesize, then uncompressed filesize
            buffer[0] = (byte)((compFileData.Length >> 24) & 0xff);
            buffer[1] = (byte)((compFileData.Length >> 16) & 0xff);
            buffer[2] = (byte)((compFileData.Length >> 8) & 0xff);
            buffer[3] = (byte)(compFileData.Length & 0xff);
            try { theStream.Write(buffer, 0, 4); }
            catch (Exception ex) { if (!Aborted) ErrorBox("Error sending Filesize:\n" + ex.Message); theStream.Close(); theClient.Close(); return false; }

            if (compress)
            {
                buffer[0] = (byte)((fileData.Length >> 24) & 0xff);
                buffer[1] = (byte)((fileData.Length >> 16) & 0xff);
                buffer[2] = (byte)((fileData.Length >> 8) & 0xff);
                buffer[3] = (byte)(fileData.Length & 0xff);
                try { theStream.Write(buffer, 0, 4); }
                catch (Exception ex) { if (!Aborted) ErrorBox("Error sending Filesize:\n" + ex.Message); theStream.Close(); theClient.Close(); return false; }
            }

            StatusUpdate("Filesize Sent... Sending File...");
            int off = 0;
            int cur = 0;
            int count = compFileData.Length / Blocksize;
            int left = compFileData.Length % Blocksize;

            try
            {
                do
                {
                    StatusUpdate("Sending File: " + ((cur + 1) * 100 / count).ToString() + "%");
                    theStream.Write(compFileData, off, Blocksize);
                    off += Blocksize;
                    cur++;
                } while (cur < count);

                if (left > 0)
                {
                    theStream.Write(compFileData, off, compFileData.Length - off);
                }
            }
            catch (Exception ex) { if (!Aborted) ErrorBox("Error sending File:\n" + ex.Message); theStream.Close(); theClient.Close(); return false; }

            StatusUpdate("File Sent... Sending Arguments...");
            byte[] theArgs = new byte[fileName.Length + 2];
            for (int i = 0; i < fileName.Length; i++) { theArgs[i] = (byte)fileName.ToCharArray()[i]; }
            try { theStream.Write(theArgs, 0, theArgs.Length); }
            catch (Exception ex) { if (!Aborted) ErrorBox("Error sending Arguments:\n" + ex.Message); theStream.Close(); theClient.Close(); return false; }

            theStream.Close();
            theClient.Close();

            StatusUpdate(string.Empty);

            stopper.Stop();

            this.timeElapsed = (int)stopper.ElapsedMilliseconds;
            this.usedCompression = compress;
            this.transmittedLength = Math.Round(compFileData.Length * 0.0009765625, 2);
            if (compress && fileData.Length != 0)
                this.compressionRatio = (compFileData.Length * 100) / fileData.Length;

            return true;
        }

        //private bool Transmit(string fileName, byte[] fileData, bool _JODI)
        //{
        //    TcpClient theClient = new TcpClient();
        //    NetworkStream theStream;

        //    int Blocksize = 4 * 1024;
        //    if (!_JODI) Blocksize = 16 * 1024;
        //    byte[] buffer = new byte[4];
        //    string theIP = tbIP.Text;

        //    StatusUpdate("Connecting...");
        //    try { theClient.Connect(theIP, 4299); }
        //    catch (Exception ex) { if (!Aborted) ErrorBox("Connection Failed:\n" + ex.Message); theClient.Close(); return false; }
        //    theStream = theClient.GetStream();

        //    StatusUpdate("Connected... Sending Magic...");
        //    buffer[0] = (byte)'H';
        //    buffer[1] = (byte)'A';
        //    buffer[2] = (byte)'X';
        //    buffer[3] = (byte)'X';
        //    try { theStream.Write(buffer, 0, 4); }
        //    catch (Exception ex) { if (!Aborted) ErrorBox("Error sending Magic:\n" + ex.Message); theStream.Close(); theClient.Close(); return false; }

        //    StatusUpdate("Magic Sent... Sending Version Info...");
        //    if (_JODI)
        //    {
        //        buffer[0] = 0;
        //        buffer[1] = 5;
        //        buffer[2] = (byte)(((fileName.Length + 2) >> 8) & 0xff);
        //        buffer[3] = (byte)((fileName.Length + 2) & 0xff);
        //    }
        //    else
        //    {
        //        buffer[0] = 0;
        //        buffer[1] = 1;
        //        buffer[2] = 0;
        //        buffer[3] = 0;
        //    }
        //    try { theStream.Write(buffer, 0, 4); }
        //    catch (Exception ex) { if (!Aborted) ErrorBox("Error sending Version Info:\n" + ex.Message); theStream.Close(); theClient.Close(); return false; }

        //    StatusUpdate("Version Info Sent... Sending Filesize...");
        //    //First compressed filesize, then uncompressed filesize
        //    buffer[0] = (byte)((fileData.Length >> 24) & 0xff);
        //    buffer[1] = (byte)((fileData.Length >> 16) & 0xff);
        //    buffer[2] = (byte)((fileData.Length >> 8) & 0xff);
        //    buffer[3] = (byte)(fileData.Length & 0xff);
        //    try { theStream.Write(buffer, 0, 4); }
        //    catch (Exception ex) { if (!Aborted) ErrorBox("Error sending Filesize:\n" + ex.Message); theStream.Close(); theClient.Close(); return false; }

        //    if (_JODI)
        //    {
        //        buffer[0] = 0;
        //        buffer[1] = 0;
        //        buffer[2] = 0;
        //        buffer[3] = 0;
        //        try { theStream.Write(buffer, 0, 4); }
        //        catch (Exception ex) { if (!Aborted) ErrorBox("Error sending Filesize:\n" + ex.Message); theStream.Close(); theClient.Close(); return false; }
        //    }

        //    StatusUpdate("Filesize Sent... Sending File...");
        //    int off = 0;
        //    int cur = 0;
        //    int count = fileData.Length / Blocksize;
        //    int left = fileData.Length % Blocksize;

        //    try
        //    {
        //        do
        //        {
        //            StatusUpdate("Sending File: " + ((cur + 1) * 100 / count).ToString() + "%");
        //            theStream.Write(fileData, off, Blocksize);
        //            off += Blocksize;
        //            cur++;
        //        } while (cur < count);

        //        if (left > 0)
        //        {
        //            theStream.Write(fileData, off, fileData.Length - off);
        //        }
        //    }
        //    catch (Exception ex) { if (!Aborted) ErrorBox("Error sending File:\n" + ex.Message); theStream.Close(); theClient.Close(); return false; }

        //    if (_JODI)
        //    {
        //        StatusUpdate("File Sent... Sending Arguments...");
        //        byte[] theArgs = new byte[fileName.Length + 2];
        //        for (int i = 0; i < fileName.Length; i++) { theArgs[i] = (byte)fileName.ToCharArray()[i]; }
        //        try { theStream.Write(theArgs, 0, theArgs.Length); }
        //        catch (Exception ex) { if (!Aborted) ErrorBox("Error sending Arguments:\n" + ex.Message); theStream.Close(); theClient.Close(); return false; }
        //    }

        //    theStream.Close();
        //    theClient.Close();

        //    StatusUpdate(string.Empty);

        //    return true;
        //}
    }
}
