using System;
using System.Linq;
using Interfaces;
using JetBrains.Annotations;
using Objects.Boxes;
using Objects.Switchers;
using UnityEngine;

namespace InnerGame
{
    public class InnerGame15Puzzle : MonoBehaviour
    {
        [Header("Коробки мини-игры.")] [SerializeField]
        GameObject boxes;

        [Header("Выделяющийся короб. Который необходимо довести до финишной позиции.")] [SerializeField]
        GameObject distinctiveBox;

        [Header("Внутренний трансформ при достижении которой мини-игра закончится.")] [SerializeField]
        Transform finishPosition;

        [Header("Объекты которые нужно отключить после завершения мини-игры.")] [SerializeField]
        GameObject[] deactivateObjects;

        [Header("Трансформ, к которому нужно прикрепить выделяющуюся коробку после завершения игры.")] [SerializeField]
        Transform distinctiveBoxParent;

        [Header("Управление мини-игрой. Управление не обязательно должно быть во все 4 стороны.")] [SerializeField]
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
                    m_distinctiveBox.Freezed = false;
                    if (m_distinctiveBox.GetTransform.TryGetComponent<Box>(out var box))
                    {
                        box.canFall = true;
                    }
                    distinctiveBox.transform.SetParent(distinctiveBoxParent);

                    if (deactivateObjects is not null)
                    {
                        foreach (var go in deactivateObjects)
                        {
                            go.SetActive(false);
                        }
                    }
                    m_distinctiveBox.CanMove(finishPosition.forward);
                    
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