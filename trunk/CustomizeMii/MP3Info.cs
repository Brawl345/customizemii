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
using System.Collections;
using System.IO;

class MP3Info
{
    //Private Variables
    private int fileSize;
    private int headerPos;
    private BitArray bitHeader;
    private MpegVersion mpegVersion;
    private MpegLayer mpegLayer;
    private int bitrate;
    private int frequency;
    private bool paddingBit;
    private int frameLength;
    private int frameCount;
    private double secLength;
    private int waveSamples;

    //Public Variables
    public int FileSize { get { return fileSize; } }
    public MpegVersion MPEGVersion { get { return mpegVersion; } }
    public MpegLayer MPEGLayer { get { return mpegLayer; } }
    public int Bitrate { get { return bitrate; } }
    public int Frequency { get { return frequency; } }
    public int FrameLength { get { return frameLength; } }
    public int FrameCount { get { return frameCount; } }
    public double Seconds { get { return secLength; } }
    public int AudioSamples { get { return waveSamples; } }

    //Public Functions
    public MP3Info(string mp3File)
    {
        FileInfo fi = new FileInfo(mp3File);
        this.fileSize = (int)fi.Length;

        FileStream fs = fi.OpenRead();
        fs.Seek(0, SeekOrigin.Begin);

        this.headerPos = SearchHeader(fs);

        byte[] header = new byte[4];
        fs.Seek(this.headerPos, SeekOrigin.Begin);
        fs.Read(header, 0, header.Length);
        fs.Close();

        GetHeaderBits(header);
        LoadHeader();
    }

    public MP3Info(byte[] mp3Header)
    {
        GetHeaderBits(mp3Header);
        LoadHeader();
    }

    //Private Functions
    private void ShowHeader()
    {
        //For debugging
        if (this.bitHeader != null)
        {
            string strBits = string.Empty;
            foreach (bool x in this.bitHeader)
                if (x) strBits += "1"; else strBits += "0";

            for (int i = strBits.Length - 8; i >= 0; i -= 8)
                strBits = strBits.Insert(i, " ");

            System.Windows.Forms.MessageBox.Show(strBits);
        }
    }

    private int SearchHeader(Stream fileStream)
    {
        int tmp;

        while ((tmp = fileStream.ReadByte()) != -1)
        {
            if (tmp == 255)
            {
                tmp = fileStream.ReadByte();
                BitArray tmpBits = new BitArray(new int[] { tmp });
                if (tmpBits[7] && tmpBits[6] && tmpBits[5]) return (int)(fileStream.Position - 2);
            }
        }

        throw new Exception("Frame Header couldn't be found!");
    }

    private void GetHeaderBits(byte[] fourBytes)
    {
        BitArray tmp = new BitArray(fourBytes);
        this.bitHeader = new BitArray(4 * 8);
        int count = 0;

        for (int i = 0; i < 32; i += 8)
            for (int j = 7; j >= 0; j--)
                this.bitHeader[count++] = tmp[i + j];
    }

    private void LoadHeader()
    {
        GetVersion(this.bitHeader[11], this.bitHeader[12]); 
        if (this.mpegVersion == MpegVersion.Reserved) throw new NotSupportedException("Unsupported MPEG Version!");
        GetLayer(this.bitHeader[13], this.bitHeader[14]);
        if (this.mpegLayer == MpegLayer.Reserved) throw new NotSupportedException("Unsupported MPEG Layer!");
        GetBitrate(this.bitHeader[16], this.bitHeader[17], this.bitHeader[18], this.bitHeader[19]);
        if (this.bitrate == 0) throw new NotSupportedException("Unsupported Bitrate!");
        GetFrequency(this.bitHeader[20], this.bitHeader[21]);
        if (this.frequency == -1) throw new NotSupportedException("Unsupported Frequency");
        this.paddingBit = this.bitHeader[22];

        GetFrameLength();
        GetFrameCount();
        GetSeconds();
        GetWaveSamples();
    }

