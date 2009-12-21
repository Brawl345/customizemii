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
