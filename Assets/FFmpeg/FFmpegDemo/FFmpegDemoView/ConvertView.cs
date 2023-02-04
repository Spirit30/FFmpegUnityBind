using UnityEngine;

namespace FFmpeg.Demo
{
	public class ConvertView : MonoBehaviour
	{
        BaseData config = new BaseData();

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

		public void OnOutputPath(string fullPath)
		{
			config.outputPath = fullPath;
		}


		//------------------------------

		public void OnConvert()
		{
            FFmpegCommands.Convert(config);
			gameObject.SetActive(false);
		}

		//------------------------------
	}
}