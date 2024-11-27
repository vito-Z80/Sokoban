using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Objects
{
    public class Door : MonoBehaviour
    {
        Vector3 m_targetPosition;
        BoxCollider m_boxCollider;

        void Start()
        {
            m_boxCollider = GetComponent<BoxCollider>();
        }

        public async Task OpenDoor(GameObject target = null)
        {
            if (target is not null)
            {
                while (Vector3.Distance(target.transform.position, transform.position) > 3.0f)
                {
                    await Task.Yield();
                }
            }
            m_targetPosition = transform.position + transform.right;
            await MoveDoor();
        }

        public async Task CloseDoor(GameObject target = null)
        {
            if (target is not null)
            {
                var targetPosition = transform.position - transform.right + transform.forward;
                while (Vector3.Distance(targetPosition,target.transform.position) > 0.7f )
                {
                    await Task.Yield();
                }
                m_boxCollider.size = new Vector3(2.5f, m_boxCollider.size.y, m_boxCollider.size.z);
            }
            m_targetPosition = transform.position - transform.right;
            await MoveDoor();
        }


        async Task MoveDoor()
        {
            while (Vector3.Distance(transform.position, m_targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, m_targetPosition, Time.deltaTime);
                await Task.Yield();
            }

            transform.position = m_targetPosition;
        }
    }
}