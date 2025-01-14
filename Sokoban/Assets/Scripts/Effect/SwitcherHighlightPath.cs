using UnityEngine;

namespace Effect
{
    public class SwitcherHighlightPath : MonoBehaviour
    {
        [SerializeField] Transform startTransform;
        LineRenderer m_lineRenderer;
        Material m_material;
        int m_emissionColorPropId;
        const string EmissionKeyword = "_EMISSION";
        bool m_highlighted;

        void Start()
        {
            m_lineRenderer = GetComponent<LineRenderer>();
            m_material = m_lineRenderer.material;
            m_emissionColorPropId = Shader.PropertyToID("_EmissionColor");

            for (var i = 0; i < m_lineRenderer.positionCount; i++)
            {
                var pos = m_lineRenderer.GetPosition(i);
                var newPosition = startTransform.TransformPoint(pos);
                newPosition.y = -0.49f;
                m_lineRenderer.SetPosition(i, newPosition);
            }
        }

        public void SetHighlight(bool highlighted, Color color)
        {
            m_highlighted = highlighted;
            m_material.SetColor(m_emissionColorPropId, color * 100.0f);
            m_material.EnableKeyword(EmissionKeyword);
        }

        float m_time;

        void Update()
        {
            if (!m_highlighted) return;
            m_material.mainTextureOffset = new Vector2(-m_time, 0);
            m_time += Time.deltaTime * 4.0f;
        }
    }
}