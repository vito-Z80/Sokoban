using UnityEngine;

namespace Effect
{
    public class Lightning : MonoBehaviour
    {
        [SerializeField] Texture[] lightningTextures;

        int m_textureCount;
        LineRenderer m_lineRenderer;
        Material m_material;

        void Start()
        {
            m_lineRenderer = GetComponent<LineRenderer>();
            m_material = m_lineRenderer.material;
        }


        float m_textureOffsetX;
        float m_time;

        void Update()
        {
            m_textureOffsetX += Time.deltaTime * 4.39f;
            m_material.mainTextureOffset = new Vector2(m_textureOffsetX, 0.0f);
            m_time += Time.deltaTime;
            if (!(m_time > 1.0f / 15.0f)) return;
            m_time = 0.0f;
            m_material.mainTexture = NextTexture();

        }


        Texture NextTexture()
        {
            m_textureCount++;
            if (m_textureCount >= lightningTextures.Length) m_textureCount = 0;
            return lightningTextures[m_textureCount];
        }
    }
}