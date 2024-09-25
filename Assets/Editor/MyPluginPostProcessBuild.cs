using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using UnityEditor.iOS.Xcode;
using System.IO;

public class MyPluginPostProcessBuild
{
    [PostProcessBuild]
    public static void ChangeXcodePlist(BuildTarget buildTarget, string pathToBuiltProject)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            // Get plist
            string plistPath = pathToBuiltProject + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));
           
            // Get root
            PlistElementDict rootDict = plist.root;
           
            // Add Camera Usage Description
            rootDict.SetString("NSCameraUsageDescription", 
                "This app requires camera access to detect hand gestures for element control.");

            // Add Microphone Usage Description
            rootDict.SetString("NSMicrophoneUsageDescription", 
                "This app requires microphone access for voice commands to control elements.");

            // Add Speech Recognition Usage Description
            rootDict.SetString("NSSpeechRecognitionUsageDescription", 
                "This app uses speech recognition to interpret voice commands for element control.");

            // Write to file
            File.WriteAllText(plistPath, plist.WriteToString());

            // Change project name
            string projectPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
            PBXProject project = new PBXProject();
            project.ReadFromFile(projectPath);

            string targetGuid = project.GetUnityMainTargetGuid();
            project.SetBuildProperty(targetGuid, "PRODUCT_NAME", "Movementals");
            project.SetBuildProperty(targetGuid, "PRODUCT_BUNDLE_IDENTIFIER", "CowoGreenFlag.Movementals");

            project.WriteToFile(projectPath);

            // Rename .xcodeproj folder
            string oldProjectName = Path.GetFileName(pathToBuiltProject);
            string newProjectName = "Movementals.xcodeproj";
            string oldProjectPath = Path.Combine(pathToBuiltProject, oldProjectName);
            string newProjectPath = Path.Combine(pathToBuiltProject, newProjectName);

            if (Directory.Exists(oldProjectPath))
            {
                Directory.Move(oldProjectPath, newProjectPath);
            }
        }
    }
}

