using System;
using UnityEngine;

namespace FFmpeg
{
    public static class FFmpegParser
    {
        internal static IFFmpegHandler Handler { get; set; }
        //Data
        public const string COMMAND_CODE = "FFmpeg COMMAND: ";
        public const string ERROR_CODE = "FFmpeg EXCEPTION: ";
        public const string START_CODE = "onStart";
        public const string PROGRESS_CODE = "onProgress: ";
        public const string FAILURE_CODE = "onFailure: ";
        public const string SUCCESS_CODE = "onSuccess: ";
        public const string FINISH_CODE = "onFinish";

        //------------------------------

        public static void Handle(string message)
        {
            if(string.IsNullOrEmpty(message))
            {
                Debug.LogWarning("FFmpeg callback is null.");
                return;
            }

            if (IsCode(ref message, COMMAND_CODE))
            {
                if (IsCode(message, START_CODE))
                {
                    Handler.OnStart();
                }
                else if (IsCode(ref message, PROGRESS_CODE))
                {
                    Handler.OnProgress(message);
                }
                else if (IsCode(ref message, FAILURE_CODE))
                {
                    Handler.OnFailure(message);
                }
                else if (IsCode(ref message, SUCCESS_CODE))
                {
                    Handler.OnSuccess(message);
                }
                else if (IsCode(message, FINISH_CODE))
                {
                    Handler.OnFinish();
                }
            }
            else if(IsCode(message, ERROR_CODE))
            {
                Debug.LogError(message);
            }
        }

        static bool IsCode(ref string message, string CODE)
        {
            if (string.IsNullOrEmpty(message))
            {
                Debug.LogWarning("FFmpegParser: Empty callback message.");
            }
            else if (message.Contains(CODE))
            {
                message = message.Remove(0, CODE.Length);
                return true;
            }
            return false;
        }

		static bool IsCode(string message, string CODE)
		{
            if (string.IsNullOrEmpty(message))
            {
                Debug.LogWarning("FFmpegParser: Empty callback message.");
                return false;
            }
            return message.Contains(CODE);
		}
    }
}