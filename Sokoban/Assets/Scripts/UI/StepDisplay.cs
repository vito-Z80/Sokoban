using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StepDisplay : MonoBehaviour
    {
        public static Action<int> OnStepDisplay;
        Text m_text;

        void OnEnable()
        {
            OnStepDisplay += Display;
        }

        void Display(int obj)
        {
            m_text.text = obj.ToString();
        }

        void Start()
        {
            m_text = GetComponent<Text>();
        }


        void OnDisable()
        {
            OnStepDisplay -= Display;
        }
    }
}