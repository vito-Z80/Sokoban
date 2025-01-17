using System;
using System.Linq;
using Objects;
using UnityEngine;

namespace Level.Tasks
{
    public class TaskActivatePoints : MonoBehaviour, ILevelTask
    {
        ContactorBoxContainer[] m_points;
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
        }

        
        void PointContact()
        {
            Debug.Log(string.Join(", ", m_points.Select(x => x.GetContact())));
            if (m_points.All(p => p.GetContact()))
            {
                OnTaskCompleted?.Invoke();
            
                //  TODO НУЖНО ОТКЛЮЧИТЬ ЦВЕТНЫЕ КОРОБКИ...  а нужно ли ???
            
                ShowEndEffect();    
            }
            
        }


        void ShowEndEffect()
        {
            foreach (var point in m_points)
            {
                point.PlayEffectWhirlCube();
            }
        }

        // void CheckLevelState()
        // {
        //     if (m_points.Count(container => container.GetContact()) != m_points.Length) return;
        //     m_levelCompleted = true;
        //
        //     foreach (var box in m_coloredBoxes)
        //     {
        //         box.DisableActions();
        //     }
        //
        //     OnTaskCompleted?.Invoke();
        // }
    }
}