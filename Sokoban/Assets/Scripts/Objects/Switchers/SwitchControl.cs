using System;
using UnityEngine;

namespace Objects.Switchers
{
    public class SwitchControl : MonoBehaviour
    {
        [Header("Переключатели активирующие объекты взаимодействия.")] [SerializeField]
        protected Switcher[] switchesUsed;

        [Space(8)] [Header("Объекты взаимодействия контролируемые переключателями.")] [SerializeField]
        protected MonoBehaviour[] interacting;

        protected Switcher[] AllSwitches;

        void OnValidate()
        {
            if (interacting == null || interacting.Length == 0) return;
            for (var i = 0; i < interacting.Length; i++)
            {
                if (interacting[i] is null || interacting[i] is IInteracting) continue;
                Debug.LogError($"Interacting[{i}] должен иметь интерфейс IInteracting");
            }
        }

        void OnEnable()
        {
            AllSwitches = GetComponentsInChildren<Switcher>(true);
        }
    }
}