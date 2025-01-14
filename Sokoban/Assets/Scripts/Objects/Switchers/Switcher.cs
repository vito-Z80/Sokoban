using System;
using Data;
using JetBrains.Annotations;
using UnityEngine;

namespace Objects.Switchers
{
    public abstract class Switcher : MainObject
    {
        [SerializeField] SwitcherColor color;
        public bool isOn;
        [CanBeNull] Collider m_collider;

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
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        protected abstract void Touch();
        protected abstract void UnTouch();



        void OnTriggerEnter(Collider other)
        {
            m_collider = other;
            Touch();
        }

        void OnTriggerExit(Collider other)
        {
            if (m_collider == other)
            {
                UnTouch();
            }
        }
    }
}