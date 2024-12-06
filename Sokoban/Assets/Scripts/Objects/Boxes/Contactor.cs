using System.Threading.Tasks;
using UnityEngine;

namespace Objects.Boxes
{
    public class Contactor : MainObject
    {
        bool m_isMaterialize;
        int m_shaderDissolveId;


        void Start()
        {
            m_shaderDissolveId = Shader.PropertyToID("_Dissolve");
            Renderer.material.SetFloat(m_shaderDissolveId, 1.0f);
        }

        
        public async Task Materialize()
        {
            m_isMaterialize = true;

            await Task.Delay((int)(Random.value * 1000));
            var value = 1.0f;
            var material = Renderer.material;
            while (value >= 0.0f)
            {
                value -= Time.deltaTime / 3.0f;
                material.SetFloat(m_shaderDissolveId, value);
                await Task.Yield();
            }

            m_isMaterialize = false;
        }
        

    }
}