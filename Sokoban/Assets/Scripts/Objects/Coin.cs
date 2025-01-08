using System;
using Level;
using UnityEngine;

namespace Objects
{
    public class Coin : MonoBehaviour
    {
        float m_angle;
        bool m_pickedUp;
        Vector3 m_velocity;
        Vector3 m_removePosition;
        BoxCollider m_boxCollider;

        void Start()
        {
            m_boxCollider = GetComponent<BoxCollider>();
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

        void OnTriggerEnter(Collider other)
        {
            other.TryGetComponent<Assembler>(out var character);
            if (character is null) return;
            m_pickedUp = true;
            LevelManager.AvailableMovesBack++;
        }
    }
}