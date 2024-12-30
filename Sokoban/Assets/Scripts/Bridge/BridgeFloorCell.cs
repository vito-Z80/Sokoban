using UnityEngine;

namespace Bridge
{
    public class BridgeFloorCell : MonoBehaviour
    {
        readonly Quaternion m_invisible = Quaternion.Euler(180, 0, 0);

        float m_waitToStart;

        void OnEnable()
        {
            transform.rotation = m_invisible;
        }

        public void Init(Vector3 position, float waitToStart)
        {
            transform.position = position;
            transform.rotation = m_invisible;
            m_waitToStart = waitToStart;
        }


        public void Show()
        {
            if (transform.rotation == Quaternion.identity) return;

            m_waitToStart -= Time.deltaTime;
            if (m_waitToStart > 0.0f) return;


            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.identity,
                Time.deltaTime * 4.0f
            );
        }

        void Hide()
        {
            if (Mathf.Approximately(transform.rotation.eulerAngles.x, m_invisible.x)) return;
            transform.RotateAround(transform.position, Vector3.right, Time.deltaTime * 4.0f);
            if (transform.rotation.eulerAngles.x <= m_invisible.x)
            {
                transform.rotation = m_invisible;
            }
        }
    }
}