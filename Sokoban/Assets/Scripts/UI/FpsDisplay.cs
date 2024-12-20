using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FpsDisplay : MonoBehaviour
    {
        Text m_text;
        float m_deltaTime;


        void Start()
        {
            m_text = GetComponent<Text>();
        }


        void Update()
        {
            m_deltaTime += (Time.deltaTime - m_deltaTime) * 0.1f;
            var fps = 1.0f / m_deltaTime;
            m_text.text = $"FPS: {Mathf.RoundToInt(fps)}";
        }
    }
}