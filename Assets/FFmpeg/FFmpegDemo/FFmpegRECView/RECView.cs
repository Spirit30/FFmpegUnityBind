using UnityEngine;
using UnityEngine.UI;

namespace FFmpeg.Demo.REC
{
    public class RECView : MonoBehaviour
    {
        public FFmpegREC recLogic;
        public Button startBtn, stopBtn;
        public Text output;
        const int charsLimit = 2000;

        //------------------------------

        void Awake()
        {
            recLogic.Init(Output, Finish);
            startBtn.onClick.AddListener(OnStart);
            stopBtn.onClick.AddListener(OnStop);
        }

        //------------------------------

        public void OnStart()
        {
            startBtn.interactable = false;
            stopBtn.interactable = true;
            recLogic.StartREC();
        }

        //------------------------------

        public void OnStop()
        {
            stopBtn.interactable = false;
            recLogic.StopREC();
        }

        //------------------------------

        void Output(string msg)
        {
            string newOutput = output.text + msg;
            if (newOutput.Length > charsLimit)
                newOutput = newOutput.Remove(0, newOutput.Length - charsLimit);
            output.text = newOutput;
        }

        public void Finish(string outputVideo)
        {
            startBtn.interactable = true;
            Debug.Log("Video saved to: " + outputVideo);
            string localURL = "file://" + outputVideo;
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
            Handheld.PlayFullScreenMovie(
                localURL,
                Color.black,
                FullScreenMovieControlMode.Full,
                FullScreenMovieScalingMode.AspectFit);
#else
            Application.OpenURL(localURL);
#endif
        }

        //------------------------------

        void OnDestroy()
        {
            startBtn.onClick.RemoveListener(OnStart);
            stopBtn.onClick.RemoveListener(OnStop);
        }
    }
}