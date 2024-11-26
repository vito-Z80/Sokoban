using UnityEngine;

public class LightningPillar : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;
    Material m_material;

    void Start()
    {
        m_material = meshRenderer.materials[0];
    }

    void Update()
    {
        if (m_material == null) return;
        
        var x = m_material.mainTextureOffset.x + Time.deltaTime / 4.0f;
        var y = m_material.mainTextureOffset.y - Time.deltaTime / 32.0f;
        m_material.mainTextureOffset = new Vector2(x, y);

        // var scaleX = Mathf.Sin(y);
        // var scaleY = Mathf.Sin(x);
        // m_material.mainTextureScale = new Vector2(scaleX, scaleY);
    }
}