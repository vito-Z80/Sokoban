// using UnityEngine;
// #if UNITY_EDITOR
// using UnityEditor;
// using UnityEditor.Build;
// using UnityEditor.Build.Reporting;
// using UnityEngine.SceneManagement;
// #endif
//
// public class PrefabModifier : MonoBehaviour
// {
// #if UNITY_EDITOR
//     // ======== Обработка Play Mode ========
//
//     [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
//     private static void RegisterPlayModeHandler()
//     {
//         EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
//     }
//
//     private static void OnPlayModeStateChanged(PlayModeStateChange state)
//     {
//         var prefabGUIDs = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Prefabs/Levels" });
//
//         if (state == PlayModeStateChange.EnteredPlayMode)
//         {
//             SaveAndDisablePrefabs(prefabGUIDs);
//         }
//         else if (state == PlayModeStateChange.ExitingPlayMode)
//         {
//             RestorePrefabs(prefabGUIDs);
//         }
//     }
//
//     // ======== Обработка сборки ========
//
//     private class DisablePrefabsBuildProcessor : IProcessSceneWithReport
//     {
//         public int callbackOrder => 0;
//
//         public void OnProcessScene(Scene scene, BuildReport report)
//         {
//             var prefabGUIDs = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Prefabs/Levels" });
//             SaveAndDisablePrefabs(prefabGUIDs);
//         }
//     }
//
//     [InitializeOnLoadMethod]
//     private static void RestoreAfterBuild()
//     {
//         EditorApplication.update += () =>
//         {
//             if (!BuildPipeline.isBuildingPlayer)
//             {
//                 var prefabGUIDs = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Prefabs/Levels" });
//                 RestorePrefabs(prefabGUIDs);
//
//                 EditorApplication.update -= RestoreAfterBuild;
//             }
//         };
//     }
//
//     // ======== Основные методы ========
//
//      static void SaveAndDisablePrefabs(string[] prefabGUIDs)
//     {
//         foreach (var guid in prefabGUIDs)
//         {
//             string path = AssetDatabase.GUIDToAssetPath(guid);
//             GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
//
//             if (prefab != null)
//             {
//                 prefab.SetActive(false);
//             }
//         }
//     }
//
//      static void RestorePrefabs(string[] prefabGUIDs)
//     {
//         foreach (var guid in prefabGUIDs)
//         {
//             string path = AssetDatabase.GUIDToAssetPath(guid);
//             GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
//
//             if (prefab != null)
//             {
//                 prefab.SetActive(true);
//             }
//         }
//     }
// #endif
// }
//
//
// // [InitializeOnLoad]
// // public static class PrefabModifier
// // {
// //     static PrefabModifier()
// //     {
// //         EditorApplication.playModeStateChanged += StateChange;
// //     }
// //
// //     static void StateChange(PlayModeStateChange obj)
// //     {
// //         string[] levelGuids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Prefabs/Levels" });
// //
// //         Debug.Log(string.Join("\n", levelGuids));
// //
// //         switch (obj)
// //         {
// //             case PlayModeStateChange.EnteredPlayMode:
// //                 ModifyLevelPrefabs(levelGuids);
// //                 break;
// //             case PlayModeStateChange.ExitingPlayMode:
// //                 RestoreLevelPrefabs(levelGuids);
// //                 break;
// //         }
// //     }
// //
// //
// //     static void ModifyLevelPrefabs(string[] guids)
// //     {
// //         foreach (var guid in guids)
// //         {
// //             var path = AssetDatabase.GUIDToAssetPath(guid);
// //             var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
// //             prefab.SetActive(false);
// //             Debug.Log($"Modifying prefab: [{prefab.name}][{prefab.activeSelf}] {path}");
// //
// //         }
// //
// //     }
// //
// //     static void RestoreLevelPrefabs(string[] guids)
// //     {
// //         foreach (var guid in guids)
// //         {
// //             var path = AssetDatabase.GUIDToAssetPath(guid);
// //             var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
// //             prefab.SetActive(true);
// //             Debug.Log($"Restoring prefab: [{prefab.name}][{prefab.activeSelf}] {path}");
// //         }
// //     }
// //
// //     static Vector3[] SavePositions(Transform[] transforms)
// //     {
// //         return transforms.Select(transform => transform.position).ToArray();
// //     }
// //
// //     static void RestorePositions(Transform[] transforms, Vector3[] positions)
// //     {
// //         if (positions.Length != transforms.Length)
// //         {
// //             throw new Exception("There must be the same number of transforms");
// //         }
// //
// //         for (var i = 0; i < transforms.Length; i++)
// //         {
// //             transforms[i].position = positions[i];
// //         }
// //     }
// // }
// // #endif