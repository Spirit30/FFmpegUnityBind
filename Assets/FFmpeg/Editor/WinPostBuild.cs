#if UNITY_STANDALONE_WIN
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;

public class WinPostBuild
{
	[PostProcessBuild(1000)]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
	{
		PostProcessBuild(pathToBuiltProject);
	}

    private static void PostProcessBuild(string path)
    {
        string binarySource =
            Path.Combine(
                Application.dataPath,
                FFmpeg.StandaloneProxy.EDITOR_BINARY_PATH);

        string extension = Path.GetExtension(path);
        string dataFolder = path.Replace(extension, "_Data");
        string binaryDestination = Path.Combine(dataFolder, "ffmpeg");

        File.Copy(binarySource, binaryDestination, true);
    }
}
#endif