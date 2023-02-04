using UnityEngine;
using System;
using System.IO;
using System.Text;

namespace FFmpeg.Demo.REC
{
    [RequireComponent(typeof(RecMicAudio), typeof(RecSystemAudio))]
    public class FFmpegREC : MonoBehaviour, IFFmpegHandler
    {
        //Data
        [Header("Targeted FPS")]
        public int FPS = 30;
        float actualFPS;
        [Header("0 for max resolution")]
        public int width = 854;
        public int height = 480;
        [Header("Change before initialization")]
        public RecAudioSource recAudioSource = RecAudioSource.System;
        Rect camRect;

        //References
        IRecAudio soundRecorder;
        Texture2D frameBuffer;

        //Paths
        const string FILE_FORMAT = "{0}_Frame.jpg";
        const string SOUND_NAME = "RecordedAudio.wav";
        const string VIDEO_NAME = "ScreenCapture.mp4";
        string cashDir, imgFilePathFormat, firstImgFilePath, soundPath, outputVideoPath;

        //Variables
        int framesCount;
        float startTime, frameInterval, frameTimer, totalTime;

        public bool isREC { get; private set; }
        public bool isProducing { get; private set; }
#if !UNITY_EDITOR
        int initialWidth, initialHeight;
        public bool overrideResolution { get { return width > 0 || height > 0; } }
#endif

        //References
        Action<string> onOutput, onFinish;

        //PUBLIC INTERFACE
        //------------------------------

        public void Init(Action<string> _onOutput, Action<string> _onFinish)
        {
            //Subscription to FFmpeg callbacks
            FFmpegParser.Handler = this;

            //Paths initialization
            cashDir = Path.Combine(Application.temporaryCachePath, "RecordingCash");
            imgFilePathFormat = Path.Combine(cashDir, FILE_FORMAT);
            firstImgFilePath = String.Format(imgFilePathFormat, "%0d");
            soundPath = Path.Combine(cashDir, SOUND_NAME);
            outputVideoPath = Path.Combine(Application.temporaryCachePath, VIDEO_NAME);

#if !UNITY_EDITOR
            initialWidth = Screen.width;
            initialHeight = Screen.height;
#endif

            //Sound source initialization
            if (recAudioSource == RecAudioSource.Mic)
            {
                soundRecorder = GetComponent<RecMicAudio>();
            }
            else if (recAudioSource == RecAudioSource.System)
            {
                soundRecorder = GetComponent<RecSystemAudio>();
            }
            else if (recAudioSource == RecAudioSource.None)
            {
                soundRecorder = null;
            }

            onOutput = _onOutput;
            onFinish = _onFinish;
        }

        void Clear()
        {
            if (Directory.Exists(cashDir))
                Directory.Delete(cashDir, true);
        }

        public void StartREC()
        {
            if (!isREC && !isProducing)
            {
                Clear();
                Directory.CreateDirectory(cashDir);
                Debug.Log("CashDir created: " + cashDir);

#if !UNITY_EDITOR
                if (overrideResolution)
                {
                    //Low level operation of resolution change
                    Screen.SetResolution(width, height, Screen.fullScreen);
                }
                else 
#endif
                {
                    width = Screen.width;
                    height = Screen.height;
                }

                frameBuffer = new Texture2D(width, height, TextureFormat.RGB24, false, true);
                camRect = new Rect(0, 0, width, height);
                startTime = Time.time;
                framesCount = 0;
                frameInterval = 1.0f / FPS;
                frameTimer = frameInterval;

                isREC = true;

                if (recAudioSource != RecAudioSource.None)
                {
                    soundRecorder.StartRecording();
                }
            }
        }

        public void StopREC()
        {
            isREC = false;
            isProducing = true;

            totalTime = Time.time - startTime;
            actualFPS = framesCount / totalTime;

            Debug.Log("Actual FPS: " + actualFPS);

            if (recAudioSource != RecAudioSource.None)
            {
                soundRecorder.StopRecording(soundPath);
            }

            CreateVideo();

#if !UNITY_EDITOR
            if (overrideResolution)
                //Return to initial screen resolution
                Screen.SetResolution(initialWidth, initialHeight, Screen.fullScreen);
#endif
        }

        //INTERNAL IMPLEMENTATION
        //------------------------------

        void OnPostRender()
        {
            if (isREC && (frameTimer += Time.deltaTime) > frameInterval)
            {
                frameTimer -= frameInterval;

                frameBuffer.ReadPixels(camRect, 0, 0);

                File.WriteAllBytes(NextImgFilePath(), frameBuffer.EncodeToJPG());
            }
        }

        string NextImgFilePath()
        {
            return String.Format(imgFilePathFormat, framesCount++);
        }

        void CreateVideo()
        {
            StringBuilder command = new StringBuilder();

            Debug.Log("firstImgFilePath: " + firstImgFilePath);
            Debug.Log("soundPath: " + soundPath);
            Debug.Log("outputVideoPath: " + outputVideoPath);

            //Input Image sequence params
            command.
                   Append("-y -framerate ").
                   Append(actualFPS.ToString()).
                   Append(" -f image2 -i ").
                   Append(AddQuotation(firstImgFilePath));

            //Input Audio params
            if (recAudioSource != RecAudioSource.None)
            {
                command.
                   Append(" -i ").
                       Append(AddQuotation(soundPath)).
                       Append(" -ss 0 -t ").
                       Append(totalTime);
            }

            //Output Video params
            command.
                   Append(" -vcodec libx264 -crf 25 -pix_fmt yuv420p ").
                   Append(AddQuotation(outputVideoPath));

            Debug.Log(command.ToString());

            FFmpegCommands.DirectInput(command.ToString());
        }

        string AddQuotation(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new Exception("Empty path.");
            }
#if UNITY_EDITOR || UNITY_STANDALONE
            const char DOUBLE_QUOTATION = '\"';
            if (path[0] != DOUBLE_QUOTATION)
            {
                return DOUBLE_QUOTATION + path + DOUBLE_QUOTATION;
            }
#endif
            return path;
        }

        //FFmpeg processing callbacks
        //------------------------------

        //Begining of video processing
        public void OnStart()
        {
            onOutput("VideoProducing Started\n");
        }

        //You can make custom progress bar here (parse msg)
        public void OnProgress(string msg)
        {
            onOutput(msg);
        }

        //Notify user about failure here
        public void OnFailure(string msg)
        {
            onOutput(msg);
        }

        //Notify user about success here
        public void OnSuccess(string msg)
        {
            onOutput(msg);
        }

        //Last callback - do whatever you need next
        public void OnFinish()
        {
            onFinish(outputVideoPath);
            Clear();
            isProducing = false;
        }
    }
}