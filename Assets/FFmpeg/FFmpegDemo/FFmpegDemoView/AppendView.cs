using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace FFmpeg.Demo
{
    public class AppendView : MonoBehaviour
    {
        public Dropdown modeDropdown;

        public InputField inputFieldOrigin;
        List<InputField> inputFields = new List<InputField>();
        int MIN_INPUT_VIDEOS = 2;

        AppendData config = new AppendData();
        bool fastMode;

        //------------------------------

        void Awake()
        {
            OnFastOrFull(modeDropdown.value);

            for (int i = 0; i < MIN_INPUT_VIDEOS; ++i)
            {
                OnAddInput();
            }
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        //------------------------------

        public void OnFastOrFull(int fast)
        {
            fastMode = fast > 0;
        }

        public void OnOutputPath(string fullPath)
        {
            config.outputPath = fullPath;
        }

        public void OnAddInput()
        {
			InputField inputInstance =
					Instantiate(inputFieldOrigin, inputFieldOrigin.transform.parent);
			inputInstance.gameObject.SetActive(true);
			inputInstance.transform.SetSiblingIndex(inputFields.Count);
			inputFields.Add(inputInstance);
        }

        public void OnRemoveInput()
        {
            if (inputFields.Count > MIN_INPUT_VIDEOS)
            {
                int lastIndex = inputFields.Count - 1;
                Destroy(inputFields[lastIndex].gameObject);
                inputFields.RemoveAt(lastIndex);
            }
        }

        //------------------------------

        public void OnAppend()
        {
            config.inputPaths.Clear();

			//Collect input paths. NOTE: Videos should be in same orientation.
			foreach (InputField inputField in inputFields)
            {
                config.inputPaths.Add(
                #if UNITY_IOS && !UNITY_EDITOR
                    inputField.gameObject.GetComponentInChildren<IOS.IOSInputView>().iosPath.text);
                #else
                    inputField.text);
                #endif
			}

            if (fastMode)
                FFmpegCommands.AppendFast(config);
            else
                FFmpegCommands.AppendFull(config);
            
			gameObject.SetActive(false);
		}

		//------------------------------
	}
}