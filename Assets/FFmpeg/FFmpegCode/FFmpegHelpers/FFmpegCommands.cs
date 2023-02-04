using UnityEngine;
using FFmpeg;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

/// <summary>
/// Entry point for FFmpeg
/// </summary>
public static class FFmpegCommands
{
    //Do not call this
    static FFmpegWrapper w;
    //Call this ------------|
    //                      |
    //                     \|/
    static FFmpegWrapper Wrapper
    {
        get
        {
            if (w == null)
            {
                w = MonoBehaviour.FindObjectOfType<FFmpegWrapper>();
                if (w == null)
                    Debug.LogException(new Exception("Place a FFmpeg.prefab in the scene"));
            }
            return w;
        }
    }
    //Data (instructions)
    public const char SEPARATOR = ' ';
    public const char QUOTE = '\'';
    public const char DOUBLE_QUOTE = '\"';
    public const string VERSION_INSTRUCTION = "-version";
    public const string REWRITE_INSTRUCTION = "-y";
    public const string INPUT_INSTRUCTION = "-i";
    public const string INDEX_PREFIX_INSTRUCTION = "%";
    public const string INDEX_SUFIX_INSTRUCTION = "d";
    public const string RESIZE_INSTRUCTION = "-r";
    public const string SS_INSTRUCTION = "-ss";
	public const string CODEC_INSTRUCTION = "-codec";
    public const string C_CODEC_INSTRUCTION = "-c";
	public const string COPY_INSTRUCTION = "copy";
    public const string TIME_INSTRUCTION = "-t";
    public const string CODEC_VIDEO_INSTRUCTION = "-c:v";
    public const string CODEC_AUDIO_INSTRUCTION = "-c:a";
    public const string LIB_X264_INSTRUCTION = "libx264";
    public const string CONSTANT_RATE_FACTOR_INSTRUCTION = "-crf";
    public const string FILE_FORMAT_INPUT_INSTRUCTION = "-f";
    public const string CONCAT_INSTRUCTION = "concat";
    public const string SAFE_INSTRUCTION = "-safe";
    public const string ZERO_INSTRUCTION = "0";
    public const string FILTER_COMPLEX_INSTRUCTION = "-filter_complex";
    public const string MAP_INSTRUCTION = "-map";
    public const string PRESET_INSTRUCTION = "-preset";
    public const string VIDEO_INSTRUCTION = "[v]";
    public const string AUDIO_INSTRUCTION = "[a]";
    public const string ULTRASAFE_INSTRUCTION = "ultrafast";
    public const string VIDEO_FORMAT = "[{0}:v:0] ";
    public const string AUDIO_FORMAT = "[{0}:a:0] ";
    public const string CONCAT_FORMAT = "{0}=n={1}:v=1:a=1";
    public const string FIRST_INPUT_VIDEO_CHANNEL = "0:v";
    public const string SECOND_INPUT_AUDIO_CHANNEL = "1:a";
    public const string SHORTEST_INSTRUCTION = "-shortest";
    public const string PIXEL_FORMAT = "-pix_fmt";
    public const string YUV_420P = "yuv420p";

    //------------------------------

    public static void GetVersion()
    {
        Wrapper.Execute(new string[] { VERSION_INSTRUCTION });
    }

	//------------------------------

    public static void Convert(BaseData config)
	{
		//-y -i .../input.mp4 .../output.mp3
		string[] command =
		{
			REWRITE_INSTRUCTION,
			INPUT_INSTRUCTION,
			config.inputPath,
			config.outputPath
		};

		DebugCommand(command);

		Wrapper.Execute(command);
	}

    //------------------------------

    public static void Trim(TrimData config)
    {
		//-y -i .../input.mp4 -ss 00:00:50.0 -codec copy -t 20 .../output.mp4
		string[] command =
		{
			REWRITE_INSTRUCTION,
			INPUT_INSTRUCTION,
            config.inputPath,
            SS_INSTRUCTION,
            config.fromTime,
            CODEC_INSTRUCTION,
            COPY_INSTRUCTION,
            TIME_INSTRUCTION,
            config.durationSec.ToString(),
            config.outputPath
		};

		DebugCommand(command);

		Wrapper.Execute(command);
    }

	//------------------------------

