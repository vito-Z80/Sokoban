using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Objects.Switchers
{
    public abstract class Switcher : MainObject
    {
        public bool isOn;

        void Awake()
        {
            if (TryGetComponent<BoxCollider>(out _)) return;
            throw new ArgumentNullException("Switcher is missing a " + nameof(BoxCollider));
        }

        protected abstract void Touch();
        protected abstract void UnTouch();


        [CanBeNull] Collider m_collider;

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