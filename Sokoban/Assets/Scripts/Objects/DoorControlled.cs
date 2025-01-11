using Objects.Switchers;
using UnityEngine;

namespace Objects
{
    public class DoorControlled : Controlled
    {
        Vector3 m_closedDoorPosition;
        Vector3 m_openDoorPosition;

        void Start()
        {
            targetPosition = transform.position;
            m_closedDoorPosition = transform.position;
            m_openDoorPosition = transform.position + Vector3.down * 0.98f;
        }


        public override void Activate()
        {
            targetPosition = m_openDoorPosition;
        }
        
        public override void Deactivate()
        {
            targetPosition = m_closedDoorPosition;
        }

        void Update()
        {
            Move(Time.deltaTime);
        }


        public override void PushState()
        {
            
        }

        public override void PopState()
        {
            
        }
    }
}