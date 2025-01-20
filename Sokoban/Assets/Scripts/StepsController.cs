using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Interfaces;
using UnityEngine;

public class StepsController
{
    //  TODO наверное сделать монобехом и на сцену закинуть, либо синглтон.

    readonly List<IUndo> m_undoObjects = new(128);

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
            obj.Stack.Clear();
        }

        m_undoObjects.Clear();
        go.GetComponentsInChildren(true, m_undoObjects);
        m_undoObjects.Add(m_assembler);
        Debug.Log(m_undoObjects.Count);
    }

    void Push()
    {
        foreach (var undo in m_undoObjects)
        {
            if (undo.Stack is null) continue;
            undo?.Push();
        }
    }

    void Pop()
    {
        if (Global.Instance.gameState.movesBack == 0 || Global.Instance.levelPhase != LevelPhase.SearchSolution) return;

        var canPop = m_undoObjects.Aggregate(false, (current, mo) => current | mo.Stack.Count > 0);
        if (!canPop) return;


        Global.Instance.gameState.movesBack--;
        Global.Instance.gameState.steps--;

        foreach (var undo in m_undoObjects)
        {
            // if (undo == null) continue;
            undo?.Pop();
        }
    }
}