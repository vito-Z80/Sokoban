using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;

namespace Objects.Boxes
{
    /// <summary>
    /// <b><i>Что можно сделать с этой штукой или во что расширить.</i></b><br/>
    /// 1) Турникет - что-то открывает/закрывает.<br/>
    /// 2) Указатель - на что-то влияет в зависимости от того куда смотрит.<br/>
    /// 3) Лазерная пушка. Можно крутить в четырех направлениях. Пробить стену или направить лазер в отражатель.<br/>
    /// 4) Лазерная пушка. Можно крутить в четырех направлениях. Пробить стену или направить лазер в отражатель. + Ее саму можно перемещать.<br/>
    /// 5) Управление высотой подъема лифта. (типа на лифте поднять короб который поедет дальше по эскалатору.)
    /// </summary>
    public class Turnstile : MonoBehaviour, IMovable, IUndo
    {
        Quaternion m_targetRotation;
        Vector3 m_targetPosition;
        bool m_freezed;
        bool m_isRotating;

        int m_sideLayerMask;

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


        void Start()
        {
            m_sideLayerMask = LayerMask.GetMask("Assembler", "Box", "Wall", "Collectible");
        }

        void Update()
        {
            if (!m_isRotating) return;

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                m_targetRotation,
                Global.Instance.gameSpeed * 90.0f * Time.deltaTime
            );


            if (Quaternion.Angle(transform.rotation, m_targetRotation) <= 0f)
            {
                m_isRotating = false;
            }
        }

        public bool CanMove(Vector3 direction)
        {
            if (direction != Vector3.zero)
            {
                var right = transform.right;
                direction.Normalize();
                var rightDot = Vector3.Dot(right, direction);

                if (rightDot > 0.5f)
                {
                    //  Объект подошел слева
                    m_targetRotation = Quaternion.LookRotation(right);
                    m_isRotating = IsSidesFree(right);
                }
                else if (rightDot < -0.5f)
                {
                    // Объект подошел справа
                    m_targetRotation = Quaternion.LookRotation(-right);
                    m_isRotating = IsSidesFree(-right);
                }
            }

            return m_isRotating;
        }


        bool IsSidesFree(Vector3 direction)
        {
            var side = Physics.CheckSphere(transform.position + direction, 0.49f, m_sideLayerMask);
            if (side) return false;
            var forward = Physics.CheckSphere(transform.position + direction + transform.forward, 0.49f, m_sideLayerMask);
            return !forward;
        }

        public List<BackStepTransform> Stack { get; } = new();
        public void Push()
        {
            Stack.Add(new BackStepTransform(transform));
        }

        public void Pop()
        {
            if (Stack.Count == 0) return;
            var data = Stack.Last();
            transform.rotation = data.Rotation;
            transform.localScale = data.Scale;
            m_targetPosition = data.Position;
            transform.position = data.Position;
            Stack.RemoveAt(Stack.Count - 1);
        }
    }
}