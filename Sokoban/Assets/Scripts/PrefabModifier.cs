using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class PrefabModifier
{
    static PrefabModifier()
    {
        EditorApplication.playModeStateChanged += StateChange;
    }

    static void StateChange(PlayModeStateChange obj)
    {
        string[] levelGuids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Prefabs/Levels" });

        Debug.Log(string.Join("\n", levelGuids));

        switch (obj)
        {
            case PlayModeStateChange.EnteredPlayMode:
                ModifyLevelPrefabs(levelGuids);
                break;
            case PlayModeStateChange.ExitingPlayMode:
                RestoreLevelPrefabs(levelGuids);
                break;
        }
    }


    static void ModifyLevelPrefabs(string[] guids)
    {
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            prefab.SetActive(false);
            Debug.Log($"Modifying prefab: [{prefab.name}][{prefab.activeSelf}] {path}");

        }

    }

    static void RestoreLevelPrefabs(string[] guids)
    {
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            prefab.SetActive(true);
            Debug.Log($"Restoring prefab: [{prefab.name}][{prefab.activeSelf}] {path}");
        }
    }

    static Vector3[] SavePositions(Transform[] transforms)
    {
        return transforms.Select(transform => transform.position).ToArray();
    }

    static void RestorePositions(Transform[] transforms, Vector3[] positions)
    {
        if (positions.Length != transforms.Length)
        {
            throw new Exception("There must be the same number of transforms");
        }

        for (var i = 0; i < transforms.Length; i++)
        {
            transforms[i].position = positions[i];
        }
    }
}