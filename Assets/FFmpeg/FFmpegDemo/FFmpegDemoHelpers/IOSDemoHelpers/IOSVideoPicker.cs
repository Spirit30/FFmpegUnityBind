using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace FFmpeg.Demo.IOS
{
    //Should be placed in the scene for callbacks initialization
    public class IOSVideoPicker : MonoBehaviour
    {
#if UNITY_IOS && !UNITY_EDITOR
        static Action<string> Callback;

        //https://forums.macrumors.com/threads/function-pointers-in-obj-c.253962/
        //void get_video_path(void (* callback)(const char*));
        [DllImport("__Internal")]
        static extern int get_video_path(IOSCallback callback);
        //void play(const char* path);
        [DllImport("__Internal")]
        static extern int play(string path);

        delegate void IOSCallback(string msg);
        [AOT.MonoPInvokeCallback(typeof(IOSCallback))]
        static void IOSCallbacFunc(string msg)
        {
            Debug.Log("Unity receive video path: " + msg);
            Callback(msg);
        }

        public static void GetVideoPath(Action<string> _callback)
        {
            get_video_path(IOSCallbacFunc);
            Callback = _callback;
        }

        public static void Play(string path)
        {
            play(path);
        }
#else
		//Dummy Interface
		//------------------------------

        public static void GetVideoPath(Action<string> _callback)
        {
            UnSupportedWarn();
        }

        public static void Play(string path)
        {
            UnSupportedWarn();
        }

		//------------------------------

		static void UnSupportedWarn()
        {
            Debug.LogWarning("IOSVideoPicker can't be used on " + Application.platform);
        }
#endif
	}
}