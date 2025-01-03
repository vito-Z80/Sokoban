using System;
using UnityEngine;

namespace Objects.Switchers
{
    public class Switcher : MainObject
    {
        bool m_isPushed;
        public event Action OnSwitcherChanged;
        Vector3 m_startPosition;


        void OnEnable()
        {
            m_startPosition = transform.position;
            PushOut();
        }

        public bool IsPushed()
        {
            return m_isPushed;
        }

        public void Switch()
        {
            m_isPushed = !m_isPushed;
            OnSwitcherChanged?.Invoke();
            if (m_isPushed)
            {
                PushIn();
            }
            else
            {
                PushOut();
            }
        }

        void PushIn()
        {
            targetPosition = m_startPosition + Vector3.down * 0.04f;
        }

        void PushOut()
        {
            targetPosition = m_startPosition;
        }

        void OnTriggerEnter(Collider other)
        {
            Switch();
        }

        void OnTriggerExit(Collider other)
        {
            Switch();
        }

    }
}