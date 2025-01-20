using System;
using Interfaces;
using Objects.Boxes;
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

    public class Portal : MonoBehaviour, IMagnetizable
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
            Accept,
            Wait,
            Broken
        }

        IMovable m_inside;

        bool m_isBlocked;
        bool m_isBroken;

        State m_state;

        public State GetState()
        {
            return m_state;
        }

        bool TeleportTarget(IMovable target)
        {
            // if (m_inside != null) return false;
            m_inside = target;
            // teleportEffect?.Play();

            var savePosition = m_inside.GetTransform.position;
            m_inside.GetTransform.position = transform.position;
            if (m_inside.CanMove(transform.forward))
            {
                return true;
            }

            m_inside.GetTransform.position = savePosition;


            return false;
        }


        // bool MoveAside()
        // {
        //     if (m_inside.TryGetComponent<Box>(out var box))
        //     {
        //         var isMoveAside = m_inside.CanMove(transform.forward);
        //         m_inside = null;
        //         return isMoveAside;
        //     }
        //
        //     return true;
        // }


        void Update()
        {
            Debug.DrawRay(transform.position + Vector3.down * 0.6f, Vector3.up * 0.5f, Color.red);
            Execute();
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
                    if (m_inside.TargetPosition == transform.position)
                    {
                        m_state = State.Send;
                    }
                    break;
                case State.Send:
                    Debug.Log("Send");
                    Send();
                    break;
                case State.Accept:
                    Debug.Log("Accept");
                    Accept();

                    break;
                case State.Wait:
                    Debug.Log("Block");
                    Wait();
                    break;
                case State.Broken:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void Accept()
        {

            if (m_inside.CanMove(transform.forward))
            {
                m_state = State.Wait;
            }
            else
            {
                m_state = State.Inactive;
            }
        }

        void Wait()
        {
            if (m_inside.GetTransform.position == m_inside.TargetPosition)
            {
                m_state = State.Inactive;
                m_inside = null;
            }
        }

        void Send()
        {


            if (otherSidePortal.m_inside == null)
            {
                otherSidePortal.m_inside = m_inside;
                m_inside = null;
                
                otherSidePortal.m_state = State.Accept;
                m_state = State.Inactive;
            }
            else
            {
                // m_state = State.Broken;
                m_state = State.Inactive;
                m_inside.Freezed = false;
                m_inside = null;
            }
        }


        // void OnTriggerEnter(Collider other)
        // {
        //     Debug.Log(other.gameObject.name);
        // }

        public bool Magnetize(IMovable movable)
        {
            if (m_inside != null) return false;
            m_state = State.Pull;
            m_inside = movable;
            m_inside.TargetPosition = transform.position;
            m_inside.Freezed = true;
            return true;
        }
    }
}