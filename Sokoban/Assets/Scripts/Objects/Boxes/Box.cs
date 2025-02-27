using System.Collections.Generic;
using System.Linq;
using Data;
using Interfaces;
using UnityEngine;

namespace Objects.Boxes
{
    public class Box : MainObject, IMovable, IUndo
    {
        [SerializeField] public BoxColor boxColor;
        public bool canFall = true;

        Vector3 m_targetPosition;
        bool m_freezed;

        public float boxSpeed = 1.0f;


        int m_portalLayerMask;
        int m_sideLayerMask;
        int m_bottomLayerMask;

        public Transform GetTransform => transform;

        public Vector3 TargetPosition
        {
            get => m_targetPosition;
            set => m_targetPosition = value;
        }

        public bool Freezed
        {
            get => m_freezed;
            set => m_freezed = value;
        }

        public List<BackStepTransform> Stack { get; } = new();

        void OnEnable()
        {
            m_freezed = true;
            m_targetPosition = transform.position.Round();
            m_portalLayerMask = LayerMask.GetMask("Portal");
            m_sideLayerMask = LayerMask.GetMask("Assembler", "Box", "Wall", "Collectible", "Door");
            m_bottomLayerMask = LayerMask.GetMask("Box", "Floor", "Point", "Swich");
        }

        int m_waitCount;

        void LateUpdate()
        {
            Debug.DrawRay(transform.position, Vector3.down * 0.6f, Color.red);
            var deltaTime = Time.deltaTime;
            if (canFall)
            {
                if (!m_freezed && transform.position == m_targetPosition)
                {
                    m_waitCount++;
                    if (m_waitCount == 1)
                    {
                        if (!Raycast(transform.position, Vector3.down, out var hit, 0.6f, m_bottomLayerMask))
                        {
                            m_targetPosition = transform.position + Vector3.down;
                        }
                    }

                    return;
                }
            }

            var correctSpeed = transform.position.y > m_targetPosition.y ? boxSpeed * 2.0f : boxSpeed;

            transform.position = Vector3.MoveTowards(transform.position, m_targetPosition, deltaTime * Global.Instance.gameSpeed * correctSpeed);
            m_waitCount = 0;
        }

        public bool CanMove(Vector3 direction)
        {
            if (!m_freezed && m_targetPosition == transform.position)
            {
                var position = transform.position;
                if (canFall)
                {
                    if (Raycast(position, Vector3.down, out var hit, 0.6f, m_bottomLayerMask))
                    {
                        if (Physics.CheckSphere(position + direction, 0.49f, m_sideLayerMask))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        m_targetPosition = (transform.position + Vector3.down).Round();
                        return false;
                    }
                }
                else
                {
                    if (Raycast(position , direction, out var hit, 1.0f, m_portalLayerMask))
                    {
                        if (hit.transform.forward == -direction)
                        {
                            return false;
                        }
                    }
                    if (Physics.CheckSphere(position + direction, 0.49f, m_sideLayerMask))
                    {
                        return false;
                    }
                }

                m_targetPosition = (transform.position + direction).Round();

                return true;
            }

            return false;
        }

        public void FreezedColoredBox()
        {
            if (boxColor != BoxColor.None)
            {
                Freezed = true;
            }
        }

        public void Push()
        {
            Stack.Add(new BackStepTransform(transform));
        }

        public void Pop()
        {
            if (Stack.Count == 0) return;
            var data = Stack.Last();
            if (data.Position.y % 1.0f == 0.0f && Physics.Raycast(data.Position, Vector3.down, out var hit, 0.6f))
            {
                if (hit.transform != transform)
                {
                    transform.rotation = data.Rotation;
                    transform.localScale = data.Scale;
                    m_targetPosition = data.Position;
                    transform.position = data.Position;
                }
            }

            Stack.RemoveAt(Stack.Count - 1);
        }
    }
}