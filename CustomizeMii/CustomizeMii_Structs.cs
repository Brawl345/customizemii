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
    public struct BnsConversionInfo
    {
        public enum LoopType
        {
            None,
            FromWave,
            Manual
        }

        public LoopType Loop;
        public int LoopStartSample;
        public string AudioFile;
        public bool StereoToMono;
    }

    public struct WadCreationInfo
    {
        public enum NandLoader : int
        {
            comex = 0,
            Waninkoko = 1
        }

        public string outFile;
        public NandLoader nandLoader;
    }

    public struct Progress
    {
        public int progressValue;
        public string progressState;
    }
}
