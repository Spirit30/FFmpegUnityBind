using UnityEngine;

namespace FFmpeg.Demo
{
    public class EncodeView : MonoBehaviour
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
            config.inputPath = fullPath;
        }

        public void OnSoundPathInput(string fullPath)
        {
            config.soundPath = fullPath;
        }

        public void OnOutputPathInput(string fullPath)
        {
            config.outputPath = fullPath;
        }

        public void OnFPSInput(string fps)
        {
            config.fps = float.Parse(fps);
        }

        //------------------------------

        public void OnEncode()
        {
            FFmpegCommands.Encode(config);
            gameObject.SetActive(false);
        }

        //------------------------------
    }
}