    private void GetVersion(params bool[] twoBits)
    {
        if (twoBits[0] && twoBits[1]) this.mpegVersion = MpegVersion.MpegVersion1;
        else if (twoBits[0] && !twoBits[1]) this.mpegVersion = MpegVersion.MpegVersion2;
        else if (!twoBits[0] && twoBits[1]) this.mpegVersion = MpegVersion.Reserved;
        else if (!twoBits[0] && !twoBits[1]) this.mpegVersion = MpegVersion.MpegVersion25;
    }

    private void GetLayer(params bool[] twoBits)
    {
        if (twoBits[0] && twoBits[1]) this.mpegLayer = MpegLayer.I;
        else if (twoBits[0] && !twoBits[1]) this.mpegLayer = MpegLayer.II;
        else if (!twoBits[0] && twoBits[1]) this.mpegLayer = MpegLayer.III;
        else if (!twoBits[0] && !twoBits[1]) this.mpegLayer = MpegLayer.Reserved;
    }

    private void GetBitrate(params bool[] fourBits)
    {
        int[] bitrateTable = GetBitrateTable();

        Array.Reverse(fourBits);
        BitArray bArray = new BitArray(fourBits);
        byte[] index = new byte[1];
        bArray.CopyTo(index, 0);

        this.bitrate = bitrateTable[index[0]];
    }

    private int[] GetBitrateTable()
    {
        if (this.mpegVersion == MpegVersion.MpegVersion1)
        {
            if (this.mpegLayer == MpegLayer.I) return new int[] { 0, 32, 64, 96, 128, 160, 192, 224, 256, 288, 320, 352, 384, 416, 448, 0 };
            if (this.mpegLayer == MpegLayer.II) return new int[] { 0, 32, 48, 56, 64, 80, 96, 112, 128, 160, 192, 224, 256, 320, 384, 0 };
            if (this.mpegLayer == MpegLayer.III) return new int[] { 0, 32, 40, 48, 56, 64, 80, 96, 112, 128, 160, 192, 224, 256, 320, 0 };
        }
        else
        {
            if (this.mpegLayer == MpegLayer.I) return new int[] { 0, 32, 48, 56, 64, 80, 96, 112, 128, 144, 160, 176, 192, 224, 256, 0 };
            if (this.mpegLayer == MpegLayer.II ||
                this.mpegLayer == MpegLayer.III) return new int[] { 0, 8, 16, 24, 32, 40, 48, 56, 64, 80, 96, 112, 128, 144, 160, 0 };
        }

        throw new NotSupportedException("Unsupported MPEG Version or Layer!");
    }

    private void GetFrequency(params bool[] twoBits)
    {
        int[] frequencyTable = GetFrequencyTable();

        Array.Reverse(twoBits);
        BitArray bArray = new BitArray(twoBits);
        byte[] index = new byte[1];
        bArray.CopyTo(index, 0);

        this.frequency = frequencyTable[index[0]];
    }

    private int[] GetFrequencyTable()
    {
        if (this.mpegVersion == MpegVersion.MpegVersion1)
            return new int[] { 44100, 48000, 32000, -1 };
        else if (this.mpegVersion == MpegVersion.MpegVersion2)
            return new int[] { 22050, 24000, 16000, -1 };
        else if (this.mpegVersion == MpegVersion.MpegVersion25)
            return new int[] { 32000, 16000, 8000, -1 };
        else
            return new int[] { 0, 0, 0, -1 };
    }

    private void GetFrameLength()
    {
        double frmSize = (double)((this.mpegLayer == MpegLayer.I ? 12 : 144) * ((1000.0 * (float)this.bitrate / (float)this.frequency)));
        this.frameLength = (int)Math.Ceiling(frmSize);
    }

    private void GetFrameCount()
    {
        this.frameCount = (this.fileSize - this.headerPos) / this.frameLength;
    }

    private void GetSeconds()
    {
        this.secLength = ((this.fileSize - this.headerPos) * 8.00) / (1000.00 * this.bitrate);
    }

    private void GetWaveSamples()
    {
        this.waveSamples = this.frameCount * 1125;
    }

    //Structs & Enums
    public enum MpegVersion : int
    {
        Reserved = 0,
        MpegVersion1 = 1,
        MpegVersion25 = 2,
        MpegVersion2 = 3
    }

    public enum MpegLayer : int
    {
        Reserved = 0,
        I = 1,
        II = 2,
        III = 3
    }
}
