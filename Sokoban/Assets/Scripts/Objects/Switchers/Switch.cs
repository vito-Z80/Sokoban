using System;
using Data;
using JetBrains.Annotations;
using UnityEngine;

namespace Objects.Switchers
{
    public abstract class Switch : MonoBehaviour
    {
        [SerializeField] SwitcherColor color;
        public bool isOn;
        [CanBeNull] Collider m_collider;
        
        public Action<Switch> OnSwich;

        void Awake()
        {
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
            Debug.Log(other.gameObject.name);
        }

        void OnTriggerExit(Collider other)
        {
            if (m_collider == other)
            {
                isOn = false;
                UnTouch();
                OnSwich?.Invoke(this);
            }
        }
    }
}