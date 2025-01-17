using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameFog : MonoBehaviour
{
    [SerializeField] Transform quad;
    [SerializeField] DecalProjector decal;

    Vector2 m_decalOffset;
        

    void Update()
    {
        var deltaTime = Time.deltaTime;

        m_decalOffset.y += deltaTime / 32.0f;
        decal.uvBias = m_decalOffset;
        decal.transform.Rotate(Vector3.forward, deltaTime);
        quad.Rotate(Vector3.back, deltaTime);
    }
}