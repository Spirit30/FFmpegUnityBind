#if UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR
using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

namespace FFmpeg
{
    public class StandaloneProxy
    {
#if UNITY_EDITOR
        public const string EDITOR_BINARY_PATH =
            "FFmpeg/Standalone/" + PLATFORM + "/ffmpeg";
        const string PLATFORM =
#if UNITY_EDITOR_OSX
            "Mac";
#elif UNITY_EDITOR_WIN
            "Win";
#endif
#endif
        static string binaryPath;
        static StringBuilder buffer;
        static Action<string> callback;
        static bool isRunning { get; set; }

#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        [System.Runtime.InteropServices.DllImport("libc", EntryPoint = "chmod", SetLastError = true)]
        static extern int sys_chmod(string path, uint mode);
#endif

        //------------------------------

        public static void Begin(Action<string> _callback)
        {
#if UNITY_EDITOR 
            binaryPath = Path.Combine(Application.dataPath, EDITOR_BINARY_PATH);
#elif UNITY_STANDALONE_OSX
            string buildDir = Directory.GetCurrentDirectory();
            binaryPath = Path.Combine(buildDir, "ffmpeg");
#elif UNITY_STANDALONE_WIN
            string buildDir = Directory.GetCurrentDirectory();
            string dataDir = Directory.GetDirectories(buildDir, "*_Data")[0];
            binaryPath = Path.Combine(dataDir, "ffmpeg");
#endif
            if (!File.Exists(binaryPath))
            {
                string err = "Binary is not found at " + binaryPath;
                _callback(err);
                throw new FileNotFoundException(err);
            }

#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
            //Grant permission to ffmpeg binary file work
            sys_chmod(binaryPath, 755);
#endif

            callback = _callback;
        }

        public static void Execute(string command)
        {
            if (isRunning)
                return;

            //Clear output buffer
            buffer = new StringBuilder(short.MaxValue);

            new Thread(() =>
            {
                isRunning = true;
                Thread.CurrentThread.IsBackground = true;

                //Execute binary
                Process p = new Process();
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.FileName = binaryPath;
                p.StartInfo.Arguments = command;

                p.OutputDataReceived += (s, e) =>
                {
                    callback(FFmpegParser.COMMAND_CODE + FFmpegParser.PROGRESS_CODE + AppendLog(e.Data));
                };
                p.ErrorDataReceived += (s, e) =>
                {

                    if (!string.IsNullOrEmpty(e.Data) && e.Data.ToLower().Contains("error"))
                        callback(FFmpegParser.ERROR_CODE + AppendLog(e.Data));
                    else
                        callback(FFmpegParser.COMMAND_CODE + FFmpegParser.PROGRESS_CODE + AppendLog(e.Data));
                };

                p.Start();

                callback(FFmpegParser.COMMAND_CODE + FFmpegParser.START_CODE + "\nStarted\n");

                p.BeginOutputReadLine();
                p.BeginErrorReadLine();

                p.WaitForExit();

                callback(FFmpegParser.COMMAND_CODE +
                         (p.ExitCode == 0 ?
                          FFmpegParser.SUCCESS_CODE + AppendLog("Success!") :
                          FFmpegParser.FAILURE_CODE + AppendLog("Failure. Search details above")));

                p.Close();

                callback(FFmpegParser.COMMAND_CODE + FFmpegParser.FINISH_CODE + "\nFinished\n");
                isRunning = false;

            }).Start();
        }

        static string AppendLog(string msg)
        {
            return buffer.Append(msg).Append("\n").ToString();
        }

    }
}
#endif