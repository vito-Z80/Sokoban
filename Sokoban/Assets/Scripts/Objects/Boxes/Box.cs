using System.Threading.Tasks;
using Data;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Objects.Boxes
{
    public class Box : MainObject
    {
        
        [SerializeField] public BoxColor boxColor;
        
        bool m_isDisable;
        int m_shaderDissolveId;

        Renderer m_renderer;
        [CanBeNull] ContactorBoxContainer m_contactorBoxContainer;


        void OnEnable()
        {
            TargetPosition = transform.position;
        }

        void Start()
        {
            m_renderer = GetComponentInChildren<Renderer>();
            m_shaderDissolveId = Shader.PropertyToID("_Dissolve");
            m_renderer.material.SetFloat(m_shaderDissolveId, 1.0f);

            m_contactorBoxContainer = DetectNearestComponent<ContactorBoxContainer>(Vector3.down);
            m_contactorBoxContainer?.SubmitContact();
        }

        void Update()
        {
            var deltaTime = Time.deltaTime;

            var isMove = Move(deltaTime);

            if (isMove)
            {
                var contactorBoxContainer = DetectNearestComponent<ContactorBoxContainer>(Vector3.down);
                if (contactorBoxContainer is not null)
                {
                    if (contactorBoxContainer.boxColor == boxColor)
                    {
                        contactorBoxContainer.SubmitContact();
                    }

                    if (m_contactorBoxContainer != contactorBoxContainer)
                    {
                        m_contactorBoxContainer?.BreakContact();
                    }

                    m_contactorBoxContainer = contactorBoxContainer;
                }
                else
                {
                    m_contactorBoxContainer?.BreakContact();
                    m_contactorBoxContainer = null;
                }
            }
        }

        public bool DisableActions()
        {
            m_isDisable = true;
            return transform.position == TargetPosition;
        }

        public bool CanStep(Vector3 direction)
        {
            if (m_isDisable)
            {
                transform.position = transform.position.RoundWithoutY();
                TargetPosition = transform.position;
                return false;
            }

            var front = DetectNearestComponent<Transform>(direction);
            if (front is not null) return false;
            TargetPosition = transform.position + direction;
            return true;
        }

        public async Task Materialize()
        {
            await Task.Delay((int)(Random.value * 1000));
            var value = 1.0f;
            var material = m_renderer.material;
            while (value >= 0.0f)
            {
                value -= Time.deltaTime / 3.0f;
                material.SetFloat(m_shaderDissolveId, value);
                await Task.Yield();
            }
        }
    }
}