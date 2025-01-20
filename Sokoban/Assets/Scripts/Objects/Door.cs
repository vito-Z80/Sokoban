using UnityEngine;

namespace Objects
{
    public class Door : MainObject
    {
        Vector3 m_targetPosition;
        BoxCollider m_boxCollider;

        bool m_isDoorMoving;

        void Start()
        {
            m_boxCollider = GetComponent<BoxCollider>();
        }

        void Update()
        {
            if (!m_isDoorMoving) return;
            transform.position = Vector3.MoveTowards(transform.position, m_targetPosition, Time.deltaTime * Global.Instance.gameSpeed);
            m_isDoorMoving = m_targetPosition != transform.position;
        }

        public void OpenDoor()
        {
            m_isDoorMoving = true;
            m_targetPosition = transform.position + transform.right;
        }

        public void CloseDoor()
        {
            m_isDoorMoving = true;
            m_boxCollider.size = new Vector3(2.5f, m_boxCollider.size.y, m_boxCollider.size.z);
            m_targetPosition = transform.position - transform.right;
        }
    }
}