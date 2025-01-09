using TMPro;
using UnityEngine;

namespace UI
{
    public class StepDisplay : MonoBehaviour
    {
        TextMeshProUGUI m_text;
        int m_lastStep;

        void Start()
        {
            m_text ??= GetComponent<TextMeshProUGUI>();
            m_text.text = $"{Global.Instance.gameState.steps}";
        }

        void Update()
        {
            if (m_lastStep == Global.Instance.gameState.steps) return;
            m_lastStep = Global.Instance.gameState.steps;
            m_text.text = $"{m_lastStep}";
        }
    }
}