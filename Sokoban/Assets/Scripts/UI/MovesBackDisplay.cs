using TMPro;
using UnityEngine;

namespace UI
{
    public class MovesBackDisplay : MonoBehaviour
    {
        TextMeshProUGUI m_text;

        int m_lastMovesBack;
        
        void Start()
        {
            m_text ??= GetComponent<TextMeshProUGUI>();
            m_text.text = $"{Global.Instance.gameState.movesBack}";
        }

        void Update()
        {
            if (m_lastMovesBack == Global.Instance.gameState.movesBack) return;
            m_lastMovesBack = Global.Instance.gameState.movesBack;
            m_text.text = $"{m_lastMovesBack}";
        }
    }
}