using UnityEngine;

public class GameFog : MonoBehaviour
{
    [SerializeField] Transform quad;

    Vector2 m_decalOffset;
        

    void Update()
    {
        var deltaTime = Time.deltaTime;

        m_decalOffset.y += deltaTime / 32.0f;
        // decal.uvBias = m_decalOffset;
        // decal.transform.Rotate(Vector3.forward, deltaTime);
        quad.Rotate(Vector3.back, deltaTime);
    }
}