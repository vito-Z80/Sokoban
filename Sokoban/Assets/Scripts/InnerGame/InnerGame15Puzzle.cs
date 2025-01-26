using System;
using System.Linq;
using Interfaces;
using JetBrains.Annotations;
using Objects.Switchers;
using UnityEngine;

namespace InnerGame
{
    public class InnerGame15Puzzle : MonoBehaviour
    {
        [SerializeField] GameObject boxes;
        [SerializeField] GameObject distinctiveBox;
        [SerializeField] Transform finishPosition;

        [Header("Controller")] [SerializeField]
        Switch toLeft;

        [SerializeField] Switch toRight;
        [SerializeField] Switch toUp;
        [SerializeField] Switch toDown;


        IMovable[] m_boxes;
        IMovable m_distinctiveBox;

        [HideInInspector] public bool isFinished;

        void Start()
        {
            m_distinctiveBox = distinctiveBox.GetComponent<IMovable>();
            m_boxes = boxes.GetComponentsInChildren<IMovable>().ToArray();
            FreezeBoxes(false);
        }

        void Update()
        {
            if (isFinished) return;
            Control();
            if (Vector3.Distance(finishPosition.position, m_distinctiveBox.GetTransform.position) < 0.1f)
            {
                if (m_distinctiveBox.TargetPosition == m_distinctiveBox.GetTransform.position)
                {
                    isFinished = true;
                    FreezeBoxes(true);
                }
            }
        }


        void Control()
        {
            if (IsPushed(toLeft, ToLeft)) return;
            if (IsPushed(toRight, ToRight)) return;
            if (IsPushed(toUp, Forward)) return;
            IsPushed(toDown, Backward);
        }

        bool IsPushed([CanBeNull] Switch switcher, Action action)
        {
            if (switcher is null) return false;
            if (!switcher.isOn) return false;
            switcher.isOn = false;
            action();
            return true;
        }

        public void ToLeft()
        {
            MoveBox(Vector3.left);
        }

        public void ToRight()
        {
            MoveBox(Vector3.right);
        }

        public void Forward()
        {
            MoveBox(Vector3.forward);
        }

        public void Backward()
        {
            MoveBox(Vector3.back);
        }


        void FreezeBoxes(bool freeze)
        {
            foreach (var box in m_boxes)
            {
                box.Freezed = freeze;
            }
        }

        void MoveBox(Vector3 direction)
        {
            var d = transform.TransformDirection(direction);
            _ = m_boxes.Any(box => box.CanMove(d));
        }
    }
}