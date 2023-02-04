using UnityEngine;
using UnityEngine.UI;

namespace FFmpeg.Demo
{
    public class ProgressView : MonoBehaviour, IFFmpegHandler
    {
        public Text progressField;
        public Image progressBar;
        public Color normalColor, failureColor;

        float durationBuffer, progress;
        bool? success;

        //PUBLIC
		//------------------------------

        public void OnStart()
        {
            success = null;
            progressBar.color = normalColor;
            durationBuffer = progress = 0;
            UpdateBar();
            progressField.text = "Started.";
        }

        public void OnProgress(string msg)
        {
            if (success == null)
            {
                FFmpegProgressParser.Parse(msg, ref durationBuffer, ref progress);
                progressField.text = "Progress: " + (int)(progress * 100) + "% / 100%";
                UpdateBar();
            }
        }

		public void OnFailure(string msg)
		{
            OnProgress(msg);
            success = false;
		}

		public void OnSuccess(string msg)
		{
            OnProgress(msg);
            success = true;
		}

		public void OnFinish()
		{
			progress = 1;
			UpdateBar();
            if (success == true)
			{
				progressField.text = "Success!";
			}
            else if (success == false)
			{
				progressField.text = "Failure.";
				progressBar.color = failureColor;
			}
            else
            {
                progressField.text = "Finish.";
            }
		}

        //------------------------------

        void UpdateBar()
        {
            Vector3 scale = Vector3.one;
            scale.x = progress;
            progressBar.transform.localScale = scale;
        }
    }
}