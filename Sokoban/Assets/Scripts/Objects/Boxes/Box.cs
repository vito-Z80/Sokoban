using System;
using Data;
using JetBrains.Annotations;
using UnityEngine;

namespace Objects.Boxes
{
    public class Box : MainObject
    {
        [SerializeField] public BoxColor boxColor;

        bool m_isDisable;


        [CanBeNull] ContactorBoxContainer m_contactorBoxContainer;


        BoxAction m_action = BoxAction.Stay;
        Direction m_direction;

        void OnEnable()
        {
            TargetPosition = transform.position.Round();
            SetPointContact();
        }
        
        void Update()
        {
            var deltaTime = Time.deltaTime;
            
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


        void SetPointContact()
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
                m_isDisable = true;
            }

            return transform.position == TargetPosition;
        }

        public bool CanStep(Vector3 direction)
        {
            if (m_action == BoxAction.Fall) return false;
            if (m_isDisable || m_action == BoxAction.Fall)
            {
                transform.position = transform.position.Round();
                TargetPosition = transform.position;
                return false;
            }

            var front = DetectNearestComponent<Transform>(direction);
            if (front is not null) return false;
            TargetPosition = transform.position + direction.Round();
            m_action = BoxAction.Move;
            return true;
        }

        bool CanFall()
        {
            var fromBelow = DetectNearestComponent<Transform>(Vector3.down, 10.0f);
            if (fromBelow is null)
            {
                TargetPosition = transform.position + Vector3.down * 10.0f;
                return true;
            }

            if (Vector3.Distance(transform.position, fromBelow.position) < 0.9f)
            {
                return false;
            }

            // var belowHeight = fromBelow.GetComponent<MeshFilter>().mesh.bounds.center.y;
            // var height = GetComponent<MeshFilter>().mesh.bounds.extents.y;
            TargetPosition = (fromBelow.position + Vector3.up).Round();// * (height + belowHeight);
            return true;
        }
    }
}