	public static void Decode(DecodeEncodeData config)
	{
		//-y -i .../video.mp4 -r 30 .../image%1d.jpg .../track.mp3 
		string[] command =
		{
			REWRITE_INSTRUCTION,
			INPUT_INSTRUCTION,
			config.inputPath,
			RESIZE_INSTRUCTION,
			config.fps.ToString(),
			config.outputPath,
			config.soundPath
		};

		DebugCommand(command);

		Wrapper.Execute(command);
	}

	//------------------------------

	public static void Encode(DecodeEncodeData config)
	{
        //-y -i .../image%1d.jpg -r 30 -i .../track.mp3 -pix_fmt yuv420p .../video.mp4
		string[] command =
		{
			REWRITE_INSTRUCTION,
			INPUT_INSTRUCTION,
			config.inputPath,
			RESIZE_INSTRUCTION,
			config.fps.ToString(),
			INPUT_INSTRUCTION,
			config.soundPath,
            PIXEL_FORMAT,
            YUV_420P,
			config.outputPath
		};

		DebugCommand(command);

		Wrapper.Execute(command);
	}

    //------------------------------

    public static void Compress(CompressionData config)
    {
        //-i .../input.mp4 -c:v libx264 -crf 23 .../output.mp4 -y
		string[] command =
        {
            INPUT_INSTRUCTION,
            config.inputPath,
            CODEC_VIDEO_INSTRUCTION,
            LIB_X264_INSTRUCTION,
            CONSTANT_RATE_FACTOR_INSTRUCTION,
            config.crf.ToString(),
            config.outputPath,
            REWRITE_INSTRUCTION
        };

		DebugCommand(command);

		Wrapper.Execute(command);
    }

    //------------------------------

    public static void AppendFast(AppendData config)
	{
        //-f concat -safe 0 -i .../mylist.txt -c copy .../output.mp4 -y
        string[] command =
        {
            FILE_FORMAT_INPUT_INSTRUCTION,
            CONCAT_INSTRUCTION,
            SAFE_INSTRUCTION,
            ZERO_INSTRUCTION,
            INPUT_INSTRUCTION,
            GetInputsFile(config.inputPaths),
            C_CODEC_INSTRUCTION,
            COPY_INSTRUCTION,
            config.outputPath,
            REWRITE_INSTRUCTION
		};

		DebugCommand(command);

		Wrapper.Execute(command);
	}

    static string GetInputsFile(List<string> inputPaths)
	{
        string inputFolder = Path.GetDirectoryName(
            //Remove standalone style
            inputPaths[0].Replace("\"", string.Empty));

        StringBuilder fileBuffer = new StringBuilder();
        fileBuffer.Append("# File with input videos\n");
        foreach(string ip in inputPaths)
		{
            //Relative to input folder
            string inputPath = ip.Remove(0, inputFolder.Length);
            //Remove standalone style
            inputPath = ip.Replace("\"", string.Empty);

            fileBuffer.Append("file '" + inputPath + "'\n");
		}

		string filePath = Path.Combine(inputFolder, "AppendInputFiles.txt");
		using (FileStream fileStream = File.Create(filePath))
		{
            byte[] buffer = 
                new UTF8Encoding(true).GetBytes(fileBuffer.ToString());
			fileStream.Write(buffer, 0, buffer.Length);
		}
#if UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR
        filePath = DOUBLE_QUOTE + filePath + DOUBLE_QUOTE;
#endif
        Debug.Log("FilePath: " + filePath);
		return filePath;
	}

	//------------------------------

