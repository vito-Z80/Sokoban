using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Objects
{
    public class ContactorBoxContainer : MainObject
    {
        [SerializeField] public BoxColor boxColor;
        bool m_contacted;

        readonly Stack<bool> m_boolStack = new();

        public bool GetContact() => m_contacted;


        public void SubmitContact()
        {
            m_contacted = true;
        }

        public void BreakContact()
        {
            m_contacted = false;
        }


        public override void ClearStack()
        {
            m_boolStack.Clear();
        }

        public override void PopState()
        {
            if (m_boolStack.Count == 0) return;
            m_contacted = m_boolStack.Pop();
        }

        public override void PushState()
        {
            m_boolStack.Push(m_contacted);
        }
    }
}