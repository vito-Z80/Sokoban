using UnityEngine;

public class TestDissolve : MonoBehaviour
{
    
    Renderer m_renderer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // m_renderer = GetComponentInChildren<Renderer>();
        // m_renderer.material.SetFloat("_Dissolve", 2.5f);
        gameObject.isStatic = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