	public static void AppendFull(AppendData config)
	{
        //ffmpeg - i .../input1.mp4 - i .../input2.webm \
        //-filter_complex "[0:v:0] [0:a:0] [1:v:0] [1:a:0] concat=n=2:v=1:a=1 [v] [a]" \
        //-map "[v]" - map "[a]" < encoding options > .../output.mkv -y
        List<string> cmd = new List<string>();
        StringBuilder filter = new StringBuilder();
#if UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR
        filter.Append(DOUBLE_QUOTE);
#endif
        for (int i = 0; i < config.inputPaths.Count; ++i)
		{
            cmd.Add(INPUT_INSTRUCTION);
            cmd.Add(config.inputPaths[i]);

            filter.Append(string.Format(VIDEO_FORMAT, i)).Append(string.Format(AUDIO_FORMAT, i));
		}

        filter.
              Append(string.Format(CONCAT_FORMAT, CONCAT_INSTRUCTION, config.inputPaths.Count)).
              Append(SEPARATOR).
              Append(VIDEO_INSTRUCTION).
              Append(SEPARATOR).
              Append(AUDIO_INSTRUCTION);
#if UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR
        filter.Append(DOUBLE_QUOTE);
#endif

        cmd.Add(FILTER_COMPLEX_INSTRUCTION);
        cmd.Add(filter.ToString());
        cmd.Add(MAP_INSTRUCTION);
        cmd.Add(VIDEO_INSTRUCTION);
        cmd.Add(MAP_INSTRUCTION);
        cmd.Add(AUDIO_INSTRUCTION);
        cmd.Add(PRESET_INSTRUCTION);
        cmd.Add(ULTRASAFE_INSTRUCTION);
        cmd.Add(config.outputPath);
        cmd.Add(REWRITE_INSTRUCTION);

        string[] command = cmd.ToArray();

		DebugCommand(command);

		Wrapper.Execute(command);
	}

	//------------------------------

    public static void AddSoundFast(SoundData config)
    {
        //aac is compatible with mp4 - no need full re-encoding.
        //-y -i .../input.mp4 -i .../audio.aac -c copy -map 0:v -map 1:a -shortest .../output.mp4
        string[] command =
        {
            REWRITE_INSTRUCTION,
            INPUT_INSTRUCTION,
            config.inputPath,
            INPUT_INSTRUCTION,
            config.soundPath,
            C_CODEC_INSTRUCTION,
            COPY_INSTRUCTION,
            MAP_INSTRUCTION,
            FIRST_INPUT_VIDEO_CHANNEL,
            MAP_INSTRUCTION,
            SECOND_INPUT_AUDIO_CHANNEL,
            SHORTEST_INSTRUCTION,
            config.outputPath
        };

        DebugCommand(command);

        Wrapper.Execute(command);
    }

    //------------------------------

    public static void AddSoundFull(SoundData config)
    {
        //mp3 is does not compatible with mp4. Full re-encoding handles this.
        //-y -i .../audio.mp3 -i .../input.mp4 .../output.mp4
        string[] command =
        {
            REWRITE_INSTRUCTION,
            INPUT_INSTRUCTION,
            config.soundPath,
            INPUT_INSTRUCTION,
            config.inputPath,
            config.outputPath
        };

        DebugCommand(command);

        Wrapper.Execute(command);
    }

    //------------------------------

    public static void Watermark(WatermarkData config)
    {
        //-y  -i .../watermark.png -i .../input.mp4 -filter_complex \
        //"[0:v]scale=iw*0.25:ih*0.25 [ovrl], [1:v][ovrl]overlay=x=(main_w-overlay_w)*0.95:y=(main_h-overlay_h)*0.05" \
        //FFmpegUnityBindDemo.mp4
        StringBuilder filter = new StringBuilder();
#if UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR
        filter.Append(DOUBLE_QUOTE);
#endif
        filter.
              Append("[0:v]scale=iw*").
              Append(config.imageScale).
              Append(":ih*").
              Append(config.imageScale).
              Append(" [ovrl], [1:v][ovrl]overlay=x=(main_w-overlay_w)*").
              Append(config.xPosNormal).
              Append(":y=(main_h-overlay_h)*").
              Append(config.yPosNormal);
#if UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR
        filter.Append(DOUBLE_QUOTE);
#endif

        string[] command =
        {
            REWRITE_INSTRUCTION,
            INPUT_INSTRUCTION,
            config.imagePath,
            INPUT_INSTRUCTION,
            config.inputPath,
            FILTER_COMPLEX_INSTRUCTION,
            filter.ToString(),
            config.outputPath
        };

        DebugCommand(command);

        Wrapper.Execute(command);
    }

    //------------------------------

	public static void DirectInput(string input)
	{
		string[] command = input.Split(' ');

        DebugCommand(command);

		Wrapper.Execute(command);
	}

	//------------------------------

	static void DebugCommand(string[] command)
    {
        StringBuilder debugCommand = new StringBuilder();
        foreach (string instruction in command)
            debugCommand.Append(instruction + " ");
        Debug.Log(debugCommand.ToString());
    }
}