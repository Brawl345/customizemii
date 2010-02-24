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

//These classes are based on bns.py (by megazig) of the Wii.py Framework with improvements by me (Leathl)
//Thanks to Xuzz, SquidMan, megazig, Matt_P, Omega and The Lemon Man, the authors of Wii.py!

using System;
using System.Collections.Generic;
using System.IO;
using WaveFile;

namespace BNS
{
    internal class BNS_Data
    {
        //Private Varialbes
        private byte[] magic = new byte[] { (byte)'D', (byte)'A', (byte)'T', (byte)'A' };
        private UInt32 size = 0x0004d000;
        private byte[] data;

        //Public Variables
        public UInt32 Size { get { return size; } set { size = value; } }
        public byte[] Data { get { return data; } set { data = value; } }

        public BNS_Data() { }

        public void Write(Stream outStream)
        {
            byte[] temp = BitConverter.GetBytes(size); Array.Reverse(temp);

            outStream.Write(magic, 0, magic.Length);
            outStream.Write(temp, 0, temp.Length);
            outStream.Write(data, 0, data.Length);
        }
    }

    internal class BNS_Info
    {
        //Private Variables
        private byte[] magic = new byte[] { (byte)'I', (byte)'N', (byte)'F', (byte)'O' };
        private UInt32 size = 0x000000a0;
        private byte codec = 0x00;
        private byte hasLoop = 0x00;
        private byte channelCount = 0x02;
        private byte zero = 0x00;
        private UInt16 sampleRate = 0xac44;
        private UInt16 pad0 = 0x0000;
        private UInt32 loopStart = 0x00000000;
        private UInt32 loopEnd = 0x00000000; //Or total sample count
        private UInt32 offsetToChannelStart = 0x00000018;
        private UInt32 pad1 = 0x00000000;
        private UInt32 channel1StartOffset = 0x00000020;
        private UInt32 channel2StartOffset = 0x0000002C;
        private UInt32 channel1Start = 0x00000000;
        private UInt32 coefficients1Offset = 0x0000038;
        private UInt32 pad2 = 0x00000000;
        private UInt32 channel2Start = 0x00000000;
        private UInt32 coefficients2Offset = 0x00000068;
        private UInt32 pad3 = 0x00000000;
        private int[] coefficients1 = new int[16];
        private UInt16 channel1Gain = 0x0000;
        private UInt16 channel1PredictiveScale = 0x0000;
        private UInt16 channel1PreviousValue = 0x0000;
        private UInt16 channel1NextPreviousValue = 0x0000;
        private UInt16 channel1LoopPredictiveScale = 0x0000;
        private UInt16 channel1LoopPreviousValue = 0x0000;
        private UInt16 channel1LoopNextPreviousValue = 0x0000;
        private UInt16 channel1LoopPadding = 0x0000;
        private int[] coefficients2 = new int[16];
        private UInt16 channel2Gain = 0x0000;
        private UInt16 channel2PredictiveScale = 0x0000;
        private UInt16 channel2PreviousValue = 0x0000;
        private UInt16 channel2NextPreviousValue = 0x0000;
        private UInt16 channel2LoopPredictiveScale = 0x0000;
        private UInt16 channel2LoopPreviousValue = 0x0000;
        private UInt16 channel2LoopNextPreviousValue = 0x0000;
        private UInt16 channel2LoopPadding = 0x0000;

        //Public Variables
        public byte HasLoop { get { return hasLoop; } set { hasLoop = value; } }
        public UInt32 Coefficients1Offset { get { return coefficients1Offset; } set { coefficients1Offset = value; } }
        public UInt32 Channel1StartOffset { get { return channel1StartOffset; } set { channel1StartOffset = value; } }
        public UInt32 Channel2StartOffset { get { return channel2StartOffset; } set { channel2StartOffset = value; } }
        public UInt32 Size { get { return size; } set { size = value; } }
        public UInt16 SampleRate { get { return sampleRate; } set { sampleRate = value; } }
        public byte ChannelCount { get { return channelCount; } set { channelCount = value; } }
        public UInt32 Channel1Start { get { return channel1Start; } set { channel1Start = value; } }
        public UInt32 Channel2Start { get { return channel2Start; } set { channel2Start = value; } }
        public UInt32 LoopStart { get { return loopStart; } set { loopStart = value; } }
        public UInt32 LoopEnd { get { return loopEnd; } set { loopEnd = value; } }
        public int[] Coefficients1 { get { return coefficients1; } set { coefficients1 = value; } }
        public int[] Coefficients2 { get { return coefficients2; } set { coefficients2 = value; } }

