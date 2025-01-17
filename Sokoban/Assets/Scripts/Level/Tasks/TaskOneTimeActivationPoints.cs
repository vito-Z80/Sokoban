using System;
using System.Linq;
using System.Threading.Tasks;
using Objects;
using Objects.Boxes;
using UnityEngine;

namespace Level.Tasks
{
    /// <summary>
    /// Задача считается завершенной когда все m_points заполнены коробками того-же цвета.
    /// </summary>
    public class TaskOneTimeActivationPoints : MonoBehaviour, ILevelTask
    {
        [SerializeField] GameObject boxes;

        ContactorBoxContainer[] m_points;
        Box[] m_boxes;
        public event Action OnTaskCompleted;
        public static Action OnPointContact;

        int m_pointCount;

        void OnEnable()
        {
            OnPointContact += PointContact;
        }

        void OnDisable()
        {
            OnPointContact -= PointContact;
        }

        void Start()
        {
            m_points = GetComponentsInChildren<ContactorBoxContainer>();
            _ = GetBoxes();
        }


        void PointContact()
        {
            if (m_points.All(p => p.GetContact()))
            {

                if (m_boxes != null)
                {
                    foreach (var box in m_boxes)
                    {
                        box.DisableColoredBox();
                    }
                }

                ShowEndEffect();
                OnTaskCompleted?.Invoke();
            }
        }


        void ShowEndEffect()
        {
            foreach (var point in m_points)
            {
                point.PlayEffectWhirlCube();
            }
        }


        Task GetBoxes()
        {
            m_boxes = boxes.GetComponentsInChildren<Box>();
            return Task.CompletedTask;
        }
    }
}