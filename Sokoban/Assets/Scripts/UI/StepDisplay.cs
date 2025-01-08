using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class StepDisplay : MonoBehaviour
    {
        public static Action<int> OnStepDisplay;
        TextMeshProUGUI m_text;

        void OnEnable()
        {
            OnStepDisplay += Display;
            m_text.text = "0";
        }

        void Display(int obj)
        {
            m_text.text = obj.ToString();
        }

        void Start()
        {
            m_text = GetComponent<TextMeshProUGUI>();
        }


        void OnDisable()
        {
            OnStepDisplay -= Display;
        }
    }
}