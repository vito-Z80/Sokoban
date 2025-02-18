using System;
using Data;
using JetBrains.Annotations;
using UnityEngine;

namespace Objects.Switchers
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class Switch : MonoBehaviour
    {
        
        [SerializeField] SwitcherColor color;
        public bool isOn;
        [CanBeNull] Collider m_collider;
        
        public Action<Switch> OnSwich;

        
        AudioSource m_audio;
        
        void Awake()
        {
            m_audio = GetComponent<AudioSource>();
            if (TryGetComponent<BoxCollider>(out _)) return;
            throw new ArgumentNullException("Switcher is missing a " + nameof(BoxCollider));
        }

        protected Color GetColor()
        {
            switch (color)
            {
                case SwitcherColor.Blue:
                    return Color.blue;
                case SwitcherColor.Red:
                    return Color.red;
                case SwitcherColor.Green:
                    return Color.green;
                case SwitcherColor.White:
                    return Color.white;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        protected abstract void Touch();
        protected abstract void UnTouch();



        void OnTriggerEnter(Collider other)
        {;
            isOn = true;
            m_collider = other;
            Touch();
            OnSwich?.Invoke(this);
            m_audio.clip = Global.Instance.buttonDownSound;
            m_audio.Play();
        }

        void OnTriggerExit(Collider other)
        {
            if (m_collider == other)
            {
                isOn = false;
                UnTouch();
                OnSwich?.Invoke(this);
                m_audio.clip = Global.Instance.buttonUpSound;
                m_audio.Play();
            }
        }
    }
}