        public BNS_Info() { }

        public void Write(Stream outStream)
        {
            outStream.Write(magic, 0, magic.Length);

            byte[] temp = BitConverter.GetBytes(size); Array.Reverse(temp);
            outStream.Write(temp, 0, temp.Length);

            outStream.WriteByte(codec);
            outStream.WriteByte(hasLoop);
            outStream.WriteByte(channelCount);
            outStream.WriteByte(zero);

            temp = BitConverter.GetBytes(sampleRate); Array.Reverse(temp);
            outStream.Write(temp, 0, temp.Length);

            temp = BitConverter.GetBytes(pad0); Array.Reverse(temp);
            outStream.Write(temp, 0, temp.Length);

            temp = BitConverter.GetBytes(loopStart); Array.Reverse(temp);
            outStream.Write(temp, 0, temp.Length);

            temp = BitConverter.GetBytes(loopEnd); Array.Reverse(temp);
            outStream.Write(temp, 0, temp.Length);

            temp = BitConverter.GetBytes(offsetToChannelStart); Array.Reverse(temp);
            outStream.Write(temp, 0, temp.Length);

            temp = BitConverter.GetBytes(pad1); Array.Reverse(temp);
            outStream.Write(temp, 0, temp.Length);

            temp = BitConverter.GetBytes(channel1StartOffset); Array.Reverse(temp);
            outStream.Write(temp, 0, temp.Length);

            temp = BitConverter.GetBytes(channel2StartOffset); Array.Reverse(temp);
            outStream.Write(temp, 0, temp.Length);

            temp = BitConverter.GetBytes(channel1Start); Array.Reverse(temp);
            outStream.Write(temp, 0, temp.Length);

            temp = BitConverter.GetBytes(coefficients1Offset); Array.Reverse(temp);
            outStream.Write(temp, 0, temp.Length);

            if (this.channelCount == 2)
            {
                temp = BitConverter.GetBytes(pad2); Array.Reverse(temp);
                outStream.Write(temp, 0, temp.Length);

                temp = BitConverter.GetBytes(channel2Start); Array.Reverse(temp);
                outStream.Write(temp, 0, temp.Length);

                temp = BitConverter.GetBytes(coefficients2Offset); Array.Reverse(temp);
                outStream.Write(temp, 0, temp.Length);

                temp = BitConverter.GetBytes(pad3); Array.Reverse(temp);
                outStream.Write(temp, 0, temp.Length);

                foreach (int thisInt in coefficients1)
                {
                    temp = BitConverter.GetBytes(thisInt); Array.Reverse(temp);
                    outStream.Write(temp, 2, temp.Length - 2);
                }

                temp = BitConverter.GetBytes(channel1Gain); Array.Reverse(temp);
                outStream.Write(temp, 0, temp.Length);

                temp = BitConverter.GetBytes(channel1PredictiveScale); Array.Reverse(temp);
                outStream.Write(temp, 0, temp.Length);

                temp = BitConverter.GetBytes(channel1PreviousValue); Array.Reverse(temp);
                outStream.Write(temp, 0, temp.Length);

                temp = BitConverter.GetBytes(channel1NextPreviousValue); Array.Reverse(temp);
                outStream.Write(temp, 0, temp.Length);

                temp = BitConverter.GetBytes(channel1LoopPredictiveScale); Array.Reverse(temp);
                outStream.Write(temp, 0, temp.Length);

                temp = BitConverter.GetBytes(channel1LoopPreviousValue); Array.Reverse(temp);
                outStream.Write(temp, 0, temp.Length);

                temp = BitConverter.GetBytes(channel1LoopNextPreviousValue); Array.Reverse(temp);
                outStream.Write(temp, 0, temp.Length);

                temp = BitConverter.GetBytes(channel1LoopPadding); Array.Reverse(temp);
                outStream.Write(temp, 0, temp.Length);

                foreach (int thisInt in coefficients2)
                {
                    temp = BitConverter.GetBytes(thisInt); Array.Reverse(temp);
                    outStream.Write(temp, 2, temp.Length - 2);
                }

                temp = BitConverter.GetBytes(channel2Gain); Array.Reverse(temp);
                outStream.Write(temp, 0, temp.Length);

                temp = BitConverter.GetBytes(channel2PredictiveScale); Array.Reverse(temp);
                outStream.Write(temp, 0, temp.Length);

                temp = BitConverter.GetBytes(channel2PreviousValue); Array.Reverse(temp);
                outStream.Write(temp, 0, temp.Length);

                temp = BitConverter.GetBytes(channel2NextPreviousValue); Array.Reverse(temp);
                outStream.Write(temp, 0, temp.Length);

                temp = BitConverter.GetBytes(channel2LoopPredictiveScale); Array.Reverse(temp);
                outStream.Write(temp, 0, temp.Length);

                temp = BitConverter.GetBytes(channel2LoopPreviousValue); Array.Reverse(temp);
                outStream.Write(temp, 0, temp.Length);

                temp = BitConverter.GetBytes(channel2LoopNextPreviousValue); Array.Reverse(temp);
                outStream.Write(temp, 0, temp.Length);

                temp = BitConverter.GetBytes(channel2LoopPadding); Array.Reverse(temp);
                outStream.Write(temp, 0, temp.Length);
            }
            else if (this.channelCount == 1)
            {
                foreach (int thisInt in coefficients1)
                {
                    temp = BitConverter.GetBytes(thisInt); Array.Reverse(temp);
                    outStream.Write(temp, 2, temp.Length - 2);
                }

                temp = BitConverter.GetBytes(channel1Gain); Array.Reverse(temp);
                outStream.Write(temp, 0, temp.Length);

                temp = BitConverter.GetBytes(channel1PredictiveScale); Array.Reverse(temp);
                outStream.Write(temp, 0, temp.Length);

                temp = BitConverter.GetBytes(channel1PreviousValue); Array.Reverse(temp);
                outStream.Write(temp, 0, temp.Length);

                temp = BitConverter.GetBytes(channel1NextPreviousValue); Array.Reverse(temp);
                outStream.Write(temp, 0, temp.Length);

                temp = BitConverter.GetBytes(channel1LoopPredictiveScale); Array.Reverse(temp);
                outStream.Write(temp, 0, temp.Length);

                temp = BitConverter.GetBytes(channel1LoopPreviousValue); Array.Reverse(temp);
                outStream.Write(temp, 0, temp.Length);

                temp = BitConverter.GetBytes(channel1LoopNextPreviousValue); Array.Reverse(temp);
                outStream.Write(temp, 0, temp.Length);

                temp = BitConverter.GetBytes(channel1LoopPadding); Array.Reverse(temp);
                outStream.Write(temp, 0, temp.Length);
            }
        }
    }

