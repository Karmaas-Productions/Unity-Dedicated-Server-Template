using UnityEditor;
using System.Collections.Generic;

public class BuildScript
{
    static string[] GetEnabledScenes()
    {
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        List<string> enabledScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in scenes)
        {
            if (scene.enabled)
            {
                enabledScenes.Add(scene.path);
            }
        }
        return enabledScenes.ToArray();
    }

    [MenuItem("Build/Build/Windows 64-bit")]
    static void BuildWindows64()
    {
        string projectName = "Terron"; // Set the name of your project here
        string outputPath = $"Builds/Windows64/{projectName}64Windows.exe";
        
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        
        buildPlayerOptions.scenes = GetEnabledScenes();
        buildPlayerOptions.locationPathName = outputPath;
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.subtarget = (int)StandaloneBuildSubtarget.Player;
        
        // Set the scripting backend to IL2CPP for Windows 64-bit
        //PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, ScriptingImplementation.IL2CPP);
        //PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, "WINDOWS64");

        // Disable development build options
        EditorUserBuildSettings.development = false;

        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }

    [MenuItem("Build/Build/Linux 64-bit Dedicated Server")]
    static void BuildLinux64DedicatedServer()
    {
        string projectName = "Terron"; // Set the name of your project here
        string outputPath = $"Builds/Linux64DedicatedServer/{projectName}64DedicatedServer.x86_64";
        
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        
        buildPlayerOptions.scenes = GetEnabledScenes();
        buildPlayerOptions.locationPathName = outputPath;
        buildPlayerOptions.target = BuildTarget.StandaloneLinux64;
        buildPlayerOptions.subtarget = (int)StandaloneBuildSubtarget.Server;

        // Set the scripting backend to IL2CPP for Linux 64-bit Dedicated Server
        //PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, ScriptingImplementation.IL2CPP);
        //PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, "DEDICATED_SERVER");
        
        // Disable development build options
        EditorUserBuildSettings.development = false;

        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }
}
