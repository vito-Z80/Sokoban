using System;
using Data;
using JetBrains.Annotations;
using UnityEngine;

namespace Objects.Boxes
{
    public class Box : MainObject
    {
        [SerializeField] public BoxColor boxColor;


        [CanBeNull] ContactorBoxContainer m_contactorBoxContainer;


        BoxAction m_action = BoxAction.Stay;
        // Direction m_direction;

        void OnEnable()
        {
            isDisable = true;
            targetPosition = transform.position.Round();
            SetPointContact();
        }


        bool m_isStopped;

        void Update()
        {
            var deltaTime = Time.deltaTime;
            isMoving = Move(deltaTime);


            if (!isMoving && !isDisable)
            {
                CanFall();
                SetPointContact();
            }

            return;
            switch (m_action)
            {
                case BoxAction.Stay:

                    break;
                case BoxAction.Move:
                    if (!Move(deltaTime))
                    {
                        m_action = CanFall() ? BoxAction.Fall : BoxAction.Stay;
                        SetPointContact();
                    }

                    break;
                case BoxAction.Fall:
                    if (!Move(deltaTime))
                    {
                        m_action = BoxAction.Stay;
                        SetPointContact();
                    }

                    break;
                case BoxAction.Controlled:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        public void SetPointContact()
        {
            var contactorBoxContainer = DetectNearestComponent<ContactorBoxContainer>(Vector3.down);

            m_contactorBoxContainer?.BreakContact();
            m_contactorBoxContainer = contactorBoxContainer;

            if (m_contactorBoxContainer is not null && m_contactorBoxContainer.boxColor == boxColor)
            {
                m_contactorBoxContainer.SubmitContact();
                return;
            }

            m_contactorBoxContainer = null;
        }

        public bool DisableActions()
        {
            if (boxColor != BoxColor.None)
            {
                isDisable = true;
            }

            return transform.position == targetPosition;
        }

        public void EnableActions()
        {
            isDisable = false;
        }

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
            if (!DirectionComponent(Vector3.down, out Transform _/*, 10.0f*/))
            {
                return false;
            }
            //  Если по направлению движения есть объект.
            if (DirectionComponent(direction, out Transform _))
            {
                return false;
            }

            targetPosition = transform.position + direction.Round();
            return true;


            if (m_action == BoxAction.Fall) return false;
            if (isDisable || m_action == BoxAction.Fall)
            {
                transform.position = transform.position.Round();
                targetPosition = transform.position;
                return false;
            }

            var front = DetectNearestComponent<Transform>(direction);
            if (front is not null) return false;
            targetPosition = transform.position + direction.Round();
            m_action = BoxAction.Move;
            return true;
        }

        bool CanFall()
        {
            if (DirectionComponent(Vector3.down, out Transform component, 10.0f))
            {
                targetPosition = (component.position + Vector3.up).Round();
                return true;
            }

            return false;

            var fromBelow = DetectNearestComponent<Transform>(Vector3.down, 10.0f);
            if (fromBelow is null)
            {
                targetPosition = transform.position.Round() + Vector3.down * 10.0f;
                return true;
            }

            if (Vector3.Distance(transform.position, fromBelow.position) < 0.9f)
            {
                return false;
            }

            // var belowHeight = fromBelow.GetComponent<MeshFilter>().mesh.bounds.center.y;
            // var height = GetComponent<MeshFilter>().mesh.bounds.extents.y;
            targetPosition = (fromBelow.position + Vector3.up).Round(); // * (height + belowHeight);
            return true;
        }
    }
}