using Interfaces;
using UnityEngine;

namespace Objects
{
    public class ColoredDoor : MainObject, IInteracting
    {
        Vector3 m_closedDoorPosition;
        Vector3 m_openDoorPosition;
        Vector3 m_targetPosition;

        void Start()
        {
            m_targetPosition = transform.position;
            m_closedDoorPosition = transform.position;
            m_openDoorPosition = transform.position + Vector3.down * 0.98f;
        }

        void Update()
        {
            if (m_targetPosition == transform.position) return;
            transform.position = Vector3.MoveTowards(transform.position, m_targetPosition, Time.deltaTime * Global.Instance.gameSpeed);
        }

        public void Affect(bool affect)
        {
            m_targetPosition = affect ? m_openDoorPosition : m_closedDoorPosition;
        }
    }
}