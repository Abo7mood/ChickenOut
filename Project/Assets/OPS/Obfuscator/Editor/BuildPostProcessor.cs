#if UNITY_EDITOR

// System
using System;
using System.Linq;

// Unity
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;

namespace OPS.Obfuscator
{

#if UNITY_2018_2_OR_NEWER
    public class BuildPostProcessor : IPreprocessBuildWithReport, IPostBuildPlayerScriptDLLs, IFilterBuildAssemblies
#else
	public class BuildPostProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport
#endif
    {
        // Defines if an Obfuscation Process took place.
        private static bool hasObfuscated = false;

        //Revert Unity Assets and external Assemblies, if postprocess got not called or update got cleared!
        [InitializeOnLoad]
        public static class OnInitializeOnLoad
        {
            static OnInitializeOnLoad()
            {
                EditorApplication.update += RestoreAssemblies;
            }
        }

        public int callbackOrder
        {
            get { return int.MaxValue; }
        }

#if UNITY_2018_2_OR_NEWER
        public string[] OnFilterAssemblies(BuildOptions _BuildOptions, string[] _Assemblies)
        {
            if (UnityEngine.Debug.isDebugBuild)
            {
                // Return all assemblies, include OPS.Obfuscator.dll in Debug/Development build.
                return _Assemblies;
            }
            else
            {
				// TODO: Currently a bug.
                // Remove OPS.Obfuscator.dll in Release build.
                // String[] var_Returned_Assemblies = _Assemblies.Where(a => !a.Contains("OPS.Obfuscator.dll")).ToArray();

                // return var_Returned_Assemblies;
				return _Assemblies;
            }
        }
#endif

        public void OnPreprocessBuild(BuildReport report)
        {
            // Settings
            OPS.Obfuscator.Editor.Settings.Unity.Editor.EditorSettings var_EditorSettings = new Editor.Settings.Unity.Editor.EditorSettings();
            OPS.Obfuscator.Editor.Settings.Unity.Build.BuildSettings var_BuildSettings = new Editor.Settings.Unity.Build.BuildSettings();
            var_BuildSettings.BuildTarget = UnityEditor.EditorUserBuildSettings.activeBuildTarget;

            // PreBuild
            bool var_NoError = OPS.Obfuscator.Editor.Obfuscator.PreBuild(var_EditorSettings, var_BuildSettings);
        }

        //Obfuscate Assemblies after first scene build.
        [PostProcessScene(1)]
        public static void OnPostProcessScene()
        {
            if (!hasObfuscated)
            {
                if (BuildPipeline.isBuildingPlayer && !EditorApplication.isPlayingOrWillChangePlaymode)
                {
                    try
                    {
                        UnityEditor.EditorApplication.LockReloadAssemblies();

                        // Settings
                        OPS.Obfuscator.Editor.Settings.Unity.Editor.EditorSettings var_EditorSettings = new Editor.Settings.Unity.Editor.EditorSettings();
                        OPS.Obfuscator.Editor.Settings.Unity.Build.BuildSettings var_BuildSettings = new Editor.Settings.Unity.Build.BuildSettings();
                        var_BuildSettings.BuildTarget = UnityEditor.EditorUserBuildSettings.activeBuildTarget;

                        // PreBuild
                        bool var_NoError = OPS.Obfuscator.Editor.Obfuscator.Obfuscate(var_EditorSettings, var_BuildSettings);

                        if (var_NoError)
                        {
                            EditorApplication.update += RestoreAssemblies;
                        }

                        hasObfuscated = true;
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogError("[OPS] Error: " + e.ToString());
                    }
                    finally
                    {
                        UnityEditor.EditorApplication.UnlockReloadAssemblies();
                    }
                }
            }
        }

        //Revert Unity Assets and external Assemblies. 
#if UNITY_2018_2_OR_NEWER
        public void OnPostBuildPlayerScriptDLLs(BuildReport report)
#else
		public void OnPostprocessBuild(BuildReport report)
#endif
        {
            if (hasObfuscated)
            {
                RestoreAssemblies();
            }

            RefreshAll();
        }

        private static void RestoreAssemblies()
        {
            if (BuildPipeline.isBuildingPlayer == false)
            {
                try
                {
                    // Settings
                    OPS.Obfuscator.Editor.Settings.Unity.Editor.EditorSettings var_EditorSettings = new Editor.Settings.Unity.Editor.EditorSettings();

                    // PreBuild
                    bool var_NoError = OPS.Obfuscator.Editor.Obfuscator.PostBuild(var_EditorSettings);

                    EditorApplication.update -= RestoreAssemblies;
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogWarning("[OPS.OBF] " + e.ToString());
                }
            }
        }

        public static void ManualRestore()
        {
            RestoreAssemblies();
        }

        private static void RefreshAll()
        {
            hasObfuscated = false;
        }
    }
}
#endif