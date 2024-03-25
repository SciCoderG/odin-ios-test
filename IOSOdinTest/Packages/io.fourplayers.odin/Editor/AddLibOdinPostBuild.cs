using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using UnityEditor.iOS.Xcode;
using UnityEditor.iOS.Xcode.Extensions;

public class AddLibOdinPostBuild 
{
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        if (target == BuildTarget.iOS)
        {
            PBXProject pbxProject = new PBXProject();
            string projectPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
            pbxProject.ReadFromFile(projectPath);
            
            string unityFrameworkTarget = pbxProject.TargetGuidByName("UnityFramework");
            string odinFrameworkGuid = pbxProject.FindFileGuidByProjectPath("Frameworks/io.fourplayers.odin/Plugins/ios/universal/Odin.framework");
            pbxProject.RemoveFileFromBuild(unityFrameworkTarget, odinFrameworkGuid);
 
            File.WriteAllText(projectPath, pbxProject.WriteToString());
        }
    }
}