    internal class BNS_Header
    {
        //Private Variables
        private byte[] magic = new byte[] { (byte)'B', (byte)'N', (byte)'S', (byte)' ' };
        private UInt32 flags = 0xfeff0100;
        private UInt32 fileSize = 0x0004d0c0;
        private UInt16 size = 0x0020;
        private UInt16 chunkCount = 0x0002;
        private UInt32 infoOffset = 0x00000020;
        private UInt32 infoLength = 0x000000a0;
        private UInt32 dataOffset = 0x000000c0;
        private UInt32 dataLength = 0x0004d000;

        //Public Varialbes
        public UInt32 DataOffset { get { return dataOffset; } set { dataOffset = value; } }
        public UInt32 InfoLength { get { return infoLength; } set { infoLength = value; } }
        public UInt16 Size { get { return size; } set { size = value; } }
        public UInt32 DataLength { get { return dataLength; } set { dataLength = value; } }
        public UInt32 FileSize { get { return fileSize; } set { fileSize = value; } }

        public BNS_Header() { }

        public void Write(Stream outStream)
        {
            outStream.Write(magic, 0, magic.Length);

            byte[] temp = BitConverter.GetBytes(flags); Array.Reverse(temp);
            outStream.Write(temp, 0, temp.Length);

            temp = BitConverter.GetBytes(fileSize); Array.Reverse(temp);
            outStream.Write(temp, 0, temp.Length);

            temp = BitConverter.GetBytes(size); Array.Reverse(temp);
            outStream.Write(temp, 0, temp.Length);

            temp = BitConverter.GetBytes(chunkCount); Array.Reverse(temp);
            outStream.Write(temp, 0, temp.Length);

            temp = BitConverter.GetBytes(infoOffset); Array.Reverse(temp);
            outStream.Write(temp, 0, temp.Length);

            temp = BitConverter.GetBytes(infoLength); Array.Reverse(temp);
            outStream.Write(temp, 0, temp.Length);

            temp = BitConverter.GetBytes(dataOffset); Array.Reverse(temp);
            outStream.Write(temp, 0, temp.Length);

            temp = BitConverter.GetBytes(dataLength); Array.Reverse(temp);
            outStream.Write(temp, 0, temp.Length);
        }
    }

