using UnityEngine;

namespace TransportSystem
{
    public class TransportSystemTextureUVRotator : MonoBehaviour
    {
        
        [Header("Render коробки транспортной системы.")] [SerializeField]
        Renderer meshRenderer;
        
        Material m_material;
        int m_transporterTextureID;
        Vector2 m_uvOffset;
        readonly Vector2 m_uvSpeed = new(0.0f, 8.0f);

        void Start()
        {
            m_material = meshRenderer.GetComponent<Renderer>().sharedMaterial;
            m_transporterTextureID = Shader.PropertyToID("_MainTex");
        }


        void Update()
        {
            if (m_material != null)
            {
                m_uvOffset += m_uvSpeed * Time.deltaTime;
                m_material.SetTextureOffset(m_transporterTextureID, m_uvOffset);
            }
        }
    }
}