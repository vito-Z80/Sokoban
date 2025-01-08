using System;
using System.Collections.Generic;
using Objects;
using UI;
using Unity.VisualScripting;
using UnityEngine;

public class StepsController
{
    readonly List<MainObject> m_steps = new(128);

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
        foreach (var obj in m_steps)
        {
            obj.ClearStack();
        }
        m_steps.Clear();
        go.GetComponentsInChildren(m_steps);
        m_steps.Add(m_assembler);
        Debug.Log(m_steps.Count);
    }

    void Push()
    {
        foreach (var undo in m_steps)
        {
            if (!undo.gameObject.IsDestroyed())
            {
                undo.PushState();
            }
        }
    }

    void Pop()
    {
        if (Assembler.Step > 0)
        {
            StepDisplay.OnStepDisplay?.Invoke(--Assembler.Step);
        }
        foreach (var undo in m_steps)
        {
            undo?.PopState();
        }
    }
}