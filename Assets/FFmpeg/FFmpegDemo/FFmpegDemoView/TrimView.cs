using UnityEngine;

namespace FFmpeg.Demo
{
	public class TrimView : MonoBehaviour
	{
        TrimData config = new TrimData();

		//------------------------------

		public void Open()
		{
			gameObject.SetActive(true);
		}

		//------------------------------

		public void OnInputPath(string fullPath)
		{
            config.inputPath = fullPath;
		}

		public void OnStartTime(string time)
		{
            config.fromTime = time;
		}

		public void OnOutputPath(string fullPath)
		{
            config.outputPath = fullPath;
		}

		public void OnDuration(string duration)
		{
            config.durationSec = int.Parse(duration);
		}

		//------------------------------

		public void OnTrim()
		{
            FFmpegCommands.Trim(config);
			gameObject.SetActive(false);
		}

		//------------------------------
	}
}