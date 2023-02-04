using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace FFmpeg.Demo
{
    public class AddSoundView : MonoBehaviour
    {
        public Dropdown modeDropdown;

        public InputField inputVideoField;
        public InputField inputAudioField;
        public InputField outputField;

        SoundData config = new SoundData();
        bool fastMode;

        //------------------------------

        void Awake()
        {
            OnFastOrFull(modeDropdown.value);
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        //------------------------------

        public void OnOutputPath(string fullPath)
        {
            config.outputPath = fullPath;
        }

        public void OnSoundInputPath(string fullPath)
        {
            config.soundPath = fullPath;
        }

        public void OnVideoInputPath(string fullPath)
        {
            config.inputPath = fullPath;
        }

        public void OnFastOrFull(int fast)
        {
            fastMode = fast > 0;
        }

        //------------------------------

        public void OnAddSound()
        {
            if (fastMode)
                FFmpegCommands.AddSoundFast(config);
            else
                FFmpegCommands.AddSoundFull(config);

            gameObject.SetActive(false);
        }

        //------------------------------
    }
}