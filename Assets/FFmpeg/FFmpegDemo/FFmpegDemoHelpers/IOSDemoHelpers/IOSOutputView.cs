using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace FFmpeg.Demo.IOS
{
    public class IOSOutputView : MonoBehaviour
    {
		[System.Serializable]
        public class PathEvent : UnityEvent<string> { }

        public Text iosDirectory;
        public Text fileNamePlaceholder;
        public PathEvent pathSetter;

        void Awake()
        {
#if UNITY_IOS && !UNITY_EDITOR
            //It's a default sanbox place to keep Application files (Documents directory)
			//https://developer.apple.com/library/content/documentation/FileManagement/Conceptual/FileSystemProgrammingGuide/FileSystemOverview/FileSystemOverview.html
			iosDirectory.text = Application.persistentDataPath + "/";
            OnFileNameInput(fileNamePlaceholder.text);
#else
			gameObject.SetActive(false);
#endif
		}

        /// <summary>
        /// Is called from InputField (inspector) as well.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public void OnFileNameInput(string fileName)
        {
            pathSetter.Invoke(iosDirectory.text + fileName);
        }
	}
}