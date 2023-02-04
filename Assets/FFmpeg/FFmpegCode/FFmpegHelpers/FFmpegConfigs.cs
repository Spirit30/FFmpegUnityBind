using System;
using System.Collections.Generic;

namespace FFmpeg
{
	[Serializable]
	public class BaseData
	{
		public string inputPath;
		public string outputPath;
	}

    [Serializable]
    public class SoundData : BaseData
    {
        public string soundPath;
    }

    [Serializable]
    public class DecodeEncodeData : SoundData
    {
        public float fps;
    }

	[Serializable]
	public class TrimData : BaseData
	{
        public string fromTime; //"00:00:01.0" - after first second
		public int durationSec;
	}

	[Serializable]
    public class CompressionData : BaseData
	{
        public int crf;
	}

	[Serializable]
	public class AppendData
	{
        public List<string> inputPaths = new List<string>();
		public string outputPath;
	}

    [Serializable]
    public class WatermarkData : BaseData
    {
        public string imagePath;
        public float imageScale;
        public float xPosNormal;
        public float yPosNormal;
    }
}
