using System;
using Data;
using Level.Tasks;
using Objects.Boxes;
using UnityEngine;

namespace Objects
{
    [RequireComponent(typeof(AudioSource))]
    public class ContactorBoxContainer : MainObject
    {
        [SerializeField] public BoxColor pointColor;
        [SerializeField] ParticleSystem magicPoint;
        [SerializeField] ParticleSystem whirlCube;
        bool m_contacted;

        Box m_contactBox;

        AudioSource m_audioSource;

        //  TODO 2 рядом стоящие точки одного цвета затригеряться если с одной на другую перетащить коробку их цвета.
        //   Нужно отключать контакт точки как только начинается движение коробки с нее, и включать контакт как коробка полностью встала на точку.


        void Start()
        {
            m_audioSource = GetComponent<AudioSource>();
        }

        public bool GetContact()
        {
            return m_contacted;
        }

        void PlayEffectMagicPoint()
        {
            if (magicPoint.isPlaying)
            {
                magicPoint.Stop(true);
            }

            magicPoint.Play();
            m_audioSource.clip = Global.Instance.boxOnPointSound;
            m_audioSource.Play();
        }

        public void PlayEffectWhirlCube()
        {
            if (whirlCube.isPlaying)
            {
                whirlCube.Stop(true);
            }

            whirlCube.Play();
        }


        void Update()
        {
            if (m_contacted || m_contactBox is null) return;

            // if (m_contactBox == null)
            // {
            //     m_contactBox = null;
            //     m_contacted = false;
            //     return;
            // }

            if (
                Mathf.Abs(m_contactBox.transform.position.x - transform.position.x) < 0.1f &&
                Mathf.Abs(m_contactBox.transform.position.z - transform.position.z) < 0.1f
            )
            {
                m_contacted = true;
                TaskOneTimeActivationPoints.OnPointContact?.Invoke();
                PlayEffectMagicPoint();
            }
        }


        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Box>(out var box))
            {
                if (box.boxColor == pointColor)
                {
                    m_contactBox = box;
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            m_contacted = false;
            m_contactBox = null;
            TaskOneTimeActivationPoints.OnPointContact?.Invoke();
        }
    }
}