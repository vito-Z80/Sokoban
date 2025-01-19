using System.Linq;
using Bridge;
using Data;
using Objects.Portals;
using UnityEngine;

namespace Objects.Boxes
{
    public class Box : MainObject
    {
        [SerializeField] public BoxColor boxColor;

        float m_boxSpeed = 1.0f;

        int m_sideLayerMask;

        void OnEnable()
        {
            isDisable = true;
            targetPosition = transform.position.Round();
            m_sideLayerMask = LayerMask.GetMask("Box", "Portal", "Wall");
        }

        void Update()
        {
            Debug.DrawRay(transform.position, Vector3.down * 0.6f, Color.red);
            var deltaTime = Time.deltaTime;
            isMoving = Move(deltaTime * m_boxSpeed);

            if (IsStopped())
            {
                if (DirectionComponent(Vector3.down, out Transform component, 10.0f))
                {
                    if (component.TryGetComponent<BridgeFloorCell>(out var floorCell))
                    {
                        //  Швырнуть коробку в стратосферу если вынесли на мост.
                        targetPosition = (floorCell.transform.position + Vector3.up * 100.0f).Round();
                        m_boxSpeed = 6.0f;
                    }
                    else
                    {
                        targetPosition = (component.position + Vector3.up).Round();
                    }
                }
            }
        }

        int m_touchedGroundCount;

        bool IsStopped()
        {
            if (isMoving || isDisable)
            {
                m_touchedGroundCount = 0;
                return false;
            }

            if (m_touchedGroundCount == 0)
            {
                m_touchedGroundCount++;
                return true;
            }

            return false;
        }

        public bool DisableColoredBox()
        {
            if (boxColor != BoxColor.None)
            {
                isDisable = true;
            }

            return transform.position == targetPosition;
        }

        public void EnableBox()
        {
            isDisable = false;
        }


        // public void PlayEffectColoredBox()
        // {
        //     if (boxColor == BoxColor.None) return;
        //     var ps = gameObject.GetComponentInChildren<ParticleSystem>();
        //     ps.Play();
        // }

        public bool Push(Vector3 direction)
        {
            if (isMoving) return false;
            if (isDisable)
            {
                transform.position = transform.position.Round();
                targetPosition = transform.position;
                return false;
            }

            //  Если снизу пусто.
            if (!DirectionComponent(Vector3.down, out Transform _ /*, 10.0f*/))
            {
                return false;
            }

            if (Raycast(transform.position, direction, out var hit, 1.0f, m_sideLayerMask))
            {
                if (EnterPortal(hit.transform)) return true;
            }


            //  Если по направлению движения есть объект.
            if (DirectionComponent(direction, out Transform _))
            {
                return false;
            }

            targetPosition = transform.position + direction.Round();
            return true;
        }

        bool EnterPortal(Transform t)
        {
            if (t.TryGetComponent<Portal>(out var portal))
            {
                if (portal.GetState() == Portal.State.Inactive)
                {
                    isDisable = true;
                    return true;
                }
            }

            return false;
        }

        public override void PopState()
        {
            if (Stack.Count == 0) return;
            var data = Stack.Last();
            if (data.Position.y % 1.0f == 0.0f && Physics.Raycast(data.Position, Vector3.down, out var hit, 0.6f))
            {
                if (hit.transform != transform)
                {
                    transform.rotation = data.Rotation;
                    transform.localScale = data.Scale;
                    targetPosition = data.Position;
                    transform.position = data.Position;
                }
            }

            Stack.RemoveAt(Stack.Count - 1);
        }
    }
}