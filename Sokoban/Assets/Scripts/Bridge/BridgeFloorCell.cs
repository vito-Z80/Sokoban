using UnityEngine;

namespace Bridge
{
    public class BridgeFloorCell : MonoBehaviour
    {
        Quaternion m_to;

        bool m_hideAfterUpdate;
        bool m_isUpdate;
        float m_waitToStart;

        float m_axis = 180.0f;

        public void Init(Vector3 position, float waitToStart, Vector3 forward, bool hideAfterUpdate)
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
            m_hideAfterUpdate = hideAfterUpdate;
        }


        public void Show()
        {
            if (!m_isUpdate)
            {
                if (m_hideAfterUpdate) gameObject.SetActive(false);
                return;
            }
            m_waitToStart -= Time.deltaTime;
            if (m_waitToStart > 0.0f) return;
            gameObject.SetActive(true);
            Rotate();
            m_isUpdate = transform.rotation != m_to;
        }
        void Rotate()
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, m_to, Time.deltaTime * 128.0f);
        }
    }
}