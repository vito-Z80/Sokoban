using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Objects
{
    public class Box : MainObject
    {
        Vector3 m_landingPosition;
        Vector3 m_targetPosition;

        bool m_isMoving;
        bool m_isMaterialize;
        bool m_isDisable;

        int m_shaderDissolveId;


        Renderer m_renderer;

        [CanBeNull] Point m_point;

        void Start()
        {
            m_renderer = GetComponentInChildren<Renderer>();
            m_shaderDissolveId = Shader.PropertyToID("_Dissolve");
            m_renderer.material.SetFloat(m_shaderDissolveId, 1.0f);
        }


        public override void Init()
        {
            m_targetPosition = transform.position;
            Debug.Log(name);
        }

        void Update()
        {
            if (m_isDisable) return;
            var deltaTime = Time.deltaTime;


            if (m_isMoving || !m_isMaterialize)
            {
                Move(deltaTime);
                PointDetection();
            }
        }

        public void DisableActions()
        {
            m_isDisable = true;
        }

        void Move(float deltaTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_targetPosition, deltaTime * 1.0f);
            m_isMoving = transform.position != m_targetPosition;
        }


        public bool CanStep(Vector3 direction)
        {
            if (Physics.Raycast(transform.position, direction, 0.6f)) return false;
            m_targetPosition = transform.position + direction;
            m_isMoving = true;
            return true;
        }

        public async Task Materialize()
        {
            m_isMaterialize = true;

            await Task.Delay((int)(Random.value * 1000));
            var value = 1.0f;
            var material = m_renderer.material;
            while (value >= 0.0f)
            {
                value -= Time.deltaTime / 3.0f;
                material.SetFloat(m_shaderDissolveId, value);
                await Task.Yield();
            }

            m_isMaterialize = false;
        }

        void PointDetection()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out var hit, 0.6f))
            {
                if (hit.transform.TryGetComponent<Point>(out var point))
                {
                    if (m_point is null)
                    {
                        m_point = point;
                    }
                    else if (m_point != point)
                    {
                        m_point.isInvolved = false;
                        m_point = point;
                    }

                    point.isInvolved = true;
                }
                else
                {
                    if (m_point is not null)
                    {
                        m_point.isInvolved = false;
                    }
                }
            }
        }
    }
}