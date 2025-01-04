using UnityEngine;

namespace Bridge
{
    public class BridgeFloorCell : MonoBehaviour
    {
        Quaternion m_to;

        bool m_isUpdate;
        float m_waitToStart;

        float m_axis = 180.0f;

        public void Init(Vector3 position, float waitToStart, Vector3 forward)
        {
            if (Mathf.Approximately(m_axis, 180.0f))
            {
                transform.rotation = Quaternion.LookRotation(forward, transform.up);
            }

            transform.position = position;
            var rotationStep = Quaternion.AngleAxis(m_axis, transform.right);
            m_to = rotationStep * transform.rotation;

            m_waitToStart = waitToStart;
            m_axis = -m_axis;
            m_isUpdate = true;
        }


        public void Show()
        {
            // if (transform.rotation == Quaternion.identity) return;
            if (!m_isUpdate) return;
            m_waitToStart -= Time.deltaTime;
            if (m_waitToStart > 0.0f) return;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, m_to, Time.deltaTime * 128.0f);
            m_isUpdate = transform.rotation != m_to;
        }

        // void Hide()
        // {
        //     if (Mathf.Approximately(transform.rotation.eulerAngles.x, m_invisible.x)) return;
        //     transform.RotateAround(transform.position, Vector3.right, Time.deltaTime * 4.0f);
        //     if (transform.rotation.eulerAngles.x <= m_invisible.x)
        //     {
        //         transform.rotation = m_invisible;
        //     }
        // }
    }
}