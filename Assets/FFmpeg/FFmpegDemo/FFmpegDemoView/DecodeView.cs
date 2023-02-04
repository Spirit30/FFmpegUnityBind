using UnityEngine;

namespace FFmpeg.Demo
{
    public class DecodeView : MonoBehaviour
    {
        DecodeEncodeData config = new DecodeEncodeData();

        //------------------------------

        public void Open()
        {
            gameObject.SetActive(true);
        }

        //------------------------------

        public void OnImagesPathInput(string fullPath)
        {
            config.outputPath = fullPath;
        }

        public void OnSoundPathInput(string fullPath)
        {
            config.soundPath = fullPath;
        }

        public void OnVideoPathInput(string fullPath)
        {
            config.inputPath = fullPath;
        }

        public void OnFPSInput(string fps)
        {
            config.fps = float.Parse(fps);
        }

        //------------------------------

        public void OnDecode()
        {
            FFmpegCommands.Decode(config);
            gameObject.SetActive(false);
        }

        //------------------------------
    }
}