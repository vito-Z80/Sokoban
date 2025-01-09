using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Objects;
using Unity.VisualScripting;
using UnityEngine;

public class StepsController
{
    //  TODO наверное сделать монобехом и на сцену закинуть, либо синглтон.

    readonly List<MainObject> m_undoObjects = new(128);

    public static Action OnPush;
    public static Action OnPop;
    readonly Assembler m_assembler;

    public StepsController(Assembler assembler)
    {
        m_assembler = assembler;
        OnPush += Push;
        OnPop += Pop;
    }


    public void Dispose()
    {
        OnPush -= Push;
        OnPop -= Pop;
    }

    public void CollectMainObjects(GameObject go)
    {
        foreach (var obj in m_undoObjects)
        {
            obj.ClearStack();
        }

        m_undoObjects.Clear();
        go.GetComponentsInChildren(m_undoObjects);
        m_undoObjects.Add(m_assembler);
    }

    void Push()
    {
        foreach (var mo in m_undoObjects)
        {
            if (mo == null || mo.gameObject == null || mo.gameObject.IsDestroyed()) continue;
            mo.PushState();
        }
    }

    void Pop()
    {
        if (Global.Instance.gameState.movesBack == 0 || Global.Instance.levelPhase != LevelPhase.SearchSolution) return;

        var canPop = m_undoObjects.Aggregate(false, (current, mo) => current | mo.StackCount() > 0);
        if (!canPop) return;


        Global.Instance.gameState.movesBack--;
        Global.Instance.gameState.steps--;

        foreach (var mo in m_undoObjects)
        {
            if (mo == null || mo.gameObject == null || mo.gameObject.IsDestroyed()) continue;
            mo.PopState();
        }
    }
}