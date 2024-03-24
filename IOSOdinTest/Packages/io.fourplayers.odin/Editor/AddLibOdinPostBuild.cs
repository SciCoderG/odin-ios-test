using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using UnityEditor.iOS.Xcode;
using UnityEditor.iOS.Xcode.Extensions;

public class AddLibOdinPostBuild 
{
#if UNITY_EDITOR && UNITY_IOS

    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        if (target == BuildTarget.iOS)
        {
            // Path to the Xcode project
            string projPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);

            PBXProject proj = new PBXProject();
            proj.ReadFromString(File.ReadAllText(projPath));

            string targetGuid = proj.GetUnityMainTargetGuid();

            // Path to the Odin.framework relative to the Unity project, adjust as necessary
            string frameworkPath = "Frameworks/libodin.dylib";

            // Add the framework to the project
            // proj.AddFrameworkToProject(targetGuid, "libodin.dylib", false);
            Debug.Log("Adding framework libodin.dylib to project");


            string fullPackagePath =
                Path.GetFullPath("Packages/io.fourplayers.odin/Plugins/ios/universal/libodin.dylib");
            Debug.Log($"Received full package path: {fullPackagePath}");
            // If you need to copy the framework into the Xcode project, you might also need to use:
            string fileGuid = proj.AddFile(fullPackagePath, "Frameworks/libodin.dylib", PBXSourceTree.Source);
            proj.AddFileToEmbedFrameworks(targetGuid, fileGuid);


            // Save the changes to the Xcode project
            File.WriteAllText(projPath, proj.WriteToString());
        }
    }
    #endif
}