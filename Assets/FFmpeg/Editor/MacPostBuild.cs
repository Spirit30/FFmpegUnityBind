#if UNITY_STANDALONE_OSX
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

    static void PostProcessBuild(string path)
    {
        string binarySource =
            Path.Combine(
                Application.dataPath,
                FFmpeg.StandaloneProxy.EDITOR_BINARY_PATH);

        string buildFolder = Path.GetDirectoryName(path);
        string binaryDestination = Path.Combine(buildFolder, "ffmpeg");

        File.Copy(binarySource, binaryDestination, true);
    }
}
#endif