    public class BNS_File
    {
        //Private Variables
        private BNS_Header bnsHeader = new BNS_Header();
        private BNS_Info bnsInfo = new BNS_Info();
        private BNS_Data bnsData = new BNS_Data();
        private int[,] lSamples = new int[2, 2];
        private int[,] rlSamples = new int[2, 2];
        private int[] tlSamples = new int[2];
        private int[] hbcDefTbl = new int[] { 674, 1040, 3598, -1738, 2270, -583, 3967, -1969, 1516, 381, 3453, -1468, 2606, -617, 3795, -1759 };
        private int[] defTbl = new int[] { 1820, -856, 3238, -1514, 2333, -550, 3336, -1376, 2444, -949, 3666, -1764, 2654, -701, 3420, -1398 };
        private int[] pHist1 = new int[2];
        private int[] pHist2 = new int[2];
        private int tempSampleCount;
        private string waveFile;
        private bool loopFromWave = false;
        private bool converted = false;
        private bool toMono = false;



        //Public Variables
        /// <summary>
        /// 0x00 (0) = No Loop, 0x01 (1) = Loop
        /// </summary>
        public byte HasLoop { get { return this.bnsInfo.HasLoop; } set { this.bnsInfo.HasLoop = value; } }
        /// <summary>
        /// The start sample of the Loop
        /// </summary>
        public UInt32 LoopStart { get { return this.bnsInfo.LoopStart; } set { this.bnsInfo.LoopStart = value; } }
        /// <summary>
        /// The total number of samples in this file
        /// </summary>
        public UInt32 TotalSampleCount { get { return this.bnsInfo.LoopEnd; } set { this.bnsInfo.LoopEnd = value; } }
        /// <summary>
        /// If true and the input Wave file is stereo, the BNS will be converted to Mono.
        /// Be sure to set this before you call Convert()!
        /// </summary>
        public bool StereoToMono { get { return toMono; } set { toMono = value; } }



        //Public Functions

        public BNS_File(string waveFile)
        {
            this.waveFile = waveFile;
        }

        public BNS_File(string waveFile, bool loopFromWave)
        {
            this.waveFile = waveFile;
            this.loopFromWave = loopFromWave;
        }

        /// <summary>
        /// Returns the progress of the conversion
        /// </summary>
        public event EventHandler<System.ComponentModel.ProgressChangedEventArgs> ProgressChanged;

        /// <summary>
        /// Converts the Wave file to BNS
        /// </summary>
        public void Convert()
        {
            Convert(waveFile, loopFromWave);
        }

        /// <summary>
        /// Returns the BNS file as a Byte Array. If not already converted, it will be done first.
        /// </summary>
        /// <returns></returns>
        public byte[] ToByteArray()
        {
            return ToMemoryStream().ToArray();
        }

        /// <summary>
        /// Returns the BNS file as a Memory Stream. If not already converted, it will be done first.
        /// </summary>
        /// <returns></returns>
        public MemoryStream ToMemoryStream()
        {
            if (!converted)
                Convert(waveFile, loopFromWave);

            MemoryStream ms = new MemoryStream();

            this.bnsHeader.Write(ms);
            this.bnsInfo.Write(ms);
            this.bnsData.Write(ms);

            return ms;
        }

        /// <summary>
        /// Saves the BNS file to the given path. If not already converted, it will be done first.
        /// </summary>
        /// <param name="destionationFile"></param>
        public void Save(string destionationFile)
        {
            if (File.Exists(destionationFile)) File.Delete(destionationFile);

            using (FileStream fs = new FileStream(destionationFile, FileMode.Create))
            {
                byte[] bnsFile = ToMemoryStream().ToArray();
                fs.Write(bnsFile, 0, bnsFile.Length);
            }
        }

        /// <summary>
        /// Sets the Loop to the given Start Sample. Be sure that you call Convert() first!
        /// </summary>
        /// <param name="loopStartSample"></param>
        public void SetLoop(int loopStartSample)
        {
            this.bnsInfo.HasLoop = 0x01;
            this.bnsInfo.LoopStart = (uint)loopStartSample;
        }



        //Private Functions

