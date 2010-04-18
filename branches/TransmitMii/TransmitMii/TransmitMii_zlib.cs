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
using System.IO;
using System.Runtime.InteropServices;

namespace TransmitMii
{
    public static class zlib
    {
        public enum ZLibError : int
        {
            Z_OK = 0,
            Z_STREAM_END = 1,
            Z_NEED_DICT = 2,
            Z_ERRNO = (-1),
            Z_STREAM_ERROR = (-2),
            Z_DATA_ERROR = (-3),
            Z_MEM_ERROR = (-4),
            Z_BUF_ERROR = (-5),
            Z_VERSION_ERROR = (-6)
        }

        [DllImport("zlib1.dll")]
        private static extern ZLibError compress2(byte[] dest, ref int destLength, byte[] source, int sourceLength, int level);

        public static byte[] Compress(byte[] inFile)
        {
            ZLibError err;
            byte[] outFile = new byte[inFile.Length];
            int outLength = -1;

            err = compress2(outFile, ref outLength, inFile, inFile.Length, 6);

            if (err == ZLibError.Z_OK && (outLength > -1 && outLength < inFile.Length))
            {
                Array.Resize(ref outFile, outLength);
                return outFile;
            }
            else
                throw new Exception("An error occured while compressing! Code: " + err.ToString());
        }
    }
}
