using System;
using System.Collections.Generic;
using Objects;
using UnityEngine;

public class StepsController
{
    readonly List<MainObject> m_steps = new();

    public static Action OnPush;
    public static Action OnPop;
    Assembler m_assembler;

    public StepsController(Assembler assembler)
    {
        m_assembler = assembler;
        OnPush += Push;
        OnPop += Pop;
    }


    ~StepsController()
    {
        Debug.Log("Undos destructor");
        OnPush -= Push;
        OnPop -= Pop;
    }

    public void CollectMainObjects(GameObject go)
    {
        m_steps.Clear();
        go.GetComponentsInChildren(m_steps);
        m_steps.Add(m_assembler);
        Debug.Log(m_steps.Count);
    }

    void Push()
    {
        foreach (var undo in m_steps)
        {
            undo.Push();
            
        }
    }

    void Pop()
    {
        foreach (var undo in m_steps)
        {
            undo.Pop();
        }
    }
}