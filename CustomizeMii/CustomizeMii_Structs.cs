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

namespace CustomizeMii
{
    public struct TplImage
    {
        public System.Drawing.Image tplImage;
        public System.Drawing.Image checkerBoard;
        public string tplFormat;
        public string fileName;
    }

    public struct TransmitInfo
    {
        public double compressionRatio;
        public double transmittedLength;
        public int timeElapsed;
    }

    public struct BnsConversionInfo
    {
        public enum LoopType
        {
            None,
            FromWave,
            Manual
        }

        public LoopType loopType;
        public int loopStartSample;
        public string audioFile;
        public bool stereoToMono;
    }

    public struct WadCreationInfo
    {
        public enum NandLoader : int
        {
            comex = 0,
            Waninkoko = 1,
        }

        public string titleId;
        public string[] titles;
        public string allLangTitle;
        public int startupIos;
        public string sound;
        public string dol;
        public string outFile;
        public NandLoader nandLoader;
        public bool sendToWii;
        public libWiiSharp.Protocol transmitProtocol;
        public string transmitIp;
        public int transmitIos;
        public bool saveAfterTransmit;
        public bool success;
        public bool sendWadReady;
        public byte[] wadFile;
        public bool lz77;
    }

    public struct Progress
    {
        public int progressValue;
        public string progressState;
    }
}
