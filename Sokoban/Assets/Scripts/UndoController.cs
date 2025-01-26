using System.Linq;
using Data;
using Interfaces;
using Unity.VisualScripting;
using UnityEngine;

public static class UndoController
{

    static GameObject[] m_undoObjects;
    
    public static void CollectUndoableObjects(GameObject go, Assembler character)
    {
        m_undoObjects = go.GetComponentsInChildren<MonoBehaviour>(true)
            .Where(o => o is IUndo )
            .Select(o => o.gameObject)
            .Append(character.gameObject)
            .ToArray();
    }

    public static void Push()
    {
        foreach (var go in m_undoObjects)
        {
            if (go is null || go.IsDestroyed()) continue;
            if (go.TryGetComponent<IUndo>(out var component))
            {
                component.Push();
            }
        }
    }

    public static void Pop()
    {
        if (Global.Instance.gameState.movesBack == 0 || Global.Instance.levelPhase != LevelPhase.SearchSolution) return;

        // var canPop = m_undoObjects.Aggregate(false, (current, mo) => current | mo.Stack.Count > 0);
        // if (!canPop) return;


        Global.Instance.gameState.movesBack--;
        Global.Instance.gameState.steps--;

        foreach (var go in m_undoObjects)
        {
            if (go is null || go.IsDestroyed()) continue;
            if (go.TryGetComponent<IUndo>(out var component))
            {
                component.Pop();
            }
        }
    }
}