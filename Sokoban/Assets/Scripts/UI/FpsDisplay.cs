using System.Globalization;
using System.Text;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FpsDisplay : MonoBehaviour
    {
        Text m_text;
        float m_deltaTime;

        StringBuilder m_sb = new StringBuilder();
        ProfilerRecorder setPassCallsRecorder;
        ProfilerRecorder drawCallsRecorder;
        ProfilerRecorder verticesRecorder;

        void OnEnable()
        {
            setPassCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "SetPass Calls Count");
            drawCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count");
            verticesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Vertices Count");
        }
        
        void OnDisable()
        {
            setPassCallsRecorder.Dispose();
            drawCallsRecorder.Dispose();
            verticesRecorder.Dispose();
        }

        void Start()
        {
            m_text = GetComponent<Text>();
        }


        void Update()
        {
            m_deltaTime += (Time.deltaTime - m_deltaTime) * 0.1f;
            var fps = 1.0f / m_deltaTime;

            m_sb.Clear();
            m_sb.AppendLine($"{Mathf.RoundToInt(fps)}");
            // m_sb.AppendLine($"Draw Calls: {drawCallsRecorder.LastValue}");
            // m_sb.AppendLine($"SetPass Calls: {setPassCallsRecorder.LastValue}");
            // m_sb.AppendLine($"Vertices: {verticesRecorder.LastValue}");
            m_text.text = m_sb.ToString();
        }
    }
}