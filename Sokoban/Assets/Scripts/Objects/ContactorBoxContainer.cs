using Data;
using Objects.Boxes;
using UnityEngine;

namespace Objects
{
    public class ContactorBoxContainer : MainObject
    {
        [SerializeField] public BoxColor pointColor;
        bool m_contacted;

        public bool GetContact()
        {
            return m_contacted;
        }

        public override void ClearStack()
        {
        }

        public override void PopState()
        {
        }

        public override void PushState()
        {
        }


        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Box>(out var box))
            {
                if (box.boxColor == pointColor) m_contacted = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
                m_contacted = false;
        }
    }
}