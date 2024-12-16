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

        bool m_checkState = false;


        void OnEnable()
        {
            if (mask.Length != complexObjects.Length)
            {
                throw new Exception($"Complex switches must have the same number of mask objects. [{GetType().Name}].");
            }

            foreach (var switcher in complexObjects)
            {
                switcher.OnSwitcherChanged += CheckSwitches;
            }
        }

        void OnDestroy()
        {
            foreach (var switcher in complexObjects)
            {
                switcher.OnSwitcherChanged -= CheckSwitches;
            }
        }


        void CheckSwitches()
        {
            var checkState = AllStandBy();
            if (checkState == m_checkState) return;
            m_checkState = checkState;
            SwitchAll();
        }

        bool AllStandBy()
        {
            for (var i = 0; i < mask.Length; i++)
            {
                if (mask[i] != complexObjects[i].IsPushed()) return false;
            }

            return true;
        }

        void SwitchAll()
        {
            foreach (var affectObject in affectObjects)
            {
                affectObject.Execute();
            }
        }
    }
}