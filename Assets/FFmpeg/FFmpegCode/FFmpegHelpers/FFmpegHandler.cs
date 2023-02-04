using System;
using UnityEngine;

namespace FFmpeg
{
    interface IFFmpegHandler
    {
        void OnStart();
        void OnProgress(string msg);
        void OnFailure(string msg);
        void OnSuccess(string msg);
        void OnFinish();
    }


    public class FFmpegHandler : IFFmpegHandler
    {
        //Default implementation 
        //------------------------------
        public void OnStart() { Debug.Log("FFmpegHandler.Start"); }
        public void OnProgress(string msg) { Debug.Log("FFmpegHandler.Progress: " + msg); }
        public void OnFailure(string msg) { Debug.Log("FFmpegHandler.Failure: " + msg); }
        public void OnSuccess(string msg) { Debug.Log("FFmpegHandler.Success: " + msg); }
        public void OnFinish() { Debug.Log("FFmpegHandler.Finish"); }
    }
}