using UnityEngine;

namespace FFmpeg.Demo
{
    public class FFmpegDemo : MonoBehaviour, IFFmpegHandler
    {
        public ProgressView progressView;
        public ConvertView convertView;
        public TrimView trimView;
        public DecodeView decodeView;
        public EncodeView encodeView;
        public CompressView compressView;
        public AppendView appendView;
        public AddSoundView addSoundView;
        public WatermarkView watermarkView;

        FFmpegHandler defaultHandler = new FFmpegHandler();

        //------------------------------

        void Awake()
        {
            FFmpegParser.Handler = this;
        }

        //------------------------------

        public void OnVersion()
        {
            FFmpegCommands.GetVersion();
        }

        //------------------------------

        public void OnConvert()
        {
            convertView.Open();
        }

        //------------------------------

        public void OnTrim()
        {
            trimView.Open();
        }

        //------------------------------

        public void OnDecode()
        {
            decodeView.Open();
        }

        //------------------------------

        public void OnEncode()
        {
            encodeView.Open();
        }

		//------------------------------

		public void OnCompress()
		{
            compressView.Open();
		}

		//------------------------------

		public void OnAppend()
		{
			appendView.Open();
		}

        //------------------------------

        public void OnAddSound()
        {
            addSoundView.Open();
        }

        //------------------------------

        public void OnWatermark()
        {
            watermarkView.Open();
        }

		//------------------------------

		public void OnDirectInput(string commands)
        {
            FFmpegCommands.DirectInput(commands);
        }

        //FFmpeg processing callbacks
        //------------------------------

        //Begining of video processing
        public void OnStart()
        {
			defaultHandler.OnStart();
            progressView.OnStart();
        }

		//You can make custom progress bar here (parse msg)
		public void OnProgress(string msg)
        {
            defaultHandler.OnProgress(msg);
            progressView.OnProgress(msg);
            Console.Print(msg);
        }

		//Notify user about failure here
		public void OnFailure(string msg)
        {
            defaultHandler.OnFailure(msg);
            progressView.OnFailure(msg);
            Console.Print(msg);
        }

		//Notify user about success here
		public void OnSuccess(string msg)
        {
			defaultHandler.OnSuccess(msg);
            progressView.OnSuccess(msg);
            Console.Print(msg);
        }

		//Last callback - do whatever you need next
		public void OnFinish()
        {
            defaultHandler.OnFinish();
            progressView.OnFinish();
        }
    }
}