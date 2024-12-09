using System;
using UnityEngine;

namespace Objects
{
    public class MutagenFloor : MonoBehaviour
    {
        Vector3 m_targetPosition;
        float m_startTime;
        bool m_isMaterialize;

        void Start()
        {
            // gameObject.SetActive(false);
        }

        public void Init(Vector3 startPosition, Vector3 targetPosition, float startTime)
        {
            transform.position = startPosition;
            m_targetPosition = targetPosition;
            m_startTime = startTime;
            // gameObject.SetActive(true);
        }


        void Update()
        {
            m_isMaterialize = Mutagen();
        }


        public bool IsMaterialize() => m_isMaterialize;

        bool Mutagen()
        {
            m_startTime -= Time.deltaTime;
            if (m_startTime <= 0.0f)
            {
                transform.position = Vector3.MoveTowards(transform.position, m_targetPosition, Time.deltaTime * 1.5f);
                var distance = Vector3.Distance(transform.position, m_targetPosition);

                if (distance < 0.01f)
                {
                    transform.position = m_targetPosition;
                    return false;
                }
            }

            return true;
        }
    }
}