        private void Convert(string waveFile, bool loopFromWave)
        {
            Wave wave = new Wave(waveFile);
            int waveLoopCount = wave.LoopCount;
            int waveLoopStart = wave.LoopStart;

            this.bnsInfo.ChannelCount = (byte)wave.ChannelCount;
            this.bnsInfo.SampleRate = (ushort)wave.SampleRate;

            if (this.bnsInfo.ChannelCount > 2 || this.bnsInfo.ChannelCount < 1)
                throw new Exception("Unsupported Count of Channels!");
            if (wave.BitDepth != 16)
                throw new Exception("Only 16bit Wave files are supported!");
            if (wave.DataFormat != 1)
                throw new Exception("The format of this Wave file is not supported!");

            this.bnsData.Data = Encode(wave.GetAllFrames());
            wave.Close();

            if (this.bnsInfo.ChannelCount == 1)
            {
                this.bnsHeader.InfoLength = 0x60;
                this.bnsHeader.DataOffset = 0x80;

                this.bnsInfo.Size = 0x60;
                this.bnsInfo.Channel1StartOffset = 0x0000001C;
                this.bnsInfo.Channel2StartOffset = 0x00000000;
                this.bnsInfo.Channel1Start = 0x00000028;
                this.bnsInfo.Coefficients1Offset = 0x00000000;
            }

            this.bnsData.Size = (uint)bnsData.Data.Length + 8;

            this.bnsHeader.DataLength = this.bnsData.Size;
            this.bnsHeader.FileSize = this.bnsHeader.Size + this.bnsInfo.Size + this.bnsData.Size;

            if (loopFromWave)
                if (waveLoopCount == 1)
                    if (waveLoopStart != -1)
                    { this.bnsInfo.LoopStart = (uint)waveLoopStart; this.bnsInfo.HasLoop = 0x01; }

            this.bnsInfo.LoopEnd = (uint)tempSampleCount;                   

            for (int i = 0; i < 16; i++)
            {
                this.bnsInfo.Coefficients1[i] = this.defTbl[i];

                if (this.bnsInfo.ChannelCount == 2)
                    this.bnsInfo.Coefficients2[i] = this.defTbl[i];
            }

            this.converted = true;
        }

        private byte[] Encode(byte[] inputFrames)
        {
            int offset = 0;
            int[] sampleBuffer = new int[14];

            this.tempSampleCount = inputFrames.Length / (bnsInfo.ChannelCount == 2 ? 4 : 2);
            int modLength = (inputFrames.Length / (bnsInfo.ChannelCount == 2 ? 4 : 2)) % 14;

            Array.Resize(ref inputFrames, inputFrames.Length + ((14 - modLength) * (bnsInfo.ChannelCount == 2 ? 4 : 2)));

            int sampleCount = inputFrames.Length / (bnsInfo.ChannelCount == 2 ? 4 : 2);

            int blocks = (sampleCount + 13) / 14;

            List<int> soundDataLeft = new List<int>();
            List<int> soundDataRight = new List<int>();

            int co = offset;

            if (this.toMono && this.bnsInfo.ChannelCount == 2) this.bnsInfo.ChannelCount = 1;
            else if (this.toMono) this.toMono = false;

            for (int j = 0; j < sampleCount; j++)
            {
                soundDataLeft.Add(BitConverter.ToInt16(inputFrames, co));
                co += 2;

                if (this.bnsInfo.ChannelCount == 2 || toMono)
                {
                    soundDataRight.Add(BitConverter.ToInt16(inputFrames, co));
                    co += 2;
                }
            }

            byte[] data = new byte[(this.bnsInfo.ChannelCount == 2 ? (blocks * 16) : (blocks * 8))];

            int data1Offset = 0;
            int data2Offset = blocks * 8;

            this.bnsInfo.Channel2Start = (this.bnsInfo.ChannelCount == 2 ? (uint)data2Offset : 0);

            int[] leftSoundData = soundDataLeft.ToArray();
            int[] rightSoundData = soundDataRight.ToArray();

            for (int y = 0; y < blocks; y++)
            {
                try
                {
                    if (y % (int)(blocks / 100) == 0 || (y + 1) == blocks)
                        ChangeProgress((y + 1) * 100 / blocks);
                }
                catch { }

                for (int a = 0; a < 14; a++)
                    sampleBuffer[a] = leftSoundData[y * 14 + a];

                byte[] outBuffer = RepackAdpcm(0, this.defTbl, sampleBuffer);

                for (int a = 0; a < 8; a++)
                    data[data1Offset + a] = outBuffer[a];

                data1Offset += 8;

                if (this.bnsInfo.ChannelCount == 2)
                {
                    for (int a = 0; a < 14; a++)
                        sampleBuffer[a] = rightSoundData[y * 14 + a];

                    outBuffer = RepackAdpcm(1, this.defTbl, sampleBuffer);

                    for (int a = 0; a < 8; a++)
                        data[data2Offset + a] = outBuffer[a];
                    
                    data2Offset += 8;
                }
            }

            this.bnsInfo.LoopEnd = (uint)(blocks * 7);

            return data;
        }

