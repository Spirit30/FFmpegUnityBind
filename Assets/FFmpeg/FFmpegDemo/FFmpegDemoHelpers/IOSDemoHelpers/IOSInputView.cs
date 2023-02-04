using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace FFmpeg.Demo.IOS
{
    public class IOSInputView : MonoBehaviour
    {
		[System.Serializable]
        public class PathEvent : UnityEvent<string> { }
        
        public Text iosPath;
        public PathEvent pathSetter;

        public void OnPress()
        {
#if UNITY_IOS && !UNITY_EDITOR
            IOSVideoPicker.GetVideoPath((string path) =>
            {
                iosPath.text = path;
                pathSetter.Invoke(path);
            });
#endif
        }

#if !UNITY_IOS || UNITY_EDITOR
        void Awake()
        {
            gameObject.SetActive(false);
        }
#endif
    }
}