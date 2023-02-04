using UnityEngine;
using UnityEngine.UI;

namespace FFmpeg.Demo
{
	public class CompressView : MonoBehaviour
	{
        CompressionData config = new CompressionData();
        public InputField crfField;
        public Slider amountSlider;
        const int MIN_CRF = 0;
        const int MAX_CRF = 51;

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

		public void OnCRF(string crf)
		{
            config.crf = int.Parse(crf);
            amountSlider.value = (float)config.crf / MAX_CRF;
		}

		public void OnOutputPath(string fullPath)
		{
			config.outputPath = fullPath;
		}

		public void OnAmount(float amount)
		{
            config.crf = (int)Mathf.Lerp(MIN_CRF, MAX_CRF, amount);
            crfField.text = config.crf.ToString();
		}

		//------------------------------

		public void OnCompress()
		{
            FFmpegCommands.Compress(config);
			gameObject.SetActive(false);
		}

		//------------------------------
	}
}