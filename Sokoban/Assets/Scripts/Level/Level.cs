using System;
using System.Collections;
using System.Linq;
using Data;
using JetBrains.Annotations;
using Objects;
using Objects.Boxes;
using UnityEngine;

namespace Level
{
    public class Level : MonoBehaviour
    {
        [SerializeField] GameObject points;
        [SerializeField] GameObject walls;
        [SerializeField] GameObject floor;
        [SerializeField] public GameObject boxes;

        [SerializeField] public Door enterDoor;
        [SerializeField] public Door exitDoor;


        ContactorBoxContainer[] m_points;
        Box[] m_coloredBoxes;

        public const float LevelDistance = 10.0f;

        public static Action OnLevelCompleted;

        bool m_levelCompleted;

        void Start()
        {
            m_points = points.GetComponentsInChildren<ContactorBoxContainer>();
            m_coloredBoxes = boxes.GetComponentsInChildren<Box>().Where(box => box.boxColor != BoxColor.None).ToArray();
        }

        void LateUpdate()
        {
            if (m_levelCompleted) return;
            CheckLevelState();
        }


        void CheckLevelState()
        {
            if (m_points.Count(container => container.GetContact()) != m_points.Length) return;
            if (m_coloredBoxes.Any(box => !box.DisableActions())) return;
            m_levelCompleted = true;
            OnLevelCompleted?.Invoke();
        }

        public Vector3 LevelOffset([CanBeNull] Transform previousExit)
        {
            if (previousExit is null)
            {
                return Vector3.zero;
            }

            var forward = previousExit.transform.forward;
            var offsetBetweenPreviousAndNextDoors = previousExit.position - (enterDoor.transform.position - transform.position) + forward * LevelDistance;
            return offsetBetweenPreviousAndNextDoors;
        }
    }
}