using UnityEngine;

namespace Objects.CollectibleObjects
{
    public class Coin : Collectible
    {
        float m_angle;
        bool m_pickedUp;
        Vector3 m_velocity;
        Vector3 m_removePosition;

        void Start()
        {
            m_removePosition = transform.position + Vector3.up * 20.0f;
        }

        void Update()
        {
            m_angle += Time.deltaTime * 192.0f;
            RotateCoin();
            if (m_pickedUp)
            {
                RemoveCoin();
                if (Vector3.Distance(transform.position, m_removePosition) < 0.1f)
                {
                    Destroy(gameObject);
                }
            }
        }

        void RemoveCoin()
        {
            transform.position = Vector3.SmoothDamp(transform.position, m_removePosition, ref m_velocity, 0.75f);
        }


        void RotateCoin()
        {
            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles.x,
                m_angle,
                transform.rotation.eulerAngles.z
            );
        }
        
        public override bool Collect()
        {
            Global.Instance.gameSpeed += 0.5f;
            return m_pickedUp = true;
        }
    }
}