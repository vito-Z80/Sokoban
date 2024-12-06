using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Objects.Boxes
{
    public class Box : MainObject
    {
        Vector3 m_landingPosition;
        // Vector3 m_targetPosition;

        // bool m_isMoving;
        bool m_isMaterialize;
        bool m_isDisable;

        int m_shaderDissolveId;


        Renderer m_renderer;

        [SerializeField] [CanBeNull] ContactorBoxContainer m_contactorBoxContainer;


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
                var fromBelow = DetectNearestComponent<ContactorBoxContainer>(Vector3.down);
                if (fromBelow is not null)
                {
                    fromBelow.SubmitContact();

                    if (m_contactorBoxContainer != fromBelow)
                    {
                        m_contactorBoxContainer?.BreakContact();
                    }
                    m_contactorBoxContainer = fromBelow;
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


        //  Пересмотреть систему движения куба. Нужно, что бы последний куб замирал на последней точке для завершения уровня.

        // bool Move(float deltaTime)
        // {
        //     // if (!m_isMoving && m_isDisable) return;
        //     transform.position = Vector3.MoveTowards(transform.position, m_targetPosition, deltaTime * 1.0f);
        //     return transform.position != m_targetPosition;
        // }


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

        // void PointDetection()
        // {
        //     if (Physics.Raycast(transform.position, Vector3.down, out var hit, 0.6f))
        //     {
        //         if (hit.transform.TryGetComponent<ContactorBoxContainer>(out var point))
        //         {
        //             if (m_contactorBoxContainer is null)
        //             {
        //                 m_contactorBoxContainer = point;
        //             }
        //             else if (m_contactorBoxContainer != point)
        //             {
        //                 m_contactorBoxContainer.BreakContact();
        //                 m_contactorBoxContainer = point;
        //             }
        //
        //             point.SubmitContact();
        //         }
        //         else
        //         {
        //             m_contactorBoxContainer?.BreakContact();
        //         }
        //     }
        // }
    }
}