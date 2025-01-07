using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Objects
{
    public class ContactorBoxContainer : MainObject
    {
        [SerializeField] public BoxColor boxColor;
        bool m_contacted;

        readonly Stack<bool> m_stack = new();

        public bool GetContact() => m_contacted;


        public void SubmitContact()
        {
            m_contacted = true;
        }

        public void BreakContact()
        {
            m_contacted = false;
        }


        public override void Pop()
        {
            if (m_stack.Count == 0) return;
            m_contacted = m_stack.Pop();
        }

        public override void Push()
        {
            m_stack.Push(m_contacted);
        }
    }
}