using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Objects.Boxes
{
    public class MaterializedBox : Box
    {
        int m_shaderDissolveId;
        Renderer m_renderer;

        public async Task Materialize()
        {
            m_renderer ??= GetComponent<Renderer>();
            m_shaderDissolveId = Shader.PropertyToID("_Dissolve");
            m_renderer.sharedMaterial.SetFloat(m_shaderDissolveId, 1.0f);
            
            await Task.Delay((int)(Random.value * 1000));
            var value = 1.0f;
            while (value >= 0.0f)
            {
                value -= Time.deltaTime / 3.0f;
                m_renderer.sharedMaterial.SetFloat(m_shaderDissolveId, value);
                await Task.Yield();
            }
        }
    }
}