using System;
using Interfaces;
using UnityEngine;

namespace Objects.Portals
{
    /*
     * Работа портала:
     *
     * Триггер портала ожидает нового вхождения.
     * Портал ждет пока в него войдут полностью.
     * Портал проверяет свободно ли с другой стороны.
     *      Если свободно: издаем звук и эффект телепортации, переносим объект на другую сторону.
     *      Если занято:
     *                  Проверяем, можем ли мы отодвинуть предмет с той стороны в положение не блокирующее его дальнейшее движение.
     *                      Можем: издаем звук и эффект телепортации, указываем объекту с другой стороны новую позицию, переносим объект с этой стороны на другую сторону.
     *                      Не можем: издаем звук ошибки, эффект сломанного портала, с объектом ни чего не делаем.
     */

    public class Portal : MonoBehaviour
    {
        [SerializeField] Portal otherSidePortal;
        [SerializeField] ParticleSystem teleportEffect;
        [SerializeField] ParticleSystem errorEffect;


        public enum State
        {
            Inactive,
            TrySend,
            Pull,
            Send,
            Push,
            Wait,
            Broken
        }

        public IMovable m_inside;

        bool m_isBlocked;
        bool m_isBroken;

        State m_state;

        public State GetState()
        {
            return m_state;
        }
        
        void Update()
        {
            Debug.DrawRay(transform.position + Vector3.down * 0.6f, Vector3.up * 0.5f, Color.red);
            Execute();
            
            Debug.Log($"Portal: {gameObject.name} : {m_inside}");
        }


        void Execute()
        {
            switch (m_state)
            {
                case State.Inactive:
                    Debug.Log("Inactive");
                    break;
                case State.Pull:
                    Debug.Log("Pull");
                    Pull();
                    break;
                case State.Send:
                    Debug.Log("Send");
                    Send();
                    break;
                case State.Push:
                    Debug.Log("Push");
                    Push();

                    break;
                // case State.Wait:
                //     Debug.Log("Block");
                //     Wait();
                //     break;
                case State.Broken:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void Push()
        {
            if (m_inside.CanMove(transform.forward))
            {
                m_inside = null;
            }

            m_state = State.Inactive;
        }

        void Pull()
        {
            if (m_inside.GetTransform.position == m_inside.TargetPosition)
            {
                m_state = State.Send;
            }
        }

        // void Wait()
        // {
        //     if (m_inside.GetTransform.position == m_inside.TargetPosition)
        //     {
        //         m_state = State.Inactive;
        //         m_inside = null;
        //     }
        // }

        void Send()
        {
            if (otherSidePortal.m_inside == null)
            {
                m_inside.GetTransform.position = new Vector3(
                    otherSidePortal.transform.position.x,
                    m_inside.GetTransform.position.y,
                    otherSidePortal.transform.position.z
                    );
                m_inside.TargetPosition = m_inside.GetTransform.position;
                otherSidePortal.m_inside = m_inside;

                m_inside = null;

                otherSidePortal.m_state = State.Push;
            }
            else
            {
                m_inside.Freezed = false;
                m_inside = null;
            }

            m_state = State.Inactive;
        }


        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IMovable movable))
            {
                m_inside = movable;
                if (movable.Freezed)
                {
                    //  push
                    movable.Freezed = false;
                    Push();
                }
                else
                {
                    //  pull
                    movable.Freezed = true;
                    movable.TargetPosition = new Vector3(transform.position.x, movable.GetTransform.position.y, transform.position.z);
                    m_state = State.Pull;
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IMovable movable))
            {
                if (movable == m_inside) m_inside = null;
            }
        }
    }
}