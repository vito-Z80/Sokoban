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
            Move(Time.deltaTime);
            m_isDoorMoving = targetPosition != transform.position;
        }

        public void OpenDoor()
        {
            m_isDoorMoving = true;
            targetPosition = transform.position + transform.right;
        }

        public void CloseDoor()
        {
            m_isDoorMoving = true;
            m_boxCollider.size = new Vector3(2.5f, m_boxCollider.size.y, m_boxCollider.size.z);
            targetPosition = transform.position - transform.right;
        }


        public override void Push()
        {
            
        }

        public override void Pop()
        {
            
        }
    }
}