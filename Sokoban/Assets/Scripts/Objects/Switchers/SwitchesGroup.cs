using System;
using UnityEngine;

namespace Objects.Switchers
{
    public class SwitchesGroup : MonoBehaviour
    {
        [Header("Группа переключателей активирующая подконтрольные объекты.")] [SerializeField]
        Switcher[] complexObjects;

        [Header("Маска по которой должна сработать активация.")] [SerializeField]
        bool[] mask;

        [Space(8)] [Header("Подконтрольные объекты группы переключателей.")] [SerializeField]
        Controlled[] affectObjects;

        void OnEnable()
        {
            if (mask.Length != complexObjects.Length)
            {
                throw new Exception($"Complex switches must have the same number of mask objects. [{GetType().Name}].");
            }
        }
        
        void LateUpdate()
        {
            if (IsAllActivated())
            {
                foreach (var affectObject in affectObjects)
                {
                    affectObject.Activate();
                }
            }
            else
            {
                foreach (var affectObject in affectObjects)
                {
                    affectObject.Deactivate();
                }
            }
        }

        bool IsAllActivated()
        {
            for (var i = 0; i < mask.Length; i++)
            {
                if (mask[i] != complexObjects[i].isOn) return false;
            }

            return true;
        }
    }
}