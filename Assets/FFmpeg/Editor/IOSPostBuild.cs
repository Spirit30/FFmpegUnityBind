﻿#if UNITY_IOS
using UnityEngine;
using System.Collections;
using UnityEditor.Callbacks;
using UnityEditor;
using System.Diagnostics;
using System.IO;
using UnityEditor.iOS.Xcode;
using System.Linq;

public class IOSPostBuild
{
	[PostProcessBuild(1000)]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
	{
		PostProcessBuild(pathToBuiltProject);
	}

    private static void PostProcessBuild(string path)
    {
#region pbxproj
        string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

        // PBXProject class represents a project build settings file,
        // here is how to read that in.
        PBXProject proj = new PBXProject();
        proj.ReadFromFile(projPath);

        // This is the Xcode target in the generated project
        string target = proj.TargetGuidByName("Unity-iPhone");

        // Write PBXProject object back to the file
        //proj.AddBuildProperty(target, "ENABLE_BITCODE", "NO");

        proj.AddFrameworkToProject(target, "VideoToolbox.framework", false);
        proj.AddFrameworkToProject(target, "libz.tbd", false);
        proj.AddFrameworkToProject(target, "libbz2.tbd", false);
        proj.AddFrameworkToProject(target, "libiconv.tbd", false);

        proj.WriteToFile(projPath);
#endregion

#region info plist
        string plistPath = path + "/Info.plist";

		PlistDocument plist = new PlistDocument();
		plist.ReadFromFile(plistPath);
        plist.root.SetString("NSMicrophoneUsageDescription", "User can record himself and video");
		plist.WriteToFile(plistPath);
#endregion
    }
}
#endif