        private byte[] RepackAdpcm(int index, int[] table, int[] inputBuffer)
        {
            byte[] data = new byte[8];
            int[] blSamples = new int[2];
            int bestIndex = -1;
            double bestError = 999999999.0;
            double error;

            for (int tableIndex = 0; tableIndex < 8; tableIndex++)
            {
                byte[] testData = CompressAdpcm(index, table, tableIndex, inputBuffer, out error);

                if (error < bestError)
                {
                    bestError = error;

                    for (int i = 0; i < 8; i++)
                        data[i] = testData[i];
                    for (int i = 0; i < 2; i++)
                        blSamples[i] = this.tlSamples[i];

                    bestIndex = tableIndex;
                }
            }

            for (int i = 0; i < 2; i++)
                this.rlSamples[index, i] = blSamples[i];

            return data;
        }

        private byte[] CompressAdpcm(int index, int[] table, int tableIndex, int[] inputBuffer, out double outError)
        {
            byte[] data = new byte[8];
            int error = 0;
            int factor1 = table[2 * tableIndex + 0];
            int factor2 = table[2 * tableIndex + 1];

            int exponent = DetermineStdExponent(index, table, tableIndex, inputBuffer);

            while (exponent <= 15)
            {
                bool breakIt = false;
                error = 0;
                data[0] = (byte)(exponent | (tableIndex << 4));

                for (int i = 0; i < 2; i++)
                    this.tlSamples[i] = this.rlSamples[index, i];

                int j = 0;

                for (int i = 0; i < 14; i++)
                {
                    int predictor = (int)((this.tlSamples[1] * factor1 + this.tlSamples[0] * factor2) >> 11);
                    int residual = (inputBuffer[i] - predictor) >> exponent;

                    if (residual > 7 || residual < -8)
                    {
                        exponent++;
                        breakIt = true;
                        break;
                    }

                    int nibble = Clamp(residual, -8, 7);

                    if ((i & 1) != 0)
                        data[i / 2 + 1] = (byte)(data[i / 2 + 1] | (nibble & 0xf));
                    else
                        data[i / 2 + 1] = (byte)(nibble << 4);

                    predictor += nibble << exponent;

                    this.tlSamples[0] = this.tlSamples[1];
                    this.tlSamples[1] = Clamp(predictor, -32768, 32767);

                    error += (int)(Math.Pow((double)(this.tlSamples[1] - inputBuffer[i]), 2));
                }

                if (!breakIt) j = 14;

                if (j == 14) break;
            }

            outError = error;
            return data;
        }

        private int DetermineStdExponent(int index, int[] table, int tableIndex, int[] inputBuffer)
        {
            int[] elSamples = new int[2];
            int maxResidual = 0;
            int factor1 = table[2 * tableIndex + 0];
            int factor2 = table[2 * tableIndex + 1];

            for (int i = 0; i < 2; i++)
                elSamples[i] = this.rlSamples[index, i];

            for (int i = 0; i < 14; i++)
            {
                int predictor = (elSamples[1] * factor1 + elSamples[0] * factor2) >> 11;
                int residual = inputBuffer[i] - predictor;

                if (residual > maxResidual)
                    maxResidual = residual;

                elSamples[0] = elSamples[1];
                elSamples[1] = inputBuffer[i];
            }

            return FindExponent(maxResidual);
        }

        private int FindExponent(double residual)
        {
            int exponent = 0;

            while (residual > 7.5 || residual < -8.5)
            {
                exponent++;
                residual /= 2.0;
            }

            return exponent;
        }

        private int Clamp(int input, int min, int max)
        {
            if (input < min) return min;
            if (input > max) return max;
            return input;
        }

        private void ChangeProgress(int progressPercentage)
        {
            EventHandler<System.ComponentModel.ProgressChangedEventArgs> progressChanged = ProgressChanged;
            if (progressChanged != null)
            {
                progressChanged(new object(), new System.ComponentModel.ProgressChangedEventArgs(progressPercentage, new object()));
            }
        }
    }
}
