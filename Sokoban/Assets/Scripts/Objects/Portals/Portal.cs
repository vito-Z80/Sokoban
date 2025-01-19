using System;
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

    public class Portal : MonoBehaviour
    {
        [SerializeField] Portal otherSidePortal;
        [SerializeField] ParticleSystem teleportEffect;
        [SerializeField] ParticleSystem errorEffect;


        public enum State
        {
            Inactive,
            Wait,
            TrySend,
            Send,
            Accept,
            Block,
            Broken
        }

        MainObject m_inside;

        bool m_isBlocked;
        bool m_isBroken;

        State m_state;

        public State GetState()
        {
            return m_state;
        }

        bool TeleportTarget(MainObject target)
        {
            // if (m_inside != null) return false;
            m_inside = target;
            // teleportEffect?.Play();
            if (m_inside.TryGetComponent<Box>(out var box))
            {
                var savePosition = box.transform.position;
                box.transform.position = transform.position;
                if (box.Push(transform.forward))
                {
                    return true;
                }

                box.transform.position = savePosition;
            }

            return false;
        }


        bool MoveAside()
        {
            if (m_inside.TryGetComponent<Box>(out var box))
            {
                m_inside = null;
                return box.Push(transform.forward);
            }

            return true;
        }


        void Update()
        {
            Debug.DrawRay(transform.position + Vector3.down * 0.6f, Vector3.up * 0.5f, Color.red);
            // if (m_inside is null)
            // {
            //     if (Physics.Raycast(transform.position + Vector3.down * 0.6f, Vector3.up, out var hit, 0.5f))
            //     {
            //         if (hit.transform.TryGetComponent<MainObject>(out var mo))
            //         {
            //             m_inside = mo;
            //         }
            //         else
            //         {
            //             return;
            //         }
            //     }
            //     else
            //     {
            //         return;
            //     }
            // }
            //
            // var distance = Vector3.Distance(m_inside.transform.position, transform.position);
            // if (distance > 0.1f) return;
            // Debug.Log("Приблизился к центру телепорта.");
            // if (otherSidePortal.m_inside is null)
            // {
            //     otherSidePortal.TeleportTarget(m_inside);
            //     Debug.Log("Другая сторона портала свободна. Свободный телепорт.");
            // }
            // else
            // {
            //     if (otherSidePortal.MoveAside())
            //     {
            //         otherSidePortal.TeleportTarget(m_inside);
            //         Debug.Log("Другая сторона портала занята но была возможность сдвинуть объект занимающий портал.");
            //     }
            //     else
            //     {
            //         // errorEffect?.Play();
            //         Debug.Log("Другая сторона портала занята без возможности отослать туда объект.");
            //     }
            // }
            //
            // m_inside = null;

            Execute();
        }


        void Execute()
        {
            switch (m_state)
            {
                case State.Inactive:
                    OnEnter();
                    break;
                case State.Wait:
                    OnWait();
                    break;
                case State.TrySend:
                    TrySend();
                    break;
                case State.Send:
                    Send();
                    break;
                case State.Accept:
                    Accept();

                    break;
                case State.Block:
                    break;
                case State.Broken:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void Accept()
        {
            if (m_inside.transform.position == m_inside.targetPosition)
            {
                m_state = State.Inactive;
            }
        }

        void Send()
        {
            if (otherSidePortal.TeleportTarget(m_inside))
            {
                otherSidePortal.m_state = State.Accept;
                m_state = State.Inactive;
            }
            else
            {
                otherSidePortal.m_state = State.TrySend;
            }
        }


        void TrySend()
        {
            if (otherSidePortal.m_inside is null)
            {
                m_state = State.Send;
            }
            else
            {
                m_state = State.Broken;
            }
        }

        void OnWait()
        {
            var distance = Vector3.Distance(m_inside.transform.position, transform.position);
            if (distance > 0.1f) return;
            m_state = State.Send;
        }

        void OnEnter()
        {
            m_inside = null;
            if (Physics.Raycast(transform.position + Vector3.down * 0.6f, Vector3.up, out var hit, 0.5f))
            {
                if (hit.transform.TryGetComponent(out m_inside))
                {
                    m_state = State.Wait;
                }
            }
        }

        void OnTriggerEnter(Collider other)
        {
            
            Debug.Log(other.gameObject.name);
        }
    }
}